using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;

namespace aphrodite {
    public enum ConfigType : int {
        None = -1,

        All = 0,

        Initialization = 1,

        FormSettings = 2,

        General = 3,

        Tags = 4,

        Pools = 5,

        Images = 6
    }
    class Config {
        public static volatile Config Settings;

        public Config_Initialization Initialization;
        public Config_FormSettings FormSettings;
        public Config_General General;
        public Config_Tags Tags;
        public Config_Pools Pools;
        public Config_Images Images;

        public Config() {

            Program.Log(LogAction.WriteToLog, "Initializing Config");

            switch (Program.UseIni) {
                case true:
                    Program.Log(LogAction.WriteToLog, "UseIni is enabled, skipping internal settings");
                    break;

                case false:
                    Program.Log(LogAction.WriteToLog, "UseIni is disabled, using internal settings");
                    break;
            }

            Initialization = new Config_Initialization();

        }

        public void Load(ConfigType Type) {
            switch (Type) {
                case ConfigType.All:
                    FormSettings = new Config_FormSettings();
                    General = new Config_General();
                    Images = new Config_Images();
                    Pools = new Config_Pools();
                    Tags = new Config_Tags();
                    break;

                case ConfigType.FormSettings:
                    FormSettings = new Config_FormSettings();
                    break;

                case ConfigType.General:
                    General = new Config_General();
                    break;

                case ConfigType.Images:
                    Images = new Config_Images();
                    break;

                case ConfigType.Pools:
                    Pools = new Config_Pools();
                    break;

                case ConfigType.Tags:
                    Tags = new Config_Tags();
                    break;
            }
        }

        public void Save(ConfigType Type) {
            switch (Type) {
                case ConfigType.All:
                    FormSettings.Save();
                    General.Save();
                    Tags.Save();
                    Pools.Save();
                    Images.Save();
                    break;

                case ConfigType.Initialization:
                    Initialization.Save();
                    break;

                case ConfigType.FormSettings:
                    FormSettings.Save();
                    break;

                case ConfigType.General:
                    General.Save();
                    break;

                case ConfigType.Images:
                    Images.Save();
                    break;

                case ConfigType.Pools:
                    Pools.Save();
                    break;

                case ConfigType.Tags:
                    Tags.Save();
                    break;
            }
        }

        public class Config_Initialization {
            public Config_Initialization() {
                Program.Log(LogAction.WriteToLog, "Initializing Config_Initialization (ironic)");
                Load();
            }

            public bool firstTime = true;

            public void Save() {
                Program.Log(LogAction.WriteToLog, "Attempting to save Config_Initialization settings");

                switch (Program.UseIni) {
                    case true:
                        Program.Ini.WriteBool("firstTime", firstTime, "General");
                        break;

                    case false: {
                            bool Save = false;

                            switch (aphrodite.Settings.General.Default.firstTime != firstTime) {
                                case true:
                                    aphrodite.Settings.General.Default.firstTime = firstTime;
                                    Save = true;
                                    break;
                            }

                            switch (Save) {
                                case true:
                                    Program.Log(LogAction.WriteToLog, "Saving Initialization");
                                    aphrodite.Settings.General.Default.Save();
                                    break;
                            }
                            break;
                        }
                }
            }
            public void Load() {
                Program.Log(LogAction.WriteToLog, "Loading Config_Initialization settings");

                switch (Program.UseIni) {
                    case true:
                        switch (Program.Ini.KeyExists("firstTime", "General")) {
                            case true:
                                firstTime = Program.Ini.ReadBool("firstTime", "General");
                                break;

                            case false:
                                break;
                        }
                        break;

                    case false:
                        firstTime = aphrodite.Settings.General.Default.firstTime;
                        break;
                }
            }
        }

        public class Config_FormSettings {
            public Config_FormSettings(bool SkipLoad = false) {
                Program.Log(LogAction.WriteToLog, "Initializing Config_FormSettings");
                if (!SkipLoad) {
                    Load();
                }
            }

            #region Variables
            public Point frmMain_Location = new Point(-32000, -32000);
            //public decimal frmMain_numTagsPageLimit = 0;
            //public decimal frmMain_numTagsImageLimit = 0;
            //public bool frmMain_chkTagsUseMinimumScore = false;
            //public bool frmMain_chkTagsUseScoreAsTag = true;
            //public decimal frmMain_numTagsMinimumScore = 0;
            //public bool frmMain_chkTagsDownloadExplicit = true;
            //public bool frmMain_chkTagsDownloadQuestionable = true;
            //public bool frmMain_chkTagsDownloadSafe = true;
            //public bool frmMain_chkTagsSeparateRatings = true;
            //public bool frmMain_chkTagsOpenAfterDownload = false;
            //public bool frmMain_chkTagSeparateNonImages = true;
            //public bool frmMain_chkPoolOpenAfter = false;
            //public bool frmMain_chkPoolMergeBlacklisted = true;
            //public bool frmMain_chkImageSeparateRatings = true;
            //public bool frmMain_chkImageSeparateBlacklisted = true;
            //public bool frmMain_chkImageSeparateArtists = false;
            //public bool frmMain_chkImageSeparateNonImages = true;
            //public bool frmMain_chkImageUseForm = false;
            //public bool frmMain_chkImageOpenAfter = false;

