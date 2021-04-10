using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace aphrodite {

    public partial class frmPoolWishlist : Form {
        List<string>PoolNames = new List<string>();
        List<string>PoolURLS = new List<string>();

        public frmPoolWishlist(bool AddToWishlist = false, string AddUrl = null, string AddTitle = null) {
            InitializeComponent();
            if (AddToWishlist) {
                txtName.Text = AddTitle.Replace('|', '_');
                txtURL.Text = AddUrl;
            }
            lPoolLink.Text = string.Empty;
        }

        private void frmPoolWishlist_Load(object sender, EventArgs e) {
            lPoolLink.Text = string.Empty;
            if (!string.IsNullOrWhiteSpace(Config.Settings.Pools.wishlist) && !string.IsNullOrWhiteSpace(Config.Settings.Pools.wishlistNames)) {
                PoolNames.AddRange(Config.Settings.Pools.wishlistNames.Split('|'));
                PoolURLS.AddRange(Config.Settings.Pools.wishlist.Split('|'));
                listWishlistItems.Items.AddRange(PoolNames.ToArray());
            }
        }

        private void frmPoolWishlist_FormClosing(object sender, FormClosingEventArgs e) {
            Config.Config_Pools.SaveWishlist(PoolNames, PoolURLS);
        }

        private void btnAddUpdate_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(txtURL.Text) || string.IsNullOrEmpty(txtName.Text)) {
                return;
            }

            if (apiTools.IsValidPoolLink(txtURL.Text)) {
                if (chkUpdate.Checked) {
                    PoolNames[listWishlistItems.SelectedIndex] = txtName.Text;
                    PoolURLS[listWishlistItems.SelectedIndex] = txtURL.Text;
                    listWishlistItems.Items[listWishlistItems.SelectedIndex] = txtName.Text;
                    lPoolLink.Text = txtURL.Text;
                }
                else {
                    PoolURLS.Add(txtURL.Text);
                    PoolNames.Add(txtName.Text);
                    listWishlistItems.Items.Add(txtName.Text);
                    txtName.Clear();
                    txtURL.Clear();
                }
            }
            else {
                System.Media.SystemSounds.Hand.Play();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e) {
            int indx = listWishlistItems.SelectedIndex;
            if (indx > -1) {
                PoolURLS.RemoveAt(listWishlistItems.SelectedIndex);
                PoolNames.RemoveAt(listWishlistItems.SelectedIndex);
                listWishlistItems.Items.RemoveAt(listWishlistItems.SelectedIndex);
                if (indx == 0 && listWishlistItems.Items.Count > 0) {
                    listWishlistItems.SelectedIndex = 0;
                }
                else {
                    listWishlistItems.SelectedIndex = indx - 1;
                }
            }
        }

        private void btnDownload_Click(object sender, EventArgs e) {
            if (apiTools.IsValidPoolLink(PoolURLS[listWishlistItems.SelectedIndex])) {            
                Downloader.Arguments.DownloadPool(apiTools.GetPoolOrPostId(PoolURLS[listWishlistItems.SelectedIndex]));
            }
        }

        private void lbWish_SelectedIndexChanged(object sender, EventArgs e) {
            if (listWishlistItems.SelectedIndex > -1) {
                if (chkUpdate.Checked) {
                    txtName.Text = PoolNames[listWishlistItems.SelectedIndex];
                    txtURL.Text = PoolURLS[listWishlistItems.SelectedIndex];
                    lPoolLink.Text = PoolURLS[listWishlistItems.SelectedIndex];
                    lPoolLink.Visible = true;
                }
                else {
                    lPoolLink.Text = PoolURLS[listWishlistItems.SelectedIndex];
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
                if (listWishlistItems.SelectedIndex > -1) {
                    txtName.Text = PoolNames[listWishlistItems.SelectedIndex];
                    txtURL.Text = PoolURLS[listWishlistItems.SelectedIndex];
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
            if (e.Button == System.Windows.Forms.MouseButtons.Left) { 
                Process.Start(PoolURLS[listWishlistItems.SelectedIndex]);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                menuWishlist.Show(lPoolLink, new System.Drawing.Point(0, lPoolLink.Height - 2));
            }
        }

        private void mOpenPoolInBrowser_Click(object sender, EventArgs e) {
            if (apiTools.IsValidPoolLink(PoolURLS[listWishlistItems.SelectedIndex])) {
                Process.Start(PoolURLS[listWishlistItems.SelectedIndex]);
            }
        }

        private void mCopyPoolLink_Click(object sender, EventArgs e) {
            if (apiTools.IsValidPoolLink(PoolURLS[listWishlistItems.SelectedIndex])) {
                Clipboard.SetText(PoolURLS[listWishlistItems.SelectedIndex]);
            }
        }

        private void mCopyPoolId_Click(object sender, EventArgs e) {
            if (apiTools.IsValidPoolLink(PoolURLS[listWishlistItems.SelectedIndex])) {
                Clipboard.SetText(apiTools.GetPoolOrPostId(PoolURLS[listWishlistItems.SelectedIndex]));
            }
        }
    }
}
