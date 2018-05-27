using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace aphrodite {
    public partial class frmTagDownloader : Form {

    #region Variables
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
        public bool usePageLimit = false;           // Setting for using page limit.
        public int pageLimit = 0;                   // Setting for page limit.
        public bool saveInfo = false;               // Setting for saving the info.
        public string[] ratings = null;             // Setting for the ratings.
        public bool separateRatings = true;         // Setting for separating the ratings.

        public static readonly string tagJson = "https://e621.net/post/index.json?tags=";   // API url.
        public static readonly string limitJson = "&limit=";                             // Maximum limit suffix
        public static readonly string pageJson = "&page=";                                  // Page suffix.

        string[] badFolderChars = new string[] { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };                           // The characters forbidden in folder names.
        string[] replacementFolderChars = new string[] { "%5C", "%2F", "%3A", "%2A", "%3F", "%22", "%3C", "%3E", "%7C" };   // Replacement for the forbidden names.

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
        
        Thread tagDownload;                 // New thread for the downloader.

        int cleanTotalCount = 0;            // Will be the count of how many files that are set for download.
        int cleanExplicitCount = 0;         // Will be the count of how many explicit files that are set for download.
        int cleanQuestionableCount = 0;     // Will be the count of how many questionable files that are set for download.
        int cleanSafeCount = 0;             // Will be the count of how many safe files that are set for download.

        int graylistTotalCount = 0;         // Will be the count of how many graylisted files that are set for download.
        int graylistExplicitCount = 0;      // Will be the count of how many explicit graylisted files that are set for download.
        int graylistQuestionableCount = 0;  // Will be the count of how many questionable graylisted files that are set for download.
        int graylistSafeCount = 0;          // Will be the count of how many safe graylisted files that are set for download.

        int blacklistCount = 0;             // Will be the count of how many blacklisted files that will be skipped.
        int totalCount = 0;                 // Will be the count of how many files that were parsed.
    #endregion

    #region Form
        public frmTagDownloader() {
            InitializeComponent();
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

            if (usePageLimit)
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
        }
        private void frmDownload_FormClosing(object sender, FormClosingEventArgs e) {
            tagDownload.Abort();
            this.Dispose();
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
                        if (downloadTags()) {
                            if (ignoreFinish)
                                this.DialogResult = DialogResult.OK;
                        }
                        else
                            this.DialogResult = DialogResult.Abort;
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

        private bool downloadTags() {
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
            int pageCount = 1;                                              // Will be the count of the pages parsed.

            bool morePages = false;                                         // Will determine if there are more than 1 page.

            try {
                // Set the saveTo.
                string newTagName = tags;
                for (int i = 0; i < badFolderChars.Length; i++)                                        // Replace bad characters (if present).
                    newTagName = newTagName.Replace(badFolderChars[i], replacementFolderChars[i]);
                if (useMinimumScore)                                                                    // Add minimum score to folder name.
                    newTagName += " (scores " + (minimumScore) + "+)";
                if (!this.saveTo.EndsWith("\\Tags\\" + newTagName))                                     // Set the output folder.
                    this.saveTo += "\\Tags\\" + newTagName;

                // Start the buffer for the .nfo files.
                if (useMinimumScore) {
                    tagInfo = "TAGS: " + tags + "\nMINIMUM SCORE: " + minimumScore + "\n\n";
                    blacklistInfo = "TAGS: " + tags + "\nBLACKLISTED TAGS: " + graylist + "\nMINIMUM SCORE: " + minimumScore + "\n\n";
                }
                else {
                    tagInfo = "TAGS: " + tags + "\nMINIMUM SCORE: n/a\n\n";
                    blacklistInfo = "TAGS: " + tags + "\nBLACKLISTED TAGS: " + graylist + "\nMINIMUM SCORE: n/a\n\n";
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

                // Set the API limit.
                tags += "&limit=320";

                // Get XML of page.
                changeTask("Downloading tag information for page 1...");
                url = tagJson + tags;
                xml = apiTools.getJSON(url, webHeader);
                if (apiTools.isXmlDead(xml))
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

                // Determine the pages by counting the posts.
                int itemCount = xmlID.Count;
                if (itemCount == 320) {
                    morePages = true;
                    pageCount++;
                }

                // Begin parsing the XML for tag information per item.
                for (int i = 0; i < xmlID.Count; i++) {
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

                            labelBuffer = string.Format("{0} posts ({1} e, {2} q, {3} s\n{4} blacklisted posts ({5} e, {6} q, {7} s)\n{8} skipped (zero-tolerance)\n{9} total files", counts);
                        }
                        else {
                            object[] counts = new object[] { cleanTotalCount.ToString(), graylistTotalCount.ToString(), blacklistCount.ToString(), totalCount.ToString() };

                            labelBuffer = string.Format("{0} posts\n{1} blacklisted\n{2} skipped (zero-tolerance)\n{3} total files", counts);
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

                        blacklistInfo += "POST " + xmlID[i].InnerText + ":\n    MD5: " + xmlMD5[i].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[i].InnerText + "\n    SCORE: " + xmlScore[i].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[i].InnerText + "\"\n    OFFENDING TAGS: " + foundGraylistedTags + "\n\n";
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

                        tagInfo += "POST " + xmlID[i].InnerText + ":\n    MD5: " + xmlMD5[i].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[i].InnerText + "\n    SCORE: " + xmlScore[i].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[i].InnerText + "\"\n\n";
                    }
                }

                // Check for extra pages and then parse them as well.
                if (morePages) {
                    bool deadPage = false;
                    while (!deadPage) {
                        changeTask("Downloading tag information for page " + (pageCount) + "...");
                        url = tagJson + tags + limitJson + "320" + pageJson + pageCount;
                        xml = apiTools.getJSON(tagJson + tags + limitJson + "320" + pageJson + pageCount, webHeader);

                        if (usePageLimit && pageCount == pageLimit)
                            break;

                        if (apiTools.isXmlDead(xml)) {
                            deadPage = true;
                            break;
                        }

                        // Everything below here is basically a copy & paste from above.
                        doc.LoadXml(xml);
                        xmlID = doc.DocumentElement.SelectNodes("/root/item/id");
                        xmlMD5 = doc.DocumentElement.SelectNodes("/root/item/md5");
                        xmlURL = doc.DocumentElement.SelectNodes("/root/item/file_url");
                        xmlArtist = doc.DocumentElement.SelectNodes("/root/item/artist");
                        xmlTags = doc.DocumentElement.SelectNodes("/root/item/tags");
                        xmlScore = doc.DocumentElement.SelectNodes("/root/item/score");
                        xmlRating = doc.DocumentElement.SelectNodes("/root/item/rating");
                        xmlDescription = doc.DocumentElement.SelectNodes("/root/item/description");

                        for (int i = 0; i < xmlID.Count; i++) {
                            string artists = string.Empty;
                            string rating = xmlRating[i].InnerText;
                            bool isGraylisted = false;
                            bool isBlacklisted = false;
                            List<string> foundTags = xmlTags[i].InnerText.Split(' ').ToList();
                            string foundGraylistedTags = string.Empty;
                            if (imageLimit > 0 && imageLimit == totalCount)
                                break;

                            if (rating == "e")
                                rating = "Explicit";
                            else if (rating == "q")
                                rating = "Questionable";
                            else if (rating == "s")
                                rating = "Safe";
                            else
                                rating = "Unknown";

                            for (int j = 0; j < xmlArtist[i].ChildNodes.Count; j++)
                                artists += xmlArtist[i].ChildNodes[j].InnerText + "\n               ";
                            artists = artists.TrimEnd(' ');
                            artists = artists.TrimEnd('\n');

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

                                    labelBuffer = string.Format("{0} posts ({1} e, {2} q, {3} s\n{4} blacklisted posts ({5} e, {6} q, {7} s)\n{8} skipped (zero-tolerance)\n{9} total files", counts);
                                }
                                else {
                                    object[] counts = new object[] { cleanTotalCount.ToString(), graylistTotalCount.ToString(), blacklistCount.ToString(), totalCount.ToString() };

                                    labelBuffer = string.Format("{0} posts\n{1} blacklisted\n{2} skipped (zero-tolerance)\n{3} total files", counts);
                                }
                                lbBlacklist.Text = labelBuffer;
                            }));

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
                            else {
                                if (useMinimumScore && Int32.Parse(xmlScore[i].InnerText) < minimumScore)
                                    continue;

                                if (!ratings.Any(xmlRating[i].InnerText.Contains))
                                    continue;
                            }
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

                                blacklistInfo += "POST " + xmlID[i].InnerText + ":\n    MD5: " + xmlMD5[i].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[i].InnerText + "\n    SCORE: " + xmlScore[i].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[i].InnerText + "\"\n    OFFENDING TAGS: " + foundGraylistedTags + "\n\n";
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

                                tagInfo += "POST " + xmlID[i].InnerText + ":\n    MD5: " + xmlMD5[i].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[i].InnerText + "\n    SCORE: " + xmlScore[i].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[i].InnerText + "\"\n\n";
                            }
                        }

                        pageCount++;
                    }
                }

                // Create output folders
                if (separateRatings) {
                    if (ExplicitURLs.Count > 0)
                        if (!Directory.Exists(saveTo + "\\explicit"))
                            Directory.CreateDirectory(saveTo + "\\explicit");
                    if (QuestionableURLs.Count > 0)
                        if (!Directory.Exists(saveTo + "\\questionable"))
                            Directory.CreateDirectory(saveTo + "\\questionable");
                    if (SafeURLs.Count > 0)
                        if (!Directory.Exists(saveTo + "\\safe"))
                            Directory.CreateDirectory(saveTo + "\\safe");

                    if (saveBlacklistedFiles) {
                        if (GraylistedExplicitURLs.Count > 0)
                            if (!Directory.Exists(saveTo + "\\explicit\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\explicit\\blacklisted");
                        if (GraylistedQuestionableURLs.Count > 0)
                            if (!Directory.Exists(saveTo + "\\questionable\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\questionable\\blacklisted");
                        if (GraylistedSafeURLs.Count > 0)
                            if (!Directory.Exists(saveTo + "\\safe\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\safe\\blacklisted");
                    }
                }
                else {
                    if (!Directory.Exists(saveTo))
                        Directory.CreateDirectory(saveTo);

                    if (saveBlacklistedFiles && GraylistedURLs.Count > 0)
                        if (!Directory.Exists(saveTo + "\\blacklisted"))
                            Directory.CreateDirectory(saveTo + "\\blacklisted");
                }

                // Save the info files from the buffer
                if (saveInfo) {
                    tagInfo.TrimEnd('\n');
                    File.WriteAllText(saveTo + "\\tags.nfo", tagInfo, Encoding.UTF8);

                    if (saveBlacklistedFiles) {
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

                        labelBuffer = string.Format("{0} posts ({1} e, {2} q, {3} s\n{4} blacklisted posts ({5} e, {6} q, {7} s)\n{8} skipped (zero-tolerance)\n{9} total files", counts);
                    }
                    else {
                        object[] counts = new object[] { cleanTotalCount.ToString(), graylistTotalCount.ToString(), blacklistCount.ToString(), totalCount.ToString() };

                        labelBuffer = string.Format("{0} posts\n{1} blacklisted\n{2} skipped (zero-tolerance)\n{3} total files", counts);
                    }
                    lbBlacklist.Text = labelBuffer;
                }));

                // Start the download
                using (WebClient wc = new WebClient()) {
                    wc.DownloadProgressChanged += (s, e) => {
                        if (!this.IsDisposed) {
                            //if (!sizeRecieved) {
                            //    //this.Invoke((MethodInvoker)(() => pbDownloadStatus.Maximum = 101));
                            //    if (!lbFile.Text.Contains(("(" + e.TotalBytesToReceive / 1024) + "kb)"))
                            //        this.Invoke((MethodInvoker)(() => lbFile.Text += " (" + (e.TotalBytesToReceive / 1024) + "kb)"));
                            //    sizeRecieved = true;
                            //    Debug.Print((e.TotalBytesToReceive / 1024).ToString());
                            //}
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
                    wc.Headers.Add(webHeader);

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
                    status.Text = "Finished downloading tags";
                    this.Text = "Tags download completed";
                }));
                return true;
            }
            catch (ThreadAbortException thrEx) {
                Debug.Print("Thread was requested to be, and has been, aborted.");
                Debug.Print("========== BEGIN THREADABORTEXCEPTION ==========");
                Debug.Print(thrEx.ToString());
                Debug.Print("========== END THREADABORTEXCEPTION ==========");
                return false;
                throw thrEx;
            }
            catch (WebException WebE) {
                Debug.Print("A WebException has occured.");
                Debug.Print("========== BEGIN WEBEXCEPTION ==========");
                Debug.Print(WebE.ToString());
                Debug.Print("========== END WEBEXCEPTION ==========");
                apiTools.webError(WebE, url);
                return false;
                throw WebE;
            }
            catch (Exception ex) {
                Debug.Print("A gneral exception has occured.");
                Debug.Print("========== BEGIN EXCEPTION ==========");
                Debug.Print(ex.ToString());
                Debug.Print("========== END EXCEPTION ==========");
                return false;
                throw ex;
            }
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
                string newTagName = tags;
                for (int i = 0; i < badFolderChars.Length; i++)                                             // Replace bad characters (if present).
                    newTagName = newTagName.Replace(badFolderChars[i], replacementFolderChars[i]);
                if (useMinimumScore)                                                                        // Add minimum score to folder name.
                    newTagName += " (scores " + (minimumScore) + "+)";
                if (!this.saveTo.EndsWith("\\Pages\\" + newTagName + " (page " + pageNumber + ")"))   // Set the output folder.
                    this.saveTo += "\\Pages\\" + newTagName + " (page " + pageNumber + ")";

            // Start the buffer for the .nfo files.
                if (useMinimumScore) {
                    tagInfo = "TAGS: " + tags + "\nMINIMUM SCORE: " + minimumScore + "\n\n";
                    blacklistInfo = "TAGS: " + tags + "\nBLACKLISTED TAGS: " + graylist + "\nMINIMUM SCORE: " + minimumScore + "\n\n";
                }
                else {
                    tagInfo = "TAGS: " + tags + "\nMINIMUM SCORE: n/a\n\n";
                    blacklistInfo = "TAGS: " + tags + "\nBLACKLISTED TAGS: " + graylist + "\nMINIMUM SCORE: n/a\n\n";
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
                xml = apiTools.getJSON(url, webHeader);
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

                                    labelBuffer = string.Format("{0} posts ({1} e, {2} q, {3} s\n{4} blacklisted posts ({5} e, {6} q, {7} s)\n{8} skipped (zero-tolerance)\n{9} total files", counts);
                                }
                                else {
                                    object[] counts = new object[] { cleanTotalCount.ToString(), graylistTotalCount.ToString(), blacklistCount.ToString(), totalCount.ToString() };

                                    labelBuffer = string.Format("{0} posts\n{1} blacklisted\n{2} skipped (zero-tolerance)\n{3} total files", counts);
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

                        blacklistInfo += "POST " + xmlID[i].InnerText + ":\n    MD5: " + xmlMD5[i].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[i].InnerText + "\n    SCORE: " + xmlScore[i].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[i].InnerText + "\"\n    OFFENDING TAGS: " + foundGraylistedTags + "\n\n";
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

                        tagInfo += "POST " + xmlID[i].InnerText + ":\n    MD5: " + xmlMD5[i].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[i].InnerText + "\n    SCORE: " + xmlScore[i].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[i].InnerText + "\"\n\n";
                    }
                }

            // Create output folders
                if (separateRatings) {
                    if (ExplicitURLs.Count > 0)
                        if (!Directory.Exists(saveTo + "\\explicit"))
                            Directory.CreateDirectory(saveTo + "\\explicit");
                    if (QuestionableURLs.Count > 0)
                        if (!Directory.Exists(saveTo + "\\questionable"))
                            Directory.CreateDirectory(saveTo + "\\questionable");
                    if (SafeURLs.Count > 0)
                        if (!Directory.Exists(saveTo + "\\safe"))
                            Directory.CreateDirectory(saveTo + "\\safe");

                    if (saveBlacklistedFiles) {
                        if (GraylistedExplicitURLs.Count > 0)
                            if (!Directory.Exists(saveTo + "\\explicit\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\explicit\\blacklisted");
                        if (GraylistedQuestionableURLs.Count > 0)
                            if (!Directory.Exists(saveTo + "\\questionable\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\questionable\\blacklisted");
                        if (GraylistedSafeURLs.Count > 0)
                            if (!Directory.Exists(saveTo + "\\safe\\blacklisted"))
                                Directory.CreateDirectory(saveTo + "\\safe\\blacklisted");
                    }
                }
                else {
                    if (!Directory.Exists(saveTo))
                        Directory.CreateDirectory(saveTo);

                    if (saveBlacklistedFiles && GraylistedURLs.Count > 0)
                        if (!Directory.Exists(saveTo + "\\blacklisted"))
                            Directory.CreateDirectory(saveTo + "\\blacklisted");
                }

            // Save the info files from the buffer
                if (saveInfo) {
                    tagInfo.TrimEnd('\n');
                    File.WriteAllText(saveTo + "\\tags.nfo", tagInfo, Encoding.UTF8);

                    if (saveBlacklistedFiles) {
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

                        labelBuffer = string.Format("{0} posts ({1} e, {2} q, {3} s\n{4} blacklisted posts ({5} e, {6} q, {7} s)\n{8} skipped (zero-tolerance)\n{9} total files", counts);
                    }
                    else {
                        object[] counts = new object[] { cleanTotalCount.ToString(), graylistTotalCount.ToString(), blacklistCount.ToString(), totalCount.ToString() };

                        labelBuffer = string.Format("{0} posts\n{1} blacklisted\n{2} skipped (zero-tolerance)\n{3} total files", counts);
                    }
                    lbBlacklist.Text = labelBuffer;
                }));

            // Start the download
                using (WebClient wc = new WebClient()) {
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
            catch (ThreadAbortException thrEx) {
                Debug.Print("Thread was requested to be, and has been, aborted.");
                Debug.Print("========== BEGIN THREADABORTEXCEPTION ==========");
                Debug.Print(thrEx.ToString());
                Debug.Print("========== END THREADABORTEXCEPTION ==========");
                return false;
                throw thrEx;
            }
            catch (WebException WebE) {
                Debug.Print("A WebException has occured.");
                Debug.Print("========== BEGIN WEBEXCEPTION ==========");
                Debug.Print(WebE.ToString());
                Debug.Print("========== END WEBEXCEPTION ==========");
                apiTools.webError(WebE, url);
                return false;
                throw WebE;
            }
            catch (Exception ex) {
                Debug.Print("A gneral exception has occured.");
                Debug.Print("========== BEGIN EXCEPTION ==========");
                Debug.Print(ex.ToString());
                Debug.Print("========== END EXCEPTION ==========");
                return false;
                throw ex;
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