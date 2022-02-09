/// ExceptionForm Base 1.0.5

using System;
using System.Drawing;
using System.Net;

using aphrodite;
using murrty.classes;
using murrty.classcontrols;

namespace murrty.forms {

    public partial class frmException : ExtendedForm {

        #region Fields & Properties

        /// <summary>
        /// The constant string of the URL to the github issues page.
        /// </summary>
        private const string GithubPage = "https://github.com/murrty/aphrodite";

        /// <summary>
        /// The private ExceptionInfo holding information about the exception.
        /// </summary>
        private ExceptionInfo ReportedException { get; }

        private readonly DwmComposition DwmCompositor;
        private readonly DwmCompositionInfo DwmInfo;

        #endregion

        #region Form Events

        public frmException(ExceptionInfo ReportedException) {
            this.ReportedException = ReportedException;

            InitializeComponent();
            LoadLanguage();

            // Check if there is a valid github link
            if (string.IsNullOrWhiteSpace(GithubPage)) {
                btnExceptionGithub.Enabled = false;
                btnExceptionGithub.Visible = false;
                // since there's no github page, we're gonna move the reported date/time.
                lbDate.Location = new(btnExceptionRetry.Location.X - 119, lbDate.Location.Y);
            }
            else {
                btnExceptionGithub.Enabled = true;
                btnExceptionGithub.Visible = true;
            }

            // Check if allow retry.
            if (!ReportedException.AllowRetry) {
                btnExceptionRetry.Enabled = false;
                btnExceptionRetry.Visible = false;

                // Since the retry button isn't allowed, we need to check the github button
                if (string.IsNullOrWhiteSpace(GithubPage)) {
                    // We move it directly next to the OK button.
                    lbDate.Location = new(btnExceptionOk.Location.X - 119, lbDate.Location.Y);
                }
                else {
                    // We move stuff one point to the right.
                    btnExceptionGithub.Location = btnExceptionRetry.Location;
                    lbDate.Location = new(btnExceptionGithub.Location.X - 119, lbDate.Location.Y);
                }
            }
            else {
                btnExceptionRetry.Enabled = true;
                btnExceptionRetry.Visible = true;
            }

            // Add the date
            lbDate.Text = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";

            if (DwmComposition.CompositionSupported) {
                DwmCompositor = new();
                DwmInfo = new(
                    this.Handle,
                    new() {
                        m_Top = pnDWM.Height,
                        m_Bottom = 0,
                        m_Left = 0,
                        m_Right = 0
                    },
                    new(
                        pnDWM.Location.X,
                        pnDWM.Location.Y,
                        this.MaximumSize.Width,
                        pnDWM.Size.Height
                    ),
                    new(
                        lbExceptionHeader.Text,
                        lbExceptionHeader.Font,
                        Color.FromKnownColor(KnownColor.ActiveCaptionText),
                        10,
                        new(
                            lbExceptionHeader.Location.X,
                            lbExceptionHeader.Location.Y,
                            lbExceptionHeader.Size.Width,
                            lbExceptionHeader.Size.Height
                        )
                    )
                );

                pnDWM.Visible = false;
                lbExceptionHeader.Visible = false;
                DwmCompositor.ExtendFrame(DwmInfo);
                this.Paint += (s, e) => {
                    DwmCompositor.FillBlackRegion(DwmInfo);
                    DwmCompositor.DrawTextOnGlass(DwmInfo, DwmInfo.Text);
                };
                this.MouseDown += (s, e) => {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left && DwmInfo.DwmRectangle.Contains(e.Location)) {
                        DwmCompositor.MoveForm(DwmInfo);
                    }
                };
            }
            else {
                lbExceptionHeader.Visible = true;
                pnDWM.BackColor = Color.FromKnownColor(KnownColor.Menu);
                pnDWM.Visible = true;
            }

            this.ShownSound = System.Media.SystemSounds.Hand;
        }

