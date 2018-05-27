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
        public string tags = string.Empty;
        public bool openAfter = false;
        public int minimumScore = 0;
        public bool useMinimumScore = false;
        public bool scoreAsTag = false;
        public int imageAmount = 0;
        public bool usePageLimit = false;
        public int pageLimit = 0;
        public bool saveInfo = false;
        public string blacklistedTags = string.Empty;
        public string zblacklistedTags = string.Empty;
        public string saveTo = Settings.Default.saveLocation + "\\Tags";
        public string[] ratings = null;
        public bool separateRatings = true;

        public bool abortThread = false;

        public bool fromURL = false;
        public string url = string.Empty;
        Thread tagDownload;
        #endregion

        public frmTagDownloader() {
            InitializeComponent();
        }
        private void frmDownload_Load(object sender, EventArgs e) {
            this.lbPercentage.BackColor = System.Drawing.Color.Transparent;
        }
        private void frmDownload_Shown(object sender, EventArgs e) {
            if (fromURL) {
                txtTags.Text = url.Split('/')[6].Replace("%20", " ");
            }
            else {
                txtTags.Text = tags.Replace("%25-2F", "/");
            }
            string min = "disabled";
            if (useMinimumScore) {
                min = (minimumScore).ToString();
            }
            lbBlacklist.Text = "0 posts (0 e, 0 q, 0 s)\n0 blacklisted (0 e,0 q, 0 s)\n0 zero-toleranced (skipped)\n0 in total" + "\n\nScore minimum: " + min;
            startDownload();
        }
        private void frmDownload_FormClosing(object sender, FormClosingEventArgs e) {
            tagDownload.Abort();
            abortThread = true;
            this.Dispose();
        }

        #region downloader
        public static readonly string tagJson = "https://e621.net/post/index.json?tags=";
        public static readonly string pageJson = "&page=";
        public static readonly string emptyXML = "<root type=\"array\"></root>";

        private string getJSON(string url, string header) {
            Debug.Print("getJson starting");

            if (!header.StartsWith("User-Agent: ")) {
                header = "User-Agent: " + header;
                Debug.Print("Set header to " + header);
            }

            try {
                Debug.Print("Starting tag json download");
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
            catch (ObjectDisposedException disEx) {
                Debug.Print("Seems like the object got disposed.");
                Debug.Print("==========BEGIN OBJDISPOSEDEXCEPTION==========");
                Debug.Print(disEx.ToString());
                Debug.Print("==========END OBJDISPOSEDEXCEPTION==========");
                return null;
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

        private bool downloadTags(bool isUrl = false, string url = "") {
            string dlURL = string.Empty;
            string header = Program.UserAgent;
            Debug.Print("downloadTags starting");
            this.Invoke((MethodInvoker)(() => status.Text = "Preparing to download..."));

            // Set the blacklist list
            string tagName = tags;
            List<string> blacklist = new List<string>();
            List<string> blacklistedURLS = new List<string>();
            List<string> zblacklist = new List<string>();
            List<string> rExplicit = new List<string>();
            List<string> rQuestionable = new List<string>();
            List<string> rSafe = new List<string>();
            List<string> rbExplicit = new List<string>();
            List<string> rbQuestionable = new List<string>();
            List<string> rbSafe = new List<string>();
            int blacklistCount = 0;
            int skippedposts = 0;
            string[] tagLength;
            string pagestr = string.Empty;
            List<string> urls = new List<string>();
            string taginfo = string.Empty;
            string blacklistinfo = string.Empty;
            if (useMinimumScore) {
                taginfo = "TAGS: " + tags + "\nMINIMUM SCORE: " + minimumScore + "\n\n";
                blacklistinfo = "TAGS: " + tags + "\nBLACKLISTED TAGS: " + blacklistedTags + "\nMINIMUM SCORE: " + minimumScore + "\n\n";
            }
            else {
                taginfo = "TAGS: " + tags + "\nMINIMUM SCORE: n/a\n\n";
                blacklistinfo = "TAGS: " + tags + "\nBLACKLISTED TAGS: " + blacklistedTags + "\nMINIMUM SCORE: n/a\n\n";
            }
            tagLength  = tags.Split(' ');
            if (tagLength.Length > 6){
                MessageBox.Show("6 tags is the maximum length you're allowed to download from e621. If your tag has a space between words, be sure to add an underscore. (_)");
                return false;
            }

            if (tagLength.Length < 6 && scoreAsTag) {
                tags = tags + " score:>" + (minimumScore - 1);
            }

            if (!string.IsNullOrEmpty(blacklistedTags) && !string.IsNullOrWhiteSpace(blacklistedTags))
                blacklist = new List<string>(blacklistedTags.Split(' '));

            if (!string.IsNullOrEmpty(zblacklistedTags) && !string.IsNullOrWhiteSpace(zblacklistedTags))
                zblacklist = new List<string>(zblacklistedTags.Split(' '));

            if (!this.saveTo.EndsWith("\\Tags"))
                this.saveTo += "\\Tags";

            string[] badchars = new string[] { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };
            string[] newchars = new string[] { "%5C", "%2F", "%3A", "%2A", "%3F", "%22", "%3C", "%3E", "%7C" };

            // \ = %5C
            // / = %2F
            // : = %3A
            // * = %2A
            // ? = %3F
            // " = %22
            // < = %3C
            // > = %3E
            // | = %7C
            // (space) = %20

            Debug.Print("Changing output directory to " + this.saveTo + "\\" + tagName);
            string outputFolderName = tagName.Replace("%20", " ");
            for (int i = 0; i < badchars.Length; i++) {
                outputFolderName = outputFolderName.Replace(badchars[i], newchars[i]);
            }
            this.saveTo += "\\" + outputFolderName;
            if (useMinimumScore) {
                this.saveTo += " (scores " + (minimumScore) + "+)";
            }
            
            try {
                if (isUrl) {
                    // get the page json from e621
                    Debug.Print("Calling getJson for " + tags);
                    //5 page, 6 tags
                    pagestr = url.Split('/')[5];
                    dlURL = tagJson + tags + pageJson + pagestr;
                    string xml = getJSON(tagJson + tags + pageJson + pagestr, header);
                    taginfo = "Tags: " + tags.Replace("%20", " ") + "\n\n";
                    blacklistinfo = "Tags: " + tags.Replace("%20", " ") + "\nBlacklisted tags: " + blacklistedTags + "\n\n";

                    if (xml == emptyXML || string.IsNullOrWhiteSpace(xml)) {
                        Debug.Print("xml is empty, aborting");
                        this.Invoke((MethodInvoker)(() => status.Text = "The tag's json returned empty or null. Aborting."));
                        return false;
                    }

                    Debug.Print("Loading xml");
                    this.Invoke((MethodInvoker)(() => status.Text = "Parsing json..."));
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);

                    Debug.Print("Gathering list of URLs from the page");
                    XmlNodeList xmlMD5 = doc.DocumentElement.SelectNodes("/root/item/md5");
                    XmlNodeList xmlID = doc.DocumentElement.SelectNodes("/root/item/id");
                    XmlNodeList xmlURL = doc.DocumentElement.SelectNodes("/root/item/file_url");
                    XmlNodeList xmlTags = doc.DocumentElement.SelectNodes("/root/item/tags");
                    XmlNodeList xmlArtist = doc.DocumentElement.SelectNodes("/root/item/artist");
                    XmlNodeList xmlScore = doc.DocumentElement.SelectNodes("/root/item/score");
                    XmlNodeList xmlRating = doc.DocumentElement.SelectNodes("/root/item/rating");
                    XmlNodeList xmlDescription = doc.DocumentElement.SelectNodes("/root/item/description");
                    Debug.Print("There are " + xmlURL.Count + " posts on the page");
                    for (int j = 0; j < xmlTags.Count; j++) {
                        string artists = string.Empty; //= xmlArtist[j].InnerXml.ToString().Replace("</item><item type=\"string\">", "\n               ").Replace("<item type=\"string\">", "").Replace("</item>", "");
                        string rating = xmlRating[j].InnerText;
                        bool blacklisted = false;
                        bool skip = false;
                        string foundblacklistedtags = string.Empty;
                        List<string> foundTags = xmlTags[j].InnerText.Split(' ').ToList();

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

                        for (int k = 0; k < xmlArtist[j].ChildNodes.Count; k++ ) {
                            artists += xmlArtist[j].ChildNodes[k].InnerText + "\n               ";
                        }

                        if (imageAmount > 0 && urls.Count == imageAmount)
                            break;

                        for (int k = 0; k < foundTags.Count; k++) {
                            if (zblacklist.Count > 0) {
                                for (int l = 0; l < zblacklist.Count; l++) {
                                    if (foundTags[k] == zblacklist[l]) {
                                        skippedposts++;
                                        skip = true;
                                        break;
                                    }
                                    if (skip)
                                        break;
                                }
                            }

                            if (skip)
                                break;

                            if (blacklist.Count > 0) {
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
                                Debug.Print("Blacklisted tag found, adding to the blacklist. Offending tag: " + foundblacklistedtags + " on id " + xmlID[j].InnerText);
                                blacklistCount++;
                            }
                        }

                        if (skip)
                            continue;

                        if (blacklisted) {
                            if (Settings.Default.saveBlacklisted) {
                                if (useMinimumScore && Int32.Parse(xmlScore[j].InnerText) < minimumScore) {
                                    continue;
                                }
                                else {
                                    if (!ratings.Any(xmlRating[j].InnerText.Contains)) {
                                        continue;
                                    }
                                }
                            }
                        }
                        else {
                            if (useMinimumScore && Int32.Parse(xmlScore[j].InnerText) < minimumScore) {
                                continue;
                            }
                            else {
                                if (!ratings.Any(xmlRating[j].InnerText.Contains)) {
                                    continue;
                                }
                            }
                        }

                        //for (int k = 0; k < xmlArtist.Count; k++) {
                        //    artists += xmlArtist[k].InnerText + "\n               ";
                        //}
                        //artists = artists.TrimEnd(' ');
                        //artists = artists.TrimEnd('\n');

                        if (!blacklisted) {
                            if (separateRatings) {
                                if (xmlRating[j].InnerText == "e") {
                                    rExplicit.Add(xmlURL[j].InnerText);
                                }
                                else if (xmlRating[j].InnerText == "q") {
                                    rQuestionable.Add(xmlURL[j].InnerText);
                                }
                                else if (xmlRating[j].InnerText == "s") {
                                    rSafe.Add(xmlURL[j].InnerText);
                                }

                                urls.Add(xmlURL[j].InnerText);
                            }
                            else {
                                urls.Add(xmlURL[j].InnerText);
                            }
                            taginfo += "POST " + xmlID[j].InnerText + ":\n    MD5: " + xmlMD5[j].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[j].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[j].InnerText + "\n    SCORE: " + xmlScore[j].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[j].InnerText + "\"\n\n";
                        }
                        else if (blacklisted) {
                            if (separateRatings) {
                                if (xmlRating[j].InnerText == "e") {
                                    rbExplicit.Add(xmlURL[j].InnerText);
                                }
                                else if (xmlRating[j].InnerText == "q") {
                                    rbQuestionable.Add(xmlURL[j].InnerText);
                                }
                                else if (xmlRating[j].InnerText == "s") {
                                    rbSafe.Add(xmlURL[j].InnerText);
                                }
                                else {
                                    blacklistedURLS.Add(xmlURL[j].InnerText);
                                }
                            }
                            else {
                                blacklistedURLS.Add(xmlURL[j].InnerText);
                            }
                            blacklistinfo += "POST " + xmlID[j].InnerText + ":\n    MD5: " + xmlMD5[j].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[j].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[j].InnerText + "\n    SCORE: " + xmlScore[j].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[j].InnerText + "\"\n    OFFENDING TAGS: " + foundblacklistedtags + "\n\n";
                        }

                        artists = string.Empty;

                        if (imageAmount > 0 && urls.Count == imageAmount) {
                            break;
                        }
                    }
                }
                else {
                    if (!tags.Contains("&limit=")) {
                        tags += "&limit=320";
                        Debug.Print("Set the limit to 320, one was not specified before");
                    }

                    dlURL = tagJson + tags;
                    Debug.Print("Calling getJson for " + tags);
                    this.Invoke((MethodInvoker)(() => status.Text = "Downloading tag information for page 1..."));

                    string xml = getJSON(tagJson + tags, header);
                    if (xml == emptyXML || string.IsNullOrWhiteSpace(xml)) {
                        Debug.Print("xml is empty, aborting");
                        this.Invoke((MethodInvoker)(() => status.Text = "The tag's json returned empty or null. Aborting."));
                        return false;
                    }

                    Debug.Print("Loading xml");
                    this.Invoke((MethodInvoker)(() => status.Text = "Parsing json..."));
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);
                    XmlNodeList xmlID = doc.DocumentElement.SelectNodes("/root/item/id"); // ID of each item

                    int count = xmlID.Count;
                    int page = 1;
                    if (count >= 320) {
                        Debug.Print("Multiple pages are possible");
                        page++;
                    }

                    Debug.Print("Gathering list of URLs from the page");
                    XmlNodeList xmlMD5 = doc.DocumentElement.SelectNodes("/root/item/md5");
                    XmlNodeList xmlURL = doc.DocumentElement.SelectNodes("/root/item/file_url");
                    XmlNodeList xmlTags = doc.DocumentElement.SelectNodes("/root/item/tags");
                    XmlNodeList xmlArtist = doc.DocumentElement.SelectNodes("/root/item/artist");
                    XmlNodeList xmlScore = doc.DocumentElement.SelectNodes("/root/item/score");
                    XmlNodeList xmlRating = doc.DocumentElement.SelectNodes("/root/item/rating");
                    XmlNodeList xmlDescription = doc.DocumentElement.SelectNodes("/root/item/description");
                    Debug.Print("There are " + xmlURL.Count + " posts on the page");
                    for (int j = 0; j < xmlTags.Count; j++) {
                        string artists = string.Empty; //= xmlArtist[j].InnerXml.ToString().Replace("</item><item type=\"string\">", "\n               ").Replace("<item type=\"string\">", "").Replace("</item>", "");
                        string rating = xmlRating[j].InnerText;
                        bool blacklisted = false;
                        bool skip = false;
                        string foundblacklistedtags = string.Empty;
                        List<string> foundTags = xmlTags[j].InnerText.Split(' ').ToList();
                        
                        if (imageAmount > 0 && urls.Count == imageAmount)
                            break;

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
                            artists += xmlArtist[j].ChildNodes[k].InnerText + "\n               ";
                        }

                        for (int k = 0; k < foundTags.Count; k++) {
                            if (zblacklist.Count > 0) {
                                for (int l = 0; l < zblacklist.Count; l++) {
                                    if (foundTags[k] == zblacklist[l]) {
                                        skippedposts++;
                                        skip = true;
                                        break;
                                    }
                                    if (skip)
                                        break;
                                }
                            }

                            if (skip)
                                break;

                            if (blacklist.Count > 0) {
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
                                Debug.Print("Blacklisted tag found, adding to the blacklist. Offending tag: " + foundblacklistedtags + " on id " + xmlID[j].InnerText);
                                blacklistCount++;
                            }
                        }

                        if (skip)
                            continue;

                        if (blacklisted) {
                            if (Settings.Default.saveBlacklisted) {
                                if (useMinimumScore && Int32.Parse(xmlScore[j].InnerText) < minimumScore) {
                                    continue;
                                }
                                else {
                                    if (!ratings.Any(xmlRating[j].InnerText.Contains)) {
                                        continue;
                                    }
                                }
                            }
                        }
                        else {
                            if (useMinimumScore && Int32.Parse(xmlScore[j].InnerText) < minimumScore) {
                                continue;
                            }
                            else {
                                if (!ratings.Any(xmlRating[j].InnerText.Contains)) {
                                    continue;
                                }
                            }
                        }

                        //XmlNodeList xmlArtistsToo = doc.DocumentElement.SelectNodes("/root/" + (j) + "/artists/item");
                        //for (int k = 0; k < xmlArtistsToo.Count; k++) {
                        //    artists += xmlArtistsToo[k].InnerText + "\n               ";
                        //}
                        //artists = artists.TrimEnd(' ');
                        //artists = artists.TrimEnd('\n');

                        if (!blacklisted) {
                            if (separateRatings) {
                                if (xmlRating[j].InnerText == "e") {
                                    rExplicit.Add(xmlURL[j].InnerText);
                                }
                                else if (xmlRating[j].InnerText == "q") {
                                    rQuestionable.Add(xmlURL[j].InnerText);
                                }
                                else if (xmlRating[j].InnerText == "s") {
                                    rSafe.Add(xmlURL[j].InnerText);
                                }

                                urls.Add(xmlURL[j].InnerText);
                            }
                            else {
                                urls.Add(xmlURL[j].InnerText);
                            }
                            taginfo += "POST " + xmlID[j].InnerText + ":\n    MD5: " + xmlMD5[j].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[j].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[j].InnerText + "\n    SCORE: " + xmlScore[j].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[j].InnerText + "\"\n\n";
                        }
                        else if (blacklisted) {
                            if (separateRatings) {
                                if (xmlRating[j].InnerText == "e") {
                                    rbExplicit.Add(xmlURL[j].InnerText);
                                }
                                else if (xmlRating[j].InnerText == "q") {
                                    rbQuestionable.Add(xmlURL[j].InnerText);
                                }
                                else if (xmlRating[j].InnerText == "s") {
                                    rbSafe.Add(xmlURL[j].InnerText);
                                }
                                else {
                                    blacklistedURLS.Add(xmlURL[j].InnerText);
                                }
                            }
                            else {
                                blacklistedURLS.Add(xmlURL[j].InnerText);
                            }
                            blacklistinfo += "POST " + xmlID[j].InnerText + ":\n    MD5: " + xmlMD5[j].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[j].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[j].InnerText + "\n    SCORE: " + xmlScore[j].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[j].InnerText + "\"\n    OFFENDING TAGS: " + foundblacklistedtags + "\n\n";
                        }

                        if (imageAmount > 0 && urls.Count == imageAmount) {
                            break;
                        }
                    }

                    if (page > 1) {
                        Debug.Print("Gathering list of URLs from each page");
                        bool PageDead = false;
                        while (PageDead == false) {
                            this.Invoke((MethodInvoker)(() => status.Text = "Downloading tag information for page " + (page) + "..."));
                            dlURL = tagJson + tags + pageJson + page;
                            xml = getJSON(tagJson + tags + pageJson + page, header);
                            page++;

                            if (usePageLimit && page == pageLimit + 1)
                                break;

                            if (xml == emptyXML) {
                                PageDead = true;
                                break;
                            }
                            else {
                                doc.LoadXml(xml);
                                xmlID = doc.DocumentElement.SelectNodes("/root/item/id");
                                xmlMD5 = doc.DocumentElement.SelectNodes("/root/item/md5");
                                xmlURL = doc.DocumentElement.SelectNodes("/root/item/file_url");
                                xmlTags = doc.DocumentElement.SelectNodes("/root/item/tags");
                                xmlArtist = doc.DocumentElement.SelectNodes("/root/item/artist");
                                xmlScore = doc.DocumentElement.SelectNodes("/root/item/score");
                                xmlRating = doc.DocumentElement.SelectNodes("/root/item/rating");
                                xmlDescription = doc.DocumentElement.SelectNodes("/root/item/description");
                                Debug.Print("There are " + xmlURL.Count + " posts on page " + page);
                                for (int j = 0; j < xmlTags.Count; j++) {
                                    string artists = string.Empty; //= xmlArtist[j].InnerXml.ToString().Replace("</item><item type=\"string\">", "\n               ").Replace("<item type=\"string\">", "").Replace("</item>", "");
                                    string rating = xmlRating[j].InnerText;
                                    bool blacklisted = false;
                                    bool skip = false;
                                    string foundblacklistedtags = string.Empty;
                                    List<string> foundTags = xmlTags[j].InnerText.Split(' ').ToList();

                                    if (imageAmount > 0 && urls.Count == imageAmount)
                                        break;

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
                                        artists += xmlArtist[j].ChildNodes[k].InnerText + "\n               ";
                                    }


                                    for (int k = 0; k < foundTags.Count; k++) {
                                        if (zblacklist.Count > 0) {
                                            for (int l = 0; l < zblacklist.Count; l++) {
                                                if (foundTags[k] == zblacklist[l]) {
                                                    skippedposts++;
                                                    skip = true;
                                                    break;
                                                }
                                                if (skip)
                                                    break;
                                            }
                                        }

                                        if (skip)
                                            break;

                                        if (blacklist.Count > 0) {
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
                                            Debug.Print("Blacklisted tag found, adding to the blacklist. Offending tag: " + foundblacklistedtags + " on id " + xmlID[j].InnerText);
                                            blacklistCount++;
                                        }
                                    }

                                    if (skip)
                                        continue;

                                    if (blacklisted) {
                                        if (Settings.Default.saveBlacklisted) {
                                            if (useMinimumScore && Int32.Parse(xmlScore[j].InnerText) < minimumScore) {
                                                continue;
                                            }
                                            else {
                                                if (!ratings.Any(xmlRating[j].InnerText.Contains)) {
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                    else {
                                        if (useMinimumScore && Int32.Parse(xmlScore[j].InnerText) < minimumScore) {
                                            continue;
                                        }
                                        else {
                                            if (!ratings.Any(xmlRating[j].InnerText.Contains)) {
                                                continue;
                                            }
                                        }
                                    }

                                    //for (int k = 0; k < xmlArtist.Count; k++) {
                                    //    artists += xmlArtist[k].InnerText + "\n               ";
                                    //}
                                    //artists = artists.TrimEnd(' ');
                                    //artists = artists.TrimEnd('\n');

                                    if (!blacklisted) {
                                        if (separateRatings) {
                                            if (xmlRating[j].InnerText == "e") {
                                                rExplicit.Add(xmlURL[j].InnerText);
                                            }
                                            else if (xmlRating[j].InnerText == "q") {
                                                rQuestionable.Add(xmlURL[j].InnerText);
                                            }
                                            else if (xmlRating[j].InnerText == "s") {
                                                rSafe.Add(xmlURL[j].InnerText);
                                            }

                                            urls.Add(xmlURL[j].InnerText);
                                        }
                                        else {
                                            urls.Add(xmlURL[j].InnerText);
                                        }
                                        taginfo += "POST " + xmlID[j].InnerText + ":\n    MD5: " + xmlMD5[j].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[j].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[j].InnerText + "\n    SCORE: " + xmlScore[j].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[j].InnerText + "\"\n\n";
                                    }
                                    else if (blacklisted) {
                                        if (separateRatings) {
                                            if (xmlRating[j].InnerText == "e") {
                                                rbExplicit.Add(xmlURL[j].InnerText);
                                            }
                                            else if (xmlRating[j].InnerText == "q") {
                                                rbQuestionable.Add(xmlURL[j].InnerText);
                                            }
                                            else if (xmlRating[j].InnerText == "s") {
                                                rbSafe.Add(xmlURL[j].InnerText);
                                            }
                                            else {
                                                blacklistedURLS.Add(xmlURL[j].InnerText);
                                            }
                                        }
                                        else {
                                            blacklistedURLS.Add(xmlURL[j].InnerText);
                                        }
                                        blacklistinfo += "POST " + xmlID[j].InnerText + ":\n    MD5: " + xmlMD5[j].InnerText + "\n    URL: https://e621.net/post/show/" + xmlID[j].InnerText + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[j].InnerText + "\n    SCORE: " + xmlScore[j].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"" + xmlDescription[j].InnerText + "\"\n    OFFENDING TAGS: " + foundblacklistedtags + "\n\n";
                                    }

                                    if (imageAmount > 0 && urls.Count == imageAmount) {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }


                if (separateRatings) {
                    int total = 0;
                    if (Settings.Default.saveBlacklisted) {
                        total = rExplicit.Count + rQuestionable.Count + rSafe.Count + rbExplicit.Count + rbQuestionable.Count + rbSafe.Count;
                    }
                    else {
                        total = rExplicit.Count + rQuestionable.Count + rSafe.Count;
                    }
                    this.Invoke((MethodInvoker)(() => lbFile.Text = "File 0 of " + (total)));
                    this.Invoke((MethodInvoker)(() => lbBlacklist.Text = (rExplicit.Count + rQuestionable.Count + rSafe.Count) + " posts (" + (rExplicit.Count) + " e, " + (rQuestionable.Count) + " q, " + (rSafe.Count) + " s)\n" + (rbExplicit.Count + rbQuestionable.Count + rbSafe.Count) + " blacklisted (" + (rbExplicit.Count) + " e, " + (rbQuestionable.Count) + " q, " + (rbSafe.Count) + " s)\n" + (skippedposts) + " zero-toleranced (skipped)\n" + (rExplicit.Count + rQuestionable.Count + rSafe.Count + rbExplicit.Count + rbQuestionable.Count + rbSafe.Count + skippedposts) + " in total" + "\n\nscore minimum: " + (minimumScore)));
                }
                else {
                    this.Invoke((MethodInvoker)(() => lbFile.Text = "File 0 of " + urls.Count));
                    this.Invoke((MethodInvoker)(() => lbBlacklist.Text = (urls.Count) + " posts\n" + (blacklistCount) + " blacklisted\n" + (skippedposts) + " zero-toleranced (skipped)\n" + (urls.Count + blacklistCount + skippedposts) + " in total" + "\n\nscore minimum: " + (minimumScore)));
                }

                if (!Directory.Exists(saveTo)) {
                    Debug.Print("Save directory does not exist, creating...");
                    Directory.CreateDirectory(saveTo);
                    if (Settings.Default.saveBlacklisted && blacklistCount > 0 && !separateRatings)
                        Directory.CreateDirectory(saveTo + "\\blacklisted");
                }

                if (separateRatings) {
                    if (rExplicit.Count > 0 || rbExplicit.Count > 0) {
                        if (!Directory.Exists(saveTo + "\\explicit"))
                            Directory.CreateDirectory(saveTo + "\\explicit");
                    }
                    if (rQuestionable.Count > 0 || rbQuestionable.Count > 0) {
                        if (!Directory.Exists(saveTo + "\\questionable"))
                            Directory.CreateDirectory(saveTo + "\\questionable");
                    }
                    if (rSafe.Count > 0 || rbSafe.Count > 0) {
                        if (!Directory.Exists(saveTo + "\\safe"))
                            Directory.CreateDirectory(saveTo + "\\safe");
                    }

                    if (Settings.Default.saveBlacklisted) {
                        if (rbExplicit.Count > 0)
                            Directory.CreateDirectory(saveTo + "\\explicit\\blacklisted");
                        if (rbQuestionable.Count > 0)
                                Directory.CreateDirectory(saveTo + "\\questionable\\blacklisted");
                        if (rbSafe.Count > 0)
                            Directory.CreateDirectory(saveTo + "\\safe\\blacklisted");
                    }
                }

                if (saveInfo) {
                    taginfo.TrimEnd('\n');
                    Debug.Print("Saving tags.nfo");
                    this.Invoke((MethodInvoker)(() => status.Text = "Saving tags.nfo"));
                    File.WriteAllText(saveTo + "\\tags.nfo", taginfo, Encoding.UTF8);

                    if (Settings.Default.saveBlacklisted && blacklistCount > 0) {
                        blacklistinfo.TrimEnd('\n');
                        this.Invoke((MethodInvoker)(() => status.Text = "Saving tags.blacklisted.nfo"));
                        if (separateRatings)
                            File.WriteAllText(saveTo + "\\tags.blacklisted.nfo", blacklistinfo, Encoding.UTF8);
                        else
                            File.WriteAllText(saveTo + "\\blacklisted\\tags.blacklisted.nfo", blacklistinfo, Encoding.UTF8);
                    }
                }

                this.Invoke((MethodInvoker)(() => pbDownloadStatus.Style = ProgressBarStyle.Continuous));

                using (WebClient wc = new WebClient()) {
                    //bool sizeRecieved = false;
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
                    wc.Proxy = WebProxy.GetDefaultProxy();
                    wc.Headers.Add(header);
                    Debug.Print("Header set, starting download of all " + urls.Count + " posts.");

                    if (separateRatings) {
                        if (rExplicit.Count > 0) {
                            for (int y = 0; y < rExplicit.Count; y++) {
                                dlURL = rExplicit[y];
                                //wc.OpenRead(dlURL);
                                //Int64 size = Convert.ToInt64(wc.ResponseHeaders["Content-Length"]);
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading explicit file " + (y + 1) + " of " + (rExplicit.Count)));// + " (" + (size / 1024) + "kb)"));

                                string filename = dlURL.Split('/')[6];
                                if (!File.Exists(saveTo + "\\explicit\\" + filename)) {
                                    Debug.Print("Downloading " + dlURL + " as " + filename);
                                    this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + filename));
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(dlURL), saveTo + "\\explicit\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                    Debug.Print("Finished, saved at " + saveTo + "\\explicit\\" + filename);
                                }
                                else {
                                    Debug.Print("File " + filename + " exists, has it already been downloaded?");
                                    this.Invoke((MethodInvoker)(() => status.Text = filename + " already exists"));
                                }
                            }
                        }
                        if (rQuestionable.Count > 0) {
                            for (int y = 0; y < rQuestionable.Count; y++) {
                                dlURL = rQuestionable[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading questionable file " + (y + 1) + " of " + (rQuestionable.Count)));

                                string filename = dlURL.Split('/')[6];
                                if (!File.Exists(saveTo + "\\questionable\\" + filename)) {
                                    Debug.Print("Downloading " + dlURL + " as " + filename);
                                    this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + filename));
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(dlURL), saveTo + "\\questionable\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                    Debug.Print("Finished, saved at " + saveTo + "\\questionable\\" + filename);
                                }
                                else {
                                    Debug.Print("File " + filename + " exists, has it already been downloaded?");
                                }
                            }
                        }
                        if (rSafe.Count > 0) {
                            for (int y = 0; y < rSafe.Count; y++) {
                                dlURL = rSafe[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading safe file " + (y + 1) + " of " + (rSafe.Count)));

                                string filename = dlURL.Split('/')[6];
                                if (!File.Exists(saveTo + "\\safe\\" + filename)) {
                                    Debug.Print("Downloading " + dlURL + " as " + filename);
                                    this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + filename));
                                    var sync = new Object();
                                    lock (sync) {
                                        wc.DownloadFileAsync(new Uri(dlURL), saveTo + "\\safe\\" + filename, sync);
                                        Monitor.Wait(sync);
                                    }
                                    Debug.Print("Finished, saved at " + saveTo + "\\safe\\" + filename);
                                }
                                else {
                                    Debug.Print("File " + filename + " exists, has it already been downloaded?");
                                }
                            }
                        }

                        if (Settings.Default.saveBlacklisted) {
                            if (rbExplicit.Count > 0) {
                                for (int y = 0; y < rbExplicit.Count; y++) {
                                    dlURL = rbExplicit[y];
                                    this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (e) file " + (y + 1) + " of " + (rbExplicit.Count)));

                                    string filename = dlURL.Split('/')[6];
                                    if (!File.Exists(saveTo + "\\explicit\\blacklisted\\" + filename)) {
                                        Debug.Print("Downloading " + dlURL + " as " + filename);
                                        this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + filename));
                                        var sync = new Object();
                                        lock (sync) {
                                            wc.DownloadFileAsync(new Uri(dlURL), saveTo + "\\explicit\\blacklisted\\" + filename, sync);
                                            Monitor.Wait(sync);
                                        }
                                        Debug.Print("Finished, saved at " + saveTo + "\\explicit\\blacklisted\\" + filename);
                                    }
                                    else {
                                        Debug.Print("File " + filename + " exists, has it already been downloaded?");
                                    }
                                }
                            }

                            if (rbQuestionable.Count > 0) {
                                for (int y = 0; y < rbQuestionable.Count; y++) {
                                    dlURL = rbQuestionable[y];
                                    this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (q) file " + (y + 1) + " of " + (rbQuestionable.Count)));

                                    string filename = dlURL.Split('/')[6];
                                    if (!File.Exists(saveTo + "\\questionable\\blacklisted\\" + filename)) {
                                        Debug.Print("Downloading " + dlURL + " as " + filename);
                                        this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + filename));
                                        var sync = new Object();
                                        lock (sync) {
                                            wc.DownloadFileAsync(new Uri(dlURL), saveTo + "\\questionable\\blacklisted\\" + filename, sync);
                                            Monitor.Wait(sync);
                                        }
                                        Debug.Print("Finished, saved at " + saveTo + "\\questionable\\blacklisted\\" + filename);
                                    }
                                    else {
                                        Debug.Print("File " + filename + " exists, has it already been downloaded?");
                                    }
                                }
                            }
                            if (rbSafe.Count > 0) {
                                for (int y = 0; y < rbSafe.Count; y++) {
                                    dlURL = rbSafe[y];
                                    this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted (s) file " + (y + 1) + " of " + (rbSafe.Count)));

                                    string filename = dlURL.Split('/')[6];
                                    if (!File.Exists(saveTo + "\\safe\\blacklisted\\" + filename)) {
                                        Debug.Print("Downloading " + dlURL + " as " + filename);
                                        this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + filename));
                                        var sync = new Object();
                                        lock (sync) {
                                            wc.DownloadFileAsync(new Uri(dlURL), saveTo + "\\safe\\blacklisted\\" + filename, sync);
                                            Monitor.Wait(sync);
                                        }
                                        Debug.Print("Finished, saved at " + saveTo + "\\safe\\blacklisted\\" + filename);
                                    }
                                    else {
                                        Debug.Print("File " + filename + " exists, has it already been downloaded?");
                                    }
                                }
                            }
                        }
                    }
                    else {
                        for (int y = 0; y < urls.Count; y++) {
                            dlURL = urls[y];
                            this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading file " + (y + 1) + " of " + (urls.Count)));

                            string filename = dlURL.Split('/')[6];
                            if (!File.Exists(saveTo + "\\" + filename)) {
                                Debug.Print("Downloading " + dlURL + " as " + filename);
                                this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + filename));
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

                        if (Settings.Default.saveBlacklisted && blacklistedURLS.Count > 0) {
                            if (!Directory.Exists(saveTo + "\\blacklisted")) {
                                Debug.Print("Blacklist save directory does not exist, creating...");
                                Directory.CreateDirectory(saveTo + "\\blacklisted");
                            }
                            for (int y = 0; y < blacklistedURLS.Count; y++) {
                                dlURL = blacklistedURLS[y];
                                this.Invoke((MethodInvoker)(() => lbFile.Text = "Downloading blacklisted file " + (y) + " of " + (blacklistedURLS.Count)));

                                string filename = dlURL.Split('/')[6];
                                if (!File.Exists(saveTo + "\\blacklisted\\" + filename)) {
                                    Debug.Print("Downloading " + dlURL + " as " + filename);
                                    this.Invoke((MethodInvoker)(() => status.Text = "Downloading " + filename));
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
                        }
                    }
                }

                int imageCount = 0;
                if (separateRatings) {
                    imageCount += (rExplicit.Count + rQuestionable.Count + rSafe.Count);
                    if (Settings.Default.saveBlacklisted)
                        imageCount += (rbExplicit.Count + rbQuestionable.Count + rbSafe.Count);
                }
                else {
                    imageCount += urls.Count;
                    if (Settings.Default.saveBlacklisted)
                        imageCount += blacklistedURLS.Count;
                }

                Debug.Print("Tags have been downloaded successfully, returning");

                this.BeginInvoke(new MethodInvoker(() => {
                    lbFile.Text = "All " + (imageCount) + " files downloaded.";
                    pbDownloadStatus.Value = 101;
                    lbPercentage.Text = "Done";
                    tmrTitle.Stop();
                    status.Text = "Finished downloading tags.";
                    this.Text = "Tags downloaded";
                }));
                return true;
            }
            //catch (InvalidOperationException IOex) {
            //    MessageBox.Show("Tricked");
            //    throw IOex;
            //}
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

        public static void webError(WebException WebE, string url = "Not defined.") {
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

            Debug.Print(url);
        }
        #endregion

        private void startDownload() {
            try {
                tagDownload = new Thread(() => {
                    string saveDir = Settings.Default.saveLocation;
                    Thread.CurrentThread.IsBackground = true;
                    if (fromURL) {
                        tags = url.Split('/')[6].TrimEnd('#');
                        if (downloadTags(true, url)) {
                            if (Settings.Default.ignoreFinish)
                                this.DialogResult = DialogResult.OK;
                        }
                    }
                    else {
                        if (downloadTags(false, string.Empty)) {
                            if (Settings.Default.ignoreFinish)
                                this.DialogResult = DialogResult.OK;
                        }
                    }
                });
                tagDownload.Start();
                tmrTitle.Start();
            }
            catch (ObjectDisposedException obDis) {
                MessageBox.Show("Caught");
            }
            
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