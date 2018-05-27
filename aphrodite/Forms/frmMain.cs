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

        // Valid protocols:
        //                  'pools:'
        //                  'tags:'
        //                  'images:'
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

            for (int i = 1; i < Environment.GetCommandLineArgs().Length; i++) { // count each runtime argument, int 0 = boot directory.
                string arg = Environment.GetCommandLineArgs()[i]; // buffer for the arg
                if (arg.StartsWith("installProtocol")) { // if the argument is installProtocol
                    frmSettings settings = new frmSettings(); // start the protocol section of the settings.
                    settings.isAdmin = isAdmin;
                    settings.protocol = true;
                    settings.ShowDialog();
                }
                if (arg.StartsWith("pools:configuresettings") || arg.StartsWith("tags:configuresettings") || arg.StartsWith("images:configuresettings") || arg.StartsWith("configuresettings")) { // check for configure settings argument
                    frmSettings settings = new frmSettings(); // configure settings argument was passed
                    settings.pluginChange = true; // boolean to switch to the tab on load
                    settings.isAdmin = isAdmin; // sets isAdmin for the protocol.
                    if (arg.StartsWith("tags:")) // if it's changing tag settings
                        settings.plugin = 1;
                    else if (arg.StartsWith("pools:")) // if it's changing pool settings
                        settings.plugin = 2;
                    else if (arg.StartsWith("images:")) // if it's changing image settings
                        settings.plugin = 3;
                    else // otherwise just use the first tab
                        settings.plugin = 0;
                    settings.ShowDialog();
                    Environment.Exit(0);
                }
                if (arg.StartsWith("poolwl:")) {
                    // Add to pool wishlist
                    if (arg.StartsWith("poolwl:showwl")) {
                        frmPoolWishlist wl = new frmPoolWishlist();
                        wl.ShowDialog();
                        Environment.Exit(0);
                    }
                    else if (Pools.Default.addWishlistSilent) {
                        string[] split = arg.Split('$');
                        string url = split[0].Replace("poolwl:", "");
                        if (Pools.Default.wishlist.Contains(url)) {
                            Environment.Exit(0);
                        }
                        string title = string.Empty;

                        if (split.Length > 2) {
                            for (int j = 1; j < split.Length; j++) {
                                title += split[j] + "$";
                            }
                            title = title.TrimEnd('$');
                            title = title.Replace("%20", " ");
                            title = title.TrimEnd(' ');
                        }
                        else {
                            title = split[1].Replace("%20", " ");
                            title = title.TrimEnd(' ');
                        }

                        Pools.Default.wishhlistNames += "|" + title;
                        Pools.Default.wishlist += "|" + url;
                        Pools.Default.Save();
                    }
                    else {
                        frmPoolWishlist wl = new frmPoolWishlist();
                        wl.addToWishlist = true;
                        string[] split = arg.Split('$'); // Split into an array using the split identifier
                        wl.addURL = split[0].Replace("poolwl:", ""); // set the url from the first split
                        string title = string.Empty; // buffer for pool name

                        if (split.Length > 2) { // there is more than 1 split identifier in the argument
                            for (int j = 1; j < split.Length; j++) { // start count at int 1, as int 0 is the first split
                                title += split[j] + "$"; // join the identified parts past the initial split, for any pools with "$" in the title.
                            }
                            title = title.TrimEnd('$'); // trim the excess
                            title = title.Replace("%20", " "); // replace the %20 with spaces
                            title = title.TrimEnd(' '); // trim the excess
                        }
                        else { // only 1 split, for the url and title
                            title = split[1].Replace("%20", " "); // replace the %20 with spaces
                            title = title.TrimEnd(' '); // trim the excess
                        }

                        wl.addTitle = title; // set the title
                        wl.ShowDialog();
                    }
                    Environment.Exit(0);
                }
                if (arg.StartsWith("pools:") && apiTools.isValidPoolLink(arg.Replace("pools:", ""))) {
                    string poolID = arg.Replace("pools:", "").Replace("http://","https://").Replace("www.","");
                    if (!poolID.StartsWith("https://"))
                        poolID = "https://" + poolID;
                    if (poolID.Contains("?"))
                        poolID = poolID.Split('?')[0];
                    poolID = poolID.Split('/')[5];

                    frmPoolDownloader poolDL = new frmPoolDownloader();
                    poolDL.poolID = poolID;

                    poolDL.header = Program.UserAgent;
                    poolDL.saveTo = Settings.Default.saveLocation;
                    poolDL.graylist = Settings.Default.blacklist;
                    poolDL.blacklist = Settings.Default.zeroToleranceBlacklist;

                    poolDL.saveInfo = Settings.Default.saveInfo;
                    poolDL.ignoreFinish = Settings.Default.ignoreFinish;
                    poolDL.saveBlacklisted = Settings.Default.saveBlacklisted;

                    poolDL.usePoolName = Pools.Default.usePoolName;
                    poolDL.mergeBlacklisted = Pools.Default.mergeBlacklisted;
                    poolDL.openAfter = Pools.Default.openAfter;

                    poolDL.ShowDialog();
                    Environment.Exit(0);
                }
                else if (arg.StartsWith("tags:")) {
                    if (apiTools.isValidPostLink(arg.Replace("tags:", ""))) {
                        frmTagDownloader tagDL = new frmTagDownloader();
                        tagDL.fromURL = true;
                        tagDL.downloadUrl = arg.Replace("tags:", "");
                        tagDL.graylist = Settings.Default.blacklist;
                        tagDL.blacklist = Settings.Default.zeroToleranceBlacklist;
                        tagDL.saveTo = Settings.Default.saveLocation;
                        tagDL.saveInfo = Settings.Default.saveInfo;
                        tagDL.openAfter = false;
                        tagDL.saveBlacklistedFiles = Settings.Default.saveBlacklisted;
                        tagDL.ignoreFinish = Settings.Default.ignoreFinish;
                        tagDL.useMinimumScore = Tags.Default.enableScoreMin;
                        if (tagDL.useMinimumScore) {
                            tagDL.minimumScore = Tags.Default.scoreMin;
                            tagDL.scoreAsTag = Tags.Default.scoreAsTag;
                        }
                        if (Tags.Default.imageLimit > 0)
                            tagDL.imageLimit = Tags.Default.imageLimit;
                        tagDL.usePageLimit = Tags.Default.usePageLimit;
                        if (tagDL.usePageLimit)
                            tagDL.pageLimit = Tags.Default.pageLimit;
                        tagDL.separateRatings = Tags.Default.separateRatings;
                        if (tagDL.separateRatings) {
                            string ratings = string.Empty;
                            if (Tags.Default.Explicit)
                                ratings += "e ";
                            if (Tags.Default.Questionable)
                                ratings += "q ";
                            if (Tags.Default.Safe)
                                ratings += "s ";
                            ratings.TrimEnd(' ');
                            tagDL.ratings = ratings.Split(' ');
                        }
                        tagDL.webHeader = Program.UserAgent;
                        tagDL.ShowDialog();
                        Environment.Exit(0);
                    }
                    else {
                        txtTags.Text = getTags(Environment.GetCommandLineArgs());
                    }
                }
                else if (arg.StartsWith("images:") && apiTools.isValidImageLink(arg.Replace("images:", ""))) {
                    if (Images.Default.useForm) {
                        frmImageDownloader imageDL = new frmImageDownloader();
                        imageDL.url = arg.Replace("images:", "");
                        imageDL.header = Program.UserAgent;
                        imageDL.saveTo = Settings.Default.saveLocation;
                        imageDL.graylist = Settings.Default.blacklist;
                        imageDL.blacklist = Settings.Default.zeroToleranceBlacklist;
                        imageDL.separateRatings = Images.Default.separateRatings;
                        imageDL.separateBlacklisted = Images.Default.separateBlacklisted;
                        imageDL.saveInfo = Settings.Default.saveInfo;
                        imageDL.ignoreFinish = Settings.Default.ignoreFinish;
                        imageDL.fileNameCode = Images.Default.fileNameCode;
                        imageDL.ShowDialog();
                        Environment.Exit(0);
                    }
                    else {
                        ImageDownloader imageDL = new ImageDownloader();
                        imageDL.url = arg.Replace("images:", "");
                        imageDL.header = Program.UserAgent;
                        imageDL.saveTo = Settings.Default.saveLocation;
                        imageDL.graylist = Settings.Default.blacklist;
                        imageDL.blacklist = Settings.Default.zeroToleranceBlacklist;
                        imageDL.separateRatings = Images.Default.separateRatings;
                        imageDL.separateBlacklisted = Images.Default.separateBlacklisted;
                        imageDL.saveInfo = Settings.Default.saveInfo;
                        imageDL.ignoreFinish = Settings.Default.ignoreFinish;
                        imageDL.fileNameCode = Images.Default.fileNameCode;
                        if (imageDL.downloadImage()) {
                            if (!Settings.Default.ignoreFinish)
                                MessageBox.Show("Image " + imageDL.postID + " has finished downloading.");
                        }
                        else {
                            MessageBox.Show("An error occured when trying to download the image.");
                        }
                        Environment.Exit(0);
                    }
                }
                else {
                    txtTags.Text += arg.Replace("%20", " ") + " ";
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
            chkSeparateRatings.Checked = Tags.Default.separateRatings;
            chkMinimumScore.Checked = Tags.Default.enableScoreMin;
            chkScoreAsTag.Checked = Tags.Default.scoreAsTag;
            numScore.Value = Convert.ToDecimal(Tags.Default.scoreMin);
            numLimit.Value = Convert.ToDecimal(Tags.Default.imageLimit);
            chkPageLimit.Checked = Tags.Default.usePageLimit;
            numPageLimit.Value = Convert.ToDecimal(Tags.Default.pageLimit);

            chkOpen.Checked = Pools.Default.openAfter;
            chkMerge.Checked = Pools.Default.mergeBlacklisted;

            RegistryKey keyPools = Registry.ClassesRoot.OpenSubKey("pools\\shell\\open\\command", false);
            RegistryKey keyPoolWl = Registry.ClassesRoot.OpenSubKey("poolwl\\shell\\open\\command", false);
            RegistryKey keyTags = Registry.ClassesRoot.OpenSubKey("tags\\shell\\open\\command", false);
            if (keyPools == null || keyPoolWl == null || keyTags == null) {
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
            chkSeparateRatings.Checked = Tags.Default.separateRatings;
            chkMinimumScore.Checked = Tags.Default.enableScoreMin;
            chkScoreAsTag.Checked = Tags.Default.scoreAsTag;
            numScore.Value = Convert.ToDecimal(Tags.Default.scoreMin);
            numLimit.Value = Convert.ToDecimal(Tags.Default.imageLimit);
            chkPageLimit.Checked = Tags.Default.usePageLimit;
            numPageLimit.Value = Convert.ToDecimal(Tags.Default.pageLimit);

            chkOpen.Checked = Pools.Default.openAfter;
            chkMerge.Checked = Pools.Default.mergeBlacklisted;
        }
        private void mBlacklist_Click(object sender, EventArgs e) {
            frmBlacklist blackList = new frmBlacklist();
            blackList.ShowDialog();
        }
        private void mReverseSearch_Click(object sender, EventArgs e) {
            Process.Start("https://iqdb.harry.lu/");
        }
        private void mWishlist_Click(object sender, EventArgs e) {
            frmPoolWishlist wl = new frmPoolWishlist();
            wl.ShowDialog();
            wl.Dispose();
        }
        private void mRedownloader_Click(object sender, EventArgs e) {
            frmRedownloader rd = new frmRedownloader();
            rd.Show();
        }
        private void mIndexer_Click(object sender, EventArgs e) {
            frmIndexer idx = new frmIndexer();
            idx.Show();
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
    #endregion

    #region Tags
        public static string getTags(string[] args) {
            string tags = null;
            for (int i =0; i < args.Length; i++) {
                if (args[i].StartsWith("tags:") || args[i].StartsWith("tag:")) {
                    tags += args[i].Replace("tags:", "").Replace("tag:", "") + " ";
                }
            }
            tags = tags.TrimEnd(' ');
            return tags;
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
                if (MessageBox.Show("Downloading won't be limited. This may take a long while or even blacklist you. Continue anyway?", "aphrodite", MessageBoxButtons.YesNo) == DialogResult.No) {
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

            frmTagDownloader tagDL = new frmTagDownloader();
        // Global settings first
            tagDL.webHeader = Program.UserAgent;
            tagDL.graylist = Settings.Default.blacklist;
            tagDL.blacklist = Settings.Default.zeroToleranceBlacklist;
            tagDL.saveTo = Settings.Default.saveLocation;
            tagDL.saveInfo = Settings.Default.saveInfo;
            tagDL.openAfter = false;
            tagDL.saveBlacklistedFiles = Settings.Default.saveBlacklisted;
            tagDL.ignoreFinish = Settings.Default.ignoreFinish;

        // Form settings next
            tagDL.tags = txtTags.Text;
            tagDL.useMinimumScore = chkMinimumScore.Checked;
            if (tagDL.useMinimumScore) {
                tagDL.scoreAsTag = chkScoreAsTag.Checked;
                tagDL.minimumScore = Convert.ToInt32(numScore.Value);
            }
            if (numLimit.Value > 0) {
                tagDL.imageLimit = Convert.ToInt32(numLimit.Value);
            }
            tagDL.usePageLimit = chkPageLimit.Checked;
            if (tagDL.usePageLimit)
                tagDL.pageLimit = Convert.ToInt32(numPageLimit.Value);
            tagDL.separateRatings = chkSeparateRatings.Checked;
            if (tagDL.separateRatings) {
                string rates = string.Empty;
                if (chkExplicit.Checked)
                    rates += "e ";
                if (chkQuestionable.Checked)
                    rates += "q ";
                if (chkSafe.Checked)
                    rates += "s ";
                rates = rates.TrimEnd(' ');
                tagDL.ratings = rates.Split(' ');
            }
            tagDL.Show();
            txtTags.Clear();
        }

        private void chkMinimumScore_CheckedChanged(object sender, EventArgs e) {
            numScore.Enabled = chkMinimumScore.Checked;
        }
        private void chkPageLimit_CheckedChanged(object sender, EventArgs e) {
            numPageLimit.Enabled = chkPageLimit.Checked;
        }
    #endregion

    #region Pools
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
                MessageBox.Show("Please specify pool ID to download.");
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
            poolDL.poolID = txtID.Text;

            poolDL.header = Program.UserAgent;
            poolDL.saveTo = Settings.Default.saveLocation;
            poolDL.graylist = Settings.Default.blacklist;
            poolDL.blacklist = Settings.Default.zeroToleranceBlacklist;

            poolDL.saveInfo = Settings.Default.saveInfo;
            poolDL.ignoreFinish = Settings.Default.ignoreFinish;
            poolDL.saveBlacklisted = Settings.Default.saveBlacklisted;

            poolDL.usePoolName = Pools.Default.usePoolName;
            poolDL.mergeBlacklisted = Pools.Default.mergeBlacklisted;
            poolDL.openAfter = Pools.Default.openAfter;

            poolDL.ShowDialog();

            txtID.Clear();
        }
    #endregion
    }
}