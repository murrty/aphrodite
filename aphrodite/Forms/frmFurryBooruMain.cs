using System;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmFurryBooruMain : Form {

        public frmFurryBooruMain(string Argument = null) {
            InitializeComponent();

            if (Config.ValidPoint(Config.Settings.FormSettings.frmFurryBooruMain_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmFurryBooruMain_Location;
            }

            txtTags.Text = Argument ?? "";
        }

        private void frmFurryBooruMain_FormClosing(Object sender, FormClosingEventArgs e) {
            Config.Settings.FormSettings.frmFurryBooruMain_Location = this.Location;
        }

        private void txtTags_ButtonClick(object sender, EventArgs e) {
            txtTags.Clear();
            txtTags.Focus();
        }

        private void btnDownload_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtTags.Text)) {
                txtTags.Focus();
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            txtTags.Text = txtTags.Text.Trim();
            frmTagDownloader DownloadForm = new(new(txtTags.Text, DownloadSite.FurryBooru));
            DownloadForm.Show();
        }

        private void txtTags_KeyPress(Object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Return) {
                btnDownload_Click(txtTags, null);
            }
        }
    }
}
