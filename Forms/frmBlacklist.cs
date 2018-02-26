using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmBlacklist : Form {
        public frmBlacklist() {
            InitializeComponent();
        }

        private void frmBlacklist_Load(object sender, EventArgs e) {
            rtbBlacklist.Text = Settings.Default.blacklist.Replace(' ', '\n');
            rtbZTB.Text = Settings.Default.zeroToleranceBlacklist.Replace(' ', '\n');
        }

        private void rtbBlacklist_TextChanged(object sender, EventArgs e) {
            var txtSender = (RichTextBox)sender;
            var curPos = txtSender.SelectionStart;
            txtSender.Text = Regex.Replace(txtSender.Text, " ", "\n");
            txtSender.SelectionStart = curPos;
        }
        private void rtbZTB_TextChanged(object sender, EventArgs e) {
            var txtSender = (RichTextBox)sender;
            var curPos = txtSender.SelectionStart;
            txtSender.Text = Regex.Replace(txtSender.Text, " ", "\n");
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
            Settings.Default.blacklist = rtbBlacklist.Text.Replace('\n', ' ');
            Settings.Default.zeroToleranceBlacklist = rtbZTB.Text.Replace('\n', ' ');
            Settings.Default.Save();
            this.DialogResult = DialogResult.OK;
        }
        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
