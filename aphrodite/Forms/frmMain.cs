using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmMain : Form {

        #region Variables
        frmAbout About;
        frmPoolWishlist PoolWishlist;
        frmRedownloader Redownloader;

        Thread UpdateCheckThread;
        // Valid protocols:
        //                  'pools:'
        //                  'tags:'
        //                  'images:'
        #endregion

        #region Form
        public frmMain(string Argument = null, DownloadType Type = DownloadType.None) {
            InitializeComponent();

            UpdateCheckThread = new Thread(() => {
                try {
                    Updater.CheckForUpdate(false);
                } catch { } // if something happens, who cares.
            });
            UpdateCheckThread.Name = "Update checker thread";

            switch (Type) {
                case DownloadType.Tags:
                    if (Argument != null) {
                        txtTags.Text = Argument;
                    }
                    break;
                case DownloadType.Pools:
                    if (Argument != null) {
                        txtPoolId.Text = Argument;
                    }
                    tabMain.SelectedTab = tabPools;
                    break;
                case DownloadType.Images:
                    if (Argument != null) {
                        txtImageUrl.Text = Argument;
                    }
                    tabMain.SelectedTab = tabImages;
                    break;
            }

            if (Program.UseIni) {
                this.Text += " (ini)";
            }

            txtTags.Refresh();
            txtPoolId.Refresh();
            txtImageUrl.Refresh();
        }
        private void frmMain_Load(object sender, EventArgs e) {
            if (Config.Settings.General.CheckForUpdates) {
                UpdateCheckThread.Start();
            }

            if (string.IsNullOrWhiteSpace(Config.Settings.General.saveLocation)) {
                Config.Settings.General.saveLocation = Environment.CurrentDirectory;
            }

            chkTagsDownloadExplicit.Checked = Config.Settings.Tags.Explicit;
            chkTagsDownloadQuestionable.Checked = Config.Settings.Tags.Questionable;
            chkTagsDownloadSafe.Checked = Config.Settings.Tags.Safe;
            chkTagsSeparateRatings.Checked = Config.Settings.Tags.separateRatings;
            chkTagsUseMinimumScore.Checked = Config.Settings.Tags.enableScoreMin;
            chkTagsMinimumScoreAsTag.Checked = Config.Settings.Tags.scoreAsTag;
            numTagsMinimumScore.Value = Convert.ToDecimal(Config.Settings.Tags.scoreMin);
            numTagsImageLimit.Value = Convert.ToDecimal(Config.Settings.Tags.imageLimit);
            numTagsPageLimit.Value = Convert.ToDecimal(Config.Settings.Tags.pageLimit);

            chkPoolMergeGraylisted.Checked = Config.Settings.Pools.mergeGraylisted;

            chkImageSeparateRatings.Checked = Config.Settings.Images.separateRatings;
            chkImageSeparateGraylisted.Checked = Config.Settings.Images.separateGraylisted;
            chkImageUseForm.Checked = Config.Settings.Images.useForm;

            if (Config.Settings.FormSettings.frmMain_Location.X != -32000 && Config.Settings.FormSettings.frmMain_Location.Y != -32000) {
                this.Location = Config.Settings.FormSettings.frmMain_Location;
            }
        }
        private void frmMain_Shown(object sender, EventArgs e) {
            txtTags.Focus();
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
            if (UpdateCheckThread != null && UpdateCheckThread.IsAlive) {
                UpdateCheckThread.Abort();
            }
            
            this.Opacity = 0;

            if (this.WindowState != FormWindowState.Normal) {
                this.WindowState = FormWindowState.Normal;
            }

            Config.Settings.FormSettings.frmMain_Location = this.Location;
        }
        private void tbMain_SelectedIndexChanged(object sender, EventArgs e) {
            if (tabMain.SelectedTab == tabTags) {
                txtTags.Focus();
                this.AcceptButton = btnDownloadTags;
            }
            else if (tabMain.SelectedTab == tabPools) {
                txtPoolId.Focus();
                this.AcceptButton = btnDownloadPool;
            }
            else if (tabMain.SelectedTab == tabImages) {
                txtImageUrl.Focus();
                this.AcceptButton = btnDownloadImage;
            }
        }

        private void mSettings_Click(object sender, EventArgs e) {
            using (frmSettings SettingsForm = new frmSettings()) {
                switch (SettingsForm.ShowDialog()) {
                    case System.Windows.Forms.DialogResult.OK:
                        if (Program.UseIni) {
                            if (!this.Text.EndsWith(" (ini)")) {
                                this.Text += " (ini)";
                            }
                        }
                        else {
                            this.Text = this.Text.Substring(0, this.Text.Length - 6);
                        }
                        break;
                }
            }
        }
        private void mBlacklist_Click(object sender, EventArgs e) {
            using (frmBlacklist Blacklist = new frmBlacklist()) {
                Blacklist.ShowDialog();
            }
        }
        private void mReverseSearch_Click(object sender, EventArgs e) {
            //Process.Start("https://saucenao.com/index.php");
            Process.Start("https://e621.net/iqdb_queries");
        }
        private void mWishlist_Click(object sender, EventArgs e) {
            if (PoolWishlist == null || PoolWishlist.IsDisposed) {
                PoolWishlist = new frmPoolWishlist();
                PoolWishlist.Show();
            }
            else {
                if (PoolWishlist.WindowState == FormWindowState.Minimized) {
                    PoolWishlist.WindowState = FormWindowState.Normal;
                }
                PoolWishlist.Activate();
            }
        }
        private void mRedownloader_Click(object sender, EventArgs e) {
            if (Redownloader == null || Redownloader.IsDisposed) {
                Redownloader = new frmRedownloader();
                Redownloader.Show();
            }
            else {
                if (Redownloader.WindowState == FormWindowState.Minimized) {
                    Redownloader.WindowState = FormWindowState.Normal;
                }
                Redownloader.Activate();
            }
        }
        private void mLog_Click(object sender, EventArgs e) {
            Program.Log(LogAction.ShowLog);
        }

        private void mAbout_Click(object sender, EventArgs e) {
            if (About == null || About.IsDisposed) {
                About = new frmAbout();
                About.Show();
            }
            else {
                if (About.WindowState == FormWindowState.Minimized) {
                    About.WindowState = FormWindowState.Normal;
                }
                About.Activate();
            }
        }
        #endregion

        #region Tags
        private void txtTags_KeyPress(object sender, KeyPressEventArgs e) {
            // Enforce 6 tag limit
            switch (txtTags.Text.Count(x => x == ' ') >= 5 && e.KeyChar == (char)Keys.Space) {
                case true:
                    e.Handled = true;
                    System.Media.SystemSounds.Beep.Play();
                    break;
            }
        }
        private void txtTags_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode == Keys.Enter) {
                case true:
                    btnDownloadTags_Click(this, new EventArgs());
                    e.Handled = true;
                    break;

                case false:
                    switch (e.KeyCode == Keys.V && e.Modifiers == Keys.Control) {
                        case true:
                            switch (Clipboard.ContainsText()) {
                                case true:
                                    string[] input = Clipboard.GetText().Split(' ');
                                    switch (input.Length > 6) {
                                        case true:
                                            txtTags.Text = string.Format("{0} {1} {2} {3} {4} {5}",
                                                input[0],
                                                input[1],
                                                input[2],
                                                input[3],
                                                input[4],
                                                input[5]
                                            );
                                            System.Media.SystemSounds.Beep.Play();
                                            break;

                                        case false:
                                            txtTags.Text = Clipboard.GetText();
                                            break;
                                    }
                                    txtTags.SelectionLength = 0;
                                    txtTags.SelectionStart = txtTags.Text.Length;
                                    break;

                                case false:
                                    System.Media.SystemSounds.Beep.Play();
                                    break;
                            }
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                            break;
                    }
                    break;
            }

        }
        private void txtTags_ButtonClick(object sender, EventArgs e) {
            txtTags.Clear();
        }
        private void rbTagsMinimumScore_CheckedChanged(object sender, System.EventArgs e) {
            if (rbTagsMinimumScore.Checked) {
                panelTagsMinimumScore.Visible = true;
                panelTagsMinimumFavorites.Visible = false;
                panelTagsLimits.Visible = false;
                panelTagsRatings.Visible = false;
                panelTagsOtherSettings.Visible = false;
                lbTagsLimitsHint.Visible = false;
            }
        }
        private void rbTagsMinimumFavorites_CheckedChanged(object sender, System.EventArgs e) {
            if (rbTagsMinimumFavorites.Checked) {
                panelTagsMinimumScore.Visible = false;
                panelTagsMinimumFavorites.Visible = true;
                panelTagsLimits.Visible = false;
                panelTagsRatings.Visible = false;
                panelTagsOtherSettings.Visible = false;
                lbTagsLimitsHint.Visible = true;
                lbTagsLimitsHint.Location = new Point(119, 67);
            }
        }
        private void rbTagsLimits_CheckedChanged(object sender, System.EventArgs e) {
            if (rbTagsLimits.Checked) {
                panelTagsMinimumScore.Visible = false;
                panelTagsMinimumFavorites.Visible = false;
                panelTagsLimits.Visible = true;
                panelTagsRatings.Visible = false;
                panelTagsOtherSettings.Visible = false;
                lbTagsLimitsHint.Visible = true;
                lbTagsLimitsHint.Location = new Point(119, 55);
            }
        }
        private void rbTagsRatings_CheckedChanged(object sender, System.EventArgs e) {
            if (rbTagsRatings.Checked) {
                panelTagsMinimumScore.Visible = false;
                panelTagsMinimumFavorites.Visible = false;
                panelTagsLimits.Visible = false;
                panelTagsRatings.Visible = true;
                panelTagsOtherSettings.Visible = false;
                lbTagsLimitsHint.Visible = false;
            }
        }
        private void rbTagsOtherSettings_CheckedChanged(object sender, System.EventArgs e) {
            if (rbTagsOtherSettings.Checked) {
                panelTagsMinimumScore.Visible = false;
                panelTagsMinimumFavorites.Visible = false;
                panelTagsLimits.Visible = false;
                panelTagsRatings.Visible = false;
                panelTagsOtherSettings.Visible = true;
                lbTagsLimitsHint.Visible = false;
            }
        }
        private void chkTagsUseMinimumScore_CheckedChanged(object sender, EventArgs e) {
            numTagsMinimumScore.Enabled = chkTagsUseMinimumScore.Checked;
            chkTagsMinimumScoreAsTag.Enabled = chkTagsUseMinimumScore.Checked;
        }

        private void btnDownloadTags_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtTags.Text)) {
                MessageBox.Show("Please specify tags to download.");
                return;
            }

            string[] length = txtTags.Text.Split(' ');
            if (length.Length > 6) {
                MessageBox.Show("6 tags is the maximum length you're allowed to download from e621. If your tag has a space between words, be sure to add an underscore. (_)");
                return;
            }
            string NewTags = txtTags.Text;
            if (NewTags.StartsWith(" ")) {
                NewTags = NewTags.TrimStart(' ');
            }
            if (NewTags.EndsWith(" ")) {
                NewTags = NewTags.TrimEnd(' ');
            }
            if (NewTags.Contains("/")) {
                NewTags = NewTags.Replace("/", "%25-2F");
            }

            TagDownloadInfo NewInfo = new TagDownloadInfo(NewTags);
            NewInfo.PageLimit = (int)numTagsPageLimit.Value;
            NewInfo.UseMinimumScore = chkTagsUseMinimumScore.Checked;
            NewInfo.MinimumScoreAsTag = chkTagsMinimumScoreAsTag.Checked;
            NewInfo.MinimumScore = (int)numTagsMinimumScore.Value;
            NewInfo.ImageLimit = (int)numTagsImageLimit.Value;
            NewInfo.SaveExplicit = chkTagsDownloadExplicit.Checked;
            NewInfo.SaveQuestionable = chkTagsDownloadQuestionable.Checked;
            NewInfo.SaveSafe = chkTagsDownloadSafe.Checked;
            NewInfo.SeparateRatings = chkTagsSeparateRatings.Checked;
            NewInfo.SeparateNonImages = chkTagSeparateNonImages.Checked;
            NewInfo.OpenAfter = chkTagsOpenAfterDownload.Checked;
            NewInfo.DownloadNewestToOldest = chkTagsDownloadInUploadOrder.Checked;
            NewInfo.FavoriteCount = (int)numTagsMinimumFavorites.Value;
            NewInfo.FavoriteCountAsTag = chkTagsMinimumFavoritesAsTag.Checked;
            frmTagDownloader Downloader = new frmTagDownloader();
            Downloader.DownloadInfo = NewInfo;
            Downloader.Show();
        }
        #endregion

        #region Pools
        private void txtPoolId_KeyPress(object sender, KeyPressEventArgs e) {
            switch (e.KeyChar) {
                case '0': case (char)Keys.Back:
                case '1': case '2': case '3':
                case '4': case '5': case '6':
                case '7': case '8': case '9':
                    e.Handled = false;
                    break;

                case 'v': case 'V': case (char)22:
                case 'a': case 'A': case (char)1:
                    e.Handled = true;
                    break;

                default:
                    e.Handled = true;
                    System.Media.SystemSounds.Beep.Play();
                    break;
            }
        }
        private void txtPoolId_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Enter:
                    btnDownloadPool_Click(this, new EventArgs());
                    e.Handled = true;
                    break;

                case Keys.A:
                    switch (e.Modifiers) {
                        case Keys.Control:
                            txtPoolId.SelectAll();
                            break;

                        default:
                            System.Media.SystemSounds.Beep.Play();
                            break;
                    }
                    e.Handled = true;
                    break;

                case Keys.V:
                    switch (e.Modifiers) {
                        case Keys.Control:
                            switch (Clipboard.ContainsText()) {
                                case true:
                                    string cb = Clipboard.GetText().Replace("e621.net", "").Split('?')[0];
                                    string output = string.Empty;
                                    for (int i = 0; i < cb.Length; i++) {
                                        switch (char.IsDigit(cb[i])) {
                                            case true:
                                                output += cb[i];
                                                break;
                                        }
                                    }

                                    switch (string.IsNullOrWhiteSpace(output)) {
                                        case false:
                                            txtPoolId.Text = output;
                                            break;

                                        case true:
                                            System.Media.SystemSounds.Beep.Play();
                                            break;
                                    }
                                    break;

                                case false:
                                    System.Media.SystemSounds.Beep.Play();
                                    break;
                            }
                            break;

                        default:
                            System.Media.SystemSounds.Beep.Play();
                            break;
                    }

                    e.Handled = true;
                    break;
            }
        }
        private void txtPoolId_ButtonClick(object sender, EventArgs e) {
            txtPoolId.Clear();
        }

        private void btnDownloadPool_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtPoolId.Text)) {
                MessageBox.Show("Please specify pool ID to download.");
                return;
            }
            string ID = txtPoolId.Text;
            if (apiTools.IsValidPoolLink(ID)) {
                ID = ID.Split('/')[4].Split('?')[0];
            }

            PoolDownloadInfo NewInfo = new PoolDownloadInfo(txtPoolId.Text);
            NewInfo.OpenAfter = chkPoolOpenAfter.Checked;
            NewInfo.MergeGraylistedPages = chkPoolMergeGraylisted.Checked;
            NewInfo.DownloadBlacklistedPages = chkPoolDownloadBlacklistedImages.Checked;
            NewInfo.MergeBlacklistedPages = chkPoolMergeBlacklisted.Checked;
            frmPoolDownloader Downloader = new frmPoolDownloader();
            Downloader.DownloadInfo = NewInfo;
            Downloader.Show();
        }
        #endregion

        #region Images
        private void txtImageUrl_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                btnDownloadImage_Click(this, new EventArgs());
            }
        }
        private void txtImageUrl_ButtonClick(object sender, EventArgs e) {
            txtImageUrl.Clear();
        }

        private void btnDownloadImage_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtImageUrl.Text)) {
                MessageBox.Show("Please enter a valid image url or id");
                return;
            }

            ImageDownloadInfo NewInfo = new ImageDownloadInfo(txtImageUrl.Text);
            NewInfo.SeparateRatings = chkImageSeparateRatings.Checked;
            NewInfo.SeparateGraylisted = chkImageSeparateGraylisted.Checked;
            NewInfo.SeparateBlacklisted = chkImageSeparateBlacklisted.Checked;
            NewInfo.SeparateNonImages = chkImageSeparateNonImages.Checked;
            NewInfo.SeparateArtists = chkImageSeparateArtists.Checked;
            NewInfo.UseForm = chkImageUseForm.Checked;
            NewInfo.OpenAfter = chkImageOpenAfter.Checked;

            if (chkImageUseForm.Checked) {
                frmImageDownloader Downloader = new frmImageDownloader();
                Downloader.DownloadInfo = NewInfo;
                Downloader.Show();
            }
            else {
                ImageDownloader Downloader = new ImageDownloader(NewInfo);
            }
        }
        #endregion

    }
}