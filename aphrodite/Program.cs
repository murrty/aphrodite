using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;

namespace aphrodite {
    static class Program {
        public static readonly string UserAgent = "aphrodite/" + (Properties.Settings.Default.currentVersion) + " (Contact: https://github.com/murrty/aphrodite ... open an issue)";
        public static readonly string ApplicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static volatile bool IsDebug = false;
        public static volatile bool IsAdmin = false;
        public static volatile IniFile Ini = new IniFile(ApplicationPath + "\\aphrodite.ini");
        public static volatile bool UseIni = false;

        private static volatile DownloadType type = DownloadType.None;
        private static volatile string arg = null;
        private static volatile frmLog Logger;
        private static volatile bool LogEnabled = false;

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SetDebug();

            if (!TaskbarProgress.Windows7OrGreater) {
                MessageBox.Show("Windows 7 or higher is required to run this application.");
                Environment.Exit(0);
            }
            else {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (File.Exists(ApplicationPath + "\\aphrodite.ini")) {
                    if (Ini.KeyExists("useIni")) {
                        if (Ini.ReadBool("useIni")) {
                            UseIni = true;
                        }
                    }
                }

                Log(LogAction.EnableLog);
                Config.Settings = new Config();

                switch ((new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator)) {
                    case true:
                        Log(LogAction.WriteToLog, "Running as administrator. I hope you know what you're doing");
                        IsAdmin = true;
                        break;

                    case false:
                        Log(LogAction.WriteToLog, "Not running as administrator (bad for protocols, good for everything else)");
                        IsAdmin = false;
                        break;
                }

                if (Config.Settings.Initialization.firstTime) {
                    Log(LogAction.WriteToLog, "First time running! Look at my notice");
                    MessageBox.Show(
                        "This is your first time running aphrodite, so read this before continuing:\n\n" +
                        "This program is \"advertised\" as a porn downloader. You don't have to download any 18+ material using it, but it's emphasised on porn.\n" +
                        "As soon as you get access into the program, change the settings and set your prefered file name schema. I don't want to be responsible for any excess files being downloaded.\n\n" +
                        "Just so you know."
                    );
                    Config.Settings.Initialization.firstTime = false;
                    Config.Settings.Save(ConfigType.Initialization);
                }
                if (!HasArgumentsThatSkipMainForm()) {
                    Log(LogAction.WriteToLog, "No arguments were passed that didn't require the main form");
                    Config.Settings.Load(ConfigType.All);
                    Application.Run(new frmMain(arg, type));
                }
            }
        }

