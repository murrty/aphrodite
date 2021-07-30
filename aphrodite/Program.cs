using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;

namespace aphrodite {
    static class Program {
        public static readonly string UserAgent = "aphrodite/" + (Properties.Settings.Default.CurrentVersion) + " (Contact: https://github.com/murrty/aphrodite ... open an issue)";
        public static volatile string ApplicationName = System.AppDomain.CurrentDomain.FriendlyName;
        public static volatile string ApplicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static volatile string FullApplicationPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        public static volatile bool IsDebug = false;
        public static volatile bool IsAdmin = false;
        public static volatile IniFile Ini = new IniFile(ApplicationPath + "\\aphrodite.ini");
        public static volatile bool UseIni = false;

        private static volatile string PushedArgument = null;
        private static volatile DownloadType PushedType = DownloadType.None;
        private static volatile frmLog Logger;
        private static volatile bool LogEnabled = false;

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SetDebug();

            if (!Controls.TaskbarProgress.Windows7OrGreater) {
                MessageBox.Show("Windows 7 or higher is required to run this application.");
                Environment.Exit(0);
            }
            else {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (Environment.CurrentDirectory != ApplicationPath) {
                    Log(LogAction.WriteToLog, "Environment.CurrentDirectory is set to \"" + Environment.CurrentDirectory + "\", when it should be \"" + ApplicationPath + "\".");
                    Environment.CurrentDirectory = ApplicationPath;
                    Log(LogAction.WriteToLog, "Set the Environment.CurrentDirectory to the correct one.");
                }

                switch (File.Exists(ApplicationPath + "\\aphrodite.ini") && Ini.KeyExists("useIni") && Ini.ReadBool("useIni")) {
                    case true:
                        UseIni = true;
                        break;
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
                    Application.Run(new frmMain(PushedArgument, PushedType));
                    Log(LogAction.DisableLog);
                }
                Config.Settings.FormSettings.Save();
            }
        }

