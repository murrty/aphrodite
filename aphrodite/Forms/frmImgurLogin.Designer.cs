namespace aphrodite {
    partial class frmImgurLogin {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.txtClientId = new murrty.controls.ExtendedTextBox();
            this.pnLower = new System.Windows.Forms.Panel();
            this.btnCancel = new murrty.controls.ExtendedButton();
            this.btnTestSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pnLower.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtClientId
            // 
            this.txtClientId.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtClientId.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtClientId.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtClientId.ButtonEnabled = true;
            this.txtClientId.ButtonFont = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClientId.ButtonImageIndex = -1;
            this.txtClientId.ButtonImageKey = "";
            this.txtClientId.ButtonSize = new System.Drawing.Size(22, 19);
            this.txtClientId.ButtonText = "X";
            this.txtClientId.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtClientId.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClientId.Location = new System.Drawing.Point(25, 49);
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.Size = new System.Drawing.Size(154, 20);
            this.txtClientId.SyncronizeFont = true;
            this.txtClientId.TabIndex = 0;
            this.txtClientId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtClientId.TextHint = "imgur client id...";
            this.txtClientId.TextType = murrty.controls.AllowedCharacters.AlphaNumericOnly;
            this.txtClientId.ButtonClick += new System.EventHandler(this.txtClientId_ButtonClick);
            this.txtClientId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtClientId_KeyPress);
            // 
            // pnLower
            // 
            this.pnLower.BackColor = System.Drawing.SystemColors.Menu;
            this.pnLower.Controls.Add(this.btnCancel);
            this.pnLower.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnLower.Location = new System.Drawing.Point(0, 91);
            this.pnLower.Name = "pnLower";
            this.pnLower.Size = new System.Drawing.Size(292, 42);
            this.pnLower.TabIndex = 29;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(209, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(71, 23);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnTestSave
            // 
            this.btnTestSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTestSave.Location = new System.Drawing.Point(193, 46);
            this.btnTestSave.Name = "btnTestSave";
            this.btnTestSave.Size = new System.Drawing.Size(75, 23);
            this.btnTestSave.TabIndex = 30;
            this.btnTestSave.Text = "test && save";
            this.btnTestSave.UseVisualStyleBackColor = true;
            this.btnTestSave.Click += new System.EventHandler(this.btnTestSave_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(280, 24);
            this.label1.TabIndex = 31;
            this.label1.Text = "you must specify your own client id";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmImgurLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(292, 133);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTestSave);
            this.Controls.Add(this.pnLower);
            this.Controls.Add(this.txtClientId);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(310, 170);
            this.MinimumSize = new System.Drawing.Size(310, 170);
            this.Name = "frmImgurLogin";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "login to imgur";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmImgurLogin_FormClosing);
            this.pnLower.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private murrty.controls.ExtendedTextBox txtClientId;
        private System.Windows.Forms.Panel pnLower;
        private murrty.controls.ExtendedButton btnCancel;
        private System.Windows.Forms.Button btnTestSave;
        private System.Windows.Forms.Label label1;
    }
}