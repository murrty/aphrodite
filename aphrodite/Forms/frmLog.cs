using System;
using System.Windows.Forms;

namespace murrty.forms {
    public partial class frmLog : Form {

        public bool IsShown {
            get; set;
        } = false;

        public frmLog() {
            InitializeComponent();
        }

        private void frmLog_Load(object sender, EventArgs e) {
            if (aphrodite.Config.ValidPoint(aphrodite.Config.Settings.FormSettings.frmLog_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = aphrodite.Config.Settings.FormSettings.frmLog_Location;
            }

            if (aphrodite.Config.ValidSize(aphrodite.Config.Settings.FormSettings.frmLog_Size)) {
                this.Size = aphrodite.Config.Settings.FormSettings.frmLog_Size;
            }
        }

        private void frmLog_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            this.Hide();
            IsShown = false;
        }

        private void btnClear_Click(object sender, EventArgs e) {
            rtbLog.Clear();
            Append("Log has been cleared", true);
        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.Hide();
            IsShown = false;
        }

        /// <summary>
        /// Appends text to the log.
        /// </summary>
        /// <param name="message">The message to append.</param>
        /// <param name="initial">Whether the message is the first message of the log.</param>
        [System.Diagnostics.DebuggerStepThrough]
        public void Append(string message, bool initial = false) {
            if (rtbLog.InvokeRequired) {
                rtbLog.Invoke((Action)delegate () {
                    Append(message, initial);
                });
            }
            else {
                rtbLog.AppendText(
                    $"{(initial ? "" : "\n")}[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}] {message}"
                );
            }
        }

        /// <summary>
        /// Appends text to the log, not including date/time of the message.
        /// </summary>
        /// <param name="message">The message to append.</param>
        /// <param name="initial">Whether the message is the first message of the log.</param>
        [System.Diagnostics.DebuggerStepThrough]
        public void AppendNoDate(string message, bool initial = false) {
            if (rtbLog.InvokeRequired) {
                rtbLog.Invoke((Action)delegate () {
                    Append(message, initial);
                });
            }
            else {
                rtbLog.AppendText(
                    $"{(initial ? "" : "\n")}{message}"
                );
            }
        }

    }
}
