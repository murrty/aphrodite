using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;

namespace aphrodite {
    static class Program {

        /// <summary>
        /// The current release version of the program.
        /// </summary>
        public const decimal CurrentVersion = 2.21m;
        /// <summary>
        /// Whether the program is a beta version.
        /// </summary>
        public const bool IsBetaVersion = true;
        /// <summary>
        /// The beta version of the program (if <see cref="IsBetaVersion"/> is true).
        /// </summary>
        public const string BetaVersion = "2.3-pre1";
        /// <summary>
        /// The const delay for when threads sleep.
        /// </summary>
        public const int SleepDelay = 100;

        /// <summary>
        /// The absolute name of the application, with extension.
        /// </summary>
        public static readonly string ApplicationName = AppDomain.CurrentDomain.FriendlyName;
        /// <summary>
        /// The path of the application.
        /// </summary>
        public static readonly string ApplicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        /// <summary>
        /// The absolute path of the application.
        /// </summary>
        public static readonly string FullApplicationPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        /// <summary>
        /// The user agent that the program will use.
        /// </summary>
        public static readonly string UserAgent = $"aphrodite/{(IsBetaVersion ? BetaVersion : CurrentVersion)} (https://github.com/murrty/aphrodite)";

        /// <summary>
        /// Whether the program is debugging.
        /// </summary>
        public static bool IsDebug = false;
        /// <summary>
        /// Whether the program has ran as administrator.
        /// </summary>
        public static bool IsAdmin = false;

        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
            IsDebug = true;
#endif

