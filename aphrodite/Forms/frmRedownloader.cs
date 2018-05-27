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

    #region Methods
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
            if (tagLine.StartsWith("MINIMUM SCORE: n/a") || !tagLine.StartsWith("MINIMUM SCORE: "))
                return null;
            else
                return tagLine.Replace("MINIMUM SCORE: ", "");
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
                string tags = getTags(Settings.Default.saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem));
                bool minimumScore = false;
                int minScore = 0;
                if (string.IsNullOrWhiteSpace(tags)) {
                    tags = lbTags.GetItemText(lbTags.SelectedItem);
                }
                if (hasMinimumScore(Settings.Default.saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem))) {
                    minimumScore = true;
                    minScore = Int32.Parse(getMinimumScore(Settings.Default.saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem)));
                }
                if (Tags.Default.imageLimit == 0) {
                    if (MessageBox.Show("Redownloading tags \"" + tags + "\"\nDownloading won't be limited. This may take a long while or even blacklist you. Continue anyway?", "aphrodite", MessageBoxButtons.YesNo) == DialogResult.No) {
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

                frmTagDownloader tagDL = new frmTagDownloader();
                tagDL.tags = tags;
                tagDL.graylist = Settings.Default.blacklist;
                tagDL.blacklist = Settings.Default.zeroToleranceBlacklist;
                tagDL.saveTo = Settings.Default.saveLocation;
                tagDL.saveInfo = Settings.Default.saveInfo;
                tagDL.openAfter = false;
                tagDL.saveBlacklistedFiles = Settings.Default.saveBlacklisted;
                tagDL.ignoreFinish = Settings.Default.ignoreFinish;
                tagDL.useMinimumScore = minimumScore;
                if (tagDL.useMinimumScore) {
                    tagDL.minimumScore = minScore;
                    tagDL.scoreAsTag = Tags.Default.scoreAsTag;
                }
                if (Tags.Default.imageLimit > 0)
                    tagDL.imageLimit = Tags.Default.imageLimit;
                tagDL.usePageLimit = Tags.Default.usePageLimit;
                if (tagDL.usePageLimit)
                    tagDL.pageLimit = Tags.Default.pageLimit;
                tagDL.separateRatings = Tags.Default.separateRatings;
                if (tagDL.separateRatings) {
                    string ratings = string.Empty;
                    if (Tags.Default.Explicit)
                        ratings += "e ";
                    if (Tags.Default.Questionable)
                        ratings += "q ";
                    if (Tags.Default.Safe)
                        ratings += "s ";
                    ratings.TrimEnd(' ');
                    tagDL.ratings = ratings.Split(' ');
                }
                tagDL.webHeader = Program.UserAgent;
                tagDL.Show();
            }
            else if (tcMain.SelectedIndex == 1) {
                // pools
                int poolid = getPoolID(Settings.Default.saveLocation + "\\Pools\\" + lbPools.GetItemText(lbPools.SelectedItem));
                if (poolid == -1) {
                    return;
                }

                frmPoolDownloader poolDL = new frmPoolDownloader();
                poolDL.poolID = (poolid).ToString();

                poolDL.header = Program.UserAgent;
                poolDL.saveTo = Settings.Default.saveLocation;
                poolDL.graylist = Settings.Default.blacklist;
                poolDL.blacklist = Settings.Default.zeroToleranceBlacklist;

                poolDL.saveInfo = Settings.Default.saveInfo;
                poolDL.ignoreFinish = Settings.Default.ignoreFinish;
                poolDL.saveBlacklisted = Settings.Default.saveBlacklisted;

                poolDL.usePoolName = Pools.Default.usePoolName;
                poolDL.mergeBlacklisted = Pools.Default.mergeBlacklisted;
                poolDL.openAfter = Pools.Default.openAfter;

                poolDL.ShowDialog();
            }
        }
        private void lbTags_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lbTags.IndexFromPoint(e.Location) != System.Windows.Forms.ListBox.NoMatches) {
                if (tcMain.SelectedIndex == 0) {
                    // tags
                    string tags = getTags(Settings.Default.saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem));
                    bool minimumScore = false;
                    int minScore = 0;
                    if (string.IsNullOrWhiteSpace(tags)) {
                        tags = lbTags.GetItemText(lbTags.SelectedItem);
                    }
                    if (hasMinimumScore(Settings.Default.saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem))) {
                        minimumScore = true;
                        minScore = Int32.Parse(getMinimumScore(Settings.Default.saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem)));
                    }
                    if (Tags.Default.imageLimit == 0) {
                        if (MessageBox.Show("Redownloading tags \"" + tags + "\"\nDownloading won't be limited. This may take a long while or even blacklist you. Continue anyway?", "aphrodite", MessageBoxButtons.YesNo) == DialogResult.No) {
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

                    frmTagDownloader tagDL = new frmTagDownloader();
                    tagDL.tags = tags;
                    tagDL.graylist = Settings.Default.blacklist;
                    tagDL.blacklist = Settings.Default.zeroToleranceBlacklist;
                    tagDL.saveTo = Settings.Default.saveLocation;
                    tagDL.saveInfo = Settings.Default.saveInfo;
                    tagDL.openAfter = false;
                    tagDL.saveBlacklistedFiles = Settings.Default.saveBlacklisted;
                    tagDL.ignoreFinish = Settings.Default.ignoreFinish;
                    tagDL.useMinimumScore = minimumScore;
                    if (tagDL.useMinimumScore) {
                        tagDL.minimumScore = minScore;
                        tagDL.scoreAsTag = Tags.Default.scoreAsTag;
                    }
                    if (Tags.Default.imageLimit > 0)
                        tagDL.imageLimit = Tags.Default.imageLimit;
                    tagDL.usePageLimit = Tags.Default.usePageLimit;
                    if (tagDL.usePageLimit)
                        tagDL.pageLimit = Tags.Default.pageLimit;
                    tagDL.separateRatings = Tags.Default.separateRatings;
                    if (tagDL.separateRatings) {
                        string ratings = string.Empty;
                        if (Tags.Default.Explicit)
                            ratings += "e ";
                        if (Tags.Default.Questionable)
                            ratings += "q ";
                        if (Tags.Default.Safe)
                            ratings += "s ";
                        ratings.TrimEnd(' ');
                        tagDL.ratings = ratings.Split(' ');
                    }
                    tagDL.webHeader = Program.UserAgent;
                    tagDL.Show();
                }
                else if (tcMain.SelectedIndex == 1) {
                    // pools
                    int poolid = getPoolID(Settings.Default.saveLocation + "\\Pools\\" + lbPools.GetItemText(lbPools.SelectedItem));
                    if (poolid == -1) {
                        return;
                    }

                    frmPoolDownloader poolDL = new frmPoolDownloader();
                    poolDL.poolID = (poolid).ToString();

                    poolDL.header = Program.UserAgent;
                    poolDL.saveTo = Settings.Default.saveLocation;
                    poolDL.graylist = Settings.Default.blacklist;
                    poolDL.blacklist = Settings.Default.zeroToleranceBlacklist;

                    poolDL.saveInfo = Settings.Default.saveInfo;
                    poolDL.ignoreFinish = Settings.Default.ignoreFinish;
                    poolDL.saveBlacklisted = Settings.Default.saveBlacklisted;

                    poolDL.usePoolName = Pools.Default.usePoolName;
                    poolDL.mergeBlacklisted = Pools.Default.mergeBlacklisted;
                    poolDL.openAfter = Pools.Default.openAfter;

                    poolDL.ShowDialog();
                }
            }
        }

        private void btnRenumerate_Click(object sender, EventArgs e) {
            loadDownloads();
        }
    #endregion

    }
}
