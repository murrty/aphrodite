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
    public partial class frmPoolDownloader : Form {
        public frmPoolDownloader() {
            InitializeComponent();
        }

        public string id;
        public string poolurl;
        public bool openAfter = false;
        public bool fromURL = false;
        Thread poolDownload;

        #region downloader
        public static string poolJson = "https://e621.net/pool/show.json?id=";
        public static string poolPageJson = "&page=";
        public static string emptyXML = "<root type=\"array\"></root>";
        public static bool isValidPoolLink(string url) {
            if (url.StartsWith("http://e621.net/pool/show/") || url.StartsWith("https://e621.net/pool/show/") || url.StartsWith("http://www.e621.net/pool/show/") || url.StartsWith("https://www.e621.net/pool/show/") || url.StartsWith("e621.net/pool/show/") || url.StartsWith("www.e621.net/pool/show/")) {
                return true;
            }
            else {
                return false;
            }
        }

        private string getJSON(string url, string header = "User-Agent: Undefined/0.0") {
            Debug.Print("getJson starting");
            this.Invoke((MethodInvoker)(() => status.Text = "Downloading pool information..."));

            if (!header.StartsWith("User-Agent: ")) {
                header = "User-Agent: " + header;
                Debug.Print("Set header to " + header);
            }

            try {
                Debug.Print("Starting pool json download");
                using (WebClient wc = new WebClient()) {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    wc.Proxy = WebProxy.GetDefaultProxy();
                    wc.Headers.Add(header);
                    string json = wc.DownloadString(url);
                    byte[] bytes = Encoding.ASCII.GetBytes(json);
                    using (var stream = new MemoryStream(bytes)) {
                        var quotas = new XmlDictionaryReaderQuotas();
                        var jsonReader = JsonReaderWriterFactory.CreateJsonReader(stream, quotas);
                        var xml = XDocument.Load(jsonReader);
                        stream.Flush();
                        stream.Close();
                        if (xml != null) {
                            Debug.Print("Json converted, returning full json");
                            return xml.ToString();
                        }
                        else {
                            Debug.Print("Json returned null, returning null");
                            return null;
                        }
                    }
                }
            }
            catch (ThreadAbortException thrEx) {
                Debug.Print("Thread was requested to be, and has been, aborted.");
                Debug.Print("==========BEGIN THREADABORTEXCEPTION==========");
                Debug.Print(thrEx.ToString());
                Debug.Print("==========END THREADABORTEXCEPTION==========");
                return null;
                throw thrEx;
            }
            catch (WebException WebE) {
                Debug.Print("A WebException has occured.");
                Debug.Print("==========BEGIN WEBEXCEPTION==========");
                Debug.Print(WebE.ToString());
                Debug.Print("==========END WEBEXCEPTION==========");
                webError(WebE, url);
                return null;
                throw WebE;
            }
            catch (Exception ex) {
                Debug.Print("A gneral exception has occured.");
                Debug.Print("==========BEGIN EXCEPTION==========");
                Debug.Print(ex.ToString());
                Debug.Print("==========END EXCEPTION==========");
                return null;
                throw ex;
            }
        }

        private bool downloadPool(string poolID, string saveTo, bool saveInfo = true, bool usePoolName = true, string header = "User-Agent: Undefined/0.0") {
            string dlURL = string.Empty;
            Debug.Print("downloadPool starting");
            this.Invoke((MethodInvoker)(() => status.Text = "Preparing to download..."));
            string poolName;
            List<string> urls = new List<string>();
            List<string> blacklist = new List<string>();
            List<string> blacklistedURLS = new List<string>();
            int blacklistCount = 0;
            string poolInfo = string.Empty;
            string blacklistInfo = string.Empty;
            int page = 0;

            if (!header.StartsWith("User-Agent: ")) {
                header = "User-Agent: " + header;
                Debug.Print("Set header to " + header);
            }

            if (!saveTo.EndsWith("\\Pools"))
                saveTo += "\\Pools";

            if (!string.IsNullOrEmpty(Settings.Default.blacklist) || !string.IsNullOrWhiteSpace(Settings.Default.blacklist))
                blacklist = new List<string>(Settings.Default.blacklist.Split(' '));

            try {
                // get the json file from e621
                Debug.Print("Calling getJson for " + poolID);
                dlURL = poolJson + poolID;
                string xml = getJSON(poolJson + poolID, header);

                if (xml == emptyXML || string.IsNullOrEmpty(xml)) {
                    Debug.Print("xml is empty, aborting");
                    this.Invoke((MethodInvoker)(() => status.Text = "The pool's json returned empty or null. Aborting."));
                    return false;
                }

                // convert to xmldocument since why should i learn json parsing
                Debug.Print("Loading xml as xmldoc");
                this.Invoke((MethodInvoker)(() => status.Text = "Parsing pool json..."));
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNodeList xmlName = doc.DocumentElement.SelectNodes("/root/name");
                XmlNodeList xmlDescription = doc.DocumentElement.SelectNodes("/root/description");
                XmlNodeList xmlCount = doc.DocumentElement.SelectNodes("/root/post_count");
                poolInfo += "POOL: " + poolID + "\n    NAME: " + xmlName[0].InnerText + "\n    PAGES: " + xmlCount[0].InnerText + "\n    URL: https://e621.net/pool/show/" + poolID + "\n    DESCRIPTION:\n\"" + xmlDescription[0].InnerText + "\"\n\n";

                Debug.Print("Calculating pages to download");
                int count = Convert.ToInt32(xmlCount[0].InnerText);
                int pages = 1;
                if (count > 24) {
                    decimal pageGuess = decimal.Divide(count, 24);
                    pages = Convert.ToInt32(Math.Ceiling(pageGuess));
                    this.Invoke((MethodInvoker)(() => lbFile.Text = "File 0 of " + (count)));
                    Debug.Print("Post count exceeded 24, there are ~" + pages.ToString());
                }
                else {
                    this.Invoke((MethodInvoker)(() => lbFile.Text = "File 0 of " + (count)));
                    Debug.Print("There is only 1 page to download from");
                }

                // Set saveTo
                poolName = xmlName[0].InnerText.Replace("\\","_").Replace("/", "_").Replace(":","_").Replace("*","_").Replace("?","_").Replace("\"","_").Replace("<","_").Replace(">","_").Replace("|","_");
                Debug.Print("Changing output directory to " + saveTo + "\\" + poolName);
                saveTo += "\\" + poolName.Replace("_", " ");
                this.Invoke((MethodInvoker)(() => lbName.Text = poolName.Replace("_", " ")));

                Debug.Print("Gathering list of URLs from the pool");
                XmlNodeList xmlID = doc.DocumentElement.SelectNodes("/root/posts/item/id");
                XmlNodeList xmlMD5 = doc.DocumentElement.SelectNodes("/root/posts/item/md5");
                XmlNodeList xmlUrl = doc.DocumentElement.SelectNodes("/root/posts/item/file_url");
                XmlNodeList xmlTags = doc.DocumentElement.SelectNodes("/root/posts/item/tags");
                XmlNodeList xmlArtist = doc.DocumentElement.SelectNodes("/root/posts/item/artist");
                XmlNodeList xmlScore = doc.DocumentElement.SelectNodes("/root/posts/item/score");
                XmlNodeList xmlRating = doc.DocumentElement.SelectNodes("/root/posts/item/rating");
                xmlDescription = doc.DocumentElement.SelectNodes("/root/posts/item/description");
                Debug.Print("There are " + xmlUrl.Count + 1 + " posts in this pool");
                // Check blacklist
                for (int j = 0; j < xmlTags.Count; j++) {
                    string artists = string.Empty; //= xmlArtist[j].InnerXml.ToString().Replace("</item><item type=\"string\">", "\n                   ").Replace("<item type=\"string\">", "").Replace("</item>", "");
                    string rating = xmlRating[j].InnerText;
                    bool blacklisted = false;
                    string foundblacklistedtags = string.Empty;
                    List<string> foundTags = xmlTags[j].InnerText.Split(' ').ToList();
                    page++;

                    if (rating == "e") {
                        rating = rating.Replace("e", "Explicit");
                    }
                    else if (rating == "q") {
                        rating = rating.Replace("q", "Questionable");
                    }
                    else if (rating == "s") {
                        rating = rating.Replace("s", "Safe");
                    }
                    else {
                        rating = "unknown";
                    }

                    for (int k = 0; k < xmlArtist[j].ChildNodes.Count; k++) {
                        artists += xmlArtist[j].ChildNodes[k].InnerText + "\n                   ";
                    }
                    artists = artists.TrimEnd(' ');
                    artists = artists.TrimEnd('\n');

                    if (blacklist.Count > 0) {
                        for (int k = 0; k < foundTags.Count; k++) {
                            for (int l = 0; l < blacklist.Count; l++) {
                                if (foundTags[k] == blacklist[l]) {
                                    if (blacklisted) {
                                        foundblacklistedtags += " " + foundTags[k];
                                    }
                                    else {
                                        foundblacklistedtags += foundTags[k];
                                        blacklisted = true;
                                    }
                                }
                            }
                        }

                        if (blacklisted) {
                            Debug.Print("Blacklisted tag found, adding to the blacklist. Offending tags: " + foundblacklistedtags + " on id " + xmlID[j].InnerText);
                            blacklistCount++;
                        }
                    }

                    urls.Add(xmlUrl[j].InnerText);

                    //for (int k = 0; k < xmlArtist.Count; k++) {
                    //    artists += xmlArtist[k].InnerText + "\n                   ";
                    //}
                    //artists = artists.TrimEnd(' ');
                    //artists = artists.TrimEnd('\n');

                    if (!blacklisted) {
                        poolInfo += "    PAGE: " + (page) + "\n        MD5: " + xmlMD5[j].InnerText + "\n        URL: https://e621.net/post/show/" + xmlID[j].InnerText + "\n        ARTIST(S): " + artists + "\n        TAGS: " + xmlTags[j].InnerText + "\n        SCORE: " + xmlScore[j].InnerText + "\n        RATING: " + rating + "\n        DESCRIPITON:\n\"" + xmlDescription[j].InnerText + "\"\n\n";
                    }
                    else if (blacklisted) {
                        blacklistedURLS.Add(xmlUrl[j].InnerText);
                        blacklistInfo += "    PAGE " + (page) + ":\n        MD5: " + xmlMD5[j].InnerText + "\n        URL: https://e621.net/post/show/" + xmlID[j].InnerText + "\n        ARTIST(S): " + artists + "\n        TAGS: " + xmlTags[j].InnerText + "\n        SCORE: " + xmlScore[j].InnerText + "\n        RATING: " + rating + "\n        DESCRIPITON:\n\"" + xmlDescription[j].InnerText + "\"\n        OFFENDING TAGS: " + foundblacklistedtags + "\n\n";
                        if (!Settings.Default.saveBlacklisted || Pools.Default.mergeBlacklisted)
                            poolInfo += "    BLACKLISTED PAGE: " + (page) + "\n        MD5: " + xmlMD5[j].InnerText + "\n        URL: https://e621.net/post/show/" + xmlID[j].InnerText + "\n        ARTIST(S): " + artists + "\n        TAGS: " + xmlTags[j].InnerText + "\n        SCORE: " + xmlScore[j].InnerText + "\n        RATING: " + rating + "\n        DESCRIPITON:\n\"" + xmlDescription[j].InnerText + "\"\n\n";
                    }
                }

                // Get a list of Json links in each additional page (if applicable), otherwise just get the links from the pages.
                if (pages > 1) {
                    Debug.Print("Gathering list of URLs from each page in the pool");
                    for (int i = 2; i < pages + 1; i++) {
                        this.Invoke((MethodInvoker)(() => status.Text = "Downloading pool information from page " + (i) + "..."));
                        Debug.Print("Page number " + i);
                        dlURL = poolJson + poolID + poolPageJson + i;
                        xml = getJSON(poolJson + poolID + poolPageJson + i, header);
                        if (xml == emptyXML || string.IsNullOrWhiteSpace(xml))
                            break;

                        doc.LoadXml(xml);
                        xmlID = doc.DocumentElement.SelectNodes("/root/posts/item/id");
                        xmlMD5 = doc.DocumentElement.SelectNodes("/root/posts/item/md5");
                        xmlUrl = doc.DocumentElement.SelectNodes("/root/posts/item/file_url");
                        xmlTags = doc.DocumentElement.SelectNodes("/root/posts/item/tags");
                        xmlArtist = doc.DocumentElement.SelectNodes("/root/posts/item/artist");
                        xmlScore = doc.DocumentElement.SelectNodes("/root/posts/item/score");
                        xmlRating = doc.DocumentElement.SelectNodes("/root/posts/item/rating");
                        xmlDescription = doc.DocumentElement.SelectNodes("/root/posts/item/description");
                        Debug.Print("There are " + xmlUrl.Count + 1 + " posts on page " + i);
                        // Check blacklist
                        for (int j = 0; j < xmlTags.Count; j++) {
                            string artists = string.Empty; //= xmlArtist[j].InnerXml.ToString().Replace("</item><item type=\"string\">", "\n                   ").Replace("<item type=\"string\">", "").Replace("</item>", "");
                            string rating = xmlRating[j].InnerText;
                            bool blacklisted = false;
                            string foundblacklistedtags = string.Empty;
                            List<string> foundTags = xmlTags[j].InnerText.Split(' ').ToList();
                            page++;

                            if (rating == "e") {
                                rating = rating.Replace("e", "Explicit");
                            }
                            else if (rating == "q") {
                                rating = rating.Replace("q", "Questionable");
                            }
                            else if (rating == "s") {
                                rating = rating.Replace("s", "Safe");
                            }
                            else {
                                rating = "unknown";
                            }

                            for (int k = 0; k < xmlArtist[j].ChildNodes.Count; k++) {
                                artists += xmlArtist[j].ChildNodes[k].InnerText + "\n                   ";
                            }
                            artists = artists.TrimEnd(' ');
                            artists = artists.TrimEnd('\n');

                            if (blacklist.Count > 0) {
                                for (int k = 0; k < foundTags.Count; k++) {
                                    for (int l = 0; l < blacklist.Count; l++) {
                                        if (foundTags[k] == blacklist[l]) {
                                            if (blacklisted) {
                                                foundblacklistedtags += " " + foundTags[k];
                                            }
                                            else {
                                                foundblacklistedtags += foundTags[k];
                                                blacklisted = true;
                                            }
                                        }
                                    }
                                }

                                if (blacklisted) {
                                    Debug.Print("Blacklisted tag found, adding to the blacklist. Offending tags: " + foundblacklistedtags + " on id " + xmlID[j].InnerText);
                                    blacklistCount++;
                                }
                            }

                            urls.Add(xmlUrl[j].InnerText);

                            //for (int k = 0; k < xmlArtist.Count; k++) {
                            //    artists += xmlArtist[k].InnerText + "\n                   ";
                            //}
                            //artists = artists.TrimEnd(' ');
                            //artists = artists.TrimEnd('\n');

                            if (!blacklisted) {
                                poolInfo += "    PAGE: " + (page) + "\n        MD5: " + xmlMD5[j].InnerText + "\n        URL: https://e621.net/post/show/" + xmlID[j].InnerText + "\n        ARTIST(S): " + artists + "\n        TAGS: " + xmlTags[j].InnerText + "\n        SCORE: " + xmlScore[j].InnerText + "\n        RATING: " + rating + "\n        DESCRIPITON:\n\"" + xmlDescription[j].InnerText + "\"\n\n";
                            }
                            else if (blacklisted) {
                                blacklistedURLS.Add(xmlUrl[j].InnerText);
                                blacklistInfo += "    PAGE " + (page) + ":\n        MD5: " + xmlMD5[j].InnerText + "\n        URL: https://e621.net/post/show/" + xmlID[j].InnerText + "\n        ARTIST: " + xmlArtist[j].InnerText + "\n        TAGS: " + xmlTags[j].InnerText + "\n        SCORE: " + xmlScore[j].InnerText + "\n        RATING: " + rating + "\n        DESCRIPITON:\n\"" + xmlDescription[j].InnerText + "\"\n        OFFENDING TAGS: " + foundblacklistedtags + "\n\n";
                                if (!Settings.Default.saveBlacklisted || Pools.Default.mergeBlacklisted)
                                    poolInfo += "    BLACKLISTED PAGE: " + (page) + "\n        MD5: " + xmlMD5[j].InnerText + "\n        URL: https://e621.net/post/show/" + xmlID[j].InnerText + "\n        ARTIST: " + xmlArtist[j].InnerText + "\n        TAGS: " + xmlTags[j].InnerText + "\n        SCORE: " + xmlScore[j].InnerText + "\n        RATING: " + rating + "\n        DESCRIPITON:\n\"" + xmlDescription[j].InnerText + "\"\n\n";
                            }
                        }
                    }
                }

                this.Invoke((MethodInvoker)(() => lbTotal.Text = (urls.Count - blacklistedURLS.Count) + " pages\n" + (blacklistedURLS.Count) + " blacklisted\n" + (urls.Count) + " total"));

                if (urls.Count <= 0)
                    return false;

                // Create save directory & save info
                if (!Directory.Exists(saveTo)) {
                    Debug.Print("Save directory does not exist, creating...");
                    Directory.CreateDirectory(saveTo);
                }
                if (Settings.Default.saveBlacklisted && !Directory.Exists(saveTo + "\\blacklisted")) {
                    if (!Pools.Default.mergeBlacklisted)
                        Directory.CreateDirectory(saveTo + "\\blacklisted");
                }
                if (saveInfo) {
                    poolInfo = poolInfo.TrimEnd('\n');
                    Debug.Print("Saving pool.nfo");
                    this.Invoke((MethodInvoker)(() => status.Text = "Saving pool.nfo"));
                    File.WriteAllText(saveTo + "\\pool.nfo", poolInfo, Encoding.UTF8);
                    if (Settings.Default.saveBlacklisted && !Pools.Default.mergeBlacklisted) {
                        blacklistInfo = blacklistInfo.TrimEnd('\n');
                        Debug.Print("Saving pool.blacklisted.nfo");
                        this.Invoke((MethodInvoker)(() => status.Text = "Saving pool.blacklisted.nfo"));
                        File.WriteAllText(saveTo + "\\blacklisted\\pool.blacklisted.nfo", poolInfo, Encoding.UTF8);
                    }
                }

                this.Invoke((MethodInvoker)(() => pbDownloadStatus.Style = ProgressBarStyle.Blocks));

                // Finally, download them. (URL .Split('/')[6] = FileName)
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
                        if (!this.Disposing || !this.IsDisposed) {
                            lock (e.UserState) {
                                this.BeginInvoke(new MethodInvoker(() => {
                                    pbDownloadStatus.Value = 0;
                                    lbPercentage.Text = "0%";
                                }));
                                Monitor.Pulse(e.UserState);
                            }
                        }
                    };
                    wc.Proxy = WebProxy.GetDefaultProxy();
                    wc.Headers.Add(header);
                    Debug.Print("Header has been set, starting download of all posts");
                    for (int y = 0; y < urls.Count; y++) {
                        dlURL = urls[y];
                        bool blacklisted = false;
                        for (int z = 0; z < blacklistedURLS.Count; z++) {
                            if (dlURL == blacklistedURLS[z]) {
                                if (blacklisted != true)
                                    blacklisted = true;
                                break;
                            }
                            else {
                                if (blacklisted != false)
                                   blacklisted = false;
                            }
                        }

                        string pagenumber = y.ToString();
                        if (y < 10) {
                            pagenumber = "0" + (y + 1);
                        }
                        else {
                            pagenumber = (y + 1).ToString();
                        }
                        this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading page " + (y + 1) + " of " + (urls.Count)));

                        string filename;
                        if (usePoolName) {
                            filename = poolName + "_" + pagenumber + "." + dlURL.Split('.')[3];
                        }
                        else {
                            filename = pagenumber + "_" + dlURL.Split('/')[6];
                        }

                        if (blacklisted && Settings.Default.saveBlacklisted && !Pools.Default.mergeBlacklisted) {
                            if (!File.Exists(saveTo + "\\blacklisted\\" + filename)) {
                                Debug.Print("Downloading " + dlURL + " as blacklisted/" + filename);
                                this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + dlURL.Split('/')[6]));
                                var sync = new Object();
                                lock (sync) {
                                    wc.DownloadFileAsync(new Uri(dlURL), saveTo + "\\blacklisted\\" + filename, sync);
                                    Monitor.Wait(sync);
                                }
                                Debug.Print("Finished, saved at " + saveTo + "\\blacklisted\\" + filename);
                            }
                            else {
                                Debug.Print("File " + filename + " exists, has it already been downloaded?");
                            }
                        }
                        else if (blacklisted && Settings.Default.saveBlacklisted && Pools.Default.mergeBlacklisted) {
                            if (!File.Exists(saveTo + "\\" + filename)) {
                                Debug.Print("Downloading " + dlURL + " as " + filename);
                                this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + dlURL.Split('/')[6]));
                                var sync = new Object();
                                lock (sync) {
                                    wc.DownloadFileAsync(new Uri(dlURL), saveTo + "\\" + filename, sync);
                                    Monitor.Wait(sync);
                                }
                                Debug.Print("Finished, saved at " + saveTo + "\\" + filename);
                            }
                            else {
                                Debug.Print("File " + filename + " exists, has it already been downloaded?");
                            }
                        }
                        else if (blacklisted) {
                            Debug.Print("File " + filename + "is blacklisted. Not downloading.");
                            continue;
                        }
                        else {
                            if (!File.Exists(saveTo + "\\" + filename)) {
                                Debug.Print("Downloading " + dlURL + " as " + filename);
                                this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + dlURL.Split('/')[6]));
                                var sync = new Object();
                                lock (sync) {
                                    wc.DownloadFileAsync(new Uri(dlURL), saveTo + "\\" + filename, sync);
                                    Monitor.Wait(sync);
                                }
                                Debug.Print("Finished, saved at " + saveTo + "\\" + filename);
                            }
                            else {
                                Debug.Print("File " + filename + " exists, has it already been downloaded?");
                            }
                        }
                    }
                }

                this.BeginInvoke(new MethodInvoker(() => {
                    lbPercentage.Text = "Done";
                    lbFile.Text = "All " + (urls.Count) + " pages downloaded.";
                    pbDownloadStatus.Value = 101;
                    status.Text = "Finished downloading pool " + poolID;
                    this.Text = "Pool downloaded";
                }));

                Debug.Print("Pool has been downloaded successfully, returning");
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
                webError(WebE, dlURL);
                return false;
                throw WebE;
            }
            catch (Exception ex) {
                Debug.Print("A gneral exception has occured.");
                Debug.Print("==========BEGIN EXCEPTION==========");
                Debug.Print(ex.ToString());
                Debug.Print("==========END EXCEPTION==========");
                return false;
                throw ex;
            }
        }

        private void webError(WebException WebE, string url = "Not defined.") {
            var resp = WebE.Response as HttpWebResponse;
            int respID = (int)resp.StatusCode;
            if (resp != null) {
                if (respID == 404) {
                    MessageBox.Show("404 at " + url + "\nThe item was not found.");
                }
                else if (respID == 403) {
                    MessageBox.Show("403 at " + url + "\nYou do not have access to this.");
                }
                else if (respID == 421) {
                    MessageBox.Show("421 at " + url + "\nYou are throttled. Try again later.");
                }
                else if (respID == 500) {
                    MessageBox.Show("500 at " + url + "\nAn error occured on the server. Try again later.");
                }
                else if (respID == 502) {
                    MessageBox.Show("502 at " + url + "\ne621 sent an invalid response. Try again later.");
                }
                else if (respID == 503) {
                    MessageBox.Show("503 at " + url + "\ne621 cannot handle your request or you have exceeded the request limit.\nTry again later, or decrease your downloads");
                }
                else {
                    MessageBox.Show(respID + " at " + url + "\nThe error is not documented in the source. It's either unrelated or not relevant.\nTry again, either now or later.");
                }
            }
        }
        #endregion

        private void startDownload() {
            poolDownload = new Thread(() => {
                string saveDir = Settings.Default.saveLocation;
                Thread.CurrentThread.IsBackground = true;
                if (fromURL) {
                    // extract pool id from url
                    poolurl = poolurl.Replace("http://", "https://");
                    if (!poolurl.StartsWith("https://")) {
                        poolurl = "https://" + poolurl;
                    }
                    string poolid = poolurl.Split('/')[5];
                    if (poolid.Contains("?")) {
                        poolid = poolid.Split('?')[0];
                    }
                    this.Invoke((MethodInvoker)(() => lbID.Text = "Pool ID " + poolid));
                    
                    if (downloadPool(poolid, saveDir, Settings.Default.saveInfo, Pools.Default.usePoolName, Program.UserAgent)) {
                        tmrTitle.Stop();
                        if (openAfter)
                            Process.Start(saveDir);

                        if (Settings.Default.ignoreFinish) {
                            this.DialogResult = DialogResult.OK;
                        }
                        else {
                            MessageBox.Show("Pool " + poolid + " has finished downloading.");
                        }
                    }
                    else {
                        this.DialogResult = DialogResult.Abort;
                    }
                }
                else {
                    if (downloadPool(id, saveDir, Settings.Default.saveInfo, Pools.Default.usePoolName, Program.UserAgent)) {
                        tmrTitle.Stop();
                        if (!Settings.Default.ignoreFinish)
                            MessageBox.Show("Pool " + id + " has finished downloading.");

                        if (openAfter)
                            Process.Start(saveDir);

                        if (Settings.Default.ignoreFinish) {
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                    else {
                        this.DialogResult = DialogResult.Abort;
                    }
                }
            });
            tmrTitle.Start();
            poolDownload.Start();
        }

        private void frmDownload_Load(object sender, EventArgs e) {
            this.lbPercentage.BackColor = System.Drawing.Color.Transparent;
            if (!fromURL) {
                lbID.Text = "Pool ID " + id;
            }
        }
        private void frmDownload_Shown(object sender, EventArgs e) {
            startDownload();
        }
        private void frmDownload_FormClosing(object sender, FormClosingEventArgs e) {
            poolDownload.Abort();
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
    }
}
