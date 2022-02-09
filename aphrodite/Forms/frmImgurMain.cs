using System;
using System.Windows.Forms;

namespace aphrodite {

    public partial class frmImgurMain : Form {

        public frmImgurMain(string Argument = null) {
            if (!Config.Settings.Imgur.ReadWarning && !Program.IsDebug) {
                Log.MessageBox("Imgur uses an API key to access the data.\n\nCurrently, the key is encrypted using a randomly generated password that is only usable by the current user. If you move to another computer, the API key will not be able to be decrypted unless you bring the key with you.\n\nBy setting the API key, you accept the following:\n\n* Its' not that difficult to find the key to decrypt your api key.\n\n* You took proper precautions by either using a throw-away account, or you have nothing to lose and/or just don't give a hoot.\n\n* You won't hold me responsible for anything that occurs using your key.\n\n(This is the only time you'll see this message, unless you set the config setting to false, or delete it.)");
                Config.Settings.Imgur.ReadWarning = true;
            }

            InitializeComponent();

            if (Config.ValidPoint(Config.Settings.FormSettings.frmImgurMain_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmImgurMain_Location;
            }

            txtAlbum.Text = Argument ?? "";
            mLogin.Text = string.IsNullOrWhiteSpace(Config.Settings.Imgur.ClientID) ? "login" : "change user";
            mClientID.Enabled = txtAlbum.Enabled = chkSeparateNonImages.Enabled = numImageLimit.Enabled = txtFileNameSchema.Enabled = btnDownload.Enabled = !string.IsNullOrWhiteSpace(Config.Settings.Imgur.ClientID);
            if (btnDownload.Enabled) {
                chkSeparateNonImages.Checked = Config.Settings.Imgur.SeparateNonImages;
                numImageLimit.Value = (decimal)Config.Settings.Imgur.ImageLimit;
                txtFileNameSchema.Text = Config.Settings.Imgur.FileNameSchema;
            }
        }

        private void frmImgurMain_FormClosing(Object sender, FormClosingEventArgs e) {
            Config.Settings.FormSettings.frmImgurMain_Location = this.Location;
            if (!string.IsNullOrWhiteSpace(Config.Settings.Imgur.ClientID)) {
                Config.Settings.Imgur.SeparateNonImages = chkSeparateNonImages.Checked;
                Config.Settings.Imgur.ImageLimit = (int)numImageLimit.Value;
                Config.Settings.Imgur.FileNameSchema = txtFileNameSchema.Text;
                Config.Settings.Imgur.Save();
            }
        }

        private void mLogin_Click(Object sender, EventArgs e) {
            using frmImgurLogin Login = new();
            Login.ShowDialog();
            mClientID.Enabled = txtAlbum.Enabled = chkSeparateNonImages.Enabled = numImageLimit.Enabled = txtFileNameSchema.Enabled = btnDownload.Enabled = !string.IsNullOrWhiteSpace(Config.Settings.Imgur.ClientID);
        }

        private void mCopyDecryptedClientID_Click(Object sender, EventArgs e) {
            if (!string.IsNullOrWhiteSpace(Config.Settings.Imgur.ClientID)) {
                Clipboard.SetText(Cryptography.Decrypt(Config.Settings.Imgur.ClientID));
                System.Media.SystemSounds.Asterisk.Play();
            }
        }

        private void mShowClientID_Click(Object sender, EventArgs e) {
            Log.MessageBox($"client id: {(string.IsNullOrWhiteSpace(Config.Settings.Imgur.ClientID) ? "[not set]" : Config.Settings.Imgur.ClientID)}", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void myShowDecryptedClientID_Click(Object sender, EventArgs e) {
            Log.MessageBox($"client id: {(string.IsNullOrWhiteSpace(Config.Settings.Imgur.ClientID) ? "[not set]" : Cryptography.Decrypt(Config.Settings.Imgur.ClientID))}", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void txtAlbum_ButtonClick(Object sender, EventArgs e) {
            txtAlbum.Clear();
            txtAlbum.Focus();
        }

        private void txtAlbum_KeyPress(Object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Return) {
                btnDownload_Click(txtAlbum, null);
            }
        }

        private void llSchemaHelp_LinkClicked(Object sender, LinkLabelLinkClickedEventArgs e) {
            Log.MessageBox(
                "The following flags are available for imgur:\n\n" +

                "%albumid% - The ID of the album" + "\n" +
                "%albumtitle% - The title of the album" + "\n" +
                "%albumaccounturl% - The account url that uploaded the album" + "\n" +
                "%albumaccountid% - The ID of the account that uploaded the album" + "\n" +
                "%imageindex% - The index of the image in the album\n" +
                "%imageid% - The ID of the image" + "\n" +
                "%imagetitle% - The title of the image" + "\n" +
                "%imagename% - The name of the image" + "\n" +
                "%imageext% - The extension of the image"
            );
        }

        private void btnDownload_Click(Object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(Config.Settings.Imgur.ClientID)) {
                Log.MessageBox("ClientID has not been set up. Login and try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtAlbum.Text)) {
                txtAlbum.Focus();
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            frmImgurDownloader DownloadForm = new(new(txtAlbum.Text) {
                ImageLimit = (int)numImageLimit.Value,
                SeparateNonImages = chkSeparateNonImages.Checked,
                FileNameSchema = string.IsNullOrWhiteSpace(txtFileNameSchema.Text) ? Config.Settings.Imgur.FileNameSchema : txtFileNameSchema.Text,
            });
            DownloadForm.Show();

            Config.Settings.Imgur.ImageLimit = (int)numImageLimit.Value;
            Config.Settings.Imgur.SeparateNonImages = chkSeparateNonImages.Checked;
            Config.Settings.Imgur.FileNameSchema = txtFileNameSchema.Text;
        }

        private void txtAlbum_KeyDown(Object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Enter: {
                    //btnDownload_Click(txtAlbum, new EventArgs());
                    //e.Handled = true;
                    //e.SuppressKeyPress = true;
                } break;

                case Keys.A: {
                    switch (e.Modifiers) {
                        case Keys.Control:
                            txtAlbum.SelectAll();
                            break;

                        default:
                            System.Media.SystemSounds.Beep.Play();
                            break;
                    }
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                } break;

                case Keys.V: {
                    if (e.Modifiers == Keys.Control && Clipboard.ContainsText()) {
                        if (ApiTools.IsValidImgurLink(Clipboard.GetText())) {
                            string AlbumID = Clipboard.GetText();
                            AlbumID = AlbumID.Substring(AlbumID.IndexOf("/a/") + 3).Split('?')[0].Split('/')[0];
                            txtAlbum.Text = AlbumID;
                            System.Media.SystemSounds.Asterisk.Play();
                        }
                    }
                    else {
                        System.Media.SystemSounds.Beep.Play();
                    }
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                } break;
            }
        }
    }
}
