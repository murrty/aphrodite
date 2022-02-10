using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace murrty.controls {

    /// <summary>
    /// Represents a label that does not render a background, rendering it as standard text.
    /// </summary>
    [System.Diagnostics.DebuggerStepThrough]
    internal class TransparentLabel : Control {

        #region Fields
        /// <summary>
        /// The <see cref="ContentAlignment"/> of the current label.
        /// </summary>
        private ContentAlignment fContentAlignment = ContentAlignment.MiddleLeft;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The <see cref="T:System.Drawing.Font"/> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont"/> property.
        /// </returns>
        [Category("Appearance")]
        [DefaultValue(default(Font))]
        [Description("The font that the label will use when rendering text.")]
        public new Font Font {
            get {
                return base.Font;
            }
            set {
                base.Font = value;
                base.RecreateHandle();
            }
        }

        /// <summary>
        /// Gets or sets whether the mouse will pass-through the control to any underlying components.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// Whether the mouse will pass-through the control.
        /// </returns>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("Indicates that the mouse will be passed through the control to any underlying components.")]
        public bool MousePassthrough {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// One of the <see cref="T:System.Windows.Forms.RightToLeft"/> values. The default is <see cref="F:System.Windows.Forms.RightToLeft.Inherit"/>.
        /// </returns>
        /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
        /// The assigned value is not one of the <see cref="T:System.Windows.Forms.RightToLeft"/> values.
        /// </exception>
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("The alignment of the text on the control.")]
        public override RightToLeft RightToLeft {
            get {
                return base.RightToLeft;
            }
            set {
                base.RightToLeft = value;
                RecreateHandle();
            }
        }

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The <see cref="T:System.Drawing.ContentAlignment"/> to align the text on the control. The default value is <see cref="F:System.Drawing.ContentAlignment.MiddleLeft"/>.
        /// </returns>
        /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
        /// The assigned value is not one of the <see cref="T:System.Windows.Forms.ContentAlignment"/> values.
        /// </exception>
        [Category("Appearance")]
        [DefaultValue(ContentAlignment.MiddleLeft)]
        [Description("The alignment of the text on the control.")]
        public ContentAlignment TextAlign {
            get {
                return fContentAlignment;
            }
            set {
                fContentAlignment = value;
                base.RecreateHandle();
            }
        }

        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        /// <returns>
        /// The text associated with this control.
        /// </returns>
        [Category("Appearance")]
        [Description("The alignment of the text on the control.")]
        public override string Text {
            get {
                return base.Text;
            }
            set {
                base.Text = value;
                RecreateHandle();
            }
        }
        #endregion

        #region Native Methods
        /// <summary>
        /// WindowProc message to tell the control to paint itself.
        /// </summary>
        private const int WM_PAINT = 0x000F;
        /// <summary>
        /// Param for the control to be transparent-aware.
        /// </summary>
        private const int WS_EX_TRANSPARENT = 0x0020;

        /// <summary>
        /// Sent as a WndProc message to determine where the cursor is.
        /// </summary>
        private const int WM_NCHITTEST = 0x0084;
        /// <summary>
        /// WM_NCHITTEST result for the cursor hit to be in a different window in the same thread.
        /// The same message is sent to underlying controls until one of them doesn't respond with it.
        /// </summary>
        private const int HTTRANSPARENT = -1;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new <see cref="TransparentLabel"/> instance.
        /// </summary>
        public TransparentLabel() {
            TabStop = false;
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Gets the creation parameters.
        /// </summary>
        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_TRANSPARENT;
                return cp;
            }
        }

        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);
            switch (m.Msg) {
                case WM_NCHITTEST: {
                    if (MousePassthrough && !DesignMode) m.Result = (IntPtr)HTTRANSPARENT;
                    else base.WndProc(ref m);
                } break;

                case WM_PAINT: {
                    DrawText();
                } break;
            }
        }

        /// <summary>
        /// Paints the control background.
        /// </summary>
        /// <param name="e">EventArgs regarding the painting event, not used in this control.</param>
        protected override void OnPaintBackground(PaintEventArgs e) {
            if (DesignMode) {
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                    Color.Red, 1, ButtonBorderStyle.Dashed,
                    Color.Red, 1, ButtonBorderStyle.Dashed,
                    Color.Red, 1, ButtonBorderStyle.Dashed,
                    Color.Red, 1, ButtonBorderStyle.Dashed
                );
            }
        }

        /// <summary>
        /// Paints the control text.
        /// </summary>
        /// <param name="e">EventArgs regarding the painting event, not used in this control.</param>
        protected override void OnPaint(PaintEventArgs e) => DrawText();
        #endregion

        #region Methods
        private void DrawText() {
            using Graphics TextGraphics = CreateGraphics();
            using SolidBrush TextBrush = new(ForeColor);
            SizeF size = TextGraphics.MeasureString(Text, Font);
            TextGraphics.DrawString(
                Text, Font, TextBrush,

                fContentAlignment switch {
                    ContentAlignment.TopLeft or
                    ContentAlignment.MiddleLeft or
                    ContentAlignment.BottomLeft => RightToLeft == RightToLeft.Yes ? (Width - size.Width) : -1,

                    ContentAlignment.TopCenter or
                    ContentAlignment.MiddleCenter or
                    ContentAlignment.BottomCenter => (Width - size.Width) / 2,

                    ContentAlignment.TopRight or
                    ContentAlignment.MiddleRight or
                    ContentAlignment.BottomRight => RightToLeft == RightToLeft.Yes ? -1 : (Width - size.Width),

                    _ => 0
                },

                fContentAlignment switch {
                    ContentAlignment.MiddleLeft or
                    ContentAlignment.MiddleCenter or
                    ContentAlignment.MiddleRight => (Height - size.Height) / 2,

                    ContentAlignment.TopLeft or
                    ContentAlignment.TopCenter or
                    ContentAlignment.TopRight => (Height - size.Height),

                    _ => 0
                }
            );
        }
        #endregion

    }
}
