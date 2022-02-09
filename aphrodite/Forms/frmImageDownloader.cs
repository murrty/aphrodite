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

    public partial class frmImageDownloader : ExtendedForm {

        #region Fields
        /// <summary>
        /// The constant string of the post base used for parsing the api.
        /// </summary>
        private const string ApiImageBase = "https://e621.net/posts/{0}.json";

        /// <summary>
        /// The information about the download of this current instance.
        /// </summary>
        public readonly ImageDownloadInfo DownloadInfo;

        /// <summary>
        /// Whether the hardcoded artist filter should be used.
        /// </summary>
        private bool UseHardcodedFilter = false;
        #endregion

        public frmImageDownloader(ImageDownloadInfo NewInfo) {
            InitializeComponent();

            if (Config.ValidPoint(Config.Settings.FormSettings.frmImageDownloader_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmImageDownloader_Location;
            }

            DownloadInfo = NewInfo;

            if (NewInfo.UseForm) {
                this.Opacity = 100;
                this.ShowIcon = true;
                this.ShowInTaskbar = true;
            }
            else {
                this.Opacity = 0;
                this.ShowIcon = false;
                this.ShowInTaskbar = false;
            }
        }

        private void frmImageDownloaderUpdated_FormClosing(Object sender, FormClosingEventArgs e) {
            if (DownloadInfo.UseForm) {
                Config.Settings.FormSettings.frmImageDownloader_Location = this.Location;
            }
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
                }
                break;

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
                            this.DownloadAbort = true;
                            Status = DownloadStatus.Aborted;
                            if (DownloadThread != null && DownloadThread.IsAlive) {
                                DownloadThread.Abort();
                            }
                        }
                        break;

                        default: {
                            e.Cancel = !DownloadInfo.IgnoreFinish;
                        }
                        break;
                    }
                }
                break;
            }
        }

        protected override void PrepareDownload() {
            txtPostId.Text = "null";
            if (DownloadInfo == null) {
                this.Text = "Unable to download image";
                pbDownloadStatus.Text = "Download info is null";
                sbStatus.Text = "Info null";
            }
            else if (string.IsNullOrWhiteSpace(DownloadInfo.PostId)) {
                this.Text = "Unable to download image";
                pbDownloadStatus.Text = "No image id was specified";
                sbStatus.Text = "No post ID";
            }
            else if (Downloader.ImagesBeingDownloaded.Contains(DownloadInfo.PostId)) {
                txtPostId.Text = DownloadInfo.PostId;
                Status = DownloadStatus.AlreadyBeingDownloaded;
                this.Text = "Unable to download image";
                pbDownloadStatus.Text = "Image already being downloaded";
                sbStatus.Text = "Already in queue";
            }
            else {
                if (string.IsNullOrWhiteSpace(DownloadInfo.FileNameSchema)) {
                    DownloadInfo.FileNameSchema = "%artist%_%id%";
                }

                if (!DownloadInfo.DownloadPath.EndsWith("\\Images")) {
                    DownloadInfo.DownloadPath += "\\Images";
                }

                UseHardcodedFilter = DownloadInfo.UndesiredTags.Length > 0;

                txtPostId.Text = DownloadInfo.PostId;

                base.PrepareDownload();
            }
        }

        protected override void StartDownload() {
            DownloadThread = new(async () => {
                Log.Write($"Starting Image download for \"{DownloadInfo.PostId}\"");

                #region Try block
                try {

                    #region Defining new variables
                    byte[] ApiData;                             // The data byte-array the API returns. It's a byte array so it's faster to translate to xml.
                    string CurrentXml = string.Empty;           // The string of the current XML to be loaded into a XmlDocument;
                    string ImageUrl = string.Empty;             // The string of the image url.
                    string ImageFileName = string.Empty;        // The string of the file name to display to the user.
                    string ImageFilePath = string.Empty;        // The string of the full-path of the image to be download to.
                    string ImageInfo = string.Empty;            // The buffer for the images.nfo file.
                    bool ImageIsGraylisted = false;             // Whether the image is graylisted.
                    bool ImageIsBlacklisted = false;            // Whether the image is blacklisted.
                    bool ShouldRetry = true;                    // Whether retry-able exception catches should retry the action.
                    string FoundGraylistedTags = string.Empty;  // The found graylisted tags.
                    string FoundBlacklistedTags = string.Empty; // The found blacklisted tags.
                    string ImageRating = string.Empty;          // The rating of the image.
                    string ImageDescription = string.Empty;     // The description of the image.
                    List<string> PostTags = new();              // The list of the tags on the image.
                    string ReadTags = string.Empty;             // The read tags of the image.
                    #endregion

                    #region Parse the api for image info
                    Status = DownloadStatus.Parsing;

                    #region image api download + xml elements
                    this.Invoke((Action)delegate () {
                        sbStatus.Text = $"Downloading api data for image...";
                    });

                    CurrentUrl = string.Format(ApiImageBase, DownloadInfo.PostId);

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

                                if (ApiTools.IsXmlDead(CurrentXml)) {
                                    DownloadError = true;
                                    Status = DownloadStatus.ApiReturnedNullOrEmpty;
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
                                    Status = DownloadStatus.Errored;
                                    DownloadError = true;
                                    ShouldRetry = false;
                                }
                            }
                        }
                    } while (ShouldRetry && !DownloadAbort);

                    if (DownloadError || DownloadAbort) {
                        return;
                    }

                    xmlDoc = new();
                    xmlDoc.LoadXml(CurrentXml);
                    CurrentXml = string.Empty;

                    XmlNodeList xmlID = xmlDoc.DocumentElement.SelectNodes("/root/post/id");
                    XmlNodeList xmlMD5 = xmlDoc.DocumentElement.SelectNodes("/root/post/file/md5");
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
                    XmlNodeList xmlScore = xmlDoc.DocumentElement.SelectNodes("/root/post/score/total");
                    XmlNodeList xmlScoreUp = xmlDoc.DocumentElement.SelectNodes("/root/post/score/up");
                    XmlNodeList xmlScoreDown = xmlDoc.DocumentElement.SelectNodes("/root/post/score/down");
                    XmlNodeList xmlFavCount = xmlDoc.DocumentElement.SelectNodes("/root/post/fav_count");
                    XmlNodeList xmlRating = xmlDoc.DocumentElement.SelectNodes("/root/post/rating");
                    XmlNodeList xmlAuthor = xmlDoc.DocumentElement.SelectNodes("/root/post/uploader_id");
                    XmlNodeList xmlDescription = xmlDoc.DocumentElement.SelectNodes("/root/post/description");
                    XmlNodeList xmlExt = xmlDoc.DocumentElement.SelectNodes("/root/post/file/ext");
                    XmlNodeList xmlDeleted = xmlDoc.DocumentElement.SelectNodes("/root/post/flags/deleted");

                    this.Invoke((Action)delegate () {
                        sbStatus.Text = $"Parsing api data...";
                    });
                    #endregion

                    #region Check if it was deleted first
                    if (xmlDeleted[0].InnerText.ToLower() == "true") {
                        throw new PoolOrPostWasDeletedException("Image was deleted");
                    }
                    #endregion

                    #region bypass global blacklist
                    string NewFileUrl = xmlURL[0].InnerText;
                    if (string.IsNullOrEmpty(NewFileUrl)) {
                        if (xmlDeleted[0].InnerText.ToLower() == "false") {
                            NewFileUrl = ApiTools.GetBlacklistedImageUrl(xmlMD5[0].InnerText, xmlExt[0].InnerText);
                            if (NewFileUrl == null) {
                                throw new ImageWasNullAfterBypassingException($"{DownloadInfo.PostId} was still null.");
                            }
                        }
                    }
                    #endregion

                    #region tags + graylist/blacklist checks
                    ReadTags += "\r\n          General: [ ";
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

                    for (int j = 0; j < PostTags.Count; j++) {
                        if (DownloadInfo.Graylist.Length > 0) {
                            for (int k = 0; k < DownloadInfo.Graylist.Length; k++) {
                                if (PostTags[j] == DownloadInfo.Graylist[k]) {
                                    FoundGraylistedTags += " " + DownloadInfo.Graylist[k];
                                    ImageIsGraylisted = true;
                                }
                            }
                        }

                        if (DownloadInfo.Blacklist.Length > 0) {
                            for (int k = 0; k < DownloadInfo.Blacklist.Length; k++) {
                                if (PostTags[j] == DownloadInfo.Blacklist[k]) {
                                    FoundBlacklistedTags += DownloadInfo.Blacklist[k] + ", ";
                                    ImageIsBlacklisted = true;
                                }
                            }
                        }
                    }
                    FoundBlacklistedTags = "[ " + FoundBlacklistedTags.Trim(' ').TrimEnd(',') + " ]";
                    #endregion

                    #region File name fiddling
                    ImageRating = xmlRating[0].InnerText.ToLower() switch {
                        "e" or "explicit" => "Explicit",
                        "q" or "questionable" => "Questionable",
                        "s" or "safe" => "Safe",
                        _ => "Unknown"
                    };

                    // File name artist for the schema
                    string FileNameArtist = "(none)";
                    if (xmlTagsArtist[0].ChildNodes.Count > 0) {
                        for (int CurrentArtistTag = 0; CurrentArtistTag < xmlTagsArtist[0].ChildNodes.Count; CurrentArtistTag++) {
                            if (UseHardcodedFilter) {
                                if (!UndesiredTags.IsUndesiredHardcoded(xmlTagsArtist[0].ChildNodes[CurrentArtistTag].InnerText)) {
                                    FileNameArtist = xmlTagsArtist[0].ChildNodes[CurrentArtistTag].InnerText;
                                    break;
                                }
                            }
                            else {
                                if (!UndesiredTags.IsUndesired(xmlTagsArtist[0].ChildNodes[CurrentArtistTag].InnerText, DownloadInfo.UndesiredTags)) {
                                    FileNameArtist = xmlTagsArtist[0].ChildNodes[CurrentArtistTag].InnerText;
                                    break;
                                }
                            }
                        }
                    }

                    string NewFileName =
                        DownloadInfo.FileNameSchema
                            .Replace("%md5%", xmlMD5[0].InnerText)
                            .Replace("%id%", xmlID[0].InnerText)
                            .Replace("%rating%", ImageRating.ToLower())
                            .Replace("%rating2%", xmlRating[0].InnerText)
                            .Replace("%artist%", FileNameArtist)
                            .Replace("%ext%", xmlExt[0].InnerText)
                            .Replace("%fav_count%", xmlFavCount[0].InnerText)
                            .Replace("%score%", xmlScore[0].InnerText)
                            .Replace("%scoreup%", xmlScoreUp[0].InnerText)
                            .Replace("%scoredown%", xmlScoreDown[0].InnerText)
                            .Replace("%author%", xmlAuthor[0].InnerText) + "." + xmlExt[0].InnerText;
                    #endregion

                    #region nfo buffer
                    if (DownloadInfo.SaveInfo) {
                        if (!string.IsNullOrWhiteSpace(xmlDescription[0].InnerText)) {
                            ImageDescription = "\n                \"" + xmlDescription[0].InnerText + "\"";
                        }
                        else {
                            ImageDescription = "[no description]";
                        }

                        ImageInfo =
                            $"{(ImageIsBlacklisted ? "BLACKLISTED " : (ImageIsGraylisted ? " GRAYLISTED " : ""))}IMAGE {DownloadInfo.PostId}\n" +
                            $"    MD5: {xmlMD5[0].InnerText}\n" +
                            $"    URL: https://e621.net/posts/{xmlID[0].InnerText}\n" +
                            $"{(ImageIsBlacklisted ? $"    OFFENDING TAGS: {FoundBlacklistedTags.Trim() + (ImageIsGraylisted ? " |<~Bl|Gr~>| " + FoundGraylistedTags.Trim() : "")}\n" : (ImageIsGraylisted ? $"    OFFENDING TAGS: {FoundGraylistedTags.Trim()}\n" : ""))}" +
                            $"    TAGS: {ReadTags}\n" +
                            $"    SCORE: Up {xmlScoreUp[0].InnerText}, Down {xmlScoreDown[0].InnerText}, Total {xmlScore[0].InnerText}\n" +
                            $"    RATING: {ImageRating}\n" +
                            $"    DESCRIPTION: {ImageDescription}";
                    }
                    #endregion

                    #region Image path, link, and name
                    string NewPath = DownloadInfo.DownloadPath;

                    if (DownloadInfo.SeparateRatings) {
                        NewPath += xmlRating[0].InnerText.ToLower() switch {
                            "e" or "explicit" => "\\explicit",
                            "q" or "questionable" => "\\questionable",
                            "s" or "safe" => "\\safe",
                            _ => "\\unknown"
                        };
                    }

                    if (ImageIsBlacklisted && DownloadInfo.SeparateBlacklisted) {
                        NewPath += "\\blacklisted";
                    }
                    else if (ImageIsGraylisted && DownloadInfo.SeparateGraylisted) {
                        NewPath += "\\graylisted";
                    }

                    if (DownloadInfo.SeparateNonImages) {
                        NewPath += xmlExt[0].InnerText.ToLower() switch {
                            "gif" => "\\gif",
                            "apng" => "\\apng",
                            "webm" => "\\webm",
                            "swf" => "\\swf",
                            _ => ""
                        };
                    }

                    if (File.Exists(NewPath + "\\" + NewFileName)) {
                        Status = DownloadStatus.FileAlreadyExists;
                        return;
                    }

                    ImageUrl = NewFileUrl;
                    ImageFileName = NewFileName;
                    ImageFilePath = $"{NewPath}\\{NewFileName}";
                    #endregion

                    #endregion

                    #region Pre-download stuff
                    Status = DownloadStatus.ReadyToDownload;

                    this.Invoke((Action)delegate () {
                        Log.Write($"Image {DownloadInfo.PostId} ready to download, performing final steps.");
                        sbStatus.Text = "Preparing download...";
                    });

                    if (!Directory.Exists(NewPath)) {
                        Directory.CreateDirectory(NewPath);
                    }

                    if (DownloadInfo.SaveInfo) {
                        if (ImageIsBlacklisted) {
                            if (File.Exists(DownloadInfo.DownloadPath + "\\images.blacklisted.nfo")
                            && new FileInfo(DownloadInfo.DownloadPath + "\\images.blacklisted.nfo").Length > 0) {
                                File.AppendAllText(DownloadInfo.DownloadPath + "\\images.blacklisted.nfo", $"\n\n{ImageInfo}");
                            }
                            else {
                                File.WriteAllText(DownloadInfo.DownloadPath + "\\images.blacklisted.nfo", ImageInfo, Encoding.UTF8);
                            }
                        }
                        else if (ImageIsBlacklisted) {
                            if (File.Exists(DownloadInfo.DownloadPath + "\\images.graylisted.nfo")
                            && new FileInfo(DownloadInfo.DownloadPath + "\\images.graylisted.nfo").Length > 0) {
                                File.AppendAllText(DownloadInfo.DownloadPath + "\\images.graylisted.nfo", $"\n\n{ImageInfo}");
                            }
                            else {
                                File.WriteAllText(DownloadInfo.DownloadPath + "\\images.graylisted.nfo", ImageInfo, Encoding.UTF8);
                            }
                        }
                        else {
                            if (File.Exists(DownloadInfo.DownloadPath + "\\images.nfo")
                            && new FileInfo(DownloadInfo.DownloadPath + "\\images.nfo").Length > 0) {
                                File.AppendAllText(DownloadInfo.DownloadPath + "\\images.nfo", $"\n\n{ImageInfo}");
                            }
                            else {
                                File.WriteAllText(DownloadInfo.DownloadPath + "\\images.nfo", ImageInfo, Encoding.UTF8);
                            }
                        }
                    }

                    #endregion

                    #region Main download
                    Status = DownloadStatus.Downloading;

                    this.Invoke((Action)delegate () {
                        Log.Write($"Downloading pool {DownloadInfo.PostId}");
                        pbDownloadStatus.Style = ProgressBarStyle.Blocks;
                        pbDownloadStatus.Text = "Starting download...";
                    });

                    using (DownloadClient = new()) {
                        if (DownloadInfo.UseForm) {
                            DownloadClient.DownloadProgressChanged += (s, e) => {
                                ThrottleCount++;
                                switch (ThrottleCount % 40) {
                                    case 0: {
                                        this.Invoke((Action)delegate () {
                                            pbDownloadStatus.Value = e.ProgressPercentage;
                                            pbDownloadStatus.Text = $"{DownloadHelpers.GetTransferRate(e.BytesReceived, e.TotalBytesToReceive)} ({e.ProgressPercentage}%)";
                                        });
                                    }
                                    break;
                                }

                            };
                            DownloadClient.DownloadFileCompleted += (s, e) => {
                                this.Invoke((Action)delegate () {
                                    pbDownloadStatus.Value = 100;
                                });
                            };

                            this.Invoke((Action)delegate () {
                                sbStatus.Text = $"Downloading {ImageFileName}";
                            });
                        }

                        DownloadClient.Proxy = WebRequest.GetSystemWebProxy();
                        DownloadClient.UserAgent = Program.UserAgent;
                        DownloadClient.Method = HttpMethod.GET;


                        ShouldRetry = true;
                        do {
                            try {
                                await DownloadClient.DownloadFileTaskAsync(ImageUrl, ImageFilePath);
                                ShouldRetry = false;
                            }
                            catch (ThreadAbortException) {
                                return;
                            }
                            catch (Exception ex) {
                                this.Invoke((Action)delegate () {
                                    pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                                });
                                if (Log.ReportRetriableException(ex, ImageUrl) != DialogResult.Retry) {
                                    ShouldRetry = false;
                                    Status = DownloadStatus.Errored;
                                    return;
                                }
                                else {
                                    this.Invoke((Action)delegate () {
                                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Normal;
                                        pbDownloadStatus.Value = 0;
                                    });
                                }
                            }
                        } while (ShouldRetry && !DownloadAbort);

                        if (DownloadAbort) return;

                    }

                    #endregion

                    #region Post-download
                    Status = DownloadStatus.Finished;
                    Log.Write($"Pool {DownloadInfo.PostId} finished.");
                    #endregion

                }
                #endregion

                #region Catch block
                catch (PoolOrPostWasDeletedException) {
                    Log.Write($"Image \"{DownloadInfo.PostId}\" was deleted.");
                    Status = DownloadStatus.PostOrPoolWasDeleted;
                    DownloadError = true;
                }
                catch (ApiReturnedNullOrEmptyException) {
                    Log.Write($"Image \"{DownloadInfo.PostId}\" Api returned null or empty.");
                    Status = DownloadStatus.ApiReturnedNullOrEmpty;
                    DownloadError = true;
                }
                catch (NoFilesToDownloadException) {
                    Log.Write($"No file is available for image \"{DownloadInfo.PostId}\".");
                    Status = DownloadStatus.NothingToDownload;
                    DownloadError = true;
                }
                catch (ThreadAbortException) {
                    Log.Write($"The image download \"{DownloadInfo.PostId}\" thread was aborted.");
                    if (DownloadClient != null && DownloadClient.IsBusy) {
                        DownloadClient.CancelAsync();
                    }
                    Status = DownloadStatus.Aborted;
                    DownloadError = true;
                }
                catch (ObjectDisposedException) {
                    Log.Write($"Seems like the image download \"{DownloadInfo.PostId}\" form got disposed.");
                    Status = DownloadStatus.FormWasDisposed;
                    DownloadError = true;
                }
                catch (WebException WebE) {
                    Log.Write($"A WebException occured download image \"{DownloadInfo.PostId}\".");
                    Status = DownloadStatus.Errored;
                    Log.ReportException(WebE, CurrentUrl);
                    DownloadError = true;
                }
                catch (Exception ex) {
                    Log.Write($"An Exception occured downloading image \"{DownloadInfo.PostId}\".");
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
            Downloader.ImagesBeingDownloaded.Add(DownloadInfo.PostId);
            DownloadThread.IsBackground = true;
            DownloadThread.Name = $"Image thread \"{DownloadInfo.PostId}\"";
            pbDownloadStatus.Style = ProgressBarStyle.Marquee;
            DownloadThread.Start();
            tmrTitle.Start();
        }

        protected override void FinishDownload() {
            tmrTitle.Stop();
            pbDownloadStatus.Style = ProgressBarStyle.Blocks;
            Downloader.ImagesBeingDownloaded.Remove(DownloadInfo.PostId);

            if (!ExitBox) {
                ExitBox = true;
            }

            if (DownloadInfo.OpenAfter && !DownloadError) {
                System.Diagnostics.Process.Start(DownloadInfo.DownloadPath);
            }

            base.FinishDownload();
            if (DownloadInfo.IgnoreFinish || !DownloadInfo.UseForm) {
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
                        this.Text = "Download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.Parsing: {
                        pbDownloadStatus.Text = "Finished while parsing?";
                        this.Text = "Image download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.ReadyToDownload: {
                        pbDownloadStatus.Text = "Finished while ready?";
                        this.Text = "Image download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.Downloading: {
                        pbDownloadStatus.Text = "Finished while downloading?";
                        this.Text = "Image download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.Finished: {
                        pbDownloadStatus.Text = "Downloading complete";
                        this.Text = "Image download complete";
                        sbStatus.Text = "Finished";
                        pbDownloadStatus.Value = pbDownloadStatus.Maximum;
                    } break;

                    case DownloadStatus.Errored: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "An error occurred";
                        this.Text = "Image download error";
                        sbStatus.Text = "Error";
                    } break;

                    case DownloadStatus.Aborted: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "The download was aborted";
                        this.Text = "Image download aborted";
                        sbStatus.Text = "Aborted";
                    } break;

                    case DownloadStatus.Forbidden: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "The connection was forbidden";
                        this.Text = "Image download error";
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
                        this.Text = "Image download complete";
                        sbStatus.Text = "Already exists";
                    } break;

                    case DownloadStatus.NothingToDownload: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "There's nothing to download";
                        this.Text = "Image download complete";
                        sbStatus.Text = "Nothing to download";
                    } break;

                    case DownloadStatus.PostOrPoolWasDeleted: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "The post was deleted.";
                        this.Text = "Post was deleted";
                        sbStatus.Text = "Post was deleted";
                    } break;

                    case DownloadStatus.ApiReturnedNullOrEmpty: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "The api returned null or empty";
                        this.Text = "Api null/empty error";
                        sbStatus.Text = "API parsing error";
                    } break;

                    case DownloadStatus.FileWasNullAfterBypassingBlacklist: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "Blacklist bypass failed";
                        this.Text = "Blacklist bypass error";
                        sbStatus.Text = "Blacklist bypass error";
                    } break;
                }
            }
        }

    }
}
