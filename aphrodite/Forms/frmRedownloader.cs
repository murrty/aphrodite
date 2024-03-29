﻿using System;
using System.IO;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmRedownloader : Form {

    #region Methods
        private void loadDownloads() {
            lbTags.Items.Clear();
            lbPools.Items.Clear();

            string[] Tags = Directory.GetDirectories(Config.Settings.General.saveLocation + "\\Tags");
            string[] Pools = Directory.GetDirectories(Config.Settings.General.saveLocation + "\\Pools");

            if (Tags.Length > 0) {
                for (int i = 0; i < Tags.Length; i++) {
                    lbTags.Items.Add(Tags[i].Replace(Config.Settings.General.saveLocation + "\\Tags\\", string.Empty));
                }
            }
            if (Pools.Length > 0) {
                for (int i = 0; i < Pools.Length; i++) {
                    lbPools.Items.Add(Pools[i].Replace(Config.Settings.General.saveLocation + "\\Pools\\", string.Empty));
                }
            }
        }

        private int getPoolID(string nfoLocation) {
            if (!nfoLocation.EndsWith("pool.nfo")) {
                nfoLocation += "\\pool.nfo";
            }

            string poolLine;
            using (StreamReader sr = new(nfoLocation)) {
                poolLine = sr.ReadLine();
            }

            if (!poolLine.StartsWith("POOL: "))
                return -1;
            else
                poolLine = poolLine.Replace("POOL: ", "");

            Int32.TryParse(poolLine, out int poolID);
            return poolID;
        }

        private string getPoolDownloadOn(string nfoLocation) {
            if (!nfoLocation.EndsWith("pool.nfo")) {
                nfoLocation += "\\pool.nfo";

                if (!File.Exists(nfoLocation))
                    return "n/a";
            }
            string downloadedOnLine;
            using (StreamReader sr = new(nfoLocation)) {
                sr.ReadLine();
                downloadedOnLine = sr.ReadLine();
            }

            if (downloadedOnLine.StartsWith("DOWNLOADED ON: "))
                return downloadedOnLine.Replace("DOWNLOADED ON: ", "");
            else
                return "n/a";
        }

        private string getTags(string nfoLocation) {
            if (!nfoLocation.EndsWith("tags.nfo") && !nfoLocation.EndsWith("tags.blacklisted.nfo")) {
                if (File.Exists(nfoLocation + "\\tags.nfo"))
                    nfoLocation += "\\tags.nfo";
                else if (File.Exists(nfoLocation + "\\tags.blacklisted.nfo"))
                    nfoLocation += "\\tags.blacklisted.nfo";
                else
                    return null;
            }
            string tagLine;
            using (StreamReader sr = new(nfoLocation)) {
                tagLine = sr.ReadLine();
            }
            if (!tagLine.StartsWith("TAGS: "))
                return null;
            else
                return tagLine.Replace("TAGS: ", "");
        }

        private bool hasMinimumScore(string nfoLocation, out string MinimumScore) {
            MinimumScore = null;
            if (!nfoLocation.EndsWith("tags.nfo") && !nfoLocation.EndsWith("tags.blacklisted.nfo")) {
                if (File.Exists(nfoLocation + "\\tags.nfo"))
                    nfoLocation += "\\tags.nfo";
                else if (File.Exists(nfoLocation + "\\tags.blacklisted.nfo"))
                    nfoLocation += "\\tags.blacklisted.nfo";
                else
                    return false;
            }
            string tagLine;
            using (StreamReader sr = new(nfoLocation)) {
                sr.ReadLine();
                tagLine = sr.ReadLine();
            }
            if (tagLine.StartsWith("MINIMUM SCORE: n/a") || !tagLine.StartsWith("MINIMUM SCORE: "))
                return false;
            else {
                MinimumScore = tagLine.Replace("MINIMUM SCORE: ", "");
                return true;
            }
        }

        private string getTagDownloadOn(string nfoLocation) {
            if (!nfoLocation.EndsWith("tags.nfo") && !nfoLocation.EndsWith("tags.blacklisted.nfo")) {
                if (File.Exists(nfoLocation + "\\tags.nfo"))
                    nfoLocation += "\\tags.nfo";
                else if (File.Exists(nfoLocation + "\\tags.blacklisted.nfo"))
                    nfoLocation += "\\tags.blacklisted.nfo";
                else
                    return "n/a";
            }
            string downloadedOnLine;
            using (StreamReader sr = new(nfoLocation)) {
                sr.ReadLine();
                sr.ReadLine();
                downloadedOnLine = sr.ReadLine();
            }

            if (downloadedOnLine.StartsWith("DOWNLOADED ON: "))
                return downloadedOnLine.Replace("DOWNLOADED ON: ", "");
            else
                return "n/a";
        }
    #endregion

    #region Form
        public frmRedownloader() {
            InitializeComponent();
            if (Config.ValidPoint(Config.Settings.FormSettings.frmRedownloader_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmRedownloader_Location;
            }
        }
        
        private void frmTagRedownloader_Load(object sender, EventArgs e) {
            loadDownloads();
        }
        private void frmRedownloader_FormClosing(object sender, FormClosingEventArgs e) {
            Config.Settings.FormSettings.frmRedownloader_Location = this.Location;
        }

        private void btnRedownload_Click(object sender, EventArgs e) {
            switch (tcMain.SelectedIndex) {
                case 0: {
                    Downloader.DownloadTags(getTags(Config.Settings.General.saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem)), false);
                } break;

                case 1: {
                    int poolid = getPoolID(Config.Settings.General.saveLocation + "\\Pools\\" + lbPools.GetItemText(lbPools.SelectedItem));
                    if (poolid != -1) {
                        Downloader.DownloadPool(poolid.ToString(), false);
                    }
                } break;
            }
        }
        private void lbTags_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lbTags.IndexFromPoint(e.Location) != ListBox.NoMatches) {
                Downloader.DownloadTags(getTags(Config.Settings.General.saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem)), false);
            }
        }
        private void lbTags_SelectedIndexChanged(object sender, EventArgs e) {
            if (lbTags.SelectedIndex > -1) {
                lbDownloadedOn.Text = "Selected tag: " + lbTags.GetItemText(lbTags.SelectedItem) + "\r\nDownloaded on: " + getTagDownloadOn(Config.Settings.General.saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem));
            }
        }
        private void lbPools_MouseDoubleClick(object sender, MouseEventArgs e) {
            //if (lbTags.IndexFromPoint(e.Location) != ListBox.NoMatches) {
            if (lbTags.SelectedIndex > -1) {
                int poolid = getPoolID(Config.Settings.General.saveLocation + "\\Pools\\" + lbPools.GetItemText(lbPools.SelectedItem));
                if (poolid == -1) {
                    return;
                }

                Downloader.DownloadPool(poolid.ToString(), false);
            }
        }
        private void lbPools_SelectedIndexChanged(object sender, EventArgs e) {
            if (lbPools.SelectedIndex > -1) {
                lbDownloadedOn.Text = "Selected pool: " + lbPools.GetItemText(lbPools.SelectedItem) + "\r\nDownloaded on: " + getPoolDownloadOn(Config.Settings.General.saveLocation + "\\Pools\\" + lbPools.GetItemText(lbPools.SelectedItem));
            }
        }

        private void btnRenumerate_Click(object sender, EventArgs e) {
            loadDownloads();
        }
    #endregion

    }
}