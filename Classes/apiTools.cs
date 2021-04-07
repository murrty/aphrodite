using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace aphrodite {
    class apiTools {
        public static readonly string EmptyXML = "<root type=\"array\"></root>";
        public static readonly string[] BadChars = { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };
        public static readonly string[] ReplacementChars = new string[] { "%5C", "%2F", "%3A", "%2A", "%3F", "%22", "%3C", "%3E", "%7C" };

        public static string GetJsonToXml(string JsonURL) {
            apiTools.SendDebugMessage("getJson starting");

            try {
                apiTools.SendDebugMessage("Downloading JSON at " + JsonURL);
                string JSONOutput = null;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(JsonURL);
                Request.UserAgent = Program.UserAgent;
                Request.Method = "GET";
                using (var Response = (HttpWebResponse)Request.GetResponse())
                using (var ResponseStream = Response.GetResponseStream())
                using (var Reader = new StreamReader(ResponseStream)) {
                    string JSONString = Reader.ReadToEnd();
                    byte[] JSONBytes = Encoding.ASCII.GetBytes(JSONString);
                    using (var MemoryStream = new MemoryStream(JSONBytes)) {
                        var Quotas = new XmlDictionaryReaderQuotas();
                        var JSONReader = JsonReaderWriterFactory.CreateJsonReader(MemoryStream, Quotas);
                        var XMLJSON = XDocument.Load(JSONReader);
                        JSONOutput = XMLJSON.ToString();
                    }
                }

                if (JSONOutput != null && JSONOutput != EmptyXML) {
                    return JSONOutput;
                }

                return null;
            }
            catch {
                throw;
            }
        }
        public static bool DownloadImage(string ImageURL, string SavePath) {
            try {
                using (ExWebClient WC = new ExWebClient()) {
                    WC.Proxy = WebRequest.GetSystemWebProxy();
                    WC.Method = "GET";
                    WC.Headers.Add("User-Agent: " + Program.UserAgent);
                    apiTools.SendDebugMessage("Downloading file " + ImageURL);
                    WC.DownloadFile(ImageURL, SavePath);
                }
                return true;
            }
            catch (ThreadAbortException) {
                throw;
            }
            catch (WebException) {
                throw;
            }
            catch (Exception) {
                throw;
            }
        }
        public static bool IsXmlDead(string xml) {
            if (string.IsNullOrWhiteSpace(xml) || xml == EmptyXML)
                return true;
            else
                return false;
        }

        public static void SendDebugMessage(string message) {
            Debug.Print(message);
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
            return "https://static1.e621.net/data/" + md5.Substring(0, 2) + "/" + md5.Substring(2, 2) + "/" + md5 + "." + ext;
        }

        public static bool GetTagsFromUrl(string url, out string output) {
            output = null;

            url = SetUrlString(url);

            if (!IsValidE621Link(url)) {
                return false;
            }
            try {
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
                            url = urls[i].Replace("tags=", "").Replace("+", " ");
                            break;
                        }
                    }
                }
                output = url;
                return true;
            }
            catch {
                throw;
            }
        }
        public static string GetPoolIdFromUrl(string url) {
            try {
                string output = null;

                if (url != null) {
                    url = SetUrlString(url);

                    if (IsValidE621Link(url)) {
                        url = url.Split('/')[4];
                        if (url.Contains('?')) {
                            url = url.Split('?')[0];
                        }

                        output = url;
                    }
                }

                return output;
            }
            catch {
                throw;
            }
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

            if (url.IndexOf("https://e621.net/") == 0) {
                return true;
            }
            else {
                return false;
            }
        }
        public static bool IsValidPoolLink(string url) {
            url = SetUrlString(url);

            if (!IsValidE621Link(url)) {
                return false;
            }

            if (url.IndexOf("https://e621.net/pools/") == 0){
                return true;
            }
            else {
                return false;
            }
        }
        public static bool IsValidPageLink(string url) {
            url = SetUrlString(url);

            if (!IsValidE621Link(url)) {
                return false;
            }

            if (url.IndexOf("https://e621.net/posts") == 0 || url.IndexOf("https://e621.net/posts?tags=") == 0) {
                return true;
            }
            else {
                return false;
            }
        }
        public static bool IsValidImageLink(string url) {
            url = SetUrlString(url);

            if (!IsValidE621Link(url)) {
                return false;
            }

            if (url.IndexOf("https://e621.net/posts/") == 0) {
                return true;
            }
            else {
                return false;
            }
        }

        public static string SetUrlString(string input) {
            if (input.StartsWith("http://")) {
                input = input.Replace("http://", "https://");
            }
            if (!input.StartsWith("https://")) {
                input = "https://" + input;
            }

            return input.Replace("https://www.", "https://");
        }
        #endregion

    }

    [Serializable]
    public class ApiReturnedNullOrEmptyException : Exception {
        public static string ReportedEmpty = "API was reported as empty.";
        public static string ReportedDead = "API was reported as dead.";
        public ApiReturnedNullOrEmptyException () { }

        public ApiReturnedNullOrEmptyException (string message) : base(message) { }

        public ApiReturnedNullOrEmptyException(string message, Exception inner) : base (message, inner) { }
    }
    [Serializable]
    public class ApiReturnedNullException : Exception {
        public ApiReturnedNullException () { }
        public ApiReturnedNullException(string message) : base(message) { }
        public ApiReturnedNullException(string message, Exception inner) : base(message, inner) { }
    }


}