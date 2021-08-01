using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;

namespace aphrodite {

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

        public void ConvertConfig(bool UseIni) {
            if (Program.UseIni && !UseIni) {
                Program.Ini.Write("useIni", false);
            }
            else {
                Program.Ini.Write("useIni", true);
            }

            Program.UseIni = UseIni;
            Initialization.ForceSave();
            FormSettings.ForceSave();
            General.ForceSave();
            Tags.ForceSave();
            Pools.ForceSave();
            Images.ForceSave();
        }

        public class Config_Initialization {
            public Config_Initialization() {
                Program.Log(LogAction.WriteToLog, "Initializing Config_Initialization (ironic)");
                Load();
            }

            public bool firstTime = true;
            public bool SkipArgumentCheck = false;
            public bool AutoDownloadWithArguments = true;
            public bool ArgumentFormTopMost = true;
            public decimal SkippedVersion = -1;

            private bool firstTime_First = true;
            private bool SkipArgumentCheck_First = false;
            private bool AutoDownloadWithArguments_First = true;
            private bool ArgumentFormTopMost_First = true;
            private decimal SkippedVersion_First = -1;

            public void Save() {
                Program.Log(LogAction.WriteToLog, "Attempting to save Config_Initialization settings");

                switch (Program.UseIni) {
                    case true:
                        switch (firstTime != firstTime_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> firstTime changed!");
                                Program.Ini.Write("firstTime", firstTime, "General");
                                firstTime_First = firstTime;
                                break;
                        }

                        switch (SkipArgumentCheck != SkipArgumentCheck_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> SkipArgumentCheck changed!");
                                Program.Ini.Write("SkipArgumentCheck", SkipArgumentCheck, "General");
                                SkipArgumentCheck_First = SkipArgumentCheck;
                                break;
                        }

                        switch (AutoDownloadWithArguments != AutoDownloadWithArguments_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> SkipArgumentCheck changed!");
                                Program.Ini.Write("AutoDownloadWithArguments", AutoDownloadWithArguments, "General");
                                AutoDownloadWithArguments_First = AutoDownloadWithArguments;
                                break;
                        }

                        switch (ArgumentFormTopMost != ArgumentFormTopMost_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> ArgumentFormTopMost changed!");
                                Program.Ini.Write("ArgumentFormTopMost", ArgumentFormTopMost, "General");
                                ArgumentFormTopMost_First = ArgumentFormTopMost;
                                break;
                        }

                        switch (SkippedVersion != SkippedVersion_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "SkippedVersion changed!");
                                Program.Ini.Write("SkippedVersion", SkippedVersion);
                                SkippedVersion_First = SkippedVersion;
                                break;
                        }
                        break;

                    case false: {
                            bool Save = false;

                            switch (aphrodite.Settings.General.Default.firstTime != firstTime) {
                                case true:
                                    aphrodite.Settings.General.Default.firstTime = firstTime;
                                    Save = true;
                                    break;
                            }

                            switch (aphrodite.Settings.General.Default.SkipArgumentCheck != SkipArgumentCheck) {
                                case true:
                                    aphrodite.Settings.General.Default.SkipArgumentCheck = SkipArgumentCheck;
                                    Save = true;
                                    break;
                            }

                            switch (aphrodite.Settings.General.Default.AutoDownloadWithArguments != AutoDownloadWithArguments) {
                                case true:
                                    aphrodite.Settings.General.Default.AutoDownloadWithArguments = AutoDownloadWithArguments;
                                    Save = true;
                                    break;
                            }

                            switch (aphrodite.Settings.General.Default.ArgumentFormTopMost != ArgumentFormTopMost) {
                                case true:
                                    aphrodite.Settings.General.Default.ArgumentFormTopMost = ArgumentFormTopMost;
                                    Save = true;
                                    break;
                            }

                            switch (aphrodite.Properties.Settings.Default.SkippedVersion != SkippedVersion) {
                                case true:
                                    aphrodite.Properties.Settings.Default.SkippedVersion = SkippedVersion;
                                    Save = true;
                                    break;
                            }

                            switch (Save) {
                                case true:
                                    Program.Log(LogAction.WriteToLog, "Saving Initialization");
                                    aphrodite.Settings.General.Default.Save();
                                    aphrodite.Properties.Settings.Default.Save();
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
                                firstTime_First = firstTime;
                                break;

                            case false:
                                break;
                        }

                        switch (Program.Ini.KeyExists("SkipArgumentCheck", "General")) {
                            case true:
                                SkipArgumentCheck = Program.Ini.ReadBool("SkipArgumentCheck", "General");
                                SkipArgumentCheck_First = SkipArgumentCheck;
                                break;

                            case false:
                                break;
                        }

                        switch (Program.Ini.KeyExists("AutoDownloadWithArguments", "General")) {
                            case true:
                                AutoDownloadWithArguments = Program.Ini.ReadBool("AutoDownloadWithArguments", "General");
                                AutoDownloadWithArguments_First = AutoDownloadWithArguments;
                                break;

                            case false:
                                break;
                        }

                        switch (Program.Ini.KeyExists("ArgumentFormTopMost", "General")) {
                            case true:
                                ArgumentFormTopMost = Program.Ini.ReadBool("ArgumentFormTopMost", "General");
                                ArgumentFormTopMost_First = ArgumentFormTopMost;
                                break;

                            case false:
                                break;
                        }

                        switch (Program.Ini.KeyExists("SkippedVersion")) {
                            case true:
                                SkippedVersion = Program.Ini.ReadDecimal("SkippedVersion");
                                SkippedVersion_First = SkippedVersion;
                                break;

                            case false:
                                break;
                        }
                        break;

                    case false:
                        firstTime = aphrodite.Settings.General.Default.firstTime;
                        SkipArgumentCheck = aphrodite.Settings.General.Default.SkipArgumentCheck;
                        AutoDownloadWithArguments = aphrodite.Settings.General.Default.AutoDownloadWithArguments;
                        ArgumentFormTopMost = aphrodite.Settings.General.Default.ArgumentFormTopMost;
                        SkippedVersion = aphrodite.Properties.Settings.Default.SkippedVersion;
                        break;
                }
            }

            public void ForceSave() {
                Program.Log(LogAction.WriteToLog, "Force saving Config_Initialization settings");

                switch (Program.UseIni) {
                    case true:
                        Program.Ini.Write("firstTime", firstTime, "General");
                        firstTime_First = firstTime;

                        Program.Ini.Write("SkipArgumentCheck", SkipArgumentCheck, "General");
                        SkipArgumentCheck_First = SkipArgumentCheck;

                        Program.Ini.Write("AutoDownloadWithArguments", AutoDownloadWithArguments, "General");
                        AutoDownloadWithArguments_First = AutoDownloadWithArguments;

                        Program.Ini.Write("ArgumentFormTopMost", ArgumentFormTopMost, "General");
                        ArgumentFormTopMost_First = ArgumentFormTopMost;

                        Program.Ini.Write("SkippedVersion", SkippedVersion);
                        SkippedVersion_First = SkippedVersion;
                        break;

                    case false:
                        aphrodite.Settings.General.Default.firstTime = firstTime;
                        aphrodite.Settings.General.Default.SkipArgumentCheck = SkipArgumentCheck;
                        aphrodite.Settings.General.Default.AutoDownloadWithArguments = AutoDownloadWithArguments;
                        aphrodite.Settings.General.Default.ArgumentFormTopMost = ArgumentFormTopMost;
                        aphrodite.Settings.General.Default.Save();

                        aphrodite.Properties.Settings.Default.SkippedVersion = SkippedVersion;
                        aphrodite.Properties.Settings.Default.Save();
                        break;
                }
            }
        }

        public class Config_FormSettings {
            public Config_FormSettings(bool SkipLoad = false) {
                Program.Log(LogAction.WriteToLog, "Initializing Config_FormSettings");
                switch (!SkipLoad) {
                    case true:
                        Load();
                        break;
                }
            }

