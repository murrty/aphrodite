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
        public int pageCount = 0;               // Will update the page count before download.
        public int blacklistedPageCount = 0;    // Will count the blacklisted pages before download.
        #endregion

        #region Private Variables
        private bool DownloadHasFinished = false;
        private bool DownloadHasErrored = false;
        private bool DownloadHasAborted = false;
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
            startDownload();
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
        private void startDownload() {
            poolDownloader = new Thread(() => {
                Thread.CurrentThread.IsBackground = true;
                downloadPool();
            });
            tmrTitle.Start();
            poolDownloader.Start();
        }
        private void AfterDownload() {
            if (DownloadInfo.OpenAfter && DownloadHasFinished) {
                Process.Start(DownloadInfo.DownloadPath);
            }

            if (DownloadInfo.IgnoreFinish) {
                if (DownloadHasFinished) {
                    this.DialogResult = DialogResult.Yes;
                }
                else if (DownloadHasErrored) {
                    this.DialogResult = DialogResult.No;
                }
                else if (DownloadHasAborted) {
                    this.DialogResult = DialogResult.Abort;
                }
                else {
                    this.DialogResult = DialogResult.Ignore;
                }
            }
            if (DownloadHasFinished) {
                lbFile.Text = "All " + pageCount + " files downloaded";
                pbDownloadStatus.Value = pbDownloadStatus.Maximum;
                lbPercentage.Text = "Done";
                tmrTitle.Stop();
                this.Text = "Pool " + DownloadInfo.PoolId + " finished downloading";
                status.Text = "Finished downloading pool";
            }
            else if (DownloadHasErrored) {
                lbFile.Text = "Downloading has encountered an error";
                pbDownloadStatus.State = ProgressBarState.Error;
                lbPercentage.Text = "Error";
                tmrTitle.Stop();
                this.Text = "Download error";
                status.Text = "Downloading has resulted in an error";
            }
            else if (DownloadHasAborted) {
                lbFile.Text = "Download canceled";
                pbDownloadStatus.State = ProgressBarState.Error;
                lbPercentage.Text = "Canceled";
                tmrTitle.Stop();
                this.Text = "Download canceled";
                status.Text = "Download has canceled";
            }
            else {
                // assume it completed
                lbFile.Text = "Download assumed to be completed...";
                pbDownloadStatus.Value = pbDownloadStatus.Maximum;
                lbPercentage.Text = "Done?";
                tmrTitle.Stop();
                this.Text = "Pool " + DownloadInfo.PoolId + " finished downloading";
                status.Text = "Download status booleans not set, assuming the download completed";
            }
        }
        private void downloadPool() {
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
                List<string> MetadataArtists = new List<string>();
                List<string> MetadataTags = new List<string>();
                List<bool> urlBlacklisted = new List<bool>();
                List<string> PostIDs = new List<string>();
                string poolName = string.Empty;
                string poolInfo = string.Empty;
                string blacklistInfo = string.Empty;
                bool hasBlacklistedFiles = false;
                int currentPage = 0;

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
                if (postXML == apiTools.EmptyXML || string.IsNullOrWhiteSpace(postXML)) {
                    DownloadHasErrored = true;
                    this.BeginInvoke(new MethodInvoker(() => {
                        pbDownloadStatus.Style = ProgressBarStyle.Continuous;
                        AfterDownload();
                    }));
                    AfterDownload();
                    return;
                }
                writeToConsole("XML is valid.");
                xmlDoc.LoadXml(postXML);
                #endregion

                #region initial api parse for main data
              // XmlNodeLists for pool information.
                XmlNodeList xmlName = xmlDoc.DocumentElement.SelectNodes("/root/name");
                XmlNodeList xmlDescription = xmlDoc.DocumentElement.SelectNodes("/root/description");
                XmlNodeList xmlCount = xmlDoc.DocumentElement.SelectNodes("/root/post_count");
                XmlNodeList xmlPoolDeleted = xmlDoc.DocumentElement.SelectNodes("/root/is_deleted");
                XmlNodeList xmlPostIDs = xmlDoc.DocumentElement.SelectNodes("/root/post_ids/item");

                if (xmlPoolDeleted[0].InnerText.ToLower() == "true") {
                    // idk how to handle it because I haven't found a deleted pool yet
                }

                string poolDescription = "No description";
                if (xmlDescription[0].InnerText != "") {
                    poolDescription = "\"" + xmlDescription[0].InnerText + "\"";
                }

                poolInfo += "POOL: " + DownloadInfo.PoolId + "\n" +
                            "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\n" +
                            "    NAME: " + xmlName[0].InnerText + "\n" +
                            "    PAGES: " + xmlCount[0].InnerText + "\n" +
                            "    URL: https://e621.net/pool/show/" + DownloadInfo.PoolId + "\n" +
                            "    DESCRIPTION:\n    " + poolDescription +
                            "\n\n";

                blacklistInfo += "POOL: " + DownloadInfo.PoolId + "\n" +
                                 "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\n" +
                                 "    NAME: " + xmlName[0].InnerText + "\n" +
                                 "    PAGES: " + xmlCount[0].InnerText + "\n" +
                                 "    URL: https://e621.net/pool/show/" + DownloadInfo.PoolId + "\n" +
                                 "    DESCRIPTION:\n    " + poolDescription +
                                 "    BLACKLISTED TAGS: " + string.Join(" ", DownloadInfo.Graylist) +
                                 "\n\n";

                for (int i = 0; i < xmlPostIDs.Count; i++) {
                    PostIDs.Add(xmlPostIDs[i].InnerText);
                }

                this.Invoke((MethodInvoker)(() => lbName.Text = xmlName[0].InnerText));

              // Count the image count and do math for pages.
                int pages = apiTools.CountPoolPages(Convert.ToDecimal(xmlCount[0].InnerText));

              // Set the output folder name.
                poolName = apiTools.ReplaceIllegalCharacters(xmlName[0].InnerText);
                DownloadInfo.DownloadPath += "\\" + poolName;
                writeToConsole("Updated saveTo to \\Pools\\" + poolName);
                #endregion

                #region First page rip
              // Begin ripping the rest of the pool Json.
                xmlDoc = new XmlDocument();
                CurrentURL = poolJsonPage;
                postXML = apiTools.GetJsonToXml(poolJsonPage + "&page=1");
                xmlDoc.LoadXml(postXML);

                XmlNodeList xmlID = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/id");
                XmlNodeList xmlMD5 = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/file/md5");
                XmlNodeList xmlURL = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/file/url");
                XmlNodeList xmlTagsGeneral = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags/general");
                XmlNodeList xmlTagsSpecies = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags/species");
                XmlNodeList xmlTagsCharacter = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags/character");
                XmlNodeList xmlTagsCopyright = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags/copyright");
                XmlNodeList xmlTagsArtist = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags/artist");
                XmlNodeList xmlTagsInvalid = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags/invalid");
                XmlNodeList xmlTagsLore = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags/lore");
                XmlNodeList xmlTagsMeta = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags/meta");
                XmlNodeList xmlTagsLocked = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/locked_tags");
                XmlNodeList xmlScore = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/score/total");
                XmlNodeList xmlScoreUp = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/score/up");
                XmlNodeList xmlScoreDown = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/score/down");
                XmlNodeList xmlFavCount = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/fav_count");
                XmlNodeList xmlRating = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/rating");
                XmlNodeList xmlAuthor = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/uploader_id");
                xmlDescription = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/description");
                XmlNodeList xmlExt = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/file/ext");
                XmlNodeList xmlDeleted = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/flags/deleted");
                for (int i = 0; i < xmlID.Count; i++) {
                    string artists = string.Empty;
                    string artistsMetadata = string.Empty;
                    bool isBlacklisted = false;
                    string foundBlacklistedTags = string.Empty;
                    string rating;
                    List<string> PostTags = new List<string>();
                    string ReadTags = string.Empty;
                    string tagsMetadata = string.Empty;
                    currentPage++;

                    ReadTags += "          General: [ ";
                    switch (xmlTagsGeneral[i].ChildNodes.Count > 0) {
                        case true:
                            for (int x = 0; x < xmlTagsGeneral[i].ChildNodes.Count; x++) {
                                PostTags.Add(xmlTagsGeneral[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsGeneral[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            break;

                        case false:
                            ReadTags += "none";
                            break;
                    }

                    ReadTags += " ]\r\n          Species: [ ";
                    switch (xmlTagsSpecies[i].ChildNodes.Count > 0) {
                        case true:
                            for (int x = 0; x < xmlTagsSpecies[i].ChildNodes.Count; x++) {
                                PostTags.Add(xmlTagsSpecies[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsSpecies[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            break;

                        case false:
                            ReadTags += "none";
                            break;
                    }

                    ReadTags += " ]\r\n          Character(s): [ ";
                    switch (xmlTagsCharacter[i].ChildNodes.Count > 0) {
                        case true:
                            for (int x = 0; x < xmlTagsCharacter[i].ChildNodes.Count; x++) {
                                PostTags.Add(xmlTagsCharacter[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsCharacter[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            break;

                        case false:
                            ReadTags += "none";
                            break;
                    }

                    ReadTags += " ]\r\n          Copyright: [ ";
                    switch (xmlTagsCopyright[i].ChildNodes.Count > 0) {
                        case true:
                            for (int x = 0; x < xmlTagsCopyright[i].ChildNodes.Count; x++) {
                                PostTags.Add(xmlTagsCopyright[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsCopyright[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            break;

                        case false:
                            ReadTags += "none";
                            break;
                    }

                    ReadTags += " ]\r\n          Artist: [ ";
                    switch (xmlTagsArtist[i].ChildNodes.Count > 0) {
                        case true:
                            for (int x = 0; x < xmlTagsArtist[i].ChildNodes.Count; x++) {
                                PostTags.Add(xmlTagsArtist[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsArtist[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            break;

                        case false:
                            ReadTags += "none";
                            break;
                    }

                    ReadTags += " ]\r\n          Lore: [ ";
                    switch (xmlTagsLore[i].ChildNodes.Count > 0) {
                        case true:
                            for (int x = 0; x < xmlTagsLore[i].ChildNodes.Count; x++) {
                                PostTags.Add(xmlTagsLore[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsLore[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            break;

                        case false:
                            ReadTags += "none";
                            break;
                    }

                    ReadTags += " ]\r\n          Meta: [ ";
                    switch (xmlTagsMeta[i].ChildNodes.Count > 0) {
                        case true:
                            for (int x = 0; x < xmlTagsMeta[i].ChildNodes.Count; x++) {
                                PostTags.Add(xmlTagsMeta[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsMeta[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            break;

                        case false:
                            ReadTags += "none";
                            break;
                    }

                    ReadTags += " ]\r\n          Locked tags: [ ";
                    switch (xmlTagsLocked[i].ChildNodes.Count > 0) {
                        case true:
                            for (int x = 0; x < xmlTagsLocked[i].ChildNodes.Count; x++) {
                                PostTags.Add(xmlTagsLocked[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsLocked[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',') + " ]";
                            break;

                        case false:
                            ReadTags += "none";
                            break;
                    }

                    ReadTags += " ]\r\n          Invalid: [ ";
                    switch (xmlTagsInvalid[i].ChildNodes.Count > 0) {
                        case true:
                            for (int x = 0; x < xmlTagsInvalid[i].ChildNodes.Count; x++) {
                                PostTags.Add(xmlTagsInvalid[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsInvalid[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            break;

                        case false:
                            ReadTags += "none";
                            break;
                    }
                    ReadTags += " ]";

                    switch (xmlRating[i].InnerText) {
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
                    if (xmlDescription[i].InnerText != "") {
                        poolDescription = "\n                    \"" + xmlDescription[i].InnerText + "\"";
                    }

                    if (xmlTagsArtist[i].ChildNodes.Count > 0) {
                        for (int j = 0; j < xmlTagsArtist[i].ChildNodes.Count; j++) {
                            artists += xmlTagsArtist[i].ChildNodes[j].InnerText + "\n                   ";
                            artistsMetadata += xmlTagsArtist[i].ChildNodes[j].InnerText + " ";
                        }
                        artists = artists.TrimEnd(' ');
                        artists = artists.TrimEnd('\n');
                        artistsMetadata = artistsMetadata.TrimEnd(' ');
                    }
                    else {
                        artists = "(none)";
                        artistsMetadata = "(no artist)";
                    }

                    string fileNamePage = string.Empty;
                    fileNamePage = (PostIDs.IndexOf(xmlID[i].InnerText) + 1).ToString("0000.##");

                  // File name artist for the schema
                    string fileNameArtist = "(none)";
                    bool useHardcodedFilter = false;
                    if (string.IsNullOrEmpty(Config.Settings.General.undesiredTags)) {
                        useHardcodedFilter = true;
                    }

                    if (xmlTagsArtist[i].ChildNodes.Count > 0) {
                        for (int j = 0; j < xmlTagsArtist[i].ChildNodes.Count; j++) {
                            if (useHardcodedFilter) {
                                if (!UndesiredTags.isUndesiredHardcoded(xmlTagsArtist[i].ChildNodes[j].InnerText)) {
                                    fileNameArtist = xmlTagsArtist[i].ChildNodes[j].InnerText;
                                    break;
                                }
                            }
                            else {
                                if (!UndesiredTags.isUndesired(xmlTagsArtist[i].ChildNodes[j].InnerText)) {
                                    fileNameArtist = xmlTagsArtist[i].ChildNodes[j].InnerText;
                                    break;
                                }
                            }
                        }
                    }

                    string fileName = DownloadInfo.FileNameSchema.Replace("%poolname%", poolName)
                                .Replace("%poolid%", DownloadInfo.PoolId)
                                .Replace("%page%", fileNamePage)
                                .Replace("%md5%", xmlMD5[i].InnerText)
                                .Replace("%id%", xmlID[i].InnerText)
                                .Replace("%rating%", rating.ToLower())
                                .Replace("%rating2%", xmlRating[i].InnerText)
                                .Replace("%artist%", fileNameArtist)
                                .Replace("%ext%", xmlExt[i].InnerText)
                                .Replace("%fav_count%", xmlFavCount[i].InnerText)
                                .Replace("%score%", xmlScore[i].InnerText)
                                .Replace("%scoreup%", xmlScoreUp[i].InnerText)
                                .Replace("%scoredown%", xmlScoreDown[i].InnerText)
                                .Replace("%author%", xmlAuthor[i].InnerText) + "." + xmlExt[i].InnerText;

                    string fileUrl = xmlURL[i].InnerText;
                    if (string.IsNullOrEmpty(fileUrl)) {
                        if (xmlDeleted[i].InnerText.ToLower() == "false") {
                            fileUrl = apiTools.GetBlacklistedImageUrl(xmlMD5[i].InnerText, xmlExt[i].InnerText);
                        }
                    }

                    for (int j = 0; j < PostTags.Count; j++) {
                        if (DownloadInfo.Graylist.Length > 0) {
                            for (int k = 0; k < DownloadInfo.Graylist.Length; k++) {
                                if (PostTags[j] == DownloadInfo.Graylist[k]) {
                                    foundBlacklistedTags += " " + DownloadInfo.Graylist[k];
                                    isBlacklisted = true;
                                    hasBlacklistedFiles = true;
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
                    if (isBlacklisted) {
                        urlBlacklisted.Add(true);
                        blacklistedPageCount++;
                    }
                    else {
                        urlBlacklisted.Add(false);
                        pageCount++;
                    }

                    updateTotals();

                    if (isBlacklisted && DownloadInfo.SaveBlacklistedFiles && !DownloadInfo.MergeBlacklisted) {
                        blacklistInfo += "    BLACKLISTED PAGE " + fileNamePage + ":\n" +
                                         "        MD5: " + xmlMD5[i].InnerText + "\n" +
                                         "        URL: https://e621.net/posts/" + xmlID[i].InnerText + "\n" +
                                         "        ARTIST(S): " + artists + "\n" +
                                         "        TAGS:" + ReadTags + "\n" +
                                         "        SCORE: Up " + xmlScoreUp[i].InnerText + ", Down " + xmlScoreDown[i].InnerText + ", Total " + xmlScore[i].InnerText + "\n" +
                                         "        RATING: " + rating + "\n" +
                                         "        DESCRIPITON:" + poolDescription + "\n" +
                                         "        OFFENDING TAGS: " + foundBlacklistedTags +
                                         "\n\n";

                    }
                    else {
                        poolInfo += "    PAGE: " + fileNamePage + "\n" +
                                    "        MD5: " + xmlMD5[i].InnerText + "\n" +
                                    "        URL: https://e621.net/posts/" + xmlID[i].InnerText + "\n" +
                                    "        ARTIST(S): " + artists + "\n" +
                                    "        TAGS: " + ReadTags + "\n" +
                                    "        SCORE: Up " + xmlScoreUp[i].InnerText + ", Down " + xmlScoreDown[i].InnerText + ", Total " + xmlScore[i].InnerText + "\n" +
                                    "        RATING: " + rating + "\n" +
                                    "        DESCRIPITON:" + poolDescription +
                                    "\n\n";
                    }
                }
                #endregion

                #region consecutive pages
              // Redo above but for other pages.
                if (pages > 1) {
                    for (int i = 2; i < pages + 1; i++) {
                        writeToConsole("Starting JSON download for page " + i + "...", true);
                        this.Invoke((MethodInvoker)(() => status.Text = "Getting pool information for page " + i));
                        xmlDoc = new XmlDocument();

                        CurrentURL = poolJson + DownloadInfo.PoolId;
                        postXML = apiTools.GetJsonToXml(poolJson + DownloadInfo.PoolId + "&page=" + i);
                        writeToConsole("JSON Downloaded.", true);

                        writeToConsole("Checking XML...");
                        if (postXML == apiTools.EmptyXML || string.IsNullOrWhiteSpace(postXML)) {
                            writeToConsole("XML is empty, assuming dead page.");
                            break;
                        }
                        writeToConsole("XML is valid.");
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
                        for (int j = 0; j < xmlID.Count; j++) {
                            string artists = string.Empty;
                            string artistsMetadata = string.Empty;
                            bool isBlacklisted = false;
                            string foundBlacklistedTags = string.Empty;
                            string rating;
                            List<string> PostTags = new List<string>();
                            string ReadTags = string.Empty;
                            string tagsMetadata = string.Empty;
                            currentPage++;

                            ReadTags += "          General: [ ";
                            switch (xmlTagsGeneral[j].ChildNodes.Count > 0) {
                                case true:
                                    for (int x = 0; x < xmlTagsGeneral[j].ChildNodes.Count; x++) {
                                        PostTags.Add(xmlTagsGeneral[j].ChildNodes[x].InnerText);
                                        ReadTags += xmlTagsGeneral[j].ChildNodes[x].InnerText + ", ";
                                    }
                                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                                    break;

                                case false:
                                    ReadTags += "none";
                                    break;
                            }

                            ReadTags += " ]\r\n          Species: [ ";
                            switch (xmlTagsSpecies[j].ChildNodes.Count > 0) {
                                case true:
                                    for (int x = 0; x < xmlTagsSpecies[j].ChildNodes.Count; x++) {
                                        PostTags.Add(xmlTagsSpecies[j].ChildNodes[x].InnerText);
                                        ReadTags += xmlTagsSpecies[j].ChildNodes[x].InnerText + ", ";
                                    }
                                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                                    break;

                                case false:
                                    ReadTags += "none";
                                    break;
                            }

                            ReadTags += " ]\r\n          Character(s): [ ";
                            switch (xmlTagsCharacter[j].ChildNodes.Count > 0) {
                                case true:
                                    for (int x = 0; x < xmlTagsCharacter[j].ChildNodes.Count; x++) {
                                        PostTags.Add(xmlTagsCharacter[j].ChildNodes[x].InnerText);
                                        ReadTags += xmlTagsCharacter[j].ChildNodes[x].InnerText + ", ";
                                    }
                                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                                    break;

                                case false:
                                    ReadTags += "none";
                                    break;
                            }

                            ReadTags += " ]\r\n          Copyright: [ ";
                            switch (xmlTagsCopyright[j].ChildNodes.Count > 0) {
                                case true:
                                    for (int x = 0; x < xmlTagsCopyright[j].ChildNodes.Count; x++) {
                                        PostTags.Add(xmlTagsCopyright[j].ChildNodes[x].InnerText);
                                        ReadTags += xmlTagsCopyright[j].ChildNodes[x].InnerText + ", ";
                                    }
                                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                                    break;

                                case false:
                                    ReadTags += "none";
                                    break;
                            }

                            ReadTags += " ]\r\n          Artist: [ ";
                            switch (xmlTagsArtist[j].ChildNodes.Count > 0) {
                                case true:
                                    for (int x = 0; x < xmlTagsArtist[j].ChildNodes.Count; x++) {
                                        PostTags.Add(xmlTagsArtist[j].ChildNodes[x].InnerText);
                                        ReadTags += xmlTagsArtist[j].ChildNodes[x].InnerText + ", ";
                                    }
                                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                                    break;

                                case false:
                                    ReadTags += "none";
                                    break;
                            }

                            ReadTags += " ]\r\n          Lore: [ ";
                            switch (xmlTagsLore[j].ChildNodes.Count > 0) {
                                case true:
                                    for (int x = 0; x < xmlTagsLore[j].ChildNodes.Count; x++) {
                                        PostTags.Add(xmlTagsLore[j].ChildNodes[x].InnerText);
                                        ReadTags += xmlTagsLore[j].ChildNodes[x].InnerText + ", ";
                                    }
                                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                                    break;

                                case false:
                                    ReadTags += "none";
                                    break;
                            }

                            ReadTags += " ]\r\n          Meta: [ ";
                            switch (xmlTagsMeta[j].ChildNodes.Count > 0) {
                                case true:
                                    for (int x = 0; x < xmlTagsMeta[j].ChildNodes.Count; x++) {
                                        PostTags.Add(xmlTagsMeta[j].ChildNodes[x].InnerText);
                                        ReadTags += xmlTagsMeta[j].ChildNodes[x].InnerText + ", ";
                                    }
                                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                                    break;

                                case false:
                                    ReadTags += "none";
                                    break;
                            }

                            ReadTags += " ]\r\n          Locked tags: [ ";
                            switch (xmlTagsLocked[j].ChildNodes.Count > 0) {
                                case true:
                                    for (int x = 0; x < xmlTagsLocked[j].ChildNodes.Count; x++) {
                                        PostTags.Add(xmlTagsLocked[j].ChildNodes[x].InnerText);
                                        ReadTags += xmlTagsLocked[j].ChildNodes[x].InnerText + ", ";
                                    }
                                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',') + " ]";
                                    break;

                                case false:
                                    ReadTags += "none";
                                    break;
                            }

                            ReadTags += " ]\r\n          Invalid: [ ";
                            switch (xmlTagsInvalid[j].ChildNodes.Count > 0) {
                                case true:
                                    for (int x = 0; x < xmlTagsInvalid[j].ChildNodes.Count; x++) {
                                        PostTags.Add(xmlTagsInvalid[j].ChildNodes[x].InnerText);
                                        ReadTags += xmlTagsInvalid[j].ChildNodes[x].InnerText + ", ";
                                    }
                                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                                    break;

                                case false:
                                    ReadTags += "none";
                                    break;
                            }
                            ReadTags += " ]";

                            switch (xmlRating[j].InnerText) {
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
                            if (xmlDescription[j].InnerText != "") {
                                poolDescription = "\n                    \"" + xmlDescription[j].InnerText + "\"";
                            }

                            if (xmlTagsArtist[j].ChildNodes.Count > 0) {
                                for (int x = 0; x < xmlTagsArtist[j].ChildNodes.Count; x++) {
                                    artists += xmlTagsArtist[j].ChildNodes[x].InnerText + "\n                   ";
                                    artistsMetadata += xmlTagsArtist[j].ChildNodes[x].InnerText + " ";
                                }
                                artists = artists.TrimEnd(' ');
                                artists = artists.TrimEnd('\n');
                                artistsMetadata = artistsMetadata.TrimEnd(' ');
                            }
                            else {
                                artists = "(none)";
                                artistsMetadata = "(no artist)";
                            }

                            string fileNamePage = string.Empty;
                            fileNamePage = (PostIDs.IndexOf(xmlID[j].InnerText) + 1).ToString("0000.##");

                            string fileNameArtist = "(none)";
                            bool useHardcodedFilter = false;
                            if (string.IsNullOrEmpty(Config.Settings.General.undesiredTags)) {
                                useHardcodedFilter = true;
                            }

                            if (xmlTagsArtist[j].ChildNodes.Count > 0) {
                                for (int k = 0; k < xmlTagsArtist[j].ChildNodes.Count; k++) {
                                    if (useHardcodedFilter) {
                                        if (!UndesiredTags.isUndesiredHardcoded(xmlTagsArtist[j].ChildNodes[k].InnerText)) {
                                            fileNameArtist = xmlTagsArtist[j].ChildNodes[k].InnerText;
                                            break;
                                        }
                                    }
                                    else {
                                        if (!UndesiredTags.isUndesired(xmlTagsArtist[j].ChildNodes[k].InnerText)) {
                                            fileNameArtist = xmlTagsArtist[j].ChildNodes[k].InnerText;
                                            break;
                                        }
                                    }
                                }
                            }

                            string fileName = DownloadInfo.FileNameSchema.Replace("%poolname%", poolName)
                                        .Replace("%poolid%", DownloadInfo.PoolId)
                                        .Replace("%page%", fileNamePage)
                                        .Replace("%md5%", xmlMD5[j].InnerText)
                                        .Replace("%id%", xmlID[j].InnerText)
                                        .Replace("%rating%", rating.ToLower())
                                        .Replace("%rating2%", xmlRating[j].InnerText)
                                        .Replace("%artist%", fileNameArtist)
                                        .Replace("%ext%", xmlExt[j].InnerText)
                                        .Replace("%fav_count%", xmlFavCount[j].InnerText)
                                        .Replace("%score%", xmlScore[j].InnerText)
                                        .Replace("%scoreup%", xmlScoreUp[j].InnerText)
                                        .Replace("%scoredown%", xmlScoreDown[j].InnerText)
                                        .Replace("%author%", xmlAuthor[j].InnerText) + "." + xmlExt[j].InnerText;

                            string fileUrl = xmlURL[j].InnerText;
                            if (string.IsNullOrEmpty(fileUrl)) {
                                if (xmlDeleted[j].InnerText.ToLower() == "false") {
                                    fileUrl = apiTools.GetBlacklistedImageUrl(xmlMD5[j].InnerText, xmlExt[j].InnerText);
                                }
                                else {
                                    continue;
                                }
                            }

                            for (int k = 0; k < PostTags.Count; k++) {
                                if (DownloadInfo.Graylist.Length > 0) {
                                    for (int l = 0; l < DownloadInfo.Graylist.Length; l++) {
                                        if (PostTags[k] == DownloadInfo.Graylist[l]) {
                                            foundBlacklistedTags += " " + DownloadInfo.Graylist[l];
                                            isBlacklisted = true;
                                            hasBlacklistedFiles = true;
                                        }
                                    }
                                }

                                if (DownloadInfo.Blacklist.Length > 0) {
                                    for (int l = 0; l < DownloadInfo.Blacklist.Length; l++) {
                                        if (PostTags[k] == DownloadInfo.Blacklist[l]) {
                                            foundBlacklistedTags += " " + DownloadInfo.Blacklist[l];
                                            isBlacklisted = true;
                                            hasBlacklistedFiles = true;
                                        }
                                    }
                                }
                            }


                            URLs.Add(fileUrl);
                            FileNames.Add(fileName);
                            if (isBlacklisted) {
                                urlBlacklisted.Add(true);
                                blacklistedPageCount++;
                            }
                            else {
                                urlBlacklisted.Add(false);
                                pageCount++;
                            }

                            updateTotals();

                            if (isBlacklisted && DownloadInfo.SaveBlacklistedFiles && !DownloadInfo.MergeBlacklisted) {
                                blacklistInfo += "    BLACKLISTED PAGE " + fileNamePage + ":\n" +
                                                 "        MD5: " + xmlMD5[j].InnerText + "\n" +
                                                 "        URL: https://e621.net/posts/" + xmlID[j].InnerText + "\n" +
                                                 "        ARTIST(S): " + artists + "\n" +
                                                 "        TAGS:" + ReadTags + "\n" +
                                                 "        SCORE: Up " + xmlScoreUp[j].InnerText + ", Down " + xmlScoreDown[j].InnerText + ", Total " + xmlScore[j].InnerText + "\n" +
                                                 "        RATING: " + rating + "\n" +
                                                 "        DESCRIPITON:" + poolDescription + "\n" +
                                                 "        OFFENDING TAGS: " + foundBlacklistedTags +
                                                 "\n\n";

                            }
                            else {
                                poolInfo += "    PAGE: " + fileNamePage + "\n" +
                                            "        MD5: " + xmlMD5[j].InnerText + "\n" +
                                            "        URL: https://e621.net/posts/" + xmlID[j].InnerText + "\n" +
                                            "        ARTIST(S): " + artists + "\n" +
                                            "        TAGS: " + ReadTags + "\n" +
                                            "        SCORE: Up " + xmlScoreUp[j].InnerText + ", Down " + xmlScoreDown[j].InnerText + ", Total " + xmlScore[j].InnerText + "\n" +
                                            "        RATING: " + rating + "\n" +
                                            "        DESCRIPITON:" + poolDescription +
                                            "\n\n";
                            }
                        }
                    }
                }
                #endregion

                #region pre-download checks
                // Check for files.
                if (URLs.Count <= 0) {
                    writeToConsole("No files were found while downloading. Press any key to continue...", true);
                    this.BeginInvoke(new MethodInvoker(() => {
                        status.Text = "URL count is empty. Empty pool?";
                        pbDownloadStatus.Style = ProgressBarStyle.Blocks;
                        tmrTitle.Stop();
                    }));
                    MessageBox.Show("No files are to be downloaded.");
                    return;
                }

                writeToConsole("There are " + URLs.Count + " files in total.", true);

                // Create directories.
                if (!Directory.Exists(DownloadInfo.DownloadPath))
                    Directory.CreateDirectory(DownloadInfo.DownloadPath);
                if (hasBlacklistedFiles && DownloadInfo.SaveBlacklistedFiles && !DownloadInfo.MergeBlacklisted && !Directory.Exists(DownloadInfo.DownloadPath + "\\blacklisted"))
                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted");
                #endregion

                #region Downloading
                // Save .nfo files.
                if (DownloadInfo.SaveInfo) {
                    poolInfo = poolInfo.TrimEnd('\n');
                    blacklistInfo = blacklistInfo.TrimEnd('\n');
                    writeToConsole("Saving pool.nfo");
                    File.WriteAllText(DownloadInfo.DownloadPath + "\\pool.nfo", poolInfo, Encoding.UTF8);
                    if (hasBlacklistedFiles && DownloadInfo.SaveBlacklistedFiles && !DownloadInfo.MergeBlacklisted) {
                        writeToConsole("Saving pool.blacklisted.nfo");
                        File.WriteAllText(DownloadInfo.DownloadPath + "\\blacklisted\\pool.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                    }
                }

                this.BeginInvoke(new MethodInvoker(() => {
                    pbDownloadStatus.Style = ProgressBarStyle.Blocks;
                    int total;

                    if (DownloadInfo.SaveBlacklistedFiles) {
                        total = (pageCount + blacklistedPageCount);

                        if (Directory.Exists(DownloadInfo.DownloadPath)) {
                            total -= apiTools.CountFiles(DownloadInfo.DownloadPath);
                            if (File.Exists(DownloadInfo.DownloadPath + "\\pool.nfo")) {
                                total--;
                            }

                            if (Directory.Exists(DownloadInfo.DownloadPath + "\\blacklisted")) {
                                total -= apiTools.CountFiles(DownloadInfo.DownloadPath + "\\blacklisted");
                                if (File.Exists(DownloadInfo.DownloadPath + "\\blacklisted\\pool.blacklisted.nfo")) {
                                    total--;
                                }
                            }
                        }

                        pbTotalStatus.Maximum = total;
                    }
                    else {
                        total = pageCount;
                        total -= apiTools.CountFiles(DownloadInfo.DownloadPath) - 1;

                        pbTotalStatus.Maximum = total;
                        //pbTotalStatus.Maximum = pageCount - apiTools.countFiles(saveTo);
                    }
                }));

                int currentDownloadFile = 1;
                // Download pool.
                writeToConsole("Starting pool download...", true);
                string outputBar = string.Empty;
                currentPage = 0;
                using (ExWebClient wc = new ExWebClient()) {
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
                            currentDownloadFile++;
                            Monitor.Pulse(e.UserState);
                        }
                    };

                    wc.Proxy = WebRequest.GetSystemWebProxy();
                    wc.Headers.Add("user-agent", Program.UserAgent);
                    wc.Method = "GET";

                    for (int y = 0; y < URLs.Count; y++) {
                        CurrentURL = URLs[y].Replace("www.", "");
                        currentPage++;
                        if (urlBlacklisted[y] && !DownloadInfo.SaveBlacklistedFiles)
                            continue;

                        string fileName = "\\" + FileNames[y];

                        this.BeginInvoke(new MethodInvoker(() => {
                            lbFile.Text = "File " + (currentDownloadFile) + " of " + (pageCount + blacklistedPageCount);
                            status.Text = "Downloading " + fileName;
                        }));

                        if (urlBlacklisted[y] && !DownloadInfo.MergeBlacklisted) {
                            if (File.Exists(DownloadInfo.DownloadPath + "\\blacklisted" + fileName))
                                continue;

                            var sync = new Object();
                            lock (sync) {
                                wc.DownloadFileAsync(new Uri(CurrentURL), DownloadInfo.DownloadPath + "\\blacklisted" + fileName, sync);
                                Monitor.Wait(sync);
                            }
                        }
                        else {
                            if (File.Exists(DownloadInfo.DownloadPath + fileName))
                                continue;

                            var sync = new Object();
                            lock (sync) {
                                wc.DownloadFileAsync(new Uri(CurrentURL), DownloadInfo.DownloadPath + fileName, sync);
                                Monitor.Wait(sync);
                            }
                        }
                    }
                }
                #endregion

                // Finish the job.
                this.BeginInvoke(new MethodInvoker(() => {
                    pbDownloadStatus.Value = 100;
                    pbTotalStatus.Value = pbTotalStatus.Maximum;
                    lbPercentage.Text = "100%";
                    lbFile.Text = "All files downloaded.";
                }));

                DownloadHasFinished = true;
            }
            catch (ThreadAbortException) {
                apiTools.SendDebugMessage("Thread was requested to be, and has been, aborted. (frmPoolDownloader.cs)");
                DownloadHasAborted = true;
            }
            catch (ObjectDisposedException) {
                apiTools.SendDebugMessage("Seems like the object got disposed. (frmPoolDownloader.cs)");
                DownloadHasErrored = true;
            }
            catch (WebException WebE) {
                apiTools.SendDebugMessage("A WebException has occured. (frmPoolDownloader.cs)");
                this.BeginInvoke(new MethodInvoker(() => {
                    status.Text = "A WebException has occured";
                    pbDownloadStatus.State = ProgressBarState.Error;
                    pbTotalStatus.State = ProgressBarState.Error;
                }));
                ErrorLog.ReportWebException(WebE, "frmPoolDownloader.cs", CurrentURL);
                DownloadHasErrored = true;
            }
            catch (Exception ex) {
                apiTools.SendDebugMessage("An exception has occured. (frmPoolDownloader.cs)");
                this.BeginInvoke(new MethodInvoker(() => {
                    status.Text = "A Exception has occured";
                    pbDownloadStatus.State = ProgressBarState.Error;
                    pbTotalStatus.State = ProgressBarState.Error;
                }));
                ErrorLog.ReportException(ex, "frmPoolDownloader.cs", false);
                DownloadHasErrored = true;
            }
            finally {
                this.BeginInvoke(new MethodInvoker(() => { AfterDownload(); }));
            }
        }

        public void updateTotals() {
            this.BeginInvoke(new MethodInvoker(() => {
                lbTotal.Text = (pageCount) + " clean pages\n" +
                               (blacklistedPageCount) + " blacklisted pages\n" +
                               (pageCount + blacklistedPageCount) + " total pages";
                if (DownloadInfo.SaveBlacklistedFiles)
                    lbFile.Text = "File 1 of " + (pageCount + blacklistedPageCount);
                else
                    lbFile.Text = "File 1 of " + (pageCount);
            }));
        }
        public void writeToConsole(string message, bool important = false) {
            apiTools.SendDebugMessage(message);
        }
        #endregion

    }
}