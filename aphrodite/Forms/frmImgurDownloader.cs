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

//Valid Imgur Schema Names: (? = may be too long or null/empty)
//albumid(data/id)
//albumtitle(data/title) ?
//albumaccounturl(data/account_url)
//albumaccountid(data/account_id)
//albumimagecount(data/images_count)
//albumdeletehash(data/deletehash)
//imageid(data/images/item/id)
//imagetitle(data/images/item/title) ?
//imageext(data/images/item/type) (Will require parsing though)
//imagewidth(data/images/item/width)
//imageheight(data/images/item/height)
//imageviews(data/images/item/views)
//imagedeletehash(data/images/item/deletehash)
//imagename(data/images/item/name)

    public partial class frmImgurDownloader : ExtendedForm {

        #region Fields
        private const string ApiBase = "https://api.imgur.com/3/album/{0}?client_id={1}";

        /// <summary>
        /// The information about the download of this current instance.
        /// </summary>
        public readonly ImgurDownloadInfo DownloadInfo;

        /// <summary>
        /// The count of files that will be downloaded.
        /// </summary>
        private int TotalFilesToDownload;
        /// <summary>
        /// The count of files that have already been downloaded.
        /// </summary>
        private int ExistingFiles;
        /// <summary>
        /// The count of files that have been parsed in the API.
        /// </summary>
        private int TotalParsed;
        /// <summary>
        /// The count of files that have been downloaded.
        /// </summary>
        private int DownloadedFiles;
        #endregion

        public frmImgurDownloader(ImgurDownloadInfo NewInfo) {
            InitializeComponent();

            if (Config.ValidPoint(Config.Settings.FormSettings.frmImgurDownloader_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmImgurDownloader_Location;
            }

            DownloadInfo = NewInfo;
        }

        private void frmImgurDownloader_FormClosing(Object sender, FormClosingEventArgs e) {
            Config.Settings.FormSettings.frmImgurDownloader_Location = this.Location;
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
            txtTags.Text = "null";
            if (DownloadInfo == null) {
                this.Text = "Unable to download image";
                pbDownloadStatus.Text = "Download info is null";
                sbStatus.Text = "Info null";
            }
            else if (string.IsNullOrWhiteSpace(DownloadInfo.Album)) {
                this.Text = "Unable to download image";
                pbDownloadStatus.Text = "No image id was specified";
                sbStatus.Text = "No post ID";
            }
            else if (Downloader.ImgurAlbumsBeingDownloaded.Contains(DownloadInfo.Album)) {
                txtTags.Text = DownloadInfo.Album;
                Status = DownloadStatus.AlreadyBeingDownloaded;
                this.Text = "Unable to download image";
                pbDownloadStatus.Text = "Image already being downloaded";
                sbStatus.Text = "Already in queue";
            }
            else {
                if (string.IsNullOrWhiteSpace(DownloadInfo.FileNameSchema)) {
                    DownloadInfo.FileNameSchema = "%index%_%id%";
                }

                if (!DownloadInfo.DownloadPath.EndsWith("\\Imgur")) {
                    DownloadInfo.DownloadPath += "\\Imgur";
                }

                txtTags.Text = DownloadInfo.Album;

                base.PrepareDownload();
            }
        }

        protected override void StartDownload() {
            DownloadThread = new(async () => {

                #region Try block
                try {

                    #region Defining new variables
                    List<string> FileURLs = new();      // The list of file urls that will be downloaded.
                    List<string> FileNames = new();     // The list of file names that will appear to the user.
                    List<string> FilePaths = new();     // The list of absolute paths to where the files will download to.
                    List<string> DirectoriesToMake = new(); // The directories that will be made.
                    byte[] ApiData;                     // The data byte-array the API returns. It's a byte array so it's faster to translate to xml.
                    string CurrentXml = string.Empty;   // The string of the current XML to be loaded into a XmlDocument;
                    string AlbumInfo = string.Empty;    // The buffer for the images.nfo file.
                    bool ShouldRetry = true;            // Whether retry-able exception catches should retry the action.
                    #endregion

                    #region Pre-api parse
                    DownloadInfo.DownloadPath += $"\\{DownloadInfo.Album}";
                    #endregion

                    #region Parse the api for the album info
                    Status = DownloadStatus.Parsing;

                    #region image api download + xml elements

                    #region Download data from api
                    this.Invoke((Action)delegate () {
                        sbStatus.Text = $"Downloading api data for image...";
                    });

                    CurrentUrl = string.Format(ApiBase, DownloadInfo.Album, Cryptography.Decrypt(DownloadInfo.ClientID));

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
                    //CurrentUrl = null;

                    if (DownloadError || DownloadAbort) {
                        return;
                    }
                    #endregion

                    #region Load api data
                    xmlDoc = new();
                    xmlDoc.LoadXml(CurrentXml);
                    //CurrentXml = string.Empty;

                    XmlNodeList xmlResultSuccess = xmlDoc.DocumentElement.SelectNodes("/root/success");
                    XmlNodeList xmlResultStatus = xmlDoc.DocumentElement.SelectNodes("/root/status");

                    XmlNodeList xmlAlbumId = xmlDoc.DocumentElement.SelectNodes("/root/data/id");
                    XmlNodeList xmlAlbumAccountUrl = xmlDoc.DocumentElement.SelectNodes("/root/data/account_url");
                    XmlNodeList xmlAlbumAccountId = xmlDoc.DocumentElement.SelectNodes("/root/data/account_id");
                    XmlNodeList xmlAlbumDescription = xmlDoc.DocumentElement.SelectNodes("/root/data/description");
                    XmlNodeList xmlAlbumImagesCount = xmlDoc.DocumentElement.SelectNodes("/root/data/images_count");
                    XmlNodeList xmlAlbumTitle = xmlDoc.DocumentElement.SelectNodes("/root/data/title");
                    //XmlNodeList xmlAlbumDeleteHash = xmlDoc.DocumentElement.SelectNodes("/root/data/deletehash");

                    XmlNodeList xmlImageLink = xmlDoc.DocumentElement.SelectNodes("/root/data/images/item/link");
                    XmlNodeList xmlImageId = xmlDoc.DocumentElement.SelectNodes("/root/data/images/item/id");
                    XmlNodeList xmlImageDescription = xmlDoc.DocumentElement.SelectNodes("/root/data/images/item/description");
                    XmlNodeList xmlImageName = xmlDoc.DocumentElement.SelectNodes("/root/data/images/item/name");
                    XmlNodeList xmlImageTitle = xmlDoc.DocumentElement.SelectNodes("/root/data/images/item/title");
                    //XmlNodeList xmlImageWidth = xmlDoc.DocumentElement.SelectNodes("/root/data/images/item/width");
                    //XmlNodeList xmlImageHeight = xmlDoc.DocumentElement.SelectNodes("/root/data/images/item/height");
                    //XmlNodeList xmlImageViews = xmlDoc.DocumentElement.SelectNodes("/root/data/images/item/views");
                    //XmlNodeList xmlImageDeleteHash = xmlDoc.DocumentElement.SelectNodes("/root/data/images/item/deletehash");
                    #endregion

                    this.Invoke((Action)delegate () {
                        sbStatus.Text = $"Parsing api data...";
                    });
                    #endregion

                    #region Check if it was deleted first
                    if (xmlResultSuccess[0].InnerText.ToLower() == "false") {
                        throw new NotImplementedException("The status result was false.");
                    }
                    else if (xmlResultStatus[0].InnerText.ToLower() != "200") {
                        throw new NotImplementedException("The status result was 200.");
                    }
                    else if (xmlAlbumImagesCount[0].InnerText == "0") {
                        throw new NoFilesToDownloadException("0 files were reported in xmlImagesCount.");
                    }
                    #endregion

                    #region Set the album info
                    AlbumInfo +=
                        $"ALBUM {xmlAlbumId[0].InnerText}\n" +
                        $"    URL: https://imgur.com/a/{DownloadInfo.Album}\n" +
                        $"    IMAGE COUNT: {xmlAlbumImagesCount[0].InnerText}\n" +
                        $"    ACCOUNT: ID {xmlAlbumAccountId[0].InnerText}, URL {xmlAlbumAccountUrl[0].InnerText}\n" +
                        //$"    DELETE HASH: {xmlAlbumDeleteHash[0].InnerText}\n" +
                        $"    TITLE: {xmlAlbumTitle[0].InnerText ?? "[No title]"}\n" +
                        $"    DESCRIPTION: [{(string.IsNullOrWhiteSpace(xmlAlbumDescription[0].InnerText) ? "no description" : $"\n{xmlAlbumDescription[0].InnerText}\n")}]\n" +
                        $"==========|~~|==========\n\n";
                    #endregion

                    #region Parse through each image
                    string NewFileName;
                    string NewPath;
                    string FileExtension;
                    bool AlbumHasName = xmlImageName.Count > 0;
                    for (int CurrentImage = 0; CurrentImage < xmlImageLink.Count; CurrentImage++) {
                        if (DownloadInfo.ImageLimit > 0 && DownloadInfo.ImageLimit > CurrentImage) {
                            Log.Write("ImageLimit reached, breaking parse loop.");
                            break;
                        }

                        if (!string.IsNullOrWhiteSpace(xmlImageLink[CurrentImage].InnerText)) {

                            FileExtension = Path.GetExtension(xmlImageLink[CurrentImage].InnerText).Trim('.');

                            //NewFileName = DownloadInfo.FileNameSchema;
                            //NewFileName = NewFileName.Replace("%albumid%", xmlAlbumId[0].InnerText);
                            //NewFileName = NewFileName.Replace("%albumtitle%", xmlAlbumTitle[0].InnerText);
                            //NewFileName = NewFileName.Replace("%albumaccounturl%", xmlAlbumAccountUrl[0].InnerText);
                            //NewFileName = NewFileName.Replace("%albumaccountid%", xmlAlbumAccountId[0].InnerText);
                            //NewFileName = NewFileName.Replace("%imageindex%", $"{CurrentImage + 1:0000.##}");
                            //NewFileName = NewFileName.Replace("%imageid%", xmlImageId[CurrentImage].InnerText);
                            //NewFileName = NewFileName.Replace("%imagetitle%", xmlImageTitle[CurrentImage].InnerText ?? "no-title");
                            //NewFileName = NewFileName.Replace("%imagename%", xmlImageName[CurrentImage].InnerText ?? "no-name");
                            //NewFileName = NewFileName.Replace("%imageext%", FileExtension);
                            //NewFileName += "." + FileExtension;

                            NewFileName = DownloadInfo.FileNameSchema;

                            NewFileName =
                                NewFileName
                                    .Replace("%albumid%", xmlAlbumId[0].InnerText)
                                    .Replace("%albumtitle%", xmlAlbumTitle[0].InnerText ?? "no-album-title")
                                    .Replace("%albumaccounturl%", xmlAlbumAccountUrl[0].InnerText)
                                    .Replace("%albumaccountid%", xmlAlbumAccountId[0].InnerText)
                                    .Replace("%imageindex%", $"{CurrentImage + 1:0000.##}")
                                    .Replace("%imageid%", xmlImageId[CurrentImage].InnerText)
                                    .Replace("%imagetitle%", xmlImageTitle[CurrentImage].InnerText ?? "no-image-title")
                                    .Replace("%imagename%", AlbumHasName ? xmlImageName[CurrentImage].InnerText ?? "no-name" : "no-name")
                                    .Replace("%imageext%", FileExtension) + $".{FileExtension}";

                            NewPath = $"{DownloadInfo.DownloadPath}";
                            if (DownloadInfo.SeparateNonImages) {
                                NewPath += FileExtension switch {
                                    "jpeg" or "jpg" or "png" => "",
                                    _ => "\\" + FileExtension
                                };
                            }

                            if (File.Exists(NewPath + "\\" + NewFileName)) {
                                ExistingFiles++;
                                continue;
                            }
                            else {
                                TotalFilesToDownload++;
                                if (!DirectoriesToMake.Contains(NewPath)) {
                                    DirectoriesToMake.Add(NewPath);
                                }
                                FileURLs.Add(xmlImageLink[CurrentImage].InnerText);
                                FileNames.Add(NewFileName);
                                FilePaths.Add(NewPath + "\\" + NewFileName);
                                if (DownloadInfo.SaveInfo) {
                                    AlbumInfo +=
                                        $"IMAGE #{CurrentImage + 1:0000.##} {xmlImageId[CurrentImage].InnerText}:\n" +
                                        $"    URL: {xmlImageLink[CurrentImage].InnerText}\n" +
                                        $"    TITLE: {xmlImageTitle[CurrentImage].InnerText ?? "[no title]"}\n" +
                                        $"    NAME: {(AlbumHasName ? xmlImageName[CurrentImage].InnerText ?? "": "[no name]")}\n" +
                                        $"    DESCRIPTION: [{(string.IsNullOrWhiteSpace(xmlImageDescription[CurrentImage].InnerText) ? "no description" : $"\n{xmlImageDescription[CurrentImage].InnerText}\n")}]\n\n";
                                }
                            }
                        }
                        TotalParsed++;
                    }
                    #endregion

                    #region Update totals
                    this.Invoke((Action)delegate () {
                        lbFileStatus.Text =
                            $"total files parsed: {TotalParsed}\n\n" +

                            $"files to download: {TotalFilesToDownload}\n\n" +
                            
                            $"files that exist: {ExistingFiles}";
                    });
                    #endregion

                    #endregion

                    #region Pre-download
                    Status = DownloadStatus.ReadyToDownload;
                    this.Invoke((Action)delegate {
                        sbStatus.Text = "Preparing download...";
                    });

                    for (int i = 0; i < DirectoriesToMake.Count; i++) {
                        if (!Directory.Exists(DirectoriesToMake[i])) {
                            Directory.CreateDirectory(DirectoriesToMake[i]);
                        }
                    }
                    if (DownloadInfo.SaveInfo) {
                        //if (File.Exists(DownloadInfo.DownloadPath + "\\album.nfo") && new FileInfo(DownloadInfo.DownloadPath + "\\album.nfo").Length > 0) {
                        //    File.AppendAllText(DownloadInfo.DownloadPath + "\\album.nfo", "\n\n" + AlbumInfo, Encoding.UTF8);
                        //}
                        //else {
                        File.WriteAllText(DownloadInfo.DownloadPath + "\\album.nfo", AlbumInfo.Trim(), Encoding.UTF8);
                        //}
                    }

                    #endregion

                    #region Download
                    Status = DownloadStatus.Downloading;

                    this.Invoke((Action)delegate {
                        Log.Write($"There are {TotalFilesToDownload:N0} files to download with the inkbunny terms \"{DownloadInfo.Album}\"");
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
                                }
                                break;
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

                        if (TotalFilesToDownload > 0) {
                            Log.Write($"Downloading files for imgur album form \"{DownloadInfo.Album}\"");
                            for (int CurrentFile = 0; CurrentFile < FileURLs.Count; CurrentFile++) {
                                if (!string.IsNullOrWhiteSpace(FileURLs[CurrentFile])) {
                                    this.Invoke((Action)delegate () {
                                        pbTotalStatus.Text = $"download file {CurrentFile + 1:N0} of {TotalFilesToDownload:N0}";
                                        sbStatus.Text = $"Downloading {FileNames[CurrentFile]}";
                                    });
                                    ShouldRetry = true;
                                    do {
                                        try {
                                            await DownloadClient.DownloadFileTaskAsync(FileURLs[CurrentFile], FilePaths[CurrentFile]);
                                            DownloadedFiles++;
                                            ShouldRetry = false;
                                        }
                                        catch (ThreadAbortException) {
                                            return;
                                        }
                                        catch (Exception ex) {
                                            if (Log.ReportRetriableException(ex, FileURLs[CurrentFile]) != DialogResult.Retry) {
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

                    }
                    #endregion

                    #region Post-download
                    Status = DownloadStatus.Finished;
                    Log.Write($"Imgur download \"{DownloadInfo.Album}\" finished with {DownloadedFiles:N0} files downloaded.");
                    #endregion

                }
                #endregion

                #region Catch block
                catch (ApiReturnedNullOrEmptyException) {
                    Log.Write($"Imgur album \"{DownloadInfo.Album}\" Api returned null or empty.");
                    Status = DownloadStatus.ApiReturnedNullOrEmpty;
                    DownloadError = true;
                }
                catch (NoFilesToDownloadException) {
                    Log.Write($"No files are available for imgur album \"{DownloadInfo.Album}\".");
                    Status = DownloadStatus.NothingToDownload;
                    DownloadError = true;
                }
                catch (ThreadAbortException) {
                    Log.Write($"Imgur album download \"{DownloadInfo.Album}\" thread was aborted.");
                    if (DownloadClient != null && DownloadClient.IsBusy) {
                        DownloadClient.CancelAsync();
                    }
                    Status = DownloadStatus.Aborted;
                    DownloadError = true;
                }
                catch (ObjectDisposedException) {
                    Log.Write($"Seems like the imgur album download \"{DownloadInfo.Album}\" form got disposed.");
                    Status = DownloadStatus.FormWasDisposed;
                    DownloadError = true;
                }
                catch (WebException WebE) {
                    Log.Write($"A WebException occured downloading from the imgur album \"{DownloadInfo.Album}\".");
                    Status = DownloadStatus.Errored;
                    Log.ReportException(WebE, CurrentUrl);
                    DownloadError = true;
                }
                catch (Exception ex) {
                    Log.Write($"An Exception occured downloading from the imgur album \"{DownloadInfo.Album}\".");
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
            Downloader.ImgurAlbumsBeingDownloaded.Add(DownloadInfo.Album);
            DownloadThread.IsBackground = true;
            DownloadThread.Name = $"Imgur album thread \"{DownloadInfo.Album}\"";
            pbDownloadStatus.Style = ProgressBarStyle.Marquee;
            DownloadThread.Start();
            tmrTitle.Start();
        }

        protected override void FinishDownload() {
            tmrTitle.Stop();
            pbDownloadStatus.Style = ProgressBarStyle.Blocks;
            Downloader.ImgurAlbumsBeingDownloaded.Remove(DownloadInfo.Album);

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
                        this.Text = "Imgur album download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.Parsing: {
                        pbDownloadStatus.Text = "Finished while parsing?";
                        pbTotalStatus.Text = "Weird error occurred";
                        this.Text = "Imgur album download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.ReadyToDownload: {
                        pbDownloadStatus.Text = "Finished while ready?";
                        pbTotalStatus.Text = "Weird error occurred";
                        this.Text = "Imgur album download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.Downloading: {
                        pbDownloadStatus.Text = "Finished while downloading?";
                        pbTotalStatus.Text = "Weird error occurred";
                        this.Text = "Imgur album download finished?";
                        sbStatus.Text = "Finished?";
                    } break;

                    case DownloadStatus.Finished: {
                        pbDownloadStatus.Text = "Downloading complete";
                        pbTotalStatus.Text = $"{DownloadedFiles} / {TotalFilesToDownload} files downloaded";
                        this.Text = "Imgur album download complete";
                        sbStatus.Text = "Finished";
                        pbDownloadStatus.Value = pbDownloadStatus.Maximum;
                        pbTotalStatus.Value = pbTotalStatus.Maximum;
                    } break;

                    case DownloadStatus.Errored: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "Error";
                        pbTotalStatus.Text = "An error occurred";
                        this.Text = "Imgur album download error";
                        sbStatus.Text = "Error";
                    } break;

                    case DownloadStatus.Aborted: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "Aborted";
                        pbTotalStatus.Text = "The download was aborted";
                        this.Text = "Imgur album download aborted";
                        sbStatus.Text = "Aborted";
                    } break;

                    case DownloadStatus.Forbidden: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "Forbidden";
                        pbTotalStatus.Text = "The connection was forbidden";
                        this.Text = "Imgur album download error";
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
                        this.Text = "Imgur album download complete";
                        sbStatus.Text = "Already exists";
                    } break;

                    case DownloadStatus.NothingToDownload: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "There's nothing to download";
                        pbTotalStatus.Text = "Nothing to download";
                        this.Text = "Imgur album download complete";
                        sbStatus.Text = "Nothing to download";
                    } break;

                    case DownloadStatus.PostOrPoolWasDeleted: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "The post was deleted.";
                        pbTotalStatus.Text = "An error occurred";
                        this.Text = "Imgur album was deleted";
                        sbStatus.Text = "Post was deleted";
                    } break;

                    case DownloadStatus.ApiReturnedNullOrEmpty: {
                        pbDownloadStatus.State = murrty.controls.ProgressBarState.Error;
                        pbDownloadStatus.Text = "The api returned null or empty";
                        pbTotalStatus.Text = "An error occurred";
                        this.Text = "Imgur api null/empty error";
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
