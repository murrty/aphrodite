namespace aphrodite {
    partial class frmArgumentDialog {
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
            this.panelOk = new System.Windows.Forms.Panel();
            this.btnMainForm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.lbDescription = new System.Windows.Forms.Label();
            this.txtArguments = new aphrodite.Controls.ExtendedTextBox();
            this.panelOk.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelOk
            // 
            this.panelOk.BackColor = System.Drawing.SystemColors.Menu;
            this.panelOk.Controls.Add(this.btnMainForm);
            this.panelOk.Controls.Add(this.btnCancel);
            this.panelOk.Controls.Add(this.btnDownload);
            this.panelOk.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelOk.Location = new System.Drawing.Point(0, 101);
            this.panelOk.Name = "panelOk";
            this.panelOk.Size = new System.Drawing.Size(332, 42);
            this.panelOk.TabIndex = 0;
            // 
            // btnMainForm
            // 
            this.btnMainForm.Location = new System.Drawing.Point(164, 9);
            this.btnMainForm.Name = "btnMainForm";
            this.btnMainForm.Size = new System.Drawing.Size(75, 23);
            this.btnMainForm.TabIndex = 3;
            this.btnMainForm.Text = "Main form";
            this.btnMainForm.UseVisualStyleBackColor = true;
            this.btnMainForm.Click += new System.EventHandler(this.btnMainForm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(83, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.Location = new System.Drawing.Point(245, 9);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(75, 23);
            this.btnDownload.TabIndex = 0;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // lbDescription
            // 
            this.lbDescription.Location = new System.Drawing.Point(12, 9);
            this.lbDescription.Name = "lbDescription";
            this.lbDescription.Size = new System.Drawing.Size(308, 43);
            this.lbDescription.TabIndex = 2;
            this.lbDescription.Text = "Verify the arguments for the {0} is correct below.";
            this.lbDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtArguments
            // 
            this.txtArguments.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtArguments.ButtonAlignment = aphrodite.Controls.ButtonAlignments.Left;
            this.txtArguments.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtArguments.ButtonEnabled = true;
            this.txtArguments.ButtonFont = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArguments.ButtonImageIndex = -1;
            this.txtArguments.ButtonImageKey = "";
            this.txtArguments.ButtonSize = new System.Drawing.Size(22, 19);
            this.txtArguments.ButtonText = "";
            this.txtArguments.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtArguments.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArguments.Location = new System.Drawing.Point(51, 58);
            this.txtArguments.Name = "txtArguments";
            this.txtArguments.Size = new System.Drawing.Size(230, 20);
            this.txtArguments.TabIndex = 1;
            this.txtArguments.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtArguments.TextHint = "Argument will appear here";
            this.txtArguments.TextType = aphrodite.Controls.AllowedTextTypes.AlphaNumericOnly;
            this.txtArguments.ButtonClick += new System.EventHandler(this.txtArguments_ButtonClick);
            // 
            // frmArgumentDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(332, 143);
            this.Controls.Add(this.lbDescription);
            this.Controls.Add(this.txtArguments);
            this.Controls.Add(this.panelOk);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(350, 180);
            this.MinimumSize = new System.Drawing.Size(350, 180);
            this.Name = "frmArgumentDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "arguments passed";
            this.panelOk.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelOk;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnCancel;
        private Controls.ExtendedTextBox txtArguments;
        private System.Windows.Forms.Label lbDescription;
        private System.Windows.Forms.Button btnMainForm;
    }
}