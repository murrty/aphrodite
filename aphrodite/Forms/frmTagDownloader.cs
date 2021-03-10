using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace aphrodite {
    public partial class frmTagDownloader : Form {

    #region Public Variables
        public string tags = string.Empty;          // The tags that will be downloaded.
        public string downloadUrl = string.Empty;   // The URL that will be downloaded.
        public string webHeader = string.Empty;     // The WebClient header.

        public string graylist = string.Empty;      // The graylist (Will be downloaded into separate folder).
        public string blacklist = string.Empty;     // The blacklist (Will never be downloaded or saved into tags.nfo).

        public bool fromURL = false;                // Determines if the page is being downloaded or not.

        public string saveTo = String.Empty;        // Global setting for the save directory for the downloader.
        public bool openAfter = false;              // Global setting for opening the folder after download.
        public bool saveBlacklistedFiles = true;    // Global setting for saving graylisted files..
        public bool ignoreFinish = false;           // Global setting for closing after finishing

        public bool useMinimumScore = false;        // Setting for using minimum score.
        public bool scoreAsTag = false;             // Setting for using the score as a tag.
        public int minimumScore = 0;                // Setting for the minimum score.
        public int imageLimit = 0;                  // Setting for image limit.
        public int pageLimit = 0;                   // Setting for page limit.
        public bool saveInfo = false;               // Setting for saving the info.
        public string[] ratings = null;             // Setting for the ratings.
        public bool separateRatings = true;         // Setting for separating the ratings.
        public bool separateNonImages = true;      // Setting for separating non images (gif, apng, webm, swf)
        public bool skipExistingFile = false;       // Skip files if they exist
        public string fileNameSchema = "%md5%";     // The schema used for the file name.
                                                    // %md5%        = the md5 of the file
                                                    // %id%         = the id of the page
                                                    // %rating%     = the rating of the image (eg: safe)
                                                    // %rating2%    = the lettered rating of the image (eg: s)
                                                    // %artist%     = the first artist in the artists array
                                                    // %ext%        = the extension
                                                    // %fav_count%  = the amount of favorites the post has
                                                    // %score%      = the score of the post
                                                    // %author%     = the user who submitted the post to e621

        //public static readonly string tagJson = "https://e621.net/post/index.json?tags="; // Old API url.
        public static readonly string tagJson = "https://e621.net/posts.json?tags=";        // Updated API url.
        public static readonly string limitJson = "&limit=320";                             // Maximum limit suffix
        public static readonly string pageJson = "&page=";                                  // Page suffix.
        public static readonly string imgUrl = "https://static1.e621.net/data/";

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
            this.Icon = Properties.Resources.Brad;
        }
        private void frmDownload_Load(object sender, EventArgs e) {
            if (fromURL)
                txtTags.Text = downloadUrl.Split('/')[6].Replace("%20", " ");
            else
                txtTags.Text = tags.Replace("%25-2F", "/");

            lbBlacklist.Text = "No file counts.\nWaiting for first json parse.";

            string minScore = "Minimum score: disabled";
            string imgLim = "Image limit: disabled";
            string pageLim = "Page limit: disabled";

            int tagCount = tags.Split(' ').Length;

            if (tagCount == 6 && scoreAsTag)
                scoreAsTag = false;

            if (useMinimumScore)
                minScore = "Minimum score: " + minimumScore.ToString();

            if (scoreAsTag)
                minScore += " (as tag)";

            if (imageLimit > 0)
                imgLim = "Image limit: " + imageLimit.ToString() + "images";

            if (pageLimit > 0)
                pageLim = "Page limit: " + pageLimit.ToString() + " pages";

            string ratingBuffer = "\nRatings: ";
            if (ratings.Contains("e"))
                ratingBuffer += "e";
            if (ratings.Contains("q")) {
                if (ratingBuffer.EndsWith("e"))
                    ratingBuffer += ", ";

                ratingBuffer += "q";
            }
            if (ratings.Contains("s")) {
                if (ratingBuffer.EndsWith("e") || ratingBuffer.EndsWith("q"))
                    ratingBuffer += ", ";

                ratingBuffer += "s";
            }

            if (separateRatings)
                ratingBuffer += " (separating)";
            else
                ratingBuffer = string.Empty;

            lbLimits.Text = minScore + "\n" + imgLim + "\n" + pageLim + ratingBuffer;
        }
        private void frmDownload_Shown(object sender, EventArgs e) {
            startDownload();
            this.CenterToScreen();
        }
        private void frmDownload_FormClosing(object sender, FormClosingEventArgs e) {
            if (!DownloadHasFinished && !DownloadHasErrored && !DownloadHasAborted) {
                tmrTitle.Stop();
                DownloadHasAborted = true;

                if (tagDownload.IsAlive) {
                    tagDownload.Abort();
                }

                if (!ignoreFinish) {
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
        private void startDownload() {
            try {
                tagDownload = new Thread(() => {
                    Thread.CurrentThread.IsBackground = true;

                    if (fromURL) {
                        if (downloadPage(downloadUrl)) {
                            if (ignoreFinish)
                                this.DialogResult = DialogResult.OK;
                        }
                        else
                            this.DialogResult = DialogResult.Abort;
                    }
                    else {
                        downloadTags();
                    }
                });

                tagDownload.Start();
                tmrTitle.Start();
            }
            catch (ObjectDisposedException obDis) {
                MessageBox.Show("Caught a disposed object exception.\n\n" + obDis.ToString());
                this.DialogResult = DialogResult.Abort;
            }
        }

        private void AfterDownload(int ErrorType = 0) {
            if (ignoreFinish) {
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
                this.Text = "Finished downloading tags " + tags;
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

        private void downloadTags() {
            changeTask("Awaiting API call");
            #region Download variables
            string url = string.Empty;                                          // The URL being accessed, changes per API call/File download.

            string tagInfo = string.Empty;                                      // The buffer for the 'tag.nfo' file that will be created.
            string blacklistInfo = string.Empty;                                // The buffer for the 'tag.blacklisted.nfo' file that will be created.

            string xml = string.Empty;                                          // The XML string.

            List<string> GraylistedTags = new List<string>();                   // The list of files that will be downloaded into a separate folder (if saveBlacklisted = true).
            List<string> BlacklistedTags = new List<string>();                  // The list of files that will be skipped entirely.

            List<string> URLs = new List<string>();                             // The URLs that will be downloaded (if separateRatings = false).
            List<string> GraylistedURLs = new List<string>();                   // The Blacklisted URLs that will be downloaded (if separateRatings = false).

            List<string> FileNames = new List<string>();                        // Contains the file names of the images
            List<string> GraylistedFileNames = new List<string>();              // Contains the file names of the graylisted images

            List<bool> FileExists = new List<bool>();
            List<bool> GraylistedFileExists = new List<bool>();

            List<string> ExplicitURLs = new List<string>();                     // The list of Explicit files.
            List<string> ExplicitFileNames = new List<string>();                // The list of Explicit file names.
            List<bool> ExplicitFileExists = new List<bool>();

            List<string> QuestionableURLs = new List<string>();                 // The list of Questionable files.
            List<string> QuestionableFileNames = new List<string>();            // The list of Questionable file names.
            List<bool> QuestionableFileExists = new List<bool>();

            List<string> SafeURLs = new List<string>();                         // The list of Safe files.
            List<string> SafeFileNames = new List<string>();                    // The list of Safe file names.
            List<bool> SafeFileExists = new List<bool>();

            List<string> GraylistedExplicitURLs = new List<string>();           // The list of Graylisted Explicit files.
            List<string> GraylistedExplicitFileNames = new List<string>();      // The list of Graylisted Explicit file names.
            List<bool> GraylistedExplicitFileExists = new List<bool>();

            List<string> GraylistedQuestionableURLs = new List<string>();       // The list of Graylisted Questionable files.
            List<string> GraylistedQuestionableFileNames = new List<string>();  // The list of Graylitsed Questionable file names.
            List<bool> GraylistedQuestionableFileExists = new List<bool>();

            List<string> GraylistedSafeURLs = new List<string>();               // The list of Graylisted Safe files.
            List<string> GraylistedSafeFileNames = new List<string>();          // The list of Graylisted Safe file names.
            List<bool> GraylistedSafeFileExists = new List<bool>();

            int tagLength = 0;                                                  // Will be the count of tags being downloaded (1-6).
            int pageCount = 1;                                                  // Will be the count of the pages parsed.

            int count = -1;
            int presentCount = -1;

            bool morePages = false;                                             // Will determine if there are more than 1 page.

            #endregion

            #region Downloader try-statement
            try {
            #region initialization
            //Properties.Settings.Default.Log += "Tag downloader starting for tags " + tags + "\n";
            // Set the saveTo.
                string newTagName = apiTools.ReplaceIllegalCharacters(tags);
                if (useMinimumScore)                                                                    // Add minimum score to folder name.
                    newTagName += " (scores " + (minimumScore) + "+)";
                if (!this.saveTo.EndsWith("\\Tags\\" + newTagName))                                     // Set the output folder.
                    this.saveTo += "\\Tags\\" + newTagName;

            // Start the buffer for the .nfo files.
                if (useMinimumScore) {
                    tagInfo = "TAGS: " + tags + "\n" +
                              "MINIMUM SCORE: " + minimumScore + "\n" +
                              "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") +
                              "\n\n";

                    blacklistInfo = "TAGS: " + tags + "\n" +
                                    "MINIMUM SCORE: " + minimumScore + "\n" +
                                    "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\n" +
                                    "BLACKLISTED TAGS: " + graylist +
                                    "\n\n";
                }
                else {
                    tagInfo = "TAGS: " + tags + "\n" +
                              "MINIMUM SCORE: n/a\n" +
                              "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") +
                              "\n\n";

                    blacklistInfo = "TAGS: " + tags + "\n" +
                                    "MINIMUM SCORE: n/a\n" +
                                    "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\n" +
                                    "BLACKLISTED TAGS: " + graylist +
                                    "\n\n";
                }

            // Get the tag length.
                tagLength = tags.Split(' ').Length;

            // Add the minimum score to the search tags (if applicable).
                if (tagLength < 6 && scoreAsTag) {
                    tags += " score:>" + (minimumScore - 1);
                }

            // Set the blacklist.. lists...
                if (!string.IsNullOrWhiteSpace(graylist))
                    GraylistedTags = graylist.Split(' ').ToList();
                if (!string.IsNullOrWhiteSpace(blacklist))
                    BlacklistedTags = blacklist.Split(' ').ToList();
            #endregion

            #region first page of the tag
            // Get XML of page.
                changeTask("Downloading tag information for page 1...");
                url = tagJson + tags + limitJson;
                xml = apiTools.GetJSON(url, webHeader);
                if (apiTools.IsXmlDead(xml)) {
                    throw new ApiReturnedNullOrEmptyException(ApiReturnedNullOrEmptyException.ReportedEmpty);
                }
            #endregion

            #region first page parsing
            // Parse the XML file.
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNodeList xmlID = doc.DocumentElement.SelectNodes("/root/posts/item/id");
                XmlNodeList xmlMD5 = doc.DocumentElement.SelectNodes("/root/posts/item/file/md5");
                XmlNodeList xmlURL = doc.DocumentElement.SelectNodes("/root/posts/item/file/url");
                XmlNodeList xmlTagsGeneral = doc.DocumentElement.SelectNodes("/root/posts/item/tags/general");
                XmlNodeList xmlTagsSpecies = doc.DocumentElement.SelectNodes("/root/posts/item/tags/species");
                XmlNodeList xmlTagsCharacter = doc.DocumentElement.SelectNodes("/root/posts/item/tags/character");
                XmlNodeList xmlTagsCopyright = doc.DocumentElement.SelectNodes("/root/posts/item/tags/copyright");
                XmlNodeList xmlTagsArtist = doc.DocumentElement.SelectNodes("/root/posts/item/tags/artist");
                XmlNodeList xmlTagsInvalid = doc.DocumentElement.SelectNodes("/root/posts/item/tags/invalid");
                XmlNodeList xmlTagsLore = doc.DocumentElement.SelectNodes("/root/posts/item/tags/lore");
                XmlNodeList xmlTagsMeta = doc.DocumentElement.SelectNodes("/root/posts/item/tags/meta");
                XmlNodeList xmlTagsLocked = doc.DocumentElement.SelectNodes("/root/posts/item/locked_tags");
                XmlNodeList xmlScore = doc.DocumentElement.SelectNodes("/root/posts/item/score/total");
                XmlNodeList xmlScoreUp = doc.DocumentElement.SelectNodes("/root/posts/item/score/up");
                XmlNodeList xmlScoreDown = doc.DocumentElement.SelectNodes("/root/posts/item/score/down");
                XmlNodeList xmlFavCount = doc.DocumentElement.SelectNodes("/root/posts/item/fav_count");
                XmlNodeList xmlRating = doc.DocumentElement.SelectNodes("/root/posts/item/rating");
                XmlNodeList xmlAuthor = doc.DocumentElement.SelectNodes("/root/posts/item/uploader_id");
                XmlNodeList xmlDescription = doc.DocumentElement.SelectNodes("/root/posts/item/description");
                XmlNodeList xmlExt = doc.DocumentElement.SelectNodes("/root/posts/item/file/ext");
                XmlNodeList xmlDeleted = doc.DocumentElement.SelectNodes("/root/posts/item/flags/deleted");

            // Determine the pages by counting the posts.
                int itemCount = xmlID.Count;
                if (itemCount == 320) {
                //if (itemCount == 10) { // Debug count
                    morePages = true;
                    pageCount++;
                }

            // Begin parsing the XML for tag information per item.
                for (int i = 0; i < xmlID.Count; i++) {
                    string artists = string.Empty;                                          // The artists that worked on the file.
                    //Properties.Settings.Default.Log += "Finding rating at " + i + "\n";
                    string rating = xmlRating[i].InnerText;                                 // Get the rating of the current file.
                    bool isGraylisted = false;                                              // Will determine if the file is graylisted.
                    bool isBlacklisted = false;                                             // Will determine if the file is blacklisted.
                    bool alreadyExists = false;                                             // Will determine if the file exists.
                    List<string> foundTags = new List<string>();                            // Get the entire tag list of the file.
                    string foundGraylistedTags = string.Empty;                              // The buffer for the tags that are graylisted.
                    string ReadTags = string.Empty;

                // Check the image limit & break if reached.
                    if (imageLimit > 0 && imageLimit == totalCount) {
                        morePages = false;
                        break;
                    }

                // Create new tag list to merge all the tag groups into one.
                    ReadTags += "          General: [";
                    for (int x = 0; x < xmlTagsGeneral[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsGeneral[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsGeneral[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n          Species: [";
                    for (int x = 0; x < xmlTagsSpecies[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsSpecies[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsSpecies[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n          Character: [";
                    for (int x = 0; x < xmlTagsCharacter[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsCharacter[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsCharacter[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n          Copyright: [";
                    for (int x = 0; x < xmlTagsCopyright[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsCopyright[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsCopyright[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n          Artist: [";
                    for (int x = 0; x < xmlTagsArtist[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsArtist[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsArtist[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n          Invalid: [";
                    for (int x = 0; x < xmlTagsInvalid[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsInvalid[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsInvalid[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n          Lore: [";
                    for (int x = 0; x < xmlTagsLore[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsLore[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsLore[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n          Meta: [";
                    for (int x = 0; x < xmlTagsMeta[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsMeta[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsMeta[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n          Locked tags: [";
                    for (int x = 0; x < xmlTagsLocked[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsLocked[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsLocked[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n";

                // Set the rating for the .nfo file.
                    if (rating == "e")
                        rating = "Explicit";
                    else if (rating == "q")
                        rating = "Questionable";
                    else if (rating == "s")
                        rating = "Safe";
                    else
                        rating = "Unknown";

                // Set the description
                    string imageDescription = " No description";
                    if (xmlDescription[i].InnerText != "") {
                        imageDescription = "\n                \"" + xmlDescription[0].InnerText + "\"";
                    }

                // Gets the artists of the file and then trims the end of garbage.
                    for (int j = 0; j < xmlTagsArtist[i].ChildNodes.Count; j++)
                        artists += xmlTagsArtist[i].ChildNodes[j].InnerText + "\n               ";
                    artists = artists.TrimEnd(' ');
                    artists = artists.TrimEnd('\n');

                // File name artist for the schema
                    string fileNameArtist = "(none)";
                    bool useHardcodedFilter = false;
                    if (string.IsNullOrEmpty(General.Default.undesiredTags))
                        useHardcodedFilter = true;

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

                    if (xmlTagsArtist[i].ChildNodes.Count > 0) {
                        if (!string.IsNullOrEmpty(xmlTagsArtist[i].ChildNodes[0].InnerText)) {
                            fileNameArtist = xmlTagsArtist[i].ChildNodes[0].InnerText;
                        }
                    }

                    string fileName = fileNameSchema.Replace("%md5%", xmlMD5[i].InnerText)
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
                            //fileUrl = imgUrl + xmlMD5[i].InnerText.Substring(0, 2) + "/" + xmlMD5[i].InnerText.Substring(2, 2) + "." + xmlExt[i].InnerText;
                            fileUrl = apiTools.GetBlacklistedImageUrl(xmlMD5[i].InnerText, xmlExt[i].InnerText);
                        }
                    }
                    

                // Check for blacklisted and graylisted tags.
                    for (int j = 0; j < foundTags.Count; j++) {
                        if (BlacklistedTags.Count > 0) {
                            for (int k = 0; k < BlacklistedTags.Count; k++) {
                                if (foundTags[j] == BlacklistedTags[k]) {
                                    isBlacklisted = true;
                                    continue;
                                }
                            }
                        }

                        if (isBlacklisted)
                            break;

                        if (GraylistedTags.Count > 0) {
                            for (int k = 0; k < GraylistedTags.Count; k++) {
                                if (foundTags[j] == GraylistedTags[k]) {
                                    isGraylisted = true;
                                    foundGraylistedTags += GraylistedTags[k] + " ";
                                }
                            }
                        }
                    }

                // Add to the counts (and break for blacklisted)
                    string outputDir = saveTo;
                    if (isBlacklisted) {
                        blacklistCount++;
                        totalCount++;
                    }
                    else if (isGraylisted) {
                        if (separateRatings) {
                            outputDir += "\\" + rating.ToLower() + "\\blacklisted\\";
                            if (separateNonImages) {
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
                            alreadyExists = File.Exists(outputDir);
                        }
                        else {
                            outputDir += "\\blacklisted\\";
                            if (separateNonImages) {
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
                            alreadyExists = File.Exists(outputDir);
                        }
                        

                        switch (xmlRating[i].InnerText.ToLower()) {
                            case "e":
                                if (alreadyExists) {
                                    graylistExplicitExistCount++;
                                }
                                else {
                                    graylistExplicitCount++;
                                }
                                break;
                            case "q":
                                if (alreadyExists) {
                                    graylistQuestionableExistCount++;
                                }
                                else {
                                    graylistQuestionableCount++;
                                }
                                break;
                            case "s":
                                if (alreadyExists) {
                                    graylistSafeExistCount++;
                                }
                                else {
                                    graylistSafeCount++;
                                }
                                break;
                            default:
                                MessageBox.Show("An error occured when determining the rating. Open an issue.\n\nFound rating: " + xmlRating[i].InnerText);
                                break;
                        }

                        if (alreadyExists) {
                            graylistTotalExistCount++;
                        }
                        else {
                            graylistTotalCount++;
                        }
                        totalCount++;
                    }
                    else {
                        if (separateRatings) {
                            outputDir += "\\" + rating.ToLower() + "\\";
                            if (separateNonImages) {
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
                            alreadyExists = File.Exists(outputDir);
                        }
                        else {
                            outputDir += "\\";
                            if (separateNonImages) {
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
                            alreadyExists = File.Exists(outputDir);
                        }

                        switch (xmlRating[i].InnerText.ToLower()) {
                            case "e":
                                if (alreadyExists) {
                                    cleanExplicitExistCount++;
                                }
                                else {
                                    cleanExplicitCount++;
                                }
                                break;
                            case "q":
                                if (alreadyExists) {
                                    cleanQuestionableExistCount++;
                                }
                                else {
                                    cleanQuestionableCount++;
                                }
                                break;
                            case "s":
                                if (alreadyExists) {
                                    cleanSafeExistCount++;
                                }
                                else {
                                    cleanSafeCount++;
                                }
                                break;
                            default:
                                MessageBox.Show("An error occured when determining the rating. Open an issue.\n\nFound rating: " + xmlRating[i].InnerText);
                                break;
                        }

                        if (alreadyExists) {
                            cleanTotalExistCount++;
                        }
                        else {
                            cleanTotalCount++;
                        }
                        totalCount++;
                    }

                    if (isBlacklisted)
                        continue;

                // Graylist check & options check
                    if (isGraylisted) {
                        if (saveBlacklistedFiles) {
                            if (useMinimumScore && Int32.Parse(xmlScore[i].InnerText) < minimumScore)
                                continue;

                            if (!ratings.Any(xmlRating[i].InnerText.Contains))
                                continue;
                        }
                        else {
                            continue;
                        }
                    }
                    else { // Not blacklisted
                        if (useMinimumScore && Int32.Parse(xmlScore[i].InnerText) < minimumScore)
                            continue;

                        if (!ratings.Any(xmlRating[i].InnerText.Contains))
                            continue;
                    }

                // Start adding to the nfo buffer and URL lists
                    if (isGraylisted && saveBlacklistedFiles) {
                        if (separateRatings) {
                            if (xmlRating[i].InnerText == "e")  {
                                //GraylistedExplicitURLs.Add(xmlURL[i].InnerText);
                                GraylistedExplicitURLs.Add(fileUrl);
                                GraylistedExplicitFileNames.Add(fileName);
                                GraylistedExplicitFileExists.Add(alreadyExists);
                            }
                            else if (xmlRating[i].InnerText == "q") {
                                //GraylistedQuestionableURLs.Add(xmlURL[i].InnerText);
                                GraylistedQuestionableURLs.Add(fileUrl);
                                GraylistedQuestionableFileNames.Add(fileName);
                                GraylistedQuestionableFileExists.Add(alreadyExists);
                            }
                            else if (xmlRating[i].InnerText == "s") {
                                //GraylistedSafeURLs.Add(xmlURL[i].InnerText);
                                GraylistedSafeURLs.Add(fileUrl);
                                GraylistedSafeFileNames.Add(fileName);
                                GraylistedSafeFileExists.Add(alreadyExists);
                            }
                        }
                        else {
                            //GraylistedURLs.Add(xmlURL[i].InnerText);
                            GraylistedURLs.Add(fileUrl);
                            GraylistedFileNames.Add(fileName);
                            GraylistedFileExists.Add(alreadyExists);
                        }

                        blacklistInfo += "POST " + xmlID[i].InnerText + ":\n" +
                                         "    MD5: " + xmlMD5[i].InnerText + "\n" +
                                         "    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n" +
                                         "    ARTIST(S): " + artists + "\n" +
                                         "    TAGS: " + ReadTags + //string.Concat(foundTags.ToArray()) + "\n" +
                                         "    OFFENDING TAGS: " + foundGraylistedTags +
                                         "    SCORE: Up " + xmlScoreUp[i].InnerText + ", Down " + xmlScoreDown[i].InnerText + ", Total " + xmlScore[i].InnerText + "\n" +
                                         "    RATING: " + rating + "\n" +
                                         "    DESCRIPITON:" + imageDescription +
                                         "\n\n";
                    }
                    else {
                        if (separateRatings) {
                            if (xmlRating[i].InnerText == "e") {
                                //ExplicitURLs.Add(xmlURL[i].InnerText);
                                ExplicitURLs.Add(fileUrl);
                                ExplicitFileNames.Add(fileName);
                                ExplicitFileExists.Add(alreadyExists);
                            }
                            else if (xmlRating[i].InnerText == "q") {
                                //QuestionableURLs.Add(xmlURL[i].InnerText);
                                QuestionableURLs.Add(fileUrl);
                                QuestionableFileNames.Add(fileName);
                                QuestionableFileExists.Add(alreadyExists);
                            }
                            else if (xmlRating[i].InnerText == "s") {
                                //SafeURLs.Add(xmlURL[i].InnerText);
                                SafeURLs.Add(fileUrl);
                                SafeFileNames.Add(fileName);
                                SafeFileExists.Add(alreadyExists);
                            }
                        }
                        else {
                            //URLs.Add(xmlURL[i].InnerText);
                            URLs.Add(fileUrl);
                            FileNames.Add(fileName);
                            FileExists.Add(alreadyExists);
                        }

                        tagInfo += "POST " + xmlID[i].InnerText + ":\n" +
                                   "    MD5: " + xmlMD5[i].InnerText + "\n" +
                                   "    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n" +
                                   "    ARTIST(S): " + artists + "\n" +
                                   "    TAGS:\n" + ReadTags + //string.Concat(foundTags.ToArray()) + "\n" +
                                   "    SCORE: Up " + xmlScoreUp[i].InnerText + ", Down " + xmlScoreDown[i].InnerText + ", Total " + xmlScore[i].InnerText + "\n" +
                                   "    RATING: " + rating + "\n" +
                                   "    DESCRIPITON:" + imageDescription +
                                   "\n\n";
                    }
                }


                this.BeginInvoke(new MethodInvoker(() => {
                    string labelBuffer = "";
                    if (separateRatings) {
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

                        labelBuffer = string.Format("Files: {0} ( {1} E | {2} Q | {3} S )\n" +
                                                    "Blacklisted: {4} ( {5} E | {6} Q | {7} S )\n" +
                                                    "Zero Tolerance: {8}\n" +
                                                    "Total: {9}\n\n" +
                                                    "Files that exist: {10} ( {11} E | {12} Q | {13} S )\n" +
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

                        labelBuffer = string.Format("Files: {0}\n" +
                                                    "Blacklisted: {1}\n" +
                                                    "Zero Tolerance: {2}\n" +
                                                    "Total: {3}\n\n" +
                                                    "Files that exist: {4}\n" +
                                                    "Blacklisted that exist: {5}", counts);
                    }
                    lbBlacklist.Text = labelBuffer;
                }));
            #endregion

            #region parse extra pages past the initial 320 images (copy + paste of the first block)
            // Check for extra pages and then parse them as well.
                if (morePages) {
                    //Properties.Settings.Default.Log += "More pages detected\n";
                    bool deadPage = false;
                    while (!deadPage) {
                        changeTask("Downloading tag information for page " + (pageCount) + "...");
                        url = tagJson + tags + limitJson + pageJson + pageCount;
                        xml = apiTools.GetJSON(url, webHeader);

                        if (pageLimit > 0 && pageCount > pageLimit)
                            break;

                        if (apiTools.IsXmlDead(xml)) {
                            deadPage = true;
                            break;
                        }

                        // Everything below here is basically a copy & paste from above.
                        doc.LoadXml(xml);
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

                        for (int i = 0; i < xmlID.Count; i++) {
                            if (xmlDeleted[i].InnerText == "true") {
                                continue;
                            }
                            string artists = string.Empty;
                            string rating = xmlRating[i].InnerText;
                            bool isGraylisted = false;
                            bool isBlacklisted = false;
                            bool alreadyExists = false;
                            List<string> foundTags = new List<string>();
                            string foundGraylistedTags = string.Empty;
                            string ReadTags = string.Empty;

                            if (imageLimit > 0 && imageLimit == totalCount) {
                                morePages = false;
                                break;
                            }

                            ReadTags += "          General: [";
                            for (int x = 0; x < xmlTagsGeneral[i].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsGeneral[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsGeneral[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n          Species: [";
                            for (int x = 0; x < xmlTagsSpecies[i].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsSpecies[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsSpecies[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n          Character: [";
                            for (int x = 0; x < xmlTagsCharacter[i].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsCharacter[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsCharacter[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n          Copyright: [";
                            for (int x = 0; x < xmlTagsCopyright[i].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsCopyright[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsCopyright[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n          Artist: [";
                            for (int x = 0; x < xmlTagsArtist[i].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsArtist[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsArtist[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n          Invalid: [";
                            for (int x = 0; x < xmlTagsInvalid[i].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsInvalid[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsInvalid[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n          Lore: [";
                            for (int x = 0; x < xmlTagsLore[i].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsLore[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsLore[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n          Meta: [";
                            for (int x = 0; x < xmlTagsMeta[i].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsMeta[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsMeta[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n          Locked tags: [";
                            for (int x = 0; x < xmlTagsLocked[i].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsLocked[i].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsLocked[i].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n";

                            if (rating == "e")
                                rating = "Explicit";
                            else if (rating == "q")
                                rating = "Questionable";
                            else if (rating == "s")
                                rating = "Safe";
                            else
                                rating = "Unknown";
                            
                            string imageDescription = " No description";
                            if (xmlDescription[0].InnerText != "") {
                                imageDescription = "\n                \"" + xmlDescription[i].InnerText + "\"";
                            }

                            for (int j = 0; j < xmlTagsArtist[i].ChildNodes.Count; j++)
                                artists += xmlTagsArtist[i].ChildNodes[j].InnerText + "\n               ";
                            artists = artists.TrimEnd(' ');
                            artists = artists.TrimEnd('\n');

                            string fileNameArtist = "(none)";
                            bool useHardcodedFilter = false;
                            if (string.IsNullOrEmpty(General.Default.undesiredTags))
                                useHardcodedFilter = true;

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

                            string fileName = fileNameSchema.Replace("%md5%", xmlMD5[i].InnerText)
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

                            for (int j = 0; j < foundTags.Count; j++) {
                                if (BlacklistedTags.Count > 0) {
                                    for (int k = 0; k < BlacklistedTags.Count; k++) {
                                        if (foundTags[j] == BlacklistedTags[k]) {
                                            isBlacklisted = true;
                                            continue;
                                        }
                                    }
                                }

                                if (isBlacklisted)
                                    break;

                                if (GraylistedTags.Count > 0) {
                                    for (int k = 0; k < GraylistedTags.Count; k++) {
                                        if (foundTags[j] == GraylistedTags[k]) {
                                            isGraylisted = true;
                                            foundGraylistedTags += GraylistedTags[k] + " ";
                                        }
                                    }
                                }
                            }

                            string outputDir = saveTo;
                            if (isBlacklisted) {
                                blacklistCount++;
                                totalCount++;
                            }
                            else if (isGraylisted) {
                                if (separateRatings) {
                                    outputDir += "\\" + rating.ToLower() + "\\blacklisted\\";
                                    if (separateNonImages) {
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
                                    alreadyExists = File.Exists(outputDir);
                                }
                                else {
                                    outputDir += "\\blacklisted\\";
                                    if (separateNonImages) {
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
                                    alreadyExists = File.Exists(outputDir);
                                }


                                switch (xmlRating[i].InnerText.ToLower()) {
                                    case "e":
                                        if (alreadyExists) {
                                            graylistExplicitExistCount++;
                                        }
                                        else {
                                            graylistExplicitCount++;
                                        }
                                        break;
                                    case "q":
                                        if (alreadyExists) {
                                            graylistQuestionableExistCount++;
                                        }
                                        else {
                                            graylistQuestionableCount++;
                                        }
                                        break;
                                    case "s":
                                        if (alreadyExists) {
                                            graylistSafeExistCount++;
                                        }
                                        else {
                                            graylistSafeCount++;
                                        }
                                        break;
                                    default:
                                        MessageBox.Show("An error occured when determining the rating. Open an issue.\n\nFound rating: " + xmlRating[i].InnerText);
                                        break;
                                }

                                if (alreadyExists) {
                                    graylistTotalExistCount++;
                                }
                                else {
                                    graylistTotalCount++;
                                }
                                totalCount++;
                            }
                            else {
                                if (separateRatings) {
                                    outputDir += "\\" + rating.ToLower() + "\\";
                                    if (separateNonImages) {
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
                                    alreadyExists = File.Exists(outputDir);
                                }
                                else {
                                    outputDir += "\\";
                                    if (separateNonImages) {
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
                                    alreadyExists = File.Exists(outputDir);
                                }

                                switch (xmlRating[i].InnerText.ToLower()) {
                                    case "e":
                                        if (alreadyExists) {
                                            cleanExplicitExistCount++;
                                        }
                                        else {
                                            cleanExplicitCount++;
                                        }
                                        break;
                                    case "q":
                                        if (alreadyExists) {
                                            cleanQuestionableExistCount++;
                                        }
                                        else {
                                            cleanQuestionableCount++;
                                        }
                                        break;
                                    case "s":
                                        if (alreadyExists) {
                                            cleanSafeExistCount++;
                                        }
                                        else {
                                            cleanSafeCount++;
                                        }
                                        break;
                                    default:
                                        MessageBox.Show("An error occured when determining the rating. Open an issue.\n\nFound rating: " + xmlRating[i].InnerText);
                                        break;
                                }

                                if (alreadyExists) {
                                    cleanTotalExistCount++;
                                }
                                else {
                                    cleanTotalCount++;
                                }
                                totalCount++;
                            }

                            if (isBlacklisted)
                                continue;

                            if (isGraylisted) {
                                if (saveBlacklistedFiles) {
                                    if (useMinimumScore && Int32.Parse(xmlScore[i].InnerText) < minimumScore)
                                        continue;

                                    if (!ratings.Any(xmlRating[i].InnerText.Contains))
                                        continue;
                                }
                                else {
                                    continue;
                                }
                            }
                            else { // Not blacklisted
                                if (useMinimumScore && Int32.Parse(xmlScore[i].InnerText) < minimumScore)
                                    continue;

                                if (!ratings.Any(xmlRating[i].InnerText.Contains))
                                    continue;
                            }

                            if (isGraylisted && saveBlacklistedFiles) {
                                if (separateRatings) {
                                    if (xmlRating[i].InnerText == "e") {
                                        //GraylistedExplicitURLs.Add(xmlURL[i].InnerText);
                                        GraylistedExplicitURLs.Add(fileUrl);
                                        GraylistedExplicitFileNames.Add(fileName);
                                        GraylistedExplicitFileExists.Add(alreadyExists);
                                    }
                                    else if (xmlRating[i].InnerText == "q") {
                                        //GraylistedQuestionableURLs.Add(xmlURL[i].InnerText);
                                        GraylistedQuestionableURLs.Add(fileUrl);
                                        GraylistedQuestionableFileNames.Add(fileName);
                                        GraylistedQuestionableFileExists.Add(alreadyExists);
                                    }
                                    else if (xmlRating[i].InnerText == "s") {
                                        //GraylistedSafeURLs.Add(xmlURL[i].InnerText);
                                        GraylistedSafeURLs.Add(fileUrl);
                                        GraylistedSafeFileNames.Add(fileName);
                                        GraylistedSafeFileExists.Add(alreadyExists);
                                    }
                                }
                                else {
                                    //GraylistedURLs.Add(xmlURL[i].InnerText);
                                    GraylistedURLs.Add(fileUrl);
                                    GraylistedFileNames.Add(fileName);
                                    GraylistedFileExists.Add(alreadyExists);
                                }

                                blacklistInfo += "POST " + xmlID[i].InnerText + ":\n" +
                                                 "    MD5: " + xmlMD5[i].InnerText + "\n" +
                                                 "    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n" +
                                                 "    ARTIST(S): " + artists + "\n" +
                                                 "    TAGS: " + ReadTags + //string.Concat(foundTags.ToArray()) + "\n" +
                                                 "    OFFENDING TAGS: " + foundGraylistedTags +
                                                 "    SCORE: Up " + xmlScoreUp[i].InnerText + ", Down " + xmlScoreDown[i].InnerText + ", Total " + xmlScore[i].InnerText + "\n" +
                                                 "    RATING: " + rating + "\n" +
                                                 "    DESCRIPITON:" + imageDescription +
                                                 "\n\n";
                            }
                            else {
                                if (separateRatings) {
                                    if (xmlRating[i].InnerText == "e") {
                                        //ExplicitURLs.Add(xmlURL[i].InnerText);
                                        ExplicitURLs.Add(fileUrl);
                                        ExplicitFileNames.Add(fileName);
                                        ExplicitFileExists.Add(alreadyExists);
                                    }
                                    else if (xmlRating[i].InnerText == "q") {
                                        //QuestionableURLs.Add(xmlURL[i].InnerText);
                                        QuestionableURLs.Add(fileUrl);
                                        QuestionableFileNames.Add(fileName);
                                        QuestionableFileExists.Add(alreadyExists);
                                    }
                                    else if (xmlRating[i].InnerText == "s") {
                                        //SafeURLs.Add(xmlURL[i].InnerText);
                                        SafeURLs.Add(fileUrl);
                                        SafeFileNames.Add(fileName);
                                        SafeFileExists.Add(alreadyExists);
                                    }
                                }
                                else {
                                    //URLs.Add(xmlURL[i].InnerText);
                                    URLs.Add(fileUrl);
                                    FileNames.Add(fileName);
                                    FileExists.Add(alreadyExists);
                                }

                                tagInfo += "POST " + xmlID[i].InnerText + ":\n" +
                                           "    MD5: " + xmlMD5[i].InnerText + "\n" +
                                           "    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n" +
                                           "    ARTIST(S): " + artists + "\n" +
                                           "    TAGS:\n" + ReadTags + //string.Concat(foundTags.ToArray()) + "\n" +
                                           "    SCORE: Up " + xmlScoreUp[i].InnerText + ", Down " + xmlScoreDown[i].InnerText + ", Total " + xmlScore[i].InnerText + "\n" +
                                           "    RATING: " + rating + "\n" +
                                           "    DESCRIPITON:" + imageDescription +
                                           "\n\n";
                            }
                        }

                        this.BeginInvoke(new MethodInvoker(() => {
                            string labelBuffer = "";
                            if (separateRatings) {
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

                                labelBuffer = string.Format("Files: {0} ( {1} E | {2} Q | {3} S )\n" +
                                                            "Blacklisted: {4} ( {5} E | {6} Q | {7} S )\n" +
                                                            "Zero Tolerance: {8}\n" +
                                                            "Total: {9}\n\n" +
                                                            "Files that exist: {10} ( {11} E | {12} Q | {13} S )\n" +
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

                                labelBuffer = string.Format("Files: {0}\n" +
                                                            "Blacklisted: {1}\n" +
                                                            "Zero Tolerance: {2}\n" +
                                                            "Total: {3}\n\n" +
                                                            "Files that exist: {4}\n" +
                                                            "Blacklisted that exist: {5}", counts);
                            }
                            lbBlacklist.Text = labelBuffer;
                        }));

                        pageCount++;
                    }
                }
            #endregion

            #region output logic
            // Create output folders
                if (separateRatings) {
                    if (ExplicitURLs.Count > 0) {
                        if (!Directory.Exists(saveTo + "\\explicit"))
                            Directory.CreateDirectory(saveTo + "\\explicit");
                    }
                    if (QuestionableURLs.Count > 0) {
                        if (!Directory.Exists(saveTo + "\\questionable"))
                            Directory.CreateDirectory(saveTo + "\\questionable");
                    }
                    if (SafeURLs.Count > 0) {
                        if (!Directory.Exists(saveTo + "\\safe"))
                            Directory.CreateDirectory(saveTo + "\\safe");
                    }

                    if (saveBlacklistedFiles) {
                        if (GraylistedExplicitURLs.Count > 0) {
                            if (!Directory.Exists(saveTo + "\\explicit\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\explicit\\blacklisted");
                        }
                        if (GraylistedQuestionableURLs.Count > 0) {
                            if (!Directory.Exists(saveTo + "\\questionable\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\questionable\\blacklisted");
                        }
                        if (GraylistedSafeURLs.Count > 0) {
                            if (!Directory.Exists(saveTo + "\\safe\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\safe\\blacklisted");
                        }
                    }
                }
                else {
                    if (!Directory.Exists(saveTo))
                        Directory.CreateDirectory(saveTo);

                    if (saveBlacklistedFiles && GraylistedURLs.Count > 0) {
                        if (!Directory.Exists(saveTo + "\\blacklisted"))
                            Directory.CreateDirectory(saveTo + "\\blacklisted");
                    }
                }
            #endregion

            #region pre-download logic
            // Update totals
                if (separateRatings) {
                    cleanTotalCount = cleanExplicitCount + cleanQuestionableCount + cleanSafeCount;
                    graylistTotalCount = graylistExplicitCount + graylistQuestionableCount + graylistSafeCount;
                }

            // Save the info files from the buffer
                if (saveInfo) {
                    tagInfo.TrimEnd('\n');
                    File.WriteAllText(saveTo + "\\tags.nfo", tagInfo, Encoding.UTF8);

                    if (saveBlacklistedFiles && graylistTotalCount > 0) {
                        blacklistInfo.TrimEnd('\n');
                        if (separateRatings)
                            File.WriteAllText(saveTo + "\\tags.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                        else
                            File.WriteAllText(saveTo + "\\blacklisted\\tags.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                    }
                }

            // Set the progressbar style.
                this.BeginInvoke(new MethodInvoker(() => {
                    string labelBuffer = "";
                    if (separateRatings) {
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

                        labelBuffer = string.Format("Files: {0} ( {1} E | {2} Q | {3} S )\n" +
                                                    "Blacklisted: {4} ( {5} E | {6} Q | {7} S )\n" +
                                                    "Zero Tolerance: {8}\n" +
                                                    "Total: {9}\n\n" +
                                                    "Files that exist: {10} ( {11} E | {12} Q | {13} S )\n" +
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

                        labelBuffer = string.Format("Files: {0}\n" +
                                                    "Blacklisted: {1}\n" +
                                                    "Zero Tolerance: {2}\n" +
                                                    "Total: {3}\n\n" +
                                                    "Files that exist: {4}\n" +
                                                    "Blacklisted that exist: {5}", counts);
                    }
                    lbBlacklist.Text = labelBuffer;

                    if (saveBlacklistedFiles)
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
                    wc.Headers.Add(webHeader);
                    wc.Method = "GET";

                    if (separateRatings) {
                        if (ExplicitURLs.Count > 0) {
                            for (int y = 0; y < ExplicitURLs.Count; y++) {
                                if (string.IsNullOrEmpty(ExplicitURLs[y])) { continue; }
                                url = ExplicitURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading explicit file " + (y + 1) + " of " + (cleanExplicitCount)));

                                string fileName = ExplicitFileNames[y];
                                string outputDir = saveTo + "\\explicit\\";
                                switch (separateNonImages) {
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
                                        wc.DownloadFileAsync(new Uri(url), outputDir, sync);
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
                                url = QuestionableURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading questionable file " + (y + 1) + " of " + (cleanQuestionableCount)));

                                string fileName = QuestionableFileNames[y];
                                string outputDir = saveTo + "\\questionable\\";
                                switch (separateNonImages) {
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
                                        wc.DownloadFileAsync(new Uri(url), outputDir, sync);
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
                                url = SafeURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading safe file " + (y + 1) + " of " + (cleanSafeCount)));

                                string fileName = SafeFileNames[y];
                                string outputDir = saveTo + "\\safe\\";
                                switch (separateNonImages) {
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
                                        wc.DownloadFileAsync(new Uri(url), outputDir, sync);
                                        Monitor.Wait(sync);
                                    }
                                }
                                else {
                                    this.Invoke((MethodInvoker)(() => status.Text = fileName + " already exists"));
                                }
                            }
                        }

                        if (saveBlacklistedFiles) {
                            if (GraylistedExplicitURLs.Count > 0) {
                                for (int y = 0; y < GraylistedExplicitURLs.Count; y++) {
                                    if (string.IsNullOrEmpty(GraylistedExplicitURLs[y])) { continue; }
                                    url = GraylistedExplicitURLs[y];
                                    this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (e) file " + (y + 1) + " of " + (graylistExplicitCount)));

                                    string fileName = GraylistedExplicitFileNames[y];
                                    string outputDir = saveTo + "\\explicit\\blacklisted\\";
                                    switch (separateNonImages) {
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
                                            wc.DownloadFileAsync(new Uri(url), outputDir, sync);
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
                                url = GraylistedQuestionableURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (q) file " + (y + 1) + " of " + (graylistQuestionableCount)));

                                string fileName = GraylistedQuestionableFileNames[y];
                                string outputDir = saveTo + "\\questionable\\blacklisted\\";
                                switch (separateNonImages) {
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
                                        wc.DownloadFileAsync(new Uri(url), outputDir, sync);
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
                                url = GraylistedSafeURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (s) file " + (y + 1) + " of " + (graylistSafeCount)));

                                string fileName = GraylistedSafeFileNames[y];
                                string outputDir = saveTo + "\\safe\\blacklisted\\";
                                switch (separateNonImages) {
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
                                        wc.DownloadFileAsync(new Uri(url), outputDir, sync);
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
                                url = URLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading file " + (y + 1) + " of " + (cleanTotalCount)));

                                string fileName = FileNames[y];
                                string outputDir = saveTo;
                                switch (separateNonImages) {
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
                                        wc.DownloadFileAsync(new Uri(url), outputDir, sync);
                                        Monitor.Wait(sync);
                                    }
                                }
                                else {
                                    this.Invoke((MethodInvoker)(() => status.Text = fileName + " already exists"));
                                }
                            }
                        }

                        if (saveBlacklistedFiles && GraylistedURLs.Count > 0) {
                            for (int y = 0; y < GraylistedURLs.Count; y++) {
                                if (string.IsNullOrEmpty(GraylistedURLs[y])) { continue; }
                                url = GraylistedURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted file " + (y) + " of " + (graylistTotalCount)));

                                string fileName = GraylistedFileNames[y];
                                string outputDir = saveTo + "\\blacklisted\\";
                                switch (separateNonImages) {
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
                                        wc.DownloadFileAsync(new Uri(url), outputDir, sync);
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
                if (separateRatings) {
                    TotalFiles += (ExplicitURLs.Count + QuestionableURLs.Count + SafeURLs.Count);
                    PresentFiles += cleanTotalCount;
                    if (saveBlacklistedFiles) {
                        TotalFiles += (GraylistedExplicitURLs.Count + GraylistedQuestionableURLs.Count + GraylistedSafeURLs.Count);
                        PresentFiles += graylistTotalCount;
                    }
                }
                else {
                    TotalFiles += URLs.Count;
                    PresentFiles += cleanTotalCount;
                    if (saveBlacklistedFiles) {
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
                ErrorLog.ReportWebException(WebE,url, "frmTagDownloader.cs");
            }
            catch (Exception ex) {
                apiTools.SendDebugMessage("A gneral exception has occured. (frmTagDownloader.cs)");
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
        private bool downloadPage(string pageURL) {
            string pageNumber = pageURL.Split('/')[5];
            tags = pageURL.Split('/')[6].Split('#')[0].Replace("%20", " ");

            changeTask("Awaiting API call");
            string url = string.Empty;                                      // The URL being accessed, changes per API call/File download.

            string tagInfo = string.Empty;                                  // The buffer for the 'tag.nfo' file that will be created.
            string blacklistInfo = string.Empty;                            // The buffer for the 'tag.blacklisted.nfo' file that will be created.

            string xml = string.Empty;                                      // The XML string.

            List<string> GraylistedTags = new List<string>();               // The list of files that will be downloaded into a separate folder (if saveBlacklisted = true).
            List<string> BlacklistedTags = new List<string>();              // The list of files that will be skipped entirely.

            List<string> URLs = new List<string>();                         // The URLs that will be downloaded (if separateRatings = false).
            List<string> GraylistedURLs = new List<string>();               // The Blacklisted URLs that will be downloaded (if separateRatings = false).

            List<string> ExplicitURLs = new List<string>();                 // The list of Explicit files.
            List<string> QuestionableURLs = new List<string>();             // The list of Questionable files.
            List<string> SafeURLs = new List<string>();                     // The list of Safe files.
            List<string> GraylistedExplicitURLs = new List<string>();       // The list of Graylisted Explicit files.
            List<string> GraylistedQuestionableURLs = new List<string>();   // The list of Graylisted Questionable files.
            List<string> GraylistedSafeURLs = new List<string>();           // The list of Graylisted Safe files.

            int tagLength = 0;                                              // Will be the count of tags being downloaded (1-6).

            try {
            // Set the saveTo.
                string newTagName = apiTools.ReplaceIllegalCharacters(tags);
                if (useMinimumScore)                                                                        // Add minimum score to folder name.
                    newTagName += " (scores " + (minimumScore) + "+)";
                if (!this.saveTo.EndsWith("\\Pages\\" + newTagName + " (page " + pageNumber + ")"))   // Set the output folder.
                    this.saveTo += "\\Pages\\" + newTagName + " (page " + pageNumber + ")";

            // Start the buffer for the .nfo files.
                if (useMinimumScore) {
                    tagInfo = "TAGS: " + tags + "\n" +
                              "MINIMUM SCORE: " + minimumScore + "\n" +
                              "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") +
                              "\n\n";

                    blacklistInfo = "TAGS: " + tags + "\n" +
                                    "MINIMUM SCORE: " + minimumScore + "\n" +
                                    "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\n" +
                                    "BLACKLISTED TAGS: " + graylist +
                                    "\n\n";
                }
                else {
                    tagInfo = "TAGS: " + tags + "\n" +
                              "MINIMUM SCORE: n/a\n" +
                              "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") +
                              "\n\n";

                    blacklistInfo = "TAGS: " + tags + "\n" +
                                    "MINIMUM SCORE: n/a\n" +
                                    "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\n" +
                                    "BLACKLISTED TAGS: " + graylist +
                                    "\n\n";
                }

            // Get the tag length.
                tagLength = tags.Split(' ').Length;

            // Add the minimum score to the search tags (if applicable).
                if (tagLength < 6 && scoreAsTag) {
                    tags += " score:>" + (minimumScore - 1);
                }

            // Set the blacklist.. lists...
                if (!string.IsNullOrWhiteSpace(graylist))
                    GraylistedTags = graylist.Split(' ').ToList();
                if (!string.IsNullOrWhiteSpace(blacklist))
                    BlacklistedTags = blacklist.Split(' ').ToList();

            // Get XML of page.
                changeTask("Downloading page information...");
                url = tagJson + tags + pageJson + pageNumber;
                xml = apiTools.GetJSON(url, webHeader);
                if (xml == null)
                    return false;

            // Parse the XML file.
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNodeList xmlID = doc.DocumentElement.SelectNodes("/root/item/id");
                XmlNodeList xmlMD5 = doc.DocumentElement.SelectNodes("/root/item/md5");
                XmlNodeList xmlURL = doc.DocumentElement.SelectNodes("/root/item/file_url");
                XmlNodeList xmlArtist = doc.DocumentElement.SelectNodes("/root/item/artist");
                XmlNodeList xmlTags = doc.DocumentElement.SelectNodes("/root/item/tags");
                XmlNodeList xmlScore = doc.DocumentElement.SelectNodes("/root/item/score");
                XmlNodeList xmlRating = doc.DocumentElement.SelectNodes("/root/item/rating");
                XmlNodeList xmlDescription = doc.DocumentElement.SelectNodes("/root/item/description");

           // Begin parsing the XML for tag information per item.
                for (int i = 0; i < xmlID.Count; i++) {
                    totalCount++;   // Add to the total count.

                    string artists = string.Empty;                                          // The artists that worked on the file.
                    string rating = xmlRating[i].InnerText;                                 // Get the rating of the current file.
                    bool isGraylisted = false;                                              // Will determine if the file is graylisted.
                    bool isBlacklisted = false;                                             // Will determine if the file is blacklisted.
                    List<string> foundTags = xmlTags[i].InnerText.Split(' ').ToList();      // Get the entire tag list of the file.
                    string foundGraylistedTags = string.Empty;                              // The buffer for the tags that are graylisted.

                // Check the image limit & break if reached.
                    if (imageLimit > 0 && imageLimit == totalCount)
                        break;

                // Set the rating for the .nfo file.
                    if (rating == "e")
                        rating = "Explicit";
                    else if (rating == "q")
                        rating = "Questionable";
                    else if (rating == "s")
                        rating = "Safe";
                    else
                        rating = "Unknown";

                // Gets the artists of the file and then trims the end of garbage.
                    for (int j = 0; j < xmlArtist[i].ChildNodes.Count; j++)
                        artists += xmlArtist[i].ChildNodes[j].InnerText + "\n               ";
                    artists = artists.TrimEnd(' ');
                    artists = artists.TrimEnd('\n');

                // Check for blacklisted and graylisted tags.
                    for (int j = 0; j < foundTags.Count; j++) {
                        if (BlacklistedTags.Count > 0) {
                            for (int k = 0; k < BlacklistedTags.Count; k++) {
                                if (foundTags[j] == BlacklistedTags[k]) {
                                    isBlacklisted = true;
                                    break;
                                }
                            }
                        }

                        if (isBlacklisted)
                            break;

                        if (GraylistedTags.Count > 0) {
                            for (int k = 0; k < GraylistedTags.Count; k++) {
                                if (foundTags[j] == GraylistedTags[k]) {
                                    isGraylisted = true;
                                    foundGraylistedTags += GraylistedTags[k] + " ";
                                }
                            }
                        }
                    }

                // Add to the counts (and break for blacklisted)
                    if (isBlacklisted) {
                                blacklistCount++;
                            }
                            else if (isGraylisted) {
                                graylistTotalCount++;
                                switch (xmlRating[i].InnerText.ToLower()) {
                                    case "e":
                                        graylistExplicitCount++;
                                        break;
                                    case "q":
                                        graylistQuestionableCount++;
                                        break;
                                    case "s":
                                        graylistSafeCount++;
                                        break;
                                }
                            }
                            else {
                                cleanTotalCount++;
                                switch (xmlRating[i].InnerText.ToLower()) {
                                    case "e":
                                        cleanExplicitCount++;
                                        break;
                                    case "q":
                                        cleanQuestionableCount++;
                                        break;
                                    case "s":
                                        cleanSafeCount++;
                                        break;
                                }
                            }
                            totalCount++;

                            this.BeginInvoke(new MethodInvoker(() => {
                                string labelBuffer = "";
                                if (separateRatings) {
                                    object[] counts = new object[] { cleanTotalCount.ToString(), cleanExplicitCount.ToString(), cleanQuestionableCount.ToString(), cleanSafeCount.ToString(), graylistTotalCount.ToString(), graylistExplicitCount.ToString(), graylistQuestionableCount.ToString(), graylistSafeCount.ToString(), blacklistCount.ToString(), totalCount.ToString() };

                                    labelBuffer = string.Format("{0} posts ({1} e, {2} q, {3} s)\n" +
                                                                "{4} blacklisted posts ({5} e, {6} q, {7} s)\n" +
                                                                "{8} skipped (zero-tolerance)\n" +
                                                                "{9} total files", counts);
                                }
                                else {
                                    object[] counts = new object[] { cleanTotalCount.ToString(), graylistTotalCount.ToString(), blacklistCount.ToString(), totalCount.ToString() };

                                    labelBuffer = string.Format("{0} posts\n" +
                                                                "{1} blacklisted\n" +
                                                                "{2} skipped (zero-tolerance)\n" +
                                                                "{3} total files", counts);
                                }
                                lbBlacklist.Text = labelBuffer;
                            }));

                            if (isBlacklisted)
                                continue;

                // Graylist check & options check
                    if (isGraylisted) {
                        if (saveBlacklistedFiles) {
                            if (useMinimumScore && Int32.Parse(xmlScore[i].InnerText) < minimumScore)
                                continue;

                            if (!ratings.Any(xmlRating[i].InnerText.Contains))
                                continue;
                        }
                        else {
                            continue;
                        }
                    }
                    else { // Not blacklisted
                        if (useMinimumScore && Int32.Parse(xmlScore[i].InnerText) < minimumScore)
                            continue;

                        if (!ratings.Any(xmlRating[i].InnerText.Contains))
                            continue;
                    }

                // Start adding to the nfo buffer and URL lists
                    if (isGraylisted && saveBlacklistedFiles) {
                        if (separateRatings) {
                            if (xmlRating[i].InnerText == "e")
                                GraylistedExplicitURLs.Add(xmlURL[i].InnerText);
                            else if (xmlRating[i].InnerText == "q")
                                GraylistedQuestionableURLs.Add(xmlURL[i].InnerText);
                            else if (xmlRating[i].InnerText == "s")
                                GraylistedSafeURLs.Add(xmlURL[i].InnerText);
                        }
                        else {
                            GraylistedURLs.Add(xmlURL[i].InnerText);
                        }

                        blacklistInfo += "POST " + xmlID[i].InnerText + ":\n" +
                                         "    MD5: " + xmlMD5[i].InnerText + "\n" +
                                         "    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n" +
                                         "    ARTIST(S): " + artists + "\n" +
                                         "    TAGS: " + xmlTags[i].InnerText + "\n" +
                                         "    SCORE: " + xmlScore[i].InnerText + "\n" +
                                         "    RATING: " + rating + "\n" +
                                         "    DESCRIPITON:\n\"" + xmlDescription[i].InnerText + "\"\n" +
                                         "    OFFENDING TAGS: " + foundGraylistedTags +
                                         "\n\n";
                    }
                    else {
                        if (separateRatings) {
                            if (xmlRating[i].InnerText == "e")
                                ExplicitURLs.Add(xmlURL[i].InnerText);
                            else if (xmlRating[i].InnerText == "q")
                                QuestionableURLs.Add(xmlURL[i].InnerText);
                            else if (xmlRating[i].InnerText == "s")
                                SafeURLs.Add(xmlURL[i].InnerText);
                        }
                        else {
                            URLs.Add(xmlURL[i].InnerText);
                        }

                        tagInfo += "POST " + xmlID[i].InnerText + ":\n" +
                                   "    MD5: " + xmlMD5[i].InnerText + "\n" +
                                   "    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n" +
                                   "    ARTIST(S): " + artists + "\n" +
                                   "    TAGS: " + xmlTags[i].InnerText + "\n" +
                                   "    SCORE: " + xmlScore[i].InnerText + "\n" +
                                   "    RATING: " + rating + "\n" +
                                   "    DESCRIPITON:\n\"" + xmlDescription[i].InnerText + "\"" +
                                   "\n\n";
                    }
                }

            // Create output folders
                if (separateRatings) {
                    if (ExplicitURLs.Count > 0) {
                        if (!Directory.Exists(saveTo + "\\explicit"))
                            Directory.CreateDirectory(saveTo + "\\explicit");
                        else
                            cleanExplicitCount -= apiTools.CountFiles(saveTo + "\\explicit");
                    }
                    if (QuestionableURLs.Count > 0) {
                        if (!Directory.Exists(saveTo + "\\questionable"))
                            Directory.CreateDirectory(saveTo + "\\questionable");
                        else
                            cleanQuestionableCount -= apiTools.CountFiles(saveTo + "\\questionable");
                    }
                    if (SafeURLs.Count > 0) {
                        if (!Directory.Exists(saveTo + "\\safe"))
                            Directory.CreateDirectory(saveTo + "\\safe");
                        else
                            cleanSafeCount -= apiTools.CountFiles(saveTo + "\\safe");
                    }

                    if (saveBlacklistedFiles) {
                        if (GraylistedExplicitURLs.Count > 0) {
                            if (!Directory.Exists(saveTo + "\\explicit\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\explicit\\blacklisted");
                            else
                                graylistExplicitCount -= apiTools.CountFiles(saveTo + "\\explicit\\blacklisted");
                        }
                        if (GraylistedQuestionableURLs.Count > 0) {
                            if (!Directory.Exists(saveTo + "\\questionable\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\questionable\\blacklisted");
                            else
                                graylistQuestionableCount -= apiTools.CountFiles(saveTo + "\\questionable\\blacklisted");
                        }
                        if (GraylistedSafeURLs.Count > 0) {
                            if (!Directory.Exists(saveTo + "\\safe\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\safe\\blacklisted");
                            else
                                graylistSafeCount -= apiTools.CountFiles(saveTo + "\\safe\\blacklisted");
                        }
                    }
                }
                else {
                    if (!Directory.Exists(saveTo))
                        Directory.CreateDirectory(saveTo);
                    else
                        cleanTotalCount -= apiTools.CountFiles(saveTo);

                    if (saveBlacklistedFiles && GraylistedURLs.Count > 0) {
                        if (!Directory.Exists(saveTo + "\\blacklisted"))
                            Directory.CreateDirectory(saveTo + "\\blacklisted");
                        else
                            graylistTotalCount -= apiTools.CountFiles(saveTo + "\\blacklisted");
                    }
                }

            // Update totals
                if (separateRatings) {
                    cleanTotalCount = cleanExplicitCount + cleanQuestionableCount + cleanSafeCount;
                    graylistTotalCount = graylistExplicitCount + graylistQuestionableCount + graylistSafeCount;
                }

            // Save the info files from the buffer
                if (saveInfo) {
                    tagInfo.TrimEnd('\n');
                    File.WriteAllText(saveTo + "\\tags.nfo", tagInfo, Encoding.UTF8);

                    if (saveBlacklistedFiles && graylistTotalCount > 0) {
                        blacklistInfo.TrimEnd('\n');
                        if (separateRatings)
                            File.WriteAllText(saveTo + "\\tags.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                        else
                            File.WriteAllText(saveTo + "\\blacklisted\\tags.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                    }
                }

            // Set the progressbar style.
                this.Invoke((MethodInvoker)(() => pbDownloadStatus.Style = ProgressBarStyle.Continuous));

                this.BeginInvoke(new MethodInvoker(() => {
                    string labelBuffer = "";
                    if (separateRatings) {
                        object[] counts = new object[] { cleanTotalCount.ToString(), cleanExplicitCount.ToString(), cleanQuestionableCount.ToString(), cleanSafeCount.ToString(), graylistTotalCount.ToString(), graylistExplicitCount.ToString(), graylistQuestionableCount.ToString(), graylistSafeCount.ToString(), blacklistCount.ToString(), totalCount.ToString() };

                        labelBuffer = string.Format("{0} posts ({1} e, {2} q, {3} s)\n" +
                                                    "{4} blacklisted posts ({5} e, {6} q, {7} s)\n" +
                                                    "{8} skipped (zero-tolerance)\n" +
                                                    "{9} total files", counts);
                    }
                    else {
                        object[] counts = new object[] { cleanTotalCount.ToString(), graylistTotalCount.ToString(), blacklistCount.ToString(), totalCount.ToString() };

                        labelBuffer = string.Format("{0} posts\n" +
                                                    "{1} blacklisted\n" +
                                                    "{2} skipped (zero-tolerance)\n" +
                                                    "{3} total files", counts);
                    }
                    lbBlacklist.Text = labelBuffer;
                }));

            // Start the download
                using (ExWebClient wc = new ExWebClient()) {
                    wc.DownloadProgressChanged += (s, e) => {
                        if (!this.IsDisposed) {
                            this.BeginInvoke(new MethodInvoker(() => {
                                pbDownloadStatus.Value = e.ProgressPercentage;
                                pbDownloadStatus.Value++;
                                pbDownloadStatus.Value--;
                                lbPercentage.Text = e.ProgressPercentage.ToString() + "%";
                            }));
                        }
                    };
                    wc.DownloadFileCompleted += (s, e) => {
                        if (!pbDownloadStatus.IsDisposed && !lbPercentage.IsDisposed) {
                            lock (e.UserState) {
                                this.BeginInvoke(new MethodInvoker(() => {
                                    pbDownloadStatus.Value = 0;
                                    lbPercentage.Text = "0%";
                                }));
                                Monitor.Pulse(e.UserState);
                            }
                        }
                    };

                    wc.Proxy = WebRequest.GetSystemWebProxy();
                    wc.Headers.Add("user-agent", webHeader);
                    wc.Method = "GET";

                    if (separateRatings) {
                        if (ExplicitURLs.Count > 0) {
                            for (int y = 0; y < ExplicitURLs.Count; y++) {
                                url = ExplicitURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading explicit file " + (y + 1) + " of " + (ExplicitURLs.Count)));

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\explicit\\" + filename)) {
                                    this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + filename));
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\explicit\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }
                                else {
                                    this.Invoke((MethodInvoker)(() => status.Text = filename + " already exists"));
                                }
                            }
                        }

                        if (QuestionableURLs.Count > 0) {
                            for (int y = 0; y < QuestionableURLs.Count; y++) {
                                url = QuestionableURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading questionable file " + (y + 1) + " of " + (QuestionableURLs.Count)));

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\questionable\\" + filename)) {
                                    this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + filename));
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\questionable\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }
                                else {
                                    this.Invoke((MethodInvoker)(() => status.Text = filename + " already exists"));
                                }
                            }
                        }

                        if (SafeURLs.Count > 0) {
                            for (int y = 0; y < SafeURLs.Count; y++) {
                                url = SafeURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading safe file " + (y + 1) + " of " + (SafeURLs.Count)));

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\safe\\" + filename)) {
                                    this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + filename));
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\safe\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }
                                else {
                                    this.Invoke((MethodInvoker)(() => status.Text = filename + " already exists"));
                                }
                            }
                        }

                        if (saveBlacklistedFiles) {
                            if (GraylistedExplicitURLs.Count > 0) {
                                for (int y = 0; y < GraylistedExplicitURLs.Count; y++) {
                                    url = GraylistedExplicitURLs[y];
                                    this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (e) file " + (y + 1) + " of " + (GraylistedExplicitURLs.Count)));

                                    string filename = url.Split('/')[6];
                                    if (!File.Exists(saveTo + "\\explicit\\blacklisted\\" + filename)) {
                                        this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + filename));
                                        var sync = new Object();
                                        lock (sync) {
                                            wc.DownloadFileAsync(new Uri(url), saveTo + "\\explicit\\blacklisted\\" + filename, sync);
                                            Monitor.Wait(sync);
                                        }
                                    }
                                    else {
                                        this.Invoke((MethodInvoker)(() => status.Text = filename + " already exists"));
                                    }
                                }
                            }
                        }

                        if (GraylistedQuestionableURLs.Count > 0) {
                            for (int y = 0; y < GraylistedQuestionableURLs.Count; y++) {
                                url = GraylistedQuestionableURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (q) file " + (y + 1) + " of " + (GraylistedQuestionableURLs.Count)));

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\questionable\\blacklisted\\" + filename)) {
                                    this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + filename));
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\questionable\\blacklisted\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }
                                else {
                                    this.Invoke((MethodInvoker)(() => status.Text = filename + " already exists"));
                                }
                            }
                        }

                        if (GraylistedSafeURLs.Count > 0) {
                            for (int y = 0; y < GraylistedSafeURLs.Count; y++) {
                                url = GraylistedSafeURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (s) file " + (y + 1) + " of " + (GraylistedSafeURLs.Count)));

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\safe\\blacklisted\\" + filename)) {
                                    this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + filename));
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\safe\\blacklisted\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }
                                else {
                                    this.Invoke((MethodInvoker)(() => status.Text = filename + " already exists"));
                                }
                            }
                        }
                    }
                    else {
                        if (URLs.Count > 0) {
                            for (int y = 0; y < URLs.Count; y++) {
                                url = URLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading file " + (y + 1) + " of " + (URLs.Count)));

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\" + filename)) {
                                    this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + filename));
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }
                                else {
                                    this.Invoke((MethodInvoker)(() => status.Text = filename + " already exists"));
                                }
                            }
                        }

                        if (saveBlacklistedFiles && GraylistedURLs.Count > 0) {
                            for (int y = 0; y < GraylistedURLs.Count; y++) {
                                url = GraylistedURLs[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted file " + (y) + " of " + (GraylistedURLs.Count)));

                                string filename = url.Split('/')[6];
                                if (!File.Exists(saveTo + "\\blacklisted\\" + filename)) {
                                    this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + filename));
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(url), saveTo + "\\blacklisted\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                }
                                else {
                                    this.Invoke((MethodInvoker)(() => status.Text = filename + " already exists"));
                                }
                            }
                        }
                    }
                }

                int count = 0;
                if (separateRatings) {
                    count += (ExplicitURLs.Count + QuestionableURLs.Count + SafeURLs.Count);
                    if (saveBlacklistedFiles)
                        count += (GraylistedExplicitURLs.Count + GraylistedQuestionableURLs.Count + GraylistedSafeURLs.Count);
                }
                else {
                    count += URLs.Count;
                    if (saveBlacklistedFiles)
                        count += GraylistedURLs.Count;
                }

                this.BeginInvoke(new MethodInvoker(() => {
                    lbFile.Text = "All " + (count) + " files downloaded.";
                    pbDownloadStatus.Value = pbDownloadStatus.Maximum;
                    lbPercentage.Text = "Done";
                    tmrTitle.Stop();
                    status.Text = "Finished downloading page";
                    this.Text = "Page download completed";
                }));
                return true;
            }
            catch (ThreadAbortException) {
                apiTools.SendDebugMessage("Thread was requested to be, and has been, aborted. (frmTagDownloader.cs)");
                return false;
            }
            catch (ObjectDisposedException) {
                apiTools.SendDebugMessage("An ObjectDisposedException occured. (frmTagDownloader.cs)");
                return false;
            }
            catch (WebException WebE) {
                apiTools.SendDebugMessage("A WebException has occured. (frmTagDownloader.cs)");
                ErrorLog.ReportWebException(WebE, url, "frmTagDownloader.cs");
                return false;
            }
            catch (Exception ex) {
                apiTools.SendDebugMessage("A gneral exception has occured. (frmTagDowloader.cs)");
                ErrorLog.ReportException(ex, "frmTagDownloader.cs");
                return false;
            }
        }

        private void changeTask(string curStatus) {
            this.BeginInvoke(new MethodInvoker(() => {
                status.Text = curStatus;
            }));
        }
    #endregion

    }
}