            if (Environment.OSVersion.Version.Major >= 6 || (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 1)) {
                Log.InitializeLogging();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (Environment.CurrentDirectory != ApplicationPath) {
                    Log.Write($"Environment.CurrentDirectory is set to \"{Environment.CurrentDirectory}\", when it should be \"{ApplicationPath}\".");
                    Environment.CurrentDirectory = ApplicationPath;
                    Log.Write("Set the Environment.CurrentDirectory to the correct one.");
                }

                Config.Settings = new Config();
                Config.Settings.Initialize();
                IsAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
                if (IsAdmin) {
                    Log.Write("Running as administrator. I hope you know what you're doing");
                }

                if (Config.Settings.Initialization.FirstTime && !IsDebug) {
                    Log.Write("First time running! Look at my notice");
                    Log.MessageBox(
                        "This is your first time running aphrodite, so read this before continuing:\n\n" +
                        "This program is \"advertised\" as a porn downloader. You don't have to download any 18+ material using it, but it's emphasised on porn.\n" +
                        "As soon as you get access into the program, change the settings and set your prefered file name schema. I don't want to be responsible for any excess files being downloaded.\n\n" +
                        "Just so you know."
                    );
                    Config.Settings.Initialization.FirstTime = false;
                }
                Config.Settings.FormSettings.Load();
                Arguments.ParseArguments(args);
                switch (Arguments.ArgumentType) {

                    #region Update protocols
                    case ArgumentType.UpdateProtocol: {
                        if (IsAdmin && !IsDebug) {
                            Environment.ExitCode = SystemRegistry.SetRegistryKey() ? 0 : 1;
                        }
                        else Environment.ExitCode = 1;
                    } break;
                    #endregion

                    #region Downloads
                    case ArgumentType.DownloadTags: {
                        Config.Settings.General.Load();
                        Config.Settings.Tags.Load();
                        Downloader.DownloadTags(Arguments.ArgumentData, true);
                        Config.Settings.General.Save();
                        Config.Settings.Tags.Save();
                        Config.Settings.FormSettings.Save();
                    } break;

                    case ArgumentType.DownloadPage: {
                        Config.Settings.General.Load();
                        Config.Settings.Tags.Load();
                        Downloader.DownloadPage(Arguments.ArgumentData, true);
                        Config.Settings.General.Save();
                        Config.Settings.Tags.Save();
                        Config.Settings.FormSettings.Save();
                    } break;

                    case ArgumentType.DownloadPools: {
                        Config.Settings.General.Load();
                        Config.Settings.Pools.Load();
                        Downloader.DownloadPool(Arguments.ArgumentData, true);
                        Config.Settings.General.Save();
                        Config.Settings.Pools.Save();
                        Config.Settings.FormSettings.Save();
                    } break;

                    case ArgumentType.DownloadImages: {
                        Config.Settings.General.Load();
                        Config.Settings.Images.Load();
                        Downloader.DownloadImage(Arguments.ArgumentData, true);
                        Config.Settings.General.Save();
                        Config.Settings.Images.Save();
                        Config.Settings.FormSettings.Save();
                    } break;

                    case ArgumentType.DownloadFurryBooru: {
                        Config.Settings.General.Load();
                        Config.Settings.Tags.Load();
                        Downloader.DownloadFurryBooru(Arguments.ArgumentData, true);
                        Config.Settings.General.Save();
                        Config.Settings.Tags.Save();
                        Config.Settings.FormSettings.Save();
                    } break;

                    case ArgumentType.DownloadInkBunny: {
                        if (Arguments.ArgumentData != null && Arguments.ArgumentDataArray.Length >= 3) {
                            Config.Settings.General.Load();
                            Config.Settings.InkBunny.Load();
                            Downloader.DownloadInkbunny(Arguments.ArgumentDataArray[0], Arguments.ArgumentDataArray[1], Arguments.ArgumentDataArray[2], true);
                            Config.Settings.General.Save();
                            Config.Settings.InkBunny.Save();
                            Config.Settings.FormSettings.Save();
                        }
                    } break;

                    case ArgumentType.DownloadImgur: {
                        Config.Settings.General.Load();
                        Config.Settings.Imgur.Load();
                        Downloader.DownloadImgurAlbum(Arguments.ArgumentData, true);
                        Config.Settings.General.Save();
                        Config.Settings.Imgur.Save();
                        Config.Settings.FormSettings.Save();
                    } break;
                    #endregion

                    #region Push arguments
                    case ArgumentType.PushTags: {
                        Config.Settings.Load();
                        Log.Write("Tags are in the arguments, pushing to the main form.");
                        Application.Run(new frmMain(DownloadType.Tags, Arguments.ArgumentData));
                        Config.Settings.Save();
                    } break;

                    case ArgumentType.PushPools: {
                        Config.Settings.Load();
                        Log.Write("A pool is in the arguments, pushing to the main form.");
                        Application.Run(new frmMain(DownloadType.Pools, Arguments.ArgumentData));
                        Config.Settings.Save();
                    } break;

                    case ArgumentType.PushImages: {
                        Config.Settings.Load();
                        Log.Write("An image is in the arguments, pushing to the main form.");
                        Application.Run(new frmMain(DownloadType.Images, Arguments.ArgumentData));
                        Config.Settings.Save();
                    } break;

                    case ArgumentType.PushFurryBooru: {
                        Config.Settings.Load();
                        Log.Write("FurryBooru tags are in the arguments, pushing to the main form.");
                        Application.Run(new frmFurryBooruMain(Arguments.ArgumentData));
                        Config.Settings.Save();
                    } break;

                    case ArgumentType.PushInkBunny: {
                        Config.Settings.Load();
                        Log.Write("InkBunny terms are in the arguments, pushing to the main form.");
                        Application.Run(new frmInkBunnyMain(DownloadType.InkBunnyKeywords, Arguments.ArgumentData));
                        Config.Settings.Save();
                    } break;

                    case ArgumentType.PushImgur: {
                        Config.Settings.Load();
                        Log.Write("An imgur album is in the arguments, pushing to the main form.");
                        Application.Run(new frmImgurMain(Arguments.ArgumentData));
                        Config.Settings.Save();
                    } break;
                    #endregion

                    #region Wishlist
                    case ArgumentType.AddToPoolWishlist: {
                        // PoolWishlist does not need to be loaded.
                        Config.Settings.Pools.Load();
                        if (Arguments.ArgumentDataArray.Length == 2) {
                            PoolWishlist.AppendToWishlist(Arguments.ArgumentDataArray[0], Arguments.ArgumentDataArray[1]);
                        }
                        else if (Arguments.ArgumentDataArray.Length == 1) {
                            PoolWishlist.AppendToWishlist(Arguments.ArgumentDataArray[0], "");
                        }
                        else {
                            PoolWishlist.AppendToWishlist(Arguments.ArgumentData, "");
                        }
                        Config.Settings.FormSettings.Save();
                    } break;
                    #endregion

                    #region Show forms
                    case ArgumentType.ShowRedownloader: {
                        Config.Settings.Load();
                        Application.Run(new frmRedownloader());
                        Config.Settings.Save();
                    } break;

                    case ArgumentType.ShowBlacklist: {
                        Config.Settings.Load();
                        Application.Run(new frmBlacklist());
                        Config.Settings.Save();
                    } break;

                    case ArgumentType.ShowPoolWishlist: {
                        PoolWishlist.LoadWishlist();
                        Config.Settings.Pools.Load();
                        Application.Run(new frmPoolWishlist());
                    } break;

                    case ArgumentType.ShowFurryBooru: {
                        Config.Settings.Load();
                        Application.Run(new frmFurryBooruMain());
                        Config.Settings.Save();
                    } break;

                    case ArgumentType.ShowInkBunny: {
                        Config.Settings.Load();
                        Application.Run(new frmInkBunnyMain());
                        Config.Settings.Save();
                    } break;

                    case ArgumentType.ShowImgur: {
                        Config.Settings.Load();
                        Application.Run(new frmImgurMain());
                        Config.Settings.Save();
                    } break;
                    #endregion

                    #region Configure settings
                    case ArgumentType.ConfigureSettings:
                    case ArgumentType.ConfigureTagsSettings:
                    case ArgumentType.ConfigurePoolsSettings:
                    case ArgumentType.ConfigureImagesSettings:
                    case ArgumentType.ConfigureMiscSettings:
                    case ArgumentType.ConfigureProtocolSettings:
                    case ArgumentType.ConfigureSchemaSettings:
                    case ArgumentType.ConfigureImportExportSettings:
                    case ArgumentType.ConfigurePortableSettings: {
                        Config.Settings.Load();
                        using frmSettings Settings = new(Arguments.ArgumentType);
                        Settings.ShowDialog();
                        Config.Settings.Save();
                    } break;
                    #endregion

                    // We catch it and do nothing, because it was cancelled.
                    case ArgumentType.CancelledArgumentsDownload: {
                    } break;

                    default: {
                        Log.Write("No arguments were passed that didn't require the main form");
                        Config.Settings.Load();
                        Application.Run(new frmMain());
                        Config.Settings.Save();
                    } break;
                }
                Log.DisableLogging();
            }
            else {
                Log.MessageBox("Windows 7 or higher is required to run this application. Sorry.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Environment.ExitCode = 0;
            }
        }
    }
}