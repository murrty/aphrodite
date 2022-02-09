using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace aphrodite {
#pragma warning disable IDE0075 // Simplify conditional expression

    /// <summary>
    /// Main class that handles the program configuration.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    class Config {
        /// <summary>
        /// All settings used by the program.
        /// </summary>
        public static Config Settings;

        /// <summary>
        /// The pre-launch configurations.
        /// </summary>
        public Config_Initialization Initialization;
        /// <summary>
        /// The form-specific settings, locations and sizes.
        /// </summary>
        public Config_FormSettings FormSettings;
        /// <summary>
        /// General settings, used by all downloaders.
        /// </summary>
        public Config_General General;
        /// <summary>
        /// Tags settings, used by downloaders that download using Tags.
        /// </summary>
        public Config_Tags Tags;
        /// <summary>
        /// Pools settings, used by downloaders that download pools (groups of posts).
        /// </summary>
        public Config_Pools Pools;
        /// <summary>
        /// Images settings, used by downloaders that download single images.
        /// </summary>
        public Config_Images Images;
        /// <summary>
        /// InkBunny settings, used by the InkBunny downloader.
        /// </summary>
        public Config_InkBunny InkBunny;
        /// <summary>
        /// Imgur settings, used by the Imgur downloader.
        /// </summary>
        public Config_Imgur Imgur;

        /// <summary>
        /// The portable ini file used by the program.
        /// </summary>
        public IniFile Ini;
        /// <summary>
        /// Whether the program should use the portable ini.
        /// </summary>
        public bool UseIni = false;

        /// <summary>
        /// Initializes a new instance of the Config class.
        /// </summary>
        public Config() {
            Log.Write("Initializing Config");
            Log.Write(UseIni ? "UseIni is enabled, skipping internal settings" : "UseIni is disabled, using internal settings");
            Ini = new(Program.ApplicationPath + "\\aphrodite.ini", out UseIni);

            Initialization = new();
            FormSettings = new();
            General = new();
            Tags = new();
            Pools = new();
            Images = new();
            Imgur = new();
            InkBunny = new();
        }

        /// <summary>
        /// Initalizes the pre-launch configurations.
        /// </summary>
        public void Initialize() {
            Initialization.Load();
        }

        /// <summary>
        /// Loads all configurations.
        /// </summary>
        public void Load() {
            // Form settings is loaded by itself.
            //FormSettings = new();
            General.Load();
            Tags.Load();
            Pools.Load();
            Images.Load();
            Imgur.Load();
            InkBunny.Load();
            PoolWishlist.LoadWishlist();
        }

        /// <summary>
        /// Saves all configurations.
        /// </summary>
        public void Save() {
            Initialization.Save();
            FormSettings.Save();
            General.Save();
            Tags.Save();
            Pools.Save();
            Images.Save();
            Imgur.Save();
            InkBunny.Save();
        }

        /// <summary>
        /// Converts the configuration to either local settings or portable ini.
        /// </summary>
        /// <param name="UseIni">Whether the program will use the portable ini file.</param>
        public void ConvertConfig(bool UseIni) {
            if (this.UseIni != UseIni) {
                Settings.Ini.Write("UseIni", UseIni);
                this.UseIni = UseIni;
                Initialization.ForceSave();
                FormSettings.ForceSave();
                General.ForceSave();
                Tags.ForceSave();
                Pools.ForceSave();
                Images.ForceSave();
                Imgur.ForceSave();
                InkBunny.ForceSave();
                PoolWishlist.SaveWishlist();
            }
        }

        /// <summary>
        /// Whether the <paramref name="point"/> is a valid point.
        /// </summary>
        /// <param name="point">The <see cref="Point"/> to verify.</param>
        /// <returns>True if the point can be used in the program; otherwise, false.</returns>
        public static bool ValidPoint(Point point) =>
            point != null && point.X != -32000 && point.Y != -32000;

        /// <summary>
        /// Whether the <paramref name="size"/> is a valid size.
        /// </summary>
        /// <param name="size">The <see cref="Size"/> to verify.</param>
        /// <returns>True if the size can be used in the program; otherwise, false.</returns>
        public static bool ValidSize(Size size) =>
            size != null && size.Width > 0 && size.Height > 0;

    }

    /// <summary>
    /// Main Imgur configuration class that handles pre-launch settings.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class Config_Initialization {
        // No ConfigName used.

        #region Fields
        /// <summary>
        /// Whether the program is running for the first time.
        /// </summary>
        public bool FirstTime;
        /// <summary>
        /// Whether the program should skip showing/pushing data to the respective form, and should automatically start downloading.
        /// </summary>
        public bool AutoDownloadWithArguments;
        /// <summary>
        /// Whether the program should skip argument-checking and go straight into downloading.
        /// </summary>
        public bool SkipArgumentCheck;
        /// <summary>
        /// Whether the argument form should be top-most.
        /// </summary>
        public bool ArgumentFormTopMost;
        /// <summary>
        /// Whether the program should check for updates.
        /// </summary>
        public bool CheckForUpdates;
        /// <summary>
        /// Whether to check for beta versions of the program.
        /// </summary>
        public bool CheckForBetaUpdates;
        /// <summary>
        /// The decimal of the update that was skipped.
        /// </summary>
        public decimal SkippedVersion;
        /// <summary>
        /// The string of the beta update that was skipped.
        /// </summary>
        public string SkippedBetaVersion;

        private bool FirstTime_First;
        private bool AutoDownloadWithArguments_First;
        private bool SkipArgumentCheck_First;
        private bool ArgumentFormTopMost_First;
        private bool CheckForUpdates_First;
        private bool CheckForBetaUpdates_First;
        private decimal SkippedVersion_First;
        private string SkippedBetaVersion_First;
        #endregion

        /// <summary>
        /// Saves the configuration to the respective save location.
        /// </summary>
        public void Save() {
            Log.Write("Saving Config_Initialization");
            if (Config.Settings.UseIni) {
                if (FirstTime != FirstTime_First) {
                    Config.Settings.Ini.Write("FirstTime", FirstTime);
                    FirstTime_First = FirstTime;
                }
                if (SkipArgumentCheck != SkipArgumentCheck_First) {
                    Config.Settings.Ini.Write("SkipArgumentCheck", SkipArgumentCheck);
                    SkipArgumentCheck_First = SkipArgumentCheck;
                }
                if (AutoDownloadWithArguments != AutoDownloadWithArguments_First) {
                    Config.Settings.Ini.Write("AutoDownloadWithArguments", AutoDownloadWithArguments);
                    AutoDownloadWithArguments_First = AutoDownloadWithArguments;
                }
                if (ArgumentFormTopMost != ArgumentFormTopMost_First) {
                    Config.Settings.Ini.Write("ArgumentFormTopMost", ArgumentFormTopMost);
                    ArgumentFormTopMost_First = ArgumentFormTopMost;
                }
                if (CheckForUpdates != CheckForUpdates_First) {
                    Config.Settings.Ini.Write("CheckForUpdates", CheckForUpdates);
                    CheckForUpdates_First = CheckForUpdates;
                }
                if (CheckForBetaUpdates != CheckForBetaUpdates_First) {
                    Config.Settings.Ini.Write("CheckForBetaUpdates", CheckForBetaUpdates);
                    CheckForBetaUpdates_First = CheckForBetaUpdates;
                }
                if (SkippedVersion != SkippedVersion_First) {
                    Config.Settings.Ini.Write("SkippedVersion", SkippedVersion);
                    SkippedVersion_First = SkippedVersion;
                }
                if (SkippedBetaVersion != SkippedBetaVersion_First) {
                    Config.Settings.Ini.Write("SkippedBetaVersion", SkippedBetaVersion);
                    SkippedBetaVersion_First = SkippedBetaVersion;
                }
            }
            else {
                bool Save = false;

                if (aphrodite.Properties.Settings.Default.FirstTime != FirstTime) {
                    aphrodite.Properties.Settings.Default.FirstTime = FirstTime_First = FirstTime;
                    Save = true;
                }
                if (aphrodite.Properties.Settings.Default.SkipArgumentCheck != SkipArgumentCheck) {
                    aphrodite.Properties.Settings.Default.SkipArgumentCheck = SkipArgumentCheck_First = SkipArgumentCheck;
                    Save = true;
                }
                if (aphrodite.Properties.Settings.Default.AutoDownloadWithArguments != AutoDownloadWithArguments) {
                    aphrodite.Properties.Settings.Default.AutoDownloadWithArguments = AutoDownloadWithArguments_First = AutoDownloadWithArguments;
                    Save = true;
                }
                if (aphrodite.Properties.Settings.Default.ArgumentFormTopMost != ArgumentFormTopMost) {
                    aphrodite.Properties.Settings.Default.ArgumentFormTopMost = ArgumentFormTopMost_First = ArgumentFormTopMost;
                    Save = true;
                }
                if (aphrodite.Properties.Settings.Default.CheckForUpdates != CheckForUpdates) {
                    aphrodite.Properties.Settings.Default.CheckForUpdates = CheckForUpdates_First = CheckForUpdates;
                    Save = true;
                }
                if (aphrodite.Properties.Settings.Default.CheckForBetaUpdates != CheckForBetaUpdates) {
                    aphrodite.Properties.Settings.Default.CheckForBetaUpdates = CheckForBetaUpdates_First = CheckForBetaUpdates;
                    Save = true;
                }
                if (aphrodite.Properties.Settings.Default.SkippedVersion != SkippedVersion) {
                    aphrodite.Properties.Settings.Default.SkippedVersion = SkippedVersion_First = SkippedVersion;
                    Save = true;
                }
                if (aphrodite.Properties.Settings.Default.SkippedBetaVersion != SkippedBetaVersion) {
                    aphrodite.Properties.Settings.Default.SkippedBetaVersion = SkippedBetaVersion_First = SkippedBetaVersion;
                    Save = true;
                }

                if (Save) {
                    aphrodite.Settings.General.Default.Save();
                    aphrodite.Properties.Settings.Default.Save();
                }
            }
        }

        /// <summary>
        /// Loads the configuration from the respective save location.
        /// </summary>
        public void Load() {
            Log.Write("Loading Config_Initialization");

            if (Config.Settings.UseIni) {
                FirstTime = FirstTime_First = Config.Settings.Ini.KeyExists("FirstTime") ?
                    Config.Settings.Ini.ReadBool("FirstTime") : true;

                SkipArgumentCheck = SkipArgumentCheck_First = Config.Settings.Ini.KeyExists("SkipArgumentCheck") ?
                    Config.Settings.Ini.ReadBool("SkipArgumentCheck") : false;

                AutoDownloadWithArguments = AutoDownloadWithArguments_First = Config.Settings.Ini.KeyExists("AutoDownloadWithArguments") ?
                    Config.Settings.Ini.ReadBool("AutoDownloadWithArguments") : true;

                ArgumentFormTopMost = ArgumentFormTopMost_First = Config.Settings.Ini.KeyExists("ArgumentFormTopMost") ?
                    Config.Settings.Ini.ReadBool("ArgumentFormTopMost") : true;

                CheckForUpdates = CheckForUpdates_First = Config.Settings.Ini.KeyExists("CheckForUpdates") ?
                    Config.Settings.Ini.ReadBool("CheckForUpdates") : false;

                CheckForBetaUpdates = CheckForBetaUpdates_First = Config.Settings.Ini.KeyExists("CheckForBetaUpdates") ?
                    Config.Settings.Ini.ReadBool("CheckForBetaUpdates") : true;

                SkippedVersion = SkippedVersion_First = Config.Settings.Ini.KeyExists("SkippedVersion") ?
                    Config.Settings.Ini.ReadDecimal("SkippedVersion") : -1;

                SkippedBetaVersion = SkippedBetaVersion_First = Config.Settings.Ini.KeyExists("SkippedBetaVersion") ?
                    Config.Settings.Ini.ReadString("SkippedBetaVersion") : "";
            }
            else {
                FirstTime = FirstTime_First = aphrodite.Properties.Settings.Default.FirstTime;
                SkipArgumentCheck = SkipArgumentCheck_First = aphrodite.Properties.Settings.Default.SkipArgumentCheck;
                AutoDownloadWithArguments = AutoDownloadWithArguments_First = aphrodite.Properties.Settings.Default.AutoDownloadWithArguments;
                ArgumentFormTopMost = ArgumentFormTopMost_First = aphrodite.Properties.Settings.Default.ArgumentFormTopMost;
                CheckForUpdates = CheckForUpdates_First = aphrodite.Properties.Settings.Default.CheckForUpdates;
                CheckForBetaUpdates = CheckForBetaUpdates_First = aphrodite.Properties.Settings.Default.CheckForBetaUpdates;
                SkippedVersion = SkippedVersion_First = aphrodite.Properties.Settings.Default.SkippedVersion;
                SkippedBetaVersion = SkippedBetaVersion_First = aphrodite.Properties.Settings.Default.SkippedBetaVersion;
            }
        }

        /// <summary>
        /// Force-saves the configuration, overwriting any saved values.
        /// This method does not check for previous values.
        /// </summary>
        public void ForceSave() {
            Log.Write("Force-saving Config_Initialization");

            if (Config.Settings.UseIni) {
                Config.Settings.Ini.Write("firstTime", FirstTime);
                Config.Settings.Ini.Write("SkipArgumentCheck", SkipArgumentCheck);
                Config.Settings.Ini.Write("AutoDownloadWithArguments", AutoDownloadWithArguments);
                Config.Settings.Ini.Write("ArgumentFormTopMost", ArgumentFormTopMost);
                Config.Settings.Ini.Write("CheckForUpdates", CheckForUpdates);
                Config.Settings.Ini.Write("CheckForBetaUpdates", CheckForBetaUpdates);
                Config.Settings.Ini.Write("SkippedVersion", SkippedVersion);
                Config.Settings.Ini.Write("SkippedBetaVersion", SkippedBetaVersion);
            }
            else {
                aphrodite.Properties.Settings.Default.FirstTime = FirstTime;
                aphrodite.Properties.Settings.Default.SkipArgumentCheck = SkipArgumentCheck;
                aphrodite.Properties.Settings.Default.AutoDownloadWithArguments = AutoDownloadWithArguments;
                aphrodite.Properties.Settings.Default.ArgumentFormTopMost = ArgumentFormTopMost;
                aphrodite.Properties.Settings.Default.CheckForUpdates = CheckForUpdates;
                aphrodite.Properties.Settings.Default.CheckForBetaUpdates = CheckForBetaUpdates;
                aphrodite.Properties.Settings.Default.SkippedVersion = SkippedVersion;
                aphrodite.Properties.Settings.Default.SkippedBetaVersion = SkippedBetaVersion;
                aphrodite.Properties.Settings.Default.Save();
            }

            FirstTime_First = FirstTime;
            SkipArgumentCheck_First = SkipArgumentCheck;
            AutoDownloadWithArguments_First = AutoDownloadWithArguments;
            ArgumentFormTopMost_First = ArgumentFormTopMost;
            CheckForUpdates_First = CheckForUpdates;
            CheckForBetaUpdates_First = CheckForBetaUpdates;
            SkippedVersion_First = SkippedVersion;
            SkippedBetaVersion_First = SkippedBetaVersion;
        }
    }

    /// <summary>
    /// Main Imgur configuration class that handles form-related settings.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class Config_FormSettings {
        /// <summary>
        /// The ini section name of the current configuration class.
        /// </summary>
        private const string ConfigName = "FormSettings";

        #region Fields
        /// <summary>
        /// The previous location of the Blacklist form.
        /// </summary>
        public Point frmBlacklist_Location;
        /// <summary>
        /// The previous location of the image downloader form.
        /// </summary>
        public Point frmImageDownloader_Location;
        /// <summary>
        /// The previous location of the log form.
        /// </summary>
        public Point frmLog_Location;
        /// <summary>
        /// The previous size of the log form.
        /// </summary>
        public Size frmLog_Size;
        /// <summary>
        /// The previous location of the main form.
        /// </summary>
        public Point frmMain_Location;
        /// <summary>
        /// The previous location of the pool downloader form.
        /// </summary>
        public Point frmPoolDownloader_Location;
        /// <summary>
        /// The previous location of the pool wishlist form.
        /// </summary>
        public Point frmPoolWishlist_Location;
        /// <summary>
        /// The previous location of the redownloader form.
        /// </summary>
        public Point frmRedownloader_Location;
        /// <summary>
        /// The previous location of the settings form.
        /// </summary>
        public Point frmSettings_Location;
        /// <summary>
        /// The previous location of the tag downloader form.
        /// </summary>
        public Point frmTagDownloader_Location;
        /// <summary>
        /// The preivous location of the undesired tags form.
        /// </summary>
        public Point frmUndesiredTags_Location;
        /// <summary>
        /// The previous location of the argument dialog form.
        /// </summary>
        public Point frmArgument_Location;
        /// <summary>
        /// The previous location of the furry booru main form.
        /// </summary>
        public Point frmFurryBooruMain_Location;
        /// <summary>
        /// The previous location of the furry booru downloader form.
        /// </summary>
        public Point frmFurryBooruDownloader_Location;
        /// <summary>
        /// The previous location of the inkbunny main form.
        /// </summary>
        public Point frmInkBunnyMain_Location;
        /// <summary>
        /// The previous location of the inkbunny login form.
        /// </summary>
        public Point frmInkBunnyLogin_Location;
        /// <summary>
        /// The previous location of the inkbunny downloader form.
        /// </summary>
        public Point frmInkBunnyDownloader_Location;
        /// <summary>
        /// The previous location of the imgur main form.
        /// </summary>
        public Point frmImgurMain_Location;
        /// <summary>
        /// The previous location of the imgur login form.
        /// </summary>
        public Point frmImgurLogin_Location;
        /// <summary>
        /// The previous location of the imgur downloader form.
        /// </summary>
        public Point frmImgurDownloader_Location;

        private Point
            frmBlacklist_Location_First,
            frmImageDownloader_Location_First,
            frmLog_Location_First,
            frmMain_Location_First,
            frmPoolDownloader_Location_First,
            frmPoolWishlist_Location_First,
            frmRedownloader_Location_First,
            frmSettings_Location_First,
            frmTagDownloader_Location_First,
            frmUndesiredTags_Location_First,
            frmFurryBooruMain_Location_First,
            frmFurryBooruDownloader_Location_First,
            frmInkBunnyMain_Location_First,
            frmInkBunnyLogin_Location_First,
            frmInkBunnyDownloader_Location_First,
            frmImgurMain_Location_First,
            frmImgurLogin_Location_First,
            frmImgurDownloader_Location_First,
            frmArgument_Location_First;
        private Size frmLog_Size_First;
        #endregion

        /// <summary>
        /// Saves the configuration to the respective save location.
        /// </summary>
        public void Save() {
            Log.Write("Saving Config_FormSettings");

            if (Config.Settings.UseIni) {
                if (frmBlacklist_Location != frmBlacklist_Location_First) {
                    Config.Settings.Ini.Write("frmBlacklist_Location", frmBlacklist_Location, ConfigName);
                    frmBlacklist_Location_First = frmBlacklist_Location;
                }
                if (frmImageDownloader_Location != frmImageDownloader_Location_First) {
                    Config.Settings.Ini.Write("frmImageDownloader_Location", frmImageDownloader_Location, ConfigName);
                    frmImageDownloader_Location_First = frmImageDownloader_Location;
                }
                if (frmLog_Location != frmLog_Location_First) {
                    Config.Settings.Ini.Write("frmLog_Location", frmLog_Location, ConfigName);
                    frmLog_Location_First = frmLog_Location;
                }
                if (frmLog_Size != frmLog_Size_First) {
                    Config.Settings.Ini.Write("frmLog_Size", frmLog_Size, ConfigName);
                    frmLog_Size_First = frmLog_Size;
                }
                if (frmMain_Location != frmMain_Location_First) {
                    Config.Settings.Ini.Write("frmMain_Location", frmMain_Location, ConfigName);
                    frmMain_Location_First = frmMain_Location;
                }
                if (frmPoolDownloader_Location != frmPoolDownloader_Location_First) {
                    Config.Settings.Ini.Write("frmPoolDownloader_Location", frmPoolDownloader_Location, ConfigName);
                    frmPoolDownloader_Location_First = frmPoolDownloader_Location;
                }
                if (frmPoolWishlist_Location != frmPoolWishlist_Location_First) {
                    Config.Settings.Ini.Write("frmPoolWishlist_Location", frmPoolWishlist_Location, ConfigName);
                    frmPoolWishlist_Location_First = frmPoolWishlist_Location;
                }
                if (frmRedownloader_Location != frmRedownloader_Location_First) {
                    Config.Settings.Ini.Write("frmRedownloader_Location", frmRedownloader_Location, ConfigName);
                    frmRedownloader_Location_First = frmRedownloader_Location;
                }
                if (frmSettings_Location != frmSettings_Location_First) {
                    Config.Settings.Ini.Write("frmSettings_Location", frmSettings_Location, ConfigName);
                    frmSettings_Location_First = frmSettings_Location;
                }
                if (frmTagDownloader_Location != frmTagDownloader_Location_First) {
                    Config.Settings.Ini.Write("frmTagDownloader_Location", frmTagDownloader_Location, ConfigName);
                    frmTagDownloader_Location_First = frmTagDownloader_Location;
                }
                if (frmUndesiredTags_Location != frmUndesiredTags_Location_First) {
                    Config.Settings.Ini.Write("frmUndesiredTags_Location", frmUndesiredTags_Location, ConfigName);
                    frmUndesiredTags_Location_First = frmUndesiredTags_Location;
                }
                if (frmFurryBooruMain_Location != frmFurryBooruMain_Location_First) {
                    Config.Settings.Ini.Write("frmFurryBooruMain_Location", frmFurryBooruMain_Location, ConfigName);
                    frmFurryBooruMain_Location_First = frmFurryBooruMain_Location;
                }
                if (frmFurryBooruDownloader_Location != frmFurryBooruDownloader_Location_First) {
                    Config.Settings.Ini.Write("frmFurryBooruDownloader_Location", frmFurryBooruDownloader_Location, ConfigName);
                    frmFurryBooruDownloader_Location_First = frmFurryBooruDownloader_Location;
                }
                if (frmInkBunnyMain_Location != frmInkBunnyMain_Location_First) {
                    Config.Settings.Ini.Write("frmInkBunnyMain_Location", frmInkBunnyMain_Location, ConfigName);
                    frmInkBunnyMain_Location_First = frmInkBunnyMain_Location;
                }
                if (frmInkBunnyLogin_Location != frmInkBunnyLogin_Location_First) {
                    Config.Settings.Ini.Write("frmInkBunnyLogin_Location", frmInkBunnyLogin_Location, ConfigName);
                    frmInkBunnyLogin_Location_First = frmInkBunnyLogin_Location;
                }
                if (frmInkBunnyDownloader_Location != frmInkBunnyDownloader_Location_First) {
                    Config.Settings.Ini.Write("frmInkBunnyDownloader_Location", frmInkBunnyDownloader_Location, ConfigName);
                    frmInkBunnyDownloader_Location_First = frmInkBunnyDownloader_Location;
                }
                if (frmImgurMain_Location != frmImgurMain_Location_First) {
                    Config.Settings.Ini.Write("frmImgurMain_Location", frmImgurMain_Location, ConfigName);
                    frmImgurMain_Location_First = frmImgurMain_Location;
                }
                if (frmImgurLogin_Location != frmImgurLogin_Location_First) {
                    Config.Settings.Ini.Write("frmImgurLogin_Location", frmImgurLogin_Location, ConfigName);
                    frmImgurLogin_Location_First = frmImgurLogin_Location;
                }
                if (frmImgurDownloader_Location != frmImgurDownloader_Location_First) {
                    Config.Settings.Ini.Write("frmImgurDownloader_Location", frmImgurDownloader_Location, ConfigName);
                    frmImgurDownloader_Location_First = frmImgurDownloader_Location;
                }
                if (frmArgument_Location != frmArgument_Location_First) {
                    Config.Settings.Ini.Write("frmArgument_Location", frmArgument_Location, ConfigName);
                    frmArgument_Location_First = frmArgument_Location;
                }
            }
            else {
                bool Save = false;

                if (aphrodite.Settings.FormSettings.Default.frmBlacklist_Location != frmBlacklist_Location) {
                    aphrodite.Settings.FormSettings.Default.frmBlacklist_Location = frmBlacklist_Location_First = frmBlacklist_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmImageDownloader_Location != frmImageDownloader_Location) {
                    aphrodite.Settings.FormSettings.Default.frmImageDownloader_Location = frmImageDownloader_Location_First = frmImageDownloader_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmLog_Location != frmLog_Location) {
                    aphrodite.Settings.FormSettings.Default.frmLog_Location = frmLog_Location_First = frmLog_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmLog_Size != frmLog_Size) {
                    aphrodite.Settings.FormSettings.Default.frmLog_Size = frmLog_Size_First = frmLog_Size;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmMain_Location != frmMain_Location) {
                    aphrodite.Settings.FormSettings.Default.frmMain_Location = frmMain_Location_First = frmMain_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmPoolDownloader_Location != frmPoolDownloader_Location) {
                    aphrodite.Settings.FormSettings.Default.frmPoolDownloader_Location = frmPoolDownloader_Location_First = frmPoolDownloader_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmPoolWishlist_Location != frmPoolWishlist_Location) {
                    aphrodite.Settings.FormSettings.Default.frmPoolWishlist_Location = frmPoolWishlist_Location_First = frmPoolWishlist_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmRedownloader_Location != frmRedownloader_Location) {
                    aphrodite.Settings.FormSettings.Default.frmRedownloader_Location = frmRedownloader_Location_First = frmRedownloader_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmSettings_Location != frmSettings_Location) {
                    aphrodite.Settings.FormSettings.Default.frmSettings_Location = frmSettings_Location_First = frmSettings_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmTagDownloader_Location != frmTagDownloader_Location) {
                    aphrodite.Settings.FormSettings.Default.frmTagDownloader_Location = frmTagDownloader_Location_First = frmTagDownloader_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmUndesiredTags_Location != frmUndesiredTags_Location) {
                    aphrodite.Settings.FormSettings.Default.frmUndesiredTags_Location = frmUndesiredTags_Location_First = frmUndesiredTags_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmFurryBooruMain_Location != frmFurryBooruMain_Location) {
                    aphrodite.Settings.FormSettings.Default.frmFurryBooruMain_Location = frmFurryBooruMain_Location_First = frmFurryBooruMain_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmFurryBooruDownloader_Location != frmFurryBooruDownloader_Location) {
                    aphrodite.Settings.FormSettings.Default.frmFurryBooruDownloader_Location = frmFurryBooruDownloader_Location_First = frmFurryBooruDownloader_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmInkBunnyMain_Location != frmInkBunnyMain_Location) {
                    aphrodite.Settings.FormSettings.Default.frmInkBunnyMain_Location = frmInkBunnyMain_Location_First = frmInkBunnyMain_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmInkBunnyLogin_Location != frmInkBunnyLogin_Location) {
                    aphrodite.Settings.FormSettings.Default.frmInkBunnyLogin_Location = frmInkBunnyLogin_Location_First = frmInkBunnyLogin_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmInkBunnyDownloader_Location != frmInkBunnyDownloader_Location) {
                    aphrodite.Settings.FormSettings.Default.frmInkBunnyDownloader_Location = frmInkBunnyDownloader_Location_First = frmInkBunnyDownloader_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmImgurMain_Location != frmImgurMain_Location) {
                    aphrodite.Settings.FormSettings.Default.frmImgurMain_Location = frmImgurMain_Location_First = frmImgurMain_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmImgurLogin_Location != frmImgurLogin_Location) {
                    aphrodite.Settings.FormSettings.Default.frmImgurLogin_Location = frmImgurLogin_Location_First = frmImgurLogin_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmImgurDownloader_Location != frmImgurDownloader_Location) {
                    aphrodite.Settings.FormSettings.Default.frmImgurDownloader_Location = frmImgurDownloader_Location_First = frmImgurDownloader_Location;
                    Save = true;
                }
                if (aphrodite.Settings.FormSettings.Default.frmArgument_Location != frmArgument_Location) {
                    aphrodite.Settings.FormSettings.Default.frmArgument_Location = frmArgument_Location_First = frmArgument_Location;
                    Save = true;
                }

                if (Save) {
                    aphrodite.Settings.FormSettings.Default.Save();
                }
            }
        }

        /// <summary>
        /// Loads the configuration from the respective save location.
        /// </summary>
        public void Load() {
            Log.Write("Loading Config_FormSettings");

            if (Config.Settings.UseIni) {
                frmBlacklist_Location = frmBlacklist_Location_First = Config.Settings.Ini.KeyExists("frmBlacklist_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmBlacklist_Location", ConfigName) : new(-32000, -32000);

                frmImageDownloader_Location = frmImageDownloader_Location_First = Config.Settings.Ini.KeyExists("frmImageDownloader_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmImageDownloader_Location", ConfigName) : new(-32000, -32000);

                frmLog_Location = frmLog_Location_First = Config.Settings.Ini.KeyExists("frmLog_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmLog_Location", ConfigName) : new(-32000, -32000);

                frmLog_Size = frmLog_Size_First = Config.Settings.Ini.KeyExists("frmLog_Size", ConfigName) ?
                    Config.Settings.Ini.ReadSize("frmLog_Size", ConfigName) : new(-32000, -32000);

                frmMain_Location = frmMain_Location_First = Config.Settings.Ini.KeyExists("frmMain_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmMain_Location", ConfigName) : new(-32000, -32000);

                frmPoolDownloader_Location = frmPoolDownloader_Location_First = Config.Settings.Ini.KeyExists("frmPoolDownloader_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmPoolDownloader_Location", ConfigName) : new(-32000, -32000);

                frmPoolWishlist_Location = frmPoolWishlist_Location_First = Config.Settings.Ini.KeyExists("frmPoolWishlist_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmPoolWishlist_Location", ConfigName) : new(-32000, -32000);

                frmRedownloader_Location = frmRedownloader_Location_First = Config.Settings.Ini.KeyExists("frmRedownloader_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmRedownloader_Location", ConfigName) : new(-32000, -32000);

                frmSettings_Location = frmSettings_Location_First = Config.Settings.Ini.KeyExists("frmSettings_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmSettings_Location", ConfigName) : new(-32000, -32000);

                frmTagDownloader_Location = frmTagDownloader_Location_First = Config.Settings.Ini.KeyExists("frmTagDownloader_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmTagDownloader_Location", ConfigName) : new(-32000, -32000);

                frmUndesiredTags_Location = frmUndesiredTags_Location_First = Config.Settings.Ini.KeyExists("frmUndesiredTags_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmUndesiredTags_Location", ConfigName) : new(-32000, -32000);

                frmFurryBooruMain_Location = frmFurryBooruMain_Location_First = Config.Settings.Ini.KeyExists("frmFurryBooruMain_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmFurryBooruMain_Location", ConfigName) : new(-32000, -32000);

                frmFurryBooruDownloader_Location = frmFurryBooruDownloader_Location_First = Config.Settings.Ini.KeyExists("frmFurryBooruDownloader_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmFurryBooruDownloader_Location", ConfigName) : new(-32000, -32000);

                frmInkBunnyMain_Location = frmInkBunnyMain_Location_First = Config.Settings.Ini.KeyExists("frmInkBunnyMain_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmInkBunnyMain_Location", ConfigName) : new(-32000, -32000);

                frmInkBunnyLogin_Location = frmInkBunnyLogin_Location_First = Config.Settings.Ini.KeyExists("frmInkBunnyLogin_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmInkBunnyLogin_Location", ConfigName) : new(-32000, -32000);

                frmInkBunnyDownloader_Location = frmInkBunnyDownloader_Location_First = Config.Settings.Ini.KeyExists("frmInkBunnyDownloader_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmInkBunnyDownloader_Location", ConfigName) : new(-32000, -32000);

                frmImgurMain_Location = frmImgurMain_Location_First = Config.Settings.Ini.KeyExists("frmImgurMain_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmImgurMain_Location", ConfigName) : new(-32000, -32000);

                frmImgurLogin_Location = frmImgurLogin_Location_First = Config.Settings.Ini.KeyExists("frmImgurLogin_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmImgurLogin_Location", ConfigName) : new(-32000, -32000);

                frmImgurDownloader_Location = frmImgurDownloader_Location_First = Config.Settings.Ini.KeyExists("frmImgurDownloader_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmImgurDownloader_Location", ConfigName) : new(-32000, -32000);

                frmArgument_Location = frmArgument_Location_First = Config.Settings.Ini.KeyExists("frmArgumentDialog_Location", ConfigName) ?
                    Config.Settings.Ini.ReadPoint("frmArgument_Location", ConfigName) : new(-32000, -32000);
            }
            else {
                frmBlacklist_Location = frmBlacklist_Location_First = aphrodite.Settings.FormSettings.Default.frmBlacklist_Location;
                frmImageDownloader_Location = frmImageDownloader_Location_First = aphrodite.Settings.FormSettings.Default.frmImageDownloader_Location;
                frmLog_Location = frmLog_Location_First = aphrodite.Settings.FormSettings.Default.frmLog_Location;
                frmLog_Size = frmLog_Size_First = aphrodite.Settings.FormSettings.Default.frmLog_Size;
                frmMain_Location = frmMain_Location_First = aphrodite.Settings.FormSettings.Default.frmMain_Location;
                frmPoolDownloader_Location = frmPoolDownloader_Location_First = aphrodite.Settings.FormSettings.Default.frmPoolDownloader_Location;
                frmPoolWishlist_Location = frmPoolWishlist_Location_First = aphrodite.Settings.FormSettings.Default.frmPoolWishlist_Location;
                frmRedownloader_Location = frmRedownloader_Location_First = aphrodite.Settings.FormSettings.Default.frmRedownloader_Location;
                frmSettings_Location = frmSettings_Location_First = aphrodite.Settings.FormSettings.Default.frmSettings_Location;
                frmTagDownloader_Location = frmTagDownloader_Location_First = aphrodite.Settings.FormSettings.Default.frmTagDownloader_Location;
                frmUndesiredTags_Location = frmUndesiredTags_Location_First = aphrodite.Settings.FormSettings.Default.frmUndesiredTags_Location;
                frmFurryBooruMain_Location = frmFurryBooruMain_Location_First = aphrodite.Settings.FormSettings.Default.frmFurryBooruMain_Location;
                frmFurryBooruDownloader_Location = frmFurryBooruDownloader_Location_First = aphrodite.Settings.FormSettings.Default.frmFurryBooruDownloader_Location;
                frmInkBunnyMain_Location = frmInkBunnyMain_Location_First = aphrodite.Settings.FormSettings.Default.frmInkBunnyMain_Location;
                frmInkBunnyLogin_Location = frmInkBunnyLogin_Location_First = aphrodite.Settings.FormSettings.Default.frmInkBunnyLogin_Location;
                frmInkBunnyDownloader_Location = frmInkBunnyDownloader_Location_First = aphrodite.Settings.FormSettings.Default.frmInkBunnyDownloader_Location;
                frmImgurMain_Location = frmImgurMain_Location_First = aphrodite.Settings.FormSettings.Default.frmImgurMain_Location;
                frmImgurLogin_Location = frmImgurLogin_Location_First = aphrodite.Settings.FormSettings.Default.frmImgurLogin_Location;
                frmImgurDownloader_Location = frmImgurDownloader_Location_First = aphrodite.Settings.FormSettings.Default.frmImgurDownloader_Location;
                frmArgument_Location = frmArgument_Location_First = aphrodite.Settings.FormSettings.Default.frmArgument_Location;
            }
        }

        /// <summary>
        /// Force-saves the configuration, overwriting any saved values.
        /// This method does not check for previous values.
        /// </summary>
        public void ForceSave() {
            Log.Write("Force saving Config_FormSettings");

            if (Config.Settings.UseIni) {
                Config.Settings.Ini.Write("frmBlacklist_Location", frmBlacklist_Location, ConfigName);
                Config.Settings.Ini.Write("frmImageDownloader_Location", frmImageDownloader_Location, ConfigName);
                Config.Settings.Ini.Write("frmLog_Location", frmLog_Location, ConfigName);
                Config.Settings.Ini.Write("frmLog_Size", frmLog_Size, ConfigName);
                Config.Settings.Ini.Write("frmMain_Location", frmMain_Location, ConfigName);
                Config.Settings.Ini.Write("frmPoolDownloader_Location", frmPoolDownloader_Location, ConfigName);
                Config.Settings.Ini.Write("frmPoolWishlist_Location", frmPoolWishlist_Location, ConfigName);
                Config.Settings.Ini.Write("frmRedownloader_Location", frmRedownloader_Location, ConfigName);
                Config.Settings.Ini.Write("frmSettings_Location", frmSettings_Location, ConfigName);
                Config.Settings.Ini.Write("frmTagDownloader_Location", frmTagDownloader_Location, ConfigName);
                Config.Settings.Ini.Write("frmUndesiredTags_Location", frmUndesiredTags_Location, ConfigName);
                Config.Settings.Ini.Write("frmFurryBooruMain_Location", frmFurryBooruMain_Location, ConfigName);
                Config.Settings.Ini.Write("frmFurryBooruDownloader_Location", frmFurryBooruDownloader_Location, ConfigName);
                Config.Settings.Ini.Write("frmInkBunnyMain_Location", frmInkBunnyMain_Location, ConfigName);
                Config.Settings.Ini.Write("frmInkBunnyLogin_Location", frmInkBunnyLogin_Location, ConfigName);
                Config.Settings.Ini.Write("frmInkBunnyDownloader_Location", frmInkBunnyDownloader_Location, ConfigName);
                Config.Settings.Ini.Write("frmImgurMain_Location", frmImgurMain_Location, ConfigName);
                Config.Settings.Ini.Write("frmImgurLogin_Location", frmImgurLogin_Location, ConfigName);
                Config.Settings.Ini.Write("frmImgurDownloader_Location", frmImgurDownloader_Location, ConfigName);
                Config.Settings.Ini.Write("frmArgument_Location", frmArgument_Location, ConfigName);
            }
            else {
                aphrodite.Settings.FormSettings.Default.frmBlacklist_Location = frmBlacklist_Location;
                aphrodite.Settings.FormSettings.Default.frmImageDownloader_Location = frmImageDownloader_Location;
                aphrodite.Settings.FormSettings.Default.frmLog_Location = frmLog_Location;
                aphrodite.Settings.FormSettings.Default.frmLog_Size = frmLog_Size;
                aphrodite.Settings.FormSettings.Default.frmMain_Location = frmMain_Location;
                aphrodite.Settings.FormSettings.Default.frmPoolDownloader_Location = frmPoolDownloader_Location;
                aphrodite.Settings.FormSettings.Default.frmPoolWishlist_Location = frmPoolWishlist_Location;
                aphrodite.Settings.FormSettings.Default.frmRedownloader_Location = frmRedownloader_Location;
                aphrodite.Settings.FormSettings.Default.frmSettings_Location = frmSettings_Location;
                aphrodite.Settings.FormSettings.Default.frmTagDownloader_Location = frmTagDownloader_Location;
                aphrodite.Settings.FormSettings.Default.frmUndesiredTags_Location = frmUndesiredTags_Location;
                aphrodite.Settings.FormSettings.Default.frmFurryBooruMain_Location = frmFurryBooruMain_Location;
                aphrodite.Settings.FormSettings.Default.frmFurryBooruDownloader_Location = frmFurryBooruDownloader_Location;
                aphrodite.Settings.FormSettings.Default.frmInkBunnyMain_Location = frmInkBunnyMain_Location;
                aphrodite.Settings.FormSettings.Default.frmInkBunnyLogin_Location = frmInkBunnyLogin_Location;
                aphrodite.Settings.FormSettings.Default.frmInkBunnyDownloader_Location = frmInkBunnyDownloader_Location;
                aphrodite.Settings.FormSettings.Default.frmImgurMain_Location = frmImgurMain_Location;
                aphrodite.Settings.FormSettings.Default.frmImgurLogin_Location = frmImgurLogin_Location;
                aphrodite.Settings.FormSettings.Default.frmImgurDownloader_Location = frmImgurDownloader_Location;
                aphrodite.Settings.FormSettings.Default.frmArgument_Location = frmArgument_Location;
                aphrodite.Settings.FormSettings.Default.Save();
            }

            frmBlacklist_Location_First = frmBlacklist_Location;
            frmImageDownloader_Location_First = frmImageDownloader_Location;
            frmLog_Location_First = frmLog_Location;
            frmLog_Size_First = frmLog_Size;
            frmMain_Location_First = frmMain_Location;
            frmPoolDownloader_Location_First = frmPoolDownloader_Location;
            frmPoolWishlist_Location_First = frmPoolWishlist_Location;
            frmRedownloader_Location_First = frmRedownloader_Location;
            frmSettings_Location_First = frmSettings_Location;
            frmTagDownloader_Location_First = frmTagDownloader_Location;
            frmUndesiredTags_Location_First = frmUndesiredTags_Location;
            frmFurryBooruMain_Location_First = frmFurryBooruMain_Location;
            frmFurryBooruDownloader_Location_First = frmFurryBooruDownloader_Location;
            frmInkBunnyMain_Location_First = frmInkBunnyMain_Location;
            frmInkBunnyLogin_Location_First = frmInkBunnyLogin_Location;
            frmInkBunnyDownloader_Location_First = frmInkBunnyDownloader_Location;
            frmImgurMain_Location_First = frmImgurMain_Location;
            frmImgurLogin_Location_First = frmImgurLogin_Location;
            frmImgurDownloader_Location_First = frmImgurDownloader_Location;
            frmArgument_Location_First = frmArgument_Location;
        }

    }

    /// <summary>
    /// Main Imgur configuration class that handles general settings, either program or download.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class Config_General {
        /// <summary>
        /// The ini section name of the current configuration class.
        /// </summary>
        private const string ConfigName = "General";
        /// <summary>
        /// The path of the grayist file, if portable mode is active.
        /// </summary>
        public static readonly string GraylistFile = $"{Program.ApplicationPath}\\graylist.cfg";
        /// <summary>
        /// The path of the blacklist file, if portable mode is active.
        /// </summary>
        public static readonly string BlacklistFile = $"{Program.ApplicationPath}\\blacklist.cfg";

        #region Fields
        /// <summary>
        /// The path of where files will be downloaded to.
        /// </summary>
        public string saveLocation;
        /// <summary>
        /// The graylisted tags or terms.
        /// </summary>
        public string Graylist;
        /// <summary>
        /// The blacklisted tags or terms.
        /// </summary>
        public string Blacklist;
        /// <summary>
        /// The tags or terms that could be considered invalid for artist names.
        /// </summary>
        public string undesiredTags;
        /// <summary>
        /// Whether graylisted tags or terms will be downloaded.
        /// </summary>
        public bool saveGraylisted;
        /// <summary>
        /// Whether information relating to the download and the files will be saved to an nfo file in the download path.
        /// </summary>
        public bool saveInfo;
        /// <summary>
        /// Whether when the download finish, the form should close.
        /// </summary>
        public bool ignoreFinish;
        /// <summary>
        /// Whether when the download finishes successfully, the download path should open.
        /// </summary>
        public bool openAfter;

        private string
            saveLocation_First,
            Graylist_First,
            Blacklist_First,
            undesiredTags_First;
        private bool
            saveGraylisted_First,
            saveInfo_First,
            ignoreFinish_First,
            openAfter_First;
        #endregion

        /// <summary>
        /// Saves the configuration to the respective save location.
        /// </summary>
        public void Save() {
            Log.Write("Saving Config_General");

            if (Config.Settings.UseIni) {
                if (saveLocation != saveLocation_First) {
                    Config.Settings.Ini.Write("saveLocation", saveLocation, ConfigName);
                    saveLocation_First = saveLocation;
                }
                if (Graylist != Graylist_First) {
                    if (Graylist.Length > 0) {
                        File.WriteAllText(GraylistFile, Graylist.Replace(" ", "\n").Replace(" ", "\n"));
                    }
                    else {
                        File.Delete(GraylistFile);
                    }
                    Graylist_First = Graylist;
                }
                if (Blacklist != Blacklist_First) {
                    if (Graylist.Length > 0) {
                        File.WriteAllText(BlacklistFile, Blacklist.Replace(" ", "\r\n").Replace(" ", "\n"));
                    }
                    else {
                        File.Delete(BlacklistFile);
                    }
                    Blacklist_First = Blacklist;
                }
                if (saveGraylisted != saveGraylisted_First) {
                    Config.Settings.Ini.Write("saveGraylisted", saveGraylisted, ConfigName);
                    saveGraylisted_First = saveGraylisted;
                }
                if (saveInfo != saveInfo_First) {
                    Config.Settings.Ini.Write("saveInfo", saveInfo, ConfigName);
                    saveInfo_First = saveInfo;
                }
                if (ignoreFinish != ignoreFinish_First) {
                    Config.Settings.Ini.Write("ignoreFinish", ignoreFinish, ConfigName);
                    ignoreFinish_First = ignoreFinish;
                }
                if (undesiredTags != undesiredTags_First) {
                    Config.Settings.Ini.Write("undesiredTags", undesiredTags, ConfigName);
                    undesiredTags_First = undesiredTags;
                }
                if (openAfter != openAfter_First) {
                    Config.Settings.Ini.Write("openAfter", openAfter, ConfigName);
                    openAfter_First = openAfter;
                }
            }
            else {
                bool Save = false;

                if (aphrodite.Settings.General.Default.saveLocation != saveLocation) {
                    aphrodite.Settings.General.Default.saveLocation = saveLocation_First = saveLocation;
                    Save = true;
                }
                if (aphrodite.Settings.General.Default.Graylist != Graylist) {
                    aphrodite.Settings.General.Default.Graylist = Graylist_First = Graylist;
                    Save = true;
                }
                if (aphrodite.Settings.General.Default.Blacklist != Blacklist) {
                    aphrodite.Settings.General.Default.Blacklist = Blacklist_First = Blacklist;
                    Save = true;
                }
                if (aphrodite.Settings.General.Default.saveGraylisted != saveGraylisted) {
                    aphrodite.Settings.General.Default.saveGraylisted = saveGraylisted_First = saveGraylisted;
                    Save = true;
                }
                if (aphrodite.Settings.General.Default.saveInfo != saveInfo) {
                    aphrodite.Settings.General.Default.saveInfo = saveInfo_First = saveInfo;
                    Save = true;
                }
                if (aphrodite.Settings.General.Default.ignoreFinish != ignoreFinish) {
                    aphrodite.Settings.General.Default.ignoreFinish = ignoreFinish_First = ignoreFinish;
                    Save = true;
                }
                if (aphrodite.Settings.General.Default.undesiredTags != undesiredTags) {
                    aphrodite.Settings.General.Default.undesiredTags = undesiredTags_First = undesiredTags;
                    Save = true;
                }
                if (aphrodite.Settings.General.Default.openAfter != openAfter) {
                    aphrodite.Settings.General.Default.openAfter = openAfter_First = openAfter;
                    Save = true;
                }

                if (Save) {
                    Log.Write("Saving General");
                    aphrodite.Settings.General.Default.Save();
                }
            }
        }

        /// <summary>
        /// Loads the configuration from the respective save location.
        /// </summary>
        public void Load() {
            Log.Write("Loading Config_General");
            if (Config.Settings.UseIni) {
                saveLocation = saveLocation_First = Config.Settings.Ini.KeyExists("saveLocation", ConfigName) ?
                    Config.Settings.Ini.ReadString("saveLocation", ConfigName) : string.Empty;

                Graylist = Graylist_First = File.Exists(GraylistFile) && new FileInfo(GraylistFile).Length > 0 ?
                    string.Join(" ", File.ReadAllLines(GraylistFile)) : string.Empty;

                Blacklist = Blacklist_First = File.Exists(BlacklistFile) && new FileInfo(BlacklistFile).Length > 0 ?
                    string.Join(" ", File.ReadAllLines(BlacklistFile)) : string.Empty;

                undesiredTags = undesiredTags_First = Config.Settings.Ini.KeyExists("undesiredTags", ConfigName) ?
                    Config.Settings.Ini.ReadString("undesiredTags", ConfigName) : string.Empty;

                saveGraylisted = saveGraylisted_First = Config.Settings.Ini.KeyExists("saveGraylisted", ConfigName) ?
                    Config.Settings.Ini.ReadBool("saveGraylisted", ConfigName) : true;

                saveInfo = saveInfo_First = Config.Settings.Ini.KeyExists("saveInfo", ConfigName) ?
                    Config.Settings.Ini.ReadBool("saveInfo", ConfigName) : true;

                ignoreFinish = ignoreFinish_First = Config.Settings.Ini.KeyExists("ignoreFinish", ConfigName) ?
                    Config.Settings.Ini.ReadBool("ignoreFinish", ConfigName) : false;

                openAfter = openAfter_First = Config.Settings.Ini.KeyExists("openAfter", ConfigName) ?
                    Config.Settings.Ini.ReadBool("openAfter", ConfigName) : false;
            }
            else {
                saveLocation = saveLocation_First = aphrodite.Settings.General.Default.saveLocation;
                Graylist = Graylist_First = aphrodite.Settings.General.Default.Graylist;
                Blacklist = Blacklist_First = aphrodite.Settings.General.Default.Blacklist;
                undesiredTags = undesiredTags_First = aphrodite.Settings.General.Default.undesiredTags;
                saveGraylisted = saveGraylisted_First = aphrodite.Settings.General.Default.saveGraylisted;
                saveInfo = saveInfo_First = aphrodite.Settings.General.Default.saveInfo;
                ignoreFinish = ignoreFinish_First = aphrodite.Settings.General.Default.ignoreFinish;
                openAfter = openAfter_First = aphrodite.Settings.General.Default.openAfter;
            }
        }

        /// <summary>
        /// Force-saves the configuration, overwriting any saved values.
        /// This method does not check for previous values.
        /// </summary>
        public void ForceSave() {
            Log.Write("Force saving Config_General");
            if (Config.Settings.UseIni) {
                Config.Settings.Ini.Write("saveLocation", saveLocation, ConfigName);
                Config.Settings.Ini.Write("saveGraylisted", saveGraylisted, ConfigName);
                if (Graylist.Length > 0) {
                    File.WriteAllText(GraylistFile, Graylist.Replace(" ", "\r\n"));
                }
                else {
                    File.Delete(GraylistFile);
                }
                if (Blacklist.Length > 0) {
                    File.WriteAllText(BlacklistFile, Blacklist.Replace(" ", "\r\n"));
                }
                else {
                    File.Delete(BlacklistFile);
                }
                Config.Settings.Ini.Write("undesiredTags", undesiredTags, ConfigName);
                Config.Settings.Ini.Write("saveInfo", saveInfo, ConfigName);
                Config.Settings.Ini.Write("ignoreFinish", ignoreFinish, ConfigName);
                Config.Settings.Ini.Write("openAfter", openAfter, ConfigName);
            }
            else {
                aphrodite.Settings.General.Default.saveLocation = saveLocation;
                aphrodite.Settings.General.Default.Graylist = Graylist;
                aphrodite.Settings.General.Default.Blacklist = Blacklist;
                aphrodite.Settings.General.Default.undesiredTags = undesiredTags;
                aphrodite.Settings.General.Default.saveGraylisted = saveGraylisted;
                aphrodite.Settings.General.Default.saveInfo = saveInfo;
                aphrodite.Settings.General.Default.ignoreFinish = ignoreFinish;
                aphrodite.Settings.General.Default.openAfter = openAfter;

                aphrodite.Settings.General.Default.Save();
            }
            saveLocation_First = saveLocation;
            Graylist_First = Graylist;
            Blacklist_First = Blacklist;
            undesiredTags_First = undesiredTags;
            saveGraylisted_First = saveGraylisted;
            saveInfo_First = saveInfo;
            ignoreFinish_First = ignoreFinish;
            openAfter_First = openAfter;
        }
    }

    /// <summary>
    /// Main Imgur configuration class that handles Tag-related settings.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class Config_Tags {
        /// <summary>
        /// The ini section name of the current configuration class.
        /// </summary>
        private const string ConfigName = "Tags";

        #region Fields
        /// <summary>
        /// Whether safe-rated files should be downloaded.
        /// </summary>
        public bool Safe;
        /// <summary>
        /// Whether questionable-rated files should be downloaded.
        /// </summary>
        public bool Questionable;
        /// <summary>
        /// Whether explicit-rated files should be downloaded.
        /// </summary>
        public bool Explicit;
        /// <summary>
        /// Whether files should be separated into their own folders based on their rating.
        /// </summary>
        public bool separateRatings;
        /// <summary>
        /// Whether non-image files should be separated into their own folders.
        /// </summary>
        public bool separateNonImages;
        /// <summary>
        /// Whether the minimum score should be used.
        /// <para>This is an option because scores can be below 0.</para>
        /// </summary>
        public bool enableScoreMin;
        /// <summary>
        /// Whether the minimum score should be used as a tag (if there are less than 6 search tags).
        /// <para>This is yielded to <see cref="FavoriteCountAsTag"/> if there is not enough space for both.</para>
        /// </summary>
        public bool scoreAsTag;
        /// <summary>
        /// Whether blacklisted files should be downloaded.
        /// </summary>
        public bool downloadBlacklisted;
        /// <summary>
        /// Whether the files should be downloaded in newest-to-oldest.
        /// If <see cref="separateRatings"/> is true, this setting is redundant.
        /// </summary>
        public bool DownloadNewestToOldest;
        /// <summary>
        /// Whether the favorite count should be used as a tag (if there are less than 6 search tags).
        /// <para>This takes precedent over <see cref="scoreAsTag"/> if there is not enough space for both.</para>
        /// </summary>
        public bool FavoriteCountAsTag;
        /// <summary>
        /// The minimum score of the files to download.
        /// </summary>
        public int scoreMin;
        /// <summary>
        /// The limit of how many images are to be downloaded. 0 is off.
        /// </summary>
        public int imageLimit;
        /// <summary>
        /// The limit of how many pages are to be parsed. 0 is off.
        /// </summary>
        public int pageLimit;
        /// <summary>
        /// The minimum favorite count of the files to download.
        /// </summary>
        public int FavoriteCount;
        /// <summary>
        /// The file-name schema used to translate file-specific data into the file name.
        /// </summary>
        public string fileNameSchema;

        private bool
            Safe_First,
            Questionable_First,
            Explicit_First,
            separateRatings_First,
            separateNonImages_First,
            enableScoreMin_First,
            scoreAsTag_First,
            downloadBlacklisted_First,
            DownloadNewestToOldest_First,
            FavoriteCountAsTag_First;
        private int
            scoreMin_First,
            imageLimit_First,
            pageLimit_First,
            FavoriteCount_First;
        private string
            fileNameSchema_First;
        #endregion

        /// <summary>
        /// Saves the configuration to the respective save location.
        /// </summary>
        public void Save() {
            Log.Write("Saving Config_Tags");

            if (Config.Settings.UseIni) {
                if (Safe != Safe_First) {
                    Config.Settings.Ini.Write("Safe", Safe, ConfigName);
                    Safe_First = Safe;
                }
                if (Questionable != Questionable_First) {
                    Config.Settings.Ini.Write("Questionable", Questionable, ConfigName);
                    Questionable_First = Questionable;
                }
                if (Explicit != Explicit_First) {
                    Config.Settings.Ini.Write("Explicit", Explicit, ConfigName);
                    Explicit_First = Explicit;
                }
                if (separateRatings != separateRatings_First) {
                    Config.Settings.Ini.Write("separateRatings", separateRatings, ConfigName);
                    separateRatings_First = separateRatings;
                }
                if (separateNonImages != separateNonImages_First) {
                    Config.Settings.Ini.Write("separateNonImages", separateNonImages, ConfigName);
                    separateNonImages_First = separateNonImages;
                }
                if (enableScoreMin != enableScoreMin_First) {
                    Config.Settings.Ini.Write("enableScoreMin", enableScoreMin, ConfigName);
                    enableScoreMin_First = enableScoreMin;
                }
                if (scoreAsTag != scoreAsTag_First) {
                    Config.Settings.Ini.Write("scoreAsTag", scoreAsTag, ConfigName);
                    scoreAsTag_First = scoreAsTag;
                }
                if (scoreMin != scoreMin_First) {
                    Config.Settings.Ini.Write("scoreMin", scoreMin, ConfigName);
                    scoreMin_First = scoreMin;
                }
                if (imageLimit != imageLimit_First) {
                    Config.Settings.Ini.Write("imageLimit", imageLimit, ConfigName);
                    imageLimit_First = imageLimit;
                }
                if (pageLimit != pageLimit_First) {
                    Config.Settings.Ini.Write("pageLimit", pageLimit, ConfigName);
                    pageLimit_First = pageLimit;
                }
                if (fileNameSchema != fileNameSchema_First) {
                    Config.Settings.Ini.Write("fileNameSchema", fileNameSchema, ConfigName);
                    fileNameSchema_First = fileNameSchema;
                }
                if (downloadBlacklisted != downloadBlacklisted_First) {
                    Config.Settings.Ini.Write("downloadBlacklisted", downloadBlacklisted, ConfigName);
                    downloadBlacklisted_First = downloadBlacklisted;
                }
                if (DownloadNewestToOldest != DownloadNewestToOldest_First) {
                    Config.Settings.Ini.Write("DownloadNewestToOldest", DownloadNewestToOldest, ConfigName);
                    DownloadNewestToOldest_First = DownloadNewestToOldest;
                }
                if (FavoriteCount != FavoriteCount_First) {
                    Config.Settings.Ini.Write("FavoriteCount", FavoriteCount, ConfigName);
                    FavoriteCount_First = FavoriteCount;
                }
                if (FavoriteCountAsTag != FavoriteCountAsTag_First) {
                    Config.Settings.Ini.Write("FavoriteCountAsTag", FavoriteCountAsTag, ConfigName);
                    FavoriteCountAsTag_First = FavoriteCountAsTag;
                }
            }
            else {
                bool Save = false;

                if (aphrodite.Settings.Tags.Default.Safe != Safe) {
                    aphrodite.Settings.Tags.Default.Safe = Safe_First = Safe;
                    Save = true;
                }
                if (aphrodite.Settings.Tags.Default.Questionable != Questionable) {
                    aphrodite.Settings.Tags.Default.Questionable = Questionable_First = Questionable;
                    Save = true;
                }
                if (aphrodite.Settings.Tags.Default.Explicit != Explicit) {
                    aphrodite.Settings.Tags.Default.Explicit = Explicit_First = Explicit;
                    Save = true;
                }
                if (aphrodite.Settings.Tags.Default.separateRatings != separateRatings) {
                    aphrodite.Settings.Tags.Default.separateRatings = separateRatings_First = separateRatings;
                    Save = true;
                }
                if (aphrodite.Settings.Tags.Default.separateNonImages != separateNonImages) {
                    aphrodite.Settings.Tags.Default.separateNonImages = separateNonImages_First = separateNonImages;
                    Save = true;
                }
                if (aphrodite.Settings.Tags.Default.enableScoreMin != enableScoreMin) {
                    aphrodite.Settings.Tags.Default.enableScoreMin = enableScoreMin_First = enableScoreMin;
                    Save = true;
                }
                if (aphrodite.Settings.Tags.Default.scoreAsTag != scoreAsTag) {
                    aphrodite.Settings.Tags.Default.scoreAsTag = scoreAsTag_First = scoreAsTag;
                    Save = true;
                }
                if (aphrodite.Settings.Tags.Default.scoreMin != scoreMin) {
                    aphrodite.Settings.Tags.Default.scoreMin = scoreMin_First = scoreMin;
                    Save = true;
                }
                if (aphrodite.Settings.Tags.Default.imageLimit != imageLimit) {
                    aphrodite.Settings.Tags.Default.imageLimit = imageLimit_First = imageLimit;
                    Save = true;
                }
                if (aphrodite.Settings.Tags.Default.pageLimit != pageLimit) {
                    aphrodite.Settings.Tags.Default.pageLimit = pageLimit_First = pageLimit;
                    Save = true;
                }
                if (aphrodite.Settings.Tags.Default.fileNameSchema != fileNameSchema) {
                    aphrodite.Settings.Tags.Default.fileNameSchema = fileNameSchema_First = fileNameSchema;
                    Save = true;
                }
                if (aphrodite.Settings.Tags.Default.downloadBlacklisted != downloadBlacklisted) {
                    aphrodite.Settings.Tags.Default.downloadBlacklisted = downloadBlacklisted_First = downloadBlacklisted;
                    Save = true;
                }
                if (aphrodite.Settings.Tags.Default.DownloadNewestToOldest != DownloadNewestToOldest) {
                    aphrodite.Settings.Tags.Default.DownloadNewestToOldest = DownloadNewestToOldest_First = DownloadNewestToOldest;
                    Save = true;
                }
                if (aphrodite.Settings.Tags.Default.FavoriteCount != FavoriteCount) {
                    aphrodite.Settings.Tags.Default.FavoriteCount = FavoriteCount_First = FavoriteCount;
                    Save = true;
                }
                if (aphrodite.Settings.Tags.Default.FavoriteCountAsTag != FavoriteCountAsTag) {
                    aphrodite.Settings.Tags.Default.FavoriteCountAsTag = FavoriteCountAsTag_First = FavoriteCountAsTag;
                    Save = true;
                }

                if (Save) {
                    aphrodite.Settings.Tags.Default.Save();
                }
            }
        }

        /// <summary>
        /// Loads the configuration from the respective save location.
        /// </summary>
        public void Load() {
            Log.Write("Loading Config_Tags");

            if (Config.Settings.UseIni) {
                Safe_First = Safe = Config.Settings.Ini.KeyExists("Safe", ConfigName) ?
                    Config.Settings.Ini.ReadBool("Safe", ConfigName) : true;

                Questionable_First = Questionable = Config.Settings.Ini.KeyExists("Questionable", ConfigName) ?
                    Config.Settings.Ini.ReadBool("Questionable", ConfigName) : true;

                Explicit_First = Explicit = Config.Settings.Ini.KeyExists("Explicit", ConfigName) ?
                    Config.Settings.Ini.ReadBool("Explicit", ConfigName) : true;

                separateRatings_First = separateRatings = Config.Settings.Ini.KeyExists("separateRatings", ConfigName) ?
                    Config.Settings.Ini.ReadBool("separateRatings", ConfigName) : true;

                separateNonImages_First = separateNonImages = Config.Settings.Ini.KeyExists("separateNonImages", ConfigName) ?
                    Config.Settings.Ini.ReadBool("separateNonImages", ConfigName) : true;

                enableScoreMin_First = enableScoreMin = Config.Settings.Ini.KeyExists("enableScoreMin", ConfigName) ?
                    Config.Settings.Ini.ReadBool("enableScoreMin", ConfigName) : false;

                scoreAsTag_First = scoreAsTag = Config.Settings.Ini.KeyExists("scoreAsTag", ConfigName) ?
                    Config.Settings.Ini.ReadBool("scoreAsTag", ConfigName) : true;

                scoreMin_First = scoreMin = Config.Settings.Ini.KeyExists("scoreMin", ConfigName) ?
                    Config.Settings.Ini.ReadInt("scoreMin", ConfigName) : 0;

                imageLimit_First = imageLimit = Config.Settings.Ini.KeyExists("imageLimit", ConfigName) ?
                    Config.Settings.Ini.ReadInt("imageLimit", ConfigName) : 0;

                pageLimit_First = pageLimit = Config.Settings.Ini.KeyExists("pageLimit", ConfigName) ?
                    Config.Settings.Ini.ReadInt("pageLimit", ConfigName) : 0;

                fileNameSchema_First = fileNameSchema = Config.Settings.Ini.KeyExists("fileNameSchema", ConfigName) ?
                    ApiTools.ReplaceIllegalCharacters(Config.Settings.Ini.ReadString("fileNameSchema", ConfigName)) : "%id%_%md5%";

                downloadBlacklisted_First = downloadBlacklisted = Config.Settings.Ini.KeyExists("downloadBlacklisted", ConfigName) ?
                    Config.Settings.Ini.ReadBool("downloadBlacklisted", ConfigName) : false;

                DownloadNewestToOldest_First = DownloadNewestToOldest = Config.Settings.Ini.KeyExists("DownloadNewestToOldest", ConfigName) ?
                    Config.Settings.Ini.ReadBool("DownloadNewestToOldest", ConfigName) : false;

                FavoriteCount_First = FavoriteCount = Config.Settings.Ini.KeyExists("FavoriteCount", ConfigName) ?
                    Config.Settings.Ini.ReadInt("FavoriteCount", ConfigName) : 0;

                FavoriteCountAsTag_First = FavoriteCountAsTag = Config.Settings.Ini.KeyExists("FavoriteCountAsTag", ConfigName) ?
                    Config.Settings.Ini.ReadBool("FavoriteCountAsTag", ConfigName) : false;
            }
            else {
                Safe_First = Safe = aphrodite.Settings.Tags.Default.Safe;
                Questionable_First = Questionable = aphrodite.Settings.Tags.Default.Questionable;
                Explicit_First = Explicit = aphrodite.Settings.Tags.Default.Explicit;
                separateRatings_First = separateRatings = aphrodite.Settings.Tags.Default.separateRatings;
                separateNonImages_First = separateNonImages = aphrodite.Settings.Tags.Default.separateNonImages;
                enableScoreMin_First = enableScoreMin = aphrodite.Settings.Tags.Default.enableScoreMin;
                scoreAsTag_First = scoreAsTag = aphrodite.Settings.Tags.Default.scoreAsTag;
                scoreMin_First = scoreMin = aphrodite.Settings.Tags.Default.scoreMin;
                imageLimit_First = imageLimit = aphrodite.Settings.Tags.Default.imageLimit;
                pageLimit_First = pageLimit = aphrodite.Settings.Tags.Default.pageLimit;
                fileNameSchema_First = fileNameSchema = ApiTools.ReplaceIllegalCharacters(aphrodite.Settings.Tags.Default.fileNameSchema);
                downloadBlacklisted_First = downloadBlacklisted = aphrodite.Settings.Tags.Default.downloadBlacklisted;
                DownloadNewestToOldest_First = DownloadNewestToOldest = aphrodite.Settings.Tags.Default.DownloadNewestToOldest;
                FavoriteCount_First = FavoriteCount = aphrodite.Settings.Tags.Default.FavoriteCount;
                FavoriteCountAsTag_First = FavoriteCountAsTag = aphrodite.Settings.Tags.Default.FavoriteCountAsTag;
            }
        }

        /// <summary>
        /// Force-saves the configuration, overwriting any saved values.
        /// This method does not check for previous values.
        /// </summary>
        public void ForceSave() {
            Log.Write("Force saving Config_Tags");
            if (Config.Settings.UseIni) {
                Config.Settings.Ini.Write("Safe", Safe, ConfigName);
                Config.Settings.Ini.Write("Questionable", Questionable, ConfigName);
                Config.Settings.Ini.Write("Explicit", Explicit, ConfigName);
                Config.Settings.Ini.Write("separateRatings", separateRatings, ConfigName);
                Config.Settings.Ini.Write("separateNonImages", separateNonImages, ConfigName);
                Config.Settings.Ini.Write("enableScoreMin", enableScoreMin, ConfigName);
                Config.Settings.Ini.Write("scoreAsTag", scoreAsTag, ConfigName);
                Config.Settings.Ini.Write("scoreMin", scoreMin, ConfigName);
                Config.Settings.Ini.Write("imageLimit", imageLimit, ConfigName);
                Config.Settings.Ini.Write("pageLimit", pageLimit, ConfigName);
                Config.Settings.Ini.Write("fileNameSchema", fileNameSchema, ConfigName);
                Config.Settings.Ini.Write("downloadBlacklisted", downloadBlacklisted, ConfigName);
                Config.Settings.Ini.Write("DownloadNewestToOldest", DownloadNewestToOldest, ConfigName);
                Config.Settings.Ini.Write("FavoriteCount", FavoriteCount, ConfigName);
                Config.Settings.Ini.Write("FavoriteCountAsTag", FavoriteCountAsTag, ConfigName);
            }
            else {
                aphrodite.Settings.Tags.Default.Safe = Safe;
                aphrodite.Settings.Tags.Default.Questionable = Questionable;
                aphrodite.Settings.Tags.Default.Explicit = Explicit;
                aphrodite.Settings.Tags.Default.separateRatings = separateRatings;
                aphrodite.Settings.Tags.Default.separateNonImages = separateNonImages;
                aphrodite.Settings.Tags.Default.enableScoreMin = enableScoreMin;
                aphrodite.Settings.Tags.Default.scoreAsTag = scoreAsTag;
                aphrodite.Settings.Tags.Default.scoreMin = scoreMin;
                aphrodite.Settings.Tags.Default.imageLimit = imageLimit;
                aphrodite.Settings.Tags.Default.pageLimit = pageLimit;
                aphrodite.Settings.Tags.Default.fileNameSchema = fileNameSchema;
                aphrodite.Settings.Tags.Default.downloadBlacklisted = downloadBlacklisted;
                aphrodite.Settings.Tags.Default.DownloadNewestToOldest = DownloadNewestToOldest;
                aphrodite.Settings.Tags.Default.FavoriteCount = FavoriteCount;
                aphrodite.Settings.Tags.Default.FavoriteCountAsTag = FavoriteCountAsTag;
                aphrodite.Settings.Tags.Default.Save();
            }
            Safe_First = Safe;
            Questionable_First = Questionable;
            Explicit_First = Explicit;
            separateRatings_First = separateRatings;
            separateNonImages_First = separateNonImages;
            enableScoreMin_First = enableScoreMin;
            scoreAsTag_First = scoreAsTag;
            scoreMin_First = scoreMin;
            imageLimit_First = imageLimit;
            pageLimit_First = pageLimit;
            fileNameSchema_First = fileNameSchema;
            downloadBlacklisted_First = downloadBlacklisted;
            DownloadNewestToOldest_First = DownloadNewestToOldest;
            FavoriteCount_First = FavoriteCount;
            FavoriteCountAsTag_First = FavoriteCountAsTag;
        }
    }

    /// <summary>
    /// Main Imgur configuration class that handles Pool-related settings.
    /// The pool wishlist is not handled in this class, see <seealso cref="PoolWishlist"/>.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class Config_Pools {
        /// <summary>
        /// The ini section name of the current configuration class.
        /// </summary>
        private const string ConfigName = "Pools";

        #region Fields
        /// <summary>
        /// Whether when adding pools to the wishlist, it should do it in the background, instead of showing the wishlist form.
        /// </summary>
        public bool addWishlistSilent;
        /// <summary>
        /// Whether blacklisted pages should be downloaded.
        /// </summary>
        public bool downloadBlacklisted;
        /// <summary>
        /// Whether graylisted pages should be merged into the main pool folder.
        /// </summary>
        public bool mergeGraylisted;
        /// <summary>
        /// Whether blacklisted pages should be merged into the main pool folder.
        /// </summary>
        public bool mergeBlacklisted;
        //public string wishlist,
        //public string wishlistNames,
        /// <summary>
        /// The file-name schema used to translate pool-specific data into the file name.
        /// </summary>
        public string fileNameSchema;

        private bool
            addWishlistSilent_First,
            downloadBlacklisted_First,
            mergeGraylisted_First,
            mergeBlacklisted_First;
        private string
            //wishlist_First,
            //wishlistNames_First,
            fileNameSchema_First;
        #endregion

        /// <summary>
        /// Saves the configuration to the respective save location.
        /// </summary>
        public void Save() {
            Log.Write("Saving Config_Pools");

            if (Config.Settings.UseIni) {
                if (mergeGraylisted != mergeGraylisted_First) {
                    Config.Settings.Ini.Write("mergeGraylisted", mergeGraylisted, ConfigName);
                    mergeGraylisted_First = mergeGraylisted;
                }
                if (addWishlistSilent != addWishlistSilent_First) {
                    Config.Settings.Ini.Write("addWishlistSilent", addWishlistSilent, ConfigName);
                    addWishlistSilent_First = addWishlistSilent;
                }
                if (fileNameSchema != fileNameSchema_First) {
                    Config.Settings.Ini.Write("fileNameSchema", fileNameSchema, ConfigName);
                    fileNameSchema_First = fileNameSchema;
                }
                if (downloadBlacklisted != downloadBlacklisted_First) {
                    Config.Settings.Ini.Write("downloadBlacklisted", downloadBlacklisted, ConfigName);
                    downloadBlacklisted_First = downloadBlacklisted;
                }
                if (mergeBlacklisted != mergeBlacklisted_First) {
                    Config.Settings.Ini.Write("mergeBlacklisted", mergeBlacklisted, ConfigName);
                    mergeBlacklisted_First = mergeBlacklisted;
                }
            }
            else {
                bool Save = false;

                if (aphrodite.Settings.Pools.Default.mergeGraylisted != mergeGraylisted) {
                    aphrodite.Settings.Pools.Default.mergeGraylisted = mergeGraylisted_First = mergeGraylisted;
                    Save = true;
                }
                //if (aphrodite.Settings.Pools.Default.wishlist != wishlist) {
                //    aphrodite.Settings.Pools.Default.wishlist = wishlist_First = wishlist;
                //    Save = true;
                //}
                //if (aphrodite.Settings.Pools.Default.wishlistNames != wishlistNames) {
                //    aphrodite.Settings.Pools.Default.wishlistNames = wishlistNames_First = wishlistNames;
                //    Save = true;
                //}
                if (aphrodite.Settings.Pools.Default.addWishlistSilent != addWishlistSilent) {
                    aphrodite.Settings.Pools.Default.addWishlistSilent = addWishlistSilent_First = addWishlistSilent;
                    Save = true;
                }
                if (aphrodite.Settings.Pools.Default.fileNameSchema != fileNameSchema) {
                    aphrodite.Settings.Pools.Default.fileNameSchema = fileNameSchema_First = fileNameSchema;
                    Save = true;
                }
                if (aphrodite.Settings.Pools.Default.downloadBlacklisted != downloadBlacklisted) {
                    aphrodite.Settings.Pools.Default.downloadBlacklisted = downloadBlacklisted_First = downloadBlacklisted;
                    Save = true;
                }
                if (aphrodite.Settings.Pools.Default.mergeBlacklisted != mergeBlacklisted) {
                    aphrodite.Settings.Pools.Default.mergeBlacklisted = mergeBlacklisted_First = mergeBlacklisted;
                    Save = true;
                }

                if (Save) {
                    aphrodite.Settings.Pools.Default.Save();
                }
            }
        }

        /// <summary>
        /// Loads the configuration from the respective save location.
        /// </summary>
        public void Load() {
            Log.Write("Loading Config_Pools");

            if (Config.Settings.UseIni) {
                mergeGraylisted = mergeGraylisted_First = Config.Settings.Ini.KeyExists("mergeGraylisted", ConfigName) ?
                    Config.Settings.Ini.ReadBool("mergeGraylisted", ConfigName) : true;

                addWishlistSilent = addWishlistSilent_First = Config.Settings.Ini.KeyExists("addWishlistSilent", ConfigName) ?
                    Config.Settings.Ini.ReadBool("addWishlistSilent", ConfigName) : false;

                fileNameSchema = fileNameSchema_First = Config.Settings.Ini.KeyExists("fileNameSchema", ConfigName) ?
                    ApiTools.ReplaceIllegalCharacters(Config.Settings.Ini.ReadString("fileNameSchema", ConfigName)) : "%poolname%_%page%";

                downloadBlacklisted = downloadBlacklisted_First = Config.Settings.Ini.KeyExists("downloadBlacklisted", ConfigName) ?
                    Config.Settings.Ini.ReadBool("downloadBlacklisted", ConfigName) : true;

                mergeBlacklisted = mergeBlacklisted_First = Config.Settings.Ini.KeyExists("mergeBlacklisted", ConfigName) ?
                    Config.Settings.Ini.ReadBool("mergeBlacklisted", ConfigName) : false;

            }
            else {
                mergeGraylisted = mergeGraylisted_First = aphrodite.Settings.Pools.Default.mergeGraylisted;
                addWishlistSilent = addWishlistSilent_First = aphrodite.Settings.Pools.Default.addWishlistSilent;
                fileNameSchema = fileNameSchema_First = ApiTools.ReplaceIllegalCharacters(aphrodite.Settings.Pools.Default.fileNameSchema);
                downloadBlacklisted = downloadBlacklisted_First = aphrodite.Settings.Pools.Default.downloadBlacklisted;
                mergeBlacklisted = mergeBlacklisted_First = aphrodite.Settings.Pools.Default.mergeBlacklisted;
            }
        }

        /// <summary>
        /// Force-saves the configuration, overwriting any saved values.
        /// This method does not check for previous values.
        /// </summary>
        public void ForceSave() {
            Log.Write("Force saving Config_Pools");

            if (Config.Settings.UseIni) {
                Config.Settings.Ini.Write("mergeGraylisted", mergeGraylisted, ConfigName);
                Config.Settings.Ini.Write("addWishlistSilent", addWishlistSilent, ConfigName);
                Config.Settings.Ini.Write("fileNameSchema", fileNameSchema, ConfigName);
                Config.Settings.Ini.Write("downloadBlacklisted", downloadBlacklisted, ConfigName);
                Config.Settings.Ini.Write("mergeBlacklisted", mergeBlacklisted, ConfigName);
            }
            else {
                aphrodite.Settings.Pools.Default.mergeGraylisted = mergeGraylisted;
                aphrodite.Settings.Pools.Default.addWishlistSilent = addWishlistSilent;
                aphrodite.Settings.Pools.Default.fileNameSchema = fileNameSchema;
                aphrodite.Settings.Pools.Default.downloadBlacklisted = downloadBlacklisted;
                aphrodite.Settings.Pools.Default.mergeBlacklisted = mergeBlacklisted;
                aphrodite.Settings.Pools.Default.Save();
            }

            mergeGraylisted_First = mergeGraylisted;
            addWishlistSilent_First = addWishlistSilent;
            fileNameSchema_First = fileNameSchema;
            downloadBlacklisted_First = downloadBlacklisted;
            mergeBlacklisted_First = mergeBlacklisted;
            //wishlist_First = wishlist;
            //wishlistNames_First = wishlistNames;
        }
    }

    /// <summary>
    /// Main Imgur configuration class that handles Image-related settings.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class Config_Images {
        /// <summary>
        /// The ini section name of the current configuration class.
        /// </summary>
        private const string ConfigName = "Images";

        #region Fields
        /// <summary>
        /// Whether images should be sparated into a folder based on its' rating.
        /// </summary>
        public bool separateRatings;
        /// <summary>
        /// Whether graylisted images should be separated into another folder.
        /// </summary>
        public bool separateGraylisted;
        /// <summary>
        /// Whether the downloader should show the form.
        /// </summary>
        public bool useForm;
        /// <summary>
        /// Whether images should be separated into a folder based on the artist.
        /// The artist folder may not be correct.
        /// </summary>
        public bool separateArtists;
        /// <summary>
        /// Whether non-image images should be separated into its' own folder.
        /// </summary>
        public bool separateNonImages;
        /// <summary>
        /// Whether blacklisted images should be separated into another folder.
        /// </summary>
        public bool separateBlacklisted;
        /// <summary>
        /// The file-name schema used to translate file-specific data into the file name.
        /// </summary>
        public string fileNameSchema;

        private bool
            separateRatings_First,
            separateGraylisted_First,
            useForm_First,
            separateArtists_First,
            separateNonImages_First,
            separateBlacklisted_First;
        private string
            fileNameSchema_First;
        #endregion

        /// <summary>
        /// Saves the configuration to the respective save location.
        /// </summary>
        public void Save() {
            Log.Write("Saving Config_Images");

            if (Config.Settings.UseIni) {
                if (separateRatings != separateRatings_First) {
                    Config.Settings.Ini.Write("separateRatings", separateRatings, ConfigName);
                    separateRatings_First = separateRatings;
                }
                if (separateGraylisted != separateGraylisted_First) {
                    Config.Settings.Ini.Write("separateBlacklisted", separateGraylisted, ConfigName);
                    separateGraylisted_First = separateGraylisted;
                }
                if (useForm != useForm_First) {
                    Config.Settings.Ini.Write("useForm", useForm, ConfigName);
                    useForm_First = useForm;
                }
                if (separateArtists != separateArtists_First) {
                    Config.Settings.Ini.Write("separateArtists", separateArtists, ConfigName);
                    separateArtists_First = separateArtists;
                }
                if (fileNameSchema != fileNameSchema_First) {
                    Config.Settings.Ini.Write("fileNameSchema", fileNameSchema, ConfigName);
                    fileNameSchema_First = fileNameSchema;
                }
                if (separateNonImages != separateNonImages_First) {
                    Config.Settings.Ini.Write("separateNonImages", separateNonImages, ConfigName);
                    separateNonImages_First = separateNonImages;
                }
                if (separateBlacklisted != separateBlacklisted_First) {
                    Config.Settings.Ini.Write("separateBlacklisted", separateBlacklisted, ConfigName);
                    separateBlacklisted_First = separateBlacklisted;
                }
            }
            else {
                bool Save = false;

                if (aphrodite.Settings.Images.Default.separateRatings != separateRatings) {
                    aphrodite.Settings.Images.Default.separateRatings = separateRatings_First = separateRatings;
                    Save = true;
                }
                if (aphrodite.Settings.Images.Default.separateGraylisted != separateGraylisted) {
                    aphrodite.Settings.Images.Default.separateGraylisted = separateGraylisted_First = separateGraylisted;
                    Save = true;
                }
                if (aphrodite.Settings.Images.Default.useForm != useForm) {
                    aphrodite.Settings.Images.Default.useForm = useForm_First = useForm;
                    Save = true;
                }
                if (aphrodite.Settings.Images.Default.separateArtists != separateArtists) {
                    aphrodite.Settings.Images.Default.separateArtists = separateArtists_First = separateArtists;
                    Save = true;
                }
                if (aphrodite.Settings.Images.Default.fileNameSchema != fileNameSchema) {
                    aphrodite.Settings.Images.Default.fileNameSchema = fileNameSchema_First = fileNameSchema;
                    Save = true;
                }
                if (aphrodite.Settings.Images.Default.separateNonImages != separateNonImages) {
                    aphrodite.Settings.Images.Default.separateNonImages = separateNonImages_First = separateNonImages;
                    Save = true;
                }
                if (aphrodite.Settings.Images.Default.separateBlacklisted != separateBlacklisted) {
                    aphrodite.Settings.Images.Default.separateBlacklisted = separateBlacklisted_First = separateBlacklisted;
                    Save = true;
                }

                if (Save) {
                    aphrodite.Settings.Images.Default.Save();
                }
            }
        }

        /// <summary>
        /// Loads the configuration from the respective save location.
        /// </summary>
        public void Load() {
            Log.Write("Loading Config_Images settings");
            if (Config.Settings.UseIni) {
                separateRatings_First = separateRatings = Config.Settings.Ini.KeyExists("separateRatings", ConfigName) ?
                    Config.Settings.Ini.ReadBool("separateRatings", ConfigName) : true;

                separateGraylisted_First = separateGraylisted = Config.Settings.Ini.KeyExists("separateBlacklisted", ConfigName) ?
                    Config.Settings.Ini.ReadBool("separateBlacklisted", ConfigName) : true;

                useForm_First = useForm = Config.Settings.Ini.KeyExists("useForm", ConfigName) ?
                    Config.Settings.Ini.ReadBool("useForm", ConfigName) : false;

                separateArtists_First = separateArtists = Config.Settings.Ini.KeyExists("separateArtists", ConfigName) ?
                    Config.Settings.Ini.ReadBool("separateArtists", ConfigName) : false;

                fileNameSchema_First = fileNameSchema = Config.Settings.Ini.KeyExists("fileNameSchema", ConfigName) ?
                    ApiTools.ReplaceIllegalCharacters(Config.Settings.Ini.ReadString("fileNameSchema", ConfigName)) : "%artist%_%id%";

                separateNonImages_First = separateNonImages = Config.Settings.Ini.KeyExists("separateNonImages", ConfigName) ?
                    Config.Settings.Ini.ReadBool("separateNonImages", ConfigName) : true;

                separateBlacklisted_First = separateBlacklisted = Config.Settings.Ini.KeyExists("separateBlacklisted", ConfigName) ?
                    Config.Settings.Ini.ReadBool("separateBlacklisted", ConfigName) : true;
            }
            else {
                separateRatings = separateRatings_First = aphrodite.Settings.Images.Default.separateRatings;
                separateGraylisted = separateGraylisted_First = aphrodite.Settings.Images.Default.separateGraylisted;
                useForm = useForm_First = aphrodite.Settings.Images.Default.useForm;
                separateArtists = separateArtists_First = aphrodite.Settings.Images.Default.separateArtists;
                fileNameSchema = fileNameSchema_First = ApiTools.ReplaceIllegalCharacters(aphrodite.Settings.Images.Default.fileNameSchema);
                separateNonImages = separateNonImages_First = aphrodite.Settings.Images.Default.separateNonImages;
                separateBlacklisted = separateBlacklisted_First = aphrodite.Settings.Images.Default.separateBlacklisted;
            }
        }

        /// <summary>
        /// Force-saves the configuration, overwriting any saved values.
        /// This method does not check for previous values.
        /// </summary>
        public void ForceSave() {
            Log.Write("Force saving Config_Images");
            if (Config.Settings.UseIni) {
                Config.Settings.Ini.Write("separateRatings", separateRatings, ConfigName);
                Config.Settings.Ini.Write("separateBlacklisted", separateGraylisted, ConfigName);
                Config.Settings.Ini.Write("useForm", useForm, ConfigName);
                Config.Settings.Ini.Write("separateArtists", separateArtists, ConfigName);
                Config.Settings.Ini.Write("fileNameSchema", fileNameSchema, ConfigName);
                Config.Settings.Ini.Write("separateNonImages", separateNonImages, ConfigName);
                Config.Settings.Ini.Write("separateBlacklisted", separateBlacklisted, ConfigName);
            }
            else {
                aphrodite.Settings.Images.Default.separateRatings = separateRatings;
                aphrodite.Settings.Images.Default.separateGraylisted = separateGraylisted;
                aphrodite.Settings.Images.Default.useForm = useForm;
                aphrodite.Settings.Images.Default.separateArtists = separateArtists;
                aphrodite.Settings.Images.Default.fileNameSchema = fileNameSchema;
                aphrodite.Settings.Images.Default.separateNonImages = separateNonImages;
                aphrodite.Settings.Images.Default.separateBlacklisted = separateBlacklisted;
                aphrodite.Settings.Images.Default.Save();
            }
            separateRatings_First = separateRatings;
            separateGraylisted_First = separateGraylisted;
            useForm_First = useForm;
            separateArtists_First = separateArtists;
            fileNameSchema_First = fileNameSchema;
            separateNonImages_First = separateNonImages;
            separateBlacklisted_First = separateBlacklisted;
        }
    }

    /// <summary>
    /// Main Imgur configuration class that handles InkBunny-related settings.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class Config_InkBunny {
        /// <summary>
        /// The ini section name of the current configuration class.
        /// </summary>
        private const string ConfigName = "InkBunny";

        #region Fields
        /// <summary>
        /// Whether the InkBunny API Session ID encryption/decryption warning was displayed.
        /// </summary>
        public bool ReadWarning;
        /// <summary>
        /// Whether the Guest Session ID should be used instead of the Users' Session ID.
        /// Only usable if <see cref="GuestSessionID"/> is not null, empty, or whitespace.
        /// </summary>
        public bool GuestPriority;
        /// <summary>
        /// Whether the guest can download files that are in General-rated submissions.
        /// </summary>
        public bool GuestGeneral;
        /// <summary>
        /// Whether the guest can download files that are in Mature (Nudity)-rated submissions.
        /// </summary>
        public bool GuestMatureNudity;
        /// <summary>
        /// Whether the guest can download files that are in Mature (Violence)-rated submissions.
        /// </summary>
        public bool GuestMatureViolence;
        /// <summary>
        /// Whether the guest can download files that are in Adult (Sexual Themes)-rated submissions.
        /// </summary>
        public bool GuestAdultSexualThemes;
        /// <summary>
        /// Whether the guest can download files that are in Adult (Strong Violence)-rated submissions.
        /// </summary>
        public bool GuestAdultStrongViolence;
        /// <summary>
        /// Whether to download files in General-rated submissions.
        /// </summary>
        public bool General;
        /// <summary>
        /// Whether to download files in Mature (Nudity)-rated submissions.
        /// </summary>
        public bool MatureNudity;
        /// <summary>
        /// Whether to download files in Mature (Violence)-rated submissions.
        /// </summary>
        public bool MatureViolence;
        /// <summary>
        /// Whether to download files in Adult (Sexual Themes)-rated submissions.
        /// </summary>
        public bool AdultSexualThemes;
        /// <summary>
        /// Whether to download files in Adult (Strong Violence)-rated submissions.
        /// </summary>
        public bool AdultStrongViolence;
        /// <summary>
        /// Whether to search keywords in submission Keywords.
        /// </summary>
        public bool SearchInKeywords;
        /// <summary>
        /// Whether to search keywords in submission Titles.
        /// </summary>
        public bool SearchInTitle;
        /// <summary>
        /// Whether to search keywords in submission Descriptions or Stories.
        /// </summary>
        public bool SearchInDescriptionOrStory;
        /// <summary>
        /// Whether to search for MD5 hashes. The MD5 hash must be in the Keywords field.
        /// </summary>
        public bool SearchInMd5Hash;
        /// <summary>
        /// Whether to download files with the Picture/Pinup submission type.
        /// </summary>
        public bool PicturePinup;
        /// <summary>
        /// Whether to download files with the Sketch submission type.
        /// </summary>
        public bool Sketch;
        /// <summary>
        /// Whether to download files with the Picture Series submission type.
        /// </summary>
        public bool PictureSeries;
        /// <summary>
        /// Whether to download files with the Comic submission type.
        /// </summary>
        public bool Comic;
        /// <summary>
        /// Whether to download files with the Portfolio submission type.
        /// </summary>
        public bool Portfolio;
        /// <summary>
        /// Whether to download files with the Flash (Animation) submission type.
        /// </summary>
        public bool FlashAnimation;
        /// <summary>
        /// Whether to download files with the Flash (Interactive) submission type.
        /// </summary>
        public bool FlashInteractive;
        /// <summary>
        /// Whether to download files with the Video (Animation) submission type.
        /// </summary>
        public bool VideoAnimation;
        /// <summary>
        /// Whether to download files with the Music (Single Track) submission type.
        /// </summary>
        public bool MusicSingleTrack;
        /// <summary>
        /// Whether to download files with the Music (Album) submission type.
        /// </summary>
        public bool MusicAlbum;
        /// <summary>
        /// Whether to download files with the Video (Feature Length) submission type.
        /// </summary>
        public bool VideoFeatureLength;
        /// <summary>
        /// Whether to download files with the Writing/Document submission type.
        /// </summary>
        public bool WritingDocument;
        /// <summary>
        /// Whether to download files with the Charcter Sheet submission type.
        /// </summary>
        public bool CharacterSheet;
        /// <summary>
        /// Whether to download files with the Photography submission type.
        /// </summary>
        public bool Photography;
        /// <summary>
        /// Whether multi-post submissions should be separated into another folder, and then separated again into their own folder.
        /// </summary>
        public bool SeparateMultiPosts;
        /// <summary>
        /// Whether to download graylisted submissions.
        /// </summary>
        public bool DownloadGraylistedSubmissions;
        /// <summary>
        /// Whether to download blacklisted submissions.
        /// </summary>
        public bool DownloadBlacklistedSubmissions;
        /// <summary>
        /// Whether non-images should be separated into their own folder.
        /// </summary>
        public bool SeparateNonImages;
        /// <summary>
        /// The encrypted InkBunny Session ID.
        /// </summary>
        public string SessionID;
        /// <summary>
        /// The username of the user logged in. It can be manually set or removed. Not important for the program to work.
        /// </summary>
        public string LoggedInUsername;
        /// <summary>
        /// The Guest Session ID. Does not require encryption/decryption, due to it being a guest ID.
        /// </summary>
        public string GuestSessionID;
        /// <summary>
        /// The file-name schema use to translate file-specific data into the file name.
        /// </summary>
        public string FileNameSchema;
        /// <summary>
        /// The file-name schema use to translate file-specific data into the file name, with multi-post flags available.
        /// </summary>
        public string FileNameSchemaMultiPost;
        /// <summary>
        /// The limit of how many images are to be downloaded. 0 is off.
        /// </summary>
        public int ImageLimit;
        /// <summary>
        /// The limit of how many pages are to be parsed. 0 is off.
        /// </summary>
        public int PageLimit;

        private bool
            ReadWarning_First,
            GuestPriority_First,
            GuestGeneral_First,
            GuestMatureNudity_First,
            GuestMatureViolence_First,
            GuestAdultSexualThemes_First,
            GuestAdultStrongViolence_First,
            General_First,
            MatureNudity_First,
            MatureViolence_First,
            AdultSexualThemes_First,
            AdultStrongViolence_First,
            SearchInKeywords_First,
            SearchInTitle_First,
            SearchInDescriptionOrStory_First,
            SearchInMd5Hash_First,
            PicturePinup_First,
            Sketch_First,
            PictureSeries_First,
            Comic_First,
            Portfolio_First,
            FlashAnimation_First,
            FlashInteractive_First,
            VideoAnimation_First,
            MusicSingleTrack_First,
            MusicAlbum_First,
            VideoFeatureLength_First,
            WritingDocument_First,
            CharacterSheet_First,
            Photography_First,
            SeparateMultiPosts_First,
            DownloadGraylistedSubmissions_First,
            DownloadBlacklistedSubmissions_First,
            SeparateNonImages_First;
        private string
            SessionID_First,
            LoggedInUsername_First,
            GuestSessionID_First,
            FileNameSchema_First,
            FileNameSchemaMultiPost_First;
        private int
            ImageLimit_First,
            PageLimit_First;
        #endregion

        /// <summary>
        /// Saves the configuration to the respective save location.
        /// </summary>
        public void Save() {
            Log.Write("Saving Config_InkBunny");
            if (Config.Settings.UseIni) {
                if (ReadWarning_First != ReadWarning) {
                    Config.Settings.Ini.Write("ReadWarning", ReadWarning, ConfigName);
                    ReadWarning_First = ReadWarning;
                }
                if (SessionID_First != SessionID) {
                    Config.Settings.Ini.Write("SessionID", SessionID, ConfigName);
                    SessionID_First = SessionID;
                }
                if (LoggedInUsername_First != LoggedInUsername) {
                    Config.Settings.Ini.Write("LoggedInUsername", LoggedInUsername, ConfigName);
                    LoggedInUsername_First = LoggedInUsername;
                }
                if (GuestPriority_First != GuestPriority) {
                    Config.Settings.Ini.Write("GuestPriority", GuestPriority, ConfigName);
                    GuestPriority_First = GuestPriority;
                }
                if (GuestSessionID_First != GuestSessionID) {
                    Config.Settings.Ini.Write("GuestSessionID", GuestSessionID, ConfigName);
                    GuestSessionID_First = GuestSessionID;
                }
                if (GuestGeneral_First != GuestGeneral) {
                    Config.Settings.Ini.Write("GuestGeneral", GuestGeneral, ConfigName);
                    GuestGeneral_First = GuestGeneral;
                }
                if (GuestMatureNudity_First != GuestMatureNudity) {
                    Config.Settings.Ini.Write("GuestMatureNudity", GuestMatureNudity, ConfigName);
                    GuestMatureNudity_First = GuestMatureNudity;
                }
                if (GuestMatureViolence_First != GuestMatureViolence) {
                    Config.Settings.Ini.Write("GuestMatureViolence", GuestMatureViolence, ConfigName);
                    GuestMatureViolence_First = GuestMatureViolence;
                }
                if (GuestAdultSexualThemes_First != GuestAdultSexualThemes) {
                    Config.Settings.Ini.Write("GuestAdultSexualThemes", GuestAdultSexualThemes, ConfigName);
                    GuestAdultSexualThemes_First = GuestAdultSexualThemes;
                }
                if (GuestAdultStrongViolence_First != GuestAdultStrongViolence) {
                    Config.Settings.Ini.Write("GuestAdultStrongViolence", GuestAdultStrongViolence, ConfigName);
                    GuestAdultStrongViolence_First = GuestAdultStrongViolence;
                }
                if (General != General_First) {
                    Config.Settings.Ini.Write("General", General, ConfigName);
                    General_First = General;
                }
                if (MatureNudity != MatureNudity_First) {
                    Config.Settings.Ini.Write("MatureNudity", MatureNudity, ConfigName);
                    MatureNudity_First = MatureNudity;
                }
                if (MatureViolence != MatureViolence_First) {
                    Config.Settings.Ini.Write("MatureViolence", MatureViolence, ConfigName);
                    MatureViolence_First = MatureViolence;
                }
                if (AdultSexualThemes != AdultSexualThemes_First) {
                    Config.Settings.Ini.Write("AdultSexualThemes", AdultSexualThemes, ConfigName);
                    AdultSexualThemes_First = AdultSexualThemes;
                }
                if (AdultStrongViolence != AdultStrongViolence_First) {
                    Config.Settings.Ini.Write("AdultStrongViolence", AdultStrongViolence, ConfigName);
                    AdultStrongViolence_First = AdultStrongViolence;
                }
                if (SearchInKeywords != SearchInKeywords_First) {
                    Config.Settings.Ini.Write("SearchInKeywords", SearchInKeywords, ConfigName);
                    SearchInKeywords_First = SearchInKeywords;
                }
                if (SearchInTitle != SearchInTitle_First) {
                    Config.Settings.Ini.Write("SearchInTitle", SearchInTitle, ConfigName);
                    SearchInTitle_First = SearchInTitle;
                }
                if (SearchInDescriptionOrStory != SearchInDescriptionOrStory_First) {
                    Config.Settings.Ini.Write("SearchInDescriptionOrStory", SearchInDescriptionOrStory, ConfigName);
                    SearchInDescriptionOrStory_First = SearchInDescriptionOrStory;
                }
                if (SearchInMd5Hash != SearchInMd5Hash_First) {
                    Config.Settings.Ini.Write("SearchInMd5Hash", SearchInMd5Hash, ConfigName);
                    SearchInMd5Hash_First = SearchInMd5Hash;
                }
                if (PicturePinup != PicturePinup_First) {
                    Config.Settings.Ini.Write("PicturePinup", PicturePinup, ConfigName);
                    PicturePinup_First = PicturePinup;
                }
                if (Sketch != Sketch_First) {
                    Config.Settings.Ini.Write("Sketch", Sketch, ConfigName);
                    Sketch_First = Sketch;
                }
                if (PictureSeries != PictureSeries_First) {
                    Config.Settings.Ini.Write("PictureSeries", PictureSeries, ConfigName);
                    PictureSeries_First = PictureSeries;
                }
                if (Comic != Comic_First) {
                    Config.Settings.Ini.Write("Comic", Comic, ConfigName);
                    Comic_First = Comic;
                }
                if (Portfolio != Portfolio_First) {
                    Config.Settings.Ini.Write("Portfolio", Portfolio, ConfigName);
                    Portfolio_First = Portfolio;
                }
                if (FlashAnimation != FlashAnimation_First) {
                    Config.Settings.Ini.Write("FlashAnimation", FlashAnimation, ConfigName);
                    FlashAnimation_First = FlashAnimation;
                }
                if (FlashInteractive != FlashInteractive_First) {
                    Config.Settings.Ini.Write("FlashInteractive", FlashInteractive, ConfigName);
                    FlashInteractive_First = FlashInteractive;
                }
                if (VideoAnimation != VideoAnimation_First) {
                    Config.Settings.Ini.Write("VideoAnimation", VideoAnimation, ConfigName);
                    VideoAnimation_First = VideoAnimation;
                }
                if (MusicSingleTrack != MusicSingleTrack_First) {
                    Config.Settings.Ini.Write("MusicSingleTrack", MusicSingleTrack, ConfigName);
                    MusicSingleTrack_First = MusicSingleTrack;
                }
                if (MusicAlbum != MusicAlbum_First) {
                    Config.Settings.Ini.Write("MusicAlbum", MusicAlbum, ConfigName);
                    MusicAlbum_First = MusicAlbum;
                }
                if (VideoFeatureLength != VideoFeatureLength_First) {
                    Config.Settings.Ini.Write("VideoFeatureLength", VideoFeatureLength, ConfigName);
                    VideoFeatureLength_First = VideoFeatureLength;
                }
                if (WritingDocument != WritingDocument_First) {
                    Config.Settings.Ini.Write("WritingDocument", WritingDocument, ConfigName);
                    WritingDocument_First = WritingDocument;
                }
                if (CharacterSheet != CharacterSheet_First) {
                    Config.Settings.Ini.Write("CharacterSheet", CharacterSheet, ConfigName);
                    CharacterSheet_First = CharacterSheet;
                }
                if (Photography != Photography_First) {
                    Config.Settings.Ini.Write("Photography", Photography, ConfigName);
                    Photography_First = Photography;
                }
                if (SeparateMultiPosts != SeparateMultiPosts_First) {
                    Config.Settings.Ini.Write("SeparateMultiPosts", SeparateMultiPosts, ConfigName);
                    SeparateMultiPosts_First = SeparateMultiPosts;
                }
                if (FileNameSchema != FileNameSchema_First) {
                    Config.Settings.Ini.Write("FileNameSchema", FileNameSchema, ConfigName);
                    FileNameSchema_First = FileNameSchema;
                }
                if (FileNameSchemaMultiPost != FileNameSchemaMultiPost_First) {
                    Config.Settings.Ini.Write("FileNameSchemaMultiPost", FileNameSchemaMultiPost, ConfigName);
                    FileNameSchemaMultiPost_First = FileNameSchemaMultiPost;
                }
                if (DownloadGraylistedSubmissions != DownloadGraylistedSubmissions_First) {
                    Config.Settings.Ini.Write("DownloadGraylistedSubmissions", DownloadGraylistedSubmissions, ConfigName);
                    DownloadGraylistedSubmissions_First = DownloadGraylistedSubmissions;
                }
                if (DownloadBlacklistedSubmissions != DownloadBlacklistedSubmissions_First) {
                    Config.Settings.Ini.Write("DownloadBlacklistedSubmissions", DownloadBlacklistedSubmissions, ConfigName);
                    DownloadBlacklistedSubmissions_First = DownloadBlacklistedSubmissions;
                }
                if (SeparateNonImages != SeparateNonImages_First) {
                    Config.Settings.Ini.Write("SeparateNonImages", SeparateNonImages, ConfigName);
                    SeparateNonImages_First = SeparateNonImages;
                }
                if (ImageLimit != ImageLimit_First) {
                    Config.Settings.Ini.Write("ImageLimit", ImageLimit, ConfigName);
                    ImageLimit_First = ImageLimit;
                }
                if (PageLimit != PageLimit_First) {
                    Config.Settings.Ini.Write("PageLimit", PageLimit, ConfigName);
                    PageLimit_First = PageLimit;
                }
            }
            else {
                bool Save = false;

                if (aphrodite.Settings.InkBunny.Default.ReadWarning != ReadWarning) {
                    aphrodite.Settings.InkBunny.Default.ReadWarning = ReadWarning_First = ReadWarning;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.SessionID != SessionID) {
                    aphrodite.Settings.InkBunny.Default.SessionID = SessionID_First = SessionID;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.LoggedInUsername != LoggedInUsername) {
                    aphrodite.Settings.InkBunny.Default.LoggedInUsername = LoggedInUsername_First = LoggedInUsername;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.GuestPriority != GuestPriority) {
                    aphrodite.Settings.InkBunny.Default.GuestPriority = GuestPriority_First = GuestPriority;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.GuestSessionID != GuestSessionID) {
                    aphrodite.Settings.InkBunny.Default.GuestSessionID = GuestSessionID_First = GuestSessionID;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.GuestGeneral != GuestGeneral) {
                    aphrodite.Settings.InkBunny.Default.GuestGeneral = GuestGeneral_First = GuestGeneral;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.GuestMatureNudity != GuestMatureNudity) {
                    aphrodite.Settings.InkBunny.Default.GuestMatureNudity = GuestMatureNudity_First = GuestMatureNudity;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.GuestMatureViolence != GuestMatureViolence) {
                    aphrodite.Settings.InkBunny.Default.GuestMatureViolence = GuestMatureViolence_First = GuestMatureViolence;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.GuestAdultSexualThemes != GuestAdultSexualThemes) {
                    aphrodite.Settings.InkBunny.Default.GuestAdultSexualThemes = GuestAdultSexualThemes_First = GuestAdultSexualThemes;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.GuestAdultStrongViolence != GuestAdultStrongViolence) {
                    aphrodite.Settings.InkBunny.Default.GuestAdultStrongViolence = GuestAdultStrongViolence_First = GuestAdultStrongViolence;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.General != General) {
                    aphrodite.Settings.InkBunny.Default.General = General_First = General;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.MatureNudity != MatureNudity) {
                    aphrodite.Settings.InkBunny.Default.MatureNudity = MatureNudity_First = MatureNudity;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.MatureViolence != MatureViolence) {
                    aphrodite.Settings.InkBunny.Default.MatureViolence = MatureViolence_First = MatureViolence;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.AdultSexualThemes != AdultSexualThemes) {
                    aphrodite.Settings.InkBunny.Default.AdultSexualThemes = AdultSexualThemes_First = AdultSexualThemes;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.AdultStrongViolence != AdultStrongViolence) {
                    aphrodite.Settings.InkBunny.Default.AdultStrongViolence = AdultStrongViolence_First = AdultStrongViolence;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.SearchInKeywords != SearchInKeywords) {
                    aphrodite.Settings.InkBunny.Default.SearchInKeywords = SearchInKeywords_First = SearchInKeywords;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.SearchInTitle != SearchInTitle) {
                    aphrodite.Settings.InkBunny.Default.SearchInTitle = SearchInTitle_First = SearchInTitle;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.SearchInDescriptionOrStory != SearchInDescriptionOrStory) {
                    aphrodite.Settings.InkBunny.Default.SearchInDescriptionOrStory = SearchInDescriptionOrStory_First = SearchInDescriptionOrStory;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.SearchInMd5Hash != SearchInMd5Hash) {
                    aphrodite.Settings.InkBunny.Default.SearchInMd5Hash = SearchInMd5Hash_First = SearchInMd5Hash;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.PicturePinup != PicturePinup) {
                    aphrodite.Settings.InkBunny.Default.PicturePinup = PicturePinup_First = PicturePinup;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.Sketch != Sketch) {
                    aphrodite.Settings.InkBunny.Default.Sketch = Sketch_First = Sketch;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.PictureSeries != PictureSeries) {
                    aphrodite.Settings.InkBunny.Default.PictureSeries = PictureSeries_First = PictureSeries;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.Comic != Comic) {
                    aphrodite.Settings.InkBunny.Default.Comic = Comic_First = Comic;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.Portfolio != Portfolio) {
                    aphrodite.Settings.InkBunny.Default.Portfolio = Portfolio_First = Portfolio;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.FlashAnimation != FlashAnimation) {
                    aphrodite.Settings.InkBunny.Default.FlashAnimation = FlashAnimation_First = FlashAnimation;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.FlashInteractive != FlashInteractive) {
                    aphrodite.Settings.InkBunny.Default.FlashInteractive = FlashInteractive_First = FlashInteractive;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.VideoAnimation != VideoAnimation) {
                    aphrodite.Settings.InkBunny.Default.VideoAnimation = VideoAnimation_First = VideoAnimation;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.MusicSingleTrack != MusicSingleTrack) {
                    aphrodite.Settings.InkBunny.Default.MusicSingleTrack = MusicSingleTrack_First = MusicSingleTrack;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.MusicAlbum != MusicAlbum) {
                    aphrodite.Settings.InkBunny.Default.MusicAlbum = MusicAlbum_First = MusicAlbum;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.VideoFeatureLength != VideoFeatureLength) {
                    aphrodite.Settings.InkBunny.Default.VideoFeatureLength = VideoFeatureLength_First = VideoFeatureLength;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.WritingDocument != WritingDocument) {
                    aphrodite.Settings.InkBunny.Default.WritingDocument = WritingDocument_First = WritingDocument;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.CharacterSheet != CharacterSheet) {
                    aphrodite.Settings.InkBunny.Default.CharacterSheet = CharacterSheet_First = CharacterSheet;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.Photography != Photography) {
                    aphrodite.Settings.InkBunny.Default.Photography = Photography_First = Photography;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.SeparateMultiPosts != SeparateMultiPosts) {
                    aphrodite.Settings.InkBunny.Default.SeparateMultiPosts = SeparateMultiPosts_First = SeparateMultiPosts;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.FileNameSchema != FileNameSchema) {
                    aphrodite.Settings.InkBunny.Default.FileNameSchema = FileNameSchema_First = FileNameSchema;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.FileNameSchemaMultiPost != FileNameSchemaMultiPost) {
                    aphrodite.Settings.InkBunny.Default.FileNameSchemaMultiPost = FileNameSchemaMultiPost_First = FileNameSchemaMultiPost;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.DownloadGraylistedSubmissions != DownloadGraylistedSubmissions) {
                    aphrodite.Settings.InkBunny.Default.DownloadGraylistedSubmissions = DownloadGraylistedSubmissions_First = DownloadGraylistedSubmissions;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.DownloadBlacklistedSubmissions != DownloadBlacklistedSubmissions) {
                    aphrodite.Settings.InkBunny.Default.DownloadBlacklistedSubmissions = DownloadBlacklistedSubmissions_First = DownloadBlacklistedSubmissions;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.SeparateNonImages != SeparateNonImages) {
                    aphrodite.Settings.InkBunny.Default.SeparateNonImages = SeparateNonImages_First = SeparateNonImages;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.ImageLimit != ImageLimit) {
                    aphrodite.Settings.InkBunny.Default.ImageLimit = ImageLimit_First = ImageLimit;
                    Save = true;
                }
                if (aphrodite.Settings.InkBunny.Default.PageLimit != PageLimit) {
                    aphrodite.Settings.InkBunny.Default.PageLimit = PageLimit_First = PageLimit;
                    Save = true;
                }

                if (Save) {
                    aphrodite.Settings.InkBunny.Default.Save();
                }
            }
        }

        /// <summary>
        /// Loads the configuration from the respective save location.
        /// </summary>
        public void Load() {
            Log.Write("Loading Config_InkBunny");
            if (Config.Settings.UseIni) {
                ReadWarning = ReadWarning_First = Config.Settings.Ini.KeyExists("ReadWarning", ConfigName) ?
                    Config.Settings.Ini.ReadBool("ReadWarning", ConfigName) : false;

                SessionID = SessionID_First = Config.Settings.Ini.KeyExists("SessionID", ConfigName) ?
                    Config.Settings.Ini.ReadString("SessionID", ConfigName) : string.Empty;

                LoggedInUsername = LoggedInUsername_First = Config.Settings.Ini.KeyExists("LoggedInUsername", ConfigName) ?
                    Config.Settings.Ini.ReadString("LoggedInUsername", ConfigName) : string.Empty;

                GuestPriority = GuestPriority_First = Config.Settings.Ini.KeyExists("GuestPriority", ConfigName) ?
                    Config.Settings.Ini.ReadBool("GuestPriority", ConfigName) : false;

                GuestSessionID = GuestSessionID_First = Config.Settings.Ini.KeyExists("GuestSessionID", ConfigName) ?
                    Config.Settings.Ini.ReadString("GuestSessionID", ConfigName) : string.Empty;

                GuestGeneral = GuestGeneral_First = Config.Settings.Ini.KeyExists("GuestGeneral", ConfigName) ?
                    Config.Settings.Ini.ReadBool("GuestGeneral", ConfigName) : false;

                GuestMatureNudity = GuestMatureNudity_First = Config.Settings.Ini.KeyExists("GuestMatureNudity", ConfigName) ?
                    Config.Settings.Ini.ReadBool("GuestMatureNudity", ConfigName) : false;

                GuestMatureViolence = GuestMatureViolence_First = Config.Settings.Ini.KeyExists("GuestMatureViolence", ConfigName) ?
                    Config.Settings.Ini.ReadBool("GuestMatureViolence", ConfigName) : false;

                GuestAdultSexualThemes = GuestAdultSexualThemes_First = Config.Settings.Ini.KeyExists("GuestAdultSexualThemes", ConfigName) ?
                    Config.Settings.Ini.ReadBool("GuestAdultSexualThemes", ConfigName) : false;

                GuestAdultStrongViolence = GuestAdultStrongViolence_First = Config.Settings.Ini.KeyExists("GuestAdultStrongViolence", ConfigName) ?
                    Config.Settings.Ini.ReadBool("GuestAdultStrongViolence", ConfigName) : false;

                General = General_First = Config.Settings.Ini.KeyExists("General", ConfigName) ?
                    Config.Settings.Ini.ReadBool("General", ConfigName) : true;

                MatureNudity = MatureNudity_First = Config.Settings.Ini.KeyExists("MatureNudity", ConfigName) ?
                    Config.Settings.Ini.ReadBool("MatureNudity", ConfigName) : true;

                MatureViolence = MatureViolence_First = Config.Settings.Ini.KeyExists("MatureViolence", ConfigName) ?
                    Config.Settings.Ini.ReadBool("MatureViolence", ConfigName) : true;

                AdultSexualThemes = AdultSexualThemes_First = Config.Settings.Ini.KeyExists("AdultSexualThemes", ConfigName) ?
                    Config.Settings.Ini.ReadBool("AdultSexualThemes", ConfigName) : true;

                AdultStrongViolence = AdultStrongViolence_First = Config.Settings.Ini.KeyExists("AdultStrongViolence", ConfigName) ?
                    Config.Settings.Ini.ReadBool("AdultStrongViolence", ConfigName) : true;

                SearchInKeywords = SearchInKeywords_First = Config.Settings.Ini.KeyExists("SearchInKeywords", ConfigName) ?
                    Config.Settings.Ini.ReadBool("SearchInKeywords", ConfigName) : true;

                SearchInTitle = SearchInTitle_First = Config.Settings.Ini.KeyExists("SearchInTitle", ConfigName) ?
                    Config.Settings.Ini.ReadBool("SearchInTitle", ConfigName) : false;

                SearchInDescriptionOrStory = SearchInDescriptionOrStory_First = Config.Settings.Ini.KeyExists("SearchInDescriptionOrStory", ConfigName) ?
                    Config.Settings.Ini.ReadBool("SearchInDescriptionOrStory", ConfigName) : false;

                SearchInMd5Hash = SearchInMd5Hash_First = Config.Settings.Ini.KeyExists("SearchInMd5Hash", ConfigName) ?
                    Config.Settings.Ini.ReadBool("SearchInMd5Hash", ConfigName) : false;

                PicturePinup = PicturePinup_First = Config.Settings.Ini.KeyExists("PicturePinup", ConfigName) ?
                    Config.Settings.Ini.ReadBool("PicturePinup", ConfigName) : true;

                Sketch = Sketch_First = Config.Settings.Ini.KeyExists("Sketch", ConfigName) ?
                    Config.Settings.Ini.ReadBool("Sketch", ConfigName) : true;

                PictureSeries = PictureSeries_First = Config.Settings.Ini.KeyExists("PictureSeries", ConfigName) ?
                    Config.Settings.Ini.ReadBool("PictureSeries", ConfigName) : true;

                Comic = Comic_First = Config.Settings.Ini.KeyExists("Comic", ConfigName) ?
                    Config.Settings.Ini.ReadBool("Comic", ConfigName) : true;

                Portfolio = Portfolio_First = Config.Settings.Ini.KeyExists("Portfolio", ConfigName) ?
                    Config.Settings.Ini.ReadBool("Portfolio", ConfigName) : true;

                FlashAnimation = FlashAnimation_First = Config.Settings.Ini.KeyExists("FlashAnimation", ConfigName) ?
                    Config.Settings.Ini.ReadBool("FlashAnimation", ConfigName) : true;

                FlashInteractive = FlashInteractive_First = Config.Settings.Ini.KeyExists("FlashInteractive", ConfigName) ?
                    Config.Settings.Ini.ReadBool("FlashInteractive", ConfigName) : true;

                VideoAnimation = VideoAnimation_First = Config.Settings.Ini.KeyExists("VideoAnimation", ConfigName) ?
                    Config.Settings.Ini.ReadBool("VideoAnimation", ConfigName) : true;

                MusicSingleTrack = MusicSingleTrack_First = Config.Settings.Ini.KeyExists("MusicSingleTrack", ConfigName) ?
                    Config.Settings.Ini.ReadBool("MusicSingleTrack", ConfigName) : false;

                MusicAlbum = MusicAlbum_First = Config.Settings.Ini.KeyExists("MusicAlbum", ConfigName) ?
                    Config.Settings.Ini.ReadBool("MusicAlbum", ConfigName) : false;

                VideoFeatureLength = VideoFeatureLength_First = Config.Settings.Ini.KeyExists("VideoFeatureLength", ConfigName) ?
                    Config.Settings.Ini.ReadBool("VideoFeatureLength", ConfigName) : false;

                WritingDocument = WritingDocument_First = Config.Settings.Ini.KeyExists("WritingDocument", ConfigName) ?
                    Config.Settings.Ini.ReadBool("WritingDocument", ConfigName) : true;

                CharacterSheet = CharacterSheet_First = Config.Settings.Ini.KeyExists("CharacterSheet", ConfigName) ?
                    Config.Settings.Ini.ReadBool("CharacterSheet", ConfigName) : true;

                Photography = Photography_First = Config.Settings.Ini.KeyExists("Photography", ConfigName) ?
                    Config.Settings.Ini.ReadBool("Photography", ConfigName) : true;

                SeparateMultiPosts = SeparateMultiPosts_First = Config.Settings.Ini.KeyExists("SeparateMultiPosts", ConfigName) ?
                    Config.Settings.Ini.ReadBool("SeparateMultiPosts", ConfigName) : true;

                FileNameSchema = FileNameSchema_First = Config.Settings.Ini.KeyExists("FileNameSchema", ConfigName) ?
                    Config.Settings.Ini.ReadString("FileNameSchema", ConfigName) : "%id%";

                FileNameSchemaMultiPost = FileNameSchemaMultiPost_First = Config.Settings.Ini.KeyExists("FileNameSchemaMultiPost", ConfigName) ?
                    Config.Settings.Ini.ReadString("FileNameSchemaMultiPost", ConfigName) : "%id%_%multipostindex%";

                DownloadGraylistedSubmissions = DownloadGraylistedSubmissions_First = Config.Settings.Ini.KeyExists("DownloadGraylistedSubmissions", ConfigName) ?
                    Config.Settings.Ini.ReadBool("DownloadGraylistedSubmissions", ConfigName) : true;

                DownloadBlacklistedSubmissions = DownloadBlacklistedSubmissions_First = Config.Settings.Ini.KeyExists("DownloadBlacklistedSubmissions", ConfigName) ?
                    Config.Settings.Ini.ReadBool("DownloadBlacklistedSubmissions", ConfigName) : false;

                SeparateNonImages = SeparateNonImages_First = Config.Settings.Ini.KeyExists("SeparateNonImages", ConfigName) ?
                    Config.Settings.Ini.ReadBool("SeparateNonImages", ConfigName) : true;

                ImageLimit = ImageLimit_First = Config.Settings.Ini.KeyExists("ImageLimit", ConfigName) ?
                    Config.Settings.Ini.ReadInt("ImageLimit", ConfigName) : 0;

                PageLimit = PageLimit_First = Config.Settings.Ini.KeyExists("PageLimit", ConfigName) ?
                    Config.Settings.Ini.ReadInt("PageLimit", ConfigName) : 0;
            }
            else {
                ReadWarning = ReadWarning_First = aphrodite.Settings.InkBunny.Default.ReadWarning;
                SessionID = SessionID_First = aphrodite.Settings.InkBunny.Default.SessionID;
                LoggedInUsername = LoggedInUsername_First = aphrodite.Settings.InkBunny.Default.LoggedInUsername;
                GuestPriority = GuestPriority_First = aphrodite.Settings.InkBunny.Default.GuestPriority;
                GuestSessionID = GuestSessionID_First = aphrodite.Settings.InkBunny.Default.GuestSessionID;
                GuestGeneral = GuestGeneral_First = aphrodite.Settings.InkBunny.Default.GuestGeneral;
                GuestMatureNudity = GuestMatureNudity_First = aphrodite.Settings.InkBunny.Default.GuestMatureNudity;
                GuestMatureViolence = GuestMatureViolence_First = aphrodite.Settings.InkBunny.Default.GuestMatureViolence;
                GuestAdultSexualThemes = GuestAdultSexualThemes_First = aphrodite.Settings.InkBunny.Default.GuestAdultSexualThemes;
                GuestAdultStrongViolence = GuestAdultStrongViolence_First = aphrodite.Settings.InkBunny.Default.GuestAdultStrongViolence;
                General = General_First = aphrodite.Settings.InkBunny.Default.General;
                MatureNudity = MatureNudity_First = aphrodite.Settings.InkBunny.Default.MatureNudity;
                MatureViolence = MatureViolence_First = aphrodite.Settings.InkBunny.Default.MatureViolence;
                AdultSexualThemes = AdultSexualThemes_First = aphrodite.Settings.InkBunny.Default.AdultSexualThemes;
                AdultStrongViolence = AdultStrongViolence_First = aphrodite.Settings.InkBunny.Default.AdultStrongViolence;
                SearchInKeywords = SearchInKeywords_First = aphrodite.Settings.InkBunny.Default.SearchInKeywords;
                SearchInTitle = SearchInTitle_First = aphrodite.Settings.InkBunny.Default.SearchInTitle;
                SearchInDescriptionOrStory = SearchInDescriptionOrStory_First = aphrodite.Settings.InkBunny.Default.SearchInDescriptionOrStory;
                SearchInMd5Hash = SearchInMd5Hash_First = aphrodite.Settings.InkBunny.Default.SearchInMd5Hash;
                PicturePinup = PicturePinup_First = aphrodite.Settings.InkBunny.Default.PicturePinup;
                Sketch = Sketch_First = aphrodite.Settings.InkBunny.Default.Sketch;
                PictureSeries = PictureSeries_First = aphrodite.Settings.InkBunny.Default.PictureSeries;
                Comic = Comic_First = aphrodite.Settings.InkBunny.Default.Comic;
                Portfolio = Portfolio_First = aphrodite.Settings.InkBunny.Default.Portfolio;
                FlashAnimation = FlashAnimation_First = aphrodite.Settings.InkBunny.Default.FlashAnimation;
                FlashInteractive = FlashInteractive_First = aphrodite.Settings.InkBunny.Default.FlashInteractive;
                VideoAnimation = VideoAnimation_First = aphrodite.Settings.InkBunny.Default.VideoAnimation;
                MusicSingleTrack = MusicSingleTrack_First = aphrodite.Settings.InkBunny.Default.MusicSingleTrack;
                MusicAlbum = MusicAlbum_First = aphrodite.Settings.InkBunny.Default.MusicAlbum;
                VideoFeatureLength = VideoFeatureLength_First = aphrodite.Settings.InkBunny.Default.VideoFeatureLength;
                WritingDocument = WritingDocument_First = aphrodite.Settings.InkBunny.Default.WritingDocument;
                CharacterSheet = CharacterSheet_First = aphrodite.Settings.InkBunny.Default.CharacterSheet;
                Photography = Photography_First = aphrodite.Settings.InkBunny.Default.Photography;
                SeparateMultiPosts = SeparateMultiPosts_First = aphrodite.Settings.InkBunny.Default.SeparateMultiPosts;
                FileNameSchema = FileNameSchema_First = aphrodite.Settings.InkBunny.Default.FileNameSchema;
                FileNameSchemaMultiPost = FileNameSchemaMultiPost_First = aphrodite.Settings.InkBunny.Default.FileNameSchemaMultiPost;
                DownloadGraylistedSubmissions = DownloadGraylistedSubmissions_First = aphrodite.Settings.InkBunny.Default.DownloadGraylistedSubmissions;
                DownloadBlacklistedSubmissions = DownloadBlacklistedSubmissions_First = aphrodite.Settings.InkBunny.Default.DownloadBlacklistedSubmissions;
                SeparateNonImages = SeparateNonImages_First = aphrodite.Settings.InkBunny.Default.SeparateNonImages;
                ImageLimit = ImageLimit_First = aphrodite.Settings.InkBunny.Default.ImageLimit;
                PageLimit = PageLimit_First = aphrodite.Settings.InkBunny.Default.PageLimit;
            }
        }

        /// <summary>
        /// Force-saves the configuration, overwriting any saved values.
        /// This method does not check for previous values.
        /// </summary>
        public void ForceSave() {
            Log.Write("Force saving Config_InkBunny");
            if (Config.Settings.UseIni) {
                Config.Settings.Ini.Write("ReadWarning", ReadWarning, ConfigName);
                Config.Settings.Ini.Write("SessionID", SessionID, ConfigName);
                Config.Settings.Ini.Write("LoggedInUsername", LoggedInUsername, ConfigName);
                Config.Settings.Ini.Write("GuestPriority", GuestPriority, ConfigName);
                Config.Settings.Ini.Write("GuestSessionID", GuestSessionID, ConfigName);
                Config.Settings.Ini.Write("GuestGeneral", GuestGeneral, ConfigName);
                Config.Settings.Ini.Write("GuestMatureNudity", GuestMatureNudity, ConfigName);
                Config.Settings.Ini.Write("GuestMatureViolence", GuestMatureViolence, ConfigName);
                Config.Settings.Ini.Write("GuestAdultSexualThemes", GuestAdultSexualThemes, ConfigName);
                Config.Settings.Ini.Write("GuestAdultStrongViolence", GuestAdultStrongViolence, ConfigName);
                Config.Settings.Ini.Write("General", General, ConfigName);
                Config.Settings.Ini.Write("MatureNudity", MatureNudity, ConfigName);
                Config.Settings.Ini.Write("MatureViolence", MatureViolence, ConfigName);
                Config.Settings.Ini.Write("AdultSexualThemes", AdultSexualThemes, ConfigName);
                Config.Settings.Ini.Write("AdultStrongViolence", AdultStrongViolence, ConfigName);
                Config.Settings.Ini.Write("SearchInKeywords", SearchInKeywords, ConfigName);
                Config.Settings.Ini.Write("SearchInTitle", SearchInTitle, ConfigName);
                Config.Settings.Ini.Write("SearchInDescriptionOrStory", SearchInDescriptionOrStory, ConfigName);
                Config.Settings.Ini.Write("SearchInMd5Hash", SearchInMd5Hash, ConfigName);
                Config.Settings.Ini.Write("PicturePinup", PicturePinup, ConfigName);
                Config.Settings.Ini.Write("Sketch", Sketch, ConfigName);
                Config.Settings.Ini.Write("PictureSeries", PictureSeries, ConfigName);
                Config.Settings.Ini.Write("Comic", Comic, ConfigName);
                Config.Settings.Ini.Write("Portfolio", Portfolio, ConfigName);
                Config.Settings.Ini.Write("FlashAnimation", FlashAnimation, ConfigName);
                Config.Settings.Ini.Write("FlashInteractive", FlashInteractive, ConfigName);
                Config.Settings.Ini.Write("VideoAnimation", VideoAnimation, ConfigName);
                Config.Settings.Ini.Write("MusicSingleTrack", MusicSingleTrack, ConfigName);
                Config.Settings.Ini.Write("MusicAlbum", MusicAlbum, ConfigName);
                Config.Settings.Ini.Write("VideoFeatureLength", VideoFeatureLength, ConfigName);
                Config.Settings.Ini.Write("WritingDocument", WritingDocument, ConfigName);
                Config.Settings.Ini.Write("CharacterSheet", CharacterSheet, ConfigName);
                Config.Settings.Ini.Write("Photography", Photography, ConfigName);
                Config.Settings.Ini.Write("SeparateMultiPosts", SeparateMultiPosts, ConfigName);
                Config.Settings.Ini.Write("FileNameSchema", FileNameSchema, ConfigName);
                Config.Settings.Ini.Write("FileNameSchemaMultiPost", FileNameSchemaMultiPost, ConfigName);
                Config.Settings.Ini.Write("DownloadGraylistedSubmissions", DownloadGraylistedSubmissions, ConfigName);
                Config.Settings.Ini.Write("DownloadBlacklistedSubmissions", DownloadBlacklistedSubmissions, ConfigName);
                Config.Settings.Ini.Write("SeparateNonImages", SeparateNonImages, ConfigName);
                Config.Settings.Ini.Write("ImageLimit", ImageLimit, ConfigName);
                Config.Settings.Ini.Write("PageLimit", PageLimit, ConfigName);
            }
            else {
                aphrodite.Settings.InkBunny.Default.ReadWarning = ReadWarning;
                aphrodite.Settings.InkBunny.Default.SessionID = SessionID;
                aphrodite.Settings.InkBunny.Default.LoggedInUsername = LoggedInUsername;
                aphrodite.Settings.InkBunny.Default.GuestPriority = GuestPriority;
                aphrodite.Settings.InkBunny.Default.GuestSessionID = GuestSessionID;
                aphrodite.Settings.InkBunny.Default.GuestGeneral = GuestGeneral;
                aphrodite.Settings.InkBunny.Default.GuestMatureNudity = GuestMatureNudity;
                aphrodite.Settings.InkBunny.Default.GuestMatureViolence = GuestMatureViolence;
                aphrodite.Settings.InkBunny.Default.GuestAdultSexualThemes = GuestAdultSexualThemes;
                aphrodite.Settings.InkBunny.Default.GuestAdultStrongViolence = GuestAdultStrongViolence;
                aphrodite.Settings.InkBunny.Default.General = General;
                aphrodite.Settings.InkBunny.Default.MatureNudity = MatureNudity;
                aphrodite.Settings.InkBunny.Default.MatureViolence = MatureViolence;
                aphrodite.Settings.InkBunny.Default.AdultSexualThemes = AdultSexualThemes;
                aphrodite.Settings.InkBunny.Default.AdultStrongViolence = AdultStrongViolence;
                aphrodite.Settings.InkBunny.Default.SearchInKeywords = SearchInKeywords;
                aphrodite.Settings.InkBunny.Default.SearchInTitle = SearchInTitle;
                aphrodite.Settings.InkBunny.Default.SearchInDescriptionOrStory = SearchInDescriptionOrStory;
                aphrodite.Settings.InkBunny.Default.SearchInMd5Hash = SearchInMd5Hash;
                aphrodite.Settings.InkBunny.Default.PicturePinup = PicturePinup;
                aphrodite.Settings.InkBunny.Default.Sketch = Sketch;
                aphrodite.Settings.InkBunny.Default.PictureSeries = PictureSeries;
                aphrodite.Settings.InkBunny.Default.Comic = Comic;
                aphrodite.Settings.InkBunny.Default.Portfolio = Portfolio;
                aphrodite.Settings.InkBunny.Default.FlashAnimation = FlashAnimation;
                aphrodite.Settings.InkBunny.Default.FlashInteractive = FlashInteractive;
                aphrodite.Settings.InkBunny.Default.VideoAnimation = VideoAnimation;
                aphrodite.Settings.InkBunny.Default.MusicSingleTrack = MusicSingleTrack;
                aphrodite.Settings.InkBunny.Default.MusicAlbum = MusicAlbum;
                aphrodite.Settings.InkBunny.Default.VideoFeatureLength = VideoFeatureLength;
                aphrodite.Settings.InkBunny.Default.WritingDocument = WritingDocument;
                aphrodite.Settings.InkBunny.Default.CharacterSheet = CharacterSheet;
                aphrodite.Settings.InkBunny.Default.Photography = Photography;
                aphrodite.Settings.InkBunny.Default.SeparateMultiPosts = SeparateMultiPosts;
                aphrodite.Settings.InkBunny.Default.FileNameSchema = FileNameSchema;
                aphrodite.Settings.InkBunny.Default.FileNameSchemaMultiPost = FileNameSchemaMultiPost;
                aphrodite.Settings.InkBunny.Default.DownloadGraylistedSubmissions = DownloadGraylistedSubmissions;
                aphrodite.Settings.InkBunny.Default.DownloadBlacklistedSubmissions = DownloadBlacklistedSubmissions;
                aphrodite.Settings.InkBunny.Default.SeparateNonImages = SeparateNonImages;
                aphrodite.Settings.InkBunny.Default.ImageLimit = ImageLimit;
                aphrodite.Settings.InkBunny.Default.PageLimit = PageLimit;
                aphrodite.Settings.InkBunny.Default.Save();
            }
            ReadWarning_First = ReadWarning;
            SessionID_First = SessionID;
            LoggedInUsername_First = LoggedInUsername;
            GuestPriority_First = GuestPriority;
            GuestSessionID_First = GuestSessionID;
            GuestGeneral_First = GuestGeneral;
            GuestMatureNudity_First = GuestMatureNudity;
            GuestMatureViolence_First = GuestMatureViolence;
            GuestAdultSexualThemes_First = GuestAdultSexualThemes;
            GuestAdultStrongViolence_First = GuestAdultStrongViolence;
            General_First = General;
            MatureNudity_First = MatureNudity;
            MatureViolence_First = MatureViolence;
            AdultSexualThemes_First = AdultSexualThemes;
            AdultStrongViolence_First = AdultStrongViolence;
            SearchInKeywords_First = SearchInKeywords;
            SearchInTitle_First = SearchInTitle;
            SearchInDescriptionOrStory_First = SearchInDescriptionOrStory;
            SearchInMd5Hash_First = SearchInMd5Hash;
            PicturePinup_First = PicturePinup;
            Sketch_First = Sketch;
            PictureSeries_First = PictureSeries;
            Comic_First = Comic;
            Portfolio_First = Portfolio;
            FlashAnimation_First = FlashAnimation;
            FlashInteractive_First = FlashInteractive;
            VideoAnimation_First = VideoAnimation;
            MusicSingleTrack_First = MusicSingleTrack;
            MusicAlbum_First = MusicAlbum;
            VideoFeatureLength_First = VideoFeatureLength;
            WritingDocument_First = WritingDocument;
            CharacterSheet_First = CharacterSheet;
            Photography_First = Photography;
            SeparateMultiPosts_First = SeparateMultiPosts;
            FileNameSchema_First = FileNameSchema;
            FileNameSchemaMultiPost_First = FileNameSchemaMultiPost;
            DownloadGraylistedSubmissions_First = DownloadGraylistedSubmissions;
            DownloadBlacklistedSubmissions_First = DownloadBlacklistedSubmissions;
            SeparateNonImages_First = SeparateNonImages;
            ImageLimit_First = ImageLimit;
            PageLimit_First = PageLimit;
        }

    }

    /// <summary>
    /// Main Imgur configuration class that handles Imgur-related settings.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class Config_Imgur {
        /// <summary>
        /// The ini section name of the current configuration class.
        /// </summary>
        private const string ConfigName = "Imgur";

        #region Fields
        /// <summary>
        /// Whether the Imgur API Client ID encryption/decryption warning was displayed.
        /// </summary>
        public bool ReadWarning;
        /// <summary>
        /// Whether Imgur downloads will separate non-images into their own extension folder.
        /// </summary>
        public bool SeparateNonImages;
        /// <summary>
        /// The limit of images to download. 0 is off.
        /// </summary>
        public int ImageLimit;
        /// <summary>
        /// The encrypted client ID.
        /// </summary>
        public string ClientID;
        /// <summary>
        /// The file-name schema used to translate album-specific data into the file name.
        /// </summary>
        public string FileNameSchema;

        private bool ReadWarning_First;
        private bool SeparateNonImages_First;
        private int ImageLimit_First;
        private string ClientID_First;
        private string FileNameSchema_First;
        #endregion

        /// <summary>
        /// Saves the configuration to the respective save location.
        /// </summary>
        public void Save() {
            Log.Write("Saving Config_Imgur");
            if (Config.Settings.UseIni) {
                if (ReadWarning_First != ReadWarning) {
                    Config.Settings.Ini.Write("ReadWarning", ReadWarning, ConfigName);
                    ReadWarning_First = ReadWarning;
                }
                if (ClientID_First != ClientID) {
                    Config.Settings.Ini.Write("ClientID", ClientID, ConfigName);
                    ClientID_First = ClientID;
                }
                if (FileNameSchema_First != FileNameSchema) {
                    Config.Settings.Ini.Write("FileNameSchema", FileNameSchema, ConfigName);
                    FileNameSchema_First = FileNameSchema;
                }
                if (SeparateNonImages_First != SeparateNonImages) {
                    Config.Settings.Ini.Write("SeparateNonImages", SeparateNonImages, ConfigName);
                    SeparateNonImages_First = SeparateNonImages;
                }
                if (ImageLimit_First != ImageLimit) {
                    Config.Settings.Ini.Write("ImageLimit", ImageLimit, ConfigName);
                    ImageLimit_First = ImageLimit;
                }
            }
            else {
                bool Save = false;

                if (aphrodite.Settings.Imgur.Default.ReadWarning != ReadWarning) {
                    aphrodite.Settings.Imgur.Default.ReadWarning = ReadWarning_First = ReadWarning;
                    Save = true;
                }
                if (aphrodite.Settings.Imgur.Default.ClientID != ClientID) {
                    aphrodite.Settings.Imgur.Default.ClientID = ClientID_First = ClientID;
                    Save = true;
                }
                if (aphrodite.Settings.Imgur.Default.FileNameSchema != FileNameSchema) {
                    aphrodite.Settings.Imgur.Default.FileNameSchema = FileNameSchema_First = FileNameSchema;
                    Save = true;
                }
                if (aphrodite.Settings.Imgur.Default.SeparateNonImages != SeparateNonImages) {
                    aphrodite.Settings.Imgur.Default.SeparateNonImages = SeparateNonImages_First = SeparateNonImages;
                    Save = true;
                }
                if (aphrodite.Settings.Imgur.Default.ImageLimit != ImageLimit) {
                    aphrodite.Settings.Imgur.Default.ImageLimit = ImageLimit_First = ImageLimit;
                    Save = true;
                }

                if (Save) {
                    aphrodite.Settings.Imgur.Default.Save();
                }
            }
        }

        /// <summary>
        /// Loads the configuration from the respective save location.
        /// </summary>
        public void Load() {
            Log.Write("Loading Config_Imgur");
            if (Config.Settings.UseIni) {
                ReadWarning = ReadWarning_First = Config.Settings.Ini.KeyExists("ReadWarning", ConfigName) ?
                    Config.Settings.Ini.ReadBool("ReadWarning", ConfigName) : false;

                ClientID = ClientID_First = Config.Settings.Ini.KeyExists("ClientID", ConfigName) ?
                    Config.Settings.Ini.ReadString("ClientID", ConfigName) : string.Empty;

                FileNameSchema = FileNameSchema_First = Config.Settings.Ini.KeyExists("FileNameSchema", ConfigName) ?
                    Config.Settings.Ini.ReadString("FileNameSchema", ConfigName) : "%imageindex%_%imageid%";

                SeparateNonImages = SeparateNonImages_First = Config.Settings.Ini.KeyExists("SeparateNonImages", ConfigName) ?
                    Config.Settings.Ini.ReadBool("SeparateNonImages", ConfigName) : true;

                ImageLimit = ImageLimit_First = Config.Settings.Ini.KeyExists("ImageLimit", ConfigName) ?
                    Config.Settings.Ini.ReadInt("ImageLimit", ConfigName) : 0;
            }
            else {
                ReadWarning = ReadWarning_First = aphrodite.Settings.Imgur.Default.ReadWarning;
                ClientID = ClientID_First = aphrodite.Settings.Imgur.Default.ClientID;
                FileNameSchema = FileNameSchema_First = aphrodite.Settings.Imgur.Default.FileNameSchema;
                SeparateNonImages = SeparateNonImages_First = aphrodite.Settings.Imgur.Default.SeparateNonImages;
                ImageLimit = ImageLimit_First = aphrodite.Settings.Imgur.Default.ImageLimit;
            }
        }

        /// <summary>
        /// Force-saves the configuration, overwriting any saved values.
        /// This method does not check for previous values.
        /// </summary>
        public void ForceSave() {
            Log.Write("Force saving Config_Imgur");
            if (Config.Settings.UseIni) {
                Config.Settings.Ini.Write("ReadWarning", ReadWarning, ConfigName);
                Config.Settings.Ini.Write("ClientID", ClientID, ConfigName);
                Config.Settings.Ini.Write("FileNameSchema", FileNameSchema, ConfigName);
                Config.Settings.Ini.Write("SeparateNonImages", SeparateNonImages, ConfigName);
                Config.Settings.Ini.Write("ImageLimit", ImageLimit, ConfigName);
            }
            else {
                aphrodite.Settings.Imgur.Default.ReadWarning = ReadWarning;
                aphrodite.Settings.Imgur.Default.ClientID = ClientID;
                aphrodite.Settings.Imgur.Default.FileNameSchema = FileNameSchema;
                aphrodite.Settings.Imgur.Default.SeparateNonImages = SeparateNonImages;
                aphrodite.Settings.Imgur.Default.ImageLimit = ImageLimit;
                aphrodite.Settings.Imgur.Default.Save();
            }
            ReadWarning_First = ReadWarning;
            ClientID_First = ClientID;
            FileNameSchema_First = FileNameSchema;
            SeparateNonImages_First = SeparateNonImages;
            ImageLimit_First = ImageLimit;
        }
    }

    /// <summary>
    /// Main class that handles the pool wishlist functionality.
    /// All saving/loading/appending must be done through this class.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class PoolWishlist {
        /// <summary>
        /// The absolute path of the pool wishlist file, if portable mode is enabled.
        /// </summary>
        public static readonly string WishlistFile = Environment.CurrentDirectory + "\\PoolWishlist.cfg";

        /// <summary>
        /// The list of pool URLs, with the respective name in the same index in <see cref="PoolNames"/>.
        /// </summary>
        public static List<string> PoolURLs;
        /// <summary>
        /// The list of pool names, with the respective URL in the same index in <see cref="PoolURLs"/>.
        /// </summary>
        public static List<string> PoolNames;

        /// <summary>
        /// Loads the wishlist.
        /// </summary>
        public static void LoadWishlist() {
            PoolURLs = new();
            PoolNames = new();
            if (Config.Settings.UseIni) {
                if (File.Exists(WishlistFile) && new FileInfo(WishlistFile).Length > 0) {
                    string[] WishlistFileBuffer = File.ReadAllLines(WishlistFile);
                    string[] CurrentPool;
                    for (int CurrentPoolIndex = 0; CurrentPoolIndex < WishlistFileBuffer.Length; CurrentPoolIndex++) {
                        CurrentPool = WishlistFileBuffer[CurrentPoolIndex].Split('|');
                        if (CurrentPool.Length > 0) {
                            PoolURLs.Add(CurrentPool[0]);
                            PoolNames.Add(CurrentPool.Length > 1 ? CurrentPool[1] : "No name");
                        }
                    }
                }
            }
            else {
                PoolURLs = new(aphrodite.Settings.Pools.Default.wishlist.Split('|'));
                PoolNames = new(aphrodite.Settings.Pools.Default.wishlistNames.Split('|'));
            }
        }

        /// <summary>
        /// Appends a new pool to the wishlist.
        /// </summary>
        /// <param name="URL">The URL of the pool that will be appended.</param>
        /// <param name="Name">The Name of the pool that will be appended.</param>
        public static void AppendToWishlist(string URL, string Name) {
            Log.Write("Appending new Wishlist pool");
            if (string.IsNullOrWhiteSpace(URL)) {
                Log.ReportException(new ArgumentException("The pool url was null, empty, or whitespace."));
                return;
            }

            URL = URL.Split('?')[0];
            Name = Name.Replace("|", "_");
            
            if (Config.Settings.Pools.addWishlistSilent) {
                if (Config.Settings.UseIni) {
                    if (File.Exists(WishlistFile) && new FileInfo(WishlistFile).Length >= 0) {
                        if (!File.ReadAllLines(WishlistFile).Any(x => x.StartsWith(URL))) {
                            File.AppendAllText(WishlistFile, $"\n{URL}|{Name}");
                            System.Media.SystemSounds.Asterisk.Play();
                        }
                        else {
                            System.Media.SystemSounds.Exclamation.Play();
                        }
                    }
                    else {
                        File.AppendAllText(WishlistFile, $"{URL}|{Name}");
                        System.Media.SystemSounds.Asterisk.Play();
                    }
                }
                else {
                    if (string.IsNullOrWhiteSpace(aphrodite.Settings.Pools.Default.wishlistNames) && string.IsNullOrWhiteSpace(aphrodite.Settings.Pools.Default.wishlist)) {
                        aphrodite.Settings.Pools.Default.wishlist = URL;
                        aphrodite.Settings.Pools.Default.wishlistNames = Name;
                        System.Media.SystemSounds.Asterisk.Play();
                    }
                    else {
                        if (!aphrodite.Settings.Pools.Default.wishlist.Contains(URL)) {
                            aphrodite.Settings.Pools.Default.wishlist += "|" + URL;
                            aphrodite.Settings.Pools.Default.wishlistNames += "|" + Name;
                            System.Media.SystemSounds.Asterisk.Play();
                        }
                        else {
                            System.Media.SystemSounds.Exclamation.Play();
                        }
                    }
                }
            }
            else {
                LoadWishlist();
                if (PoolURLs.Contains(URL)) {
                    if (Log.MessageBox($"The pool already exists in the pool wishlist.\n\nPool: \"{Name}\"\nURL: \"{URL}\"\n\nWould you like to view the wishlist?", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes) {
                        using frmPoolWishlist Wishlist = new();
                        Wishlist.ShowDialog();
                    }
                }
                else {
                    using frmPoolWishlist Wishlist = new(URL, Name);
                    Wishlist.ShowDialog();
                }
            }
        }

        /// <summary>
        /// Saves the wishlist.
        /// </summary>
        public static void SaveWishlist() {
            Log.Write("Updating pool wishlist");

            if (PoolURLs.Count != PoolNames.Count) {
                Log.ReportException(new ArgumentException("\"URLs\" and \"Names\" lists count do not match. To prevent failure, the pools will not be saved."));
                return;
            }

            if (PoolURLs.Count > 0 && PoolNames.Count > 0) {
                if (Config.Settings.UseIni) {
                    string FileOutput = string.Empty;
                    for (int i = 0; i < PoolURLs.Count; i++) {
                        FileOutput += PoolURLs[i] + "|" + PoolNames[i].Replace("|", "_") + "\r\n";
                    }
                    FileOutput = FileOutput.Trim('\n').Trim('\r');
                    File.WriteAllText(Program.ApplicationPath + "\\PoolWishlist.cfg", FileOutput);

                }
                else {
                    aphrodite.Settings.Pools.Default.wishlist = string.Join("|", PoolURLs);
                    aphrodite.Settings.Pools.Default.wishlistNames = string.Join("|", PoolNames);
                    aphrodite.Settings.Pools.Default.Save();
                }
            }
            else {
                if (Config.Settings.UseIni) {
                    if (File.Exists(WishlistFile)) {
                        File.Delete(WishlistFile);
                    }
                }
                else {
                    aphrodite.Settings.Pools.Default.wishlist = string.Empty;
                    aphrodite.Settings.Pools.Default.wishlistNames = string.Empty;
                    aphrodite.Settings.Pools.Default.Save();
                }
            }
        }
    }

    /// <summary>
    /// Main class that handles reading and writing to the registry.
    /// Used for setting/updating/checking the protocol.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    internal class SystemRegistry {

        /// <summary>
        /// The RegistryKey that's used to verify the protocol.
        /// </summary>
        private static RegistryKey AphroditeRegistryKey = null;

        /// <summary>
        /// Checks if the protocol is installed and pointing to the current program path.
        /// </summary>
        /// <returns></returns>
        public static bool CheckRegistryKey() {
            AphroditeRegistryKey = Registry.ClassesRoot.OpenSubKey("aphrodite\\shell\\open\\command", false);
            if (Registry.ClassesRoot.GetValue("aphrodite", "URL Protocol") != null
            && AphroditeRegistryKey != null 
            && AphroditeRegistryKey.GetValue("").ToString() == $"\"{Program.FullApplicationPath}\" \"%1\"") {
                AphroditeRegistryKey.Close();
                AphroditeRegistryKey.Dispose();
                return true;
            }
            else {
                AphroditeRegistryKey?.Close();
                AphroditeRegistryKey?.Dispose();
                return false;
            }
        }

        /// <summary>
        /// Sets the protocol in the registry to the current program path.
        /// </summary>
        /// <returns></returns>
        public static bool SetRegistryKey() {
            if (!Program.IsAdmin) return false;
            try {
                //Registry.ClassesRoot.CreateSubKey("aphrodite");
                //AphroditeRegistryKey = Registry.ClassesRoot.OpenSubKey("aphrodite", true);
                AphroditeRegistryKey = Registry.ClassesRoot.CreateSubKey("aphrodite");
                AphroditeRegistryKey.SetValue("URL Protocol", "");
                AphroditeRegistryKey.Close();

                //Registry.ClassesRoot.CreateSubKey("aphrodite\\shell");
                //Registry.ClassesRoot.CreateSubKey("aphrodite\\shell\\open");
                //Registry.ClassesRoot.CreateSubKey("aphrodite\\shell\\open\\command");
                //AphroditeRegistryKey = Registry.ClassesRoot.OpenSubKey("aphrodite\\shell\\open\\command", true);
                AphroditeRegistryKey = Registry.ClassesRoot.CreateSubKey("aphrodite\\shell\\open\\command");
                AphroditeRegistryKey.SetValue("", $"\"{Program.FullApplicationPath}\" \"%1\"");
                AphroditeRegistryKey.Close();

                //Registry.ClassesRoot.CreateSubKey("aphrodite\\DefaultIcon");
                //AphroditeRegistryKey = Registry.ClassesRoot.OpenSubKey("aphrodite\\DefaultIcon", true);
                AphroditeRegistryKey = Registry.ClassesRoot.CreateSubKey("aphrodite\\DefaultIcon");
                AphroditeRegistryKey.SetValue("", $"\"{Program.FullApplicationPath}\"");
                AphroditeRegistryKey.Close();
                AphroditeRegistryKey.Dispose();

                bool OldTagsProtocolExists = Registry.ClassesRoot.OpenSubKey("tags") != null,
                     OldPoolProtocolExists = Registry.ClassesRoot.OpenSubKey("pools") != null,
                     OldImagesProtocolExists = Registry.ClassesRoot.OpenSubKey("images") != null,
                     OldPoolWishlistProtocolExists = Registry.ClassesRoot.OpenSubKey("poolwl") != null,
                     OldProtocolExists = 
                         OldTagsProtocolExists || OldPoolProtocolExists || OldImagesProtocolExists || OldPoolWishlistProtocolExists;

                if (OldProtocolExists
                && Log.MessageBox("One or more protocols exist using the old formats. Would you like to delete them?", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes) {
                    if (OldTagsProtocolExists) {
                        Registry.ClassesRoot.DeleteSubKey("tags");
                    }
                    if (OldPoolProtocolExists) {
                        Registry.ClassesRoot.DeleteSubKey("pools");
                    }
                    if (OldImagesProtocolExists) {
                        Registry.ClassesRoot.DeleteSubKey("images");
                    }
                    if (OldPoolWishlistProtocolExists) {
                        Registry.ClassesRoot.DeleteSubKey("poolwl");
                    }
                }

                return true;
            }
            catch (Exception ex) {
                Log.ReportException(ex);
                return false;
            }
        }

    }

    /// <summary>
    /// The class containing the ini file handling.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    internal class IniFile {

        /// <summary>
        /// The full path of the Ini File (Generally, in the same folder as the executable)
        /// </summary>
        private string IniPath;

        /// <summary>
        /// The name of the executing file.
        /// </summary>
        private const string ExecutableName = "aphrodite"; // Assembly.GetExecutingAssembly().GetName().Name;
        /// <summary>
        /// Const (A)RGB color format.
        /// </summary>
        private const string RegexARGB = "^([0-9]{1,3},( )?)?[0-9]{1,3},( )?[0-9]{1,3},( )?[0-9]{1,3}$";

        /// <summary>
        /// The IniFile Constructor.
        /// </summary>
        /// <param name="NewIniPath">The string path of the ini file. Defaults to executing directory.</param>
        public IniFile(string Path = null) {
            ChangeIniPath(Path);
        }

        /// <summary>
        /// Constructor with out UseIni
        /// </summary>
        /// <param name="Path">The absolute path to the ini file.</param>
        /// <param name="UseIni">Output of whether the "UseIni" key exists and is true/false.</param>
        public IniFile(string Path, out bool UseIni) {
            ChangeIniPath(Path);
            UseIni = File.Exists(Path) && KeyExists("UseIni") && ReadBool("UseIni");
        }

        /// <summary>
        /// Changes the ini path to a new location.
        /// </summary>
        /// <param name="NewIniPath">The full path of the ini.</param>
        public void ChangeIniPath(string NewIniPath = null, bool MoveIni = false) {
            if (NewIniPath != null) {
                if (!NewIniPath.ToLower().EndsWith(".ini")) {
                    NewIniPath += ".ini";
                }
                if (MoveIni && File.Exists(IniPath)) {
                    if (File.Exists(NewIniPath)) {
                        File.Delete(NewIniPath);
                    }
                    File.Move(IniPath, NewIniPath);
                }
            }
            IniPath = new FileInfo(NewIniPath ?? ExecutableName + ".ini").FullName.ToString();
        }

        /// <summary>
        /// Reads a very short string (1 character long) for key verification.
        /// </summary>
        /// <param name="Key">The string name of the key to retrieve.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        /// <returns>The string of the retrieved key.</returns>
        private string ReadShortString(string Key, string Section = null) {
            StringBuilder Value = new();
            NativeMethods.GetPrivateProfileString(Section ?? ExecutableName, Key, "", Value, 2, IniPath);
            return Value.ToString();
        }

        /// <summary>
        /// Returns a string of from the ini file, used internally.
        /// </summary>
        /// <param name="Key">The string name of the key to retrieve.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        /// <returns>The string of the retrieved key.</returns>
        private string Read(string Key, string Section = null) {
            StringBuilder Value = new(65535);
            NativeMethods.GetPrivateProfileString(Section ?? ExecutableName, Key, "", Value, Value.Capacity, IniPath);
            return Value.ToString();
        }

        /// <summary>
        /// Write a simple string to the ini file, used internally.
        /// </summary>
        /// <param name="Key">The string name of the key to write to.</param>
        /// <param name="Value">The string value to write to the key.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        private void WriteString(string Key, string Value, string Section = null) {
            NativeMethods.WritePrivateProfileString(Section ?? ExecutableName, Key, Value, IniPath);
        }

        /// <summary>
        /// Reads a string from the ini file.
        /// </summary>
        /// <param name="Key">The string name of the key to retrieve.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        /// <returns>The string of the retrieved key.</returns>
        public string ReadString(string Key, string Section = null) {
            return Read(Key, Section);
        }
        /// <summary>
        /// Reads a boolean from the ini file.
        /// </summary>
        /// <param name="Key">The string name of the key to retrieve.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        /// <returns>The bool of the retrieved key. Defaults to false.</returns>
        public bool ReadBool(string Key, string Section = null) {
            return Read(Key, Section).ToLower() switch {
                "true" or "1" => true,
                _ => false,
            };
        }
        /// <summary>
        /// Reads an int from the ini file.
        /// </summary>
        /// <param name="Key">The string name of the key to retrieve.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        /// <returns>The int of the retrieved key. Defaults to -1.</returns>
        public int ReadInt(string Key, string Section = null) {
            return int.TryParse(Read(Key, Section), out int NewInt) ? NewInt : -1;
        }
        /// <summary>
        /// Reads a decimal from the ini file.
        /// </summary>
        /// <param name="Key">The string name of the key to retrieve.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        /// <returns>The decimal of the retrieved key. Defaults to -1.</returns>
        public decimal ReadDecimal(string Key, string Section = null) {
            return decimal.TryParse(Read(Key, Section), out decimal NewDecimal) ? NewDecimal : -1;
        }
        /// <summary>
        /// Reads a Point from the ini file.
        /// </summary>
        /// <param name="Key">The string name of the key to retrieve.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        /// <returns>The Point of the retrieved key. Defaults to (-32000, -32000).</returns>
        public Point ReadPoint(string Key, string Section = null) {
            string[] Value = Read(Key, Section).Replace(" ", "").Split(',');
            if (Value.Length == 2 && int.TryParse(Value[0], out int OutX) && int.TryParse(Value[1], out int OutY)) {
                return new(OutX, OutY);
            }
            return new(-32000, -32000);
        }
        /// <summary>
        /// Reads a Size from the ini file.
        /// </summary>
        /// <param name="Key">The string name of the key to retrieve.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        /// <returns>The Size of the retrieved key. Defaults to (-32000, -32000).</returns>
        public Size ReadSize(string Key, string Section = null) {
            string[] Value = Read(Key, Section).Replace(" ", "").Split(',');
            if (Value.Length == 2 && int.TryParse(Value[0], out int OutW) && int.TryParse(Value[1], out int OutH)) {
                return new(OutW, OutH);
            }
            return new(-32000, -32000);
        }
        /// <summary>
        /// Reads a Color from the ini file, in either Hexadecimal or (A)RGB format.
        /// </summary>
        /// <param name="Key">The string name of the key to retrieve.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        /// <returns>The Point of the retrieved key. Defaults to ARGB(255, 128, 128, 128).</returns>
        public Color ReadColor(string Key, string Section = null) {
            string Value = Read(Key, Section).ToLower();
            string ColorIntBuffer = "";
            if (System.Text.RegularExpressions.Regex.IsMatch(Value, RegexARGB)) {
                string[] DecimalValues = Value.Replace(" ", "").Split(' ');
                if (DecimalValues.Length == 3
                && int.TryParse(DecimalValues[0], out int Red)
                && int.TryParse(DecimalValues[1], out int Green)
                && int.TryParse(DecimalValues[2], out int Blue)) {
                    return Color.FromArgb(
                        Math.Min(Red, 255),
                        Math.Min(Green, 255),
                        Math.Min(Blue, 255)
                    );
                }
                else if (DecimalValues.Length == 4
                && int.TryParse(DecimalValues[0], out int Alpha)
                && int.TryParse(DecimalValues[1], out Red)
                && int.TryParse(DecimalValues[2], out Green)
                && int.TryParse(DecimalValues[3], out Blue)) {
                    return Color.FromArgb(
                        Math.Min(Alpha, 255),
                        Math.Min(Red, 255),
                        Math.Min(Green, 255),
                        Math.Min(Blue, 255)
                    );
                }
                else return Color.FromArgb(255, 128, 128, 128);
            }
            else {
                switch (Value.Length) {
                    case 7: case 6: { // Can contain a pound sign, not enforced.
                        for (int i = 0; i < Value.Length; i++) {
                            switch (Value[i]) {
                                case '0': case '1': case '2': case '3':
                                case '4': case '5': case '6': case '7':
                                case '8': case '9': case 'a': case 'b':
                                case 'c': case 'd': case 'e': case 'f': {
                                    ColorIntBuffer += Value[i];
                                } break;

                                // We don't worry about the pound.
                                case '#': {
                                } continue;

                                // Invalid hexadecimal format.
                                default: {
                                } return Color.FromArgb(128, 128, 128);
                            }
                        }
                        return ColorIntBuffer.Length switch {
                            6 => Color.FromArgb(
                                    255,
                                    int.Parse($"{ColorIntBuffer[0]}{ColorIntBuffer[1]}", System.Globalization.NumberStyles.HexNumber),
                                    int.Parse($"{ColorIntBuffer[2]}{ColorIntBuffer[3]}", System.Globalization.NumberStyles.HexNumber),
                                    int.Parse($"{ColorIntBuffer[4]}{ColorIntBuffer[5]}", System.Globalization.NumberStyles.HexNumber)
                                ),
                            _ => Color.FromArgb(128, 128, 128)
                        };
                    }

                    default: {
                    } return Color.FromArgb(255, 128, 128, 128);
                }
            }
        }

        /// <summary>
        /// Writes a string to the ini file.
        /// </summary>
        /// <param name="Key">The string name of the key to write to.</param>
        /// <param name="Value">The string value to write to the key.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        public void Write(string Key, string Value, string Section = null) {
            WriteString(Key, Value, Section);
        }
        /// <summary>
        /// Writes a bool to the ini file.
        /// </summary>
        /// <param name="Key">The string name of the key to write to.</param>
        /// <param name="Value">The bool value to write to the key.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        public void Write(string Key, bool Value, string Section = null) {
            WriteString(Key, Value ? "True" : "False", Section);
        }
        /// <summary>
        /// Writes an int to the ini file.
        /// </summary>
        /// <param name="Key">The string name of the key to write to.</param>
        /// <param name="Value">The int value to write to the key.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        public void Write(string Key, int Value, string Section = null) {
            WriteString(Key, Value.ToString(), Section);
        }
        /// <summary>
        /// Writes a decimal to the ini file.
        /// </summary>
        /// <param name="Key">The string name of the key to write to.</param>
        /// <param name="Value">The string value to write to the key.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        public void Write(string Key, decimal Value, string Section = null) {
            WriteString(Key, Value.ToString(), Section);
        }
        /// <summary>
        /// Writes a Point to the ini file.
        /// </summary>
        /// <param name="Key">The string name of the key to write to.</param>
        /// <param name="Value">The Point value to write to the key.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        public void Write(string Key, Point Value, string Section = null) {
            WriteString(Key, $"{Value.X},{Value.Y}", Section);
        }
        /// <summary>
        /// Writes a Size to the ini file.
        /// </summary>
        /// <param name="Key">The string name of the key to write to.</param>
        /// <param name="Value">The Size value to write to the key.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        public void Write(string Key, Size Value, string Section = null) {
            WriteString(Key, $"{Value.Width},{Value.Height}", Section);
        }
        /// <summary>
        /// Writes a Color to the ini file.
        /// </summary>
        /// <param name="Key">The string name of the key to write to.</param>
        /// <param name="Value">The Color value to write to the key.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        public void Write(string Key, Color Value, string Section = null) {
            WriteString(Key, $"#{Value.ToArgb().ToString("X").Substring(2)}", Section);
        }

        /// <summary>
        /// Deletes a key from the ini file.
        /// </summary>
        /// <param name="Key">The string name of the key to write to.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        public void DeleteKey(string Key, string Section = null) {
            NativeMethods.WritePrivateProfileString(Section ?? ExecutableName, Key, null, IniPath);
        }
        /// <summary>
        /// Deletes a sectionfrom the ini file.
        /// </summary>
        /// <param name="Section">The string name of the section to delete. Default to ExecutableName.</param>
        public void DeleteSection(string Section = null) {
            NativeMethods.WritePrivateProfileString(Section ?? ExecutableName, null, null, IniPath);
        }

        /// <summary>
        /// Checks if a key exists in the ini file.
        /// </summary>
        /// <param name="Key">The string name of the key to write to.</param>
        /// <param name="Section">The string name of the section it's located at. Default to ExecutableName.</param>
        /// <returns>If the key exists in the ini file.</returns>
        public bool KeyExists(string Key, string Section = null) {
            return ReadShortString(Key, Section).Length > 0;
        }

        public bool KeyExists_FullCheck(string Key, string Section = null) {
            using StreamReader Ini = new(IniPath);

            string ReadLine;
            bool InSection = false;

            while ((ReadLine = Ini.ReadLine()) != null) {
                if (InSection) {
                    if (ReadLine.StartsWith("[")) {
                        break;
                    }
                    if (ReadLine.StartsWith(Key + "=")) {
                        return true;
                    }
                }
                else if (ReadLine.StartsWith($"[{Section ?? ExecutableName}]")) {
                    InSection = true;
                }
                else
                    continue;
            }

            return false;
        }

    }

#pragma warning restore IDE0075 // Simplify conditional expression
}