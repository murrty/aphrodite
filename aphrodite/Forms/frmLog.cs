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

        public void Append(string LogEntry, bool InitialMessage = false) {
            string Now = DateTime.Now.Year.ToString("0000.##") + "/" + DateTime.Now.Month.ToString("00.##") + "/" + DateTime.Now.Day.ToString("00.##") + " " + DateTime.Now.Hour.ToString("00.##") + ":" + DateTime.Now.Minute.ToString("00.##") + ":" + DateTime.Now.Second.ToString("00.##") + "." + DateTime.Now.Millisecond.ToString("000.##");
            System.Diagnostics.Debug.Print(LogEntry);
            switch (InitialMessage) {
                case false:
                    rtbLog.AppendText(string.Format("\r\n[{0}] {1}", Now, LogEntry));
                    break;
                case true:
                    rtbLog.AppendText(string.Format("[{0}] {1}", Now, LogEntry));
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
