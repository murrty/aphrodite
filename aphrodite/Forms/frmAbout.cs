using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmAbout : Form {
        List<Thread> thr = new List<Thread>();

        public frmAbout() { InitializeComponent(); this.Icon = Properties.Resources.Brad; }

        private void About_Shown(object sender, EventArgs e) {
            lbVersion.Text = "v" + Properties.Settings.Default.currentVersion.ToString();
            if (Properties.Settings.Default.showDebugDates) {
                lbVersion.Text += " (" + Properties.Settings.Default.debugDate + ")";
                this.Text = "About aphrodite (debug)";
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

    }
}