        static bool HasArgumentsThatSkipMainForm() {
            Log(LogAction.WriteToLog, "Checking arguments");
            string CurrentArg;

            switch (Environment.GetCommandLineArgs().Length < 1) {
                case true:
                    return false;
            }

            for (int ArgIndex = 1; ArgIndex < Environment.GetCommandLineArgs().Length; ArgIndex++) {
                CurrentArg = Environment.GetCommandLineArgs()[ArgIndex];

                #region "-installprotocol"
                if (CurrentArg.ToLower().StartsWith("-installprotocol")) { // if the argument is installProtocol
                    Config.Settings.Load(ConfigType.All);
                    frmSettings Settings = new frmSettings();
                    Settings.InstallProtocol = true;
                    Settings.ShowDialog();
                    Settings.Dispose();
                    return true;
                }
                #endregion

                #region "-updateprotocol"
                else if (CurrentArg.ToLower().StartsWith("-updateprotocol")) {
                    if (CurrentArg.ToLower().StartsWith("-updateprotocol:tags")) {
                        string NewPath = CurrentArg.Split('|')[1];

                    }
                    else if (CurrentArg.ToLower().StartsWith("-updateprotocol:pools")) {
                        string NewPath = CurrentArg.Split('|')[1];

                    }
                    else if (CurrentArg.ToLower().StartsWith("-updateprotocol:images")) {
                        string NewPath = CurrentArg.Split('|')[1];

                    }
                    else {
                        return false;
                    }
                    return true;
                }
                #endregion

                #region "-settings", "*:configuresettings", "protocol", "-portable", "-schema", "configuresettings"
                else if (CurrentArg.ToLower().StartsWith("-settings") || CurrentArg.StartsWith("tags:configuresettings") || CurrentArg.StartsWith("pools:configuresettings") || CurrentArg.StartsWith("images:configuresettings") || CurrentArg.StartsWith("-protocol") || CurrentArg.StartsWith("-portable") || CurrentArg.StartsWith("-schema") || CurrentArg.StartsWith("configuresettings")) {
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
                    if (CurrentArg == "start") {
                        Log(LogAction.WriteToLog, "Using tags: protocol to start aphrodite");
                        PushedType = DownloadType.Tags;
                        return false;
                    }
                    else {
                        Log(LogAction.WriteToLog, "Some tags were passed to the program");
                        bool IsPage = false;
                        if (apiTools.IsValidPageLink(CurrentArg)) {
                            IsPage = true;
                        }

                        if (Config.Settings.Initialization.AutoDownloadWithArguments) {
                            if (!Config.Settings.Initialization.SkipArgumentCheck) {
                                using (frmArgumentDialog ArgumentDialog = new frmArgumentDialog(CurrentArg, DownloadType.Tags)) {
                                    switch (ArgumentDialog.ShowDialog()) {
                                        case DialogResult.Yes:
                                            CurrentArg = ArgumentDialog.AppendedArgument;
                                            Config.Settings.Load(ConfigType.General);
                                            Config.Settings.Load(ConfigType.Tags);
                                            Config.Settings.Load(ConfigType.FormSettings);

                                            if (IsPage) {
                                                Downloader.Arguments.DownloadPage(CurrentArg);
                                            }
                                            else {
                                                Downloader.Arguments.DownloadTags(CurrentArg.Replace("%20", " "));
                                            }
                                            return true;

                                        case DialogResult.No:
                                            CurrentArg = ArgumentDialog.AppendedArgument;
                                            Log(LogAction.WriteToLog, "Pushing the arguments to the main form");
                                            PushedArgument = CurrentArg;
                                            PushedType = DownloadType.Tags;
                                            return false;

                                        case DialogResult.Cancel:
                                            return true;
                                    }
                                }
                            }

                            Config.Settings.Load(ConfigType.General);
                            Config.Settings.Load(ConfigType.Tags);
                            Config.Settings.Load(ConfigType.FormSettings);

                            if (IsPage) {
                                Downloader.Arguments.DownloadPage(CurrentArg);
                            }
                            else {
                                Downloader.Arguments.DownloadTags(CurrentArg.Replace("%20", " "));
                            }
                        }
                        else {
                            Log(LogAction.WriteToLog, "Pushing the arguments to the main form");
                            PushedArgument = CurrentArg;
                            PushedType = DownloadType.Tags;
                            return false;
                        }
                    }
                }
                #endregion

                #region "pools:"
                else if (CurrentArg.StartsWith("pools:")) {
                    CurrentArg = CurrentArg.Substring(6);

                    if (CurrentArg == "start") {
                        Log(LogAction.WriteToLog, "Using pools: protocol to start aphrodite");
                        PushedType = DownloadType.Pools;
                        return false;
                    }
                    else {
                        Log(LogAction.WriteToLog, "A pool was passed to the program");
                        if (apiTools.IsValidPoolLink(CurrentArg)) {
                            Log(LogAction.WriteToLog, "It seems like it was a valid pool link.");
                            CurrentArg = apiTools.GetPoolOrPostId(CurrentArg);
                        }

                        if (Config.Settings.Initialization.AutoDownloadWithArguments) {
                            if (!Config.Settings.Initialization.SkipArgumentCheck) {
                                using (frmArgumentDialog ArgumentDialog = new frmArgumentDialog(CurrentArg, DownloadType.Pools)) {
                                    switch (ArgumentDialog.ShowDialog()) {
                                        case DialogResult.Yes:
                                            CurrentArg = ArgumentDialog.AppendedArgument;
                                            Config.Settings.Load(ConfigType.General);
                                            Config.Settings.Load(ConfigType.Pools);
                                            Config.Settings.Load(ConfigType.FormSettings);
                                            Downloader.Arguments.DownloadPool(CurrentArg);
                                            return true;

                                        case DialogResult.No:
                                            CurrentArg = ArgumentDialog.AppendedArgument;
                                            Log(LogAction.WriteToLog, "Pushing the arguments to the main form");
                                            PushedArgument = CurrentArg;
                                            PushedType = DownloadType.Pools;
                                            return false;

                                        case DialogResult.Cancel:
                                            return true;
                                    }
                                }
                            }

                        }
                        else {
                            Log(LogAction.WriteToLog, "Pushing the arguments to the main form");
                            PushedArgument = CurrentArg;
                            PushedType = DownloadType.Pools;
                            return false;
                        }
                    }
                }
                #endregion

                #region "images:"
                else if (CurrentArg.StartsWith("images:")) {
                    CurrentArg = CurrentArg.Substring(7);
                    if (CurrentArg == "start") {
                        Log(LogAction.WriteToLog, "Using images: protocol to start aphrodite");
                        PushedType = DownloadType.Images;
                        return false;
                    }
                    else {
                        Log(LogAction.WriteToLog, "An image was passed to the program");
                        if (apiTools.IsValidImageLink(CurrentArg)) {
                            Log(LogAction.WriteToLog, "It seems like it was a valid image link.");
                            CurrentArg = apiTools.GetPoolOrPostId(CurrentArg);
                        }

                        if (Config.Settings.Initialization.AutoDownloadWithArguments) {
                            if (!Config.Settings.Initialization.SkipArgumentCheck) {
                                using (frmArgumentDialog ArgumentDialog = new frmArgumentDialog(CurrentArg, DownloadType.Images)) {
                                    switch (ArgumentDialog.ShowDialog()) {
                                        case DialogResult.Yes:
                                            CurrentArg = ArgumentDialog.AppendedArgument;
                                            Config.Settings.Load(ConfigType.General);
                                            Config.Settings.Load(ConfigType.Images);
                                            Config.Settings.Load(ConfigType.FormSettings);
                                            Downloader.Arguments.DownloadImage(CurrentArg);
                                            return true;

                                        case DialogResult.No:
                                            CurrentArg = ArgumentDialog.AppendedArgument;
                                            Log(LogAction.WriteToLog, "Pushing the arguments to the main form");
                                            PushedArgument = CurrentArg;
                                            PushedType = DownloadType.Images;
                                            return false;

                                        case DialogResult.Cancel:
                                            return true;
                                    }
                                }
                            }

                            Config.Settings.Load(ConfigType.General);
                            Config.Settings.Load(ConfigType.Images);
                            Config.Settings.Load(ConfigType.FormSettings);
                            Downloader.Arguments.DownloadImage(CurrentArg);
                            return true;
                        }
                        else {
                            Log(LogAction.WriteToLog, "Pushing the arguments to the main form");
                            PushedArgument = CurrentArg;
                            PushedType = DownloadType.Images;
                            return false;
                        }
                    }
                }
                #endregion

                #region "poolwl:*"
                else if (CurrentArg.StartsWith("poolwl:")) {
                    Config.Settings.Load(ConfigType.Pools);
                    Config.Settings.Load(ConfigType.FormSettings);
                    if (CurrentArg == "poolwl:showwl") {
                        frmPoolWishlist WishList = new frmPoolWishlist();
                        WishList.ShowDialog();
                    }
                    else if (CurrentArg == "start") {
                        Log(LogAction.WriteToLog, "Using poolwl: protocol to start aphrodite");
                        return false;
                    }
                    else {
                        CurrentArg = CurrentArg.Substring(7).Replace("%$|%", " ");
                        string[] ArgumentSplit = CurrentArg.Split('|');
                        if (ArgumentSplit.Length == 2) {
                            string url = ArgumentSplit[0];
                            string title = title = ArgumentSplit[1];

                            if (apiTools.IsValidPoolLink(ArgumentSplit[0])) {
                                title = title
                                    .Replace("%20", " ")
                                    .Replace("%22", "\"")
                                    .Trim(' ');

                                Config.Config_Pools.AppendToWishlist(title, url);
                            }
                            else {
                                System.Media.SystemSounds.Hand.Play();
                            }

                        }
                    }
                    return true;
                }
                #endregion

                #region "-redownloader"
                else if (CurrentArg.ToLower().StartsWith("-redownloader")) {
                    Config.Settings.Load(ConfigType.All);
                    frmRedownloader rDownloader = new frmRedownloader();
                    rDownloader.ShowDialog();
                    return true;
                }
                #endregion

                #region "-blacklist"
                else if (CurrentArg.ToLower().StartsWith("-blacklist")) {
                    Config.Settings.Load(ConfigType.General);
                    Config.Settings.Load(ConfigType.FormSettings);
                    using (frmBlacklist Blacklist = new frmBlacklist()) {
                        Blacklist.ShowDialog();
                        if (Config.Settings.FormSettings.frmBlacklist_Location != Blacklist.Location) {
                            Config.Settings.FormSettings.frmBlacklist_Location = Blacklist.Location;
                        }
                    }
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

        [System.Diagnostics.DebuggerStepThrough]
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

                        if (Logger.WindowState == FormWindowState.Minimized || Logger.WindowState == FormWindowState.Maximized) {
                            Logger.Opacity = 0;
                            Logger.WindowState = FormWindowState.Normal;
                        }

                        if (Logger.Location != Config.Settings.FormSettings.frmLog_Location) {
                            Config.Settings.FormSettings.frmLog_Location = Logger.Location;
                        }
                        if (Logger.Size != Config.Settings.FormSettings.frmLog_Size) {
                            Config.Settings.FormSettings.frmLog_Size = Logger.Size;
                        }

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

                case LogAction.WriteToLogWithInvoke:
                    switch (LogEnabled) {
                        case true:
                            if (Logger != null) {
                                Logger.AppendWithInvoke(LogMessage);
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
        [System.Diagnostics.DebuggerStepThrough]
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