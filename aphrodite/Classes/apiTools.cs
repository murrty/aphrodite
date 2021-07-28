using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace aphrodite {

    class apiTools {
        public static readonly string EmptyXML = "<root type=\"array\"></root>";
        public static readonly string[] BadChars = { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };
        public static readonly string[] ReplacementChars = new string[] { "%5C", "%2F", "%3A", "%2A", "%3F", "%22", "%3C", "%3E", "%7C" };

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string DownloadJsonToXml(string JsonURL) {
            try {
                string JSONOutput = null;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(JsonURL);
                Request.UserAgent = Program.UserAgent;
                Request.Method = "GET";
                using (var Response = (HttpWebResponse)Request.GetResponse())
                using (var ResponseStream = Response.GetResponseStream())
                using (var Reader = new StreamReader(ResponseStream)) {
                    JSONOutput = ConvertJsonToXml(Reader.ReadToEnd());
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

        public static string ConvertJsonToXml(string Json) {
            XDocument XMLJSON = XDocument.Load(
                JsonReaderWriterFactory.CreateJsonReader(
                    new MemoryStream(Encoding.UTF8.GetBytes(Json)),
                    new XmlDictionaryReaderQuotas()
                )
            );
            return XMLJSON.ToString();
        }

        public static bool IsXmlDead(string xml) {
            if (string.IsNullOrWhiteSpace(xml) || xml == EmptyXML)
                return true;
            else
                return false;
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

        public static int CountPoolPages(decimal PageCount) {
            return (int)Math.Ceiling(PageCount / 320);
        }

        public static string GetPoolOrPostId(string url) {
            return SetUrlString(url).Split('/')[4].Split('?')[0];
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
    public class PoolOrPostWasDeletedException : Exception {
        public PoolOrPostWasDeletedException() { }
        public PoolOrPostWasDeletedException(string message) : base(message) { }
        public PoolOrPostWasDeletedException(string message, Exception inner) : base(message, inner) { }
    }
    [Serializable]
    public class NoFilesToDownloadException : Exception {
        public NoFilesToDownloadException() { }
        public NoFilesToDownloadException(string message) : base(message) { }
        public NoFilesToDownloadException(string message, Exception inner) : base(message, inner) { }
    }
    [Serializable]
    public class ImageWasNullAfterBypassingException : Exception {
        public ImageWasNullAfterBypassingException() { }
        public ImageWasNullAfterBypassingException(string message) : base(message) { }
        public ImageWasNullAfterBypassingException(string message, Exception inner) : base(message, inner) { }
    }

}