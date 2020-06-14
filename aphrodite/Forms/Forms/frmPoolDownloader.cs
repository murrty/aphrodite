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

                #region define new variables
                // New variables for the API parse
                string poolJson = string.Format("https://e621.net/pools/{0}.json", poolID);
                string poolJsonPage = string.Format("https://e621.net/posts.json?tags=pool:{0}+order:id&limit=320", poolID);

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
                int currentPage = 0;

                XmlDocument xmlDoc = new XmlDocument();
                writeToConsole("Configured variables");
                #endregion

                #region initial xml download
                // Begin the XML download
                this.Invoke((MethodInvoker)(() => status.Text = "Getting pool information for page 1"));
                writeToConsole("Starting JSON download for page 1...", true);
                url = poolJson;
                string postXML = apiTools.GetJSON(poolJson, header);
                writeToConsole("JSON Downloaded.", true);

                // Check the XML.
                writeToConsole("Checking XML...");
                if (postXML == apiTools.EmptyXML || string.IsNullOrWhiteSpace(postXML)) {
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
                #endregion

                #region initial api parse for main data
                // XmlNodeLists for pool information.
                XmlNodeList xmlName = xmlDoc.DocumentElement.SelectNodes("/root/name");
                XmlNodeList xmlDescription = xmlDoc.DocumentElement.SelectNodes("/root/description");
                XmlNodeList xmlCount = xmlDoc.DocumentElement.SelectNodes("/root/post_count");
                XmlNodeList xmlPoolDeleted = xmlDoc.DocumentElement.SelectNodes("/root/is_deleted");

                if (xmlPoolDeleted[0].InnerText.ToLower() == "true") {
                    // idk how to handle it because I haven't found a deleted pool yet
                }

                string poolDescription = "No description";
                if (xmlDescription[0].InnerText != "") {
                    poolDescription = "\"" + xmlDescription[0].InnerText + "\"";
                }

                poolInfo += "POOL: " + poolID + "\n" +
                            "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\n" +
                            "    NAME: " + xmlName[0].InnerText + "\n" +
                            "    PAGES: " + xmlCount[0].InnerText + "\n" +
                            "    URL: https://e621.net/pool/show/" + poolID + "\n" +
                            "    DESCRIPTION:\n    " + poolDescription +
                            "\n\n";

                blacklistInfo += "POOL: " + poolID + "\n" +
                                 "DOWNLOADED ON: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm (tt)") + "\n" +
                                 "    NAME: " + xmlName[0].InnerText + "\n" +
                                 "    PAGES: " + xmlCount[0].InnerText + "\n" +
                                 "    URL: https://e621.net/pool/show/" + poolID + "\n" +
                                 "    DESCRIPTION:\n    " + poolDescription +
                                 "    BLACKLISTED TAGS: " + graylist +
                                 "\n\n";

                this.Invoke((MethodInvoker)(() => lbName.Text = xmlName[0].InnerText));

                // Count the image count and do math for pages.
                int pages = apiTools.CountPoolPages(Convert.ToDecimal(xmlCount[0].InnerText));

                // Set the output folder name.
                poolName = apiTools.ReplaceIllegalCharacters(xmlName[0].InnerText);
                saveTo += "\\" + poolName;
                writeToConsole("Updated saveTo to \\Pools\\" + poolName);
                #endregion

                #region First page rip
                // Begin ripping the rest of the pool Json.
                xmlDoc = new XmlDocument();
                url = poolJsonPage;
                postXML = apiTools.GetJSON(poolJsonPage + "&page=1", header);
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
                    List<string> foundTags = new List<string>();
                    string ReadTags = string.Empty;
                    string tagsMetadata = string.Empty;
                    currentPage++;

                    ReadTags += "\n             General: [";
                    for (int x = 0; x < xmlTagsGeneral[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsGeneral[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsGeneral[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n             Species: [";
                    for (int x = 0; x < xmlTagsSpecies[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsSpecies[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsSpecies[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n             Character: [";
                    for (int x = 0; x < xmlTagsCharacter[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsCharacter[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsCharacter[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n             Copyright: [";
                    for (int x = 0; x < xmlTagsCopyright[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsCopyright[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsCopyright[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n             Artist: [";
                    for (int x = 0; x < xmlTagsArtist[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsArtist[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsArtist[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n             Invalid: [";
                    for (int x = 0; x < xmlTagsInvalid[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsInvalid[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsInvalid[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n             Lore: [";
                    for (int x = 0; x < xmlTagsLore[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsLore[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsLore[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n             Meta: [";
                    for (int x = 0; x < xmlTagsMeta[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsMeta[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsMeta[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n             Locked tags: [";
                    for (int x = 0; x < xmlTagsLocked[i].ChildNodes.Count; x++) {
                        foundTags.Add(xmlTagsLocked[i].ChildNodes[x].InnerText);
                        ReadTags += xmlTagsLocked[i].ChildNodes[x].InnerText + ", ";
                    }
                    ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                    ReadTags += "]\n";

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
                                                    .Replace ("%scoreup%", xmlScoreUp[i].InnerText)
                                                    .Replace("%scoredown%", xmlScoreDown[i].InnerText)
                                                    .Replace("%author%", xmlAuthor[i].InnerText) + "." + xmlExt[i].InnerText;

                    string fileUrl = xmlURL[i].InnerText;
                    if (fileUrl == null) {
                        if (xmlDeleted[i].InnerText.ToLower() == "false") {
                            fileUrl = apiTools.GetBlacklistedImageUrl(xmlMD5[i].InnerText, xmlExt[i].InnerText);
                        }
                    }

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

                    if (isBlacklisted && saveBlacklisted && !mergeBlacklisted) {
                        blacklistInfo += "    BLACKLISTED PAGE " + (currentPage) + ":\n" +
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
                        poolInfo += "    PAGE: " + (currentPage) + "\n" +
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

                        url = poolJson + poolID;
                        postXML = apiTools.GetJSON(poolJson + poolID + "&page=" + i, header);
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
                            List<string> foundTags = new List<string>();
                            string ReadTags = string.Empty;
                            string tagsMetadata = string.Empty;
                            currentPage++;

                            ReadTags += "\n             General: [";
                            for (int x = 0; x < xmlTagsGeneral[j].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsGeneral[j].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsGeneral[j].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n             Species: [";
                            for (int x = 0; x < xmlTagsSpecies[j].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsSpecies[j].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsSpecies[j].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n             Character: [";
                            for (int x = 0; x < xmlTagsCharacter[j].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsCharacter[j].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsCharacter[j].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n             Copyright: [";
                            for (int x = 0; x < xmlTagsCopyright[j].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsCopyright[j].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsCopyright[j].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n             Artist: [";
                            for (int x = 0; x < xmlTagsArtist[j].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsArtist[j].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsArtist[j].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n             Invalid: [";
                            for (int x = 0; x < xmlTagsInvalid[j].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsInvalid[j].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsInvalid[j].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n             Lore: [";
                            for (int x = 0; x < xmlTagsLore[j].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsLore[j].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsLore[j].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n             Meta: [";
                            for (int x = 0; x < xmlTagsMeta[j].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsMeta[j].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsMeta[j].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n             Locked tags: [";
                            for (int x = 0; x < xmlTagsLocked[j].ChildNodes.Count; x++) {
                                foundTags.Add(xmlTagsLocked[j].ChildNodes[x].InnerText);
                                ReadTags += xmlTagsLocked[j].ChildNodes[x].InnerText + ", ";
                            }
                            ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                            ReadTags += "]\n";

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
                                for (int  x = 0; x < xmlTagsArtist[j].ChildNodes.Count; x++) {
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

                            string fileName = fileNameSchema.Replace("%poolname%", poolName)
                                                            .Replace("%poolid%", poolID)
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
                            if (fileUrl == null) {
                                if (xmlDeleted[j].InnerText.ToLower() == "false") {
                                    fileUrl = apiTools.GetBlacklistedImageUrl(xmlMD5[j].InnerText, xmlExt[j].InnerText);
                                }
                                else {
                                    continue;
                                }
                            }

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

                            if (isBlacklisted && saveBlacklisted && !mergeBlacklisted) {
                                blacklistInfo += "    BLACKLISTED PAGE " + (currentPage) + ":\n" +
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
                                poolInfo += "    PAGE: " + (currentPage) + "\n" +
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
                #endregion

                #region Downloading
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
                            total -= apiTools.CountFiles(saveTo) - 1;
                        if (File.Exists(saveTo + "\\blacklisted\\pool.blacklisted.nfo"))
                            total -= apiTools.CountFiles(saveTo + "\\blacklisted") - 1;

                        pbTotalStatus.Maximum = total;
                    }
                    else {
                        total = pageCount;
                        total -= apiTools.CountFiles(saveTo) - 1;

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
                #endregion

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
            catch (ThreadAbortException) {
                apiTools.SendDebugMessage("Thread was requested to be, and has been, aborted. (frmPoolDownloader.cs)");
                return false;
            }
            catch (ObjectDisposedException) {
                apiTools.SendDebugMessage("Seems like the object got disposed. (frmPoolDownloader.cs)");
                return false;
            }
            catch (WebException WebE) {
                apiTools.SendDebugMessage("A WebException has occured. (frmPoolDownloader.cs)");
                this.BeginInvoke(new MethodInvoker(() => {
                    status.Text = "A WebException has occured";
                    pbDownloadStatus.State = ProgressBarState.Error;
                    pbTotalStatus.State = ProgressBarState.Error;
                }));
                ErrorLog.ReportWebException(WebE, "frmPoolDownloader.cs", url);
                return false;
            }
            catch (Exception ex) {
                apiTools.SendDebugMessage("An exception has occured. (frmPoolDownloader.cs)");
                this.BeginInvoke(new MethodInvoker(() => {
                    status.Text = "A Exception has occured";
                    pbDownloadStatus.State = ProgressBarState.Error;
                    pbTotalStatus.State = ProgressBarState.Error;
                }));
                ErrorLog.ReportException(ex, "frmPoolDownloader.cs", false);
                return false;
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
            apiTools.SendDebugMessage(message);
        }
        #endregion

    }
}