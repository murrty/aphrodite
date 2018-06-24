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
        public bool useIni = false;
        IniFile ini = new IniFile();

    #region Methods
        private void loadDownloads() {
            lbTags.Items.Clear();
            lbPools.Items.Clear();

            if (useIni) {
                string[] Tags = Directory.GetDirectories(Environment.CurrentDirectory + "\\Tags");
                string[] Pools = Directory.GetDirectories(Environment.CurrentDirectory + "\\Pools");

                if (Tags.Length > 0) {
                    for (int i = 0; i < Tags.Length; i++) {
                        lbTags.Items.Add(Tags[i].Replace(Environment.CurrentDirectory + "\\Tags\\", string.Empty));
                    }
                }
                if (Pools.Length > 0) {
                    for (int i = 0; i < Pools.Length; i++) {
                        lbPools.Items.Add(Pools[i].Replace(Environment.CurrentDirectory + "\\Pools\\", string.Empty));
                    }
                }
            }
            else {
                string[] Tags = new string[0];
                string[] Pools = new string[0];

                if (Directory.Exists(Settings.Default.saveLocation + "\\Tags")) {
                    Tags = Directory.GetDirectories(Settings.Default.saveLocation + "\\Tags");
                }
                if (Directory.Exists(Settings.Default.saveLocation + "\\Pools")) {
                    Pools = Directory.GetDirectories(Settings.Default.saveLocation + "\\Pools");
                }

                if (Tags.Length > 0) {
                    for (int i = 0; i < Tags.Length; i++) {
                        lbTags.Items.Add(Tags[i].Replace(Settings.Default.saveLocation + "\\Tags\\", string.Empty));
                    }
                }
                if (Pools.Length > 0) {
                    for (int i = 0; i < Pools.Length; i++) {
                        lbPools.Items.Add(Pools[i].Replace(Settings.Default.saveLocation + "\\Pools\\", string.Empty));
                    }
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
                tagDL.useMinimumScore = minimumScore;
                if (tagDL.useMinimumScore) {
                    tagDL.minimumScore = minScore;
                    if (useIni)
                        tagDL.scoreAsTag = ini.ReadBool("scoreAsTag", "Tags");
                    else
                        tagDL.scoreAsTag = Tags.Default.scoreAsTag;
                }
                tagDL.openAfter = false;
                if (useIni) {
                    tagDL.graylist = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg");
                    tagDL.blacklist = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg");
                    tagDL.saveTo = Environment.CurrentDirectory;
                    tagDL.saveInfo = ini.ReadBool("saveInfo", "Global");
                    tagDL.saveBlacklistedFiles = ini.ReadBool("saveBlacklisted", "Global");
                    tagDL.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");
                    if (ini.ReadInt("imageLimit", "Tags") > 0)
                        tagDL.imageLimit = ini.ReadInt("imageLimit", "Tags");
                    tagDL.usePageLimit = ini.ReadBool("usePageLimit", "Tags");
                    if (tagDL.usePageLimit)
                        tagDL.pageLimit = ini.ReadInt("pageLimit", "Tags");
                    tagDL.separateRatings = ini.ReadBool("separateRatings", "Tags");
                    if (tagDL.separateRatings) {
                        string ratings = string.Empty;
                        if (ini.ReadBool("Explicit", "Tags"))
                            ratings += "e ";
                        if (ini.ReadBool("Questionable", "Tags"))
                            ratings += "q ";
                        if (ini.ReadBool("Safe", "Tags"))
                            ratings += "s";
                        ratings = ratings.TrimEnd(' ');
                        tagDL.ratings = ratings.Split(' ');
                    }
                }
                else {
                    Settings.Default.Reload();
                    Tags.Default.Reload();
                    tagDL.graylist = Settings.Default.blacklist;
                    tagDL.blacklist = Settings.Default.zeroToleranceBlacklist;
                    tagDL.saveTo = Settings.Default.saveLocation;
                    tagDL.saveInfo = Settings.Default.saveInfo;
                    tagDL.saveBlacklistedFiles = Settings.Default.saveBlacklisted;
                    tagDL.ignoreFinish = Settings.Default.ignoreFinish;
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
                        ratings = ratings.TrimEnd(' ');
                        tagDL.ratings = ratings.Split(' ');
                    }
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
                if (useIni) {
                    poolDL.saveTo = Environment.CurrentDirectory;
                    poolDL.graylist = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg");
                    poolDL.blacklist = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg");

                    poolDL.saveInfo = ini.ReadBool("saveInfo", "Global");
                    poolDL.saveBlacklisted = ini.ReadBool("saveBlacklisted", "Global");
                    poolDL.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");

                    poolDL.usePoolName = ini.ReadBool("usePoolName", "Pools");
                    poolDL.mergeBlacklisted = ini.ReadBool("mergeBlacklisted", "Pools");
                    poolDL.openAfter = ini.ReadBool("openAfter", "Pools");
                }
                else {
                    Settings.Default.Reload();
                    Pools.Default.Reload();
                    poolDL.saveTo = Settings.Default.saveLocation;
                    poolDL.graylist = Settings.Default.blacklist;
                    poolDL.blacklist = Settings.Default.zeroToleranceBlacklist;

                    poolDL.saveInfo = Settings.Default.saveInfo;
                    poolDL.ignoreFinish = Settings.Default.ignoreFinish;
                    poolDL.saveBlacklisted = Settings.Default.saveBlacklisted;

                    poolDL.usePoolName = Pools.Default.usePoolName;
                    poolDL.mergeBlacklisted = Pools.Default.mergeBlacklisted;
                    poolDL.openAfter = Pools.Default.openAfter;
                }

                poolDL.ShowDialog();
            }
        }
        private void lbTags_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lbTags.IndexFromPoint(e.Location) != System.Windows.Forms.ListBox.NoMatches) {
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
                tagDL.useMinimumScore = minimumScore;
                if (tagDL.useMinimumScore) {
                    tagDL.minimumScore = minScore;
                    if (useIni)
                        tagDL.scoreAsTag = ini.ReadBool("scoreAsTag", "Tags");
                    else
                        tagDL.scoreAsTag = Tags.Default.scoreAsTag;
                }
                tagDL.openAfter = false;
                if (useIni) {
                    tagDL.graylist = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg");
                    tagDL.blacklist = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg");
                    tagDL.saveTo = Environment.CurrentDirectory;
                    tagDL.saveInfo = ini.ReadBool("saveInfo", "Global");
                    tagDL.saveBlacklistedFiles = ini.ReadBool("saveBlacklisted", "Global");
                    tagDL.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");
                    if (ini.ReadInt("imageLimit", "Tags") > 0)
                        tagDL.imageLimit = ini.ReadInt("imageLimit", "Tags");
                    tagDL.usePageLimit = ini.ReadBool("usePageLimit", "Tags");
                    if (tagDL.usePageLimit)
                        tagDL.pageLimit = ini.ReadInt("pageLimit", "Tags");
                    tagDL.separateRatings = ini.ReadBool("separateRatings", "Tags");
                    if (tagDL.separateRatings) {
                        string ratings = string.Empty;
                        if (ini.ReadBool("Explicit", "Tags"))
                            ratings += "e ";
                        if (ini.ReadBool("Questionable", "Tags"))
                            ratings += "q ";
                        if (ini.ReadBool("Safe", "Tags"))
                            ratings += "s";
                        ratings = ratings.TrimEnd(' ');
                        tagDL.ratings = ratings.Split(' ');
                    }
                }
                else {
                    Settings.Default.Reload();
                    Tags.Default.Reload();
                    tagDL.graylist = Settings.Default.blacklist;
                    tagDL.blacklist = Settings.Default.zeroToleranceBlacklist;
                    tagDL.saveTo = Settings.Default.saveLocation;
                    tagDL.saveInfo = Settings.Default.saveInfo;
                    tagDL.saveBlacklistedFiles = Settings.Default.saveBlacklisted;
                    tagDL.ignoreFinish = Settings.Default.ignoreFinish;
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
                        ratings = ratings.TrimEnd(' ');
                        tagDL.ratings = ratings.Split(' ');
                    }
                }
                tagDL.webHeader = Program.UserAgent;
                tagDL.Show();
            }
        }
        private void lbTags_SelectedIndexChanged(object sender, EventArgs e) {
            lbDownloadedOn.Text = "Downloaded on: " + getTagDownloadOn(Settings.Default.saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem));
        }
        private void lbPools_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lbTags.IndexFromPoint(e.Location) != System.Windows.Forms.ListBox.NoMatches) {
                int poolid = getPoolID(Settings.Default.saveLocation + "\\Pools\\" + lbPools.GetItemText(lbPools.SelectedItem));
                if (poolid == -1) {
                    return;
                }

                frmPoolDownloader poolDL = new frmPoolDownloader();
                poolDL.poolID = (poolid).ToString();
                poolDL.header = Program.UserAgent;
                if (useIni) {
                    poolDL.saveTo = Environment.CurrentDirectory;
                    poolDL.graylist = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg");
                    poolDL.blacklist = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg");

                    poolDL.saveInfo = ini.ReadBool("saveInfo", "Global");
                    poolDL.saveBlacklisted = ini.ReadBool("saveBlacklisted", "Global");
                    poolDL.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");

                    poolDL.usePoolName = ini.ReadBool("usePoolName", "Pools");
                    poolDL.mergeBlacklisted = ini.ReadBool("mergeBlacklisted", "Pools");
                    poolDL.openAfter = ini.ReadBool("openAfter", "Pools");
                }
                else {
                    Settings.Default.Reload();
                    Pools.Default.Reload();

                    poolDL.saveTo = Settings.Default.saveLocation;
                    poolDL.graylist = Settings.Default.blacklist;
                    poolDL.blacklist = Settings.Default.zeroToleranceBlacklist;

                    poolDL.saveInfo = Settings.Default.saveInfo;
                    poolDL.ignoreFinish = Settings.Default.ignoreFinish;
                    poolDL.saveBlacklisted = Settings.Default.saveBlacklisted;

                    poolDL.usePoolName = Pools.Default.usePoolName;
                    poolDL.mergeBlacklisted = Pools.Default.mergeBlacklisted;
                    poolDL.openAfter = Pools.Default.openAfter;
                }

                poolDL.ShowDialog();
            }
        }
        private void lbPools_SelectedIndexChanged(object sender, EventArgs e) {
            lbDownloadedOn.Text = "Downloaded on: " + getPoolDownloadOn(Settings.Default.saveLocation + "\\Pools\\" + lbPools.GetItemText(lbPools.SelectedItem));
        }

        private void btnRenumerate_Click(object sender, EventArgs e) {
            loadDownloads();
        }
    #endregion

    }
}
