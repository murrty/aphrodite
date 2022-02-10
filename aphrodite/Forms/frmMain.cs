using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace aphrodite {

    public partial class frmMain : Form {

        #region Variables
        private frmPoolWishlist PoolWishlist;
        private frmRedownloader Redownloader;

        private Thread UpdateCheckThread;
        #endregion

        #region Form
        public frmMain(DownloadType Type = DownloadType.None, string Argument = null) {
            InitializeComponent();

            if (Config.ValidPoint(Config.Settings.FormSettings.frmMain_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmMain_Location;
            }

            switch (Type) {
                case DownloadType.Tags: {
                    txtTags.Text = Argument ?? "";
                } break;
                case DownloadType.Pools: {
                    txtPoolId.Text = Argument ?? "";
                    tabMain.SelectedTab = tabPools;
                } break;
                case DownloadType.Images: {
                    txtImageUrl.Text = Argument ?? "";
                    tabMain.SelectedTab = tabImages;
                } break;
            }

            if (Config.Settings.UseIni) {
                this.Text += " (ini)";
            }

            txtTags.Refresh();
            txtPoolId.Refresh();
            txtImageUrl.Refresh();
        }
        private void frmMain_Load(object sender, EventArgs e) {
            if (Config.Settings.Initialization.CheckForUpdates && !Program.IsDebug) {
                UpdateCheckThread = new(() => {
                    try {
                        if (Updater.CheckForUpdate(false)) {
                            this.Invoke(delegate () {
                                mUpdateAvailable.Visible = mUpdateAvailable.Enabled = true;
                            });
                        }
                    }
                    catch { } // if something happens, who cares.
                });
                UpdateCheckThread.Name = "Update checker thread";
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
            using frmSettings SettingsForm = new();
            switch (SettingsForm.ShowDialog()) {
                case DialogResult.OK:
                    if (Config.Settings.UseIni) {
                        if (!this.Text.EndsWith(" (ini)")) {
                            this.Text += " (ini)";
                        }
                    }
                    else {
                        this.Text = this.Text[0..^6];
                    }
                    break;
            }
        }
        private void mBlacklist_Click(object sender, EventArgs e) {
            using frmBlacklist Blacklist = new();
            Blacklist.ShowDialog();
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
        private void mFurryBooru_Click(object sender, EventArgs e) {
            using frmFurryBooruMain FurryBooru = new();
            FurryBooru.ShowDialog();
        }

        private void mInkBunny_Click(Object sender, EventArgs e) {
            using frmInkBunnyMain InkBunny = new();
            InkBunny.ShowDialog();
        }

        private void mImgurAlbums_Click(Object sender, EventArgs e) {
            using frmImgurMain Imgur = new();
            Imgur.ShowDialog();
        }
        private void mLog_Click(object sender, EventArgs e) {
            Log.ShowLog();
        }
        private void mAbout_Click(object sender, EventArgs e) {
            frmAbout About = new();
            About.ShowDialog();
        }
        private void mUpdateAvailable_Click(Object sender, EventArgs e) {
            Updater.CheckForUpdate(false);
        }
        #endregion

        #region Tags
        private void txtTags_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode == Keys.Enter) {
                case true: {
                    btnDownloadTags_Click(txtTags, new());
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                } break;

                case false: {
                    if (e.Modifiers == Keys.Control) {
                        if (e.KeyCode == Keys.V && Clipboard.ContainsText()) {
                            string[] input = Clipboard.GetText().Split(' ');
                            if (input.Length > 6) {
                                txtTags.Text = $"{input[0]} {input[1]} {input[2]} {input[3]} {input[4]} {input[5]}";
                                System.Media.SystemSounds.Asterisk.Play();
                            }
                            else {
                                txtTags.Text = Clipboard.GetText();
                            }
                            txtTags.SelectionLength = 0;
                            txtTags.SelectionStart = txtTags.Text.Length;
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                        }
                        else {
                            //e.Handled = true;
                            //e.SuppressKeyPress = true;
                            //System.Media.SystemSounds.Beep.Play();
                        }
                    }
                    else {
                        if (txtTags.Text.Count(x => x == ' ') >= 5 && e.KeyCode == Keys.Space) {
                            e.Handled = true;
                            e.SuppressKeyPress = true;
                            System.Media.SystemSounds.Beep.Play();
                        }
                    }
                } break;
            }

        }
        private void txtTags_ButtonClick(object sender, EventArgs e) {
            txtTags.Clear();
            txtTags.Focus();
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
                txtTags.Focus();
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            string[] length = txtTags.Text.Split(' ');
            if (length.Length > 6) {
                Log.MessageBox("6 tags is the maximum length you're allowed to download from e621. If your tag has a space between words, be sure to add an underscore. (_)", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtTags.Focus();
                txtTags.SelectAll();
                return;
            }

            frmTagDownloader DownloadForm = new(
                new(txtTags.Text.Trim().Replace("/", "%25-2F"), DownloadSite.e621) {
                    PageLimit = (int)numTagsPageLimit.Value,
                    UseMinimumScore = chkTagsUseMinimumScore.Checked,
                    MinimumScoreAsTag = chkTagsMinimumScoreAsTag.Checked,
                    MinimumScore = (int)numTagsMinimumScore.Value,
                    ImageLimit = (int)numTagsImageLimit.Value,
                    SaveExplicit = chkTagsDownloadExplicit.Checked,
                    SaveQuestionable = chkTagsDownloadQuestionable.Checked,
                    SaveSafe = chkTagsDownloadSafe.Checked,
                    SeparateRatings = chkTagsSeparateRatings.Checked,
                    SeparateNonImages = chkTagSeparateNonImages.Checked,
                    OpenAfter = chkTagsOpenAfterDownload.Checked,
                    DownloadNewestToOldest = chkTagsDownloadInUploadOrder.Checked,
                    FavoriteCount = (int)numTagsMinimumFavorites.Value,
                    FavoriteCountAsTag = chkTagsMinimumFavoritesAsTag.Checked,
                }
            );
            DownloadForm.Show();
        }
        #endregion

        #region Pools
        private void txtPoolId_KeyPress(object sender, KeyPressEventArgs e) {
            switch (e.KeyChar) {
                case (char)Keys.Return: {
                    btnDownloadPool_Click(txtPoolId, new());
                } break;

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
                    System.Media.SystemSounds.Exclamation.Play();
                    break;
            }
        }
        private void txtPoolId_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Enter: {
                    //btnDownloadPool_Click(this, new EventArgs());
                    //e.Handled = true;
                } break;

                case Keys.A: {
                    switch (e.Modifiers) {
                        case Keys.Control:
                            txtPoolId.SelectAll();
                            break;

                        default:
                            System.Media.SystemSounds.Beep.Play();
                            break;
                    }
                    e.Handled = true;
                } break;

                case Keys.V: {
                    if (e.Modifiers == Keys.Control && Clipboard.ContainsText()) {
                        if (ApiTools.IsValidPoolLink(Clipboard.GetText())) {
                            string PoolId = ApiTools.GetPoolOrPostId(Clipboard.GetText());
                            txtPoolId.Text = PoolId;
                            System.Media.SystemSounds.Asterisk.Play();
                        }
                    }
                    else {
                        System.Media.SystemSounds.Beep.Play();
                    }
                    e.Handled = true;
                } break;
            }
        }
        private void txtPoolId_ButtonClick(object sender, EventArgs e) {
            txtPoolId.Clear();
        }

        private void btnDownloadPool_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtPoolId.Text)) {
                txtPoolId.Focus();
                txtPoolId.SelectAll();
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            string ID = txtPoolId.Text;
            if (ApiTools.IsValidPoolLink(ID)) {
                ID = ID.Split('/')[4].Split('?')[0];
            }

            frmPoolDownloader DownloadForm = new(
                new(ID) {
                    OpenAfter = chkPoolOpenAfter.Checked,
                    MergeGraylistedPages = chkPoolMergeGraylisted.Checked,
                    SaveBlacklistedFiles = chkPoolDownloadBlacklistedImages.Checked,
                    MergeBlacklistedPages = chkPoolMergeBlacklisted.Checked,
                }
            );
            DownloadForm.Show();
        }
        #endregion

        #region Images
        private void txtImageUrl_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.D0:
                case Keys.D1: case Keys.D2: case Keys.D3:
                case Keys.D4: case Keys.D5: case Keys.D6:
                case Keys.D7: case Keys.D8: case Keys.D9: {
                    e.Handled = e.SuppressKeyPress = e.Shift || e.Alt || e.Control;
                    if (e.Handled) {
                        System.Media.SystemSounds.Beep.Play();
                    }
                } break;

                case Keys.A:
                case Keys.X:
                case Keys.C: {
                    e.Handled = e.SuppressKeyPress = !e.Control || e.Alt || e.Shift;
                    if (e.Handled) {
                        System.Media.SystemSounds.Beep.Play();
                    }
                } break;

                case Keys.V: {
                    e.Handled = e.SuppressKeyPress = true;
                    if (e.Control && !e.Alt && !e.Shift && Clipboard.ContainsText()) {
                        if (ApiTools.IsValidPageLink(Clipboard.GetText())) {
                            txtImageUrl.Text = ApiTools.GetPoolOrPostId(Clipboard.GetText());
                            System.Media.SystemSounds.Asterisk.Play();
                        }
                        else if (ApiTools.IsNumericOnly(Clipboard.GetText())) {
                            e.Handled = e.SuppressKeyPress = false;
                            System.Media.SystemSounds.Asterisk.Play();
                        }
                        else {
                            System.Media.SystemSounds.Beep.Play();
                        }
                    }
                    else {
                        System.Media.SystemSounds.Beep.Play();
                    }
                } break;

                // Suppress the sound if these are pressed.
                // Backspace should work regardless.
                case Keys.Control:
                case Keys.ControlKey:
                case Keys.LControlKey:
                case Keys.RControlKey:
                case Keys.Alt:
                case Keys.Menu:
                case Keys.LMenu:
                case Keys.RMenu:
                case Keys.Shift:
                case Keys.ShiftKey:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                case Keys.Back: {
                    e.Handled = e.SuppressKeyPress = false;
                } break;

                case Keys.Return: {
                    btnDownloadImage_Click(txtImageUrl, new());
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                } break;

                default: {
                    e.Handled = e.SuppressKeyPress = true;
                    System.Media.SystemSounds.Beep.Play();
                } break;
            }
        }
        private void txtImageUrl_ButtonClick(object sender, EventArgs e) {
            txtImageUrl.Clear();
            txtImageUrl.Focus();
        }

        private void btnDownloadImage_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtImageUrl.Text) || !ApiTools.IsValidImageLink(txtImageUrl.Text) && !int.TryParse(txtImageUrl.Text, out _)) {
                txtImageUrl.Focus();
                txtImageUrl.SelectAll();
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            frmImageDownloader DownloadForm = new(
                new(txtImageUrl.Text) {
                    SeparateRatings = chkImageSeparateRatings.Checked,
                    SeparateGraylisted = chkImageSeparateGraylisted.Checked,
                    SeparateBlacklisted = chkImageSeparateBlacklisted.Checked,
                    SeparateNonImages = chkImageSeparateNonImages.Checked,
                    SeparateArtists = chkImageSeparateArtists.Checked,
                    UseForm = chkImageUseForm.Checked,
                    OpenAfter = chkImageOpenAfter.Checked,
                }
            );
            DownloadForm.Show();
        }
        #endregion

    }
}