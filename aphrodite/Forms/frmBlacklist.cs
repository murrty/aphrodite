using System;
using System.Linq;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmBlacklist : Form {

        /// <summary>
        /// The index of where the selection beings, for modifying the lists programatically.
        /// </summary>
        private int SelectionPos;
        /// <summary>
        /// A temporary listbox for sorting the lists.
        /// </summary>
        private ListBox SortingList;

        public frmBlacklist() {
            InitializeComponent();

            if (Config.ValidPoint(Config.Settings.FormSettings.frmBlacklist_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmBlacklist_Location;
            }

            rtbGraylist.Text = Config.Settings.General.Graylist.Replace(" ", "\r\n");
            rtbBlacklist.Text = Config.Settings.General.Blacklist.Replace(" ", "\r\n");

            if (Config.Settings.UseIni) {
                this.Text += " (editing portable lists)";
            }
        }

        private void frmBlacklist_FormClosing(Object sender, FormClosingEventArgs e) {
            Config.Settings.FormSettings.frmBlacklist_Location = this.Location;
        }

        private void List_TextChanged(object sender, EventArgs e) {
            RichTextBox SentList = (RichTextBox)sender;
            SelectionPos = SentList.SelectionStart;
            SentList.Text = SentList.Text.Replace(" ", "\r\n");
            SentList.SelectionStart = SelectionPos;
        }

        private void ListSort_Click(object sender, EventArgs e) {
            RichTextBox SentList;
            try {
                SentList =
                    (Button)sender == btnSortGraylist ? rtbGraylist :
                    (Button)sender == btnSortBlacklist ? rtbBlacklist :
                    throw new NullReferenceException("Attempted to sort a list with no valid button sender.");
            }
            catch (Exception ex) {
                Log.ReportException(ex);
                return;
            }

            SelectionPos = SentList.SelectionStart;
            SentList.Text = SentList.Text.Replace(" ", "\n").Replace("\r\n", "\n").Trim();
            while (SentList.Text.Contains("\n\n")) {
                SentList.Text = SentList.Text.Replace("\n\n", "\n");
            }
            (SortingList = new()).Items.AddRange(SentList.Text.Split('\n'));
            SortingList.Sorted = true;
            SentList.Text = string.Join("\r\n", SortingList.Items.Cast<string>().ToArray());
            SentList.SelectionStart = SelectionPos;
            SentList.Focus();
            SortingList.Dispose();
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void btnSave_Click(object sender, EventArgs e) {
            Config.Settings.General.Graylist = rtbGraylist.Text.Replace(" ", "_").Replace("\r\n", " ").Replace('\n', ' ').Trim();
            Config.Settings.General.Blacklist = rtbBlacklist.Text.Replace(" ", "_").Replace("\r\n", " ").Replace('\n', ' ').Trim();
            Config.Settings.General.Save();
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
        }

        private void llGraylist_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Log.MessageBox("Tags in this category are downloaded into their own separate directory by default. Use this category for tags you might enjoy on a rare occasion, or just want to be separated out.");
        }

        private void llBlacklist_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Log.MessageBox("Tags in this category are not downloaded by default. Use this category for tags you never want to see, no matter what, not even if hell freezes over with semen.");
        }

    }
}
