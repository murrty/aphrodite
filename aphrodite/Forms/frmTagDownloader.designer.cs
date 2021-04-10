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
            this.status = new System.Windows.Forms.StatusBar();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.tmrTitle = new System.Windows.Forms.Timer(this.components);
            this.lbLimits = new System.Windows.Forms.Label();
            this.lbBlacklist = new System.Windows.Forms.Label();
            this.lbBytes = new System.Windows.Forms.Label();
            this.lbRemoved = new System.Windows.Forms.Label();
            this.lbPercentage = new System.Windows.Forms.Label();
            this.pbTotalStatus = new aphrodite.Controls.ExtendedProgressBar();
            this.pbDownloadStatus = new aphrodite.Controls.ExtendedProgressBar();
            this.SuspendLayout();
            // 
            // lbID
            // 
            this.lbID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbID.Location = new System.Drawing.Point(115, 2);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(82, 16);
            this.lbID.TabIndex = 0;
            this.lbID.Text = "Tags";
            this.lbID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbFile
            // 
            this.lbFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFile.Location = new System.Drawing.Point(12, 230);
            this.lbFile.Name = "lbFile";
            this.lbFile.Size = new System.Drawing.Size(288, 18);
            this.lbFile.TabIndex = 2;
            this.lbFile.Text = "Downloading file 0 of 0";
            this.lbFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(0, 321);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(312, 22);
            this.status.SizingGrip = false;
            this.status.TabIndex = 5;
            this.status.Text = "Waiting for initial start";
            // 
            // txtTags
            // 
            this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTags.Location = new System.Drawing.Point(12, 20);
            this.txtTags.Name = "txtTags";
            this.txtTags.ReadOnly = true;
            this.txtTags.Size = new System.Drawing.Size(288, 22);
            this.txtTags.TabIndex = 1;
            this.txtTags.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tmrTitle
            // 
            this.tmrTitle.Interval = 1000;
            this.tmrTitle.Tick += new System.EventHandler(this.tmrTitle_Tick);
            // 
            // lbLimits
            // 
            this.lbLimits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLimits.Location = new System.Drawing.Point(15, 172);
            this.lbLimits.Name = "lbLimits";
            this.lbLimits.Size = new System.Drawing.Size(285, 55);
            this.lbLimits.TabIndex = 8;
            this.lbLimits.Text = "Minimum score: disabled\r\nImage limit: disabled\r\nPage limit: disabled\r\nRatings: e," +
    " q, s (separating)";
            this.lbLimits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBlacklist
            // 
            this.lbBlacklist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbBlacklist.Location = new System.Drawing.Point(12, 46);
            this.lbBlacklist.Name = "lbBlacklist";
            this.lbBlacklist.Size = new System.Drawing.Size(288, 122);
            this.lbBlacklist.TabIndex = 7;
            this.lbBlacklist.Text = resources.GetString("lbBlacklist.Text");
            this.lbBlacklist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbBytes
            // 
            this.lbBytes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbBytes.Location = new System.Drawing.Point(12, 246);
            this.lbBytes.Name = "lbBytes";
            this.lbBytes.Size = new System.Drawing.Size(288, 20);
            this.lbBytes.TabIndex = 12;
            this.lbBytes.Text = "0kb / 0kb";
            this.lbBytes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbRemoved
            // 
            this.lbRemoved.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbRemoved.AutoSize = true;
            this.lbRemoved.Location = new System.Drawing.Point(86, 295);
            this.lbRemoved.Name = "lbRemoved";
            this.lbRemoved.Size = new System.Drawing.Size(141, 13);
            this.lbRemoved.TabIndex = 11;
            this.lbRemoved.Text = "Removed images detected";
            this.lbRemoved.Visible = false;
            // 
            // lbPercentage
            // 
            this.lbPercentage.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbPercentage.BackColor = System.Drawing.Color.Transparent;
            this.lbPercentage.Location = new System.Drawing.Point(136, 272);
            this.lbPercentage.Name = "lbPercentage";
            this.lbPercentage.Size = new System.Drawing.Size(41, 14);
            this.lbPercentage.TabIndex = 4;
            this.lbPercentage.Text = "0%";
            this.lbPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbTotalStatus
            // 
            this.pbTotalStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTotalStatus.ContainerControl = this;
            this.pbTotalStatus.Location = new System.Drawing.Point(12, 293);
            this.pbTotalStatus.Name = "pbTotalStatus";
            this.pbTotalStatus.ShowInTaskbar = true;
            this.pbTotalStatus.Size = new System.Drawing.Size(288, 18);
            this.pbTotalStatus.TabIndex = 9;
            // 
            // pbDownloadStatus
            // 
            this.pbDownloadStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbDownloadStatus.ContainerControl = this;
            this.pbDownloadStatus.Location = new System.Drawing.Point(12, 270);
            this.pbDownloadStatus.Name = "pbDownloadStatus";
            this.pbDownloadStatus.Size = new System.Drawing.Size(288, 18);
            this.pbDownloadStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbDownloadStatus.TabIndex = 10;
            // 
            // frmTagDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(312, 343);
            this.Controls.Add(this.lbBytes);
            this.Controls.Add(this.lbRemoved);
            this.Controls.Add(this.pbTotalStatus);
            this.Controls.Add(this.txtTags);
            this.Controls.Add(this.status);
            this.Controls.Add(this.lbPercentage);
            this.Controls.Add(this.lbFile);
            this.Controls.Add(this.lbID);
            this.Controls.Add(this.lbBlacklist);
            this.Controls.Add(this.lbLimits);
            this.Controls.Add(this.pbDownloadStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(330, 380);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(330, 380);
            this.Name = "frmTagDownloader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Downloading tags ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDownload_FormClosing);
            this.Load += new System.EventHandler(this.frmDownload_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbID;
        private System.Windows.Forms.Label lbFile;
        private System.Windows.Forms.StatusBar status;
        private System.Windows.Forms.TextBox txtTags;
        private System.Windows.Forms.Timer tmrTitle;
        private System.Windows.Forms.Label lbLimits;
        private Controls.ExtendedProgressBar pbTotalStatus;
        private Controls.ExtendedProgressBar pbDownloadStatus;
        private System.Windows.Forms.Label lbBlacklist;
        private System.Windows.Forms.Label lbBytes;
        private System.Windows.Forms.Label lbRemoved;
        private System.Windows.Forms.Label lbPercentage;
    }
}