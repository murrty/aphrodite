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
            this.status = new System.Windows.Forms.StatusBar();
            this.tmrTitle = new System.Windows.Forms.Timer(this.components);
            this.lbPercentage = new System.Windows.Forms.Label();
            this.lbInfo = new System.Windows.Forms.Label();
            this.pbDownloadStatus = new aphrodite.Controls.ExtendedProgressBar();
            this.lbBytes = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(0, 101);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(282, 22);
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
            this.lbPercentage.Location = new System.Drawing.Point(121, 74);
            this.lbPercentage.Name = "lbPercentage";
            this.lbPercentage.Size = new System.Drawing.Size(41, 14);
            this.lbPercentage.TabIndex = 6;
            this.lbPercentage.Text = "0%";
            this.lbPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbInfo
            // 
            this.lbInfo.Location = new System.Drawing.Point(11, 13);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(260, 25);
            this.lbInfo.TabIndex = 7;
            this.lbInfo.Text = "Waiting for parse";
            this.lbInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbDownloadStatus
            // 
            this.pbDownloadStatus.ContainerControl = this;
            this.pbDownloadStatus.Location = new System.Drawing.Point(11, 72);
            this.pbDownloadStatus.Name = "pbDownloadStatus";
            this.pbDownloadStatus.ShowInTaskbar = true;
            this.pbDownloadStatus.Size = new System.Drawing.Size(260, 18);
            this.pbDownloadStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbDownloadStatus.TabIndex = 8;
            // 
            // lbBytes
            // 
            this.lbBytes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbBytes.Location = new System.Drawing.Point(-3, 46);
            this.lbBytes.Name = "lbBytes";
            this.lbBytes.Size = new System.Drawing.Size(288, 20);
            this.lbBytes.TabIndex = 13;
            this.lbBytes.Text = "0mb / 0mb";
            this.lbBytes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmImageDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 123);
            this.Controls.Add(this.lbBytes);
            this.Controls.Add(this.lbInfo);
            this.Controls.Add(this.lbPercentage);
            this.Controls.Add(this.status);
            this.Controls.Add(this.pbDownloadStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MinimumSize = new System.Drawing.Size(300, 140);
            this.Name = "frmImageDownloader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Downloading image ....";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmImageDownloader_FormClosing);
            this.Load += new System.EventHandler(this.frmImageDownloader_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.StatusBar status;
        private System.Windows.Forms.Timer tmrTitle;
        private System.Windows.Forms.Label lbPercentage;
        private System.Windows.Forms.Label lbInfo;
        private Controls.ExtendedProgressBar pbDownloadStatus;
        private System.Windows.Forms.Label lbBytes;
    }
}