            #region Variables
            public Point frmBlacklist_Location = new Point(-32000, -32000);
            public Point frmImageDownloader_Location = new Point(-32000, -32000);
            public Point frmLog_Location = new Point(-32000, -32000);
            public Size frmLog_Size = new Size(-32000, -32000);
            public Point frmMain_Location = new Point(-32000, -32000);
            public Point frmPoolDownloader_Location = new Point(-32000, -32000);
            public Point frmPoolWishlist_Location = new Point(-32000, -32000);
            public Point frmRedownloader_Location = new Point(-32000, -32000);
            public Point frmSettings_Location = new Point(-32000, -32000);
            public Point frmTagDownloader_Location = new Point(-32000, -32000);
            public Point frmUndesiredTags_Location = new Point(-32000, -32000);

            private Point frmBlacklist_Location_First = new Point(-32000, -32000);
            private Point frmImageDownloader_Location_First = new Point(-32000, -32000);
            private Point frmLog_Location_First = new Point(-32000, -32000);
            private Size frmLog_Size_First = new Size(-32000, -32000);
            private Point frmMain_Location_First = new Point(-32000, -32000);
            private Point frmPoolDownloader_Location_First = new Point(-32000, -32000);
            private Point frmPoolWishlist_Location_First = new Point(-32000, -32000);
            private Point frmRedownloader_Location_First = new Point(-32000, -32000);
            private Point frmSettings_Location_First = new Point(-32000, -32000);
            private Point frmTagDownloader_Location_First = new Point(-32000, -32000);
            private Point frmUndesiredTags_Location_First = new Point(-32000, -32000);
            #endregion

