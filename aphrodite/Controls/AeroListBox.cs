using System.Windows.Forms;

namespace aphrodite {
    class AeroListBox : ListBox {
        public AeroListBox() {
            NativeMethods.SetWindowTheme(this.Handle, "explorer", null);
        }
    }
}
