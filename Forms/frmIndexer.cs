using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace aphrodite {
    public partial class frmIndexer : Form {
        public static readonly string emptyXML = "<root type=\"array\"></root>";
        public static readonly string pageJson = "&page=";
        public static readonly string imageJson = "https://e621.net/post/show.json?id=";
        public static readonly string poolJson = "https://e621.net/pool/show.json?id=";
        public static readonly string tagJson = "https://e621.net/post/index.json?tags=";
        public static readonly string tagLimit = "&limit=120";

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public frmIndexer() {
            InitializeComponent();
        }

        private string getJSON(string url) {
            Debug.Print("getJson starting");

            try {
                Debug.Print("Starting tag json download");
                using (WebClient wc = new WebClient()) {
                    wc.Headers.Add("User-Agent: Humina/1.0");
                    wc.Proxy = WebProxy.GetDefaultProxy();
                    string json = wc.DownloadString(url);
                    byte[] bytes = Encoding.ASCII.GetBytes(json);
                    using (var stream = new MemoryStream(bytes)) {
                        var quotas = new XmlDictionaryReaderQuotas();
                        var jsonReader = JsonReaderWriterFactory.CreateJsonReader(stream, quotas);
                        var xml = XDocument.Load(jsonReader);
                        stream.Flush();
                        stream.Close();
                        if (xml != null) {
                            Debug.Print("Json converted, returning full json");
                            return xml.ToString();
                        }
                        else {
                            Debug.Print("Json returned null, returning null");
                            return null;
                        }
                    }
                }
            }
            catch (ThreadAbortException thrEx) {
                Debug.Print("Thread was requested to be, and has been, aborted.");
                Debug.Print("==========BEGIN THREADABORTEXCEPTION==========");
                Debug.Print(thrEx.ToString());
                Debug.Print("==========END THREADABORTEXCEPTION==========");
                return null;
                throw thrEx;
            }
            catch (WebException WebE) {
                Debug.Print("A WebException has occured.");
                Debug.Print("==========BEGIN WEBEXCEPTION==========");
                Debug.Print(WebE.ToString());
                Debug.Print("==========END WEBEXCEPTION==========");
                MessageBox.Show(WebE.ToString());
                return null;
                throw WebE;
            }
            catch (Exception ex) {
                Debug.Print("A gneral exception has occured.");
                Debug.Print("==========BEGIN EXCEPTION==========");
                Debug.Print(ex.ToString());
                Debug.Print("==========END EXCEPTION==========");
                return null;
                throw ex;
            }
        }

        private void btnRetrieve_Click(object sender, EventArgs e) {
            int n;
            bool isNumeric = int.TryParse(txtInput.Text, out n);

            if (rbImage.Checked) {
                if (!isNumeric)
                    return;
                //addImageTab("test md5", "testartist", "test tags", "test desc", "test score", "test rating");
                if (getImageInfo(txtInput.Text)) {
                    btnDisposeTab.Enabled = true;
                    btnDisposeAll.Enabled = true;
                }
            }
            else if (rbPool.Checked) {
                if (!isNumeric)
                    return;
                //addPoolTab("test md5", "testartist testartist2", "test tags", "test desc", "testurl.com", "test score", "test rating");
                getPoolInfo(txtInput.Text);
            }
            else if (rbTag.Checked) {
                //addTagTab("000", "test md5", "testartist testartist2", "test tags", "test desc", "testurl.com", "test score", "test rating");
                getTagInfo(txtInput.Text);
            }
        }

        private bool getImageInfo(string id) {
            try {
                string xml = getJSON(imageJson + id);
                if (xml == emptyXML) {
                    return false;
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                XmlNodeList xmlMD5 = doc.DocumentElement.SelectNodes("/root/md5");
                XmlNodeList xmlArtist = doc.DocumentElement.SelectNodes("/root/artist/item");
                XmlNodeList xmlTags = doc.DocumentElement.SelectNodes("/root/tags");
                XmlNodeList xmlDesc = doc.DocumentElement.SelectNodes("/root/description");
                XmlNodeList xmlUrl = doc.DocumentElement.SelectNodes("/root/file_url");

                XmlNodeList xmlScore = doc.DocumentElement.SelectNodes("/root/score");
                XmlNodeList xmlRating = doc.DocumentElement.SelectNodes("/root/rating");

                string artists = string.Empty;
                for (int i = 0; i < xmlArtist.Count; i++) {
                    artists += xmlArtist[i].InnerText + " ";
                }
                artists = artists.TrimEnd(' ');

                addImageTab(id, xmlMD5[0].InnerText, artists, xmlTags[0].InnerText, xmlDesc[0].InnerText, xmlUrl[0].InnerText, xmlScore[0].InnerText, xmlRating[0].InnerText);


                return true;
            }
            catch (Exception) {
                return false;
            }
        }
        private bool getPoolInfo(string poolid) {
            txtPoolId.Text = poolid;

            if (tcPoolPages.TabPages.Count > 0) {
                for (int i = 0; i < tcPoolPages.TabPages.Count; i++) {
                    tcPoolPages.TabPages[i].Dispose();
                }
                tcPoolPages.TabPages.Clear();
            }
            
            try {
                string xml = getJSON(poolJson + poolid);
                if (xml == emptyXML) {
                    return false;
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNodeList xmlName = doc.DocumentElement.SelectNodes("/root/name");
                XmlNodeList xmlCount = doc.DocumentElement.SelectNodes("/root/post_count");
                XmlNodeList xmlDescription = doc.DocumentElement.SelectNodes("/root/description");

                txtPoolName.Text = xmlName[0].InnerText;
                txtPoolPages.Text = xmlCount[0].InnerText;
                txtPoolDesc.Text = xmlDescription[0].InnerText.Replace("_", " ");

                int count = Convert.ToInt32(xmlCount[0].InnerText);
                int pages = 1;
                if (count > 24) {
                    // More than 24 pages, math to get the required pages
                    decimal pageGuess = decimal.Divide(count, 24);
                    pages = Convert.ToInt32(Math.Ceiling(pageGuess));
                }

                XmlNodeList xmlID = doc.DocumentElement.SelectNodes("/root/posts/item/id");

                XmlNodeList xmlMD5 = doc.DocumentElement.SelectNodes("/root/posts/item/md5");
                XmlNodeList xmlArtist = doc.DocumentElement.SelectNodes("/root/posts/item/artist");
                XmlNodeList xmlTags = doc.DocumentElement.SelectNodes("/root/posts/item/tags");
                XmlNodeList xmlDesc = doc.DocumentElement.SelectNodes("/root/posts/item/description");
                XmlNodeList xmlUrl = doc.DocumentElement.SelectNodes("/root/posts/item/file_url");

                XmlNodeList xmlScore = doc.DocumentElement.SelectNodes("/root/posts/item/score");
                XmlNodeList xmlRating = doc.DocumentElement.SelectNodes("/root/posts/item/rating");

                for (int i = 0; i < xmlID.Count; i++) {
                    addPoolTab(xmlMD5[i].InnerText, xmlArtist[i].InnerText, xmlTags[i].InnerText, xmlDesc[i].InnerText, xmlUrl[i].InnerText, xmlScore[i].InnerText, xmlRating[i].InnerText);
                }

                if (pages > 1) {
                    for (int h = 2; h < pages + 1; h++) {
                        xml = getJSON(poolJson + poolid + pageJson + (h));
                        doc.LoadXml(xml);
                        if (xml == emptyXML) {
                            break;
                        }

                        xmlID = doc.DocumentElement.SelectNodes("/root/posts/item/id");

                        xmlMD5 = doc.DocumentElement.SelectNodes("/root/posts/item/md5");
                        xmlArtist = doc.DocumentElement.SelectNodes("/root/posts/item/artist");
                        xmlTags = doc.DocumentElement.SelectNodes("/root/posts/item/tags");
                        xmlDesc = doc.DocumentElement.SelectNodes("/root/posts/item/description");
                        xmlUrl = doc.DocumentElement.SelectNodes("/root/posts/item/file_url");

                        xmlScore = doc.DocumentElement.SelectNodes("/root/posts/item/score");
                        xmlRating = doc.DocumentElement.SelectNodes("/root/posts/item/rating");

                        for (int i = 0; i < xmlID.Count; i++) {
                            addPoolTab(xmlMD5[i].InnerText, xmlArtist[i].InnerText, xmlTags[i].InnerText, xmlDesc[i].InnerText, xmlUrl[i].InnerText, xmlScore[i].InnerText, xmlRating[i].InnerText);
                        }
                    }
                }

                return true;
            }
            catch (Exception) {
                return false;
            }
        }
        private bool getTagInfo(string tags) {
            if (tcTags.TabPages.Count > 0) {
                for (int i = 0; i < tcTags.TabPages.Count; i++) {
                    tcTags.TabPages[i].Dispose();
                }
                tcTags.TabPages.Clear();
            }

            try {
                string xml = getJSON(tagJson + tags + tagLimit);
                if (xml == emptyXML) {
                    return false;
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNodeList xmlID = doc.DocumentElement.SelectNodes("/root/item/id");

                XmlNodeList xmlMD5 = doc.DocumentElement.SelectNodes("/root/item/md5");
                XmlNodeList xmlArtist = doc.DocumentElement.SelectNodes("/root/item/artist");
                XmlNodeList xmlTags = doc.DocumentElement.SelectNodes("/root/item/tags");
                XmlNodeList xmlDesc = doc.DocumentElement.SelectNodes("/root/item/description");
                XmlNodeList xmlURL = doc.DocumentElement.SelectNodes("/root/item/file_url");

                XmlNodeList xmlScore = doc.DocumentElement.SelectNodes("/root/item/score");
                XmlNodeList xmlRating = doc.DocumentElement.SelectNodes("/root/item/rating");

                for (int i = 0; i < xmlID.Count; i++) {
                    string artists = string.Empty;

                    for (int j = 0; j < xmlArtist[i].ChildNodes.Count; j++) {
                        artists += xmlArtist[i].ChildNodes[j].InnerText + " ";
                    }

                    artists = artists.TrimEnd(' ');

                    addTagTab(xmlID[i].InnerText, xmlMD5[i].InnerText, artists, xmlTags[i].InnerText, xmlDesc[i].InnerText, xmlURL[i].InnerText, xmlScore[i].InnerText, xmlRating[i].InnerText);
                }
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        private void addPoolTab(string md5, string artists, string tags, string desc, string url, string score, string rating) {
            TabPage tbp = new TabPage();
            string number = (tcPoolPages.TabPages.Count + 1).ToString();
            tbp.Name = "tbPoolPage" + number;
            tbp.Text = "Page " + number;
            Label lbMd = new Label();
            lbMd.Text = "md5";
            lbMd.Name = "lbPageMd5" + number;
            lbMd.Location = new Point(21, 12);
            lbMd.AutoSize = true;
            Label lbArtists = new Label();
            lbArtists.Text = "artist(s)";
            lbArtists.Name = "lbPageArtist" + number;
            lbArtists.Location = new Point(21, 38);
            lbArtists.AutoSize = true;
            Label lbTags = new Label();
            lbTags.Text = "tags";
            lbTags.Name = "lbPageTags" + number;
            lbTags.Location = new Point(21, 64);
            lbTags.AutoSize = true;
            Label lbDesc = new Label();
            lbDesc.Text = "desc.";
            lbDesc.Name = "lbPageDesc" + number;
            lbDesc.Location = new Point(21, 90);
            lbDesc.AutoSize = true;
            Label lbUrl = new Label();
            lbUrl.Text = "url";
            lbUrl.Name = "lbPageUrl" + number;
            lbUrl.Location = new Point(21, 116);
            lbUrl.AutoSize = true;

            TextBox txtPageMd = new TextBox();
            txtPageMd.Text = md5;
            txtPageMd.Name = "txtPageMd5" + number;
            txtPageMd.Size = new Size(322, 20);
            txtPageMd.Location = new Point(69, 9);
            txtPageMd.ReadOnly = true;
            TextBox txtPageArtists = new TextBox();
            txtPageArtists.Text = artists;
            txtPageArtists.Name = "txtPageArtists" + number;
            txtPageArtists.Size = new Size(322, 20);
            txtPageArtists.Location = new Point(69, 35);
            txtPageArtists.ReadOnly = true;
            TextBox txtPageTags = new TextBox();
            txtPageTags.Text = tags;
            txtPageTags.Name = "txtPageTags" + number;
            txtPageTags.Size = new Size(322, 20);
            txtPageTags.Location = new Point(69, 61);
            txtPageTags.ReadOnly = true;
            TextBox txtPageDesc = new TextBox();
            txtPageDesc.Text = desc;
            txtPageDesc.Name = "txtPageDesc" + number;
            txtPageDesc.Size = new Size(322, 20);
            txtPageDesc.Location = new Point(69, 87);
            txtPageDesc.ReadOnly = true;
            TextBox txtPageUrl = new TextBox();
            txtPageUrl.Text = url;
            txtPageUrl.Name = "txtPageUrl" + number;
            txtPageUrl.Size = new Size(322, 20);
            txtPageUrl.Location = new Point(69, 113);
            txtPageUrl.ReadOnly = true;

            Label lbPageScore = new Label();
            lbPageScore.Text = "score: " + score;
            lbPageScore.Name = "lbPageScore" + number;
            lbPageScore.Location = new Point(28, 148);
            lbPageScore.AutoSize = true;

            Label lbPageRating = new Label();
            lbPageRating.Text = "rating: " + rating;
            lbPageRating.Name = "lbPageRating" + number;
            lbPageRating.Location = new Point(28, 173);
            lbPageScore.AutoSize = true;


            tbp.Controls.Add(lbMd);
            tbp.Controls.Add(lbArtists);
            tbp.Controls.Add(lbTags);
            tbp.Controls.Add(lbDesc);
            tbp.Controls.Add(lbUrl);
            tbp.Controls.Add(txtPageMd);
            tbp.Controls.Add(txtPageArtists);
            tbp.Controls.Add(txtPageTags);
            tbp.Controls.Add(txtPageDesc);
            tbp.Controls.Add(txtPageUrl);
            tbp.Controls.Add(lbPageScore);
            tbp.Controls.Add(lbPageRating);


            tcPoolPages.TabPages.Add(tbp);
        }
        private void addImageTab(string id, string md5, string artists, string tags, string desc, string url, string score, string rating) {
            TabPage tbp = new TabPage();
            string number = (tcImages.TabPages.Count + 1).ToString();
            tbp.Name = "tbImage" + number;
            tbp.Text = "Image " + id;
            Label lbMd = new Label();
            lbMd.Text = "md5";
            lbMd.Name = "lbImageMd5" + number;
            lbMd.Location = new Point(16, 16);
            lbMd.AutoSize = true;
            Label lbArtists = new Label();
            lbArtists.Text = "artist(s)";
            lbArtists.Name = "lbImageArtist" + number;
            lbArtists.Location = new Point(16, 42);
            lbArtists.AutoSize = true;
            Label lbTags = new Label();
            lbTags.Text = "tags";
            lbTags.Name = "lbImageTags" + number;
            lbTags.Location = new Point(16, 68);
            lbTags.AutoSize = true;
            Label lbDesc = new Label();
            lbDesc.Text = "desc.";
            lbDesc.Name = "lbImageDesc" + number;
            lbDesc.Location = new Point(16, 94);
            lbDesc.AutoSize = true;
            Label lbUrl = new Label();
            lbUrl.Text = "url";
            lbUrl.Name = "lbImageUrl" + number;
            lbUrl.Location = new Point(16, 120);
            lbUrl.AutoSize = true;

            TextBox txtPageMd = new TextBox();
            txtPageMd.Text = md5;
            txtPageMd.Name = "txtImageMd5" + number;
            txtPageMd.Size = new Size(333, 20);
            txtPageMd.Location = new Point(64, 13);
            txtPageMd.ReadOnly = true;
            TextBox txtPageArtists = new TextBox();
            txtPageArtists.Text = artists;
            txtPageArtists.Name = "txtImageArtists" + number;
            txtPageArtists.Size = new Size(333, 20);
            txtPageArtists.Location = new Point(64, 39);
            txtPageArtists.ReadOnly = true;
            TextBox txtPageTags = new TextBox();
            txtPageTags.Text = tags;
            txtPageTags.Name = "txtImageTags" + number;
            txtPageTags.Size = new Size(333, 20);
            txtPageTags.Location = new Point(64, 65);
            txtPageTags.ReadOnly = true;
            TextBox txtPageDesc = new TextBox();
            txtPageDesc.Text = desc;
            txtPageDesc.Name = "txtImageDesc" + number;
            txtPageDesc.Size = new Size(333, 20);
            txtPageDesc.Location = new Point(64, 91);
            txtPageDesc.ReadOnly = true;
            TextBox txtPageUrl = new TextBox();
            txtPageUrl.Text = url;
            txtPageUrl.Name = "txtImageUrl" + number;
            txtPageUrl.Size = new Size(333, 20);
            txtPageUrl.Location = new Point(64, 117);
            txtPageUrl.ReadOnly = true;

            Label lbPageScore = new Label();
            lbPageScore.Text = "score: " + score;
            lbPageScore.Name = "lbImageScore" + number;
            lbPageScore.Location = new Point(29, 152);
            lbPageScore.AutoSize = true;

            Label lbPageRating = new Label();
            lbPageRating.Text = "rating: " + rating;
            lbPageRating.Name = "lbImageRating" + number;
            lbPageRating.Location = new Point(29, 177);
            lbPageScore.AutoSize = true;


            tbp.Controls.Add(lbMd);
            tbp.Controls.Add(lbArtists);
            tbp.Controls.Add(lbTags);
            tbp.Controls.Add(lbDesc);
            tbp.Controls.Add(lbUrl);

            tbp.Controls.Add(txtPageMd);
            tbp.Controls.Add(txtPageArtists);
            tbp.Controls.Add(txtPageTags);
            tbp.Controls.Add(txtPageDesc);
            tbp.Controls.Add(txtPageUrl);

            tbp.Controls.Add(lbPageScore);
            tbp.Controls.Add(lbPageRating);


            tcImages.TabPages.Add(tbp);
        }
        private void addTagTab(string id, string md5, string artists, string tags, string desc, string url, string score, string rating) {
            TabPage tbp = new TabPage();
            string number = (tcImages.TabPages.Count + 1).ToString();
            tbp.Name = "tbTag" + number;
            tbp.Text = "Tag " + id;
            Label lbMd = new Label();
            lbMd.Text = "md5";
            lbMd.Name = "lbTagMd5" + number;
            lbMd.Location = new Point(23, 15);
            lbMd.AutoSize = true;
            Label lbArtists = new Label();
            lbArtists.Text = "artist(s)";
            lbArtists.Name = "lbTagArtist" + number;
            lbArtists.Location = new Point(23, 41);
            lbArtists.AutoSize = true;
            Label lbTags = new Label();
            lbTags.Text = "tags";
            lbTags.Name = "lbTagTags" + number;
            lbTags.Location = new Point(23, 67);
            lbTags.AutoSize = true;
            Label lbDesc = new Label();
            lbDesc.Text = "desc.";
            lbDesc.Name = "lbTagDesc" + number;
            lbDesc.Location = new Point(23, 93);
            lbDesc.AutoSize = true;
            Label lbUrl = new Label();
            lbUrl.Text = "url";
            lbUrl.Name = "lbTagUrl" + number;
            lbUrl.Location = new Point(23, 119);
            lbUrl.AutoSize = true;

            TextBox txtPageMd = new TextBox();
            txtPageMd.Text = md5;
            txtPageMd.Name = "txtTagMd5" + number;
            txtPageMd.Size = new Size(320, 20);
            txtPageMd.Location = new Point(69, 12);
            txtPageMd.ReadOnly = true;
            TextBox txtPageArtists = new TextBox();
            txtPageArtists.Text = artists;
            txtPageArtists.Name = "txtTagArtists" + number;
            txtPageArtists.Size = new Size(320, 20);
            txtPageArtists.Location = new Point(69, 38);
            txtPageArtists.ReadOnly = true;
            TextBox txtPageTags = new TextBox();
            txtPageTags.Text = tags;
            txtPageTags.Name = "txtTagTags" + number;
            txtPageTags.Size = new Size(320, 20);
            txtPageTags.Location = new Point(69, 64);
            txtPageTags.ReadOnly = true;
            TextBox txtPageDesc = new TextBox();
            txtPageDesc.Text = desc;
            txtPageDesc.Name = "txtTagDesc" + number;
            txtPageDesc.Size = new Size(320, 20);
            txtPageDesc.Location = new Point(69, 90);
            txtPageDesc.ReadOnly = true;
            TextBox txtPageUrl = new TextBox();
            txtPageUrl.Text = url;
            txtPageUrl.Name = "txtTagUrl" + number;
            txtPageUrl.Size = new Size(320, 20);
            txtPageUrl.Location = new Point(69, 116);
            txtPageUrl.ReadOnly = true;

            Label lbPageScore = new Label();
            lbPageScore.Text = "score: " + score;
            lbPageScore.Name = "lbTagScore" + number;
            lbPageScore.Location = new Point(45, 150);
            lbPageScore.AutoSize = true;

            Label lbPageRating = new Label();
            lbPageRating.Text = "rating: " + rating;
            lbPageRating.Name = "lbTagRating" + number;
            lbPageRating.Location = new Point(45, 175);
            lbPageScore.AutoSize = true;


            tbp.Controls.Add(lbMd);
            tbp.Controls.Add(lbArtists);
            tbp.Controls.Add(lbTags);
            tbp.Controls.Add(lbDesc);
            tbp.Controls.Add(lbUrl);

            tbp.Controls.Add(txtPageMd);
            tbp.Controls.Add(txtPageArtists);
            tbp.Controls.Add(txtPageTags);
            tbp.Controls.Add(txtPageDesc);
            tbp.Controls.Add(txtPageUrl);

            tbp.Controls.Add(lbPageScore);
            tbp.Controls.Add(lbPageRating);

            tcTags.TabPages.Add(tbp);
        }

        private void btnDisposeTab_Click(object sender, EventArgs e) {
            if (tcImages.TabPages.Count > 0) {
                tcImages.TabPages[tcTabs.SelectedIndex].Dispose();
            }

            if (tcImages.TabPages.Count == 0) {
                btnDisposeTab.Enabled = false;
                btnDisposeAll.Enabled = false;
            }
        }
        private void btnDisposeAll_Click(object sender, EventArgs e) {
            if (tcImages.TabPages.Count > 0) {
                if (MessageBox.Show("dispose all tabs?", "tagindexer", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    for (int i = 0; i < tcImages.TabPages.Count; i++) {
                        tcImages.TabPages[i].Dispose();
                    }
                    tcImages.TabPages.Clear();
                    btnDisposeTab.Enabled = false;
                    btnDisposeAll.Enabled = false;
                }
            }
        }

        private void txtInput_KeyPress(object sender, KeyPressEventArgs e) {
            if (txtInput.Text.Count(x => x == ' ') >= 5 && e.KeyChar != (char)8 && e.KeyChar == (char)Keys.Space && txtInput.SelectionLength != txtInput.TextLength)
                e.Handled = true;
            else
                e.Handled = false;
        }

        private void pbHint_MouseEnter(object sender, EventArgs e) {
            lbPoss.Visible = true;
        }

        private void pbHint_MouseLeave(object sender, EventArgs e) {
            lbPoss.Visible = false;
        }

        private void frmIndexer_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pbHint_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
