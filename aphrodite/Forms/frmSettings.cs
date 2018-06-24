using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmSettings : Form {
        #region Variables
        public bool pluginChange = false;   // is the form being changed from the userscript?
        public int plugin = 0;              // Used to determine the tab that will be selected on boot.

        public bool protocol = false;       // is installprotocol triggered on startup?
        public bool isAdmin = false;        // is the user admin?

        public bool tags = false;           // is the tags protocol installed?
        public bool pools = false;          // is the pools protocol installed?
        public bool poolwl = false;         // is the poolwl protocol installed?
        public bool images = false;         // is the images protocol installed?
        public bool noprotocols = true;     // Are there no protocols installed?

        public bool useIni = false;         // Determine if the ini file will be used.

        IniFile ini = new IniFile();
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

            Settings.Default.Reload();
            Tags.Default.Reload();
            Pools.Default.Reload();
            Images.Default.Reload();
        }
        private void frmSettings_Load(object sender, EventArgs e) {
            loadSettings();
            if (useIni){
                tbMain.TabPages.Remove(tbProtocol);
                tbMain.TabPages.Remove(tbPortable);
            }
            else
                checkProtocols();

            if (pluginChange) {
                switch (plugin) {
                    case 0:
                        tbMain.SelectedTab = tbGeneral;
                        break;
                    case 1:
                        tbMain.SelectedTab = tbTags;
                        break;
                    case 2:
                        tbMain.SelectedTab = tbPools;
                        break;
                    case 3:
                        tbMain.SelectedTab = tbImages;
                        break;
                }
            }

            if (tags == true) {
                btnTagsProtocol.Enabled = false;
                btnTagsProtocol.Text = "'tags:' protocol installed";
            }
            else {
                if (!isAdmin)
                    UACShield(btnTagsProtocol);
            }

            if (pools == true && poolwl == true) {
                btnPoolsProtocol.Enabled = false;
                btnPoolsProtocol.Text = "pool protocols installed";
            }
            else {
                if (!isAdmin)
                    UACShield(btnPoolsProtocol);
            }

            if (images == true) {
                btnImagesProtocol.Enabled = false;
                btnImagesProtocol.Text = "'images:' protocol installed";
            }
            else {
                if (!isAdmin)
                    UACShield(btnImagesProtocol);
            }

            if (protocol) {
                tbMain.SelectedTab = tbProtocol;
            }
        }

        private void saveSettings() {
            if (useIni) {
                FileInfo fI = new FileInfo(Environment.CurrentDirectory + "\\aphrodite.ini");
                fI.IsReadOnly = false;

                // General
                //ini.WriteString("saveTo", "@");
                ini.WriteBool("saveInfo", chkSaveInfo.Checked, "Global");
                ini.WriteBool("saveBlacklisted", chkSaveBlacklisted.Checked, "Global");
                ini.WriteBool("ignoreFinish", chkIgnoreFinish.Checked, "Global");

                // Tags
                ini.WriteBool("Explicit", chkExplicit.Checked, "Tags");
                ini.WriteBool("Questionable", chkQuestionable.Checked, "Tags");
                ini.WriteBool("Safe", chkSafe.Checked, "Tags");
                ini.WriteBool("separateRatings", chkSeparate.Checked, "Tags");
                ini.WriteBool("useMinimumScore", chkMinimumScore.Checked, "Tags");
                ini.WriteBool("scoreAsTag", chkScoreAsTag.Checked, "Tags");
                ini.WriteInt("scoreMin", Convert.ToInt32(numScore.Value), "Tags");
                ini.WriteInt("imageLimit", Convert.ToInt32(numLimit.Value), "Tags");
                ini.WriteBool("usePageLimit", chkPageLimit.Checked, "Tags");
                ini.WriteInt("pageLimit", Convert.ToInt32(numLimit.Value), "Tags");

                // Pools
                ini.WriteBool("usePoolName", chkPoolName.Checked, "Pools");
                ini.WriteBool("mergeBlacklisted", chkMerge.Checked, "Pools");
                ini.WriteBool("openAfter", chkOpen.Checked, "Pools");

                // Images
                if (rbMD5.Checked)
                    ini.WriteInt("fileNameCode", 0, "Images");
                else
                    ini.WriteInt("fileNameCode", 1, "Images");
                ini.WriteBool("separateRatings", chkSeparateImages.Checked, "Images");
                ini.WriteBool("separateBlacklisted", chkSeparateBlacklisted.Checked, "Images");
                ini.WriteBool("useForm", chkUseForm.Checked, "Images");
            }
            else {
                // General
                Settings.Default.saveLocation = txtSaveTo.Text;
                Settings.Default.saveInfo = chkSaveInfo.Checked;
                Settings.Default.saveBlacklisted = chkSaveBlacklisted.Checked;
                Settings.Default.ignoreFinish = chkIgnoreFinish.Checked;

                // Tags
                Tags.Default.Explicit = chkExplicit.Checked;
                Tags.Default.Questionable = chkQuestionable.Checked;
                Tags.Default.Safe = chkSafe.Checked;
                Tags.Default.separateRatings = chkSeparate.Checked;
                Tags.Default.enableScoreMin = chkMinimumScore.Checked;
                Tags.Default.scoreAsTag = chkScoreAsTag.Checked;
                Tags.Default.scoreMin = Convert.ToInt32(numScore.Value);
                Tags.Default.imageLimit = Convert.ToInt32(numLimit.Value);
                Tags.Default.usePageLimit = chkPageLimit.Checked;
                Tags.Default.pageLimit = Convert.ToInt32(numPageLimit.Value);

                // Pools
                Pools.Default.usePoolName = chkPoolName.Checked;
                Pools.Default.mergeBlacklisted = chkMerge.Checked;
                Pools.Default.openAfter = chkOpen.Checked;
                Pools.Default.addWishlistSilent = chkAddWishlistSilent.Checked;

                // Images
                if (rbMD5.Checked)
                    Images.Default.fileNameCode = 0;
                else
                    Images.Default.fileNameCode = 1;
                Images.Default.separateRatings = chkSeparateImages.Checked;
                Images.Default.separateBlacklisted = chkSeparateBlacklisted.Checked;
                Images.Default.useForm = chkUseForm.Checked;

                // Save all 4
                Settings.Default.Save();
                Tags.Default.Save();
                Pools.Default.Save();
                Images.Default.Save();
            }
        }
        private void loadSettings() {
            if (useIni) {
                // General
                txtSaveTo.Text = Environment.CurrentDirectory;
                txtSaveTo.Enabled = false;
                btnBrws.Enabled = false;
                chkSaveInfo.Checked = ini.ReadBool("saveInfo", "Global");
                chkSaveBlacklisted.Checked = ini.ReadBool("saveBlacklisted", "Global");
                chkIgnoreFinish.Checked = ini.ReadBool("ignoreFinish", "Global");

                // Tags
                chkExplicit.Checked = ini.ReadBool("Explicit", "Tags");
                chkQuestionable.Checked = ini.ReadBool("Questionable", "Tags");
                chkSafe.Checked = ini.ReadBool("Safe", "Tags");
                chkSeparate.Checked = ini.ReadBool("separateRatings", "Tags");
                chkMinimumScore.Checked = ini.ReadBool("useMinimumScore", "Tags");
                chkScoreAsTag.Checked = ini.ReadBool("scoreAsTag", "Tags");
                numScore.Value = Convert.ToDecimal(ini.ReadInt("scoreMin", "Tags"));
                numLimit.Value = Convert.ToDecimal(ini.ReadInt("imageLimit", "Tags"));
                chkPageLimit.Checked = ini.ReadBool("usePageLimit", "Tags");
                numPageLimit.Value = Convert.ToDecimal(ini.ReadInt("pageLimit", "Tags"));

                // Pools
                chkPoolName.Checked = ini.ReadBool("usePoolName", "Pools");
                chkMerge.Checked = ini.ReadBool("mergeBlacklisted", "Pools");
                chkOpen.Checked = ini.ReadBool("openAfter", "Pools");
                chkAddWishlistSilent.Checked = false;
                chkAddWishlistSilent.Enabled = false;

                // Images
                switch (ini.ReadInt("fileNameCode", "Images")) {
                    case 0:
                        rbMD5.Checked = true;
                        break;
                    case 1:
                        rbArtist.Checked = true;
                        break;
                    default:
                        rbArtist.Checked = true;
                        break;
                }
                chkSeparateImages.Checked = ini.ReadBool("separateRatings", "Images");
                chkSeparateBlacklisted.Checked = ini.ReadBool("separateBlacklisted", "Images");
                chkUseForm.Checked = ini.ReadBool("useForm", "Images");
            }
            else {
                // General
                txtSaveTo.Text = Settings.Default.saveLocation;
                chkSaveInfo.Checked = Settings.Default.saveInfo;
                chkSaveBlacklisted.Checked = Settings.Default.saveBlacklisted;
                chkIgnoreFinish.Checked = Settings.Default.ignoreFinish;

                // Tags
                chkExplicit.Checked = Tags.Default.Explicit;
                chkQuestionable.Checked = Tags.Default.Questionable;
                chkSafe.Checked = Tags.Default.Safe;
                chkSeparate.Checked = Tags.Default.separateRatings;
                chkMinimumScore.Checked = Tags.Default.enableScoreMin;
                chkScoreAsTag.Checked = Tags.Default.scoreAsTag;
                numScore.Value = Convert.ToDecimal(Tags.Default.scoreMin);
                numLimit.Value = Convert.ToDecimal(Tags.Default.imageLimit);
                chkPageLimit.Checked = Tags.Default.usePageLimit;
                numPageLimit.Value = Convert.ToDecimal(Tags.Default.pageLimit);

                // Pools
                chkPoolName.Checked = Pools.Default.usePoolName;
                chkMerge.Checked = Pools.Default.mergeBlacklisted;
                chkOpen.Checked = Pools.Default.openAfter;
                chkAddWishlistSilent.Checked = Pools.Default.addWishlistSilent;

                // Images
                switch (Images.Default.fileNameCode) {
                    case 0:
                        rbMD5.Checked = true;
                        break;
                    case 1:
                        rbArtist.Checked = true;
                        break;
                    default:
                        rbArtist.Checked = true;
                        break;
                }
                chkSeparateImages.Checked = Images.Default.separateRatings;
                chkSeparateBlacklisted.Checked = Images.Default.separateBlacklisted;
                chkUseForm.Checked = Images.Default.useForm;
            }
        }
        private void checkAdmin() {
            if (!isAdmin) {
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

            if (Registry.ClassesRoot.GetValue("tags", "URL Protocol") == null || keyTags == null)
                tags = false;
            else
                tags = true;

            if (Registry.ClassesRoot.GetValue("pools", "URL Protocol") == null || keyPools == null)
                pools = false;
            else
                pools = true;

            if (Registry.ClassesRoot.GetValue("poolwl", "URL Protocol") == null || keyPoolWl == null)
                poolwl = false;
            else
                poolwl = true;

            if (Registry.ClassesRoot.GetValue("images", "URL Protocol") == null || keyImages == null)
                images = false;
            else
                images = true;

            if (tags == false && pools == false && images == false)
                noprotocols = true;
            else 
                noprotocols = false;
        }
        private string createProtocolDir() {
            if (!string.IsNullOrWhiteSpace(Settings.Default.saveLocation)) {
                return Settings.Default.saveLocation;
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
            if (useIni)
                return;

            FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select a folder to store downloads" };
            if (!string.IsNullOrEmpty(txtSaveTo.Text))
                fbd.SelectedPath = txtSaveTo.Text;

            if (fbd.ShowDialog() == DialogResult.OK)
                txtSaveTo.Text = fbd.SelectedPath;
        }
        private void btnSave_Click(object sender, EventArgs e) {
            saveSettings();
            if (pluginChange)
                Environment.Exit(0);
            this.DialogResult = DialogResult.OK;
        }
        private void btnCancel_Click(object sender, EventArgs e) {
            if (pluginChange)
                Environment.Exit(0);
            this.DialogResult = DialogResult.Cancel;
        }

        private void chkMinimumScore_CheckedChanged(object sender, EventArgs e) {
            numScore.Enabled = chkMinimumScore.Checked;
            chkScoreAsTag.Enabled = chkMinimumScore.Checked;
        }
        private void chkPageLimit_CheckedChanged(object sender, EventArgs e) {
            numPageLimit.Enabled = chkPageLimit.Checked;
        }

        private void btnBlacklist_Click(object sender, EventArgs e) {
            frmBlacklist blackList = new frmBlacklist();
            blackList.useIni = useIni;
            if (blackList.ShowDialog() == DialogResult.OK) {
                Settings.Default.Save();
            }
        }

        private void btnTagsProtocol_Click(object sender, EventArgs e) {
            checkAdmin();

            if (noprotocols) {
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

            btnTagsProtocol.Enabled = false;
            btnTagsProtocol.Text = "'tags:' protocol installed";

            tags = true;
            noprotocols = false;
        }
        private void btnPoolsProtocol_Click(object sender, EventArgs e) {
            checkAdmin();

            if (noprotocols) {
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

                if (!pools) {
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

                if (!poolwl) {
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

            btnPoolsProtocol.Enabled = false;
            btnPoolsProtocol.Text = "pool protocols installed";

            pools = true;
            poolwl = true;
            noprotocols = false;
        }
        private void btnImagesProtocol_Click(object sender, EventArgs e) {
            checkAdmin();

            if (noprotocols) {
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

            btnImagesProtocol.Enabled = false;
            btnImagesProtocol.Text = "'images:' protocol installed";

            images = true;
            noprotocols = false;
        }

        private void btnUserscript_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/murrty/aphrodite/raw/master/Resources/aphrodite.user.js");
        }
        private void btnImagesUserscript_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/murrty/aphrodite/raw/master/Resources/aphrodite.images.user.js");
        }

        private void btnExportIni_Click(object sender, EventArgs e) {
            MessageBox.Show("You can use the system-based settings for aphrodite by changing \"useIni\" to \"False\". That requires the ini to be unset as read-only.\nAlso, graylist & blacklist have spaces between tags and are separate files, so... keep that in mind.");
            string bufferINI = "[aphrodite]\nuseIni=True";
            File.WriteAllText(Environment.CurrentDirectory + "\\graylist.cfg", Settings.Default.blacklist);
            File.WriteAllText(Environment.CurrentDirectory + "\\blacklist.cfg", Settings.Default.zeroToleranceBlacklist);
            //bufferINI += "\nsaveTo=@";

            bufferINI += "\n[Global]";
            bufferINI += "\nsaveInfo=" + Settings.Default.saveInfo;
            bufferINI += "\nsaveBlacklisted=" + Settings.Default.saveBlacklisted;
            bufferINI += "\nignoreFinish=" + Settings.Default.ignoreFinish;

            bufferINI += "\n[Tags]";
            bufferINI += "\nuseMinimumScore=" + Tags.Default.enableScoreMin;
            bufferINI += "\nscoreAsTag=" + Tags.Default.scoreAsTag;
            bufferINI += "\nscoreMin=" + Tags.Default.scoreMin;
            bufferINI += "\nimageLimit=" + Tags.Default.imageLimit;
            bufferINI += "\nusePageLimit=" + Tags.Default.usePageLimit;
            bufferINI += "\npageLimit=" + Tags.Default.pageLimit;
            bufferINI += "\nseparateRatings=" + Tags.Default.separateRatings;
            bufferINI += "\nExplicit=" + Tags.Default.Explicit;
            bufferINI += "\nQuestionable=" + Tags.Default.Questionable;
            bufferINI += "\nSafe=" + Tags.Default.Safe;

            bufferINI += "\n[Pools]";
            bufferINI += "\nusePoolName=" + Pools.Default.usePoolName;
            bufferINI += "\nmergeBlacklisted=" + Pools.Default.mergeBlacklisted;
            bufferINI += "\nopenAfter=" + Pools.Default.openAfter;

            bufferINI += "\n[Images]";
            bufferINI += "\nfileNameCode=" + Images.Default.fileNameCode;
            bufferINI += "\nseparateRatings=" + Images.Default.separateRatings;
            bufferINI += "\nseparateBlacklisted=" + Images.Default.separateBlacklisted;
            bufferINI += "\nuseForm=" + Images.Default.useForm;

            File.WriteAllText(Environment.CurrentDirectory + "\\aphrodite.ini", bufferINI);

            //FileInfo fI = new FileInfo(Environment.CurrentDirectory + "\\aphrodite.ini");
            //fI.IsReadOnly = true;
        }
    }
}
