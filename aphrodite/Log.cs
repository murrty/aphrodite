using System;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Windows.Forms;

using murrty.forms;

namespace aphrodite {
    public sealed class Log {

        private static volatile frmLog LogForm = null;

        private static volatile bool LogEnabled = false;

        public static string ComputerVersionInformation {
            get; private set;
        } = "Not initialized.";

        #region Log stuff
        public static void InitializeLogging() {
            EnableLogging();
            Write("Creating ComputerVersionInformation for exceptions.");
            ManagementObjectSearcher searcher = new("SELECT * FROM Win32_OperatingSystem");
            ManagementObject info = searcher.Get().Cast<ManagementObject>().FirstOrDefault();
            ComputerVersionInformation =
                $"System Caption: {info.Properties["Caption"].Value ?? "couldn't query"}\n" +
                $"Version: {info.Properties["Version"].Value ?? "couldn't query"}\n" +
                $"Service Pack Major: {info.Properties["ServicePackMajorVersion"].Value ?? "couldn't query"}\n" +
                $"Service Pack Minor: {info.Properties["ServicePackMinorVersion"].Value ?? "couldn't query"}";

            Write("Creating unhandled exception event.");
            AppDomain.CurrentDomain.UnhandledException += (sender, exception) => {
                using frmException UnrecoverableException = new(new(exception) {
                    Unrecoverable = true
                });
                UnrecoverableException.ShowDialog();
            };
            Write("Creating unhandled thread exception event.");
            Application.ThreadException += (sender, exception) => {
                using frmException UnrecoverableException = new(new(exception) {
                    Unrecoverable = true
                });
                UnrecoverableException.ShowDialog();
            };
        }

        /// <summary>
        /// Enables the logging form.
        /// </summary>
        [DebuggerStepThrough]
        public static void EnableLogging() {
            if (LogEnabled && LogForm != null) {
                Write("Logging is already enabled.");
            }
            else {
                LogForm = new frmLog();
                LogEnabled = true;
                Write("Logging has been enabled.", true);
            }
        }

        /// <summary>
        /// Disables the logging form, but debug logging will still occur.
        /// </summary>
        [DebuggerStepThrough]
        public static void DisableLogging() {
            if (LogEnabled) {
                Write("Disabling logging.");

                if (LogForm.WindowState == FormWindowState.Minimized || LogForm.WindowState == FormWindowState.Maximized) {
                    LogForm.Opacity = 0;
                    LogForm.WindowState = FormWindowState.Normal;
                }

                LogForm?.Dispose();
                LogEnabled = false;
            }
        }

        /// <summary>
        /// Shows the log form.
        /// </summary>
        [DebuggerStepThrough]
        public static void ShowLog() {
            if (LogEnabled && LogForm != null) {
                if (LogForm.IsShown) {
                    Write("The log form is already shown.");
                    LogForm.Activate();
                }
                else {
                    LogForm.Append("Showing log");
                    LogForm.IsShown = true;
                    LogForm.Show();
                }
            }
        }

        /// <summary>
        /// Writes a message to the log.
        /// </summary>
        /// <param name="message">The message to be sent to the log</param>
        /// <param name="initial">If the message is the initial one to be sent, does not add a new line break.</param>
        [DebuggerStepThrough]
        public static void Write(string message, bool initial = false) {
            Debug.Print(message);
            if (LogEnabled) {
                LogForm?.Append(message, initial);
            }
        }
        #endregion

        #region Exception handling
        public static DialogResult ReportException(dynamic ReceivedException, object ExtraInfo = null) {
            if (ReceivedException is Exception RecEx) {
                Write($"An {RecEx.GetType().Name} occurred.");
            }
            else {
                Write("An exception occurred.");
            }
            using frmException NewException = new(new(ReceivedException) {
                AllowRetry = false,
                ExtraInfo = ExtraInfo
            });
            return NewException.ShowDialog();
        }

        public static DialogResult ReportRetriableException(dynamic ReceivedException, object ExtraInfo = null) {
            if (ReceivedException is Exception RecEx) {
                Write($"An {RecEx.GetType().Name} occurred.");
            }
            else {
                Write("An exception occurred.");
            }
            using frmException NewException = new(new(ReceivedException) {
                AllowRetry = true,
                ExtraInfo = ExtraInfo
            });
            return NewException.ShowDialog();
        }
        #endregion

        #region Message box
        /// <summary>
        /// Displays a message dialog with a pre-defined caption.
        /// </summary>
        /// <param name="Message">The message that will be displayed in the dialog.</param>
        /// <returns>The dialog result of the message box.</returns>
        public static DialogResult MessageBox
        (string Message) =>
            System.Windows.Forms.MessageBox.Show(Message, "aphrodite");

        /// <summary>
        /// Displays a message dialog with a pre-defined caption.
        /// </summary>
        /// <param name="Message">The message that will be displayed in the dialog.</param>
        /// <param name="Buttons">The buttons that will appear within the dialog.</param>
        /// <returns>The dialog result of the message box.</returns>
        public static DialogResult MessageBox
        (string Message, MessageBoxButtons Buttons = MessageBoxButtons.OK) =>
            System.Windows.Forms.MessageBox.Show(Message, "aphrodite", Buttons);

        /// <summary>
        /// Displays a message dialog with a pre-defined caption.
        /// </summary>
        /// <param name="Message">The message that will be displayed in the dialog.</param>
        /// <param name="Buttons">The buttons that will appear within the dialog.</param>
        /// <param name="Icon">The icon that will appear before the message in the dialog.</param>
        /// <returns>The dialog result of the message box.</returns>
        public static DialogResult MessageBox
        (string Message, MessageBoxButtons Buttons, MessageBoxIcon Icon) =>
            System.Windows.Forms.MessageBox.Show(Message, "aphrodite", Buttons, Icon);

        /// <summary>
        /// Displays a message dialog with a pre-defined caption.
        /// </summary>
        /// <param name="Message">The message that will be displayed in the dialog.</param>
        /// <param name="Buttons">The buttons that will appear within the dialog.</param>
        /// <param name="DefaultButton">The default button that will be focused when the dialog appears.</param>
        /// <returns>The dialog result of the message box.</returns>
        public static DialogResult MessageBox
        (string Message, MessageBoxButtons Buttons, MessageBoxDefaultButton DefaultButton) =>
            System.Windows.Forms.MessageBox.Show(Message, "aphrodite", Buttons, MessageBoxIcon.None, DefaultButton);

        /// <summary>
        /// Displays a message dialog with a pre-defined caption.
        /// </summary>
        /// <param name="Message">The message that will be displayed in the dialog.</param>
        /// <param name="Buttons">The buttons that will appear within the dialog.</param>
        /// <param name="DefaultButton">The default button that will be focused when the dialog appears.</param>
        /// <param name="Icon">The icon that will appear before the message in the dialog.</param>
        /// <returns>The dialog result of the message box.</returns>
        public static DialogResult MessageBox
        (string Message, MessageBoxButtons Buttons, MessageBoxIcon Icon, MessageBoxDefaultButton DefaultButton) =>
            System.Windows.Forms.MessageBox.Show(Message, "aphrodite", Buttons, Icon, DefaultButton);
        #endregion
    }
}