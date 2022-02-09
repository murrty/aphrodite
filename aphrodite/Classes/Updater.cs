using System;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;

namespace aphrodite {
#pragma warning disable CS0162 // Unreachable code detected (Program.IsBetaVersion)
    class Updater {

        private const string GithubURL = "https://github.com/murrty/aphrodite";
        private const string LatestReleaseJSON = "https://api.github.com/repos/murrty/aphrodite/releases/latest";
        private const string LatestAllJSON = "https://api.github.com/repos/murrty/aphrodite/releases";

        /// <summary>
        /// Checks for updates on Github
        /// </summary>
        /// <param name="ForceCheck">Determines if the check is being forced (aka, from the About form)</param>
        /// <returns></returns>
        public static bool CheckForUpdate(bool ForceCheck) {
            try {
                if (Git.Data.UpdateChecked != UpdateType.NotChecked && !ForceCheck) {
                    if (Git.Data.UpdateChecked == UpdateType.Release && Config.Settings.Initialization.CheckForBetaUpdates
                    || Git.Data.UpdateChecked == UpdateType.PreRelease && !Config.Settings.Initialization.CheckForBetaUpdates) {
                        Log.Write("Update checked doesn't match the expected update type.");
                        return CheckForUpdate(true);
                    }

                    if (Program.IsBetaVersion) {
                        if (Config.Settings.Initialization.CheckForBetaUpdates) {
                            if (Git.Data.UpdatePreReleaseVersion != Program.BetaVersion) {
                                Log.Write("The program has a known update, but it wasn't forced to check for a new update.");
                                ShowUpdateForm(true, true);
                                return true;
                            }
                            else {
                                Log.Write("No update available, didn't check Github.");
                                return false;
                            }
                        }
                        else {
                            if (Git.Data.UpdateLatestVersion.ToString() != Program.BetaVersion) {
                                Log.Write("The program has a known update, but it wasn't forced to check for a new update.");
                                ShowUpdateForm(true, true);
                                return true;
                            }
                            else {
                                Log.Write("No update available, didn't check Github.");
                                return false;
                            }
                        }
                    }
                    else {
                        if (Config.Settings.Initialization.CheckForBetaUpdates) {
                            if (Git.Data.UpdatePreReleaseVersion != Program.CurrentVersion.ToString()) {
                                Log.Write("The program has a known update, but it wasn't forced to check for a new update.");
                                ShowUpdateForm(true, true);
                                return true;
                            }
                            else {
                                Log.Write("No update available, didn't check Github.");
                                return false;
                            }
                        }
                        else {
                            if (Git.Data.UpdateLatestVersion > Program.CurrentVersion) {
                                Log.Write("The program has a known update, but it wasn't forced to check for a new update.");
                                ShowUpdateForm(true, true);
                                return true;
                            }
                            else {
                                Log.Write("No update available, didn't check Github.");
                                return false;
                            }
                        }
                    }
                }
                else {
                    bool UpdateAvailable;
                    Log.Write("Checking for an update on github...");
                    string xml = ApiTools.DownloadJsonToXml(Config.Settings.Initialization.CheckForBetaUpdates ? LatestAllJSON : LatestReleaseJSON);
                    XmlDocument xmlDoc = new();
                    xmlDoc.LoadXml(xml);

                    if (Config.Settings.Initialization.CheckForBetaUpdates) {
                        Log.Write("Parsing API data for a pre-release...");
                        XmlNodeList xmlTag = xmlDoc.DocumentElement.SelectNodes("/root/item/tag_name");
                        XmlNodeList xmlHeader = xmlDoc.DocumentElement.SelectNodes("/root/item/name");
                        XmlNodeList xmlBody = xmlDoc.DocumentElement.SelectNodes("/root/item/body");
                        if (xmlTag.Count > 0) {
                            Log.Write("API data contains a valid tag_name node.");
                            Git.Data.UpdateChecked = UpdateType.PreRelease;
                            Git.Data.UpdateHeader = xmlHeader[0].InnerText;
                            Git.Data.UpdateBody = xmlBody[0].InnerText;
                            Git.Data.UpdatePreReleaseVersion = xmlTag[0].InnerText;

                            if (Program.IsBetaVersion) {
                                UpdateAvailable = Git.Data.UpdatePreReleaseVersion != Program.BetaVersion;
                            }
                            else {
                                decimal TemporaryCloudVersion = decimal.Parse(
                                    xmlTag[0].InnerText.Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator).Split('-')[0],
                                    NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture
                                );

                                UpdateAvailable = TemporaryCloudVersion > Program.CurrentVersion;
                            }

                            if (UpdateAvailable) {
                                if (!ForceCheck && Git.Data.UpdatePreReleaseVersion == Config.Settings.Initialization.SkippedBetaVersion) {
                                    Log.Write($"Update v{Git.Data.UpdatePreReleaseVersion} is available, but this version is being skipped.");
                                    return true;
                                }
                                Log.Write($"Update v{Git.Data.UpdatePreReleaseVersion} is available.");

                                ShowUpdateForm(true, ForceCheck);
                                return true;
                            }
                            else if (ForceCheck) {
                                Log.MessageBox("No updates are available at this time.");
                            }
                            return false;
                        }
                        else throw new ApiReturnedNullOrEmptyException("The xmlTag node count was 0 and cannot be parsed.");
                    }
                    else {
                        Log.Write("Parsing API data for a release...");
                        XmlNodeList xmlTag = xmlDoc.DocumentElement.SelectNodes("/root/tag_name");
                        XmlNodeList xmlHeader = xmlDoc.DocumentElement.SelectNodes("/root/name");
                        XmlNodeList xmlBody = xmlDoc.DocumentElement.SelectNodes("/root/body");
                        if (xmlTag.Count > 0) {
                            Log.Write("API data contains a valid tag_name node.");
                            Git.Data.UpdateChecked = UpdateType.Release;
                            Git.Data.UpdateHeader = xmlHeader[0].InnerText;
                            Git.Data.UpdateBody = xmlBody[0].InnerText;
                            Git.Data.UpdateLatestVersion = decimal.Parse(
                                xmlTag[0].InnerText.Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator),
                                NumberStyles.AllowDecimalPoint,
                                CultureInfo.InvariantCulture
                            );

                            if (Program.IsBetaVersion) {
                                decimal TemporaryProgramVersion = decimal.Parse(
                                    Program.BetaVersion.Replace(".", CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator).Split('-')[0],
                                    NumberStyles.AllowDecimalPoint,
                                    CultureInfo.InvariantCulture
                                );

                                UpdateAvailable = Git.Data.UpdateLatestVersion > TemporaryProgramVersion;
                            }
                            else {
                                UpdateAvailable = Git.Data.UpdateLatestVersion > Program.CurrentVersion;
                            }

                            if (UpdateAvailable) {
                                if (!ForceCheck && Git.Data.UpdateLatestVersion == Config.Settings.Initialization.SkippedVersion) {
                                    Log.Write($"Update v{Git.Data.UpdateLatestVersion} is available, but this version is being skipped.");
                                    return true;
                                }
                                Log.Write($"Update v{Git.Data.UpdateLatestVersion} is available.");

                                ShowUpdateForm(false, ForceCheck);
                                return true;
                            }
                            else if (ForceCheck) {
                                Log.MessageBox("No updates are available at this time.");
                            }
                            return false;

                        }
                        else throw new ApiReturnedNullOrEmptyException("The xmlTag node count was 0 and cannot be parsed.");
                    }
                }
            }
            catch (Exception ex) {
                Log.ReportException(ex);
                return false;
            }
        }

        /// <summary>
        /// Shows the update form.
        /// </summary>
        /// <param name="PreRelease">Whether the update form should display as a pre-release.</param>
        /// <param name="BlockSkip">Whether the "skip version" button should be disabled.</param>
        private static void ShowUpdateForm(bool PreRelease, bool BlockSkip) {
            using frmUpdateAvailable UpdateDialog = new();
            UpdateDialog.NewVersion = PreRelease ? Git.Data.UpdatePreReleaseVersion : Git.Data.UpdateLatestVersion.ToString();
            UpdateDialog.UpdateHeader = Git.Data.UpdateHeader;
            UpdateDialog.UpdateBody = Git.Data.UpdateBody;
            UpdateDialog.BlockSkip = BlockSkip;
            UpdateDialog.PreRelease = PreRelease;
            switch (UpdateDialog.ShowDialog()) {
                case DialogResult.Yes: {
                    System.Diagnostics.Process.Start(GithubURL);
                } break;

                case DialogResult.Ignore: {
                    if (!BlockSkip) {
                        Log.Write($"Ignoring update v{(PreRelease ? Git.Data.UpdatePreReleaseVersion : Git.Data.UpdateLatestVersion.ToString())}");

                        if (PreRelease) {
                            Config.Settings.Initialization.SkippedBetaVersion = Git.Data.UpdatePreReleaseVersion;
                        }
                        else {
                            Config.Settings.Initialization.SkippedVersion = Git.Data.UpdateLatestVersion;
                        }

                        Config.Settings.Initialization.Save();
                    }
                } break;
            }
        }

    }
