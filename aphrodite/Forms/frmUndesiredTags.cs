using System;
using System.Linq;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmUndesiredTags : Form {

        public frmUndesiredTags() {
            InitializeComponent();

            if (Config.ValidPoint(Config.Settings.FormSettings.frmUndesiredTags_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmUndesiredTags_Location;
            }

            if (!string.IsNullOrEmpty(Config.Settings.General.undesiredTags)) {
                listTags.Items.AddRange(UndesiredTags.HardcodedUndesiredTags);
            }
        }

        private void frmUndesiredTags_FormClosing(object sender, FormClosingEventArgs e) {
            Config.Settings.FormSettings.frmUndesiredTags_Location = this.Location;
        }

        private void btnRemove_Click(object sender, EventArgs e) {
            for (int i = listTags.SelectedIndices.Count - 1; i > -1; i--) {
                listTags.Items.RemoveAt(listTags.SelectedIndices[i]);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtUndesired.Text)) {
                txtUndesired.Focus();
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }
            listTags.Items.Add(txtUndesired.Text.Replace(' ', '_'));
        }

        private void txtUndesired_KeyPress(Object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Return) {
                btnAdd_Click(txtUndesired, null);
            }
        }

        private void txtUndesired_TextChanged(object sender, EventArgs e) {
            int curPos = txtUndesired.SelectionStart;
            txtUndesired.Text = txtUndesired.Text.Replace(" ", "_");
            txtUndesired.SelectionStart = curPos;
        }

        private void btnSave_Click(object sender, EventArgs e) {
            Config.Settings.General.undesiredTags = string.Join(" ", listTags.Items.Cast<string>().ToArray()).Trim();
            Config.Settings.General.Save();
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Dispose();
        }

        private void btnReset_Click(object sender, EventArgs e) {
            listTags.Items.Clear();
            listTags.Items.AddRange(UndesiredTags.HardcodedUndesiredTags);
        }
    }
}
