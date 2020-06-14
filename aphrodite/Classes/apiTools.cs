using System;
using System.Diagnostics;
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
    class apiTools {
        public static readonly string emptyXML = "<root type=\"array\"></root>";
        public static readonly string[] badChars = { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };
        public static readonly string[] replacementChars = new string[] { "%5C", "%2F", "%3A", "%2A", "%3F", "%22", "%3C", "%3E", "%7C" };
        public static string getJSON(string url, string header) {
            Debug.Print("getJson starting");

            if (!header.StartsWith("User-Agent: ")) {
                header = "User-Agent: " + header;
            }

            try {
                Debug.Print("Downloading JSON at " + url);
                using (ExWebClient wc = new ExWebClient()) {
                    wc.Proxy = WebRequest.GetSystemWebProxy();
                    wc.Headers.Add(header);
                    wc.Method = "GET";
                    string json = wc.DownloadString(url);
                    if (json == "{\"posts\":[]}") {
                        return null;
                    }
                    url = string.Empty;
                    byte[] bytes = Encoding.ASCII.GetBytes(json);
                    using (var stream = new MemoryStream(bytes)) {
                        var quotas = new XmlDictionaryReaderQuotas();
                        var jsonReader = JsonReaderWriterFactory.CreateJsonReader(stream, quotas);
                        var xml = XDocument.Load(jsonReader);
                        stream.Flush();
                        stream.Close();
                        if (xml != null && xml.ToString() != emptyXML) {
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
            catch (ThreadAbortException) {
                Debug.Print("Thread was requested to be aborted. (apiTools.cs)");
                return null;
            }
            catch (ObjectDisposedException) {
                Debug.Print("Seems like the object got disposed. (apiTools.cs)");
                return null;
            }
            catch (WebException WebE) {
                Debug.Print("A WebException has occured. (apiTools.cs)");
                ErrorLog.ReportException(WebE, "apiTools.cs");
                return null;
            }
            catch (Exception ex) {
                Debug.Print("A Exception has occured. (apiTools.cs)");
                ErrorLog.ReportException(ex, "apiTools.cs");
                return null;
            }
        }
        public static bool isXmlDead(string xml) {
            if (xml == null || xml == emptyXML)
                return true;
            else
                return false;
        }

        public static void debugMessage(string message) {
            Debug.Print(message);
        }

        public static string replaceIllegalCharacters(string input, bool WithCodes = true) {
            for (int i = 0; i < badChars.Length; i++) {
                if (WithCodes) {
                    input = input.Replace(badChars[i], replacementChars[i]);
                }
                else {
                    input = input.Replace(badChars[i], "_");
                }
            }
            return input;
        }

        public static int countFiles(string dir, SearchOption option = SearchOption.TopDirectoryOnly) {
            if (!Directory.Exists(dir))
                return 0;

            string[] files = Directory.EnumerateFiles(dir, "*", option).ToArray();
            return files.Length;
        }

        public static string getIdFromURL(string url) {
            url = setUrlString(url);

            if (!isValidE621Link(url))
                return null;

            return url.Split('/')[5];
        }

        public static string getBlacklistedImageUrl(string md5, string ext) {
            return "https://static1.e621.net/data/" + md5.Substring(0, 2) + "/" + md5.Substring(2, 2) + "." + ext;
        }

        public static int CountPoolPages(decimal PageCount) {
            return (int)Math.Ceiling(PageCount / 320);
        }

    #region Validation checks for e621
        public static bool isValidE621Link(string url) {
            url = setUrlString(url);

            if (url.IndexOf("https://e621.net/") == 0)
                return true;
            else
                return false;
        }
        public static bool isValidPoolLink(string url) {
            url = setUrlString(url);

            if (!isValidE621Link(url))
                return false;

            if (url.IndexOf("https://e621.net/pool/show/") == 0)
                return true;
            else
                return false;
        }
        public static bool isValidPageLink(string url) {
            url = setUrlString(url);

            if (!isValidE621Link(url))
                return false;

            if (url.IndexOf("https://e621.net/post/index/") == 0 || url.IndexOf("https://e621.net/post?tags=") == 0)
                return true;
            else
                return false;
        }
        public static bool isValidImageLink(string url) {
            url = setUrlString(url);

            if (!isValidE621Link(url))
                return false;

            if (url.IndexOf("https://e621.net/post/show/") == 0)
                return true;
            else
                return false;
        }

        public static string setUrlString(string input) {
            if (input.StartsWith("http://"))
                input = input.Replace("http://", "https://");
            if (!input.StartsWith("https://"))
                input = "https://" + input;
            return input.Replace("https://www.", "https://");
        }
    #endregion
    }
}
