using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace aphrodite.Controls {

    public enum AllowedTextTypes {
        All,
        AlphabeticalOnly,
        NumericOnly,
        AlphaNumericOnly
    }

    public enum ButtonAlignments {
        Left,
        Right
    }

    /// <summary>
    /// An extension of Windows.Forms.TextBox to include extra functionality.
    /// </summary>
    class ExtendedTextBox : TextBox {

        private Button btn = new Button() {
            Enabled = false,
            TextAlign = ContentAlignment.MiddleCenter,
            UseVisualStyleBackColor = true,
            Visible = false,
        };

        public ExtendedTextBox() {
            UpdateButton();
            this.Controls.Add(btn);
            this.Refresh();
        }

        #region Private variables
        private ButtonAlignments _ButtonAlignment = ButtonAlignments.Left;
        private string _TextHint = string.Empty;
        private AllowedTextTypes _TextType = AllowedTextTypes.All;
        private bool _ButtonEnabled = false;
        #endregion

        #region Methods
        private void UpdateButton() {
            btn.Size = new Size(22, this.ClientSize.Height + 3);
            switch (_ButtonAlignment) {
                default:
                    btn.Location = new Point(this.ClientSize.Width - btn.Width, -2);
                    break;

                case ButtonAlignments.Right:
                    btn.Location = new Point(0, -2);
                    break;
            }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Refreshes the TextBox and realigns the Text to fit with the Button.
        /// </summary>
        public override void Refresh() {
            switch (_ButtonEnabled) {
                case true:
                    switch (_ButtonAlignment) {
                        default:
                            NativeMethods.SendMessage(this.Handle, NativeMethods.EM_SETMARGINS, (IntPtr)NativeMethods.EC_RIGHTMARGIN, (IntPtr)(btn.Width << 16));
                            break;

                        case ButtonAlignments.Right:
                            NativeMethods.SendMessage(this.Handle, NativeMethods.EM_SETMARGINS, (IntPtr)NativeMethods.EC_LEFTMARGIN, (IntPtr)(btn.Width));
                            break;
                    }
                    break;

                case false:
                    NativeMethods.SendMessage(this.Handle, NativeMethods.EM_SETMARGINS, (IntPtr)NativeMethods.EC_LEFTMARGIN, IntPtr.Zero);
                    NativeMethods.SendMessage(this.Handle, NativeMethods.EM_SETMARGINS, (IntPtr)NativeMethods.EC_RIGHTMARGIN, IntPtr.Zero);
                    break;
            }
            base.Refresh();
        }

        protected override void OnResize(EventArgs e) {
            UpdateButton();
            this.Refresh();
            base.OnResize(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e) {

            switch (_TextType) {
                case AllowedTextTypes.AlphabeticalOnly:
                    switch (e.KeyChar) {
                        case (char)Keys.Back: case (char)Keys.Space:
                        case (char)Keys.Return:
                        case 'a': case 'A': case 'b': case 'B': case 'c': case 'C':
                        case 'd': case 'D': case 'e': case 'E': case 'f': case 'F':
                        case 'g': case 'G': case 'h': case 'H': case 'i': case 'I':
                        case 'j': case 'J': case 'k': case 'K': case 'l': case 'L':
                        case 'm': case 'M': case 'o': case 'O': case 'p': case 'P':
                        case 'q': case 'Q': case 'r': case 'R': case 's': case 'S':
                        case 't': case 'T': case 'u': case 'U': case 'v': case 'V':
                        case 'w': case 'W': case 'x': case 'X': case 'y': case 'Y':
                        case 'z': case 'Z':
                            e.Handled = false;
                            break;

                        default:
                            e.Handled = true;
                            break;
                    }
                    break;

                case AllowedTextTypes.NumericOnly:
                    switch (e.KeyChar) {
                        case (char)Keys.Back: case (char)Keys.Space:
                        case (char)Keys.Return:
                        case '.': case '0':
                        case '1': case '2': case '3':
                        case '4': case '5': case '6':
                        case '7': case '8': case '9':
                            e.Handled = false;
                            break;

                        default:
                            e.Handled = true;
                            break;
                    }
                    break;

                case AllowedTextTypes.AlphaNumericOnly:
                    switch (e.KeyChar) {
                        case (char)Keys.Back: case (char)Keys.Space:
                        case (char)Keys.Return:
                        case '.': case '0':
                        case '1': case '2': case '3':
                        case '4': case '5': case '6':
                        case '7': case '8': case '9':
                        case 'a': case 'A': case 'b': case 'B': case 'c': case 'C':
                        case 'd': case 'D': case 'e': case 'E': case 'f': case 'F':
                        case 'g': case 'G': case 'h': case 'H': case 'i': case 'I':
                        case 'j': case 'J': case 'k': case 'K': case 'l': case 'L':
                        case 'm': case 'M': case 'o': case 'O': case 'p': case 'P':
                        case 'q': case 'Q': case 'r': case 'R': case 's': case 'S':
                        case 't': case 'T': case 'u': case 'U': case 'v': case 'V':
                        case 'w': case 'W': case 'x': case 'X': case 'y': case 'Y':
                        case 'z': case 'Z':
                            e.Handled = false;
                            break;

                        default:
                            e.Handled = true;
                            break;
                    }
                    break;
            }

            base.OnKeyPress(e);
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            switch (e.Modifiers == Keys.Control) {
                case true:
                    switch (e.KeyCode) {
                        //case Keys.V:
                        //    switch (Clipboard.ContainsText()) {
                        //        case true:
                        //            e.SuppressKeyPress = true;
                        //            this.Text = Clipboard.GetText();
                        //            break;
                        //    }
                        //    break;

                        case Keys.A:
                            e.SuppressKeyPress = true;
                            this.SelectAll();
                            break;
                    }
                    break;
            }
            base.OnKeyDown(e);
        }
        #endregion

        #region Events
        /// <summary>
        /// Event raised when the Button in the TextBox is clicked.
        /// </summary>
        public event EventHandler ButtonClick {
            add { btn.Click += value; }
            remove { btn.Click -= value; }
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Sets the location of the Button in the TextBox.
        /// </summary>
        public ButtonAlignments ButtonAlignment {
            get { return _ButtonAlignment; }
            set { _ButtonAlignment = value; UpdateButton(); this.Refresh(); }
        }

        /// <summary>
        /// Gets or sets the cursor of the Button.
        /// </summary>
        public Cursor ButtonCursor {
            get { return btn.Cursor; }
            set { btn.Cursor = value; }
        }

        /// <summary>
        /// Gets or sets the Font on the Button.
        /// </summary>
        public Font ButtonFont {
            get { return btn.Font; }
            set { btn.Font = value; }
        }

        /// <summary>
        /// Gets or sets the Image on the Button.
        /// </summary>
        public Image ButtonImage {
            get { return btn.Image; }
            set { btn.Image = value; }
        }

        /// <summary>
        /// Gets or sets the Image Align on the Button.
        /// </summary>
        public ContentAlignment ButtonImageAlign {
            get { return btn.ImageAlign; }
            set { btn.ImageAlign = value; }
        }

        /// <summary>
        /// Gets or sets the Image Index on the Button.
        /// </summary>
        public int ButtonImageIndex {
            get { return btn.ImageIndex; }
            set { btn.ImageIndex = value; }
        }

        /// <summary>
        /// Gets or sets the Image Key on the Button.
        /// </summary>
        public string ButtonImageKey {
            get { return btn.ImageKey; }
            set { btn.ImageKey = value; }
        }

        /// <summary>
        /// Gets or sets the Image List on the Button.
        /// </summary>
        public ImageList ButtonImageList {
            get { return btn.ImageList; }
            set { btn.ImageList = value; }
        }

        /// <summary>
        /// Gets or sets the size of the Button.
        /// </summary>
        public Size ButtonSize {
            get { return btn.Size; }
            set { btn.Size = value; }
        }

        /// <summary>
        /// Gets or sets the text visible on the Button.
        /// </summary>
        public string ButtonText {
            get { return btn.Text; }
            set { btn.Text = value; }
        }

        /// <summary>
        /// Gets or sets the Text Alignment on the Button.
        /// </summary>
        public ContentAlignment ButtonTextAlign {
            get { return btn.TextAlign; }
            set { btn.TextAlign = value; }
        }

        /// <summary>
        /// Enable or Disable the Button in the TextBox.
        /// </summary>
        public bool ButtonEnabled {
            get { return _ButtonEnabled; }
            set {
                btn.Visible = value;
                btn.Enabled = value;
               _ButtonEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the text hint on the TextBox.
        /// </summary>
        public string TextHint {
            get { return _TextHint; }
            set {
                _TextHint = value;
                NativeMethods.SendMessage(this.Handle, 0x1501, (IntPtr)1, value);
            }
        }

        /// <summary>
        /// Sets the text that's allowed to be entered into the text box.
        /// </summary>
        public AllowedTextTypes TextType {
            get { return _TextType; }
            set { _TextType = value; }
        }
        #endregion

    }
}
