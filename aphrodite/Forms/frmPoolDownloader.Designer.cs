namespace aphrodite {
    partial class frmPoolDownloader {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPoolDownloader));
            this.lbID = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.lbFile = new System.Windows.Forms.Label();
            this.pbDownloadStatus = new System.Windows.Forms.ProgressBar();
            this.lbPercentage = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.StatusBar();
            this.lbTotal = new System.Windows.Forms.Label();
            this.tmrTitle = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // lbID
            // 
            this.lbID.Location = new System.Drawing.Point(84, 5);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(100, 14);
            this.lbID.TabIndex = 0;
            this.lbID.Text = "Pool ID ???";
            this.lbID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbName
            // 
            this.lbName.Location = new System.Drawing.Point(12, 19);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(244, 14);
            this.lbName.TabIndex = 1;
            this.lbName.Text = "unknown pool name";
            this.lbName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbFile
            // 
            this.lbFile.Location = new System.Drawing.Point(12, 90);
            this.lbFile.Name = "lbFile";
            this.lbFile.Size = new System.Drawing.Size(244, 14);
            this.lbFile.TabIndex = 2;
            this.lbFile.Text = "File ? of ?";
            this.lbFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbDownloadStatus
            // 
            this.pbDownloadStatus.Location = new System.Drawing.Point(12, 107);
            this.pbDownloadStatus.MarqueeAnimationSpeed = 50;
            this.pbDownloadStatus.Maximum = 101;
            this.pbDownloadStatus.Name = "pbDownloadStatus";
            this.pbDownloadStatus.Size = new System.Drawing.Size(244, 18);
            this.pbDownloadStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbDownloadStatus.TabIndex = 3;
            // 
            // lbPercentage
            // 
            this.lbPercentage.BackColor = System.Drawing.Color.Transparent;
            this.lbPercentage.Location = new System.Drawing.Point(114, 109);
            this.lbPercentage.Name = "lbPercentage";
            this.lbPercentage.Size = new System.Drawing.Size(41, 14);
            this.lbPercentage.TabIndex = 4;
            this.lbPercentage.Text = "0%";
            this.lbPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(0, 133);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(268, 22);
            this.status.SizingGrip = false;
            this.status.TabIndex = 5;
            this.status.Text = "Waiting for initial start";
            // 
            // lbTotal
            // 
            this.lbTotal.Location = new System.Drawing.Point(12, 43);
            this.lbTotal.Name = "lbTotal";
            this.lbTotal.Size = new System.Drawing.Size(244, 38);
            this.lbTotal.TabIndex = 6;
            this.lbTotal.Text = "? posts\r\n? blacklisted\r\n? total";
            this.lbTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrTitle
            // 
            this.tmrTitle.Enabled = true;
            this.tmrTitle.Interval = 1000;
            this.tmrTitle.Tick += new System.EventHandler(this.tmrTitle_Tick);
            // 
            // frmPoolDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(268, 155);
            this.Controls.Add(this.status);
            this.Controls.Add(this.lbPercentage);
            this.Controls.Add(this.pbDownloadStatus);
            this.Controls.Add(this.lbFile);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.lbID);
            this.Controls.Add(this.lbTotal);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(276, 185);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(276, 185);
            this.Name = "frmPoolDownloader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Downloading pool ....";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDownload_FormClosing);
            this.Load += new System.EventHandler(this.frmDownload_Load);
            this.Shown += new System.EventHandler(this.frmDownload_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbID;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label lbFile;
        private System.Windows.Forms.ProgressBar pbDownloadStatus;
        private System.Windows.Forms.Label lbPercentage;
        private System.Windows.Forms.StatusBar status;
        private System.Windows.Forms.Label lbTotal;
        private System.Windows.Forms.Timer tmrTitle;
    }
}