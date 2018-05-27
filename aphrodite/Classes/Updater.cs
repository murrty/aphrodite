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

        public static decimal getCloudVersion() {
            try {
                string xml = apiTools.getJSON(githubJSON, Program.UserAgent);
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
                    if (cloudVersion != -1)
                        MessageBox.Show("Wow, future version user!");
                    return false;
                }
                else if (Properties.Settings.Default.currentVersion < cloudVersion)
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