        private void frmError_Load(object sender, EventArgs e) {
            // We need to figure out what exception occurred.
            // If the custom description is null, we can generate one.
            if (ReportedException.CustomDescription is null) {
                switch (ReportedException.ReceivedException) {
                    case UnhandledExceptionEventArgs UnEx: {
                        rtbExceptionDetails.Text = "An unhandled exception occurred.\n" +
                                                  $"The application will exit after closing this dialog.\n\n" +
                                                  $"{(ReportedException.ExtraMessage is not null ? $"{ReportedException.ExtraMessage}\n\n" : "")}" +
                                                  $"{UnEx.ExceptionObject}\n";
                    } break;

                    case System.Threading.ThreadExceptionEventArgs ThrEx: {
                        rtbExceptionDetails.Text = "An unhandled thread exception occurred.\n" +
                                                  $"The application will exit after closing this dialog.\n\n" +
                                                  $"{(ReportedException.ExtraMessage is not null ? $"{ReportedException.ExtraMessage}\n\n" : "")}" +
                                                  $"{ThrEx.Exception}\n";
                    } break;

                    case ApiReturnedNullOrEmptyException ApRtNuEmpEx: {
                        rtbExceptionDetails.Text = (ReportedException.Unrecoverable ? "An unrecoverable ApiReturnedNullOrEmptyException occurred, and the application will exit." : "A caught ApiReturnedNullOrEmptyException occurred.") + "\n\n" +
                                                  $"This exception may have been pushed here on accident.\n" +
                                                  $"{(ReportedException.ExtraMessage is not null ? $"{ReportedException.ExtraMessage}\n\n" : "")}" +
                                                  $"Message: {ApRtNuEmpEx.Message}\n" +
                                                  $"Stacktrace: {ApRtNuEmpEx.StackTrace}\n" +
                                                  $"Source: {ApRtNuEmpEx.Source}\n" +
                                                  $"Target Site: {ApRtNuEmpEx.TargetSite}\n" +
                                                  $"Inner Exception: {ApRtNuEmpEx.InnerException}\n";
                    } break;

                    case ImageWasNullAfterBypassingException ImNuBypEx: {
                        rtbExceptionDetails.Text = (ReportedException.Unrecoverable ? "An unrecoverable ImageWasNullAfterBypassingException occurred, and the application will exit." : "A caught ImageWasNullAfterBypassingException occurred.") + "\n\n" +
                                                  $"This exception may have been pushed here on accident.\n" +
                                                  $"{(ReportedException.ExtraMessage is not null ? $"{ReportedException.ExtraMessage}\n\n" : "")}" +
                                                  $"Message: {ImNuBypEx.Message}\n" +
                                                  $"Stacktrace: {ImNuBypEx.StackTrace}\n" +
                                                  $"Source: {ImNuBypEx.Source}\n" +
                                                  $"Target Site: {ImNuBypEx.TargetSite}\n" +
                                                  $"Inner Exception: {ImNuBypEx.InnerException}\n";
                    } break;

                    case NoFilesToDownloadException NoDlEx: {
                        rtbExceptionDetails.Text = (ReportedException.Unrecoverable ? "An unrecoverable NoFilesToDownloadException occurred, and the application will exit." : "A caught NoFilesToDownloadException occurred.") + "\n\n" +
                                                  $"This exception may have been pushed here on accident.\n" +
                                                  $"{(ReportedException.ExtraMessage is not null ? $"{ReportedException.ExtraMessage}\n\n" : "")}" +
                                                  $"Message: {NoDlEx.Message}\n" +
                                                  $"Stacktrace: {NoDlEx.StackTrace}\n" +
                                                  $"Source: {NoDlEx.Source}\n" +
                                                  $"Target Site: {NoDlEx.TargetSite}\n" +
                                                  $"Inner Exception: {NoDlEx.InnerException}\n";
                    } break;

                    case PoolOrPostWasDeletedException PwdEx: {
                        rtbExceptionDetails.Text = (ReportedException.Unrecoverable ? "An unrecoverable PoolOrPostWasDeletedException occurred, and the application will exit." : "A caught PoolOrPostWasDeletedException occurred.") + "\n\n" +
                                                  $"This exception may have been pushed here on accident.\n" +
                                                  $"{(ReportedException.ExtraMessage is not null ? $"{ReportedException.ExtraMessage}\n\n" : "")}" +
                                                  $"Message: {PwdEx.Message}\n" +
                                                  $"Stacktrace: {PwdEx.StackTrace}\n" +
                                                  $"Source: {PwdEx.Source}\n" +
                                                  $"Target Site: {PwdEx.TargetSite}\n" +
                                                  $"Inner Exception: {PwdEx.InnerException}\n";
                    } break;

                    case System.Threading.ThreadAbortException ThrAbrEx: {
                        rtbExceptionDetails.Text = (ReportedException.Unrecoverable ? "An unrecoverable ThreadAbortException occurred, and the application will exit." : "A caught ThreadAbortException occurred.") + "\n\n" +
                                                  $"This exception may have been pushed here on accident.\n" +
                                                  $"{(ReportedException.ExtraMessage is not null ? $"{ReportedException.ExtraMessage}\n\n" : "")}" +
                                                  $"Message: {ThrAbrEx.Message}\n" +
                                                  $"Stacktrace: {ThrAbrEx.StackTrace}\n" +
                                                  $"Source: {ThrAbrEx.Source}\n" +
                                                  $"Target Site: {ThrAbrEx.TargetSite}\n" +
                                                  $"Inner Exception: {ThrAbrEx.InnerException}\n";
                    } break;

                    case WebException WebEx: {
                        rtbExceptionDetails.Text = (ReportedException.Unrecoverable ? "An unrecoverable WebException occurred, and the application will exit." : "A caught WebException occured.") + "\n\n" +
                                                  $"{(ReportedException.ExtraMessage is not null ? $"{ReportedException.ExtraMessage}\n\n" : "")}" +
                                                  $"{(ReportedException.ExtraInfo is not null ? "Web Address: " + $"{ReportedException.ExtraInfo}\n" : "")}" +
                                                  $"Message: {WebEx.Message}\n" +
                                                  $"Stacktrace: {WebEx.StackTrace}\n" +
                                                  $"Source: {WebEx.Source}\n" +
                                                  $"Target Site: {WebEx.TargetSite}\n" +
                                                  $"Inner Exception: {WebEx.InnerException}\n" +
                                                  $"Response: {WebEx.Response}\n";
                    } break;

                    case Exception Ex: {
                        rtbExceptionDetails.Text = (ReportedException.Unrecoverable ? $"An unrecoverable {ReportedException.ReceivedException.GetType().Name} occurred, and the application will exit." : $"A caught {ReportedException.ReceivedException.GetType().Name} occured.") + "\n\n" +
                                                  $"{(ReportedException.ExtraMessage is not null ? $"{ReportedException.ExtraMessage}\n\n" : "")}" +
                                                  $"Message: {Ex.Message}\n" +
                                                  $"Stacktrace: {Ex.StackTrace}\n" +
                                                  $"Source: {Ex.Source}\n" +
                                                  $"Target Site: {Ex.TargetSite}\n" +
                                                  $"Inner Exception: {Ex.InnerException}\n";
                    } break;

                    default: {
                        rtbExceptionDetails.Text = "An uncast exception occurred. The updater may exit after this dialog closes." +
                                                  $"{(ReportedException.ExtraMessage is not null ? $"\n\n{ReportedException.ExtraMessage}" : "")}\n";
                    } break;
                }

                rtbExceptionDetails.Text += "\n========== FULL REPORT ==========\n" +
                                            ReportedException.ReceivedException.ToString() +
                                            "\n========== END  REPORT ==========\n";

            }
            // A custom description was set, so we aren't going to write anything except for
            // what was written in the custom descrption.
            else rtbExceptionDetails.Text = $"{ReportedException.CustomDescription}\n";

            rtbExceptionDetails.Text += "\n========== OS  INFO ==========\n" +
                                        "(Please don't omit this info, it may be important)\n" +
                                        GetRelevantInformation() +
                                        "\n========== END INFO ==========";

            lbVersion.Text = "v" + (Program.IsBetaVersion ?
                                    Program.BetaVersion   :
                                    Program.CurrentVersion.ToString());
        }

