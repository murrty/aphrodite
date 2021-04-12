using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmAbout : Form {
        Thread UpdateCheckThread;

        public frmAbout() {
            InitializeComponent();
            pbIcon.Cursor = NativeMethods.SystemHandCursor;
            llbCheckForUpdates.Cursor = NativeMethods.SystemHandCursor;
            lnkLicense.Cursor = NativeMethods.SystemHandCursor;
            lbBody.Text += "\r\n\r\n(last debug date " + Properties.Settings.Default.DebugDate + ")";
            if (Program.IsDebug) {
                this.Text = "About aphrodite (debug)";
            }
            else {
                if (Properties.Settings.Default.IsBetaVersion) {
                    lbVersion.Text = "v" + Properties.Settings.Default.BetaVersion;
                }
                else {

                    lbVersion.Text = "v" + Properties.Settings.Default.CurrentVersion;
                }
            }
        }

        private void llbCheckForUpdates_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if (UpdateCheckThread != null && UpdateCheckThread.IsAlive) {
                UpdateCheckThread.Abort();
            }

            try {
                UpdateCheckThread = new Thread(() => {
                    Updater.CheckForUpdate(true);
                });

                UpdateCheckThread.Start();
            }
            catch (ThreadAbortException) {
                return;
            }

        }

        private void pbIcon_Click(object sender, EventArgs e) { Process.Start("https://github.com/murrty/aphrodite/"); }

        private void About_FormClosing(object sender, FormClosingEventArgs e) {
            this.Dispose();
        }

        private void lnkLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            frmLicensing license = new frmLicensing();
            license.ShowDialog();
        }

    }

}