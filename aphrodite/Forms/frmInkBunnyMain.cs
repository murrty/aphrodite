using System;
using System.Windows.Forms;

namespace aphrodite {

    public partial class frmInkBunnyMain : Form {

        public frmInkBunnyMain(DownloadType Type = DownloadType.None, string Argument = null) {
            if (!Config.Settings.InkBunny.ReadWarning && !Program.IsDebug) {
                Log.MessageBox("InkBunny uses an API key to access the data.\n\nCurrently, the key is encrypted using a randomly generated password that is only usable by the current user. If you move to another computer, the API key will not be able to be decrypted unless you bring the key with you.\n\nBy setting the API key, you accept the following:\n\n* Its' not that difficult to find the key to decrypt your api key.\n\n* You took proper precautions by either using a throw-away account, or you have nothing to lose and/or just don't give a hoot.\n\n* You won't hold me responsible for anything that occurs using your key.\n\n(This is the only time you'll see this message, unless you set the config setting to false, or delete it.)");
                Config.Settings.InkBunny.ReadWarning = true;
            }

            InitializeComponent();

            switch (Type) {
                case DownloadType.InkBunnyKeywords: {
                    txtSearchKeywordsMD5.Text = Argument ?? "";
                } break;

                case DownloadType.InkBunnyArtistGallery: {
                    txtSearchArtistName.Text = Argument ?? "";
                } break;

                case DownloadType.InkBunnyUsersFavorites: {
                    txtSearchUsersFavorites.Text = Argument ?? "";
                } break;
            }

            chkSkipDebug.Visible = Program.IsDebug;
            chkSkipDebug.Enabled = Program.IsDebug;

            if (Config.ValidPoint(Config.Settings.FormSettings.frmInkBunnyMain_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmInkBunnyMain_Location;
            }

            chkRatingGeneral.Checked = Config.Settings.InkBunny.General;
            chkRatingMatureNudity.Checked = Config.Settings.InkBunny.MatureNudity;
            chkRatingMatureViolence.Checked = Config.Settings.InkBunny.MatureViolence;
            chkRatingAdultSexualThemes.Checked = Config.Settings.InkBunny.AdultSexualThemes;
            chkRatingAdultStrongViolence.Checked = Config.Settings.InkBunny.AdultStrongViolence;

            chkSearchInKeywords.Checked = Config.Settings.InkBunny.SearchInKeywords;
            chkSearchInTitle.Checked = Config.Settings.InkBunny.SearchInTitle;
            chkSearchInDescriptionOrStory.Checked = Config.Settings.InkBunny.SearchInDescriptionOrStory;
            chkSearchInMD5Hash.Checked = Config.Settings.InkBunny.SearchInMd5Hash;

            chkDownloadGraylistedSubmissions.Checked = Config.Settings.InkBunny.DownloadGraylistedSubmissions;
            chkDownloadBlacklistedSubmissions.Checked = Config.Settings.InkBunny.DownloadBlacklistedSubmissions;

            clbSubmissionType.SetItemChecked(0, Config.Settings.InkBunny.PicturePinup);
            clbSubmissionType.SetItemChecked(1, Config.Settings.InkBunny.Sketch);
            clbSubmissionType.SetItemChecked(2, Config.Settings.InkBunny.PictureSeries);
            clbSubmissionType.SetItemChecked(3, Config.Settings.InkBunny.Comic);
            clbSubmissionType.SetItemChecked(4, Config.Settings.InkBunny.Portfolio);
            clbSubmissionType.SetItemChecked(5, Config.Settings.InkBunny.FlashAnimation);
            clbSubmissionType.SetItemChecked(6, Config.Settings.InkBunny.FlashInteractive);
            clbSubmissionType.SetItemChecked(7, Config.Settings.InkBunny.VideoFeatureLength);
            clbSubmissionType.SetItemChecked(8, Config.Settings.InkBunny.VideoAnimation);
            clbSubmissionType.SetItemChecked(9, Config.Settings.InkBunny.MusicSingleTrack);
            clbSubmissionType.SetItemChecked(10, Config.Settings.InkBunny.MusicAlbum);
            clbSubmissionType.SetItemChecked(11, Config.Settings.InkBunny.WritingDocument);
            clbSubmissionType.SetItemChecked(12, Config.Settings.InkBunny.CharacterSheet);
            clbSubmissionType.SetItemChecked(13, Config.Settings.InkBunny.Photography);

            txtFileNameSchema.Text = Config.Settings.InkBunny.FileNameSchema;
            txtFileNameSchemaMultiPost.Text = Config.Settings.InkBunny.FileNameSchemaMultiPost;

            if (Arguments.ArgumentType == ArgumentType.PushInkBunny) {
                if (Arguments.ArgumentDataArray != null) {
                    switch (Arguments.ArgumentDataArray.Length) {
                        case 1: {
                            txtSearchKeywordsMD5.Text = Arguments.ArgumentDataArray[0];
                        } break;

                        case 2: {
                            txtSearchKeywordsMD5.Text = Arguments.ArgumentDataArray[0];
                            txtSearchArtistName.Text = Arguments.ArgumentDataArray[1];
                        } break;

                        case 3: {
                            txtSearchKeywordsMD5.Text = Arguments.ArgumentDataArray[0];
                            txtSearchArtistName.Text = Arguments.ArgumentDataArray[1];
                            txtSearchUsersFavorites.Text = Arguments.ArgumentDataArray[3];
                        } break;
                    }
                }
                else if (Arguments.ArgumentData != null) {
                    txtSearchKeywordsMD5.Text = Arguments.ArgumentData;
                }
            }

            CheckAvailableRatings();
        }

        private void frmInkBunnyMain_FormClosing(Object sender, FormClosingEventArgs e) {
            Config.Settings.FormSettings.frmInkBunnyMain_Location = this.Location;
        }

        private void CheckAvailableRatings() {
            if (!string.IsNullOrEmpty(Config.Settings.InkBunny.GuestSessionID) || !string.IsNullOrWhiteSpace(Config.Settings.InkBunny.SessionID)) {
                mLogin.Text = "change user";
                btnDownload.Enabled = true;
                gbRatings.Enabled = true;
                gbSearchIn.Enabled = true;
                txtSearchKeywordsMD5.Enabled = true;
                txtSearchArtistName.Enabled = true;
                txtSearchUsersFavorites.Enabled = true;
                clbSubmissionType.Enabled = true;
                txtFileNameSchema.Enabled = true;
                txtFileNameSchemaMultiPost.Enabled = true;
                chkDownloadGraylistedSubmissions.Enabled = true;
                chkDownloadBlacklistedSubmissions.Enabled = true;
                mSessionIDs.Enabled = true;
                if (Config.Settings.InkBunny.GuestPriority && !string.IsNullOrWhiteSpace(Config.Settings.InkBunny.GuestSessionID)) {
                    chkRatingGeneral.Enabled = Config.Settings.InkBunny.GuestGeneral;
                    chkRatingMatureNudity.Enabled = Config.Settings.InkBunny.GuestMatureNudity;
                    chkRatingMatureViolence.Enabled = Config.Settings.InkBunny.GuestMatureViolence;
                    chkRatingAdultSexualThemes.Enabled = Config.Settings.InkBunny.GuestAdultSexualThemes;
                    chkRatingAdultStrongViolence.Enabled = Config.Settings.InkBunny.GuestAdultSexualThemes;
                    lbDownloadingAs.Text = "logged in: guest";
                }
                else {
                    lbDownloadingAs.Text = "logged in: " + (string.IsNullOrWhiteSpace(Config.Settings.InkBunny.LoggedInUsername) ? "[unknown]" : Config.Settings.InkBunny.LoggedInUsername);
                }
            }
            else {
                mLogin.Text = "login";
                btnDownload.Enabled = false;
                gbRatings.Enabled = false;
                gbSearchIn.Enabled = false;
                txtSearchKeywordsMD5.Enabled = false;
                txtSearchArtistName.Enabled = false;
                txtSearchUsersFavorites.Enabled = false;
                clbSubmissionType.Enabled = false;
                txtFileNameSchema.Enabled = false;
                txtFileNameSchemaMultiPost.Enabled = false;
                chkDownloadGraylistedSubmissions.Enabled = false;
                chkDownloadBlacklistedSubmissions.Enabled = false;
                mSessionIDs.Enabled = false;
                lbDownloadingAs.Text = "not logged in";
            }
        }

        private void mLogin_Click(Object sender, EventArgs e) {
            using frmInkBunnyLogin Login = new();
            if (Login.ShowDialog() == DialogResult.OK) {
                CheckAvailableRatings();
            }
        }

        private void mShowSessionIDs_Click(Object sender, EventArgs e) {
            Log.MessageBox(
                $"SID: {(!string.IsNullOrWhiteSpace(Config.Settings.InkBunny.SessionID) ? Config.Settings.InkBunny.SessionID : "[not set]")}\n\n" +
                $"Guest SID: {(!string.IsNullOrWhiteSpace(Config.Settings.InkBunny.GuestSessionID) ? Config.Settings.InkBunny.GuestSessionID : "[not set]")}"
            );
        }

        private void mShowDecryptedSessionIDs_Click(Object sender, EventArgs e) {
            Log.MessageBox(
                $"SID: {(!string.IsNullOrWhiteSpace(Config.Settings.InkBunny.SessionID) ? Cryptography.Decrypt(Config.Settings.InkBunny.SessionID) : "[not set]")}\n\n" +
                $"Guest SID: {(!string.IsNullOrWhiteSpace(Config.Settings.InkBunny.GuestSessionID) ? Cryptography.Decrypt(Config.Settings.InkBunny.GuestSessionID) : "[not set]")}"
            );
        }

        private void mCopyDecryptedSessionID_Click(Object sender, EventArgs e) {
            if (!string.IsNullOrWhiteSpace(Config.Settings.InkBunny.SessionID)) {
                Clipboard.SetText(Cryptography.Decrypt(Config.Settings.InkBunny.SessionID));
                System.Media.SystemSounds.Asterisk.Play();
            }
        }

        private void mCopyGuestSessionID_Click(Object sender, EventArgs e) {
            if (!string.IsNullOrWhiteSpace(Config.Settings.InkBunny.GuestSessionID)) {
                Clipboard.SetText(Cryptography.Decrypt(Config.Settings.InkBunny.GuestSessionID));
                System.Media.SystemSounds.Asterisk.Play();
            }
        }

        private void mSettings_Click(Object sender, EventArgs e) {
            using frmSettings Settings = new();
            Settings.ShowDialog();
        }

        private void mLog_Click(Object sender, EventArgs e) {
            Log.ShowLog();
        }

        private void mAbout_Click(Object sender, EventArgs e) {
            frmAbout About = new();
            About.ShowDialog();
        }

        private void txtSearchKeywordsMD5_ButtonClick(Object sender, EventArgs e) {
            txtSearchKeywordsMD5.Clear();
            txtSearchKeywordsMD5.Focus();
        }

        private void txtSearchArtistName_ButtonClick(Object sender, EventArgs e) {
            txtSearchArtistName.Clear();
            txtSearchArtistName.Focus();
        }

        private void txtSearchUsersFavorites_ButtonClick(Object sender, EventArgs e) {
            txtSearchUsersFavorites.Clear();
            txtSearchUsersFavorites.Focus();
        }

        private void txtDownloadOnReturn_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Return) {
                btnDownload_Click(sender, null);
            }
        }

