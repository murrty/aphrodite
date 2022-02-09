using System;
using System.Windows.Forms;

namespace aphrodite {

    public partial class frmArgument : Form {

        /// <summary>
        /// The arguments sent to the form.
        /// </summary>
        public string BaseArgument {
            get;
        }

        /// <summary>
        /// The modified arguments.
        /// </summary>
        public string AppendedArgument {
            get; set;
        }

        public string[] BaseArguments {
            get; set;
        }

        public frmArgument(string Arguments, DownloadType Type) {
            InitializeComponent();

            if (Config.ValidPoint(Config.Settings.FormSettings.frmArgument_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmArgument_Location;
            }

            switch (Type) {
                case DownloadType.Tags: {
                    lbDescription.Text = string.Format(lbDescription.Text, "tags");
                    txtArguments.TextHint = "Tags here...";
                    this.Text = "tags argument check";
                }
                break;
                case DownloadType.Page: {
                    lbDescription.Text = string.Format(lbDescription.Text, "page") + "\n(new uploads may hide older images in the page)";
                    txtArguments.TextHint = "Page here...";
                    this.Text = "page argument check";
                }
                break;
                case DownloadType.Pools: {
                    lbDescription.Text = string.Format(lbDescription.Text, "pool");
                    txtArguments.TextHint = "Pool id here...";
                    this.Text = "pool argument check";
                }
                break;
                case DownloadType.Images: {
                    lbDescription.Text = string.Format(lbDescription.Text, "image");
                    txtArguments.TextHint = "Image id here...";
                    this.Text = "image argument check";
                }
                break;
                case DownloadType.FurryBooru: {
                    lbDescription.Text = string.Format(lbDescription.Text, "furrybooru");
                    txtArguments.TextHint = "Image id here...";
                    this.Text = "furrybooru argument check";
                }
                break;
                case DownloadType.InkBunny: {
                    lbDescription.Text = string.Format(lbDescription.Text, "inkbunny");
                    txtArguments.TextHint = "Image id here...";
                    this.Text = "inkbunny argument check";
                }
                break;
                case DownloadType.Imgur: {
                    lbDescription.Text = string.Format(lbDescription.Text, "imgur");
                    txtArguments.TextHint = "Image id here...";
                    this.Text = "imgur argument check";
                }
                break;
                default: {
                    lbDescription.Text = string.Format(lbDescription.Text, "uninitialized");
                    this.Text = "argument check";
                }
                break;
            }

            BaseArgument = Arguments ?? "";
            AppendedArgument = BaseArgument;
            txtArguments.Text = Arguments ?? "[CRITICAL: arguments is null]";
            txtArguments.Refresh();

            this.TopMost = Config.Settings.Initialization.ArgumentFormTopMost;
        }

        public frmArgument(string[] Arguments, DownloadType Type) {
            InitializeComponent();

            if (Config.ValidPoint(Config.Settings.FormSettings.frmArgument_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmArgument_Location;
            }

            switch (Type) {
                case DownloadType.InkBunny: {
                    lbDescription.Text = string.Format(lbDescription.Text, "tags");
                    txtArguments.TextHint = "Tags here...";
                    this.Text = "tags argument check";
                    if (Arguments.Length == 3) {
                        BaseArguments = Arguments;
                        txtArguments.Text = $"[Keywords=\"{Arguments[0] ?? "none"}\"] [artist=\"{Arguments[1] ?? "none"}\" [userfavs=\"{Arguments[2] ?? "none"}\"]";
                        txtArguments.Refresh();
                    }
                } break;
            }

            this.TopMost = Config.Settings.Initialization.ArgumentFormTopMost;
        }

        private void txtArguments_ButtonClick(object sender, EventArgs e) {
            if (txtArguments.Text != BaseArgument && Log.MessageBox("Reset arguments?", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                txtArguments.Text = BaseArgument;
            }
            txtArguments.Focus();
        }

        private void btnDownload_Click(object sender, EventArgs e) {
            AppendedArgument = txtArguments.Text.Trim();
            this.DialogResult = string.IsNullOrWhiteSpace(AppendedArgument) switch {
                true => DialogResult.No,
                false => DialogResult.Yes
            };
        }

        private void btnMainForm_Click(object sender, EventArgs e) {
            AppendedArgument = txtArguments.Text.Trim(' ');
            this.DialogResult = DialogResult.No;
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmArgumentDialog_FormClosing(Object sender, FormClosingEventArgs e) {
            if (Config.Settings.FormSettings.frmArgument_Location != this.Location) {
                Config.Settings.FormSettings.frmArgument_Location = this.Location;
                Config.Settings.FormSettings.Save();
            }
        }

        private void txtArguments_KeyPress(Object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Return) {
                if (string.IsNullOrEmpty(txtArguments.Text)) {
                    e.Handled = true;
                    txtArguments.Focus();
                    System.Media.SystemSounds.Beep.Play();
                }
                else {
                    btnDownload_Click(txtArguments, new());
                }
            }
        }
    }
}
