﻿using System;
using System.Net;
using System.Windows.Forms;

namespace aphrodite {

    public partial class frmException : Form {
        public Exception ReportedException = null;
        public WebException ReportedWebException = null;
        public string WebAddress = string.Empty;
        public int WebErrorCode = -1;
        public bool SetCustomDescription = false;
        public string CustomDescription = null;
        //Language lang = Language.GetInstance();
        public bool FromLanguage = false;
        public bool AllowRetry = false;

        public frmException() {
            InitializeComponent();
            //loadLanguage();
            DateTime TimeNow = DateTime.Now;
            lbDate.Text = string.Format("{0}/{1}/{2} {3}:{4}:{5}", TimeNow.Year, TimeNow.Month, TimeNow.Day, TimeNow.Hour, TimeNow.Minute, TimeNow.Second);
        }

        void loadLanguage() {
            //if (FromLanguage) {
            //    this.Text = Language.InternalEnglish.frmException;
            //    lbExceptionHeader.Text = Language.InternalEnglish.lbExceptionHeader;
            //    lbExceptionDescription.Text = Language.InternalEnglish.lbExceptionDescription;
            //    rtbExceptionDetails.Text = Language.InternalEnglish.rtbExceptionDetails;
            //    btnExceptionGithub.Text = Language.InternalEnglish.btnExceptionGithub;
            //    btnExceptionOk.Text = Language.InternalEnglish.btnExceptionOk;
            //}
            //else {
            //    this.Text = lang.frmException;
            //    lbExceptionHeader.Text = lang.lbExceptionHeader;
            //    lbExceptionDescription.Text = lang.lbExceptionDescription;
            //    lbExceptionDescription.Text = lang.lbExceptionDescription;
            //    btnExceptionGithub.Text = lang.btnExceptionGithub;
            //    btnExceptionOk.Text = lang.btnExceptionOk;
            //}
        }

        private void frmException_Load(object sender, EventArgs e) {
            string Exception = string.Empty;
            if (ReportedException != null) {
                Exception += "An exception occured" + "\n";
                Exception += "Message: " + ReportedException.Message + "\n";
                Exception += "Stacktrace: " + ReportedException.StackTrace + "\n";
                Exception += "Source: " + ReportedException.Source + "\n";
                Exception += "Target Site: " + ReportedException.TargetSite + "\n";


                Exception += "Full report:\n" + ReportedException.ToString();
            }
            else if (ReportedWebException != null) {
                Exception += "A web exception occured" + "\n";
                Exception += "Message: " + ReportedWebException.Message + "\n";
                Exception += "Stacktrace: " + ReportedWebException.StackTrace + "\n";
                Exception += "Source: " + ReportedWebException.Source + "\n";
                Exception += "Target Site: " + ReportedWebException.TargetSite + "\n";
                Exception += "Inner Exception: " + ReportedWebException.InnerException + "\n";
                Exception += "Response: " + ReportedWebException.Response + "\n";
                Exception += "Web Address: " + WebAddress + "\n";


                Exception += "Full report:\n" + ReportedWebException.ToString();
            }
            else if (CustomDescription != null) {
                rtbExceptionDetails.Text = CustomDescription;
            }
            else {
                Exception = "An exception occured, but it didn't parse properly.\nCreate a new issue and tell me how you got here.";
            }

            string outputBuffer = "\n\nVersion: {0}\n" + Exception;
            if (Properties.Settings.Default.IsBetaVersion) {
                outputBuffer = string.Format(outputBuffer, Properties.Settings.Default.BetaVersion);
                lbVersion.Text = "v" + Properties.Settings.Default.BetaVersion;
            }
            else {
                outputBuffer = string.Format(outputBuffer, Properties.Settings.Default.CurrentVersion.ToString());
                lbVersion.Text = "v" + Properties.Settings.Default.CurrentVersion.ToString();
            }

            if (AllowRetry) {
                btnExceptionRetry.Enabled = true;
            }

            rtbExceptionDetails.Text += outputBuffer;
            System.Media.SystemSounds.Hand.Play();
        }

        private void btnExceptionOk_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

        private void btnExceptionGithub_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("https://github.com/murrty/aphrodite/issues");
        }

        private void btnExceptionRetry_Click(object sender, EventArgs e) {
            if (AllowRetry) {
                this.DialogResult = DialogResult.Retry;
                this.Dispose();
            }
        }
    }
}
