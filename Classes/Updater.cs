using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace aphrodite {
    class Updater {

        public static string githubURL = "https://github.com/murrty/aphrodite";
        public static string githubJSON = "https://api.github.com/repos/murrty/aphrodite/releases/latest";

        public static string getJSON(string url) {
            try {
                using (WebClient wc = new WebClient()) {
                    wc.Headers.Add("User-Agent: " + Properties.Settings.Default.UserAgent);
                    string json = wc.DownloadString(url);
                    byte[] bytes = Encoding.ASCII.GetBytes(json);
                    using (var stream = new MemoryStream(bytes)) {
                        var quotas = new XmlDictionaryReaderQuotas();
                        var jsonReader = JsonReaderWriterFactory.CreateJsonReader(stream, quotas);
                        var xml = XDocument.Load(jsonReader);
                        stream.Flush();
                        stream.Close();
                        return xml.ToString();
                    }
                }
            }
            catch (WebException WebE) {
                Debug.Print(WebE.ToString());
                MessageBox.Show(WebE.ToString());
                return null;
                throw WebE;
            }
            catch (Exception ex) {
                Debug.Print(ex.ToString());
                MessageBox.Show(ex.ToString());
                return null;
                throw ex;
            }
        }

        public static decimal getCloudVersion() {
            try {
                string xml = getJSON(githubJSON);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNodeList xmlTag = doc.DocumentElement.SelectNodes("/root/tag_name");

                return decimal.Parse(xmlTag[0].InnerText.Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), NumberStyles.Any, CultureInfo.InvariantCulture);
            }
            catch (Exception ex) {
                Debug.Print(ex.ToString());
                return -1;
            }
        }
        public static bool isUpdateAvailable(decimal cloudVersion) {
            try {
                if (Properties.Settings.Default.currentVersion > cloudVersion) {
                    MessageBox.Show("Wow, future version user!");
                    return false;
                }
                if (Properties.Settings.Default.currentVersion < cloudVersion)
                    return true;
                else
                    return false;
            }
            catch (Exception ex) {
                Debug.Print(ex.ToString());
                return false;
            }
        }
    }
}