        private void btnDownload_Click(Object sender, EventArgs e) {
            if (string.IsNullOrEmpty(Config.Settings.InkBunny.GuestSessionID) && string.IsNullOrWhiteSpace(Config.Settings.InkBunny.SessionID)) {
                Log.MessageBox("A session id has not be set. Login, set a session id, or generate a guest sid to download.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtSearchKeywordsMD5.Text) && string.IsNullOrWhiteSpace(txtSearchArtistName.Text) && string.IsNullOrWhiteSpace(txtSearchUsersFavorites.Text)) {
                txtSearchKeywordsMD5.Focus();
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }
            if (!chkSearchInKeywords.Checked && !chkSearchInTitle.Checked && !chkSearchInDescriptionOrStory.Checked && !chkSearchInMD5Hash.Checked) {
                Log.MessageBox("You must select a area to search in.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!chkRatingGeneral.Checked && !chkRatingMatureNudity.Checked && !chkRatingMatureViolence.Checked && !chkRatingAdultSexualThemes.Checked && chkRatingAdultStrongViolence.Checked) {
                Log.MessageBox("You must select one rating minimum.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (clbSubmissionType.CheckedItems.Count == 0) {
                Log.MessageBox("You must select one submission type minimum.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string Keywords = txtSearchKeywordsMD5.Text;
            string Artists = txtSearchArtistName.Text;
            string Users = txtSearchUsersFavorites.Text;

            if (ApiTools.IsValidInkBunnyLink(Artists)) {
                if (ApiTools.IsValidInkBunnyUserLink(Artists)) {
                    Artists = Artists.Substring(Artists.IndexOf("inkbunny.net")).Split('/')[0].Split('?')[0];
                }
                else {
                    Log.MessageBox("The artists text box is a valid ink bunny link, but not a valid user link.");
                    return;
                }
            }

            if (ApiTools.IsValidInkBunnyLink(Users)) {
                if (ApiTools.IsValidInkBunnyUserFavoritesLink(Users)) {
                    if (Users.IndexOf("favs_user_id") > 0) {
                        Users = Users.Substring(Users.IndexOf("favs_user_id")).Split('=')[1].Split('&')[0];
                    }
                    else if (Users.IndexOf("user_id") > 0) {
                        Users = Users.Substring(Users.IndexOf("user_id")).Split('=')[1].Split('&')[0];
                    }
                    else {
                        Log.MessageBox("The users' favorites text box is a valid favorites link, but there was no index to get the ID from.");
                        return;
                    }
                }
            }

            frmInkBunnyDownloader DownloadForm = new(
                new(
                    Keywords,
                    Artists,
                    Users
                ) {
                    General =
                        Config.Settings.InkBunny.GuestPriority && !string.IsNullOrWhiteSpace(Config.Settings.InkBunny.GuestSessionID) ? Config.Settings.InkBunny.GuestGeneral && chkRatingGeneral.Checked : chkRatingGeneral.Checked,

                    MatureNudity =
                        Config.Settings.InkBunny.GuestPriority && !string.IsNullOrWhiteSpace(Config.Settings.InkBunny.GuestSessionID) ? Config.Settings.InkBunny.GuestMatureNudity && chkRatingMatureNudity.Checked : chkRatingMatureNudity.Checked,

                    MatureViolence =
                        Config.Settings.InkBunny.GuestPriority && !string.IsNullOrWhiteSpace(Config.Settings.InkBunny.GuestSessionID) ? Config.Settings.InkBunny.GuestMatureViolence && chkRatingMatureViolence.Checked : chkRatingMatureViolence.Checked,

                    AdultSexualThemes =
                        Config.Settings.InkBunny.GuestPriority && !string.IsNullOrWhiteSpace(Config.Settings.InkBunny.GuestSessionID) ? Config.Settings.InkBunny.GuestAdultSexualThemes && chkRatingAdultSexualThemes.Checked : chkRatingAdultSexualThemes.Checked,

                    AdultStrongViolence =
                        Config.Settings.InkBunny.GuestPriority && !string.IsNullOrWhiteSpace(Config.Settings.InkBunny.GuestSessionID) ? Config.Settings.InkBunny.GuestAdultStrongViolence && chkRatingAdultStrongViolence.Checked : chkRatingAdultStrongViolence.Checked,

                    SearchInKeywords = chkSearchInKeywords.Checked,
                    SearchInTitle = chkSearchInTitle.Checked,
                    SearchInDescriptionOrStory = chkSearchInDescriptionOrStory.Checked,
                    SearchInMd5Hash = chkSearchInMD5Hash.Checked,

                    PicturePinup = clbSubmissionType.GetItemChecked(0),
                    Sketch = clbSubmissionType.GetItemChecked(1),
                    PictureSeries = clbSubmissionType.GetItemChecked(2),
                    Comic = clbSubmissionType.GetItemChecked(3),
                    Portfolio = clbSubmissionType.GetItemChecked(4),
                    FlashAnimation = clbSubmissionType.GetItemChecked(5),
                    FlashInteractive = clbSubmissionType.GetItemChecked(6),
                    VideoFeatureLength = clbSubmissionType.GetItemChecked(7),
                    VideoAnimation = clbSubmissionType.GetItemChecked(8),
                    MusicSingleTrack = clbSubmissionType.GetItemChecked(9),
                    MusicAlbum = clbSubmissionType.GetItemChecked(10),
                    WritingDocument = clbSubmissionType.GetItemChecked(11),
                    CharacterSheet = clbSubmissionType.GetItemChecked(12),
                    Photography = clbSubmissionType.GetItemChecked(13),

                    FileNameSchema = txtFileNameSchema.Text,
                    FileNameSchemaMultiPost = txtFileNameSchemaMultiPost.Text
                }
            );
            DownloadForm.Show();

            Config.Settings.InkBunny.General = chkRatingGeneral.Checked;
            Config.Settings.InkBunny.MatureNudity = chkRatingMatureNudity.Checked;
            Config.Settings.InkBunny.MatureViolence = chkRatingMatureViolence.Checked;
            Config.Settings.InkBunny.AdultSexualThemes = chkRatingAdultSexualThemes.Checked;
            Config.Settings.InkBunny.AdultStrongViolence = chkRatingAdultStrongViolence.Checked;

            Config.Settings.InkBunny.SearchInKeywords = chkSearchInKeywords.Checked;
            Config.Settings.InkBunny.SearchInTitle = chkSearchInTitle.Checked;
            Config.Settings.InkBunny.SearchInDescriptionOrStory = chkSearchInDescriptionOrStory.Checked;
            Config.Settings.InkBunny.SearchInMd5Hash = chkSearchInMD5Hash.Checked;

            Config.Settings.InkBunny.DownloadGraylistedSubmissions = chkDownloadGraylistedSubmissions.Checked;
            Config.Settings.InkBunny.DownloadBlacklistedSubmissions = chkDownloadBlacklistedSubmissions.Checked;

            Config.Settings.InkBunny.PicturePinup = clbSubmissionType.GetItemChecked(0);
            Config.Settings.InkBunny.Sketch = clbSubmissionType.GetItemChecked(1);
            Config.Settings.InkBunny.PictureSeries = clbSubmissionType.GetItemChecked(2);
            Config.Settings.InkBunny.Comic = clbSubmissionType.GetItemChecked(3);
            Config.Settings.InkBunny.Portfolio = clbSubmissionType.GetItemChecked(4);
            Config.Settings.InkBunny.FlashAnimation = clbSubmissionType.GetItemChecked(5);
            Config.Settings.InkBunny.FlashInteractive = clbSubmissionType.GetItemChecked(6);
            Config.Settings.InkBunny.VideoFeatureLength = clbSubmissionType.GetItemChecked(7);
            Config.Settings.InkBunny.VideoAnimation = clbSubmissionType.GetItemChecked(8);
            Config.Settings.InkBunny.MusicSingleTrack = clbSubmissionType.GetItemChecked(9);
            Config.Settings.InkBunny.MusicAlbum = clbSubmissionType.GetItemChecked(10);
            Config.Settings.InkBunny.WritingDocument = clbSubmissionType.GetItemChecked(11);
            Config.Settings.InkBunny.CharacterSheet = clbSubmissionType.GetItemChecked(12);
            Config.Settings.InkBunny.Photography = clbSubmissionType.GetItemChecked(13);

            Config.Settings.InkBunny.FileNameSchema = txtFileNameSchema.Text;
            Config.Settings.InkBunny.FileNameSchemaMultiPost = txtFileNameSchemaMultiPost.Text;
        }

        private void llbSchemaHelp_LinkClicked(Object sender, LinkLabelLinkClickedEventArgs e) {
            Log.MessageBox(

                "The following replacements are available:\n\n" +

                $"%id% - The id of the submission.\n" +
                $"%multipostindex% - the index number of the image in the submission.\n" +
                $"%fileid% - the id of the file.\n" +
                $"%ext% - the extension of the file."

            );
        }

        private void chkSearchInMD5Hash_CheckedChanged(Object sender, EventArgs e) {
            chkSearchInKeywords.Enabled = chkSearchInTitle.Enabled = chkSearchInDescriptionOrStory.Enabled = !chkSearchInMD5Hash.Checked;
            txtSearchKeywordsMD5.TextHint = chkSearchInMD5Hash.Checked ? "md5 hash..." : "keywords...";
        }

    }
}
