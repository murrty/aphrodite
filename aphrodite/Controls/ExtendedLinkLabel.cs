using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace murrty.controls {

    /// <summary>
    /// Represents a derived Windows label control that can display hyperlinks, with added functionality.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    internal class ExtendedLinkLabel : LinkLabel {

        #region Native Methods

        /// <summary>
        /// The WndProc message for setting the systems' cursor.
        /// </summary>
        private const int WM_SETCURSOR = 0x0020;
        /// <summary>
        /// The user32.h resource identifier for the systems' hand cursor.
        /// </summary>
        private const int IDC_HAND = 32649;
        /// <summary>
        /// The IntPtr value of IDC_HAND.
        /// </summary>
        private static readonly IntPtr SystemHand = LoadCursor(IntPtr.Zero, (IntPtr)IDC_HAND);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadCursor(IntPtr hInstance, IntPtr lpCursorName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SetCursor(IntPtr hCursor);

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ExtendedLinkLabel() {
            this.LinkColor = System.Drawing.Color.FromArgb(0x00, 0x66, 0xCC);
            this.VisitedLinkColor = System.Drawing.Color.FromArgb(0x80, 0x00, 0x80);
            this.ActiveLinkColor = System.Drawing.Color.FromArgb(0xFF, 0x00, 0x00);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Processes the specified Windows message, overriding WM_SETCURSOR.
        /// </summary>
        /// <param name="m">The message to process.</param>
        [System.Diagnostics.DebuggerStepThrough]
        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                case WM_SETCURSOR: {
                    SetCursor(SystemHand);
                    m.Result = IntPtr.Zero;
                } break;

                default: {
                    base.WndProc(ref m);
                } break;
            }
        }

        #endregion

    }

}
