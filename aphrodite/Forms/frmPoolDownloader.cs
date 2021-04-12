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
        #endregion

        #region Private Variables
        private Thread DownloadThread;
        private Controls.ExtendedWebClient DownloadClient;
        private DownloadStatus CurrentStatus = DownloadStatus.Waiting;

        private string CurrentURL;

        private int CleanPageExplicitCount = 0;
        private int CleanPageQuestionableCount = 0;
        private int CleanPageSafeCount = 0;
        private int CleanPageTotalCount = 0;

        private int GraylistPageExplicitCount = 0;
        private int GraylistPageQuestionableCount = 0;
        private int GraylistPageSafeCount = 0;
        private int GraylistPageTotalCount = 0;

        private int BlacklistPageExplicitCount = 0;
        private int BlacklistPageQuestionableCount = 0;
        private int BlacklistPageSafeCount = 0;
        private int BlacklistPageTotalCount = 0;

        int CleanExplicitExistingFiles = 0;
        int CleanQuestionableExistingFiles = 0;
        int CleanSafeExistingFiles = 0;
        int CleanExistingTotalFiles = 0;
        
        int GraylistExplicitExistingFiles = 0;
        int GraylistQuestionableExistingFiles = 0;
        int GraylistSafeExistingFiles = 0;
        int GraylistExistingTotalFiles = 0;

        int BlacklistExplicitExistingFiles = 0;
        int BlacklistQuestionableExistingFiles = 0;
        int BlacklistSafeExistingFiles = 0;
        int BlacklistExistingTotalFiles = 0;
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
            if (DownloadThread != null && DownloadThread.IsAlive) {
                DownloadThread.Abort();
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
            DownloadThread = new Thread(() => {
                Thread.CurrentThread.IsBackground = true;
                DownloadPool();
            });
            tmrTitle.Start();
            DownloadThread.Start();
        }
        private void AfterDownload() {
            Program.Log(LogAction.WriteToLog, "Pool download for \"" + DownloadInfo.PoolId + "\" finished.");
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
                        pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                        lbFile.Text = "Downloading has encountered an error";
                        lbPercentage.Text = "Error";
                        status.Text = "Downloading has resulted in an error";
                        this.Text = "Pool " + DownloadInfo.PoolId + " download error";
                        System.Media.SystemSounds.Hand.Play();
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
                        lbFile.Text = "All " + CleanPageTotalCount + " files downloaded";
                        lbPercentage.Text = "Done";
                        status.Text = "Finished downloading pool";
                        this.Text = "Pool " + DownloadInfo.PoolId + " finished downloading";
                        System.Media.SystemSounds.Exclamation.Play();
                        break;

                    case DownloadStatus.Errored:
                        pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                        lbFile.Text = "Downloading has encountered an error";
                        lbPercentage.Text = "Error";
                        status.Text = "Downloading has resulted in an error";
                        this.Text = "Pool " + DownloadInfo.PoolId + " download error";
                        System.Media.SystemSounds.Hand.Play();
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
                        System.Media.SystemSounds.Asterisk.Play();
                        break;

                    case DownloadStatus.PostOrPoolWasDeleted:
                        pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                        pbDownloadStatus.Value = pbDownloadStatus.Minimum;
                        lbFile.Text = "The pool was deleted";
                        lbPercentage.Text = "-";
                        status.Text = "The pool was deleted";
                        this.Text = "Pool " + DownloadInfo.PoolId + " was deleted";
                        System.Media.SystemSounds.Hand.Play();
                        break;

                    case DownloadStatus.ApiReturnedNullOrEmpty:
                        pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                        pbDownloadStatus.Value = pbDownloadStatus.Minimum;
                        lbFile.Text = "Initial api returned null/empty";
                        lbPercentage.Text = "-";
                        status.Text = "Initial api returned null/empty";
                        this.Text = "Pool " + DownloadInfo.PoolId + " initial api returned null/empty";
                        System.Media.SystemSounds.Hand.Play();
                        break;

                    default:
                        pbDownloadStatus.Value = pbDownloadStatus.Maximum;
                        lbFile.Text = "Download assumed to be completed";
                        lbPercentage.Text = "Done?";
                        status.Text = "Download status not set, assuming the download completed";
                        this.Text = "Pool " + DownloadInfo.PoolId + " download completed?";
                        System.Media.SystemSounds.Asterisk.Play();
                        break;
                }
            }
        }
        private async void DownloadPool() {

            #region define new variables
            // New variables for the API parse
            string poolJson = string.Format("https://e621.net/pools/{0}.json", DownloadInfo.PoolId);
            string poolJsonPage = string.Format("https://e621.net/posts.json?tags=pool:{0}+order:id&limit=320&page=", DownloadInfo.PoolId);

            List<string> URLs = new List<string>();
            List<string> FileNames = new List<string>();
            List<string> FilePaths = new List<string>();
            List<string> PostIDs = new List<string>();
            string poolName = string.Empty;
            string poolDescription;
            string poolInfo = string.Empty;
            bool hasGraylistedFiles = false;
            bool hasBlacklistedFiles = false;
            int currentPage = 0;

            #endregion

            #region main try statements
            try {
                // Set the saveTo to \\Pools.
                if (!DownloadInfo.DownloadPath.EndsWith("\\Pools")) {
                    DownloadInfo.DownloadPath += "\\Pools";
                }

                #region initial api parse for main pool info
                // Begin the XML download
                this.Invoke((Action)delegate() {
                    Program.Log(LogAction.WriteToLog, "Downloading pool information. (frmPoolDownloader.cs)");
                    status.Text = "Getting pool information...";
                });
                CurrentURL = poolJson;
                string postXML = apiTools.GetJsonToXml(poolJson);

                // Check the XML.
                if (apiTools.IsXmlDead(postXML)) {
                    throw new ApiReturnedNullOrEmptyException("API is null or empty");
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(postXML);
                XmlNodeList xmlName = xmlDoc.DocumentElement.SelectNodes("/root/name");
                XmlNodeList xmlPoolDescription = xmlDoc.DocumentElement.SelectNodes("/root/description");
                XmlNodeList xmlCount = xmlDoc.DocumentElement.SelectNodes("/root/post_count");
                XmlNodeList xmlPoolDeleted = xmlDoc.DocumentElement.SelectNodes("/root/is_deleted");
                XmlNodeList xmlPostIDs = xmlDoc.DocumentElement.SelectNodes("/root/post_ids/item");

                if (xmlPoolDeleted[0].InnerText.ToLower() != "false") {
                    // idk how to handle it because I haven't found a deleted pool yet
                    throw new PoolOrPostWasDeletedException("Pool was deleted");
                }
                if (DownloadInfo.SaveInfo) {
                    poolDescription = "No description";
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
                }

                // Add the pages in their pool index
                // (so that way the page is true to the pool)
                for (int i = 0; i < xmlPostIDs.Count; i++) {
                    PostIDs.Add(xmlPostIDs[i].InnerText);
                }

                this.Invoke((Action)delegate() {
                    txtPoolName.Text = xmlName[0].InnerText;
                });

                // Count the image count and do math for pages.
                int TotalPagesToParse = apiTools.CountPoolPages(Convert.ToDecimal(xmlCount[0].InnerText));

                // Set the output folder name.
                poolName = apiTools.ReplaceIllegalCharacters(xmlName[0].InnerText);
                DownloadInfo.DownloadPath += "\\" + poolName;
                this.Invoke((Action)delegate() {
                    Program.Log(LogAction.WriteToLog, "Updated saveTo to \"\\Pools\\" + poolName + "\". (frmPoolDownloader.cs)");
                });
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
                    postXML = apiTools.GetJsonToXml(poolJsonPage + ApiPage);
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

                        // first check for the null url (e621's hard blacklist for users with no accounts)
                        string fileUrl = xmlURL[CurrentPost].InnerText;
                        if (string.IsNullOrEmpty(fileUrl)) {
                            if (xmlDeleted[CurrentPost].InnerText.ToLower() == "false") {
                                fileUrl = apiTools.GetBlacklistedImageUrl(xmlMD5[CurrentPost].InnerText, xmlExt[CurrentPost].InnerText);
                            }
                        }

                        #region tags + graylist/blacklist checks
                        ReadTags += "\r\n          General: [ ";
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
                        #endregion

                        #region File name fiddling
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

                        string fileNamePage = (PostIDs.IndexOf(xmlID[CurrentPost].InnerText) + 1).ToString("0000.##");

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
                        #endregion

                        #region nfo buffer
                        if (DownloadInfo.SaveInfo) {
                            poolDescription = " No description";
                            if (xmlDescription[CurrentPost].InnerText != "") {
                                poolDescription = "\n                    \"" + xmlDescription[CurrentPost].InnerText + "\"";
                            }

                            string InfoBuffer = "PAGE: " + fileNamePage + "\r\n" +
                                            "        MD5: " + xmlMD5[CurrentPost].InnerText + "\r\n" +
                                            "        URL: https://e621.net/posts/" + xmlID[CurrentPost].InnerText + "\r\n" +
                                            "        TAGS: " + ReadTags + "\n" +
                                            "        SCORE: Up " + xmlScoreUp[CurrentPost].InnerText + ", Down " + xmlScoreDown[CurrentPost].InnerText + ", Total " + xmlScore[CurrentPost].InnerText + "\r\n" +
                                            "        RATING: " + rating + "\r\n" +
                                            "        DESCRIPITON:" + poolDescription;

                            if (isBlacklisted && DownloadInfo.DownloadBlacklistedPages) {
                                InfoBuffer = "BLACKLISTED " + InfoBuffer + "\r\n" +
                                                 "        OFFENDING TAGS: " + foundBlacklistedTags;
                            }
                            else if (isGraylisted && DownloadInfo.DownloadGraylistedPages) {
                                InfoBuffer = "GRAYLISTED " + InfoBuffer +
                                                 "        OFFENDING TAGS: " + foundBlacklistedTags;
                            }

                            poolInfo += "    " + InfoBuffer + "\r\n\r\n";
                        }
                        #endregion

                        #region Counts + lists
                        string NewPath = DownloadInfo.DownloadPath;

                        switch (xmlRating[CurrentPost].InnerText) {
                            case "e": case "explicit":
                                if (isBlacklisted) {
                                    if (!DownloadInfo.DownloadBlacklistedPages) {
                                        continue;
                                    }

                                    if (!DownloadInfo.MergeBlacklistedPages) {
                                        NewPath += "\\blacklisted";
                                    }

                                    if (File.Exists(NewPath + "\\" + fileName)) {
                                        BlacklistExplicitExistingFiles++;
                                        BlacklistExistingTotalFiles++;
                                        continue;
                                    }
                                    else {
                                        BlacklistPageExplicitCount++;
                                        BlacklistPageTotalCount++;
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
                                        GraylistPageExplicitCount++;
                                        GraylistExistingTotalFiles++;
                                        continue;
                                    }
                                    else {
                                        GraylistExplicitExistingFiles++;
                                        GraylistPageTotalCount++;
                                    }
                                }
                                else {
                                    if (File.Exists(NewPath + "\\" + fileName)) {
                                        CleanExplicitExistingFiles++;
                                        CleanExistingTotalFiles++;
                                        continue;
                                    }
                                    else {
                                        CleanPageExplicitCount++;
                                        CleanPageTotalCount++;
                                    }
                                }
                                break;

                            case "q": case "questionable":
                                if (isBlacklisted) {
                                    if (!DownloadInfo.DownloadBlacklistedPages) {
                                        continue;
                                    }

                                    if (!DownloadInfo.MergeBlacklistedPages) {
                                        NewPath += "\\blacklisted";
                                    }

                                    if (File.Exists(NewPath + "\\" + fileName)) {
                                        BlacklistQuestionableExistingFiles++;
                                        BlacklistExistingTotalFiles++;
                                        continue;
                                    }
                                    else {
                                        BlacklistPageQuestionableCount++;
                                        BlacklistPageTotalCount++;
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
                                        GraylistPageQuestionableCount++;
                                        GraylistExistingTotalFiles++;
                                        continue;
                                    }
                                    else {
                                        GraylistQuestionableExistingFiles++;
                                        GraylistPageTotalCount++;
                                    }
                                }
                                else {
                                    if (File.Exists(NewPath + "\\" + fileName)) {
                                        CleanQuestionableExistingFiles++;
                                        CleanExistingTotalFiles++;
                                        continue;
                                    }
                                    else {
                                        CleanPageQuestionableCount++;
                                        CleanPageTotalCount++;
                                    }
                                }
                                break;

                            case "s": case "safe":
                                if (isBlacklisted) {
                                    if (!DownloadInfo.DownloadBlacklistedPages) {
                                        continue;
                                    }

                                    if (!DownloadInfo.MergeBlacklistedPages) {
                                        NewPath += "\\blacklisted";
                                    }

                                    if (File.Exists(NewPath + "\\" + fileName)) {
                                        BlacklistSafeExistingFiles++;
                                        BlacklistExistingTotalFiles++;
                                        continue;
                                    }
                                    else {
                                        BlacklistPageSafeCount++;
                                        BlacklistPageTotalCount++;
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
                                        GraylistPageSafeCount++;
                                        GraylistExistingTotalFiles++;
                                        continue;
                                    }
                                    else {
                                        GraylistSafeExistingFiles++;
                                        GraylistPageTotalCount++;
                                    }
                                }
                                else {
                                    if (File.Exists(NewPath + "\\" + fileName)) {
                                        CleanSafeExistingFiles++;
                                        CleanExistingTotalFiles++;
                                        continue;
                                    }
                                    else {
                                        CleanPageSafeCount++;
                                        CleanPageTotalCount++;
                                    }
                                }
                                break;
                        }

                        URLs.Add(fileUrl);
                        FileNames.Add(fileName);
                        FilePaths.Add(NewPath + "\\" + fileName);
                        #endregion

                    }

                    #region update totals
                    this.Invoke((Action)delegate() {
                        object[] Counts = new object[] {
                            CleanPageTotalCount, CleanPageExplicitCount, CleanPageQuestionableCount, CleanPageSafeCount,
                            GraylistPageTotalCount, GraylistPageExplicitCount, GraylistPageQuestionableCount, GraylistPageSafeCount,
                            BlacklistPageTotalCount, BlacklistPageExplicitCount, BlacklistPageQuestionableCount, BlacklistPageSafeCount,
                            (CleanPageTotalCount + GraylistPageTotalCount + BlacklistPageTotalCount),

                            CleanExistingTotalFiles, CleanExplicitExistingFiles, CleanQuestionableExistingFiles, CleanSafeExistingFiles,
                            GraylistExistingTotalFiles, GraylistExplicitExistingFiles, GraylistQuestionableExistingFiles, GraylistSafeExistingFiles,
                            BlacklistExistingTotalFiles, BlacklistExplicitExistingFiles, BlacklistQuestionableExistingFiles, BlacklistSafeExistingFiles,
                            (CleanExistingTotalFiles + GraylistExistingTotalFiles + BlacklistExistingTotalFiles)
                        };

                        string InfoLabelBuffer = "{0} clean pages ( {1} e, {2} q, {3} s )\r\n" +
                                                 "not saving graylisted pages\r\n" + //"{4} graylisted pages ( {5} e, {6} q, {7} s )\r\n" +
                                                 "not saving blacklisted pages\r\n" + //"{8} blacklisted pages ( {9} e, {10} q, {11} s )\r\n" +
                                                 "{12} total pages\r\n\r\n" +

                                                 "{13} clean exist ( {14} e, {15} q, {16} s )\r\n" +
                                                 "{17} graylisted exist ( {18} e, {19} q, {20} s )\r\n" +
                                                 "{21} blacklisted exist ( {22} e, {23} q, {24} s )\r\n" +
                                                 "{25} total exist";

                        if (DownloadInfo.DownloadGraylistedPages) {
                            InfoLabelBuffer = InfoLabelBuffer.Replace("not saving graylisted pages", "{4} graylisted pages ( {5} e, {6} q, {7} s )");
                        }
                        if (DownloadInfo.DownloadBlacklistedPages) {
                            InfoLabelBuffer = InfoLabelBuffer.Replace("not saving blacklisted pages", "{8} blacklisted pages ( {9} e, {10} q, {11} s )");
                        }

                        lbTotal.Text = string.Format(InfoLabelBuffer, Counts);
                    });
                    #endregion
                }


                #endregion

                #region pre-download checks
                // Check for files.
                if (URLs.Count < 1) {
                    throw new NoFilesToDownloadException("No files are available to download");
                }

                this.Invoke((Action)delegate() {
                    Program.Log(LogAction.WriteToLog, "There are " + URLs.Count + " pages to download. (frmPoolDownloader.cs)");
                });

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
                    File.WriteAllText(DownloadInfo.DownloadPath + "\\pool.nfo", poolInfo, Encoding.UTF8);
                }

                this.Invoke((Action)delegate() {
                    pbDownloadStatus.Style = ProgressBarStyle.Blocks;
                    pbTotalStatus.Maximum = URLs.Count;
                    lbFile.Text = "File 1 of " + (CleanPageTotalCount + GraylistPageTotalCount + BlacklistPageTotalCount);
                });

                // Download pool.
                this.Invoke((Action)delegate() {
                    Program.Log(LogAction.WriteToLog, "Downloading pool pages. (frmPoolDownloader.cs)");
                });
                using (DownloadClient = new Controls.ExtendedWebClient()) {
                    DownloadClient.DownloadProgressChanged += (s, e) => {
                        this.Invoke((Action)delegate() {
                            if (pbDownloadStatus.Value < 100) {
                                pbDownloadStatus.Value = e.ProgressPercentage;
                            }
                            lbPercentage.Text = e.ProgressPercentage.ToString() + "%";
                            lbBytes.Text = (e.BytesReceived / 1024) + " kb / " + (e.TotalBytesToReceive / 1024) + " kb";
                        });
                    };

                    DownloadClient.DownloadFileCompleted += (s, e) => {
                        this.Invoke((Action)delegate() {
                            pbDownloadStatus.Value = 0;
                            lbPercentage.Text = "0%";

                            if (pbTotalStatus.Value != pbTotalStatus.Maximum)
                                pbTotalStatus.Value++;
                        });
                    };

                    DownloadClient.Proxy = WebRequest.GetSystemWebProxy();
                    DownloadClient.Headers.Add("user-agent", Program.UserAgent);
                    DownloadClient.Method = "GET";

                    for (int y = 0; y < URLs.Count; y++) {
                        CurrentURL = URLs[y].Replace("www.", "");

                        if (!File.Exists(FilePaths[y])) {
                            await DownloadClient.DownloadFileTaskAsync(new Uri(CurrentURL), FilePaths[y]);
                        }
                    }
                }
                #endregion

                this.Invoke((Action)delegate() {
                    Program.Log(LogAction.WriteToLog, "The pool " + DownloadInfo.PoolId + " was downloaded. (frmPoolDownloader.cs)");
                });
                CurrentStatus = DownloadStatus.Finished;
            }
            #endregion

            #region catch statements
            catch (PoolOrPostWasDeletedException) {
                this.Invoke((Action)delegate() {
                    Program.Log(LogAction.WriteToLog, "The pool " + DownloadInfo.PoolId + " was deleted. (frmPoolDownloader.cs)");
                });
                CurrentStatus = DownloadStatus.PostOrPoolWasDeleted;
            }
            catch (ApiReturnedNullOrEmptyException) {
                this.Invoke((Action)delegate() {
                    Program.Log(LogAction.WriteToLog, "The pool " + DownloadInfo.PoolId + " API Returned null or empty. (frmPoolDownloader.cs)");
                });
                CurrentStatus = DownloadStatus.ApiReturnedNullOrEmpty;
            }
            catch (NoFilesToDownloadException) {
                this.Invoke((Action)delegate() {
                    Program.Log(LogAction.WriteToLog, "No files are available for pool " + DownloadInfo.PoolId + ". (frmPoolDownloader.cs)");
                });
                CurrentStatus = DownloadStatus.NothingToDownload;
            }
            catch (ThreadAbortException) {
                this.Invoke((Action)delegate() {
                    Program.Log(LogAction.WriteToLog, "The download thread was aborted. (frmPoolDownloader.cs)");
                });
                if (DownloadClient != null && DownloadClient.IsBusy) {
                    DownloadClient.CancelAsync();
                }
                CurrentStatus = DownloadStatus.Aborted;
            }
            catch (ObjectDisposedException) {
                this.Invoke((Action)delegate() {
                    Program.Log(LogAction.WriteToLog, "Seems like the form got disposed. (frmPoolDownloader.cs)");
                });
                CurrentStatus = DownloadStatus.FormWasDisposed;
            }
            catch (WebException WebE) {
                this.Invoke((Action)delegate() {
                    Program.Log(LogAction.WriteToLog, "A WebException occured. (frmPoolDownloader.cs");
                    status.Text = "A WebException has occured";
                    pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                    pbTotalStatus.State = aphrodite.Controls.ProgressBarState.Error;
                });
                ErrorLog.ReportWebException(WebE, "frmPoolDownloader.cs", CurrentURL);
                CurrentStatus = DownloadStatus.Errored;
            }
            catch (Exception ex) {
                this.Invoke((Action)delegate() {
                    Program.Log(LogAction.WriteToLog, "An Exception occured. (frmPoolDownloader.cs");
                    status.Text = "An Exception has occured";
                    pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                    pbTotalStatus.State = aphrodite.Controls.ProgressBarState.Error;
                });
                if (DownloadClient != null && DownloadClient.IsBusy) {
                    DownloadClient.CancelAsync();
                }
                ErrorLog.ReportException(ex, "frmPoolDownloader.cs", false);
                CurrentStatus = DownloadStatus.Errored;
            }
            #endregion

            #region finally statement
            finally {
                if (CurrentStatus != DownloadStatus.FormWasDisposed) {
                    this.Invoke((Action)delegate() {
                        AfterDownload();
                    });
                }
            }
            #endregion

        }
        #endregion

    }
}