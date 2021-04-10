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
    public partial class frmTagDownloader : Form {

        #region Public Variables
        public TagDownloadInfo DownloadInfo;
        public static readonly string TagURL = "https://e621.net/posts.json?tags={0}&limit=320";
        public static readonly string PageURL = "https://e621.net/posts.json?tags={0}&limit=320&page={1}";

        //       \ = %5C
        //       / = %2F
        //       : = %3A
        //       * = %2A
        //       ? = %3F
        //       " = %22
        //       < = %3C
        //       > = %3E
        //       | = %7C
        // (space) = %20
        #endregion

        #region Private Variables
        private Thread TagDownloadThread;                     // New thread for the downloader.

        private int CleanTotalCount = 0;                // Will be the count of how many files that are set for download.
        private int CleanExplicitCount = 0;             // Will be the count of how many explicit files that are set for download.
        private int CleanQuestionableCount = 0;         // Will be the count of how many questionable files that are set for download.
        private int CleanSafeCount = 0;                 // Will be the count of how many safe files that are set for download.
        private int CleanTotalExistCount = 0;           // Will be the count of how many files already exist in total.
        private int CleanExplicitExistCount = 0;        // Will be the count of how many explicit files already exist.
        private int CleanQuestionableExistCount = 0;    // Will be the count of how many questionable files already exist.
        private int CleanSafeExistCount = 0;            // Will be the count of how many safe files already exist.

        private int GraylistTotalCount = 0;             // Will be the count of how many graylisted files that are set for download.
        private int GraylistExplicitCount = 0;          // Will be the count of how many explicit graylisted files that are set for download.
        private int GraylistQuestionableCount = 0;      // Will be the count of how many questionable graylisted files that are set for download.
        private int GraylistSafeCount = 0;              // Will be the count of how many safe graylisted files that are set for download.
        private int GraylistTotalExistCount = 0;        // Will be the count of how many gralisted files already exist in total.
        private int GraylistExplicitExistCount = 0;     // Will be the count of how many explicit graylisted files already exist.
        private int GraylistQuestionableExistCount = 0; // Will be the count of how many questionable graylisted files already exist.
        private int GraylistSafeExistCount = 0;         // Will be the count of how many safe graylisted files already exist.

        private int BlacklistTotalCount = 0;             // Will be the count of how many blacklisted files that are set for download.
        private int BlacklistExplicitCount = 0;          // Will be the count of how many explicit blacklisted files that are set for download.
        private int BlacklistQuestionableCount = 0;      // Will be the count of how many questionable blacklisted files that are set for download.
        private int BlacklistSafeCount = 0;              // Will be the count of how many safe blacklisted files that are set for download.
        private int BlacklistTotalExistCount = 0;        // Will be the count of how many gralisted files already exist in total.
        private int BlacklistExplicitExistCount = 0;     // Will be the count of how many explicit blacklisted files already exist.
        private int BlacklistQuestionableExistCount = 0; // Will be the count of how many questionable blacklisted files already exist.
        private int BlacklistSafeExistCount = 0;         // Will be the count of how many safe blacklisted files already exist.

        private int TotalCount = 0;                     // Will be the count of how many files that were parsed.
        private int CleanSkippedCount = 0;
        private int GraylistSkippedCount = 0;                  // Will be the count of how many graylisted files that will be skipped.
        private int BlacklistSkippedCount = 0;

        private int PresentFiles = 0;
        private int TotalFiles = 0;

        private DownloadStatus CurrentStatus = DownloadStatus.Waiting;
        #endregion

        private bool UseNewDownloadLogic = true;

        #region Form
        public frmTagDownloader() {
            InitializeComponent();
        }
        private void frmDownload_Load(object sender, EventArgs e) {
            if (!DownloadInfo.SaveExplicit && !DownloadInfo.SaveQuestionable && !DownloadInfo.SaveSafe) {
                MessageBox.Show("No ratings were selected!", "aphrodite", MessageBoxButtons.OK);
                this.Opacity = 0;
                this.DialogResult = DialogResult.No;
                return;
            }
            else if (DownloadInfo.ImageLimit == 0 && DownloadInfo.PageLimit == 0) {
                if (MessageBox.Show("Downloading won't be limited. This may take a long while or even blacklist you. Continue anyway?", "aphrodite", MessageBoxButtons.YesNo) == DialogResult.No) {
                    this.Opacity = 0;
                    this.DialogResult = DialogResult.No;
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(DownloadInfo.FileNameSchema)) {
                DownloadInfo.FileNameSchema = "%md5%";
            }

            tmrTitle.Start();
            txtTags.Text = DownloadInfo.Tags.Replace("%25-2F", "/");

            lbBlacklist.Text = "No file counts.\nWaiting for first json parse.";

            string minScore = "Minimum score: disabled";
            string imgLim = "Image limit: disabled";
            string pageLim = "Page limit: disabled";

            int tagCount = DownloadInfo.Tags.Split(' ').Length;

            if (tagCount == 6 && DownloadInfo.MinimumScoreAsTag) {
                DownloadInfo.MinimumScoreAsTag = false;
            }

            if (DownloadInfo.UseMinimumScore) {
                minScore = "Minimum score: " + DownloadInfo.MinimumScore.ToString();
            }

            if (DownloadInfo.MinimumScoreAsTag) {
                minScore += " (as tag)";
            }

            if (DownloadInfo.ImageLimit > 0) {
                imgLim = "Image limit: " + DownloadInfo.ImageLimit.ToString() + "images";
            }

            if (DownloadInfo.PageLimit > 0) {
                pageLim = "Page limit: " + DownloadInfo.PageLimit.ToString() + " pages";
            }

            string ratingBuffer = "\nRatings: ";
            if (DownloadInfo.SaveExplicit) {
                ratingBuffer += "e";
            }
            if (DownloadInfo.SaveQuestionable) {
                if (ratingBuffer.EndsWith("e")) {
                    ratingBuffer += ", ";
                }
                ratingBuffer += "q";
            }
            if (DownloadInfo.SaveSafe) {
                if (ratingBuffer.EndsWith("e") || ratingBuffer.EndsWith("q")) {
                    ratingBuffer += ", ";
                }

                ratingBuffer += "s";
            }

            if (DownloadInfo.SeparateRatings) {
                ratingBuffer += " (separating)";
            }

            lbLimits.Text = minScore + "\r\n" + imgLim + "\r\n" + pageLim + ratingBuffer;
            StartDownload();
            this.CenterToScreen();
        }
        private void frmDownload_FormClosing(object sender, FormClosingEventArgs e) {
            switch (CurrentStatus) {
                case DownloadStatus.Finished:
                case DownloadStatus.Errored:
                case DownloadStatus.Aborted:
                    this.Dispose();
                    break;

                default:
                    if (TagDownloadThread != null && TagDownloadThread.IsAlive) {
                        TagDownloadThread.Abort();
                    }

                    if (!DownloadInfo.IgnoreFinish) {
                        e.Cancel = true;
                    }
                    break;
            }
        }

        private void tmrTitle_Tick(object sender, EventArgs e) {
            if (this.Text.EndsWith("...."))
                this.Text = this.Text.TrimEnd('.');
            else
                this.Text += ".";
        }
        #endregion

        #region Downloader
        private void StartDownload() {
            Program.Log(LogAction.WriteToLog, "Starting Tag download for \"" + DownloadInfo.Tags + "\"");
            TagDownloadThread = new Thread(() => {
                Thread.CurrentThread.IsBackground = true;
                DownloadPosts();
            });

            TagDownloadThread.Start();
            tmrTitle.Start();
        }

        private void AfterDownload() {
            Program.Log(LogAction.WriteToLog, "Tag download for \"" + DownloadInfo.Tags + "\" finished.");
            tmrTitle.Stop();
            if (DownloadInfo.OpenAfter) {
                Process.Start(DownloadInfo.DownloadPath);
            }

            if (DownloadInfo.IgnoreFinish) {
                switch (CurrentStatus) {
                    case DownloadStatus.Finished:
                    case DownloadStatus.NothingToDownload:
                        this.DialogResult = DialogResult.Yes;
                        break;

                    case DownloadStatus.Errored:
                        lbFile.Text = "Downloading has encountered an error";
                        pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                        lbPercentage.Text = "Error";
                        this.Text = "Download error";
                        status.Text = "Downloading has resulted in an error";
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
                        lbFile.Text = "All " + (PresentFiles) + " file(s) downloaded. (" + (TotalFiles) + " total files downloaded)";
                        pbDownloadStatus.Value = pbDownloadStatus.Maximum;
                        lbPercentage.Text = "Done";
                        this.Text = "Finished downloading tags " + DownloadInfo.Tags;
                        status.Text = "Finished downloading tags";
                        System.Media.SystemSounds.Exclamation.Play();
                        break;

                    case DownloadStatus.Errored:
                        lbFile.Text = "Downloading has encountered an error";
                        pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                        lbPercentage.Text = "Error";
                        this.Text = "Download error";
                        status.Text = "Downloading has resulted in an error";
                        System.Media.SystemSounds.Hand.Play();
                        break;

                    case DownloadStatus.Aborted:
                        lbFile.Text = "Download canceled";
                        pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                        lbPercentage.Text = "Canceled";
                        this.Text = "Download canceled";
                        status.Text = "Download has canceled";
                        break;

                    case DownloadStatus.NothingToDownload:
                        lbFile.Text = "No files were available for download.";
                        pbDownloadStatus.Value = pbDownloadStatus.Minimum;
                        lbPercentage.Text = "-";
                        this.Text = "No downloads for tags " + DownloadInfo.Tags;
                        status.Text = "No downloads available";
                        System.Media.SystemSounds.Asterisk.Play();
                        break;

                    case DownloadStatus.ApiReturnedNullOrEmpty:
                        lbFile.Text = "API parsing has resulted in an error";
                        pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                        lbPercentage.Text = "Error";
                        this.Text = "API parsing error";
                        status.Text = "API parsing has resulted in an error";
                        System.Media.SystemSounds.Hand.Play();
                        break;

                    default:
                        // assume it completed
                        lbFile.Text = "Download assumed to be completed";
                        pbDownloadStatus.Value = pbDownloadStatus.Maximum;
                        lbPercentage.Text = "Done?";
                        this.Text = "Tags download completed?";
                        status.Text = "Download status not set, assuming the download completed";
                        System.Media.SystemSounds.Asterisk.Play();
                        break;
                }
            }
        }

        private async void DownloadPosts() {
            #region Download variables
            string CurrentUrl = string.Empty;               // The URL being accessed, changes per API call/File download.
            string CleanInfoBuffer = string.Empty;            // The buffer for the 'tag.nfo' file that will be created.
            string GraylistInfoBuffer = string.Empty;
            string BlacklistInfoBuffer = string.Empty;      // The buffer for the 'tag.blacklisted.nfo' file that will be created.

            string CurrentXml = string.Empty;               // The XML string.
            int CurrentPage = 1;                            // Will be the count of the pages parsed.
            bool ImageLimitReached = false;                    // Determines if the maximum files have been parsed.

            // SeparateRatings disabled \\
            List<string> CleanedURLs = new List<string>();             // The URLs that will be downloaded (if separateRatings = false).
            List<string> CleanedFileNames = new List<string>();            // Contains the file names of the images
            List<string> CleanedFilePaths = new List<string>();
            List<string> GraylistedURLs = new List<string>();   // The Blacklisted URLs that will be downloaded (if separateRatings = false).
            List<string> GraylistedFileNames = new List<string>();  // Contains the file names of the graylisted images
            List<string> GraylistedFilePaths = new List<string>();
            List<string> BlacklistedURLs = new List<string>();
            List<string> BlacklistedFileNames = new List<string>();
            List<string> BlacklistedFilePaths = new List<string>();
            //\\//\\//\\//\\//\\//\\//\\//\\

            // Separate ratings enabled \\
            List<string> CleanedExplicitURLs = new List<string>();         // The list of Explicit files.
            List<string> CleanedExplicitFileNames = new List<string>();    // The list of Explicit file names.
            List<string> CleanedExplicitFilePaths = new List<string>();
            List<string> CleanedQuestionableURLs = new List<string>();         // The list of Questionable files.
            List<string> CleanedQuestionableFileNames = new List<string>();    // The list of Questionable file names.
            List<string> CleanedQuestionableFilePaths = new List<string>();
            List<string> CleanedSafeURLs = new List<string>();                     // The list of Safe files.
            List<string> CleanedSafeFileNames = new List<string>();                // The list of Safe file names.
            List<string> CleanedSafeFilePaths = new List<string>();

            List<string> GraylistedExplicitURLs = new List<string>();       // The list of Graylisted Explicit files.
            List<string> GraylistedExplicitFileNames = new List<string>();  // The list of Graylisted Explicit file names.
            List<string> GraylistedExplicitFilePaths = new List<string>();
            List<string> GraylistedQuestionableURLs = new List<string>();       // The list of Graylisted Questionable files.
            List<string> GraylistedQuestionableFileNames = new List<string>();  // The list of Graylitsed Questionable file names.
            List<string> GraylistedQuestionableFilePaths = new List<string>();
            List<string> GraylistedSafeURLs = new List<string>();           // The list of Graylisted Safe files.
            List<string> GraylistedSafeFileNames = new List<string>();      // The list of Graylisted Safe file names.
            List<string> GraylistedSafeFilePaths = new List<string>();

            List<string> BlacklistedExplicitURLs = new List<string>();       // The list of Blacklisted Explicit files.
            List<string> BlacklistedExplicitFileNames = new List<string>();  // The list of Blacklisted Explicit file names.
            List<string> BlacklistedExplicitFilePaths = new List<string>();
            List<string> BlacklistedQuestionableURLs = new List<string>();       // The list of Blacklisted Questionable files.
            List<string> BlacklistedQuestionableFileNames = new List<string>();  // The list of Blacklitsed Questionable file names.
            List<string> BlacklistedQuestionableFilePaths = new List<string>();
            List<string> BlacklistedSafeURLs = new List<string>();           // The list of Blacklisted Safe files.
            List<string> BlacklistedSafeFileNames = new List<string>();      // The list of Blacklisted Safe file names.
            List<string> BlacklistedSafeFilePaths = new List<string>();
            //\\//\\//\\//\\//\\//\\//\\//\\

            // the Boolean lists of if non-images exist to create the save folder
            // 0 = gif, 1 = apng, 2 = webm, 3 = swf
            List<bool> CleanedFileNonImages = new List<bool> { false, false, false, false };
            List<bool> GraylistedFileNonImages = new List<bool> { false, false, false, false };
            List<bool> BlacklistedFileNonImages = new List<bool> { false, false, false, false };
            List<bool> CleanedExplicitNonImages = new List<bool> { false, false, false, false };
            List<bool> CleanedQuestionableNonImages = new List<bool> { false, false, false, false };
            List<bool> CleanedSafeNonImages = new List<bool> { false, false, false, false };
            List<bool> GraylistedExplicitNonImages = new List<bool> { false, false, false, false };
            List<bool> GraylistedQuestionableNonImages = new List<bool> { false, false, false, false };
            List<bool> GraylistedSafeNonImages = new List<bool> { false, false, false, false };
            List<bool> BlacklistedExplicitNonImages = new List<bool> { false, false, false, false };
            List<bool> BlacklistedQuestionableNonImages = new List<bool> { false, false, false, false };
            List<bool> BlacklistedSafeNonImages = new List<bool> { false, false, false, false };
            #endregion

            #region Downloader try-statement
            try {
                #region initialization
                //Properties.Settings.Default.Log += "Tag downloader starting for tags " + tags + "\r\n";
                string OutputFolderTags = apiTools.ReplaceIllegalCharacters(DownloadInfo.Tags);
                if (DownloadInfo.UseMinimumScore) { // Add minimum score to folder name.
                    OutputFolderTags += " (scores " + (DownloadInfo.MinimumScore) + "+)";
                }

                // Set the saveTo.
                if (DownloadInfo.FromUrl) {
                    DownloadInfo.DownloadPath += "\\Pages\\" + OutputFolderTags + " (page " + DownloadInfo.PageNumber + ")";
                }
                else {
                    DownloadInfo.DownloadPath += "\\Tags\\" + OutputFolderTags;
                }

                // Start the buffer for the .nfo files.
                if (DownloadInfo.SaveInfo) {
                    if (DownloadInfo.UseMinimumScore) {
                        CleanInfoBuffer = "TAGS: " + DownloadInfo.Tags + "\r\n" +
                                  "MINIMUM SCORE: " + DownloadInfo.MinimumScore + "\r\n" +
                                  "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") +
                                  "\r\n\r\n";

                        BlacklistInfoBuffer = "TAGS: " + DownloadInfo.Tags + "\r\n" +
                                        "MINIMUM SCORE: " + DownloadInfo.MinimumScore + "\r\n" +
                                        "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\r\n" +
                                        "BLACKLISTED TAGS: " + string.Join(" ", DownloadInfo.Graylist) +
                                        "\r\n\r\n";
                    }
                    else {
                        CleanInfoBuffer = "TAGS: " + DownloadInfo.Tags + "\r\n" +
                                  "MINIMUM SCORE: n/a\r\n" +
                                  "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") +
                                  "\r\n\r\n";

                        BlacklistInfoBuffer = "TAGS: " + DownloadInfo.Tags + "\r\n" +
                                        "MINIMUM SCORE: n/a\r\n" +
                                        "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\r\n" +
                                        "BLACKLISTED TAGS: " + string.Join(" ", DownloadInfo.Graylist) +
                                        "\r\n\r\n";
                    }
                }

                // Add the minimum score to the search tags (if applicable).
                if (DownloadInfo.UseMinimumScore) {
                    if (DownloadInfo.Tags.Split(' ').Length < 6 && DownloadInfo.MinimumScoreAsTag) {
                        DownloadInfo.Tags += " score:>" + (DownloadInfo.MinimumScore - 1);
                    }
                }
                #endregion

                #region download pages & parse
                XmlDocument doc;
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

                for (int ApiPage = 0; ApiPage < CurrentPage; ApiPage++) {
                    if (DownloadInfo.PageLimit > 0 && CurrentPage > DownloadInfo.PageLimit) {
                        this.BeginInvoke(new MethodInvoker(() => {
                            Program.Log(LogAction.WriteToLog, "PageLimit reached, breaking parse loop.");
                        }));
                        break;
                    }

                    this.BeginInvoke(new MethodInvoker(() => {
                        Program.Log(LogAction.WriteToLog, "Downloading tag api page " + CurrentPage);
                        status.Text = "Downloading api page " + CurrentPage;
                    }));

                    // Get XML of page.
                    if (DownloadInfo.FromUrl) {
                        CurrentUrl = DownloadInfo.PageUrl.Replace("e621.net/posts", "e621.net/posts.json");
                    }
                    else {
                        CurrentUrl = string.Format(PageURL, DownloadInfo.Tags, CurrentPage);
                    }
                    CurrentXml = apiTools.GetJsonToXml(CurrentUrl);
                    if (apiTools.IsXmlDead(CurrentXml)) {
                        if (ApiPage < 1) {
                            throw new ApiReturnedNullOrEmptyException();
                        }
                        else {
                            break;
                        }
                    }

                    // Parse the XML file.
                    doc = new XmlDocument();
                    doc.LoadXml(CurrentXml);
                    xmlID = doc.DocumentElement.SelectNodes("/root/posts/item/id");
                    xmlMD5 = doc.DocumentElement.SelectNodes("/root/posts/item/file/md5");
                    xmlURL = doc.DocumentElement.SelectNodes("/root/posts/item/file/url");
                    xmlTagsGeneral = doc.DocumentElement.SelectNodes("/root/posts/item/tags/general");
                    xmlTagsSpecies = doc.DocumentElement.SelectNodes("/root/posts/item/tags/species");
                    xmlTagsCharacter = doc.DocumentElement.SelectNodes("/root/posts/item/tags/character");
                    xmlTagsCopyright = doc.DocumentElement.SelectNodes("/root/posts/item/tags/copyright");
                    xmlTagsArtist = doc.DocumentElement.SelectNodes("/root/posts/item/tags/artist");
                    xmlTagsInvalid = doc.DocumentElement.SelectNodes("/root/posts/item/tags/invalid");
                    xmlTagsLore = doc.DocumentElement.SelectNodes("/root/posts/item/tags/lore");
                    xmlTagsMeta = doc.DocumentElement.SelectNodes("/root/posts/item/tags/meta");
                    xmlTagsLocked = doc.DocumentElement.SelectNodes("/root/posts/item/locked_tags");
                    xmlScore = doc.DocumentElement.SelectNodes("/root/posts/item/score/total");
                    xmlScoreUp = doc.DocumentElement.SelectNodes("/root/posts/item/score/up");
                    xmlScoreDown = doc.DocumentElement.SelectNodes("/root/posts/item/score/down");
                    xmlFavCount = doc.DocumentElement.SelectNodes("/root/posts/item/fav_count");
                    xmlRating = doc.DocumentElement.SelectNodes("/root/posts/item/rating");
                    xmlAuthor = doc.DocumentElement.SelectNodes("/root/posts/item/uploader_id");
                    xmlDescription = doc.DocumentElement.SelectNodes("/root/posts/item/description");
                    xmlExt = doc.DocumentElement.SelectNodes("/root/posts/item/file/ext");
                    xmlDeleted = doc.DocumentElement.SelectNodes("/root/posts/item/flags/deleted");

                    if (xmlID.Count > 0) {
                        this.BeginInvoke(new MethodInvoker(() => {
                            Program.Log(LogAction.WriteToLog, "Parsing tag api page " + CurrentPage);
                            status.Text = "Parsing api page " + CurrentPage;
                        }));

                        // Begin parsing the XML for tag information per item.
                        for (int i = 0; i < xmlID.Count; i++) {

                            if (DownloadInfo.ImageLimit > 0 && DownloadInfo.ImageLimit == TotalCount) {
                                ImageLimitReached = true;
                                this.BeginInvoke(new MethodInvoker(() => {
                                    Program.Log(LogAction.WriteToLog, "ImageLimit reached, breaking parse loop.");
                                }));
                                break;
                            }

                            string rating = xmlRating[i].InnerText;         // Get the rating of the current file.
                            bool PostIsGraylisted = false;                  // Will determine if the file is graylisted.
                            bool PostIsBlacklisted = false;                 // Will determine if the file is blacklisted.
                            List<string> PostTags = new List<string>();     // Get the entire tag list of the file.
                            string FoundGraylistedTags = string.Empty;      // The buffer for the tags that are graylisted.
                            string ReadTags = string.Empty;                 // The buffer for the tags for the .nfo

                            switch (xmlRating[i].InnerText.ToLower()) {
                                case "e":
                                case "explicit":
                                    switch (DownloadInfo.SaveExplicit) {
                                        case false:
                                            continue;
                                    }
                                    rating = "Explicit";
                                    break;

                                case "q":
                                case "questionable":
                                    switch (DownloadInfo.SaveQuestionable) {
                                        case false:
                                            continue;
                                    }
                                    rating = "Questionable";
                                    break;

                                case "s":
                                case "safe":
                                    switch (DownloadInfo.SaveSafe) {
                                        case false:
                                            continue;
                                    }
                                    rating = "Safe";
                                    break;

                                default:
                                    rating = "Unknown";
                                    break;
                            }

                            #region Tag parsing + filtering
                            // Create new tag list to merge all the tag groups into one.
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

                            // Check the tags
                            for (int j = 0; j < PostTags.Count; j++) {
                                if (DownloadInfo.Blacklist.Length > 0) {
                                    for (int k = 0; k < DownloadInfo.Blacklist.Length; k++) {
                                        if (PostTags[j] == DownloadInfo.Blacklist[k]) {
                                            PostIsBlacklisted = true;
                                            break;
                                        }
                                    }
                                }

                                if (DownloadInfo.Graylist.Length > 0) {
                                    for (int k = 0; k < DownloadInfo.Graylist.Length; k++) {
                                        if (PostTags[j] == DownloadInfo.Graylist[k]) {
                                            PostIsGraylisted = true;
                                            FoundGraylistedTags += DownloadInfo.Graylist[k] + " ";
                                        }
                                    }
                                }
                            }

                            // Continue if graylisted or blacklisted
                            switch (PostIsBlacklisted && !DownloadInfo.SaveBlacklistedFiles) {
                                case true:
                                    BlacklistSkippedCount++;
                                    TotalCount++;
                                    continue;
                            }
                            switch (PostIsGraylisted && !DownloadInfo.SaveGraylistedFiles) {
                                case true:
                                    GraylistSkippedCount++;
                                    TotalCount++;
                                    continue;
                            }
                            switch (DownloadInfo.UseMinimumScore && Int32.Parse(xmlScore[i].InnerText) < DownloadInfo.MinimumScore) {
                                case true:
                                    TotalCount++;
                                    if (PostIsBlacklisted) {
                                        BlacklistSkippedCount++;
                                        continue;
                                    }
                                    else if (PostIsGraylisted) {
                                        GraylistSkippedCount++;
                                        continue;
                                    }
                                    else {
                                        CleanSkippedCount++;
                                        continue;
                                    }
                            }
                            #endregion

                            #region file name + e621 global blacklist fix
                            // File name artist for the schema
                            string fileNameArtist = "(none)";
                            bool useHardcodedFilter = false;
                            switch (string.IsNullOrWhiteSpace(DownloadInfo.UndesiredTags)) {
                                case true:
                                    useHardcodedFilter = true;
                                    break;
                            }

                            switch (xmlTagsArtist[i].ChildNodes.Count > 0) {
                                case true:
                                    for (int j = 0; j < xmlTagsArtist[i].ChildNodes.Count; j++) {
                                        switch (useHardcodedFilter) {
                                            case true:
                                                if (!UndesiredTags.isUndesiredHardcoded(xmlTagsArtist[i].ChildNodes[j].InnerText)) {
                                                    fileNameArtist = xmlTagsArtist[i].ChildNodes[j].InnerText;
                                                    break;
                                                }
                                                break;

                                            case false:
                                                if (!UndesiredTags.isUndesired(xmlTagsArtist[i].ChildNodes[j].InnerText)) {
                                                    fileNameArtist = xmlTagsArtist[i].ChildNodes[j].InnerText;
                                                    break;
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }

                            string NewFileName = DownloadInfo.FileNameSchema.Replace("%md5%", xmlMD5[i].InnerText)
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

                            // Check for null file url + fix for files that are auto blacklisted :)
                            string NewUrl = xmlURL[i].InnerText;
                            if (string.IsNullOrEmpty(NewUrl)) {
                                if (xmlDeleted[i].InnerText.ToLower() == "false") {
                                    NewUrl = apiTools.GetBlacklistedImageUrl(xmlMD5[i].InnerText, xmlExt[i].InnerText);
                                }
                            }
                            #endregion

                            #region counts + lists
                            // Add to the lists and counts
                            string NewPath = DownloadInfo.DownloadPath;

                            switch (xmlRating[i].InnerText.ToLower()) {
                                case "e":
                                case "explicit":
                                    if (DownloadInfo.SeparateRatings) {
                                        NewPath += "\\explicit";
                                    }
                                    if (PostIsBlacklisted) {
                                        NewPath += "\\blacklisted";
                                    }
                                    else if (PostIsGraylisted) {
                                        NewPath += "\\graylisted";
                                    }

                                    if (DownloadInfo.SeparateNonImages) {
                                        switch (xmlExt[i].InnerText.ToLower()) {
                                            case "gif":
                                                NewPath += "\\gif";
                                                break;
                                            case "apng":
                                                NewPath += "\\apng";
                                                break;
                                            case "webm":
                                                NewPath += "\\webm";
                                                break;
                                            case "swf":
                                                NewPath += "\\swf";
                                                break;
                                        }
                                    }

                                    NewPath += "\\" + NewFileName;

                                    if (File.Exists(NewPath)) {
                                        if (PostIsBlacklisted) {
                                            BlacklistExplicitExistCount++;
                                            BlacklistTotalExistCount++;
                                        }
                                        else if (PostIsGraylisted) {
                                            GraylistExplicitExistCount++;
                                            GraylistTotalExistCount++;
                                        }
                                        else {
                                            CleanExplicitExistCount++;
                                            CleanTotalExistCount++;
                                        }
                                        continue;
                                    }
                                    else {
                                        if (PostIsBlacklisted) {
                                            if (DownloadInfo.SeparateNonImages) {
                                                switch (xmlExt[i].InnerText.ToLower()) {
                                                    case "gif":
                                                        BlacklistedExplicitNonImages[0] = true;
                                                        break;
                                                    case "apng":
                                                        BlacklistedExplicitNonImages[1] = true;
                                                        break;
                                                    case "webm":
                                                        BlacklistedExplicitNonImages[2] = true;
                                                        break;
                                                    case "swf":
                                                        BlacklistedExplicitNonImages[3] = true;
                                                        break;
                                                }
                                            }

                                            BlacklistExplicitCount++;
                                            BlacklistTotalCount++;
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
                                                switch (xmlExt[i].InnerText.ToLower()) {
                                                    case "gif":
                                                        GraylistedExplicitNonImages[0] = true;
                                                        break;
                                                    case "apng":
                                                        GraylistedExplicitNonImages[1] = true;
                                                        break;
                                                    case "webm":
                                                        GraylistedExplicitNonImages[2] = true;
                                                        break;
                                                    case "swf":
                                                        GraylistedExplicitNonImages[3] = true;
                                                        break;
                                                }
                                            }

                                            GraylistExplicitCount++;
                                            GraylistTotalCount++;
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
                                                switch (xmlExt[i].InnerText.ToLower()) {
                                                    case "gif":
                                                        CleanedExplicitNonImages[0] = true;
                                                        break;
                                                    case "apng":
                                                        CleanedExplicitNonImages[1] = true;
                                                        break;
                                                    case "webm":
                                                        CleanedExplicitNonImages[2] = true;
                                                        break;
                                                    case "swf":
                                                        CleanedExplicitNonImages[3] = true;
                                                        break;
                                                }
                                            }
                                            CleanExplicitCount++;
                                            CleanTotalCount++;
                                            if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                CleanedExplicitURLs.Add(NewUrl);
                                                CleanedExplicitFileNames.Add(NewFileName);
                                                CleanedExplicitFilePaths.Add(NewPath);
                                            }
                                            else {
                                                CleanedURLs.Add(NewUrl);
                                                CleanedFileNames.Add(NewFileName);
                                                CleanedFilePaths.Add(NewPath);
                                            }
                                        }
                                    }
                                    break;

                                case "q":
                                case "questionable":
                                    if (DownloadInfo.SeparateRatings) {
                                        NewPath += "\\questionable";
                                    }
                                    if (PostIsBlacklisted) {
                                        NewPath += "\\blacklisted";
                                    }
                                    else if (PostIsGraylisted) {
                                        NewPath += "\\graylisted";
                                    }

                                    if (DownloadInfo.SeparateNonImages) {
                                        switch (xmlExt[i].InnerText.ToLower()) {
                                            case "gif":
                                                NewPath += "\\gif";
                                                break;
                                            case "apng":
                                                NewPath += "\\apng";
                                                break;
                                            case "webm":
                                                NewPath += "\\webm";
                                                break;
                                            case "swf":
                                                NewPath += "\\swf";
                                                break;
                                        }
                                    }

                                    NewPath += "\\" + NewFileName;

                                    if (File.Exists(NewPath)) {
                                        if (PostIsBlacklisted) {
                                            BlacklistQuestionableExistCount++;
                                            BlacklistTotalExistCount++;
                                        }
                                        else if (PostIsGraylisted) {
                                            GraylistQuestionableExistCount++;
                                            GraylistTotalExistCount++;
                                        }
                                        else {
                                            CleanQuestionableExistCount++;
                                            CleanTotalExistCount++;
                                        }
                                        continue;
                                    }
                                    else {
                                        if (PostIsBlacklisted) {
                                            if (DownloadInfo.SeparateNonImages) {
                                                switch (xmlExt[i].InnerText.ToLower()) {
                                                    case "gif":
                                                        BlacklistedQuestionableNonImages[0] = true;
                                                        break;
                                                    case "apng":
                                                        BlacklistedQuestionableNonImages[1] = true;
                                                        break;
                                                    case "webm":
                                                        BlacklistedQuestionableNonImages[2] = true;
                                                        break;
                                                    case "swf":
                                                        BlacklistedQuestionableNonImages[3] = true;
                                                        break;
                                                }
                                            }
                                            BlacklistQuestionableCount++;
                                            BlacklistTotalCount++;
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
                                                switch (xmlExt[i].InnerText.ToLower()) {
                                                    case "gif":
                                                        GraylistedQuestionableNonImages[0] = true;
                                                        break;
                                                    case "apng":
                                                        GraylistedQuestionableNonImages[1] = true;
                                                        break;
                                                    case "webm":
                                                        GraylistedQuestionableNonImages[2] = true;
                                                        break;
                                                    case "swf":
                                                        GraylistedQuestionableNonImages[3] = true;
                                                        break;
                                                }
                                            }
                                            GraylistQuestionableCount++;
                                            GraylistTotalCount++;
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
                                                switch (xmlExt[i].InnerText.ToLower()) {
                                                    case "gif":
                                                        CleanedQuestionableNonImages[0] = true;
                                                        break;
                                                    case "apng":
                                                        CleanedQuestionableNonImages[1] = true;
                                                        break;
                                                    case "webm":
                                                        CleanedQuestionableNonImages[2] = true;
                                                        break;
                                                    case "swf":
                                                        CleanedQuestionableNonImages[3] = true;
                                                        break;
                                                }
                                            }
                                            CleanQuestionableCount++;
                                            CleanTotalCount++;
                                            if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                CleanedQuestionableURLs.Add(NewUrl);
                                                CleanedQuestionableFileNames.Add(NewFileName);
                                                CleanedQuestionableFilePaths.Add(NewPath);
                                            }
                                            else {
                                                CleanedURLs.Add(NewUrl);
                                                CleanedFileNames.Add(NewFileName);
                                                CleanedFilePaths.Add(NewPath);
                                            }
                                        }
                                    }
                                    break;

                                case "s":
                                case "safe":
                                    if (DownloadInfo.SeparateRatings) {
                                        NewPath += "\\safe";
                                    }
                                    if (PostIsBlacklisted) {
                                        NewPath += "\\blacklisted";
                                    }
                                    else if (PostIsGraylisted) {
                                        NewPath += "\\graylisted";
                                    }

                                    if (DownloadInfo.SeparateNonImages) {
                                        switch (xmlExt[i].InnerText.ToLower()) {
                                            case "gif":
                                                NewPath += "\\gif";
                                                break;
                                            case "apng":
                                                NewPath += "\\apng";
                                                break;
                                            case "webm":
                                                NewPath += "\\webm";
                                                break;
                                            case "swf":
                                                NewPath += "\\swf";
                                                break;
                                        }
                                    }

                                    NewPath += "\\" + NewFileName;

                                    if (File.Exists(NewPath)) {
                                        if (PostIsBlacklisted) {
                                            BlacklistSafeExistCount++;
                                            BlacklistTotalExistCount++;
                                        }
                                        else if (PostIsGraylisted) {
                                            GraylistSafeExistCount++;
                                            GraylistTotalExistCount++;
                                        }
                                        else {
                                            CleanSafeExistCount++;
                                            CleanTotalExistCount++;
                                        }
                                        continue;
                                    }
                                    else {
                                        if (PostIsBlacklisted) {
                                            if (DownloadInfo.SeparateNonImages) {
                                                switch (xmlExt[i].InnerText.ToLower()) {
                                                    case "gif":
                                                        BlacklistedSafeNonImages[0] = true;
                                                        break;
                                                    case "apng":
                                                        BlacklistedSafeNonImages[1] = true;
                                                        break;
                                                    case "webm":
                                                        BlacklistedSafeNonImages[2] = true;
                                                        break;
                                                    case "swf":
                                                        BlacklistedSafeNonImages[3] = true;
                                                        break;
                                                }
                                            }
                                            BlacklistSafeCount++;
                                            BlacklistTotalCount++;
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
                                                switch (xmlExt[i].InnerText.ToLower()) {
                                                    case "gif":
                                                        GraylistedSafeNonImages[0] = true;
                                                        break;
                                                    case "apng":
                                                        GraylistedSafeNonImages[1] = true;
                                                        break;
                                                    case "webm":
                                                        GraylistedSafeNonImages[2] = true;
                                                        break;
                                                    case "swf":
                                                        GraylistedSafeNonImages[3] = true;
                                                        break;
                                                }
                                            }
                                            GraylistSafeCount++;
                                            GraylistTotalCount++;
                                            if (DownloadInfo.SaveGraylistedFiles) {
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
                                        }
                                        else {
                                            if (DownloadInfo.SeparateNonImages) {
                                                switch (xmlExt[i].InnerText.ToLower()) {
                                                    case "gif":
                                                        CleanedSafeNonImages[0] = true;
                                                        break;
                                                    case "apng":
                                                        CleanedSafeNonImages[1] = true;
                                                        break;
                                                    case "webm":
                                                        CleanedSafeNonImages[2] = true;
                                                        break;
                                                    case "swf":
                                                        CleanedSafeNonImages[3] = true;
                                                        break;
                                                }
                                            }
                                            CleanSafeCount++;
                                            CleanTotalCount++;
                                            if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                                                CleanedSafeURLs.Add(NewUrl);
                                                CleanedSafeFileNames.Add(NewFileName);
                                                CleanedSafeFilePaths.Add(NewPath);
                                            }
                                            else {
                                                CleanedURLs.Add(NewUrl);
                                                CleanedFileNames.Add(NewFileName);
                                                CleanedFilePaths.Add(NewPath);
                                            }
                                        }
                                    }
                                    break;
                            }
                            #endregion

                            #region description + .nfo files
                            // Start adding to the nfo buffer and URL lists
                            if (DownloadInfo.SaveInfo) {
                                // Set the description
                                string ImageDescription = " No description";
                                switch (string.IsNullOrWhiteSpace(xmlDescription[i].InnerText)) {
                                    case false:
                                        ImageDescription = xmlDescription[i].InnerText;
                                        break;
                                }

                                if (PostIsBlacklisted) {
                                    string[] Info = new string[] {
                                        xmlID[i].InnerText,         //0
                                        xmlMD5[i].InnerText,        // 1
                                        xmlID[i].InnerText,         // 2
                                        ReadTags,                   // 3
                                        FoundGraylistedTags,        // 4
                                        xmlScoreUp[i].InnerText,    // 5
                                        xmlScoreDown[i].InnerText,  // 6
                                        xmlScore[i].InnerText,      // 7
                                        rating,                     // 8
                                        ImageDescription            // 9
                                    };

                                    BlacklistInfoBuffer += string.Format(
                                        "POST {0}\r\n" +
                                        "    MD5: {1}\r\n" +
                                        "    URL: https://e621.net/posts/show/{2}\r\n" +
                                        "    TAGS {3}\r\n" +
                                        "    OFFENDING TAGS: {4}\r\n" +
                                        "    SCORE: Up {5}, Down {6}, Total {7}\r\n" +
                                        "    RATING: {8}\r\n" +
                                        "    DESCRIPTION: {9}\r\n\r\n", Info
                                    );
                                }
                                else if (PostIsGraylisted && DownloadInfo.SaveGraylistedFiles) {
                                    string[] Info = new string[] {
                                        xmlID[i].InnerText,         //0
                                        xmlMD5[i].InnerText,        // 1
                                        xmlID[i].InnerText,         // 2
                                        ReadTags,                   // 3
                                        FoundGraylistedTags,        // 4
                                        xmlScoreUp[i].InnerText,    // 5
                                        xmlScoreDown[i].InnerText,  // 6
                                        xmlScore[i].InnerText,      // 7
                                        rating,                     // 8
                                        ImageDescription            // 9
                                    };

                                    GraylistInfoBuffer += string.Format(
                                        "POST {0}\r\n" +
                                        "    MD5: {1}\r\n" +
                                        "    URL: https://e621.net/posts/show/{2}\r\n" +
                                        "    TAGS {3}\r\n" +
                                        "    OFFENDING TAGS: {4}\r\n" +
                                        "    SCORE: Up {5}, Down {6}, Total {7}\r\n" +
                                        "    RATING: {8}\r\n" +
                                        "    DESCRIPTION: {9}\r\n\r\n", Info
                                    );
                                }
                                else {
                                    CleanInfoBuffer += "POST " + xmlID[i].InnerText + ":\r\n" +
                                               "    MD5: " + xmlMD5[i].InnerText + "\r\n" +
                                               "    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\r\n" +
                                               "    TAGS:\r\n" + ReadTags + "\r\n" +
                                               "    SCORE: Up " + xmlScoreUp[i].InnerText + ", Down " + xmlScoreDown[i].InnerText + ", Total " + xmlScore[i].InnerText + "\r\n" +
                                               "    RATING: " + rating + "\r\n" +
                                               "    DESCRIPITON:" + ImageDescription +
                                               "\r\n\r\n";
                                }
                            }
                            #endregion
                        }

                        #region after page update totals
                        this.BeginInvoke(new MethodInvoker(() => {
                            string labelBuffer = "";

                            //if (DownloadInfo.SeparateRatings) {
                            object[] Counts = new object[] {
                                    CleanTotalCount, CleanExplicitCount, CleanQuestionableCount, CleanSafeCount,
                                    GraylistTotalCount, GraylistExplicitCount, GraylistQuestionableCount, GraylistSafeCount,
                                    BlacklistTotalCount, BlacklistExplicitCount, BlacklistQuestionableCount, BlacklistSafeCount,
                                    (CleanTotalCount + GraylistTotalCount + BlacklistTotalCount),

                                    CleanTotalExistCount, CleanExplicitExistCount, CleanQuestionableExistCount, CleanSafeExistCount,
                                    GraylistTotalExistCount, GraylistExplicitExistCount, GraylistQuestionableExistCount, GraylistSafeExistCount,
                                    BlacklistTotalExistCount, BlacklistExplicitExistCount, BlacklistQuestionableExistCount, BlacklistSafeExistCount,
                                    (CleanTotalExistCount + GraylistTotalExistCount + BlacklistTotalExistCount)
                                };

                            labelBuffer = string.Format("files: {0} ( {1} E | {2} Q | {3} S )\r\n" +
                                          "graylisted: {4} ( {5} E | {6} Q | {7} S )\r\n" +
                                          "blacklisted: {8} ( {9} E | {10} Q | {11} S )\r\n" +
                                          "total parsed: {12}\r\n\r\n" +

                                          "files that exist: {13} ( {14} E | {15} Q | {16} S )\r\n" +
                                          "graylisted that exist: {17} ( {18} E | {19} Q | {20} S )\r\n" +
                                          "blacklisted that exist: {21} ( {22} E | {23} Q | {24} S )\r\n" +
                                          "total exist: {25}", Counts);
                            //}
                            //else {
                            //    object[] Counts = new object[] {
                            //        CleanTotalCount,
                            //        GraylistTotalCount,
                            //        BlacklistTotalCount,
                            //        (CleanTotalCount + GraylistTotalCount + BlacklistTotalCount),

                            //        CleanTotalExistCount,
                            //        GraylistTotalExistCount,
                            //        BlacklistTotalExistCount,
                            //        (CleanTotalExistCount + GraylistTotalExistCount + BlacklistTotalExistCount),
                            //    };
                            //    labelBuffer = string.Format("files: {0}\r\n" +
                            //                  "graylisted: {1}\r\n" +
                            //                  "blacklisted: {2}\r\n" +
                            //                  "total parsed: {3}\r\n\r\n" +

                            //                  "files that exist: {4}\r\n" +
                            //                  "graylisted that exist: {5}\r\n" +
                            //                  "blacklisted that exist: {6}\r\n" +
                            //                  "total exist: {7}", Counts);
                            //}

                            lbBlacklist.Text = labelBuffer;
                        }));
                        #endregion

                    }

                    if (ImageLimitReached) {
                        break;
                    }

                    switch (!DownloadInfo.FromUrl && xmlID.Count == 320) {
                        case true:
                            CurrentPage++;
                            break;
                    }

                }

                if (CleanedURLs.Count == 0 && CleanedSafeURLs.Count == 0 && CleanedQuestionableURLs.Count == 0 && CleanedExplicitURLs.Count == 0 &&
                    GraylistedSafeURLs.Count == 0 && GraylistedQuestionableURLs.Count == 0 && GraylistedExplicitURLs.Count == 0) {
                    throw new NoFilesToDownloadException();
                }
                #endregion

                #region output logic
                // Create output folders
                this.BeginInvoke(new MethodInvoker(() => {
                    Program.Log(LogAction.WriteToLog, "Creating output folders (because System.Net doesn't do it for you...)");
                }));
                if (DownloadInfo.SeparateRatings) {
                    if (CleanedExplicitURLs.Count > 0) {
                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit");
                        if (DownloadInfo.SeparateNonImages) {
                            if (CleanedExplicitNonImages[0]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\gif");
                            }
                            if (CleanedExplicitNonImages[1]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\apng");
                            }
                            if (CleanedExplicitNonImages[2]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\webm");
                            }
                            if (CleanedExplicitNonImages[3]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\swf");
                            }
                        }
                    }
                    if (CleanedQuestionableURLs.Count > 0) {
                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable");
                        if (DownloadInfo.SeparateNonImages) {
                            if (CleanedQuestionableNonImages[0]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\gif");
                            }
                            if (CleanedQuestionableNonImages[1]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\apng");
                            }
                            if (CleanedQuestionableNonImages[2]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\webm");
                            }
                            if (CleanedQuestionableNonImages[3]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\swf");
                            }
                        }
                    }
                    if (CleanedSafeURLs.Count > 0) {
                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe");
                        if (DownloadInfo.SeparateNonImages) {
                            if (CleanedSafeNonImages[0]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\gif");
                            }
                            if (CleanedSafeNonImages[1]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\apng");
                            }
                            if (CleanedSafeNonImages[2]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\webm");
                            }
                            if (CleanedSafeNonImages[3]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\swf");
                            }
                        }
                    }

                    if (DownloadInfo.SaveGraylistedFiles) {
                        if (GraylistedExplicitURLs.Count > 0) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\graylisted");
                            if (DownloadInfo.SeparateNonImages) {
                                if (GraylistedExplicitNonImages[0]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\graylisted\\gif");
                                }
                                if (GraylistedExplicitNonImages[1]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\graylisted\\apng");
                                }
                                if (GraylistedExplicitNonImages[2]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\graylisted\\webm");
                                }
                                if (GraylistedExplicitNonImages[3]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\graylisted\\swf");
                                }
                            }
                        }
                        if (GraylistedQuestionableURLs.Count > 0) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\graylisted");
                            if (DownloadInfo.SeparateNonImages) {
                                if (GraylistedQuestionableNonImages[0]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\graylisted\\gif");
                                }
                                if (GraylistedQuestionableNonImages[1]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\graylisted\\apng");
                                }
                                if (GraylistedQuestionableNonImages[2]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\graylisted\\webm");
                                }
                                if (GraylistedQuestionableNonImages[3]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\graylisted\\swf");
                                }
                            }
                        }
                        if (GraylistedSafeURLs.Count > 0) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\graylisted");
                            if (DownloadInfo.SeparateNonImages) {
                                if (GraylistedSafeNonImages[0]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\graylisted\\gif");
                                }
                                if (GraylistedSafeNonImages[1]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\graylisted\\apng");
                                }
                                if (GraylistedSafeNonImages[2]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\graylisted\\webm");
                                }
                                if (GraylistedSafeNonImages[3]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\graylisted\\swf");
                                }
                            }
                        }
                    }

                    if (DownloadInfo.SaveBlacklistedFiles) {
                        if (BlacklistedExplicitURLs.Count > 0) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\blacklisted");
                            if (DownloadInfo.SeparateNonImages) {
                                if (BlacklistedExplicitNonImages[0]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\blacklisted\\gif");
                                }
                                if (BlacklistedExplicitNonImages[1]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\blacklisted\\apng");
                                }
                                if (BlacklistedExplicitNonImages[2]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\blacklisted\\webm");
                                }
                                if (BlacklistedExplicitNonImages[3]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\blacklisted\\swf");
                                }
                            }
                        }
                        if (BlacklistedQuestionableURLs.Count > 0) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\blacklisted");
                            if (DownloadInfo.SeparateNonImages) {
                                if (BlacklistedQuestionableNonImages[0]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\blacklisted\\gif");
                                }
                                if (BlacklistedQuestionableNonImages[1]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\blacklisted\\apng");
                                }
                                if (BlacklistedQuestionableNonImages[2]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\blacklisted\\webm");
                                }
                                if (BlacklistedQuestionableNonImages[3]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\blacklisted\\swf");
                                }
                            }
                        }
                        if (BlacklistedSafeURLs.Count > 0) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\blacklisted");
                            if (DownloadInfo.SeparateNonImages) {
                                if (BlacklistedSafeNonImages[0]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\blacklisted\\gif");
                                }
                                if (BlacklistedSafeNonImages[1]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\blacklisted\\apng");
                                }
                                if (BlacklistedSafeNonImages[2]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\blacklisted\\webm");
                                }
                                if (BlacklistedSafeNonImages[3]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\blacklisted\\swf");
                                }
                            }
                        }
                    }
                }
                else {
                    Directory.CreateDirectory(DownloadInfo.DownloadPath);
                    if (DownloadInfo.SeparateNonImages) {
                        if (CleanedFileNonImages[0]) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\gif");
                        }
                        if (CleanedFileNonImages[1]) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\apng");
                        }
                        if (CleanedFileNonImages[2]) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\webm");
                        }
                        if (CleanedFileNonImages[3]) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\swf");
                        }
                    }

                    if (DownloadInfo.SaveGraylistedFiles && GraylistedURLs.Count > 0) {
                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\graylisted");
                        if (DownloadInfo.SeparateNonImages) {
                            if (GraylistedFileNonImages[0]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\graylisted\\gif");
                            }
                            if (GraylistedFileNonImages[1]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\graylisted\\apng");
                            }
                            if (GraylistedFileNonImages[2]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\graylisted\\webm");
                            }
                            if (GraylistedFileNonImages[3]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\graylisted\\swf");
                            }
                        }
                    }


                    if (DownloadInfo.SaveBlacklistedFiles && BlacklistedURLs.Count > 0) {
                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted");
                        if (DownloadInfo.SeparateNonImages) {
                            if (BlacklistedFileNonImages[0]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted\\gif");
                            }
                            if (BlacklistedFileNonImages[1]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted\\apng");
                            }
                            if (BlacklistedFileNonImages[2]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted\\webm");
                            }
                            if (BlacklistedFileNonImages[3]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted\\swf");
                            }
                        }
                    }
                }
                #endregion

                #region pre-download logic
                // Update totals
                if (DownloadInfo.SeparateRatings) {
                    CleanTotalCount = CleanExplicitCount + CleanQuestionableCount + CleanSafeCount;
                    GraylistTotalCount = GraylistExplicitCount + GraylistQuestionableCount + GraylistSafeCount;
                }

                // Save the info files from the buffer
                if (DownloadInfo.SaveInfo) {
                    this.BeginInvoke(new MethodInvoker(() => {
                        Program.Log(LogAction.WriteToLog, "Saving infos.");
                    }));
                    if (CleanTotalCount > 0) {
                        CleanInfoBuffer.TrimEnd('\r').TrimEnd('\n');
                        File.WriteAllText(DownloadInfo.DownloadPath + "\\tags.nfo", CleanInfoBuffer, Encoding.UTF8);
                    }

                    if (DownloadInfo.SaveGraylistedFiles && GraylistTotalCount > 0) {
                        GraylistInfoBuffer.TrimEnd('\r').TrimEnd('\n');
                        File.WriteAllText(DownloadInfo.DownloadPath + "\\tags.graylisted.nfo", GraylistInfoBuffer, Encoding.UTF8);
                    }

                    if (DownloadInfo.SaveBlacklistedFiles && BlacklistTotalCount > 0) {
                        BlacklistInfoBuffer.TrimEnd('\r').TrimEnd('\n');
                        File.WriteAllText(DownloadInfo.DownloadPath + "\\tags.blacklisted.nfo", BlacklistInfoBuffer, Encoding.UTF8);
                    }
                }

                // Set the progressbar style.
                this.BeginInvoke(new MethodInvoker(() => {
                    string labelBuffer = "";

                    //if (DownloadInfo.SeparateRatings) {
                    object[] Counts = new object[] {
                                    CleanTotalCount, CleanExplicitCount, CleanQuestionableCount, CleanSafeCount,
                                    GraylistTotalCount, GraylistExplicitCount, GraylistQuestionableCount, GraylistSafeCount,
                                    BlacklistTotalCount, BlacklistExplicitCount, BlacklistQuestionableCount, BlacklistSafeCount,
                                    (CleanTotalCount + GraylistTotalCount + BlacklistTotalCount),

                                    CleanTotalExistCount, CleanExplicitExistCount, CleanQuestionableExistCount, CleanSafeExistCount,
                                    GraylistTotalExistCount, GraylistExplicitExistCount, GraylistQuestionableExistCount, GraylistSafeExistCount,
                                    BlacklistTotalExistCount, BlacklistExplicitExistCount, BlacklistQuestionableExistCount, BlacklistSafeExistCount,
                                    (CleanTotalExistCount + GraylistTotalExistCount + BlacklistTotalExistCount)
                                };

                    labelBuffer = string.Format("files: {0} ( {1} E | {2} Q | {3} S )\r\n" +
                                                "graylisted: {4} ( {5} E | {6} Q | {7} S )\r\n" +
                                                "blacklisted: {8} ( {9} E | {10} Q | {11} S )\r\n" +
                                                "total parsed: {12}\r\n\r\n" +

                                                "files that exist: {13} ( {14} E | {15} Q | {16} S )\r\n" +
                                                "graylisted that exist: {17} ( {18} E | {19} Q | {20} S )\r\n" +
                                                "blacklisted that exist: {21} ( {22} E | {23} Q | {24} S )\r\n" +
                                                "total exist: {25}", Counts);
                    //}
                    //else {
                    //    object[] Counts = new object[] {
                    //                CleanTotalCount,
                    //                GraylistTotalCount,
                    //                BlacklistTotalCount,
                    //                (CleanTotalCount + GraylistTotalCount + BlacklistTotalCount),

                    //                CleanTotalExistCount,
                    //                GraylistTotalExistCount,
                    //                BlacklistTotalExistCount,
                    //                (CleanTotalExistCount + GraylistTotalExistCount + BlacklistTotalExistCount),
                    //            };
                    //    labelBuffer = string.Format("files: {0}\r\n" +
                    //                                "graylisted: {1}\r\n" +
                    //                                "blacklisted: {2}\r\n" +
                    //                                "total parsed: {3}\r\n\r\n" +

                    //                                "files that exist: {4}\r\n" +
                    //                                "graylisted that exist: {5}\r\n" +
                    //                                "blacklisted that exist: {6}\r\n" +
                    //                                "total exist: {7}", Counts);
                    //}

                    lbBlacklist.Text = labelBuffer;

                    pbTotalStatus.Maximum = CleanTotalCount;
                    if (DownloadInfo.SaveGraylistedFiles) {
                        pbTotalStatus.Maximum += GraylistTotalCount;
                    }
                    if (DownloadInfo.SaveBlacklistedFiles) {
                        pbTotalStatus.Maximum += BlacklistTotalCount;
                    }

                    pbDownloadStatus.Style = ProgressBarStyle.Blocks;

                }));
                #endregion

                // Enable using old logic by disabling UseNewDownloadLogic
                #region download the images

                // Start the download
                using (Controls.ExtendedWebClient wc = new Controls.ExtendedWebClient()) {
                    wc.DownloadProgressChanged += (s, e) => {
                        this.BeginInvoke(new MethodInvoker(() => {
                            pbDownloadStatus.Value = e.ProgressPercentage;
                            switch (pbDownloadStatus.Value) {
                                case 100:
                                    pbDownloadStatus.Value--;
                                    pbDownloadStatus.Value++;
                                    break;
                                default:
                                    pbDownloadStatus.Value++;
                                    pbDownloadStatus.Value--;
                                    pbDownloadStatus.Value++;
                                    break;
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

                                // protect from overflow
                                if (pbTotalStatus.Value < pbTotalStatus.Maximum) {
                                    pbTotalStatus.Value++;
                                }
                                else {
                                    lbRemoved.Visible = true;
                                }
                            }));
                            Monitor.Pulse(e.UserState);
                        }
                    };

                    wc.Proxy = WebRequest.GetSystemWebProxy();
                    wc.Headers.Add("user-agent", Program.UserAgent);
                    wc.Method = "GET";

                    #region download (new logic, though breaks for some reason)
                    if (UseNewDownloadLogic) {
                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                            if (CleanedExplicitURLs.Count > 0) {
                                this.BeginInvoke((MethodInvoker)delegate() {
                                    Program.Log(LogAction.WriteToLog, "Downloading explicit files.");
                                });
                                for (int CurrentFile = 0; CurrentFile < CleanedExplicitURLs.Count; CurrentFile++) {
                                    this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading explicit file " + (CurrentFile + 1) + " of " + (CleanExplicitCount)));
                                    if (!string.IsNullOrEmpty(CleanedExplicitURLs[CurrentFile]) && !File.Exists(CleanedExplicitFilePaths[CurrentFile])) {
                                        this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + CleanedExplicitFileNames[CurrentFile]));
                                        await wc.DownloadFileTaskAsync(new Uri(CleanedExplicitURLs[CurrentFile]), CleanedExplicitFilePaths[CurrentFile]);
                                    }
                                }
                            }

                            if (CleanedQuestionableURLs.Count > 0) {
                                this.BeginInvoke((MethodInvoker)delegate() {
                                    Program.Log(LogAction.WriteToLog, "Downloading questionable files.");
                                });
                                for (int CurrentFile = 0; CurrentFile < CleanedQuestionableURLs.Count; CurrentFile++) {
                                    this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading questionable file " + (CurrentFile + 1) + " of " + (CleanQuestionableCount)));
                                    if (!string.IsNullOrEmpty(CleanedQuestionableURLs[CurrentFile]) && !File.Exists(CleanedQuestionableFilePaths[CurrentFile])) {
                                        this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + CleanedQuestionableFileNames[CurrentFile]));
                                        await wc.DownloadFileTaskAsync(new Uri(CleanedQuestionableURLs[CurrentFile]), CleanedQuestionableFilePaths[CurrentFile]);
                                    }
                                }
                            }

                            if (CleanedSafeURLs.Count > 0) {
                                this.BeginInvoke((MethodInvoker)delegate() {
                                    Program.Log(LogAction.WriteToLog, "Downloading safe files.");
                                });
                                for (int CurrentFile = 0; CurrentFile < CleanedSafeURLs.Count; CurrentFile++) {
                                    this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading safe file " + (CurrentFile + 1) + " of " + (CleanSafeCount)));
                                    if (!string.IsNullOrEmpty(CleanedSafeURLs[CurrentFile]) && !File.Exists(CleanedSafeFilePaths[CurrentFile])) {
                                        this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + CleanedSafeFileNames[CurrentFile]));
                                        await wc.DownloadFileTaskAsync(new Uri(CleanedSafeURLs[CurrentFile]), CleanedSafeFilePaths[CurrentFile]);
                                    }
                                }
                            }

                            if (DownloadInfo.SaveGraylistedFiles) {
                                if (GraylistedExplicitURLs.Count > 0) {
                                    this.BeginInvoke((MethodInvoker)delegate() {
                                        Program.Log(LogAction.WriteToLog, "Downloading graylisted explicit files.");
                                    });
                                    for (int CurrentFile = 0; CurrentFile < GraylistedExplicitURLs.Count; CurrentFile++) {
                                        this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading graylisted (e) file " + (CurrentFile + 1) + " of " + (GraylistExplicitCount)));
                                        if (!string.IsNullOrEmpty(GraylistedExplicitURLs[CurrentFile]) && !File.Exists(GraylistedExplicitFilePaths[CurrentFile])) {
                                            this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + GraylistedExplicitFileNames[CurrentFile]));
                                            await wc.DownloadFileTaskAsync(new Uri(GraylistedExplicitURLs[CurrentFile]), GraylistedExplicitFilePaths[CurrentFile]);
                                        }
                                    }
                                }

                                if (GraylistedQuestionableURLs.Count > 0) {
                                    this.BeginInvoke((MethodInvoker)delegate() {
                                        Program.Log(LogAction.WriteToLog, "Downloading graylisted questionable files.");
                                    });
                                    for (int CurrentFile = 0; CurrentFile < GraylistedQuestionableURLs.Count; CurrentFile++) {
                                        this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading graylisted (q) file " + (CurrentFile + 1) + " of " + (GraylistQuestionableCount)));
                                        if (!string.IsNullOrEmpty(GraylistedQuestionableURLs[CurrentFile]) && !File.Exists(GraylistedQuestionableFilePaths[CurrentFile])) {
                                            this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + GraylistedQuestionableFileNames[CurrentFile]));
                                            await wc.DownloadFileTaskAsync(new Uri(GraylistedQuestionableURLs[CurrentFile]), GraylistedQuestionableFilePaths[CurrentFile]);
                                        }
                                    }
                                }

                                if (GraylistedSafeURLs.Count > 0) {
                                    this.BeginInvoke((MethodInvoker)delegate() {
                                        Program.Log(LogAction.WriteToLog, "Downloading graylisted safe files.");
                                    });
                                    for (int CurrentFile = 0; CurrentFile < GraylistedSafeURLs.Count; CurrentFile++) {
                                        this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading graylisted (s) file " + (CurrentFile + 1) + " of " + (GraylistSafeCount)));
                                        if (!string.IsNullOrEmpty(GraylistedSafeURLs[CurrentFile]) && !File.Exists(GraylistedSafeFilePaths[CurrentFile])) {
                                            this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + GraylistedSafeFileNames[CurrentFile]));
                                            await wc.DownloadFileTaskAsync(new Uri(GraylistedSafeURLs[CurrentFile]), GraylistedSafeFilePaths[CurrentFile]);
                                        }
                                    }
                                }
                            }

                            if (DownloadInfo.SaveBlacklistedFiles) {
                                if (BlacklistedExplicitURLs.Count > 0) {
                                    this.BeginInvoke((MethodInvoker)delegate() {
                                        Program.Log(LogAction.WriteToLog, "Downloading blacklisted explicit files.");
                                    });
                                    for (int CurrentFile = 0; CurrentFile < BlacklistedExplicitURLs.Count; CurrentFile++) {
                                        this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (e) file " + (CurrentFile + 1) + " of " + (BlacklistExplicitCount)));
                                        if (!string.IsNullOrEmpty(BlacklistedExplicitURLs[CurrentFile]) && !File.Exists(BlacklistedExplicitFilePaths[CurrentFile])) {
                                            this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + BlacklistedExplicitFileNames[CurrentFile]));
                                            await wc.DownloadFileTaskAsync(new Uri(BlacklistedExplicitURLs[CurrentFile]), BlacklistedExplicitFilePaths[CurrentFile]);
                                        }
                                    }
                                }

                                if (BlacklistedQuestionableURLs.Count > 0) {
                                    this.BeginInvoke((MethodInvoker)delegate() {
                                        Program.Log(LogAction.WriteToLog, "Downloading blacklisted questionable files.");
                                    });
                                    for (int CurrentFile = 0; CurrentFile < BlacklistedQuestionableURLs.Count; CurrentFile++) {
                                        this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (q) file " + (CurrentFile + 1) + " of " + (BlacklistQuestionableCount)));
                                        if (!string.IsNullOrEmpty(BlacklistedQuestionableURLs[CurrentFile]) && !File.Exists(BlacklistedQuestionableFilePaths[CurrentFile])) {
                                            this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + BlacklistedQuestionableFileNames[CurrentFile]));
                                            await wc.DownloadFileTaskAsync(new Uri(BlacklistedQuestionableURLs[CurrentFile]), BlacklistedQuestionableFilePaths[CurrentFile]);
                                        }
                                    }
                                }

                                if (BlacklistedSafeURLs.Count > 0) {
                                    this.BeginInvoke((MethodInvoker)delegate() {
                                        Program.Log(LogAction.WriteToLog, "Downloading blacklisted safe files.");
                                    });
                                    for (int CurrentFile = 0; CurrentFile < BlacklistedSafeURLs.Count; CurrentFile++) {
                                        this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (s) file " + (CurrentFile + 1) + " of " + (BlacklistSafeCount)));
                                        if (!string.IsNullOrEmpty(BlacklistedSafeURLs[CurrentFile]) && !File.Exists(BlacklistedSafeFilePaths[CurrentFile])) {
                                            this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + BlacklistedSafeFileNames[CurrentFile]));
                                            await wc.DownloadFileTaskAsync(new Uri(BlacklistedSafeURLs[CurrentFile]), BlacklistedSafeFilePaths[CurrentFile]);
                                        }
                                    }
                                }
                            }
                        }
                        else {
                            if (CleanedURLs.Count > 0) {
                                this.BeginInvoke((MethodInvoker)delegate() {
                                    Program.Log(LogAction.WriteToLog, "Downloading clean files.");
                                });

                                for (int CurrentFile = 0; CurrentFile < CleanedURLs.Count; CurrentFile++) {
                                    this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading file " + (CurrentFile + 1) + " of " + (CleanTotalCount)));
                                    if (!string.IsNullOrEmpty(CleanedURLs[CurrentFile]) && !File.Exists(CleanedFilePaths[CurrentFile])) {
                                        this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + CleanedFileNames[CurrentFile]));
                                        await wc.DownloadFileTaskAsync(new Uri(CleanedURLs[CurrentFile]), CleanedFilePaths[CurrentFile]);
                                    }
                                }
                            }

                            if (DownloadInfo.SaveGraylistedFiles && GraylistedURLs.Count > 0) {
                                this.BeginInvoke((MethodInvoker)delegate() {
                                    Program.Log(LogAction.WriteToLog, "Downloading graylisted files.");
                                });

                                for (int CurrentFile = 0; CurrentFile < GraylistedURLs.Count; CurrentFile++) {
                                    this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading graylisted file " + (CurrentFile + 1) + " of " + (GraylistTotalCount)));
                                    if (!string.IsNullOrEmpty(GraylistedURLs[CurrentFile]) && !File.Exists(GraylistedFilePaths[CurrentFile])) {
                                        this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + GraylistedFileNames[CurrentFile]));
                                        await wc.DownloadFileTaskAsync(new Uri(GraylistedURLs[CurrentFile]), GraylistedFilePaths[CurrentFile]);
                                    }
                                }
                            }

                            if (DownloadInfo.SaveBlacklistedFiles && BlacklistedURLs.Count > 0) {
                                this.BeginInvoke((MethodInvoker)delegate() {
                                    Program.Log(LogAction.WriteToLog, "Downloading blacklisted files.");
                                });

                                for (int CurrentFile = 0; CurrentFile < BlacklistedURLs.Count; CurrentFile++) {
                                    this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted file " + (CurrentFile + 1) + " of " + (BlacklistTotalCount)));
                                    if (!string.IsNullOrEmpty(BlacklistedURLs[CurrentFile]) && !File.Exists(BlacklistedFilePaths[CurrentFile])) {
                                        this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + BlacklistedFileNames[CurrentFile]));
                                        await wc.DownloadFileTaskAsync(new Uri(BlacklistedURLs[CurrentFile]), BlacklistedFilePaths[CurrentFile]);
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Old download logic (doesnt break)
                    // UseNewDownloadLogic = false;
                    else {
                        Object sync = new Object();
                        if (DownloadInfo.SeparateRatings && !DownloadInfo.DownloadNewestToOldest) {
                            if (CleanedExplicitURLs.Count > 0) {
                                this.BeginInvoke((MethodInvoker)delegate() {
                                    Program.Log(LogAction.WriteToLog, "Downloading explicit files.");
                                });
                                for (int y = 0; y < CleanedExplicitURLs.Count; y++) {
                                    if (string.IsNullOrEmpty(CleanedExplicitURLs[y])) { continue; }
                                    CurrentUrl = CleanedExplicitURLs[y];

                                    string fileName = CleanedExplicitFileNames[y];
                                    string outputDir = DownloadInfo.DownloadPath + "\\explicit\\";
                                    switch (DownloadInfo.SeparateNonImages) {
                                        case true:
                                            if (fileName.EndsWith("gif")) {
                                                outputDir += "gif\\";
                                            }
                                            else if (fileName.EndsWith("apng")) {
                                                outputDir += "apng\\";
                                            }
                                            else if (fileName.EndsWith("webm")) {
                                                outputDir += "webm\\";
                                            }
                                            else if (fileName.EndsWith("swf")) {
                                                outputDir += "swf\\";
                                            }
                                            break;
                                    }
                                    outputDir += fileName;
                                    if (!File.Exists(outputDir)) {
                                        this.BeginInvoke((MethodInvoker)delegate() {
                                            status.Text = "Downloading " + fileName;
                                            lbFile.Text = "Downloading explicit file " + (y + 1) + " of " + (CleanExplicitCount);
                                        });
                                        lock (sync) {
                                            wc.DownloadFileAsync(new Uri(CurrentUrl), outputDir, sync);
                                            Monitor.Wait(sync);
                                        }
                                    }
                                    else {
                                        this.Invoke((MethodInvoker)(() => status.Text = fileName + " already exists"));
                                    }
                                }
                            }

                            if (CleanedQuestionableURLs.Count > 0) {
                                this.BeginInvoke((MethodInvoker)delegate() {
                                    Program.Log(LogAction.WriteToLog, "Downloading questionable files.");
                                });

                                for (int y = 0; y < CleanedQuestionableURLs.Count; y++) {
                                    if (string.IsNullOrEmpty(CleanedQuestionableURLs[y])) { continue; }
                                    CurrentUrl = CleanedQuestionableURLs[y];
                                    this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading questionable file " + (y + 1) + " of " + (CleanQuestionableCount)));

                                    string fileName = CleanedQuestionableFileNames[y];
                                    string outputDir = DownloadInfo.DownloadPath + "\\questionable\\";
                                    switch (DownloadInfo.SeparateNonImages) {
                                        case true:
                                            if (fileName.EndsWith("gif")) {
                                                outputDir += "gif\\";
                                            }
                                            else if (fileName.EndsWith("apng")) {
                                                outputDir += "apng\\";
                                            }
                                            else if (fileName.EndsWith("webm")) {
                                                outputDir += "webm\\";
                                            }
                                            else if (fileName.EndsWith("swf")) {
                                                outputDir += "swf\\";
                                            }
                                            break;
                                    }
                                    outputDir += fileName;
                                    if (!File.Exists(outputDir)) {
                                        this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + fileName));
                                        lock (sync) {
                                            wc.DownloadFileAsync(new Uri(CurrentUrl), outputDir, sync);
                                            Monitor.Wait(sync);
                                        }
                                    }
                                    else {
                                        this.Invoke((MethodInvoker)(() => status.Text = fileName + " already exists"));
                                    }
                                }
                            }

                            if (CleanedSafeURLs.Count > 0) {
                                this.BeginInvoke((MethodInvoker)delegate() {
                                    Program.Log(LogAction.WriteToLog, "Downloading safe files.");
                                });

                                for (int y = 0; y < CleanedSafeURLs.Count; y++) {
                                    if (string.IsNullOrEmpty(CleanedSafeURLs[y])) { continue; }
                                    CurrentUrl = CleanedSafeURLs[y];
                                    this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading safe file " + (y + 1) + " of " + (CleanSafeCount)));

                                    string fileName = CleanedSafeFileNames[y];
                                    string outputDir = DownloadInfo.DownloadPath + "\\safe\\";
                                    switch (DownloadInfo.SeparateNonImages) {
                                        case true:
                                            if (fileName.EndsWith("gif")) {
                                                outputDir += "gif\\";
                                            }
                                            else if (fileName.EndsWith("apng")) {
                                                outputDir += "apng\\";
                                            }
                                            else if (fileName.EndsWith("webm")) {
                                                outputDir += "webm\\";
                                            }
                                            else if (fileName.EndsWith("swf")) {
                                                outputDir += "swf\\";
                                            }
                                            break;
                                    }
                                    outputDir += fileName;
                                    if (!File.Exists(outputDir)) {
                                        this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + fileName));
                                        lock (sync) {
                                            wc.DownloadFileAsync(new Uri(CurrentUrl), outputDir, sync);
                                            Monitor.Wait(sync);
                                        }
                                    }
                                    else {
                                        this.Invoke((MethodInvoker)(() => status.Text = fileName + " already exists"));
                                    }
                                }
                            }

                            if (DownloadInfo.SaveGraylistedFiles) {
                                if (GraylistedExplicitURLs.Count > 0) {
                                    this.BeginInvoke((MethodInvoker)delegate() {
                                        Program.Log(LogAction.WriteToLog, "Downloading graylisted explicit files.");
                                    });

                                    for (int y = 0; y < GraylistedExplicitURLs.Count; y++) {
                                        if (string.IsNullOrEmpty(GraylistedExplicitURLs[y])) { continue; }
                                        CurrentUrl = GraylistedExplicitURLs[y];
                                        this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading graylisted (e) file " + (y + 1) + " of " + (GraylistExplicitCount)));

                                        string fileName = GraylistedExplicitFileNames[y];
                                        string outputDir = DownloadInfo.DownloadPath + "\\explicit\\graylisted\\";
                                        switch (DownloadInfo.SeparateNonImages) {
                                            case true:
                                                if (fileName.EndsWith("gif")) {
                                                    outputDir += "gif\\";
                                                }
                                                else if (fileName.EndsWith("apng")) {
                                                    outputDir += "apng\\";
                                                }
                                                else if (fileName.EndsWith("webm")) {
                                                    outputDir += "webm\\";
                                                }
                                                else if (fileName.EndsWith("swf")) {
                                                    outputDir += "swf\\";
                                                }
                                                break;
                                        }
                                        outputDir += fileName;
                                        if (!File.Exists(outputDir)) {
                                            this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + fileName));
                                            lock (sync) {
                                                wc.DownloadFileAsync(new Uri(CurrentUrl), outputDir, sync);
                                                Monitor.Wait(sync);
                                            }
                                        }
                                        else {
                                            this.Invoke((MethodInvoker)(() => status.Text = fileName + " already exists"));
                                        }
                                    }
                                }

                                if (GraylistedQuestionableURLs.Count > 0) {
                                    this.BeginInvoke((MethodInvoker)delegate() {
                                        Program.Log(LogAction.WriteToLog, "Downloading graylisted questionable files.");
                                    });

                                    for (int y = 0; y < GraylistedQuestionableURLs.Count; y++) {
                                        if (string.IsNullOrEmpty(GraylistedQuestionableURLs[y])) { continue; }
                                        CurrentUrl = GraylistedQuestionableURLs[y];
                                        this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading graylisted (q) file " + (y + 1) + " of " + (GraylistQuestionableCount)));

                                        string fileName = GraylistedQuestionableFileNames[y];
                                        string outputDir = DownloadInfo.DownloadPath + "\\questionable\\graylisted\\";
                                        switch (DownloadInfo.SeparateNonImages) {
                                            case true:
                                                if (fileName.EndsWith("gif")) {
                                                    outputDir += "gif\\";
                                                }
                                                else if (fileName.EndsWith("apng")) {
                                                    outputDir += "apng\\";
                                                }
                                                else if (fileName.EndsWith("webm")) {
                                                    outputDir += "webm\\";
                                                }
                                                else if (fileName.EndsWith("swf")) {
                                                    outputDir += "swf\\";
                                                }
                                                break;
                                        }
                                        outputDir += fileName;
                                        if (!File.Exists(outputDir)) {
                                            this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + fileName));
                                            lock (sync) {
                                                wc.DownloadFileAsync(new Uri(CurrentUrl), outputDir, sync);
                                                Monitor.Wait(sync);
                                            }
                                        }
                                        else {
                                            this.Invoke((MethodInvoker)(() => status.Text = fileName + " already exists"));
                                        }
                                    }
                                }

                                if (GraylistedSafeURLs.Count > 0) {
                                    this.BeginInvoke((MethodInvoker)delegate() {
                                        Program.Log(LogAction.WriteToLog, "Downloading graylisted safe files.");
                                    });

                                    for (int y = 0; y < GraylistedSafeURLs.Count; y++) {
                                        if (string.IsNullOrEmpty(GraylistedSafeURLs[y])) { continue; }
                                        CurrentUrl = GraylistedSafeURLs[y];
                                        this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading graylisted (s) file " + (y + 1) + " of " + (GraylistSafeCount)));

                                        string fileName = GraylistedSafeFileNames[y];
                                        string outputDir = DownloadInfo.DownloadPath + "\\safe\\graylisted\\";
                                        switch (DownloadInfo.SeparateNonImages) {
                                            case true:
                                                if (fileName.EndsWith("gif")) {
                                                    outputDir += "gif\\";
                                                }
                                                else if (fileName.EndsWith("apng")) {
                                                    outputDir += "apng\\";
                                                }
                                                else if (fileName.EndsWith("webm")) {
                                                    outputDir += "webm\\";
                                                }
                                                else if (fileName.EndsWith("swf")) {
                                                    outputDir += "swf\\";
                                                }
                                                break;
                                        }
                                        outputDir += fileName;
                                        if (!File.Exists(outputDir)) {
                                            this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + fileName));
                                            lock (sync) {
                                                wc.DownloadFileAsync(new Uri(CurrentUrl), outputDir, sync);
                                                Monitor.Wait(sync);
                                            }
                                        }
                                        else {
                                            this.Invoke((MethodInvoker)(() => status.Text = fileName + " already exists"));
                                        }
                                    }
                                }
                            }

                            if (DownloadInfo.SaveBlacklistedFiles) {
                                if (BlacklistedExplicitURLs.Count > 0) {
                                    this.BeginInvoke((MethodInvoker)delegate() {
                                        Program.Log(LogAction.WriteToLog, "Downloading Blacklisted explicit files.");
                                    });

                                    for (int y = 0; y < BlacklistedExplicitURLs.Count; y++) {
                                        if (string.IsNullOrEmpty(BlacklistedExplicitURLs[y])) { continue; }
                                        CurrentUrl = BlacklistedExplicitURLs[y];
                                        this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (e) file " + (y + 1) + " of " + (BlacklistExplicitCount)));

                                        string fileName = BlacklistedExplicitFileNames[y];
                                        string outputDir = DownloadInfo.DownloadPath + "\\explicit\\blacklisted\\";
                                        switch (DownloadInfo.SeparateNonImages) {
                                            case true:
                                                if (fileName.EndsWith("gif")) {
                                                    outputDir += "gif\\";
                                                }
                                                else if (fileName.EndsWith("apng")) {
                                                    outputDir += "apng\\";
                                                }
                                                else if (fileName.EndsWith("webm")) {
                                                    outputDir += "webm\\";
                                                }
                                                else if (fileName.EndsWith("swf")) {
                                                    outputDir += "swf\\";
                                                }
                                                break;
                                        }
                                        outputDir += fileName;
                                        if (!File.Exists(outputDir)) {
                                            this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + fileName));
                                            lock (sync) {
                                                wc.DownloadFileAsync(new Uri(CurrentUrl), outputDir, sync);
                                                Monitor.Wait(sync);
                                            }
                                        }
                                        else {
                                            this.Invoke((MethodInvoker)(() => status.Text = fileName + " already exists"));
                                        }
                                    }
                                }

                                if (BlacklistedQuestionableURLs.Count > 0) {
                                    this.BeginInvoke((MethodInvoker)delegate() {
                                        Program.Log(LogAction.WriteToLog, "Downloading Blacklisted questionable files.");
                                    });

                                    for (int y = 0; y < BlacklistedQuestionableURLs.Count; y++) {
                                        if (string.IsNullOrEmpty(BlacklistedQuestionableURLs[y])) { continue; }
                                        CurrentUrl = BlacklistedQuestionableURLs[y];
                                        this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (q) file " + (y + 1) + " of " + (BlacklistQuestionableCount)));

                                        string fileName = BlacklistedQuestionableFileNames[y];
                                        string outputDir = DownloadInfo.DownloadPath + "\\questionable\\blacklisted\\";
                                        switch (DownloadInfo.SeparateNonImages) {
                                            case true:
                                                if (fileName.EndsWith("gif")) {
                                                    outputDir += "gif\\";
                                                }
                                                else if (fileName.EndsWith("apng")) {
                                                    outputDir += "apng\\";
                                                }
                                                else if (fileName.EndsWith("webm")) {
                                                    outputDir += "webm\\";
                                                }
                                                else if (fileName.EndsWith("swf")) {
                                                    outputDir += "swf\\";
                                                }
                                                break;
                                        }
                                        outputDir += fileName;
                                        if (!File.Exists(outputDir)) {
                                            this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + fileName));
                                            lock (sync) {
                                                wc.DownloadFileAsync(new Uri(CurrentUrl), outputDir, sync);
                                                Monitor.Wait(sync);
                                            }
                                        }
                                        else {
                                            this.Invoke((MethodInvoker)(() => status.Text = fileName + " already exists"));
                                        }
                                    }
                                }

                                if (BlacklistedSafeURLs.Count > 0) {
                                    this.BeginInvoke((MethodInvoker)delegate() {
                                        Program.Log(LogAction.WriteToLog, "Downloading Blacklisted safe files.");
                                    });

                                    for (int y = 0; y < BlacklistedSafeURLs.Count; y++) {
                                        if (string.IsNullOrEmpty(BlacklistedSafeURLs[y])) { continue; }
                                        CurrentUrl = BlacklistedSafeURLs[y];
                                        this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (s) file " + (y + 1) + " of " + (BlacklistSafeCount)));

                                        string fileName = BlacklistedSafeFileNames[y];
                                        string outputDir = DownloadInfo.DownloadPath + "\\safe\\blacklisted\\";
                                        switch (DownloadInfo.SeparateNonImages) {
                                            case true:
                                                if (fileName.EndsWith("gif")) {
                                                    outputDir += "gif\\";
                                                }
                                                else if (fileName.EndsWith("apng")) {
                                                    outputDir += "apng\\";
                                                }
                                                else if (fileName.EndsWith("webm")) {
                                                    outputDir += "webm\\";
                                                }
                                                else if (fileName.EndsWith("swf")) {
                                                    outputDir += "swf\\";
                                                }
                                                break;
                                        }
                                        outputDir += fileName;
                                        if (!File.Exists(outputDir)) {
                                            this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + fileName));
                                            lock (sync) {
                                                wc.DownloadFileAsync(new Uri(CurrentUrl), outputDir, sync);
                                                Monitor.Wait(sync);
                                            }
                                        }
                                        else {
                                            this.Invoke((MethodInvoker)(() => status.Text = fileName + " already exists"));
                                        }
                                    }
                                }
                            }
                        }
                        else {
                            if (CleanedURLs.Count > 0) {
                                this.BeginInvoke((MethodInvoker)delegate() {
                                    Program.Log(LogAction.WriteToLog, "Downloading clean files.");
                                });

                                for (int y = 0; y < CleanedURLs.Count; y++) {
                                    if (string.IsNullOrEmpty(CleanedURLs[y])) { continue; }
                                    CurrentUrl = CleanedURLs[y];
                                    this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading file " + (y + 1) + " of " + (CleanTotalCount)));

                                    string fileName = CleanedFileNames[y];
                                    string outputDir = DownloadInfo.DownloadPath;
                                    switch (DownloadInfo.SeparateNonImages) {
                                        case true:
                                            if (fileName.EndsWith("gif")) {
                                                outputDir += "\\gif\\";
                                            }
                                            else if (fileName.EndsWith("apng")) {
                                                outputDir += "\\apng\\";
                                            }
                                            else if (fileName.EndsWith("webm")) {
                                                outputDir += "\\webm\\";
                                            }
                                            else if (fileName.EndsWith("swf")) {
                                                outputDir += "\\swf\\";
                                            }
                                            break;
                                    }
                                    outputDir += fileName;
                                    if (!File.Exists(outputDir)) {
                                        this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + fileName));
                                        lock (sync) {
                                            wc.DownloadFileAsync(new Uri(CurrentUrl), outputDir, sync);
                                            Monitor.Wait(sync);
                                        }
                                    }
                                    else {
                                        this.Invoke((MethodInvoker)(() => status.Text = fileName + " already exists"));
                                    }
                                }
                            }

                            if (DownloadInfo.SaveGraylistedFiles && GraylistedURLs.Count > 0) {
                                this.BeginInvoke((MethodInvoker)delegate() {
                                    Program.Log(LogAction.WriteToLog, "Downloading graylisted files.");
                                });

                                for (int y = 0; y < GraylistedURLs.Count; y++) {
                                    if (string.IsNullOrEmpty(GraylistedURLs[y])) { continue; }
                                    CurrentUrl = GraylistedURLs[y];
                                    this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading graylisted file " + (y) + " of " + (GraylistTotalCount)));

                                    string fileName = GraylistedFileNames[y];
                                    string outputDir = DownloadInfo.DownloadPath + "\\graylisted\\";
                                    switch (DownloadInfo.SeparateNonImages) {
                                        case true:
                                            if (fileName.EndsWith("gif")) {
                                                outputDir += "gif\\";
                                            }
                                            else if (fileName.EndsWith("apng")) {
                                                outputDir += "apng\\";
                                            }
                                            else if (fileName.EndsWith("webm")) {
                                                outputDir += "webm\\";
                                            }
                                            else if (fileName.EndsWith("swf")) {
                                                outputDir += "swf\\";
                                            }
                                            break;
                                    }
                                    outputDir += fileName;
                                    if (!File.Exists(outputDir)) {
                                        this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + fileName));
                                        lock (sync) {
                                            wc.DownloadFileAsync(new Uri(CurrentUrl), outputDir, sync);
                                            Monitor.Wait(sync);
                                        }
                                    }
                                    else {
                                        this.Invoke((MethodInvoker)(() => status.Text = fileName + " already exists"));
                                    }
                                }
                            }

                            if (DownloadInfo.SaveBlacklistedFiles && BlacklistedURLs.Count > 0) {
                                this.BeginInvoke((MethodInvoker)delegate() {
                                    Program.Log(LogAction.WriteToLog, "Downloading blacklisted files.");
                                });

                                for (int y = 0; y < BlacklistedURLs.Count; y++) {
                                    if (string.IsNullOrEmpty(BlacklistedURLs[y])) { continue; }
                                    CurrentUrl = BlacklistedURLs[y];
                                    this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted file " + (y) + " of " + (BlacklistTotalCount)));

                                    string fileName = BlacklistedFileNames[y];
                                    string outputDir = DownloadInfo.DownloadPath + "\\blacklisted\\";
                                    switch (DownloadInfo.SeparateNonImages) {
                                        case true:
                                            if (fileName.EndsWith("gif")) {
                                                outputDir += "gif\\";
                                            }
                                            else if (fileName.EndsWith("apng")) {
                                                outputDir += "apng\\";
                                            }
                                            else if (fileName.EndsWith("webm")) {
                                                outputDir += "webm\\";
                                            }
                                            else if (fileName.EndsWith("swf")) {
                                                outputDir += "swf\\";
                                            }
                                            break;
                                    }
                                    outputDir += fileName;
                                    if (!File.Exists(outputDir)) {
                                        this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + fileName));
                                        lock (sync) {
                                            wc.DownloadFileAsync(new Uri(CurrentUrl), outputDir, sync);
                                            Monitor.Wait(sync);
                                        }
                                    }
                                    else {
                                        this.Invoke((MethodInvoker)(() => status.Text = fileName + " already exists"));
                                    }
                                }
                            }
                        }
                    }
                }
                    #endregion

                #endregion

                #region post download
                if (DownloadInfo.SeparateRatings) {
                    TotalFiles += (CleanedExplicitURLs.Count + CleanedQuestionableURLs.Count + CleanedSafeURLs.Count);
                    PresentFiles += CleanTotalCount;
                    if (DownloadInfo.SaveGraylistedFiles) {
                        TotalFiles += (GraylistedExplicitURLs.Count + GraylistedQuestionableURLs.Count + GraylistedSafeURLs.Count);
                        PresentFiles += GraylistTotalCount;
                    }
                }
                else {
                    TotalFiles += CleanedURLs.Count;
                    PresentFiles += CleanTotalCount;
                    if (DownloadInfo.SaveGraylistedFiles) {
                        TotalFiles += GraylistedURLs.Count;
                        PresentFiles += GraylistTotalCount;
                    }
                }

                this.BeginInvoke(new MethodInvoker(() => {
                    Program.Log(LogAction.WriteToLog, "Tags \"" + DownloadInfo.Tags + "\" was downloaded. (frmTagDownloader.cs)");
                }));

                CurrentStatus = DownloadStatus.Finished;
                #endregion
            }
            #endregion

            #region Downloader catch-statements
            catch (ApiReturnedNullOrEmptyException) {
                this.BeginInvoke(new MethodInvoker(() => {
                    Program.Log(LogAction.WriteToLog, "Tags \"" + DownloadInfo.Tags + "\" Api returned null or empty. (frmTagDownloader.cs)");
                }));
                CurrentStatus = DownloadStatus.ApiReturnedNullOrEmpty;
            }
            catch (NoFilesToDownloadException) {
                this.BeginInvoke(new MethodInvoker(() => {
                    Program.Log(LogAction.WriteToLog, "No files are available for tags \"" + DownloadInfo.Tags + "\". (frmTagDownloader.cs)");
                }));
                CurrentStatus = DownloadStatus.NothingToDownload;
            }
            catch (ThreadAbortException) {
                this.BeginInvoke(new MethodInvoker(() => {
                    Program.Log(LogAction.WriteToLog, "The download thread was aborted. (frmTagDownloader.cs)");
                }));
                CurrentStatus = DownloadStatus.Aborted;
            }
            catch (ObjectDisposedException) {
                this.BeginInvoke(new MethodInvoker(() => {
                    Program.Log(LogAction.WriteToLog, "Seems like the form got disposed. (frmTagDownloader.cs)");
                }));
                CurrentStatus = DownloadStatus.FormWasDisposed;
            }
            catch (WebException WebE) {
                this.BeginInvoke(new MethodInvoker(() => {
                    Program.Log(LogAction.WriteToLog, "A WebException has occured. (frmTagDownloader.cs)");
                    status.Text = "A WebException has occured";
                    pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                    pbTotalStatus.State = aphrodite.Controls.ProgressBarState.Error;
                }));
                CurrentStatus = DownloadStatus.Errored;
                ErrorLog.ReportWebException(WebE, CurrentUrl, "frmTagDownloader.cs");
            }
            catch (Exception ex) {
                this.BeginInvoke(new MethodInvoker(() => {
                    Program.Log(LogAction.WriteToLog, "An Exception has occured. (frmTagDownloader.cs)");
                    status.Text = "A Exception has occured";
                    pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                    pbTotalStatus.State = aphrodite.Controls.ProgressBarState.Error;
                }));
                CurrentStatus = DownloadStatus.Errored;
                ErrorLog.ReportException(ex, "frmTagDownloader.cs");
            }
            #endregion

            #region After download finally statement
            finally {
                if (CurrentStatus != DownloadStatus.FormWasDisposed) {
                    this.BeginInvoke(new MethodInvoker(() => {
                        AfterDownload();
                    }));
                }
            }
            #endregion
        }
        #endregion

    }
}