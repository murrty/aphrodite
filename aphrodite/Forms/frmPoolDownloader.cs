using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace aphrodite {
    public partial class frmPoolDownloader : Form {

        #region Public Variables
        public PoolDownloadInfo DownloadInfo;
        public int CleanPageCount = 0;               // Will update the page count before download.
        public int GraylistedPageCount = 0;    // Will count the blacklisted pages before download.
        public int BlacklistedPageCount = 0;
        #endregion

        #region Private Variables
        private DownloadStatus CurrentStatus = DownloadStatus.Waiting;
        private string CurrentURL;

        private Thread poolDownloader;
        #endregion

        #region Form
        public frmPoolDownloader() {
            InitializeComponent();
        }

        private void frmDownload_Load(object sender, EventArgs e) {
            lbID.Text = "Pool ID " + DownloadInfo.PoolId;

            if (string.IsNullOrWhiteSpace(DownloadInfo.FileNameSchema)) {
                DownloadInfo.FileNameSchema = "%poolname%_%page%";
            }
        }
        private void frmDownload_Shown(object sender, EventArgs e) {
            tmrTitle.Start();
            StartDownload();
        }
        private void frmDownload_FormClosing(object sender, FormClosingEventArgs e) {
            if (poolDownloader != null && poolDownloader.IsAlive) {
                poolDownloader.Abort();
                if (!DownloadInfo.IgnoreFinish) {
                    e.Cancel = true;
                    return;
                }
            }
            tmrTitle.Stop();
            this.Dispose();
        }

        private void tmrTitle_Tick(object sender, EventArgs e) {
            if (this.Text.EndsWith("....")) {
                this.Text = this.Text.TrimEnd('.');
            }
            else {
                this.Text += ".";
            }
        }
        #endregion

        #region Downloader
        private void StartDownload() {
            poolDownloader = new Thread(() => {
                Thread.CurrentThread.IsBackground = true;
                DownloadPool();
            });
            tmrTitle.Start();
            poolDownloader.Start();
        }
        private void AfterDownload() {
            tmrTitle.Stop();
            pbDownloadStatus.Style = ProgressBarStyle.Blocks;
            if (DownloadInfo.OpenAfter && CurrentStatus == DownloadStatus.Finished) {
                Process.Start(DownloadInfo.DownloadPath);
            }

            if (DownloadInfo.IgnoreFinish) {
                switch (CurrentStatus) {
                    case DownloadStatus.Finished:
                        this.DialogResult = DialogResult.Yes;
                        break;

                    case DownloadStatus.Errored:
                        this.DialogResult = DialogResult.No;
                        break;

                    case DownloadStatus.Aborted:
                        this.DialogResult = DialogResult.Abort;
                        break;

                    default:
                        this.DialogResult = DialogResult.Ignore;
                        break;
                }
            }
            else {
                switch (CurrentStatus) {
                    case DownloadStatus.Finished:
                        pbDownloadStatus.Value = pbDownloadStatus.Maximum;
                        pbTotalStatus.Value = pbTotalStatus.Maximum;
                        lbFile.Text = "All " + CleanPageCount + " files downloaded";
                        lbPercentage.Text = "Done";
                        status.Text = "Finished downloading pool";
                        this.Text = "Pool " + DownloadInfo.PoolId + " finished downloading";
                        break;

                    case DownloadStatus.Errored:
                        pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                        lbFile.Text = "Downloading has encountered an error";
                        lbPercentage.Text = "Error";
                        status.Text = "Downloading has resulted in an error";
                        this.Text = "Pool " + DownloadInfo.PoolId + " download error";
                        break;

                    case DownloadStatus.Aborted:
                        pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                        lbFile.Text = "Download cancelled";
                        lbPercentage.Text = "cancelled";
                        status.Text = "Download has cancelled";
                        this.Text = "Pool " + DownloadInfo.PoolId + " download cancelled";
                        break;

                    case DownloadStatus.NothingToDownload:
                        pbDownloadStatus.Value = pbDownloadStatus.Minimum;
                        lbFile.Text = "No files to download";
                        lbPercentage.Text = "-";
                        status.Text = "No files to download";
                        this.Text = "Pool " + DownloadInfo.PoolId + " No files to download";
                        break;

                    case DownloadStatus.PostOrPoolWasDeleted:
                        pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                        pbDownloadStatus.Value = pbDownloadStatus.Minimum;
                        lbFile.Text = "The pool was deleted";
                        lbPercentage.Text = "-";
                        status.Text = "The pool was deleted";
                        this.Text = "Pool " + DownloadInfo.PoolId + " was deleted";
                        break;

                    case DownloadStatus.ApiReturnedNullOrEmpty:
                        pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                        pbDownloadStatus.Value = pbDownloadStatus.Minimum;
                        lbFile.Text = "Initial api returned null/empty";
                        lbPercentage.Text = "-";
                        status.Text = "Initial api returned null/empty";
                        this.Text = "Pool " + DownloadInfo.PoolId + " initial api returned null/empty";
                        break;
                }
            }
        }
        private void DownloadPool() {
            try {
                // Set the saveTo to \\Pools.
                if (!DownloadInfo.DownloadPath.EndsWith("\\Pools")) {
                    DownloadInfo.DownloadPath += "\\Pools";
                }

                writeToConsole("Set output to " + DownloadInfo.DownloadPath);

                #region define new variables
                // New variables for the API parse
                string poolJson = string.Format("https://e621.net/pools/{0}.json", DownloadInfo.PoolId);
                string poolJsonPage = string.Format("https://e621.net/posts.json?tags=pool:{0}+order:id&limit=320", DownloadInfo.PoolId);

                List<string> URLs = new List<string>();
                List<string> FileNames = new List<string>();
                List<string> FilePaths = new List<string>();
                List<string> PostIDs = new List<string>();
                string poolName = string.Empty;
                string poolInfo = string.Empty;
                bool hasGraylistedFiles = false;
                bool hasBlacklistedFiles = false;
                int currentPage = 0;
                int CleanExistingFiles = 0;
                int GraylistedExistingFiles = 0;
                int BlacklistedExistingFiles = 0;

                XmlDocument xmlDoc = new XmlDocument();
                writeToConsole("Configured variables");
                #endregion

                #region initial xml download
                // Begin the XML download
                this.Invoke((MethodInvoker)(() => status.Text = "Getting pool information for page 1"));
                writeToConsole("Starting JSON download for page 1...", true);
                CurrentURL = poolJson;
                string postXML = apiTools.GetJsonToXml(poolJson);
                writeToConsole("JSON Downloaded.", true);

                // Check the XML.
                writeToConsole("Checking XML...");
                if (apiTools.IsXmlDead(postXML)) {
                    throw new ApiReturnedNullOrEmptyException("API is null or empty");
                }
                xmlDoc.LoadXml(postXML);
                #endregion

                #region initial api parse for main pool info
                // XmlNodeLists for pool information.
                XmlNodeList xmlName = xmlDoc.DocumentElement.SelectNodes("/root/name");
                XmlNodeList xmlPoolDescription = xmlDoc.DocumentElement.SelectNodes("/root/description");
                XmlNodeList xmlCount = xmlDoc.DocumentElement.SelectNodes("/root/post_count");
                XmlNodeList xmlPoolDeleted = xmlDoc.DocumentElement.SelectNodes("/root/is_deleted");
                XmlNodeList xmlPostIDs = xmlDoc.DocumentElement.SelectNodes("/root/post_ids/item");

                if (xmlPoolDeleted[0].InnerText.ToLower() == "true") {
                    // idk how to handle it because I haven't found a deleted pool yet
                    throw new PoolOrPostWasDeletedException("Pool was deleted");
                }

                string poolDescription = "No description";
                if (xmlPoolDescription[0].InnerText != "") {
                    poolDescription = "\"" + xmlPoolDescription[0].InnerText + "\"";
                }

                poolInfo += "POOL: " + DownloadInfo.PoolId + "\n" +
                            "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\n" +
                            "    NAME: " + xmlName[0].InnerText + "\n" +
                            "    PAGES: " + xmlCount[0].InnerText + "\n" +
                            "    URL: https://e621.net/pool/show/" + DownloadInfo.PoolId + "\n" +
                            "    DESCRIPTION:\n    " + poolDescription +
                            "\n\n";

                // Add the pages in their pool index
                // (so that way the page is true to the pool)
                for (int i = 0; i < xmlPostIDs.Count; i++) {
                    PostIDs.Add(xmlPostIDs[i].InnerText);
                }

                this.BeginInvoke((MethodInvoker)delegate() {
                    txtPoolName.TextAlign = HorizontalAlignment.Left;
                    txtPoolName.Text = xmlName[0].InnerText;
                });

                // Count the image count and do math for pages.
                int TotalPagesToParse = apiTools.CountPoolPages(Convert.ToDecimal(xmlCount[0].InnerText));

                // Set the output folder name.
                poolName = apiTools.ReplaceIllegalCharacters(xmlName[0].InnerText);
                DownloadInfo.DownloadPath += "\\" + poolName;
                writeToConsole("Updated saveTo to \\Pools\\" + poolName);
                #endregion

                #region Parse for the images in the pools
                XmlNodeList xmlID;
                XmlNodeList xmlMD5;
                XmlNodeList xmlURL;
                XmlNodeList xmlTagsGeneral;
                XmlNodeList xmlTagsSpecies;
                XmlNodeList xmlTagsCharacter;
                XmlNodeList xmlTagsCopyright;
                XmlNodeList xmlTagsArtist;
                XmlNodeList xmlTagsInvalid;
                XmlNodeList xmlTagsLore;
                XmlNodeList xmlTagsMeta;
                XmlNodeList xmlTagsLocked;
                XmlNodeList xmlScore;
                XmlNodeList xmlScoreUp;
                XmlNodeList xmlScoreDown;
                XmlNodeList xmlFavCount;
                XmlNodeList xmlRating;
                XmlNodeList xmlAuthor;
                XmlNodeList xmlDescription;
                XmlNodeList xmlExt;
                XmlNodeList xmlDeleted;
                // Begin ripping the rest of the pool Json.
                for (int ApiPage = 1; ApiPage < TotalPagesToParse + 1; ApiPage++) {
                    CurrentURL = poolJsonPage;
                    postXML = apiTools.GetJsonToXml(poolJsonPage + "&page=" + ApiPage);
                    if (apiTools.IsXmlDead(postXML)) {
                        break;
                    }
                    xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(postXML);

                    xmlID = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/id");
                    xmlMD5 = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/file/md5");
                    xmlURL = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/file/url");
                    xmlTagsGeneral = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags/general");
                    xmlTagsSpecies = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags/species");
                    xmlTagsCharacter = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags/character");
                    xmlTagsCopyright = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags/copyright");
                    xmlTagsArtist = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags/artist");
                    xmlTagsInvalid = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags/invalid");
                    xmlTagsLore = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags/lore");
                    xmlTagsMeta = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags/meta");
                    xmlTagsLocked = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/locked_tags");
                    xmlScore = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/score/total");
                    xmlScoreUp = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/score/up");
                    xmlScoreDown = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/score/down");
                    xmlFavCount = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/fav_count");
                    xmlRating = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/rating");
                    xmlAuthor = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/uploader_id");
                    xmlDescription = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/description");
                    xmlExt = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/file/ext");
                    xmlDeleted = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/flags/deleted");

                    for (int CurrentPost = 0; CurrentPost < xmlID.Count; CurrentPost++) {
                        bool isGraylisted = false;
                        bool isBlacklisted = false;
                        string foundBlacklistedTags = string.Empty;
                        string rating;
                        List<string> PostTags = new List<string>();
                        string ReadTags = string.Empty;
                        string tagsMetadata = string.Empty;
                        currentPage++;

                        ReadTags += "          General: [ ";
                        switch (xmlTagsGeneral[CurrentPost].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsGeneral[CurrentPost].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsGeneral[CurrentPost].ChildNodes[x].InnerText);
                                    ReadTags += xmlTagsGeneral[CurrentPost].ChildNodes[x].InnerText + ", ";
                                }
                                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                                break;

                            case false:
                                ReadTags += "none";
                                break;
                        }

                        ReadTags += " ]\r\n          Species: [ ";
                        switch (xmlTagsSpecies[CurrentPost].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsSpecies[CurrentPost].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsSpecies[CurrentPost].ChildNodes[x].InnerText);
                                    ReadTags += xmlTagsSpecies[CurrentPost].ChildNodes[x].InnerText + ", ";
                                }
                                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                                break;

                            case false:
                                ReadTags += "none";
                                break;
                        }

                        ReadTags += " ]\r\n          Character(s): [ ";
                        switch (xmlTagsCharacter[CurrentPost].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsCharacter[CurrentPost].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsCharacter[CurrentPost].ChildNodes[x].InnerText);
                                    ReadTags += xmlTagsCharacter[CurrentPost].ChildNodes[x].InnerText + ", ";
                                }
                                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                                break;

                            case false:
                                ReadTags += "none";
                                break;
                        }

                        ReadTags += " ]\r\n          Copyright: [ ";
                        switch (xmlTagsCopyright[CurrentPost].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsCopyright[CurrentPost].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsCopyright[CurrentPost].ChildNodes[x].InnerText);
                                    ReadTags += xmlTagsCopyright[CurrentPost].ChildNodes[x].InnerText + ", ";
                                }
                                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                                break;

                            case false:
                                ReadTags += "none";
                                break;
                        }

                        ReadTags += " ]\r\n          Artist: [ ";
                        switch (xmlTagsArtist[CurrentPost].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsArtist[CurrentPost].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsArtist[CurrentPost].ChildNodes[x].InnerText);
                                    ReadTags += xmlTagsArtist[CurrentPost].ChildNodes[x].InnerText + ", ";
                                }
                                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                                break;

                            case false:
                                ReadTags += "none";
                                break;
                        }

                        ReadTags += " ]\r\n          Lore: [ ";
                        switch (xmlTagsLore[CurrentPost].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsLore[CurrentPost].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsLore[CurrentPost].ChildNodes[x].InnerText);
                                    ReadTags += xmlTagsLore[CurrentPost].ChildNodes[x].InnerText + ", ";
                                }
                                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                                break;

                            case false:
                                ReadTags += "none";
                                break;
                        }

                        ReadTags += " ]\r\n          Meta: [ ";
                        switch (xmlTagsMeta[CurrentPost].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsMeta[CurrentPost].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsMeta[CurrentPost].ChildNodes[x].InnerText);
                                    ReadTags += xmlTagsMeta[CurrentPost].ChildNodes[x].InnerText + ", ";
                                }
                                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                                break;

                            case false:
                                ReadTags += "none";
                                break;
                        }

                        ReadTags += " ]\r\n          Locked tags: [ ";
                        switch (xmlTagsLocked[CurrentPost].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsLocked[CurrentPost].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsLocked[CurrentPost].ChildNodes[x].InnerText);
                                    ReadTags += xmlTagsLocked[CurrentPost].ChildNodes[x].InnerText + ", ";
                                }
                                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',') + " ]";
                                break;

                            case false:
                                ReadTags += "none";
                                break;
                        }

                        ReadTags += " ]\r\n          Invalid: [ ";
                        switch (xmlTagsInvalid[CurrentPost].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsInvalid[CurrentPost].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsInvalid[CurrentPost].ChildNodes[x].InnerText);
                                    ReadTags += xmlTagsInvalid[CurrentPost].ChildNodes[x].InnerText + ", ";
                                }
                                ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                                break;

                            case false:
                                ReadTags += "none";
                                break;
                        }
                        ReadTags += " ]";

                        switch (xmlRating[CurrentPost].InnerText) {
                            case "e":
                                rating = "Explicit";
                                break;
                            case "q":
                                rating = "Questionable";
                                break;
                            case "s":
                                rating = "Safe";
                                break;
                            default:
                                rating = "Unknown";
                                break;
                        }

                        poolDescription = " No description";
                        if (xmlDescription[CurrentPost].InnerText != "") {
                            poolDescription = "\n                    \"" + xmlDescription[CurrentPost].InnerText + "\"";
                        }

                        string fileNamePage = string.Empty;
                        fileNamePage = (PostIDs.IndexOf(xmlID[CurrentPost].InnerText) + 1).ToString("0000.##");

                        // File name artist for the schema
                        string fileNameArtist = "(none)";
                        bool useHardcodedFilter = false;
                        if (string.IsNullOrEmpty(Config.Settings.General.undesiredTags)) {
                            useHardcodedFilter = true;
                        }

                        if (xmlTagsArtist[CurrentPost].ChildNodes.Count > 0) {
                            for (int j = 0; j < xmlTagsArtist[CurrentPost].ChildNodes.Count; j++) {
                                if (useHardcodedFilter) {
                                    if (!UndesiredTags.isUndesiredHardcoded(xmlTagsArtist[CurrentPost].ChildNodes[j].InnerText)) {
                                        fileNameArtist = xmlTagsArtist[CurrentPost].ChildNodes[j].InnerText;
                                        break;
                                    }
                                }
                                else {
                                    if (!UndesiredTags.isUndesired(xmlTagsArtist[CurrentPost].ChildNodes[j].InnerText)) {
                                        fileNameArtist = xmlTagsArtist[CurrentPost].ChildNodes[j].InnerText;
                                        break;
                                    }
                                }
                            }
                        }

                        string fileName = DownloadInfo.FileNameSchema.Replace("%poolname%", poolName)
                                    .Replace("%poolid%", DownloadInfo.PoolId)
                                    .Replace("%page%", fileNamePage)
                                    .Replace("%md5%", xmlMD5[CurrentPost].InnerText)
                                    .Replace("%id%", xmlID[CurrentPost].InnerText)
                                    .Replace("%rating%", rating.ToLower())
                                    .Replace("%rating2%", xmlRating[CurrentPost].InnerText)
                                    .Replace("%artist%", fileNameArtist)
                                    .Replace("%ext%", xmlExt[CurrentPost].InnerText)
                                    .Replace("%fav_count%", xmlFavCount[CurrentPost].InnerText)
                                    .Replace("%score%", xmlScore[CurrentPost].InnerText)
                                    .Replace("%scoreup%", xmlScoreUp[CurrentPost].InnerText)
                                    .Replace("%scoredown%", xmlScoreDown[CurrentPost].InnerText)
                                    .Replace("%author%", xmlAuthor[CurrentPost].InnerText) + "." + xmlExt[CurrentPost].InnerText;

                        string fileUrl = xmlURL[CurrentPost].InnerText;
                        if (string.IsNullOrEmpty(fileUrl)) {
                            if (xmlDeleted[CurrentPost].InnerText.ToLower() == "false") {
                                fileUrl = apiTools.GetBlacklistedImageUrl(xmlMD5[CurrentPost].InnerText, xmlExt[CurrentPost].InnerText);
                            }
                        }

                        for (int j = 0; j < PostTags.Count; j++) {
                            if (DownloadInfo.Graylist.Length > 0) {
                                for (int k = 0; k < DownloadInfo.Graylist.Length; k++) {
                                    if (PostTags[j] == DownloadInfo.Graylist[k]) {
                                        foundBlacklistedTags += " " + DownloadInfo.Graylist[k];
                                        isGraylisted = true;
                                        hasGraylistedFiles = true;
                                    }
                                }
                            }

                            if (DownloadInfo.Blacklist.Length > 0) {
                                for (int k = 0; k < DownloadInfo.Blacklist.Length; k++) {
                                    if (PostTags[j] == DownloadInfo.Blacklist[k]) {
                                        foundBlacklistedTags += " " + DownloadInfo.Blacklist[k];
                                        isBlacklisted = true;
                                        hasBlacklistedFiles = true;
                                    }
                                }
                            }
                        }


                        URLs.Add(fileUrl);
                        FileNames.Add(fileName);

                        string NewPath = DownloadInfo.DownloadPath;

                        if (isBlacklisted) {
                            if (!DownloadInfo.DownloadBlacklistedPages) {
                                continue;
                            }

                            if (!DownloadInfo.MergeBlacklistedPages) {
                                NewPath += "\\blacklisted";
                            }

                            if (File.Exists(NewPath + "\\" + fileName)) {
                                BlacklistedExistingFiles++;
                                continue;
                            }
                            else {
                                BlacklistedPageCount++;
                            }
                        }
                        else if (isGraylisted) {
                            if (!DownloadInfo.DownloadGraylistedPages) {
                                continue;
                            }

                            if (!DownloadInfo.MergeGraylistedPages) {
                                NewPath += "\\graylisted";
                            }

                            if (File.Exists(NewPath + "\\" + fileName)) {
                                GraylistedExistingFiles++;
                                continue;
                            }
                            else {
                                GraylistedPageCount++;
                            }
                        }
                        else {
                            if (File.Exists(NewPath + "\\" + fileName)) {
                                CleanExistingFiles++;
                                continue;
                            }
                            else {
                                CleanPageCount++;
                            }
                        }

                        FilePaths.Add(NewPath + "\\" + fileName);

                        UpdateTotals();

                        string InfoBuffer = "PAGE: " + fileNamePage + "\n" +
                                        "        MD5: " + xmlMD5[CurrentPost].InnerText + "\n" +
                                        "        URL: https://e621.net/posts/" + xmlID[CurrentPost].InnerText + "\n" +
                                        "        TAGS: " + ReadTags + "\n" +
                                        "        SCORE: Up " + xmlScoreUp[CurrentPost].InnerText + ", Down " + xmlScoreDown[CurrentPost].InnerText + ", Total " + xmlScore[CurrentPost].InnerText + "\n" +
                                        "        RATING: " + rating + "\n" +
                                        "        DESCRIPITON:" + poolDescription;

                        if (isBlacklisted && DownloadInfo.DownloadBlacklistedPages) {
                            InfoBuffer = "BLACKLISTED " + InfoBuffer +
                                             "        OFFENDING TAGS: " + foundBlacklistedTags;
                        }
                        else if (isGraylisted && DownloadInfo.DownloadGraylistedPages) {
                            InfoBuffer = "GRAYLISTED " + InfoBuffer +
                                             "        OFFENDING TAGS: " + foundBlacklistedTags;
                        }

                        poolInfo += "    " + InfoBuffer + "\r\n\r\n";
                    }
                }


                #endregion

                #region pre-download checks
                // Check for files.
                if (URLs.Count < 1) {
                    throw new NoFilesToDownloadException("No files are available to download");
                }

                writeToConsole("There are " + URLs.Count + " files in total.", true);

            // Create directories.
                if (!Directory.Exists(DownloadInfo.DownloadPath)) {
                    Directory.CreateDirectory(DownloadInfo.DownloadPath);
                }
                if (hasGraylistedFiles && DownloadInfo.DownloadGraylistedPages && !DownloadInfo.MergeGraylistedPages && !Directory.Exists(DownloadInfo.DownloadPath + "\\graylisted")) {
                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\graylisted");
                }
                if (hasBlacklistedFiles && DownloadInfo.DownloadBlacklistedPages && !DownloadInfo.MergeBlacklistedPages && !Directory.Exists(DownloadInfo.DownloadPath + "\\blacklisted")) {
                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted");
                }
                #endregion

                #region Downloading
            // Save .nfo files.
                if (DownloadInfo.SaveInfo) {
                    poolInfo = poolInfo.TrimEnd('\n');
                    poolInfo = poolInfo.TrimEnd('\r').TrimEnd('\n').TrimEnd('\r').TrimEnd('\n');
                    writeToConsole("Saving pool.nfo");
                    File.WriteAllText(DownloadInfo.DownloadPath + "\\pool.nfo", poolInfo, Encoding.UTF8);
                }

                this.BeginInvoke(new MethodInvoker(() => {
                    pbDownloadStatus.Style = ProgressBarStyle.Blocks;
                    pbTotalStatus.Maximum = URLs.Count;
                    lbFile.Text = "File 1 of " + (CleanPageCount + GraylistedPageCount + BlacklistedPageCount);
                }));

            // Download pool.
                writeToConsole("Starting pool download...", true);
                using (Controls.ExtendedWebClient wc = new Controls.ExtendedWebClient()) {
                    wc.DownloadProgressChanged += (s, e) => {
                        this.BeginInvoke(new MethodInvoker(() => {
                            if (pbDownloadStatus.Value < 100) {
                                pbDownloadStatus.Value = e.ProgressPercentage;
                            }
                            lbPercentage.Text = e.ProgressPercentage.ToString() + "%";
                            lbBytes.Text = (e.BytesReceived / 1024) + " kb / " + (e.TotalBytesToReceive / 1024) + " kb";
                        }));
                    };

                    wc.DownloadFileCompleted += (s, e) => {
                        lock (e.UserState) {
                            this.BeginInvoke(new MethodInvoker(() => {
                                pbDownloadStatus.Value = 0;
                                lbPercentage.Text = "0%";

                                if (pbTotalStatus.Value != pbTotalStatus.Maximum)
                                    pbTotalStatus.Value++;
                            }));
                            Monitor.Pulse(e.UserState);
                        }
                    };

                    wc.Proxy = WebRequest.GetSystemWebProxy();
                    wc.Headers.Add("user-agent", Program.UserAgent);
                    wc.Method = "GET";

                    for (int y = 0; y < URLs.Count; y++) {
                        CurrentURL = URLs[y].Replace("www.", "");

                        if (File.Exists(FilePaths[y])) {
                            continue;
                        }

                        var sync = new Object();
                        lock (sync) {
                            wc.DownloadFileAsync(new Uri(CurrentURL), FilePaths[y], sync);
                            Monitor.Wait(sync);
                        }
                    }
                }
                #endregion

                CurrentStatus = DownloadStatus.Finished;
            }
            catch (PoolOrPostWasDeletedException) {
                CurrentStatus = DownloadStatus.PostOrPoolWasDeleted;
            }
            catch (ApiReturnedNullOrEmptyException) {
                CurrentStatus = DownloadStatus.ApiReturnedNullOrEmpty;
            }
            catch (NoFilesToDownloadException) {
                CurrentStatus = DownloadStatus.NothingToDownload;
            }
            catch (ThreadAbortException) {
                //Program.Log(LogAction.WriteToLog, "Thread was requested to be, and has been, aborted. (frmPoolDownloader.cs)");
                CurrentStatus = DownloadStatus.Aborted;
            }
            catch (ObjectDisposedException) {
                //Program.Log(LogAction.WriteToLog, "Seems like the object got disposed. (frmPoolDownloader.cs)");
                CurrentStatus = DownloadStatus.Errored;
            }
            catch (WebException WebE) {
                //Program.Log(LogAction.WriteToLog, "A WebException has occured. (frmPoolDownloader.cs)");
                this.BeginInvoke(new MethodInvoker(() => {
                    status.Text = "A WebException has occured";
                    pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                    pbTotalStatus.State = aphrodite.Controls.ProgressBarState.Error;
                }));
                ErrorLog.ReportWebException(WebE, "frmPoolDownloader.cs", CurrentURL);
                CurrentStatus = DownloadStatus.Errored;
            }
            catch (Exception ex) {
                //Program.Log(LogAction.WriteToLog, "An exception has occured. (frmPoolDownloader.cs)");
                this.BeginInvoke(new MethodInvoker(() => {
                    status.Text = "A Exception has occured";
                    pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                    pbTotalStatus.State = aphrodite.Controls.ProgressBarState.Error;
                }));
                ErrorLog.ReportException(ex, "frmPoolDownloader.cs", false);
                CurrentStatus = DownloadStatus.Errored;
            }
            finally {
                this.BeginInvoke(new MethodInvoker(() => {
                    AfterDownload();
                }));
            }
        }

        public void UpdateTotals() {
            this.BeginInvoke(new MethodInvoker(() => {
                lbTotal.Text = (CleanPageCount) + " clean pages\r\n" +
                               (GraylistedPageCount) + " graylisted pages\r\n" +
                               (BlacklistedPageCount) + " blacklisted pages\r\n" +
                               (CleanPageCount + GraylistedPageCount + BlacklistedPageCount) + " total pages";
            }));
        }
        public void writeToConsole(string message, bool important = false) {
            //Program.Log(LogAction.WriteToLog, message);
        }
        #endregion

    }
}