using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmBlacklist : Form {
        public frmBlacklist() {
            InitializeComponent();
        }

        private void frmBlacklist_Load(object sender, EventArgs e) {
            rtbBlacklist.Text = Settings.Default.blacklist.Replace(' ', '\n');
        }

        private void btnSort_Click(object sender, EventArgs e) {
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

        private void btnSave_Click(object sender, EventArgs e) {
            Clipboard.SetText(rtbBlacklist.Text.Replace('\n', ' '));
            Settings.Default.blacklist = rtbBlacklist.Text.Replace('\n', ' ');
            Settings.Default.Save();
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
