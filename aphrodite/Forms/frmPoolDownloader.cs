using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using murrty.classcontrols;

namespace aphrodite {

    public partial class frmPoolDownloader : ExtendedForm {

        #region Fields
        /// <summary>
        /// The constant string of the pool api used for gathering pool info.
        /// </summary>
        private const string ApiPoolBase = "https://e621.net/pools/{0}.json";
        /// <summary>
        /// The constant string of the page base used for parsing the api.
        /// </summary>
        private const string ApiPageBase = "https://e621.net/posts.json?tags=pool:{0}+order:id&limit=320&page={1}";

        /// <summary>
        /// The information about the download of this current instance.
        /// </summary>
        public readonly PoolDownloadInfo DownloadInfo;

        /// <summary>
        /// Contains the count of explicit pages that will be downloaded in this instance.
        /// </summary>
        private int CleanExplicitPagesCount = 0;
        /// <summary>
        /// Contains the count of questionable pages that will be downloaded in this instance.
        /// </summary>
        private int CleanQuestionablePagesCount = 0;
        /// <summary>
        /// Contains the count of safe pages that will be downloaded in this instance.
        /// </summary>
        private int CleanSafePagesCount = 0;
        /// <summary>
        /// Contains the count of explicit pages that already exist.
        /// </summary>
        private int CleanExplicitPagesExistCount = 0;
        /// <summary>
        /// Contains the count of questionable pages that already exist.
        /// </summary>
        private int CleanQuestionablePagesExistCount = 0;
        /// <summary>
        /// Contains the count of safe pages that already exist.
        /// </summary>
        private int CleanSafePagesExistCount = 0;

        /// <summary>
        /// Contains the count of explicit graylisted pages that will be downloaded in this instance.
        /// </summary>
        private int GraylistedExplicitPagesCount = 0;
        /// <summary>
        /// Contains the count of questionable graylisted pages that will be downloaded in this instance.
        /// </summary>
        private int GraylistedQuestionablePagesCount = 0;
        /// <summary>
        /// Contains the count of safe graylisted pages that will be downloaded in this instance.
        /// </summary>
        private int GraylistedSafePagesCount = 0;
        /// <summary>
        /// Contains the count of explicit graylisted pages that already exist.
        /// </summary>
        private int GraylistedExplicitPagesExistCount = 0;
        /// <summary>
        /// Contains the count of questionable graylisted pages that already exist.
        /// </summary>
        private int GraylistedQuestionablePagesExistCount = 0;
        /// <summary>
        /// Contains the count of safe graylisted pages that already exist.
        /// </summary>
        private int GraylistedSafePagesExistCount = 0;

        /// <summary>
        /// Contains the count of explicit blacklisted pages that will be downloaded in this instance.
        /// </summary>
        private int BlacklistedExplicitPagesCount = 0;
        /// <summary>
        /// Contains the count of questionable blacklisted pages that will be downloaded in this instance.
        /// </summary>
        private int BlacklistedQuestionablePagesCount = 0;
        /// <summary>
        /// Contains the count of safe blacklisted pages that will be downloaded in this instance.
        /// </summary>
        private int BlacklistedSafePagesCount = 0;
        /// <summary>
        /// Contains the count of explicit blacklisted pages that already exist.
        /// </summary>
        private int BlacklistedExplicitPagesExistCount = 0;
        /// <summary>
        /// Contains the count of questionable blacklisted pages that already exist.
        /// </summary>
        private int BlacklistedQuestionablePagesExistCount = 0;
        /// <summary>
        /// Contains the count of safe blacklisted pages that already exist.
        /// </summary>
        private int BlacklistedSafePagesExistCount = 0;

        /// <summary>
        /// Contains the count of graylisted pages skipped during this instance, if not downloading graylisted files.
        /// </summary>
        private int GraylistedPagesSkippedCount = 0;
        /// <summary>
        /// Contains the count of blacklisted pages skipped during this instance, if not downloading blacklisted files.
        /// </summary>
        private int BlacklistedPagesSkippedCount = 0;
        /// <summary>
        /// Contains the count of pages that have been parsed during this instance.
        /// </summary>
        private int TotalPagesParsed = 0;

        /// <summary>
        /// Contains the count of pages that have been downloaded during this instance. Used for total progress tracking with pbTotalStatus.
        /// </summary>
        private int DownloadedPages = 0;
        /// <summary>
        /// Contains the total count of pages that will be downloaded. Updated before the download loops begin.
        /// </summary>
        private int CleanPagesToDownload = 0;
        /// <summary>
        /// Contains the total count of graylisted pages that will be downloaded. Updated before the download loops begin.
        /// </summary>
        private int GraylistedPagesToDownload = 0;
        /// <summary>
        /// Contains the total count of blacklisted pages that will be downloaded. Updated before the download loops begin.
        /// </summary>
        private int BlacklistedPagesToDownload = 0;
        /// <summary>
        /// Contains the total count of pages that will be downloaded. Updated before the download loops begin.
        /// </summary>
        private int TotalPagesToDownload = 0;

        /// <summary>
        /// Whether the hardcoded artist filter should be used.
        /// </summary>
        private bool UseHardcodedFilter = false;
        #endregion

