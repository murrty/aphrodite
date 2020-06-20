﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Shell;

namespace aphrodite {
    public partial class frmMain : Form {

    #region Variables
        bool useIni = false;            // Determinse if the ini file will be used, enabling portable mode.
        IniFile ini = new IniFile();    // The ini file variable, doesn't create anything, or enable portable mode, on it's own.

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

        public void checkArgs(string[] args) {
            string testString = string.Join(" ", args.Skip(1));
            MessageBox.Show(testString);
            MessageBox.Show(testString.IndexOf("pools").ToString());
        }
    #endregion

    #region Form
        public frmMain() {
            InitializeComponent();
            this.Icon = Properties.Resources.Brad;

            if (Program.IsDebug) {
                this.Text += " (debug " + Properties.Settings.Default.debugDate + ")";
            }

            if (File.Exists(Environment.CurrentDirectory + "\\aphrodite.ini"))
                if (ini.KeyExists("useIni"))
                    if (ini.ReadBool("useIni"))
                        useIni = true;

            if (useIni) {
                if (ini.ReadInt("iniVersion") < Settings.Default.iniVersion) {
                    MessageBox.Show("A new setting has been added to the program and your ini file is out of date.\nOpen the Settings form and save it to set it as the default, or change it if the new settings is not what you want to have enabled.");
                }
            }

            SetTextBoxHint(txtTags.Handle, "Tags to download...");
            SetTextBoxHint(txtID.Handle, "Pool ID...");
        }
        private void frmMain_Load(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(Settings.Default.saveLocation) || string.IsNullOrEmpty(Settings.Default.saveLocation)) {
                Settings.Default.saveLocation = Environment.CurrentDirectory;
            }

