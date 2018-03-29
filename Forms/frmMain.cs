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
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmMain : Form {
        #region Variables
        bool isAdmin = false;
        #endregion

        #region Methods
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, string lp);
        private void SetTextBoxHint(IntPtr TextboxHandle, string Hint) {
            SendMessage(TextboxHandle, 0x1501, (IntPtr)1, Hint);
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        public static void UACShield(Button btn) {
            const Int32 BCM_SETSHIELD = 0x160C;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            SendMessage(btn.Handle, BCM_SETSHIELD, 0, 1);
        }
        #endregion

        #region Form
        public frmMain() {
            InitializeComponent();
            if (!(new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator))
                isAdmin = false;
            else
                isAdmin = true;

            SetTextBoxHint(txtTags.Handle, "Tags to download...");
            SetTextBoxHint(txtID.Handle, "Pool ID...");
        }
        private void frmMain_Load(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(Settings.Default.saveLocation) || string.IsNullOrEmpty(Settings.Default.saveLocation))
                Settings.Default.saveLocation = Environment.CurrentDirectory;

            for (int i = 1; i < Environment.GetCommandLineArgs().Length; i++) {
                string arg = Environment.GetCommandLineArgs()[i];
                if (arg.StartsWith("installProtocol")) {
                    frmSettings settings = new frmSettings();
                    settings.isAdmin = isAdmin;
                    settings.protocol = true;
                    settings.ShowDialog();
                }
                if (arg.StartsWith("pools:configuresettings") || arg.StartsWith("tags:configuresettings") || arg.StartsWith("images:configuresettings") || arg.StartsWith("configuresettings")) {
                    frmSettings settings = new frmSettings();
                    settings.pluginChange = true;
                    settings.isAdmin = isAdmin;
                    if (arg.StartsWith("tags:"))
                        settings.plugin = 1;
                    else if (arg.StartsWith("pools:"))
                        settings.plugin = 2;
                    else if (arg.StartsWith("images:"))
                        settings.plugin = 3;
                    else
                        settings.plugin = 0;
                    settings.ShowDialog();
                    Environment.Exit(0);
                }
                if (arg.StartsWith("pools:") && isValidPoolLink(arg.Replace("pools:", ""))) {
                    frmPoolDownloader poolDL = new frmPoolDownloader();
                    poolDL.fromURL = true;
                    poolDL.poolurl = arg.Replace("pools:", "");
                    poolDL.ShowDialog();
                    Environment.Exit(0);
                }
                else if (arg.StartsWith("tags:") && isValidTagLink(arg.Replace("tags:", ""))) {
                    frmTagDownloader tagDL = new frmTagDownloader();
                    tagDL.fromURL = true;
                    tagDL.url = arg.Replace("tags:", "");
                    tagDL.saveInfo = Settings.Default.saveInfo;
                    tagDL.blacklistedTags = Settings.Default.blacklist;
                    tagDL.ratings = "e q s".Split(' ');
                    tagDL.ShowDialog();
                    Environment.Exit(0);
                }
                else if (arg.StartsWith("images:") && isValidImageLink(arg.Replace("images:", ""))) {
                    if (Images.Default.useForm) {
                        frmImageDownloader imgdl = new frmImageDownloader();
                        imgdl.saveInfo = Settings.Default.saveInfo;
                        imgdl.saveTo = Settings.Default.saveLocation + "\\Images";
                        imgdl.header = Program.UserAgent;
                        imgdl.url = arg.Replace("images:", "");
                        imgdl.ShowDialog();
                        Environment.Exit(0);
                    }
                    else {
                        ImageDownloader imgdl = new ImageDownloader();
                        imgdl.saveInfo = Settings.Default.saveInfo;
                        imgdl.saveTo = Settings.Default.saveLocation + "\\Images";
                        imgdl.header = Program.UserAgent;
                        if (imgdl.downloadImage(arg.Replace("images:", ""))) {
                            if (!Settings.Default.ignoreFinish)
                                MessageBox.Show("Image has finished downloading.");
                            Environment.Exit(0);
                        }
                        else {
                            MessageBox.Show("An error occured while downloading the image.");
                            Environment.Exit(0);
                        }
                    }
                }
                else if (arg.StartsWith("tags:")) {
                    txtTags.Text += arg.Replace("tags:", "").Replace("%20", " ") + " ";
                }
                else {
                    if (File.Exists(arg)) {
                        tbMain.SelectedIndex = 2;
                    }
                    else {
                        txtTags.Text += arg.Replace("%20", " ") + " ";
                    }
                }

                if (txtTags.Text.StartsWith(" ")) {
                    txtTags.Text = txtTags.Text.TrimStart(' ');
                }

                if (txtTags.Text.EndsWith(" ")) {
                    txtTags.Text = txtTags.Text.TrimEnd(' ');
                }
            }

            chkExplicit.Checked = Tags.Default.Explicit;
            chkQuestionable.Checked = Tags.Default.Questionable;
            chkSafe.Checked = Tags.Default.Safe;
            chkMinimumScore.Checked = Tags.Default.enableScoreMin;
            numScore.Value = Convert.ToDecimal(Tags.Default.scoreMin);
            numLimit.Value = Convert.ToDecimal(Tags.Default.imageLimit);

            chkOpen.Checked = Pools.Default.openAfter;
            chkMerge.Checked = Pools.Default.mergeBlacklisted;

            RegistryKey keyPools = Registry.ClassesRoot.OpenSubKey("pools\\shell\\open\\command", false);
            RegistryKey keyTags = Registry.ClassesRoot.OpenSubKey("tags\\shell\\open\\command", false);
            if (keyPools == null || keyTags == null) {
                mProtocol.Visible = true;
                mProtocol.Enabled = true;
            }
        }
        private void frmMain_Shown(object sender, EventArgs e) {
            txtTags.Focus();
        }
        private void tbMain_SelectedIndexChanged(object sender, EventArgs e) {
            if (tbMain.SelectedTab == tbTags) {
                txtTags.Focus();
            }
            else if (tbMain.SelectedTab == tbPools) {
                txtID.Focus();
            }
        }

        private void mSettings_Click(object sender, EventArgs e) {
            frmSettings settings = new frmSettings();
            settings.isAdmin = isAdmin;
            settings.ShowDialog();
            chkExplicit.Checked = Tags.Default.Explicit;
            chkQuestionable.Checked = Tags.Default.Questionable;
            chkSafe.Checked = Tags.Default.Safe;
            chkMinimumScore.Checked = Tags.Default.enableScoreMin;
            numScore.Value = Convert.ToDecimal(Tags.Default.scoreMin);
            numLimit.Value = Convert.ToDecimal(Tags.Default.imageLimit);

            chkOpen.Checked = Pools.Default.openAfter;
            chkMerge.Checked = Pools.Default.mergeBlacklisted;
        }
        private void mBlacklist_Click(object sender, EventArgs e) {
            frmBlacklist blackList = new frmBlacklist();
            blackList.ShowDialog();
        }
        private void mAbout_Click(object sender, EventArgs e) {
            frmAbout frAbout = new frmAbout();
            frAbout.Show();
        }
        private void mProtocol_Click(object sender, EventArgs e) {
            frmSettings settings = new frmSettings();
            settings.isAdmin = isAdmin;
            settings.protocol = true;
            settings.ShowDialog();
        }
        private void btnHLQ_Click(object sender, EventArgs e) {
            Process.Start("https://iqdb.harry.lu/");
        }
        #endregion

        #region Tags
        public static bool isValidTagLink(string url) {
            if (url.StartsWith("http://e621.net/post/index/") || url.StartsWith("https://e621.net/post/index/") || url.StartsWith("http://www.e621.net/post/index/") || url.StartsWith("https://www.e621.net/post/index/") || url.StartsWith("e621.net/post/index/") || url.StartsWith("www.e621.net/post/index/") || url.StartsWith("https://e621.net/post?tags=") || url.StartsWith("https://e621.net/post?tags=") || url.StartsWith("http://www.e621.net/post?tags=") || url.StartsWith("https://www.e621.net/post?tags=") || url.StartsWith("e621.net/post?tags=") || url.StartsWith("www.e621.net/post?tags=")) {
                return true;
            }
            else {
                return false;
            }
        }

        private void txtTags_KeyPress(object sender, KeyPressEventArgs e) {
            // Enforce 6 tag limit
            if (txtTags.Text.Count(x => x == ' ') >= 5 && e.KeyChar != (char)8 && e.KeyChar == (char)Keys.Space && txtTags.SelectionLength != txtTags.TextLength) {
                e.Handled = true;
            }
            else {
                e.Handled = false;
            }
        }
        private void txtTags_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter)
                btnDownloadTags_Click(this, new EventArgs());
        }

        private void btnDownloadTags_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtTags.Text)) {
                MessageBox.Show("Please specify tags to download.");
                return;
            }
            if (string.IsNullOrWhiteSpace(Settings.Default.saveLocation)) {
                MessageBox.Show("Please configure a save location to download images to.");
                return;
            }

            if (numLimit.Value == 0) {
                if (MessageBox.Show("Downloading won't be limited. This may take a long while or even blacklist you. Continue anyway?", "TagDownloader", MessageBoxButtons.YesNo) == DialogResult.No) {
                    return;
                }
            }

            if (txtTags.Text.StartsWith(" ")) {
                txtTags.Text = txtTags.Text.TrimStart(' ');
            }
            if (txtTags.Text.EndsWith(" ")) {
                txtTags.Text = txtTags.Text.TrimEnd(' ');
            }

            if (txtTags.Text.Contains("/")) {
                txtTags.Text = txtTags.Text.Replace("/", "%25-2F");
            }
            
            string[] length = txtTags.Text.Split(' ');
            if (length.Length > 6) {
                MessageBox.Show("6 tags is the maximum length you're allowed to download from e621. If your tag has a space between words, be sure to add an underscore. (_)");
                return;
            }

            string dlTags = txtTags.Text;
            string ratings = string.Empty;
            if (chkExplicit.Checked)
                ratings += "e ";
            if (chkQuestionable.Checked)
                ratings += "q ";
            if (chkSafe.Checked)
                ratings += "s ";

            if (string.IsNullOrWhiteSpace(ratings)) {
                MessageBox.Show("Please specify the ratings you want for images");
                return;
            }

            if (ratings.EndsWith(" "))
                ratings = ratings.TrimEnd(' ');

            Tags.Default.Explicit = chkExplicit.Checked;
            Tags.Default.Questionable = chkQuestionable.Checked;
            Tags.Default.Safe = chkSafe.Checked;
            Tags.Default.enableScoreMin = chkMinimumScore.Checked;
            Tags.Default.scoreMin = Convert.ToInt32(numScore.Value);
            Tags.Default.imageLimit = Convert.ToInt32(numLimit.Value);
            Tags.Default.Save();

            frmTagDownloader tagDL = new frmTagDownloader();
            tagDL.tags = txtTags.Text;
            tagDL.openAfter = false;
            tagDL.fromURL = false;
            if (chkMinimumScore.Checked) {
                tagDL.useMinimumScore = true;
                if (numScore.Value > 0) {
                    tagDL.minimumScore = Convert.ToInt32(numScore.Value);
                }
                else {
                    tagDL.minimumScore = 0;
                }
            }
            tagDL.imageAmount = Convert.ToInt32(numLimit.Value);
            tagDL.saveInfo = Settings.Default.saveInfo;
            tagDL.blacklistedTags = Settings.Default.blacklist;
            tagDL.zblacklistedTags = Settings.Default.zeroToleranceBlacklist;
            tagDL.ratings = ratings.Split(' ');
            tagDL.Show();

            txtTags.Clear();
        }

        private void chkMinimumScore_CheckedChanged(object sender, EventArgs e) {
            numScore.Enabled = chkMinimumScore.Checked;
        }
        #endregion

        #region Pools
        public static bool isValidPoolLink(string url) {
            if (url.StartsWith("http://e621.net/pool/show/") || url.StartsWith("https://e621.net/pool/show/") || url.StartsWith("http://www.e621.net/pool/show/") || url.StartsWith("https://www.e621.net/pool/show/") || url.StartsWith("e621.net/pool/show/") || url.StartsWith("www.e621.net/pool/show/")) {
                return true;
            }
            else {
                return false;
            }
        }
        
        private void txtID_TextChanged(object sender, EventArgs e) {
            var txtSender = (TextBox)sender;
            var curPos = txtSender.SelectionStart;
            txtSender.Text = Regex.Replace(txtSender.Text, "[^0-9]", "");
            txtSender.SelectionStart = curPos;
        }
        private void txtID_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter)
                btnDownloadPool_Click(this, new EventArgs());
        }

        private void btnDownloadPool_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtID.Text)) {
                MessageBox.Show("Please specify tags to download.");
                return;
            }
            if (string.IsNullOrWhiteSpace(Settings.Default.saveLocation)) {
                MessageBox.Show("Please configure a save location to download images to.");
                return;
            }

            Pools.Default.openAfter = chkOpen.Checked;
            Pools.Default.mergeBlacklisted = chkMerge.Checked;
            Pools.Default.Save();

            frmPoolDownloader poolDL = new frmPoolDownloader();
            poolDL.id = txtID.Text;
            poolDL.openAfter = chkOpen.Checked;
            poolDL.Show();

            txtID.Clear();
        }
        #endregion

        #region Images
        public static bool isValidImageLink(string url) {
            if (url.StartsWith("http://e621.net/post/show/") || url.StartsWith("https://e621.net/post/show/") || url.StartsWith("http://www.e621.net/post/show/") || url.StartsWith("https://www.e621.net/post/show/") || url.StartsWith("e621.net/post/show/") || url.StartsWith("www.e621.net/post/show/")) {
                return true;
            }
            else {
                return false;
            }
        }
        #endregion
    }
}
