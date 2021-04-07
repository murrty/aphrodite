using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace aphrodite {

    class Config {
        public static volatile Config Settings;

        public Config_FormSettings FormSettings;
        public Config_General General;
        public Config_Tags Tags;
        public Config_Pools Pools;
        public Config_Images Images;

        public Config() {

            if (Program.UseIni) {
                Debug.Print("Using ini file at " + Program.ApplicationPath + "\\aphrodite.ini");
            }

            FormSettings = new Config_FormSettings();

            General = new Config_General();

            Tags = new Config_Tags();

            Pools = new Config_Pools();

            Images = new Config_Images();

        }

        public void Save() {
            FormSettings.Save();
            General.Save();
            Tags.Save();
            Pools.Save();
            Images.Save();
        }

        public class Config_FormSettings {
            public Config_FormSettings(bool SkipLoad = false) {
                if (!SkipLoad) {
                    Load();
                }
            }

            public Point frmMainLocation = new Point(-32000, -32000);
            private Point frmMainLocation_First = new Point(-32000, -32000);

            public void Save() {
                switch (Program.UseIni) {
                    case true:
                        switch (frmMainLocation != frmMainLocation_First) {
                            case true:
                                Program.Ini.WritePoint("frmMainLocation", frmMainLocation, "FormSettings");
                                frmMainLocation_First = frmMainLocation;
                                break;
                        }
                        break;

                    case false: {
                            bool Save = false;

                            switch (aphrodite.Settings.FormSettings.Default.frmMainLocation != frmMainLocation) {
                                case true:
                                    aphrodite.Settings.FormSettings.Default.frmMainLocation = frmMainLocation;
                                    Save = true;
                                    break;
                            }

                            switch (Save) {
                                case true:
                                    aphrodite.Settings.FormSettings.Default.Save();
                                    break;
                            }
                            break;
                        }
                }
            }
            public void Load() {
                switch (Program.UseIni) {
                    case true:
                        if (Program.Ini.KeyExists("frmMainLocation", "FormSettings")) {
                            frmMainLocation = Program.Ini.ReadPoint("frmMainLocation", "FormSettings");
                            frmMainLocation_First = frmMainLocation;
                        }
                        break;

                    case false:
                        frmMainLocation = aphrodite.Settings.FormSettings.Default.frmMainLocation;
                        break;
                }
            }
        }

        public class Config_General {
            public Config_General(bool SkipLoad = false) {
                if (!SkipLoad) {
                    Load();
                }
            }

            #region Variables
            public string saveLocation = string.Empty;
            public string blacklist = string.Empty;
            public bool saveBlacklisted = true;
            public bool saveInfo = true;
            public bool ignoreFinish = false;
            public string zeroToleranceBlacklist = string.Empty;
            public bool firstTime = true;
            public string undesiredTags = string.Empty;
            public bool openAfter = false;

            private string saveLocation_First = string.Empty;
            private string blacklist_First = string.Empty;
            private bool saveBlacklisted_First = true;
            private bool saveInfo_First = true;
            private bool ignoreFinish_First = false;
            private string zeroToleranceBlacklist_First = string.Empty;
            private bool firstTime_First = true;
            private string undesiredTags_First = string.Empty;
            private bool openAfter_First = false;
            #endregion

            public void Save() {
                switch (Program.UseIni) {
                    case true:
                        switch (saveLocation != saveLocation_First) {
                            case true:
                                Program.Ini.WriteString("saveLocation", saveLocation, "General");
                                saveLocation_First = saveLocation;
                                break;
                        }
                        switch (blacklist != blacklist_First) {
                            case true:
                                if (blacklist.Length > 0) {
                                    File.WriteAllText(Program.ApplicationPath + "\\graylist.cfg", blacklist.Replace(" ", "_").Replace("\r\n", " "));
                                }
                                else {
                                    File.Delete(Program.ApplicationPath + "\\graylist.cfg");
                                }
                                break;
                        }
                        switch (saveBlacklisted != saveBlacklisted_First) {
                            case true:
                                Program.Ini.WriteBool("saveBlacklisted", saveBlacklisted, "General");
                                saveBlacklisted_First = saveBlacklisted;
                                break;
                        }
                        switch (saveInfo != saveInfo_First) {
                            case true:
                                Program.Ini.WriteBool("saveInfo", saveInfo, "General");
                                saveInfo_First = saveInfo;
                                break;
                        }
                        switch (ignoreFinish != ignoreFinish_First) {
                            case true:
                                Program.Ini.WriteBool("ignoreFinish", ignoreFinish, "General");
                                ignoreFinish_First = ignoreFinish;
                                break;
                        }
                        switch (zeroToleranceBlacklist != zeroToleranceBlacklist_First) {
                            case true:
                                if (blacklist.Length > 0) {
                                    File.WriteAllText(Program.ApplicationPath + "\\blacklist.cfg", zeroToleranceBlacklist.Replace(" ", "_").Replace("\r\n", " "));
                                }
                                else {
                                    File.Delete(Program.ApplicationPath + "\\blacklist.cfg");
                                }
                                break;
                        }
                        switch (firstTime != firstTime_First) {
                            case true:
                                Program.Ini.WriteBool("firstTime", firstTime, "General");
                                firstTime_First = firstTime;
                                break;
                        }
                        switch (undesiredTags != undesiredTags_First) {
                            case true:
                                Program.Ini.WriteString("undesiredTags", undesiredTags, "General");
                                undesiredTags_First = undesiredTags;
                                break;
                        }
                        switch (openAfter != openAfter_First) {
                            case true:
                                Program.Ini.WriteBool("openAfter", openAfter, "General");
                                openAfter_First = openAfter;
                                break;
                        }
                        break;

                    case false:
                        bool Save = false;

                        switch (aphrodite.Settings.General.Default.saveLocation != saveLocation_First) {
                            case true:
                                aphrodite.Settings.General.Default.saveLocation = saveLocation;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.General.Default.blacklist != blacklist) {
                            case true:
                                aphrodite.Settings.General.Default.blacklist = blacklist;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.General.Default.saveBlacklisted != saveBlacklisted) {
                            case true:
                                aphrodite.Settings.General.Default.saveBlacklisted = saveBlacklisted;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.General.Default.saveInfo != saveInfo) {
                            case true:
                                aphrodite.Settings.General.Default.saveInfo = saveInfo;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.General.Default.ignoreFinish != ignoreFinish) {
                            case true:
                                aphrodite.Settings.General.Default.ignoreFinish = ignoreFinish;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.General.Default.zeroToleranceBlacklist != zeroToleranceBlacklist) {
                            case true:
                                aphrodite.Settings.General.Default.zeroToleranceBlacklist = zeroToleranceBlacklist;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.General.Default.firstTime != firstTime) {
                            case true:
                                aphrodite.Settings.General.Default.firstTime = firstTime;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.General.Default.undesiredTags != undesiredTags) {
                            case true:
                                aphrodite.Settings.General.Default.undesiredTags = undesiredTags;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.General.Default.openAfter != openAfter) {
                            case true:
                                aphrodite.Settings.General.Default.openAfter = openAfter;
                                Save = true;
                                break;
                        }

                        switch (Save) {
                            case true:
                                aphrodite.Settings.General.Default.Save();
                                break;
                        }
                        break;
                }
            }
            public void Load() {
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
                                blacklist = string.Join(" ", File.ReadAllLines(Program.ApplicationPath + "\\graylist.cfg"));
                                blacklist_First = blacklist;
                                break;
                        }
                        switch (Program.Ini.KeyExists("saveBlacklisted", "General")) {
                            case true:
                                saveBlacklisted = Program.Ini.ReadBool("saveBlacklisted", "General");
                                saveBlacklisted_First = saveBlacklisted;
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
                                zeroToleranceBlacklist = string.Join(" ", File.ReadAllLines(Program.ApplicationPath + "\\blacklist.cfg"));
                                zeroToleranceBlacklist_First = zeroToleranceBlacklist;
                                break;
                        }
                        switch (Program.Ini.KeyExists("firstTime", "General")) {
                            case true:
                                firstTime = Program.Ini.ReadBool("firstTime", "General");
                                firstTime_First = firstTime;
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
                        blacklist = aphrodite.Settings.General.Default.blacklist;
                        saveBlacklisted = aphrodite.Settings.General.Default.saveBlacklisted;
                        saveInfo = aphrodite.Settings.General.Default.saveInfo;
                        ignoreFinish = aphrodite.Settings.General.Default.ignoreFinish;
                        zeroToleranceBlacklist = aphrodite.Settings.General.Default.zeroToleranceBlacklist;
                        firstTime = aphrodite.Settings.General.Default.firstTime;
                        undesiredTags = aphrodite.Settings.General.Default.undesiredTags;
                        openAfter = aphrodite.Settings.General.Default.openAfter;
                        break;
                }
            }
        }

        public class Config_Tags {
            public Config_Tags(bool SkipLoad = false) {
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
                switch (Program.UseIni) {
                    case true:
                        switch (Safe != Safe_First) {
                            case true:
                                Program.Ini.WriteBool("Safe", Safe, "Tags");
                                Safe_First = Safe;
                                break;
                        }
                        switch (Questionable != Questionable_First) {
                            case true:
                                Program.Ini.WriteBool("Questionable", Questionable, "Tags");
                                Questionable_First = Questionable;
                                break;
                        }
                        switch (Explicit != Explicit_First) {
                            case true:
                                Program.Ini.WriteBool("Explicit", Explicit, "Tags");
                                Explicit_First = Explicit;
                                break;
                        }
                        switch (separateRatings != separateRatings_First) {
                            case true:
                                Program.Ini.WriteBool("separateRatings", separateRatings, "Tags");
                                separateRatings_First = separateRatings;
                                break;
                        }
                        switch (separateNonImages != separateNonImages_First) {
                            case true:
                                Program.Ini.WriteBool("separateNonImages", separateNonImages, "Tags");
                                separateNonImages_First = separateNonImages;
                                break;
                        }
                        switch (enableScoreMin != enableScoreMin_First) {
                            case true:
                                Program.Ini.WriteBool("enableScoreMin", enableScoreMin, "Tags");
                                enableScoreMin_First = enableScoreMin;
                                break;
                        }
                        switch (scoreAsTag != scoreAsTag_First) {
                            case true:
                                Program.Ini.WriteBool("scoreAsTag", scoreAsTag, "Tags");
                                scoreAsTag_First = scoreAsTag;
                                break;
                        }
                        switch (scoreMin != scoreMin_First) {
                            case true:
                                Program.Ini.WriteInt("scoreMin", scoreMin, "Tags");
                                scoreMin_First = scoreMin;
                                break;
                        }
                        switch (imageLimit != imageLimit_First) {
                            case true:
                                Program.Ini.WriteInt("imageLimit", imageLimit, "Tags");
                                imageLimit_First = imageLimit;
                                break;
                        }
                        switch (pageLimit != pageLimit_First) {
                            case true:
                                Program.Ini.WriteInt("pageLimit", pageLimit, "Tags");
                                pageLimit_First = pageLimit;
                                break;
                        }
                        switch (fileNameSchema != fileNameSchema_First) {
                            case true:
                                Program.Ini.WriteString("fileNameSchema", fileNameSchema, "Tags");
                                fileNameSchema_First = fileNameSchema;
                                break;
                        }
                        break;

                    case false:
                        bool Save = false;

                        switch (aphrodite.Settings.Tags.Default.Safe != Safe) {
                            case true:
                                aphrodite.Settings.Tags.Default.Safe = Safe;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.Questionable != Questionable) {
                            case true:
                                aphrodite.Settings.Tags.Default.Questionable = Questionable;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.Explicit != Explicit) {
                            case true:
                                aphrodite.Settings.Tags.Default.Explicit = Explicit;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.separateRatings != separateRatings) {
                            case true:
                                aphrodite.Settings.Tags.Default.separateRatings = separateRatings;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.separateNonImages != separateNonImages) {
                            case true:
                                aphrodite.Settings.Tags.Default.separateNonImages = separateNonImages;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.enableScoreMin != enableScoreMin) {
                            case true:
                                aphrodite.Settings.Tags.Default.enableScoreMin = enableScoreMin;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.scoreAsTag != scoreAsTag) {
                            case true:
                                aphrodite.Settings.Tags.Default.scoreAsTag = scoreAsTag;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.scoreMin != scoreMin) {
                            case true:
                                aphrodite.Settings.Tags.Default.scoreMin = scoreMin;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.imageLimit != imageLimit) {
                            case true:
                                aphrodite.Settings.Tags.Default.imageLimit = imageLimit;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.pageLimit != pageLimit) {
                            case true:
                                aphrodite.Settings.Tags.Default.pageLimit = pageLimit;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.fileNameSchema != fileNameSchema) {
                            case true:
                                aphrodite.Settings.Tags.Default.fileNameSchema = fileNameSchema;
                                Save = true;
                                break;
                        }

                        switch (Save) {
                            case true:
                                aphrodite.Settings.Tags.Default.Save();
                                break;
                        }
                        break;
                }
            }
            public void Load() {
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
                Load();
            }

            public static void AppendToWishlist(string Name, string URL) {
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
                    if (!aphrodite.Settings.Pools.Default.wishlist.Contains(URL)) {
                        if (aphrodite.Settings.Pools.Default.addWishlistSilent) {
                            if (!string.IsNullOrWhiteSpace(aphrodite.Settings.Pools.Default.wishlistNames) && !string.IsNullOrWhiteSpace(aphrodite.Settings.Pools.Default.wishlist)) {
                                aphrodite.Settings.Pools.Default.wishlist += "|" + URL;
                                aphrodite.Settings.Pools.Default.wishlistNames += "|" + Name;
                            }
                            else {
                                aphrodite.Settings.Pools.Default.wishlist = URL;
                                aphrodite.Settings.Pools.Default.wishlistNames = Name;
                            }
                            aphrodite.Settings.Pools.Default.Save();
                        }
                        else {
                            frmPoolWishlist WishList = new frmPoolWishlist(true, URL, Name);
                            WishList.ShowDialog();
                        }
                    }
                }
            }
            public static void SaveWishlist(List<string> Names, List<string> URLs) {
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
                        break;
                }
            }

            #region Variables
            public bool mergeBlacklisted = true;
            public string wishlist = string.Empty;
            public string wishlistNames = string.Empty;
            public bool addWishlistSilent = false;
            public string fileNameSchema = "%poolname%_%page%";

            private bool mergeBlacklisted_First = true;
            public string wishlist_First = string.Empty;
            public string wishlistNames_First = string.Empty;
            private bool addWishlistSilent_First = false;
            private string fileNameSchema_First = "%poolname%_%page%";
            #endregion

            public void Save() {
                switch (Program.UseIni) {
                    case true:
                        switch (mergeBlacklisted != mergeBlacklisted_First) {
                            case true:
                                Program.Ini.WriteBool("mergeBlacklisted", mergeBlacklisted, "Pools");
                                mergeBlacklisted_First = mergeBlacklisted;
                                break;
                        }
                        switch (addWishlistSilent != addWishlistSilent_First) {
                            case true:
                                Program.Ini.WriteBool("addWishlistSilent", addWishlistSilent, "Pools");
                                addWishlistSilent_First = addWishlistSilent;
                                break;
                        }
                        switch (fileNameSchema != fileNameSchema_First) {
                            case true:
                                Program.Ini.WriteString("fileNameSchema", fileNameSchema, "Pools");
                                fileNameSchema_First = fileNameSchema;
                                break;
                        }
                        break;

                    case false:
                        bool Save = false;

                        switch (aphrodite.Settings.Pools.Default.mergeBlacklisted != mergeBlacklisted) {
                            case true:
                                aphrodite.Settings.Pools.Default.mergeBlacklisted = mergeBlacklisted;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Pools.Default.addWishlistSilent != addWishlistSilent) {
                            case true:
                                aphrodite.Settings.Pools.Default.addWishlistSilent = addWishlistSilent;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Pools.Default.fileNameSchema != fileNameSchema) {
                            case true:
                                aphrodite.Settings.Pools.Default.fileNameSchema = fileNameSchema;
                                Save = true;
                                break;
                        }

                        switch (Save) {
                            case true:
                                aphrodite.Settings.Pools.Default.Save();
                                break;
                        }
                        break;
                }
            }
            public void Load() {
                switch (Program.UseIni) {
                    case true:
                        switch (Program.Ini.KeyExists("mergeBlacklisted", "Pools")) {
                            case true:
                                mergeBlacklisted = Program.Ini.ReadBool("mergeBlacklisted", "Pools");
                                mergeBlacklisted_First = mergeBlacklisted;
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
                        mergeBlacklisted = aphrodite.Settings.Pools.Default.mergeBlacklisted;
                        wishlist = aphrodite.Settings.Pools.Default.wishlist;
                        wishlistNames = aphrodite.Settings.Pools.Default.wishlistNames;
                        addWishlistSilent = aphrodite.Settings.Pools.Default.addWishlistSilent;
                        fileNameSchema = aphrodite.Settings.Pools.Default.fileNameSchema;
                        break;
                }
            }
        }
        public class Config_Images {
            public Config_Images() {
                Load();
            }

            #region Variables
            public bool separateRatings = true;
            public bool separateBlacklisted = true;
            public bool useForm = false;
            public bool separateArtists = false;
            public string fileNameSchema = "%artist%_%md5";
            public bool separateNonImages = true;

            private bool separateRatings_First = true;
            private bool separateBlacklisted_First = true;
            private bool useForm_First = false;
            private bool separateArtists_First = false;
            private string fileNameSchema_First = "%artist%_%md5";
            private bool separateNonImages_First = true;
            #endregion

            public void Save() {
                switch (Program.UseIni) {
                    case true:
                        switch (separateRatings != separateRatings_First) {
                            case true:
                                Program.Ini.WriteBool("separateRatings", separateRatings, "Images");
                                separateRatings_First = separateRatings;
                                break;
                        }
                        switch (separateBlacklisted != separateBlacklisted_First) {
                            case true:
                                Program.Ini.WriteBool("separateBlacklisted", separateBlacklisted, "Images");
                                separateBlacklisted_First = separateBlacklisted;
                                break;
                        }
                        switch (useForm != useForm_First) {
                            case true:
                                Program.Ini.WriteBool("useForm", useForm, "Images");
                                useForm_First = useForm;
                                break;
                        }
                        switch (separateArtists != separateArtists_First) {
                            case true:
                                Program.Ini.WriteBool("separateArtists", separateArtists, "Images");
                                separateArtists_First = separateArtists;
                                break;
                        }
                        switch (fileNameSchema != fileNameSchema_First) {
                            case true:
                                Program.Ini.WriteString("fileNameSchema", fileNameSchema, "Images");
                                fileNameSchema_First = fileNameSchema;
                                break;
                        }
                        switch (separateNonImages != separateNonImages_First) {
                            case true:
                                Program.Ini.WriteBool("separateNonImages", separateNonImages, "Images");
                                separateNonImages_First = separateNonImages;
                                break;
                        }
                        break;

                    case false:
                        bool Save = false;

                        switch (aphrodite.Settings.Images.Default.separateRatings != separateRatings) {
                            case true:
                                aphrodite.Settings.Images.Default.separateRatings = separateRatings;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Images.Default.separateBlacklisted != separateBlacklisted) {
                            case true:
                                aphrodite.Settings.Images.Default.separateBlacklisted = separateBlacklisted;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Images.Default.useForm != useForm) {
                            case true:
                                aphrodite.Settings.Images.Default.useForm = useForm;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Images.Default.separateArtists != separateArtists) {
                            case true:
                                aphrodite.Settings.Images.Default.separateArtists = separateArtists;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Images.Default.fileNameSchema != fileNameSchema) {
                            case true:
                                aphrodite.Settings.Images.Default.fileNameSchema = fileNameSchema;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Images.Default.separateNonImages != separateNonImages) {
                            case true:
                                aphrodite.Settings.Images.Default.separateNonImages = separateNonImages;
                                Save = true;
                                break;
                        }

                        switch (Save) {
                            case true:
                                aphrodite.Settings.Images.Default.Save();
                                break;
                        }
                        break;
                }
            }
            public void Load() {
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
                                separateBlacklisted = Program.Ini.ReadBool("separateBlacklisted", "Images");
                                separateBlacklisted_First = separateBlacklisted;
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
                        break;

                    case false:
                        separateRatings = aphrodite.Settings.Images.Default.separateRatings;
                        separateBlacklisted = aphrodite.Settings.Images.Default.separateBlacklisted;
                        useForm = aphrodite.Settings.Images.Default.useForm;
                        separateArtists = aphrodite.Settings.Images.Default.separateArtists;
                        fileNameSchema = aphrodite.Settings.Images.Default.fileNameSchema;
                        separateNonImages = aphrodite.Settings.Images.Default.separateNonImages;
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
