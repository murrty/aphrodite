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
            if (string.IsNullOrWhiteSpace(General.Default.saveLocation)) {
                General.Default.saveLocation = Environment.CurrentDirectory;
            }

            if (Program.UseIni) {
                mProtocol.Visible = false;
                mProtocol.Enabled = false;
                lbIni.Visible = true;

                chkTagsDownloadExplicit.Checked = Program.Ini.ReadBool("Explicit", "Tags");
                chkTagsDownloadQuestionable.Checked = Program.Ini.ReadBool("Questionable", "Tags");
                chkTagsDownloadSafe.Checked = Program.Ini.ReadBool("Safe", "Tags");
                chkTagsSeparateRatings.Checked = Program.Ini.ReadBool("separateRatings", "Tags");
                chkTagsUseMinimumScore.Checked = Program.Ini.ReadBool("useMinimumScore", "Tags");
                chkTagsUseScoreAsTag.Checked = Program.Ini.ReadBool("scoreAsTag", "Tags");
                numTagsMinimumScore.Value = Convert.ToDecimal(Program.Ini.ReadInt("scoreMin", "Tags"));
                numTagsImageLimit.Value = Convert.ToDecimal(Program.Ini.ReadInt("imageLimit", "Tags"));
                numTagsPageLimit.Value = Convert.ToDecimal(Program.Ini.ReadInt("pageLimit", "Tags"));

                chkPoolsMergeBlacklisted.Checked = Program.Ini.ReadBool("mergeBlacklisted", "Pools");
                chkPoolsOpenAfter.Checked = Program.Ini.ReadBool("openAfter", "Pools");

                chkImageSeparateRatings.Checked = Program.Ini.ReadBool("separateRatings", "Images");
                chkImageSeparateBlacklisted.Checked = Program.Ini.ReadBool("separateBlacklisted", "Images");
                chkImageUseForm.Checked = Program.Ini.ReadBool("useForm", "Images");

                if (Program.Ini.KeyExists("frmMainLocation", "FormSettings")) {
                    Point p = Program.Ini.ReadPoint("frmMainLocation", "FormSettings");
                    if (p.X == -32000 || p.Y == -32000) {
                        this.StartPosition = FormStartPosition.CenterScreen;
                    }
                    else {
                        this.Location = p;
                    }
                }
            }
            else {
                chkTagsDownloadExplicit.Checked = Tags.Default.Explicit;
                chkTagsDownloadQuestionable.Checked = Tags.Default.Questionable;
                chkTagsDownloadSafe.Checked = Tags.Default.Safe;
                chkTagsSeparateRatings.Checked = Tags.Default.separateRatings;
                chkTagsUseMinimumScore.Checked = Tags.Default.enableScoreMin;
                chkTagsUseScoreAsTag.Checked = Tags.Default.scoreAsTag;
                numTagsMinimumScore.Value = Convert.ToDecimal(Tags.Default.scoreMin);
                numTagsImageLimit.Value = Convert.ToDecimal(Tags.Default.imageLimit);
                numTagsPageLimit.Value = Convert.ToDecimal(Tags.Default.pageLimit);

                chkPoolsMergeBlacklisted.Checked = Pools.Default.mergeBlacklisted;
                chkPoolsOpenAfter.Checked = Pools.Default.openAfter;

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

            if (FormSettings.Default.frmMainLocation.X != -32000 && FormSettings.Default.frmMainLocation.Y != -32000) {
                if (Program.UseIni) {
                    this.Location = new Point(Program.Ini.ReadInt("frmMainX", "Forms"), Program.Ini.ReadInt("frmMainY", "Forms"));
                }
                else {
                    this.Location = FormSettings.Default.frmMainLocation;
                }
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

            if (Program.UseIni) {
                Program.Ini.WritePoint("frmMainLocation", this.Location, "FormSettings");
            }
            else {
                FormSettings.Default.frmMainLocation = this.Location;
                FormSettings.Default.Save();
            }
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

            if (Program.UseIni) {
                chkTagsDownloadExplicit.Checked = Program.Ini.ReadBool("Explicit", "Tags");
                chkTagsDownloadQuestionable.Checked = Program.Ini.ReadBool("Questionable", "Tags");
                chkTagsDownloadSafe.Checked = Program.Ini.ReadBool("Safe", "Tags");
                chkTagsSeparateRatings.Checked = Program.Ini.ReadBool("separateRatings", "Tags");
                chkTagsUseMinimumScore.Checked = Program.Ini.ReadBool("useMinimumScore", "Tags");
                chkTagsUseScoreAsTag.Checked = Program.Ini.ReadBool("scoreAsTag", "Tags");
                numTagsMinimumScore.Value = Convert.ToDecimal(Program.Ini.ReadInt("scoreMin", "Tags"));
                numTagsImageLimit.Value = Convert.ToDecimal(Program.Ini.ReadInt("imageLimit", "Tags"));
                numTagsPageLimit.Value = Convert.ToDecimal(Program.Ini.ReadInt("pageLimit", "Tags"));

                chkPoolsOpenAfter.Checked = Program.Ini.ReadBool("openAfter", "Pools");
                chkPoolsMergeBlacklisted.Checked = Program.Ini.ReadBool("mergeBlacklisted", "Pools");
            }
            else {
                chkTagsDownloadExplicit.Checked = Tags.Default.Explicit;
                chkTagsDownloadQuestionable.Checked = Tags.Default.Questionable;
                chkTagsDownloadSafe.Checked = Tags.Default.Safe;
                chkTagsSeparateRatings.Checked = Tags.Default.separateRatings;
                chkTagsUseMinimumScore.Checked = Tags.Default.enableScoreMin;
                chkTagsUseScoreAsTag.Checked = Tags.Default.scoreAsTag;
                numTagsMinimumScore.Value = Convert.ToDecimal(Tags.Default.scoreMin);
                numTagsImageLimit.Value = Convert.ToDecimal(Tags.Default.imageLimit);
                numTagsPageLimit.Value = Convert.ToDecimal(Tags.Default.pageLimit);

                chkPoolsOpenAfter.Checked = Pools.Default.openAfter;
                chkPoolsMergeBlacklisted.Checked = Pools.Default.mergeBlacklisted;
            }

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
            Pools.Default.Reload();
            frmPoolWishlist wl = new frmPoolWishlist();
            wl.ShowDialog();
            wl.Dispose();
        }
        private void mRedownloader_Click(object sender, EventArgs e) {
            frmRedownloader rd = new frmRedownloader();
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
            settings.InstallProtocol = true;
            settings.ShowDialog();
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

            if (NewInfo.UseForm) {
                ImageDownloader Downloader = new ImageDownloader();
                Downloader.DownloadInfo = NewInfo;
                Downloader.downloadImage();
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