        static bool HasArgumentsThatSkipMainForm() {
            Log(LogAction.WriteToLog, "Checking arguments");
            string CurrentArg;

            for (int ArgIndex = 1; ArgIndex < Environment.GetCommandLineArgs().Length; ArgIndex++) {
                CurrentArg = Environment.GetCommandLineArgs()[ArgIndex];

                #region "installProtocol"
                if (CurrentArg.StartsWith("installProtocol")) { // if the argument is installProtocol
                    Config.Settings.Load(ConfigType.All);
                    frmSettings Settings = new frmSettings();
                    Settings.InstallProtocol = true;
                    Settings.ShowDialog();
                    Settings.Dispose();
                    return true;
                }
                #endregion

                #region "-settings", "*:configuresettings", "protocol", "-portable", "-schema", "configuresettings"
                else if (CurrentArg.StartsWith("-settings") || CurrentArg.StartsWith("tags:configuresettings") || CurrentArg.StartsWith("pools:configuresettings") || CurrentArg.StartsWith("images:configuresettings") || CurrentArg.StartsWith("-protocol") || CurrentArg.StartsWith("-portable") || CurrentArg.StartsWith("-schema") || CurrentArg.StartsWith("configuresettings")) {
                    Config.Settings.Load(ConfigType.All);

                    frmSettings Settings = new frmSettings();
                    Settings.SwitchTab = true;

                    switch (CurrentArg.ToLower()) {
                        case "-settings":
                            Settings.Tab = 0;
                            break;
                        case "tags:configuresettings":
                            Settings.Tab = 1;
                            break;
                        case "pools:configuresettings":
                            Settings.Tab = 2;
                            break;
                        case "images:configuresettings":
                            Settings.Tab = 3;
                            break;
                        case "-protocol":
                            Settings.Tab = 4;
                            break;
                        case "-portable":
                            Settings.Tab = 5;
                            break;
                        case "-schema":
                            Settings.Tab = 6;
                            break;
                    }

                    Settings.ShowDialog();
                    Settings.Dispose();

                    return true;
                }
                #endregion

                #region "tags:*" (only for pages)
                else if (CurrentArg.StartsWith("tags:")) {
                    CurrentArg = CurrentArg.Substring(5).Replace("|", " ");

                    if (apiTools.IsValidPageLink(CurrentArg)) {
                        Config.Settings.Load(ConfigType.General);
                        Config.Settings.Load(ConfigType.Tags);
                        Downloader.Arguments.DownloadPage(CurrentArg);
                        return true;
                    }
                    else {
                        Log(LogAction.WriteToLog, "Some tags were passed to the program");
                        type = DownloadType.Tags;
                        arg = CurrentArg.Replace("%20", " ");
                        return false;
                    }
                }
                #endregion

                #region "poolwl:*"
                else if (CurrentArg.StartsWith("poolwl:")) {
                    Config.Settings.Load(ConfigType.Pools);
                    if (CurrentArg == "poolwl:showwl") {
                        frmPoolWishlist WishList = new frmPoolWishlist();
                        WishList.ShowDialog();
                    }
                    else {
                        CurrentArg = CurrentArg.Substring(7);
                        string[] ArgumentSplit = CurrentArg.Split('|');
                        if (ArgumentSplit.Length != 1) {
                            string url = ArgumentSplit[0];
                            string title = string.Empty;

                            if (ArgumentSplit.Length > 2) {
                                for (int j = 1; j < ArgumentSplit.Length; j++) {
                                    switch (string.IsNullOrWhiteSpace(ArgumentSplit[j])) {
                                        case true:
                                            continue;
                                    }
                                    title += ArgumentSplit[j] + "|";
                                }
                                title = title.Substring(0, title.Length - 1);
                            }
                            else {
                                title = ArgumentSplit[1];
                            }

                            title = title
                                .Replace("%20", " ")
                                .Replace("%22", "\"")
                                .Trim(' ');

                            Config.Config_Pools.AppendToWishlist(title, url);

                        }
                    }
                    return true;
                }
                #endregion

                #region "pools:"
                else if (CurrentArg.StartsWith("pools:") && apiTools.IsValidPoolLink(CurrentArg.Replace("pools:", ""))) {
                    CurrentArg = CurrentArg.Substring(6);
                    CurrentArg = CurrentArg.Replace("http://", "https://").Replace("https://www.", "https://");

                    if (apiTools.IsValidPoolLink(CurrentArg)) {
                        Config.Settings.Load(ConfigType.General);
                        Config.Settings.Load(ConfigType.Pools);
                        if (CurrentArg.Contains("?")) {
                            CurrentArg = CurrentArg.Split('?')[0];
                        }

                        CurrentArg = CurrentArg.Split('/')[4];

                        Downloader.Arguments.DownloadPool(CurrentArg);
                        return true;
                    }
                    else {
                        Log(LogAction.WriteToLog, "A pool id was passed to the program");
                        arg = CurrentArg;
                        type = DownloadType.Pools;
                        return false;
                    }

                }
                #endregion

                #region "images:"
                else if (CurrentArg.StartsWith("images:") && apiTools.IsValidImageLink(CurrentArg.Replace("images:", ""))) {
                    CurrentArg = CurrentArg.Substring(7);
                    CurrentArg = CurrentArg.Split('?')[0];
                    if (apiTools.IsValidImageLink(CurrentArg)) {
                        Config.Settings.Load(ConfigType.General);
                        Config.Settings.Load(ConfigType.Images);
                        Downloader.Arguments.DownloadImage(CurrentArg);
                        return true;
                    }
                    else {
                        Log(LogAction.WriteToLog, "An image id was passed to the program");
                        arg = CurrentArg;
                        type = DownloadType.Images;
                        return false;
                    }
                }
                #endregion

                #region "-redownloader"
                else if (CurrentArg.StartsWith("-redownloader")) {
                    Config.Settings.Load(ConfigType.All);
                    frmRedownloader rDownloader = new frmRedownloader();
                    rDownloader.ShowDialog();
                    return true;
                }
                #endregion

                #region "-blacklist"
                else if (CurrentArg.StartsWith("-blacklist")) {
                    Config.Settings.Load(ConfigType.General);
                    frmBlacklist bList = new frmBlacklist();
                    bList.ShowDialog();
                    return true;
                }
                #endregion

            }

            return false;
        }

