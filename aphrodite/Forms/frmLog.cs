using System;
using System.Windows.Forms;

namespace aphrodite {

    public enum LogAction : int {
        WriteToLog = 0,
        InitialWriteToLog = 1,
        EnableLog = 2,
        DisableLog = 3,
        ShowLog = 4,
        WriteToLogWithInvoke = 5,
    }

    public partial class frmLog : Form {
        public bool IsShown = false;

        public frmLog() {
            InitializeComponent();
        }
        private void frmLog_Load(object sender, EventArgs e) {
            if (Config.Settings.FormSettings.frmLog_Location.X == -32000 || Config.Settings.FormSettings.frmLog_Location.Y == -32000) {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else {
                this.Location = Config.Settings.FormSettings.frmLog_Location;
            }

            if (Config.Settings.FormSettings.frmLog_Size.Width != -32000 && Config.Settings.FormSettings.frmLog_Size.Height != -32000) {
                this.Size = Config.Settings.FormSettings.frmLog_Size;
            }
        }

        [System.Diagnostics.DebuggerStepThrough]
        public void Append(string LogEntry, bool InitialMessage = false) {
            string Now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
            System.Diagnostics.Debug.Print(LogEntry);

            string Message = string.Format("[{0}] {1}", Now, LogEntry);
            switch (InitialMessage) {
                case false:
                    Message = Message = "\r\n" + Message;
                    break;
            }

            switch (InvokeRequired) {
                case true:
                    this.BeginInvoke((MethodInvoker)delegate() {
                        rtbLog.AppendText(Message);
                    });
                    break;

                case false:
                    rtbLog.AppendText(Message);
                    break;
            }
        }
        [System.Diagnostics.DebuggerStepThrough]
        public void AppendWithInvoke(string LogEntry, bool InitialMessage = false) {
            string Now = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
            System.Diagnostics.Debug.Print(LogEntry);

            string Message = string.Format("[{0}] {1}", Now, LogEntry);
            switch (InitialMessage) {
                case false:
                    Message = Message = "\r\n" + Message;
                    break;
            }

            this.Invoke((MethodInvoker)delegate() {
                rtbLog.AppendText(Message);
            });
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

    }
}
