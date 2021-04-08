﻿using System;
using System.Windows.Forms;

namespace aphrodite.Controls {
    class LinkLabelHand : LinkLabel {
        [System.Diagnostics.DebuggerStepThrough]
        protected override void WndProc(ref Message m) {
            if (m.Msg == 0x0020) {
                NativeMethods.SetCursor(NativeMethods.LoadCursor(IntPtr.Zero, NativeMethods.HAND));
                m.Result = IntPtr.Zero;
                return;
            }
            
            base.WndProc(ref m);
        }
    }
}
