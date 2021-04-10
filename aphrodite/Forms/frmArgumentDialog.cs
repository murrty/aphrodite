using System;
using System.Windows.Forms;

namespace aphrodite {

    public partial class frmArgumentDialog : Form {
        public string AppendedArgument;
        public frmArgumentDialog(string Arguments, DownloadType Type) {
            InitializeComponent();
            switch (Type) {
                case DownloadType.Tags:
                    lbDescription.Text = string.Format(lbDescription.Text, "tags");
                    txtArguments.TextHint = "Tags here...";
                    this.Text = "tags argument check";
                    break;
                case DownloadType.Pools:
                    lbDescription.Text = string.Format(lbDescription.Text, "pool");
                    txtArguments.TextHint = "Pool id here...";
                    this.Text = "pool argument check";
                    break;
                case DownloadType.Images:
                    lbDescription.Text = string.Format(lbDescription.Text, "image");
                    txtArguments.TextHint = "Image id here...";
                    this.Text = "image argument check";
                    break;
                default:
                    lbDescription.Text = string.Format(lbDescription.Text, "uninitialized");
                    this.Text = "argument check";
                    break;
            }
            txtArguments.Text = Arguments;
            AppendedArgument = Arguments;
            txtArguments.Refresh();
        }

        private void txtArguments_ButtonClick(object sender, EventArgs e) {
            if (!string.IsNullOrWhiteSpace(txtArguments.Text)) {
                switch (MessageBox.Show("Clear arguments?", "aphrodite", MessageBoxButtons.YesNo)) {
                    case DialogResult.Yes:
                        txtArguments.Clear();
                        break;
                }
            }
        }

        private void btnDownload_Click(object sender, EventArgs e) {
            AppendedArgument = txtArguments.Text.Trim(' ');
            if (string.IsNullOrWhiteSpace(AppendedArgument)) {
                this.DialogResult = DialogResult.No;
            }
            else {
                this.DialogResult = DialogResult.Yes;
            }
        }

        private void btnMainForm_Click(object sender, EventArgs e) {
            AppendedArgument = txtArguments.Text.Trim(' ');
            this.DialogResult = DialogResult.No;
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
