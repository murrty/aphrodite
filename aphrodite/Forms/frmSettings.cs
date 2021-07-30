using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace aphrodite {
    // theory: read the registry key for the protocol to determine location of aphridite.exe,
    // if aphrodite.exe is this one, disable the button, otherwise enable it to move it to the
    // currently running version's directory.
    public partial class frmSettings : Form {

        #region Variables
        public bool SwitchTab = false;   // is the form being changed from the userscript?
        public int Tab = 0;              // Used to determine the tab that will be selected on boot.

        private bool NoProtocols = true;             // Are there no protocols installed?
        private bool TagsProtocol = false;           // is the tags protocol installed?
        private bool PoolsProtocol = false;          // is the pools protocol installed?
        private bool PoolsWishlistProtocol = false;  // is the poolwl protocol installed?
        private bool ImagesProtocol = false;         // is the images protocol installed?

        private bool LoadingForm = false;
        #endregion

        public frmSettings() {
            InitializeComponent();
            txtSaveTo.TextHint = Environment.CurrentDirectory;
        }
        private void frmSettings_Load(object sender, EventArgs e) {
            LoadingForm = true; 
            loadSettings();

            chkEnableIni.Checked = Program.UseIni;

            if (Program.UseIni) {
                tbMain.TabPages.Remove(tabImportExport);
            }

            CheckProtocols();

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

            if (Config.Settings.FormSettings.frmSettings_Location.X == -32000 || Config.Settings.FormSettings.frmSettings_Location.Y == -32000) {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else {
                this.Location = Config.Settings.FormSettings.frmSettings_Location;
            }

            LoadingForm = false;
        }
        private void frmSettings_FormClosing(object sender, FormClosingEventArgs e) {
            if (Config.Settings.FormSettings.frmSettings_Location != this.Location) {
                Config.Settings.FormSettings.frmSettings_Location = this.Location;
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
            Config.Settings.Initialization.ArgumentFormTopMost = chkArgumentFormTopMost.Checked;
            Config.Settings.General.CheckForUpdates = chkCheckForUpdates.Checked;

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
            Config.Settings.Tags.DownloadNewestToOldest = chkTagsDownloadNewestToOldest.Checked;
            Config.Settings.Tags.FavoriteCount = Convert.ToInt32(numTagsFavoriteCount.Value);
            Config.Settings.Tags.FavoriteCountAsTag = chkTagsFavoriteCountAsTag.Checked;

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
            chkArgumentFormTopMost.Checked = Config.Settings.Initialization.ArgumentFormTopMost;
            chkCheckForUpdates.Checked = Config.Settings.General.CheckForUpdates;

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
            chkTagsDownloadNewestToOldest.Checked = Config.Settings.Tags.DownloadNewestToOldest;
            numTagsFavoriteCount.Value = Convert.ToDecimal(Config.Settings.Tags.FavoriteCount);
            chkTagsFavoriteCountAsTag.Checked = Config.Settings.Tags.FavoriteCountAsTag;

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
        private void CheckProtocols() {
            if (SystemRegistry.CheckRegistryKey(SystemRegistry.KeyName.Tags)) {
                TagsProtocol = true;
            }
            else {
                TagsProtocol = false;
            }

            if (SystemRegistry.CheckRegistryKey(SystemRegistry.KeyName.Pools)) {
                PoolsProtocol = true;
            }
            else {
                PoolsProtocol = false;
            }

            if (SystemRegistry.CheckRegistryKey(SystemRegistry.KeyName.PoolWl)) {
                PoolsWishlistProtocol = true;
            }
            else {
                PoolsWishlistProtocol = false;
            }

            if (SystemRegistry.CheckRegistryKey(SystemRegistry.KeyName.Images)) {
                ImagesProtocol = true;
            }
            else {
                ImagesProtocol = false;
            }

            if (TagsProtocol == false && PoolsProtocol == false && ImagesProtocol == false) {
                NoProtocols = true;
            }
            else {
                NoProtocols = false;
            }
        }
        private bool InstallProtocol(SystemRegistry.KeyName Name) {
            Process ProtocolProcess = new Process() {
                StartInfo = new ProcessStartInfo() {
                    FileName = Program.FullApplicationPath,
                    WorkingDirectory = Program.ApplicationPath,
                    Verb = "runas"
                }
            };

            switch (Name) {
                case SystemRegistry.KeyName.Tags:
                    ProtocolProcess.StartInfo.Arguments = "-updateprotocol:tags";
                    break;

                case SystemRegistry.KeyName.Pools: case SystemRegistry.KeyName.PoolWl: case SystemRegistry.KeyName.BothPools:
                    ProtocolProcess.StartInfo.Arguments = "-updateprotocol:pools";
                    break;

                case SystemRegistry.KeyName.Images:
                    ProtocolProcess.StartInfo.Arguments = "-updateprotocol:images";
                    break;

            }

            ProtocolProcess.Start();
            ProtocolProcess.WaitForExit();

            if (ProtocolProcess.ExitCode == 0) {
                return true;
            }
            else {
                return false;
            }
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
            using (frmBlacklist Blacklist = new frmBlacklist()) {
                Blacklist.ShowDialog();
                if (Config.Settings.FormSettings.frmBlacklist_Location != Blacklist.Location) {
                    Config.Settings.FormSettings.frmBlacklist_Location = Blacklist.Location;
                }
            }
        }

        private void btnTagsProtocol_Click(object sender, EventArgs e) {
            if (InstallProtocol(SystemRegistry.KeyName.Tags)) {
                btnProtocolInstallTags.Enabled = false;
                btnProtocolInstallTags.Text = "tags protocol installed";
                btnProtocolInstallTags.ShowUACShield = false;

                TagsProtocol = true;
                NoProtocols = false;
            }
            else {
                MessageBox.Show("Failed to install protocol");
            }
        }
        private void btnPoolsProtocol_Click(object sender, EventArgs e) {
            if (InstallProtocol(SystemRegistry.KeyName.BothPools)) {
                btnProtocolInstallPools.Enabled = false;
                btnProtocolInstallPools.Text = "pools protocols installed";
                btnProtocolInstallPools.ShowUACShield = false;

                PoolsProtocol = true;
                PoolsWishlistProtocol = true;
                NoProtocols = false;
            }
            else {
                MessageBox.Show("Failed to install protocol");
            }
        }
        private void btnImagesProtocol_Click(object sender, EventArgs e) {
            if (InstallProtocol(SystemRegistry.KeyName.Images)) {
                btnProtocolInstallImages.Enabled = false;
                btnProtocolInstallImages.Text = "images protocol installed";
                btnProtocolInstallImages.ShowUACShield = false;

                ImagesProtocol = true;
                NoProtocols = false;
            }
            else {
                MessageBox.Show("Failed to install protocol");
            }
        }

        private void btnUserscript_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/murrty/aphrodite/raw/master/Resources/aphrodite.user.js");
        }
        private void btnImagesUserscript_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/murrty/aphrodite/raw/master/Resources/aphrodite.images.user.js");
        }

        private void btnSchemaUndesiredTags_Click(object sender, EventArgs e) {
            using (frmUndesiredTags UndesiredTagsForm = new frmUndesiredTags()) {
                UndesiredTagsForm.ShowDialog();
            }
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

        private void chkEnableIni_CheckedChanged(object sender, EventArgs e) {
            if (!LoadingForm && !chkDontOverwrite.Checked) {
                Config.Settings.ConvertConfig(chkEnableIni.Checked);
            }
        }

    }
}
