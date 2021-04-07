namespace aphrodite {
    partial class frmLicensing {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLicensing));
            this.rtbLicenses = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtbLicenses
            // 
            this.rtbLicenses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLicenses.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbLicenses.Location = new System.Drawing.Point(0, 0);
            this.rtbLicenses.Name = "rtbLicenses";
            this.rtbLicenses.ReadOnly = true;
            this.rtbLicenses.Size = new System.Drawing.Size(522, 263);
            this.rtbLicenses.TabIndex = 0;
            this.rtbLicenses.Text = resources.GetString("rtbLicenses.Text");
            // 
            // frmLicensing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 263);
            this.Controls.Add(this.rtbLicenses);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(538, 300);
            this.Name = "frmLicensing";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Licensing";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbLicenses;
    }
}