            public void Save() {
                Program.Log(LogAction.WriteToLog, "Attempting to save Config_FormSettings settings");

                switch (Program.UseIni) {
                    case true:
                        switch (frmBlacklist_Location != frmBlacklist_Location_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "FormSettings -> frmBlacklist_Location changed!");
                                Program.Ini.Write("frmBlacklist_Location", frmBlacklist_Location, "FormSettings");
                                frmBlacklist_Location_First = frmBlacklist_Location;
                                break;
                        }
                        switch (frmImageDownloader_Location != frmImageDownloader_Location_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "FormSettings -> frmImageDownloader_Location changed!");
                                Program.Ini.Write("frmImageDownloader_Location", frmImageDownloader_Location, "FormSettings");
                                frmImageDownloader_Location_First = frmImageDownloader_Location;
                                break;
                        }
                        switch (frmLog_Location != frmLog_Location_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "FormSettings -> frmLog_Location changed!");
                                Program.Ini.Write("frmLog_Location", frmLog_Location, "FormSettings");
                                frmLog_Location_First = frmLog_Location;
                                break;
                        }
                        switch (frmLog_Size != frmLog_Size_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "FormSettings -> frmLog_Size changed!");
                                Program.Ini.Write("frmLog_Size", frmLog_Size, "FormSettings");
                                frmLog_Size_First = frmLog_Size;
                                break;
                        }
                        switch (frmMain_Location != frmMain_Location_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "FormSettings -> frmMain_Location changed!");
                                Program.Ini.Write("frmMain_Location", frmMain_Location, "FormSettings");
                                frmMain_Location_First = frmMain_Location;
                                break;
                        }
                        switch (frmPoolDownloader_Location != frmPoolDownloader_Location_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "FormSettings -> frmPoolDownloader_Location changed!");
                                Program.Ini.Write("frmPoolDownloader_Location", frmPoolDownloader_Location, "FormSettings");
                                frmPoolDownloader_Location_First = frmPoolDownloader_Location;
                                break;
                        }
                        switch (frmPoolWishlist_Location != frmPoolWishlist_Location_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "FormSettings -> frmPoolWishlist_Location changed!");
                                Program.Ini.Write("frmPoolWishlist_Location", frmPoolWishlist_Location, "FormSettings");
                                frmPoolWishlist_Location_First = frmPoolWishlist_Location;
                                break;
                        }
                        switch (frmRedownloader_Location != frmRedownloader_Location_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "FormSettings -> frmRedownloader_Location changed!");
                                Program.Ini.Write("frmRedownloader_Location", frmRedownloader_Location, "FormSettings");
                                frmRedownloader_Location_First = frmRedownloader_Location;
                                break;
                        }
                        switch (frmSettings_Location != frmSettings_Location_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "FormSettings -> frmSettings_Location changed!");
                                Program.Ini.Write("frmSettings_Location", frmSettings_Location, "FormSettings");
                                frmSettings_Location_First = frmSettings_Location;
                                break;
                        }
                        switch (frmTagDownloader_Location != frmTagDownloader_Location_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "FormSettings -> frmTagDownloader_Location changed!");
                                Program.Ini.Write("frmTagDownloader_Location", frmTagDownloader_Location, "FormSettings");
                                frmTagDownloader_Location_First = frmTagDownloader_Location;
                                break;
                        }
                        switch (frmUndesiredTags_Location != frmUndesiredTags_Location_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "FormSettings -> frmUndesiredTags_Location changed!");
                                Program.Ini.Write("frmUndesiredTags_Location", frmUndesiredTags_Location, "FormSettings");
                                frmUndesiredTags_Location_First = frmUndesiredTags_Location;
                                break;
                        }
                        break;

                    case false: {
                            bool Save = false;

                            switch (aphrodite.Settings.FormSettings.Default.frmBlacklist_Location != frmBlacklist_Location) {
                                case true:
                                    Program.Log(LogAction.WriteToLog, "FormSettings -> frmBlacklist_Location changed!");
                                    aphrodite.Settings.FormSettings.Default.frmBlacklist_Location = frmBlacklist_Location;
                                    Save = true;
                                    break;
                            }
                            switch (aphrodite.Settings.FormSettings.Default.frmImageDownloader_Location != frmImageDownloader_Location) {
                                case true:
                                    Program.Log(LogAction.WriteToLog, "FormSettings -> frmImageDownloader_Location changed!");
                                    aphrodite.Settings.FormSettings.Default.frmImageDownloader_Location = frmImageDownloader_Location;
                                    Save = true;
                                    break;
                            }
                            switch (aphrodite.Settings.FormSettings.Default.frmLog_Location != frmLog_Location) {
                                case true:
                                    Program.Log(LogAction.WriteToLog, "FormSettings -> frmLog_Location changed!");
                                    aphrodite.Settings.FormSettings.Default.frmLog_Location = frmLog_Location;
                                    Save = true;
                                    break;
                            }
                            switch (aphrodite.Settings.FormSettings.Default.frmLog_Size != frmLog_Size) {
                                case true:
                                    Program.Log(LogAction.WriteToLog, "FormSettings -> frmLog_Size changed!");
                                    aphrodite.Settings.FormSettings.Default.frmLog_Size = frmLog_Size;
                                    Save = true;
                                    break;
                            }
                            switch (aphrodite.Settings.FormSettings.Default.frmMain_Location != frmMain_Location) {
                                case true:
                                    Program.Log(LogAction.WriteToLog, "FormSettings -> frmMain_Location changed!");
                                    aphrodite.Settings.FormSettings.Default.frmMain_Location = frmMain_Location;
                                    Save = true;
                                    break;
                            }
                            switch (aphrodite.Settings.FormSettings.Default.frmPoolDownloader_Location != frmPoolDownloader_Location) {
                                case true:
                                    Program.Log(LogAction.WriteToLog, "FormSettings -> frmPoolDownloader_Location changed!");
                                    aphrodite.Settings.FormSettings.Default.frmPoolDownloader_Location = frmPoolDownloader_Location;
                                    Save = true;
                                    break;
                            }
                            switch (aphrodite.Settings.FormSettings.Default.frmPoolWishlist_Location != frmPoolWishlist_Location) {
                                case true:
                                    Program.Log(LogAction.WriteToLog, "FormSettings -> frmPoolWishlist_Location changed!");
                                    aphrodite.Settings.FormSettings.Default.frmPoolWishlist_Location = frmPoolWishlist_Location;
                                    Save = true;
                                    break;
                            }
                            switch (aphrodite.Settings.FormSettings.Default.frmRedownloader_Location != frmRedownloader_Location) {
                                case true:
                                    Program.Log(LogAction.WriteToLog, "FormSettings -> frmRedownloader_Location changed!");
                                    aphrodite.Settings.FormSettings.Default.frmRedownloader_Location = frmRedownloader_Location;
                                    Save = true;
                                    break;
                            }
                            switch (aphrodite.Settings.FormSettings.Default.frmSettings_Location != frmSettings_Location) {
                                case true:
                                    Program.Log(LogAction.WriteToLog, "FormSettings -> frmSettings_Location changed!");
                                    aphrodite.Settings.FormSettings.Default.frmSettings_Location = frmSettings_Location;
                                    Save = true;
                                    break;
                            }
                            switch (aphrodite.Settings.FormSettings.Default.frmTagDownloader_Location != frmTagDownloader_Location) {
                                case true:
                                    Program.Log(LogAction.WriteToLog, "FormSettings -> frmTagDownloader_Location changed!");
                                    aphrodite.Settings.FormSettings.Default.frmTagDownloader_Location = frmTagDownloader_Location;
                                    Save = true;
                                    break;
                            }
                            switch (aphrodite.Settings.FormSettings.Default.frmUndesiredTags_Location != frmUndesiredTags_Location) {
                                case true:
                                    Program.Log(LogAction.WriteToLog, "FormSettings -> frmUndesiredTags_Location changed!");
                                    aphrodite.Settings.FormSettings.Default.frmUndesiredTags_Location = frmUndesiredTags_Location;
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
                        switch (Program.Ini.KeyExists("frmBlacklist_Location", "FormSettings")) {
                            case true:
                                frmBlacklist_Location = Program.Ini.ReadPoint("frmBlacklist_Location", "FormSettings");
                                frmBlacklist_Location_First = frmBlacklist_Location;
                                break;
                        }
                        switch (Program.Ini.KeyExists("frmImageDownloader_Location", "FormSettings")) {
                            case true:
                                frmImageDownloader_Location = Program.Ini.ReadPoint("frmImageDownloader_Location", "FormSettings");
                                frmImageDownloader_Location_First = frmImageDownloader_Location;
                                break;
                        }
                        switch (Program.Ini.KeyExists("frmLog_Location", "FormSettings")) {
                            case true:
                                frmLog_Location = Program.Ini.ReadPoint("frmLog_Location", "FormSettings");
                                frmLog_Location_First = frmLog_Location;
                                break;
                        }
                        switch (Program.Ini.KeyExists("frmLog_Size", "FormSettings")) {
                            case true:
                                frmLog_Size = Program.Ini.ReadSize("frmLog_Size", "FormSettings");
                                frmLog_Size_First = frmLog_Size;
                                break;
                        }
                        switch (Program.Ini.KeyExists("frmMain_Location", "FormSettings")) {
                            case true:
                                frmMain_Location = Program.Ini.ReadPoint("frmMain_Location", "FormSettings");
                                frmMain_Location_First = frmMain_Location;
                                break;
                        }
                        switch (Program.Ini.KeyExists("frmPoolDownloader_Location", "FormSettings")) {
                            case true:
                                frmPoolDownloader_Location = Program.Ini.ReadPoint("frmPoolDownloader_Location", "FormSettings");
                                frmPoolDownloader_Location_First = frmPoolDownloader_Location;
                                break;
                        }
                        switch (Program.Ini.KeyExists("frmPoolWishlist_Location", "FormSettings")) {
                            case true:
                                frmPoolWishlist_Location = Program.Ini.ReadPoint("frmPoolWishlist_Location", "FormSettings");
                                frmPoolWishlist_Location_First = frmPoolWishlist_Location;
                                break;
                        }
                        switch (Program.Ini.KeyExists("frmRedownloader_Location", "FormSettings")) {
                            case true:
                                frmRedownloader_Location = Program.Ini.ReadPoint("frmRedownloader_Location", "FormSettings");
                                frmRedownloader_Location_First = frmRedownloader_Location;
                                break;
                        }
                        switch (Program.Ini.KeyExists("frmSettings_Location", "FormSettings")) {
                            case true:
                                frmSettings_Location = Program.Ini.ReadPoint("frmSettings_Location", "FormSettings");
                                frmSettings_Location_First = frmSettings_Location;
                                break;
                        }
                        switch (Program.Ini.KeyExists("frmTagDownloader_Location", "FormSettings")) {
                            case true:
                                frmTagDownloader_Location = Program.Ini.ReadPoint("frmTagDownloader_Location", "FormSettings");
                                frmTagDownloader_Location_First = frmTagDownloader_Location;
                                break;
                        }
                        switch (Program.Ini.KeyExists("frmUndesiredTags_Location", "FormSettings")) {
                            case true:
                                frmUndesiredTags_Location = Program.Ini.ReadPoint("frmUndesiredTags_Location", "FormSettings");
                                frmUndesiredTags_Location_First = frmUndesiredTags_Location;
                                break;
                        }
                        break;

                    case false:
                        frmBlacklist_Location = aphrodite.Settings.FormSettings.Default.frmBlacklist_Location;
                        frmImageDownloader_Location = aphrodite.Settings.FormSettings.Default.frmImageDownloader_Location;
                        frmLog_Location = aphrodite.Settings.FormSettings.Default.frmLog_Location;
                        frmLog_Size = aphrodite.Settings.FormSettings.Default.frmLog_Size;
                        frmMain_Location = aphrodite.Settings.FormSettings.Default.frmMain_Location;
                        frmPoolDownloader_Location = aphrodite.Settings.FormSettings.Default.frmPoolDownloader_Location;
                        frmPoolWishlist_Location = aphrodite.Settings.FormSettings.Default.frmPoolWishlist_Location;
                        frmRedownloader_Location = aphrodite.Settings.FormSettings.Default.frmRedownloader_Location;
                        frmSettings_Location = aphrodite.Settings.FormSettings.Default.frmSettings_Location;
                        frmTagDownloader_Location = aphrodite.Settings.FormSettings.Default.frmTagDownloader_Location;
                        frmUndesiredTags_Location = aphrodite.Settings.FormSettings.Default.frmUndesiredTags_Location;
                        break;
                }
            }

            public void ForceSave() {
                Program.Log(LogAction.WriteToLog, "Force saving Config_FormSettings settings");

                switch (Program.UseIni) {
                    case true:
                        Program.Ini.Write("frmBlacklist_Location", frmBlacklist_Location, "FormSettings");
                        frmBlacklist_Location_First = frmBlacklist_Location;

                        Program.Ini.Write("frmImageDownloader_Location", frmImageDownloader_Location, "FormSettings");
                        frmImageDownloader_Location_First = frmImageDownloader_Location;

                        Program.Ini.Write("frmLog_Location", frmLog_Location, "FormSettings");
                        frmLog_Location_First = frmLog_Location;

                        Program.Ini.Write("frmLog_Size", frmLog_Size, "FormSettings");
                        frmLog_Size_First = frmLog_Size;

                        Program.Ini.Write("frmMain_Location", frmMain_Location, "FormSettings");
                        frmMain_Location_First = frmMain_Location;

                        Program.Ini.Write("frmPoolDownloader_Location", frmPoolDownloader_Location, "FormSettings");
                        frmPoolDownloader_Location_First = frmPoolDownloader_Location;

                        Program.Ini.Write("frmPoolWishlist_Location", frmPoolWishlist_Location, "FormSettings");
                        frmPoolWishlist_Location_First = frmPoolWishlist_Location;

                        Program.Ini.Write("frmRedownloader_Location", frmRedownloader_Location, "FormSettings");
                        frmRedownloader_Location_First = frmRedownloader_Location;

                        Program.Ini.Write("frmSettings_Location", frmSettings_Location, "FormSettings");
                        frmSettings_Location_First = frmSettings_Location;

                        Program.Ini.Write("frmTagDownloader_Location", frmTagDownloader_Location, "FormSettings");
                        frmTagDownloader_Location_First = frmTagDownloader_Location;

                        Program.Ini.Write("frmUndesiredTags_Location", frmUndesiredTags_Location, "FormSettings");
                        frmUndesiredTags_Location_First = frmUndesiredTags_Location;
                        break;

                    case false: {
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
                            aphrodite.Settings.FormSettings.Default.Save();

                            break;
                        }
                }
            }
        }

        public class Config_General {
            public Config_General(bool SkipLoad = false) {
                Program.Log(LogAction.WriteToLog, "Initializing Config_General");
                switch (!SkipLoad) {
                    case true:
                        Load();
                        break;
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
            public bool CheckForUpdates = false;

            private string saveLocation_First = string.Empty;
            private string Graylist_First = string.Empty;
            private bool saveGraylisted_First = true;
            private bool saveInfo_First = true;
            private bool ignoreFinish_First = false;
            private string Blacklist_First = string.Empty;
            private string undesiredTags_First = string.Empty;
            private bool openAfter_First = false;
            public bool CheckForUpdates_First = false;
            #endregion

            public void Save() {
                Program.Log(LogAction.WriteToLog, "Attempting to save Config_General settings");

                switch (Program.UseIni) {
                    case true:
                        switch (saveLocation != saveLocation_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> saveLocation changed!");
                                Program.Ini.Write("saveLocation", saveLocation, "General");
                                saveLocation_First = saveLocation;
                                break;
                        }
                        switch (Graylist != Graylist_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> Graylist changed!");
                                switch (Graylist.Length) {
                                    case 0:
                                        File.Delete(Program.ApplicationPath + "\\graylist.cfg");
                                        break;

                                    default:
                                        File.WriteAllText(
                                            Program.ApplicationPath + "\\graylist.cfg",
                                            Graylist.Replace(" ", "_").Replace("\r\n", " ")
                                        );
                                        break;
                                }
                                break;
                        }
                        switch (saveGraylisted != saveGraylisted_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> saveGraylisted changed!");
                                Program.Ini.Write("saveGraylisted", saveGraylisted, "General");
                                saveGraylisted_First = saveGraylisted;
                                break;
                        }
                        switch (saveInfo != saveInfo_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> saveInfo changed!");
                                Program.Ini.Write("saveInfo", saveInfo, "General");
                                saveInfo_First = saveInfo;
                                break;
                        }
                        switch (ignoreFinish != ignoreFinish_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> ignoreFinish changed!");
                                Program.Ini.Write("ignoreFinish", ignoreFinish, "General");
                                ignoreFinish_First = ignoreFinish;
                                break;
                        }
                        switch (Blacklist != Blacklist_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> Blacklist changed!");
                                switch (Graylist.Length) {
                                    case 0:
                                        File.Delete(Program.ApplicationPath + "\\blacklist.cfg");
                                        break;

                                    default:
                                        File.WriteAllText(
                                            Program.ApplicationPath + "\\blacklist.cfg",
                                            Blacklist.Replace(" ", "_").Replace("\r\n", " ")
                                        );
                                        break;
                                }
                                break;
                        }
                        switch (undesiredTags != undesiredTags_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> undesiredTags changed!");
                                Program.Ini.Write("undesiredTags", undesiredTags, "General");
                                undesiredTags_First = undesiredTags;
                                break;
                        }
                        switch (openAfter != openAfter_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> openAfter changed!");
                                Program.Ini.Write("openAfter", openAfter, "General");
                                openAfter_First = openAfter;
                                break;
                        }
                        switch (CheckForUpdates != CheckForUpdates_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> CheckForUpdates changed!");
                                Program.Ini.Write("CheckForUpdates", CheckForUpdates, "General");
                                CheckForUpdates_First = CheckForUpdates;
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
                        switch (aphrodite.Settings.General.Default.CheckForUpdates != CheckForUpdates) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "General -> CheckForUpdates changed!");
                                aphrodite.Settings.General.Default.CheckForUpdates = CheckForUpdates;
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
                        switch (Program.Ini.KeyExists("saveGraylisted", "General")) {
                            case true:
                                saveGraylisted = Program.Ini.ReadBool("saveGraylisted", "General");
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
                        switch (Program.Ini.KeyExists("CheckForUpdates", "General")) {
                            case true:
                                CheckForUpdates = Program.Ini.ReadBool("CheckForUpdates", "General");
                                CheckForUpdates_First = CheckForUpdates;
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
                        CheckForUpdates = aphrodite.Settings.General.Default.CheckForUpdates;
                        break;
                }
            }

            public void ForceSave() {
                Program.Log(LogAction.WriteToLog, "Force saving Config_General settings");

                switch (Program.UseIni) {
                    case true:
                        Program.Ini.Write("saveLocation", saveLocation, "General");
                        saveLocation_First = saveLocation;

                        Program.Ini.Write("saveGraylisted", saveGraylisted, "General");
                        saveGraylisted_First = saveGraylisted;

                        Program.Ini.Write("saveInfo", saveInfo, "General");
                        saveInfo_First = saveInfo;

                        Program.Ini.Write("ignoreFinish", ignoreFinish, "General");
                        ignoreFinish_First = ignoreFinish;

                        Program.Ini.Write("undesiredTags", undesiredTags, "General");
                        undesiredTags_First = undesiredTags;

                        Program.Ini.Write("openAfter", openAfter, "General");
                        openAfter_First = openAfter;

                        Program.Ini.Write("CheckForUpdates", CheckForUpdates, "General");
                        CheckForUpdates_First = CheckForUpdates;

                        switch (Graylist.Length) {
                            case 0:
                                File.Delete(Program.ApplicationPath + "\\graylist.cfg");
                                break;

                            default:
                                File.WriteAllText(
                                    Program.ApplicationPath + "\\graylist.cfg",
                                    Graylist.Replace(" ", "\r\n")
                                );
                                break;
                        }

                        switch (Graylist.Length) {
                            case 0:
                                File.Delete(Program.ApplicationPath + "\\blacklist.cfg");
                                break;

                            default:
                                File.WriteAllText(
                                    Program.ApplicationPath + "\\blacklist.cfg",
                                    Blacklist.Replace(" ", "\r\n")
                                );
                                break;
                        }

                        break;

                    case false:
                        aphrodite.Settings.General.Default.saveLocation = saveLocation;
                        aphrodite.Settings.General.Default.saveGraylisted = saveGraylisted;
                        aphrodite.Settings.General.Default.saveInfo = saveInfo;
                        aphrodite.Settings.General.Default.ignoreFinish = ignoreFinish;
                        aphrodite.Settings.General.Default.undesiredTags = undesiredTags;
                        aphrodite.Settings.General.Default.openAfter = openAfter;
                        aphrodite.Settings.General.Default.CheckForUpdates = CheckForUpdates;

                        aphrodite.Settings.General.Default.Graylist = Graylist;
                        aphrodite.Settings.General.Default.Blacklist = Blacklist;

                        aphrodite.Settings.General.Default.Save();

                        break;
                }
            }
        }

        public class Config_Tags {
            public Config_Tags(bool SkipLoad = false) {
                Program.Log(LogAction.WriteToLog, "Initializing Config_Tags");
                switch (!SkipLoad) {
                    case true:
                        Load();
                        break;
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
            public bool downloadBlacklisted = false;
            public bool DownloadNewestToOldest = false;
            public int FavoriteCount = 0;
            public bool FavoriteCountAsTag = false;

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
            private bool downloadBlacklisted_First = false;
            private bool DownloadNewestToOldest_First = false;
            public int FavoriteCount_First = 0;
            public bool FavoriteCountAsTag_First = false;
            #endregion

            public void Save() {
                Program.Log(LogAction.WriteToLog, "Attempting to save Config_Tags settings");

                switch (Program.UseIni) {
                    case true:
                        switch (Safe != Safe_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> Safe changed!");
                                Program.Ini.Write("Safe", Safe, "Tags");
                                Safe_First = Safe;
                                break;
                        }
                        switch (Questionable != Questionable_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> Questionable changed!");
                                Program.Ini.Write("Questionable", Questionable, "Tags");
                                Questionable_First = Questionable;
                                break;
                        }
                        switch (Explicit != Explicit_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> Explicit changed!");
                                Program.Ini.Write("Explicit", Explicit, "Tags");
                                Explicit_First = Explicit;
                                break;
                        }
                        switch (separateRatings != separateRatings_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> separateRatings changed!");
                                Program.Ini.Write("separateRatings", separateRatings, "Tags");
                                separateRatings_First = separateRatings;
                                break;
                        }
                        switch (separateNonImages != separateNonImages_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> separateNonImages changed!");
                                Program.Ini.Write("separateNonImages", separateNonImages, "Tags");
                                separateNonImages_First = separateNonImages;
                                break;
                        }
                        switch (enableScoreMin != enableScoreMin_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> enableScoreMin changed!");
                                Program.Ini.Write("enableScoreMin", enableScoreMin, "Tags");
                                enableScoreMin_First = enableScoreMin;
                                break;
                        }
                        switch (scoreAsTag != scoreAsTag_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> scoreAsTag changed!");
                                Program.Ini.Write("scoreAsTag", scoreAsTag, "Tags");
                                scoreAsTag_First = scoreAsTag;
                                break;
                        }
                        switch (scoreMin != scoreMin_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> scoreMin changed!");
                                Program.Ini.Write("scoreMin", scoreMin, "Tags");
                                scoreMin_First = scoreMin;
                                break;
                        }
                        switch (imageLimit != imageLimit_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> imageLimit changed!");
                                Program.Ini.Write("imageLimit", imageLimit, "Tags");
                                imageLimit_First = imageLimit;
                                break;
                        }
                        switch (pageLimit != pageLimit_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> pageLimit changed!");
                                Program.Ini.Write("pageLimit", pageLimit, "Tags");
                                pageLimit_First = pageLimit;
                                break;
                        }
                        switch (fileNameSchema != fileNameSchema_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> fileNameSchema changed!");
                                Program.Ini.Write("fileNameSchema", fileNameSchema, "Tags");
                                fileNameSchema_First = fileNameSchema;
                                break;
                        }
                        switch (downloadBlacklisted != downloadBlacklisted_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> downloadBlacklisted changed!");
                                Program.Ini.Write("downloadBlacklisted", downloadBlacklisted, "Tags");
                                downloadBlacklisted_First = downloadBlacklisted;
                                break;
                        }
                        switch (DownloadNewestToOldest != DownloadNewestToOldest_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> DownloadNewestToOldest changed!");
                                Program.Ini.Write("DownloadNewestToOldest", DownloadNewestToOldest, "Tags");
                                DownloadNewestToOldest_First = DownloadNewestToOldest;
                                break;
                        }
                        switch (FavoriteCount != FavoriteCount_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> FavoriteCount changed!");
                                Program.Ini.Write("FavoriteCount", FavoriteCount, "Tags");
                                FavoriteCount_First = FavoriteCount;
                                break;
                        }
                        switch (FavoriteCountAsTag != FavoriteCountAsTag_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> FavoriteCountAsTag changed!");
                                Program.Ini.Write("FavoriteCountAsTag", FavoriteCountAsTag, "Tags");
                                FavoriteCountAsTag_First = FavoriteCountAsTag;
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
                        switch (aphrodite.Settings.Tags.Default.downloadBlacklisted != downloadBlacklisted) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> downloadBlacklisted changed!");
                                aphrodite.Settings.Tags.Default.downloadBlacklisted = downloadBlacklisted;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.DownloadNewestToOldest != DownloadNewestToOldest) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> DownloadNewestToOldest changed!");
                                aphrodite.Settings.Tags.Default.DownloadNewestToOldest = DownloadNewestToOldest;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.FavoriteCount != FavoriteCount) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> FavoriteCount changed!");
                                aphrodite.Settings.Tags.Default.FavoriteCount = FavoriteCount;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Tags.Default.FavoriteCountAsTag != FavoriteCountAsTag) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Tags -> FavoriteCountAsTag changed!");
                                aphrodite.Settings.Tags.Default.FavoriteCountAsTag = FavoriteCountAsTag;
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
                        switch (Program.Ini.KeyExists("Safe", "Tags")) {
                            case true:
                                Safe = Program.Ini.ReadBool("Safe", "Tags");
                                Safe_First = Safe;
                                break;
                        }
                        switch (Program.Ini.KeyExists("Questionable", "Tags")) {
                            case true:
                                Questionable = Program.Ini.ReadBool("Questionable", "Tags");
                                Questionable_First = Questionable;
                                break;
                        }
                        switch (Program.Ini.KeyExists("Explicit", "Tags")) {
                            case true:
                                Explicit = Program.Ini.ReadBool("Explicit", "Tags");
                                Explicit_First = Explicit;
                                break;
                        }
                        switch (Program.Ini.KeyExists("separateRatings", "Tags")) {
                            case true:
                                separateRatings = Program.Ini.ReadBool("separateRatings", "Tags");
                                separateRatings_First = separateRatings;
                                break;
                        }
                        switch (Program.Ini.KeyExists("separateNonImages", "Tags")) {
                            case true:
                                separateNonImages = Program.Ini.ReadBool("separateNonImages", "Tags");
                                separateNonImages_First = separateNonImages;
                                break;
                        }
                        switch (Program.Ini.KeyExists("enableScoreMin", "Tags")) {
                            case true:
                                enableScoreMin = Program.Ini.ReadBool("enableScoreMin", "Tags");
                                enableScoreMin_First = enableScoreMin;
                                break;
                        }
                        switch (Program.Ini.KeyExists("scoreAsTag", "Tags")) {
                            case true:
                                scoreAsTag = Program.Ini.ReadBool("scoreAsTag", "Tags");
                                scoreAsTag_First = scoreAsTag;
                                break;
                        }
                        switch (Program.Ini.KeyExists("scoreMin", "Tags")) {
                            case true:
                                scoreMin = Program.Ini.ReadInt("scoreMin", "Tags");
                                scoreMin_First = scoreMin;
                                break;
                        }
                        switch (Program.Ini.KeyExists("imageLimit", "Tags")) {
                            case true:
                                imageLimit = Program.Ini.ReadInt("imageLimit", "Tags");
                                imageLimit_First = imageLimit;
                                break;
                        }
                        switch (Program.Ini.KeyExists("pageLimit", "Tags")) {
                            case true:
                                pageLimit = Program.Ini.ReadInt("pageLimit", "Tags");
                                pageLimit_First = pageLimit;
                                break;
                        }
                        switch (Program.Ini.KeyExists("fileNameSchema", "Tags")) {
                            case true:
                                fileNameSchema = apiTools.ReplaceIllegalCharacters(Program.Ini.ReadString("fileNameSchema", "Tags"));
                                fileNameSchema_First = fileNameSchema;
                                break;
                        }
                        switch (Program.Ini.KeyExists("downloadBlacklisted", "Tags")) {
                            case true:
                                downloadBlacklisted = Program.Ini.ReadBool("downloadBlacklisted", "Tags");
                                downloadBlacklisted_First = downloadBlacklisted;
                                break;
                        }
                        switch (Program.Ini.KeyExists("DownloadNewestToOldest", "Tags")) {
                            case true:
                                DownloadNewestToOldest = Program.Ini.ReadBool("DownloadNewestToOldest", "Tags");
                                DownloadNewestToOldest_First = DownloadNewestToOldest;
                                break;
                        }
                        switch (Program.Ini.KeyExists("FavoriteCount", "Tags")) {
                            case true:
                                FavoriteCount = Program.Ini.ReadInt("FavoriteCount", "Tags");
                                FavoriteCount_First = FavoriteCount;
                                break;
                        }
                        switch (Program.Ini.KeyExists("FavoriteCountAsTag", "Tags")) {
                            case true:
                                FavoriteCountAsTag = Program.Ini.ReadBool("FavoriteCountAsTag", "Tags");
                                FavoriteCountAsTag_First = FavoriteCountAsTag;
                                break;
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
                        fileNameSchema = apiTools.ReplaceIllegalCharacters(aphrodite.Settings.Tags.Default.fileNameSchema);
                        downloadBlacklisted = aphrodite.Settings.Tags.Default.downloadBlacklisted;
                        DownloadNewestToOldest = aphrodite.Settings.Tags.Default.DownloadNewestToOldest;
                        FavoriteCount = aphrodite.Settings.Tags.Default.FavoriteCount;
                        FavoriteCountAsTag = aphrodite.Settings.Tags.Default.FavoriteCountAsTag;
                        break;
                }
            }

            public void ForceSave() {
                Program.Log(LogAction.WriteToLog, "Force saving Config_Tags settings");
                switch (Program.UseIni) {
                    case true:
                        Program.Ini.Write("Safe", Safe, "Tags");
                        Safe_First = Safe;

                        Program.Ini.Write("Questionable", Questionable, "Tags");
                        Questionable_First = Questionable;

                        Program.Ini.Write("Explicit", Explicit, "Tags");
                        Explicit_First = Explicit;

                        Program.Ini.Write("separateRatings", separateRatings, "Tags");
                        separateRatings_First = separateRatings;

                        Program.Ini.Write("separateNonImages", separateNonImages, "Tags");
                        separateNonImages_First = separateNonImages;

                        Program.Ini.Write("enableScoreMin", enableScoreMin, "Tags");
                        enableScoreMin_First = enableScoreMin;

                        Program.Ini.Write("scoreAsTag", scoreAsTag, "Tags");
                        scoreAsTag_First = scoreAsTag;

                        Program.Ini.Write("scoreMin", scoreMin, "Tags");
                        scoreMin_First = scoreMin;

                        Program.Ini.Write("imageLimit", imageLimit, "Tags");
                        imageLimit_First = imageLimit;

                        Program.Ini.Write("pageLimit", pageLimit, "Tags");
                        pageLimit_First = pageLimit;

                        Program.Ini.Write("fileNameSchema", fileNameSchema, "Tags");
                        fileNameSchema_First = fileNameSchema;

                        Program.Ini.Write("downloadBlacklisted", downloadBlacklisted, "Tags");
                        downloadBlacklisted_First = downloadBlacklisted;

                        Program.Ini.Write("DownloadNewestToOldest", DownloadNewestToOldest, "Tags");
                        DownloadNewestToOldest_First = DownloadNewestToOldest;

                        Program.Ini.Write("FavoriteCount", FavoriteCount, "Tags");
                        FavoriteCount_First = FavoriteCount;

                        Program.Ini.Write("FavoriteCountAsTag", FavoriteCountAsTag, "Tags");
                        FavoriteCountAsTag_First = FavoriteCountAsTag;
                        break;

                    case false:
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
                        break;
                }
            }
        }

        public class Config_Pools {
            public static readonly string WishlistFile = Program.ApplicationPath + "\\PoolWishlist.cfg";

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

            private string[] NamesArray;
            private string[] URLsArray;

            private string NamesString;
            private string URLsString;

            private string OutputBuffer = string.Empty;
            private string[] WishlistBuffer;
            #endregion

            public Config_Pools() {
                Program.Log(LogAction.WriteToLog, "Initializing Config_Pools");
                Load();
            }

            public static void AppendToWishlist(string Name, string URL) {
                Program.Log(LogAction.WriteToLog, "Appending new Wishlist pool");

                if (Config.Settings.Pools.addWishlistSilent) {
                    if (Program.UseIni) {
                        if (File.Exists(WishlistFile)) {
                            string WishlistBuffer = File.ReadAllText(WishlistFile);
                            if (!WishlistBuffer.Contains(URL)) {
                                File.AppendAllText(WishlistFile, Environment.NewLine + URL + "|" + Name.Replace("|", "_"));
                                System.Media.SystemSounds.Asterisk.Play();
                            }
                            else {
                                System.Media.SystemSounds.Exclamation.Play();
                            }
                        }
                        else {
                            File.Create(WishlistFile).Dispose();
                            File.AppendAllText(WishlistFile, URL + "|" + Name);
                            System.Media.SystemSounds.Asterisk.Play();
                        }
                    }
                    else {
                        if (!Config.Settings.Pools.wishlist.Contains(URL)) {
                            if (!string.IsNullOrWhiteSpace(aphrodite.Settings.Pools.Default.wishlistNames) && !string.IsNullOrWhiteSpace(Config.Settings.Pools.wishlist)) {
                                Config.Settings.Pools.wishlist += "|" + URL;
                                Config.Settings.Pools.wishlistNames += "|" + Name;
                            }
                            else {
                                Config.Settings.Pools.wishlist = URL;
                                Config.Settings.Pools.wishlistNames = Name;
                            }
                            Config.Settings.Pools.Save();
                            System.Media.SystemSounds.Asterisk.Play();
                        }
                        else {
                            System.Media.SystemSounds.Exclamation.Play();
                        }
                    }
                }
                else {
                    using (frmPoolWishlist WishList = new frmPoolWishlist(true, URL, Name)) {
                        WishList.ShowDialog();
                    }
                    return;
                }
            }
            public static void SaveWishlist(List<string> Names, List<string> URLs) {
                Program.Log(LogAction.WriteToLog, "Updating pool wishlist");

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

                            case false:
                                Settings.Pools.wishlist = string.Join("|", URLs);
                                Settings.Pools.wishlistNames = string.Join("|", Names);
                                Settings.Save(ConfigType.Pools);
                                break;
                        }
                        break;
                }
            }

            public void Save() {
                Program.Log(LogAction.WriteToLog, "Attempting to save Config_Pools settings");

                switch (Program.UseIni) {
                    case true:
                        switch (mergeGraylisted != mergeGraylisted_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Pools -> mergeGraylisted changed!");
                                Program.Ini.Write("mergeBlacklisted", mergeGraylisted, "Pools");
                                mergeGraylisted_First = mergeGraylisted;
                                break;
                        }
                        switch (addWishlistSilent != addWishlistSilent_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Pools -> addWishlistSilent changed!");
                                Program.Ini.Write("addWishlistSilent", addWishlistSilent, "Pools");
                                addWishlistSilent_First = addWishlistSilent;
                                break;
                        }
                        switch (fileNameSchema != fileNameSchema_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Pools -> fileNameSchema changed!");
                                Program.Ini.Write("fileNameSchema", fileNameSchema, "Pools");
                                fileNameSchema_First = fileNameSchema;
                                break;
                        }
                        switch (downloadBlacklisted != downloadBlacklisted_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Pools -> downloadBlacklisted changed!");
                                Program.Ini.Write("downloadBlacklisted", downloadBlacklisted, "Pools");
                                downloadBlacklisted_First = downloadBlacklisted;
                                break;
                        }
                        switch (mergeBlacklisted != mergeBlacklisted_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Pools -> mergeBlacklisted changed!");
                                Program.Ini.Write("mergeBlacklisted", mergeBlacklisted, "Pools");
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
                        switch (aphrodite.Settings.Pools.Default.wishlist != wishlist) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Pools -> wishlist changed!");
                                aphrodite.Settings.Pools.Default.wishlist = wishlist;
                                Save = true;
                                break;
                        }
                        switch (aphrodite.Settings.Pools.Default.wishlistNames != wishlistNames) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Pools -> wishlistNames changed!");
                                aphrodite.Settings.Pools.Default.wishlistNames = wishlistNames;
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
                                fileNameSchema = apiTools.ReplaceIllegalCharacters(Program.Ini.ReadString("fileNameSchema", "Pools"));
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
                                        wishlist += List[i].Split('|')[0] + "|";
                                        wishlistNames += List[i].Split('|')[1] + "|";
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
                        fileNameSchema = apiTools.ReplaceIllegalCharacters(aphrodite.Settings.Pools.Default.fileNameSchema);
                        downloadBlacklisted = aphrodite.Settings.Pools.Default.downloadBlacklisted;
                        mergeBlacklisted = aphrodite.Settings.Pools.Default.mergeBlacklisted;
                        break;
                }
            }

            public void ForceSave() {

                Program.Log(LogAction.WriteToLog, "Force saving Config_Pools settings");
                switch (Program.UseIni) {
                    case true:
                        Program.Ini.Write("mergeBlacklisted", mergeGraylisted, "Pools");
                        mergeGraylisted_First = mergeGraylisted;

                        Program.Ini.Write("addWishlistSilent", addWishlistSilent, "Pools");
                        addWishlistSilent_First = addWishlistSilent;

                        Program.Ini.Write("fileNameSchema", fileNameSchema, "Pools");
                        fileNameSchema_First = fileNameSchema;

                        Program.Ini.Write("downloadBlacklisted", downloadBlacklisted, "Pools");
                        downloadBlacklisted_First = downloadBlacklisted;

                        Program.Ini.Write("mergeBlacklisted", mergeBlacklisted, "Pools");
                        mergeBlacklisted_First = mergeBlacklisted;

                        URLsArray = wishlist.Split('|');
                        NamesArray = wishlistNames.Split('|');
                        for (int i = 0; i < URLsArray.Length; i++) {
                            OutputBuffer += URLsArray[i] + "|" + NamesArray[i] + "\r\n";
                        }

                        OutputBuffer = OutputBuffer.Trim('\n').Trim('\r');

                        File.WriteAllText(WishlistFile, OutputBuffer);

                        URLsArray = new string[] { };
                        NamesArray = new string[] { };
                        OutputBuffer = string.Empty;

                        break;

                    case false:
                        aphrodite.Settings.Pools.Default.mergeGraylisted = mergeGraylisted;
                        aphrodite.Settings.Pools.Default.wishlist = wishlist;
                        aphrodite.Settings.Pools.Default.wishlistNames = wishlistNames;
                        aphrodite.Settings.Pools.Default.addWishlistSilent = addWishlistSilent;
                        aphrodite.Settings.Pools.Default.fileNameSchema = fileNameSchema;
                        aphrodite.Settings.Pools.Default.downloadBlacklisted = downloadBlacklisted;
                        aphrodite.Settings.Pools.Default.mergeBlacklisted = mergeBlacklisted;

                        WishlistBuffer = File.ReadAllLines(WishlistFile);
                        for (int i = 0; i < WishlistBuffer.Length; i++) {
                            URLsString += WishlistBuffer[i].Split('|')[0] + "\n";
                            NamesString += WishlistBuffer[i].Split('|')[1] + "\n";
                        }

                        aphrodite.Settings.Pools.Default.wishlist = URLsString.Replace("\n", "\r\n");
                        aphrodite.Settings.Pools.Default.wishlistNames = NamesString.Replace("\n", "\r\n");

                        URLsString = string.Empty;
                        NamesString = string.Empty;
                        WishlistBuffer = new string[] { };

                        aphrodite.Settings.Pools.Default.Save();
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
                                Program.Ini.Write("separateRatings", separateRatings, "Images");
                                separateRatings_First = separateRatings;
                                break;
                        }
                        switch (separateGraylisted != separateGraylisted_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> separateBlacklisted changed!");
                                Program.Ini.Write("separateBlacklisted", separateGraylisted, "Images");
                                separateGraylisted_First = separateGraylisted;
                                break;
                        }
                        switch (useForm != useForm_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> useForm changed!");
                                Program.Ini.Write("useForm", useForm, "Images");
                                useForm_First = useForm;
                                break;
                        }
                        switch (separateArtists != separateArtists_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> separateArtists changed!");
                                Program.Ini.Write("separateArtists", separateArtists, "Images");
                                separateArtists_First = separateArtists;
                                break;
                        }
                        switch (fileNameSchema != fileNameSchema_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> fileNameSchema changed!");
                                Program.Ini.Write("fileNameSchema", fileNameSchema, "Images");
                                fileNameSchema_First = fileNameSchema;
                                break;
                        }
                        switch (separateNonImages != separateNonImages_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> separateNonImages changed!");
                                Program.Ini.Write("separateNonImages", separateNonImages, "Images");
                                separateNonImages_First = separateNonImages;
                                break;
                        }
                        switch (separateBlacklisted != separateBlacklisted_First) {
                            case true:
                                Program.Log(LogAction.WriteToLog, "Images -> separateBlacklisted changed!");
                                Program.Ini.Write("separateBlacklisted", separateBlacklisted, "Images");
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
                                fileNameSchema = apiTools.ReplaceIllegalCharacters(Program.Ini.ReadString("fileNameSchema", "Images"));
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
                        fileNameSchema = apiTools.ReplaceIllegalCharacters(aphrodite.Settings.Images.Default.fileNameSchema);
                        separateNonImages = aphrodite.Settings.Images.Default.separateNonImages;
                        separateBlacklisted = aphrodite.Settings.Images.Default.separateBlacklisted;
                        break;
                }
            }

            public void ForceSave() {
                Program.Log(LogAction.WriteToLog, "Force saving Config_Images settings");
                switch (Program.UseIni) {
                    case true:
                        Program.Ini.Write("separateRatings", separateRatings, "Images");
                        separateRatings_First = separateRatings;
                        Program.Ini.Write("separateBlacklisted", separateGraylisted, "Images");
                        separateGraylisted_First = separateGraylisted;
                        Program.Ini.Write("useForm", useForm, "Images");
                        useForm_First = useForm;
                        Program.Ini.Write("separateArtists", separateArtists, "Images");
                        separateArtists_First = separateArtists;
                        Program.Ini.Write("fileNameSchema", fileNameSchema, "Images");
                        fileNameSchema_First = fileNameSchema;
                        Program.Ini.Write("separateNonImages", separateNonImages, "Images");
                        separateNonImages_First = separateNonImages;
                        Program.Ini.Write("separateBlacklisted", separateBlacklisted, "Images");
                        separateBlacklisted_First = separateBlacklisted;
                        break;

                    case false:
                        aphrodite.Settings.Images.Default.separateRatings = separateRatings;
                        aphrodite.Settings.Images.Default.separateGraylisted = separateGraylisted;
                        aphrodite.Settings.Images.Default.useForm = useForm;
                        aphrodite.Settings.Images.Default.separateArtists = separateArtists;
                        aphrodite.Settings.Images.Default.fileNameSchema = fileNameSchema;
                        aphrodite.Settings.Images.Default.separateNonImages = separateNonImages;
                        aphrodite.Settings.Images.Default.separateBlacklisted = separateBlacklisted;
                        aphrodite.Settings.Images.Default.Save();

                        break;
                }
            }
        }
    }

    class SystemRegistry {

        public enum KeyName {
            Tags,
            Pools,
            PoolWl,
            BothPools,
            Images
        }

        private static volatile RegistryKey TagsKey;
        private static volatile RegistryKey PoolsKey;
        private static volatile RegistryKey PoolWishlistKey;
        private static volatile RegistryKey ImagesKey;
        public static readonly string ProtocolValue = "\"" + Environment.CurrentDirectory + "\\" + System.AppDomain.CurrentDomain.FriendlyName + "\" \"%1\"";

        public static bool CheckRegistryKey(KeyName Name) {
            switch (Name) {
                case KeyName.Tags:
                    TagsKey = Registry.ClassesRoot.OpenSubKey("tags\\shell\\open\\command", false);
                    if (Registry.ClassesRoot.GetValue("tags", "URL Protocol") != null && TagsKey != null && TagsKey.GetValue("").ToString() == ProtocolValue) {
                        return true;
                    }
                    else {
                        return false;
                    }

                case KeyName.Pools:
                    PoolsKey = Registry.ClassesRoot.OpenSubKey("pools\\shell\\open\\command", false);
                    if (Registry.ClassesRoot.GetValue("pools", "URL Protocol") != null && PoolsKey != null && PoolsKey.GetValue("").ToString() == ProtocolValue) {
                        return true;
                    }
                    else {
                        return false;
                    }

                case KeyName.PoolWl:
                    PoolWishlistKey = Registry.ClassesRoot.OpenSubKey("poolwl\\shell\\open\\command", false);
                    if (Registry.ClassesRoot.GetValue("poolwl", "URL Protocol") != null && PoolWishlistKey != null && PoolWishlistKey.GetValue("").ToString() == ProtocolValue) {
                        return true;
                    }
                    else {
                        return false;
                    }

                case KeyName.Images:
                    ImagesKey = Registry.ClassesRoot.OpenSubKey("images\\shell\\open\\command", false);
                    if (Registry.ClassesRoot.GetValue("images", "URL Protocol") != null && ImagesKey != null && ImagesKey.GetValue("").ToString() == ProtocolValue) {
                        return true;
                    }
                    else {
                        return false;
                    }

                default: return false;
            }
        }

        public static bool SetRegistryKey(KeyName Name) {
            string ProtocolIdentifier = null;

            switch (Name) {
                case KeyName.Tags:
                    ProtocolIdentifier = "tags";
                    break;

                case KeyName.Pools:
                    ProtocolIdentifier = "pools";
                    break;

                case KeyName.PoolWl:
                    ProtocolIdentifier = "poolwl";
                    break;

                case KeyName.BothPools:
                    if (SetRegistryKey(KeyName.Pools) &&
                        SetRegistryKey(KeyName.PoolWl)) {
                        return true;
                    }
                    else { return false; }

                case KeyName.Images:
                    ProtocolIdentifier = "images";
                    break;

                default: return false;

            }

            if (ProtocolIdentifier != null && Program.IsAdmin) {
                Registry.ClassesRoot.CreateSubKey(ProtocolIdentifier);
                RegistryKey setIdentifier = Registry.ClassesRoot.OpenSubKey(ProtocolIdentifier, true);
                setIdentifier.SetValue("URL Protocol", "");

                Registry.ClassesRoot.CreateSubKey(ProtocolIdentifier + "\\shell");
                Registry.ClassesRoot.CreateSubKey(ProtocolIdentifier + "\\shell\\open");
                Registry.ClassesRoot.CreateSubKey(ProtocolIdentifier + "\\shell\\open\\command");
                RegistryKey setProtocol = Registry.ClassesRoot.OpenSubKey(ProtocolIdentifier + "\\shell\\open\\command", true);
                setProtocol.SetValue("", "\"" + Program.FullApplicationPath + "\" \"%1\"");

                Registry.ClassesRoot.CreateSubKey(ProtocolIdentifier + "\\DefaultIcon");
                RegistryKey setIcon = Registry.ClassesRoot.OpenSubKey(ProtocolIdentifier + "\\DefaultIcon", true);
                setIcon.SetValue("", "\"" + Program.FullApplicationPath + "\"");
                return true;
            }
            else {
                return false;
            }
        }
    }

    class IniFile {

        /// <summary>
        /// The full path of the Ini File (Generally, in the same folder as the executable)
        /// </summary>
        private string IniPath;
        /// <summary>
        /// The name of the executing file.
        /// </summary>
        private string ExecutableName = Assembly.GetExecutingAssembly().GetName().Name;

        public IniFile(string NewIniPath = null) {
            ChangeIniPath(NewIniPath);
        }

        public void ChangeIniPath(string NewIniPath = null) {
            IniPath = new FileInfo(NewIniPath ?? ExecutableName + ".ini").FullName.ToString();
        }

        public string ReadString(string Key, string Section = null) {
            StringBuilder RetVal = new StringBuilder(65535);
            NativeMethods.GetPrivateProfileString(Section ?? ExecutableName, Key, "", RetVal, 65535, IniPath);
            return RetVal.ToString();
        }
        public bool ReadBool(string Key, string Section = null) {
            StringBuilder RetVal = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(Section ?? ExecutableName, Key.ToLower(), "", RetVal, 255, IniPath);
            switch (RetVal.ToString().ToLower()) {
                case "true":
                    return true;
                default:
                    return false;
            }
        }
        public int ReadInt(string Key, string Section = null) {
            StringBuilder RetVal = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(Section ?? ExecutableName, Key.ToLower(), "", RetVal, 255, IniPath);
            int RetInt;
            int.TryParse(RetVal.ToString(), out RetInt);
            return RetInt;
        }
        public decimal ReadDecimal(string Key, string Section = null) {
            StringBuilder RetVal = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(Section ?? ExecutableName, Key.ToLower(), "", RetVal, 255, IniPath);
            decimal RetDec;
            if (decimal.TryParse(RetVal.ToString(), out RetDec)) {
                return RetDec;
            }
            else {
                return -1;
            }
        }
        public Point ReadPoint(string Key, string Section = null) {
            StringBuilder RetVal = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(Section, Key, "", RetVal, 255, IniPath);
            string[] Value = RetVal.ToString().Split(',');
            switch (Value.Length) {
                case 2:
                    int OutX;
                    int OutY;
                    int.TryParse(Value[0], out OutX);
                    int.TryParse(Value[1], out OutY);
                    return new Point(OutX, OutY);

                default:
                    return new Point(0, 0);
            }
        }
        public Size ReadSize(string Key, string Section = null) {
            StringBuilder RetVal = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(Section, Key, "", RetVal, 255, IniPath);
            string[] Value = RetVal.ToString().Split(',');
            switch (Value.Length) {
                case 2:
                    int OutW;
                    int OutH;
                    int.TryParse(Value[0], out OutW);
                    int.TryParse(Value[1], out OutH);
                    return new Size(OutW, OutH);

                default:
                    return new Size(0, 0);
            }
        }

        public void Write(string Key, string Value, string Section = null) {
            NativeMethods.WritePrivateProfileString(Section ?? ExecutableName, Key, Value, IniPath);
        }
        public void Write(string Key, bool Value, string Section = null) {
            NativeMethods.WritePrivateProfileString(Section ?? ExecutableName, Key, Value ? "True" : "False", IniPath);
        }
        public void Write(string Key, int Value, string Section = null) {
            NativeMethods.WritePrivateProfileString(Section ?? ExecutableName, Key, Value.ToString(), IniPath);
        }
        public void Write(string Key, decimal Value, string Section = null) {
            NativeMethods.WritePrivateProfileString(Section ?? ExecutableName, Key, Value.ToString(), IniPath);
        }
        public void Write(string Key, Point Value, string Section = null) {
            NativeMethods.WritePrivateProfileString(Section, Key, Value.X + "," + Value.Y, IniPath);
        }
        public void Write(string Key, Size Value, string Section = null) {
            NativeMethods.WritePrivateProfileString(Section, Key, Value.Width + "," + Value.Height, IniPath);
        }

        public void DeleteKey(string Key, string Section = null) {
            //Write(Key, null, Section ?? ExecutableName);
            NativeMethods.WritePrivateProfileString(Section ?? ExecutableName, Key, null, IniPath);
        }
        public void DeleteSection(string Section = null) {
            //Write(null, null, Section ?? ExecutableName);
            NativeMethods.WritePrivateProfileString(Section ?? ExecutableName, null, null, IniPath);
        }

        public bool KeyExists(string Key, string Section = null) {
            return ReadString(Key, Section).Length > 0;
        }
    }

}