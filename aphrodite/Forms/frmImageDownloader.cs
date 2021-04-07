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
    public partial class frmImageDownloader : Form {

    #region Public Variables
        public ImageDownloadInfo DownloadInfo;
    #endregion

    #region PrivateVariables
        private Thread imageDownload;                   // The thread for the downloader.

        private bool DownloadHasFinished = false;
        private bool DownloadHasErrored = false;
        private bool DownloadHasAborted = false;
    #endregion

    #region Form
        public frmImageDownloader() {
            InitializeComponent();
        }
        private void frmImageDownloader_Load(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(DownloadInfo.FileNameSchema)) {
                DownloadInfo.FileNameSchema = "%artist%_%md5%";
            }
            startDownload();
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
            imageDownload = new Thread(() => {
                Thread.CurrentThread.IsBackground = true;
                downloadImage();
            });
            tmrTitle.Start();
            imageDownload.Start();
        }
        private void AfterDownload() {
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
                lbInfo.Text = "The image has been downloaded";
                pbDownloadStatus.Value = pbDownloadStatus.Maximum;
                lbPercentage.Text = "Done";
                tmrTitle.Stop();
                this.Text = "Image " + DownloadInfo.PostId + " finished downloading";
                status.Text = "Finished downloading pool";
            }
            else if (DownloadHasErrored) {
                lbInfo.Text = "Downloading has encountered an error";
                pbDownloadStatus.State = ProgressBarState.Error;
                lbPercentage.Text = "Error";
                tmrTitle.Stop();
                this.Text = "Download error";
                status.Text = "Downloading has resulted in an error";
            }
            else if (DownloadHasAborted) {
                lbInfo.Text = "Download canceled";
                pbDownloadStatus.State = ProgressBarState.Error;
                lbPercentage.Text = "Canceled";
                tmrTitle.Stop();
                this.Text = "Download canceled";
                status.Text = "Download has canceled";
            }
            else {
                // assume it completed
                lbInfo.Text = "Download assumed to be completed...";
                pbDownloadStatus.Value = pbDownloadStatus.Maximum;
                lbPercentage.Text = "Done?";
                tmrTitle.Stop();
                this.Text = "Image " + DownloadInfo.PostId + " finished downloading";
                status.Text = "Download status booleans not set, assuming the download completed";
            }
        }
        public void downloadImage() {
            try {
            // Set the SaveTo to \\Images.
                if (!DownloadInfo.DownloadPath.EndsWith("\\Images"))
                    DownloadInfo.DownloadPath += "\\Images";

            // Check the start URL to add the extra forward-slash for the split.
                if (DownloadInfo.ImageUrl.StartsWith("http://")) {
                    DownloadInfo.ImageUrl.Replace("http://", "https://");
                }
                if (!DownloadInfo.ImageUrl.StartsWith("https://"))
                    DownloadInfo.ImageUrl = "https://" + DownloadInfo.ImageUrl;

            // Get DownloadInfo.PostID from the split (if exists).
                if (string.IsNullOrEmpty(DownloadInfo.PostId))
                    DownloadInfo.PostId = DownloadInfo.ImageUrl.Split('/')[5];

                this.BeginInvoke(new MethodInvoker(() => {
                    lbInfo.Text = "Downloading image id " + DownloadInfo.PostId;
                    status.Text = "Waiting for JSON parse...";
                }));

            // New varaibles for the API parse.
                string imageInfo = string.Empty;
                string blacklistInfo = string.Empty;
                XmlDocument xmlDoc = new XmlDocument();
                string postJson = string.Format("https://e621.net/posts/{0}.json", DownloadInfo.PostId);

            // Begin to get the XML
                DownloadInfo.ImageUrl = postJson;
                string postXML = apiTools.GetJsonToXml(postJson);

            // Check the XML.
                if (postXML == apiTools.EmptyXML || string.IsNullOrWhiteSpace(postXML)) {
                    apiTools.SendDebugMessage("Xml is empty, aborting.");
                    throw new Exception("XML is reported as empty, cannot parse an empty xml.");
                }

            // Begin parsing XML.
                xmlDoc.LoadXml(postXML);
                apiTools.SendDebugMessage("Gathering post information from XML");
                XmlNodeList xmlID = xmlDoc.DocumentElement.SelectNodes("/root/post/id");
                XmlNodeList xmlMD5 = xmlDoc.DocumentElement.SelectNodes("/root/post/file/md5");
                XmlNodeList xmlExt = xmlDoc.DocumentElement.SelectNodes("/root/post/file/ext");
                XmlNodeList xmlURL = xmlDoc.DocumentElement.SelectNodes("/root/post/file/url");
                XmlNodeList xmlTagsGeneral = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/general");
                XmlNodeList xmlTagsSpecies = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/species");
                XmlNodeList xmlTagsCharacter = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/character");
                XmlNodeList xmlTagsCopyright = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/copyright");
                XmlNodeList xmlTagsArtist = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/artist");
                XmlNodeList xmlTagsInvalid = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/invalid");
                XmlNodeList xmlTagsLore = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/lore");
                XmlNodeList xmlTagsMeta = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/meta");
                XmlNodeList xmlTagsLocked = xmlDoc.DocumentElement.SelectNodes("/root/post/locked_tags");
                XmlNodeList xmlScoreUp = xmlDoc.DocumentElement.SelectNodes("/root/post/score/up");
                XmlNodeList xmlScoreDown = xmlDoc.DocumentElement.SelectNodes("/root/post/score/down");
                XmlNodeList xmlScoreTotal = xmlDoc.DocumentElement.SelectNodes("/root/post/score/total");
                XmlNodeList xmlRating = xmlDoc.DocumentElement.SelectNodes("/root/post/rating");
                XmlNodeList xmlFavCount = xmlDoc.DocumentElement.SelectNodes("/root/post/fav_count");
                XmlNodeList xmlAuthor = xmlDoc.DocumentElement.SelectNodes("/root/post/uploader_id");
                XmlNodeList xmlDescription = xmlDoc.DocumentElement.SelectNodes("/root/post/description");
                XmlNodeList xmlDeleted = xmlDoc.DocumentElement.SelectNodes("/root/post/flags/deleted");

                if (xmlDeleted[0].InnerText.ToLower() == "true") {
                    DownloadHasErrored = true;
                    goto Finished;
                }

                string DownloadUrl = xmlURL[0].InnerText;
                if (DownloadUrl == null) {
                    DownloadUrl = apiTools.GetBlacklistedImageUrl(xmlMD5[0].InnerText, xmlExt[0].InnerText);
                    if (DownloadUrl == null) {
                        ErrorLog.ReportCustomException("DownloadUrl was still null after attempting to bypass blacklist with MD5", "ImageDownloader.cs");
                        DownloadHasErrored = true;
                        return;
                    }
                }

                List<string> PostTags = new List<string>();
                string ReadTags = string.Empty;

                ReadTags += "          General: [ ";
                switch (xmlTagsGeneral[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsGeneral[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsGeneral[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsGeneral[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }

                ReadTags += " ]\r\n          Species: [ ";
                switch (xmlTagsSpecies[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsSpecies[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsSpecies[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsSpecies[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }

                ReadTags += " ]\r\n          Character(s): [ ";
                switch (xmlTagsCharacter[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsCharacter[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsCharacter[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsCharacter[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }

                ReadTags += " ]\r\n          Copyright: [ ";
                switch (xmlTagsCopyright[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsCopyright[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsCopyright[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsCopyright[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }

                ReadTags += " ]\r\n          Artist: [ ";
                switch (xmlTagsArtist[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsArtist[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsArtist[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsArtist[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }

                ReadTags += " ]\r\n          Lore: [ ";
                switch (xmlTagsLore[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsLore[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsLore[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsLore[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }

                ReadTags += " ]\r\n          Meta: [ ";
                switch (xmlTagsMeta[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsMeta[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsMeta[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsMeta[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }

                ReadTags += " ]\r\n          Locked tags: [ ";
                switch (xmlTagsLocked[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsLocked[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsLocked[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsLocked[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',') + " ]";
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }

                ReadTags += " ]\r\n          Invalid: [ ";
                switch (xmlTagsInvalid[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsInvalid[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsInvalid[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsInvalid[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }
                ReadTags += " ]";

            // Get the rating.
                string rating = xmlRating[0].InnerText;
                switch (rating.ToLower()) {
                    case "e": case "explicit":
                        rating = "Explicit";
                        break;
                    case "q": case "questionable":
                        rating = "Questionable";
                        break;
                    case "s": case "safe":
                        rating = "Safe";
                        break;
                }

            // Set the description
                string imageDescription = " No description";
                if (xmlDescription[0].InnerText != "") {
                    imageDescription = "\n                \"" + xmlDescription[0].InnerText + "\"";
                }

            // Get the artists.
                string foundArtists = string.Empty;
                if (xmlTagsArtist[0].ChildNodes.Count > 0) {
                    for (int i = 0; i < xmlTagsArtist[0].ChildNodes.Count; i++) {
                        foundArtists += xmlTagsArtist[0].ChildNodes[i].InnerText + "\n               ";
                    }
                    foundArtists = foundArtists.TrimEnd(' ');
                    foundArtists = foundArtists.TrimEnd('\n');
                }
                else {
                    foundArtists = "(none)";
                }

            // Read blacklists if it's blacklisted.
                bool isGraylisted = false;
                bool isBlacklisted = false;
                string offendingTags = string.Empty;
                for (int i = 0; i < PostTags.Count; i++) {
                    if (DownloadInfo.Graylist.Length > 0) {
                        for (int j = 0; j < DownloadInfo.Graylist.Length; j++) {
                            if (PostTags[i] == DownloadInfo.Graylist[j]) {
                                offendingTags += PostTags[i];
                                isGraylisted = true;
                            }
                        }
                    }
                // Since the image downloader is user specific, the blacklist won't cancel downloading.
                // Image downloading is basically the user consenting to downloading it, therefore they want it despite the blacklist.
                // Worst-case, just delete it after downloading.
                    if (DownloadInfo.Blacklist.Length > 0) {
                        for (int j = 0; j < DownloadInfo.Blacklist.Length; j++) {
                            if (PostTags[i] == DownloadInfo.Blacklist[j]) {
                                offendingTags += PostTags[i];
                                isGraylisted = true;
                                isBlacklisted = true;
                            }
                        }
                    }
                }

            // set the .nfo buffer.
                if (isGraylisted || isBlacklisted) {
                    if (DownloadInfo.SeparateBlacklisted) {
                        blacklistInfo += "BLACKLISTED POST " + DownloadInfo.PostId + ":\n" +
                                         "    MD5: " + xmlMD5[0].InnerText + "\n" +
                                         "    URL: https://e621.net/posts/" + DownloadInfo.PostId + "\n" +
                                         "    ARTIST(S): " + foundArtists + "\n" +
                                         "    TAGS:\n" + ReadTags +//xmlTags[0].InnerText + "\n" +
                                         "    SCORE: Up " + xmlScoreUp[0].InnerText + ", Down " + xmlScoreDown[0].InnerText + ", Total" + xmlScoreTotal[0].InnerText + "\n" +
                                         "    RATING: " + rating + "\n" +
                                         "    OFFENDING TAGS: " + offendingTags + "\n" +
                                         "    DESCRIPITON:" + imageDescription +
                                         "\n\n";
                    }
                    else {
                        imageInfo += "BLACKLISTED POST " + DownloadInfo.PostId + ":\n" +
                                     "    MD5: " + xmlMD5[0].InnerText + "\n" +
                                     "    URL: https://e621.net/posts/" + DownloadInfo.PostId + "\n" +
                                     "    ARTIST(S): " + foundArtists + "\n" +
                                     "    TAGS:\n" + ReadTags +//xmlTags[0].InnerText + "\n" +
                                     "    SCORE: Up " + xmlScoreUp[0].InnerText + ", Down " + xmlScoreDown[0].InnerText + ", Total" + xmlScoreTotal[0].InnerText + "\n" +
                                     "    RATING: " + rating + "\n" +
                                     "    DESCRIPITON:" + imageDescription +
                                     "\n\n";
                    }
                }
                else {
                    imageInfo += "POST " + DownloadInfo.PostId + ":\n" +
                                 "    MD5: " + xmlMD5[0].InnerText + "\n" +
                                 "    URL: https://e621.net/posts/" + DownloadInfo.PostId + "\n" +
                                 "    ARTIST(S): " + foundArtists + "\n" +
                                 "    TAGS:\n" + ReadTags + //xmlTags[0].InnerText + "\n" +
                                 "    SCORE: Up " + xmlScoreUp[0].InnerText + ", Down " + xmlScoreDown[0].InnerText + ", Total" + xmlScoreTotal[0].InnerText + "\n" +
                                 "    RATING: " + rating + "\n" +
                                 "    DESCRIPITON:" + imageDescription +
                                 "\n\n";
                }

            // Trim the excess in the buffer.
                imageInfo = imageInfo.TrimEnd('\n');
                blacklistInfo = blacklistInfo.TrimEnd('\n');

            // Work on the filename
                string fileNameArtist = "(none)";
                bool useHardcodedFilter = false;
                if (string.IsNullOrEmpty(Config.Settings.General.undesiredTags)) {

                    useHardcodedFilter = true;
                }

                if (xmlTagsArtist.Count > 0) {
                    if (!string.IsNullOrEmpty(xmlTagsArtist[0].InnerText)) {
                        for (int i = 0; i < xmlTagsArtist.Count; i++) {
                            if (useHardcodedFilter) {
                                if (!UndesiredTags.isUndesiredHardcoded(xmlTagsArtist[i].InnerText)) {
                                    fileNameArtist = xmlTagsArtist[i].InnerText;
                                    break;
                                }
                            }
                            else {
                                if (!UndesiredTags.isUndesired(xmlTagsArtist[i].InnerText)) {
                                    fileNameArtist = xmlTagsArtist[i].InnerText;
                                    break;
                                }
                            }
                        }
                    }
                }

                DownloadInfo.FileName = DownloadInfo.FileNameSchema.Replace("%md5%", xmlMD5[0].InnerText)
                                .Replace("%id%", xmlID[0].InnerText)
                                .Replace("%rating%", rating.ToLower())
                                .Replace("%rating2%", xmlRating[0].InnerText)
                                .Replace("%artist%", fileNameArtist)
                                .Replace("%ext%", xmlExt[0].InnerText)
                                .Replace("%fav_count%", xmlFavCount[0].InnerText)
                                .Replace("%score%", xmlScoreTotal[0].InnerText)
                                .Replace("%scoreup%", xmlScoreUp[0].InnerText)
                                .Replace("%scoredown%", xmlScoreDown[0].InnerText)
                                .Replace("%author%", xmlAuthor[0].InnerText) + "." + xmlExt[0].InnerText;

            // Start working on the SaveTo string.
                if (DownloadInfo.SeparateRatings) {
                    switch (xmlRating[0].InnerText.ToLower()) {
                        case "e": case "explicit":
                            DownloadInfo.DownloadPath += "\\explicit";
                            break;
                        case "q": case "questionable":
                            DownloadInfo.DownloadPath += "\\questionable";
                            break;
                        case "s": case "safe":
                            DownloadInfo.DownloadPath += "\\safe";
                            break;
                    }
                }

                if (isGraylisted || isBlacklisted && DownloadInfo.SeparateBlacklisted)
                    DownloadInfo.DownloadPath += "\\blacklisted";
                if (DownloadInfo.SeparateArtists) {
                    DownloadInfo.DownloadPath += "\\" + fileNameArtist;
                }

                switch (DownloadInfo.SeparateNonImages) {
                    case true:
                        switch (xmlExt[0].InnerText) {
                            case "gif":
                                DownloadInfo.DownloadPath += "\\gif";
                                break;
                            case "apng":
                                DownloadInfo.DownloadPath += "\\apng";
                                break;
                            case "webm":
                                DownloadInfo.DownloadPath += "\\webm";
                                break;
                            case "swf":
                                DownloadInfo.DownloadPath += "\\swf";
                                break;
                        }
                        break;
                }

            // Create output directory.
                if (!Directory.Exists(DownloadInfo.DownloadPath)) {
                    Directory.CreateDirectory(DownloadInfo.DownloadPath);
                }

            // Check file before continuing.
                if (File.Exists(DownloadInfo.DownloadPath + "\\" + DownloadInfo.FileName)) {
                    DownloadHasFinished = true;
                    goto Finished;
                }

            // Save image.nfo.
                if (DownloadInfo.SaveInfo) {
                    if (isBlacklisted && DownloadInfo.SeparateBlacklisted) {
                        if (!File.Exists(DownloadInfo.DownloadPath + "\\images.blacklisted.nfo")) {
                            File.WriteAllText(DownloadInfo.DownloadPath + "\\images.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                        }
                        else {
                            blacklistInfo = "\n\n" + blacklistInfo;

                            string readInfo = File.ReadAllText(DownloadInfo.DownloadPath + "\\images.blacklisted.nfo");
                            if (!readInfo.Contains("MD5: " + xmlMD5[0].InnerText))
                                File.AppendAllText(DownloadInfo.DownloadPath + "\\images.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                        }
                    }
                    else {
                        if (!File.Exists(DownloadInfo.DownloadPath + "\\images.nfo")) {
                            File.WriteAllText(DownloadInfo.DownloadPath + "\\images.nfo", imageInfo, Encoding.UTF8);
                        }
                        else {
                            imageInfo = "\n\n" + imageInfo;

                            string readInfo = File.ReadAllText(DownloadInfo.DownloadPath + "\\images.nfo");
                            if (!readInfo.Contains("MD5: " + xmlMD5[0].InnerText))
                                File.AppendAllText(DownloadInfo.DownloadPath + "\\images.nfo", imageInfo, Encoding.UTF8);
                        }
                    }
                }

                this.BeginInvoke(new MethodInvoker(() => {
                    status.Text = "Downloading image...";
                    pbDownloadStatus.Value = 0;
                    pbDownloadStatus.Style = ProgressBarStyle.Blocks;
                    pbDownloadStatus.State = ProgressBarState.Normal;
                }));

            // Download file.
                DownloadInfo.ImageUrl = DownloadUrl;
                using (ExWebClient wc = new ExWebClient()) {
                    wc.DownloadProgressChanged += (s, e) => {
                        this.BeginInvoke(new MethodInvoker(() => {
                            pbDownloadStatus.Value = e.ProgressPercentage;
                            pbDownloadStatus.Value++;
                            pbDownloadStatus.Value--;
                            lbPercentage.Text = e.ProgressPercentage.ToString() + "%";
                        }));
                    };
                    wc.DownloadFileCompleted += (s, e) => {
                        lock (e.UserState) {
                            this.BeginInvoke(new MethodInvoker(() => {
                                pbDownloadStatus.Value = 100;
                                lbPercentage.Text = "Done";
                            }));
                            Monitor.Pulse(e.UserState);
                        }
                    };

                    wc.Proxy = WebRequest.GetSystemWebProxy();
                    wc.Headers.Add("User-Agent: " + Program.UserAgent);
                    wc.Method = "GET";
                    apiTools.SendDebugMessage("Beginning download of file " + DownloadUrl);

                    wc.DownloadFile(DownloadUrl, DownloadInfo.DownloadPath + "\\" + DownloadInfo.FileName);
                }

                DownloadHasFinished = true;
Finished:
                Debug.Print("Finished");
            }
            catch (ThreadAbortException) {
                apiTools.SendDebugMessage("Thread was requested to be, and has been, aborted. (frmImageDownloader.cs)");
                DownloadHasAborted = true;
            }
            catch (ObjectDisposedException) {
                apiTools.SendDebugMessage("An ObjectDisposedException occured. (frmImageDownloader.cs)");
            }
            catch (WebException WebE) {
                apiTools.SendDebugMessage("A WebException has occured. (frmImageDownloader.cs)");
                this.BeginInvoke(new MethodInvoker(() => {
                    status.Text = "A WebException has occured";
                    pbDownloadStatus.State = ProgressBarState.Error;
                }));
                ErrorLog.ReportWebException(WebE, DownloadInfo.ImageUrl, "frmImageDownloader.cs");
                DownloadHasErrored = true;
            }
            catch (Exception ex) {
                apiTools.SendDebugMessage("A gneral exception has occured. (frmImageDownloader.cs)");
                this.BeginInvoke(new MethodInvoker(() => {
                    status.Text = "A Exception has occured";
                    pbDownloadStatus.State = ProgressBarState.Error;
                }));
                ErrorLog.ReportException(ex, "frmImageDownloader.cs");
                DownloadHasErrored = true;
            }
            finally {
                this.BeginInvoke(new MethodInvoker(() => {
                    AfterDownload();
                }));
            }
        }
    #endregion

    }

    class ImageDownloader {

        #region Variables
        public ImageDownloadInfo DownloadInfo;
        #endregion

        public ImageDownloader(ImageDownloadInfo Info) {
            if (string.IsNullOrWhiteSpace(Info.FileNameSchema)) {
                Info.FileNameSchema = "%artist%_%md5%";
            }
            DownloadInfo = Info;
            switch (DownloadImage()) {
                case true:
                    if (DownloadInfo.OpenAfter) {
                        Process.Start("explorer.exe", "/select, \"" + DownloadInfo.FileName + "\"");
                    }
                    break;
            }
        }

        #region Downloader
        public bool DownloadImage() {
            try {
                // Set the saveto to \\Images.
                if (!DownloadInfo.DownloadPath.EndsWith("\\Images"))
                    DownloadInfo.DownloadPath += "\\Images";

                // Check the start URL to add the extra forward-slash for the split.
                if (!string.IsNullOrEmpty(DownloadInfo.ImageUrl)) {
                    if (DownloadInfo.ImageUrl.StartsWith("http://")) {
                        DownloadInfo.ImageUrl.Replace("http://", "https://");
                    }
                    if (!DownloadInfo.ImageUrl.StartsWith("https://")) {
                        DownloadInfo.ImageUrl = "https://" + DownloadInfo.ImageUrl;
                    }
                }

                // New varaibles for the API parse.
                string imageInfo = string.Empty;
                string blacklistInfo = string.Empty;
                XmlDocument xmlDoc = new XmlDocument();
                string postJson = string.Format("https://e621.net/posts/{0}.json", DownloadInfo.PostId);

                // Begin to get the XML
                DownloadInfo.ImageUrl = postJson;
                string postXML = apiTools.GetJsonToXml(postJson);

                // Check the XML.
                if (postXML == apiTools.EmptyXML || string.IsNullOrWhiteSpace(postXML)) {
                    apiTools.SendDebugMessage("Xml is empty, aborting.");
                    return false;
                }

                // Begin parsing XML.
                xmlDoc.LoadXml(postXML);
                apiTools.SendDebugMessage("Gathering post information from XML");
                XmlNodeList xmlID = xmlDoc.DocumentElement.SelectNodes("/root/post/id");
                XmlNodeList xmlMD5 = xmlDoc.DocumentElement.SelectNodes("/root/post/file/md5");
                XmlNodeList xmlExt = xmlDoc.DocumentElement.SelectNodes("/root/post/file/ext");
                XmlNodeList xmlURL = xmlDoc.DocumentElement.SelectNodes("/root/post/file/url");
                XmlNodeList xmlTagsGeneral = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/general");
                XmlNodeList xmlTagsSpecies = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/species");
                XmlNodeList xmlTagsCharacter = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/character");
                XmlNodeList xmlTagsCopyright = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/copyright");
                XmlNodeList xmlTagsArtist = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/artist");
                XmlNodeList xmlTagsInvalid = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/invalid");
                XmlNodeList xmlTagsLore = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/lore");
                XmlNodeList xmlTagsMeta = xmlDoc.DocumentElement.SelectNodes("/root/post/tags/meta");
                XmlNodeList xmlTagsLocked = xmlDoc.DocumentElement.SelectNodes("/root/post/locked_tags");
                XmlNodeList xmlScoreUp = xmlDoc.DocumentElement.SelectNodes("/root/post/score/up");
                XmlNodeList xmlScoreDown = xmlDoc.DocumentElement.SelectNodes("/root/post/score/down");
                XmlNodeList xmlScoreTotal = xmlDoc.DocumentElement.SelectNodes("/root/post/score/total");
                XmlNodeList xmlRating = xmlDoc.DocumentElement.SelectNodes("/root/post/rating");
                XmlNodeList xmlFavCount = xmlDoc.DocumentElement.SelectNodes("/root/post/fav_count");
                XmlNodeList xmlAuthor = xmlDoc.DocumentElement.SelectNodes("/root/post/uploader_id");
                XmlNodeList xmlDescription = xmlDoc.DocumentElement.SelectNodes("/root/post/description");
                XmlNodeList xmlDeleted = xmlDoc.DocumentElement.SelectNodes("/root/post/flags/deleted");

                if (xmlDeleted[0].InnerText.ToLower() == "true") {
                    //return false;
                }

                string DownloadUrl = xmlURL[0].InnerText;
                if (string.IsNullOrEmpty(DownloadUrl)) {
                    DownloadUrl = apiTools.GetBlacklistedImageUrl(xmlMD5[0].InnerText, xmlExt[0].InnerText);
                    if (DownloadUrl == null) {
                        ErrorLog.ReportCustomException("DownloadUrl was still null after attempting to bypass blacklist with MD5", "ImageDownloader.cs");
                        return false;
                    }
                }

                List<string> PostTags = new List<string>();
                string ReadTags = string.Empty;

                ReadTags += "          General: [ ";
                switch (xmlTagsGeneral[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsGeneral[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsGeneral[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsGeneral[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }

                ReadTags += " ]\r\n          Species: [ ";
                switch (xmlTagsSpecies[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsSpecies[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsSpecies[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsSpecies[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }

                ReadTags += " ]\r\n          Character(s): [ ";
                switch (xmlTagsCharacter[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsCharacter[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsCharacter[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsCharacter[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }

                ReadTags += " ]\r\n          Copyright: [ ";
                switch (xmlTagsCopyright[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsCopyright[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsCopyright[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsCopyright[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }

                ReadTags += " ]\r\n          Artist: [ ";
                switch (xmlTagsArtist[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsArtist[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsArtist[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsArtist[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }

                ReadTags += " ]\r\n          Lore: [ ";
                switch (xmlTagsLore[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsLore[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsLore[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsLore[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }

                ReadTags += " ]\r\n          Meta: [ ";
                switch (xmlTagsMeta[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsMeta[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsMeta[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsMeta[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }

                ReadTags += " ]\r\n          Locked tags: [ ";
                switch (xmlTagsLocked[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsLocked[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsLocked[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsLocked[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',') + " ]";
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }

                ReadTags += " ]\r\n          Invalid: [ ";
                switch (xmlTagsInvalid[0].ChildNodes.Count > 0) {
                    case true:
                        for (int x = 0; x < xmlTagsInvalid[0].ChildNodes.Count; x++) {
                            PostTags.Add(xmlTagsInvalid[0].ChildNodes[x].InnerText);
                            ReadTags += xmlTagsInvalid[0].ChildNodes[x].InnerText + ", ";
                        }
                        ReadTags = ReadTags.TrimEnd(' ').TrimEnd(',');
                        break;

                    case false:
                        ReadTags += "none";
                        break;
                }
                ReadTags += " ]";

                // Get the rating.
                string rating = xmlRating[0].InnerText;
                switch (rating.ToLower()) {
                    case "e":
                    case "explicit":
                        rating = "Explicit";
                        break;
                    case "q":
                    case "questionable":
                        rating = "Questionable";
                        break;
                    case "s":
                    case "safe":
                        rating = "Safe";
                        break;
                }

                // Set the description
                string imageDescription = " No description";
                if (xmlDescription[0].InnerText != "") {
                    imageDescription = "\n                \"" + xmlDescription[0].InnerText + "\"";
                }

                // Get the artists.
                string foundArtists = string.Empty;
                if (xmlTagsArtist[0].ChildNodes.Count > 0) {
                    for (int i = 0; i < xmlTagsArtist[0].ChildNodes.Count; i++) {
                        foundArtists += xmlTagsArtist[0].ChildNodes[i].InnerText + "\n               ";
                    }
                    foundArtists = foundArtists.TrimEnd(' ');
                    foundArtists = foundArtists.TrimEnd('\n');
                }
                else {
                    foundArtists = "(none)";
                }

                // Read blacklists if it's blacklisted.
                bool isGraylisted = false;
                bool isBlacklisted = false;
                string offendingTags = string.Empty;
                for (int i = 0; i < PostTags.Count; i++) {
                    if (DownloadInfo.Graylist.Length > 0) {
                        for (int j = 0; j < DownloadInfo.Graylist.Length; j++) {
                            if (PostTags[i] == DownloadInfo.Graylist[j]) {
                                offendingTags += PostTags[i];
                                isGraylisted = true;
                            }
                        }
                    }
                    // Since the image downloader is user specific, the blacklist won't cancel downloading.
                    // Image downloading is basically the user consenting to downloading it, therefore they want it despite the blacklist.
                    // Worst-case, just delete it after downloading.
                    if (DownloadInfo.Blacklist.Length > 0) {
                        for (int j = 0; j < DownloadInfo.Blacklist.Length; j++) {
                            if (PostTags[i] == DownloadInfo.Graylist[j]) {
                                offendingTags += PostTags[i];
                                isGraylisted = true;
                                isBlacklisted = true;
                            }
                        }
                    }
                }

                // set the .nfo buffer.
                if (isGraylisted || isBlacklisted) {
                    if (DownloadInfo.SeparateBlacklisted) {
                        blacklistInfo += "BLACKLISTED POST " + DownloadInfo.PostId + ":\n" +
                                         "    MD5: " + xmlMD5[0].InnerText + "\n" +
                                         "    URL: https://e621.net/posts/" + DownloadInfo.PostId + "\n" +
                                         "    ARTIST(S): " + foundArtists + "\n" +
                                         "    TAGS:\n" + ReadTags +//xmlTags[0].InnerText + "\n" +
                                         "    SCORE: Up " + xmlScoreUp[0].InnerText + ", Down " + xmlScoreDown[0].InnerText + ", Total" + xmlScoreTotal[0].InnerText + "\n" +
                                         "    RATING: " + rating + "\n" +
                                         "    OFFENDING TAGS: " + offendingTags + "\n" +
                                         "    DESCRIPITON:" + imageDescription +
                                         "\n\n";
                    }
                    else {
                        imageInfo += "BLACKLISTED POST " + DownloadInfo.PostId + ":\n" +
                                     "    MD5: " + xmlMD5[0].InnerText + "\n" +
                                     "    URL: https://e621.net/posts/" + DownloadInfo.PostId + "\n" +
                                     "    ARTIST(S): " + foundArtists + "\n" +
                                     "    TAGS:\n" + ReadTags +//xmlTags[0].InnerText + "\n" +
                                     "    SCORE: Up " + xmlScoreUp[0].InnerText + ", Down " + xmlScoreDown[0].InnerText + ", Total" + xmlScoreTotal[0].InnerText + "\n" +
                                     "    RATING: " + rating + "\n" +
                                     "    DESCRIPITON:" + imageDescription +
                                     "\n\n";
                    }
                }
                else {
                    imageInfo += "POST " + DownloadInfo.PostId + ":\n" +
                                 "    MD5: " + xmlMD5[0].InnerText + "\n" +
                                 "    URL: https://e621.net/posts/" + DownloadInfo.PostId + "\n" +
                                 "    ARTIST(S): " + foundArtists + "\n" +
                                 "    TAGS:\n" + ReadTags + //xmlTags[0].InnerText + "\n" +
                                 "    SCORE: Up " + xmlScoreUp[0].InnerText + ", Down " + xmlScoreDown[0].InnerText + ", Total" + xmlScoreTotal[0].InnerText + "\n" +
                                 "    RATING: " + rating + "\n" +
                                 "    DESCRIPITON:" + imageDescription +
                                 "\n\n";
                }

                // Trim the excess in the buffer.
                imageInfo = imageInfo.TrimEnd('\n');
                blacklistInfo = blacklistInfo.TrimEnd('\n');

                // Work on the filename
                string fileNameArtist = "unknown";
                bool useHardcodedFilter = false;
                if (string.IsNullOrEmpty(Config.Settings.General.undesiredTags)) {
                    useHardcodedFilter = true;
                }

                if (xmlTagsArtist.Count > 0) {
                    if (!string.IsNullOrEmpty(xmlTagsArtist[0].InnerText)) {
                        for (int i = 0; i < xmlTagsArtist.Count; i++) {
                            if (useHardcodedFilter) {
                                if (!UndesiredTags.isUndesiredHardcoded(xmlTagsArtist[i].InnerText)) {
                                    fileNameArtist = xmlTagsArtist[i].InnerText;
                                    break;
                                }
                            }
                            else {
                                if (!UndesiredTags.isUndesired(xmlTagsArtist[i].InnerText)) {
                                    fileNameArtist = xmlTagsArtist[i].InnerText;
                                    break;
                                }
                            }
                        }
                    }
                }

                DownloadInfo.FileName = DownloadInfo.FileNameSchema.Replace("%md5%", xmlMD5[0].InnerText)
                                .Replace("%id%", xmlID[0].InnerText)
                                .Replace("%rating%", rating.ToLower())
                                .Replace("%rating2%", xmlRating[0].InnerText)
                                .Replace("%artist%", fileNameArtist)
                                .Replace("%ext%", xmlExt[0].InnerText)
                                .Replace("%fav_count%", xmlFavCount[0].InnerText)
                                .Replace("%score%", xmlScoreTotal[0].InnerText)
                                .Replace("%scoreup%", xmlScoreUp[0].InnerText)
                                .Replace("%scoredown%", xmlScoreDown[0].InnerText)
                                .Replace("%author%", xmlAuthor[0].InnerText) + "." + xmlExt[0].InnerText;

                // Start working on the saveTo string.
                if (DownloadInfo.SeparateRatings) {
                    switch (xmlRating[0].InnerText.ToLower()) {
                        case "e":
                        case "explicit":
                            DownloadInfo.DownloadPath += "\\explicit";
                            break;
                        case "q":
                        case "questionable":
                            DownloadInfo.DownloadPath += "\\questionable";
                            break;
                        case "s":
                        case "safe":
                            DownloadInfo.DownloadPath += "\\safe";
                            break;
                    }
                }

                if (isGraylisted || isBlacklisted && DownloadInfo.SeparateBlacklisted)
                    DownloadInfo.DownloadPath += "\\blacklisted";
                if (DownloadInfo.SeparateArtists) {
                    DownloadInfo.DownloadPath += "\\" + fileNameArtist;
                }

                switch (DownloadInfo.SeparateNonImages) {
                    case true:
                        switch (xmlExt[0].InnerText) {
                            case "gif":
                                DownloadInfo.DownloadPath += "\\gif";
                                break;
                            case "apng":
                                DownloadInfo.DownloadPath += "\\apng";
                                break;
                            case "webm":
                                DownloadInfo.DownloadPath += "\\webm";
                                break;
                            case "swf":
                                DownloadInfo.DownloadPath += "\\swf";
                                break;
                        }
                        break;
                }

                // Create output directory.
                if (!Directory.Exists(DownloadInfo.DownloadPath)) {
                    Directory.CreateDirectory(DownloadInfo.DownloadPath);
                }

                // Check file before continuing.
                if (File.Exists(DownloadInfo.DownloadPath + "\\" + DownloadInfo.FileName)) {
                    return true;
                }

                // Save image.nfo.
                if (DownloadInfo.SaveInfo) {
                    if (isGraylisted && DownloadInfo.SeparateBlacklisted) {
                        if (!File.Exists(DownloadInfo.DownloadPath + "\\images.blacklisted.nfo")) {
                            File.WriteAllText(DownloadInfo.DownloadPath + "\\images.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                        }
                        else {
                            blacklistInfo = "\n\n" + blacklistInfo;

                            string readInfo = File.ReadAllText(DownloadInfo.DownloadPath + "\\images.blacklisted.nfo");
                            if (!readInfo.Contains("MD5: " + xmlMD5[0].InnerText))
                                File.AppendAllText(DownloadInfo.DownloadPath + "\\images.blacklisted.nfo", blacklistInfo, Encoding.UTF8);
                        }
                    }
                    else {
                        if (!File.Exists(DownloadInfo.DownloadPath + "\\images.nfo")) {
                            File.WriteAllText(DownloadInfo.DownloadPath + "\\images.nfo", imageInfo, Encoding.UTF8);
                        }
                        else {
                            imageInfo = "\n\n" + imageInfo;

                            string readInfo = File.ReadAllText(DownloadInfo.DownloadPath + "\\images.nfo");
                            if (!readInfo.Contains("MD5: " + xmlMD5[0].InnerText))
                                File.AppendAllText(DownloadInfo.DownloadPath + "\\images.nfo", imageInfo, Encoding.UTF8);
                        }
                    }
                }

                // Download file.
                DownloadInfo.ImageUrl = DownloadUrl;
                apiTools.DownloadImage(DownloadUrl, DownloadInfo.DownloadPath + "\\" + DownloadInfo.FileName);
                return true;
            }
            catch (ThreadAbortException) {
                apiTools.SendDebugMessage("Thread was requested to be, and has been, aborted. (ImageDownloader.cs)");
                return false;
            }
            catch (ObjectDisposedException) {
                apiTools.SendDebugMessage("An ObjectDiposedException occured. (ImageDownloader.cs)");
                return false;
            }
            catch (WebException WebE) {
                apiTools.SendDebugMessage("A WebException has occured. (ImageDownloader.cs)");
                ErrorLog.ReportWebException(WebE, DownloadInfo.ImageUrl, "ImageDownloader.cs");
                return false;
            }
            catch (Exception ex) {
                apiTools.SendDebugMessage("A gneral exception has occured. (ImageDownloader.cs)");
                ErrorLog.ReportException(ex, "ImageDownloader.cs");
                return false;
            }
        }
        #endregion
    }
}
