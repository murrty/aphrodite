using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmUndesiredTags : Form {
        public frmUndesiredTags() {
            InitializeComponent();
        }

        private void frmUndesiredTags_Load(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(Config.Settings.General.undesiredTags)) {
                listTags.Items.AddRange(UndesiredTags.HardcodedUndesiredTags);
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            string undesiredBuffer = string.Empty;
            for (int i = 0; i < listTags.Items.Count; i++) {
                undesiredBuffer += listTags.GetItemText(listTags.Items[i]) + " ";
            }

            Config.Settings.General.undesiredTags = undesiredBuffer.Trim(' ');
            Config.Settings.General.Save();
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.Dispose();
        }

        private void btnRemove_Click(object sender, EventArgs e) {
            listTags.Items.RemoveAt(listTags.SelectedIndex);
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            listTags.Items.Add(txtUndesired.Text.Replace(' ', '_'));
        }

        private void btnReset_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(Config.Settings.General.undesiredTags)) {
                listTags.Items.AddRange(UndesiredTags.HardcodedUndesiredTags);
            }
        }
    }

    class UndesiredTags {
        public static readonly string[] HardcodedUndesiredTags = { "avoid_posting", "conditional_dnp", "sound_warning", "unknown_artist_signature" };

        /// <summary>
        /// Determine if a tag is undesired
        /// </summary>
        /// <param name="tag">The tag string</param>
        /// <returns></returns>
        public static bool isUndesiredHardcoded(string tag) {
            switch (string.IsNullOrWhiteSpace(tag)) {
                case true:
                    return false;
            }

            for (int i = 0; i < HardcodedUndesiredTags.Length; i++) {
                if (tag == HardcodedUndesiredTags[i]) {
                    return true;
                }
            }

            return false;
        }

        public static bool isUndesired(string tag) {
            switch (string.IsNullOrWhiteSpace(tag)) {
                case true:
                    return false;
            }

            List<string> undesiredTags = new List<string>();
            if (Config.Settings.General.undesiredTags.Length > 0) {
                undesiredTags.AddRange(Config.Settings.General.undesiredTags.Split(' '));
            }

            if (undesiredTags.Count > 0) {
                for (int i = 0; i < undesiredTags.Count; i++) {
                    if (tag == undesiredTags[i]) {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
