namespace aphrodite {
    partial class frmAbout {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.lbVersion = new System.Windows.Forms.Label();
            this.llbCheckForUpdates = new System.Windows.Forms.LinkLabel();
            this.lbHeader = new System.Windows.Forms.Label();
            this.lbBody = new System.Windows.Forms.Label();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.tipWoof = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Location = new System.Drawing.Point(178, 15);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(28, 13);
            this.lbVersion.TabIndex = 2;
            this.lbVersion.Text = "v0.0";
            // 
            // llbCheckForUpdates
            // 
            this.llbCheckForUpdates.AutoSize = true;
            this.llbCheckForUpdates.LinkColor = System.Drawing.Color.Blue;
            this.llbCheckForUpdates.Location = new System.Drawing.Point(85, 120);
            this.llbCheckForUpdates.Name = "llbCheckForUpdates";
            this.llbCheckForUpdates.Size = new System.Drawing.Size(94, 13);
            this.llbCheckForUpdates.TabIndex = 0;
            this.llbCheckForUpdates.TabStop = true;
            this.llbCheckForUpdates.Text = "Check for updates";
            this.llbCheckForUpdates.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbCheckForUpdates_LinkClicked);
            // 
            // lbHeader
            // 
            this.lbHeader.AutoSize = true;
            this.lbHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHeader.Location = new System.Drawing.Point(97, 8);
            this.lbHeader.Name = "lbHeader";
            this.lbHeader.Size = new System.Drawing.Size(85, 21);
            this.lbHeader.TabIndex = 1;
            this.lbHeader.Text = "aphrodite";
            // 
            // lbBody
            // 
            this.lbBody.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBody.Location = new System.Drawing.Point(12, 36);
            this.lbBody.Name = "lbBody";
            this.lbBody.Size = new System.Drawing.Size(248, 79);
            this.lbBody.TabIndex = 3;
            this.lbBody.Text = "aphrodite  by murrty\r\nicon is brad the bread.\r\ncredit to fuz for being fuz\r\ncoded" +
    " in VisualStudio 2013";
            this.lbBody.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pbIcon
            // 
            this.pbIcon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbIcon.Image = ((System.Drawing.Image)(resources.GetObject("pbIcon.Image")));
            this.pbIcon.InitialImage = null;
            this.pbIcon.Location = new System.Drawing.Point(64, 3);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(32, 32);
            this.pbIcon.TabIndex = 4;
            this.pbIcon.TabStop = false;
            this.tipWoof.SetToolTip(this.pbIcon, "bark bark (rough translation: click me)");
            this.pbIcon.Click += new System.EventHandler(this.pbIcon_Click);
            // 
            // frmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(264, 141);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.pbIcon);
            this.Controls.Add(this.lbBody);
            this.Controls.Add(this.lbHeader);
            this.Controls.Add(this.llbCheckForUpdates);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(280, 180);
            this.MinimumSize = new System.Drawing.Size(280, 180);
            this.Name = "frmAbout";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About aphrodite";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.About_FormClosing);
            this.Shown += new System.EventHandler(this.About_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.LinkLabel llbCheckForUpdates;
        private System.Windows.Forms.Label lbHeader;
        private System.Windows.Forms.Label lbBody;
        private System.Windows.Forms.PictureBox pbIcon;
        private System.Windows.Forms.ToolTip tipWoof;
    }
}