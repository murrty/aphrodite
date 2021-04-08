namespace aphrodite {
    partial class frmRedownloader {
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
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tbTags = new System.Windows.Forms.TabPage();
            this.lbTags = new aphrodite.Controls.AeroListBox();
            this.tbPools = new System.Windows.Forms.TabPage();
            this.lbPools = new aphrodite.Controls.AeroListBox();
            this.btnRedownload = new aphrodite.Controls.ExtendedButton();
            this.btnRenumerate = new aphrodite.Controls.ExtendedButton();
            this.lbDownloadedOn = new System.Windows.Forms.Label();
            this.tcMain.SuspendLayout();
            this.tbTags.SuspendLayout();
            this.tbPools.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tbTags);
            this.tcMain.Controls.Add(this.tbPools);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(292, 205);
            this.tcMain.TabIndex = 0;
            // 
            // tbTags
            // 
            this.tbTags.Controls.Add(this.lbTags);
            this.tbTags.Location = new System.Drawing.Point(4, 22);
            this.tbTags.Name = "tbTags";
            this.tbTags.Padding = new System.Windows.Forms.Padding(3);
            this.tbTags.Size = new System.Drawing.Size(284, 179);
            this.tbTags.TabIndex = 0;
            this.tbTags.Text = "Tags";
            this.tbTags.UseVisualStyleBackColor = true;
            // 
            // lbTags
            // 
            this.lbTags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTags.FormattingEnabled = true;
            this.lbTags.Location = new System.Drawing.Point(3, 3);
            this.lbTags.Name = "lbTags";
            this.lbTags.Size = new System.Drawing.Size(278, 173);
            this.lbTags.TabIndex = 0;
            this.lbTags.SelectedIndexChanged += new System.EventHandler(this.lbTags_SelectedIndexChanged);
            this.lbTags.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbTags_MouseDoubleClick);
            // 
            // tbPools
            // 
            this.tbPools.Controls.Add(this.lbPools);
            this.tbPools.Location = new System.Drawing.Point(4, 22);
            this.tbPools.Name = "tbPools";
            this.tbPools.Padding = new System.Windows.Forms.Padding(3);
            this.tbPools.Size = new System.Drawing.Size(284, 179);
            this.tbPools.TabIndex = 1;
            this.tbPools.Text = "Pools";
            this.tbPools.UseVisualStyleBackColor = true;
            // 
            // lbPools
            // 
            this.lbPools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPools.FormattingEnabled = true;
            this.lbPools.Location = new System.Drawing.Point(3, 3);
            this.lbPools.Name = "lbPools";
            this.lbPools.Size = new System.Drawing.Size(278, 173);
            this.lbPools.TabIndex = 0;
            this.lbPools.SelectedIndexChanged += new System.EventHandler(this.lbPools_SelectedIndexChanged);
            this.lbPools.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbPools_MouseDoubleClick);
            // 
            // btnRedownload
            // 
            this.btnRedownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRedownload.Location = new System.Drawing.Point(152, 239);
            this.btnRedownload.Name = "btnRedownload";
            this.btnRedownload.ShowUACShield = false;
            this.btnRedownload.Size = new System.Drawing.Size(130, 23);
            this.btnRedownload.TabIndex = 1;
            this.btnRedownload.Text = "Redownload selected";
            this.btnRedownload.UseVisualStyleBackColor = true;
            this.btnRedownload.Click += new System.EventHandler(this.btnRedownload_Click);
            // 
            // btnRenumerate
            // 
            this.btnRenumerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRenumerate.Location = new System.Drawing.Point(10, 239);
            this.btnRenumerate.Name = "btnRenumerate";
            this.btnRenumerate.ShowUACShield = false;
            this.btnRenumerate.Size = new System.Drawing.Size(80, 23);
            this.btnRenumerate.TabIndex = 2;
            this.btnRenumerate.Text = "Renumerate";
            this.btnRenumerate.UseVisualStyleBackColor = true;
            this.btnRenumerate.Click += new System.EventHandler(this.btnRenumerate_Click);
            // 
            // lbDownloadedOn
            // 
            this.lbDownloadedOn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbDownloadedOn.AutoSize = true;
            this.lbDownloadedOn.Location = new System.Drawing.Point(12, 208);
            this.lbDownloadedOn.Name = "lbDownloadedOn";
            this.lbDownloadedOn.Size = new System.Drawing.Size(233, 26);
            this.lbDownloadedOn.TabIndex = 3;
            this.lbDownloadedOn.Text = "The name of the tags/pool will appear here.\r\nThe date downloaded will appear here" +
    ".";
            // 
            // frmRedownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(292, 269);
            this.Controls.Add(this.lbDownloadedOn);
            this.Controls.Add(this.btnRenumerate);
            this.Controls.Add(this.btnRedownload);
            this.Controls.Add(this.tcMain);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(310, 306);
            this.Name = "frmRedownloader";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Re-Downloader";
            this.Load += new System.EventHandler(this.frmTagRedownloader_Load);
            this.tcMain.ResumeLayout(false);
            this.tbTags.ResumeLayout(false);
            this.tbPools.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tbTags;
        private System.Windows.Forms.TabPage tbPools;
        private Controls.AeroListBox lbTags;
        private Controls.ExtendedButton btnRedownload;
        private Controls.ExtendedButton btnRenumerate;
        private Controls.AeroListBox lbPools;
        private System.Windows.Forms.Label lbDownloadedOn;
    }
}