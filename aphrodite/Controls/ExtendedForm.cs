using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;
using aphrodite;

namespace murrty.classcontrols {

    /// <summary>
    /// Represents a derived window or dialog box that makes up an applications' user interface, with added functionality.
    /// </summary>
    [ToolboxItem(false)]
    [System.Diagnostics.DebuggerStepThrough]
    public class ExtendedForm : Form {

        /// <summary>
        /// Whether the exit box is enabled or disabled.
        /// </summary>
        protected bool _ExitBox = true;
        /// <summary>
        /// The thread that will be used to handle downloading, saving the UI thread from pain.
        /// </summary>
        protected System.Threading.Thread DownloadThread;
        /// <summary>
        /// The webclient used to download data.
        /// </summary>
        protected ExtendedWebClient DownloadClient;
        /// <summary>
        /// The current url being processed by the program. Not used during downloading.
        /// </summary>
        protected string CurrentUrl = string.Empty;
        /// <summary>
        /// Whether the download has errored during execution, used primarily for first-attempt loops.
        /// </summary>
        protected bool DownloadError = false;
        /// <summary>
        /// Whether the download is aborted.
        /// </summary>
        protected bool DownloadAbort = false;
        /// <summary>
        /// Whether the action should retry.
        /// </summary>
        protected bool ShouldRetry = false;
        /// <summary>
        /// The XmlDocument that the form will use.
        /// </summary>
        protected XmlDocument xmlDoc;
        /// <summary>
        /// The throttle count between each download progress changed interval.
        /// Used to prevent UI thread spamming.
        /// </summary>
        protected int ThrottleCount = 0;
        /// <summary>
        /// The timer used to modify the forms' name.
        /// </summary>
        protected Timer tmrTitle = new() {
            Interval = 1000
        };

        public bool ExitBox {
            get {
                return _ExitBox;
            }
            set {
                _ExitBox = value;
                EnableMenuItem(GetSystemMenu(this.Handle, false), SC_CLOSE, value ? 0u : 1u);
            }
        }

        public DownloadStatus Status {
            get;
            set;
        } = DownloadStatus.Waiting;

        public System.Media.SystemSound ShownSound {
            get;
            set;
        } = null;

        private const uint SC_CLOSE = 0xf060;
        [DllImport("user32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        public ExtendedForm() {
            tmrTitle.Tick += (s, e) => {
                this.Text = this.Text.EndsWith("....") ? this.Text.Trim('.') : this.Text + ".";
            };
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
            EnableMenuItem(GetSystemMenu(this.Handle, false), SC_CLOSE, _ExitBox ? 0u : 1u);
            PrepareDownload();
        }

        protected override void OnShown(EventArgs e) {
            base.OnShown(e);
            ShownSound?.Play();
        }

        /// <summary>
        /// Method to generate required information, as well as setting the values of the configurated settings to the form.
        /// </summary>
        protected virtual void PrepareDownload() {
            StartDownload();
        }

        /// <summary>
        /// Method to build the download thread and to start it.
        /// </summary>
        protected virtual void StartDownload() { }

        /// <summary>
        /// Method that always runs after the download, successful or not.
        /// </summary>
        protected virtual void FinishDownload() {
            switch (Status) {
                case DownloadStatus.Finished:
                case DownloadStatus.FileAlreadyExists:
                case DownloadStatus.NothingToDownload: {
                    System.Media.SystemSounds.Asterisk.Play();
                } break;

                case DownloadStatus.Errored:
                case DownloadStatus.Forbidden:
                case DownloadStatus.PostOrPoolWasDeleted:
                case DownloadStatus.ApiReturnedNullOrEmpty:
                case DownloadStatus.FileWasNullAfterBypassingBlacklist: {
                    System.Media.SystemSounds.Hand.Play();
                } break;

                case DownloadStatus.Aborted: {
                    System.Media.SystemSounds.Exclamation.Play();
                } break;

                default: {
                    System.Media.SystemSounds.Exclamation.Play();
                } break;
            }
        }
    
    }
}
