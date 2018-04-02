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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRedownloader));
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tbTags = new System.Windows.Forms.TabPage();
            this.lbTags = new System.Windows.Forms.ListBox();
            this.tbPools = new System.Windows.Forms.TabPage();
            this.lbPools = new System.Windows.Forms.ListBox();
            this.btnRedownload = new System.Windows.Forms.Button();
            this.btnRenumerate = new System.Windows.Forms.Button();
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
            this.tcMain.Size = new System.Drawing.Size(292, 210);
            this.tcMain.TabIndex = 0;
            // 
            // tbTags
            // 
            this.tbTags.Controls.Add(this.lbTags);
            this.tbTags.Location = new System.Drawing.Point(4, 22);
            this.tbTags.Name = "tbTags";
            this.tbTags.Padding = new System.Windows.Forms.Padding(3);
            this.tbTags.Size = new System.Drawing.Size(284, 184);
            this.tbTags.TabIndex = 0;
            this.tbTags.Text = "Tags";
            this.tbTags.UseVisualStyleBackColor = true;
            // 
            // lbTags
            // 
            this.lbTags.FormattingEnabled = true;
            this.lbTags.Location = new System.Drawing.Point(8, 6);
            this.lbTags.Name = "lbTags";
            this.lbTags.Size = new System.Drawing.Size(270, 173);
            this.lbTags.TabIndex = 0;
            // 
            // tbPools
            // 
            this.tbPools.Controls.Add(this.lbPools);
            this.tbPools.Location = new System.Drawing.Point(4, 22);
            this.tbPools.Name = "tbPools";
            this.tbPools.Padding = new System.Windows.Forms.Padding(3);
            this.tbPools.Size = new System.Drawing.Size(284, 184);
            this.tbPools.TabIndex = 1;
            this.tbPools.Text = "Pools";
            this.tbPools.UseVisualStyleBackColor = true;
            // 
            // lbPools
            // 
            this.lbPools.FormattingEnabled = true;
            this.lbPools.Location = new System.Drawing.Point(8, 6);
            this.lbPools.Name = "lbPools";
            this.lbPools.Size = new System.Drawing.Size(268, 173);
            this.lbPools.TabIndex = 0;
            // 
            // btnRedownload
            // 
            this.btnRedownload.Location = new System.Drawing.Point(159, 212);
            this.btnRedownload.Name = "btnRedownload";
            this.btnRedownload.Size = new System.Drawing.Size(129, 23);
            this.btnRedownload.TabIndex = 1;
            this.btnRedownload.Text = "Redownload selected";
            this.btnRedownload.UseVisualStyleBackColor = true;
            this.btnRedownload.Click += new System.EventHandler(this.btnRedownload_Click);
            // 
            // btnRenumerate
            // 
            this.btnRenumerate.Location = new System.Drawing.Point(4, 212);
            this.btnRenumerate.Name = "btnRenumerate";
            this.btnRenumerate.Size = new System.Drawing.Size(76, 23);
            this.btnRenumerate.TabIndex = 2;
            this.btnRenumerate.Text = "Renumerate";
            this.btnRenumerate.UseVisualStyleBackColor = true;
            this.btnRenumerate.Click += new System.EventHandler(this.btnRenumerate_Click);
            // 
            // frmRedownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(292, 240);
            this.Controls.Add(this.btnRenumerate);
            this.Controls.Add(this.btnRedownload);
            this.Controls.Add(this.tcMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 270);
            this.MinimumSize = new System.Drawing.Size(300, 270);
            this.Name = "frmRedownloader";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Re-Downloader";
            this.Load += new System.EventHandler(this.frmTagRedownloader_Load);
            this.tcMain.ResumeLayout(false);
            this.tbTags.ResumeLayout(false);
            this.tbPools.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tbTags;
        private System.Windows.Forms.TabPage tbPools;
        private System.Windows.Forms.ListBox lbTags;
        private System.Windows.Forms.Button btnRedownload;
        private System.Windows.Forms.Button btnRenumerate;
        private System.Windows.Forms.ListBox lbPools;
    }
}