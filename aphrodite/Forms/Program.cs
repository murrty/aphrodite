using System;
using System.Net;
using System.Security.Principal;
using System.Windows.Forms;

namespace aphrodite {
    static class Program {
        public static readonly string UserAgent = "User-Agent: aphrodite/" + (Properties.Settings.Default.currentVersion) + " (Contact: https://github.com/murrty/aphrodite ... open an issue)";
        public static volatile bool IsDebug = false;
        public static volatile bool IsAdmin = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
#if DEBUG
    IsDebug = true;
#else
    IsDebug = false;
#endif
            if (!TaskbarProgress.Windows7OrGreater) {
                MessageBox.Show("Windows 7 or higher is required to run this application.");
                Environment.Exit(0);
            }
            else {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if ((new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator)){
                    IsAdmin = true;
                }
                else {
                    IsAdmin = false;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                if (IsDebug) {
                    //ErrorLog.ReportCustomException("Test", "Program.cs");
                }

                if (Settings.Default.firstTime) {
                    MessageBox.Show(
                        "This is your first time running aphrodite, so read this before continuing:\n\n" +
                        "This program is \"advertised\" as a porn downloader. You don't have to download any 18+ material using it, but it's emphasised on porn.\n" +
                        "As soon as you get access into the program, change the settings and set your prefered file name schema. I don't want to be responsible for any excess files being downloaded.\n\n" +
                        "Just so you know."
                    );
                    Settings.Default.firstTime = false;
                    Settings.Default.Save();
                }

                Application.Run(new frmMain());
            }
        }
    }
}
