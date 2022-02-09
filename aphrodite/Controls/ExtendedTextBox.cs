using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace murrty.controls {

    /// <summary>
    /// An enumeration of types of characters allowed in the textbox.
    /// </summary>
    public enum AllowedCharacters {
        /// <summary>
        /// All characters are allowed.
        /// </summary>
        All,
        /// <summary>
        /// Only Upper and lowercase alphabetical letters are allowed.
        /// </summary>
        AlphabeticalOnly,
        /// <summary>
        /// Only numbers are allowed.
        /// </summary>
        NumericOnly,
        /// <summary>
        /// Only letters and numbers are allowed.
        /// </summary>
        AlphaNumericOnly
    }

    /// <summary>
    /// An enumeration of the alignments that the button can be aligned to.
    /// </summary>
    public enum ButtonAlignment {
        /// <summary>
        /// The Button will appear on the left side of the TextBox.
        /// </summary>
        Left,
        /// <summary>
        /// The Button will appear on the right side of the TextBox. Default value.
        /// </summary>
        Right,
    }

    /// <summary>
    /// An extension of Windows.Forms.TextBox to include extra functionality.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    public class ExtendedTextBox : TextBox {

        #region Fields

        /// <summary>
        /// The text hint.
        /// </summary>
        private string _TextHint = string.Empty;
        /// <summary>
        /// What kind of text is allowed.
        /// </summary>
        private AllowedCharacters _TextType = AllowedCharacters.All;
        /// <summary>
        /// If the button is enabled.
        /// </summary>
        private bool _ButtonEnabled = false;
        /// <summary>
        /// The alignment of the button.
        /// </summary>
        private ButtonAlignment _ButtonAlignment = ButtonAlignment.Left;
        /// <summary>
        /// If the font should be syncronized across the button and text.
        /// </summary>
        private bool _SyncFont = false;

        /// <summary>
        /// The button that appears inside the textbox.
        /// </summary>
        private readonly Button InsetButton = new() {
            Cursor = Cursors.Default,
            Enabled = false,
            TextAlign = ContentAlignment.MiddleCenter,
            UseVisualStyleBackColor = true,
            Visible = false,
        };

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ButtonAlignment of the button.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(ButtonAlignment.Right)]
        [Description("The position of the button inside the Text Box.")]
        public ButtonAlignment ButtonAlignment {
            get { return _ButtonAlignment; }
            set {
                _ButtonAlignment = value;
                UpdateButton();
                this.Refresh();
            }
        }
        
        /// <summary>
        /// Gets or sets the cursor of the button.
        /// </summary>
        [Category("Appearance")]
        [Description("The cursor that will appear when hovering over the Button.")]
        public Cursor ButtonCursor {
            get { return InsetButton.Cursor; }
            set { InsetButton.Cursor = value; }
        }

        /// <summary>
        /// Gets or sets the text font of the button.
        /// </summary>
        [Category("Appearance")]
        [Description("The Font of the text that appears within the Button.")]
        public Font ButtonFont {
            get { return InsetButton.Font; }
            set {
                InsetButton.Font = value;
                if (_SyncFont) {
                    base.Font = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the image in the button.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(null)]
        [Description("The Image that appears on the Button.")]
        public Image ButtonImage {
            get { return InsetButton.Image; }
            set { InsetButton.Image = value; }
        }

        /// <summary>
        /// Gets or sets the image alignment of the buttons' image.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(ContentAlignment.MiddleCenter)]
        [Description("The Image Alignment of an Image on the Button.")]
        public ContentAlignment ButtonImageAlign {
            get { return InsetButton.ImageAlign; }
            set { InsetButton.ImageAlign = value; }
        }

        /// <summary>
        /// Gets or sets the image index of the buttons' image key or image list.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(null)]
        [Description("The Image Index of the Image on the Button within the Image List.")]
        public int ButtonImageIndex {
            get { return InsetButton.ImageIndex; }
            set { InsetButton.ImageIndex = value; }
        }

        /// <summary>
        /// Gets or sets the buttons' image key.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(null)]
        [Description("The Image Key of the Image on the Button.")]
        public string ButtonImageKey {
            get { return InsetButton.ImageKey; }
            set { InsetButton.ImageKey = value; }
        }

        /// <summary>
        /// Gets or sets the buttons' image list.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(null)]
        [Description("The Image List for use with the Button.")]
        public ImageList ButtonImageList {
            get { return InsetButton.ImageList; }
            set { InsetButton.ImageList = value; }
        }

        /// <summary>
        /// Gets or sets the size of the button.
        /// </summary>
        [Category("Appearance")]
        [Description("The Size of the Button.")]
        public Size ButtonSize {
            get { return InsetButton.Size; }
            set { InsetButton.Size = value; }
        }

        /// <summary>
        /// Gets or sets the text of the button.
        /// </summary>
        [Category("Appearance")]
        [Description("The text that appears on the Button.")]
        public string ButtonText {
            get { return InsetButton.Text; }
            set { InsetButton.Text = value; }
        }

        /// <summary>
        /// Gets or sets the text alignment of the buttons' text.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(ContentAlignment.MiddleRight)]
        [Description("The Alignment of the text on the Button.")]
        public ContentAlignment ButtonTextAlign {
            get { return InsetButton.TextAlign; }
            set { InsetButton.TextAlign = value; }
        }

        /// <summary>
        /// Gets or sets the bool of whether the button inside the text box is enabled and can be used.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("The Button on the TextBox is enabled and usable.")]
        public bool ButtonEnabled {
            get { return _ButtonEnabled; }
            set {
                InsetButton.Visible = value;
                InsetButton.Enabled = value;
                _ButtonEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the Font of the TextBox.
        /// </summary>
        [Category("Appearance")]
        [Description("The Font of the text that appears within the TextBox.")]
        public new Font Font {
            get {
                return base.Font;
            }
            set {
                base.Font = value;
                if (_SyncFont) {
                    InsetButton.Font = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the bool whether the font should be in sync between the TextBox and Button.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("Whether the font on the button and text box should be in sync. Text box takes precedent, changing either updates the other.")]
        public bool SyncronizeFont {
            get {
                return _SyncFont;
            }
            set {
                _SyncFont = value;
                if (value) {
                    InsetButton.Font = base.Font;
                }
            }
        }

        /// <summary>
        /// Gets or sets the alignment of the text in the text box.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(HorizontalAlignment.Left)]
        [Description("Indicates how the text should be aligned for edit controls.")]
        [Localizable(true)]
        public new HorizontalAlignment TextAlign {
            get { return base.TextAlign; }
            set {
                base.TextAlign = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the hint on the TextBox.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(null)]
        [Description("The Text that will appear as a hint in the Text Box.")]
        public string TextHint {
            get { return _TextHint; }
            set {
                _TextHint = value;
                SendMessage(this.Handle, 0x1501, (IntPtr)1, value);
            }
        }

        /// <summary>
        /// Gets or sets the allowed characters to be typed in the TextBox.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue(AllowedCharacters.All)]
        [Description("Determines if the Text Box wil only accept certain kinds of characters.")]
        public AllowedCharacters TextType {
            get { return _TextType; }
            set { _TextType = value; }
        }

        #endregion

        #region Native Methods

        public const int EM_SETMARGINS = 0xD3;
        public const int EC_RIGHTMARGIN = 0x2;
        public const int EC_LEFTMARGIN = 0x1;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, string lParam);

        #endregion

        #region Constructor

        public ExtendedTextBox() {
            UpdateButton();
            Controls.Add(InsetButton);
            Refresh();
        }

        #endregion

        #region Overrides

        public override void Refresh() {
            base.Refresh();
            switch (_ButtonEnabled) {
                case true:
                    switch (_ButtonAlignment) {
                        default:
                            SendMessage(Handle, EM_SETMARGINS, (IntPtr)EC_RIGHTMARGIN, (IntPtr)(InsetButton.Width << 16));
                            break;

                        case ButtonAlignment.Right:
                            SendMessage(Handle, EM_SETMARGINS, (IntPtr)EC_LEFTMARGIN, (IntPtr)(InsetButton.Width));
                            break;
                    }
                    break;

                case false:
                    SendMessage(Handle, EM_SETMARGINS, (IntPtr)EC_LEFTMARGIN, IntPtr.Zero);
                    SendMessage(Handle, EM_SETMARGINS, (IntPtr)EC_RIGHTMARGIN, IntPtr.Zero);
                    break;
            }
            if (!string.IsNullOrWhiteSpace(_TextHint)) {
                SendMessage(this.Handle, 0x1501, (IntPtr)1, _TextHint);
            }
        }

        protected override void OnResize(EventArgs e) {
            UpdateButton();
            Refresh();
            base.OnResize(e);
        }

        //[Obsolete("Find a better way to handle text shortcuts.")]
        //protected override void OnTextChanged(EventArgs e) {
        //    base.OnTextChanged(e);
        //    switch (this._TextType) {
        //        case AllowedCharacters.AlphabeticalOnly: {
        //            int cursorPosition = this.SelectionStart;
        //            this.Text = Regex.Replace(this.Text, "[^a-zA-Z ]", "");
        //            this.SelectionStart = cursorPosition;
        //        } break;

        //        case AllowedCharacters.NumericOnly: {
        //            int cursorPosition = this.SelectionStart;
        //            this.Text = Regex.Replace(this.Text, "[^0-9 ]", "");
        //            this.SelectionStart = cursorPosition;
        //        } break;

        //        case AllowedCharacters.AlphaNumericOnly: {
        //            int cursorPosition = this.SelectionStart;
        //            this.Text = Regex.Replace(this.Text, "[^0-9a-zA-Z ]", "");
        //            this.SelectionStart = cursorPosition;
        //        } break;
        //    }
        //}

        protected override void OnKeyDown(KeyEventArgs e) {
            switch (e.KeyCode) {
                // Backspace can have either Control or Shift key held down,
                // Shift doesnt' change the behavior, but Control does.
                case Keys.Back: {
                    e.Handled = e.Alt;
                } break;

                // Return & Space can have the Shift key held down.
                // It doesn't change the behavior.
                case Keys.Return:
                case Keys.Space: {
                    e.Handled = e.Control || e.Alt;
                } break;

                // Catch the modifier keys, so it won't
                // play the sound if they're pressed.
                case Keys.Control:
                case Keys.ControlKey:
                case Keys.LControlKey:
                case Keys.RControlKey:
                case Keys.Alt:
                case Keys.Menu:
                case Keys.LMenu:
                case Keys.RMenu:
                case Keys.Shift:
                case Keys.ShiftKey:
                case Keys.LShiftKey:
                case Keys.RShiftKey: {
                } break;

                case Keys.A: case Keys.B: case Keys.C: case Keys.D:
                case Keys.E: case Keys.F: case Keys.G: case Keys.H:
                case Keys.I: case Keys.J: case Keys.K: case Keys.L:
                case Keys.M: case Keys.N: case Keys.O: case Keys.P:
                case Keys.Q: case Keys.R: case Keys.S: case Keys.T:
                case Keys.U: case Keys.V: case Keys.W: case Keys.X:
                case Keys.Y: case Keys.Z: {
                    switch (_TextType) {
                        case AllowedCharacters.AlphabeticalOnly:
                        case AllowedCharacters.AlphaNumericOnly: {
                            e.SuppressKeyPress = e.Alt || e.Control;
                            if (e.Control) {
                                switch (e.KeyCode) {
                                    case Keys.A: case Keys.C: case Keys.X: {
                                        e.SuppressKeyPress = false;
                                    }
                                    break;

                                    case Keys.V: {
                                        e.SuppressKeyPress =
                                            !(Clipboard.ContainsText() &&
                                            Regex.IsMatch(
                                                Clipboard.GetText(),
                                                _TextType switch {
                                                    AllowedCharacters.AlphabeticalOnly =>
                                                        "^[a-zA-Z]+$",

                                                    AllowedCharacters.AlphaNumericOnly =>
                                                        "^[a-zA-Z0-9]+$",

                                                    _ =>
                                                        throw new ArgumentOutOfRangeException("Ctrl + V was pressed but regex couldn't use a proper TextType.")
                                                }
                                            ));
                                    }
                                    break;
                                }
                            }
                        } break;

                        case AllowedCharacters.NumericOnly: {
                            if (e.Control && e.KeyCode == Keys.V) {
                                e.SuppressKeyPress = Clipboard.ContainsText() && Regex.IsMatch(Clipboard.GetText(), "^[0-9]+$");
                            }
                        } break;

                        default: {
                            e.SuppressKeyPress = _TextType != AllowedCharacters.All;
                        } break;
                    }
                } break;

                case Keys.D1: case Keys.D2: case Keys.D3:
                case Keys.D4: case Keys.D5: case Keys.D6:
                case Keys.D7: case Keys.D8: case Keys.D9:
                case Keys.D0: {
                    switch (_TextType) {
                        case AllowedCharacters.NumericOnly:
                        case AllowedCharacters.AlphaNumericOnly: {
                            e.SuppressKeyPress = e.Shift || e.Alt;
                        } break;

                        default: {
                            e.SuppressKeyPress = _TextType != AllowedCharacters.All;
                        } break;
                    } break;
                }

                default: {
                    e.SuppressKeyPress = _TextType != AllowedCharacters.All;
                } break;

                #region Old filtering
                    //switch (_TextType) {
                    //    case AllowedCharacters.AlphabeticalOnly: {
                    //        switch (e.KeyCode) {
                    //            case Keys.A: case Keys.B: case Keys.C: case Keys.D:
                    //            case Keys.E: case Keys.F: case Keys.G: case Keys.H:
                    //            case Keys.I: case Keys.J: case Keys.K: case Keys.L:
                    //            case Keys.M: case Keys.N: case Keys.O: case Keys.P:
                    //            case Keys.Q: case Keys.R: case Keys.S: case Keys.T:
                    //            case Keys.U: case Keys.V: case Keys.W: case Keys.X:
                    //            case Keys.Y: case Keys.Z: {
                    //                e.SuppressKeyPress = e.Alt || e.Control;
                    //                if (e.Control) {
                    //                    switch (e.KeyCode) {
                    //                        case Keys.A: case Keys.C: case Keys.X: {
                    //                            e.SuppressKeyPress = false;
                    //                        } break;

                    //                        case Keys.V: {
                    //                            if (Clipboard.ContainsText() && Regex.IsMatch(Clipboard.GetText(), "^[a-zA-Z]+$")) {
                    //                                e.SuppressKeyPress = false;
                    //                            }
                    //                        } break;
                    //                    }
                    //                }
                    //            } break;

                    //            default: {
                    //                e.SuppressKeyPress = true;
                    //            } break;
                    //        }
                    //    } break;

                    //    case AllowedCharacters.NumericOnly: {
                    //        switch (e.KeyCode) {
                    //            case Keys.D1: case Keys.D2: case Keys.D3:
                    //            case Keys.D4: case Keys.D5: case Keys.D6:
                    //            case Keys.D7: case Keys.D8: case Keys.D9:
                    //            case Keys.D0: {
                    //                e.SuppressKeyPress = e.Control || e.Shift || e.Alt;
                    //            } break;

                    //            case Keys.V: {
                    //                e.SuppressKeyPress = e.Shift || e.Alt;
                    //                if (e.Control) {
                    //                    switch (e.KeyCode) {
                    //                        case Keys.A: case Keys.C: case Keys.X: {
                    //                            e.SuppressKeyPress = false;
                    //                        } break;

                    //                        case Keys.V: {
                    //                            if (Clipboard.ContainsText() && Regex.IsMatch(Clipboard.GetText(), "^[0-9]+$")) {
                    //                                e.SuppressKeyPress = false;
                    //                            }
                    //                        } break;
                    //                    }
                    //                }
                    //            } break;

                    //            default: {
                    //                e.SuppressKeyPress = true;
                    //            } break;
                    //        }
                    //    } break;

                    //    case AllowedCharacters.AlphaNumericOnly: {
                    //        switch (e.KeyCode) {
                    //            case Keys.A: case Keys.B: case Keys.C: case Keys.D:
                    //            case Keys.E: case Keys.F: case Keys.G: case Keys.H:
                    //            case Keys.I: case Keys.J: case Keys.K: case Keys.L:
                    //            case Keys.M: case Keys.N: case Keys.O: case Keys.P:
                    //            case Keys.Q: case Keys.R: case Keys.S: case Keys.T:
                    //            case Keys.U: case Keys.V: case Keys.W: case Keys.X:
                    //            case Keys.Y: case Keys.Z: {
                    //                e.SuppressKeyPress = e.Alt || e.Control;
                    //                if (e.Control) {
                    //                    switch (e.KeyCode) {
                    //                        case Keys.A: case Keys.C: case Keys.X: {
                    //                            e.SuppressKeyPress = false;
                    //                        } break;

                    //                        case Keys.V: {
                    //                            if (Clipboard.ContainsText() && Regex.IsMatch(Clipboard.GetText(), "^[a-zA-Z0-9]+$")) {
                    //                                e.SuppressKeyPress = false;
                    //                            }
                    //                        } break;
                    //                    }
                    //                }
                    //            } break;

                    //            case Keys.D1: case Keys.D2: case Keys.D3:
                    //            case Keys.D4: case Keys.D5: case Keys.D6:
                    //            case Keys.D7: case Keys.D8: case Keys.D9:
                    //            case Keys.D0: {
                    //                e.SuppressKeyPress = e.Control || e.Shift || e.Alt;
                    //            } break;

                    //            default: {
                    //                e.SuppressKeyPress = true;
                    //            } break;
                    //        }
                    //    } break;

                    //    case AllowedCharacters.All: {
                    //        e.SuppressKeyPress = false;
                    //    } break;

                    //}
                    #endregion
            }

            e.Handled = e.SuppressKeyPress;
            if (e.SuppressKeyPress) {
                System.Media.SystemSounds.Beep.Play();
            }

            base.OnKeyDown(e);
        }

        #endregion

        #region Events

        /// <summary>
        /// Event raised when the Button in the TextBox is clicked.
        /// </summary>
        public event EventHandler ButtonClick {
            add { InsetButton.Click += value; }
            remove { InsetButton.Click -= value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the button to fix appearance issues when size or alignment changes.
        /// </summary>
        private void UpdateButton() {
            InsetButton.Size = new Size(22, ClientSize.Height + 3);
            InsetButton.Location = _ButtonAlignment switch {
                ButtonAlignment.Right => new(0, -2),
                _ => new(ClientSize.Width - InsetButton.Width, -2),
            };
        }

        #endregion

    }
}