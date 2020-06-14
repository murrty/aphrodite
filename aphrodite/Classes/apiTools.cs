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
        public static readonly string EmptyXML = "<root type=\"array\"></root>";
        public static readonly string[] BadChars = { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };
        public static readonly string[] ReplacementChars = new string[] { "%5C", "%2F", "%3A", "%2A", "%3F", "%22", "%3C", "%3E", "%7C" };

        public static string GetJSON(string url, string header) {
            apiTools.SendDebugMessage("getJson starting");

            if (!header.StartsWith("User-Agent: ")) {
                header = "User-Agent: " + header;
            }

            try {
                apiTools.SendDebugMessage("Downloading JSON at " + url);
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
                        if (xml != null && xml.ToString() != EmptyXML) {
                            apiTools.SendDebugMessage("Json converted, returning full json");
                            return xml.ToString();
                        }
                        else {
                            apiTools.SendDebugMessage("Json returned null, returning null");
                            return null;
                        }
                    }
                }
            }
            catch (ThreadAbortException) {
                apiTools.SendDebugMessage("Thread was requested to be aborted. (apiTools.cs)");
                return null;
            }
            catch (ObjectDisposedException) {
                apiTools.SendDebugMessage("Seems like the object got disposed. (apiTools.cs)");
                return null;
            }
            catch (WebException WebE) {
                apiTools.SendDebugMessage("A WebException has occured. (apiTools.cs)");
                ErrorLog.ReportException(WebE, "apiTools.cs");
                return null;
            }
            catch (Exception ex) {
                apiTools.SendDebugMessage("A Exception has occured. (apiTools.cs)");
                ErrorLog.ReportException(ex, "apiTools.cs");
                return null;
            }
        }
        public static bool IsXmlDead(string xml) {
            if (xml == null || xml == EmptyXML)
                return true;
            else
                return false;
        }

        public static void SendDebugMessage(string message) {
            if (Program.IsDebug) {
                Debug.Print(message);
            }
        }

        public static string ReplaceIllegalCharacters(string input, bool WithCodes = true) {
            for (int i = 0; i < BadChars.Length; i++) {
                if (WithCodes) {
                    input = input.Replace(BadChars[i], ReplacementChars[i]);
                }
                else {
                    input = input.Replace(BadChars[i], "_");
                }
            }
            return input;
        }

        public static int CountFiles(string dir, SearchOption option = SearchOption.TopDirectoryOnly) {
            if (!Directory.Exists(dir))
                return 0;

            string[] files = Directory.EnumerateFiles(dir, "*", option).ToArray();
            return files.Length;
        }

        public static string GetBlacklistedImageUrl(string md5, string ext) {
            return "https://static1.e621.net/data/" + md5.Substring(0, 2) + "/" + md5.Substring(2, 2) + "." + ext;
        }

        public static string GetTagsFromUrl(string url) {
            url = SetUrlString(url);

            if (!IsValidE621Link(url))
                return null;

            if (url.Contains("posts?tags=")) {
                url = url.Split('/')[3].Replace("posts?tags=", "");
                if (url.Contains('&')) {
                    url = url.Split('&')[0];
                }
            }
            else {
                string[] urls = url.Split('&');
                url = null;
                for (int i = 0; i < urls.Length; i++) {
                    if (urls[i].StartsWith("tags=")) {
                        url = urls[i].Replace("tags=", "");
                        break;
                    }
                }
            }

            return url;
        }
        public static string GetPoolIdFromUrl(string url) {
            url = SetUrlString(url);

            if (!IsValidE621Link(url)) {
                return null;
            }

            url = url.Split('/')[4];
            if (url.Contains('?')) {
                url = url.Split('?')[0];
            }

            return url;
        }
        public static string GetPostIdFromUrl(string url) {
            url = SetUrlString(url);

            if (!IsValidE621Link(url))
                return null;

            url = url.Split('/')[4];
            if (url.Contains('?')) {
                url = url.Split('?')[0];
            }

            return url;
        }

        public static int CountPoolPages(decimal PageCount) {
            return (int)Math.Ceiling(PageCount / 320);
        }

    #region Validation checks for e621
        public static bool IsValidE621Link(string url) {
            url = SetUrlString(url);

            if (url.IndexOf("https://e621.net/") == 0)
                return true;
            else
                return false;
        }
        public static bool IsValidPoolLink(string url) {
            url = SetUrlString(url);

            if (!IsValidE621Link(url))
                return false;

            if (url.IndexOf("https://e621.net/pools/") == 0)
                return true;
            else
                return false;
        }
        public static bool IsValidPageLink(string url) {
            url = SetUrlString(url);

            if (!IsValidE621Link(url))
                return false;

            if (url.IndexOf("https://e621.net/posts") == 0 || url.IndexOf("https://e621.net/posts?tags=") == 0)
                return true;
            else
                return false;
        }
        public static bool IsValidImageLink(string url) {
            url = SetUrlString(url);

            if (!IsValidE621Link(url))
                return false;

            if (url.IndexOf("https://e621.net/posts/") == 0)
                return true;
            else
                return false;
        }

        public static string SetUrlString(string input) {
            if (input.StartsWith("http://"))
                input = input.Replace("http://", "https://");
            if (!input.StartsWith("https://"))
                input = "https://" + input;
            return input.Replace("https://www.", "https://");
        }
    #endregion
    }
}