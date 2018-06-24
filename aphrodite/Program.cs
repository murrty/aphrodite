﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aphrodite {
    static class Program {
        public static readonly string UserAgent = "User-Agent: aphrodite/" + (Properties.Settings.Default.currentVersion) + " (Contact: https://github.com/murrty/aphrodite ... open an issue)";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            if (!TaskbarProgress.Windows7OrGreater) {
                MessageBox.Show("Windows 7 is required to run this application.");
                Environment.Exit(0);
            }
            else {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
        }
    }
}