            for (int i = 1; i < Environment.GetCommandLineArgs().Length; i++) { // count each runtime argument, int 0 = boot directory.
                string arg = Environment.GetCommandLineArgs()[i]; // buffer for the arg
                if (arg.StartsWith("installProtocol")) { // if the argument is installProtocol
                    frmSettings settings = new frmSettings(); // start the protocol section of the settings.
                    settings.isAdmin = Program.IsAdmin;
                    settings.protocol = true;
                    settings.useIni = useIni;
                    settings.ShowDialog();
                }
                if (arg.StartsWith("-settings") || arg.StartsWith("tags:configuresettings") || arg.StartsWith("pools:configuresettings") || arg.StartsWith("images:configuresettings") || arg.StartsWith("-protocol") || arg.StartsWith("-portable") || arg.StartsWith("-schema") || arg.StartsWith("configuresettings")) { // check for configure settings argument
                    frmSettings settings = new frmSettings(); // configure settings argument was passed
                    settings.pluginChange = true; // boolean to switch to the tab on load
                    settings.isAdmin = Program.IsAdmin; // sets isAdmin for the protocol.

                    switch (arg.ToLower()) {
                        case "-settings":
                            settings.plugin = 0;
                            break;
                        case "-protocol":
                            settings.plugin = 4;
                            break;
                        case "-portable":
                            settings.plugin = 5;
                            break;
                        case "-schema":
                            settings.plugin = 6;
                            break;
                    }

                    if (arg.StartsWith("tags:")) // if it's changing tag settings
                        settings.plugin = 1;
                    else if (arg.StartsWith("pools:")) // if it's changing pool settings
                        settings.plugin = 2;
                    else if (arg.StartsWith("images:")) // if it's changing image settings
                        settings.plugin = 3;
                    settings.ShowDialog();
                    Environment.Exit(0);
                }
                if (arg.StartsWith("poolwl:")) {
                    if (useIni) // Ini files don't use wishlists, not a limitation, I just think it'd make the ini file ugly. I can add it, though, if enough want it.
                        Environment.Exit(0);

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
                if (arg.StartsWith("pools:") && apiTools.IsValidPoolLink(arg.Replace("pools:", ""))) {
                    string poolID = arg.Replace("pools:", "").Replace("http://","https://").Replace("www.","");
                    if (!poolID.StartsWith("https://"))
                        poolID = "https://" + poolID;
                    if (poolID.Contains("?"))
                        poolID = poolID.Split('?')[0];
                    poolID = poolID.Split('/')[5];

                    downloadPool(poolID);
                    Environment.Exit(0);
                }
                else if (arg.StartsWith("tags:")) {
                    if (apiTools.IsValidPageLink(arg.Replace("tags:", ""))) {
                        downloadPageOfTags(arg.Replace("tags:", ""));
                        Environment.Exit(0);
                    }
                    else {
                        txtTags.Text = getTags(Environment.GetCommandLineArgs()).Replace("%20", " ");
                    }
                }
                else if (arg.StartsWith("images:") && apiTools.IsValidImageLink(arg.Replace("images:", ""))) {
                    downloadImage(arg.Replace("images:", ""));
                    Environment.Exit(0);
                }
                else if (arg.StartsWith("jRedownloader")) {
                    frmRedownloader rDownloader = new frmRedownloader();
                    rDownloader.ShowDialog();
                    Environment.Exit(0);
                }
                else if (arg.StartsWith("jWishlist")) {
                    frmPoolWishlist pWishlist = new frmPoolWishlist();
                    pWishlist.ShowDialog();
                    Environment.Exit(0);
                }
                else if (arg.StartsWith("jBlacklist")) {
                    frmBlacklist bList = new frmBlacklist();
                    bList.ShowDialog();
                    Environment.Exit(0);
                }
                else if (arg.StartsWith("-ib") || arg.StartsWith("-inkbunny")) {
                    // skip this arg. i could do this nicer, but eh.
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

            if (useIni) {
                mProtocol.Visible = false;
                mProtocol.Enabled = false;
                mWishlist.Enabled = false;
                lbIni.Visible = true;

                chkExplicit.Checked = ini.ReadBool("Explicit", "Tags");
                chkQuestionable.Checked = ini.ReadBool("Questionable", "Tags");
                chkSafe.Checked = ini.ReadBool("Safe", "Tags");
                chkSeparateRatings.Checked = ini.ReadBool("separateRatings", "Tags");
                chkMinimumScore.Checked = ini.ReadBool("useMinimumScore", "Tags");
                chkScoreAsTag.Checked = ini.ReadBool("scoreAsTag", "Tags");
                numScore.Value = Convert.ToDecimal(ini.ReadInt("scoreMin", "Tags"));
                numLimit.Value = Convert.ToDecimal(ini.ReadInt("imageLimit", "Tags"));
                numPageLimit.Value = Convert.ToDecimal(ini.ReadInt("pageLimit", "Tags"));

                chkMerge.Checked = ini.ReadBool("mergeBlacklisted", "Pools");
                chkOpen.Checked = ini.ReadBool("openAfter", "Pools");

                chkImageSeparateRatings.Checked = ini.ReadBool("separateRatings", "Images");
                chkImageSeparateBlacklisted.Checked = ini.ReadBool("separateBlacklisted", "Images");
                chkImageUseForm.Checked = ini.ReadBool("useForm", "Images");

            }
            else {
                chkExplicit.Checked = Tags.Default.Explicit;
                chkQuestionable.Checked = Tags.Default.Questionable;
                chkSafe.Checked = Tags.Default.Safe;
                chkSeparateRatings.Checked = Tags.Default.separateRatings;
                chkMinimumScore.Checked = Tags.Default.enableScoreMin;
                chkScoreAsTag.Checked = Tags.Default.scoreAsTag;
                numScore.Value = Convert.ToDecimal(Tags.Default.scoreMin);
                numLimit.Value = Convert.ToDecimal(Tags.Default.imageLimit);
                numPageLimit.Value = Convert.ToDecimal(Tags.Default.pageLimit);

                chkMerge.Checked = Pools.Default.mergeBlacklisted;
                chkOpen.Checked = Pools.Default.openAfter;

                chkImageSeparateRatings.Checked = Images.Default.separateRatings;
                chkImageSeparateBlacklisted.Checked = Images.Default.separateBlacklisted;
                chkImageUseForm.Checked = Images.Default.useForm;

                tbMain.TabPages.Remove(tbIni);

                RegistryKey keyPools = Registry.ClassesRoot.OpenSubKey("pools\\shell\\open\\command", false);
                RegistryKey keyPoolWl = Registry.ClassesRoot.OpenSubKey("poolwl\\shell\\open\\command", false);
                RegistryKey keyTags = Registry.ClassesRoot.OpenSubKey("tags\\shell\\open\\command", false);
                if (keyPools == null || keyPoolWl == null || keyTags == null) {
                    mProtocol.Visible = true;
                    mProtocol.Enabled = true;
                }
            }

            if (FormSettings.Default.frmMainX != -999999 && FormSettings.Default.frmMainY != -999999) {
                if (useIni) {
                    this.Location = new Point(ini.ReadInt("frmMainX", "Forms"), ini.ReadInt("frmMainY", "Forms"));
                }
                else {
                    this.Location = new Point(FormSettings.Default.frmMainX, FormSettings.Default.frmMainY);
                }
            }
        }
        private void frmMain_Shown(object sender, EventArgs e) {
            txtTags.Focus();
        }
        private void tbMain_SelectedIndexChanged(object sender, EventArgs e) {
            if (tbMain.SelectedTab == tbTags) {
                txtTags.Focus();
                this.AcceptButton = btnDownloadTags;
            }
            else if (tbMain.SelectedTab == tbPools) {
                txtID.Focus();
                this.AcceptButton = btnDownloadPool;
            }
            else if (tbMain.SelectedTab == tbImages) {
                txtImageUrl.Focus();
                this.AcceptButton = btnDownloadImage;
            }
        }

        private void mSettings_Click(object sender, EventArgs e) {
            frmSettings settings = new frmSettings();
            settings.isAdmin = Program.IsAdmin;
            settings.useIni = useIni;
            settings.ShowDialog();

            if (useIni) {
                chkExplicit.Checked = ini.ReadBool("Explicit", "Tags");
                chkQuestionable.Checked = ini.ReadBool("Questionable", "Tags");
                chkSafe.Checked = ini.ReadBool("Safe", "Tags");
                chkSeparateRatings.Checked = ini.ReadBool("separateRatings", "Tags");
                chkMinimumScore.Checked = ini.ReadBool("useMinimumScore", "Tags");
                chkScoreAsTag.Checked = ini.ReadBool("scoreAsTag", "Tags");
                numScore.Value = Convert.ToDecimal(ini.ReadInt("scoreMin", "Tags"));
                numLimit.Value = Convert.ToDecimal(ini.ReadInt("imageLimit", "Tags"));
                numPageLimit.Value = Convert.ToDecimal(ini.ReadInt("pageLimit", "Tags"));

                chkOpen.Checked = ini.ReadBool("openAfter", "Pools");
                chkMerge.Checked = ini.ReadBool("mergeBlacklisted", "Pools");
            }
            else {
                chkExplicit.Checked = Tags.Default.Explicit;
                chkQuestionable.Checked = Tags.Default.Questionable;
                chkSafe.Checked = Tags.Default.Safe;
                chkSeparateRatings.Checked = Tags.Default.separateRatings;
                chkMinimumScore.Checked = Tags.Default.enableScoreMin;
                chkScoreAsTag.Checked = Tags.Default.scoreAsTag;
                numScore.Value = Convert.ToDecimal(Tags.Default.scoreMin);
                numLimit.Value = Convert.ToDecimal(Tags.Default.imageLimit);
                numPageLimit.Value = Convert.ToDecimal(Tags.Default.pageLimit);

                chkOpen.Checked = Pools.Default.openAfter;
                chkMerge.Checked = Pools.Default.mergeBlacklisted;
            }

        }
        private void mBlacklist_Click(object sender, EventArgs e) {
            frmBlacklist blackList = new frmBlacklist();
            blackList.useIni = useIni;
            blackList.ShowDialog();
        }
        private void mReverseSearch_Click(object sender, EventArgs e) {
            Process.Start("https://saucenao.com/index.php");
        }
        private void mWishlist_Click(object sender, EventArgs e) {
            if (useIni)
                return;

            Pools.Default.Reload();
            frmPoolWishlist wl = new frmPoolWishlist();
            wl.ShowDialog();
            wl.Dispose();
        }
        private void mRedownloader_Click(object sender, EventArgs e) {
            frmRedownloader rd = new frmRedownloader();
            rd.useIni = useIni;
            rd.Show();
        }
        private void mParser_Click(object sender, EventArgs e) {
            frmParser psr = new frmParser();
            psr.ShowDialog();
        }

        private void mAbout_Click(object sender, EventArgs e) {
            frmAbout frAbout = new frmAbout();
            frAbout.Show();
        }
        private void mProtocol_Click(object sender, EventArgs e) {
            frmSettings settings = new frmSettings();
            settings.isAdmin = Program.IsAdmin;
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

            string rates = string.Empty;
            if (chkSeparateRatings.Checked) {
                if (chkExplicit.Checked) {
                    rates += "e ";
                }
                if (chkQuestionable.Checked) {
                    rates += "q ";
                }
                if (chkSafe.Checked) {
                    rates += "s ";
                }
                rates = rates.TrimEnd(' ');
            }

            Downloader.MainForm.downloadTags(txtTags.Text, (int)numPageLimit.Value, chkMinimumScore.Checked, chkScoreAsTag.Checked, (int)numScore.Value, (int)numLimit.Value, rates.Split(' '), chkSeparateRatings.Checked, useIni);
        }

        private void chkMinimumScore_CheckedChanged(object sender, EventArgs e) {
            numScore.Enabled = chkMinimumScore.Checked;
        }

        private void downloadPageOfTags(string tags) {
            // USED FOR THE ARGUMENT DOWNLOADS
            frmTagDownloader tagDL = new frmTagDownloader();
            tagDL.webHeader = Program.UserAgent;
            tagDL.fromURL = true;
            tagDL.downloadUrl = tags;
            string ratings = string.Empty;

            if (useIni) {
                tagDL.saveTo = Environment.CurrentDirectory;
                if (File.Exists(File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg")))
                    tagDL.graylist = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg");
                else
                    tagDL.graylist = string.Empty;

                if (File.Exists(Environment.CurrentDirectory + "\\blacklist.cfg"))
                    tagDL.blacklist = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg");
                else
                    tagDL.blacklist = string.Empty;



                if (ini.KeyExists("saveInfo", "Global"))
                    tagDL.saveInfo = ini.ReadBool("saveInfo", "Global");
                else
                    tagDL.saveInfo = true;

                if (ini.KeyExists("openAfter", "Global"))
                    tagDL.openAfter = false; //ini.ReadBool("openAfter", "Global");
                else
                    tagDL.openAfter = false;

                if (ini.KeyExists("saveBlacklisted", "Global"))
                    tagDL.saveBlacklistedFiles = ini.ReadBool("saveBlacklisted", "Global");
                else
                    tagDL.saveBlacklistedFiles = true;

                if (ini.KeyExists("ignoreFinish", "Global"))
                    tagDL.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");
                else
                    tagDL.ignoreFinish = false;



                if (ini.KeyExists("useMinimumScore", "Tags"))
                    tagDL.useMinimumScore = ini.ReadBool("useMinimumScore", "Tags");
                else
                    tagDL.useMinimumScore = false;

                if (tagDL.useMinimumScore) {
                    tagDL.scoreAsTag = ini.ReadBool("scoreAsTag", "Tags");
                    tagDL.minimumScore = ini.ReadInt("scoreMin", "Tags");
                }

                if (ini.KeyExists("imageLimit", "Tags"))
                    if (ini.ReadInt("imageLimit", "Tags") > 0)
                        tagDL.imageLimit = ini.ReadInt("imageLimit", "Tags");
                else
                    tagDL.imageLimit = 0;

                if (ini.KeyExists("pageLimit", "Tags"))
                    if (ini.ReadInt("pageLimit", "Tags") > 0)
                        tagDL.pageLimit = ini.ReadInt("pageLimit", "Tags");

                if (ini.KeyExists("separateRatings", "Tags"))
                    tagDL.separateRatings = ini.ReadBool("separateRatings", "Tags");
                else
                    tagDL.separateRatings = true;

                if (ini.KeyExists("Explicit", "Tags"))
                    if (ini.ReadBool("Explicit", "Tags"))
                        ratings += "e ";
                if (ini.KeyExists("Questionable", "Tags"))
                    if (ini.ReadBool("Questionable", "Tags"))
                        ratings += "q ";
                if (ini.KeyExists("Safe", "Tags"))
                    if (ini.ReadBool("Safe", "Tags"))
                        ratings += "s";
                ratings = ratings.TrimEnd(' ');

                if (tagDL.separateRatings)
                    tagDL.ratings = ratings.Split(' ');

                if (ini.KeyExists("fileNameSchema", "Tags"))
                    tagDL.fileNameSchema = ini.ReadString("fileNameSchema", "Tags").ToLower();
                else
                    tagDL.fileNameSchema = "%md5%";
            }
            else {
                Settings.Default.Reload();
                Tags.Default.Reload();
                tagDL.graylist = Settings.Default.blacklist;
                tagDL.blacklist = Settings.Default.zeroToleranceBlacklist;
                tagDL.saveTo = Settings.Default.saveLocation;
                tagDL.saveInfo = Settings.Default.saveInfo;
                tagDL.openAfter = false;
                tagDL.saveBlacklistedFiles = Settings.Default.saveBlacklisted;
                tagDL.ignoreFinish = Settings.Default.ignoreFinish;
                tagDL.useMinimumScore = Tags.Default.enableScoreMin;
                if (tagDL.useMinimumScore) {
                    tagDL.scoreAsTag = Tags.Default.scoreAsTag;
                    tagDL.minimumScore = Tags.Default.scoreMin;
                }
                if (Tags.Default.imageLimit > 0)
                    tagDL.imageLimit = Tags.Default.imageLimit;
                if (Tags.Default.pageLimit > 0)
                    tagDL.pageLimit = Tags.Default.pageLimit;
                tagDL.separateRatings = Tags.Default.separateRatings;
                if (tagDL.separateRatings) {
                    if (Tags.Default.Explicit)
                        ratings += "e ";
                    if (Tags.Default.Questionable)
                        ratings += "q ";
                    if (Tags.Default.Safe)
                        ratings += "s ";
                    ratings = ratings.TrimEnd(' ');
                    tagDL.ratings = ratings.Split(' ');
                }
                tagDL.fileNameSchema = Tags.Default.fileNameSchema.ToLower();
            }

            tagDL.ShowDialog();
        } // ARGUMENT DOWNLOADS ONLY // Downloads tags from one page.
    #endregion

    #region Pools
        private void btnDownloadPool_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtID.Text)) {
                MessageBox.Show("Please specify pool ID to download.");
                return;
            }

            Downloader.MainForm.downloadPool(txtID.Text, chkOpen.Checked, chkMerge.Checked, useIni);
        }

        private void downloadPool(string poolID) {
            Downloader.Arguments.downloadPool(poolID);
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
    #endregion

    #region Images
        private void btnDownloadImage_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(txtImageUrl.Text))
                return;

            Downloader.MainForm.downloadImage(txtImageUrl.Text, chkImageSeparateRatings.Checked, chkImageSeparateBlacklisted.Checked, chkImageSeparateArtists.Checked, chkImageUseForm.Checked);
        }
        private void downloadImage(string url) {
            Downloader.Arguments.downloadImage(url);
        }
    #endregion
    }
}