#pragma warning restore CS0162 // Unreachable code detected (Program.IsBetaVersion)

    /// <summary>
    /// Class containing update information.
    /// </summary>
    public class Git {

        /// <summary>
        /// Gets the application-wide instance of the Git class containing data regarding updates.
        /// </summary>
        public static volatile Git Data = new();

        /// <summary>
        /// Whether the update has been checked.
        /// </summary>
        public UpdateType UpdateChecked {
            get; set;
        } = UpdateType.NotChecked;

        /// <summary>
        /// The decimal of the latest update, release.
        /// </summary>
        public decimal UpdateLatestVersion {
            get; set;
        } = -1;

        /// <summary>
        /// The string of the latest update, pre-release or release.
        /// </summary>
        public string UpdatePreReleaseVersion {
            get; set;
        } = null;

        /// <summary>
        /// The string of the update header.
        /// </summary>
        public string UpdateHeader {
            get; set;
        } = null;

        /// <summary>
        /// The string of the update body.
        /// </summary>
        public string UpdateBody {
            get; set;
        } = null;

    }

    /// <summary>
    /// What type of update was checked.
    /// </summary>
    public enum UpdateType {
        /// <summary>
        /// No update was checked.
        /// </summary>
        NotChecked,
        /// <summary>
        /// Release was checked.
        /// </summary>
        Release,
        /// <summary>
        /// Re-release was checked.
        /// </summary>
        PreRelease
    }
}