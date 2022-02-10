using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace aphrodite {

    public partial class frmSettings : Form {

        #region Variables
        private bool ProtocolInstalled = false;
        private bool LoadingForm = false;
        #endregion

        public frmSettings() {
            InitializeComponent();
        }

        public frmSettings(ArgumentType Type) {
            InitializeComponent();

            tbMain.SelectedTab = Type switch {
                ArgumentType.ConfigureTagsSettings => tabTags,
                ArgumentType.ConfigurePoolsSettings => tabPools,
                ArgumentType.ConfigureImagesSettings => tabImages,
                ArgumentType.ConfigureMiscSettings => tabMisc,
                ArgumentType.ConfigureProtocolSettings => tabProtocol,
                ArgumentType.ConfigureSchemaSettings => tabSchemas,
                ArgumentType.ConfigureImportExportSettings => tabImportExport,
                ArgumentType.ConfigurePortableSettings => tabPortable,
                _ => tabGeneral,
            };
        }

        private void frmSettings_Load(object sender, EventArgs e) {
            if (Config.ValidPoint(Config.Settings.FormSettings.frmSettings_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmSettings_Location;
            }

            LoadingForm = true; 
            txtSaveTo.TextHint = Environment.CurrentDirectory;
            LoadSettings();

            chkEnableIni.Checked = Config.Settings.UseIni;

            if (Config.Settings.UseIni) {
                tbMain.TabPages.Remove(tabImportExport);
            }

            ProtocolInstalled = SystemRegistry.CheckRegistryKey();

            if (ProtocolInstalled) {
                btnInstallProtocol.Enabled = false;
                btnInstallProtocol.Text = "protocol is installed";
            }
            else {
                btnInstallProtocol.ShowUACShield = !Program.IsAdmin;
            }

            LoadingForm = false;
        }

        private void frmSettings_FormClosing(object sender, FormClosingEventArgs e) {
            Config.Settings.FormSettings.frmSettings_Location = this.Location;
            Config.Settings.FormSettings.Save();
        }

        private void SaveSettings() {
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
            Config.Settings.Initialization.ArgumentFormTopMost = chkArgumentFormTopMost.Checked;
            Config.Settings.Initialization.CheckForUpdates = chkCheckForUpdates.Checked;
            Config.Settings.Initialization.CheckForBetaUpdates = chkCheckForBetaUpdates.Checked;

          // Tags
            switch (string.IsNullOrWhiteSpace(txtTagSchema.Text)) {
                case true:
                    Config.Settings.Tags.fileNameSchema = "%md5%";
                    break;

                case false:
                    Config.Settings.Tags.fileNameSchema = ApiTools.ReplaceIllegalCharacters(txtTagSchema.Text.ToLower());
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
            Config.Settings.Tags.DownloadNewestToOldest = chkTagsDownloadNewestToOldest.Checked;
            Config.Settings.Tags.FavoriteCount = Convert.ToInt32(numTagsFavoriteCount.Value);
            Config.Settings.Tags.FavoriteCountAsTag = chkTagsFavoriteCountAsTag.Checked;

          // Pools
            switch (string.IsNullOrWhiteSpace(txtPoolSchema.Text)) {
                case true:
                    Config.Settings.Pools.fileNameSchema = "%poolname%_%page%";
                    break;

                case false:
                    Config.Settings.Pools.fileNameSchema = ApiTools.ReplaceIllegalCharacters(txtPoolSchema.Text.ToLower());
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
                    Config.Settings.Images.fileNameSchema = ApiTools.ReplaceIllegalCharacters(txtImageSchema.Text.ToLower());
                    break;
            }
            Config.Settings.Images.separateRatings = chkImagesSeparateRatings.Checked;
            Config.Settings.Images.separateGraylisted = chkImagesSeparateGraylisted.Checked;
            Config.Settings.Images.separateArtists = chkImagesSeparateArtists.Checked;
            Config.Settings.Images.useForm = chkImagesUseForm.Checked;
            Config.Settings.Images.separateBlacklisted = chkImagesSeparateBlacklisted.Checked;

          // Save all
            Config.Settings.Save();
        }

        private void LoadSettings() {
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
            chkArgumentFormTopMost.Checked = Config.Settings.Initialization.ArgumentFormTopMost;
            chkCheckForUpdates.Checked = Config.Settings.Initialization.CheckForUpdates;
            chkCheckForBetaUpdates.Checked = Config.Settings.Initialization.CheckForBetaUpdates;

            // Tags
            txtTagSchema.Text = ApiTools.ReplaceIllegalCharacters(Config.Settings.Tags.fileNameSchema.ToLower());
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
            chkTagsDownloadNewestToOldest.Checked = Config.Settings.Tags.DownloadNewestToOldest;
            numTagsFavoriteCount.Value = Convert.ToDecimal(Config.Settings.Tags.FavoriteCount);
            chkTagsFavoriteCountAsTag.Checked = Config.Settings.Tags.FavoriteCountAsTag;

            // Pools
            txtPoolSchema.Text = ApiTools.ReplaceIllegalCharacters(Config.Settings.Pools.fileNameSchema.ToLower());
            chkPoolsMergeGraylistedImages.Checked = Config.Settings.Pools.mergeGraylisted;
            chkPoolsAddToWishlistSilently.Checked = Config.Settings.Pools.addWishlistSilent;
            chkPoolsDownloadBlacklistedImages.Checked = Config.Settings.Pools.downloadBlacklisted;
            chkPoolsMergeBlacklistedImages.Checked = Config.Settings.Pools.mergeBlacklisted;

            // Images
            txtImageSchema.Text = ApiTools.ReplaceIllegalCharacters(Config.Settings.Images.fileNameSchema.ToLower());
            chkImagesSeparateRatings.Checked = Config.Settings.Images.separateRatings;
            chkImagesSeparateGraylisted.Checked = Config.Settings.Images.separateGraylisted;
            chkImagesSeparateArtists.Checked = Config.Settings.Images.separateArtists;
            chkImagesUseForm.Checked = Config.Settings.Images.useForm;
            chkImagesSeparateBlacklisted.Checked = Config.Settings.Images.separateBlacklisted;
        }

        private bool InstallProtocol() {
            if (ProtocolInstalled) {
                Log.MessageBox("The protocol is already installed, and pointing to this program.");
                return true;
            }
            int ExitCode;
            if (Program.IsAdmin) {
                ExitCode = SystemRegistry.SetRegistryKey();
            }
            else {
                Process ProtocolProcess = new() {
                    StartInfo = new ProcessStartInfo() {
                        Arguments = "-updateprotocol",
                        FileName = Program.FullApplicationPath,
                        WorkingDirectory = Program.ApplicationPath,
                        Verb = "runas"
                    }
                };
                ProtocolProcess.Start();
                ProtocolProcess.WaitForExit();
                ExitCode = ProtocolProcess.ExitCode;
            }
            
            switch (ExitCode) {
                case 0: return true;
                    
                case 1: {
                    Log.MessageBox("The protocol installation reported an exception, and most likely did not work.");
                } return false;

                case 2: {
                    Log.MessageBox("The protocol installation could not proceed, because it was not able to gain administrative permission.");
                } return false;

                case 3: {
                    Log.MessageBox("The \"tags\" protocol uninstallation failed, but the main protocol was installed.");
                } return true;

                case 4: {
                    Log.MessageBox("The \"pools\" protocol uninstallation failed, but the main protocol was installed.");
                } return true;

                case 5: {
                    Log.MessageBox("The \"imagess\" protocol uninstallation failed, but the main protocol was installed.");
                } return true;

                case 6: {
                    Log.MessageBox("The \"poolwl\" protocol uninstallation failed, but the main protocol was installed.");
                } return true;

                default: {
                    Log.MessageBox("The exit code was an unexpected code, can't determine protocol status.");
                } return false;
            }
        }

        private void btnBrowseForDownloadDirectory_Click(object sender, EventArgs e) {
            using BetterFolderBrowserNS.BetterFolderBrowser fbd = new() {
                Title = "Select a folder to store downloads...",
                RootFolder = Config.Settings.General.saveLocation,
                Multiselect = false
            };
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

                    switch (Log.MessageBox(MessageDialog + MessageTotalCount, MessageBoxButtons.YesNoCancel)) {
                        case DialogResult.Cancel:
                            return;

                        case DialogResult.Yes:
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

        private void btnSave_Click(object sender, EventArgs e) {
            SaveSettings();
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
            using frmBlacklist Blacklist = new();
            Blacklist.ShowDialog();
            if (Config.Settings.FormSettings.frmBlacklist_Location != Blacklist.Location) {
                Config.Settings.FormSettings.frmBlacklist_Location = Blacklist.Location;
            }
        }

        private void btnInstallProtocol_Click(object sender, EventArgs e) {
            if (!ProtocolInstalled) {
                if (InstallProtocol()) {
                    btnInstallProtocol.ShowUACShield = false;
                    btnInstallProtocol.Enabled = false;
                    btnInstallProtocol.Text = "protocol was installed";
                    ProtocolInstalled = true;
                }
            }
            else {
                Log.MessageBox("The protocol is already installed, and pointing to this program.");
            }
        }

        private void btnUserscript_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/murrty/aphrodite/raw/master/Resources/aphrodite.user.js");
        }

        private void btnSchemaUndesiredTags_Click(object sender, EventArgs e) {
            using frmUndesiredTags UndesiredTagsForm = new();
            UndesiredTagsForm.ShowDialog();
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
            using SaveFileDialog sfd = new();
            sfd.Title = "Save graylist as...";
            sfd.Filter = "Text File (*.txt)|*.txt|All Files (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK) {
                File.WriteAllText(sfd.FileName, Config.Settings.General.Graylist.Replace(" ", "\r\n"));
            }
        }

        private void btnImportGraylist_Click(object sender, EventArgs e) {
            using OpenFileDialog ofd = new();
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

        private void btnExportBlacklist_Click(object sender, EventArgs e) {
            using SaveFileDialog sfd = new();
            sfd.Title = "Save blacklist as...";
            sfd.Filter = "Text File (*.txt)|*.txt|All Files (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK) {
                File.WriteAllText(sfd.FileName, Config.Settings.General.Blacklist.Replace(" ", "\r\n"));
            }
        }

        private void btnImportBlacklist_Click(object sender, EventArgs e) {
            using OpenFileDialog ofd = new();
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

        private void btnExportUndesiredTags_Click(object sender, EventArgs e) {
            using SaveFileDialog sfd = new();
            sfd.Title = "Save undesired tags as...";
            sfd.Filter = "Text File (*.txt)|*.txt|All Files (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK) {
                File.WriteAllText(sfd.FileName, Config.Settings.General.undesiredTags.Replace(" ", "\r\n"));
            }
        }

        private void btnImportUndesiredTags_Click(object sender, EventArgs e) {
            using OpenFileDialog ofd = new();
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

        private void chkEnableIni_CheckedChanged(object sender, EventArgs e) {
            if (!LoadingForm && !chkDontOverwrite.Checked) {
                Config.Settings.ConvertConfig(chkEnableIni.Checked);
            }
        }

        private void chkCheckForUpdates_CheckedChanged(Object sender, EventArgs e) {
            chkCheckForBetaUpdates.Enabled = chkCheckForBetaUpdates.Checked;
        }
    }
}
