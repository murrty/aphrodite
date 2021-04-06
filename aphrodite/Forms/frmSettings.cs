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
        private bool changedSaveTo = false;         // Determines if the txtSaveTo was changed.
        #endregion

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        public static void UACShield(Button btn) {
            const Int32 BCM_SETSHIELD = 0x160C;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            SendMessage(btn.Handle, BCM_SETSHIELD, 0, 1);
        }

        public frmSettings() {
            InitializeComponent();

            General.Default.Reload();
            Tags.Default.Reload();
            Pools.Default.Reload();
            Images.Default.Reload();
        }
        private void frmSettings_Load(object sender, EventArgs e) {
            loadSettings();
            if (Program.UseIni) {
                tbMain.TabPages.Remove(tabProtocol);
                tbMain.TabPages.Remove(tabPortable);
            }
            else
                checkProtocols();

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
                        tbMain.SelectedTab = tabPortable;
                        break;
                    case 6:
                        tbMain.SelectedTab = tabSchemas;
                        break;
                    default:
                        tbMain.SelectedTab = tabGeneral;
                        break;
                }
            }

            if (TagsProtocol == true) {
                btnProtocolInstallTags.Enabled = false;
                btnProtocolInstallTags.Text = "'tags:' protocol installed";
            }
            else {
                if (!Program.IsAdmin)
                    UACShield(btnProtocolInstallTags);
            }

            if (PoolsProtocol == true && PoolsWishlistProtocol == true) {
                btnProtocolInstallPools.Enabled = false;
                btnProtocolInstallPools.Text = "pool protocols installed";
            }
            else {
                if (!Program.IsAdmin)
                    UACShield(btnProtocolInstallPools);
            }

            if (ImagesProtocol == true) {
                btnProtocolInstallImages.Enabled = false;
                btnProtocolInstallImages.Text = "'images:' protocol installed";
            }
            else {
                if (!Program.IsAdmin)
                    UACShield(btnProtocolInstallImages);
            }

            if (InstallProtocol) {
                tbMain.SelectedTab = tabProtocol;
            }
        }

        private void saveSettings() {
            if (Program.UseIni) {
                FileInfo fI = new FileInfo(Environment.CurrentDirectory + "\\aphrodite.ini");
                fI.IsReadOnly = false;

              // General
                Program.Ini.WriteBool("saveInfo", chkSaveInfoFiles.Checked, "Global");
                Program.Ini.WriteBool("saveBlacklisted", chkSaveBlacklistedImages.Checked, "Global");
                Program.Ini.WriteBool("ignoreFinish", chkIgnoreFinish.Checked, "Global");
                Program.Ini.WriteBool("saveMetadata", chkSaveMetadata.Checked, "Global");
                Program.Ini.WriteBool("saveArtistMetadata", chkSaveArtistMetadata.Checked, "Global");
                Program.Ini.WriteBool("saveTagMetadata", chkSaveTagMetadata.Checked, "Global");

              // Tags
                Program.Ini.WriteString("fileNameSchema", apiTools.ReplaceIllegalCharacters(txtTagSchema.Text.ToLower()), "Tags");
                Program.Ini.WriteBool("Explicit", chkTagsExplicit.Checked, "Tags");
                Program.Ini.WriteBool("Questionable", chkTagsQuestionable.Checked, "Tags");
                Program.Ini.WriteBool("Safe", chkTagsSafe.Checked, "Tags");
                Program.Ini.WriteBool("separateRatings", chkTagsSeparateRatings.Checked, "Tags");
                Program.Ini.WriteBool("useMinimumScore", chkTagsEnableScoreLimit.Checked, "Tags");
                Program.Ini.WriteBool("scoreAsTag", chkTagsIncludeScoreAsTag.Checked, "Tags");
                Program.Ini.WriteInt("scoreMin", Convert.ToInt32(numTagsScoreLimit.Value), "Tags");
                Program.Ini.WriteInt("imageLimit", Convert.ToInt32(numTagsDownloadLimit.Value), "Tags");
                Program.Ini.WriteInt("pageLimit", Convert.ToInt32(numTagsDownloadLimit.Value), "Tags");
                Program.Ini.WriteBool("separateNonImages", chkTagsSeparateNonImages.Checked, "Tags");

              // Pools
                Program.Ini.WriteString("fileNameSchema", apiTools.ReplaceIllegalCharacters(txtPoolSchema.Text.ToLower()), "Pools");
                Program.Ini.WriteBool("mergeBlacklisted", chkPoolsMergeBlacklistedImages.Checked, "Pools");
                Program.Ini.WriteBool("openAfter", chkPoolsOpenAfterDownload.Checked, "Pools");

              // Images
                Program.Ini.WriteString("fileNameSchema", apiTools.ReplaceIllegalCharacters(txtImageSchema.Text.ToLower()), "Images");
                Program.Ini.WriteBool("separateRatings", chkImagesSeparateRatings.Checked, "Images");
                Program.Ini.WriteBool("separateBlacklisted", chkImagesSeparateBlacklisted.Checked, "Images");
                Program.Ini.WriteBool("separateArtists", chkImagesSeparateArtists.Checked, "Images");
                Program.Ini.WriteBool("useForm", chkImagesUseForm.Checked, "Images");
            }
            else {
              // General
                if (changedSaveTo) {
                    General.Default.saveLocation = txtSaveTo.Text;
                }
                General.Default.saveInfo = chkSaveInfoFiles.Checked;
                General.Default.saveBlacklisted = chkSaveBlacklistedImages.Checked;
                General.Default.ignoreFinish = chkIgnoreFinish.Checked;
                General.Default.saveMetadata = chkSaveMetadata.Checked;
                General.Default.saveArtistMetadata = chkSaveArtistMetadata.Checked;
                General.Default.saveTagMetadata = chkSaveTagMetadata.Checked;

              // Tags
                Tags.Default.fileNameSchema = apiTools.ReplaceIllegalCharacters(txtTagSchema.Text.ToLower());
                Tags.Default.Explicit = chkTagsExplicit.Checked;
                Tags.Default.Questionable = chkTagsQuestionable.Checked;
                Tags.Default.Safe = chkTagsSafe.Checked;
                Tags.Default.separateRatings = chkTagsSeparateRatings.Checked;
                Tags.Default.enableScoreMin = chkTagsEnableScoreLimit.Checked;
                Tags.Default.scoreAsTag = chkTagsIncludeScoreAsTag.Checked;
                Tags.Default.scoreMin = Convert.ToInt32(numTagsScoreLimit.Value);
                Tags.Default.imageLimit = Convert.ToInt32(numTagsDownloadLimit.Value);
                Tags.Default.pageLimit = Convert.ToInt32(numTagsPageLimit.Value);
                Tags.Default.separateNonImages = chkTagsSeparateNonImages.Checked;

              // Pools
                Pools.Default.fileNameSchema = apiTools.ReplaceIllegalCharacters(txtPoolSchema.Text.ToLower());
                Pools.Default.mergeBlacklisted = chkPoolsMergeBlacklistedImages.Checked;
                Pools.Default.openAfter = chkPoolsOpenAfterDownload.Checked;
                Pools.Default.addWishlistSilent = chkPoolsAddToWishlistSilently.Checked;

              // Images
                Images.Default.fileNameSchema = apiTools.ReplaceIllegalCharacters(txtImageSchema.Text.ToLower());
                Images.Default.separateRatings = chkImagesSeparateRatings.Checked;
                Images.Default.separateBlacklisted = chkImagesSeparateBlacklisted.Checked;
                Images.Default.separateArtists = chkImagesSeparateArtists.Checked;
                Images.Default.useForm = chkImagesUseForm.Checked;

              // Save all 4
                General.Default.Save();
                Tags.Default.Save();
                Pools.Default.Save();
                Images.Default.Save();
            }
        }
        private void loadSettings() {
            if (Program.UseIni) {
              // General
                txtSaveTo.Text = Environment.CurrentDirectory;
                txtSaveTo.Enabled = false;
                btnBrowseForSaveTo.Enabled = false;
                chkSaveInfoFiles.Checked = Program.Ini.ReadBool("saveInfo", "Global");
                chkSaveBlacklistedImages.Checked = Program.Ini.ReadBool("saveBlacklisted", "Global");
                chkIgnoreFinish.Checked = Program.Ini.ReadBool("ignoreFinish", "Global");
                chkSaveMetadata.Checked = Program.Ini.ReadBool("saveMetadata", "Global");
                chkSaveArtistMetadata.Checked = Program.Ini.ReadBool("saveArtistMetadata", "Global");
                chkSaveTagMetadata.Checked = Program.Ini.ReadBool("saveTagMetadata", "Global");

              // Tags
                txtTagSchema.Text = apiTools.ReplaceIllegalCharacters(Program.Ini.ReadString("fileNameSchema", "Tags").ToLower());
                chkTagsExplicit.Checked = Program.Ini.ReadBool("Explicit", "Tags");
                chkTagsQuestionable.Checked = Program.Ini.ReadBool("Questionable", "Tags");
                chkTagsSafe.Checked = Program.Ini.ReadBool("Safe", "Tags");
                chkTagsSeparateRatings.Checked = Program.Ini.ReadBool("separateRatings", "Tags");
                chkTagsEnableScoreLimit.Checked = Program.Ini.ReadBool("useMinimumScore", "Tags");
                chkTagsIncludeScoreAsTag.Checked = Program.Ini.ReadBool("scoreAsTag", "Tags");
                numTagsScoreLimit.Value = Convert.ToDecimal(Program.Ini.ReadInt("scoreMin", "Tags"));
                numTagsDownloadLimit.Value = Convert.ToDecimal(Program.Ini.ReadInt("imageLimit", "Tags"));
                numTagsPageLimit.Value = Convert.ToDecimal(Program.Ini.ReadInt("pageLimit", "Tags"));
                chkTagsSeparateNonImages.Checked = Program.Ini.ReadBool("separateNonImages", "Tags");

              // Pools
                txtPoolSchema.Text = apiTools.ReplaceIllegalCharacters(Program.Ini.ReadString("fileNameSchema", "Pools").ToLower());
                chkPoolsMergeBlacklistedImages.Checked = Program.Ini.ReadBool("mergeBlacklisted", "Pools");
                chkPoolsOpenAfterDownload.Checked = Program.Ini.ReadBool("openAfter", "Pools");
                chkPoolsAddToWishlistSilently.Checked = false;
                chkPoolsAddToWishlistSilently.Enabled = false;

              // Images
                txtImageSchema.Text = apiTools.ReplaceIllegalCharacters(Program.Ini.ReadString("fileNameSchema", "Images").ToLower());
                chkImagesSeparateRatings.Checked = Program.Ini.ReadBool("separateRatings", "Images");
                chkImagesSeparateBlacklisted.Checked = Program.Ini.ReadBool("separateBlacklisted", "Images");
                chkImagesSeparateArtists.Checked = Program.Ini.ReadBool("separateArtists", "Images");
                chkImagesUseForm.Checked = Program.Ini.ReadBool("useForm", "Images");
            }
            else {
              // General
                if (string.IsNullOrEmpty(General.Default.saveLocation)) {
                    txtSaveTo.Text = Environment.CurrentDirectory;
                }
                else {
                    txtSaveTo.Text = General.Default.saveLocation;
                }
                chkSaveInfoFiles.Checked = General.Default.saveInfo;
                chkSaveBlacklistedImages.Checked = General.Default.saveBlacklisted;
                chkIgnoreFinish.Checked = General.Default.ignoreFinish;
                chkSaveMetadata.Checked = General.Default.saveMetadata;
                chkSaveArtistMetadata.Checked = General.Default.saveArtistMetadata;
                chkSaveTagMetadata.Checked = General.Default.saveTagMetadata;

              // Tags
                txtTagSchema.Text = apiTools.ReplaceIllegalCharacters(Tags.Default.fileNameSchema.ToLower());
                chkTagsExplicit.Checked = Tags.Default.Explicit;
                chkTagsQuestionable.Checked = Tags.Default.Questionable;
                chkTagsSafe.Checked = Tags.Default.Safe;
                chkTagsSeparateRatings.Checked = Tags.Default.separateRatings;
                chkTagsEnableScoreLimit.Checked = Tags.Default.enableScoreMin;
                chkTagsIncludeScoreAsTag.Checked = Tags.Default.scoreAsTag;
                numTagsScoreLimit.Value = Convert.ToDecimal(Tags.Default.scoreMin);
                numTagsDownloadLimit.Value = Convert.ToDecimal(Tags.Default.imageLimit);
                numTagsPageLimit.Value = Convert.ToDecimal(Tags.Default.pageLimit);
                chkTagsSeparateNonImages.Checked = Tags.Default.separateNonImages;

              // Pools
                txtPoolSchema.Text = apiTools.ReplaceIllegalCharacters(Pools.Default.fileNameSchema.ToLower());
                chkPoolsMergeBlacklistedImages.Checked = Pools.Default.mergeBlacklisted;
                chkPoolsOpenAfterDownload.Checked = Pools.Default.openAfter;
                chkPoolsAddToWishlistSilently.Checked = Pools.Default.addWishlistSilent;

              // Images
                txtImageSchema.Text = apiTools.ReplaceIllegalCharacters(Images.Default.fileNameSchema.ToLower());
                chkImagesSeparateRatings.Checked = Images.Default.separateRatings;
                chkImagesSeparateBlacklisted.Checked = Images.Default.separateBlacklisted;
                chkImagesSeparateArtists.Checked = Images.Default.separateArtists;
                chkImagesUseForm.Checked = Images.Default.useForm;
            }
        }
        private void checkAdmin() {
            if (!Program.IsAdmin) {
                if (MessageBox.Show("This task requires re-running as administrator. Restart elevated?", "aphrodite", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    var exeName = Process.GetCurrentProcess().MainModule.FileName;
                    ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
                    startInfo.Verb = "runas";
                    startInfo.Arguments = "installProtocol";
                    Process.Start(startInfo);
                    Environment.Exit(0);
                }
            }
        }
        private void checkProtocols() {
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
        }
        private string createProtocolDir() {
            if (!string.IsNullOrWhiteSpace(General.Default.saveLocation)) {
                return General.Default.saveLocation;
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
                if (File.Exists(directory + "\\aphrodite.exe"))
                    File.Delete(directory + "\\aphrodite.exe");

                File.Copy(Environment.CurrentDirectory + "\\" + filename, directory + "\\aphrodite.exe");
            }

            // Create directories
            if (!Directory.Exists(directory + "\\Tags"))
                Directory.CreateDirectory(directory + "\\Tags");
            if (!Directory.Exists(directory + "\\Pools"))
                Directory.CreateDirectory(directory + "\\Pools");
            if (!Directory.Exists(directory + "\\Images"))
                Directory.CreateDirectory(directory + "\\Images");

            // Return selected directory
            return directory;
        }

        private void btnBrws_Click(object sender, EventArgs e) {
            if (Program.UseIni)
                return;

            Ookii.Dialogs.WinForms.VistaFolderBrowserDialog fbd = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog();
            //FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select a folder to store downloads" };
            if (!string.IsNullOrEmpty(txtSaveTo.Text)) {
                fbd.SelectedPath = txtSaveTo.Text;
            }

            if (fbd.ShowDialog() == DialogResult.OK) {
                txtSaveTo.Text = fbd.SelectedPath;
                changedSaveTo = true;
            }
        }
        private void btnSave_Click(object sender, EventArgs e) {
            saveSettings();
            if (SwitchTab)
                Environment.Exit(0);
            this.DialogResult = DialogResult.OK;
        }
        private void btnCancel_Click(object sender, EventArgs e) {
            if (SwitchTab)
                Environment.Exit(0);
            this.DialogResult = DialogResult.Cancel;
        }

        private void chkMinimumScore_CheckedChanged(object sender, EventArgs e) {
            numTagsScoreLimit.Enabled = chkTagsEnableScoreLimit.Checked;
            chkTagsIncludeScoreAsTag.Enabled = chkTagsEnableScoreLimit.Checked;
        }

        private void btnBlacklist_Click(object sender, EventArgs e) {
            frmBlacklist blackList = new frmBlacklist();
            if (blackList.ShowDialog() == DialogResult.OK) {
                General.Default.Save();
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
            btnProtocolInstallTags.Text = "'tags:' protocol installed";

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
            btnProtocolInstallPools.Text = "pool protocols installed";

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
            btnProtocolInstallImages.Text = "'images:' protocol installed";

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
            MessageBox.Show("You can use the system-based settings for aphrodite by changing \"useIni\" to \"False\".\nAlso, graylist & blacklist have spaces between tags and are separate files, so... keep that in mind.");
            string bufferINI = "[aphrodite]\nuseIni=True";
            File.WriteAllText(Environment.CurrentDirectory + "\\graylist.cfg", General.Default.blacklist);
            File.WriteAllText(Environment.CurrentDirectory + "\\blacklist.cfg", General.Default.zeroToleranceBlacklist);

            bufferINI += "\n\n[Global]";
            bufferINI += "\nsaveInfo=" + General.Default.saveInfo;
            bufferINI += "\nsaveBlacklisted=" + General.Default.saveBlacklisted;
            bufferINI += "\nignoreFinish=" + General.Default.ignoreFinish;
            //bufferINI += "\nsaveMetadata=" + General.Default.saveMetadata;
            //bufferINI += "\nsaveArtistMetadata=" + General.Default.saveArtistMetadata;
            //bufferINI += "\nsaveTagMetadata=" + General.Default.saveTagMetadata;

            bufferINI += "\n\n[Tags]";
            bufferINI += "\nfileNameSchema=" + apiTools.ReplaceIllegalCharacters(Tags.Default.fileNameSchema);
            bufferINI += "\nuseMinimumScore=" + Tags.Default.enableScoreMin;
            bufferINI += "\nscoreAsTag=" + Tags.Default.scoreAsTag;
            bufferINI += "\nscoreMin=" + Tags.Default.scoreMin;
            bufferINI += "\nimageLimit=" + Tags.Default.imageLimit;
            bufferINI += "\npageLimit=" + Tags.Default.pageLimit;
            bufferINI += "\nseparateRatings=" + Tags.Default.separateRatings;
            bufferINI += "\nseparateNonImages=" + Tags.Default.separateNonImages;
            bufferINI += "\nExplicit=" + Tags.Default.Explicit;
            bufferINI += "\nQuestionable=" + Tags.Default.Questionable;
            bufferINI += "\nSafe=" + Tags.Default.Safe;

            bufferINI += "\n\n[Pools]";
            bufferINI += "\nfileNameSchema=" + apiTools.ReplaceIllegalCharacters(Pools.Default.fileNameSchema);
            bufferINI += "\nmergeBlacklisted=" + Pools.Default.mergeBlacklisted;
            bufferINI += "\nopenAfter=" + Pools.Default.openAfter;

            bufferINI += "\n\n[Images]";
            bufferINI += "\nfileNameSchema=" + apiTools.ReplaceIllegalCharacters(Images.Default.fileNameSchema);
            bufferINI += "\nseparateRatings=" + Images.Default.separateRatings;
            bufferINI += "\nseparateBlacklisted=" + Images.Default.separateBlacklisted;
            bufferINI += "\nseparateNonImages=" + Images.Default.separateNonImages;
            bufferINI += "\nuseForm=" + Images.Default.useForm;

            bufferINI += "\n\n[Forms]";
            bufferINI += "\nfrmMainLocation=" + FormSettings.Default.frmMainLocation.X + ", " + FormSettings.Default.frmMainLocation.Y;

            File.WriteAllText(Environment.CurrentDirectory + "\\aphrodite.ini", bufferINI);

            //FileInfo fI = new FileInfo(Environment.CurrentDirectory + "\\aphrodite.ini");
            //fI.IsReadOnly = true;
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
                    break;
            }
        }
    }
}
