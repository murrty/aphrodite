namespace aphrodite {
    partial class frmImageDownloader {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImageDownloader));
            this.status = new System.Windows.Forms.StatusBar();
            this.tmrTitle = new System.Windows.Forms.Timer(this.components);
            this.lbPercentage = new System.Windows.Forms.Label();
            this.pbDownloadStatus = new System.Windows.Forms.ProgressBar();
            this.lbInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(0, 78);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(284, 22);
            this.status.SizingGrip = false;
            this.status.TabIndex = 0;
            this.status.Text = "Waiting for initial start";
            // 
            // tmrTitle
            // 
            this.tmrTitle.Enabled = true;
            this.tmrTitle.Interval = 1000;
            this.tmrTitle.Tick += new System.EventHandler(this.tmrTitle_Tick);
            // 
            // lbPercentage
            // 
            this.lbPercentage.BackColor = System.Drawing.Color.Transparent;
            this.lbPercentage.Location = new System.Drawing.Point(122, 52);
            this.lbPercentage.Name = "lbPercentage";
            this.lbPercentage.Size = new System.Drawing.Size(41, 14);
            this.lbPercentage.TabIndex = 6;
            this.lbPercentage.Text = "0%";
            this.lbPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbDownloadStatus
            // 
            this.pbDownloadStatus.Location = new System.Drawing.Point(12, 50);
            this.pbDownloadStatus.MarqueeAnimationSpeed = 50;
            this.pbDownloadStatus.Maximum = 101;
            this.pbDownloadStatus.Name = "pbDownloadStatus";
            this.pbDownloadStatus.Size = new System.Drawing.Size(260, 18);
            this.pbDownloadStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbDownloadStatus.TabIndex = 5;
            // 
            // lbInfo
            // 
            this.lbInfo.Location = new System.Drawing.Point(12, 15);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(260, 25);
            this.lbInfo.TabIndex = 7;
            this.lbInfo.Text = "Waiting for parse";
            this.lbInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // frmImageDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 100);
            this.Controls.Add(this.lbInfo);
            this.Controls.Add(this.lbPercentage);
            this.Controls.Add(this.pbDownloadStatus);
            this.Controls.Add(this.status);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(292, 130);
            this.MinimumSize = new System.Drawing.Size(292, 130);
            this.Name = "frmImageDownloader";
            this.Text = "Downloading image ....";
            this.Load += new System.EventHandler(this.frmImageDownloader_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusBar status;
        private System.Windows.Forms.Timer tmrTitle;
        private System.Windows.Forms.Label lbPercentage;
        private System.Windows.Forms.ProgressBar pbDownloadStatus;
        private System.Windows.Forms.Label lbInfo;
    }
}