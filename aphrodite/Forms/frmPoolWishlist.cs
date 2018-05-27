using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmPoolWishlist : Form {

        public bool addToWishlist = false;
        public string addURL = string.Empty;
        public string addTitle = string.Empty;

        List<string>PoolNames = new List<string>();
        List<string>PoolURLS = new List<string>();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, string lp);
        private void SetTextBoxHint(IntPtr TextboxHandle, string Hint) {
            SendMessage(TextboxHandle, 0x1501, (IntPtr)1, Hint);
        }

        public frmPoolWishlist() {
            InitializeComponent();
            SetTextBoxHint(txtName.Handle, "Pool name...");
            SetTextBoxHint(txtURL.Handle, "Pool url...");
        }

        private void frmPoolWishlist_Load(object sender, EventArgs e) {
            lPoolLink.Text = string.Empty;
            if (!string.IsNullOrWhiteSpace(Pools.Default.wishlist) && !string.IsNullOrWhiteSpace(Pools.Default.wishhlistNames)) {
                lbWish.Items.AddRange(Pools.Default.wishhlistNames.Split('|'));
                PoolNames.AddRange(Pools.Default.wishhlistNames.Split('|'));
                PoolURLS.AddRange(Pools.Default.wishlist.Split('|'));
            }

            if (addToWishlist) {
                txtName.Text = addTitle;
                txtURL.Text = addURL;
            }
        }

        private void frmPoolWishlist_FormClosing(object sender, FormClosingEventArgs e) {
            string urls = string.Empty;
            string names = string.Empty;
            for (int i = 0; i < lbWish.Items.Count; i++) {
                urls += PoolURLS[i] + "|";
                names += PoolNames[i] + "|";
            }

            urls = urls.TrimEnd('|');
            names = names.TrimEnd('|');
            Pools.Default.wishlist = urls;
            Pools.Default.wishhlistNames = names;
            Pools.Default.Save();
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
                if (indx == 0) {
                    lbWish.SelectedIndex = 0;
                }
                else {
                    lbWish.SelectedIndex = indx - 1;
                }
            }
        }

        private void txtOpen_Click(object sender, EventArgs e) {
            string poolID = PoolURLS[lbWish.SelectedIndex].Replace("http://", "https://").Replace("www.", "");
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
