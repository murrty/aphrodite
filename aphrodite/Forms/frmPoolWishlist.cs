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
                lbWish.Items.AddRange(PoolNames.ToArray());
            }
        }

        private void frmPoolWishlist_FormClosing(object sender, FormClosingEventArgs e) {
            Config.Config_Pools.SaveWishlist(PoolNames, PoolURLS);
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(txtURL.Text) || string.IsNullOrEmpty(txtName.Text))
                return;

            if (chkUpdate.Checked) {
                PoolNames[lbWish.SelectedIndex] = txtName.Text;
                PoolURLS[lbWish.SelectedIndex] = txtURL.Text;
                lbWish.Items[lbWish.SelectedIndex] = txtName.Text;
                lPoolLink.Text = txtURL.Text;
            }
            else {
                PoolURLS.Add(txtURL.Text);
                PoolNames.Add(txtName.Text);
                lbWish.Items.Add(txtName.Text);
                txtName.Clear();
                txtURL.Clear();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e) {
            int indx = lbWish.SelectedIndex;
            if (indx > -1) {
                PoolURLS.RemoveAt(lbWish.SelectedIndex);
                PoolNames.RemoveAt(lbWish.SelectedIndex);
                lbWish.Items.RemoveAt(lbWish.SelectedIndex);
                if (indx == 0 && lbWish.Items.Count > 0) {
                    lbWish.SelectedIndex = 0;
                }
                else {
                    lbWish.SelectedIndex = indx - 1;
                }
            }
        }

        private void btnDownload_Click(object sender, EventArgs e) {
            string poolID = PoolURLS[lbWish.SelectedIndex].Replace("http://", "https://").Replace("www.", "");
            if (!poolID.StartsWith("https://"))
                poolID = "https://" + poolID;
            if (poolID.Contains("?"))
                poolID = poolID.Split('?')[0];
            poolID = poolID.Split('/')[5];
            
            Downloader.Arguments.DownloadPool(poolID);
        }

        private void lbWish_SelectedIndexChanged(object sender, EventArgs e) {
            if (chkUpdate.Checked && lbWish.SelectedItem != null) {
                txtName.Text = PoolNames[lbWish.SelectedIndex];
                txtURL.Text = PoolURLS[lbWish.SelectedIndex];
                lPoolLink.Text = PoolURLS[lbWish.SelectedIndex];
            }
            else if (lbWish.SelectedItem != null) {
                lPoolLink.Text = PoolURLS[lbWish.SelectedIndex];
            }
            else {
                lPoolLink.Text = string.Empty;
            }
        }

        private void chkUpdate_CheckedChanged(object sender, EventArgs e) {
            if (chkUpdate.Checked) {
                btnAdd.Text = "Update";
                if (lbWish.SelectedIndex > -1) {
                    txtName.Text = PoolNames[lbWish.SelectedIndex];
                    txtURL.Text = PoolURLS[lbWish.SelectedIndex];
                }
            }
            else {
                btnAdd.Text = "Add";
                txtName.Clear();
                txtURL.Clear();
            }
        }

        private void lPoolLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(PoolURLS[lbWish.SelectedIndex]);
        }
    }
}