            private Point frmMain_Location_First = new Point(-32000, -32000);
            //private decimal frmMain_numTagsPageLimit_First = 0;
            //private decimal frmMain_numTagsImageLimit_First = 0;
            //private bool frmMain_chkTagsUseMinimumScore_First = false;
            //private bool frmMain_chkTagsUseScoreAsTag_First = true;
            //private decimal frmMain_numTagsMinimumScore_First = 0;
            //private bool frmMain_chkTagsDownloadExplicit_First = true;
            //private bool frmMain_chkTagsDownloadQuestionable_First = true;
            //private bool frmMain_chkTagsDownloadSafe_First = true;
            //private bool frmMain_chkTagsSeparateRatings_First = true;
            //private bool frmMain_chkTagsOpenAfterDownload_First = false;
            //private bool frmMain_chkTagSeparateNonImages_First = true;
            //private bool frmMain_chkPoolOpenAfter_First = false;
            //private bool frmMain_chkPoolMergeBlacklisted_First = true;
            //private bool frmMain_chkImageSeparateRatings_First = true;
            //private bool frmMain_chkImageSeparateBlacklisted_First = true;
            //private bool frmMain_chkImageSeparateArtists_First = false;
            //private bool frmMain_chkImageSeparateNonImages_First = true;
            //private bool frmMain_chkImageUseForm_First = false;
            //private bool frmMain_chkImageOpenAfter_First = false;
            #endregion

            public void Save() {
                Program.Log(LogAction.WriteToLog, "Attempting to save Config_FormSettings settings");

                switch (Program.UseIni) {
                    case true:
                        switch (frmMain_Location != frmMain_Location_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "FormSettings -> frmMain_Location changed!");
                                Program.Ini.WritePoint("frmMain_Location", frmMain_Location, "FormSettings");
                                frmMain_Location_First = frmMain_Location;
                                break;
                        }
                        break;

                    case false: {
                            bool Save = false;

                            switch (aphrodite.Settings.FormSettings.Default.frmMain_Location != frmMain_Location) {
                                case true:
                                    Program.Log(LogAction.WriteToLog, "FormSettings -> frmMain_Location changed!");
                                    aphrodite.Settings.FormSettings.Default.frmMain_Location = frmMain_Location;
                                    Save = true;
                                    break;
                            }

                            switch (Save) {
                                case true:
                                    Program.Log(LogAction.WriteToLog, "Saving FormSettings");
                                    aphrodite.Settings.FormSettings.Default.Save();
                                    break;
                            }
                            break;
                        }
                }
            }
            public void Load() {
                Program.Log(LogAction.WriteToLog, "Loading Config_FormSettings settings");

                switch (Program.UseIni) {
                    case true:
                        if (Program.Ini.KeyExists("frmMain_Location", "FormSettings")) {
                            frmMain_Location = Program.Ini.ReadPoint("frmMain_Location", "FormSettings");
                            frmMain_Location_First = frmMain_Location;
                        }
                        break;

                    case false:
                        frmMain_Location = aphrodite.Settings.FormSettings.Default.frmMain_Location;
                        break;
                }
            }
        }

        public class Config_General {
            public Config_General(bool SkipLoad = false) {
                Program.Log(LogAction.WriteToLog, "Initializing Config_General");
                if (!SkipLoad) {
                    Load();
                }
            }

            #region Variables
            public string saveLocation = string.Empty;
            public string Graylist = string.Empty;
            public bool saveGraylisted = true;
            public bool saveInfo = true;
            public bool ignoreFinish = false;
            public string Blacklist = string.Empty;
            public string undesiredTags = string.Empty;
            public bool openAfter = false;

            private string saveLocation_First = string.Empty;
            private string Graylist_First = string.Empty;
            private bool saveGraylisted_First = true;
            private bool saveInfo_First = true;
            private bool ignoreFinish_First = false;
            private string Blacklist_First = string.Empty;
            private string undesiredTags_First = string.Empty;
            private bool openAfter_First = false;
            #endregion

