namespace aphrodite {
    partial class frmImageDownloader {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImageDownloader));
            this.lbTagsHint = new System.Windows.Forms.Label();
            this.txtPostId = new murrty.controls.ExtendedTextBox();
            this.sbStatus = new System.Windows.Forms.StatusBar();
            this.pbDownloadStatus = new murrty.controls.ExtendedProgressBar();
            this.SuspendLayout();
            // 
            // lbTagsHint
            // 
            this.lbTagsHint.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbTagsHint.Location = new System.Drawing.Point(9, 3);
            this.lbTagsHint.Name = "lbTagsHint";
            this.lbTagsHint.Size = new System.Drawing.Size(291, 17);
            this.lbTagsHint.TabIndex = 32;
            this.lbTagsHint.Text = "downloading image";
            this.lbTagsHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPostId
            // 
            this.txtPostId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPostId.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtPostId.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtPostId.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPostId.ButtonImageIndex = -1;
            this.txtPostId.ButtonImageKey = "";
            this.txtPostId.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtPostId.ButtonText = "";
            this.txtPostId.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtPostId.Location = new System.Drawing.Point(12, 24);
            this.txtPostId.Name = "txtPostId";
            this.txtPostId.ReadOnly = true;
            this.txtPostId.Size = new System.Drawing.Size(288, 22);
            this.txtPostId.TabIndex = 31;
            this.txtPostId.Text = "this is the image id";
            this.txtPostId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPostId.TextHint = "";
            // 
            // sbStatus
            // 
            this.sbStatus.Location = new System.Drawing.Point(0, 81);
            this.sbStatus.Name = "sbStatus";
            this.sbStatus.Size = new System.Drawing.Size(312, 22);
            this.sbStatus.SizingGrip = false;
            this.sbStatus.TabIndex = 30;
            this.sbStatus.Text = "waiting for thread to start";
            // 
            // pbDownloadStatus
            // 
            this.pbDownloadStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbDownloadStatus.ContainerParent = this;
            this.pbDownloadStatus.FastValueUpdate = true;
            this.pbDownloadStatus.Location = new System.Drawing.Point(12, 53);
            this.pbDownloadStatus.Name = "pbDownloadStatus";
            this.pbDownloadStatus.ShowInTaskbar = true;
            this.pbDownloadStatus.Size = new System.Drawing.Size(288, 22);
            this.pbDownloadStatus.State = murrty.controls.ProgressBarState.Normal;
            this.pbDownloadStatus.Style = System.Windows.Forms.ProgressBarStyle.Blocks;
            this.pbDownloadStatus.TabIndex = 29;
            this.pbDownloadStatus.Text = "waiting until api is finished being parsed";
            this.pbDownloadStatus.TextColor = System.Drawing.SystemColors.WindowText;
            this.pbDownloadStatus.TextFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // frmImageDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 103);
            this.Controls.Add(this.lbTagsHint);
            this.Controls.Add(this.txtPostId);
            this.Controls.Add(this.sbStatus);
            this.Controls.Add(this.pbDownloadStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.Location = new System.Drawing.Point(330, 140);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(330, 140);
            this.Name = "frmImageDownloader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Downloading image ....";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmImageDownloaderUpdated_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbTagsHint;
        private murrty.controls.ExtendedTextBox txtPostId;
        private System.Windows.Forms.StatusBar sbStatus;
        private murrty.controls.ExtendedProgressBar pbDownloadStatus;
    }
}