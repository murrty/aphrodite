using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aphrodite {
    class AeroListBox : ListBox {
        public AeroListBox() {
            NativeMethods.SetWindowTheme(this.Handle, "explorer", null);
        }
    }
}
