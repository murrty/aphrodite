using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmBlacklist : Form {

        public frmBlacklist() {
            InitializeComponent();
        }

        private void frmBlacklist_Load(object sender, EventArgs e) {
            rtbBlacklist.Text = Config.Settings.General.blacklist.Replace(" ", "\r\n");
            rtbZTB.Text = Config.Settings.General.zeroToleranceBlacklist.Replace(" ", "\r\n");

            if (Program.UseIni) {
                lbIni.Visible = true;
                lbMutual.Location = new Point(lbMutual.Location.X, (lbIni.Location.X + lbIni.Size.Height));
            }
        }

        private void rtbBlacklist_TextChanged(object sender, EventArgs e) {
            var txtSender = (RichTextBox)sender;
            var curPos = txtSender.SelectionStart;
            txtSender.Text = Regex.Replace(txtSender.Text, " ", "\r\n");
            txtSender.SelectionStart = curPos;
        }
        private void rtbZTB_TextChanged(object sender, EventArgs e) {
            var txtSender = (RichTextBox)sender;
            var curPos = txtSender.SelectionStart;
            txtSender.Text = Regex.Replace(txtSender.Text, " ", "\r\n");
            txtSender.SelectionStart = curPos;
        }

        private void btnSort_Click(object sender, EventArgs e) {
            rtbBlacklist.Text = rtbBlacklist.Text.Replace(" ", "\n");
            ListBox blacklist = new ListBox();
            string[] bl;
            bl = rtbBlacklist.Text.Split('\n');
            for (int y = 0; y < bl.Length; y++) {
                blacklist.Items.Add(bl[y]);
            }
            blacklist.Sorted = true;
            rtbBlacklist.Clear();
            for (int y = 0; y < blacklist.Items.Count; y++) {
                rtbBlacklist.Text += blacklist.Items[y] + "\n";
            }
            rtbBlacklist.Text = rtbBlacklist.Text.TrimEnd('\n');
        }
        private void btnSortZTB_Click(object sender, EventArgs e) {
            rtbZTB.Text = rtbZTB.Text.Replace(" ", "\n");
            ListBox blacklist = new ListBox();
            string[] bl;
            bl = rtbZTB.Text.Split('\n');
            for (int y = 0; y < bl.Length; y++) {
                blacklist.Items.Add(bl[y]);
            }
            blacklist.Sorted = true;
            rtbZTB.Clear();
            for (int y = 0; y < blacklist.Items.Count; y++) {
                rtbZTB.Text += blacklist.Items[y] + "\n";
            }
            rtbZTB.Text = rtbZTB.Text.TrimEnd('\n');
        }

        private void btnSave_Click(object sender, EventArgs e) {
            Config.Settings.General.blacklist =
                rtbBlacklist.Text.Replace(" ", "_").Replace("\r\n", " ").Replace('\n', ' ').Trim(' ');
            Config.Settings.General.zeroToleranceBlacklist =
                rtbZTB.Text.Replace(" ", "_").Replace("\r\n", " ").Replace('\n', ' ').Trim(' ');
            Config.Settings.General.Save();
            this.DialogResult = DialogResult.OK;
        }
        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
        }

        private void llGraylist_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            MessageBox.Show("Tags in this category will be downloaded into a separated folder, unless specified otherwise. If the user disallows blacklisted files from being saved, they will not be downloaded at all.", "aphrodite", MessageBoxButtons.OK);
        }

        private void llBlacklist_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            MessageBox.Show("Tags in this category will never be downloaded no matter what. They will still be parsed for counting, but never added to any download list.", "aphrodite", MessageBoxButtons.OK);
        }
    }
}
