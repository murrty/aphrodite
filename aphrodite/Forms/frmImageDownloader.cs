﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace aphrodite {

    public class ImageInfo {
        public string FileUrl;
        public string InfoBuffer;
        public bool DownloadFinish = false;
        public bool DownloadError = false;
        public string DownloadPath;

        public ImageInfo(ImageDownloadInfo Info) {
            try {
            // Set the SaveTo to \\Images.
                if (!Info.DownloadPath.EndsWith("\\Images")) {
                    Info.DownloadPath += "\\Images";
                }

            // New varaibles for the API parse.
                string imageInfo = string.Empty;
                string blacklistInfo = string.Empty;
                string url = string.Format("https://e621.net/posts/{0}.json", Info.PostId);
                DownloadPath = Info.DownloadPath;

            // Begin to get the XML
                string postXML = apiTools.GetJsonToXml(url);

            // Check the XML.
                if (postXML == apiTools.EmptyXML || string.IsNullOrWhiteSpace(postXML)) {
                    DownloadError = true;
                    return;
                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(postXML);

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

                switch (xmlDeleted[0].InnerText.ToLower() == "true") {
                    case true:
                        DownloadError = true;
                        break;

                    case false:
                        FileUrl = xmlURL[0].InnerText;
                        if (FileUrl == null) {
                            FileUrl = apiTools.GetBlacklistedImageUrl(xmlMD5[0].InnerText, xmlExt[0].InnerText);
                            if (FileUrl == null) {
                                ErrorLog.ReportCustomException("DownloadUrl was still null after attempting to bypass blacklist with MD5", "ImageDownloader.cs");
                                DownloadError = true;
                                return;
                            }
                        }

                        List<string> PostTags = new List<string>();
                        string InfoTags = string.Empty;

                        InfoTags += "          General: [ ";
                        switch (xmlTagsGeneral[0].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsGeneral[0].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsGeneral[0].ChildNodes[x].InnerText);
                                    InfoTags += xmlTagsGeneral[0].ChildNodes[x].InnerText + ", ";
                                }
                                InfoTags = InfoTags.TrimEnd(' ').TrimEnd(',');
                                break;

                            case false:
                                InfoTags += "none";
                                break;
                        }

                        InfoTags += " ]\r\n          Species: [ ";
                        switch (xmlTagsSpecies[0].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsSpecies[0].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsSpecies[0].ChildNodes[x].InnerText);
                                    InfoTags += xmlTagsSpecies[0].ChildNodes[x].InnerText + ", ";
                                }
                                InfoTags = InfoTags.TrimEnd(' ').TrimEnd(',');
                                break;

                            case false:
                                InfoTags += "none";
                                break;
                        }

                        InfoTags += " ]\r\n          Character(s): [ ";
                        switch (xmlTagsCharacter[0].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsCharacter[0].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsCharacter[0].ChildNodes[x].InnerText);
                                    InfoTags += xmlTagsCharacter[0].ChildNodes[x].InnerText + ", ";
                                }
                                InfoTags = InfoTags.TrimEnd(' ').TrimEnd(',');
                                break;

                            case false:
                                InfoTags += "none";
                                break;
                        }

                        InfoTags += " ]\r\n          Copyright: [ ";
                        switch (xmlTagsCopyright[0].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsCopyright[0].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsCopyright[0].ChildNodes[x].InnerText);
                                    InfoTags += xmlTagsCopyright[0].ChildNodes[x].InnerText + ", ";
                                }
                                InfoTags = InfoTags.TrimEnd(' ').TrimEnd(',');
                                break;

                            case false:
                                InfoTags += "none";
                                break;
                        }

                        InfoTags += " ]\r\n          Artist: [ ";
                        switch (xmlTagsArtist[0].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsArtist[0].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsArtist[0].ChildNodes[x].InnerText);
                                    InfoTags += xmlTagsArtist[0].ChildNodes[x].InnerText + ", ";
                                }
                                InfoTags = InfoTags.TrimEnd(' ').TrimEnd(',');
                                break;

                            case false:
                                InfoTags += "none";
                                break;
                        }

                        InfoTags += " ]\r\n          Lore: [ ";
                        switch (xmlTagsLore[0].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsLore[0].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsLore[0].ChildNodes[x].InnerText);
                                    InfoTags += xmlTagsLore[0].ChildNodes[x].InnerText + ", ";
                                }
                                InfoTags = InfoTags.TrimEnd(' ').TrimEnd(',');
                                break;

                            case false:
                                InfoTags += "none";
                                break;
                        }

                        InfoTags += " ]\r\n          Meta: [ ";
                        switch (xmlTagsMeta[0].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsMeta[0].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsMeta[0].ChildNodes[x].InnerText);
                                    InfoTags += xmlTagsMeta[0].ChildNodes[x].InnerText + ", ";
                                }
                                InfoTags = InfoTags.TrimEnd(' ').TrimEnd(',');
                                break;

                            case false:
                                InfoTags += "none";
                                break;
                        }

                        InfoTags += " ]\r\n          Locked tags: [ ";
                        switch (xmlTagsLocked[0].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsLocked[0].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsLocked[0].ChildNodes[x].InnerText);
                                    InfoTags += xmlTagsLocked[0].ChildNodes[x].InnerText + ", ";
                                }
                                InfoTags = InfoTags.TrimEnd(' ').TrimEnd(',') + " ]";
                                break;

                            case false:
                                InfoTags += "none";
                                break;
                        }

                        InfoTags += " ]\r\n          Invalid: [ ";
                        switch (xmlTagsInvalid[0].ChildNodes.Count > 0) {
                            case true:
                                for (int x = 0; x < xmlTagsInvalid[0].ChildNodes.Count; x++) {
                                    PostTags.Add(xmlTagsInvalid[0].ChildNodes[x].InnerText);
                                    InfoTags += xmlTagsInvalid[0].ChildNodes[x].InnerText + ", ";
                                }
                                InfoTags = InfoTags.TrimEnd(' ').TrimEnd(',');
                                break;

                            case false:
                                InfoTags += "none";
                                break;
                        }
                        InfoTags += " ]";

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

                    // Read blacklists if it's blacklisted.
                        bool isGraylisted = false;
                        bool isBlacklisted = false;
                        string offendingTags = string.Empty;
                        for (int i = 0; i < PostTags.Count; i++) {
                            if (Info.Graylist.Length > 0) {
                                for (int j = 0; j < Info.Graylist.Length; j++) {
                                    if (PostTags[i] == Info.Graylist[j]) {
                                        offendingTags += PostTags[i];
                                        isGraylisted = true;
                                    }
                                }
                            }
                        // Since the image downloader is user specific, the blacklist won't cancel downloading.
                        // Image downloading is basically the user consenting to downloading it, therefore they want it despite the blacklist.
                        // Worst-case, just delete it after downloading.
                            if (Info.Blacklist.Length > 0) {
                                for (int j = 0; j < Info.Blacklist.Length; j++) {
                                    if (PostTags[i] == Info.Blacklist[j]) {
                                        offendingTags += PostTags[i];
                                        isBlacklisted = true;
                                    }
                                }
                            }
                        }

                    // set the .nfo buffer.
                        InfoBuffer += "POST " + Info.PostId + ":\r\n" +
                            "    MD5: " + xmlMD5[0].InnerText + "\r\n" +
                            "    URL: https://e621.net/posts/" + Info.PostId + "\r\n" +
                            "    TAGS:\r\n" + InfoTags +
                            "    SCORE: Up " + xmlScoreUp[0].InnerText + ", Down " + xmlScoreDown[0].InnerText + ", Total" + xmlScoreTotal[0].InnerText + "\r\n" +
                            "    RATING: " + rating + "\r\n";

                        if (isBlacklisted) {
                            InfoBuffer = "BLACKLISTED " + InfoBuffer +
                            "    OFFENDING TAGS: " + offendingTags + "\r\n";
                        }
                        else if (isGraylisted) {
                            InfoBuffer = "GRAYLISTED " + InfoBuffer +
                            "    OFFENDING TAGS: " + offendingTags + "\r\n";
                        }

                        InfoBuffer += "    DESCRIPITON:" + imageDescription;

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

                        Info.FileName = Info.FileNameSchema.Replace("%md5%", xmlMD5[0].InnerText)
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
                        if (Info.SeparateRatings) {
                            switch (xmlRating[0].InnerText.ToLower()) {
                                case "e": case "explicit":
                                    DownloadPath += "\\explicit";
                                    break;
                                case "q": case "questionable":
                                    DownloadPath += "\\questionable";
                                    break;
                                case "s": case "safe":
                                    DownloadPath += "\\safe";
                                    break;
                            }
                        }

                        if (isBlacklisted && Info.SeparateBlacklisted) {
                            DownloadPath += "\\blacklisted";
                        }
                        else if (isGraylisted && Info.SeparateGraylisted) {
                            DownloadPath += "\\graylisted";
                        }
                        if (Info.SeparateArtists) {
                            DownloadPath += "\\" + fileNameArtist;
                        }

                        switch (Info.SeparateNonImages) {
                            case true:
                                switch (xmlExt[0].InnerText) {
                                    case "gif":
                                        DownloadPath += "\\gif";
                                        break;
                                    case "apng":
                                        DownloadPath += "\\apng";
                                        break;
                                    case "webm":
                                        DownloadPath += "\\webm";
                                        break;
                                    case "swf":
                                        DownloadPath += "\\swf";
                                        break;
                                }
                                break;
                        }


                    // Create output directory.
                        if (!Directory.Exists(DownloadPath)) {
                            Directory.CreateDirectory(DownloadPath);
                        }

                    // Set the path to the full path with file.
                        DownloadPath += "\\" + Info.FileName;

                    // Check file before continuing.
                        if (File.Exists(DownloadPath)) {
                            DownloadFinish = true;
                            return;
                        }

                    // Save image.nfo.
                        if (Info.SaveInfo) {
                            if (isBlacklisted) {
                                if (File.Exists(Info.DownloadPath + "\\images.blacklisted.nfo")) {
                                    InfoBuffer = "\n\n" + InfoBuffer;
                                    string readInfo = File.ReadAllText(Info.DownloadPath + "\\images.blacklisted.nfo");
                                    if (!readInfo.Contains("MD5: " + xmlMD5[0].InnerText)) {
                                        File.AppendAllText(Info.DownloadPath + "\\images.blacklisted.nfo", InfoBuffer, Encoding.UTF8);
                                    }
                                }
                                else {
                                    File.WriteAllText(Info.DownloadPath + "\\images.blacklisted.nfo", InfoBuffer, Encoding.UTF8);
                                }
                            }
                            else if (isGraylisted) {
                                if (File.Exists(Info.DownloadPath + "\\images.graylisted.nfo")) {
                                    InfoBuffer = "\n\n" + InfoBuffer;
                                    string readInfo = File.ReadAllText(Info.DownloadPath + "\\images.graylisted.nfo");
                                    if (!readInfo.Contains("MD5: " + xmlMD5[0].InnerText)) {
                                        File.AppendAllText(Info.DownloadPath + "\\images.graylisted.nfo", InfoBuffer, Encoding.UTF8);
                                    }
                                }
                                else {
                                    File.WriteAllText(Info.DownloadPath + "\\images.blacklisted.nfo", InfoBuffer, Encoding.UTF8);
                                }
                            }
                            else {
                                if (File.Exists(Info.DownloadPath + "\\images.nfo")) {
                                    InfoBuffer = "\n\n" + InfoBuffer;
                                    string readInfo = File.ReadAllText(Info.DownloadPath + "\\images.nfo");
                                    if (!readInfo.Contains("MD5: " + xmlMD5[0].InnerText)) {
                                        File.AppendAllText(Info.DownloadPath + "\\images.nfo", InfoBuffer, Encoding.UTF8);
                                    }
                                }
                                else {
                                    File.WriteAllText(Info.DownloadPath + "\\images.nfo", InfoBuffer, Encoding.UTF8);
                                }
                            }
                        }

                        break;
                }
            }
            catch {
                throw;
            }
        }
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
                        Process.Start("explorer.exe", "/select, \"" + DownloadInfo.DownloadPath + "\\" + DownloadInfo.FileName + "\"");
                    }
                    break;
            }
        }

        #region Downloader
        public bool DownloadImage() {
            try {

                ImageInfo Image = new ImageInfo(DownloadInfo);

                if (Image.DownloadFinish) {
                    return true;
                }
                if (Image.DownloadError) {
                    return false;
                }

                // Download file.
                using (Controls.ExtendedWebClient wc = new Controls.ExtendedWebClient()) {
                    wc.Proxy = WebRequest.GetSystemWebProxy();
                    wc.Headers.Add("User-Agent: " + Program.UserAgent);
                    wc.Method = "GET";

                    wc.DownloadFile(Image.FileUrl, Image.DownloadPath);
                }
                return true;
            }
            catch (ThreadAbortException) {
                return false;
            }
            catch (ObjectDisposedException) {
                return false;
            }
            catch (WebException WebE) {
                ErrorLog.ReportWebException(WebE, DownloadInfo.ImageUrl, "ImageDownloader.cs");
                return false;
            }
            catch (Exception ex) {
                ErrorLog.ReportException(ex, "ImageDownloader.cs");
                return false;
            }
        }
        #endregion
    }

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
            tmrTitle.Stop();
            if (DownloadInfo.OpenAfter) {
                Process.Start("explorer.exe", "/select, \"" + DownloadInfo.DownloadPath + "\\" + DownloadInfo.FileName + "\"");
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
            else {
                if (DownloadHasFinished) {
                    lbInfo.Text = "The image has been downloaded";
                    pbDownloadStatus.Value = pbDownloadStatus.Maximum;
                    lbPercentage.Text = "Done";
                    this.Text = "Image " + DownloadInfo.PostId + " finished downloading";
                    status.Text = "Finished downloading pool";
                }
                else if (DownloadHasErrored) {
                    lbInfo.Text = "Downloading has encountered an error";
                    pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                    lbPercentage.Text = "Error";
                    this.Text = "Download error";
                    status.Text = "Downloading has resulted in an error";
                }
                else if (DownloadHasAborted) {
                    lbInfo.Text = "Download canceled";
                    pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                    lbPercentage.Text = "Canceled";
                    this.Text = "Download canceled";
                    status.Text = "Download has canceled";
                }
                else {
                    // assume it completed
                    lbInfo.Text = "Download assumed to be completed...";
                    pbDownloadStatus.Value = pbDownloadStatus.Maximum;
                    lbPercentage.Text = "Done?";
                    this.Text = "Image " + DownloadInfo.PostId + " finished downloading";
                    status.Text = "Download status booleans not set, assuming the download completed";
                }
            }
        }
        public void downloadImage() {
            try {
                this.BeginInvoke(new MethodInvoker(() => {
                    lbInfo.Text = "Downloading image id " + DownloadInfo.PostId;
                    status.Text = "Waiting for JSON parse...";
                }));

                ImageInfo Image = new ImageInfo(DownloadInfo);

                if (Image.DownloadFinish) {
                    DownloadHasFinished = true;
                }
                else {
                    this.BeginInvoke(new MethodInvoker(() => {
                        status.Text = "Downloading image...";
                        pbDownloadStatus.Value = 0;
                        pbDownloadStatus.Style = ProgressBarStyle.Blocks;
                        pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Normal;
                    }));

                // Download file.
                    using (Controls.ExtendedWebClient wc = new Controls.ExtendedWebClient()) {
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

                        wc.DownloadFile(Image.FileUrl, Image.DownloadPath);
                    }

                    DownloadHasFinished = true;
                }

                Debug.Print("Finished");
            }
            catch (ThreadAbortException) {
                DownloadHasAborted = true;
            }
            catch (ObjectDisposedException) {
            }
            catch (WebException WebE) {
                this.BeginInvoke(new MethodInvoker(() => {
                    status.Text = "A WebException has occured";
                    pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
                }));
                ErrorLog.ReportWebException(WebE, DownloadInfo.ImageUrl, "frmImageDownloader.cs");
                DownloadHasErrored = true;
            }
            catch (Exception ex) {
                this.BeginInvoke(new MethodInvoker(() => {
                    status.Text = "A Exception has occured";
                    pbDownloadStatus.State = aphrodite.Controls.ProgressBarState.Error;
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

}