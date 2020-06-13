﻿namespace aphrodite {
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
            this.lbPercentage = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.StatusBar();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.tmrTitle = new System.Windows.Forms.Timer(this.components);
            this.lbLimits = new System.Windows.Forms.Label();
            this.pbTotalStatus = new aphrodite.ExProgressBar();
            this.pbDownloadStatus = new aphrodite.ExProgressBar();
            this.lbRemoved = new System.Windows.Forms.Label();
            this.lbBlacklist = new System.Windows.Forms.Label();
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
            this.lbFile.Location = new System.Drawing.Point(12, 207);
            this.lbFile.Name = "lbFile";
            this.lbFile.Size = new System.Drawing.Size(288, 18);
            this.lbFile.TabIndex = 2;
            this.lbFile.Text = "Downloading file 0 of 0";
            this.lbFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPercentage
            // 
            this.lbPercentage.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbPercentage.BackColor = System.Drawing.Color.Transparent;
            this.lbPercentage.Location = new System.Drawing.Point(136, 229);
            this.lbPercentage.Name = "lbPercentage";
            this.lbPercentage.Size = new System.Drawing.Size(41, 14);
            this.lbPercentage.TabIndex = 4;
            this.lbPercentage.Text = "0%";
            this.lbPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(0, 278);
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
            this.txtTags.Size = new System.Drawing.Size(288, 20);
            this.txtTags.TabIndex = 1;
            this.txtTags.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tmrTitle
            // 
            this.tmrTitle.Enabled = true;
            this.tmrTitle.Interval = 1000;
            this.tmrTitle.Tick += new System.EventHandler(this.tmrTitle_Tick);
            // 
            // lbLimits
            // 
            this.lbLimits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLimits.Location = new System.Drawing.Point(15, 148);
            this.lbLimits.Name = "lbLimits";
            this.lbLimits.Size = new System.Drawing.Size(285, 55);
            this.lbLimits.TabIndex = 8;
            this.lbLimits.Text = "Minimum score: disabled\r\nImage limit: disabled\r\nPage limit: disabled\r\nRatings: e," +
    " q, s (separating)";
            this.lbLimits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbTotalStatus
            // 
            this.pbTotalStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTotalStatus.ContainerControl = this;
            this.pbTotalStatus.Location = new System.Drawing.Point(12, 250);
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
            this.pbDownloadStatus.Location = new System.Drawing.Point(12, 227);
            this.pbDownloadStatus.Maximum = 101;
            this.pbDownloadStatus.Name = "pbDownloadStatus";
            this.pbDownloadStatus.Size = new System.Drawing.Size(288, 18);
            this.pbDownloadStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbDownloadStatus.TabIndex = 10;
            // 
            // lbRemoved
            // 
            this.lbRemoved.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbRemoved.AutoSize = true;
            this.lbRemoved.Location = new System.Drawing.Point(89, 252);
            this.lbRemoved.Name = "lbRemoved";
            this.lbRemoved.Size = new System.Drawing.Size(134, 13);
            this.lbRemoved.TabIndex = 11;
            this.lbRemoved.Text = "Removed images detected";
            this.lbRemoved.Visible = false;
            // 
            // lbBlacklist
            // 
            this.lbBlacklist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbBlacklist.Location = new System.Drawing.Point(12, 47);
            this.lbBlacklist.Name = "lbBlacklist";
            this.lbBlacklist.Size = new System.Drawing.Size(288, 92);
            this.lbBlacklist.TabIndex = 7;
            this.lbBlacklist.Text = "Files: 0 ( 0 E | 0 Q | 0 S )\r\nBlacklisted: 0 ( 0 E | 0 Q | 0 S )\r\nZero Tolerance:" +
    " 0\r\nTotal: 0\r\n\r\nFiles that exist: 0 E | 0 Q | 0 S\r\nBlacklisted that exist: 0 E |" +
    " 0 Q | 0 S";
            this.lbBlacklist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmTagDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(312, 300);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 184);
            this.Name = "frmTagDownloader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Downloading tags ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDownload_FormClosing);
            this.Load += new System.EventHandler(this.frmDownload_Load);
            this.Shown += new System.EventHandler(this.frmDownload_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbID;
        private System.Windows.Forms.Label lbFile;
        private System.Windows.Forms.Label lbPercentage;
        private System.Windows.Forms.StatusBar status;
        private System.Windows.Forms.TextBox txtTags;
        private System.Windows.Forms.Timer tmrTitle;
        private System.Windows.Forms.Label lbLimits;
        private ExProgressBar pbTotalStatus;
        private ExProgressBar pbDownloadStatus;
        private System.Windows.Forms.Label lbRemoved;
        private System.Windows.Forms.Label lbBlacklist;
    }
}