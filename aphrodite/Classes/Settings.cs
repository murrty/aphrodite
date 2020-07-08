namespace aphrodite {
    class Settings {
        #region Settings instance
        private static Settings Instance = new Settings();
        public static Settings GetInstance() {
            return Instance;
        }
        #endregion

        public static volatile bool UseIni = false;

        public static void SaveSettings() {
            if (UseIni) {
                if (Forms.frmMainX_HasChanged) {
                
                }
                if (Forms.frmMainY_HasChanged) {

                }

                Forms.SetChangedValues(false);
                GeneralSettings.SetChangedValues(false);
            }
            else {
                FormSettings.Default.Save();
                General.Default.Save();
                Tags.Default.Save();
                Pools.Default.Save();
                Images.Default.Save();
            }
        }
        public static void LoadSettings() {
            if (UseIni) {

            }
        }

        private class Example {
            private static volatile bool SettingName_Value = true;
            private static volatile bool SettingName_Loaded = true;
            private static volatile bool SettingName_Changed = false;
            public static bool SettingName {
                get { return SettingName_Value; }
                set {
                    SettingName_Value = value;
                    if (value != SettingName_Loaded) {
                        SettingName_Changed = true;
                    }
                    else {
                        SettingName_Changed = false;
                    }
                }
            }
            public static bool SettingName_HasChanged {
                get { return SettingName_Changed; }
            }
        }

        class Forms {
            private static volatile int frmMainX_Value = -999999;
            private static volatile int frmMainX_Loaded = -999999;
            private static volatile bool frmMainX_Changed = false;
            private static volatile int frmMainY_Value = -999999;
            private static volatile int frmMainY_Loaded = -999999;
            private static volatile bool frmMainY_Changed = false;

            public static int frmMainX {
                get { return frmMainX_Value; }
                set {
                    frmMainX_Value = value;
                    if (value != frmMainX_Loaded) {
                        frmMainX_Changed = true;
                    }
                    else {
                        frmMainX_Changed = false;
                    }
                }
            }
            public static bool frmMainX_HasChanged {
                get { return frmMainX_Changed; }
            }

            public static int frmMainY {
                get { return frmMainY_Value; }
                set {
                    frmMainY_Value = value;
                    if (value != frmMainY_Loaded) {
                        frmMainY_Changed = true;
                    }
                    else {
                        frmMainY_Changed = false;
                    }
                }
            }
            public static bool frmMainY_HasChanged {
                get { return frmMainY_Changed; }
            }

            public static void SetChangedValues(bool NewBoolean) {
                frmMainX_Changed = NewBoolean;
                frmMainY_Changed = NewBoolean;
            }
        }
        class GeneralSettings {
            // Does not include:
            // firstTime (Handled by, and not required after, Program.cs)

            #region saveLocation
            private static volatile string saveLocation_Value = string.Empty;
            private static volatile string saveLocation_Loaded = string.Empty;
            private static volatile bool saveLocation_Changed = false;
            public static string saveLocation {
                get { return saveLocation_Value; }
                set {
                    saveLocation_Value = value;
                    if (value != saveLocation_Loaded) {
                        saveLocation_Changed = true;
                    }
                    else {
                        saveLocation_Changed = false;
                    }
                }
            }
            public static bool saveLocation_HasChanged {
                get { return saveLocation_Changed; }
            }
            #endregion

            #region blacklist
            private static volatile string blacklist_Value = string.Empty;
            private static volatile string blacklist_Loaded = string.Empty;
            private static volatile bool blacklist_Changed = false;
            public static string blacklist {
                get { return blacklist_Value; }
                set {
                    blacklist_Value = value;
                    if (value != blacklist_Loaded) {
                        blacklist_Changed = true;
                    }
                    else {
                        blacklist_Changed = false;
                    }
                }
            }
            public static bool blacklist_HasChanged {
                get { return blacklist_Changed; }
            }
            #endregion

            #region saveBlacklisted
            private static volatile bool saveBlacklisted_Value = true;
            private static volatile bool saveBlacklisted_Loaded = true;
            private static volatile bool saveBlacklisted_Changed = false;
            public static bool saveBlacklisted {
                get { return saveBlacklisted_Value; }
                set {
                    saveBlacklisted_Value = value;
                    if (value != saveBlacklisted_Loaded) {
                        saveBlacklisted_Changed = true;
                    }
                    else {
                        saveBlacklisted_Changed = false;
                    }
                }
            }
            public static bool saveBlacklisted_HasChanged {
                get { return saveBlacklisted_Changed; }
            }
            #endregion

            #region saveInfo
            private static volatile bool saveInfo_Value = true;
            private static volatile bool saveInfo_Loaded = true;
            private static volatile bool saveInfo_Changed = false;
            public static bool saveInfo {
                get { return saveInfo_Value; }
                set {
                    saveInfo_Value = value;
                    if (value != saveInfo_Loaded) {
                        saveInfo_Changed = true;
                    }
                    else {
                        saveInfo_Changed = false;
                    }
                }
            }
            public static bool saveInfo_HasChanged {
                get { return saveInfo_Changed; }
            }
            #endregion

            #region ignoreFinish
            private static volatile bool ignoreFinish_Value = false;
            private static volatile bool ignoreFinish_Loaded = false;
            private static volatile bool ignoreFinish_Changed = false;
            public static bool ignoreFinish {
                get { return ignoreFinish_Value; }
                set {
                    ignoreFinish_Value = value;
                    if (value != ignoreFinish_Loaded) {
                        ignoreFinish_Changed = true;
                    }
                    else {
                        ignoreFinish_Changed = false;
                    }
                }
            }
            public static bool ignoreFinish_HasChanged {
                get { return ignoreFinish_Changed; }
            }
            #endregion

            #region zeroToleranceBlacklist
            private static volatile string zeroToleranceBlacklist_Value = string.Empty;
            private static volatile string zeroToleranceBlacklist_Loaded = string.Empty;
            private static volatile bool zeroToleranceBlacklist_Changed = false;
            public static string zeroToleranceBlacklist {
                get { return zeroToleranceBlacklist_Value; }
                set {
                    zeroToleranceBlacklist_Value = value;
                    if (value != zeroToleranceBlacklist_Loaded) {
                        zeroToleranceBlacklist_Changed = true;
                    }
                    else {
                        zeroToleranceBlacklist_Changed = false;
                    }
                }
            }
            public static bool zeroToleranceBlacklist_HasChanged {
                get { return zeroToleranceBlacklist_Changed; }
            }
            #endregion

            #region undesiredTags
            private static volatile string undesiredTags_Value = string.Empty;
            private static volatile string undesiredTags_Loaded = string.Empty;
            private static volatile bool undesiredTags_Changed = false;
            public static string undesiredTags {
                get { return undesiredTags_Value; }
                set {
                    undesiredTags_Value = value;
                    if (value != undesiredTags_Loaded) {
                        undesiredTags_Changed = true;
                    }
                    else {
                        undesiredTags_Changed = false;
                    }
                }
            }
            public static bool undesiredTags_HasChanged {
                get { return undesiredTags_Changed; }
            }
            #endregion

            #region saveMetadata
            private static volatile bool saveMetadata_Value = false;
            private static volatile bool saveMetadata_Loaded = false;
            private static volatile bool saveMetadata_Changed = false;
            public static bool saveMetadata {
                get { return saveMetadata_Value; }
                set {
                    saveMetadata_Value = value;
                    if (value != saveMetadata_Loaded) {
                        saveMetadata_Changed = true;
                    }
                    else {
                        saveMetadata_Changed = false;
                    }
                }
            }
            public static bool saveMetadata_HasChanged {
                get { return saveMetadata_Changed; }
            }
            #endregion

            #region saveArtistMetadata
            private static volatile bool saveArtistMetadata_Value = true;
            private static volatile bool saveArtistMetadata_Loaded = true;
            private static volatile bool saveArtistMetadata_Changed = false;
            public static bool saveArtistMetadata {
                get { return saveArtistMetadata_Value; }
                set {
                    saveArtistMetadata_Value = value;
                    if (value != saveArtistMetadata_Loaded) {
                        saveArtistMetadata_Changed = true;
                    }
                    else {
                        saveArtistMetadata_Changed = false;
                    }
                }
            }
            public static bool saveArtistMetadata_HasChanged {
                get { return saveArtistMetadata_Changed; }
            }
            #endregion

            #region saveTagMetadata
            private static volatile bool saveTagMetadata_Value = true;
            private static volatile bool saveTagMetadata_Loaded = true;
            private static volatile bool saveTagMetadata_Changed = false;
            public static bool saveTagMetadata {
                get { return saveTagMetadata_Value; }
                set {
                    saveTagMetadata_Value = value;
                    if (value != saveTagMetadata_Loaded) {
                        saveTagMetadata_Changed = true;
                    }
                    else {
                        saveTagMetadata_Changed = false;
                    }
                }
            }
            public static bool saveTagMetadata_HasChanged {
                get { return saveTagMetadata_Changed; }
            }
            #endregion

            public static void SetChangedValues(bool NewBoolean) {
                saveLocation_Changed = NewBoolean;
                blacklist_Changed = NewBoolean;
                saveBlacklisted_Changed = NewBoolean;
                saveInfo_Changed = NewBoolean;
                ignoreFinish_Changed = NewBoolean;
                zeroToleranceBlacklist_Changed = NewBoolean;
                undesiredTags_Changed = NewBoolean;
                saveMetadata_Changed = NewBoolean;
                saveArtistMetadata_Changed = NewBoolean;
                saveTagMetadata_Changed = NewBoolean;
            }
        }
        class TagsSettings {
            private static volatile bool Safe_Value = true;
            private static volatile bool Save_Loaded = true;
            private static volatile bool Safe_Changed = false;

            private static volatile bool Questionable_Value = true;
            private static volatile bool Questionable_Loaded = true;
            private static volatile bool Questionable_Changed = false;

            private static volatile bool Explicit_Value = true;
            private static volatile bool Explicit_Loaded = true;
            private static volatile bool Explicit_Changed = false;

            private static volatile bool enableScoreMin_Value = false;
            private static volatile bool enableScoreMin_Loaded = false;
            private static volatile bool enableScoreMin_Changed = false;

            private static volatile int scoreMin_Value = 0;
            private static volatile int scoreMin_Loaded = 0;
            private static volatile bool scoreMin_Changed = false;

            private static volatile int imageLimit_Value = 0;
            private static volatile int imageLimit_Loaded = 0;
            private static volatile bool imageLimit_Changed = false;

            private static volatile bool separateRatings_Value = true;
            private static volatile bool separateRatings_Loaded = true;
            private static volatile bool separateRatings_Changed = false;

            private static volatile int pageLimit_Value = 0;
            private static volatile int pageLimit_Loaded = 0;
            private static volatile bool pageLimit_Changed = false;

            private static volatile bool scoreAsTag_Value = true;
            private static volatile bool scoreAsTag_Loaded = true;
            private static volatile bool scoreAsTag_Changed = false;


        }
        class PoolsSettings {

        }
        class ImagesSettings {

        }
    }
}