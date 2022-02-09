using System.Windows.Forms;

namespace aphrodite.Controls {

    [System.Diagnostics.DebuggerStepThrough]
    class AeroListBox : ListBox {
        public AeroListBox() {
            NativeMethods.SetWindowTheme(this.Handle, "explorer", null);
        }
    }
}