        public frmPoolDownloader(PoolDownloadInfo NewInfo) {
            InitializeComponent();

            if (Config.ValidPoint(Config.Settings.FormSettings.frmPoolDownloader_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmPoolDownloader_Location;
            }

            DownloadInfo = NewInfo;
        }

        private void frmPoolDownloaderUpdated_FormClosing(object sender, FormClosingEventArgs e) {
            Config.Settings.FormSettings.frmPoolDownloader_Location = this.Location;
            switch (Status) {
                case DownloadStatus.Finished:
                case DownloadStatus.Errored:
                case DownloadStatus.Aborted:
                case DownloadStatus.Forbidden:
                case DownloadStatus.AlreadyBeingDownloaded:
                case DownloadStatus.FileAlreadyExists:
                case DownloadStatus.NothingToDownload:
                case DownloadStatus.PostOrPoolWasDeleted:
                case DownloadStatus.ApiReturnedNullOrEmpty:
                case DownloadStatus.FileWasNullAfterBypassingBlacklist: {
                    this.Dispose();
                } break;

                default: {
                    switch (Status) {
                        case DownloadStatus.Waiting:
                        case DownloadStatus.Parsing:
                        case DownloadStatus.ReadyToDownload:
                        case DownloadStatus.Downloading: {
                            e.Cancel = true;
                            ExitBox = false;
                            this.Text = "Cancelling...";
                            pbDownloadStatus.Text = "Cancelling download, cleaning up...";
                            pbTotalStatus.Text = "The thread needs to end peacefully";
                            this.DownloadAbort = true;
                            Status = DownloadStatus.Aborted;
                            if (DownloadThread != null && DownloadThread.IsAlive) {
                                DownloadThread.Abort();
                            }
                        } break;

                        default: {
                            e.Cancel = !DownloadInfo.IgnoreFinish;
                        } break;
                    }
                } break;
            }
        }

        protected override void PrepareDownload() {
            lbPoolId.Text = "null";
            if (DownloadInfo == null) {
                this.Text = "Unable to download pool";
                pbDownloadStatus.Text = "Cannot download";
                pbTotalStatus.Text = "Download info is null";
                sbStatus.Text = "Info null";
            }
            else if(string.IsNullOrWhiteSpace(DownloadInfo.PoolId)) {
                this.Text = "Unable to download pool";
                pbDownloadStatus.Text = "Cannot download";
                pbTotalStatus.Text = "No pool was selected.";
                sbStatus.Text = "No pool ID";
            }
            else if (Downloader.PoolsBeingDownloaded.Contains(DownloadInfo.PoolId)) {
                lbPoolId.Text = $"Pool ID: {DownloadInfo.PoolId}";
                Status = DownloadStatus.AlreadyBeingDownloaded;
                this.Text = "Unable to download pool";
                pbDownloadStatus.Text = "Pool already being downloaded";
                pbTotalStatus.Text = "Cannot download pool";
                sbStatus.Text = "Already in queue";
            }
            else {
                if (string.IsNullOrWhiteSpace(DownloadInfo.FileNameSchema)) {
                    DownloadInfo.FileNameSchema = "%poolname%_%page%";
                }

                if (!DownloadInfo.DownloadPath.EndsWith("\\Pools")) {
                    DownloadInfo.DownloadPath += "\\Pools";
                }

                UseHardcodedFilter = DownloadInfo.UndesiredTags.Length > 0;

                lbFileStatus.Text =
                    "total pages parsed: 0\n\n" +

                    "pages: 0 (0 E, 0 Q, 0 S)\n" +
                    $"graylisted: {(DownloadInfo.SaveGraylistedFiles ? "0 (0 E, 0 Q, 0 S)" : "not saving, skipped 0 pages")}\n" +
                    $"blacklisted: {(DownloadInfo.SaveBlacklistedFiles ? "0 (0 E, 0 Q, 0 S)" : "not saving, skipped 0 pages")}\n" +
                    "total to download: 0\n\n" +

                    "existing pages: 0 (0 E, 0 Q, 0 S)\n" +
                    $"existing graylisted: {(DownloadInfo.SaveGraylistedFiles ? "0 (0 E, 0 Q, 0 S)" : "not checking existing pages")}\n" +
                    $"existing blacklisted: {(DownloadInfo.SaveBlacklistedFiles ? "0 (0 E, 0 Q, 0 S)" : "not checking existing pages")}\n" +
                    "total pages that exist: 0";

                lbPoolId.Text = $"Pool ID: {DownloadInfo.PoolId}";
                chkMergeGraylistedPages.Checked = DownloadInfo.MergeGraylistedPages;
                chkMergeBlacklistedPages.Checked = DownloadInfo.MergeBlacklistedPages;

                base.PrepareDownload();
            }
        }

        protected override void StartDownload() {
            DownloadThread = new(async() => {

                #region Try block
                try {

                    #region Defining new variables
                    byte[] ApiData;                         // The data byte-array the API returns. It's a byte array so it's faster to translate to xml.
                    string CurrentXml = string.Empty;       // The string of the current XML to be loaded into a XmlDocument;
                    List<string> PageURLs = new();          // The list containing the links to the pages to be downloaded.
                    List<string> PageFileNames = new();     // The list containing the name of the files to display to the user.
                    List<string> PageFilePaths = new();     // The list containing full-paths of the pages to be download to.
                    List<string> PagePostIDs = new();       // The list containing all the IDs of the pool, for downloading in chronological order.
                    string PoolName = string.Empty;         // The name of the pool.
                    string PoolDescription = string.Empty;  // The description of the pool.
                    string PoolInfo = string.Empty;         // The buffer for the pool.nfo file.
                    bool PoolHasGraylistedPages = false;    // Whether the pool contains graylisted pages.
                    bool PoolHasBlacklistedPages = false;   // Whether the pool contains blacklisted pages.
                    int CurrentPage = 0;                    // The current page of the pool (?)
                    #endregion

                    #region First parse the pool to retrieve pool information
                    // Begin the XML download
                    Status = DownloadStatus.Parsing;
                    this.Invoke((Action)delegate () {
                        Log.Write("Downloading pool information. (frmPoolDownloader.cs)");
                        sbStatus.Text = "Getting pool information...";
                    });
                    CurrentUrl = string.Format(ApiPoolBase, DownloadInfo.PoolId);
                    ShouldRetry = true;
                    do {
                        try {
                            using (DownloadClient = new()) {
                                DownloadClient.Proxy = WebRequest.GetSystemWebProxy();
                                DownloadClient.UserAgent = Program.UserAgent;
                                DownloadClient.Method = HttpMethod.GET;

                                //ApiData = await DownloadClient.DownloadDataTaskAsync(CurrentUrl);
                                ApiData = DownloadClient.DownloadData(CurrentUrl);
                                CurrentXml = ApiTools.ConvertJsonToXml(Encoding.UTF8.GetString(ApiData));

                                if (ApiTools.IsXmlDead(CurrentXml)) {
                                    DownloadError = true;
                                    Status = DownloadStatus.ApiReturnedNullOrEmpty;
                                    throw new ApiReturnedNullOrEmptyException();
                                }
                                ShouldRetry = false;
                            }
                        }
                        catch (ThreadAbortException) {
                            return;
                        }
                        catch (Exception ex) {
                            if (ex is WebException webex && ((HttpWebResponse)webex.Response).StatusCode == HttpStatusCode.Forbidden) {
                                Status = DownloadStatus.Forbidden;
                                DownloadError = true;
                                ShouldRetry = false;
                            }
                            else {
                                if (Log.ReportRetriableException(ex, CurrentUrl) != DialogResult.Retry) {
                                    Status = DownloadStatus.Errored;
                                    DownloadError = true;
                                    ShouldRetry = false;
                                }
                            }
                        }
                    } while (ShouldRetry && !DownloadAbort);

                    if (DownloadError || DownloadAbort) {
                        return;
                    }

                    // Check the XML.
                    xmlDoc = new();
                    xmlDoc.LoadXml(CurrentXml);
                    CurrentXml = string.Empty;

                    XmlNodeList xmlPoolName = xmlDoc.DocumentElement.SelectNodes("/root/name");
                    XmlNodeList xmlPoolDescription = xmlDoc.DocumentElement.SelectNodes("/root/description");
                    XmlNodeList xmlPoolDeleted = xmlDoc.DocumentElement.SelectNodes("/root/is_deleted");
                    XmlNodeList xmlPageCount = xmlDoc.DocumentElement.SelectNodes("/root/post_count");
                    XmlNodeList xmlPageIDs = xmlDoc.DocumentElement.SelectNodes("/root/post_ids/item");

                    if (xmlPoolDeleted[0].InnerText.ToLower() != "false") {
                        // idk how to handle it because I haven't found a deleted pool yet
                        throw new PoolOrPostWasDeletedException("Pool was deleted");
                    }
                    if (DownloadInfo.SaveInfo) {
                        PoolDescription = "No description";
                        if (xmlPoolDescription[0].InnerText != "") {
                            PoolDescription = "\"" + xmlPoolDescription[0].InnerText + "\"";
                        }

                        PoolInfo += "POOL: " + DownloadInfo.PoolId + "\n" +
                                    "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\n" +
                                    "    NAME: " + xmlPoolName[0].InnerText + "\n" +
                                    "    PAGES: " + xmlPageCount[0].InnerText + "\n" +
                                    "    URL: https://e621.net/pool/show/" + DownloadInfo.PoolId + "\n" +
                                    "    DESCRIPTION:\n    " + PoolDescription +
                                    "\n\n";
                    }

                    // Add the pages in their pool index
                    // (so that way the page is true to the pool)
                    for (int i = 0; i < xmlPageIDs.Count; i++) {
                        PagePostIDs.Add(xmlPageIDs[i].InnerText);
                    }

                    // Count the image count and do math for pages.
                    int TotalPagesToParse = ApiTools.CountPoolPages(Convert.ToDecimal(xmlPageCount[0].InnerText));

                    // Set the output folder name.
                    PoolName = ApiTools.ReplaceIllegalCharacters(xmlPoolName[0].InnerText);
                    DownloadInfo.DownloadPath += "\\" + PoolName;
                    this.Invoke((Action)delegate () {
                        txtPoolName.Text = xmlPoolName[0].InnerText;
                        Log.Write("Updated saveTo to \"\\Pools\\" + PoolName + "\". (frmPoolDownloader.cs)");
                    });
                    #endregion

                    #region Parse the api for every page
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

                    Status = DownloadStatus.Parsing;

                    int NewFiles = 0;               // The count of new files that will be downloaded.
                    int NewFilesSkipped = 0;        // The count of files that will NOT be downloaded.
                    bool PageIsGraylisted;          // Whether the page is graylisted
                    bool PageIsBlacklisted;         // Whether the page is blacklisted
                    string FoundGraylistedTags;     // The found graylisted tags.
                    string FoundBlacklistedTags;    // The found blacklisted tags.
                    string PageRating;              // The rating of the page.
                    List<string> PostTags = new();  // The list of the tags on the page.
                    string ReadTags;                // The read tags of the page.

                    // Begin ripping the rest of the pool Json.
                    for (int ApiPage = 1; ApiPage < TotalPagesToParse + 1; ApiPage++) {

                        #region page download + xml elements
                        this.Invoke((Action)delegate () {
                            sbStatus.Text = $"Downloading page {ApiPage}";
                        });

                        CurrentUrl = string.Format(ApiPageBase, DownloadInfo.PoolId, ApiPage);

                        ShouldRetry = true;
                        do {
                            try {
                                using (DownloadClient = new()) {
                                    DownloadClient.Proxy = WebRequest.GetSystemWebProxy();
                                    DownloadClient.UserAgent = Program.UserAgent;
                                    DownloadClient.Method = HttpMethod.GET;

                                    //ApiData = await DownloadClient.DownloadDataTaskAsync(CurrentUrl);
                                    ApiData = DownloadClient.DownloadData(CurrentUrl);
                                    CurrentXml = ApiTools.ConvertJsonToXml(Encoding.UTF8.GetString(ApiData));

                                    if (ApiTools.IsXmlDead(CurrentXml) & ApiPage == 1) {
                                        DownloadError = true;
                                        Status = DownloadStatus.ApiReturnedNullOrEmpty;
                                        throw new ApiReturnedNullOrEmptyException();
                                    }
                                    ShouldRetry = false;
                                }
                            }
                            catch (ThreadAbortException) {
                                return;
                            }
                            catch (Exception ex) {
                                if (Log.ReportRetriableException(ex, CurrentUrl) != DialogResult.Retry) {
                                    if (ApiPage == 1) {
                                        Status = DownloadStatus.Errored;
                                        DownloadError = true;
                                    }
                                    ShouldRetry = false;
                                }
                            }
                        } while (ShouldRetry && !DownloadAbort);

                        if (DownloadAbort) return;
                        if (DownloadError) {
                            if (ApiPage == 1) return;
                            else break;
                        }

                        xmlDoc = new();
                        xmlDoc.LoadXml(CurrentXml);
                        CurrentXml = string.Empty;

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
                        #endregion

                        for (int CurrentPost = 0; CurrentPost < xmlID.Count; CurrentPost++) {

                            this.Invoke((Action)delegate () {
                                sbStatus.Text = $"Parsing page {CurrentPost + 1}";
                            });

                            NewFiles = 0;
                            NewFilesSkipped = 0;
                            CurrentPage++;
                            PageIsGraylisted = false;
                            PageIsBlacklisted = false;
                            FoundGraylistedTags = string.Empty;
                            FoundBlacklistedTags = string.Empty;
                            PageRating = string.Empty;
                            PostTags.Clear();
                            ReadTags = string.Empty;

                            // first check for the null url (e621's hard blacklist for users with no accounts)
                            string fileUrl = xmlURL[CurrentPost].InnerText;
                            if (string.IsNullOrEmpty(fileUrl)) {
                                if (xmlDeleted[CurrentPost].InnerText.ToLower() == "false") {
                                    fileUrl = ApiTools.GetBlacklistedImageUrl(xmlMD5[CurrentPost].InnerText, xmlExt[CurrentPost].InnerText);
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
                                            FoundGraylistedTags += " " + DownloadInfo.Graylist[k];
                                            PageIsGraylisted = true;
                                            PoolHasGraylistedPages = true;
                                        }
                                    }
                                }

                                if (DownloadInfo.Blacklist.Length > 0) {
                                    for (int k = 0; k < DownloadInfo.Blacklist.Length; k++) {
                                        if (PostTags[j] == DownloadInfo.Blacklist[k]) {
                                            FoundBlacklistedTags += DownloadInfo.Blacklist[k] + ", ";
                                            PageIsBlacklisted = true;
                                            PoolHasBlacklistedPages = true;
                                        }
                                    }
                                }
                            }
                            FoundBlacklistedTags = "[ " + FoundBlacklistedTags.Trim(' ').TrimEnd(',') + " ]";
                            #endregion

                            #region File name fiddling
                            PageRating = xmlRating[CurrentPost].InnerText switch {
                                "e" => "Explicit",
                                "q" => "Questionable",
                                "s" => "Safe",
                                _ => "Unknown"
                            };

                            string fileNamePage = (PagePostIDs.IndexOf(xmlID[CurrentPost].InnerText) + 1).ToString("0000.##");

                            // File name artist for the schema
                            string fileNameArtist = "(none)";
                            if (xmlTagsArtist[CurrentPost].ChildNodes.Count > 0) {
                                for (int CurrentArtistTag = 0; CurrentArtistTag < xmlTagsArtist[CurrentPost].ChildNodes.Count; CurrentArtistTag++) {
                                    if (UseHardcodedFilter) {
                                        if (!UndesiredTags.IsUndesiredHardcoded(xmlTagsArtist[CurrentPost].ChildNodes[CurrentArtistTag].InnerText)) {
                                            fileNameArtist = xmlTagsArtist[CurrentPost].ChildNodes[CurrentArtistTag].InnerText;
                                            break;
                                        }
                                    }
                                    else {
                                        if (!UndesiredTags.IsUndesired(xmlTagsArtist[CurrentPost].ChildNodes[CurrentArtistTag].InnerText, DownloadInfo.UndesiredTags)) {
                                            fileNameArtist = xmlTagsArtist[CurrentPost].ChildNodes[CurrentArtistTag].InnerText;
                                            break;
                                        }
                                    }
                                }
                            }

                            string fileName = 
                                DownloadInfo.FileNameSchema
                                    .Replace("%poolname%", PoolName)
                                    .Replace("%poolid%", DownloadInfo.PoolId)
                                    .Replace("%page%", fileNamePage)
                                    .Replace("%md5%", xmlMD5[CurrentPost].InnerText)
                                    .Replace("%id%", xmlID[CurrentPost].InnerText)
                                    .Replace("%rating%", PageRating.ToLower())
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
                                PoolDescription = " No description";
                                if (xmlDescription[CurrentPost].InnerText != "") {
                                    PoolDescription = "\n                    \"" + xmlDescription[CurrentPost].InnerText + "\"";
                                }

                                PoolInfo +=
                                    $"    {(PageIsBlacklisted ? "BLACKLISTED " : (PageIsGraylisted ? " GRAYLISTED " : ""))}PAGE {fileNamePage}\n" +
                                    $"        MD5: {xmlMD5[CurrentPost].InnerText}\n" +
                                    $"        URL: https://e621.net/posts/{xmlID[CurrentPost].InnerText}\n" +
                                    $"{(PageIsBlacklisted ? $"        OFFENDING TAGS: {FoundBlacklistedTags.Trim() + (PageIsGraylisted ? " |<~Bl|Gr~>| " + FoundGraylistedTags.Trim() : "")}\n" : (PageIsGraylisted ? $"        OFFENDING TAGS: {FoundGraylistedTags.Trim()}\n" : ""))}" +
                                    $"        TAGS: {ReadTags}\n" +
                                    $"        SCORE: Up {xmlScoreUp[CurrentPost].InnerText}, Down {xmlScoreDown[CurrentPost].InnerText}, Total {xmlScore[CurrentPost].InnerText}\n" +
                                    $"        RATING: {PageRating}\n" +
                                    $"        DESCRIPTION: {PoolDescription}";
                            }
                            #endregion

                            #region Counts + lists
                            string NewPath = DownloadInfo.DownloadPath;

                            switch (xmlRating[CurrentPost].InnerText) {
                                case "e": case "explicit":
                                    if (PageIsBlacklisted) {
                                        if (!DownloadInfo.SaveBlacklistedFiles) {
                                            BlacklistedPagesSkippedCount++;
                                            NewFilesSkipped++;
                                            TotalPagesParsed++;
                                            continue;
                                        }

                                        if (!DownloadInfo.MergeBlacklistedPages) {
                                            NewPath += "\\blacklisted";
                                        }

                                        if (File.Exists(NewPath + "\\" + fileName)) {
                                            BlacklistedExplicitPagesExistCount++;
                                            TotalPagesParsed++;
                                            continue;
                                        }
                                        else {
                                            BlacklistedExplicitPagesCount++;
                                            NewFiles++;
                                            TotalPagesParsed++;
                                        }
                                    }
                                    else if (PageIsGraylisted) {
                                        if (!DownloadInfo.SaveGraylistedFiles) {
                                            GraylistedPagesSkippedCount++;
                                            NewFilesSkipped++;
                                            TotalPagesParsed++;
                                            continue;
                                        }

                                        if (!DownloadInfo.MergeGraylistedPages) {
                                            NewPath += "\\graylisted";
                                        }

                                        if (File.Exists(NewPath + "\\" + fileName)) {
                                            GraylistedExplicitPagesExistCount++;
                                            TotalPagesParsed++;
                                            continue;
                                        }
                                        else {
                                            GraylistedExplicitPagesCount++;
                                            NewFiles++;
                                            TotalPagesParsed++;
                                        }
                                    }
                                    else {
                                        if (File.Exists(NewPath + "\\" + fileName)) {
                                            CleanExplicitPagesExistCount++;
                                            TotalPagesParsed++;
                                            continue;
                                        }
                                        else {
                                            CleanExplicitPagesCount++;
                                            NewFiles++;
                                            TotalPagesParsed++;
                                        }
                                    }
                                    break;

                                case "q": case "questionable":
                                    if (PageIsBlacklisted) {
                                        if (!DownloadInfo.SaveBlacklistedFiles) {
                                            BlacklistedPagesSkippedCount++;
                                            NewFilesSkipped++;
                                            TotalPagesParsed++;
                                            continue;
                                        }

                                        if (!DownloadInfo.MergeBlacklistedPages) {
                                            NewPath += "\\blacklisted";
                                        }

                                        if (File.Exists(NewPath + "\\" + fileName)) {
                                            BlacklistedQuestionablePagesExistCount++;
                                            TotalPagesParsed++;
                                            continue;
                                        }
                                        else {
                                            BlacklistedQuestionablePagesCount++;
                                            NewFiles++;
                                            TotalPagesParsed++;
                                        }
                                    }
                                    else if (PageIsGraylisted) {
                                        if (!DownloadInfo.SaveGraylistedFiles) {
                                            GraylistedPagesSkippedCount++;
                                            NewFilesSkipped++;
                                            TotalPagesParsed++;
                                            continue;
                                        }

                                        if (!DownloadInfo.MergeGraylistedPages) {
                                            NewPath += "\\graylisted";
                                        }

                                        if (File.Exists(NewPath + "\\" + fileName)) {
                                            GraylistedQuestionablePagesExistCount++;
                                            TotalPagesParsed++;
                                            continue;
                                        }
                                        else {
                                            GraylistedQuestionablePagesCount++;
                                            NewFiles++;
                                            TotalPagesParsed++;
                                        }
                                    }
                                    else {
                                        if (File.Exists(NewPath + "\\" + fileName)) {
                                            CleanQuestionablePagesExistCount++;
                                            TotalPagesParsed++;
                                            continue;
                                        }
                                        else {
                                            CleanQuestionablePagesCount++;
                                            NewFiles++;
                                            TotalPagesParsed++;
                                        }
                                    }
                                    break;

                                case "s": case "safe":
                                    if (PageIsBlacklisted) {
                                        if (!DownloadInfo.SaveBlacklistedFiles) {
                                            BlacklistedPagesSkippedCount++;
                                            NewFilesSkipped++;
                                            TotalPagesParsed++;
                                            continue;
                                        }

                                        if (!DownloadInfo.MergeBlacklistedPages) {
                                            NewPath += "\\blacklisted";
                                        }

                                        if (File.Exists(NewPath + "\\" + fileName)) {
                                            BlacklistedSafePagesExistCount++;
                                            TotalPagesParsed++;
                                            continue;
                                        }
                                        else {
                                            BlacklistedSafePagesCount++;
                                            NewFiles++;
                                            TotalPagesParsed++;
                                        }
                                    }
                                    else if (PageIsGraylisted) {
                                        if (!DownloadInfo.SaveGraylistedFiles) {
                                            GraylistedPagesSkippedCount++;
                                            NewFilesSkipped++;
                                            TotalPagesParsed++;
                                            continue;
                                        }

                                        if (!DownloadInfo.MergeGraylistedPages) {
                                            NewPath += "\\graylisted";
                                        }

                                        if (File.Exists(NewPath + "\\" + fileName)) {
                                            GraylistedSafePagesExistCount++;
                                            TotalPagesParsed++;
                                            continue;
                                        }
                                        else {
                                            GraylistedSafePagesCount++;
                                            NewFiles++;
                                            TotalPagesParsed++;
                                        }
                                    }
                                    else {
                                        if (File.Exists(NewPath + "\\" + fileName)) {
                                            CleanSafePagesExistCount++;
                                            TotalPagesParsed++;
                                            continue;
                                        }
                                        else {
                                            CleanSafePagesCount++;
                                            NewFiles++;
                                            TotalPagesParsed++;
                                        }
                                    }
                                    break;
                            }

                            PageURLs.Add(fileUrl);
                            PageFileNames.Add(fileName);
                            PageFilePaths.Add(NewPath + "\\" + fileName);
                            #endregion

                        }

                        #region update totals
                        Log.Write($"{NewFiles} files will be downloaded, {NewFilesSkipped} will be skipped");
                        this.Invoke((Action)delegate () {
                            lbFileStatus.Text =
                                $"total pages parsed: {TotalPagesParsed:N0}\n\n" +

                                $"pages: {CleanExplicitPagesCount + CleanQuestionablePagesCount + CleanSafePagesCount:N0} ({CleanExplicitPagesCount:N0} E, {CleanQuestionablePagesCount:N0} Q, {CleanSafePagesCount:N0} S)\n" +
                                $"graylisted: {(DownloadInfo.SaveGraylistedFiles ? $"{GraylistedExplicitPagesCount + GraylistedQuestionablePagesCount + GraylistedSafePagesCount:N0} ({GraylistedExplicitPagesCount:N0} E, {GraylistedQuestionablePagesCount:N0} Q, {GraylistedSafePagesCount:N0} S)" : $"not saving, skipped {GraylistedPagesSkippedCount:N0} pages")}\n" +
                                $"blacklisted: {(DownloadInfo.SaveBlacklistedFiles ? $"{BlacklistedExplicitPagesCount + BlacklistedQuestionablePagesCount + BlacklistedSafePagesCount:N0} ({BlacklistedExplicitPagesCount:N0} E, {BlacklistedQuestionablePagesCount:N0} Q, {BlacklistedSafePagesCount:N0} S)" : $"not saving, skipped {BlacklistedPagesSkippedCount:N0} pages")}\n" +
                                $"total pages to download: {CleanExplicitPagesCount + CleanQuestionablePagesCount + CleanSafePagesCount + GraylistedExplicitPagesCount + GraylistedQuestionablePagesCount + GraylistedSafePagesCount + BlacklistedExplicitPagesCount + BlacklistedQuestionablePagesCount + BlacklistedSafePagesCount:N0}\n\n" +

                                $"existing pages: {CleanExplicitPagesExistCount + CleanQuestionablePagesExistCount + CleanSafePagesExistCount:N0} ({CleanExplicitPagesExistCount:N0} E, {CleanQuestionablePagesExistCount:N0} Q, {CleanSafePagesExistCount:N0} S)\n" +
                                $"existing graylisted: {(DownloadInfo.SaveGraylistedFiles ? $"{GraylistedExplicitPagesExistCount + GraylistedQuestionablePagesExistCount + GraylistedSafePagesExistCount:N0} ({GraylistedExplicitPagesExistCount:N0} E, {GraylistedQuestionablePagesExistCount:N0} Q, {GraylistedSafePagesExistCount:N0} S)" : $"not checking existing pages")}\n" +
                                $"existing blacklisted: {(DownloadInfo.SaveBlacklistedFiles ? $"{BlacklistedExplicitPagesExistCount + BlacklistedQuestionablePagesExistCount + BlacklistedSafePagesExistCount:N0} ({BlacklistedExplicitPagesExistCount:N0} E, {BlacklistedQuestionablePagesExistCount:N0} Q, {BlacklistedSafePagesExistCount:N0} S)" : $"not checking existing pages")}\n" +
                                $"total pages that exist: {CleanExplicitPagesExistCount + CleanQuestionablePagesExistCount + CleanSafePagesExistCount + GraylistedExplicitPagesExistCount + GraylistedQuestionablePagesExistCount + GraylistedSafePagesExistCount + BlacklistedExplicitPagesExistCount + BlacklistedQuestionablePagesExistCount + BlacklistedSafePagesExistCount:N0}";
                        });
                        #endregion

                    }
                    #endregion

                    #region Pre-download stuff
                    if (PageURLs.Count < 1) {
                        throw new NoFilesToDownloadException("No pages are able to be downloaded.");
                    }

                    Status = DownloadStatus.ReadyToDownload;

                    this.Invoke((Action)delegate () {
                        Log.Write($"There are {PageURLs.Count:N0} pages to download for pool {DownloadInfo.PoolId}.");
                        sbStatus.Text = "Preparing download...";
                    });

                    if (!Directory.Exists(DownloadInfo.DownloadPath)) {
                        Directory.CreateDirectory(DownloadInfo.DownloadPath);
                    }
                    if (PoolHasGraylistedPages && DownloadInfo.SaveGraylistedFiles && !DownloadInfo.MergeGraylistedPages && !Directory.Exists(DownloadInfo.DownloadPath + "\\graylisted")) {
                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\graylisted");
                    }
                    if (PoolHasBlacklistedPages && DownloadInfo.SaveBlacklistedFiles && !DownloadInfo.MergeBlacklistedPages && !Directory.Exists(DownloadInfo.DownloadPath + "\\blacklisted")) {
                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted");
                    }

                    if (DownloadInfo.SaveInfo) {
                        File.WriteAllText(DownloadInfo.DownloadPath + "\\pool.nfo", PoolInfo.Trim(), Encoding.UTF8);
                    }
                    #endregion

                    #region Main download
                    Status = DownloadStatus.Downloading;
                    CleanPagesToDownload = CleanExplicitPagesCount + CleanQuestionablePagesCount + CleanSafePagesCount;
                    GraylistedPagesToDownload = GraylistedExplicitPagesCount + GraylistedQuestionablePagesCount + GraylistedSafePagesCount;
                    BlacklistedPagesToDownload = BlacklistedExplicitPagesCount + BlacklistedQuestionablePagesCount + BlacklistedSafePagesCount;
                    TotalPagesToDownload = CleanPagesToDownload + GraylistedPagesToDownload + BlacklistedPagesToDownload;

                    this.Invoke((Action)delegate () {
                        Log.Write($"Downloading pool {DownloadInfo.PoolId}");
                        pbDownloadStatus.Style = ProgressBarStyle.Blocks;
                        pbDownloadStatus.Text = "Starting download...";
                        pbTotalStatus.Maximum = TotalPagesToDownload;
                        pbTotalStatus.Text = $"File 1 of {TotalPagesToDownload:N0}";
                    });

                    using (DownloadClient = new()) {
                        DownloadClient.DownloadProgressChanged += (s, e) => {
                            ThrottleCount++;
                            switch (ThrottleCount % 40) {
                                case 0: {
                                    this.Invoke((Action)delegate () {
                                        pbDownloadStatus.Value = e.ProgressPercentage;
                                        pbDownloadStatus.Text = $"{DownloadHelpers.GetTransferRate(e.BytesReceived, e.TotalBytesToReceive)} ({e.ProgressPercentage}%)";
                                    });
                                } break;
                            }

                        };
                        DownloadClient.DownloadFileCompleted += (s, e) => {
                            this.Invoke((Action)delegate () {
                                pbDownloadStatus.Value = 0;
                                if (pbTotalStatus.Value < pbTotalStatus.Maximum && e.Error != null) {
                                    pbTotalStatus.Value = Math.Min(DownloadedPages, pbTotalStatus.Maximum);
                                }
                            });
                        };

                        DownloadClient.Proxy = WebRequest.GetSystemWebProxy();
                        DownloadClient.UserAgent = Program.UserAgent;
                        DownloadClient.Method = HttpMethod.GET;

                        for (int CurrentDownload = 0; CurrentDownload < PageURLs.Count; CurrentDownload++) {
                            if (!string.IsNullOrWhiteSpace(PageURLs[CurrentDownload])) {
                                this.Invoke((Action)delegate () {
                                    pbTotalStatus.Text = $"download page {(CurrentDownload + 1):N0} of {TotalPagesToDownload:N0}";
                                    sbStatus.Text = $"Downloading {PageFileNames[CurrentDownload]}";
                                });

                                ShouldRetry = true;
                                do {
                                    try {
                                        await DownloadClient.DownloadFileTaskAsync(PageURLs[CurrentDownload], PageFilePaths[CurrentDownload]);
                                        DownloadedPages++;
                                        ShouldRetry = false;
                                    }
                                    catch (ThreadAbortException) {
                                        return;
                                    }
                                    catch (Exception ex) {
                                        if (Log.ReportRetriableException(ex, PageURLs[CurrentDownload]) != DialogResult.Retry) {
                                            ShouldRetry = false;
                                        }
                                    }
                                } while (ShouldRetry && !DownloadAbort);
                            }
                            if (DownloadAbort) return;
                        }
                    }

                    if (DownloadAbort) return;

                    #endregion

                    #region Post-download
                    Status = DownloadStatus.Finished;
                    Log.Write($"Pool {DownloadInfo.PoolId} finished.");
                    #endregion

                }
                #endregion

                #region Catch block
                catch (PoolOrPostWasDeletedException) {
                    this.Invoke((Action)delegate () {
                        Log.Write($"The pool \"{DownloadInfo.PoolId}\" was deleted.");
                    });
                    Status = DownloadStatus.PostOrPoolWasDeleted;
                }
                catch (ApiReturnedNullOrEmptyException) {
                    this.Invoke((Action)delegate () {
                        Log.Write($"The pool \"{DownloadInfo.PoolId}\" API Returned null or empty.");
                    });
                    Status = DownloadStatus.ApiReturnedNullOrEmpty;
                }
                catch (NoFilesToDownloadException) {
                    this.Invoke((Action)delegate () {
                        Log.Write($"No files are available for pool \"{DownloadInfo.PoolId}\".");
                    });
                    Status = DownloadStatus.NothingToDownload;
                }
                catch (ThreadAbortException) {
                    this.Invoke((Action)delegate () {
                        Log.Write($"The pool download \"{DownloadInfo.PoolId}\" thread was aborted.");
                    });
                    if (DownloadClient != null && DownloadClient.IsBusy) {
                        DownloadClient.CancelAsync();
                    }
                    Status = DownloadStatus.Aborted;
                }
                catch (ObjectDisposedException) {
                    this.Invoke((Action)delegate () {
                        Log.Write($"Seems like the pool download \"{DownloadInfo.PoolId}\" form got disposed.");
                    });
                    Status = DownloadStatus.FormWasDisposed;
                }
                catch (WebException WebE) {
                    this.Invoke((Action)delegate () {
                        Log.Write($"A WebException occured downloading pool \"{DownloadInfo.PoolId}\".");
                    });
                    Log.ReportException(WebE, CurrentUrl);
                    Status = DownloadStatus.Errored;
                }
                catch (Exception ex) {
                    this.Invoke((Action)delegate () {
                        Log.Write($"An Exception occured downloading pool \"{DownloadInfo.PoolId}\".");
                    });
                    if (DownloadClient != null && DownloadClient.IsBusy) {
                        DownloadClient.CancelAsync();
                    }
                    Log.ReportException(ex);
                    Status = DownloadStatus.Errored;
                }
                #endregion

                #region Finally block
                finally {
                    this.Invoke((Action)delegate {
                        FinishDownload();
                    });
                }
                #endregion

            });
            Downloader.PoolsBeingDownloaded.Add(DownloadInfo.PoolId);
            DownloadThread.IsBackground = true;
            DownloadThread.Name = $"Tag thread \"{DownloadInfo.PoolId}\"";
            pbDownloadStatus.Style = ProgressBarStyle.Marquee;
            DownloadThread.Start();
            tmrTitle.Start();
        }

        protected override void FinishDownload() {
            tmrTitle.Stop();
            pbDownloadStatus.Style = ProgressBarStyle.Blocks;
            Downloader.PoolsBeingDownloaded.Remove(DownloadInfo.PoolId);

            if (!ExitBox) {
                ExitBox = true;
            }

            if (DownloadInfo.OpenAfter && !DownloadError) {
                System.Diagnostics.Process.Start(DownloadInfo.DownloadPath);
            }

            base.FinishDownload();
            if (DownloadInfo.IgnoreFinish) {
                switch (Status) {
                    case DownloadStatus.Finished:
                    case DownloadStatus.FileAlreadyExists:
                    case DownloadStatus.NothingToDownload: {
                        this.DialogResult = DialogResult.Yes;
                    } break;

                    case DownloadStatus.Errored:
                    case DownloadStatus.Forbidden:
                    case DownloadStatus.PostOrPoolWasDeleted:
                    case DownloadStatus.ApiReturnedNullOrEmpty:
                    case DownloadStatus.FileWasNullAfterBypassingBlacklist: {
                        this.DialogResult = DialogResult.No;
                    } break;

                    case DownloadStatus.Aborted: {
                        this.DialogResult = DialogResult.Abort;
                    } break;

                    default: {
                        this.DialogResult = DialogResult.Ignore;
                    } break;
                }
                this.Dispose();
            }
            else {
                switch (Status) {
                    case DownloadStatus.Waiting: {
                        pbDownloadStatus.Text = "Finished while waiting?";
                        pbTotalStatus.Text = "Weird error occurred";
                        this.Text = "Download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.Parsing: {
                        pbDownloadStatus.Text = "Finished while parsing?";
                        pbTotalStatus.Text = "Weird error occurred";
                        this.Text = "Download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.ReadyToDownload: {
                        pbDownloadStatus.Text = "Finished while ready?";
                        pbTotalStatus.Text = "Weird error occurred";
                        this.Text = "Download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.Downloading: {
                        pbDownloadStatus.Text = "Finished while downloading?";
                        pbTotalStatus.Text = "Weird error occurred";
                        this.Text = "Download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.Finished: {
                        pbDownloadStatus.Text = "Pool download complete";
                        pbTotalStatus.Text = $"{DownloadedPages:N0} / {CleanPagesToDownload + GraylistedPagesToDownload + BlacklistedPagesToDownload:N0} pages downloaded";
                        this.Text = "Pool download completed";
                        sbStatus.Text = "Finished";
                        pbDownloadStatus.Value = pbDownloadStatus.Maximum;
                        pbTotalStatus.Value = pbTotalStatus.Maximum;
                    } break;

                    case DownloadStatus.Errored: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "Error";
                        pbTotalStatus.Text = "An error occurred while downloading pool";
                        this.Text = "Pool download error";
                        sbStatus.Text = "Error";
                    } break;

                    case DownloadStatus.Aborted: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "Aborted";
                        pbTotalStatus.Text = $"The pool download was aborted{(DownloadedPages > 0 ? $", {DownloadedPages:N0} pages downloaded." : "")}";
                        this.Text = "Pool download aborted";
                        sbStatus.Text = "Aborted";
                    } break;

                    case DownloadStatus.Forbidden: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "Forbidden";
                        pbTotalStatus.Text = "The connection was forbidden";
                        this.Text = "Pool download error";
                        sbStatus.Text = "Forbidden";
                    } break;

                    case DownloadStatus.FormWasDisposed: {
                        // Why would you change the form controls when the form is disposed?
                        //pbDownloadStatus.Text = "Finished while waiting?";
                        //pbTotalStatus.Text = "An error occurred";
                    } break;

                    case DownloadStatus.FileAlreadyExists: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "All pages already exists";
                        pbTotalStatus.Text = "Nothing to download";
                        this.Text = "Pool download completed";
                        sbStatus.Text = "Pool download completed";
                    } break;

                    case DownloadStatus.NothingToDownload: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "There's nothing to download";
                        pbTotalStatus.Text = "Nothing to download";
                        this.Text = "Pool download completed";
                        sbStatus.Text = "Nothing to download";
                    } break;

                    case DownloadStatus.PostOrPoolWasDeleted: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "The pool was deleted.";
                        pbTotalStatus.Text = "An error occurred";
                        this.Text = "Pool download errored";
                        sbStatus.Text = "Post was deleted";
                    } break;

                    case DownloadStatus.ApiReturnedNullOrEmpty: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "The api returned null or empty";
                        pbTotalStatus.Text = "An error occurred";
                        this.Text = "Pool download api null/empty error";
                        sbStatus.Text = "API parsing error";
                    } break;

                    case DownloadStatus.FileWasNullAfterBypassingBlacklist: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "Blacklist bypass failed";
                        pbTotalStatus.Text = "An error occurred";
                        this.Text = "Pool page blacklist bypass error";
                        sbStatus.Text = "Blacklist bypass error";
                    } break;
                }
            }
        }

    }
}
