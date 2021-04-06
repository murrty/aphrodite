using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmUndesiredTags : Form {
        public frmUndesiredTags() {
            InitializeComponent();
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

            if (Program.UseIni) {
                Program.Ini.WriteString("UndesiredTags", undesiredBuffer.Trim(' '), "Global");
            }
            else {
                General.Default.undesiredTags = undesiredBuffer.Trim(' ');
                General.Default.Save();
            }
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
            if (string.IsNullOrEmpty(General.Default.undesiredTags)) {
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
            if (Program.UseIni) {
                if (Program.Ini.KeyExists("UndesiredTags", "Global")) {
                    undesiredTags.AddRange(Program.Ini.ReadString("UndesiredTags", "Global").Split(' '));
                }
            }
            else {
                undesiredTags.AddRange(General.Default.undesiredTags.Split(' '));
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
