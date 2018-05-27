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
    class apiTools {
        public static readonly string emptyXML = "<root type=\"array\"></root>";

        public static string getJSON(string url, string header) {
            Debug.Print("getJson starting");

            if (!header.StartsWith("User-Agent: ")) {
                header = "User-Agent: " + header;
            }

            try {
                Debug.Print("Starting tag json download");
                using (WebClient wc = new WebClient()) {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    wc.Proxy = WebRequest.GetSystemWebProxy();
                    wc.Headers.Add(header);
                    string json = wc.DownloadString(url);
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
            catch (ThreadAbortException thrEx) {
                Debug.Print("Thread was requested to be aborted.");
                Debug.Print("========== BEGIN THREADABORTEXCEPTION ==========");
                Debug.Print(thrEx.ToString());
                Debug.Print("========== END THREADABORTEXCEPTION ==========");
                return null;
                throw thrEx;
            }
            catch (ObjectDisposedException disEx) {
                Debug.Print("Seems like the object got disposed.");
                Debug.Print("========== BEGIN OBJDISPOSEDEXCEPTION ==========");
                Debug.Print(disEx.ToString());
                Debug.Print("========== END OBJDISPOSEDEXCEPTION ==========");
                return null;
            }
            catch (WebException WebE) {
                Debug.Print("A WebException has occured.");
                Debug.Print("========== BEGIN WEBEXCEPTION ==========");
                Debug.Print(WebE.ToString());
                Debug.Print("========== END WEBEXCEPTION ==========");
                webError(WebE, url);
                return null;
                throw WebE;
            }
            catch (Exception ex) {
                Debug.Print("A Exception has occured.");
                Debug.Print("========== BEGIN EXCEPTION ==========");
                Debug.Print(ex.ToString());
                Debug.Print("========== END EXCEPTION ==========");
                return null;
                throw ex;
            }
        }
        public static bool isXmlDead(string xml) {
            if (xml == null || xml == emptyXML)
                return true;
            else
                return false;
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

        public static void debugMessage(string message) {
            Debug.Print(message);
        }

        public static string replaceIllegalCharacters(string input) {
            string[] badChars = { "\\", "/", ":", "*", "?", "\"", "<", ">", "|" };
            for (int i = 0; i < badChars.Length; i++) {
                input = input.Replace(badChars[i], "_");
            }
            return input;
        }

    #region Validation checks
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
        public static bool isValidPostLink(string url) {
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
            return input.Replace("www.", "");
        }
    #endregion
    }
}
