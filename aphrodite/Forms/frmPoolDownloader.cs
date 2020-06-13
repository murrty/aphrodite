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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace aphrodite {
    public partial class frmPoolDownloader : Form {

        #region Variables
        public string poolID;                   // String for the pool id.
        public string header;                   // String for the header.
        public string saveTo;                   // String for the save directory.
        public string staticSaveTo;             // String for the initial saveTo string.
        public string graylist;                 // String for the graylist.
        public string blacklist;                // String for the blacklist.
        public string url;                      // String for the currently downloading URL.

        public bool saveInfo;                   // Global setting for saving images.nfo file.
        public bool ignoreFinish;               // Global setting for exiting after finishing.
        public bool saveBlacklisted;            // Global setting for saving blacklisted files.
        public bool saveMetadata;               // Global setting for saving metadata to files
        public bool saveArtistMetadata;         // Global setting for saving artists to the file's metadata
        public bool saveTagMetadata;            // Global setting for saving tags to the file's metadata

        public bool usePoolName;                // Setting for using the pool name in the file name.
        public bool mergeBlacklisted;           // Setting for merging blacklisted pages with regular pages.
        public bool openAfter;                  // Setting for opening the folder after download.
        public string fileNameSchema;           // The name of the files to be created
        // %poolname%   = the name of the pool
        // %poolid%     = the id of the pool
        // %page%       = the page number of the page
        // %md5%        = the md5 of the file
        // %id%         = the id of the page
        // %rating%     = the rating of the page (eg: safe)
        // %rating2%    = the lettered rating of the page (eg: s)
        // %artist%     = the first artist in the artists array
        // %ext%        = the extension
        // %fav_count%  = the amount of favorites the post has
        // %score%      = the score of the post
        // %author%     = the user who submitted the post to e621

        public int pageCount = 0;               // Will update the page count before download.
        public int blacklistedPageCount = 0;    // Will count the blacklisted pages before download.

        //public static readonly string poolJson = "https://e621.net/pool/show.json?id=";
        public static readonly string poolJson = "https://e621.net/pools/{0}.json";
        public static readonly string poolJsonSearch = "https://e621.net/posts?tags=pool:";
        public static readonly string poolPageJson = "&page=";

        Metadata writeMetadata = new Metadata();

        Thread poolDownloader;
        #endregion

        #region Form
        public frmPoolDownloader() {
            InitializeComponent();
            this.Icon = Properties.Resources.Brad;
        }

        private void frmDownload_Load(object sender, EventArgs e) {
            lbID.Text = "Pool ID " + poolID;
        }
        private void frmDownload_Shown(object sender, EventArgs e) {
            startDownload();
        }
        private void frmDownload_FormClosing(object sender, FormClosingEventArgs e) {
            poolDownloader.Abort();
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
            MessageBox.Show("Pool downloading is wip");
            return;
            poolDownloader = new Thread(() => {
                Thread.CurrentThread.IsBackground = true;
                if (downloadPool()) {
                    if (openAfter)
                        Process.Start(saveTo);

                    if (ignoreFinish)
                        this.DialogResult = DialogResult.OK;
                }
                else
                    this.DialogResult = DialogResult.Abort;
            });
            tmrTitle.Start();
            poolDownloader.Start();
        }

        private bool downloadPool() {
            try {
                // Set the saveTo to \\Pools.
                if (!saveTo.EndsWith("\\Pools"))
                    saveTo += "\\Pools";

                staticSaveTo = saveTo;
                writeToConsole("Set output to " + saveTo);

                // New variables for the API parse
                List<string> URLs = new List<string>();
                List<string> FileNames = new List<string>();
                List<string> MetadataArtists = new List<string>();
                List<string> MetadataTags = new List<string>();
                List<bool> urlBlacklisted = new List<bool>();
                List<string> GraylistedTags = new List<string>(graylist.Split(' '));
                List<string> BlacklistedTags = new List<string>(blacklist.Split(' '));
                string poolName = string.Empty;
                string poolInfo = string.Empty;
                string blacklistInfo = string.Empty;
                bool hasBlacklistedFiles = false;
                XmlDocument xmlDoc = new XmlDocument();
                writeToConsole("Configured variables");

                // Begin the XML download
                this.Invoke((MethodInvoker)(() => status.Text = "Getting pool information for page 1"));
                writeToConsole("Starting JSON download for page 1...", true);
                url = poolJson + poolID;
                string postXML = apiTools.getJSON(poolJson + poolID, header);
                writeToConsole("JSON Downloaded.", true);

                // Check the XML.
                writeToConsole("Checking XML...");
                if (postXML == apiTools.emptyXML || string.IsNullOrWhiteSpace(postXML)) {
                    this.BeginInvoke(new MethodInvoker(() => {
                        status.Text = "Pool failed to download.";
                        tmrTitle.Stop();
                        this.Text = "Pool download failed";
                        pbDownloadStatus.Style = ProgressBarStyle.Continuous;
                    }));
                    MessageBox.Show("The pool could not be downloaded.");
                    return false;
                }
                writeToConsole("XML is valid.");
                xmlDoc.LoadXml(postXML);

                // XmlNodeLists for pool information.
                XmlNodeList xmlName = xmlDoc.DocumentElement.SelectNodes("/root/name");
                XmlNodeList xmlDescription = xmlDoc.DocumentElement.SelectNodes("/root/description");
                XmlNodeList xmlCount = xmlDoc.DocumentElement.SelectNodes("/root/post_count");

                poolInfo += "POOL: " + poolID + "\n" +
                            "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\n" +
                            "    NAME: " + xmlName[0].InnerText + "\n" +
                            "    PAGES: " + xmlCount[0].InnerText + "\n" +
                            "    URL: https://e621.net/pool/show/" + poolID + "\n" +
                            "    DESCRIPTION:\n    \"" + xmlDescription[0].InnerText + "\"" +
                            "\n\n";

                blacklistInfo += "POOL: " + poolID + "\n" +
                                 "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\n" +
                                 "    NAME: " + xmlName[0].InnerText + "\n" +
                                 "    PAGES: " + xmlCount[0].InnerText + "\n" +
                                 "    URL: https://e621.net/pool/show/" + poolID + "\n" +
                                 "    DESCRIPTION:\n    \"" + xmlDescription[0].InnerText + "\"\n" +
                                 "    BLACKLISTED TAGS: " + graylist +
                                 "\n\n";

                this.Invoke((MethodInvoker)(() => lbName.Text = xmlName[0].InnerText));

                // Count the image count and do math for pages.
                int pages = 1;
                int imageCount = 0;
                Int32.TryParse(xmlCount[0].InnerText, out imageCount);
                if (imageCount > 24) {
                    decimal pageGuess = decimal.Divide(imageCount, 24);
                    pages = Convert.ToInt32(Math.Ceiling(pageGuess));
                    writeToConsole("Counted " + pages + " pages with " + imageCount + " files in total.", true);
                }
                else {
                    writeToConsole("Counted 1 page with " + imageCount + " files in total.", true);
                }

                // Set the output folder name.
                poolName = apiTools.replaceIllegalCharacters(xmlName[0].InnerText);
                saveTo += "\\" + poolName;
                writeToConsole("Updated saveTo to \\Pools\\" + poolName);

                // Begin ripping the rest of the pool Json.
                XmlNodeList xmlID = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/id");
                XmlNodeList xmlMD5 = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/md5");
                XmlNodeList xmlUrl = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/file_url");
                XmlNodeList xmlTags = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags");
                XmlNodeList xmlArtist = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/artist");
                XmlNodeList xmlScore = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/score");
                XmlNodeList xmlRating = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/rating");
                XmlNodeList xmlExt = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/file_ext");
                XmlNodeList xmlFavCount = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/fav_count");
                XmlNodeList xmlAuthor = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/author");
                xmlDescription = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/description");
                int currentPage = 0;
                for (int i = 0; i < xmlID.Count; i++) {
                    string artists = string.Empty;
                    string artistsMetadata = string.Empty;
                    bool isBlacklisted = false;
                    string foundBlacklistedTags = string.Empty;
                    string rating;
                    List<string> foundTags = xmlTags[i].InnerText.Split(' ').ToList();
                    string tagsMetadata = xmlTags[i].InnerText;
                    currentPage++;

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

                    if (xmlArtist[i].ChildNodes.Count > 0) {
                        for (int j = 0; j < xmlArtist[i].ChildNodes.Count; j++) {
                            artists += xmlArtist[i].ChildNodes[j].InnerText + "\n                   ";
                            artistsMetadata += xmlArtist[i].ChildNodes[j].InnerText + " ";
                        }
                        artists = artists.TrimEnd(' ');
                        artists = artists.TrimEnd('\n');
                        artistsMetadata = artistsMetadata.TrimEnd(' ');
                    }
                    else {
                        artists = "(none)";
                        artistsMetadata = "(no artist)";
                    }

                    // Add to the metadata list
                    if (saveMetadata) {
                        MetadataArtists.Add(artistsMetadata);
                        MetadataTags.Add(tagsMetadata);
                    }

                    string fileNamePage = string.Empty;
                    if (currentPage < 1000) {
                        fileNamePage += "0";
                        if (currentPage < 100) {
                            fileNamePage += "0";
                            if (currentPage < 10)
                                fileNamePage += "0";
                        }
                    }
                    fileNamePage += (currentPage);

                    // File name artist for the schema
                    string fileNameArtist = "(none)";
                    bool useHardcodedFilter = false;
                    if (string.IsNullOrEmpty(Settings.Default.undesiredTags))
                        useHardcodedFilter = true;

                    if (xmlArtist.Count > 0) {
                        if (!string.IsNullOrEmpty(xmlArtist[0].InnerText)) {
                            for (int j = 0; j < xmlArtist.Count; j++) {
                                if (useHardcodedFilter) {
                                    if (!UndesiredTags.isUndesiredHardcoded(xmlArtist[j].InnerText)) {
                                        fileNameArtist = xmlArtist[j].InnerText;
                                        break;
                                    }
                                }
                                else {
                                    if (!UndesiredTags.isUndesired(xmlArtist[j].InnerText)) {
                                        fileNameArtist = xmlArtist[j].InnerText;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    string fileName = fileNameSchema.Replace("%poolname%", poolName)
                                                    .Replace("%poolid%", poolID)
                                                    .Replace("%page%", fileNamePage)
                                                    .Replace("%md5%", xmlMD5[i].InnerText)
                                                    .Replace("%id%", xmlID[i].InnerText)
                                                    .Replace("%rating%", rating.ToLower())
                                                    .Replace("%rating2%", xmlRating[i].InnerText)
                                                    .Replace("%artist%", fileNameArtist)
                                                    .Replace("%ext%", xmlExt[i].InnerText)
                                                    .Replace("%fav_count%", xmlFavCount[i].InnerText)
                                                    .Replace("%score%", xmlScore[i].InnerText)
                                                    .Replace("%author%", xmlAuthor[i].InnerText) + "." + xmlExt[i].InnerText;


                    for (int j = 0; j < foundTags.Count; j++) {
                        if (GraylistedTags.Count > 0) {
                            for (int k = 0; k < GraylistedTags.Count; k++) {
                                if (foundTags[j] == GraylistedTags[k]) {
                                    foundBlacklistedTags += " " + GraylistedTags[k];
                                    isBlacklisted = true;
                                    hasBlacklistedFiles = true;
                                }
                            }
                        }

                        if (BlacklistedTags.Count > 0) {
                            for (int k = 0; k < BlacklistedTags.Count; k++) {
                                if (foundTags[j] == BlacklistedTags[k]) {
                                    foundBlacklistedTags += " " + BlacklistedTags[k];
                                    isBlacklisted = true;
                                    hasBlacklistedFiles = true;
                                }
                            }
                        }
                    }


                    URLs.Add(xmlUrl[i].InnerText);
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

                    if (isBlacklisted && saveBlacklisted) {
                        if (!mergeBlacklisted) {
                            blacklistInfo += "    BLACKLISTED PAGE " + (currentPage) + ":\n" +
                                             "        MD5: " + xmlMD5[i].InnerText + "\n" +
                                             "        URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n" +
                                             "        ARTIST(S): " + artists + "\n" +
                                             "        TAGS: " + xmlTags[i].InnerText + "\n" +
                                             "        SCORE: " + xmlScore[i].InnerText + "\n" +
                                             "        RATING: " + rating + "\n" +
                                             "        DESCRIPITON:\n        \"" + xmlDescription[i].InnerText + "\"\n" +
                                             "        OFFENDING TAGS: " + foundBlacklistedTags +
                                             "\n\n";
                        }
                        else {
                            poolInfo += "    BLACKLISTED PAGE: " + (currentPage) + "\n" +
                                        "        MD5: " + xmlMD5[i].InnerText + "\n" +
                                        "        URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n" +
                                        "        ARTIST(S): " + artists + "\n" +
                                        "        TAGS: " + xmlTags[i].InnerText + "\n" +
                                        "        SCORE: " + xmlScore[i].InnerText + "\n" +
                                        "        RATING: " + rating + "\n" +
                                        "        DESCRIPITON:\n        \"" + xmlDescription[i].InnerText + "\"\n" +
                                        "        OFFENDING TAGS: " + foundBlacklistedTags +
                                        "\n\n";
                        }

                    }
                    else {
                        poolInfo += "    PAGE: " + (currentPage) + "\n" +
                                    "        MD5: " + xmlMD5[i].InnerText + "\n" +
                                    "        URL: https://e621.net/post/show/" + xmlID[i].InnerText + "\n" +
                                    "        ARTIST(S): " + artists + "\n" +
                                    "        TAGS: " + xmlTags[i].InnerText + "\n" +
                                    "        SCORE: " + xmlScore[i].InnerText + "\n" +
                                    "        RATING: " + rating + "\n" +
                                    "        DESCRIPITON:\n        \"" + xmlDescription[i].InnerText +
                                    "\"\n\n";
                    }
                }

                // Redo above but for other pages.
                if (pages > 1) {
                    for (int i = 2; i < pages + 1; i++) {
                        writeToConsole("Starting JSON download for page " + i + "...", true);
                        this.Invoke((MethodInvoker)(() => status.Text = "Getting pool information for page " + i));
                        url = poolJson + poolID;
                        postXML = apiTools.getJSON(poolJson + poolID + poolPageJson + i, header);
                        writeToConsole("JSON Downloaded.", true);

                        writeToConsole("Checking XML...");
                        if (postXML == apiTools.emptyXML || string.IsNullOrWhiteSpace(postXML)) {
                            writeToConsole("XML is empty, assuming dead page.");
                            break;
                        }
                        writeToConsole("XML is valid.");
                        xmlDoc.LoadXml(postXML);

                        xmlID = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/id");
                        xmlMD5 = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/md5");
                        xmlUrl = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/file_url");
                        xmlTags = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/tags");
                        xmlArtist = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/artist");
                        xmlScore = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/score");
                        xmlRating = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/rating");
                        xmlExt = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/file_ext");
                        xmlFavCount = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/fav_count");
                        xmlAuthor = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/author");
                        xmlDescription = xmlDoc.DocumentElement.SelectNodes("/root/posts/item/description");
                        for (int j = 0; j < xmlID.Count; j++) {
                            string artists = string.Empty;
                            string artistsMetadata = string.Empty;
                            bool isBlacklisted = false;
                            string foundBlacklistedTags = string.Empty;
                            string rating;
                            List<string> foundTags = xmlTags[j].InnerText.Split(' ').ToList();
                            string tagsMetadata = xmlTags[j].InnerText;
                            currentPage++;

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

                            if (xmlArtist[i].ChildNodes.Count > 0) {
                                for (int k = 0; k < xmlArtist[i].ChildNodes.Count; k++) {
                                    artists += xmlArtist[i].ChildNodes[k].InnerText + "\n                   ";
                                    artistsMetadata += xmlArtist[i].ChildNodes[j].InnerText + " ";
                                }
                                artists = artists.TrimEnd(' ');
                                artists = artists.TrimEnd('\n');
                                artistsMetadata = artistsMetadata.TrimEnd(' ');
                            }
                            else {
                                artists = "(none)";
                                artistsMetadata = "(no artist)";
                            }

                            if (saveMetadata) {
                                MetadataArtists.Add(artistsMetadata);
                                MetadataTags.Add(tagsMetadata);
                            }

                            string fileNamePage = string.Empty;
                            if (currentPage < 1000) {
                                fileNamePage += "0";
                                if (currentPage < 100)
                                    fileNamePage += "0";
                            }
                            fileNamePage += (currentPage);

                            string fileNameArtist;
                            if (xmlArtist[i].ChildNodes.Count > 0) {
                                if (string.IsNullOrEmpty(xmlArtist[i].ChildNodes[0].InnerText)) {
                                    fileNameArtist = "(none)";
                                }
                                else {
                                    if (xmlArtist[i].ChildNodes[0].InnerText != "conditional_dnp") {
                                        fileNameArtist = xmlArtist[i].ChildNodes[0].InnerText;
                                    }
                                    else {
                                        fileNameArtist = xmlArtist[i].ChildNodes[1].InnerText;
                                    }
                                }
                            }
                            else {
                                fileNameArtist = "unknown_artist";
                            }
                            string fileName = fileNameSchema.Replace("%poolname%", poolName)
                                                            .Replace("%poolid%", poolID)
                                                            .Replace("%page%", fileNamePage)
                                                            .Replace("%md5%", xmlMD5[i].InnerText)
                                                            .Replace("%id%", xmlID[i].InnerText)
                                                            .Replace("%rating%", rating.ToLower())
                                                            .Replace("%rating2%", xmlRating[i].InnerText)
                                                            .Replace("%artist%", fileNameArtist)
                                                            .Replace("%ext%", xmlExt[i].InnerText)
                                                            .Replace("%fav_count%", xmlFavCount[i].InnerText)
                                                            .Replace("%score%", xmlScore[i].InnerText)
                                                            .Replace("%author%", xmlAuthor[i].InnerText) + "." + xmlExt[i].InnerText;

                            for (int k = 0; k < foundTags.Count; k++) {
                                if (GraylistedTags.Count > 0) {
                                    for (int l = 0; l < GraylistedTags.Count; l++) {
                                        if (foundTags[k] == GraylistedTags[l]) {
                                            foundBlacklistedTags += " " + GraylistedTags[l];
                                            isBlacklisted = true;
                                            hasBlacklistedFiles = true;
                                        }
                                    }
                                }

                                if (BlacklistedTags.Count > 0) {
                                    for (int l = 0; l < BlacklistedTags.Count; l++) {
                                        if (foundTags[k] == BlacklistedTags[l]) {
                                            foundBlacklistedTags += " " + BlacklistedTags[l];
                                            isBlacklisted = true;
                                            hasBlacklistedFiles = true;
                                        }
                                    }
                                }
                            }


                            URLs.Add(xmlUrl[j].InnerText);
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

                            if (isBlacklisted && saveBlacklisted) {
                                if (!mergeBlacklisted) {
                                    blacklistInfo += "    BLACKLISTED PAGE " + (currentPage) + ":\n" +
                                                     "        MD5: " + xmlMD5[j].InnerText + "\n" +
                                                     "        URL: https://e621.net/post/show/" + xmlID[j].InnerText + "\n" +
                                                     "        ARTIST(S): " + artists + "\n" +
                                                     "        TAGS: " + xmlTags[j].InnerText + "\n" +
                                                     "        SCORE: " + xmlScore[j].InnerText + "\n" +
                                                     "        RATING: " + rating + "\n" +
                                                     "        DESCRIPITON:\n        \"" + xmlDescription[j].InnerText + "\"\n" +
                                                     "        OFFENDING TAGS: " + foundBlacklistedTags +
                                                     "\n\n";
                                }
                                else {
                                    poolInfo += "    BLACKLIST PAGE: " + (currentPage) + "\n" +
                                                "        MD5: " + xmlMD5[j].InnerText + "\n" +
                                                "        URL: https://e621.net/post/show/" + xmlID[j].InnerText + "\n" +
                                                "        ARTIST(S): " + artists + "\n" +
                                                "        TAGS: " + xmlTags[j].InnerText + "\n" +
                                                "        SCORE: " + xmlScore[j].InnerText + "\n" +
                                                "        RATING: " + rating + "\n" +
                                                "        DESCRIPITON:\n        \"" + xmlDescription[j].InnerText + "\"\n" +
                                                "        OFFENDING TAGS: " + foundBlacklistedTags +
                                                "\n\n";
                                }

                            }
                            else {
                                poolInfo += "    PAGE: " + (currentPage) + "\n" +
                                            "        MD5: " + xmlMD5[j].InnerText + "\n" +
                                            "        URL: https://e621.net/post/show/" + xmlID[j].InnerText + "\n" +
                                            "        ARTIST(S): " + artists + "\n" +
                                            "        TAGS: " + xmlTags[j].InnerText + "\n" +
                                            "        SCORE: " + xmlScore[j].InnerText + "\n" +
                                            "        RATING: " + rating + "\n" +
                                            "        DESCRIPITON:\n        \"" + xmlDescription[j].InnerText + "\"" +
                                            "\n\n";
                            }
                        }
                    }
                }

                // Check for files.
                if (URLs.Count <= 0) {
                    writeToConsole("No files were found while downloading. Press any key to continue...", true);
                    this.Invoke((MethodInvoker)(() => status.Text = "URL count is empty. Empty pool?"));
                    MessageBox.Show("No files are to be downloaded.");
                    return false;
                }

                writeToConsole("There are " + URLs.Count + " files in total.", true);

                // Create directories.
                if (!Directory.Exists(saveTo))
                    Directory.CreateDirectory(saveTo);
                if (hasBlacklistedFiles && saveBlacklisted && !mergeBlacklisted && !Directory.Exists(saveTo + "\\blacklisted"))
                    Directory.CreateDirectory(saveTo + "\\blacklisted");

                // Save .nfo files.
                if (saveInfo) {
                    poolInfo = poolInfo.TrimEnd('\n');
                    blacklistInfo = blacklistInfo.TrimEnd('\n');
                    writeToConsole("Saving pool.nfo");
                    File.WriteAllText(saveTo + "\\pool.nfo", poolInfo, Encoding.UTF8);
                    if (hasBlacklistedFiles && saveBlacklisted && !mergeBlacklisted) {
                        writeToConsole("Saving pool.blacklisted.nfo");
                        File.WriteAllText(saveTo + "\\blacklisted\\pool.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                    }
                }

                this.BeginInvoke(new MethodInvoker(() => {
                    pbDownloadStatus.Style = ProgressBarStyle.Blocks;
                    int total;

                    if (saveBlacklisted) {
                        total = pageCount + blacklistedPageCount;

                        if (File.Exists(saveTo + "\\pool.nfo"))
                            total -= apiTools.countFiles(saveTo) - 1;
                        if (File.Exists(saveTo + "\\blacklisted\\pool.blacklisted.nfo"))
                            total -= apiTools.countFiles(saveTo + "\\blacklisted") - 1;

                        pbTotalStatus.Maximum = total;
                    }
                    else {
                        total = pageCount;
                        total -= apiTools.countFiles(saveTo) - 1;

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
                            lbFile.Text = "File " + (currentDownloadFile) + " of " + (pageCount + blacklistedPageCount);
                            pbDownloadStatus.Value = e.ProgressPercentage;
                            pbDownloadStatus.Value++;
                            pbDownloadStatus.Value--;
                            lbPercentage.Text = e.ProgressPercentage.ToString() + "%";
                        }));
                    };
                    wc.DownloadFileCompleted += (s, e) => {
                        lock (e.UserState) {
                            this.BeginInvoke(new MethodInvoker(() => {
                                pbDownloadStatus.Value = 0;
                                lbPercentage.Text = "0%";

                                if (pbTotalStatus.Value != pbTotalStatus.Maximum)
                                    pbTotalStatus.Value++;
                                else
                                    lbRemoved.Visible = true;
                            }));
                            currentDownloadFile++;
                            Monitor.Pulse(e.UserState);
                        }
                    };

                    wc.Proxy = WebRequest.GetSystemWebProxy();
                    wc.Headers.Add(header);
                    wc.Method = "GET";

                    for (int y = 0; y < URLs.Count; y++) {
                        url = URLs[y].Replace("www.", "");
                        currentPage++;
                        if (urlBlacklisted[y] && !saveBlacklisted)
                            continue;

                        string fileName = "\\" + FileNames[y];

                        this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + fileName));

                        if (urlBlacklisted[y] && !mergeBlacklisted) {
                            if (File.Exists(saveTo + "\\blacklisted" + fileName))
                                continue;

                            var sync = new Object();
                            lock (sync) {
                                wc.DownloadFileAsync(new Uri(url), saveTo + "\\blacklisted" + fileName, sync);
                                Monitor.Wait(sync);
                            }
                        }
                        else {
                            if (File.Exists(saveTo + fileName))
                                continue;

                            var sync = new Object();
                            lock (sync) {
                                wc.DownloadFileAsync(new Uri(url), saveTo + fileName, sync);
                                Monitor.Wait(sync);
                            }
                        }

                        // Save metadata to file, and update it if one exists
                        if (saveMetadata) {
                            if (fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg"))
                                writeMetadata.setMetadata(saveTo + "\\blacklisted" + fileName, MetadataArtists[y], MetadataTags[y]);
                        }
                    }
                }

                // Finish the job.
                this.BeginInvoke(new MethodInvoker(() => {
                    pbDownloadStatus.Value = 100;
                    lbPercentage.Text = "100%";
                    lbFile.Text = "All files downloaded.";
                }));

                if (!ignoreFinish) {
                    MessageBox.Show("Pool " + poolID + " finished downloading.", "aphrodite");
                }
                return true;
            }
            catch (ThreadAbortException thrEx) {
                Debug.Print("Thread was requested to be, and has been, aborted.");
                Debug.Print("==========BEGIN THREADABORTEXCEPTION==========");
                Debug.Print(thrEx.ToString());
                Debug.Print("==========END THREADABORTEXCEPTION==========");
                return false;
                throw thrEx;
            }
            catch (WebException WebE) {
                Debug.Print("A WebException has occured.");
                Debug.Print("==========BEGIN WEBEXCEPTION==========");
                Debug.Print(WebE.ToString());
                Debug.Print("==========END WEBEXCEPTION==========");
                this.BeginInvoke(new MethodInvoker(() => {
                    status.Text = "A WebException has occured";
                    pbDownloadStatus.State = ProgressBarState.Error;
                    pbTotalStatus.State = ProgressBarState.Error;
                }));
                apiTools.webError(WebE, url);
                return false;
                throw WebE;
            }
            catch (Exception ex) {
                Debug.Print("A gneral exception has occured.");
                Debug.Print("==========BEGIN EXCEPTION==========");
                Debug.Print(ex.ToString());
                Debug.Print("==========END EXCEPTION==========");
                this.BeginInvoke(new MethodInvoker(() => {
                    status.Text = "A Exception has occured";
                    pbDownloadStatus.State = ProgressBarState.Error;
                    pbTotalStatus.State = ProgressBarState.Error;
                }));
                return false;
                throw ex;
            }
        }

        public void updateTotals() {
            this.BeginInvoke(new MethodInvoker(() => {
                lbTotal.Text = (pageCount) + " clean pages\n" +
                               (blacklistedPageCount) + " blacklisted pages\n" +
                               (pageCount + blacklistedPageCount) + " total pages";
                if (saveBlacklisted)
                    lbFile.Text = "File 1 of " + (pageCount + blacklistedPageCount);
                else
                    lbFile.Text = "File 1 of " + (pageCount);
            }));
        }
        public void writeToConsole(string message, bool important = false) {
            Debug.Print(message);
        }
        #endregion

    }
}