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
        private Thread tagDownload;                     // New thread for the downloader.

        private int cleanTotalCount = 0;                // Will be the count of how many files that are set for download.
        private int cleanExplicitCount = 0;             // Will be the count of how many explicit files that are set for download.
        private int cleanQuestionableCount = 0;         // Will be the count of how many questionable files that are set for download.
        private int cleanSafeCount = 0;                 // Will be the count of how many safe files that are set for download.

        private int cleanTotalExistCount = 0;           // Will be the count of how many files already exist in total.
        private int cleanExplicitExistCount = 0;        // Will be the count of how many explicit files already exist.
        private int cleanQuestionableExistCount = 0;    // Will be the count of how many questionable files already exist.
        private int cleanSafeExistCount = 0;            // Will be the count of how many safe files already exist.

        private int graylistTotalCount = 0;             // Will be the count of how many graylisted files that are set for download.
        private int graylistExplicitCount = 0;          // Will be the count of how many explicit graylisted files that are set for download.
        private int graylistQuestionableCount = 0;      // Will be the count of how many questionable graylisted files that are set for download.
        private int graylistSafeCount = 0;              // Will be the count of how many safe graylisted files that are set for download.

        private int graylistTotalExistCount = 0;        // Will be the count of how many gralisted files already exist in total.
        private int graylistExplicitExistCount = 0;     // Will be the count of how many explicit graylisted files already exist.
        private int graylistQuestionableExistCount = 0; // Will be the count of how many questionable graylisted files already exist.
        private int graylistSafeExistCount = 0;         // Will be the count of how many safe graylisted files already exist.

        private int blacklistCount = 0;                 // Will be the count of how many blacklisted files that will be skipped.
        private int totalCount = 0;                     // Will be the count of how many files that were parsed.

        private int PresentFiles = 0;
        private int TotalFiles = 0;

        private bool DownloadHasFinished = false;
        private bool DownloadHasErrored = false;
        private bool DownloadHasAborted = false;
        #endregion

        #region Form
        public frmTagDownloader() {
            InitializeComponent();
        }
        private void frmDownload_Load(object sender, EventArgs e) {
            if (!DownloadInfo.SaveExplicit && !DownloadInfo.SaveQuestionable && !DownloadInfo.SaveSafe) {
                MessageBox.Show("No ratings were selected!", "aphrodite", MessageBoxButtons.OK);
                this.Opacity = 0;
                DownloadHasAborted = true;
                this.DialogResult = DialogResult.No;
                return;
            }
            else if (DownloadInfo.ImageLimit == 0 && DownloadInfo.PageLimit == 0) {
                if (MessageBox.Show("Downloading won't be limited. This may take a long while or even blacklist you. Continue anyway?", "aphrodite", MessageBoxButtons.YesNo) == DialogResult.No) {
                    this.Opacity = 0;
                    DownloadHasAborted = true;
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
            if (!DownloadHasFinished && !DownloadHasErrored && !DownloadHasAborted) {
                tmrTitle.Stop();
                DownloadHasAborted = true;

                if (tagDownload != null && tagDownload.IsAlive) {
                    tagDownload.Abort();
                }

                if (!DownloadInfo.IgnoreFinish) {
                    e.Cancel = true;
                }
                else {
                    lbFile.Text = "Download canceled";
                    pbDownloadStatus.State = ProgressBarState.Error;
                    lbPercentage.Text = "Canceled";
                    tmrTitle.Stop();
                    this.Text = "Download canceled";
                    status.Text = "Download has canceled";
                }
            }
            else {
                this.Dispose();
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
            try {
                tagDownload = new Thread(() => {
                    Thread.CurrentThread.IsBackground = true;

                    DownloadPosts();

                    //if (DownloadInfo.FromUrl) {
                    //    if (DownloadPage(DownloadInfo.PageUrl)) {
                    //        if (DownloadInfo.IgnoreFinish) {
                    //            this.DialogResult = DialogResult.OK;
                    //        }
                    //    }
                    //    else {
                    //        this.DialogResult = DialogResult.Abort;
                    //    }
                    //}
                    //else {
                    //    DownloadTags();
                    //}
                });

                tagDownload.Start();
                tmrTitle.Start();
            }
            catch {
                throw;
            }
        }

        private void AfterDownload(int ErrorType = 0) {
            if (DownloadInfo.OpenAfter) {
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
                lbFile.Text = "All " + (PresentFiles) + " file(s) downloaded. (" + (TotalFiles) + " total files downloaded)";
                pbDownloadStatus.Value = pbDownloadStatus.Maximum;
                lbPercentage.Text = "Done";
                tmrTitle.Stop();
                this.Text = "Finished downloading tags " + DownloadInfo.Tags;
                status.Text = "Finished downloading tags";
            }
            else if (DownloadHasErrored) {
                if (ErrorType == 0) {
                    lbFile.Text = "Downloading has encountered an error";
                    pbDownloadStatus.State = ProgressBarState.Error;
                    lbPercentage.Text = "Error";
                    tmrTitle.Stop();
                    this.Text = "Download error";
                    status.Text = "Downloading has resulted in an error";
                }
                else if (ErrorType == 1) {
                    lbFile.Text = "API parsing has resulted in an error";
                    pbDownloadStatus.State = ProgressBarState.Error;
                    lbPercentage.Text = "Error";
                    tmrTitle.Stop();
                    this.Text = "API parsing error";
                    status.Text = "API parsing has resulted in an error";
                }
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
                this.Text = "Tags download completed";
                status.Text = "Download status booleans not set, assuming the download completed";
            }
        }

        private void DownloadPosts() {
            ChangeTask("Awaiting API call");

            #region Download variables
            string CurrentUrl = string.Empty;               // The URL being accessed, changes per API call/File download.
            string TagInfoBuffer = string.Empty;            // The buffer for the 'tag.nfo' file that will be created.
            string BlacklistInfoBuffer = string.Empty;      // The buffer for the 'tag.blacklisted.nfo' file that will be created.

            string CurrentXml = string.Empty;               // The XML string.
            int CurrentPage = 1;                            // Will be the count of the pages parsed.
            bool MaximumReached = false;                    // Determines if the maximum files have been parsed.

            List<string> URLs = new List<string>();             // The URLs that will be downloaded (if separateRatings = false).
            List<string> GraylistedURLs = new List<string>();   // The Blacklisted URLs that will be downloaded (if separateRatings = false).

            List<string> FileNames = new List<string>();            // Contains the file names of the images
            List<string> GraylistedFileNames = new List<string>();  // Contains the file names of the graylisted images

            List<bool> FileExists = new List<bool>();
            List<bool> GraylistedFileExists = new List<bool>();

            List<string> ExplicitURLs = new List<string>();         // The list of Explicit files.
            List<string> ExplicitFileNames = new List<string>();    // The list of Explicit file names.
            List<bool> ExplicitFileExists = new List<bool>();       // The list of existing Explicit files.

            List<string> QuestionableURLs = new List<string>();         // The list of Questionable files.
            List<string> QuestionableFileNames = new List<string>();    // The list of Questionable file names.
            List<bool> QuestionableFileExists = new List<bool>();

            List<string> SafeURLs = new List<string>();                     // The list of Safe files.
            List<string> SafeFileNames = new List<string>();                // The list of Safe file names.
            List<bool> SafeFileExists = new List<bool>();

            List<string> GraylistedExplicitURLs = new List<string>();       // The list of Graylisted Explicit files.
            List<string> GraylistedExplicitFileNames = new List<string>();  // The list of Graylisted Explicit file names.
            List<bool> GraylistedExplicitFileExists = new List<bool>();

            List<string> GraylistedQuestionableURLs = new List<string>();       // The list of Graylisted Questionable files.
            List<string> GraylistedQuestionableFileNames = new List<string>();  // The list of Graylitsed Questionable file names.
            List<bool> GraylistedQuestionableFileExists = new List<bool>();

            List<string> GraylistedSafeURLs = new List<string>();           // The list of Graylisted Safe files.
            List<string> GraylistedSafeFileNames = new List<string>();      // The list of Graylisted Safe file names.
            List<bool> GraylistedSafeFileExists = new List<bool>();


            // the Boolean lists of if non-images exist to create the save folder
            // 0 = gif, 1 = apng, 2 = webm, 3 = swf
            List<bool> FileNonImages = new List<bool>{ false, false, false, false };
            List<bool> GraylistedFileNonImages = new List<bool> { false, false, false, false };
            List<bool> ExplicitNonImages = new List<bool> { false, false, false, false };
            List<bool> QuestionableNonImages = new List<bool> { false, false, false, false };
            List<bool> SafeNonImages = new List<bool> { false, false, false, false };
            List<bool> GraylistedExplicitNonImages = new List<bool> { false, false, false, false };
            List<bool> GraylistedQuestionableNonImages = new List<bool> { false, false, false, false };
            List<bool> GraylistedSafeNonImages = new List<bool> { false, false, false, false };
            #endregion

            #region Downloader try-statement
            try {
            #region initialization
                //Properties.Settings.Default.Log += "Tag downloader starting for tags " + tags + "\r\n";
                string newTagName = apiTools.ReplaceIllegalCharacters(DownloadInfo.Tags);
                if (DownloadInfo.UseMinimumScore) { // Add minimum score to folder name.
                    newTagName += " (scores " + (DownloadInfo.MinimumScore) + "+)";
                }

                // Set the saveTo.
                if (DownloadInfo.FromUrl) {
                    DownloadInfo.DownloadPath += "\\Pages\\" + newTagName + " (page " + DownloadInfo.PageNumber + ")";
                }
                else {
                    DownloadInfo.DownloadPath += "\\Tags\\" + newTagName;
                }

                // Start the buffer for the .nfo files.
                if (DownloadInfo.UseMinimumScore) {
                    TagInfoBuffer = "TAGS: " + DownloadInfo.Tags + "\r\n" +
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
                    TagInfoBuffer = "TAGS: " + DownloadInfo.Tags + "\r\n" +
                              "MINIMUM SCORE: n/a\r\n" +
                              "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") +
                              "\r\n\r\n";

                    BlacklistInfoBuffer = "TAGS: " + DownloadInfo.Tags + "\r\n" +
                                    "MINIMUM SCORE: n/a\r\n" +
                                    "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\r\n" +
                                    "BLACKLISTED TAGS: " + string.Join(" ", DownloadInfo.Graylist) +
                                    "\r\n\r\n";
                }

                // Add the minimum score to the search tags (if applicable).
                if (DownloadInfo.UseMinimumScore) {
                    if (DownloadInfo.Tags.Split(' ').Length < 6 && DownloadInfo.MinimumScoreAsTag) {
                        DownloadInfo.Tags += " score:>" + (DownloadInfo.MinimumScore - 1);
                    }
                }
            #endregion

            #region donwload pages & parse
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

                for (int Page = 0; Page < CurrentPage; Page++) {
                    if (DownloadInfo.PageLimit > 0 && CurrentPage > DownloadInfo.PageLimit) {
                        break;
                    }

                    // Get XML of page.
                    ChangeTask(string.Format("Downloading tag information for page {0}...", CurrentPage));
                    if (DownloadInfo.FromUrl) {
                        CurrentUrl = DownloadInfo.PageUrl.Replace("e621.net/posts", "e621.net/posts.json");
                    }
                    else {
                        CurrentUrl = string.Format(PageURL, DownloadInfo.Tags, CurrentPage);
                    }
                    CurrentXml = apiTools.GetJsonToXml(CurrentUrl);
                    if (apiTools.IsXmlDead(CurrentXml)) {
                        break;
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

                    // Determine the pages by counting the posts.
                    switch (!DownloadInfo.FromUrl && xmlID.Count == 320) {
                        case true:
                            CurrentPage++;
                            break;
                    }

                    if (xmlID.Count > 0) {
                        // Begin parsing the XML for tag information per item.
                        for (int i = 0; i < xmlID.Count; i++) {

                            if (DownloadInfo.ImageLimit > 0 && DownloadInfo.ImageLimit == totalCount) {
                                MaximumReached = true;
                                break;
                            }

                            string rating = xmlRating[i].InnerText;         // Get the rating of the current file.
                            bool PostIsGraylisted = false;                  // Will determine if the file is graylisted.
                            bool PostIsBlacklisted = false;                 // Will determine if the file is blacklisted.
                            bool PostAlreadyExists = false;                 // Will determine if the file exists.
                            List<string> PostTags = new List<string>();     // Get the entire tag list of the file.
                            string FoundGraylistedTags = string.Empty;      // The buffer for the tags that are graylisted.
                            string ReadTags = string.Empty;                 // The buffer for the tags for the .nfo

                            switch (xmlRating[i].InnerText.ToLower()) {
                                case "e": case "explicit":
                                    switch (DownloadInfo.SaveExplicit) {
                                        case false:
                                            continue;
                                    }
                                    rating = "Explicit";
                                    break;

                                case "q": case "questionable":
                                    switch (DownloadInfo.SaveQuestionable) {
                                        case false:
                                            continue;
                                    }
                                    rating = "Questionable";
                                    break;

                                case "s": case "safe":
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

                                if (PostIsBlacklisted) {
                                    break;
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
                            switch (PostIsBlacklisted) {
                                case true:
                                    blacklistCount++;
                                    totalCount++;
                                    continue;
                            }
                            switch (PostIsGraylisted && !DownloadInfo.SaveBlacklistedFiles) {
                                case true:
                                    graylistTotalCount++;
                                    totalCount++;
                                    continue;
                            }

                            // Set the description
                            string ImageDescription = " No description";
                            switch (string.IsNullOrWhiteSpace(xmlDescription[i].InnerText)) {
                                case false:
                                    ImageDescription = xmlDescription[i].InnerText;
                                    break;
                            }

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

                            string fileName = DownloadInfo.FileNameSchema.Replace("%md5%", xmlMD5[i].InnerText)
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
                            string fileUrl = xmlURL[i].InnerText;
                            if (string.IsNullOrEmpty(fileUrl)) {
                                if (xmlDeleted[i].InnerText.ToLower() == "false") {
                                    fileUrl = apiTools.GetBlacklistedImageUrl(xmlMD5[i].InnerText, xmlExt[i].InnerText);
                                }
                            }

                            // Add to the counts (and break for blacklisted)
                            string outputDir = DownloadInfo.DownloadPath;
                            if (PostIsGraylisted) {
                                if (DownloadInfo.SeparateRatings) {
                                    outputDir += "\\" + rating.ToLower() + "\\blacklisted\\";
                                    if (DownloadInfo.SeparateNonImages) {
                                        switch (xmlExt[i].InnerText.ToLower()) {
                                            case "gif":
                                                outputDir += "gif\\";
                                                break;
                                            case "apng":
                                                outputDir += "apng\\";
                                                break;
                                            case "webm":
                                                outputDir += "webm\\";
                                                break;
                                            case "swf":
                                                outputDir += "swf\\";
                                                break;
                                        }
                                    }
                                    outputDir += fileName;
                                    PostAlreadyExists = File.Exists(outputDir);
                                }
                                else {
                                    outputDir += "\\blacklisted\\";
                                    if (DownloadInfo.SeparateNonImages) {
                                        switch (xmlExt[i].InnerText.ToLower()) {
                                            case "gif":
                                                outputDir += "gif\\";
                                                break;
                                            case "apng":
                                                outputDir += "apng\\";
                                                break;
                                            case "webm":
                                                outputDir += "webm\\";
                                                break;
                                            case "swf":
                                                outputDir += "swf\\";
                                                break;
                                        }
                                    }
                                    PostAlreadyExists = File.Exists(outputDir);
                                }


                                switch (xmlRating[i].InnerText.ToLower()) {
                                    case "e":
                                    case "explicit":
                                        if (PostAlreadyExists) {
                                            graylistExplicitExistCount++;
                                        }
                                        else {
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
                                            graylistExplicitCount++;
                                        }
                                        break;
                                    case "q":
                                    case "questionable":
                                        if (PostAlreadyExists) {
                                            graylistQuestionableExistCount++;
                                        }
                                        else {
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
                                            graylistQuestionableCount++;
                                        }
                                        break;
                                    case "s":
                                    case "safe":
                                        if (PostAlreadyExists) {
                                            graylistSafeExistCount++;
                                        }
                                        else {
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
                                            graylistSafeCount++;
                                        }
                                        break;
                                    default:
                                        MessageBox.Show("An error occured when determining the rating. Open an issue.\r\n\nFound rating: " + xmlRating[i].InnerText);
                                        break;
                                }

                                if (PostAlreadyExists) {
                                    graylistTotalExistCount++;
                                }
                                else {
                                    if (DownloadInfo.SeparateNonImages) {
                                        switch (xmlExt[i].InnerText.ToLower()) {
                                            case "gif":
                                                GraylistedFileNonImages[0] = true;
                                                break;
                                            case "apng":
                                                GraylistedFileNonImages[1] = true;
                                                break;
                                            case "webm":
                                                GraylistedFileNonImages[2] = true;
                                                break;
                                            case "swf":
                                                GraylistedFileNonImages[3] = true;
                                                break;
                                        }
                                    }
                                    graylistTotalCount++;
                                }
                                totalCount++;
                            }
                            else {
                                if (DownloadInfo.SeparateRatings) {
                                    outputDir += "\\" + rating.ToLower() + "\\";
                                    if (DownloadInfo.SeparateNonImages) {
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
                                    }
                                    outputDir += fileName;
                                    PostAlreadyExists = File.Exists(outputDir);
                                }
                                else {
                                    outputDir += "\\";
                                    if (DownloadInfo.SeparateNonImages) {
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
                                    }
                                    PostAlreadyExists = File.Exists(outputDir);
                                }

                                switch (xmlRating[i].InnerText.ToLower()) {
                                    case "e":
                                    case "explicit":
                                        if (PostAlreadyExists) {
                                            cleanExplicitExistCount++;
                                        }
                                        else {
                                            if (DownloadInfo.SeparateNonImages) {
                                                switch (xmlExt[i].InnerText.ToLower()) {
                                                    case "gif":
                                                        ExplicitNonImages[0] = true;
                                                        break;
                                                    case "apng":
                                                        ExplicitNonImages[1] = true;
                                                        break;
                                                    case "webm":
                                                        ExplicitNonImages[2] = true;
                                                        break;
                                                    case "swf":
                                                        ExplicitNonImages[3] = true;
                                                        break;
                                                }
                                            }
                                            cleanExplicitCount++;
                                        }
                                        break;
                                    case "q":
                                    case "questionable":
                                        if (PostAlreadyExists) {
                                            cleanQuestionableExistCount++;
                                        }
                                        else {
                                            if (DownloadInfo.SeparateNonImages) {
                                                switch (xmlExt[i].InnerText.ToLower()) {
                                                    case "gif":
                                                        QuestionableNonImages[0] = true;
                                                        break;
                                                    case "apng":
                                                        QuestionableNonImages[1] = true;
                                                        break;
                                                    case "webm":
                                                        QuestionableNonImages[2] = true;
                                                        break;
                                                    case "swf":
                                                        QuestionableNonImages[3] = true;
                                                        break;
                                                }
                                            }
                                            cleanQuestionableCount++;
                                        }
                                        break;
                                    case "s":
                                    case "safe":
                                        if (PostAlreadyExists) {
                                            cleanSafeExistCount++;
                                        }
                                        else {
                                            if (DownloadInfo.SeparateNonImages) {
                                                switch (xmlExt[i].InnerText.ToLower()) {
                                                    case "gif":
                                                        SafeNonImages[0] = true;
                                                        break;
                                                    case "apng":
                                                        SafeNonImages[1] = true;
                                                        break;
                                                    case "webm":
                                                        SafeNonImages[2] = true;
                                                        break;
                                                    case "swf":
                                                        SafeNonImages[3] = true;
                                                        break;
                                                }
                                            }
                                            cleanSafeCount++;
                                        }
                                        break;
                                    default:
                                        MessageBox.Show("An error occured when determining the rating. Open an issue.\r\n\nFound rating: " + xmlRating[i].InnerText);
                                        break;
                                }

                                if (PostAlreadyExists) {
                                    cleanTotalExistCount++;
                                }
                                else {
                                    if (DownloadInfo.SeparateNonImages) {
                                        switch (xmlExt[i].InnerText.ToLower()) {
                                            case "gif":
                                                FileNonImages[0] = true;
                                                break;
                                            case "apng":
                                                FileNonImages[1] = true;
                                                break;
                                            case "webm":
                                                FileNonImages[2] = true;
                                                break;
                                            case "swf":
                                                FileNonImages[3] = true;
                                                break;
                                        }
                                    }
                                    cleanTotalCount++;
                                }
                                totalCount++;
                            }

                            // Graylist check & options check
                            if (PostIsGraylisted) {
                                if (DownloadInfo.SaveBlacklistedFiles) {
                                    if (DownloadInfo.UseMinimumScore && Int32.Parse(xmlScore[i].InnerText) < DownloadInfo.MinimumScore)
                                        continue;
                                }
                                else {
                                    continue;
                                }
                            }
                            else { // Not blacklisted
                                if (DownloadInfo.UseMinimumScore && Int32.Parse(xmlScore[i].InnerText) < DownloadInfo.MinimumScore)
                                    continue;
                            }

                            switch (rating.ToLower()) {
                                case "explicit":
                                    if (!DownloadInfo.SaveExplicit) {
                                        continue;
                                    }
                                    break;
                                case "questionable":
                                    if (!DownloadInfo.SaveQuestionable) {
                                        continue;
                                    }
                                    break;
                                case "safe":
                                    if (!DownloadInfo.SaveSafe) {
                                        continue;
                                    }
                                    break;
                            }

                            // Start adding to the nfo buffer and URL lists
                            if (PostIsGraylisted && DownloadInfo.SaveBlacklistedFiles) {
                                if (DownloadInfo.SeparateRatings) {
                                    if (xmlRating[i].InnerText == "e") {
                                        //GraylistedExplicitURLs.Add(xmlURL[i].InnerText);
                                        GraylistedExplicitURLs.Add(fileUrl);
                                        GraylistedExplicitFileNames.Add(fileName);
                                        GraylistedExplicitFileExists.Add(PostAlreadyExists);
                                    }
                                    else if (xmlRating[i].InnerText == "q") {
                                        //GraylistedQuestionableURLs.Add(xmlURL[i].InnerText);
                                        GraylistedQuestionableURLs.Add(fileUrl);
                                        GraylistedQuestionableFileNames.Add(fileName);
                                        GraylistedQuestionableFileExists.Add(PostAlreadyExists);
                                    }
                                    else if (xmlRating[i].InnerText == "s") {
                                        //GraylistedSafeURLs.Add(xmlURL[i].InnerText);
                                        GraylistedSafeURLs.Add(fileUrl);
                                        GraylistedSafeFileNames.Add(fileName);
                                        GraylistedSafeFileExists.Add(PostAlreadyExists);
                                    }
                                }
                                else {
                                    //GraylistedURLs.Add(xmlURL[i].InnerText);
                                    GraylistedURLs.Add(fileUrl);
                                    GraylistedFileNames.Add(fileName);
                                    GraylistedFileExists.Add(PostAlreadyExists);
                                }

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
                                    "    TAGS {3}\r\n"+
                                    "    OFFENDING TAGS: {4}\r\n" +
                                    "    SCORE: Up {5}, Down {6}, Total {7}\r\n" +
                                    "    RATING: {8}\r\n" +
                                    "    DESCRIPTION: {9}\r\n\r\n", Info
                                );
                            }
                            else {
                                if (DownloadInfo.SeparateRatings) {
                                    if (xmlRating[i].InnerText == "e") {
                                        //ExplicitURLs.Add(xmlURL[i].InnerText);
                                        ExplicitURLs.Add(fileUrl);
                                        ExplicitFileNames.Add(fileName);
                                        ExplicitFileExists.Add(PostAlreadyExists);
                                    }
                                    else if (xmlRating[i].InnerText == "q") {
                                        //QuestionableURLs.Add(xmlURL[i].InnerText);
                                        QuestionableURLs.Add(fileUrl);
                                        QuestionableFileNames.Add(fileName);
                                        QuestionableFileExists.Add(PostAlreadyExists);
                                    }
                                    else if (xmlRating[i].InnerText == "s") {
                                        //SafeURLs.Add(xmlURL[i].InnerText);
                                        SafeURLs.Add(fileUrl);
                                        SafeFileNames.Add(fileName);
                                        SafeFileExists.Add(PostAlreadyExists);
                                    }
                                }
                                else {
                                    //URLs.Add(xmlURL[i].InnerText);
                                    URLs.Add(fileUrl);
                                    FileNames.Add(fileName);
                                    FileExists.Add(PostAlreadyExists);
                                }

                                TagInfoBuffer += "POST " + xmlID[i].InnerText + ":\r\n" +
                                           "    MD5: " + xmlMD5[i].InnerText + "\r\n" +
                                           "    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\r\n" +
                                           "    TAGS:\r\n" + ReadTags + "\r\n" +
                                           "    SCORE: Up " + xmlScoreUp[i].InnerText + ", Down " + xmlScoreDown[i].InnerText + ", Total " + xmlScore[i].InnerText + "\r\n" +
                                           "    RATING: " + rating + "\r\n" +
                                           "    DESCRIPITON:" + ImageDescription +
                                           "\r\n\r\n";
                            }
                        }


                        this.BeginInvoke(new MethodInvoker(() => {
                            string labelBuffer = "";
                            if (DownloadInfo.SeparateRatings) {
                                object[] counts = new object[] {
                            cleanTotalCount.ToString(),                 // 0
                            cleanExplicitCount.ToString(),              // 1
                            cleanQuestionableCount.ToString(),          // 2
                            cleanSafeCount.ToString(),                  // 3
                            graylistTotalCount.ToString(),              // 4
                            graylistExplicitCount.ToString(),           // 5
                            graylistQuestionableCount.ToString(),       // 6
                            graylistSafeCount.ToString(),               // 7
                            blacklistCount.ToString(),                  // 8
                            totalCount.ToString(),                      // 9
                            cleanTotalExistCount.ToString(),            // 10
                            cleanExplicitExistCount.ToString(),         // 11
                            cleanQuestionableExistCount.ToString(),     // 12
                            cleanSafeExistCount.ToString(),             // 13
                            graylistTotalExistCount.ToString(),         // 14
                            graylistExplicitExistCount.ToString(),      // 15
                            graylistQuestionableExistCount.ToString(),  // 16
                            graylistSafeExistCount.ToString()           // 17
                        };

                                labelBuffer = string.Format("Files: {0} ( {1} E | {2} Q | {3} S )\r\n" +
                                                            "Blacklisted: {4} ( {5} E | {6} Q | {7} S )\r\n" +
                                                            "Zero Tolerance: {8}\r\n" +
                                                            "Total: {9}\r\n\r\n" +
                                                            "Files that exist: {10} ( {11} E | {12} Q | {13} S )\r\n" +
                                                            "Blacklisted that exist: {14} ( {15} E | {16} Q | {17} S )", counts);
                            }
                            else {
                                object[] counts = new object[] {
                            cleanTotalCount.ToString(),
                            graylistTotalCount.ToString(),
                            blacklistCount.ToString(),
                            totalCount.ToString(),
                            cleanTotalExistCount.ToString(),
                            graylistTotalExistCount.ToString()
                        };

                                labelBuffer = string.Format("Files: {0}\r\n" +
                                                            "Blacklisted: {1}\r\n" +
                                                            "Zero Tolerance: {2}\r\n" +
                                                            "Total: {3}\r\n\r\n" +
                                                            "Files that exist: {4}\r\n" +
                                                            "Blacklisted that exist: {5}", counts);
                            }
                            lbBlacklist.Text = labelBuffer;
                        }));
                    }

                    if (MaximumReached) {
                        break;
                    }
                }
            #endregion

            #region output logic
                // Create output folders
                if (DownloadInfo.SeparateRatings) {
                    if (ExplicitURLs.Count > 0) {
                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit");
                        if (DownloadInfo.SeparateNonImages) {
                            if (ExplicitNonImages[0]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\gif");
                            }
                            if (ExplicitNonImages[1]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\apng");
                            }
                            if (ExplicitNonImages[2]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\webm");
                            }
                            if (ExplicitNonImages[3]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\swf");
                            }
                        }
                    }
                    if (QuestionableURLs.Count > 0) {
                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable");
                        if (DownloadInfo.SeparateNonImages) {
                            if (QuestionableNonImages[0]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\gif");
                            }
                            if (QuestionableNonImages[1]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\apng");
                            }
                            if (QuestionableNonImages[2]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\webm");
                            }
                            if (QuestionableNonImages[3]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\swf");
                            }
                        }
                    }
                    if (SafeURLs.Count > 0) {
                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe");
                        if (DownloadInfo.SeparateNonImages) {
                            if (SafeNonImages[0]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\gif");
                            }
                            if (SafeNonImages[1]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\apng");
                            }
                            if (SafeNonImages[2]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\webm");
                            }
                            if (SafeNonImages[3]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\swf");
                            }
                        }
                    }

                    if (DownloadInfo.SaveBlacklistedFiles) {
                        if (GraylistedExplicitURLs.Count > 0) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\blacklisted");
                            if (DownloadInfo.SeparateNonImages) {
                                if (GraylistedExplicitNonImages[0]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\blacklisted\\gif");
                                }
                                if (GraylistedExplicitNonImages[1]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\blacklisted\\apng");
                                }
                                if (GraylistedExplicitNonImages[2]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\blacklisted\\webm");
                                }
                                if (GraylistedExplicitNonImages[3]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\explicit\\blacklisted\\swf");
                                }
                            }
                        }
                        if (GraylistedQuestionableURLs.Count > 0) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\blacklisted");
                            if (DownloadInfo.SeparateNonImages) {
                                if (GraylistedQuestionableNonImages[0]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\blacklisted\\gif");
                                }
                                if (GraylistedQuestionableNonImages[1]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\blacklisted\\apng");
                                }
                                if (GraylistedQuestionableNonImages[2]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\blacklisted\\webm");
                                }
                                if (GraylistedQuestionableNonImages[3]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\questionable\\blacklisted\\swf");
                                }
                            }
                        }
                        if (GraylistedSafeURLs.Count > 0) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\blacklisted");
                            if (DownloadInfo.SeparateNonImages) {
                                if (GraylistedSafeNonImages[0]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\blacklisted\\gif");
                                }
                                if (GraylistedSafeNonImages[1]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\blacklisted\\apng");
                                }
                                if (GraylistedSafeNonImages[2]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\blacklisted\\webm");
                                }
                                if (GraylistedSafeNonImages[3]) {
                                    Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\safe\\blacklisted\\swf");
                                }
                            }
                        }
                    }
                }
                else {
                    Directory.CreateDirectory(DownloadInfo.DownloadPath);
                    if (DownloadInfo.SeparateNonImages) {
                        if (FileNonImages[0]) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\gif");
                        }
                        if (FileNonImages[1]) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\apng");
                        }
                        if (FileNonImages[2]) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\webm");
                        }
                        if (FileNonImages[3]) {
                            Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\swf");
                        }
                    }

                    if (DownloadInfo.SaveBlacklistedFiles && GraylistedURLs.Count > 0) {
                        Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted");
                        if (DownloadInfo.SeparateNonImages) {
                            if (GraylistedFileNonImages[0]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted\\gif");
                            }
                            if (GraylistedFileNonImages[1]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted\\apng");
                            }
                            if (GraylistedFileNonImages[2]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted\\webm");
                            }
                            if (GraylistedFileNonImages[3]) {
                                Directory.CreateDirectory(DownloadInfo.DownloadPath + "\\blacklisted\\swf");
                            }
                        }
                    }
                }
            #endregion

            #region pre-download logic
            // Update totals
                if (DownloadInfo.SeparateRatings) {
                    cleanTotalCount = cleanExplicitCount + cleanQuestionableCount + cleanSafeCount;
                    graylistTotalCount = graylistExplicitCount + graylistQuestionableCount + graylistSafeCount;
                }

            // Save the info files from the buffer
                if (DownloadInfo.SaveInfo) {
                    TagInfoBuffer.TrimEnd('\r').TrimEnd('\n');
                    File.WriteAllText(DownloadInfo.DownloadPath + "\\tags.nfo", TagInfoBuffer, Encoding.UTF8);

                    if (DownloadInfo.SaveBlacklistedFiles && graylistTotalCount > 0) {
                        BlacklistInfoBuffer.TrimEnd('\r').TrimEnd('\n');
                        if (DownloadInfo.SeparateRatings)
                            File.WriteAllText(DownloadInfo.DownloadPath + "\\tags.blacklisted.nfo", BlacklistInfoBuffer, Encoding.UTF8);
                        else
                            File.WriteAllText(DownloadInfo.DownloadPath + "\\blacklisted\\tags.blacklisted.nfo", BlacklistInfoBuffer, Encoding.UTF8);
                    }
                }

            // Set the progressbar style.
                this.BeginInvoke(new MethodInvoker(() => {
                    string labelBuffer = "";
                    if (DownloadInfo.SeparateRatings) {
                        object[] counts = new object[] {
                            cleanTotalCount.ToString(),                 // 0
                            cleanExplicitCount.ToString(),              // 1
                            cleanQuestionableCount.ToString(),          // 2
                            cleanSafeCount.ToString(),                  // 3
                            graylistTotalCount.ToString(),              // 4
                            graylistExplicitCount.ToString(),           // 5
                            graylistQuestionableCount.ToString(),       // 6
                            graylistSafeCount.ToString(),               // 7
                            blacklistCount.ToString(),                  // 8
                            totalCount.ToString(),                      // 9
                            cleanTotalExistCount.ToString(),            // 10
                            cleanExplicitExistCount.ToString(),         // 11
                            cleanQuestionableExistCount.ToString(),     // 12
                            cleanSafeExistCount.ToString(),             // 13
                            graylistTotalExistCount.ToString(),         // 14
                            graylistExplicitExistCount.ToString(),      // 15
                            graylistQuestionableExistCount.ToString(),  // 16
                            graylistSafeExistCount.ToString()           // 17
                        };

                        labelBuffer = string.Format("Files: {0} ( {1} E | {2} Q | {3} S )\r\n" +
                                                    "Blacklisted: {4} ( {5} E | {6} Q | {7} S )\r\n" +
                                                    "Zero Tolerance: {8}\r\n" +
                                                    "Total: {9}\r\n\r\n" +
                                                    "Files that exist: {10} ( {11} E | {12} Q | {13} S )\r\n" +
                                                    "Blacklisted that exist: {14} ( {15} E | {16} Q | {17} S )", counts);
                    }
                    else {
                        object[] counts = new object[] {
                            cleanTotalCount.ToString(),
                            graylistTotalCount.ToString(),
                            blacklistCount.ToString(),
                            totalCount.ToString(),
                            cleanTotalExistCount.ToString(),
                            graylistTotalExistCount.ToString()
                        };

                        labelBuffer = string.Format("Files: {0}\r\n" +
                                                    "Blacklisted: {1}\r\n" +
                                                    "Zero Tolerance: {2}\r\n" +
                                                    "Total: {3}\r\n\r\n" +
                                                    "Files that exist: {4}\r\n" +
                                                    "Blacklisted that exist: {5}", counts);
                    }
                    lbBlacklist.Text = labelBuffer;

                    if (DownloadInfo.SaveBlacklistedFiles)
                        pbTotalStatus.Maximum = cleanTotalCount + graylistTotalCount;
                    else
                        pbTotalStatus.Maximum = cleanTotalCount;

                    pbDownloadStatus.Style = ProgressBarStyle.Blocks;
                }));
            #endregion

            #region download
            // Start the download
                using (ExWebClient wc = new ExWebClient()) {
                    wc.DownloadProgressChanged += (s, e) => {
                        if (!this.IsDisposed) {
                            //if (!sizeRecieved) {
                            //    //this.Invoke((MethodInvoker)(() => pbDownloadStatus.Maximum = 101));
                            //    if (!lbFile.Text.Contains(("(" + e.TotalBytesToReceive / 1024) + "kb)"))
                            //        this.Invoke((MethodInvoker)(() => lbFile.Text += " (" + (e.TotalBytesToReceive / 1024) + "kb)"));
                            //    sizeRecieved = true;
                            //    apiTools.SendDebugMessage((e.TotalBytesToReceive / 1024).ToString());
                            //}
                            this.BeginInvoke(new MethodInvoker(() => {
                                pbDownloadStatus.Value = e.ProgressPercentage;
                                pbDownloadStatus.Value++;
                                pbDownloadStatus.Value--;
                                lbPercentage.Text = e.ProgressPercentage.ToString() + "%";

                                lbBytes.Text = (e.BytesReceived / 1024) + " kb / " + (e.TotalBytesToReceive / 1024) + " kb";
                            }));
                        }
                    };
                    wc.DownloadFileCompleted += (s, e) => {
                        if (!pbDownloadStatus.IsDisposed && !lbPercentage.IsDisposed) {
                            lock (e.UserState) {
                                this.BeginInvoke(new MethodInvoker(() => {
                                    pbDownloadStatus.Value = 0;
                                    lbPercentage.Text = "0%";

                                    // protect from overflow
                                    if (pbTotalStatus.Value != pbTotalStatus.Maximum)
                                        pbTotalStatus.Value++;
                                    else
                                        lbRemoved.Visible = true;
                                }));
                                Monitor.Pulse(e.UserState);
                            }
                        }
                    };

                    var sync = new Object();

                    wc.Proxy = WebRequest.GetSystemWebProxy();
                    wc.Headers.Add("user-agent", Program.UserAgent);
                    wc.Method = "GET";

                    if (DownloadInfo.SeparateRatings) {
                        if (ExplicitURLs.Count > 0) {
                            for (int y = 0; y < ExplicitURLs.Count; y++) {
                                if (string.IsNullOrEmpty(ExplicitURLs[y])) { continue; }
                                CurrentUrl = ExplicitURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading explicit file " + (y + 1) + " of " + (cleanExplicitCount)));

                                string fileName = ExplicitFileNames[y];
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

                        if (QuestionableURLs.Count > 0) {
                            for (int y = 0; y < QuestionableURLs.Count; y++) {
                                if (string.IsNullOrEmpty(QuestionableURLs[y])) { continue; }
                                CurrentUrl = QuestionableURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading questionable file " + (y + 1) + " of " + (cleanQuestionableCount)));

                                string fileName = QuestionableFileNames[y];
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

                        if (SafeURLs.Count > 0) {
                            for (int y = 0; y < SafeURLs.Count; y++) {
                                if (string.IsNullOrEmpty(SafeURLs[y])) { continue; }
                                CurrentUrl = SafeURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading safe file " + (y + 1) + " of " + (cleanSafeCount)));

                                string fileName = SafeFileNames[y];
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

                        if (DownloadInfo.SaveBlacklistedFiles) {
                            if (GraylistedExplicitURLs.Count > 0) {
                                for (int y = 0; y < GraylistedExplicitURLs.Count; y++) {
                                    if (string.IsNullOrEmpty(GraylistedExplicitURLs[y])) { continue; }
                                    CurrentUrl = GraylistedExplicitURLs[y];
                                    this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (e) file " + (y + 1) + " of " + (graylistExplicitCount)));

                                    string fileName = GraylistedExplicitFileNames[y];
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
                        }

                        if (GraylistedQuestionableURLs.Count > 0) {
                            for (int y = 0; y < GraylistedQuestionableURLs.Count; y++) {
                                if (string.IsNullOrEmpty(GraylistedQuestionableURLs[y])) { continue; }
                                CurrentUrl = GraylistedQuestionableURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (q) file " + (y + 1) + " of " + (graylistQuestionableCount)));

                                string fileName = GraylistedQuestionableFileNames[y];
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

                        if (GraylistedSafeURLs.Count > 0) {
                            for (int y = 0; y < GraylistedSafeURLs.Count; y++) {
                                if (string.IsNullOrEmpty(GraylistedSafeURLs[y])) { continue; }
                                CurrentUrl = GraylistedSafeURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (s) file " + (y + 1) + " of " + (graylistSafeCount)));

                                string fileName = GraylistedSafeFileNames[y];
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
                    else {
                        if (URLs.Count > 0) {
                            for (int y = 0; y < URLs.Count; y++) {
                                if (string.IsNullOrEmpty(URLs[y])) { continue; }
                                CurrentUrl = URLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading file " + (y + 1) + " of " + (cleanTotalCount)));

                                string fileName = FileNames[y];
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

                        if (DownloadInfo.SaveBlacklistedFiles && GraylistedURLs.Count > 0) {
                            for (int y = 0; y < GraylistedURLs.Count; y++) {
                                if (string.IsNullOrEmpty(GraylistedURLs[y])) { continue; }
                                CurrentUrl = GraylistedURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted file " + (y) + " of " + (graylistTotalCount)));

                                string fileName = GraylistedFileNames[y];
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
            #endregion

            #region post download
                if (DownloadInfo.SeparateRatings) {
                    TotalFiles += (ExplicitURLs.Count + QuestionableURLs.Count + SafeURLs.Count);
                    PresentFiles += cleanTotalCount;
                    if (DownloadInfo.SaveBlacklistedFiles) {
                        TotalFiles += (GraylistedExplicitURLs.Count + GraylistedQuestionableURLs.Count + GraylistedSafeURLs.Count);
                        PresentFiles += graylistTotalCount;
                    }
                }
                else {
                    TotalFiles += URLs.Count;
                    PresentFiles += cleanTotalCount;
                    if (DownloadInfo.SaveBlacklistedFiles) {
                        TotalFiles += GraylistedURLs.Count;
                        PresentFiles += graylistTotalCount;
                    }
                }

                DownloadHasFinished = true;
            #endregion
            }
            #endregion

            #region Downloader catch-statements
            catch (ThreadAbortException) {
                apiTools.SendDebugMessage("Thread was requested to be, and has been, aborted. (frmTagDownloader.cs)");
                DownloadHasAborted = true;
            }
            catch (ObjectDisposedException) {
                apiTools.SendDebugMessage("Seems like the object got disposed. (frmTagDownloader.cs)");
                DownloadHasErrored = true;
            }
            catch (ApiReturnedNullOrEmptyException) {
                apiTools.SendDebugMessage("Api returned null or empty. (frmTagDownloader.cs)");
                DownloadHasErrored = true;
            }
            catch (WebException WebE) {
                apiTools.SendDebugMessage("A WebException has occured. (frmTagDownloader.cs)");
                this.BeginInvoke(new MethodInvoker(() => {
                    status.Text = "A WebException has occured";
                    pbDownloadStatus.State = ProgressBarState.Error;
                    pbTotalStatus.State = ProgressBarState.Error;
                }));
                DownloadHasErrored = true;
                ErrorLog.ReportWebException(WebE,CurrentUrl, "frmTagDownloader.cs");
            }
            catch (Exception ex) {
                apiTools.SendDebugMessage("A general exception has occured. (frmTagDownloader.cs)");
                this.BeginInvoke(new MethodInvoker(() => {
                    status.Text = "A Exception has occured";
                    pbDownloadStatus.State = ProgressBarState.Error;
                    pbTotalStatus.State = ProgressBarState.Error;
                }));
                DownloadHasErrored = true;
                ErrorLog.ReportException(ex, "frmTagDownloader.cs");
            }
            #endregion

            #region After download finally statement
            finally {
                this.BeginInvoke(new MethodInvoker(() => {
                    AfterDownload();
                }));
            }
            #endregion
        }

        private void ChangeTask(string curStatus) {
            this.BeginInvoke(new MethodInvoker(() => {
                status.Text = curStatus;
            }));
        }
        #endregion

    }
}