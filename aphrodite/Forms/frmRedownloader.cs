using System;
using System.IO;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmRedownloader : Form {
        public bool useIni = false;
        IniFile ini = new IniFile();
        string saveLocation = Environment.CurrentDirectory;

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

                if (Directory.Exists(General.Default.saveLocation + "\\Tags")) {
                    Tags = Directory.GetDirectories(General.Default.saveLocation + "\\Tags");
                }
                if (Directory.Exists(General.Default.saveLocation + "\\Pools")) {
                    Pools = Directory.GetDirectories(General.Default.saveLocation + "\\Pools");
                }

                if (Tags.Length > 0) {
                    for (int i = 0; i < Tags.Length; i++) {
                        lbTags.Items.Add(Tags[i].Replace(General.Default.saveLocation + "\\Tags\\", string.Empty));
                    }
                }
                if (Pools.Length > 0) {
                    for (int i = 0; i < Pools.Length; i++) {
                        lbPools.Items.Add(Pools[i].Replace(General.Default.saveLocation + "\\Pools\\", string.Empty));
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
            this.Icon = Properties.Resources.Brad;
        }

        private void frmTagRedownloader_Load(object sender, EventArgs e) {
            loadDownloads();
            if (!useIni)
                saveLocation = General.Default.saveLocation;
        }

        private void btnRedownload_Click(object sender, EventArgs e) {
            if (tcMain.SelectedIndex == 0) {
                // tags
                string tags = getTags(saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem));
                bool minimumScore = false;
                int minScore = 0;
                if (string.IsNullOrWhiteSpace(tags)) {
                    tags = lbTags.GetItemText(lbTags.SelectedItem);
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
                if (hasMinimumScore(saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem))) {
                    minimumScore = true;
                    minScore = Int32.Parse(getMinimumScore(saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem)));
                }
                if (Tags.Default.imageLimit == 0) {
                    if (MessageBox.Show("Redownloading tags \"" + tags + "\"\nDownloading won't be limited. This may take a long while or even blacklist you. Continue anyway?", "aphrodite", MessageBoxButtons.YesNo) == DialogResult.No) {
                        return;
                    }
                }

                string[] length = tags.Split(' ');
                if (length.Length > 6) {
                    MessageBox.Show("6 tags is the maximum length you're allowed to download from e621. If your tag has a space between words, be sure to add an underscore. (_)");
                    return;
                }

                try {
                    frmTagDownloader tagDL = new frmTagDownloader();
                    tagDL.webHeader = Program.UserAgent;
                    tagDL.tags = tags;
                    string ratings = string.Empty;

                    if (useIni) {
                        tagDL.saveTo = Environment.CurrentDirectory;

                        if (File.Exists(File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg")))
                            tagDL.graylist = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg");
                        else
                            tagDL.graylist = string.Empty;

                        if (File.Exists(Environment.CurrentDirectory + "\\blacklist.cfg"))
                            tagDL.blacklist = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg");
                        else
                            tagDL.blacklist = string.Empty;


                        if (ini.KeyExists("saveInfo", "Global"))
                            tagDL.saveInfo = ini.ReadBool("saveInfo", "Global");
                        else
                            tagDL.saveInfo = true;

                        if (ini.KeyExists("openAfter", "Global"))
                            tagDL.openAfter = false; //ini.ReadBool("openAfter", "Global");
                        else
                            tagDL.openAfter = false;

                        if (ini.KeyExists("saveBlacklisted", "Global"))
                            tagDL.saveBlacklistedFiles = ini.ReadBool("saveBlacklisted", "Global");
                        else
                            tagDL.saveBlacklistedFiles = true;

                        if (ini.KeyExists("ignoreFinish", "Global"))
                            tagDL.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");
                        else
                            tagDL.ignoreFinish = false;

                        if (minimumScore) {
                            tagDL.useMinimumScore = minimumScore;
                            tagDL.scoreAsTag = ini.ReadBool("scoreAsTag", "Tags");
                            tagDL.minimumScore = minScore;
                        }

                        if (ini.KeyExists("imageLimit", "Tags"))
                            if (ini.ReadInt("imageLimit", "Tags") > 0)
                                tagDL.imageLimit = ini.ReadInt("imageLimit", "Tags");
                            else
                                tagDL.imageLimit = 0;

                        if (ini.KeyExists("pageLimit", "Tags"))
                            if (ini.ReadInt("pageLimit", "Tags") > 0)
                                tagDL.pageLimit = ini.ReadInt("pageLimit", "Tags");

                        if (ini.KeyExists("separateRatings", "Tags"))
                            tagDL.separateRatings = ini.ReadBool("separateRatings", "Tags");
                        else
                            tagDL.separateRatings = true;

                        if (ini.KeyExists("Explicit", "Tags"))
                            if (ini.ReadBool("Explicit", "Tags"))
                                ratings += "e ";
                        if (ini.KeyExists("Questionable", "Tags"))
                            if (ini.ReadBool("Questionable", "Tags"))
                                ratings += "q ";
                        if (ini.KeyExists("Safe", "Tags"))
                            if (ini.ReadBool("Safe", "Tags"))
                                ratings += "s";
                        ratings = ratings.TrimEnd(' ');

                        if (tagDL.separateRatings)
                            tagDL.ratings = ratings.Split(' ');

                        if (ini.KeyExists("fileNameSchema", "Tags"))
                            tagDL.fileNameSchema = apiTools.ReplaceIllegalCharacters(ini.ReadString("fileNameSchema", "Tags").ToLower());
                        else
                            tagDL.fileNameSchema = "%md5%";
                    }
                    else {
                        General.Default.Reload();
                        Tags.Default.Reload();

                        tagDL.saveTo = General.Default.saveLocation;
                        tagDL.graylist = General.Default.blacklist;
                        tagDL.blacklist = General.Default.zeroToleranceBlacklist;

                        tagDL.saveInfo = General.Default.saveInfo;
                        tagDL.openAfter = false;
                        tagDL.saveBlacklistedFiles = General.Default.saveBlacklisted;
                        tagDL.ignoreFinish = General.Default.ignoreFinish;

                        tagDL.useMinimumScore = minimumScore;
                        if (minimumScore) {
                            tagDL.scoreAsTag = Tags.Default.scoreAsTag;
                            tagDL.minimumScore = minScore;
                        }

                        if (Tags.Default.imageLimit > 0)
                            tagDL.imageLimit = Tags.Default.imageLimit;

                        if (tagDL.pageLimit > 0)
                            tagDL.pageLimit = Tags.Default.pageLimit;

                        tagDL.separateRatings = Tags.Default.separateRatings;

                        if (Tags.Default.Explicit)
                            ratings += "e ";
                        if (Tags.Default.Questionable)
                            ratings += "q ";
                        if (Tags.Default.Safe)
                            ratings += "s";
                        ratings = ratings.TrimEnd(' ');
                        tagDL.ratings = ratings.Split(' ');

                        tagDL.fileNameSchema = apiTools.ReplaceIllegalCharacters(Tags.Default.fileNameSchema.ToLower());
                    }

                    tagDL.ShowDialog();

                }
                catch {
                    return;
                }
            }
            else if (tcMain.SelectedIndex == 1) {
                // pools
                int poolid = getPoolID(saveLocation + "\\Pools\\" + lbPools.GetItemText(lbPools.SelectedItem));
                if (poolid == -1) {
                    return;
                }

                Downloader.Arguments.downloadPool(poolid.ToString(), useIni);
            }
        }
        private void lbTags_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lbTags.IndexFromPoint(e.Location) != System.Windows.Forms.ListBox.NoMatches) {
                // tags
                string tags = getTags(saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem));
                bool minimumScore = false;
                int minScore = 0;
                if (string.IsNullOrWhiteSpace(tags)) {
                    tags = lbTags.GetItemText(lbTags.SelectedItem);
                }
                if (hasMinimumScore(saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem))) {
                    minimumScore = true;
                    minScore = Int32.Parse(getMinimumScore(saveLocation + "\\Tags\\" + lbTags.GetItemText(lbTags.SelectedItem)));
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

                try {
                    frmTagDownloader tagDL = new frmTagDownloader();
                    tagDL.webHeader = Program.UserAgent;
                    tagDL.tags = tags;
                    string ratings = string.Empty;

                    if (useIni) {
                        tagDL.saveTo = Environment.CurrentDirectory;

                        if (File.Exists(File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg")))
                            tagDL.graylist = File.ReadAllText(Environment.CurrentDirectory + "\\graylist.cfg");
                        else
                            tagDL.graylist = string.Empty;

                        if (File.Exists(Environment.CurrentDirectory + "\\blacklist.cfg"))
                            tagDL.blacklist = File.ReadAllText(Environment.CurrentDirectory + "\\blacklist.cfg");
                        else
                            tagDL.blacklist = string.Empty;


                        if (ini.KeyExists("saveInfo", "Global"))
                            tagDL.saveInfo = ini.ReadBool("saveInfo", "Global");
                        else
                            tagDL.saveInfo = true;

                        if (ini.KeyExists("openAfter", "Global"))
                            tagDL.openAfter = false; //ini.ReadBool("openAfter", "Global");
                        else
                            tagDL.openAfter = false;

                        if (ini.KeyExists("saveBlacklisted", "Global"))
                            tagDL.saveBlacklistedFiles = ini.ReadBool("saveBlacklisted", "Global");
                        else
                            tagDL.saveBlacklistedFiles = true;

                        if (ini.KeyExists("ignoreFinish", "Global"))
                            tagDL.ignoreFinish = ini.ReadBool("ignoreFinish", "Global");
                        else
                            tagDL.ignoreFinish = false;

                        if (minimumScore) {
                            tagDL.useMinimumScore = minimumScore;
                            tagDL.scoreAsTag = ini.ReadBool("scoreAsTag", "Tags");
                            tagDL.minimumScore = minScore;
                        }

                        if (ini.KeyExists("imageLimit", "Tags"))
                            if (ini.ReadInt("imageLimit", "Tags") > 0)
                                tagDL.imageLimit = ini.ReadInt("imageLimit", "Tags");
                            else
                                tagDL.imageLimit = 0;

                        if (ini.KeyExists("pageLimit", "Tags"))
                            if (ini.ReadInt("pageLimit", "Tags") > 0)
                                tagDL.pageLimit = ini.ReadInt("pageLimit", "Tags");

                        if (ini.KeyExists("separateRatings", "Tags"))
                            tagDL.separateRatings = ini.ReadBool("separateRatings", "Tags");
                        else
                            tagDL.separateRatings = true;

                        if (ini.KeyExists("Explicit", "Tags"))
                            if (ini.ReadBool("Explicit", "Tags"))
                                ratings += "e ";
                        if (ini.KeyExists("Questionable", "Tags"))
                            if (ini.ReadBool("Questionable", "Tags"))
                                ratings += "q ";
                        if (ini.KeyExists("Safe", "Tags"))
                            if (ini.ReadBool("Safe", "Tags"))
                                ratings += "s";
                        ratings = ratings.TrimEnd(' ');

                        if (tagDL.separateRatings)
                            tagDL.ratings = ratings.Split(' ');

                        if (ini.KeyExists("fileNameSchema", "Tags"))
                            tagDL.fileNameSchema = apiTools.ReplaceIllegalCharacters(ini.ReadString("fileNameSchema", "Tags").ToLower());
                        else
                            tagDL.fileNameSchema = "%md5%";
                    }
                    else {
                        General.Default.Reload();
                        Tags.Default.Reload();

                        tagDL.saveTo = General.Default.saveLocation;
                        tagDL.graylist = General.Default.blacklist;
                        tagDL.blacklist = General.Default.zeroToleranceBlacklist;

                        tagDL.saveInfo = General.Default.saveInfo;
                        tagDL.openAfter = false;
                        tagDL.saveBlacklistedFiles = General.Default.saveBlacklisted;
                        tagDL.ignoreFinish = General.Default.ignoreFinish;

                        tagDL.useMinimumScore = minimumScore;
                        if (minimumScore) {
                            tagDL.scoreAsTag = Tags.Default.scoreAsTag;
                            tagDL.minimumScore = minScore;
                        }

                        if (Tags.Default.imageLimit > 0)
                            tagDL.imageLimit = Tags.Default.imageLimit;

                        if (tagDL.pageLimit > 0)
                            tagDL.pageLimit = Tags.Default.pageLimit;

                        tagDL.separateRatings = Tags.Default.separateRatings;

                        if (Tags.Default.Explicit)
                            ratings += "e ";
                        if (Tags.Default.Questionable)
                            ratings += "q ";
                        if (Tags.Default.Safe)
                            ratings += "s";
                        ratings = ratings.TrimEnd(' ');
                        tagDL.ratings = ratings.Split(' ');

                        tagDL.fileNameSchema = apiTools.ReplaceIllegalCharacters(Tags.Default.fileNameSchema.ToLower());
                    }

                    tagDL.ShowDialog();

                }
                catch {
                    return;
                }
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

                Downloader.Arguments.downloadPool(poolid.ToString(), useIni);
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
