using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using murrty.classcontrols;

namespace aphrodite {

    public partial class frmInkBunnyDownloader : ExtendedForm {


        #region Fields
        private const string BaseApiSearch = "https://inkbunny.net/api_search.php?sid={0}&submissions_per_page=100";
        private const string BaseApiSearchIdsOnly = "https://inkbunny.net/api_search.php?sid={0}&submission_ids_only=yes&submissions_per_page=100";
        private const string BaseApiSubmissions = "https://inkbunny.net/api_submissions.php?sid={0}&submission_ids={1}";

        /// <summary>
        /// The information about the download of this current instance.
        /// </summary>
        private readonly InkBunnyDownloadInfo DownloadInfo;

        /// <summary>
        /// Contains the count of general files that will be downloaded in this instance.
        /// </summary>
        private int CleanGeneralCount = 0;
        /// <summary>
        /// Contains the count of mature (nudity) files that will be downloaded in this instance.
        /// </summary>
        private int CleanMatureNudityCount = 0;
        /// <summary>
        /// Contains the count of mature (violence) files that will be downloaded in this instance.
        /// </summary>
        private int CleanMatureViolenceCount = 0;
        /// <summary>
        /// Contains the count of adult (sexual themes) files that will be downloaded in this instance.
        /// </summary>
        private int CleanAdultSexualThemesCount = 0;
        /// <summary>
        /// Contains the count of adult (strong violence) files that will be downloaded in this instance.
        /// </summary>
        private int CleanAdultStrongViolenceCount = 0;
        /// <summary>
        /// Contains the count of general files that already exist.
        /// </summary>
        private int CleanGeneralExistCount = 0;
        /// <summary>
        /// Contains the count of mature (nudity) files that already exist.
        /// </summary>
        private int CleanMatureNudityExistCount = 0;
        /// <summary>
        /// Contains the count of mature (violence) files that already exist.
        /// </summary>
        private int CleanMatureViolenceExistCount = 0;
        /// <summary>
        /// Contains the count of adult (sexual themes) files that already exist.
        /// </summary>
        private int CleanAdultSexualThemesExistCount = 0;
        /// <summary>
        /// Contains the count of adult (strong violence) files that already exist.
        /// </summary>
        private int CleanAdultStrongViolenceExistCount = 0;

        /// <summary>
        /// Contains the count of graylisted general files that will be downloaded in this instance.
        /// </summary>
        private int GraylistedGeneralCount = 0;
        /// <summary>
        /// Contains the count of graylisted mature (nudity) files that will be downloaded in this instance.
        /// </summary>
        private int GraylistedMatureNudityCount = 0;
        /// <summary>
        /// Contains the count of graylisted mature (violence) files that will be downloaded in this instance.
        /// </summary>
        private int GraylistedMatureViolenceCount = 0;
        /// <summary>
        /// Contains the count of graylisted adult (sexual themes) files that will be downloaded in this instance.
        /// </summary>
        private int GraylistedAdultSexualThemesCount = 0;
        /// <summary>
        /// Contains the count of graylisted adult (strong violence) files that will be downloaded in this instance.
        /// </summary>
        private int GraylistedAdultStrongViolenceCount = 0;
        /// <summary>
        /// Contains the count of graylisted general files that already exist.
        /// </summary>
        private int GraylistedGeneralExistCount = 0;
        /// <summary>
        /// Contains the count of graylisted mature (nudity) files that already exist.
        /// </summary>
        private int GraylistedMatureNudityExistCount = 0;
        /// <summary>
        /// Contains the count of graylisted mature (violence) files that already exist.
        /// </summary>
        private int GraylistedMatureViolenceExistCount = 0;
        /// <summary>
        /// Contains the count of graylisted adult (sexual themes) files that already exist.
        /// </summary>
        private int GraylistedAdultSexualThemesExistCount = 0;
        /// <summary>
        /// Contains the count of graylisted adult (strong violence) files that already exist.
        /// </summary>
        private int GraylistedAdultStrongViolenceExistCount = 0;

        /// <summary>
        /// Contains the count of blacklisted general files that will be downloaded in this instance.
        /// </summary>
        private int BlacklistedGeneralCount = 0;
        /// <summary>
        /// Contains the count of blacklisted mature (nudity) files that will be downloaded in this instance.
        /// </summary>
        private int BlacklistedMatureNudityCount = 0;
        /// <summary>
        /// Contains the count of blacklisted mature (violence) files that will be downloaded in this instance.
        /// </summary>
        private int BlacklistedMatureViolenceCount = 0;
        /// <summary>
        /// Contains the count of blacklisted adult (sexual themes) files that will be downloaded in this instance.
        /// </summary>
        private int BlacklistedAdultSexualThemesCount = 0;
        /// <summary>
        /// Contains the count of blacklisted adult (strong violence) files that will be downloaded in this instance.
        /// </summary>
        private int BlacklistedAdultStrongViolenceCount = 0;
        /// <summary>
        /// Contains the count of blacklisted general files that already exist.
        /// </summary>
        private int BlacklistedGeneralExistCount = 0;
        /// <summary>
        /// Contains the count of blacklisted mature (nudity) files that already exist.
        /// </summary>
        private int BlacklistedMatureNudityExistCount = 0;
        /// <summary>
        /// Contains the count of blacklisted mature (violence) files that already exist.
        /// </summary>
        private int BlacklistedMatureViolenceExistCount = 0;
        /// <summary>
        /// Contains the count of blacklisted adult (sexual themes) files that already exist.
        /// </summary>
        private int BlacklistedAdultSexualThemesExistCount = 0;
        /// <summary>
        /// Contains the count of blacklisted adult (strong violence) files that already exist.
        /// </summary>
        private int BlacklistedAdultStrongViolenceExistCount = 0;

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
        /// Whether the image limit was reached. Used to break out of loops.
        /// </summary>
        private bool ImageLimitReached = false;
        #endregion

