using System;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmUndesiredTags : Form {
        public frmUndesiredTags() {
            InitializeComponent();
            this.Icon = Properties.Resources.Brad;
        }

        private void frmUndesiredTags_Load(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(General.Default.undesiredTags)) {
                listTags.Items.AddRange(UndesiredTags.HardcodedUndesiredTags);
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            string undesiredBuffer = string.Empty;
            for (int i = 0; i < listTags.Items.Count; i++) {
                undesiredBuffer += listTags.GetItemText(listTags.Items[i]) + " ";
            }

            General.Default.undesiredTags = undesiredBuffer.TrimEnd(' ');
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Dispose();
        }

        private void btnRemove_Click(object sender, EventArgs e) {
            listTags.Items.RemoveAt(listTags.SelectedIndex);
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            listTags.Items.Add(txtUndesired.Text);
        }

        private void btnReset_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(General.Default.undesiredTags)) {
                listTags.Items.AddRange(UndesiredTags.HardcodedUndesiredTags);
            }
        }
    }
}
