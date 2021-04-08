using System.Windows.Forms;

namespace aphrodite.Controls {
    class AeroListBox : ListBox {
        public AeroListBox() {
            NativeMethods.SetWindowTheme(this.Handle, "explorer", null);
        }
    }
}