        public frmInkBunnyDownloader(InkBunnyDownloadInfo NewInfo) {
            InitializeComponent();

            if (Config.ValidPoint(Config.Settings.FormSettings.frmInkBunnyDownloader_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmInkBunnyDownloader_Location;
            }

            DownloadInfo = NewInfo;
        }

        private void frmInkBunnyDownloader_FormClosing(object sender, FormClosingEventArgs e) {
            Config.Settings.FormSettings.frmInkBunnyDownloader_Location = this.Location;
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
            txtUsersFavorites.Text = txtArtistsGallery.Text = txtKeywords.Text = "[none]";
            if (DownloadInfo == null) {
                this.Text = "Unable to download from inkbunny";
                pbDownloadStatus.Text = "Cannot download";
                pbTotalStatus.Text = "Download info is null";
                sbStatus.Text = "Info null";
            }
            else if (Downloader.InkBunnySearchesBeingDownloaded.Contains(DownloadInfo.Identifier)) {
                txtKeywords.Text = DownloadInfo.SearchKeywords;
                txtArtistsGallery.Text = DownloadInfo.SearchArtistGallery;
                txtUsersFavorites.Text = DownloadInfo.SearchUsersFavorites;
                Status = DownloadStatus.AlreadyBeingDownloaded;
                this.Text = "Unable to download from inkbunny";
                pbDownloadStatus.Text = "Search terms already being downloaded";
                pbTotalStatus.Text = "Cannot download from inkbunny";
                sbStatus.Text = "Already in queue";
            }
            else if (string.IsNullOrWhiteSpace(DownloadInfo.SessionID)) {
                this.Text = "Unable to download from inkbunny";
                pbDownloadStatus.Text = "Cannot download";
                pbTotalStatus.Text = "No session id was specified";
                sbStatus.Text = "No session id";
            }
            else if (string.IsNullOrWhiteSpace(DownloadInfo.SearchKeywords) && string.IsNullOrWhiteSpace(DownloadInfo.SearchArtistGallery) && string.IsNullOrWhiteSpace(DownloadInfo.SearchUsersFavorites)) {
                this.Text = "Unable to download from inkbunny";
                pbDownloadStatus.Text = "Cannot download";
                pbTotalStatus.Text = "No search terms were specified";
                sbStatus.Text = "No search terms";
            }
            else if (!DownloadInfo.SearchInKeywords && !DownloadInfo.SearchInTitle && !DownloadInfo.SearchInDescriptionOrStory && !DownloadInfo.SearchInMd5Hash) {
                this.Text = "Unable to download from inkbunny";
                pbDownloadStatus.Text = "Cannot download";
                pbTotalStatus.Text = "No search area was specified";
                sbStatus.Text = "No search area";
            }
            else if (!DownloadInfo.General && !DownloadInfo.MatureNudity && !DownloadInfo.MatureViolence && !DownloadInfo.AdultSexualThemes && !DownloadInfo.AdultStrongViolence) {
                this.Text = "Unable to download from inkbunny";
                pbDownloadStatus.Text = "Cannot download";
                pbTotalStatus.Text = "No submission ratings were specified";
                sbStatus.Text = "No submission ratings";
            }
            else if (!DownloadInfo.PicturePinup && !DownloadInfo.Sketch && !DownloadInfo.PictureSeries && !DownloadInfo.Comic && !DownloadInfo.Portfolio
            && !DownloadInfo.FlashAnimation && !DownloadInfo.FlashInteractive && !DownloadInfo.VideoFeatureLength && !DownloadInfo.VideoAnimation
            && !DownloadInfo.MusicSingleTrack && !DownloadInfo.MusicAlbum && !DownloadInfo.WritingDocument && !DownloadInfo.CharacterSheet
            && !DownloadInfo.Photography) {
                this.Text = "Unable to download from inkbunny";
                pbDownloadStatus.Text = "Cannot download";
                pbTotalStatus.Text = "No submission types specified";
                sbStatus.Text = "No submission types";
            }
            else {
                if (string.IsNullOrWhiteSpace(DownloadInfo.FileNameSchema)) {
                    DownloadInfo.FileNameSchema = "%id%";
                }
                if (string.IsNullOrWhiteSpace(DownloadInfo.FileNameSchemaMultiPost)) {
                    DownloadInfo.FileNameSchemaMultiPost = "%id%_%multipostindex%";
                }

                if (!DownloadInfo.DownloadPath.EndsWith("\\InkBunny")) {
                    DownloadInfo.DownloadPath += "\\InkBunny";
                }

                txtKeywords.Text = DownloadInfo.BaseSearchKeywords ?? "[none]";
                txtArtistsGallery.Text = DownloadInfo.BaseSearchArtistsGallery ?? "[none]";
                txtUsersFavorites.Text = DownloadInfo.BaseSearchUsersFavorites ?? "[none]";
                lbFileStatus.Text =
                    $"submissions parsed: 0{(DownloadInfo.SaveGraylistedFiles ? "" : $", skipped 0 graylisted" + (DownloadInfo.SaveBlacklistedFiles ? "" : $", skipped 0 blacklisted"))}\n\n" +

                    $"general: 0 ({(DownloadInfo.SaveGraylistedFiles ? $"0" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"0" : "X")} blacklisted)\n" +
                    $"mature (nudity): 0 ({(DownloadInfo.SaveGraylistedFiles ? $"0" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"0" : "X")} blacklisted)\n" +
                    $"mature (violence): 0 ({(DownloadInfo.SaveGraylistedFiles ? $"0" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"0" : "X")} blacklisted)\n" +
                    $"adult (sexual): 0 ({(DownloadInfo.SaveGraylistedFiles ? $"0" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"0" : "X")} blacklisted)\n" +
                    $"adult (violence): 0 ({(DownloadInfo.SaveGraylistedFiles ? $"0" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"0" : "X")} blacklisted)\n\n" +

                    $"general exists: 0 ({(DownloadInfo.SaveGraylistedFiles ? $"0" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"0" : "X")} blacklisted)\n" +
                    $"mature (nudity) exists: 0 ({(DownloadInfo.SaveGraylistedFiles ? $"0" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"0" : "X")} blacklisted)\n" +
                    $"mature (violence) exists: 0 ({(DownloadInfo.SaveGraylistedFiles ? $"0" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"0" : "X")} blacklisted)\n" +
                    $"adult (sexual) exists: 0 ({(DownloadInfo.SaveGraylistedFiles ? $"0" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"0" : "X")} blacklisted)\n" +
                    $"adult (violence) exists: 0 ({(DownloadInfo.SaveGraylistedFiles ? $"0" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"0" : "X")} blacklisted)";

                DownloadInfo.SkipMature = !DownloadInfo.MatureNudity && !DownloadInfo.MatureViolence;
                DownloadInfo.SkipAdult = !DownloadInfo.AdultSexualThemes && !DownloadInfo.AdultStrongViolence;

                base.PrepareDownload();
            }
        }

        protected override void StartDownload() {
            DownloadThread = new(async () => {

                #region Try block
                try {

                    #region Defining new variables
                    byte[] ApiData;                                 // The data byte-array the API returns. It's a byte array so it's faster to translate to xml.
                    string CleanInfoBuffer = string.Empty;          // The .nfo buffer for clean files.
                    string GraylistedInfoBuffer = string.Empty;     // The .nfo buffer for graylisted files.
                    string BlacklistedInfoBuffer = string.Empty;    // The .nfo buffer for blacklisted files.
                    string CurrentXml = string.Empty;               // The current page xml.
                    string SearchTerms = string.Empty;              // The search terms that will be used per page.
                    int ParsedPagesCount = 1;                       // The api pages parsed count.
                    string SubmissionIDs = string.Empty;            // The submission ids that will be parsed.

                    // These are all the lists containing the information for the files.
                    // xURLs = The direct link to the file
                    // xFileNames = The file-name only of the file, for the user to see what file is being downloaded.
                    // xFilePaths = The full-path where the file will be downloaded to.
                    // xNonImages = Class-based bools whether non-image files are present, and to create the output folder for them.

                    // The directories that will be made.
                    List<string> DirectoriesToMake = new();

                    // Separate ratings disabled || Download newest to oldest \\
                    //List<string> CleanURLs = new();
                    //List<string> CleanFileNames = new();
                    //List<string> CleanFilePaths = new();
                    //NonImageFileTypeInkBunnyInfo CleanFileNonImages = new();

                    //List<string> GraylistedURLs = new();
                    //List<string> GraylistedFileNames = new();
                    //List<string> GraylistedFilePaths = new();
                    //NonImageFileTypeInkBunnyInfo GraylistedFileNonImages = new();

                    //List<string> BlacklistedURLs = new();
                    //List<string> BlacklistedFileNames = new();
                    //List<string> BlacklistedFilePaths = new();
                    //NonImageFileTypeInkBunnyInfo BlacklistedFileNonImages = new();
                    //\\//\\//\\//\\//\\//\\//\\//\\

                    // Separate ratings enabled \\
                    List<string> CleanGeneralURLs = new();
                    List<string> CleanGeneralFileNames = new();
                    List<string> CleanGeneralFilePaths = new();
                    List<string> CleanMatureNudityURLs = new();
                    List<string> CleanMatureNudityFileNames = new();
                    List<string> CleanMatureNudityFilePaths = new();
                    List<string> CleanMatureViolenceURLs = new();
                    List<string> CleanMatureViolenceFileNames = new();
                    List<string> CleanMatureViolenceFilePaths = new();
                    List<string> CleanAdultSexualThemesURLs = new();
                    List<string> CleanAdultSexualThemesFileNames = new();
                    List<string> CleanAdultSexualThemesFilePaths = new();
                    List<string> CleanAdultStrongViolenceURLs = new();
                    List<string> CleanAdultStrongViolenceFileNames = new();
                    List<string> CleanAdultStrongViolenceFilePaths = new();
                    //NonImageFileTypeInkBunnyInfo CleanGeneralNonImages = new();
                    //NonImageFileTypeInkBunnyInfo CleanMatureNudityNonImages = new();
                    //NonImageFileTypeInkBunnyInfo CleanMatureViolenceNonImages = new();
                    //NonImageFileTypeInkBunnyInfo CleanAdultSexualThemesNonImages = new();
                    //NonImageFileTypeInkBunnyInfo CleanAdultStrongViolenceNonImages = new();

                    List<string> GraylistedGeneralURLs = new();
                    List<string> GraylistedGeneralFileNames = new();
                    List<string> GraylistedGeneralFilePaths = new();
                    List<string> GraylistedMatureNudityURLs = new();
                    List<string> GraylistedMatureNudityFileNames = new();
                    List<string> GraylistedMatureNudityFilePaths = new();
                    List<string> GraylistedMatureViolenceURLs = new();
                    List<string> GraylistedMatureViolenceFileNames = new();
                    List<string> GraylistedMatureViolenceFilePaths = new();
                    List<string> GraylistedAdultSexualThemesURLs = new();
                    List<string> GraylistedAdultSexualThemesFileNames = new();
                    List<string> GraylistedAdultSexualThemesFilePaths = new();
                    List<string> GraylistedAdultStrongViolenceURLs = new();
                    List<string> GraylistedAdultStrongViolenceFileNames = new();
                    List<string> GraylistedAdultStrongViolenceFilePaths = new();
                    //NonImageFileTypeInkBunnyInfo GraylistedGeneralNonImages = new();
                    //NonImageFileTypeInkBunnyInfo GraylistedMatureNudityNonImages = new();
                    //NonImageFileTypeInkBunnyInfo GraylistedMatureViolenceNonImages = new();
                    //NonImageFileTypeInkBunnyInfo GraylistedAdultSexualThemesNonImages = new();
                    //NonImageFileTypeInkBunnyInfo GraylistedAdultStrongViolenceNonImages = new();

                    List<string> BlacklistedGeneralURLs = new();
                    List<string> BlacklistedGeneralFileNames = new();
                    List<string> BlacklistedGeneralFilePaths = new();
                    List<string> BlacklistedMatureNudityURLs = new();
                    List<string> BlacklistedMatureNudityFileNames = new();
                    List<string> BlacklistedMatureNudityFilePaths = new();
                    List<string> BlacklistedMatureViolenceURLs = new();
                    List<string> BlacklistedMatureViolenceFileNames = new();
                    List<string> BlacklistedMatureViolenceFilePaths = new();
                    List<string> BlacklistedAdultSexualThemesURLs = new();
                    List<string> BlacklistedAdultSexualThemesFileNames = new();
                    List<string> BlacklistedAdultSexualThemesFilePaths = new();
                    List<string> BlacklistedAdultStrongViolenceURLs = new();
                    List<string> BlacklistedAdultStrongViolenceFileNames = new();
                    List<string> BlacklistedAdultStrongViolenceFilePaths = new();
                    //NonImageFileTypeInkBunnyInfo BlacklistedGeneralNonImages = new();
                    //NonImageFileTypeInkBunnyInfo BlacklistedMatureNudityNonImages = new();
                    //NonImageFileTypeInkBunnyInfo BlacklistedMatureViolenceNonImages = new();
                    //NonImageFileTypeInkBunnyInfo BlacklistedAdultSexualThemesNonImages = new();
                    //NonImageFileTypeInkBunnyInfo BlacklistedAdultStrongViolenceNonImages = new();
                    //\\//\\//\\//\\//\\//\\//\\//\\

                    XmlNodeList xmlCurrentPage;
                    XmlNodeList xmlPageCount;
                    XmlNodeList xmlSubmissionID;
                    XmlNodeList xmlSubmissionRating;
                    XmlNodeList xmlSubmissionType;
                    XmlNodeList xmlDeleted;

                    XmlNodeList xmlContentTag;
                    XmlNodeList xmlKeywords;
                    XmlNodeList xmlFiles;
                    XmlNodeList xmlMime;
                    XmlNodeList xmlSubmissionPageCount;
                    XmlNodeList xmlContentTags;
                    XmlNodeList xmlFileNames;
                    XmlNodeList xmlFileUrls;
                    XmlNodeList xmlFileIds;
                    XmlNodeList xmlInnerKeywords;
                    #endregion

                    #region Pre-api parse
                    DownloadInfo.DownloadPath += "\\";
                    if (!string.IsNullOrWhiteSpace(DownloadInfo.SearchKeywords)) {
                        SearchTerms += "&text=" + DownloadInfo.SearchKeywords;
                        DownloadInfo.DownloadPath += "kw_" + DownloadInfo.SearchKeywords.Trim() + "+";
                    }

                    if (!string.IsNullOrWhiteSpace(DownloadInfo.SearchArtistGallery)) {
                        SearchTerms += "&username=" + DownloadInfo.SearchArtistGallery;
                        DownloadInfo.DownloadPath += "ag_" + DownloadInfo.SearchArtistGallery.Trim() + "+";
                    }

                    if (!string.IsNullOrWhiteSpace(DownloadInfo.SearchUsersFavorites)) {
                        SearchTerms += "&favs_user_id=" + DownloadInfo.SearchUsersFavorites;
                        DownloadInfo.DownloadPath += "uf_" + DownloadInfo.SearchUsersFavorites.Trim();
                    }
                    DownloadInfo.DownloadPath = DownloadInfo.DownloadPath.Trim('+', '\\');

                    // Yes, the MD5 search is the only one that is specific
                    // Otherwise, every other one is taken into account.
                    if (DownloadInfo.SearchInMd5Hash) {
                        SearchTerms += "&md5=yes";
                    }
                    else {
                        if (DownloadInfo.SearchInKeywords) {
                            SearchTerms += "&keywords=yes";
                        }
                        else {
                            SearchTerms += "&keywords=no";
                        }
                        if (DownloadInfo.SearchInTitle) {
                            SearchTerms += "&title=yes";
                        }
                        else {
                            SearchTerms += "&title=no";
                        }
                        if (DownloadInfo.SearchInDescriptionOrStory) {
                            SearchTerms += "&description=yes";
                        }
                        else {
                            SearchTerms += "&description=no";
                        }
                    }
                    #endregion

                    #region Download api & parse
                    // Decrypt now so we don't have to keep decrypting per download.
                    // Will clear after parsing.
                    DownloadInfo.SessionID = Cryptography.Decrypt(DownloadInfo.SessionID);
                    Status = DownloadStatus.Parsing;

                    do {

                        if (DownloadInfo.PageLimit > 0 && DownloadInfo.PageLimit == CurrentPage) {
                            Log.Write("PageLimit reached, breaking parse loop.");
                            break;
                        }

                        #region Download submission IDs

                        if (DownloadInfo.PageLimit > 0 && CurrentPage > DownloadInfo.PageLimit) {
                            Log.Write("PageLimit reached, breaking parse loop.");
                            break;
                        }
                        Log.Write($"Downloading api page {CurrentPage}...");
                        this.Invoke((Action)delegate () {
                            sbStatus.Text = $"Downloading api page {CurrentPage}...";
                        });

                        #region Download the API
                        CurrentUrl = string.Format(BaseApiSearch, DownloadInfo.SessionID) + SearchTerms + "&page=" + CurrentPage;
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
                                    ApiData = new byte[0];

                                    if (ApiTools.IsXmlDead(CurrentXml)) {
                                        DownloadError = true;
                                        if (CurrentPage > 1) {
                                            break;
                                        }
                                        else {
                                            Status = DownloadStatus.ApiReturnedNullOrEmpty;
                                            throw new ApiReturnedNullOrEmptyException();
                                        }
                                    }
                                    ShouldRetry = false;
                                }
                                Thread.Sleep(Program.SleepDelay);
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
                                        if (CurrentPage == 1) {
                                            Status = DownloadStatus.Errored;
                                            DownloadError = true;
                                        }
                                        ShouldRetry = false;
                                    }
                                }
                            }
                        } while (ShouldRetry && !DownloadAbort);

                        CurrentUrl = string.Empty;
                        if (DownloadAbort) return;
                        if (DownloadError) {
                            if (CurrentPage == 1) {
                                return;
                            }
                            else {
                                break;
                            }
                        }
                        #endregion

                        #region Load XML document
                        xmlDoc = new();
                        xmlDoc.LoadXml(CurrentXml);
                        CurrentXml = null;

                        // check the page count in the API.
                        // However, this will be disabled to keep up with the likely-hood of
                        // submissions being added that add an extra page.
                        // Could fix it by always parsing the page count, but still,
                        // it could happen after it's been downloaded. Let's keep it ambiguous.
                        //if (ParsedPageCount <= 1) {
                        //    XmlNodeList xmlPageCount = xmlDoc.DocumentElement.SelectNodes("/root/pages_count");
                        //    if (!int.TryParse(xmlPageCount[0].InnerText, out ParsedPageCount)) {
                        //        ParsedPageCount = 1;
                        //    }
                        //}
                        // Instead, let's check the page count & current page.
                        xmlCurrentPage = xmlDoc.DocumentElement.SelectNodes("/root/page");
                        xmlPageCount = xmlDoc.DocumentElement.SelectNodes("/root/pages_count");
                        if (xmlCurrentPage[0].InnerText == xmlPageCount[0].InnerText) {
                            if (int.TryParse(xmlCurrentPage[0].InnerText, out int ApiCurrentPage) && ApiCurrentPage < CurrentPage) {
                                break;
                            }
                        }

                        xmlSubmissionID = xmlDoc.DocumentElement.SelectNodes("/root/submissions/item/submission_id");
                        xmlSubmissionRating = xmlDoc.DocumentElement.SelectNodes("/root/submissions/item/rating_id");
                        xmlSubmissionType = xmlDoc.DocumentElement.SelectNodes("/root/submissions/item/submission_type_id");
                        xmlDeleted = xmlDoc.DocumentElement.SelectNodes("/root/submissions/item/deleted");
                        #endregion

                        #region Loop through the submissions
                        SubmissionIDs = string.Empty;
                        for (int CurrentSubmission = 0; CurrentSubmission < xmlSubmissionID.Count; CurrentSubmission++) {

                            #region Check if it's deleted
                            if (xmlDeleted[0].InnerText.ToLower() == "t") {
                                TotalPostsParsed++;
                                continue;
                            }
                            #endregion

                            #region Check the submission ratings in general (more indepth checking will occur later
                            switch (xmlSubmissionRating[CurrentSubmission].InnerText.ToLower()) {
                                case "0": {
                                    if (!DownloadInfo.General) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                } break;

                                case "1": {
                                    if (DownloadInfo.SkipMature) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                } break;

                                case "2": {
                                    if (DownloadInfo.SkipAdult) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                } break;
                            }
                            #endregion

                            #region Check the submission type
                            switch (xmlSubmissionType[CurrentSubmission].InnerText.ToLower()) {
                                case "1": {
                                    if (!DownloadInfo.PicturePinup) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                } break;

                                case "2": {
                                    if (!DownloadInfo.Sketch) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                } break;

                                case "3": {
                                    if (!DownloadInfo.PictureSeries) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                } break;

                                case "4": {
                                    if (!DownloadInfo.Comic) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                } break;

                                case "5": {
                                    if (!DownloadInfo.Portfolio) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                } break;

                                case "6": {
                                    if (!DownloadInfo.FlashAnimation) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                } break;

                                case "7": {
                                    if (!DownloadInfo.FlashInteractive) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                } break;

                                case "8": {
                                    if (!DownloadInfo.VideoFeatureLength) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                } break;

                                case "9": {
                                    if (!DownloadInfo.VideoAnimation) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                } break;

                                case "10": {
                                    if (!DownloadInfo.MusicSingleTrack) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                } break;

                                case "11": {
                                    if (!DownloadInfo.MusicAlbum) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                } break;

                                case "12": {
                                    if (!DownloadInfo.WritingDocument) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                } break;

                                case "13": {
                                    if (!DownloadInfo.CharacterSheet) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                } break;

                                case "14": {
                                    if (!DownloadInfo.Photography) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                } break;
                            }
                            #endregion

                            SubmissionIDs += xmlSubmissionID[CurrentSubmission].InnerText + ",";
                        }
                        SubmissionIDs = SubmissionIDs.Trim(',');
                        #endregion

                        #region Finalize current API page
                        CurrentPage++;
                        ParsedPagesCount++;

                        if (string.IsNullOrWhiteSpace(SubmissionIDs)) {
                            continue;
                        }
                        #endregion

                        #endregion

                        #region Download & parse submissions

                        #region Download API page
                        CurrentUrl = string.Format(BaseApiSubmissions, DownloadInfo.SessionID, SubmissionIDs);
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
                                    ApiData = new byte[0];

                                    if (ApiTools.IsXmlDead(CurrentXml)) {
                                        DownloadError = true;
                                        if (CurrentPage > 1) {
                                            break;
                                        }
                                        else {
                                            Status = DownloadStatus.ApiReturnedNullOrEmpty;
                                            throw new ApiReturnedNullOrEmptyException();
                                        }
                                    }
                                    ShouldRetry = false;
                                }
                                Thread.Sleep(Program.SleepDelay);
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
                                        if (CurrentPage == 1) {
                                            Status = DownloadStatus.Errored;
                                            DownloadError = true;
                                        }
                                        ShouldRetry = false;
                                    }
                                }
                            }
                        } while (ShouldRetry && !DownloadAbort);

                        CurrentUrl = string.Empty;
                        if (DownloadAbort) return;
                        if (DownloadError) {
                            if (CurrentPage == 1) {
                                return;
                            }
                            else {
                                break;
                            }
                        }

                        #endregion

                        #region Load XML document
                        xmlDoc = new();
                        xmlDoc.LoadXml(CurrentXml);
                        //xmlDoc.LoadXml(ApiTools.ConvertJsonToXml(File.ReadAllText(@"C:\Temp\ik.json")));
                        CurrentXml = null;

                        // We already parsed the submission type.
                        //XmlNodeList xmlSubmissionType = xmlDoc.DocumentElement.SelectNodes("/root/submissions/item/submission_type_id");
                        xmlContentTag = xmlDoc.DocumentElement.SelectNodes("/root/submissions/item/ratings");
                        xmlKeywords = xmlDoc.DocumentElement.SelectNodes("/root/submissions/item/keywords");
                        xmlFiles = xmlDoc.DocumentElement.SelectNodes("/root/submissions/item/files");
                        xmlMime = xmlDoc.DocumentElement.SelectNodes("/root/submissions/item/files/item/mimetype");
                        xmlSubmissionPageCount = xmlDoc.DocumentElement.SelectNodes("/root/submissions/item/pagecount");
                        xmlSubmissionID = xmlDoc.DocumentElement.SelectNodes("/root/submissions/item/submission_id");
                        xmlSubmissionRating = xmlDoc.DocumentElement.SelectNodes("/root/submissions/item/rating_id");
                        #endregion

                        #region Loop through the files
                        if (xmlFiles.Count > 0) {
                            bool SubmissionIsGraylisted;
                            bool SubmissionIsBlacklisted;
                            bool MultiPostSubmission;
                            int SubmissionRating;
                            string NewFileName;
                            string FileExtension;
                            string NewPath;
                            string[] Keywords;

                            for (int CurrentSubmission = 0; CurrentSubmission < xmlFiles.Count; CurrentSubmission++) {
                                if (ImageLimitReached) {
                                    break;
                                }

                                SubmissionIsGraylisted = false;
                                SubmissionIsBlacklisted = false;
                                MultiPostSubmission = false;
                                SubmissionRating = -1;
                                NewFileName = string.Empty;
                                FileExtension = string.Empty;
                                NewPath = DownloadInfo.DownloadPath;
                                Keywords = null;

                                #region Check the submission rating
                                if (xmlSubmissionRating[CurrentSubmission].InnerText.ToLower() != "0") {
                                    xmlContentTags = xmlContentTag[CurrentSubmission].SelectNodes("item/content_tag_id");
                                    switch (xmlContentTags[0].InnerText.ToLower()) {
                                        case "1": {
                                            if (!DownloadInfo.General) {
                                                TotalPostsParsed++;
                                                continue;
                                            }
                                            NewPath += "\\general";
                                            SubmissionRating = 1;
                                        } break;

                                        case "2": {
                                            if (!DownloadInfo.MatureNudity) {
                                                TotalPostsParsed++;
                                                continue;
                                            }
                                            NewPath += "\\mature-nudity";
                                            SubmissionRating = 2;
                                        } break;

                                        case "3": {
                                            if (!DownloadInfo.MatureViolence) {
                                                TotalPostsParsed++;
                                                continue;
                                            }
                                            NewPath += "\\mature-violence";
                                            SubmissionRating = 3;
                                        } break;

                                        case "4": {
                                            if (!DownloadInfo.AdultSexualThemes) {
                                                TotalPostsParsed++;
                                                continue;
                                            }
                                            NewPath += "\\adult-sexual";
                                            SubmissionRating = 4;
                                        } break;

                                        case "5": {
                                            if (!DownloadInfo.AdultStrongViolence) {
                                                TotalPostsParsed++;
                                                continue;
                                            }
                                            NewPath += "\\adult-violence";
                                            SubmissionRating = 5;
                                        } break;
                                    }
                                }
                                else {
                                    if (!DownloadInfo.General) {
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                    NewPath += "\\general";
                                    SubmissionRating = 1;
                                }
                                #endregion

                                #region Check the keywords
                                xmlInnerKeywords = xmlKeywords[CurrentSubmission].SelectNodes("item/keyword_name");
                                Keywords = xmlInnerKeywords
                                    .Cast<XmlNode>()
                                    .Select(x => x.InnerText)
                                    .Select(x => x.Contains(' ') ? x.Replace(" ", "_") : x)
                                    .ToArray();

                                for (int CurrentKeyword = 0; CurrentKeyword < Keywords.Length; CurrentKeyword++) {
                                    for (int CurrentBlacklistedTag = 0; CurrentBlacklistedTag < DownloadInfo.Blacklist.Length; CurrentBlacklistedTag++) {
                                        if (Keywords[CurrentKeyword].ToLower() == DownloadInfo.Blacklist[CurrentBlacklistedTag].ToLower()) {
                                            SubmissionIsBlacklisted = true;
                                            break;
                                        }
                                    }

                                    if (SubmissionIsBlacklisted) {
                                        break;
                                    }

                                    for (int CurrentGraylistedTag = 0; CurrentGraylistedTag < DownloadInfo.Graylist.Length; CurrentGraylistedTag++) {
                                        if (Keywords[CurrentKeyword].ToLower() == DownloadInfo.Graylist[CurrentGraylistedTag].ToLower()) {
                                            SubmissionIsGraylisted = true;
                                            break;
                                        }
                                    }
                                }

                                if (SubmissionIsBlacklisted) {
                                    if (DownloadInfo.SaveBlacklistedFiles) {
                                        NewPath += "\\blacklisted";
                                    }
                                    else {
                                        BlacklistedSkippedCount++;
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                }
                                else if (SubmissionIsGraylisted ) {
                                    if (DownloadInfo.SaveGraylistedFiles) {
                                        NewPath += "\\graylisted";
                                    }
                                    else {
                                        GraylistedSkippedCount++;
                                        TotalPostsParsed++;
                                        continue;
                                    }
                                }
                                #endregion

                                #region Parse the files
                                xmlFileIds = xmlFiles[CurrentSubmission].SelectNodes("item/file_id");
                                xmlFileNames = xmlFiles[CurrentSubmission].SelectNodes("item/file_name");
                                xmlFileUrls = xmlFiles[CurrentSubmission].SelectNodes("item/file_url_full");

                                if (xmlFileUrls.Count > 0) {
                                    if (xmlFileUrls.Count > 1) {
                                        MultiPostSubmission = true;
                                        if (DownloadInfo.SeparateMultiPosts) {
                                            NewPath += "\\multipost\\" + xmlSubmissionID[CurrentSubmission].InnerText;
                                        }
                                    }
                                    for (int CurrentFile = 0; CurrentFile < xmlFileUrls.Count; CurrentFile++) {
                                        if (DownloadInfo.ImageLimit > 0 && DownloadInfo.ImageLimit == TotalFilesToDownload) {
                                            ImageLimitReached = true;
                                            break;
                                        }

                                        FileExtension = Path.GetExtension(xmlFileUrls[CurrentFile].InnerText).Trim('.');

                                        NewFileName = MultiPostSubmission ? DownloadInfo.FileNameSchemaMultiPost : DownloadInfo.FileNameSchema;

                                        NewFileName =
                                            NewFileName
                                                .Replace("%id%", xmlSubmissionID[CurrentSubmission].InnerText)
                                                .Replace("%multipostindex%", MultiPostSubmission ? (CurrentFile + 1).ToString("000.##") : "")
                                                .Replace("%fileid%", xmlFileIds[CurrentFile].InnerText)
                                                .Replace("%ext%", FileExtension) + "." + FileExtension;

                                        if (DownloadInfo.SeparateNonImages) {
                                            NewPath += FileExtension switch {
                                                "jpeg" or "jpg" or "png" => "",
                                                _ => "\\" + FileExtension
                                            };
                                        }

                                        switch (SubmissionRating) {

                                            #region General
                                            case 1: {
                                                if (File.Exists(NewPath + "\\" + NewFileName)) {
                                                    if (SubmissionIsBlacklisted) {
                                                        BlacklistedGeneralExistCount++;
                                                    }
                                                    else if (SubmissionIsGraylisted) { 
                                                        GraylistedGeneralExistCount++;
                                                    }
                                                    else {
                                                        CleanGeneralExistCount++;
                                                    }
                                                    continue;
                                                }
                                                else {
                                                    TotalFilesToDownload++;
                                                    if (SubmissionIsBlacklisted) {
                                                        BlacklistedGeneralCount++;
                                                        BlacklistedGeneralFileNames.Add(NewFileName);
                                                        BlacklistedGeneralFilePaths.Add(NewPath + "\\" + NewFileName);
                                                        BlacklistedGeneralURLs.Add(xmlFileUrls[CurrentFile].InnerText);
                                                    }
                                                    else if (SubmissionIsGraylisted) {
                                                        GraylistedGeneralCount++;
                                                        GraylistedGeneralFileNames.Add(NewFileName);
                                                        GraylistedGeneralFilePaths.Add(NewPath + "\\" + NewFileName);
                                                        GraylistedGeneralURLs.Add(xmlFileUrls[CurrentFile].InnerText);
                                                    }
                                                    else {
                                                        CleanGeneralCount++;
                                                        CleanGeneralFileNames.Add(NewFileName);
                                                        CleanGeneralFilePaths.Add(NewPath + "\\" + NewFileName);
                                                        CleanGeneralURLs.Add(xmlFileUrls[CurrentFile].InnerText);
                                                    }
                                                }
                                            } break;
                                            #endregion

                                            #region Mature (Nudity)
                                            case 2: {
                                                if (File.Exists(NewPath + "\\" + NewFileName)) {
                                                    if (SubmissionIsBlacklisted) {
                                                        BlacklistedMatureNudityExistCount++;
                                                    }
                                                    else if (SubmissionIsGraylisted) {
                                                        GraylistedMatureNudityExistCount++;
                                                    }
                                                    else {
                                                        CleanMatureNudityExistCount++;
                                                    }
                                                    continue;
                                                }
                                                else {
                                                    TotalFilesToDownload++;
                                                    if (SubmissionIsBlacklisted) {
                                                        BlacklistedMatureNudityCount++;
                                                        BlacklistedMatureNudityFileNames.Add(NewFileName);
                                                        BlacklistedMatureNudityFilePaths.Add(NewPath + "\\" + NewFileName);
                                                        BlacklistedMatureNudityURLs.Add(xmlFileUrls[CurrentFile].InnerText);
                                                    }
                                                    else if (SubmissionIsGraylisted) {
                                                        GraylistedMatureNudityCount++;
                                                        GraylistedMatureNudityFileNames.Add(NewFileName);
                                                        GraylistedMatureNudityFilePaths.Add(NewPath + "\\" + NewFileName);
                                                        GraylistedMatureNudityURLs.Add(xmlFileUrls[CurrentFile].InnerText);
                                                    }
                                                    else {
                                                        CleanMatureNudityCount++;
                                                        CleanMatureNudityFileNames.Add(NewFileName);
                                                        CleanMatureNudityFilePaths.Add(NewPath + "\\" + NewFileName);
                                                        CleanMatureNudityURLs.Add(xmlFileUrls[CurrentFile].InnerText);
                                                    }
                                                }
                                            } break;
                                            #endregion

                                            #region Mature (Violence)
                                            case 3: {
                                                if (File.Exists(NewPath + "\\" + NewFileName)) {
                                                    if (SubmissionIsBlacklisted) {
                                                        BlacklistedMatureViolenceExistCount++;
                                                    }
                                                    else if (SubmissionIsGraylisted) {
                                                        GraylistedMatureViolenceExistCount++;
                                                    }
                                                    else {
                                                        CleanMatureViolenceExistCount++;
                                                    }
                                                    continue;
                                                }
                                                else {
                                                    TotalFilesToDownload++;
                                                    if (SubmissionIsBlacklisted) {
                                                        BlacklistedMatureViolenceCount++;
                                                        BlacklistedMatureViolenceFileNames.Add(NewFileName);
                                                        BlacklistedMatureViolenceFilePaths.Add(NewPath + "\\" + NewFileName);
                                                        BlacklistedMatureViolenceURLs.Add(xmlFileUrls[CurrentFile].InnerText);
                                                    }
                                                    else if (SubmissionIsGraylisted) {
                                                        GraylistedMatureViolenceCount++;
                                                        GraylistedMatureViolenceFileNames.Add(NewFileName);
                                                        GraylistedMatureViolenceFilePaths.Add(NewPath + "\\" + NewFileName);
                                                        GraylistedMatureViolenceURLs.Add(xmlFileUrls[CurrentFile].InnerText);
                                                    }
                                                    else {
                                                        CleanMatureViolenceCount++;
                                                        CleanMatureViolenceFileNames.Add(NewFileName);
                                                        CleanMatureViolenceFilePaths.Add(NewPath + "\\" + NewFileName);
                                                        CleanMatureViolenceURLs.Add(xmlFileUrls[CurrentFile].InnerText);
                                                    }
                                                }
                                            } break;
                                            #endregion

                                            #region Adult (Sexual Themes)
                                            case 4: {
                                                if (File.Exists(NewPath + "\\" + NewFileName)) {
                                                    if (SubmissionIsBlacklisted) {
                                                        BlacklistedAdultSexualThemesExistCount++;
                                                    }
                                                    else if (SubmissionIsGraylisted) {
                                                        GraylistedAdultSexualThemesExistCount++;
                                                    }
                                                    else {
                                                        CleanAdultSexualThemesExistCount++;
                                                    }
                                                    continue;
                                                }
                                                else {
                                                    TotalFilesToDownload++;
                                                    if (SubmissionIsBlacklisted) {
                                                        BlacklistedAdultSexualThemesCount++;
                                                        BlacklistedAdultSexualThemesFileNames.Add(NewFileName);
                                                        BlacklistedAdultSexualThemesFilePaths.Add(NewPath + "\\" + NewFileName);
                                                        BlacklistedAdultSexualThemesURLs.Add(xmlFileUrls[CurrentFile].InnerText);
                                                    }
                                                    else if (SubmissionIsGraylisted) {
                                                        GraylistedAdultSexualThemesCount++;
                                                        GraylistedAdultSexualThemesFileNames.Add(NewFileName);
                                                        GraylistedAdultSexualThemesFilePaths.Add(NewPath + "\\" + NewFileName);
                                                        GraylistedAdultSexualThemesURLs.Add(xmlFileUrls[CurrentFile].InnerText);
                                                    }
                                                    else {
                                                        CleanAdultSexualThemesCount++;
                                                        CleanAdultSexualThemesFileNames.Add(NewFileName);
                                                        CleanAdultSexualThemesFilePaths.Add(NewPath + "\\" + NewFileName);
                                                        CleanAdultSexualThemesURLs.Add(xmlFileUrls[CurrentFile].InnerText);
                                                    }
                                                }
                                            } break;
                                            #endregion

                                            #region Adult (Strong Violence)
                                            case 5: {
                                                if (File.Exists(NewPath + "\\" + NewFileName)) {
                                                    if (SubmissionIsBlacklisted) {
                                                        BlacklistedAdultStrongViolenceExistCount++;
                                                    }
                                                    else if (SubmissionIsGraylisted) {
                                                        GraylistedAdultStrongViolenceExistCount++;
                                                    }
                                                    else {
                                                        CleanAdultStrongViolenceExistCount++;
                                                    }
                                                    continue;
                                                }
                                                else {
                                                    TotalFilesToDownload++;
                                                    if (SubmissionIsBlacklisted) {
                                                        BlacklistedAdultStrongViolenceCount++;
                                                        BlacklistedAdultStrongViolenceFileNames.Add(NewFileName);
                                                        BlacklistedAdultStrongViolenceFilePaths.Add(NewPath + "\\" + NewFileName);
                                                        BlacklistedAdultStrongViolenceURLs.Add(xmlFileUrls[CurrentFile].InnerText);
                                                    }
                                                    else if (SubmissionIsGraylisted) {
                                                        GraylistedAdultStrongViolenceCount++;
                                                        GraylistedAdultStrongViolenceFileNames.Add(NewFileName);
                                                        GraylistedAdultStrongViolenceFilePaths.Add(NewPath + "\\" + NewFileName);
                                                        GraylistedAdultStrongViolenceURLs.Add(xmlFileUrls[CurrentFile].InnerText);
                                                    }
                                                    else {
                                                        CleanAdultStrongViolenceCount++;
                                                        CleanAdultStrongViolenceFileNames.Add(NewFileName);
                                                        CleanAdultStrongViolenceFilePaths.Add(NewPath + "\\" + NewFileName);
                                                        CleanAdultStrongViolenceURLs.Add(xmlFileUrls[CurrentFile].InnerText);
                                                    }
                                                }
                                            } break;
                                            #endregion

                                        }

                                        if (!DirectoriesToMake.Contains(NewPath)) {
                                            DirectoriesToMake.Add(NewPath);
                                        }

                                    }
                                }

                                TotalPostsParsed++;
                                #endregion

                            }
                        }
                        #endregion

                        #endregion

                        #region Update the totals
                        this.Invoke((Action)delegate () {
                            lbFileStatus.Text =
                                $"submissions parsed: {TotalPostsParsed:N0}{(DownloadInfo.SaveGraylistedFiles ? "" : $", skipped {GraylistedSkippedCount:N0} graylisted" + (DownloadInfo.SaveBlacklistedFiles ? "" : $", skipped {BlacklistedSkippedCount:N0} blacklisted"))}\n\n" +

                                $"general: {CleanGeneralCount:N0} ({(DownloadInfo.SaveGraylistedFiles ? $"{GraylistedGeneralCount:N0}" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"{BlacklistedGeneralCount:N0}" : "X")} blacklisted)\n" +
                                $"mature (nudity): {CleanMatureNudityCount:N0} ({(DownloadInfo.SaveGraylistedFiles ? $"{GraylistedMatureNudityCount:N0}" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"{BlacklistedMatureNudityCount:N0}" : "X")} blacklisted)\n" +
                                $"mature (violence): {CleanMatureViolenceCount:N0} ({(DownloadInfo.SaveGraylistedFiles ? $"{GraylistedMatureViolenceCount:N0}" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"{BlacklistedMatureViolenceCount:N0}" : "X")} blacklisted)\n" +
                                $"adult (sexual): {CleanAdultSexualThemesCount:N0} ({(DownloadInfo.SaveGraylistedFiles ? $"{GraylistedAdultSexualThemesCount:N0}" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"{BlacklistedAdultSexualThemesCount:N0}" : "")} blacklisted)\n" +
                                $"adult (violence): {CleanAdultStrongViolenceCount:N0} ({(DownloadInfo.SaveGraylistedFiles ? $"{GraylistedAdultStrongViolenceCount:N0}" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"{BlacklistedAdultStrongViolenceCount:N0}" : "X")} blacklisted)\n\n" +

                                $"general exists: {CleanGeneralExistCount:N0} ({(DownloadInfo.SaveGraylistedFiles ? $"{GraylistedGeneralExistCount:N0}" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"{BlacklistedGeneralExistCount:N0}" : "X")} blacklisted)\n" +
                                $"mature (nudity) exists: {CleanMatureNudityExistCount:N0} ({(DownloadInfo.SaveGraylistedFiles ? $"{GraylistedMatureNudityExistCount:N0}" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"{BlacklistedMatureNudityExistCount:N0}" : "X")} blacklisted)\n" +
                                $"mature (violence) exists: {CleanMatureViolenceExistCount:N0} ({(DownloadInfo.SaveGraylistedFiles ? $"{GraylistedMatureViolenceExistCount:N0}" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"{BlacklistedMatureViolenceExistCount:N0}" : "X")} blacklisted)\n" +
                                $"adult (sexual) exists: {CleanAdultSexualThemesExistCount:N0} ({(DownloadInfo.SaveGraylistedFiles ? $"{GraylistedAdultSexualThemesExistCount:N0}" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"{BlacklistedAdultSexualThemesExistCount:N0}" : "X")} blacklisted)\n" +
                                $"adult (violence) exists: {CleanAdultStrongViolenceExistCount:N0} ({(DownloadInfo.SaveGraylistedFiles ? $"{GraylistedAdultStrongViolenceExistCount:N0}" : $"X")} graylisted, {(DownloadInfo.SaveBlacklistedFiles ? $"{BlacklistedAdultStrongViolenceExistCount:N0}" : "X")} blacklisted)";
                        });
                        #endregion

                        if (ImageLimitReached) {
                            break;
                        }

                    } while (CurrentPage < ParsedPagesCount + 1);

                    DownloadInfo.SessionID = string.Empty;

                    #endregion

                    #region Pre-download
                    Status = DownloadStatus.ReadyToDownload;

                    this.Invoke((Action)delegate {
                        sbStatus.Text = "Preparing download...";
                    });

                    for (int NewDirectory = 0; NewDirectory < DirectoriesToMake.Count; NewDirectory++) {
                        if (!Directory.Exists(DirectoriesToMake[NewDirectory])) {
                            Directory.CreateDirectory(DirectoriesToMake[NewDirectory]);
                        }
                    }

                    CleanFilesToDownload = CleanGeneralCount + CleanMatureNudityCount + CleanMatureViolenceCount + CleanAdultSexualThemesCount + CleanAdultStrongViolenceCount;
                    GraylistedFilesToDownload = GraylistedGeneralCount + GraylistedMatureNudityCount + GraylistedMatureViolenceCount + GraylistedAdultSexualThemesCount + GraylistedAdultStrongViolenceCount;
                    BlacklistedFilesToDownload = BlacklistedGeneralCount + BlacklistedMatureNudityCount + BlacklistedMatureViolenceCount + BlacklistedAdultSexualThemesCount + BlacklistedAdultStrongViolenceCount;

                    // Uncomment when info is built.
                    //if (DownloadInfo.SaveInfo) {
                    //    if (CleanFilesToDownload > 0) {
                    //        File.WriteAllText(
                    //            DownloadInfo.DownloadPath + "\\inkbunny.nfo",
                    //            CleanInfoBuffer.Trim(),
                    //            Encoding.UTF8
                    //        );
                    //    }

                    //    if (DownloadInfo.SaveGraylistedFiles && GraylistedFilesToDownload > 0) {
                    //        File.WriteAllText(
                    //            DownloadInfo.DownloadPath + "\\inkbunny.graylisted.nfo",
                    //            GraylistedInfoBuffer.Trim(),
                    //            Encoding.UTF8
                    //        );
                    //    }

                    //    if (DownloadInfo.SaveBlacklistedFiles && BlacklistedFilesToDownload > 0) {
                    //        File.WriteAllText(
                    //            DownloadInfo.DownloadPath + "\\inkbunny.blacklisted.nfo",
                    //            BlacklistedInfoBuffer.Trim(),
                    //            Encoding.UTF8
                    //        );
                    //    }
                    //}

                    #endregion

                    #region Download
                    Status = DownloadStatus.Downloading;

                    this.Invoke((Action)delegate {
                        Log.Write($"There are {TotalFilesToDownload:N0} files to download with the inkbunny terms \"{DownloadInfo.Identifier}\"");
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
                                } break;
                            }
                            ThrottleCount++;
                        };
                        DownloadClient.DownloadFileCompleted += (s, e) => {
                            this.Invoke((Action)delegate () {
                                pbDownloadStatus.Value = 0;

                                // protect from overflow
                                if (pbTotalStatus.Value < pbTotalStatus.Maximum && e.Error != null) {
                                    pbTotalStatus.Value = Math.Min(DownloadedFiles, pbTotalStatus.Maximum);
                                }
                            });
                        };

                        DownloadClient.Proxy = WebRequest.GetSystemWebProxy();
                        DownloadClient.UserAgent = Program.UserAgent;
                        DownloadClient.Method = HttpMethod.GET;

                        #region Clean
                        if (CleanGeneralCount > 0) {
                            Log.Write($"Downloading general files for inkbunny form \"{DownloadInfo.Identifier}\"");
                            for (int CurrentFile = 0; CurrentFile < CleanGeneralURLs.Count; CurrentFile++) {
                                if (!string.IsNullOrWhiteSpace(CleanGeneralURLs[CurrentFile])) {
                                    this.Invoke((Action)delegate () {
                                        pbTotalStatus.Text = $"download general file {CurrentFile + 1:N0} of {CleanGeneralCount:N0}";
                                        sbStatus.Text = $"Downloading {CleanGeneralFileNames[CurrentFile]}";
                                    });
                                    ShouldRetry = true;
                                    do {
                                        try {
                                            await DownloadClient.DownloadFileTaskAsync(CleanGeneralURLs[CurrentFile], CleanGeneralFilePaths[CurrentFile]);
                                            DownloadedFiles++;
                                            ShouldRetry = false;
                                        }
                                        catch (ThreadAbortException) {
                                            return;
                                        }
                                        catch (Exception ex) {
                                            if (Log.ReportRetriableException(ex, CleanGeneralURLs[CurrentFile]) != DialogResult.Retry) {
                                                ShouldRetry = false;
                                            }
                                        }
                                    } while (ShouldRetry && !DownloadAbort);
                                }
                                if (DownloadAbort) return;
                            }
                            Thread.Sleep(Program.SleepDelay);
                        }

                        if (DownloadAbort) return;

                        if (CleanMatureNudityCount > 0) {
                            Log.Write($"Downloading mature (nudity) files for inkbunny form \"{DownloadInfo.Identifier}\"");
                            for (int CurrentFile = 0; CurrentFile < CleanMatureNudityURLs.Count; CurrentFile++) {
                                if (!string.IsNullOrWhiteSpace(CleanMatureNudityURLs[CurrentFile])) {
                                    this.Invoke((Action)delegate () {
                                        pbTotalStatus.Text = $"download mature (nudity) file {CurrentFile + 1:N0} of {CleanMatureNudityCount:N0}";
                                        sbStatus.Text = $"Downloading {CleanMatureNudityFileNames[CurrentFile]}";
                                    });
                                    ShouldRetry = true;
                                    do {
                                        try {
                                            await DownloadClient.DownloadFileTaskAsync(CleanMatureNudityURLs[CurrentFile], CleanMatureNudityFilePaths[CurrentFile]);
                                            DownloadedFiles++;
                                            ShouldRetry = false;
                                        }
                                        catch (ThreadAbortException) {
                                            return;
                                        }
                                        catch (Exception ex) {
                                            if (Log.ReportRetriableException(ex, CleanMatureNudityURLs[CurrentFile]) != DialogResult.Retry) {
                                                ShouldRetry = false;
                                            }
                                        }
                                    } while (ShouldRetry && !DownloadAbort);
                                }
                                if (DownloadAbort) return;
                            }
                            Thread.Sleep(Program.SleepDelay);
                        }

                        if (DownloadAbort) return;

                        if (CleanMatureViolenceCount > 0) {
                            Log.Write($"Downloading mature (violence) files for inkbunny form \"{DownloadInfo.Identifier}\"");
                            for (int CurrentFile = 0; CurrentFile < CleanMatureViolenceURLs.Count; CurrentFile++) {
                                if (!string.IsNullOrWhiteSpace(CleanMatureViolenceURLs[CurrentFile])) {
                                    this.Invoke((Action)delegate () {
                                        pbTotalStatus.Text = $"download mature (violence) file {CurrentFile + 1:N0} of {CleanMatureViolenceCount:N0}";
                                        sbStatus.Text = $"Downloading {CleanMatureViolenceFileNames[CurrentFile]}";
                                    });
                                    ShouldRetry = true;
                                    do {
                                        try {
                                            await DownloadClient.DownloadFileTaskAsync(CleanMatureViolenceURLs[CurrentFile], CleanMatureViolenceFilePaths[CurrentFile]);
                                            DownloadedFiles++;
                                            ShouldRetry = false;
                                        }
                                        catch (ThreadAbortException) {
                                            return;
                                        }
                                        catch (Exception ex) {
                                            if (Log.ReportRetriableException(ex, CleanMatureViolenceURLs[CurrentFile]) != DialogResult.Retry) {
                                                ShouldRetry = false;
                                            }
                                        }
                                    } while (ShouldRetry && !DownloadAbort);
                                }
                                if (DownloadAbort) return;
                            }
                            Thread.Sleep(Program.SleepDelay);
                        }

                        if (DownloadAbort) return;

                        if (CleanAdultSexualThemesCount > 0) {
                            Log.Write($"Downloading adult (sexual themes) files for inkbunny form \"{DownloadInfo.Identifier}\"");
                            for (int CurrentFile = 0; CurrentFile < CleanAdultSexualThemesURLs.Count; CurrentFile++) {
                                if (!string.IsNullOrWhiteSpace(CleanAdultSexualThemesURLs[CurrentFile])) {
                                    this.Invoke((Action)delegate () {
                                        pbTotalStatus.Text = $"download adult (sexual themes) file {CurrentFile + 1:N0} of {CleanAdultSexualThemesCount:N0}";
                                        sbStatus.Text = $"Downloading {CleanAdultSexualThemesFileNames[CurrentFile]}";
                                    });
                                    ShouldRetry = true;
                                    do {
                                        try {
                                            await DownloadClient.DownloadFileTaskAsync(CleanAdultSexualThemesURLs[CurrentFile], CleanAdultSexualThemesFilePaths[CurrentFile]);
                                            DownloadedFiles++;
                                            ShouldRetry = false;
                                        }
                                        catch (ThreadAbortException) {
                                            return;
                                        }
                                        catch (Exception ex) {
                                            if (Log.ReportRetriableException(ex, CleanAdultSexualThemesURLs[CurrentFile]) != DialogResult.Retry) {
                                                ShouldRetry = false;
                                            }
                                        }
                                    } while (ShouldRetry && !DownloadAbort);
                                }
                                if (DownloadAbort) return;
                            }
                            Thread.Sleep(Program.SleepDelay);
                        }

                        if (DownloadAbort) return;

                        if (CleanAdultStrongViolenceCount > 0) {
                            Log.Write($"Downloading adult (strong violence) files for inkbunny form \"{DownloadInfo.Identifier}\"");
                            for (int CurrentFile = 0; CurrentFile < CleanAdultStrongViolenceURLs.Count; CurrentFile++) {
                                if (!string.IsNullOrWhiteSpace(CleanAdultStrongViolenceURLs[CurrentFile])) {
                                    this.Invoke((Action)delegate () {
                                        pbTotalStatus.Text = $"download adult (strong violence) file {CurrentFile + 1:N0} of {CleanAdultStrongViolenceCount:N0}";
                                        sbStatus.Text = $"Downloading {CleanAdultStrongViolenceFileNames[CurrentFile]}";
                                    });
                                    ShouldRetry = true;
                                    do {
                                        try {
                                            await DownloadClient.DownloadFileTaskAsync(CleanAdultStrongViolenceURLs[CurrentFile], CleanAdultStrongViolenceFilePaths[CurrentFile]);
                                            DownloadedFiles++;
                                            ShouldRetry = false;
                                        }
                                        catch (ThreadAbortException) {
                                            return;
                                        }
                                        catch (Exception ex) {
                                            if (Log.ReportRetriableException(ex, CleanAdultStrongViolenceURLs[CurrentFile]) != DialogResult.Retry) {
                                                ShouldRetry = false;
                                            }
                                        }
                                    } while (ShouldRetry && !DownloadAbort);
                                }
                                if (DownloadAbort) return;
                            }
                            Thread.Sleep(Program.SleepDelay);
                        }
                        #endregion

                        if (DownloadAbort) return;

                        #region Graylisted
                        if (GraylistedGeneralCount > 0) {
                            Log.Write($"Downloading general files for inkbunny form \"{DownloadInfo.Identifier}\"");
                            for (int CurrentFile = 0; CurrentFile < GraylistedGeneralURLs.Count; CurrentFile++) {
                                if (!string.IsNullOrWhiteSpace(GraylistedGeneralURLs[CurrentFile])) {
                                    this.Invoke((Action)delegate () {
                                        pbTotalStatus.Text = $"download general file {CurrentFile + 1:N0} of {GraylistedGeneralCount:N0}";
                                        sbStatus.Text = $"Downloading {GraylistedGeneralFileNames[CurrentFile]}";
                                    });
                                    ShouldRetry = true;
                                    do {
                                        try {
                                            await DownloadClient.DownloadFileTaskAsync(GraylistedGeneralURLs[CurrentFile], GraylistedGeneralFilePaths[CurrentFile]);
                                            DownloadedFiles++;
                                            ShouldRetry = false;
                                        }
                                        catch (ThreadAbortException) {
                                            return;
                                        }
                                        catch (Exception ex) {
                                            if (Log.ReportRetriableException(ex, GraylistedGeneralURLs[CurrentFile]) != DialogResult.Retry) {
                                                ShouldRetry = false;
                                            }
                                        }
                                    } while (ShouldRetry && !DownloadAbort);
                                }
                                if (DownloadAbort) return;
                            }
                            Thread.Sleep(Program.SleepDelay);
                        }

                        if (DownloadAbort) return;

                        if (GraylistedMatureNudityCount > 0) {
                            Log.Write($"Downloading mature (nudity) files for inkbunny form \"{DownloadInfo.Identifier}\"");
                            for (int CurrentFile = 0; CurrentFile < GraylistedMatureNudityURLs.Count; CurrentFile++) {
                                if (!string.IsNullOrWhiteSpace(GraylistedMatureNudityURLs[CurrentFile])) {
                                    this.Invoke((Action)delegate () {
                                        pbTotalStatus.Text = $"download mature (nudity) file {CurrentFile + 1:N0} of {GraylistedMatureNudityCount:N0}";
                                        sbStatus.Text = $"Downloading {GraylistedMatureNudityFileNames[CurrentFile]}";
                                    });
                                    ShouldRetry = true;
                                    do {
                                        try {
                                            await DownloadClient.DownloadFileTaskAsync(GraylistedMatureNudityURLs[CurrentFile], GraylistedMatureNudityFilePaths[CurrentFile]);
                                            DownloadedFiles++;
                                            ShouldRetry = false;
                                        }
                                        catch (ThreadAbortException) {
                                            return;
                                        }
                                        catch (Exception ex) {
                                            if (Log.ReportRetriableException(ex, GraylistedMatureNudityURLs[CurrentFile]) != DialogResult.Retry) {
                                                ShouldRetry = false;
                                            }
                                        }
                                    } while (ShouldRetry && !DownloadAbort);
                                }
                                if (DownloadAbort) return;
                            }
                            Thread.Sleep(Program.SleepDelay);
                        }

                        if (DownloadAbort) return;

                        if (GraylistedMatureViolenceCount > 0) {
                            Log.Write($"Downloading mature (violence) files for inkbunny form \"{DownloadInfo.Identifier}\"");
                            for (int CurrentFile = 0; CurrentFile < GraylistedMatureViolenceURLs.Count; CurrentFile++) {
                                if (!string.IsNullOrWhiteSpace(GraylistedMatureViolenceURLs[CurrentFile])) {
                                    this.Invoke((Action)delegate () {
                                        pbTotalStatus.Text = $"download mature (violence) file {CurrentFile + 1:N0} of {GraylistedMatureViolenceCount:N0}";
                                        sbStatus.Text = $"Downloading {GraylistedMatureViolenceFileNames[CurrentFile]}";
                                    });
                                    ShouldRetry = true;
                                    do {
                                        try {
                                            await DownloadClient.DownloadFileTaskAsync(GraylistedMatureViolenceURLs[CurrentFile], GraylistedMatureViolenceFilePaths[CurrentFile]);
                                            DownloadedFiles++;
                                            ShouldRetry = false;
                                        }
                                        catch (ThreadAbortException) {
                                            return;
                                        }
                                        catch (Exception ex) {
                                            if (Log.ReportRetriableException(ex, GraylistedMatureViolenceURLs[CurrentFile]) != DialogResult.Retry) {
                                                ShouldRetry = false;
                                            }
                                        }
                                    } while (ShouldRetry && !DownloadAbort);
                                }
                                if (DownloadAbort) return;
                            }
                            Thread.Sleep(Program.SleepDelay);
                        }

                        if (DownloadAbort) return;

                        if (GraylistedAdultSexualThemesCount > 0) {
                            Log.Write($"Downloading adult (sexual themes) files for inkbunny form \"{DownloadInfo.Identifier}\"");
                            for (int CurrentFile = 0; CurrentFile < GraylistedAdultSexualThemesURLs.Count; CurrentFile++) {
                                if (!string.IsNullOrWhiteSpace(GraylistedAdultSexualThemesURLs[CurrentFile])) {
                                    this.Invoke((Action)delegate () {
                                        pbTotalStatus.Text = $"download adult (sexual themes) file {CurrentFile + 1:N0} of {GraylistedAdultSexualThemesCount:N0}";
                                        sbStatus.Text = $"Downloading {GraylistedAdultSexualThemesFileNames[CurrentFile]}";
                                    });
                                    ShouldRetry = true;
                                    do {
                                        try {
                                            await DownloadClient.DownloadFileTaskAsync(GraylistedAdultSexualThemesURLs[CurrentFile], GraylistedAdultSexualThemesFilePaths[CurrentFile]);
                                            DownloadedFiles++;
                                            ShouldRetry = false;
                                        }
                                        catch (ThreadAbortException) {
                                            return;
                                        }
                                        catch (Exception ex) {
                                            if (Log.ReportRetriableException(ex, GraylistedAdultSexualThemesURLs[CurrentFile]) != DialogResult.Retry) {
                                                ShouldRetry = false;
                                            }
                                        }
                                    } while (ShouldRetry && !DownloadAbort);
                                }
                                if (DownloadAbort) return;
                            }
                            Thread.Sleep(Program.SleepDelay);
                        }

                        if (DownloadAbort) return;

                        if (GraylistedAdultStrongViolenceCount > 0) {
                            Log.Write($"Downloading adult (strong violence) files for inkbunny form \"{DownloadInfo.Identifier}\"");
                            for (int CurrentFile = 0; CurrentFile < GraylistedAdultStrongViolenceURLs.Count; CurrentFile++) {
                                if (!string.IsNullOrWhiteSpace(GraylistedAdultStrongViolenceURLs[CurrentFile])) {
                                    this.Invoke((Action)delegate () {
                                        pbTotalStatus.Text = $"download adult (strong violence) file {CurrentFile + 1:N0} of {GraylistedAdultStrongViolenceCount:N0}";
                                        sbStatus.Text = $"Downloading {GraylistedAdultStrongViolenceFileNames[CurrentFile]}";
                                    });
                                    ShouldRetry = true;
                                    do {
                                        try {
                                            await DownloadClient.DownloadFileTaskAsync(GraylistedAdultStrongViolenceURLs[CurrentFile], GraylistedAdultStrongViolenceFilePaths[CurrentFile]);
                                            DownloadedFiles++;
                                            ShouldRetry = false;
                                        }
                                        catch (ThreadAbortException) {
                                            return;
                                        }
                                        catch (Exception ex) {
                                            if (Log.ReportRetriableException(ex, GraylistedAdultStrongViolenceURLs[CurrentFile]) != DialogResult.Retry) {
                                                ShouldRetry = false;
                                            }
                                        }
                                    } while (ShouldRetry && !DownloadAbort);
                                }
                                if (DownloadAbort) return;
                            }
                            Thread.Sleep(Program.SleepDelay);
                        }
                        #endregion

                        if (DownloadAbort) return;

                        #region Blacklisted
                        if (BlacklistedGeneralCount > 0) {
                            Log.Write($"Downloading general files for inkbunny form \"{DownloadInfo.Identifier}\"");
                            for (int CurrentFile = 0; CurrentFile < BlacklistedGeneralURLs.Count; CurrentFile++) {
                                if (!string.IsNullOrWhiteSpace(BlacklistedGeneralURLs[CurrentFile])) {
                                    this.Invoke((Action)delegate () {
                                        pbTotalStatus.Text = $"download general file {CurrentFile + 1:N0} of {BlacklistedGeneralCount:N0}";
                                        sbStatus.Text = $"Downloading {BlacklistedGeneralFileNames[CurrentFile]}";
                                    });
                                    ShouldRetry = true;
                                    do {
                                        try {
                                            await DownloadClient.DownloadFileTaskAsync(BlacklistedGeneralURLs[CurrentFile], BlacklistedGeneralFilePaths[CurrentFile]);
                                            DownloadedFiles++;
                                            ShouldRetry = false;
                                        }
                                        catch (ThreadAbortException) {
                                            return;
                                        }
                                        catch (Exception ex) {
                                            if (Log.ReportRetriableException(ex, BlacklistedGeneralURLs[CurrentFile]) != DialogResult.Retry) {
                                                ShouldRetry = false;
                                            }
                                        }
                                    } while (ShouldRetry && !DownloadAbort);
                                }
                                if (DownloadAbort) return;
                            }
                            Thread.Sleep(Program.SleepDelay);
                        }

                        if (DownloadAbort) return;

                        if (BlacklistedMatureNudityCount > 0) {
                            Log.Write($"Downloading mature (nudity) files for inkbunny form \"{DownloadInfo.Identifier}\"");
                            for (int CurrentFile = 0; CurrentFile < BlacklistedMatureNudityURLs.Count; CurrentFile++) {
                                if (!string.IsNullOrWhiteSpace(BlacklistedMatureNudityURLs[CurrentFile])) {
                                    this.Invoke((Action)delegate () {
                                        pbTotalStatus.Text = $"download mature (nudity) file {CurrentFile + 1:N0} of {BlacklistedMatureNudityCount:N0}";
                                        sbStatus.Text = $"Downloading {BlacklistedMatureNudityFileNames[CurrentFile]}";
                                    });
                                    ShouldRetry = true;
                                    do {
                                        try {
                                            await DownloadClient.DownloadFileTaskAsync(BlacklistedMatureNudityURLs[CurrentFile], BlacklistedMatureNudityFilePaths[CurrentFile]);
                                            DownloadedFiles++;
                                            ShouldRetry = false;
                                        }
                                        catch (ThreadAbortException) {
                                            return;
                                        }
                                        catch (Exception ex) {
                                            if (Log.ReportRetriableException(ex, BlacklistedMatureNudityURLs[CurrentFile]) != DialogResult.Retry) {
                                                ShouldRetry = false;
                                            }
                                        }
                                    } while (ShouldRetry && !DownloadAbort);
                                }
                                if (DownloadAbort) return;
                            }
                            Thread.Sleep(Program.SleepDelay);
                        }

                        if (DownloadAbort) return;

                        if (BlacklistedMatureViolenceCount > 0) {
                            Log.Write($"Downloading mature (violence) files for inkbunny form \"{DownloadInfo.Identifier}\"");
                            for (int CurrentFile = 0; CurrentFile < BlacklistedMatureViolenceURLs.Count; CurrentFile++) {
                                if (!string.IsNullOrWhiteSpace(BlacklistedMatureViolenceURLs[CurrentFile])) {
                                    this.Invoke((Action)delegate () {
                                        pbTotalStatus.Text = $"download mature (violence) file {CurrentFile + 1:N0} of {BlacklistedMatureViolenceCount:N0}";
                                        sbStatus.Text = $"Downloading {BlacklistedMatureViolenceFileNames[CurrentFile]}";
                                    });
                                    ShouldRetry = true;
                                    do {
                                        try {
                                            await DownloadClient.DownloadFileTaskAsync(BlacklistedMatureViolenceURLs[CurrentFile], BlacklistedMatureViolenceFilePaths[CurrentFile]);
                                            DownloadedFiles++;
                                            ShouldRetry = false;
                                        }
                                        catch (ThreadAbortException) {
                                            return;
                                        }
                                        catch (Exception ex) {
                                            if (Log.ReportRetriableException(ex, BlacklistedMatureViolenceURLs[CurrentFile]) != DialogResult.Retry) {
                                                ShouldRetry = false;
                                            }
                                        }
                                    } while (ShouldRetry && !DownloadAbort);
                                }
                                if (DownloadAbort) return;
                            }
                            Thread.Sleep(Program.SleepDelay);
                        }

                        if (DownloadAbort) return;

                        if (BlacklistedAdultSexualThemesCount > 0) {
                            Log.Write($"Downloading adult (sexual themes) files for inkbunny form \"{DownloadInfo.Identifier}\"");
                            for (int CurrentFile = 0; CurrentFile < BlacklistedAdultSexualThemesURLs.Count; CurrentFile++) {
                                if (!string.IsNullOrWhiteSpace(BlacklistedAdultSexualThemesURLs[CurrentFile])) {
                                    this.Invoke((Action)delegate () {
                                        pbTotalStatus.Text = $"download adult (sexual themes) file {CurrentFile + 1:N0} of {BlacklistedAdultSexualThemesCount:N0}";
                                        sbStatus.Text = $"Downloading {BlacklistedAdultSexualThemesFileNames[CurrentFile]}";
                                    });
                                    ShouldRetry = true;
                                    do {
                                        try {
                                            await DownloadClient.DownloadFileTaskAsync(BlacklistedAdultSexualThemesURLs[CurrentFile], BlacklistedAdultSexualThemesFilePaths[CurrentFile]);
                                            DownloadedFiles++;
                                            ShouldRetry = false;
                                        }
                                        catch (ThreadAbortException) {
                                            return;
                                        }
                                        catch (Exception ex) {
                                            if (Log.ReportRetriableException(ex, BlacklistedAdultSexualThemesURLs[CurrentFile]) != DialogResult.Retry) {
                                                ShouldRetry = false;
                                            }
                                        }
                                    } while (ShouldRetry && !DownloadAbort);
                                }
                                if (DownloadAbort) return;
                            }
                            Thread.Sleep(Program.SleepDelay);
                        }

                        if (DownloadAbort) return;

                        if (BlacklistedAdultStrongViolenceCount > 0) {
                            Log.Write($"Downloading adult (strong violence) files for inkbunny form \"{DownloadInfo.Identifier}\"");
                            for (int CurrentFile = 0; CurrentFile < BlacklistedAdultStrongViolenceURLs.Count; CurrentFile++) {
                                if (!string.IsNullOrWhiteSpace(BlacklistedAdultStrongViolenceURLs[CurrentFile])) {
                                    this.Invoke((Action)delegate () {
                                        pbTotalStatus.Text = $"download adult (strong violence) file {CurrentFile + 1:N0} of {BlacklistedAdultStrongViolenceCount:N0}";
                                        sbStatus.Text = $"Downloading {BlacklistedAdultStrongViolenceFileNames[CurrentFile]}";
                                    });
                                    ShouldRetry = true;
                                    do {
                                        try {
                                            await DownloadClient.DownloadFileTaskAsync(BlacklistedAdultStrongViolenceURLs[CurrentFile], BlacklistedAdultStrongViolenceFilePaths[CurrentFile]);
                                            DownloadedFiles++;
                                            ShouldRetry = false;
                                        }
                                        catch (ThreadAbortException) {
                                            return;
                                        }
                                        catch (Exception ex) {
                                            if (Log.ReportRetriableException(ex, BlacklistedAdultStrongViolenceURLs[CurrentFile]) != DialogResult.Retry) {
                                                ShouldRetry = false;
                                            }
                                        }
                                    } while (ShouldRetry && !DownloadAbort);
                                }
                                if (DownloadAbort) return;
                            }
                            Thread.Sleep(Program.SleepDelay);
                        }
                        #endregion

                        if (DownloadAbort) return;

                    }
                    #endregion

                    #region Post-download
                    Status = DownloadStatus.Finished;
                    Log.Write($"InkBunny download \"{DownloadInfo.Identifier}\" finished with {DownloadedFiles:N0} files downloaded.");
                    #endregion

                }
                #endregion

                #region Catch block
                catch (ApiReturnedNullOrEmptyException) {
                    Log.Write($"Tags \"{DownloadInfo.Identifier}\" Api returned null or empty.");
                    Status = DownloadStatus.ApiReturnedNullOrEmpty;
                    DownloadError = true;
                }
                catch (NoFilesToDownloadException) {
                    Log.Write($"No files are available for tags \"{DownloadInfo.Identifier}\".");
                    Status = DownloadStatus.NothingToDownload;
                    DownloadError = true;
                }
                catch (ThreadAbortException) {
                    Log.Write($"The inkbunny download \"{DownloadInfo.Identifier}\" thread was aborted.");
                    if (DownloadClient != null && DownloadClient.IsBusy) {
                        DownloadClient.CancelAsync();
                    }
                    Status = DownloadStatus.Aborted;
                    DownloadError = true;
                }
                catch (ObjectDisposedException) {
                    Log.Write($"Seems like the inkbunny download \"{DownloadInfo.Identifier}\" form got disposed.");
                    Status = DownloadStatus.FormWasDisposed;
                    DownloadError = true;
                }
                catch (WebException WebE) {
                    Log.Write($"A WebException occured downloading from inkbunny \"{DownloadInfo.Identifier}\".");
                    Status = DownloadStatus.Errored;
                    Log.ReportException(WebE, CurrentUrl);
                    DownloadError = true;
                }
                catch (Exception ex) {
                    Log.Write($"An Exception occured downloading from inkbunny \"{DownloadInfo.Identifier}\".");
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
                    this.Invoke((Action)delegate () {
                        FinishDownload();
                    });
                }
                #endregion

            });
            Downloader.InkBunnySearchesBeingDownloaded.Add(DownloadInfo.Identifier);
            DownloadThread.IsBackground = true;
            DownloadThread.Name = $"inkbunny thread {DownloadInfo.Identifier}";
            pbDownloadStatus.Style = ProgressBarStyle.Marquee;
            DownloadThread.Start();
            tmrTitle.Start();
        }

        protected override void FinishDownload() {
            tmrTitle.Stop();
            pbDownloadStatus.Style = ProgressBarStyle.Blocks;
            Downloader.InkBunnySearchesBeingDownloaded.Remove(DownloadInfo.Identifier);

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
                        this.Text = "Inkbunny download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.Parsing: {
                        pbDownloadStatus.Text = "Finished while parsing?";
                        pbTotalStatus.Text = "Weird error occurred";
                        this.Text = "Inkbunny download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.ReadyToDownload: {
                        pbDownloadStatus.Text = "Finished while ready?";
                        pbTotalStatus.Text = "Weird error occurred";
                        this.Text = "Inkbunny download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.Downloading: {
                        pbDownloadStatus.Text = "Finished while downloading?";
                        pbTotalStatus.Text = "Weird error occurred";
                        this.Text = "Inkbunny download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.Finished: {
                        pbDownloadStatus.Text = "Downloading complete";
                        pbTotalStatus.Text = $"{DownloadedFiles:N0} / {CleanFilesToDownload + GraylistedFilesToDownload + BlacklistedFilesToDownload:N0} files downloaded";
                        this.Text = "Inkbunny download complete";
                        sbStatus.Text = "Finished";
                        pbDownloadStatus.Value = pbDownloadStatus.Maximum;
                        pbTotalStatus.Value = pbTotalStatus.Maximum;
                    } break;

                    case DownloadStatus.Errored: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "Error";
                        pbTotalStatus.Text = "An error occurred";
                        this.Text = "Inkbunny download error";
                        sbStatus.Text = "Error";
                    } break;

                    case DownloadStatus.Aborted: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "Aborted";
                        pbTotalStatus.Text = $"The download was aborted{(DownloadedFiles > 0 ? $", {DownloadedFiles:N0} files downloaded" : "")}";
                        this.Text = "Inkbunny download aborted";
                        sbStatus.Text = "Aborted";
                    } break;

                    case DownloadStatus.Forbidden: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "Forbidden";
                        pbTotalStatus.Text = "The connection was forbidden";
                        this.Text = "Inkbunny download error";
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
                        this.Text = "Inkbunny download complete";
                        sbStatus.Text = "Already exists";
                    } break;

                    case DownloadStatus.NothingToDownload: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "There's nothing to download";
                        pbTotalStatus.Text = "Nothing to download";
                        this.Text = "Inkbunny download complete";
                        sbStatus.Text = "Nothing to download";
                    } break;

                    case DownloadStatus.PostOrPoolWasDeleted: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "The post was deleted.";
                        pbTotalStatus.Text = "An error occurred";
                        this.Text = "Inkbunny post was deleted";
                        sbStatus.Text = "Post was deleted";
                    } break;

                    case DownloadStatus.ApiReturnedNullOrEmpty: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "The api returned null or empty";
                        pbTotalStatus.Text = "An error occurred";
                        this.Text = "Inkbunny api null/empty error";
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
