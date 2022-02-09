using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aphrodite {
    public partial class frmInkBunnyLogin : Form {
        public frmInkBunnyLogin() {
            InitializeComponent();

            if (Config.ValidPoint(Config.Settings.FormSettings.frmInkBunnyLogin_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmInkBunnyLogin_Location;
            }
        }
    }
}
