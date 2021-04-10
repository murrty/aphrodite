using System.Windows.Forms;

namespace aphrodite {
    public partial class frmLicensing : Form {
        public frmLicensing() {
            InitializeComponent();
        }

        private void frmLicensing_FormClosing(object sender, FormClosingEventArgs e) {
            this.Dispose();
        }
    }
}
