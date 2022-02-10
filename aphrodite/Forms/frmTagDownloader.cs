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

    public partial class frmTagDownloader : ExtendedForm {

        #region Fields
        /// <summary>
        /// The constant string of the page base used for parsing e621s' api.
        /// </summary>
        private const string EsixBase = "https://e621.net/posts.json?tags={0}&page={1}&limit={2}";
        /// <summary>
        /// The constant string of the page base used for parsing furryboorus' api.
        /// </summary>
        private const string FurryBooruBase = "https://furry.booru.org/index.php?page=dapi&s=post&q=index&tags={0}&pid={1}&limit={2}";

        /// <summary>
        /// The information about the download of this current instance.
        /// </summary>
        public readonly TagDownloadInfo DownloadInfo;

        /// <summary>
        /// Contains the count of explicit files that will be downloaded in this instance.
        /// </summary>
        private int CleanExplicitCount = 0;
        /// <summary>
        /// Contains the count of questionable files that will be downloaded in this instance.
        /// </summary>
        private int CleanQuestionableCount = 0;
        /// <summary>
        /// Contains the count of safe files that will be downloaded in this instance.
        /// </summary>
        private int CleanSafeCount = 0;
        /// <summary>
        /// Contains the count of explicit files that already exist.
        /// </summary>
        private int CleanExplicitExistCount = 0;
        /// <summary>
        /// Contains the count of questionable files that already exist.
        /// </summary>
        private int CleanQuestionableExistCount = 0;
        /// <summary>
        /// Contains the count of safe files that already exist.
        /// </summary>
        private int CleanSafeExistCount = 0;

        /// <summary>
        /// Contains the count of explicit graylisted files that will be downloaded in this instance.
        /// </summary>
        private int GraylistedExplicitCount = 0;
        /// <summary>
        /// Contains the count of questionable graylisted files that will be downloaded in this instance.
        /// </summary>
        private int GraylistedQuestionableCount = 0;
        /// <summary>
        /// Contains the count of safe graylisted files that will be downloaded in this instance.
        /// </summary>
        private int GraylistedSafeCount = 0;
        /// <summary>
        /// Contains the count of explicit graylisted files that already exist.
        /// </summary>
        private int GraylistedExplicitExistCount = 0;
        /// <summary>
        /// Contains the count of questionable graylisted files that already exist.
        /// </summary>
        private int GraylistedQuestionableExistCount = 0;
        /// <summary>
        /// Contains the count of safe graylisted files that already exist.
        /// </summary>
        private int GraylistedSafeExistCount = 0;

        /// <summary>
        /// Contains the count of explicit blacklisted files that will be downloaded in this instance.
        /// </summary>
        private int BlacklistedExplicitCount = 0;
        /// <summary>
        /// Contains the count of questionable blacklisted files that will be downloaded in this instance.
        /// </summary>
        private int BlacklistedQuestionableCount = 0;
        /// <summary>
        /// Contains the count of safe blacklisted files that will be downloaded in this instance.
        /// </summary>
        private int BlacklistedSafeCount = 0;
        /// <summary>
        /// Contains the count of explicit blacklisted files that already exist.
        /// </summary>
        private int BlacklistedExplicitExistCount = 0;
        /// <summary>
        /// Contains the count of questionable blacklisted files that already exist.
        /// </summary>
        private int BlacklistedQuestionableExistCount = 0;
        /// <summary>
        /// Contains the count of safe blacklisted files that already exist.
        /// </summary>
        private int BlacklistedSafeExistCount = 0;

        /// <summary>
        /// Contains the count of graylisted files skipped during this instance, if not downloading graylisted files.
        /// </summary>
        private int GraylistedSkippedCount = 0;
        /// <summary>
        /// Contains the count of blacklisted files skipped during this instance, if not downloading blacklisted files.
        /// </summary>
        private int BlacklistedSkippedCount = 0;
        /// <summary>
        /// Contains the count of files that have been parsed during this instance.
        /// </summary>
        private int TotalPostsParsed = 0;

        /// <summary>
        /// Contains the count of files that have been downloaded during this instance. Used for total progress tracking with pbTotalStatus.
        /// </summary>
        private int DownloadedFiles = 0;
        /// <summary>
        /// Contains the total count of files that will be downloaded. Updated before the download loops begin.
        /// </summary>
        private int CleanFilesToDownload = 0;
        /// <summary>
        /// Contains the total count of graylisted files that will be downloaded. Updated before the download loops begin.
        /// </summary>
        private int GraylistedFilesToDownload = 0;
        /// <summary>
        /// Contains the total count of blacklisted files that will be downloaded. Updated before the download loops begin.
        /// </summary>
        private int BlacklistedFilesToDownload = 0;
        /// <summary>
        /// Contains the total count of all files that will be downloaded.
        /// Increments when a file does not exist and will be downloaded.
        /// Used to enforce the image limit.
        /// </summary>
        private int TotalFilesToDownload = 0;

        /// <summary>
        /// The current API page being parsed.
        /// </summary>
        private int CurrentPage = 1;
        /// <summary>
        /// Whether the hardcoded artist filter should be used.
        /// </summary>
        private bool UseHardcodedFilter = false;
        #endregion

        public frmTagDownloader(TagDownloadInfo NewInfo) {
            InitializeComponent();

            if (Config.ValidPoint(Config.Settings.FormSettings.frmTagDownloader_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmTagDownloader_Location;
            }

            DownloadInfo = NewInfo;
        }

        private void frmTagDownloaderUpdated_FormClosing(object sender, FormClosingEventArgs e) {
            Config.Settings.FormSettings.frmTagDownloader_Location = this.Location;
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
            txtTags.Text = "null";
            if (DownloadInfo == null) {
                this.Text = "Unable to download tag(s)";
                pbDownloadStatus.Text = "Cannot download";
                pbTotalStatus.Text = "Download info is null";
                sbStatus.Text = "Info null";
            }
            else if (DownloadInfo.Site != DownloadSite.e621 && DownloadInfo.Site != DownloadSite.FurryBooru) {
                txtTags.Text = DownloadInfo.BaseTags;
                Status = DownloadStatus.Errored;
                this.Text = "Unable to download tags";
                pbDownloadStatus.Text = "Tags download site is not supported";
                pbTotalStatus.Text = "Cannot download tags";
                sbStatus.Text = "Unsupported tags site";
            }
            else if (DownloadInfo.Site == DownloadSite.e621 && Downloader.TagsBeingDownloaded.Contains(DownloadInfo.BaseTags)) {
                txtTags.Text = DownloadInfo.BaseTags;
                Status = DownloadStatus.AlreadyBeingDownloaded;
                this.Text = "Unable to download tags";
                pbDownloadStatus.Text = "Tags already being downloaded";
                pbTotalStatus.Text = "Cannot download tags";
                sbStatus.Text = "Already in e621 queue";
            }
            else if (DownloadInfo.Site == DownloadSite.FurryBooru && Downloader.FurryBooruTagsBeingDownloaded.Contains(DownloadInfo.BaseTags)) {
                txtTags.Text = DownloadInfo.BaseTags;
                Status = DownloadStatus.AlreadyBeingDownloaded;
                this.Text = "Unable to download tags";
                pbDownloadStatus.Text = "Tags already being downloaded";
                pbTotalStatus.Text = "Cannot download tags";
                sbStatus.Text = "Already in furrybooru queue";
            }
            else if (!DownloadInfo.SaveExplicit && !DownloadInfo.SaveQuestionable && !DownloadInfo.SaveSafe) {
                this.Text = "Unable to download tags";
                pbDownloadStatus.Text = "Cannot download";
                pbDownloadStatus.Text = "Cannot download";
                pbTotalStatus.Text = "No ratings were selected.";
            }
            else if (string.IsNullOrWhiteSpace(DownloadInfo.BaseTags)) {
                this.Text = "Unable to download tags";
                pbDownloadStatus.Text = "Cannot download";
                pbTotalStatus.Text = "No tags were specified.";
                sbStatus.Text = "No tags";
            }
            else {
                if (string.IsNullOrWhiteSpace(DownloadInfo.FileNameSchema)) {
                    DownloadInfo.FileNameSchema = "%id%_%md5%";
                }

                if (!DownloadInfo.DownloadPath.EndsWith("\\Tags")) {
                    DownloadInfo.DownloadPath += "\\Tags";
                }

                UseHardcodedFilter = DownloadInfo.UndesiredTags.Length > 0;

                lbFileStatus.Text =
                    "total files parsed: 0\n\n" +

                    "files: 0 (0 E, 0 Q, 0 S)\n" +
                    $"graylisted: {(DownloadInfo.SaveGraylistedFiles ? "0 (0 E, 0 Q, 0 S)" : "not saving, skipped 0 files")}\n" +
                    $"blacklisted: {(DownloadInfo.SaveBlacklistedFiles ? "0 (0 E, 0 Q, 0 S)" : "not saving, skipped 0 files")}\n" +
                    "total to download: 0\n\n" +

                    "existing files: 0 (0 E, 0 Q, 0 S)\n" +
                    $"existing graylisted: {(DownloadInfo.SaveBlacklistedFiles ? "0 (0 E, 0 Q, 0 S)" : "not checking existing files")}\n" +
                    $"existing blacklisted: {(DownloadInfo.SaveBlacklistedFiles ? "0 (0 E, 0 Q, 0 S)" : "not checking existing files")}\n" +
                    "total that exist: 0";

                txtTags.Text = DownloadInfo.BaseTags;
                chkExplicit.Checked = DownloadInfo.SaveExplicit;
                chkQuestionable.Checked = DownloadInfo.SaveQuestionable;
                chkSafe.Checked = DownloadInfo.SaveSafe;
                chkSeparateRatings.Checked = DownloadInfo.SeparateRatings;
                chkSeparateNonImages.Checked = DownloadInfo.SeparateNonImages;
                chkMinimumScore.Checked = DownloadInfo.UseMinimumScore;
                txtMinimumScore.Text = DownloadInfo.UseMinimumScore ? DownloadInfo.UseMinimumScore.ToString() : "-";
                txtMinimumFavCount.Text = DownloadInfo.FavoriteCount > 0 ? DownloadInfo.FavoriteCount.ToString() : "-";
                txtImageLimit.Text = DownloadInfo.ImageLimit > 0 ? DownloadInfo.ImageLimit.ToString() : "-";
                txtPageLimit.Text = DownloadInfo.PageLimit > 0 ? DownloadInfo.PageLimit.ToString() : "-";

                base.PrepareDownload();
            }
        }

        protected override void StartDownload() {
            DownloadThread = new(async () => {
                Log.Write($"Starting tag download for \"{DownloadInfo.Tags}\"");

                #region Try block
                try {

                    #region Defining new variables
                    int TagCount = DownloadInfo.Tags.Split(' ').Length; // The tag count, for tag-based filters (ratings, favcount, etc...)
                    byte[] ApiData;                                     // The data byte-array the API returns. Faster to translate.
                    string CleanInfoBuffer = string.Empty;              // The .nfo buffer for clean files.
                    string GraylistedInfoBuffer = string.Empty;         // The .nfo buffer for graylisted files.
                    string BlacklistedInfoBuffer = string.Empty;        // The .nfo buffer for blacklisted files.
                    string CurrentXml = string.Empty;                   // The current page xml.
                    bool ImageLimitReached = false;                     // Whether the image limit was reached when parsing.

                    // These are all the lists containing the information for the files.
                    // xURLs = The direct link to the file
                    // xFileNames = The file-name only of the file, for the user to see what file is being downloaded.
                    // xFilePaths = The full-path where the file will be downloaded to.
                    // xNonImages = Class-based bools whether non-image files are present, and to create the output folder for them.

                    // Separate ratings disabled || Download newest to oldest \\
                    List<string> CleanURLs = new();
                    List<string> CleanFileNames = new();
                    List<string> CleanFilePaths = new();
                    NonImageFileTypeInfo CleanFileNonImages = new();

                    List<string> GraylistedURLs = new();
                    List<string> GraylistedFileNames = new();
                    List<string> GraylistedFilePaths = new();
                    NonImageFileTypeInfo GraylistedFileNonImages = new();

                    List<string> BlacklistedURLs = new();
                    List<string> BlacklistedFileNames = new();
                    List<string> BlacklistedFilePaths = new();
                    NonImageFileTypeInfo BlacklistedFileNonImages = new();
                    //\\//\\//\\//\\//\\//\\//\\//\\

                    // Separate ratings enabled \\
                    List<string> CleanExplicitURLs = new();
                    List<string> CleanExplicitFileNames = new();
                    List<string> CleanExplicitFilePaths = new();
                    List<string> CleanQuestionableURLs = new();
                    List<string> CleanQuestionableFileNames = new();
                    List<string> CleanQuestionableFilePaths = new();
                    List<string> CleanSafeURLs = new();
                    List<string> CleanSafeFileNames = new();
                    List<string> CleanSafeFilePaths = new();
                    NonImageFileTypeInfo CleanExplicitNonImages = new();
                    NonImageFileTypeInfo CleanQuestionableNonImages = new();
                    NonImageFileTypeInfo CleanSafeNonImages = new();

                    List<string> GraylistedExplicitURLs = new();
                    List<string> GraylistedExplicitFileNames = new();
                    List<string> GraylistedExplicitFilePaths = new();
                    List<string> GraylistedQuestionableURLs = new();
                    List<string> GraylistedQuestionableFileNames = new();
                    List<string> GraylistedQuestionableFilePaths = new();
                    List<string> GraylistedSafeURLs = new();
                    List<string> GraylistedSafeFileNames = new();
                    List<string> GraylistedSafeFilePaths = new();
                    NonImageFileTypeInfo GraylistedExplicitNonImages = new();
                    NonImageFileTypeInfo GraylistedQuestionableNonImages = new();
                    NonImageFileTypeInfo GraylistedSafeNonImages = new();

                    List<string> BlacklistedExplicitURLs = new();
                    List<string> BlacklistedExplicitFileNames = new();
                    List<string> BlacklistedExplicitFilePaths = new();
                    List<string> BlacklistedQuestionableURLs = new();
                    List<string> BlacklistedQuestionableFileNames = new();
                    List<string> BlacklistedQuestionableFilePaths = new();
                    List<string> BlacklistedSafeURLs = new();
                    List<string> BlacklistedSafeFileNames = new();
                    List<string> BlacklistedSafeFilePaths = new();
                    NonImageFileTypeInfo BlacklistedExplicitNonImages = new();
                    NonImageFileTypeInfo BlacklistedQuestionableNonImages = new();
                    NonImageFileTypeInfo BlacklistedSafeNonImages = new();
                    //\\//\\//\\//\\//\\//\\//\\//\\
                    #endregion

                    #region Initializing download
                    DownloadInfo.DownloadPath += $"\\{ApiTools.ReplaceIllegalCharacters(DownloadInfo.BaseTags)}";

                    if (DownloadInfo.FavoriteCount > 0) {
                        DownloadInfo.DownloadPath += $" (favcount {DownloadInfo.FavoriteCount}+)";
                        if (DownloadInfo.FavoriteCountAsTag && TagCount < 6) {
                            DownloadInfo.Tags += $" favcount:>{DownloadInfo.FavoriteCount}";
                            TagCount++;
                        }
                    }
                    if (DownloadInfo.UseMinimumScore) {
                        DownloadInfo.DownloadPath += $" (scores {DownloadInfo.MinimumScore}+)";
                        if (DownloadInfo.MinimumScoreAsTag && TagCount < 6) {
                            DownloadInfo.Tags += $" score:>{DownloadInfo.MinimumScore}";
                            TagCount++;
                        }
                    }

                    if (DownloadInfo.SaveInfo) {
                        CleanInfoBuffer = GraylistedInfoBuffer = BlacklistedInfoBuffer =
                            $"TAGS: {DownloadInfo.Tags}\n" +
                            $"MINIMUM SCORE: {(DownloadInfo.UseMinimumScore ? DownloadInfo.MinimumScore : "n/a")}\n" +
                            $"MINIMUM FAVCOUNT: {(DownloadInfo.FavoriteCount > 0 ? DownloadInfo.FavoriteCount : "n/a")}\n" +
                            $"DOWNLOADED ON: {DateTime.Now:yyyy-MM-dd HH:mm (tt)}";

                        CleanInfoBuffer += "\n\n";
                        GraylistedInfoBuffer += $"GRAYLISTED TAGS: {string.Join(" ", DownloadInfo.Graylist)}\n\n";
                        BlacklistedInfoBuffer += $"BLACKLISTED TAGS: {string.Join(" ", DownloadInfo.Blacklist)}\n\n";
                    }
                    #endregion

                    #region Cloudflare for FurryBooru here
                    if (DownloadInfo.Site == DownloadSite.FurryBooru) {

                    }
                    #endregion

                    #region Parsing API

                    #region Define api-based vars for.
                    // e621 XML data
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

                    // FurryBooru XML Data
                    XmlNodeList xmlPosts;
                    XmlAttributeCollection xmlSearchInfo;
                    XmlAttributeCollection xmlCurrentPostAttributes;

                    // Shared vars
                    int NewFiles = 0;               // The count of new files that will be downloaded.
                    int NewFilesSkipped = 0;        // The count of files that will NOT be downloaded.
                    string rating;                  // The rating of the current file.
                    bool PostIsGraylisted;          // Whether the file is graylisted.
                    bool PostIsBlacklisted;         // Whether the file is blacklisted.
                    string FoundGraylistedTags;     // The buffer of the graylisted tags, if found on the file.
                    string FoundBlacklistedTags;    // The buffer of the blacklisted tags, if found on the file.

                    // e621 vars
                    List<string> PostTags = new();  // The list of the tags on the file.
                    string ReadTags;                // The buffer for the tags for the .nfo file.

                    // furrybooru vars
                    string NewFileName;             // The string of the new file name.
                    string NewPath;                 // The string of the new file path.
                    string ScoreAttribute;          // XmlAttribute of the post score.
                    string FileUrlAttribute;        // XmlAttribute of the post file URL.
                    string RatingAttribute;         // XmlAttribute of the post rating.
                    string TagsAttribute;           // XmlAttribute of the post tags.
                    string IdAttribute;             // XmlAttribute of the post id.
                    string Md5Attribute;            // XmlAttribute of the post MD5.
                    string ExtensionFromFileUrl;    // XmlAttribute of the post file extension.
                    #endregion

                    #region Main parser
                    Status = DownloadStatus.Parsing;
                    for (int ApiPage = 0; ApiPage < CurrentPage; ApiPage++) {
                        if (DownloadInfo.PageLimit > 0 && CurrentPage > DownloadInfo.PageLimit) {
                            Log.Write("PageLimit reached, breaking parse loop.");
                            break;
                        }

                        #region Page download + xml elements
                        switch (DownloadInfo.Site) {
                            case DownloadSite.e621: {
                                Log.Write($"Downloading api page {CurrentPage}...");

                                CurrentUrl = DownloadInfo.FromUrl ?
                                    DownloadInfo.PageUrl.Replace("e621.net/posts", "e621.net/posts.json") :
                                    string.Format(EsixBase, DownloadInfo.Tags, CurrentPage, DownloadInfo.PageDisplayedImagesCount);
                            }
                            break;

                            case DownloadSite.FurryBooru: {
                                Log.Write($"Downloading furry booru api page {CurrentPage}...");

                                CurrentUrl = DownloadInfo.FromUrl ?
                                    string.Format(FurryBooruBase, DownloadInfo.BaseTags, CurrentPage, DownloadInfo.PageDisplayedImagesCount) :
                                    string.Format(FurryBooruBase, DownloadInfo.Tags, (CurrentPage - 1), DownloadInfo.PageDisplayedImagesCount);

                            }
                            break;

                            default: return;
                        }

                        this.Invoke((Action)delegate () {
                            sbStatus.Text = $"Downloading api page {CurrentPage}...";
                        });

                        ShouldRetry = true;
                        do {
                            try {
                                using (DownloadClient = new()) {
                                    DownloadClient.Proxy = WebRequest.GetSystemWebProxy();
                                    DownloadClient.UserAgent = Program.UserAgent;
                                    DownloadClient.Method = HttpMethod.GET;

                                    //ApiData = await DownloadClient.DownloadDataTaskAsync(CurrentUrl);
                                    ApiData = DownloadClient.DownloadData(CurrentUrl);

                                    CurrentXml = DownloadInfo.Site switch {
                                        DownloadSite.e621 => ApiTools.ConvertJsonToXml(Encoding.UTF8.GetString(ApiData)),
                                        DownloadSite.FurryBooru => Encoding.UTF8.GetString(ApiData),
                                        _ => null
                                    };

                                    ApiData = new byte[0];

                                    if (ApiTools.IsXmlDead(CurrentXml)) {
                                        DownloadError = true;
                                        if (ApiPage > 0) {
                                            break;
                                        }
                                        else {
                                            Status = DownloadStatus.ApiReturnedNullOrEmpty;
                                            throw new ApiReturnedNullOrEmptyException();
                                        }
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
                                        if (ApiPage == 0) {
                                            Status = DownloadStatus.Errored;
                                            DownloadError = true;
                                        }
                                        ShouldRetry = false;
                                    }
                                }
                            }
                        } while (ShouldRetry && !DownloadAbort);

                        if (DownloadAbort) return;
                        if (DownloadError) {
                            if (CurrentPage == 1) {
                                return;
                            }
                            else {
                                break;
                            }
                        }

                        // Parse the XML file.
                        xmlDoc = new();
                        xmlDoc.LoadXml(CurrentXml);
                        CurrentXml = string.Empty;
                        #endregion

                        switch (DownloadInfo.Site) {

                            #region e621 parsing
                            case DownloadSite.e621: {
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

                                if (xmlID.Count > 0) {
                                    NewFiles = 0;           // The amount of new files added, used for the log.
                                    NewFilesSkipped = 0;    // The amount of new files skipped, used for the log.

                                    Log.Write($"Parsing api page {CurrentPage}...");
                                    this.Invoke((Action)delegate () {
                                        sbStatus.Text = $"Parsing api page {CurrentPage}...";
                                    });

                                    // Begin parsing the XML for tag information per item.
                                    for (int CurrentPost = 0; CurrentPost < xmlID.Count; CurrentPost++) {
                                        if (DownloadInfo.ImageLimit > 0 && DownloadInfo.ImageLimit == TotalFilesToDownload) {
                                            ImageLimitReached = true;
                                            Log.Write("ImageLimit reached, breaking parse loop.");
                                            break;
                                        }

                                        // Set the parse variables.
                                        rating = xmlRating[CurrentPost].InnerText;
                                        PostIsGraylisted = false;
                                        PostIsBlacklisted = false;
                                        PostTags.Clear();
                                        FoundGraylistedTags = string.Empty;
                                        FoundBlacklistedTags = string.Empty;
                                        ReadTags = string.Empty;

                                        #region skip ratings & below minimums
                                        switch (xmlRating[CurrentPost].InnerText.ToLower()) {
                                            case "e":
                                            case "explicit":
                                                switch (DownloadInfo.SaveExplicit) {
                                                    case false:
                                                        TotalPostsParsed++;
                                                        continue;
                                                }
                                                rating = "Explicit";
                                                break;

                                            case "q":
                                            case "questionable":
                                                switch (DownloadInfo.SaveQuestionable) {
                                                    case false:
                                                        TotalPostsParsed++;
                                                        continue;
                                                }
                                                rating = "Questionable";
                                                break;

                                            case "s":
                                            case "safe":
                                                switch (DownloadInfo.SaveSafe) {
                                                    case false:
                                                        TotalPostsParsed++;
                                                        continue;
                                                }
                                                rating = "Safe";
                                                break;

                                            default:
                                                rating = "Unknown rating";
                                                break;
                                        }

                                        switch (DownloadInfo.FavoriteCount > 0 && Int32.Parse(xmlFavCount[CurrentPost].InnerText) < DownloadInfo.FavoriteCount) {
                                            case true:
                                                TotalPostsParsed++;
                                                continue;
                                        }

                                        switch (DownloadInfo.UseMinimumScore && Int32.Parse(xmlScore[CurrentPost].InnerText) < DownloadInfo.MinimumScore) {
                                            case true:
                                                TotalPostsParsed++;
                                                continue;
                                        }
                                        #endregion

                                        #region Tag parsing + blacklist/graylist filtering
                                        // Create new tag list to merge all the tag groups into one.
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

                                        // Check the tags
                                        for (int j = 0; j < PostTags.Count; j++) {
                                            if (DownloadInfo.Blacklist.Length > 0) {
                                                for (int k = 0; k < DownloadInfo.Blacklist.Length; k++) {
                                                    if (PostTags[j] == DownloadInfo.Blacklist[k]) {
                                                        PostIsBlacklisted = true;
                                                        FoundBlacklistedTags += DownloadInfo.Blacklist[k] + " ";
                                                    }
                                                }
                                                FoundBlacklistedTags = FoundBlacklistedTags.Trim();
                                            }

                                            if (DownloadInfo.Graylist.Length > 0) {
                                                for (int k = 0; k < DownloadInfo.Graylist.Length; k++) {
                                                    if (PostTags[j] == DownloadInfo.Graylist[k]) {
                                                        PostIsGraylisted = true;
                                                        FoundGraylistedTags += DownloadInfo.Graylist[k] + " ";
                                                    }
                                                }
                                            }
                                            FoundGraylistedTags = FoundBlacklistedTags.Trim();
                                        }

                                        // Continue if graylisted or blacklisted
                                        if (PostIsBlacklisted && !DownloadInfo.SaveBlacklistedFiles) {
                                            BlacklistedSkippedCount++;
                                            TotalPostsParsed++;
                                            NewFilesSkipped++;
                                            continue;
                                        }
                                        else if (PostIsGraylisted && !DownloadInfo.SaveGraylistedFiles) {
                                            GraylistedSkippedCount++;
                                            TotalPostsParsed++;
                                            NewFilesSkipped++;
                                            continue;
                                        }
                                        #endregion

                                        #region file name + e621 global blacklist bypass
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

                                        // Replace schema flags with post info
                                        NewFileName =
                                            DownloadInfo.FileNameSchema
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

                                        // Check for null file url + fix for files that are auto blacklisted :)
                                        string NewUrl = xmlURL[CurrentPost].InnerText;
                                        if (string.IsNullOrEmpty(NewUrl) && xmlDeleted[CurrentPost].InnerText.ToLower() == "false") {
                                            NewUrl = ApiTools.GetBlacklistedImageUrl(xmlMD5[CurrentPost].InnerText, xmlExt[CurrentPost].InnerText);
                                        }
                                        #endregion

                                        #region description + .nfo files (so the existing ones don't get replaced)
                                        // Start adding to the nfo buffer and URL lists
                                        if (DownloadInfo.SaveInfo) {
                                            // Set the description
                                            string ImageDescription = " No description";
                                            switch (string.IsNullOrWhiteSpace(xmlDescription[CurrentPost].InnerText)) {
                                                case false:
                                                    ImageDescription = xmlDescription[CurrentPost].InnerText;
                                                    break;
                                            }

                                            if (PostIsBlacklisted) {
                                                BlacklistedInfoBuffer +=
                                                    $"POST {xmlID[CurrentPost].InnerText}:\n" +
                                                    $"    MD5: {xmlMD5[CurrentPost].InnerText}\n" +
                                                    $"    URL: https://e621.net/posts/show/{xmlID[CurrentPost].InnerText}\n" +
                                                    $"    TAGS:\n{ReadTags}\n" +
                                                    $"    OFFENDING TAGS: {FoundBlacklistedTags + (PostIsGraylisted ? " |<~Bl|Gr~>| " + FoundGraylistedTags : "")}\n" +
                                                    $"    SCORE: Up {xmlScoreUp[CurrentPost].InnerText}, Down {xmlScoreDown[CurrentPost].InnerText}, Total {xmlScore[CurrentPost].InnerText}\n" +
                                                    $"    RATING: {rating}\n" +
                                                    $"    DESCRIPTION: {ImageDescription}\n\n";

                                            }
                                            else if (PostIsGraylisted) {
                                                GraylistedInfoBuffer +=
                                                    $"POST {xmlID[CurrentPost].InnerText}:\n" +
                                                    $"    MD5: {xmlMD5[CurrentPost].InnerText}\n" +
                                                    $"    URL: https://e621.net/posts/show/{xmlID[CurrentPost].InnerText}\n" +
                                                    $"    TAGS:\n{ReadTags}\n" +
                                                    $"    OFFENDING TAGS: {FoundGraylistedTags}\n" +
                                                    $"    SCORE: Up {xmlScoreUp[CurrentPost].InnerText}, Down {xmlScoreDown[CurrentPost].InnerText}, Total {xmlScore[CurrentPost].InnerText}\n" +
                                                    $"    RATING: {rating}\n" +
                                                    $"    DESCRIPTION: {ImageDescription}\n\n";
                                            }
                                            else {
                                                CleanInfoBuffer +=
                                                    $"POST {xmlID[CurrentPost].InnerText}:\n" +
                                                    $"    MD5: {xmlMD5[CurrentPost].InnerText}\n" +
                                                    $"    URL: https://e621.net/post/show/{xmlID[CurrentPost].InnerText}\n" +
                                                    $"    TAGS:\n{ReadTags}\n" +
                                                    $"    SCORE: Up {xmlScoreUp[CurrentPost].InnerText}, Down {xmlScoreDown[CurrentPost].InnerText}, Total {xmlScore[CurrentPost].InnerText}\n" +
                                                    $"    RATING: {rating}\n" +
                                                    $"    DESCRIPITON:{ImageDescription}\n\n";
                                            }
                                        }
                                        #endregion

                                        #region Add to the lists and counts
                                        NewPath = DownloadInfo.DownloadPath;

                                        if (DownloadInfo.SeparateRatings) {
                                            NewPath += xmlRating[CurrentPost].InnerText.ToLower() switch {
                                                "e" or "explicit" => "\\explicit",
                                                "q" or "questionable" => "\\questionable",
                                                "s" or "safe" => "\\safe",
                                                _ => "\\unknown"
                                            };
                                        }

                                        if (PostIsBlacklisted) {
                                            NewPath += "\\blacklisted";
                                        }
                                        else if (PostIsGraylisted) {
                                            NewPath += "\\graylisted";
                                        }

                                        if (DownloadInfo.SeparateNonImages) {
                                            NewPath += xmlExt[CurrentPost].InnerText.ToLower() switch {
                                                "gif" => "\\gif",
                                                "apng" => "\\apng",
                                                "webm" => "\\webm",
                                                "swf" => "\\swf",
                                                _ => ""
                                            };
                                        }

                                        NewPath += "\\" + NewFileName;
                                        TotalPostsParsed++;

                                        switch (xmlRating[CurrentPost].InnerText.ToLower()) {

                                            #region Explicit
                                            case "e":
                                            case "explicit":
                                                if (File.Exists(NewPath)) {
                                                    if (PostIsBlacklisted) {
                                                        BlacklistedExplicitExistCount++;
                                                    }
                                                    else if (PostIsGraylisted) {
                                                        GraylistedExplicitExistCount++;
                                                    }
                                                    else {
                                                        CleanExplicitExistCount++;
                                                    }
                                                    continue;
                                                }
                                                else {
                                                    TotalFilesToDownload++;
                                                    if (PostIsBlacklisted) {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (xmlExt[CurrentPost].InnerText.ToLower()) {
                                                                case "gif":
                                                                    BlacklistedExplicitNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    BlacklistedExplicitNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    BlacklistedExplicitNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    BlacklistedExplicitNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }

                                                        BlacklistedExplicitCount++;
                                                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                            BlacklistedExplicitURLs.Add(NewUrl);
                                                            BlacklistedExplicitFileNames.Add(NewFileName);
                                                            BlacklistedExplicitFilePaths.Add(NewPath);
                                                        }
                                                        else {
                                                            BlacklistedURLs.Add(NewUrl);
                                                            BlacklistedFileNames.Add(NewFileName);
                                                            BlacklistedFilePaths.Add(NewPath);
                                                        }
                                                    }
                                                    else if (PostIsGraylisted) {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (xmlExt[CurrentPost].InnerText.ToLower()) {
                                                                case "gif":
                                                                    GraylistedExplicitNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    GraylistedExplicitNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    GraylistedExplicitNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    GraylistedExplicitNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }

                                                        GraylistedExplicitCount++;
                                                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                            GraylistedExplicitURLs.Add(NewUrl);
                                                            GraylistedExplicitFileNames.Add(NewFileName);
                                                            GraylistedExplicitFilePaths.Add(NewPath);
                                                        }
                                                        else {
                                                            GraylistedURLs.Add(NewUrl);
                                                            GraylistedFileNames.Add(NewFileName);
                                                            GraylistedFilePaths.Add(NewPath);

                                                        }
                                                    }
                                                    else {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (xmlExt[CurrentPost].InnerText.ToLower()) {
                                                                case "gif":
                                                                    CleanExplicitNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    CleanExplicitNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    CleanExplicitNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    CleanExplicitNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }
                                                        CleanExplicitCount++;
                                                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                            CleanExplicitURLs.Add(NewUrl);
                                                            CleanExplicitFileNames.Add(NewFileName);
                                                            CleanExplicitFilePaths.Add(NewPath);
                                                        }
                                                        else {
                                                            CleanURLs.Add(NewUrl);
                                                            CleanFileNames.Add(NewFileName);
                                                            CleanFilePaths.Add(NewPath);
                                                        }
                                                    }
                                                }
                                                break;
                                            #endregion

                                            #region questionable
                                            case "q":
                                            case "questionable":
                                                if (File.Exists(NewPath)) {
                                                    if (PostIsBlacklisted) {
                                                        BlacklistedQuestionableExistCount++;
                                                    }
                                                    else if (PostIsGraylisted) {
                                                        GraylistedQuestionableExistCount++;
                                                    }
                                                    else {
                                                        CleanQuestionableExistCount++;
                                                    }
                                                    continue;
                                                }
                                                else {
                                                    TotalFilesToDownload++;
                                                    if (PostIsBlacklisted) {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (xmlExt[CurrentPost].InnerText.ToLower()) {
                                                                case "gif":
                                                                    BlacklistedQuestionableNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    BlacklistedQuestionableNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    BlacklistedQuestionableNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    BlacklistedQuestionableNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }
                                                        BlacklistedQuestionableCount++;
                                                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                            BlacklistedQuestionableURLs.Add(NewUrl);
                                                            BlacklistedQuestionableFileNames.Add(NewFileName);
                                                            BlacklistedQuestionableFilePaths.Add(NewPath);
                                                        }
                                                        else {
                                                            BlacklistedURLs.Add(NewUrl);
                                                            BlacklistedFileNames.Add(NewFileName);
                                                            BlacklistedFilePaths.Add(NewPath);
                                                        }
                                                    }
                                                    else if (PostIsGraylisted) {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (xmlExt[CurrentPost].InnerText.ToLower()) {
                                                                case "gif":
                                                                    GraylistedQuestionableNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    GraylistedQuestionableNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    GraylistedQuestionableNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    GraylistedQuestionableNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }
                                                        GraylistedQuestionableCount++;
                                                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                            GraylistedQuestionableURLs.Add(NewUrl);
                                                            GraylistedQuestionableFileNames.Add(NewFileName);
                                                            GraylistedQuestionableFilePaths.Add(NewPath);
                                                        }
                                                        else {
                                                            GraylistedURLs.Add(NewUrl);
                                                            GraylistedFileNames.Add(NewFileName);
                                                            GraylistedFilePaths.Add(NewPath);
                                                        }
                                                    }
                                                    else {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (xmlExt[CurrentPost].InnerText.ToLower()) {
                                                                case "gif":
                                                                    CleanQuestionableNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    CleanQuestionableNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    CleanQuestionableNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    CleanQuestionableNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }
                                                        CleanQuestionableCount++;
                                                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                            CleanQuestionableURLs.Add(NewUrl);
                                                            CleanQuestionableFileNames.Add(NewFileName);
                                                            CleanQuestionableFilePaths.Add(NewPath);
                                                        }
                                                        else {
                                                            CleanURLs.Add(NewUrl);
                                                            CleanFileNames.Add(NewFileName);
                                                            CleanFilePaths.Add(NewPath);
                                                        }
                                                    }
                                                }
                                                break;
                                            #endregion

                                            #region safe
                                            case "s":
                                            case "safe":
                                                if (File.Exists(NewPath)) {
                                                    if (PostIsBlacklisted) {
                                                        BlacklistedSafeExistCount++;
                                                    }
                                                    else if (PostIsGraylisted) {
                                                        GraylistedSafeExistCount++;
                                                    }
                                                    else {
                                                        CleanSafeExistCount++;
                                                    }
                                                    continue;
                                                }
                                                else {
                                                    TotalFilesToDownload++;
                                                    if (PostIsBlacklisted) {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (xmlExt[CurrentPost].InnerText.ToLower()) {
                                                                case "gif":
                                                                    BlacklistedSafeNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    BlacklistedSafeNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    BlacklistedSafeNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    BlacklistedSafeNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }
                                                        BlacklistedSafeCount++;
                                                        if (DownloadInfo.SaveBlacklistedFiles) {
                                                            if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                                BlacklistedSafeURLs.Add(NewUrl);
                                                                BlacklistedSafeFileNames.Add(NewFileName);
                                                                BlacklistedSafeFilePaths.Add(NewPath);
                                                            }
                                                            else {
                                                                BlacklistedURLs.Add(NewUrl);
                                                                BlacklistedFileNames.Add(NewFileName);
                                                                BlacklistedFilePaths.Add(NewPath);
                                                            }
                                                        }
                                                    }
                                                    else if (PostIsGraylisted) {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (xmlExt[CurrentPost].InnerText.ToLower()) {
                                                                case "gif":
                                                                    GraylistedSafeNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    GraylistedSafeNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    GraylistedSafeNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    GraylistedSafeNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }
                                                        GraylistedSafeCount++;
                                                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                            GraylistedSafeURLs.Add(NewUrl);
                                                            GraylistedSafeFileNames.Add(NewFileName);
                                                            GraylistedSafeFilePaths.Add(NewPath);
                                                        }
                                                        else {
                                                            GraylistedURLs.Add(NewUrl);
                                                            GraylistedFileNames.Add(NewFileName);
                                                            GraylistedFilePaths.Add(NewPath);
                                                        }
                                                    }
                                                    else {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (xmlExt[CurrentPost].InnerText.ToLower()) {
                                                                case "gif":
                                                                    CleanSafeNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    CleanSafeNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    CleanSafeNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    CleanSafeNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }
                                                        CleanSafeCount++;
                                                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                            CleanSafeURLs.Add(NewUrl);
                                                            CleanSafeFileNames.Add(NewFileName);
                                                            CleanSafeFilePaths.Add(NewPath);
                                                        }
                                                        else {
                                                            CleanURLs.Add(NewUrl);
                                                            CleanFileNames.Add(NewFileName);
                                                            CleanFilePaths.Add(NewPath);
                                                        }
                                                    }
                                                }
                                                break;
                                                #endregion

                                        }

                                        NewFiles++;
                                        #endregion

                                    }

                                    if (xmlID.Count == 320) {
                                        CurrentPage++;
                                    }
                                }
                            } break;
                            #endregion

                            #region FurryBooru parsing
                            case DownloadSite.FurryBooru: {
                                xmlPosts = xmlDoc.ChildNodes[1].ChildNodes;
                                xmlSearchInfo = xmlDoc.ChildNodes[1].Attributes;

                                if (xmlPosts.Count > 0) {
                                    NewFiles = 0;
                                    NewFilesSkipped = 0;

                                    Log.Write($"Parsing api page {CurrentPage}...");
                                    this.Invoke((Action)delegate () {
                                        sbStatus.Text = $"Parsing api page {CurrentPage}...";
                                    });

                                    for (int CurrentPost = 0; CurrentPost < xmlPosts.Count; CurrentPost++) {
                                        if (!xmlPosts[CurrentPost].Name.ToLower().StartsWith("post")) {
                                            continue;
                                        }
                                        if (DownloadInfo.ImageLimit > 0 && DownloadInfo.ImageLimit == TotalFilesToDownload) {
                                            ImageLimitReached = true;
                                            Log.Write("ImageLimit reached, breaking parse loop.");
                                            break;
                                        }

                                        #region Reset variables
                                        xmlCurrentPostAttributes = null;
                                        ScoreAttribute = string.Empty;
                                        FileUrlAttribute = string.Empty;
                                        RatingAttribute = string.Empty;
                                        TagsAttribute = string.Empty;
                                        IdAttribute = string.Empty;
                                        Md5Attribute = string.Empty;
                                        ExtensionFromFileUrl = string.Empty;
                                        rating = string.Empty;
                                        PostIsGraylisted = false;
                                        PostIsBlacklisted = false;
                                        FoundGraylistedTags = string.Empty;
                                        FoundBlacklistedTags = string.Empty;
                                        NewFileName = string.Empty;
                                        NewPath = string.Empty;
                                        #endregion

                                        #region Retrieve attributes
                                        xmlCurrentPostAttributes = xmlPosts[CurrentPost].Attributes;

                                        for (int CurrentPostAttributes = 0; CurrentPostAttributes < xmlCurrentPostAttributes.Count; CurrentPostAttributes++) {
                                            switch (xmlCurrentPostAttributes[CurrentPostAttributes].Name.ToLower()) {

                                                case "score": {
                                                    ScoreAttribute = xmlCurrentPostAttributes[CurrentPostAttributes].InnerText;
                                                } break;

                                                case "file_url": {
                                                    FileUrlAttribute = xmlCurrentPostAttributes[CurrentPostAttributes].InnerText;
                                                    ExtensionFromFileUrl = FileUrlAttribute.Split('.')[^1];
                                                } break;

                                                case "rating": {
                                                    RatingAttribute = xmlCurrentPostAttributes[CurrentPostAttributes].InnerText;
                                                } break;

                                                case "tags": {
                                                    TagsAttribute = xmlCurrentPostAttributes[CurrentPostAttributes].InnerText;
                                                } break;

                                                case "id": {
                                                    IdAttribute = xmlCurrentPostAttributes[CurrentPostAttributes].InnerText;
                                                } break;

                                                case "md5": {
                                                    Md5Attribute = xmlCurrentPostAttributes[CurrentPostAttributes].InnerText;
                                                } break;

                                            }
                                        }
                                        #endregion

                                        #region Check ratings & minimums
                                        switch (RatingAttribute.ToLower()) {
                                            case "e":
                                            case "explicit": {
                                                if (!DownloadInfo.SaveExplicit) {
                                                    TotalPostsParsed++;
                                                    continue;
                                                }
                                                rating = "Explicit";
                                            }
                                            break;

                                            case "q":
                                            case "questionable": {
                                                if (!DownloadInfo.SaveQuestionable) {
                                                    TotalPostsParsed++;
                                                    continue;
                                                }
                                                rating = "Questionable";
                                            }
                                            break;

                                            case "s":
                                            case "safe": {
                                                if (!DownloadInfo.SaveSafe) {
                                                    TotalPostsParsed++;
                                                    continue;
                                                }
                                                rating = "Safe";
                                            }
                                            break;

                                            default: {
                                                rating = $"Undefined rating {RatingAttribute}";
                                            }
                                            break;
                                        }

                                        if (DownloadInfo.UseMinimumScore && Int32.Parse(ScoreAttribute) < DownloadInfo.MinimumScore) {
                                            TotalPostsParsed++;
                                            continue;
                                        }
                                        #endregion

                                        #region Tag parsing & graylist/blacklist filtering
                                        PostTags = new(TagsAttribute.Trim().Split(' '));
                                        for (int i = 0; i < PostTags.Count; i++) {
                                            if (DownloadInfo.Blacklist.Length > 0) {
                                                for (int j = 0; j < DownloadInfo.Blacklist.Length; j++) {
                                                    if (PostTags[i] == DownloadInfo.Blacklist[j]) {
                                                        PostIsBlacklisted = true;
                                                        FoundBlacklistedTags += DownloadInfo.Blacklist[j] + " ";
                                                    }
                                                }
                                                FoundBlacklistedTags = FoundBlacklistedTags.Trim();
                                            }

                                            if (DownloadInfo.Graylist.Length > 0) {
                                                for (int j = 0; j < DownloadInfo.Graylist.Length; j++) {
                                                    if (PostTags[i] == DownloadInfo.Graylist[j]) {
                                                        PostIsGraylisted = true;
                                                        FoundGraylistedTags += DownloadInfo.Graylist[j] + " ";
                                                    }
                                                }
                                                FoundGraylistedTags = FoundBlacklistedTags.Trim();
                                            }
                                        }

                                        if (PostIsBlacklisted && !DownloadInfo.SaveBlacklistedFiles) {
                                            BlacklistedSkippedCount++;
                                            TotalPostsParsed++;
                                            NewFilesSkipped++;
                                            continue;
                                        }
                                        else if (PostIsGraylisted && !DownloadInfo.SaveGraylistedFiles) {
                                            GraylistedSkippedCount++;
                                            TotalPostsParsed++;
                                            NewFilesSkipped++;
                                            continue;
                                        }
                                        #endregion

                                        #region File name
                                        // Replace schema flags with post info
                                        NewFileName =
                                            DownloadInfo.FileNameSchema
                                                .Replace("%md5%", Md5Attribute)
                                                .Replace("%id%", IdAttribute)
                                                .Replace("%rating%", rating.ToLower())
                                                .Replace("%rating2%", RatingAttribute)
                                                .Replace("%ext%", ExtensionFromFileUrl)
                                                .Replace("%score%", ScoreAttribute) + "." + ExtensionFromFileUrl;
                                        #endregion

                                        #region nfo files
                                        if (PostIsBlacklisted) {
                                            BlacklistedInfoBuffer +=
                                                $"POST {IdAttribute}:\n" +
                                                $"    MD5: {Md5Attribute}\n" +
                                                $"    URL: {FileUrlAttribute}\n" +
                                                $"    TAGS: {TagsAttribute}\n" +
                                                $"    OFFENDING TAGS: {FoundBlacklistedTags + (PostIsGraylisted ? " |<~Bl|Gr~>| " + FoundGraylistedTags : "")}\n" +
                                                $"    SCORE: {ScoreAttribute}\n" +
                                                $"    RATING: {rating}\n\n";

                                        }
                                        else if (PostIsGraylisted) {
                                            GraylistedInfoBuffer +=
                                                $"POST {IdAttribute}:\n" +
                                                $"    MD5: {Md5Attribute}\n" +
                                                $"    URL: {FileUrlAttribute}\n" +
                                                $"    TAGS: {TagsAttribute}\n" +
                                                $"    OFFENDING TAGS: {FoundGraylistedTags}\n" +
                                                $"    SCORE: {ScoreAttribute}\n" +
                                                $"    RATING: {rating}\n\n";
                                        }
                                        else {
                                            CleanInfoBuffer +=
                                                $"POST {IdAttribute}:\n" +
                                                $"    MD5: {Md5Attribute}\n" +
                                                $"    URL: {FileUrlAttribute}\n" +
                                                $"    TAGS: {TagsAttribute}\n" +
                                                $"    SCORE: {ScoreAttribute}\n" +
                                                $"    RATING: {rating}\n\n";
                                        }
                                        #endregion

                                        #region Add to the lists and counts
                                        NewPath = DownloadInfo.DownloadPath;

                                        if (DownloadInfo.SeparateRatings) {
                                            NewPath += RatingAttribute.ToLower() switch {
                                                "e" or "explicit" => "\\explicit",
                                                "q" or "questionable" => "\\questionable",
                                                "s" or "safe" => "\\safe",
                                                _ => "\\unknown"
                                            };
                                        }

                                        if (PostIsBlacklisted) {
                                            NewPath += "\\blacklisted";
                                        }
                                        else if (PostIsGraylisted) {
                                            NewPath += "\\graylisted";
                                        }

                                        if (DownloadInfo.SeparateNonImages) {
                                            NewPath += ExtensionFromFileUrl.ToLower() switch {
                                                "gif" => "\\gif",
                                                "apng" => "\\apng",
                                                "webm" => "\\webm",
                                                "swf" => "\\swf",
                                                _ => ""
                                            };
                                        }

                                        NewPath += "\\" + NewFileName;

                                        switch (RatingAttribute.ToLower()) {

                                            #region Explicit
                                            case "e":
                                            case "explicit":
                                                if (File.Exists(NewPath)) {
                                                    if (PostIsBlacklisted) {
                                                        BlacklistedExplicitExistCount++;
                                                    }
                                                    else if (PostIsGraylisted) {
                                                        GraylistedExplicitExistCount++;
                                                    }
                                                    else {
                                                        CleanExplicitExistCount++;
                                                    }
                                                    continue;
                                                }
                                                else {
                                                    TotalFilesToDownload++;
                                                    if (PostIsBlacklisted) {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (ExtensionFromFileUrl.ToLower()) {
                                                                case "gif":
                                                                    BlacklistedExplicitNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    BlacklistedExplicitNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    BlacklistedExplicitNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    BlacklistedExplicitNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }

                                                        BlacklistedExplicitCount++;
                                                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                            BlacklistedExplicitURLs.Add(FileUrlAttribute);
                                                            BlacklistedExplicitFileNames.Add(NewFileName);
                                                            BlacklistedExplicitFilePaths.Add(NewPath);
                                                        }
                                                        else {
                                                            BlacklistedURLs.Add(FileUrlAttribute);
                                                            BlacklistedFileNames.Add(NewFileName);
                                                            BlacklistedFilePaths.Add(NewPath);
                                                        }
                                                    }
                                                    else if (PostIsGraylisted) {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (ExtensionFromFileUrl.ToLower()) {
                                                                case "gif":
                                                                    GraylistedExplicitNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    GraylistedExplicitNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    GraylistedExplicitNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    GraylistedExplicitNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }

                                                        GraylistedExplicitCount++;
                                                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                            GraylistedExplicitURLs.Add(FileUrlAttribute);
                                                            GraylistedExplicitFileNames.Add(NewFileName);
                                                            GraylistedExplicitFilePaths.Add(NewPath);
                                                        }
                                                        else {
                                                            GraylistedURLs.Add(FileUrlAttribute);
                                                            GraylistedFileNames.Add(NewFileName);
                                                            GraylistedFilePaths.Add(NewPath);

                                                        }
                                                    }
                                                    else {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (ExtensionFromFileUrl.ToLower()) {
                                                                case "gif":
                                                                    CleanExplicitNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    CleanExplicitNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    CleanExplicitNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    CleanExplicitNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }
                                                        CleanExplicitCount++;
                                                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                            CleanExplicitURLs.Add(FileUrlAttribute);
                                                            CleanExplicitFileNames.Add(NewFileName);
                                                            CleanExplicitFilePaths.Add(NewPath);
                                                        }
                                                        else {
                                                            CleanURLs.Add(FileUrlAttribute);
                                                            CleanFileNames.Add(NewFileName);
                                                            CleanFilePaths.Add(NewPath);
                                                        }
                                                    }
                                                }
                                                break;
                                            #endregion

                                            #region questionable
                                            case "q":
                                            case "questionable":
                                                if (File.Exists(NewPath)) {
                                                    if (PostIsBlacklisted) {
                                                        BlacklistedQuestionableExistCount++;
                                                    }
                                                    else if (PostIsGraylisted) {
                                                        GraylistedQuestionableExistCount++;
                                                    }
                                                    else {
                                                        CleanQuestionableExistCount++;
                                                    }
                                                    continue;
                                                }
                                                else {
                                                    TotalFilesToDownload++;
                                                    if (PostIsBlacklisted) {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (ExtensionFromFileUrl.ToLower()) {
                                                                case "gif":
                                                                    BlacklistedQuestionableNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    BlacklistedQuestionableNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    BlacklistedQuestionableNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    BlacklistedQuestionableNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }
                                                        BlacklistedQuestionableCount++;
                                                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                            BlacklistedQuestionableURLs.Add(FileUrlAttribute);
                                                            BlacklistedQuestionableFileNames.Add(NewFileName);
                                                            BlacklistedQuestionableFilePaths.Add(NewPath);
                                                        }
                                                        else {
                                                            BlacklistedURLs.Add(FileUrlAttribute);
                                                            BlacklistedFileNames.Add(NewFileName);
                                                            BlacklistedFilePaths.Add(NewPath);
                                                        }
                                                    }
                                                    else if (PostIsGraylisted) {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (ExtensionFromFileUrl.ToLower()) {
                                                                case "gif":
                                                                    GraylistedQuestionableNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    GraylistedQuestionableNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    GraylistedQuestionableNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    GraylistedQuestionableNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }
                                                        GraylistedQuestionableCount++;
                                                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                            GraylistedQuestionableURLs.Add(FileUrlAttribute);
                                                            GraylistedQuestionableFileNames.Add(NewFileName);
                                                            GraylistedQuestionableFilePaths.Add(NewPath);
                                                        }
                                                        else {
                                                            GraylistedURLs.Add(FileUrlAttribute);
                                                            GraylistedFileNames.Add(NewFileName);
                                                            GraylistedFilePaths.Add(NewPath);
                                                        }
                                                    }
                                                    else {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (ExtensionFromFileUrl.ToLower()) {
                                                                case "gif":
                                                                    CleanQuestionableNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    CleanQuestionableNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    CleanQuestionableNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    CleanQuestionableNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }
                                                        CleanQuestionableCount++;
                                                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                            CleanQuestionableURLs.Add(FileUrlAttribute);
                                                            CleanQuestionableFileNames.Add(NewFileName);
                                                            CleanQuestionableFilePaths.Add(NewPath);
                                                        }
                                                        else {
                                                            CleanURLs.Add(FileUrlAttribute);
                                                            CleanFileNames.Add(NewFileName);
                                                            CleanFilePaths.Add(NewPath);
                                                        }
                                                    }
                                                }
                                                break;
                                            #endregion

                                            #region safe
                                            case "s":
                                            case "safe":
                                                if (File.Exists(NewPath)) {
                                                    if (PostIsBlacklisted) {
                                                        BlacklistedSafeExistCount++;
                                                    }
                                                    else if (PostIsGraylisted) {
                                                        GraylistedSafeExistCount++;
                                                    }
                                                    else {
                                                        CleanSafeExistCount++;
                                                    }
                                                    continue;
                                                }
                                                else {
                                                    TotalFilesToDownload++;
                                                    if (PostIsBlacklisted) {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (ExtensionFromFileUrl.ToLower()) {
                                                                case "gif":
                                                                    BlacklistedSafeNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    BlacklistedSafeNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    BlacklistedSafeNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    BlacklistedSafeNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }
                                                        BlacklistedSafeCount++;
                                                        if (DownloadInfo.SaveBlacklistedFiles) {
                                                            if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                                BlacklistedSafeURLs.Add(FileUrlAttribute);
                                                                BlacklistedSafeFileNames.Add(NewFileName);
                                                                BlacklistedSafeFilePaths.Add(NewPath);
                                                            }
                                                            else {
                                                                BlacklistedURLs.Add(FileUrlAttribute);
                                                                BlacklistedFileNames.Add(NewFileName);
                                                                BlacklistedFilePaths.Add(NewPath);
                                                            }
                                                        }
                                                    }
                                                    else if (PostIsGraylisted) {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (ExtensionFromFileUrl.ToLower()) {
                                                                case "gif":
                                                                    GraylistedSafeNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    GraylistedSafeNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    GraylistedSafeNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    GraylistedSafeNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }
                                                        GraylistedSafeCount++;
                                                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                            GraylistedSafeURLs.Add(FileUrlAttribute);
                                                            GraylistedSafeFileNames.Add(NewFileName);
                                                            GraylistedSafeFilePaths.Add(NewPath);
                                                        }
                                                        else {
                                                            GraylistedURLs.Add(FileUrlAttribute);
                                                            GraylistedFileNames.Add(NewFileName);
                                                            GraylistedFilePaths.Add(NewPath);
                                                        }
                                                    }
                                                    else {
                                                        if (DownloadInfo.SeparateNonImages) {
                                                            switch (ExtensionFromFileUrl.ToLower()) {
                                                                case "gif":
                                                                    CleanSafeNonImages.Gif = true;
                                                                    break;
                                                                case "apng":
                                                                    CleanSafeNonImages.Apng = true;
                                                                    break;
                                                                case "webm":
                                                                    CleanSafeNonImages.Webm = true;
                                                                    break;
                                                                case "swf":
                                                                    CleanSafeNonImages.Swf = true;
                                                                    break;
                                                            }
                                                        }
                                                        CleanSafeCount++;
                                                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                            CleanSafeURLs.Add(FileUrlAttribute);
                                                            CleanSafeFileNames.Add(NewFileName);
                                                            CleanSafeFilePaths.Add(NewPath);
                                                        }
                                                        else {
                                                            CleanURLs.Add(FileUrlAttribute);
                                                            CleanFileNames.Add(NewFileName);
                                                            CleanFilePaths.Add(NewPath);
                                                        }
                                                    }
                                                }
                                                break;
                                                #endregion

                                        }

                                        NewFiles++;
                                        TotalPostsParsed++;
                                        #endregion

                                    }
                                }

                                if (xmlPosts.Count == 100) {
                                    CurrentPage++;
                                }
                            } break;
                            #endregion

                            default: return;
                        }

                        #region after page update totals
                        this.Invoke((Action)delegate () {
                            Log.Write($"\"{DownloadInfo.BaseTags}\" {NewFiles} files will be downloaded, {NewFilesSkipped} will be skipped");
                            lbFileStatus.Text =
                                $"total files parsed: {TotalPostsParsed:N0}\n\n" +

                                $"files: {CleanExplicitCount + CleanQuestionableCount + CleanSafeCount:N0} ({CleanExplicitCount:N0} E, {CleanQuestionableCount:N0} Q, {CleanSafeCount:N0} S)\n" +
                                $"graylisted: {(DownloadInfo.SaveGraylistedFiles ? $"{GraylistedExplicitCount + GraylistedQuestionableCount + GraylistedSafeCount:N0} ({GraylistedExplicitCount:N0} E, {GraylistedQuestionableCount:N0} Q, {GraylistedSafeCount:N0} S)" : $"not saving, skipped {GraylistedSkippedCount:N0} files")}\n" +
                                $"blacklisted: {(DownloadInfo.SaveBlacklistedFiles ? $"{BlacklistedExplicitCount + BlacklistedQuestionableCount + BlacklistedSafeCount:N0} ({BlacklistedExplicitCount:N0} E, {BlacklistedQuestionableCount:N0} Q, {BlacklistedSafeCount:N0} S)" : $"not saving, skipped {BlacklistedSkippedCount:N0} files")}\n" +
                                $"total to download: {CleanExplicitCount + CleanQuestionableCount + CleanSafeCount + GraylistedExplicitCount + GraylistedQuestionableCount + GraylistedSafeCount + BlacklistedExplicitCount + BlacklistedQuestionableCount + BlacklistedSafeCount:N0}\n\n" +

                                $"existing files: {CleanExplicitExistCount + CleanQuestionableExistCount + CleanSafeExistCount:N0} ({CleanExplicitExistCount:N0} E, {CleanQuestionableExistCount:N0} Q, {CleanSafeExistCount:N0} S)\n" +
                                $"existing graylisted: {(DownloadInfo.SaveBlacklistedFiles ? $"{GraylistedExplicitExistCount + GraylistedQuestionableExistCount + GraylistedSafeExistCount:N0} ({GraylistedExplicitExistCount:N0} E, {GraylistedQuestionableExistCount:N0} Q, {GraylistedSafeExistCount:N0} S)" : $"not checking existing files")}\n" +
                                $"existing blacklisted: {(DownloadInfo.SaveBlacklistedFiles ? $"{BlacklistedExplicitExistCount + BlacklistedQuestionableExistCount + BlacklistedSafeExistCount:N0} ({BlacklistedExplicitExistCount} E, {BlacklistedQuestionableExistCount:N0} Q, {BlacklistedSafeExistCount:N0} S)" : $"not checking existing files")}\n" +
                                $"total that exist: {CleanExplicitExistCount + CleanQuestionableExistCount + CleanSafeExistCount + GraylistedExplicitExistCount + GraylistedQuestionableExistCount + GraylistedSafeExistCount + BlacklistedExplicitExistCount + BlacklistedQuestionableExistCount + BlacklistedSafeExistCount:N0}";
                        });
                        #endregion

                        if (ImageLimitReached || DownloadInfo.FromUrl) {
                            break;
                        }
                    }
                    #endregion

                    #endregion

                    #region Pre-download stuff
                    if (CleanExplicitCount + CleanQuestionableCount + CleanSafeCount +
                    GraylistedExplicitCount + GraylistedQuestionableCount + GraylistedSafeCount +
                    BlacklistedExplicitCount + BlacklistedQuestionableCount + BlacklistedSafeCount == 0) {
                        throw new NoFilesToDownloadException();
                    }

                    this.Invoke((Action)delegate {
                        sbStatus.Text = "Preparing download...";
                    });

                    #region Output folder creation
                    Status = DownloadStatus.ReadyToDownload;

                    Log.Write("Creating output folders (because System.Net doesn't do it for you...)");
                    if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                        if (CleanExplicitURLs.Count > 0) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit");
                            if (DownloadInfo.SeparateNonImages) {
                                if (CleanExplicitNonImages.Gif) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\gif");
                                }
                                if (CleanExplicitNonImages.Apng) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\apng");
                                }
                                if (CleanExplicitNonImages.Webm) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\webm");
                                }
                                if (CleanExplicitNonImages.Swf) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\swf");
                                }
                            }
                        }
                        if (CleanQuestionableURLs.Count > 0) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable");
                            if (DownloadInfo.SeparateNonImages) {
                                if (CleanQuestionableNonImages.Gif) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\gif");
                                }
                                if (CleanQuestionableNonImages.Apng) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\apng");
                                }
                                if (CleanQuestionableNonImages.Webm) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\webm");
                                }
                                if (CleanQuestionableNonImages.Swf) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\swf");
                                }
                            }
                        }
                        if (CleanSafeURLs.Count > 0) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe");
                            if (DownloadInfo.SeparateNonImages) {
                                if (CleanSafeNonImages.Gif) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\gif");
                                }
                                if (CleanSafeNonImages.Apng) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\apng");
                                }
                                if (CleanSafeNonImages.Webm) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\webm");
                                }
                                if (CleanSafeNonImages.Swf) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\swf");
                                }
                            }
                        }

                        if (DownloadInfo.SaveGraylistedFiles) {
                            if (GraylistedExplicitURLs.Count > 0) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\graylisted");
                                if (DownloadInfo.SeparateNonImages) {
                                    if (GraylistedExplicitNonImages.Gif) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\graylisted\\gif");
                                    }
                                    if (GraylistedExplicitNonImages.Apng) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\graylisted\\apng");
                                    }
                                    if (GraylistedExplicitNonImages.Webm) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\graylisted\\webm");
                                    }
                                    if (GraylistedExplicitNonImages.Swf) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\graylisted\\swf");
                                    }
                                }
                            }
                            if (GraylistedQuestionableURLs.Count > 0) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\graylisted");
                                if (DownloadInfo.SeparateNonImages) {
                                    if (GraylistedQuestionableNonImages.Gif) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\graylisted\\gif");
                                    }
                                    if (GraylistedQuestionableNonImages.Apng) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\graylisted\\apng");
                                    }
                                    if (GraylistedQuestionableNonImages.Webm) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\graylisted\\webm");
                                    }
                                    if (GraylistedQuestionableNonImages.Swf) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\graylisted\\swf");
                                    }
                                }
                            }
                            if (GraylistedSafeURLs.Count > 0) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\graylisted");
                                if (DownloadInfo.SeparateNonImages) {
                                    if (GraylistedSafeNonImages.Gif) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\graylisted\\gif");
                                    }
                                    if (GraylistedSafeNonImages.Apng) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\graylisted\\apng");
                                    }
                                    if (GraylistedSafeNonImages.Webm) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\graylisted\\webm");
                                    }
                                    if (GraylistedSafeNonImages.Swf) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\graylisted\\swf");
                                    }
                                }
                            }
                        }

                        if (DownloadInfo.SaveBlacklistedFiles) {
                            if (BlacklistedExplicitURLs.Count > 0) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\blacklisted");
                                if (DownloadInfo.SeparateNonImages) {
                                    if (BlacklistedExplicitNonImages.Gif) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\blacklisted\\gif");
                                    }
                                    if (BlacklistedExplicitNonImages.Apng) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\blacklisted\\apng");
                                    }
                                    if (BlacklistedExplicitNonImages.Webm) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\blacklisted\\webm");
                                    }
                                    if (BlacklistedExplicitNonImages.Swf) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\blacklisted\\swf");
                                    }
                                }
                            }
                            if (BlacklistedQuestionableURLs.Count > 0) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\blacklisted");
                                if (DownloadInfo.SeparateNonImages) {
                                    if (BlacklistedQuestionableNonImages.Gif) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\blacklisted\\gif");
                                    }
                                    if (BlacklistedQuestionableNonImages.Apng) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\blacklisted\\apng");
                                    }
                                    if (BlacklistedQuestionableNonImages.Webm) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\blacklisted\\webm");
                                    }
                                    if (BlacklistedQuestionableNonImages.Swf) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\blacklisted\\swf");
                                    }
                                }
                            }
                            if (BlacklistedSafeURLs.Count > 0) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\blacklisted");
                                if (DownloadInfo.SeparateNonImages) {
                                    if (BlacklistedSafeNonImages.Gif) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\blacklisted\\gif");
                                    }
                                    if (BlacklistedSafeNonImages.Apng) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\blacklisted\\apng");
                                    }
                                    if (BlacklistedSafeNonImages.Webm) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\blacklisted\\webm");
                                    }
                                    if (BlacklistedSafeNonImages.Swf) {
                                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\blacklisted\\swf");
                                    }
                                }
                            }
                        }
                    }
                    else {
                        Directory.CreateDirectory(DownloadInfo.DownloadPath);
                        if (DownloadInfo.SeparateNonImages) {
                            if (CleanFileNonImages.Gif) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\gif");
                            }
                            if (CleanFileNonImages.Apng) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\apng");
                            }
                            if (CleanFileNonImages.Webm) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\webm");
                            }
                            if (CleanFileNonImages.Swf) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\swf");
                            }
                        }

                        if (DownloadInfo.SaveGraylistedFiles && GraylistedURLs.Count > 0) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\graylisted");
                            if (DownloadInfo.SeparateNonImages) {
                                if (GraylistedFileNonImages.Gif) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\graylisted\\gif");
                                }
                                if (GraylistedFileNonImages.Apng) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\graylisted\\apng");
                                }
                                if (GraylistedFileNonImages.Webm) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\graylisted\\webm");
                                }
                                if (GraylistedFileNonImages.Swf) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\graylisted\\swf");
                                }
                            }
                        }


                        if (DownloadInfo.SaveBlacklistedFiles && BlacklistedURLs.Count > 0) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted");
                            if (DownloadInfo.SeparateNonImages) {
                                if (BlacklistedFileNonImages.Gif) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted\\gif");
                                }
                                if (BlacklistedFileNonImages.Apng) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted\\apng");
                                }
                                if (BlacklistedFileNonImages.Webm) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted\\webm");
                                }
                                if (BlacklistedFileNonImages.Swf) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted\\swf");
                                }
                            }
                        }
                    }
                    #endregion

                    Status = DownloadStatus.ReadyToDownload;

                    // Save the .nfo
                    if (DownloadInfo.SaveInfo) {
                        Log.Write("Saving .nfo files");
                        if (CleanExplicitCount + CleanQuestionableCount + CleanSafeCount > 0) {
                            File.WriteAllText(
                                DownloadInfo.DownloadPath + "\\tags.nfo",
                                CleanInfoBuffer.Trim(),
                                Encoding.UTF8
                            );
                        }

                        if (DownloadInfo.SaveGraylistedFiles && GraylistedExplicitCount + GraylistedQuestionableCount + GraylistedSafeCount > 0) {
                            File.WriteAllText(
                                DownloadInfo.DownloadPath + "\\tags.graylisted.nfo",
                                GraylistedInfoBuffer.Trim(),
                                Encoding.UTF8
                            );
                        }

                        if (DownloadInfo.SaveBlacklistedFiles && BlacklistedExplicitCount + BlacklistedQuestionableCount + BlacklistedSafeCount > 0) {
                            File.WriteAllText(
                                DownloadInfo.DownloadPath + "\\tags.blacklisted.nfo",
                                BlacklistedInfoBuffer.Trim(),
                                Encoding.UTF8
                            );
                        }
                    }
                    #endregion

                    #region Main downloader
                    Status = DownloadStatus.Downloading;

                    CleanFilesToDownload =
                        CleanExplicitCount + CleanQuestionableCount + CleanSafeCount;

                    GraylistedFilesToDownload =
                        GraylistedExplicitCount + GraylistedQuestionableCount + GraylistedSafeCount;

                    BlacklistedFilesToDownload =
                        BlacklistedExplicitCount + BlacklistedQuestionableCount + BlacklistedSafeCount;

                    TotalFilesToDownload =
                        CleanFilesToDownload + GraylistedFilesToDownload + BlacklistedFilesToDownload;

                    this.Invoke((Action)delegate {
                        Log.Write($"There are {TotalFilesToDownload:N0} files to download with the tags \"{DownloadInfo.BaseTags}\"");
                        pbDownloadStatus.Style = ProgressBarStyle.Blocks;
                        pbDownloadStatus.Text = "Starting download...";
                        pbTotalStatus.Maximum = TotalFilesToDownload;
                        sbStatus.Text = "Starting download...";
                    });

                    using (DownloadClient = new()) {
                        DownloadClient.DownloadProgressChanged += (s, e) => {
                            switch (ThrottleCount % 40) {
                                case 0: {
                                    this.Invoke((Action)delegate () {
                                        pbDownloadStatus.Value = e.ProgressPercentage;
                                        pbDownloadStatus.Text = $"{DownloadHelpers.GetTransferRate(e.BytesReceived, e.TotalBytesToReceive)} ({e.ProgressPercentage}%)";
                                    });
                                    ThrottleCount = 0;
                                }
                                break;
                            }
                            ThrottleCount++;
                        };
                        DownloadClient.DownloadFileCompleted += (s, e) => {
                            this.Invoke((Action)delegate () {
                                pbDownloadStatus.Value = 0;

                                // protect from overflow
                                if (pbTotalStatus.Value < pbTotalStatus.Maximum) {
                                    pbTotalStatus.Value = Math.Min(DownloadedFiles, pbTotalStatus.Maximum);
                                }
                            });
                        };

                        DownloadClient.Proxy = WebRequest.GetSystemWebProxy();
                        DownloadClient.UserAgent = Program.UserAgent;
                        DownloadClient.Method = HttpMethod.GET;

                        #region Downloading
                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {

                            #region Clean
                            if (CleanExplicitURLs.Count > 0) {
                                Log.Write("Downloading clean explicit files");
                                for (int CurrentFile = 0; CurrentFile < CleanExplicitURLs.Count; CurrentFile++) {
                                    if (!string.IsNullOrWhiteSpace(CleanExplicitURLs[CurrentFile])) {
                                        this.Invoke((Action)delegate () {
                                            pbTotalStatus.Text = $"download explicit file {CurrentFile + 1:N0} of {CleanExplicitCount:N0}";
                                            sbStatus.Text = $"Downloading {CleanExplicitFileNames[CurrentFile]}";
                                        });
                                        ShouldRetry = true;
                                        do {
                                            try {
                                                await DownloadClient.DownloadFileTaskAsync(CleanExplicitURLs[CurrentFile], CleanExplicitFilePaths[CurrentFile]);
                                                DownloadedFiles++;
                                                ShouldRetry = false;
                                            }
                                            catch (ThreadAbortException) {
                                                return;
                                            }
                                            catch (Exception ex) {
                                                if (Log.ReportRetriableException(ex, CleanExplicitURLs[CurrentFile]) != DialogResult.Retry) {
                                                    ShouldRetry = false;
                                                }
                                            }
                                        } while (ShouldRetry && !DownloadAbort);
                                    }
                                    if (DownloadAbort) return;
                                }
                            }

                            if (DownloadAbort) return;

                            if (CleanQuestionableURLs.Count > 0) {
                                Log.Write("Downloading clean questionable files");
                                for (int CurrentFile = 0; CurrentFile < CleanQuestionableURLs.Count; CurrentFile++) {
                                    if (!string.IsNullOrWhiteSpace(CleanQuestionableURLs[CurrentFile])) {
                                        this.Invoke((Action)delegate () {
                                            pbTotalStatus.Text = $"download questionable file {CurrentFile + 1:N0} of {CleanQuestionableCount:N0}";
                                            sbStatus.Text = $"Downloading {CleanQuestionableFileNames[CurrentFile]}";
                                        });
                                        ShouldRetry = true;
                                        do {
                                            try {
                                                await DownloadClient.DownloadFileTaskAsync(CleanQuestionableURLs[CurrentFile], CleanQuestionableFilePaths[CurrentFile]);
                                                DownloadedFiles++;
                                                ShouldRetry = false;
                                            }
                                            catch (ThreadAbortException) {
                                                return;
                                            }
                                            catch (Exception ex) {
                                                if (Log.ReportRetriableException(ex, CleanQuestionableURLs[CurrentFile]) != DialogResult.Retry) {
                                                    ShouldRetry = false;
                                                }
                                            }
                                        } while (ShouldRetry && !DownloadAbort);
                                    }
                                    if (DownloadAbort) return;
                                }
                            }

                            if (DownloadAbort) return;

                            if (CleanSafeURLs.Count > 0) {
                                Log.Write("Downloading clean safe files");
                                for (int CurrentFile = 0; CurrentFile < CleanSafeURLs.Count; CurrentFile++) {
                                    if (!string.IsNullOrWhiteSpace(CleanSafeURLs[CurrentFile])) {
                                        this.Invoke((Action)delegate () {
                                            pbTotalStatus.Text = $"download safe file {CurrentFile + 1:N0} of {CleanSafeCount:N0}";
                                            sbStatus.Text = $"Downloading {CleanSafeFileNames[CurrentFile]}";
                                        });

                                        ShouldRetry = true;
                                        do {
                                            try {
                                                await DownloadClient.DownloadFileTaskAsync(CleanSafeURLs[CurrentFile], CleanSafeFilePaths[CurrentFile]);
                                                DownloadedFiles++;
                                                ShouldRetry = false;
                                            }
                                            catch (ThreadAbortException) {
                                                return;
                                            }
                                            catch (Exception ex) {
                                                if (Log.ReportRetriableException(ex, CleanSafeURLs[CurrentFile]) != DialogResult.Retry) {
                                                    ShouldRetry = false;
                                                }
                                            }
                                        } while (ShouldRetry && !DownloadAbort);
                                    }
                                    if (DownloadAbort) return;
                                }
                            }
                            #endregion

                            if (DownloadAbort) return;

                            #region Graylisted
                            if (GraylistedExplicitURLs.Count > 0) {
                                Log.Write("Downloading graylisted explicit files");
                                for (int CurrentFile = 0; CurrentFile < GraylistedExplicitURLs.Count; CurrentFile++) {
                                    if (!string.IsNullOrWhiteSpace(GraylistedExplicitURLs[CurrentFile])) {
                                        this.Invoke((Action)delegate () {
                                            pbTotalStatus.Text = $"download graylisted explicit file {CurrentFile + 1:N0} of {GraylistedExplicitCount:N0}";
                                            sbStatus.Text = $"Downloading {GraylistedExplicitFileNames[CurrentFile]}";
                                        });

                                        ShouldRetry = true;
                                        do {
                                            try {
                                                await DownloadClient.DownloadFileTaskAsync(GraylistedExplicitURLs[CurrentFile], GraylistedExplicitFilePaths[CurrentFile]);
                                                DownloadedFiles++;
                                                ShouldRetry = false;
                                            }
                                            catch (ThreadAbortException) {
                                                return;
                                            }
                                            catch (Exception ex) {
                                                if (Log.ReportRetriableException(ex, GraylistedExplicitURLs[CurrentFile]) != DialogResult.Retry) {
                                                    ShouldRetry = false;
                                                }
                                            }
                                        } while (ShouldRetry && !DownloadAbort);
                                    }
                                    if (DownloadAbort) return;
                                }
                            }

                            if (DownloadAbort) return;

                            if (GraylistedQuestionableURLs.Count > 0) {
                                Log.Write("Downloading graylisted questionable files");
                                for (int CurrentFile = 0; CurrentFile < GraylistedQuestionableURLs.Count; CurrentFile++) {
                                    if (!string.IsNullOrWhiteSpace(GraylistedQuestionableURLs[CurrentFile])) {
                                        this.Invoke((Action)delegate () {
                                            pbTotalStatus.Text = $"download graylisted questionable file {CurrentFile + 1:N0} of {GraylistedQuestionableCount:N0}";
                                            sbStatus.Text = $"Downloading {GraylistedQuestionableFileNames[CurrentFile]}";
                                        });

                                        ShouldRetry = true;
                                        do {
                                            try {
                                                await DownloadClient.DownloadFileTaskAsync(GraylistedQuestionableURLs[CurrentFile], GraylistedQuestionableFilePaths[CurrentFile]);
                                                DownloadedFiles++;
                                                ShouldRetry = false;
                                            }
                                            catch (ThreadAbortException) {
                                                return;
                                            }
                                            catch (Exception ex) {
                                                if (Log.ReportRetriableException(ex, GraylistedQuestionableURLs[CurrentFile]) != DialogResult.Retry) {
                                                    ShouldRetry = false;
                                                }
                                            }
                                        } while (ShouldRetry && !DownloadAbort);
                                    }
                                    if (DownloadAbort) return;
                                }
                            }

                            if (DownloadAbort) return;

                            if (GraylistedSafeURLs.Count > 0) {
                                Log.Write("Downloading graylisted safe files");
                                for (int CurrentFile = 0; CurrentFile < GraylistedSafeURLs.Count; CurrentFile++) {
                                    if (!string.IsNullOrWhiteSpace(GraylistedSafeURLs[CurrentFile])) {
                                        this.Invoke((Action)delegate () {
                                            pbTotalStatus.Text = $"download graylisted safe file {CurrentFile + 1:N0} of {GraylistedSafeCount:N0}";
                                            sbStatus.Text = $"Downloading {GraylistedSafeFileNames[CurrentFile]}";
                                        });

                                        ShouldRetry = true;
                                        do {
                                            try {
                                                await DownloadClient.DownloadFileTaskAsync(GraylistedSafeURLs[CurrentFile], GraylistedSafeFilePaths[CurrentFile]);
                                                DownloadedFiles++;
                                                ShouldRetry = false;
                                            }
                                            catch (ThreadAbortException) {
                                                return;
                                            }
                                            catch (Exception ex) {
                                                if (Log.ReportRetriableException(ex, GraylistedSafeURLs[CurrentFile]) != DialogResult.Retry) {
                                                    ShouldRetry = false;
                                                }
                                            }
                                        } while (ShouldRetry && !DownloadAbort);
                                    }
                                    if (DownloadAbort) return;
                                }
                            }
                            #endregion

                            if (DownloadAbort) return;

                            #region Blacklisted
                            if (BlacklistedExplicitURLs.Count > 0) {
                                Log.Write("Downloading blacklisted explicit files");
                                for (int CurrentFile = 0; CurrentFile < BlacklistedExplicitURLs.Count; CurrentFile++) {
                                    if (!string.IsNullOrWhiteSpace(BlacklistedExplicitURLs[CurrentFile])) {
                                        this.Invoke((Action)delegate () {
                                            pbTotalStatus.Text = $"download blacklisted explicit file {CurrentFile + 1:N0} of {BlacklistedExplicitCount:N0}";
                                            sbStatus.Text = $"Downloading {BlacklistedExplicitFileNames[CurrentFile]}";
                                        });

                                        ShouldRetry = true;
                                        do {
                                            try {
                                                await DownloadClient.DownloadFileTaskAsync(BlacklistedExplicitURLs[CurrentFile], BlacklistedExplicitFilePaths[CurrentFile]);
                                                DownloadedFiles++;
                                                ShouldRetry = false;
                                            }
                                            catch (ThreadAbortException) {
                                                return;
                                            }
                                            catch (Exception ex) {
                                                if (Log.ReportRetriableException(ex, BlacklistedExplicitURLs[CurrentFile]) != DialogResult.Retry) {
                                                    ShouldRetry = false;
                                                }
                                            }
                                        } while (ShouldRetry && !DownloadAbort);
                                    }
                                    if (DownloadAbort) return;
                                }
                            }

                            if (DownloadAbort) return;

                            if (BlacklistedQuestionableURLs.Count > 0) {
                                Log.Write("Downloading blacklisted questionable files");
                                for (int CurrentFile = 0; CurrentFile < BlacklistedQuestionableURLs.Count; CurrentFile++) {
                                    if (!string.IsNullOrWhiteSpace(BlacklistedQuestionableURLs[CurrentFile])) {
                                        this.Invoke((Action)delegate () {
                                            pbTotalStatus.Text = $"download blacklisted questionable file {CurrentFile + 1:N0} of {BlacklistedQuestionableCount:N0}";
                                            sbStatus.Text = $"Downloading {BlacklistedQuestionableFileNames[CurrentFile]}";
                                        });

                                        ShouldRetry = true;
                                        do {
                                            try {
                                                await DownloadClient.DownloadFileTaskAsync(BlacklistedQuestionableURLs[CurrentFile], BlacklistedQuestionableFilePaths[CurrentFile]);
                                                DownloadedFiles++;
                                                ShouldRetry = false;
                                            }
                                            catch (ThreadAbortException) {
                                                return;
                                            }
                                            catch (Exception ex) {
                                                if (Log.ReportRetriableException(ex, BlacklistedQuestionableURLs[CurrentFile]) != DialogResult.Retry) {
                                                    ShouldRetry = false;
                                                }
                                            }
                                        } while (ShouldRetry && !DownloadAbort);
                                    }
                                    if (DownloadAbort) return;
                                }
                            }

                            if (DownloadAbort) return;

                            if (BlacklistedSafeURLs.Count > 0) {
                                Log.Write("Downloading blacklisted safe files");
                                for (int CurrentFile = 0; CurrentFile < BlacklistedSafeURLs.Count; CurrentFile++) {
                                    if (!string.IsNullOrWhiteSpace(BlacklistedSafeURLs[CurrentFile])) {
                                        this.Invoke((Action)delegate () {
                                            pbTotalStatus.Text = $"download blacklisted safe file {CurrentFile + 1:N0} of {BlacklistedSafeCount:N0}";
                                            sbStatus.Text = $"Downloading {BlacklistedSafeFileNames[CurrentFile]}";
                                        });

                                        ShouldRetry = true;
                                        do {
                                            try {
                                                await DownloadClient.DownloadFileTaskAsync(BlacklistedSafeURLs[CurrentFile], BlacklistedSafeFilePaths[CurrentFile]);
                                                DownloadedFiles++;
                                                ShouldRetry = false;
                                            }
                                            catch (ThreadAbortException) {
                                                return;
                                            }
                                            catch (Exception ex) {
                                                if (Log.ReportRetriableException(ex, BlacklistedSafeURLs[CurrentFile]) != DialogResult.Retry) {
                                                    ShouldRetry = false;
                                                }
                                            }
                                        } while (ShouldRetry && !DownloadAbort);
                                    }
                                    if (DownloadAbort) return;
                                }
                            }
                            #endregion

                            if (DownloadAbort) return;

                        }
                        else {

                            #region Clean
                            if (CleanURLs.Count > 0) {
                                Log.Write("Downloading clean files");
                                for (int CurrentFile = 0; CurrentFile < CleanURLs.Count; CurrentFile++) {
                                    if (!string.IsNullOrWhiteSpace(CleanURLs[CurrentFile])) {
                                        this.Invoke((Action)delegate () {
                                            pbTotalStatus.Text = $"download clean file {CurrentFile + 1:N0} of {CleanFilesToDownload:N0}";
                                            sbStatus.Text = $"Downloading {CleanFileNames[CurrentFile]}";
                                        });

                                        ShouldRetry = true;
                                        do {
                                            try {
                                                await DownloadClient.DownloadFileTaskAsync(CleanURLs[CurrentFile], CleanFilePaths[CurrentFile]);
                                                DownloadedFiles++;
                                                ShouldRetry = false;
                                            }
                                            catch (ThreadAbortException) {
                                                return;
                                            }
                                            catch (Exception ex) {
                                                if (Log.ReportRetriableException(ex, CleanURLs[CurrentFile]) != DialogResult.Retry) {
                                                    ShouldRetry = false;
                                                }
                                            }
                                        } while (ShouldRetry && !DownloadAbort);
                                    }
                                    if (DownloadAbort) return;
                                }
                            }
                            #endregion

                            if (DownloadAbort) return;

                            #region Graylisted
                            if (GraylistedURLs.Count > 0) {
                                Log.Write("Downloading graylisted files");
                                for (int CurrentFile = 0; CurrentFile < GraylistedURLs.Count; CurrentFile++) {
                                    if (!string.IsNullOrWhiteSpace(GraylistedURLs[CurrentFile])) {
                                        this.Invoke((Action)delegate () {
                                            pbTotalStatus.Text = $"download graylisted file {CurrentFile + 1:N0} of {GraylistedFilesToDownload:N0}";
                                            sbStatus.Text = $"Downloading {GraylistedFileNames[CurrentFile]}";
                                        });

                                        ShouldRetry = true;
                                        do {
                                            try {
                                                await DownloadClient.DownloadFileTaskAsync(GraylistedURLs[CurrentFile], GraylistedFilePaths[CurrentFile]);
                                                DownloadedFiles++;
                                                ShouldRetry = false;
                                            }
                                            catch (ThreadAbortException) {
                                                return;
                                            }
                                            catch (Exception ex) {
                                                if (Log.ReportRetriableException(ex, GraylistedURLs[CurrentFile]) != DialogResult.Retry) {
                                                    ShouldRetry = false;
                                                }
                                            }
                                        } while (ShouldRetry && !DownloadAbort);
                                    }
                                    if (DownloadAbort) return;
                                }
                            }
                            #endregion

                            if (DownloadAbort) return;

                            #region Blacklisted
                            if (BlacklistedURLs.Count > 0) {
                                Log.Write("Downloading blacklisted files");
                                for (int CurrentFile = 0; CurrentFile < BlacklistedURLs.Count; CurrentFile++) {
                                    if (!string.IsNullOrWhiteSpace(BlacklistedURLs[CurrentFile])) {
                                        this.Invoke((Action)delegate () {
                                            pbTotalStatus.Text = $"download blacklisted file {CurrentFile + 1:N0} of {BlacklistedFilesToDownload:N0}";
                                            sbStatus.Text = $"Downloading {BlacklistedFileNames[CurrentFile]}";
                                        });

                                        ShouldRetry = true;
                                        do {
                                            try {
                                                await DownloadClient.DownloadFileTaskAsync(BlacklistedURLs[CurrentFile], BlacklistedFilePaths[CurrentFile]);
                                                DownloadedFiles++;
                                                ShouldRetry = false;
                                            }
                                            catch (ThreadAbortException) {
                                                return;
                                            }
                                            catch (Exception ex) {
                                                if (Log.ReportRetriableException(ex, BlacklistedURLs[CurrentFile]) != DialogResult.Retry) {
                                                    ShouldRetry = false;
                                                }
                                            }
                                        } while (ShouldRetry && !DownloadAbort);
                                    }
                                    if (DownloadAbort) return;
                                }
                            }
                            #endregion

                            if (DownloadAbort) return;

                        }
                        #endregion

                    }
                    #endregion

                    #region Post-download
                    Status = DownloadStatus.Finished;
                    Log.Write($"Tag download \"{DownloadInfo.BaseTags}\" finished with {DownloadedFiles:N0} files downloaded.");
                    #endregion

                }
                #endregion

                #region Catch block
                catch (ApiReturnedNullOrEmptyException) {
                    switch (DownloadInfo.Site) {
                        case DownloadSite.e621: {
                            Log.Write($"Tags \"{DownloadInfo.BaseTags}\" Api returned null or empty.");
                        } break;

                        case DownloadSite.FurryBooru: {
                            Log.Write($"Tags \"{DownloadInfo.BaseTags}\" on furry boorus' Api returned null or empty.");
                        } break;
                    }
                    Status = DownloadStatus.ApiReturnedNullOrEmpty;
                    DownloadError = true;
                }
                catch (NoFilesToDownloadException) {
                    switch (DownloadInfo.Site) {
                        case DownloadSite.e621: {
                            Log.Write($"No files are available for tags \"{DownloadInfo.BaseTags}\".");
                        } break;

                        case DownloadSite.FurryBooru: {
                            Log.Write($"No files are available for furry booru tags \"{DownloadInfo.BaseTags}\".");
                        } break;
                    }
                    Status = DownloadStatus.NothingToDownload;
                    DownloadError = true;
                }
                catch (ThreadAbortException) {
                    switch (DownloadInfo.Site) {
                        case DownloadSite.e621: {
                            Log.Write($"The tags download \"{DownloadInfo.BaseTags}\" thread was aborted.");
                        } break;

                        case DownloadSite.FurryBooru: {
                            Log.Write($"The furry booru tags download \"{DownloadInfo.BaseTags}\" thread was aborted.");
                        } break;
                    }
                    if (DownloadClient != null && DownloadClient.IsBusy) {
                        DownloadClient.CancelAsync();
                    }
                    Status = DownloadStatus.Aborted;
                    DownloadError = true;
                }
                catch (ObjectDisposedException) {
                    switch (DownloadInfo.Site) {
                        case DownloadSite.e621: {
                            Log.Write($"Seems like the tags download \"{DownloadInfo.BaseTags}\" form got disposed.");
                        } break;

                        case DownloadSite.FurryBooru: {
                            Log.Write($"Seems like the furry booru tags download \"{DownloadInfo.BaseTags}\" form got disposed.");
                        } break;
                    }
                    Status = DownloadStatus.FormWasDisposed;
                    DownloadError = true;
                }
                catch (WebException WebE) {
                    switch (DownloadInfo.Site) {
                        case DownloadSite.e621: {
                            Log.Write($"A WebException occured downloading tags \"{DownloadInfo.BaseTags}\".");
                        } break;

                        case DownloadSite.FurryBooru: {
                            Log.Write($"A WebException occured downloading furry booru tags \"{DownloadInfo.BaseTags}\".");
                        } break;
                    }
                    Status = DownloadStatus.Errored;
                    Log.ReportException(WebE, CurrentUrl);
                    DownloadError = true;
                }
                catch (Exception ex) {
                    switch (DownloadInfo.Site) {
                        case DownloadSite.e621: {
                            Log.Write($"An Exception occured downloading tags \"{DownloadInfo.BaseTags}\".");
                        } break;

                        case DownloadSite.FurryBooru: {
                            Log.Write($"An Exception occured downloading furry booru tags \"{DownloadInfo.BaseTags}\".");
                        } break;
                    }
                    if (DownloadClient != null && DownloadClient.IsBusy) {
                        DownloadClient.CancelAsync();
                    }
                    Status = DownloadStatus.Errored;
                    Log.ReportException(ex);
                    DownloadError = true;
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
            DownloadThread.IsBackground = true;
            pbDownloadStatus.Style = ProgressBarStyle.Marquee;
            switch (DownloadInfo.Site) {
                case DownloadSite.e621: {
                    Downloader.TagsBeingDownloaded.Add(DownloadInfo.BaseTags);
                    DownloadThread.Name = $"Tag thread \"{DownloadInfo.BaseTags}\"";
                } break;

                case DownloadSite.FurryBooru: {
                    Downloader.FurryBooruTagsBeingDownloaded.Add(DownloadInfo.Tags);
                    DownloadThread.Name = $"Furry booru tags \"{DownloadInfo.Tags}\"";
                } break;

                default: {
                    DownloadThread.Name = $"Unknown site tags \"{DownloadInfo.Tags}\"";
                } break;
            }
            DownloadThread.Start();
            tmrTitle.Start();
        }

        protected override void FinishDownload() {
            tmrTitle.Stop();
            pbDownloadStatus.Style = ProgressBarStyle.Blocks;

            switch (DownloadInfo.Site) {
                case DownloadSite.e621: {
                    Downloader.TagsBeingDownloaded.Remove(DownloadInfo.BaseTags);
                } break;

                case DownloadSite.FurryBooru: {
                    Downloader.FurryBooruTagsBeingDownloaded.Remove(DownloadInfo.BaseTags);
                } break;
            }

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
                        this.Text = "Tag(s) download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.Parsing: {
                        pbDownloadStatus.Text = "Finished while parsing?";
                        pbTotalStatus.Text = "Weird error occurred";
                        this.Text = "Tag(s) download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.ReadyToDownload: {
                        pbDownloadStatus.Text = "Finished while ready?";
                        pbTotalStatus.Text = "Weird error occurred";
                        this.Text = "Tag(s) download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.Downloading: {
                        pbDownloadStatus.Text = "Finished while downloading?";
                        pbTotalStatus.Text = "Weird error occurred";
                        this.Text = "Tag(s) download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.Finished: {
                        pbDownloadStatus.Text = "Downloading complete";
                        pbTotalStatus.Text = $"{DownloadedFiles:N0} / {CleanFilesToDownload + GraylistedFilesToDownload + BlacklistedFilesToDownload:N0} files downloaded";
                        this.Text = "Tag(s) download complete";
                        sbStatus.Text = "Finished";
                        pbDownloadStatus.Value = pbDownloadStatus.Maximum;
                        pbTotalStatus.Value = pbTotalStatus.Maximum;
                    } break;

                    case DownloadStatus.Errored: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "Error";
                        pbTotalStatus.Text = "An error occurred";
                        this.Text = "Tag(s) download error";
                        sbStatus.Text = "Error";
                    } break;

                    case DownloadStatus.Aborted: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "Aborted";
                        pbTotalStatus.Text = $"The download was aborted{(DownloadedFiles > 0 ? $", {DownloadedFiles:N0} files downloaded" : "")}";
                        this.Text = "Tag(s) download aborted";
                        sbStatus.Text = "Aborted";
                    } break;

                    case DownloadStatus.Forbidden: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "Forbidden";
                        pbTotalStatus.Text = "The connection was forbidden";
                        this.Text = "Tag(s) download error";
                        sbStatus.Text = "Forbidden";
                    } break;

                    case DownloadStatus.FormWasDisposed: {
                        // Why would you change the form controls when the form is disposed?
                        //pbDownloadStatus.Text = "Finished while waiting?";
                        //pbTotalStatus.Text = "An error occurred";
                    } break;

                    case DownloadStatus.FileAlreadyExists: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "The file already exists";
                        pbTotalStatus.Text = "Nothing to download";
                        this.Text = "Tag(s) download complete";
                        sbStatus.Text = "Already exists";
                    } break;

                    case DownloadStatus.NothingToDownload: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "There's nothing to download";
                        pbTotalStatus.Text = "Nothing to download";
                        this.Text = "Tag(s) download complete";
                        sbStatus.Text = "Nothing to download";
                    } break;

                    case DownloadStatus.PostOrPoolWasDeleted: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "The post was deleted.";
                        pbTotalStatus.Text = "An error occurred";
                        this.Text = "Post was deleted";
                        sbStatus.Text = "Post was deleted";
                    } break;

                    case DownloadStatus.ApiReturnedNullOrEmpty: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "The api returned null or empty";
                        pbTotalStatus.Text = "An error occurred";
                        this.Text = "Api null/empty error";
                        sbStatus.Text = "API parsing error";
                    } break;

                    case DownloadStatus.FileWasNullAfterBypassingBlacklist: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "Blacklist bypass failed";
                        pbTotalStatus.Text = "An error occurred";
                        this.Text = "Blacklist bypass error";
                        sbStatus.Text = "Blacklist bypass error";
                    } break;
                }
            }
        }

    }
}
