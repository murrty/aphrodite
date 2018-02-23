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
    public partial class frmSettings : Form {
        #region Variables
        public bool pluginChange = false;
        #endregion

        public frmSettings() {
            InitializeComponent();
        }
        private void frmSettings_Load(object sender, EventArgs e) {
            loadSettings();
        }

        private void saveSettings() {
            // General
            Settings.Default.saveLocation = txtSaveTo.Text;
            Settings.Default.saveInfo = chkSaveInfo.Checked;
            Settings.Default.saveBlacklisted = chkSaveBlacklisted.Checked;

            // Tags
            Tags.Default.Explicit = chkExplicit.Checked;
            Tags.Default.Questionable = chkQuestionable.Checked;
            Tags.Default.Safe = chkSafe.Checked;
            Tags.Default.separateRatings = chkSeparate.Checked;
            Tags.Default.enableScoreMin = chkMinimumScore.Checked;
            Tags.Default.scoreMin = Convert.ToInt32(numScore.Value);
            Tags.Default.imageLimit = Convert.ToInt32(numLimit.Value);

            // Pools
            Pools.Default.usePoolName = chkPoolName.Checked;
            Pools.Default.mergeBlacklisted = chkMerge.Checked;
            Pools.Default.openAfter = chkOpen.Checked;

            // Save all 3
            Settings.Default.Save();
            Tags.Default.Save();
            Pools.Default.Save();
        }
        private void loadSettings() {
            // General
            txtSaveTo.Text = Settings.Default.saveLocation;
            chkSaveInfo.Checked = Settings.Default.saveInfo;
            chkSaveBlacklisted.Checked = Settings.Default.saveBlacklisted;

            // Tags
            chkExplicit.Checked = Tags.Default.Explicit;
            chkQuestionable.Checked = Tags.Default.Questionable;
            chkSafe.Checked = Tags.Default.Safe;
            chkSeparate.Checked = Tags.Default.separateRatings;
            chkMinimumScore.Checked = Tags.Default.enableScoreMin;
            numScore.Value = Convert.ToDecimal(Tags.Default.scoreMin);
            numLimit.Value = Convert.ToDecimal(Tags.Default.imageLimit);

            // Pools
            chkPoolName.Checked = Pools.Default.usePoolName;
            chkMerge.Checked = Pools.Default.mergeBlacklisted;
            chkOpen.Checked = Pools.Default.openAfter;
        }
        private void btnBrws_Click(object sender, EventArgs e) {
            FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select a folder to store downloads" };
            if (!string.IsNullOrEmpty(txtSaveTo.Text))
                fbd.SelectedPath = txtSaveTo.Text;

            if (fbd.ShowDialog() == DialogResult.OK) 
                txtSaveTo.Text = fbd.SelectedPath;
        }      
        private void btnSave_Click(object sender, EventArgs e) {
            saveSettings();
            if (pluginChange)
                Environment.Exit(0);
            this.DialogResult = DialogResult.OK;
        }
        private void btnCancel_Click(object sender, EventArgs e) {
            if (pluginChange)
                Environment.Exit(0);
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnBlacklist_Click(object sender, EventArgs e) {
            frmBlacklist blackList = new frmBlacklist();
            if (blackList.ShowDialog() == DialogResult.OK) {
                Settings.Default.Save();
            }
        }

    }
}
