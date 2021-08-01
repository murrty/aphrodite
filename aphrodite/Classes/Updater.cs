using System;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;

namespace aphrodite {
    class Updater {

        public static string githubURL = "https://github.com/murrty/aphrodite";
        public static string githubJSON = "https://api.github.com/repos/murrty/aphrodite/releases/latest";

        private static volatile bool UpdateChecked = false;
        private static volatile string CheckedVersion = string.Empty;
        private static string CheckedHeader = string.Empty;
        private static string CheckedBody = string.Empty;

        /// <summary>
        /// Checks for updates on Github
        /// </summary>
        /// <param name="ForceCheck">Determines if the check is being forced (aka, from the About form)</param>
        /// <returns></returns>
        public static bool CheckForUpdate(bool ForceCheck) {
            try {
                if (UpdateChecked) {
                    decimal CloudVersion = decimal.Parse(CheckedVersion.Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), NumberStyles.Any, CultureInfo.InvariantCulture);

                    if (CloudVersion > Properties.Settings.Default.CurrentVersion) {
                        frmUpdateAvailable UpdateDialog = new frmUpdateAvailable();
                        UpdateDialog.NewVersion = CheckedVersion.ToString();
                        UpdateDialog.UpdateHeader = CheckedHeader;
                        UpdateDialog.UpdateBody = CheckedBody;
                        UpdateDialog.BlockSkip = true;
                        switch (UpdateDialog.ShowDialog()) {
                            case DialogResult.Yes:
                                System.Diagnostics.Process.Start("https://github.com/murrty/aphrodite/releases/latest");
                                break;
                        }

                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    string xml = apiTools.DownloadJsonToXml(githubJSON);
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);
                    XmlNodeList xmlTag = doc.DocumentElement.SelectNodes("/root/tag_name");
                    XmlNodeList xmlHeader = doc.DocumentElement.SelectNodes("/root/name");
                    XmlNodeList xmlBody = doc.DocumentElement.SelectNodes("/root/body");

                    decimal CloudVersion = decimal.Parse(xmlTag[0].InnerText.Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator), NumberStyles.Any, CultureInfo.InvariantCulture);

                    UpdateChecked = true;
                    CheckedVersion = xmlTag[0].InnerText;

                    if (CloudVersion > Properties.Settings.Default.CurrentVersion) {
                        // Set the Header and Body into variable (it was already downloaded, might as well)
                        CheckedHeader = xmlHeader[0].InnerText;
                        CheckedBody = xmlBody[0].InnerText;

                        if (!ForceCheck && CloudVersion == Properties.Settings.Default.SkippedVersion) {
                            Program.Log(LogAction.WriteToLogWithInvoke, "Update v" + CloudVersion + " is available, but this version is being skipped.");
                            return false;
                        }

                        Program.Log(LogAction.WriteToLog, "Update v" + CloudVersion + " is available");
                        using (frmUpdateAvailable UpdateDialog = new frmUpdateAvailable()) {
                            UpdateDialog.NewVersion = CloudVersion.ToString();
                            UpdateDialog.UpdateHeader = xmlHeader[0].InnerText;
                            UpdateDialog.UpdateBody = xmlBody[0].InnerText;
                            UpdateDialog.BlockSkip = ForceCheck;
                            switch (UpdateDialog.ShowDialog()) {
                                case DialogResult.Yes:
                                    System.Diagnostics.Process.Start("https://github.com/murrty/aphrodite/releases/latest");
                                    break;

                                case DialogResult.Ignore:
                                    Program.Log(LogAction.WriteToLogWithInvoke, "Ignoring update v" + CloudVersion);
                                    Config.Settings.Initialization.SkippedVersion = CloudVersion;
                                    Config.Settings.Save(ConfigType.Initialization);
                                    break;
                            }
                        }
                        return true;
                    }
                    else if (ForceCheck) {
                        MessageBox.Show("No updates are available at this time.");
                    }

                    return false;
                }

            }
            catch (Exception ex) {
                ErrorLog.ReportException(ex, "Updater.cs -> CheckForUpdate");
                return false;
            }
        }

    }
}