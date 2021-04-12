using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace aphrodite.Controls {

    /// <summary>
    /// An extension of a Windows.Forms.Button to include extra functionality.
    /// </summary>
    class ExtendedButton : Button {

        private bool _ShowUACShield = false;
        private bool IsRefreshing = false;

        public override string Text {
            get {

                switch (_ShowUACShield && !IsRefreshing) {

                    case true: {
                            switch (base.Text.Length > 1) {
                                case true: {
                                        return base.Text.Substring(1);
                                    }

                                case false: {
                                        return string.Empty;
                                    }
                            }
                            break;
                        }

                }

                return base.Text;
            }

            set {
                switch (_ShowUACShield) {

                    case true: {
                            switch (value.Length > 0) {
                                case true: {
                                        base.Text = " " + value;
                                        break;
                                    }

                                case false: {
                                        base.Text = value;
                                        break;
                                    }
                            }
                            break;
                        }

                    case false: {
                            base.Text = value;
                            break;
                        }

                }
            }
        }

        public override void Refresh() {
            IsRefreshing = true;
            switch (_ShowUACShield) {
                case true:
                    this.FlatStyle = System.Windows.Forms.FlatStyle.System;
                    base.Text = " " + base.Text;
                    NativeMethods.SendMessage(this.Handle, NativeMethods.BCM_SETSHIELD, IntPtr.Zero, (IntPtr)2);
                    break;

                case false:
                    this.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
                    switch (base.Text.Length > 1) {
                        case true:
                            base.Text = base.Text.Substring(1);
                            break;

                        case false:
                            base.Text = string.Empty;
                            break;
                    }
                    NativeMethods.SendMessage(this.Handle, NativeMethods.BCM_FIRST, IntPtr.Zero, (IntPtr)2);
                    break;
            }
            base.Refresh();
            IsRefreshing = false;
        }

        [Category("Appearance"), Description("Indicates if the UAC Shield should be displayed on the button."), DefaultValue(false)]
        public bool ShowUACShield {
            get { return _ShowUACShield; }
            set { _ShowUACShield = value; this.Refresh(); }
        }

    }
}