            public void Save() {
                Program.Log(LogAction.WriteToLog, "Attempting to save Config_General settings");

                switch (Program.UseIni) {
                    case true:
                        switch (saveLocation != saveLocation_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> saveLocation changed!");
                                Program.Ini.WriteString("saveLocation", saveLocation, "General");
                                saveLocation_First = saveLocation;
                                break;
                        }
                        switch (Graylist != Graylist_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> Graylist changed!");
                                if (Graylist.Length > 0) {
                                    File.WriteAllText(
                                        Program.ApplicationPath + "\\graylist.cfg",
                                        Graylist.Replace(" ", "_").Replace("\r\n", " ")
                                    );
                                }
                                else {
                                    File.Delete(Program.ApplicationPath + "\\graylist.cfg");
                                }
                                break;
                        }
                        switch (saveGraylisted != saveGraylisted_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> saveGraylisted changed!");
                                Program.Ini.WriteBool("saveGraylisted", saveGraylisted, "General");
                                saveGraylisted_First = saveGraylisted;
                                break;
                        }
                        switch (saveInfo != saveInfo_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> saveInfo changed!");
                                Program.Ini.WriteBool("saveInfo", saveInfo, "General");
                                saveInfo_First = saveInfo;
                                break;
                        }
                        switch (ignoreFinish != ignoreFinish_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> ignoreFinish changed!");
                                Program.Ini.WriteBool("ignoreFinish", ignoreFinish, "General");
                                ignoreFinish_First = ignoreFinish;
                                break;
                        }
                        switch (Blacklist != Blacklist_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> Blacklist changed!");
                                if (Graylist.Length > 0) {
                                    File.WriteAllText(
                                        Program.ApplicationPath + "\\blacklist.cfg",
                                        Blacklist.Replace(" ", "_").Replace("\r\n", " ")
                                    );
                                }
                                else {
                                    File.Delete(Program.ApplicationPath + "\\blacklist.cfg");
                                }
                                break;
                        }
                        switch (undesiredTags != undesiredTags_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> undesiredTags changed!");
                                Program.Ini.WriteString("undesiredTags", undesiredTags, "General");
                                undesiredTags_First = undesiredTags;
                                break;
                        }
                        switch (openAfter != openAfter_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> openAfter changed!");
                                Program.Ini.WriteBool("openAfter", openAfter, "General");
                                openAfter_First = openAfter;
                                break;
                        }
                        break;

                    case false:
                        bool Save = false;

                        switch (aphrodite.Settings.General.Default.saveLocation != saveLocation) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> saveLocation changed!");
                                aphrodite.Settings.General.Default.saveLocation = saveLocation;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.General.Default.Graylist != Graylist) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> Graylist changed!");
                                aphrodite.Settings.General.Default.Graylist = Graylist;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.General.Default.saveGraylisted != saveGraylisted) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> saveGraylisted changed!");
                                aphrodite.Settings.General.Default.saveGraylisted = saveGraylisted;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.General.Default.saveInfo != saveInfo) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> saveInfo changed!");
                                aphrodite.Settings.General.Default.saveInfo = saveInfo;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.General.Default.ignoreFinish != ignoreFinish) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> ignoreFinish changed!");
                                aphrodite.Settings.General.Default.ignoreFinish = ignoreFinish;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.General.Default.Blacklist != Blacklist) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> Blacklist changed!");
                                aphrodite.Settings.General.Default.Blacklist = Blacklist;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.General.Default.undesiredTags != undesiredTags) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> undesiredTags changed!");
                                aphrodite.Settings.General.Default.undesiredTags = undesiredTags;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.General.Default.openAfter != openAfter) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> openAfter changed!");
                                aphrodite.Settings.General.Default.openAfter = openAfter;
                                Save = true;
                                break;
                        }

                        switch (Save) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Saving General");
                                aphrodite.Settings.General.Default.Save();
                                break;
                        }
                        break;
                }
            }
            public void Load() {
                Program.Log(LogAction.WriteToLog, "Loading Config_General settings");

                switch (Program.UseIni) {
                    case true:
                        switch (Program.Ini.KeyExists("saveLocation", "General")) {
                            case true:
                                saveLocation = Program.Ini.ReadString("saveLocation", "General");
                                saveLocation_First = saveLocation;
                                break;
                        }
                        switch (File.Exists(Program.ApplicationPath + "\\graylist.cfg")) {
                            case true:
                                Graylist = string.Join(" ", File.ReadAllLines(Program.ApplicationPath + "\\graylist.cfg"));
                                Graylist_First = Graylist;
                                break;
                        }
                        switch (Program.Ini.KeyExists("saveBlacklisted", "General")) {
                            case true:
                                saveGraylisted = Program.Ini.ReadBool("saveBlacklisted", "General");
                                saveGraylisted_First = saveGraylisted;
                                break;
                        }
                        switch (Program.Ini.KeyExists("saveInfo", "General")) {
                            case true:
                                saveInfo = Program.Ini.ReadBool("saveInfo", "General");
                                saveInfo_First = saveInfo;
                                break;
                        }
                        switch (Program.Ini.KeyExists("ignoreFinish", "General")) {
                            case true:
                                ignoreFinish = Program.Ini.ReadBool("ignoreFinish", "General");
                                ignoreFinish_First = ignoreFinish;
                                break;
                        }
                        switch (File.Exists(Program.ApplicationPath + "\\blacklist.cfg")) {
                            case true:
                                Blacklist = string.Join(" ", File.ReadAllLines(Program.ApplicationPath + "\\blacklist.cfg"));
                                Blacklist_First = Blacklist;
                                break;
                        }
                        switch (Program.Ini.KeyExists("undesiredTags", "General")) {
                            case true:
                                undesiredTags = Program.Ini.ReadString("undesiredTags", "General");
                                undesiredTags_First = undesiredTags;
                                break;
                        }
                        switch (Program.Ini.KeyExists("openAfter", "General")) {
                            case true:
                                openAfter = Program.Ini.ReadBool("openAfter", "General");
                                openAfter_First = openAfter;
                                break;
                        }
                        break;

                    case false:
                        saveLocation = aphrodite.Settings.General.Default.saveLocation;
                        Graylist = aphrodite.Settings.General.Default.Graylist;
                        saveGraylisted = aphrodite.Settings.General.Default.saveGraylisted;
                        saveInfo = aphrodite.Settings.General.Default.saveInfo;
                        ignoreFinish = aphrodite.Settings.General.Default.ignoreFinish;
                        Blacklist = aphrodite.Settings.General.Default.Blacklist;
                        undesiredTags = aphrodite.Settings.General.Default.undesiredTags;
                        openAfter = aphrodite.Settings.General.Default.openAfter;
                        break;
                }
            }
        }

        public class Config_Tags {
            public Config_Tags(bool SkipLoad = false) {
                Program.Log(LogAction.WriteToLog, "Initializing Config_Tags");
                if (!SkipLoad) {
                    Load();
                }
            }

            #region Variables
            public bool Safe = true;
            public bool Questionable = true;
            public bool Explicit = true;
            public bool separateRatings = true;
            public bool separateNonImages = true;
            public bool enableScoreMin = false;
            public bool scoreAsTag = true;
            public int scoreMin = 0;
            public int imageLimit = 0;
            public int pageLimit = 0;
            public string fileNameSchema = "%md5%";

            private bool Safe_First = true;
            private bool Questionable_First = true;
            private bool Explicit_First = true;
            private bool separateRatings_First = true;
            private bool separateNonImages_First = true;
            private bool enableScoreMin_First = false;
            private bool scoreAsTag_First = true;
            private int scoreMin_First = 0;
            private int imageLimit_First = 0;
            private int pageLimit_First = 0;
            private string fileNameSchema_First = "%md5%";
            #endregion

            public void Save() {
                Program.Log(LogAction.WriteToLog, "Attempting to save Config_Tags settings");

                switch (Program.UseIni) {
                    case true:
                        switch (Safe != Safe_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> Safe changed!");
                                Program.Ini.WriteBool("Safe", Safe, "Tags");
                                Safe_First = Safe;
                                break;
                        }
                        switch (Questionable != Questionable_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> Questionable changed!");
                                Program.Ini.WriteBool("Questionable", Questionable, "Tags");
                                Questionable_First = Questionable;
                                break;
                        }
                        switch (Explicit != Explicit_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> Explicit changed!");
                                Program.Ini.WriteBool("Explicit", Explicit, "Tags");
                                Explicit_First = Explicit;
                                break;
                        }
                        switch (separateRatings != separateRatings_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> separateRatings changed!");
                                Program.Ini.WriteBool("separateRatings", separateRatings, "Tags");
                                separateRatings_First = separateRatings;
                                break;
                        }
                        switch (separateNonImages != separateNonImages_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> separateNonImages changed!");
                                Program.Ini.WriteBool("separateNonImages", separateNonImages, "Tags");
                                separateNonImages_First = separateNonImages;
                                break;
                        }
                        switch (enableScoreMin != enableScoreMin_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> enableScoreMin changed!");
                                Program.Ini.WriteBool("enableScoreMin", enableScoreMin, "Tags");
                                enableScoreMin_First = enableScoreMin;
                                break;
                        }
                        switch (scoreAsTag != scoreAsTag_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> scoreAsTag changed!");
                                Program.Ini.WriteBool("scoreAsTag", scoreAsTag, "Tags");
                                scoreAsTag_First = scoreAsTag;
                                break;
                        }
                        switch (scoreMin != scoreMin_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> scoreMin changed!");
                                Program.Ini.WriteInt("scoreMin", scoreMin, "Tags");
                                scoreMin_First = scoreMin;
                                break;
                        }
                        switch (imageLimit != imageLimit_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> imageLimit changed!");
                                Program.Ini.WriteInt("imageLimit", imageLimit, "Tags");
                                imageLimit_First = imageLimit;
                                break;
                        }
                        switch (pageLimit != pageLimit_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> pageLimit changed!");
                                Program.Ini.WriteInt("pageLimit", pageLimit, "Tags");
                                pageLimit_First = pageLimit;
                                break;
                        }
                        switch (fileNameSchema != fileNameSchema_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> fileNameSchema changed!");
                                Program.Ini.WriteString("fileNameSchema", fileNameSchema, "Tags");
                                fileNameSchema_First = fileNameSchema;
                                break;
                        }
                        break;

                    case false:
                        bool Save = false;

                        switch (aphrodite.Settings.Tags.Default.Safe != Safe) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> Safe changed!");
                                aphrodite.Settings.Tags.Default.Safe = Safe;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.Questionable != Questionable) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> Questionable changed!");
                                aphrodite.Settings.Tags.Default.Questionable = Questionable;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.Explicit != Explicit) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> Explicit changed!");
                                aphrodite.Settings.Tags.Default.Explicit = Explicit;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.separateRatings != separateRatings) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> separateRatings changed!");
                                aphrodite.Settings.Tags.Default.separateRatings = separateRatings;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.separateNonImages != separateNonImages) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> separateNonImages changed!");
                                aphrodite.Settings.Tags.Default.separateNonImages = separateNonImages;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.enableScoreMin != enableScoreMin) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> enableScoreMin changed!");
                                aphrodite.Settings.Tags.Default.enableScoreMin = enableScoreMin;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.scoreAsTag != scoreAsTag) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> scoreAsTag changed!");
                                aphrodite.Settings.Tags.Default.scoreAsTag = scoreAsTag;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.scoreMin != scoreMin) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> scoreMin changed!");
                                aphrodite.Settings.Tags.Default.scoreMin = scoreMin;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.imageLimit != imageLimit) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> imageLimit changed!");
                                aphrodite.Settings.Tags.Default.imageLimit = imageLimit;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.pageLimit != pageLimit) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> pageLimit changed!");
                                aphrodite.Settings.Tags.Default.pageLimit = pageLimit;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.fileNameSchema != fileNameSchema) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> fileNameSchema changed!");
                                aphrodite.Settings.Tags.Default.fileNameSchema = fileNameSchema;
                                Save = true;
                                break;
                        }

                        switch (Save) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Saving Tags");
                                aphrodite.Settings.Tags.Default.Save();
                                break;
                        }
                        break;
                }
            }
            public void Load() {
                Program.Log(LogAction.WriteToLog, "Loading Config_Tags settings");

                switch (Program.UseIni) {
                    case true:
                        if (Program.Ini.KeyExists("Safe", "Tags")) {
                            Safe = Program.Ini.ReadBool("Safe", "Tags");
                            Safe_First = Safe;
                        }
                        if (Program.Ini.KeyExists("Questionable", "Tags")) {
                            Questionable = Program.Ini.ReadBool("Questionable", "Tags");
                            Questionable_First = Questionable;
                        }
                        if (Program.Ini.KeyExists("Explicit", "Tags")) {
                            Explicit = Program.Ini.ReadBool("Explicit", "Tags");
                            Explicit_First = Explicit;
                        }
                        if (Program.Ini.KeyExists("separateRatings", "Tags")) {
                            separateRatings = Program.Ini.ReadBool("separateRatings", "Tags");
                            separateRatings_First = separateRatings;
                        }
                        if (Program.Ini.KeyExists("separateNonImages", "Tags")) {
                            separateNonImages = Program.Ini.ReadBool("separateNonImages", "Tags");
                            separateNonImages_First = separateNonImages;
                        }
                        if (Program.Ini.KeyExists("enableScoreMin", "Tags")) {
                            enableScoreMin = Program.Ini.ReadBool("enableScoreMin", "Tags");
                            enableScoreMin_First = enableScoreMin;
                        }
                        if (Program.Ini.KeyExists("scoreAsTag", "Tags")) {
                            scoreAsTag = Program.Ini.ReadBool("scoreAsTag", "Tags");
                            scoreAsTag_First = scoreAsTag;
                        }
                        if (Program.Ini.KeyExists("scoreMin", "Tags")) {
                            scoreMin = Program.Ini.ReadInt("scoreMin", "Tags");
                            scoreMin_First = scoreMin;
                        }
                        if (Program.Ini.KeyExists("imageLimit", "Tags")) {
                            imageLimit = Program.Ini.ReadInt("imageLimit", "Tags");
                            imageLimit_First = imageLimit;
                        }
                        if (Program.Ini.KeyExists("pageLimit", "Tags")) {
                            pageLimit = Program.Ini.ReadInt("pageLimit", "Tags");
                            pageLimit_First = pageLimit;
                        }
                        if (Program.Ini.KeyExists("fileNameSchema", "Tags")) {
                            fileNameSchema = Program.Ini.ReadString("fileNameSchema", "Tags");
                            fileNameSchema_First = fileNameSchema;
                        }
                        break;

                    case false:
                        Safe = aphrodite.Settings.Tags.Default.Safe;
                        Questionable = aphrodite.Settings.Tags.Default.Questionable;
                        Explicit = aphrodite.Settings.Tags.Default.Explicit;
                        separateRatings = aphrodite.Settings.Tags.Default.separateRatings;
                        separateNonImages = aphrodite.Settings.Tags.Default.separateNonImages;
                        enableScoreMin = aphrodite.Settings.Tags.Default.enableScoreMin;
                        scoreAsTag = aphrodite.Settings.Tags.Default.scoreAsTag;
                        scoreMin = aphrodite.Settings.Tags.Default.scoreMin;
                        imageLimit = aphrodite.Settings.Tags.Default.imageLimit;
                        pageLimit = aphrodite.Settings.Tags.Default.pageLimit;
                        fileNameSchema = aphrodite.Settings.Tags.Default.fileNameSchema;
                        break;
                }
            }
        }

        public class Config_Pools {
            public static readonly string WishlistFile = Program.ApplicationPath + "\\PoolWishlist.cfg";

            public Config_Pools() {
                Program.Log(LogAction.WriteToLog, "Initializing Config_Pools");
                Load();
            }

            public static void AppendToWishlist(string Name, string URL) {
                Program.Log(LogAction.WriteToLog, "Appending new Wishlist pool");

                if (Program.UseIni) {
                    if (Program.Ini.KeyExists("addWishlistSilent", "Pools") && Program.Ini.ReadBool("addWishlistSilent", "Pools")) {
                        if (File.Exists(WishlistFile)) {
                            File.AppendAllText(WishlistFile, Environment.NewLine + URL + "|" + Name);
                        }
                        else {
                            File.Create(WishlistFile).Dispose();
                            File.AppendAllText(WishlistFile, URL + "|" + Name);
                        }
                    }
                    else {
                        frmPoolWishlist WishList = new frmPoolWishlist(true, URL, Name);
                        WishList.ShowDialog();
                        return;
                    }
                }
                else {
                    if (!Config.Settings.Pools.wishlist.Contains(URL)) {
                        if (Config.Settings.Pools.addWishlistSilent) {
                            if (!string.IsNullOrWhiteSpace(aphrodite.Settings.Pools.Default.wishlistNames) && !string.IsNullOrWhiteSpace(Config.Settings.Pools.wishlist)) {
                                Config.Settings.Pools.wishlist += "|" + URL;
                                Config.Settings.Pools.wishlistNames += "|" + Name;
                            }
                            else {
                                Config.Settings.Pools.wishlist = URL;
                                Config.Settings.Pools.wishlistNames = Name;
                            }
                            Config.Settings.Save(ConfigType.Pools);
                        }
                        else {
                            frmPoolWishlist WishList = new frmPoolWishlist(true, URL, Name);
                            WishList.ShowDialog();
                        }
                    }
                }
            }
            public static void SaveWishlist(List<string> Names, List<string> URLs) {
                Program.Log(LogAction.WriteToLog, "Saving pool wishlist");

                switch (URLs.Count > 0 && Names.Count > 0 && URLs.Count == Names.Count) {
                    case true:
                        switch (Program.UseIni) {
                            case true:
                                string FileOutput = string.Empty;
                                for (int i = 0; i < URLs.Count; i++) {
                                    FileOutput += URLs[i] + "|" + Names[i] + "\r\n";
                                }
                                FileOutput = FileOutput.Trim('\n').Trim('\r');
                                File.WriteAllText(WishlistFile, FileOutput);
                                break;
                        }

                        Settings.Pools.wishlist = string.Join("|", URLs);
                        Settings.Pools.wishlistNames = string.Join("|", Names);
                        Settings.Save(ConfigType.Pools);
                        break;
                }
            }

            #region Variables
            public bool mergeGraylisted = true;
            public string wishlist = string.Empty;
            public string wishlistNames = string.Empty;
            public bool addWishlistSilent = false;
            public string fileNameSchema = "%poolname%_%page%";
            public bool downloadBlacklisted = true;
            public bool mergeBlacklisted = false;

            private bool mergeGraylisted_First = true;
            public string wishlist_First = string.Empty;
            public string wishlistNames_First = string.Empty;
            private bool addWishlistSilent_First = false;
            private string fileNameSchema_First = "%poolname%_%page%";
            private bool downloadBlacklisted_First = true;
            private bool mergeBlacklisted_First = false;
            #endregion

            public void Save() {
                Program.Log(LogAction.WriteToLog, "Attempting to save Config_Pools settings");

                switch (Program.UseIni) {
                    case true:
                        switch (mergeGraylisted != mergeGraylisted_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Pools -> mergeGraylisted changed!");
                                Program.Ini.WriteBool("mergeBlacklisted", mergeGraylisted, "Pools");
                                mergeGraylisted_First = mergeGraylisted;
                                break;
                        }
                        switch (addWishlistSilent != addWishlistSilent_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Pools -> addWishlistSilent changed!");
                                Program.Ini.WriteBool("addWishlistSilent", addWishlistSilent, "Pools");
                                addWishlistSilent_First = addWishlistSilent;
                                break;
                        }
                        switch (fileNameSchema != fileNameSchema_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Pools -> fileNameSchema changed!");
                                Program.Ini.WriteString("fileNameSchema", fileNameSchema, "Pools");
                                fileNameSchema_First = fileNameSchema;
                                break;
                        }
                        switch (downloadBlacklisted != downloadBlacklisted_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Pools -> downloadBlacklisted changed!");
                                Program.Ini.WriteBool("downloadBlacklisted", downloadBlacklisted, "Pools");
                                downloadBlacklisted_First = downloadBlacklisted;
                                break;
                        }
                        switch (mergeBlacklisted != mergeBlacklisted_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Pools -> mergeBlacklisted changed!");
                                Program.Ini.WriteBool("mergeBlacklisted", mergeBlacklisted, "Pools");
                                mergeBlacklisted_First = mergeBlacklisted;
                                break;
                        }
                        break;

                    case false:
                        bool Save = false;

                        switch (aphrodite.Settings.Pools.Default.mergeGraylisted != mergeGraylisted) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Pools -> mergeBlacklisted changed!");
                                aphrodite.Settings.Pools.Default.mergeGraylisted = mergeGraylisted;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Pools.Default.addWishlistSilent != addWishlistSilent) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Pools -> addWishlistSilent changed!");
                                aphrodite.Settings.Pools.Default.addWishlistSilent = addWishlistSilent;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Pools.Default.fileNameSchema != fileNameSchema) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Pools -> fileNameSchema changed!");
                                aphrodite.Settings.Pools.Default.fileNameSchema = fileNameSchema;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Pools.Default.downloadBlacklisted != downloadBlacklisted) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Pools -> downloadBlacklisted changed!");
                                aphrodite.Settings.Pools.Default.downloadBlacklisted = downloadBlacklisted;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Pools.Default.mergeBlacklisted != mergeBlacklisted) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Pools -> mergeBlacklisted changed!");
                                aphrodite.Settings.Pools.Default.mergeBlacklisted = mergeBlacklisted;
                                Save = true;
                                break;
                        }

                        switch (Save) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Saving Pools");
                                aphrodite.Settings.Pools.Default.Save();
                                break;
                        }
                        break;
                }
            }
            public void Load() {
                Program.Log(LogAction.WriteToLog, "Loading Config_Pools settings");

                switch (Program.UseIni) {
                    case true:
                        switch (Program.Ini.KeyExists("mergeBlacklisted", "Pools")) {
                            case true:
                                mergeGraylisted = Program.Ini.ReadBool("mergeBlacklisted", "Pools");
                                mergeGraylisted_First = mergeGraylisted;
                                break;
                        }
                        switch (Program.Ini.KeyExists("addWishlistSilent", "Pools")) {
                            case true:
                                addWishlistSilent = Program.Ini.ReadBool("addWishlistSilent", "Pools");
                                addWishlistSilent_First = addWishlistSilent;
                                break;
                        }
                        switch (Program.Ini.KeyExists("fileNameSchema", "Pools")) {
                            case true:
                                fileNameSchema = Program.Ini.ReadString("fileNameSchema", "Pools");
                                fileNameSchema_First = fileNameSchema;
                                break;
                        }
                        switch (Program.Ini.KeyExists("downloadBlacklisted", "Pools")) {
                            case true:
                                downloadBlacklisted = Program.Ini.ReadBool("downloadBlacklisted", "Pools");
                                downloadBlacklisted_First = downloadBlacklisted;
                                break;
                        }
                        switch (Program.Ini.KeyExists("mergeBlacklisted", "Pools")) {
                            case true:
                                mergeBlacklisted = Program.Ini.ReadBool("mergeBlacklisted", "Pools");
                                mergeBlacklisted_First = mergeBlacklisted;
                                break;
                        }

                        switch (File.Exists(WishlistFile)) {
                            case true:
                                string[] List = File.ReadAllLines(WishlistFile);
                                if (List.Length > 0) {
                                    for (int i = 0; i < List.Length; i++) {
                                        wishlistNames += List[i].Split('|')[0] + "|";
                                        wishlist += List[i].Split('|')[1] + "|";
                                    }

                                    wishlistNames = wishlistNames.Trim('|');
                                    wishlist = wishlist.Trim('|');
                                    wishlistNames_First = wishlistNames;
                                    wishlist_First = wishlist;
                                }
                                break;
                        }
                        break;

                    case false:
                        mergeGraylisted = aphrodite.Settings.Pools.Default.mergeGraylisted;
                        wishlist = aphrodite.Settings.Pools.Default.wishlist;
                        wishlistNames = aphrodite.Settings.Pools.Default.wishlistNames;
                        addWishlistSilent = aphrodite.Settings.Pools.Default.addWishlistSilent;
                        fileNameSchema = aphrodite.Settings.Pools.Default.fileNameSchema;
                        downloadBlacklisted = aphrodite.Settings.Pools.Default.downloadBlacklisted;
                        mergeBlacklisted = aphrodite.Settings.Pools.Default.mergeBlacklisted;
                        break;
                }
            }
        }

        public class Config_Images {
            public Config_Images() {
                Program.Log(LogAction.WriteToLog, "Initializing Config_Images");
                Load();
            }

            #region Variables
            public bool separateRatings = true;
            public bool separateGraylisted = true;
            public bool useForm = false;
            public bool separateArtists = false;
            public string fileNameSchema = "%artist%_%md5%";
            public bool separateNonImages = true;
            public bool separateBlacklisted = true;

            private bool separateRatings_First = true;
            private bool separateGraylisted_First = true;
            private bool useForm_First = false;
            private bool separateArtists_First = false;
            private string fileNameSchema_First = "%artist%_%md5%";
            private bool separateNonImages_First = true;
            private bool separateBlacklisted_First = true;
            #endregion

            public void Save() {
                Program.Log(LogAction.WriteToLog, "Attempting to save Config_Images settings");

                switch (Program.UseIni) {
                    case true:
                        switch (separateRatings != separateRatings_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> separateRatings changed!");
                                Program.Ini.WriteBool("separateRatings", separateRatings, "Images");
                                separateRatings_First = separateRatings;
                                break;
                        }
                        switch (separateGraylisted != separateGraylisted_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> separateBlacklisted changed!");
                                Program.Ini.WriteBool("separateBlacklisted", separateGraylisted, "Images");
                                separateGraylisted_First = separateGraylisted;
                                break;
                        }
                        switch (useForm != useForm_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> useForm changed!");
                                Program.Ini.WriteBool("useForm", useForm, "Images");
                                useForm_First = useForm;
                                break;
                        }
                        switch (separateArtists != separateArtists_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> separateArtists changed!");
                                Program.Ini.WriteBool("separateArtists", separateArtists, "Images");
                                separateArtists_First = separateArtists;
                                break;
                        }
                        switch (fileNameSchema != fileNameSchema_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> fileNameSchema changed!");
                                Program.Ini.WriteString("fileNameSchema", fileNameSchema, "Images");
                                fileNameSchema_First = fileNameSchema;
                                break;
                        }
                        switch (separateNonImages != separateNonImages_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> separateNonImages changed!");
                                Program.Ini.WriteBool("separateNonImages", separateNonImages, "Images");
                                separateNonImages_First = separateNonImages;
                                break;
                        }
                        switch (separateBlacklisted != separateBlacklisted_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> separateBlacklisted changed!");
                                Program.Ini.WriteBool("separateBlacklisted", separateBlacklisted, "Images");
                                separateBlacklisted_First = separateBlacklisted;
                                break;
                        }
                        break;

                    case false:
                        bool Save = false;

                        switch (aphrodite.Settings.Images.Default.separateRatings != separateRatings) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> separateRatings changed!");
                                aphrodite.Settings.Images.Default.separateRatings = separateRatings;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Images.Default.separateGraylisted != separateGraylisted) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> separateBlacklisted changed!");
                                aphrodite.Settings.Images.Default.separateGraylisted = separateGraylisted;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Images.Default.useForm != useForm) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> useForm changed!");
                                aphrodite.Settings.Images.Default.useForm = useForm;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Images.Default.separateArtists != separateArtists) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> separateArtists changed!");
                                aphrodite.Settings.Images.Default.separateArtists = separateArtists;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Images.Default.fileNameSchema != fileNameSchema) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> fileNameSchema changed!");
                                aphrodite.Settings.Images.Default.fileNameSchema = fileNameSchema;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Images.Default.separateNonImages != separateNonImages) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> separateNonImages changed!");
                                aphrodite.Settings.Images.Default.separateNonImages = separateNonImages;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Images.Default.separateBlacklisted != separateBlacklisted) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> separateBlacklisted changed!");
                                aphrodite.Settings.Images.Default.separateBlacklisted = separateBlacklisted;
                                Save = true;
                                break;
                        }

                        switch (Save) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Saving Images");
                                aphrodite.Settings.Images.Default.Save();
                                break;
                        }
                        break;
                }
            }
            public void Load() {
                Program.Log(LogAction.WriteToLog, "Loading Config_Images settings");

                switch (Program.UseIni) {
                    case true:
                        switch (Program.Ini.KeyExists("separateRatings", "Images")) {
                            case true:
                                separateRatings = Program.Ini.ReadBool("separateRatings", "Images");
                                separateRatings_First = separateRatings;
                                break;
                        }
                        switch (Program.Ini.KeyExists("separateBlacklisted", "Images")) {
                            case true:
                                separateGraylisted = Program.Ini.ReadBool("separateBlacklisted", "Images");
                                separateGraylisted_First = separateGraylisted;
                                break;
                        }
                        switch (Program.Ini.KeyExists("useForm", "Images")) {
                            case true:
                                useForm = Program.Ini.ReadBool("useForm", "Images");
                                useForm_First = useForm;
                                break;
                        }
                        switch (Program.Ini.KeyExists("separateArtists", "Images")) {
                            case true:
                                separateArtists = Program.Ini.ReadBool("separateArtists", "Images");
                                separateArtists_First = separateArtists;
                                break;
                        }
                        switch (Program.Ini.KeyExists("fileNameSchema", "Images")) {
                            case true:
                                fileNameSchema = Program.Ini.ReadString("fileNameSchema", "Images");
                                fileNameSchema_First = fileNameSchema;
                                break;
                        }
                        switch (Program.Ini.KeyExists("separateNonImages", "Images")) {
                            case true:
                                separateNonImages = Program.Ini.ReadBool("separateNonImages", "Images");
                                separateNonImages_First = separateNonImages;
                                break;
                        }
                        switch (Program.Ini.KeyExists("separateBlacklisted", "Images")) {
                            case true:
                                separateBlacklisted = Program.Ini.ReadBool("separateBlacklisted", "Images");
                                separateBlacklisted_First = separateBlacklisted;
                                break;
                        }
                        break;

                    case false:
                        separateRatings = aphrodite.Settings.Images.Default.separateRatings;
                        separateGraylisted = aphrodite.Settings.Images.Default.separateGraylisted;
                        useForm = aphrodite.Settings.Images.Default.useForm;
                        separateArtists = aphrodite.Settings.Images.Default.separateArtists;
                        fileNameSchema = aphrodite.Settings.Images.Default.fileNameSchema;
                        separateNonImages = aphrodite.Settings.Images.Default.separateNonImages;
                        separateBlacklisted = aphrodite.Settings.Images.Default.separateBlacklisted;
                        break;
                }
            }
        }
    }

    class IniFile {
        string Path;
        string EXE = Assembly.GetExecutingAssembly().GetName().Name;

        public IniFile(string IniPath = null) {
            Path = new FileInfo(IniPath ?? EXE + ".ini").FullName.ToString();
        }

        public string ReadString(string Key, string Section = null) {
            var RetVal = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(Section ?? EXE, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }
        public bool ReadBool(string Key, string Section = null) {
            var RetVal = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(Section ?? EXE, Key.ToLower(), "", RetVal, 255, Path);
            switch (RetVal.ToString().ToLower()) {
                case "true":
                    return true;
                default:
                    return false;
            }
        }
        public int ReadInt(string Key, string Section = null) {
            var RetVal = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(Section ?? EXE, Key.ToLower(), "", RetVal, 255, Path);
            string RetStr = RetVal.ToString();
            int RetInt;
            if (int.TryParse(RetStr, out RetInt)) {
                return RetInt;
            }
            return 0;
        }
        public Point ReadPoint(string Key, string Section = null) {
            var RetVal = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            string[] Value = RetVal.ToString().Split(',');
            if (Value.Length == 2) {
                int Temp;
                Point OutputPoint = new Point();
                if (int.TryParse(Value[0], out Temp)) {
                    OutputPoint.X = Temp;
                }
                else {
                    OutputPoint.X = 0;
                }
                if (int.TryParse(Value[1], out Temp)) {
                    OutputPoint.Y = Temp;
                }
                else {
                    OutputPoint.Y = 0;
                }
                return OutputPoint;
            }
            else {
                return new Point(0, 0);
            }
        }
        public Size ReadSize(string Key, string Section = null) {
            var RetVal = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            string[] Value = RetVal.ToString().Split(',');
            if (Value.Length == 2) {
                int Temp;
                Size OutputPoint = new Size();
                if (int.TryParse(Value[0], out Temp)) {
                    OutputPoint.Width = Temp;
                }
                else {
                    OutputPoint.Width = 0;
                }
                if (int.TryParse(Value[1], out Temp)) {
                    OutputPoint.Height = Temp;
                }
                else {
                    OutputPoint.Height = 0;
                }
                return OutputPoint;
            }
            else {
                return new Size(0, 0);
            }
        }

        public void WriteString(string Key, string Value, string Section = null) {
            NativeMethods.WritePrivateProfileString(Section ?? EXE, Key, Value, Path);
        }
        public void WriteBool(string Key, bool Value, string Section = null) {
            switch (Value) {
                case true:
                    NativeMethods.WritePrivateProfileString(Section ?? EXE, Key, "True", Path);
                    break;

                default:
                    NativeMethods.WritePrivateProfileString(Section ?? EXE, Key, "False", Path);
                    break;
            }
        }
        public void WriteInt(string Key, int Value, string Section = null) {
            NativeMethods.WritePrivateProfileString(Section ?? EXE, Key, Value.ToString(), Path);
        }
        public void WritePoint(string Key, Point Value, string Section = null) {
            NativeMethods.WritePrivateProfileString(Section, Key, Value.X + "," + Value.Y, Path);
        }
        public void WriteSize(string Key, Size Value, string Section = null) {
            NativeMethods.WritePrivateProfileString(Section, Key, Value.Width + "," + Value.Height, Path);
        }

        public void DeleteKey(string Key, string Section = null) {
            WriteString(Key, null, Section ?? EXE);
        }
        public void DeleteSection(string Section = null) {
            WriteString(null, null, Section ?? EXE);
        }

        public bool KeyExists(string Key, string Section = null) {
            return ReadString(Key, Section).Length > 0;
        }
    }

}
