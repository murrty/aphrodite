using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmAbout : Form {
        Thread UpdateCheckThread;

        public frmAbout() {
            InitializeComponent();
            pbIcon.Cursor = NativeMethods.SystemHandCursor;
            lbBody.Text += $"\r\n\r\n(last debug date {Properties.Resources.BuildDate})";
            if (Program.IsDebug) {
                this.Text = "About aphrodite (debug)";
            }
            lbVersion.Text = $"v{(Program.IsBetaVersion ? Program.BetaVersion : Program.CurrentVersion)}";
        }

        private void llbCheckForUpdates_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if (UpdateCheckThread == null || !UpdateCheckThread.IsAlive) {
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
        }

        private void pbIcon_Click(object sender, EventArgs e) =>
            Process.Start("https://github.com/murrty/aphrodite/");

        private void About_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            this.Hide();
        }
    }

}