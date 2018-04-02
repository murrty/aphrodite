using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmRedownloader : Form {
        public frmRedownloader() {
            InitializeComponent();
        }

        private void loadDownloads() {
            lbTags.Items.Clear();
            lbPools.Items.Clear();

            string[] Tags = Directory.GetDirectories(Settings.Default.saveLocation + "\\Tags\\");
            string[] Pools = Directory.GetDirectories(Settings.Default.saveLocation + "\\Pools\\");

            for (int i = 0; i < Tags.Length; i++) {
                lbTags.Items.Add(Tags[i].Replace(Settings.Default.saveLocation + "\\Tags\\", string.Empty));
            }
            for (int i = 0; i < Pools.Length; i++) {
                lbPools.Items.Add(Pools[i].Replace(Settings.Default.saveLocation + "\\Pools\\", string.Empty));
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

        private void frmTagRedownloader_Load(object sender, EventArgs e) {
            loadDownloads();
        }

        private void btnRedownload_Click(object sender, EventArgs e) {
            if (tcMain.SelectedIndex == 0) {
                // tags
                string tags = getTags(Settings.Default.saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem));
                if (string.IsNullOrWhiteSpace(tags)) {
                    tags = lbTags.GetItemText(lbTags.SelectedItem);
                }
                if (Tags.Default.imageLimit == 0) {
                    if (MessageBox.Show("Downloading won't be limited. This may take a long while or even blacklist you. Continue anyway?", "TagDownloader", MessageBoxButtons.YesNo) == DialogResult.No) {
                        return;
                    }
                }

                if (tags.StartsWith(" ")) {
                    tags = tags.TrimStart(' ');
                }
                if (tags.EndsWith(" ")) {
                    tags = tags.TrimEnd(' ');
                }

                if (tags.Contains("/")) {
                    tags = tags.Replace("/", "%25-2F");
                }

                string[] length = tags.Split(' ');
                if (length.Length > 6) {
                    MessageBox.Show("6 tags is the maximum length you're allowed to download from e621. If your tag has a space between words, be sure to add an underscore. (_)");
                    return;
                }

                string ratings = string.Empty;
                if (Tags.Default.Explicit)
                    ratings += "e ";
                if (Tags.Default.Questionable)
                    ratings += "q ";
                if (Tags.Default.Safe)
                    ratings += "s ";
                if (string.IsNullOrWhiteSpace(ratings)) {
                    MessageBox.Show("Please specify the ratings you want for images in the settings");
                    return;
                }
                if (ratings.EndsWith(" "))
                    ratings = ratings.TrimEnd(' ');

                frmTagDownloader tagDL = new frmTagDownloader();
                tagDL.tags = tags;
                tagDL.openAfter = false;
                tagDL.fromURL = false;
                if (Tags.Default.enableScoreMin) {
                    tagDL.useMinimumScore = true;
                    if (Tags.Default.scoreMin > 0) {
                        tagDL.minimumScore = Tags.Default.scoreMin;
                    }
                    else {
                        tagDL.minimumScore = 0;
                    }
                }
                tagDL.imageAmount = Convert.ToInt32(Tags.Default.imageLimit);
                tagDL.saveInfo = Settings.Default.saveInfo;
                tagDL.blacklistedTags = Settings.Default.blacklist;
                tagDL.zblacklistedTags = Settings.Default.zeroToleranceBlacklist;
                tagDL.ratings = ratings.Split(' ');
                tagDL.Show();
            }
            else if (tcMain.SelectedIndex == 1) {
                // pools
                int poolid = getPoolID(Settings.Default.saveLocation + "\\Pools\\" + lbPools.GetItemText(lbPools.SelectedItem));
                if (poolid == -1) {
                    return;
                }

                frmPoolDownloader poolDL = new frmPoolDownloader();
                poolDL.id = (poolid).ToString();
                poolDL.openAfter = Pools.Default.openAfter;
                poolDL.Show();
            }
        }

        private void btnRenumerate_Click(object sender, EventArgs e) {
            loadDownloads();
        }
    }
}
