using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace aphrodite {
    // theory: read the registry key for the protocol to determine location of aphridite.exe,
    // if aphrodite.exe is this one, disable the button, otherwise enable it to move it to the
    // currently running version's directory.
    public partial class frmSettings : Form {
        #region Variables
        public bool SwitchTab = false;   // is the form being changed from the userscript?
        public int Tab = 0;              // Used to determine the tab that will be selected on boot.

        public bool InstallProtocol = false;       // is installprotocol triggered on startup?

        public bool NoProtocols = true;             // Are there no protocols installed?
        public bool TagsProtocol = false;           // is the tags protocol installed?
        public bool PoolsProtocol = false;          // is the pools protocol installed?
        public bool PoolsWishlistProtocol = false;  // is the poolwl protocol installed?
        public bool ImagesProtocol = false;         // is the images protocol installed?
        #endregion

        public frmSettings() {
            InitializeComponent();
            txtSaveTo.TextHint = Environment.CurrentDirectory;
        }
        private void frmSettings_Load(object sender, EventArgs e) {
            loadSettings();
            if (Program.UseIni) {
                tbMain.TabPages.Remove(tabProtocol);
                tbMain.TabPages.Remove(tabPortable);
                tbMain.TabPages.Remove(tabImportExport);
            }
            else {
                checkProtocols();
            }


            if (SwitchTab) {
                switch (Tab) {
                    case 1:
                        tbMain.SelectedTab = tabTags;
                        break;
                    case 2:
                        tbMain.SelectedTab = tabPools;
                        break;
                    case 3:
                        tbMain.SelectedTab = tabImages;
                        break;
                    case 4:
                        tbMain.SelectedTab = tabProtocol;
                        break;
                    case 5:
                        tbMain.SelectedTab = tabSchemas;
                        break;
                    case 6:
                        tbMain.SelectedTab = tabImportExport;
                        break;
                    case 7:
                        tbMain.SelectedTab = tabPortable;
                        break;
                    default:
                        tbMain.SelectedTab = tabGeneral;
                        break;
                }
            }

            if (TagsProtocol == true) {
                btnProtocolInstallTags.Enabled = false;
                btnProtocolInstallTags.Text = "tags protocol installed";
            }
            else {
                if (!Program.IsAdmin) {
                    btnProtocolInstallTags.ShowUACShield = true;
                }
            }

            if (PoolsProtocol == true && PoolsWishlistProtocol == true) {
                btnProtocolInstallPools.Enabled = false;
                btnProtocolInstallPools.Text = "pools protocols installed";
            }
            else {
                if (!Program.IsAdmin) {
                    btnProtocolInstallPools.ShowUACShield = true;
                }
            }

            if (ImagesProtocol == true) {
                btnProtocolInstallImages.Enabled = false;
                btnProtocolInstallImages.Text = "images protocol installed";
            }
            else {
                if (!Program.IsAdmin) {
                    btnProtocolInstallImages.ShowUACShield = true;
                }
            }

            if (InstallProtocol) {
                tbMain.SelectedTab = tabProtocol;
            }
        }

        private void saveSettings() {
          // General
            switch (string.IsNullOrWhiteSpace(txtSaveTo.Text)) {
                case true:
                    Config.Settings.General.saveLocation = txtSaveTo.Text;
                    break;
                    
                case false:
                    Config.Settings.General.saveLocation = Environment.CurrentDirectory;
                    break;
            }
            Config.Settings.General.saveInfo = chkSaveInfoFiles.Checked;
            Config.Settings.General.saveGraylisted = chkSaveGraylistedImages.Checked;
            Config.Settings.General.ignoreFinish = chkIgnoreFinish.Checked;
            Config.Settings.General.openAfter = chkOpenAfterDownload.Checked;
            Config.Settings.Initialization.SkipArgumentCheck = chkSkipArgumentCheck.Checked;
            Config.Settings.Initialization.AutoDownloadWithArguments = chkAutoDownloadWithArguments.Checked;
            //Config.Settings.General.saveMetadata = chkSaveMetadata.Checked;
            //Config.Settings.General.saveArtistMetadata = chkSaveArtistMetadata.Checked;
            //Config.Settings.General.saveTagMetadata = chkSaveTagMetadata.Checked;

          // Tags
            switch (string.IsNullOrWhiteSpace(txtTagSchema.Text)) {
                case true:
                    Config.Settings.Tags.fileNameSchema = "%md5%";
                    break;

                case false:
                    Config.Settings.Tags.fileNameSchema = apiTools.ReplaceIllegalCharacters(txtTagSchema.Text.ToLower());
                    break;
            }
            Config.Settings.Tags.Explicit = chkTagsExplicit.Checked;
            Config.Settings.Tags.Questionable = chkTagsQuestionable.Checked;
            Config.Settings.Tags.Safe = chkTagsSafe.Checked;
            Config.Settings.Tags.separateRatings = chkTagsSeparateRatings.Checked;
            Config.Settings.Tags.enableScoreMin = chkTagsEnableScoreLimit.Checked;
            Config.Settings.Tags.scoreAsTag = chkTagsIncludeScoreAsTag.Checked;
            Config.Settings.Tags.scoreMin = Convert.ToInt32(numTagsScoreLimit.Value);
            Config.Settings.Tags.imageLimit = Convert.ToInt32(numTagsDownloadLimit.Value);
            Config.Settings.Tags.pageLimit = Convert.ToInt32(numTagsPageLimit.Value);
            Config.Settings.Tags.separateNonImages = chkTagsSeparateNonImages.Checked;
            Config.Settings.Tags.downloadBlacklisted = chkTagsDownloadBlacklisted.Checked;

          // Pools
            switch (string.IsNullOrWhiteSpace(txtPoolSchema.Text)) {
                case true:
                    Config.Settings.Pools.fileNameSchema = "%poolname%_%page%";
                    break;

                case false:
                    Config.Settings.Pools.fileNameSchema = apiTools.ReplaceIllegalCharacters(txtPoolSchema.Text.ToLower());
                    break;
            }
            Config.Settings.Pools.mergeGraylisted = chkPoolsMergeGraylistedImages.Checked;
            Config.Settings.Pools.addWishlistSilent = chkPoolsAddToWishlistSilently.Checked;
            Config.Settings.Pools.downloadBlacklisted = chkPoolsDownloadBlacklistedImages.Checked;
            Config.Settings.Pools.mergeBlacklisted = chkPoolsMergeBlacklistedImages.Checked;

          // Images
            switch (string.IsNullOrWhiteSpace(txtImageSchema.Text)) {
                case true:
                    Config.Settings.Images.fileNameSchema = "%artist%_%md5%";
                    break;

                case false:
                    Config.Settings.Images.fileNameSchema = apiTools.ReplaceIllegalCharacters(txtImageSchema.Text.ToLower());
                    break;
            }
            Config.Settings.Images.separateRatings = chkImagesSeparateRatings.Checked;
            Config.Settings.Images.separateGraylisted = chkImagesSeparateGraylisted.Checked;
            Config.Settings.Images.separateArtists = chkImagesSeparateArtists.Checked;
            Config.Settings.Images.useForm = chkImagesUseForm.Checked;
            Config.Settings.Images.separateBlacklisted = chkImagesSeparateBlacklisted.Checked;

          // Save all
            Config.Settings.Save(ConfigType.All);
        }
        private void loadSettings() {
            // General
            if (string.IsNullOrEmpty(Config.Settings.General.saveLocation)) {
                txtSaveTo.Text = Environment.CurrentDirectory;
            }
            else {
                txtSaveTo.Text = Config.Settings.General.saveLocation;
            }
            chkSaveInfoFiles.Checked = Config.Settings.General.saveInfo;
            chkSaveGraylistedImages.Checked = Config.Settings.General.saveGraylisted;
            chkIgnoreFinish.Checked = Config.Settings.General.ignoreFinish;
            chkOpenAfterDownload.Checked = Config.Settings.General.openAfter;
            chkSkipArgumentCheck.Checked = Config.Settings.Initialization.SkipArgumentCheck;
            chkAutoDownloadWithArguments.Checked = Config.Settings.Initialization.AutoDownloadWithArguments;
            //chkSaveMetadata.Checked = Config.Settings.General.saveMetadata;
            //chkSaveArtistMetadata.Checked = Config.Settings.General.saveArtistMetadata;
            //chkSaveTagMetadata.Checked = Config.Settings.General.saveTagMetadata;

            // Tags
            txtTagSchema.Text = apiTools.ReplaceIllegalCharacters(Config.Settings.Tags.fileNameSchema.ToLower());
            chkTagsExplicit.Checked = Config.Settings.Tags.Explicit;
            chkTagsQuestionable.Checked = Config.Settings.Tags.Questionable;
            chkTagsSafe.Checked = Config.Settings.Tags.Safe;
            chkTagsSeparateRatings.Checked = Config.Settings.Tags.separateRatings;
            chkTagsEnableScoreLimit.Checked = Config.Settings.Tags.enableScoreMin;
            chkTagsIncludeScoreAsTag.Checked = Config.Settings.Tags.scoreAsTag;
            numTagsScoreLimit.Value = Convert.ToDecimal(Config.Settings.Tags.scoreMin);
            numTagsDownloadLimit.Value = Convert.ToDecimal(Config.Settings.Tags.imageLimit);
            numTagsPageLimit.Value = Convert.ToDecimal(Config.Settings.Tags.pageLimit);
            chkTagsSeparateNonImages.Checked = Config.Settings.Tags.separateNonImages;
            chkTagsDownloadBlacklisted.Checked = Config.Settings.Tags.downloadBlacklisted;

            // Pools
            txtPoolSchema.Text = apiTools.ReplaceIllegalCharacters(Config.Settings.Pools.fileNameSchema.ToLower());
            chkPoolsMergeGraylistedImages.Checked = Config.Settings.Pools.mergeGraylisted;
            chkPoolsAddToWishlistSilently.Checked = Config.Settings.Pools.addWishlistSilent;
            chkPoolsDownloadBlacklistedImages.Checked = Config.Settings.Pools.downloadBlacklisted;
            chkPoolsMergeBlacklistedImages.Checked = Config.Settings.Pools.mergeBlacklisted;

            // Images
            txtImageSchema.Text = apiTools.ReplaceIllegalCharacters(Config.Settings.Images.fileNameSchema.ToLower());
            chkImagesSeparateRatings.Checked = Config.Settings.Images.separateRatings;
            chkImagesSeparateGraylisted.Checked = Config.Settings.Images.separateGraylisted;
            chkImagesSeparateArtists.Checked = Config.Settings.Images.separateArtists;
            chkImagesUseForm.Checked = Config.Settings.Images.useForm;
            chkImagesSeparateBlacklisted.Checked = Config.Settings.Images.separateBlacklisted;

        }
        private void checkAdmin() {
            if (!Program.IsAdmin) {
                if (MessageBox.Show("This task requires re-running as administrator. Restart elevated?", "aphrodite", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    var exeName = Process.GetCurrentProcess().MainModule.FileName;
                    ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                    startInfo.Verb = "runas";
                    Process.Start(startInfo);
                    Environment.Exit(0);
                }
            }
        }
        private void checkProtocols() {
            switch (Program.UseIni) {
                case false:
                    RegistryKey keyTags = Registry.ClassesRoot.OpenSubKey("tags\\shell\\open\\command", false);
                    RegistryKey keyPools = Registry.ClassesRoot.OpenSubKey("pools\\shell\\open\\command", false);
                    RegistryKey keyPoolWl = Registry.ClassesRoot.OpenSubKey("poolwl\\shell\\open\\command", false);
                    RegistryKey keyImages = Registry.ClassesRoot.OpenSubKey("images\\shell\\open\\command", false);

                    if (Registry.ClassesRoot.GetValue("tags", "URL Protocol") == null || keyTags == null) {
                        TagsProtocol = false;
                    }
                    else {
                        TagsProtocol = true;
                    }

                    if (Registry.ClassesRoot.GetValue("pools", "URL Protocol") == null || keyPools == null) {
                        PoolsProtocol = false;
                    }
                    else {
                        PoolsProtocol = true;
                    }

                    if (Registry.ClassesRoot.GetValue("poolwl", "URL Protocol") == null || keyPoolWl == null) {
                        PoolsWishlistProtocol = false;
                    }
                    else {
                        PoolsWishlistProtocol = true;
                    }

                    if (Registry.ClassesRoot.GetValue("images", "URL Protocol") == null || keyImages == null) {
                        ImagesProtocol = false;
                    }
                    else {
                        ImagesProtocol = true;
                    }

                    if (TagsProtocol == false && PoolsProtocol == false && ImagesProtocol == false) {
                        NoProtocols = true;
                    }
                    else {
                        NoProtocols = false;
                    }
                    break;
            }
        }
        private string createProtocolDir() {
            switch (Program.UseIni) {
                case false:
                    if (!string.IsNullOrWhiteSpace(Config.Settings.General.saveLocation)) {
                        return Config.Settings.General.saveLocation;
                    }

                    string directory = Environment.CurrentDirectory;
                    string filename = AppDomain.CurrentDomain.FriendlyName;
                    switch (MessageBox.Show("Would you like to specifiy a location to store aphrodite? Select no to use current directory.\n\nThis is required for the plugin to work properly, and is recommended that you select a place that won't be messed with AND have permission to write files to.", "aphrodite", MessageBoxButtons.YesNoCancel)) {
                        case System.Windows.Forms.DialogResult.Yes:
                            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select a directory to store aphrodite.exe", SelectedPath = Environment.CurrentDirectory, ShowNewFolderButton = true }) {
                                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                                    directory = fbd.SelectedPath;
                                }
                                else {
                                    return null;
                                }
                            }
                            break;
                        case System.Windows.Forms.DialogResult.Cancel:
                            return null;
                    }

                    if (directory != Environment.CurrentDirectory) {
                        if (File.Exists(directory + "\\aphrodite.exe")) {
                            File.Delete(directory + "\\aphrodite.exe");
                        }
                        File.Copy(Program.ApplicationPath + "\\" + filename, directory + "\\aphrodite.exe");
                    }

                    // Return selected directory
                    return directory;
            }

            return null;
        }

        private void btnBrws_Click(object sender, EventArgs e) {
            using (Ookii.Dialogs.WinForms.VistaFolderBrowserDialog fbd = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog() { Description = "Select a folder to store downloads...", UseDescriptionForTitle = true }) {
                if (!string.IsNullOrEmpty(txtSaveTo.Text)) {
                    fbd.SelectedPath = txtSaveTo.Text;
                }

                if (fbd.ShowDialog() == DialogResult.OK) {
                    if (!string.IsNullOrWhiteSpace(txtSaveTo.Text) && Directory.Exists(txtSaveTo.Text)) {
                        string MessageDialog = "Would you like to move your current downloads to the new directory?";
                        string MessageTotalCount = string.Empty;

                        if (Directory.Exists(txtSaveTo.Text + "\\Tags")) {
                            MessageTotalCount += "\r\nYou have " + Directory.GetDirectories(txtSaveTo.Text + "\\Tags", "*", SearchOption.TopDirectoryOnly).Length + " downloaded tags";
                        }
                        if (Directory.Exists(txtSaveTo.Text + "\\Pages")) {
                            MessageTotalCount += "\r\nYou have " + Directory.GetDirectories(txtSaveTo.Text + "\\Pages", "*", SearchOption.TopDirectoryOnly).Length + " downloaded pages";
                        }
                        if (Directory.Exists(txtSaveTo.Text + "\\Pools")) {
                            MessageTotalCount += "\r\nYou have " + Directory.GetDirectories(txtSaveTo.Text + "\\Pools", "*", SearchOption.TopDirectoryOnly).Length + " downloaded pools";
                        }
                        if (Directory.Exists(txtSaveTo.Text + "\\Images")) {
                            int ExplicitCount = 0;
                            int QuestionableCount = 0;
                            int SafeCount = 0;

                            if (Directory.Exists(txtSaveTo.Text + "\\Images\\explicit")) {
                                ExplicitCount += Directory.GetFiles(txtSaveTo.Text + "\\Images\\explicit", "*", SearchOption.TopDirectoryOnly).Length;
                            }
                            if (Directory.Exists(txtSaveTo.Text + "\\Images\\questionable")) {
                                QuestionableCount += Directory.GetFiles(txtSaveTo.Text + "\\Images\\questionable", "*", SearchOption.TopDirectoryOnly).Length;
                            }
                            if (Directory.Exists(txtSaveTo.Text + "\\Images\\safe")) {
                                SafeCount += Directory.GetFiles(txtSaveTo.Text + "\\Images\\safe", "*", SearchOption.TopDirectoryOnly).Length;
                            }
                            MessageTotalCount += string.Format("\r\nYou have " + (ExplicitCount + QuestionableCount + SafeCount) + " downloaded images\r\n({0} explicit, {1} questionable, {2} safe)", ExplicitCount, QuestionableCount, SafeCount);
                        }

                        if (!string.IsNullOrWhiteSpace(MessageTotalCount)) {
                            MessageTotalCount = "\r\n" + MessageTotalCount;
                        }

                        switch (MessageBox.Show(MessageDialog + MessageTotalCount, "aphrodite", MessageBoxButtons.YesNoCancel)) {
                            case System.Windows.Forms.DialogResult.Cancel:
                                return;

                            case System.Windows.Forms.DialogResult.Yes:
                                if (Directory.Exists(txtSaveTo.Text + "\\Tags")) {
                                    Directory.Move(txtSaveTo.Text + "\\Tags", fbd.SelectedPath + "\\Tags");
                                }
                                if (Directory.Exists(txtSaveTo.Text + "\\Pages")) {
                                    Directory.Move(txtSaveTo.Text + "\\Pages", fbd.SelectedPath + "\\Pages");
                                }
                                if (Directory.Exists(txtSaveTo.Text + "\\Pools")) {
                                    Directory.Move(txtSaveTo.Text + "\\Pools", fbd.SelectedPath + "\\Pools");
                                }
                                if (Directory.Exists(txtSaveTo.Text + "\\Images")) {
                                    Directory.Move(txtSaveTo.Text + "\\Images", fbd.SelectedPath + "\\Images");
                                }
                                break;
                        }
                    }

                    txtSaveTo.Text = fbd.SelectedPath;
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e) {
            saveSettings();
            this.DialogResult = DialogResult.OK;
        }
        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
        }

        private void chkMinimumScore_CheckedChanged(object sender, EventArgs e) {
            numTagsScoreLimit.Enabled = chkTagsEnableScoreLimit.Checked;
            chkTagsIncludeScoreAsTag.Enabled = chkTagsEnableScoreLimit.Checked;
        }

        private void btnBlacklist_Click(object sender, EventArgs e) {
            frmBlacklist blackList = new frmBlacklist();
            if (blackList.ShowDialog() == DialogResult.OK) {
                Config.Settings.General.Save();
            }
        }

        private void btnTagsProtocol_Click(object sender, EventArgs e) {
            checkAdmin();

            if (NoProtocols) {
                string protocolDir = createProtocolDir();
                if (protocolDir == null) {
                    return;
                }

                // Create registry keys
                Registry.ClassesRoot.CreateSubKey("tags");
                RegistryKey setIdentifier = Registry.ClassesRoot.OpenSubKey("tags", true);
                setIdentifier.SetValue("URL Protocol", "");
                Registry.ClassesRoot.CreateSubKey("tags\\shell");
                Registry.ClassesRoot.CreateSubKey("tags\\shell\\open");
                Registry.ClassesRoot.CreateSubKey("tags\\shell\\open\\command");
                RegistryKey setProtocol = Registry.ClassesRoot.OpenSubKey("tags\\shell\\open\\command", true);
                setProtocol.SetValue("", "\"" + protocolDir + "\\aphrodite.exe\" \"%1\"");
                //Registry.ClassesRoot.CreateSubKey("tags\\DefaultIcon");
                //RegistryKey setIcon = Registry.ClassesRoot.OpenSubKey("tags\\DefaultIcon", true);
                //setIcon.SetValue("", "\"" + protocolDir + "\\aphrodite.exe\",1");
            }
            else {
                string protocolDir = createProtocolDir();
                if (protocolDir == null) {
                    return;
                }

                // Create registry keys
                Registry.ClassesRoot.CreateSubKey("tags");
                RegistryKey setIdentifier = Registry.ClassesRoot.OpenSubKey("tags", true);
                setIdentifier.SetValue("URL Protocol", "");
                Registry.ClassesRoot.CreateSubKey("tags\\shell");
                Registry.ClassesRoot.CreateSubKey("tags\\shell\\open");
                Registry.ClassesRoot.CreateSubKey("tags\\shell\\open\\command");
                RegistryKey setProtocol = Registry.ClassesRoot.OpenSubKey("tags\\shell\\open\\command", true);
                setProtocol.SetValue("", "\"" + protocolDir + "\\aphrodite.exe\" \"%1\"");
                //Registry.ClassesRoot.CreateSubKey("tags\\DefaultIcon");
                //RegistryKey setIcon = Registry.ClassesRoot.OpenSubKey("tags\\DefaultIcon", true);
                //setIcon.SetValue("", "\"" + protocolDir + "\\aphrodite.exe\",1");
            }

            btnProtocolInstallTags.Enabled = false;
            btnProtocolInstallTags.Text = "tags protocol installed";

            TagsProtocol = true;
            NoProtocols = false;
        }
        private void btnPoolsProtocol_Click(object sender, EventArgs e) {
            checkAdmin();

            if (NoProtocols) {
                string protocolDir = createProtocolDir();
                if (protocolDir == null) {
                    return;
                }

                // Create registry keys
                Registry.ClassesRoot.CreateSubKey("pools");
                RegistryKey setIdentifier = Registry.ClassesRoot.OpenSubKey("pools", true);
                setIdentifier.SetValue("URL Protocol", "");
                Registry.ClassesRoot.CreateSubKey("pools\\shell");
                Registry.ClassesRoot.CreateSubKey("pools\\shell\\open");
                Registry.ClassesRoot.CreateSubKey("pools\\shell\\open\\command");
                RegistryKey setProtocol = Registry.ClassesRoot.OpenSubKey("pools\\shell\\open\\command", true);
                setProtocol.SetValue("", "\"" + protocolDir + "\\aphrodite.exe\" \"%1\"");
                //Registry.ClassesRoot.CreateSubKey("pools\\DefaultIcon");
                //RegistryKey setIcon = Registry.ClassesRoot.OpenSubKey("pools\\DefaultIcon", true);
                //setIcon.SetValue("", "\"" + protocolDir + "\\aphrodite.exe\",1");
            }
            else {
                string protocolDir = createProtocolDir();
                if (protocolDir == null) {
                    return;
                }

                if (!PoolsProtocol) {
                    // Create registry keys
                    Registry.ClassesRoot.CreateSubKey("pools");
                    RegistryKey setIdentifier = Registry.ClassesRoot.OpenSubKey("pools", true);
                    setIdentifier.SetValue("URL Protocol", "");
                    Registry.ClassesRoot.CreateSubKey("pools\\shell");
                    Registry.ClassesRoot.CreateSubKey("pools\\shell\\open");
                    Registry.ClassesRoot.CreateSubKey("pools\\shell\\open\\command");
                    RegistryKey setProtocol = Registry.ClassesRoot.OpenSubKey("pools\\shell\\open\\command", true);
                    setProtocol.SetValue("", "\"" + protocolDir + "\\aphrodite.exe\" \"%1\"");
                    //Registry.ClassesRoot.CreateSubKey("pools\\DefaultIcon");
                    //RegistryKey setIcon = Registry.ClassesRoot.OpenSubKey("pools\\DefaultIcon", true);
                    //setIcon.SetValue("", "\"" + protocolDir + "\\aphrodite.exe\",1");
                }

                if (!PoolsWishlistProtocol) {
                    // wishlist keys
                    Registry.ClassesRoot.CreateSubKey("poolwl");
                    RegistryKey setIdentifier = Registry.ClassesRoot.OpenSubKey("poolwl", true);
                    setIdentifier.SetValue("URL Protocol", "");
                    Registry.ClassesRoot.CreateSubKey("poolwl\\shell");
                    Registry.ClassesRoot.CreateSubKey("poolwl\\shell\\open");
                    Registry.ClassesRoot.CreateSubKey("poolwl\\shell\\open\\command");
                    RegistryKey setProtocol = Registry.ClassesRoot.OpenSubKey("poolwl\\shell\\open\\command", true);
                    setProtocol.SetValue("", "\"" + protocolDir + "\\aphrodite.exe\" \"%1\"");
                    //Registry.ClassesRoot.CreateSubKey("poolwl\\DefaultIcon");
                    //RegistryKey setIcon = Registry.ClassesRoot.OpenSubKey("poolwl\\DefaultIcon", true);
                    //setIcon.SetValue("", "\"" + protocolDir + "\\aphrodite.exe\",1");
                }
            }

            btnProtocolInstallPools.Enabled = false;
            btnProtocolInstallPools.Text = "pools protocols installed";

            PoolsProtocol = true;
            PoolsWishlistProtocol = true;
            NoProtocols = false;
        }
        private void btnImagesProtocol_Click(object sender, EventArgs e) {
            checkAdmin();

            if (NoProtocols) {
                string protocolDir = createProtocolDir();
                if (protocolDir == null) {
                    return;
                }

                // Create registry keys
                Registry.ClassesRoot.CreateSubKey("images");
                RegistryKey setIdentifier = Registry.ClassesRoot.OpenSubKey("images", true);
                setIdentifier.SetValue("URL Protocol", "");
                Registry.ClassesRoot.CreateSubKey("images\\shell");
                Registry.ClassesRoot.CreateSubKey("images\\shell\\open");
                Registry.ClassesRoot.CreateSubKey("images\\shell\\open\\command");
                RegistryKey setProtocol = Registry.ClassesRoot.OpenSubKey("images\\shell\\open\\command", true);
                setProtocol.SetValue("", "\"" + protocolDir + "\\aphrodite.exe\" \"%1\"");
                //Registry.ClassesRoot.CreateSubKey("images\\DefaultIcon");
                //RegistryKey setIcon = Registry.ClassesRoot.OpenSubKey("images\\DefaultIcon", true);
                //setIcon.SetValue("", "\"" + protocolDir + "\\aphrodite.exe\",1");
            }
            else {
                string protocolDir = createProtocolDir();
                if (protocolDir == null) {
                    return;
                }

                // Create registry keys
                Registry.ClassesRoot.CreateSubKey("images");
                RegistryKey setIdentifier = Registry.ClassesRoot.OpenSubKey("images", true);
                setIdentifier.SetValue("URL Protocol", "");
                Registry.ClassesRoot.CreateSubKey("images\\shell");
                Registry.ClassesRoot.CreateSubKey("images\\shell\\open");
                Registry.ClassesRoot.CreateSubKey("images\\shell\\open\\command");
                RegistryKey setProtocol = Registry.ClassesRoot.OpenSubKey("images\\shell\\open\\command", true);
                setProtocol.SetValue("", "\"" + protocolDir + "\\aphrodite.exe\" \"%1\"");
                //Registry.ClassesRoot.CreateSubKey("images\\DefaultIcon");
                //RegistryKey setIcon = Registry.ClassesRoot.OpenSubKey("images\\DefaultIcon", true);
                //setIcon.SetValue("", "\"" + protocolDir + "\\aphrodite.exe\",1");
            }

            btnProtocolInstallImages.Enabled = false;
            btnProtocolInstallImages.Text = "images protocol installed";

            ImagesProtocol = true;
            NoProtocols = false;
        }

        private void btnUserscript_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/murrty/aphrodite/raw/master/Resources/aphrodite.user.js");
        }
        private void btnImagesUserscript_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/murrty/aphrodite/raw/master/Resources/aphrodite.images.user.js");
        }

        private void btnExportIni_Click(object sender, EventArgs e) {
            switch (MessageBox.Show("Do you really want to export settings to an ini file?", "aphrodite", MessageBoxButtons.YesNo)) {
                case DialogResult.Yes:
                    break;
            }
            MessageBox.Show("You can use the system-based settings for aphrodite by changing \"useIni\" to \"False\".\r\nAlso, graylist & blacklist have spaces between tags and are separate files, so... keep that in mind.", "aphrodite");
            string bufferINI = "[aphrodite]\nuseIni=True";
            File.WriteAllText(Program.ApplicationPath + "\\graylist.cfg", Config.Settings.General.Graylist);
            File.WriteAllText(Program.ApplicationPath + "\\blacklist.cfg", Config.Settings.General.Blacklist);

            bufferINI += "\n\n[Global]";
            bufferINI += "\nsaveInfo=" + Config.Settings.General.saveInfo;
            bufferINI += "\nsaveBlacklisted=" + Config.Settings.General.saveGraylisted;
            bufferINI += "\nignoreFinish=" + Config.Settings.General.ignoreFinish;
            bufferINI += "\nopenAfter=" + Config.Settings.General.openAfter;
            //bufferINI += "\nsaveMetadata=" + Config.Settings.General.saveMetadata;
            //bufferINI += "\nsaveArtistMetadata=" + Config.Settings.General.saveArtistMetadata;
            //bufferINI += "\nsaveTagMetadata=" + Config.Settings.General.saveTagMetadata;

            bufferINI += "\n\n[Tags]";
            bufferINI += "\nfileNameSchema=" + apiTools.ReplaceIllegalCharacters(Config.Settings.Tags.fileNameSchema);
            bufferINI += "\nuseMinimumScore=" + Config.Settings.Tags.enableScoreMin;
            bufferINI += "\nscoreAsTag=" + Config.Settings.Tags.scoreAsTag;
            bufferINI += "\nscoreMin=" + Config.Settings.Tags.scoreMin;
            bufferINI += "\nimageLimit=" + Config.Settings.Tags.imageLimit;
            bufferINI += "\npageLimit=" + Config.Settings.Tags.pageLimit;
            bufferINI += "\nseparateRatings=" + Config.Settings.Tags.separateRatings;
            bufferINI += "\nseparateNonImages=" + Config.Settings.Tags.separateNonImages;
            bufferINI += "\nExplicit=" + Config.Settings.Tags.Explicit;
            bufferINI += "\nQuestionable=" + Config.Settings.Tags.Questionable;
            bufferINI += "\nSafe=" + Config.Settings.Tags.Safe;

            bufferINI += "\n\n[Pools]";
            bufferINI += "\nfileNameSchema=" + apiTools.ReplaceIllegalCharacters(Config.Settings.Pools.fileNameSchema);
            bufferINI += "\nmergeBlacklisted=" + Config.Settings.Pools.mergeGraylisted;

            bufferINI += "\n\n[Images]";
            bufferINI += "\nfileNameSchema=" + apiTools.ReplaceIllegalCharacters(Config.Settings.Images.fileNameSchema);
            bufferINI += "\nseparateRatings=" + Config.Settings.Images.separateRatings;
            bufferINI += "\nseparateBlacklisted=" + Config.Settings.Images.separateGraylisted;
            bufferINI += "\nseparateNonImages=" + Config.Settings.Images.separateNonImages;
            bufferINI += "\nuseForm=" + Config.Settings.Images.useForm;

            bufferINI += "\n\n[Forms]";
            bufferINI += "\nfrmMainLocation=" + Config.Settings.FormSettings.frmMain_Location.X + ", " + Config.Settings.FormSettings.frmMain_Location.Y;

            File.WriteAllText(Program.ApplicationPath + "\\aphrodite.ini", bufferINI);

            Program.UseIni = true;
        }

        private void btnSchemaUndesiredTags_Click(object sender, EventArgs e) {
            frmUndesiredTags undesiredTags = new frmUndesiredTags();
            undesiredTags.ShowDialog();
            undesiredTags.Dispose();
        }

        private void txtTagSchema_KeyPress(object sender, KeyPressEventArgs e) {
            switch (e.KeyChar) {
                case (char)92:  // \
                case (char)47:  // /
                case (char)58:  // :
                case (char)42:  // *
                case (char)63:  // ?
                case (char)34:  // "
                case (char)60:  // <
                case (char)62:  // >
                case (char)124: // |
                    e.Handled = true;
                    System.Media.SystemSounds.Beep.Play();
                    break;
            }
        }
        private void txtPoolSchema_KeyPress(object sender, KeyPressEventArgs e) {
            switch (e.KeyChar) {
                case (char)92:  // \
                case (char)47:  // /
                case (char)58:  // :
                case (char)42:  // *
                case (char)63:  // ?
                case (char)34:  // "
                case (char)60:  // <
                case (char)62:  // >
                case (char)124: // |
                    e.Handled = true;
                    System.Media.SystemSounds.Beep.Play();
                    break;
            }
        }
        private void txtImageSchema_KeyPress(object sender, KeyPressEventArgs e) {
            switch (e.KeyChar) {
                case (char)92:  // \
                case (char)47:  // /
                case (char)58:  // :
                case (char)42:  // *
                case (char)63:  // ?
                case (char)34:  // "
                case (char)60:  // <
                case (char)62:  // >
                case (char)124: // |
                    e.Handled = true;
                    System.Media.SystemSounds.Beep.Play();
                    break;
            }
        }

        private void txtTagSchema_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Modifiers == Keys.Control && e.KeyCode == Keys.V && Clipboard.ContainsText()) {
                case true:
                    Clipboard.SetText(Clipboard.GetText().Replace("\\", "").Replace("/", "")
                        .Replace(":", "").Replace("*", "").Replace("?", "").Replace("\"", "")
                        .Replace("<", "").Replace(">", "").Replace("|", ""));
                    break;
            }
        }

        private void txtPoolSchema_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Modifiers == Keys.Control && e.KeyCode == Keys.V && Clipboard.ContainsText()) {
                case true:
                    Clipboard.SetText(Clipboard.GetText().Replace("\\", "").Replace("/", "")
                        .Replace(":", "").Replace("*", "").Replace("?", "").Replace("\"", "")
                        .Replace("<", "").Replace(">", "").Replace("|", ""));
                    break;
            }
        }

        private void txtImageSchema_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Modifiers == Keys.Control && e.KeyCode == Keys.V && Clipboard.ContainsText()) {
                case true:
                    Clipboard.SetText(Clipboard.GetText().Replace("\\", "").Replace("/", "")
                        .Replace(":", "").Replace("*", "").Replace("?", "").Replace("\"", "")
                        .Replace("<", "").Replace(">", "").Replace("|", ""));
                    break;
            }
        }

        private void btnExportGraylist_Click(object sender, EventArgs e) {
            using (Ookii.Dialogs.WinForms.VistaSaveFileDialog sfd = new Ookii.Dialogs.WinForms.VistaSaveFileDialog()) {
                sfd.Title = "Save graylist as...";
                sfd.Filter = "Text File (*.txt)|*.txt|All Files (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK) {
                    File.WriteAllText(sfd.FileName, Config.Settings.General.Graylist.Replace(" ", "\r\n"));
                }
            }
        }

        private void btnImportGraylist_Click(object sender, EventArgs e) {
            using (Ookii.Dialogs.WinForms.VistaOpenFileDialog ofd = new Ookii.Dialogs.WinForms.VistaOpenFileDialog()) {
                ofd.Title = "Select a file for the graylist...";
                ofd.Filter = "Text File (*.txt)|*.txt|All Files(*.*)|*.*";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK) {
                    string NewGraylist = File.ReadAllText(ofd.FileName, System.Text.Encoding.Default).Replace("\r\n", "\n").Replace("\n", " ");
                    if (chkOverwriteOnImport.Checked) {
                        Config.Settings.General.Graylist = NewGraylist;
                    }
                    else {
                        Config.Settings.General.Graylist += "\r\n" + NewGraylist;
                    }
                }
            }
        }

        private void btnExportBlacklist_Click(object sender, EventArgs e) {
            using (Ookii.Dialogs.WinForms.VistaSaveFileDialog sfd = new Ookii.Dialogs.WinForms.VistaSaveFileDialog()) {
                sfd.Title = "Save blacklist as...";
                sfd.Filter = "Text File (*.txt)|*.txt|All Files (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK) {
                    File.WriteAllText(sfd.FileName, Config.Settings.General.Blacklist.Replace(" ", "\r\n"));
                }
            }
        }

        private void btnImportBlacklist_Click(object sender, EventArgs e) {
            using (Ookii.Dialogs.WinForms.VistaOpenFileDialog ofd = new Ookii.Dialogs.WinForms.VistaOpenFileDialog()) {
                ofd.Title = "Select a file for the blacklist...";
                ofd.Filter = "Text File (*.txt)|*.txt|All Files(*.*)|*.*";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK) {
                    string NewBlacklist = File.ReadAllText(ofd.FileName, System.Text.Encoding.Default).Replace("\r\n", "\n").Replace("\n", " ");
                    if (chkOverwriteOnImport.Checked) {
                        Config.Settings.General.Blacklist = NewBlacklist;
                    }
                    else {
                        Config.Settings.General.Blacklist += "\r\n" + NewBlacklist;
                    }
                }
            }
        }

        private void btnExportUndesiredTags_Click(object sender, EventArgs e) {
            using (Ookii.Dialogs.WinForms.VistaSaveFileDialog sfd = new Ookii.Dialogs.WinForms.VistaSaveFileDialog()) {
                sfd.Title = "Save undesired tags as...";
                sfd.Filter = "Text File (*.txt)|*.txt|All Files (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK) {
                    File.WriteAllText(sfd.FileName, Config.Settings.General.undesiredTags.Replace(" ", "\r\n"));
                }
            }
        }

        private void btnImportUndesiredTags_Click(object sender, EventArgs e) {
            using (Ookii.Dialogs.WinForms.VistaOpenFileDialog ofd = new Ookii.Dialogs.WinForms.VistaOpenFileDialog()) {
                ofd.Title = "Select a file for the undesired tags...";
                ofd.Filter = "Text File (*.txt)|*.txt|All Files(*.*)|*.*";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK) {
                    string NewUndesiredTags = File.ReadAllText(ofd.FileName, System.Text.Encoding.Default).Replace("\r\n", "\n").Replace("\n", " ");
                    if (chkOverwriteOnImport.Checked) {
                        Config.Settings.General.undesiredTags = NewUndesiredTags;
                    }
                    else {
                        Config.Settings.General.undesiredTags += "\r\n" + NewUndesiredTags;
                    }
                }
            }
        }
    }
}
