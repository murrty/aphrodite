using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace aphrodite {

    public class ApiTools {

        /// <summary>
        /// Constant string for an exmpy xml.
        /// </summary>
        public const string EmptyXML = @"<root type=""array""></root>";
        /// <summary>
        /// Constant string for the regex matching page with tags urls.
        /// </summary>
        private const string PageWithTagsRegex = @"^(http(s)?:\/\/(www.)?)?e621.net\/posts(?!\/).*?(tags=)";
        /// <summary>
        /// Constant string for the regex matching page urls.
        /// </summary>
        private const string PageRegex = @"^(http(s)?:\/\/(www.)?)?e621.net\/posts(?!\/)";
        /// <summary>
        /// Constant string for the regex matching for pool urls.
        /// </summary>
        private const string PoolRegex = @"^(http(s)?:\/\/(www.)?)?e621.net\/pools\/[0-9]+";
        /// <summary>
        /// Constant string for the regex matching for image urls.
        /// </summary>
        private const string ImagePostRegex = @"^(http(s)?:\/\/(www.)?)?e621.net\/posts\/[0-9]+";
        /// <summary>
        /// Constant string for the regex matching inkbunny user favorites urls.
        /// </summary>
        private const string InkBunnyUserFavoritesRegex = @"^(http(s)?:\/\/(www.)?)?inkbunny.net\/(.*?)(user_id|favs_user_id)";
        /// <summary>
        /// Constant string for the regex matching for imgur album urls.
        /// </summary>
        private const string ImgurAlbumRegex = @"^(http(s)?:\/\/(www\.)?(i\.)?)?imgur.com\/a\/[a-zA-Z0-9]+";

        /// <summary>
        /// A read-only dictionary containing illegal characters with replacement.
        /// </summary>
        private static readonly Dictionary<string, string> IllegalCharacters = new() {
            { "\\", "%5C" },
            { "/",  "%2F" },
            { ":",  "%3A" },
            { "*",  "%2A" },
            { "?",  "%3F" },
            { "\"", "%22" },
            { "<",  "%3C" },
            { ">",  "%3E" },
            { "|",  "%7C" }
        };

        /// <summary>
        /// Downloads a Json API url and returns an xml-formatted string.
        /// </summary>
        /// <param name="JsonURL">The URL of the api url.</param>
        /// <returns>An xml-formatted string.</returns>
        public static string DownloadJsonToXml(string JsonURL) {
            try {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(JsonURL);
                Request.UserAgent = Program.UserAgent;
                Request.Method = "GET";

                using HttpWebResponse Response = (HttpWebResponse)Request.GetResponse();
                using Stream ResponseStream = Response.GetResponseStream();
                using StreamReader Reader = new(ResponseStream);

                return ConvertJsonToXml(Reader.ReadToEnd());
            }
            catch {
                throw;
            }
        }

        /// <summary>
        /// Converts a json string to xml.
        /// </summary>
        /// <param name="Json">The string-json to convert.</param>
        /// <returns>An json string converted to xml.</returns>
        public static string ConvertJsonToXml(string JsonString) =>
            XDocument.Load(
                JsonReaderWriterFactory.CreateJsonReader(
                    new MemoryStream(Encoding.UTF8.GetBytes(JsonString)),
                    new XmlDictionaryReaderQuotas()
                )
            ).ToString();

        /// <summary>
        /// Whether the xml is valid to be parsed.
        /// </summary>
        /// <param name="xml">The full xml string.</param>
        /// <returns>True if it's valid and can be parsed; otherwise, false.</returns>
        public static bool IsXmlDead(string xml) =>
            string.IsNullOrWhiteSpace(xml) || xml == EmptyXML;
        
        /// <summary>
        /// Replaces the bad chars in <paramref name="Input"/>. If <paramref name="WithCodes"/> is true, it'll replace it with html-safe names.
        /// </summary>
        /// <param name="Input">The input string to filter.</param>
        /// <param name="WithCodes">If true, the replaced characters will be html-safe; otherwise, it'll be an underscore.</param>
        /// <returns>A filtered string.</returns>
        public static string ReplaceIllegalCharacters(string Input, bool WithCodes = true) =>
            IllegalCharacters.Aggregate(Input, (BadChar, NewChar) => BadChar.Replace(NewChar.Key, WithCodes ? NewChar.Value : "_"));

        /// <summary>
        /// Counts files in a directory. Obsolete.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static int CountFiles(string dir, SearchOption option = SearchOption.TopDirectoryOnly) =>
            Directory.Exists(dir) ? Directory.EnumerateFiles(dir, "*", option).ToArray().Length : 0;

        /// <summary>
        /// Attempts to reverse-engineer a url based on MD5 and EXT of a file.
        /// Due to e621 adding a global blacklist, it is required to manually parse the URL by the MD5 and EXT of the file.
        /// It's currently one way to bypass this restriction, allowing to download files without authentication.
        /// </summary>
        /// <param name="md5">The MD5 hash of the file to derive the URL from.</param>
        /// <param name="ext">The EXT extension of the file to derive the URL from.</param>
        /// <returns>A formatted URL if valid; otherwise, null.</returns>
        public static string GetBlacklistedImageUrl(string md5, string ext) =>
            string.IsNullOrWhiteSpace(md5) || string.IsNullOrWhiteSpace(ext) ? null
            : "https://static1.e621.net/data/" + md5[..2] + "/" + md5[2..4] + "/" + md5 + "." + ext;

        /// <summary>
        /// Counts the api pages to parse through for a pool.
        /// </summary>
        /// <param name="PageCount">The amount of pages in the pool.</param>
        /// <returns>The count of api pages.</returns>
        public static int CountPoolPages(decimal PageCount) =>
            (int)Math.Ceiling(PageCount / 320);

        /// <summary>
        /// Gets a post id from a url of a image.
        /// </summary>
        /// <param name="url">The url of the image to extract the id from.</param>
        /// <returns>The post id if valid; otherwise, null.</returns>
        public static string GetPoolOrPostId(string url) =>
            !string.IsNullOrWhiteSpace(url) ? SetUrlString(url).Split('/')[4].Split('?')[0] : null;

        /// <summary>
        /// Whether the input value only contains numbers.
        /// </summary>
        /// <param name="Input">The input value to check.</param>
        /// <returns>True if <paramref name="Input"/> contains only numbers; otherwise, false.</returns>
        public static bool IsNumericOnly(string Input) =>
            Regex.IsMatch(Input, "^[0-9]+$");

        /// <summary>
        /// Whether the input <paramref name="url"/> is a valid page link that contains tags.
        /// </summary>
        /// <param name="url">The input link.</param>
        /// <returns>True if the <paramref name="url"/> is a valid page link; otherwise, false.</returns>
        public static bool IsValidPageWithTagsLink(string url) =>
            Regex.IsMatch(SetUrlString(url), PageWithTagsRegex);

        /// <summary>
        /// Whether the input <paramref name="url"/> is a valid page link.
        /// </summary>
        /// <param name="url">The input link.</param>
        /// <returns>True if the <paramref name="url"/> is a valid page link; otherwise, false.</returns>
        public static bool IsValidPageLink(string url) =>
            Regex.IsMatch(SetUrlString(url), PageRegex);

        /// <summary>
        /// Whether the input <paramref name="url"/> is a valid pool link.
        /// </summary>
        /// <param name="url">The input link.</param>
        /// <returns>True if the <paramref name="url"/> is a valid pool link; otherwise, false.</returns>
        public static bool IsValidPoolLink(string url) =>
            Regex.IsMatch(url, PoolRegex);

        /// <summary>
        /// Whether the input <paramref name="url"/> is a valid image link.
        /// </summary>
        /// <param name="url">The input link.</param>
        /// <returns>True if the <paramref name="url"/> is a valid image link; otherwise, false.</returns>
        public static bool IsValidImageLink(string url) =>
            Regex.IsMatch(url, ImagePostRegex);

        public static bool IsValidInkBunnyLink(string url) =>
            SetUrlString(url).IndexOf("https://inkbunny.net/") == 0;

        /// <summary>
        /// Whether the input link is a (possibly) valid inkbunny user link.
        /// </summary>
        /// <param name="url">The input link.</param>
        /// <returns>True if the <paramref name="url"/> is a valid image link; otherwise, false.</returns>
        public static bool IsValidInkBunnyUserLink(string url) {
            url = SetUrlString(url);
            return url.IndexOf("https://inkbunny.net/") == 0 && url.IndexOf("https://inkbunny.net/s/") != 0;
        }

        /// <summary>
        /// Whether the input link is a valid inkbunny user favorites link.
        /// </summary>
        /// <param name="url">The input link.</param>
        /// <returns>True if the <paramref name="url"/> is a valid image link; otherwise, false.</returns>
        public static bool IsValidInkBunnyUserFavoritesLink(string url) =>
            Regex.IsMatch(SetUrlString(url), InkBunnyUserFavoritesRegex);

        /// <summary>
        /// Whether the input url is a valid imgur album link.
        /// </summary>
        /// <param name="url">The input link.</param>
        /// <returns>True if the <paramref name="url"/> is a valid image link; otherwise, false.</returns>
        public static bool IsValidImgurLink(string url) =>
            Regex.IsMatch(SetUrlString(url), ImgurAlbumRegex);

        /// <summary>
        /// Modifies a link to always start with "https://" while also filtering out the "www." prefix from the domain url.
        /// </summary>
        /// <param name="input">The input link to filter.</param>
        /// <returns>A filtered link.</returns>
        public static string SetUrlString(string input) {
            input = input.Replace("http://", "https://");
            if (!input.StartsWith("https://")) {
                input = "https://" + input;
            }
            return input.Replace("https://www.", "https://").Replace("https://i.", "https://");
        }

    }

    /// <summary>
    /// Represents an error that occurs when the first (or an important page) of an API returns null, empty, or default.
    /// </summary>
    [Serializable]
    public class ApiReturnedNullOrEmptyException : Exception {
        public static string ReportedEmpty { get; private set; } = "API was reported as empty.";
        public static string ReportedDead { get; private set; } = "API was reported as dead.";
        public ApiReturnedNullOrEmptyException () { }
        public ApiReturnedNullOrEmptyException (string message) : base(message) { }
        public ApiReturnedNullOrEmptyException(string message, Exception inner) : base (message, inner) { }
    }

    /// <summary>
    /// Represents an error that occurs when the post, pool, or hosting position of the resource was deleted.
    /// </summary>
    [Serializable]
    public class PoolOrPostWasDeletedException : Exception {
        public PoolOrPostWasDeletedException() { }
        public PoolOrPostWasDeletedException(string message) : base(message) { }
        public PoolOrPostWasDeletedException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Represents an error that occurs when no files are queued to be downloaded.
    /// </summary>
    [Serializable]
    public class NoFilesToDownloadException : Exception {
        public NoFilesToDownloadException() { }
        public NoFilesToDownloadException(string message) : base(message) { }
        public NoFilesToDownloadException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Represents an errorthat occurs when bypassing a blacklist still returns an image as null or empty.
    /// </summary>
    [Serializable]
    public class ImageWasNullAfterBypassingException : Exception {
        public ImageWasNullAfterBypassingException() { }
        public ImageWasNullAfterBypassingException(string message) : base(message) { }
        public ImageWasNullAfterBypassingException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Represents an errorthat occurs when parsing arguments contains a valid URL, but the URL couldn't be parsed into the required information.
    /// </summary>
    [Serializable]
    public class ArgumentParsingUrlException : Exception {
        public string Url { get; private set; } = null;
        public ArgumentParsingUrlException() { }
        public ArgumentParsingUrlException(string message) : base(message) { }
        public ArgumentParsingUrlException(string message, string url) : base(message) { Url = url; }
        public ArgumentParsingUrlException(string message, Exception inner) : base(message, inner) { }
    }

}