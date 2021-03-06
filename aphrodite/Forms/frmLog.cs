﻿using System;
using System.Windows.Forms;

namespace aphrodite {

    public enum LogAction : int {
        WriteToLog = 0,
        InitialWriteToLog = 1,
        EnableLog = 2,
        DisableLog = 3,
        ShowLog = 4
    }

    public partial class frmLog : Form {
        public bool IsShown = false;

        public frmLog() {
            InitializeComponent();

            //if (Config.Default.LogFormLocation.X == -32000 || Config.Default.LogFormLocation.Y == -32000) {
            //    this.StartPosition = FormStartPosition.CenterScreen;
            //}
            //else {
            //    this.Location = Config.Default.LogFormLocation;
            //}

            //this.Size = Config.Default.LogFormSize;
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
