using System;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmUpdateAvailable : Form {

        /// <summary>
        /// Whether the "Skip version" button should be disabled, false if it's an automatic check.
        /// </summary>
        public bool BlockSkip {
            get; set;
        } = false;

        /// <summary>
        /// Whether the update is a pre-release.
        /// </summary>
        public bool PreRelease {
            get; set;
        } = false;

        /// <summary>
        /// The new version tag, containing the version available to update to.
        /// </summary>
        public string NewVersion {
            get; set;
        }

        /// <summary>
        /// The new version update header, containing quick information about the release.
        /// </summary>
        public string UpdateHeader {
            get; set;
        }

        /// <summary>
        /// The new version update body, containing full update details.
        /// </summary>
        public string UpdateBody {
            get; set;
        }

        public frmUpdateAvailable() {
            InitializeComponent();
            lbUpdateAvailableCurrentVersion.Text = $"Current version: {(Program.IsBetaVersion ? Program.BetaVersion : Program.CurrentVersion)}";
        }

        private void frmUpdateAvailable_Load(object sender, EventArgs e) {
            btnUpdateAvailableSkip.Enabled = !BlockSkip;
            if (PreRelease) {
                lbUpdateAvailableHeader.Text = "A beta update is available";
            }
            lbUpdateAvailableUpdateVersion.Text = string.Format("Update version: {0}", NewVersion ?? "unknown");
            txtUpdateAvailableName.Text = UpdateHeader ?? "New version available";
            rtbUpdateAvailableChangelog.Text = UpdateBody ?? "A new version has been released, but no information was given to this form to show you.";
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