        [System.Diagnostics.Conditional("DEBUG")]
        static void SetDebug() {
            IsDebug = true;
        }

        public static bool Log(LogAction Action, string LogMessage = "") {
            switch (Action) {
                default:
                    return false;

                case LogAction.EnableLog:
                    if (LogEnabled) {
                        Logger.Append("The log is already enabled");
                        return false;
                    }
                    else {
                        Logger = new frmLog();
                        Logger.Append("Log has been enabled", true);
                        LogEnabled = true;
                        return true;
                    }

                case LogAction.DisableLog:
                    if (LogEnabled) {
                        System.Diagnostics.Debug.Print("Disabling log");

                        if (Logger.WindowState == FormWindowState.Minimized) {
                            Logger.Opacity = 0;
                            Logger.WindowState = FormWindowState.Normal;
                        }
                        //if (Config.Default.LogFormLocation != Logger.Location) {
                        //    Config.Default.LogFormLocation = Logger.Location;
                        //    Properties.Settings.Default.ConfigChanged = true;
                        //}

                        //if (Config.Default.LogFormSize != Logger.Size) {
                        //    Config.Default.LogFormSize = Logger.Size;
                        //    Properties.Settings.Default.ConfigChanged = true;
                        //}

                        Logger.Dispose();
                        LogEnabled = false;
                        return true;
                    }
                    return false;

                case LogAction.WriteToLog:
                    switch (LogEnabled) {
                        case true:
                            if (Logger != null) {
                                Logger.Append(LogMessage);
                                return true;
                            }
                            else {
                                System.Diagnostics.Debug.Print(LogMessage);
                            }
                            break;
                        case false:
                            System.Diagnostics.Debug.Print(LogMessage);
                            break;
                    }
                    return false;

                case LogAction.InitialWriteToLog:
                    switch (LogEnabled) {
                        case true:
                            switch (Logger == null) {
                                case false:
                                    Logger.Append(LogMessage, true);
                                    return true;
                                case true:
                                    System.Diagnostics.Debug.Print(LogMessage);
                                    break;
                            }
                            break;
                        case false:
                            System.Diagnostics.Debug.Print(LogMessage);
                            break;
                    }
                    return false;

                case LogAction.ShowLog:
                    if (LogEnabled && Logger != null) {
                        if (Logger.IsShown) {
                            Logger.Append("The log was already shown dingus, activating log");
                            Logger.Activate();
                            return true;
                        }
                        else {
                            Logger.Append("Showing log");
                            Logger.IsShown = true;
                            Logger.Show();
                            return true;
                        }
                    }
                    return false;
            }
        }
        public static bool QuickLog(string LogMessage) {
            if (LogEnabled && Logger != null) {
                Logger.Append(LogMessage);
            }
            else {
                System.Diagnostics.Debug.Print(LogMessage);
            }
            return true;
        }
    }
}
