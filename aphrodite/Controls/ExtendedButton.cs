using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace murrty.controls {

    /// <summary>
    /// Represents a derived Windows button control, with added functionality.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    class ExtendedButton : Button {

        #region Fields

        /// <summary>
        /// If the UAC shield should be shown on the button.
        /// </summary>
        private bool _ShowUACShield = false;
        /// <summary>
        /// If the control is refreshing, prevents problems.
        /// </summary>
        private bool IsRefreshing = false;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the bool <seealso cref="_ShowUACShield"/>.
        /// </summary>
        [Category("Appearance"), Description("Indicates if the UAC Shield should be displayed on the button."), DefaultValue(false)]
        public bool ShowUACShield {
            get { return _ShowUACShield; }
            set { _ShowUACShield = value; this.Refresh(); }
        }

        /// <summary>
        /// Sets the text of the button.
        /// </summary>
        [Category("Appearance"), Description("The text that is written on the button."), DefaultValue("")]
        public override string Text {
            get {
                return _ShowUACShield && !IsRefreshing ? (base.Text.Length > 1 ? base.Text.Substring(1) : string.Empty) : base.Text;
            }

            set {
                base.Text = _ShowUACShield && value.Length > 0 ? $" {value}" : value;
            }
        }

        #endregion

        #region Native Methods

        private const int BCM_FIRST = 0x1600;
        private const int BCM_SETSHIELD = 0x160C;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        #endregion

        #region Overrides

        /// <summary>
        /// Refreshes the control.
        /// </summary>
        public override void Refresh() {
            IsRefreshing = true;
            base.Refresh();
            if (_ShowUACShield) {
                this.FlatStyle = FlatStyle.System;
                base.Text = " " + base.Text;
                SendMessage(this.Handle, BCM_SETSHIELD, IntPtr.Zero, (IntPtr)2);
            }
            else {
                base.Text = base.Text.Length > 1 ? base.Text.Substring(1) : string.Empty;
                SendMessage(this.Handle, BCM_FIRST, IntPtr.Zero, (IntPtr)2);
            }
            IsRefreshing = false;
        }

        #endregion

    }
}