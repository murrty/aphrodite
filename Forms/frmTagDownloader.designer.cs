namespace aphrodite {
    partial class frmTagDownloader {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTagDownloader));
            this.lbID = new System.Windows.Forms.Label();
            this.lbFile = new System.Windows.Forms.Label();
            this.pbDownloadStatus = new System.Windows.Forms.ProgressBar();
            this.lbPercentage = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.StatusBar();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.lbBlacklist = new System.Windows.Forms.Label();
            this.tmrTitle = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lbID
            // 
            this.lbID.Location = new System.Drawing.Point(115, 2);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(62, 16);
            this.lbID.TabIndex = 0;
            this.lbID.Text = "Tags";
            this.lbID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbFile
            // 
            this.lbFile.Location = new System.Drawing.Point(12, 102);
            this.lbFile.Name = "lbFile";
            this.lbFile.Size = new System.Drawing.Size(268, 18);
            this.lbFile.TabIndex = 2;
            this.lbFile.Text = "Downloading file 0 of 0";
            this.lbFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbDownloadStatus
            // 
            this.pbDownloadStatus.Location = new System.Drawing.Point(12, 121);
            this.pbDownloadStatus.Name = "pbDownloadStatus";
            this.pbDownloadStatus.Size = new System.Drawing.Size(268, 18);
            this.pbDownloadStatus.TabIndex = 3;
            // 
            // lbPercentage
            // 
            this.lbPercentage.BackColor = System.Drawing.Color.Transparent;
            this.lbPercentage.Location = new System.Drawing.Point(126, 123);
            this.lbPercentage.Name = "lbPercentage";
            this.lbPercentage.Size = new System.Drawing.Size(41, 14);
            this.lbPercentage.TabIndex = 4;
            this.lbPercentage.Text = "0%";
            this.lbPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(0, 148);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(292, 22);
            this.status.SizingGrip = false;
            this.status.TabIndex = 5;
            this.status.Text = "Waiting for initial start";
            // 
            // txtTags
            // 
            this.txtTags.Location = new System.Drawing.Point(12, 20);
            this.txtTags.Name = "txtTags";
            this.txtTags.ReadOnly = true;
            this.txtTags.Size = new System.Drawing.Size(268, 20);
            this.txtTags.TabIndex = 6;
            this.txtTags.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbBlacklist
            // 
            this.lbBlacklist.Location = new System.Drawing.Point(12, 45);
            this.lbBlacklist.Name = "lbBlacklist";
            this.lbBlacklist.Size = new System.Drawing.Size(268, 55);
            this.lbBlacklist.TabIndex = 7;
            this.lbBlacklist.Text = "0 posts\r\n0 blacklisted\r\n0 zero-toleranced\r\n0 in total";
            this.lbBlacklist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrTitle
            // 
            this.tmrTitle.Enabled = true;
            this.tmrTitle.Interval = 1000;
            this.tmrTitle.Tick += new System.EventHandler(this.tmrTitle_Tick);
            // 
            // frmTagDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(292, 170);
            this.Controls.Add(this.txtTags);
            this.Controls.Add(this.status);
            this.Controls.Add(this.lbPercentage);
            this.Controls.Add(this.pbDownloadStatus);
            this.Controls.Add(this.lbFile);
            this.Controls.Add(this.lbID);
            this.Controls.Add(this.lbBlacklist);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 184);
            this.Name = "frmTagDownloader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Downloading tags ....";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDownload_FormClosing);
            this.Load += new System.EventHandler(this.frmDownload_Load);
            this.Shown += new System.EventHandler(this.frmDownload_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbID;
        private System.Windows.Forms.Label lbFile;
        private System.Windows.Forms.ProgressBar pbDownloadStatus;
        private System.Windows.Forms.Label lbPercentage;
        private System.Windows.Forms.StatusBar status;
        private System.Windows.Forms.TextBox txtTags;
        private System.Windows.Forms.Label lbBlacklist;
        private System.Windows.Forms.Timer tmrTitle;
    }
}