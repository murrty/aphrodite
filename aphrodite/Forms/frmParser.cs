using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmParser : Form {
        List<string> imagesPost = new List<string>();
        List<string> imagesMD5 = new List<string>();
        List<string> imagesUrl = new List<string>();
        List<string> imagesArtists = new List<string>();
        List<string> imagesTags = new List<string>();
        List<string> imagesScores = new List<string>();
        List<string> imagesRatings = new List<string>();
        List<string> imagesDescription = new List<string>();

        public frmParser() {
            InitializeComponent();
            this.Icon = Properties.Resources.Brad;
        }

        private void listImages_SelectedIndexChanged(object sender, EventArgs e) {
            lbImagePost.Text = "post " + imagesPost[listImages.SelectedIndex];
            lbImageMd5.Text = "md5: " + imagesMD5[listImages.SelectedIndex];
            linkImage.Text = imagesUrl[listImages.SelectedIndex];
            lbImageArtists.Text = "artist(s): " + imagesArtists[listImages.SelectedIndex];
            txtImageTags.Text = imagesTags[listImages.SelectedIndex];
            lbImageScore.Text = "score: " + imagesScores[listImages.SelectedIndex];
            lbImageRating.Text = "rating: " + imagesRatings[listImages.SelectedIndex];
            rtxtImages.Text = imagesDescription[listImages.SelectedIndex];
        }

        private void parseImages(string nfoLocation, bool blacklisted = false) {
            if (!File.Exists(nfoLocation))
                return;

            try {
                if (blacklisted) {

                }
                else {
                    string nextLine;
                    string artists = "n/a";
                    string description = "n/a";
                    using (StreamReader sr = new StreamReader(nfoLocation)) {
                        while (!sr.EndOfStream) {
                            imagesPost.Add(sr.ReadLine().Replace("POST ", "").Replace(":", ""));
start:
                            imagesMD5.Add(sr.ReadLine().Replace("    MD5: ", ""));
                            imagesUrl.Add(sr.ReadLine().Replace("    URL: ", ""));
                            artists = sr.ReadLine().Replace("    ARTIST(S): ", "");
                            nextLine = sr.ReadLine();
                            while (!nextLine.StartsWith("    TAGS: ")) {
                                artists += ", " + nextLine.Replace("               ", "");
                                nextLine = sr.ReadLine();
                            }
                            imagesArtists.Add(artists);
                            imagesTags.Add(nextLine.Replace("    TAGS: ", ""));
                            imagesScores.Add(sr.ReadLine().Replace("    SCORE: ", ""));
                            imagesRatings.Add(sr.ReadLine().Replace("    RATING: ", ""));
                            sr.ReadLine();
                            nextLine = sr.ReadLine();
                            description = "n/a";
                            if (nextLine != "\"    \"") {
                                while (!nextLine.StartsWith("POST")) {
                                    description += nextLine + "\n";
                                    nextLine = sr.ReadLine();
                                }
                                imagesPost.Add(nextLine.Replace("POST ", "").Replace(":", ""));
                                goto start;
                            }
                            else {
                                imagesDescription.Add(description.TrimEnd('\n'));
                            }
                            sr.ReadLine();
                        }
                    }

                    listImages.Items.AddRange(imagesMD5.ToArray());
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
        }

        private void frmParser_Load(object sender, EventArgs e) {
            parseImages("C:\\e621\\Images\\images.nfo");
        }

        private void frmParser_FormClosing(object sender, FormClosingEventArgs e) {
            this.Dispose();
        }
    }
}
