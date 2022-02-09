namespace aphrodite {
    partial class frmPoolDownloader {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPoolDownloader));
            this.pbTotalStatus = new murrty.controls.ExtendedProgressBar();
            this.pbDownloadStatus = new murrty.controls.ExtendedProgressBar();
            this.sbStatus = new System.Windows.Forms.StatusBar();
            this.txtPoolName = new murrty.controls.ExtendedTextBox();
            this.lbPoolId = new System.Windows.Forms.Label();
            this.lbFileStatus = new System.Windows.Forms.Label();
            this.chkMergeGraylistedPages = new System.Windows.Forms.CheckBox();
            this.chkMergeBlacklistedPages = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // pbTotalStatus
            // 
            this.pbTotalStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTotalStatus.ContainerParent = this;
            this.pbTotalStatus.FastValueUpdate = true;
            this.pbTotalStatus.Location = new System.Drawing.Point(12, 283);
            this.pbTotalStatus.Name = "pbTotalStatus";
            this.pbTotalStatus.State = murrty.controls.ProgressBarState.Normal;
            this.pbTotalStatus.ShowInTaskbar = true;
            this.pbTotalStatus.Size = new System.Drawing.Size(248, 22);
            this.pbTotalStatus.Style = System.Windows.Forms.ProgressBarStyle.Blocks;
            this.pbTotalStatus.TabIndex = 13;
            this.pbTotalStatus.Text = "0 files to download";
            this.pbTotalStatus.TextColor = System.Drawing.SystemColors.WindowText;
            this.pbTotalStatus.TextFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // pbDownloadStatus
            // 
            this.pbDownloadStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbDownloadStatus.ContainerParent = this;
            this.pbDownloadStatus.FastValueUpdate = true;
            this.pbDownloadStatus.Location = new System.Drawing.Point(12, 254);
            this.pbDownloadStatus.Name = "pbDownloadStatus";
            this.pbDownloadStatus.State = murrty.controls.ProgressBarState.Normal;
            this.pbDownloadStatus.Size = new System.Drawing.Size(248, 22);
            this.pbDownloadStatus.Style = System.Windows.Forms.ProgressBarStyle.Blocks;
            this.pbDownloadStatus.TabIndex = 14;
            this.pbDownloadStatus.Text = "waiting until api is finished being parsed";
            this.pbDownloadStatus.TextColor = System.Drawing.SystemColors.WindowText;
            this.pbDownloadStatus.TextFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // sbStatus
            // 
            this.sbStatus.Location = new System.Drawing.Point(0, 311);
            this.sbStatus.Name = "sbStatus";
            this.sbStatus.Size = new System.Drawing.Size(272, 22);
            this.sbStatus.SizingGrip = false;
            this.sbStatus.TabIndex = 15;
            this.sbStatus.Text = "waiting for thread to start";
            // 
            // txtPoolName
            // 
            this.txtPoolName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPoolName.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtPoolName.ButtonCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPoolName.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPoolName.ButtonImageIndex = -1;
            this.txtPoolName.ButtonImageKey = "";
            this.txtPoolName.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtPoolName.ButtonText = "";
            this.txtPoolName.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtPoolName.Location = new System.Drawing.Point(12, 29);
            this.txtPoolName.Name = "txtPoolName";
            this.txtPoolName.ReadOnly = true;
            this.txtPoolName.Size = new System.Drawing.Size(248, 22);
            this.txtPoolName.TabIndex = 17;
            this.txtPoolName.Text = "[name will appear after first parse]";
            this.txtPoolName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPoolName.TextHint = "";
            // 
            // lbPoolId
            // 
            this.lbPoolId.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbPoolId.Location = new System.Drawing.Point(56, 9);
            this.lbPoolId.Name = "lbPoolId";
            this.lbPoolId.Size = new System.Drawing.Size(160, 14);
            this.lbPoolId.TabIndex = 16;
            this.lbPoolId.Text = "Pool ID will appear here";
            this.lbPoolId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbFileStatus
            // 
            this.lbFileStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFileStatus.Location = new System.Drawing.Point(12, 54);
            this.lbFileStatus.Name = "lbFileStatus";
            this.lbFileStatus.Size = new System.Drawing.Size(248, 147);
            this.lbFileStatus.TabIndex = 18;
            this.lbFileStatus.Text = resources.GetString("lbFileStatus.Text");
            this.lbFileStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkMergeGraylistedPages
            // 
            this.chkMergeGraylistedPages.AutoSize = true;
            this.chkMergeGraylistedPages.Enabled = false;
            this.chkMergeGraylistedPages.Location = new System.Drawing.Point(66, 206);
            this.chkMergeGraylistedPages.Name = "chkMergeGraylistedPages";
            this.chkMergeGraylistedPages.Size = new System.Drawing.Size(144, 17);
            this.chkMergeGraylistedPages.TabIndex = 19;
            this.chkMergeGraylistedPages.Text = "merge graylisted pages";
            this.chkMergeGraylistedPages.UseVisualStyleBackColor = true;
            // 
            // chkMergeBlacklistedPages
            // 
            this.chkMergeBlacklistedPages.AutoSize = true;
            this.chkMergeBlacklistedPages.Enabled = false;
            this.chkMergeBlacklistedPages.Location = new System.Drawing.Point(66, 229);
            this.chkMergeBlacklistedPages.Name = "chkMergeBlacklistedPages";
            this.chkMergeBlacklistedPages.Size = new System.Drawing.Size(149, 17);
            this.chkMergeBlacklistedPages.TabIndex = 20;
            this.chkMergeBlacklistedPages.Text = "merge blacklisted pages";
            this.chkMergeBlacklistedPages.UseVisualStyleBackColor = true;
            // 
            // frmPoolDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 333);
            this.Controls.Add(this.chkMergeBlacklistedPages);
            this.Controls.Add(this.chkMergeGraylistedPages);
            this.Controls.Add(this.lbFileStatus);
            this.Controls.Add(this.txtPoolName);
            this.Controls.Add(this.lbPoolId);
            this.Controls.Add(this.sbStatus);
            this.Controls.Add(this.pbTotalStatus);
            this.Controls.Add(this.pbDownloadStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(290, 370);
            this.MinimumSize = new System.Drawing.Size(290, 370);
            this.Name = "frmPoolDownloader";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Downloading pool ....";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPoolDownloaderUpdated_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private murrty.controls.ExtendedProgressBar pbTotalStatus;
        private murrty.controls.ExtendedProgressBar pbDownloadStatus;
        private System.Windows.Forms.StatusBar sbStatus;
        private murrty.controls.ExtendedTextBox txtPoolName;
        private System.Windows.Forms.Label lbPoolId;
        private System.Windows.Forms.Label lbFileStatus;
        private System.Windows.Forms.CheckBox chkMergeBlacklistedPages;
        private System.Windows.Forms.CheckBox chkMergeGraylistedPages;
    }
}