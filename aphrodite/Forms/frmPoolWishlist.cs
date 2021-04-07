using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace aphrodite {

    public partial class frmPoolWishlist : Form {
        List<string>PoolNames = new List<string>();
        List<string>PoolURLS = new List<string>();

        public frmPoolWishlist(bool AddToWishlist = false, string AddUrl = null, string AddTitle = null) {
            InitializeComponent();
            NativeMethods.SendMessage(txtName.Handle, 0x1501, (IntPtr)1, "Pool name...");
            NativeMethods.SendMessage(txtURL.Handle, 0x1501, (IntPtr)1, "Pool url...");

            if (AddToWishlist) {
                txtName.Text = AddTitle.Replace('|', '_');
                txtURL.Text = AddUrl;
            }
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
            if (string.IsNullOrEmpty(txtURL.Text) || string.IsNullOrEmpty(txtName.Text))
                return;

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
            string poolID = PoolURLS[listWishlistItems.SelectedIndex].Replace("http://", "https://").Replace("www.", "");
            if (!poolID.StartsWith("https://"))
                poolID = "https://" + poolID;
            if (poolID.Contains("?"))
                poolID = poolID.Split('?')[0];
            poolID = poolID.Split('/')[5];
            
            Downloader.Arguments.DownloadPool(poolID);
        }

        private void lbWish_SelectedIndexChanged(object sender, EventArgs e) {
            if (chkUpdate.Checked && listWishlistItems.SelectedItem != null) {
                txtName.Text = PoolNames[listWishlistItems.SelectedIndex];
                txtURL.Text = PoolURLS[listWishlistItems.SelectedIndex];
                lPoolLink.Text = PoolURLS[listWishlistItems.SelectedIndex];
            }
            else if (listWishlistItems.SelectedItem != null) {
                lPoolLink.Text = PoolURLS[listWishlistItems.SelectedIndex];
            }
            else {
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
            }
            else {
                btnAddUpdate.Text = "Add";
                txtName.Clear();
                txtURL.Clear();
            }
        }

        private void lPoolLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(PoolURLS[listWishlistItems.SelectedIndex]);
        }
    }
}