        private void btnExceptionGithub_Click(object sender, EventArgs e) =>
            System.Diagnostics.Process.Start(GithubPage ?? "https://github.com/murrty");

        #endregion

        #region Supporting Methods

        private string GetRelevantInformation() =>
            $"Current version: " +
            $"{(Program.IsBetaVersion ? Program.BetaVersion : Program.CurrentVersion)}" +
            $"\nCurrent culture: {System.Threading.Thread.CurrentThread.CurrentCulture.EnglishName}" +
            $"\n{Log.ComputerVersionInformation}";

        private void LoadLanguage() {
            this.Text = "Excepion occowwed unu";
            lbExceptionHeader.Text = "An exception occowwed qwq";
            lbExceptionDescription.Text = ReportedException.Unrecoverable ? "The pwogwam has tow cwose :'c" : "The pwogwam wiw contiwuwu wen dis cwoses x3";
            rtbExceptionDetails.Text = "Bewo iz de pwobwem!";
            btnExceptionGithub.Text = "github >w<";
            btnExceptionOk.Text = "oki uwu";
            btnExceptionRetry.Text = "twy agaiwn";
        }

        #endregion

    }

    /// <summary>
    /// The base exception detail class containing information about the exception, and modifiers about the actions.
    /// </summary>
    public class ExceptionInfo {

        #region Variables / Fields / Whatever
        /// <summary>
        /// The dynamic exception that is received.
        /// </summary>
        public dynamic ReceivedException = null;
        /// <summary>
        /// Any extra info regarding the exception.
        /// </summary>
        public object ExtraInfo = null;
        /// <summary>
        /// An extra message that's printed before the main exception text.
        /// </summary>
        public string ExtraMessage = null;
        /// <summary>
        /// The description that is posted to the exception form instead of one that gets parsed.
        /// </summary>
        public string CustomDescription = null;
        /// <summary>
        /// If the exception was caused from loading the language file.
        /// </summary>
        public bool FromLanguage = false;
        /// <summary>
        /// If the cause of exception can be retried.
        /// </summary>
        public bool AllowRetry = false;
        /// <summary>
        /// If the exception is not recoverable, and the progarm must be terminated.
        /// </summary>
        public bool Unrecoverable = false;
        /// <summary>
        /// If the exception form should skip dwm compositing.
        /// </summary>
        public bool SkipDwmComposition = false;
        #endregion

        #region Constructor
        public ExceptionInfo(dynamic ReceivedException) {
            this.ReceivedException = ReceivedException;
        }
        #endregion

    }

}
