using System;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;

namespace aphrodite {
    class Updater {

        public static string githubURL = "https://github.com/murrty/aphrodite";
        public static string githubJSON = "https://api.github.com/repos/murrty/aphrodite/releases/latest";

        public static bool CheckForUpdate(bool DisableSkipVerison = false) {
            try {
                string xml = apiTools.GetJsonToXml(githubJSON);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNodeList xmlTag = doc.DocumentElement.SelectNodes("/root/tag_name");
                XmlNodeList xmlHeader = doc.DocumentElement.SelectNodes("/root/name");
                XmlNodeList xmlBody = doc.DocumentElement.SelectNodes("/root/body");

                decimal CloudVersion = decimal.Parse(xmlTag[0].InnerText.Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), NumberStyles.Any, CultureInfo.InvariantCulture);

                if (CloudVersion > Properties.Settings.Default.CurrentVersion) {
                    frmUpdateAvailable UpdateDialog = new frmUpdateAvailable();
                    UpdateDialog.NewVersion = CloudVersion.ToString();
                    UpdateDialog.UpdateHeader = xmlHeader[0].InnerText;
                    UpdateDialog.UpdateBody = xmlBody[0].InnerText;
                    UpdateDialog.BlockSkip = DisableSkipVerison;
                    switch (UpdateDialog.ShowDialog()) {
                        case DialogResult.Yes:
                            System.Diagnostics.Process.Start("https://github.com/murrty/aphrodite/releases/latest");
                            break;

                        case DialogResult.Ignore:
                            Properties.Settings.Default.SkippedVersion = CloudVersion;
                            Properties.Settings.Default.Save();
                            break;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex) {
                ErrorLog.ReportException(ex, "Updater.cs -> CheckForUpdate");
                return false;
            }
        }

    }
}