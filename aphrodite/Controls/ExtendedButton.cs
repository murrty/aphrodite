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
        #endregion

        #region Properties
        /// <summary>
        /// Gets the bool <seealso cref="_ShowUACShield"/>.
        /// </summary>
        [Category("Appearance"), Description("Indicates if the UAC Shield should be displayed on the button."), DefaultValue(false)]
        public bool ShowUACShield {
            get => _ShowUACShield;
            set {
                _ShowUACShield = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Sets the text of the button.
        /// </summary>
        [Category("Appearance"), Description("The text that is written on the button."), DefaultValue("")]
        public override string Text {
            get => base.Text;
            set => base.Text = value;
        }
        #endregion

        #region Native Methods
        /// <summary>
        /// No documentation found.
        /// </summary>
        private const int BCM_FIRST = 0x1600;
        /// <summary>
        /// Sets the elevation required state for a specified button or command link to display an elevated icon.
        /// </summary>
        private const int BCM_SETSHIELD = 0x160C;
        /// <summary>
        /// Sets the image onto the button.
        /// </summary>
        private const int BM_SETIMAGE = 0x00F7;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        #endregion

        #region Overrides
        /// <summary>
        /// Refreshes the control.
        /// </summary>
        public override void Refresh() {
            this.RecreateHandle();
            if (_ShowUACShield) {
                SendMessage(this.Handle, BCM_FIRST, IntPtr.Zero, (IntPtr)2);
                this.FlatStyle = FlatStyle.System;
                base.Text = " " + base.Text;
                SendMessage(this.Handle, BCM_SETSHIELD, IntPtr.Zero, (IntPtr)2);
            }
            else {
                base.Text = base.Text.Trim();
                SendMessage(this.Handle, BCM_FIRST, IntPtr.Zero, (IntPtr)2);
                SendMessage(this.Handle, BM_SETIMAGE, (IntPtr)1, IntPtr.Zero);
            }
        }
        #endregion

    }
}