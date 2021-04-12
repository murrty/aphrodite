using System;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmUpdateAvailable : Form {
        public bool BlockSkip = false;
        public string NewVersion;
        public string UpdateHeader;
        public string UpdateBody;

        public frmUpdateAvailable() {
            InitializeComponent();
            lbUpdateAvailableCurrentVersion.Text = string.Format("Current version: {0}", Properties.Settings.Default.CurrentVersion);
        }
        private void frmUpdateAvailable_Load(object sender, EventArgs e) {
            btnUpdateAvailableSkip.Enabled = !BlockSkip;
            if (NewVersion != null && UpdateHeader != null && UpdateBody != null) {
                lbUpdateAvailableUpdateVersion.Text = string.Format("Update version: {0}", NewVersion);
                txtUpdateAvailableName.Text = UpdateHeader;
                rtbUpdateAvailableChangelog.Text = UpdateBody;
            }
            else {
                lbUpdateAvailableUpdateVersion.Text = "Unknown version";
                txtUpdateAvailableName.Text = "New version available";
                rtbUpdateAvailableChangelog.Text = "A new version has been released, but no information was given to this form to show you.";
            }
        }

        private void btnUpdateAvailableSkip_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Ignore;
        }
        private void btnUpdateAvailableUpdate_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Yes;
        }

        private void btnUpdateAvailableOk_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
        }

        private void rtbUpdateAvailableChangelog_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = true;
        }
    }
}
