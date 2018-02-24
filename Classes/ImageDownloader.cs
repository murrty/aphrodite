using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    class ImageDownloader {
        #region Variables
        public string header = null;
        public string saveTo = Settings.Default.saveLocation + "\\Images";
        public bool saveInfo = false;

        public readonly string json = "https://e621.net/post/show.json?id=";
        #endregion

        #region Downloader
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
        
        public bool downloadImage(string url) {
            string dlURL = string.Empty;
            string postID = string.Empty;
            if (url.StartsWith("http://")) {
                url.Replace("http://", "https://");
            }
            if (!url.StartsWith("https://"))
                url = "https://" + url;

            postID = url.Split('/')[5];

            try {
                string imageinfo = string.Empty;
                Debug.Print("Calling getJson for image");
                dlURL = json + postID;
                string xml = getJSON(json + postID, header);

                if (xml == emptyXML || string.IsNullOrWhiteSpace(xml)) {
                    Debug.Print("xml is empty, aborting");
                    return false;
                }
                Debug.Print("Loading xml");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                Debug.Print("Gathering post information");
                XmlNodeList xmlMD5 = doc.DocumentElement.SelectNodes("/root/md5");
                XmlNodeList xmlURL = doc.DocumentElement.SelectNodes("/root/file_url");
                XmlNodeList xmlTags = doc.DocumentElement.SelectNodes("/root/tags");
                XmlNodeList xmlArtist = doc.DocumentElement.SelectNodes("/root/artist/item");
                XmlNodeList xmlScore = doc.DocumentElement.SelectNodes("/root/score");
                XmlNodeList xmlRating = doc.DocumentElement.SelectNodes("/root/rating");
                XmlNodeList xmlDescription = doc.DocumentElement.SelectNodes("/root/description");
                XmlNodeList xmlExt = doc.DocumentElement.SelectNodes("/root/file_ext");
                string artists = string.Empty;
                string rating = xmlRating[0].InnerText;
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

                for (int i = 0; i < xmlArtist.Count; i++) {
                    artists += xmlArtist[i].InnerText + "\n               ";
                }

                // Trims end of artist info
                artists = artists.TrimEnd(' ');
                artists = artists.TrimEnd('\n');

                imageinfo += "POST " + postID + ":\n    MD5: " + xmlMD5[0].InnerText + "\n    URL: https://e621.net/post/show/" + postID + "\n    ARTIST(S): " + artists + "\n    TAGS: " + xmlTags[0].InnerText + "\n    SCORE: " + xmlScore[0].InnerText + "\n    RATING: " + rating + "\n    DESCRIPITON:\n\"    " + xmlDescription[0].InnerText + "\"\n\n";

                imageinfo = imageinfo.TrimEnd('\n');

                if (!Directory.Exists(saveTo)) {
                    Debug.Print("Save directory does not exist, creating...");
                    Directory.CreateDirectory(saveTo);
                }

                if (Settings.Default.saveInfo) {
                    Debug.Print("Saving/writing images.nfo");
                    if (!File.Exists(saveTo + "\\images.nfo")) {
                        File.WriteAllText(saveTo + "\\images.nfo", imageinfo);
                    }
                    else {
                        imageinfo = "\n\n" + imageinfo;
                        File.AppendAllText(saveTo + "\\images.nfo", imageinfo);
                    }
                }

                if (Images.Default.separateRatings) {
                    if (xmlRating[0].InnerText == "e") {
                        if (!Directory.Exists(saveTo + "\\explicit"))
                            Directory.CreateDirectory(saveTo + "\\explicit");
                        saveTo += "\\explicit\\";
                    }
                    else if (xmlRating[0].InnerText == "q") {
                        if (!Directory.Exists(saveTo + "\\questionable"))
                            Directory.CreateDirectory(saveTo + "\\questionable");
                        saveTo += "\\questionable\\";
                    }
                    else if (xmlRating[0].InnerText == "s") {
                        if (!Directory.Exists(saveTo + "\\safe"))
                            Directory.CreateDirectory(saveTo + "\\safe");
                        saveTo += "\\safe\\";
                    }
                }

                string filename = string.Empty;
                if (Images.Default.artistMD5) {
                    filename = xmlArtist[0].InnerText + "_" + xmlMD5[0].InnerText + "." + xmlExt[0].InnerText;
                }
                else if (Images.Default.MD5) {
                    filename = xmlMD5[0].InnerText + "." + xmlExt[0].InnerText;
                }
                else {
                    filename = xmlMD5[0].InnerText + "." + xmlExt[0].InnerText;
                }

                using (WebClient wc = new WebClient()) {
                    wc.Headers.Add(header);
                    Debug.Print("Header set, starting download of image");

                    wc.DownloadFile(xmlURL[0].InnerText, saveTo + filename);
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
    }
}