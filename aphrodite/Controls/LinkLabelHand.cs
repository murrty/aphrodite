using System;
using System.Windows.Forms;

namespace aphrodite {
    class LinkLabelHand : LinkLabel {
        [System.Diagnostics.DebuggerStepThrough]
        protected override void WndProc(ref Message m) {
            if (m.Msg == 0x0020) {
                NativeMethods.SetCursor(NativeMethods.LoadCursor(IntPtr.Zero, 32649));
                m.Result = IntPtr.Zero;
                return;
            }
            
            base.WndProc(ref m);
        }
    }
}
