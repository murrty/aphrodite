using System;
using System.IO;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmRedownloader : Form {
        string saveLocation = Environment.CurrentDirectory;

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
                nfoLocation = nfoLocation + "\\pool.nfo";
            }

            string poolLine;
            using (StreamReader sr = new StreamReader(nfoLocation)) {
                poolLine = sr.ReadLine();
            }

            if (!poolLine.StartsWith("POOL: "))
                return -1;
            else
                poolLine = poolLine.Replace("POOL: ", "");

            int poolID = -1;
            Int32.TryParse(poolLine, out poolID);
            return poolID;
        }
        private string getPoolDownloadOn(string nfoLocation) {
            if (!nfoLocation.EndsWith("pool.nfo")) {
                nfoLocation = nfoLocation + "\\pool.nfo";

                if (!File.Exists(nfoLocation))
                    return "n/a";
            }
            string downloadedOnLine;
            using (StreamReader sr = new StreamReader(nfoLocation)) {
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
                    nfoLocation = nfoLocation + "\\tags.nfo";
                else if (File.Exists(nfoLocation + "\\tags.blacklisted.nfo"))
                    nfoLocation = nfoLocation + "\\tags.blacklisted.nfo";
                else
                    return null;
            }
            string tagLine;
            using (StreamReader sr = new StreamReader(nfoLocation)) {
                tagLine = sr.ReadLine();
            }
            if (!tagLine.StartsWith("TAGS: "))
                return null;
            else
                return tagLine.Replace("TAGS: ", "");
        }
        private bool hasMinimumScore(string nfoLocation) {
            if (!nfoLocation.EndsWith("tags.nfo") && !nfoLocation.EndsWith("tags.blacklisted.nfo")) {
                if (File.Exists(nfoLocation + "\\tags.nfo"))
                    nfoLocation = nfoLocation + "\\tags.nfo";
                else if (File.Exists(nfoLocation + "\\tags.blacklisted.nfo"))
                    nfoLocation = nfoLocation + "\\tags.blacklisted.nfo";
                else
                    return false;
            }
            string tagLine;
            using (StreamReader sr = new StreamReader(nfoLocation)) {
                sr.ReadLine();
                tagLine = sr.ReadLine();
            }
            if (tagLine.StartsWith("MINIMUM SCORE: n/a") || !tagLine.StartsWith("MINIMUM SCORE: ")) 
                return false;
            else 
                return true;
        }
        private string getMinimumScore(string nfoLocation) {
            if (!nfoLocation.EndsWith("tags.nfo") && !nfoLocation.EndsWith("tags.blacklisted.nfo")) {
                if (File.Exists(nfoLocation + "\\tags.nfo"))
                    nfoLocation = nfoLocation + "\\tags.nfo";
                else if (File.Exists(nfoLocation + "\\tags.blacklisted.nfo"))
                    nfoLocation = nfoLocation + "\\tags.blacklisted.nfo";
                else
                    return null;
            }
            string tagLine;
            using (StreamReader sr = new StreamReader(nfoLocation)) {
                sr.ReadLine();
                tagLine = sr.ReadLine();
            }

            if (tagLine.StartsWith("MINIMUM SCORE: ") || !tagLine.StartsWith("MINMIUM SCORE: n/a"))
                return tagLine.Replace("MINIMUM SCORE: ", "");
            else
                return null;
        }
        private string getTagDownloadOn(string nfoLocation) {
            if (!nfoLocation.EndsWith("tags.nfo") && !nfoLocation.EndsWith("tags.blacklisted.nfo")) {
                if (File.Exists(nfoLocation + "\\tags.nfo"))
                    nfoLocation = nfoLocation + "\\tags.nfo";
                else if (File.Exists(nfoLocation + "\\tags.blacklisted.nfo"))
                    nfoLocation = nfoLocation + "\\tags.blacklisted.nfo";
                else
                    return "n/a";
            }
            string downloadedOnLine;
            using (StreamReader sr = new StreamReader(nfoLocation)) {
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
        }
        
        private void frmTagRedownloader_Load(object sender, EventArgs e) {
            loadDownloads();
        }

        private void btnRedownload_Click(object sender, EventArgs e) {
            if (tcMain.SelectedIndex == 0) {
                // tags
                Downloader.Arguments.DownloadTags(getTags(saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem)));
            }
            else if (tcMain.SelectedIndex == 1) {
                // pools
                int poolid = getPoolID(saveLocation + "\\Pools\\" + lbPools.GetItemText(lbPools.SelectedItem));
                if (poolid == -1) {
                    return;
                }

                Downloader.Arguments.DownloadPool(poolid.ToString());
            }
        }
        private void lbTags_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lbTags.IndexFromPoint(e.Location) != System.Windows.Forms.ListBox.NoMatches) {
                Downloader.Arguments.DownloadTags(getTags(saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem)));
            }
        }
        private void lbTags_SelectedIndexChanged(object sender, EventArgs e) {
            if (lbTags.SelectedIndex == -1) {
                return;
            }
            lbDownloadedOn.Text = "Selected tag: " + lbTags.GetItemText(lbTags.SelectedItem) + "\nDownloaded on: " + getTagDownloadOn(saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem));
        }
        private void lbPools_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lbTags.IndexFromPoint(e.Location) != System.Windows.Forms.ListBox.NoMatches) {
                int poolid = getPoolID(saveLocation + "\\Pools\\" + lbPools.GetItemText(lbPools.SelectedItem));
                if (poolid == -1) {
                    return;
                }

                Downloader.Arguments.DownloadPool(poolid.ToString());
            }
        }
        private void lbPools_SelectedIndexChanged(object sender, EventArgs e) {
            if (lbPools.SelectedIndex == -1) {
                return;
            }
            lbDownloadedOn.Text = "Selected pool: " + lbPools.GetItemText(lbPools.SelectedItem) + "\nDownloaded on: " + getPoolDownloadOn(saveLocation + "\\Pools\\" + lbPools.GetItemText(lbPools.SelectedItem));
        }

        private void btnRenumerate_Click(object sender, EventArgs e) {
            loadDownloads();
        }
    #endregion

    }
}
