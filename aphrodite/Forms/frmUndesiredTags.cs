using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmUndesiredTags : Form {
        public frmUndesiredTags() {
            InitializeComponent();
        }

        private void frmUndesiredTags_Load(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(Settings.Default.undesiredTags)) {
                listTags.Items.AddRange(UndesiredTags.HardcodedUndesiredTags);
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            string undesiredBuffer = string.Empty;
            for (int i = 0; i < listTags.Items.Count; i++) {
                undesiredBuffer += listTags.GetItemText(listTags.Items[i]) + " ";
            }

            Settings.Default.undesiredTags = undesiredBuffer.TrimEnd(' ');
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
            if (string.IsNullOrEmpty(Settings.Default.undesiredTags)) {
                listTags.Items.AddRange(UndesiredTags.HardcodedUndesiredTags);
            }
        }
    }
}
