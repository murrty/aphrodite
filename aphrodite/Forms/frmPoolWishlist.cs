using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace aphrodite {

    public partial class frmPoolWishlist : Form {
        private readonly List<string> PoolNames;
        private readonly List<string> PoolURLS;
        private bool WishlistUpdated = false;

        public frmPoolWishlist(string NewPoolURL = null, string NewPoolTitle = null) {
            InitializeComponent();

            if (Config.ValidPoint(Config.Settings.FormSettings.frmPoolWishlist_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmPoolWishlist_Location;
            }

            txtName.Text = NewPoolTitle.Replace('|', '_') ?? "";
            txtURL.Text = NewPoolURL ?? "";

            lPoolLink.Text = string.Empty;
            PoolNames = PoolWishlist.PoolNames.Count > 0 ? new(PoolWishlist.PoolNames) : new();
            PoolURLS = PoolWishlist.PoolURLs.Count > 0 ? new(PoolWishlist.PoolURLs) : new();

            if (PoolNames.Count == PoolURLS.Count) {
                if (PoolNames.Count > 0) {
                    for (int CurrentName = 0; CurrentName < PoolNames.Count; CurrentName++) {
                        lbWishlistItems.Items.Add(PoolNames[CurrentName]);
                    }
                }
            }
            else {
                Log.ReportException(new ArgumentOutOfRangeException("PoolNames and PoolURLS lists do not match in count. Cannot load."));
            }
        }

        private void frmPoolWishlist_FormClosing(object sender, FormClosingEventArgs e) {
            if (WishlistUpdated) {
                switch (Log.MessageBox("Would you like to save the wishlist?", MessageBoxButtons.YesNoCancel)) {
                    case DialogResult.Yes: {
                        PoolWishlist.PoolNames = PoolNames;
                        PoolWishlist.PoolURLs = PoolURLS;
                        PoolWishlist.SaveWishlist();
                        Config.Settings.FormSettings.frmPoolWishlist_Location = this.Location;
                    } break;

                    case DialogResult.Cancel: {
                        e.Cancel = true;
                    } return;
                }
            }
        }

        private void btnAddUpdate_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(txtURL.Text)) {
                txtURL.Focus();
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }
            if (!ApiTools.IsValidPoolLink(txtURL.Text)) {
                txtURL.Focus();
                txtURL.SelectAll();
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }
            if (string.IsNullOrEmpty(txtName.Text)) {
                txtName.Focus();
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            if (chkUpdate.Checked) {
                PoolNames[lbWishlistItems.SelectedIndex] = txtName.Text.Replace("|", "_");
                PoolURLS[lbWishlistItems.SelectedIndex] = txtURL.Text;
                lbWishlistItems.Items[lbWishlistItems.SelectedIndex] = txtName.Text.Replace("|", "_");
                lPoolLink.Text = txtURL.Text;
            }
            else {
                PoolURLS.Add(txtURL.Text);
                PoolNames.Add(txtName.Text.Replace("|", "_"));
                lbWishlistItems.Items.Add(txtName.Text.Replace("|", "_"));
                txtName.Clear();
                txtURL.Clear();
            }

            WishlistUpdated = true;
        }

        private void btnRemove_Click(object sender, EventArgs e) {
            int PoolIndex = lbWishlistItems.SelectedIndex;
            if (PoolIndex > -1) {
                PoolURLS.RemoveAt(lbWishlistItems.SelectedIndex);
                PoolNames.RemoveAt(lbWishlistItems.SelectedIndex);
                lbWishlistItems.Items.RemoveAt(lbWishlistItems.SelectedIndex);
                lbWishlistItems.SelectedIndex = PoolIndex == 0 && lbWishlistItems.Items.Count > 0 ? 0 : PoolIndex - 1;
                WishlistUpdated = true;
            }
        }
        private void btnDownload_Click(object sender, EventArgs e) {
            if (ApiTools.IsValidPoolLink(PoolURLS[lbWishlistItems.SelectedIndex])) {            
                Downloader.DownloadPool(ApiTools.GetPoolOrPostId(PoolURLS[lbWishlistItems.SelectedIndex]), true);
            }
        }

        private void lbWish_SelectedIndexChanged(object sender, EventArgs e) {
            if (lbWishlistItems.SelectedIndex > -1) {
                if (chkUpdate.Checked) {
                    txtName.Text = PoolNames[lbWishlistItems.SelectedIndex];
                    txtURL.Text = PoolURLS[lbWishlistItems.SelectedIndex];
                    lPoolLink.Text = PoolURLS[lbWishlistItems.SelectedIndex];
                    lPoolLink.Visible = true;
                }
                else {
                    lPoolLink.Text = PoolURLS[lbWishlistItems.SelectedIndex];
                    lPoolLink.Visible = true;
                }
            }
            else {
                lPoolLink.Visible = false;
                lPoolLink.Text = string.Empty;
            }
        }

        private void chkUpdate_CheckedChanged(object sender, EventArgs e) {
            if (chkUpdate.Checked) {
                btnAddUpdate.Text = "Update";
                if (lbWishlistItems.SelectedIndex > -1) {
                    txtName.Text = PoolNames[lbWishlistItems.SelectedIndex];
                    txtURL.Text = PoolURLS[lbWishlistItems.SelectedIndex];
                }
                lPoolLink.Visible = false;
            }
            else {
                btnAddUpdate.Text = "Add";
                txtName.Clear();
                txtURL.Clear();
            }
        }

        private void lPoolLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if (e.Button == MouseButtons.Left) { 
                Process.Start(PoolURLS[lbWishlistItems.SelectedIndex]);
            }
            else if (e.Button == MouseButtons.Right) {
                cmWishlist.Show(lPoolLink, new System.Drawing.Point(0, lPoolLink.Height - 2));
            }
        }

        private void mOpenPoolInBrowser_Click(object sender, EventArgs e) {
            if (ApiTools.IsValidPoolLink(PoolURLS[lbWishlistItems.SelectedIndex])) {
                Process.Start(PoolURLS[lbWishlistItems.SelectedIndex]);
                System.Media.SystemSounds.Asterisk.Play();
            }
            else {
                System.Media.SystemSounds.Exclamation.Play();
            }
        }

        private void mCopyPoolLink_Click(object sender, EventArgs e) {
            if (ApiTools.IsValidPoolLink(PoolURLS[lbWishlistItems.SelectedIndex])) {
                Clipboard.SetText(PoolURLS[lbWishlistItems.SelectedIndex]);
                System.Media.SystemSounds.Asterisk.Play();
            }
            else {
                System.Media.SystemSounds.Exclamation.Play();
            }
        }

        private void mCopyPoolId_Click(object sender, EventArgs e) {
            if (ApiTools.IsValidPoolLink(PoolURLS[lbWishlistItems.SelectedIndex])) {
                Clipboard.SetText(ApiTools.GetPoolOrPostId(PoolURLS[lbWishlistItems.SelectedIndex]));
                System.Media.SystemSounds.Asterisk.Play();
            }
            else {
                System.Media.SystemSounds.Exclamation.Play();
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            if (WishlistUpdated) {
                Log.Write("Wishlist has been updated, saving new wishlist");
                PoolWishlist.PoolNames = PoolNames;
                PoolWishlist.PoolURLs = PoolURLS;
                PoolWishlist.SaveWishlist();
                Config.Settings.FormSettings.frmPoolWishlist_Location = this.Location;
                WishlistUpdated = false;
            }

            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            WishlistUpdated = false;
            this.Dispose();
        }
    }
}
