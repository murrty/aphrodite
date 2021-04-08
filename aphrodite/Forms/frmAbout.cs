using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmAbout : Form {
        List<Thread> thr = new List<Thread>();

        public frmAbout() {
            InitializeComponent();
            pbIcon.Cursor = NativeMethods.SystemHandCursor;
            llbCheckForUpdates.Cursor = NativeMethods.SystemHandCursor;
            lnkLicense.Cursor = NativeMethods.SystemHandCursor;
            lbBody.Text += "\r\n\r\n(last debug date " + Properties.Settings.Default.debugDate + ")";
            if (Program.IsDebug) {
                this.Text = "About aphrodite (debug)";
            }
            else {
                if (Properties.Settings.Default.IsBetaVersion) {
                    lbVersion.Text = "v" + Properties.Settings.Default.BetaVersion;
                }
                else {

                    lbVersion.Text = "v" + Properties.Settings.Default.currentVersion;
                }
            }
        }

        private void llbCheckForUpdates_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Thread checkUpdates = new Thread(() => {
                decimal cV = Updater.getCloudVersion();

                if (Updater.isUpdateAvailable(cV)) {
                    if (MessageBox.Show("An update is available. \nNew verison: " + cV.ToString() + " | Your version: " + Properties.Settings.Default.currentVersion.ToString() + "\n\nWould you like to update?", "aphrodite", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes) {
                        Process.Start(Updater.githubURL + "/releases/lates");
                    }
                }
                else {
                    MessageBox.Show("No update is available at this time.");
                }
                foreach (Thread thd in thr) {
                    thd.Abort();
                }
            });

            checkUpdates.Start();
            thr.Add(checkUpdates);
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