using Microsoft.Win32;
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
        // Valid protocols:
        //                  'pools:'
        //                  'tags:'
        //                  'images:'
        #endregion

        #region Form
        public frmMain(string Argument = null, DownloadType Type = DownloadType.None) {
            InitializeComponent();

            if (Argument != null && Type != DownloadType.None) {
                switch (Type) {
                    case DownloadType.Tags:
                        txtTags.Text = Argument;
                        break;
                    case DownloadType.Pools:
                        txtPoolId.Text = Argument;
                        break;
                    case DownloadType.Images:
                        txtImageUrl.Text = Argument;
                        break;
                }
            }

            if (Program.IsDebug) {
                this.Text += " (debug " + Properties.Settings.Default.debugDate + ")";
            }

            NativeMethods.SendMessage(txtTags.Handle, 0x1501, (IntPtr)1, "Tags to download...");
            NativeMethods.SendMessage(txtPoolId.Handle, 0x1501, (IntPtr)1, "Pool ID...");
            NativeMethods.SendMessage(txtImageUrl.Handle, 0x1501, (IntPtr)1, "Image ID / URL...");
        }
        private void frmMain_Load(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(Config.Settings.General.saveLocation)) {
                Config.Settings.General.saveLocation = Environment.CurrentDirectory;
            }

            if (Program.UseIni) {
                lbIni.Visible = true;
            }
            else {
                tbMain.TabPages.Remove(tbIni);
            }

            chkTagsDownloadExplicit.Checked = Config.Settings.Tags.Explicit;
            chkTagsDownloadQuestionable.Checked = Config.Settings.Tags.Questionable;
            chkTagsDownloadSafe.Checked = Config.Settings.Tags.Safe;
            chkTagsSeparateRatings.Checked = Config.Settings.Tags.separateRatings;
            chkTagsUseMinimumScore.Checked = Config.Settings.Tags.enableScoreMin;
            chkTagsUseScoreAsTag.Checked = Config.Settings.Tags.scoreAsTag;
            numTagsMinimumScore.Value = Convert.ToDecimal(Config.Settings.Tags.scoreMin);
            numTagsImageLimit.Value = Convert.ToDecimal(Config.Settings.Tags.imageLimit);
            numTagsPageLimit.Value = Convert.ToDecimal(Config.Settings.Tags.pageLimit);

            chkPoolsMergeBlacklisted.Checked = Config.Settings.Pools.mergeBlacklisted;
            chkPoolsOpenAfter.Checked = Config.Settings.Pools.openAfter;

            chkImageSeparateRatings.Checked = Config.Settings.Images.separateRatings;
            chkImageSeparateBlacklisted.Checked = Config.Settings.Images.separateBlacklisted;
            chkImageUseForm.Checked = Config.Settings.Images.useForm;

            if (Config.Settings.FormSettings.frmMainLocation.X != -32000 && Config.Settings.FormSettings.frmMainLocation.Y != -32000) {
                this.Location = Config.Settings.FormSettings.frmMainLocation;
            }
        }
        private void frmMain_Shown(object sender, EventArgs e) {
            txtTags.Focus();
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e) {
            this.Opacity = 0;

            if (this.WindowState != FormWindowState.Normal) {
                this.WindowState = FormWindowState.Normal;
            }

            Config.Settings.FormSettings.frmMainLocation = this.Location;
            Config.Settings.Save();
        }
        private void tbMain_SelectedIndexChanged(object sender, EventArgs e) {
            if (tbMain.SelectedTab == tbTags) {
                txtTags.Focus();
                this.AcceptButton = btnDownloadTags;
            }
            else if (tbMain.SelectedTab == tbPools) {
                txtPoolId.Focus();
                this.AcceptButton = btnDownloadPool;
            }
            else if (tbMain.SelectedTab == tbImages) {
                txtImageUrl.Focus();
                this.AcceptButton = btnDownloadImage;
            }
        }

        private void mSettings_Click(object sender, EventArgs e) {
            frmSettings settings = new frmSettings();
            settings.ShowDialog();
        }
        private void mBlacklist_Click(object sender, EventArgs e) {
            frmBlacklist blackList = new frmBlacklist();
            blackList.ShowDialog();
        }
        private void mReverseSearch_Click(object sender, EventArgs e) {
            //Process.Start("https://saucenao.com/index.php");
            Process.Start("https://e621.net/iqdb_queries");
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

        private void mAbout_Click(object sender, EventArgs e) {
            frmAbout frAbout = new frmAbout();
            frAbout.Show();
        }
        #endregion

        #region Tags
        private void txtTags_KeyPress(object sender, KeyPressEventArgs e) {
            // Enforce 6 tag limit
            if (txtTags.Text.Count(x => x == ' ') >= 5 &&
                e.KeyChar != (char)Keys.Back &&
                e.KeyChar == (char)Keys.Space &&
                txtTags.SelectionLength != txtTags.TextLength)
            {
                e.Handled = true;
            }
            else {
                e.Handled = false;
            }
        }
        private void txtTags_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                btnDownloadTags_Click(this, new EventArgs());
                e.Handled = true;
            }
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
            NewInfo.MinimumScoreAsTag = chkTagsUseScoreAsTag.Checked;
            NewInfo.MinimumScore = (int)numTagsMinimumScore.Value;
            NewInfo.ImageLimit = (int)numTagsImageLimit.Value;
            NewInfo.SaveExplicit = chkTagsDownloadExplicit.Checked;
            NewInfo.SaveQuestionable = chkTagsDownloadQuestionable.Checked;
            NewInfo.SaveSafe = chkTagsDownloadSafe.Checked;
            NewInfo.SeparateRatings = chkTagsSeparateRatings.Checked;
            NewInfo.SeparateNonImages = chkTagSeparateNonImages.Checked;
            NewInfo.OpenAfter = chkTagOpenAfterDownload.Checked;
            frmTagDownloader Downloader = new frmTagDownloader();
            Downloader.DownloadInfo = NewInfo;
            Downloader.Show();
        }

        private void chkTagsUseMinimumScore_CheckedChanged(object sender, EventArgs e) {
            numTagsMinimumScore.Enabled = chkTagsUseMinimumScore.Checked;
            chkTagsUseScoreAsTag.Enabled = chkTagsUseMinimumScore.Checked;
        }
        #endregion

        #region Pools
        private void btnDownloadPool_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtPoolId.Text)) {
                MessageBox.Show("Please specify pool ID to download.");
                return;
            }
            string ID = txtPoolId.Text;
            if (apiTools.IsValidPoolLink(ID)) {
                ID = apiTools.GetPoolIdFromUrl(ID);
            }

            PoolDownloadInfo NewInfo = new PoolDownloadInfo(txtPoolId.Text);
            NewInfo.OpenAfter = chkPoolsOpenAfter.Checked;
            NewInfo.MergeBlacklisted = chkPoolsMergeBlacklisted.Checked;
            frmPoolDownloader Downloader = new frmPoolDownloader();

        }

        private void txtPoolId_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = true;
            switch (e.KeyChar) {
                case '0': case (char)Keys.Back:
                case '1': case '2': case '3':
                case '4': case '5': case '6':
                case '7': case '8': case '9':
                    e.Handled = false;
                    break;
            }
        }
        private void txtPoolId_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                btnDownloadPool_Click(this, new EventArgs());
                e.Handled = true;
            }
        }
        #endregion

        #region Images
        private void btnDownloadImage_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtImageUrl.Text)) {
                MessageBox.Show("Please enter a valid image url or id");
                return;
            }

            string Image = txtImageUrl.Text;

            if (apiTools.IsValidImageLink(txtImageUrl.Text)) {
                Image = apiTools.GetPostIdFromUrl(Image);
            }

            ImageDownloadInfo NewInfo = new ImageDownloadInfo(Image);
            NewInfo.SeparateRatings = chkImageSeparateRatings.Checked;
            NewInfo.SeparateBlacklisted = chkImageSeparateBlacklisted.Checked;
            NewInfo.SeparateArtists = chkImageSeparateArtists.Checked;
            NewInfo.UseForm = chkImageUseForm.Checked;
            NewInfo.SeparateNonImages = chkImageSeparateNonImages.Checked;
            NewInfo.OpenAfter = chkImageOpenAfter.Checked;

            if (NewInfo.UseForm) {
                ImageDownloader Downloader = new ImageDownloader(NewInfo);
            }
            else {
                frmImageDownloader Downloader = new frmImageDownloader();
                Downloader.DownloadInfo = NewInfo;
                Downloader.Show();
            }
        }
        private void txtImageUrl_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                btnDownloadImage_Click(this, new EventArgs());
            }
        }
        #endregion

    }
}