namespace aphrodite {
    partial class frmImgurDownloader {
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
            this.lbTagsHint = new System.Windows.Forms.Label();
            this.txtTags = new murrty.controls.ExtendedTextBox();
            this.sbStatus = new System.Windows.Forms.StatusBar();
            this.pbTotalStatus = new murrty.controls.ExtendedProgressBar();
            this.pbDownloadStatus = new murrty.controls.ExtendedProgressBar();
            this.lbFileStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbTagsHint
            // 
            this.lbTagsHint.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbTagsHint.Location = new System.Drawing.Point(9, 8);
            this.lbTagsHint.Name = "lbTagsHint";
            this.lbTagsHint.Size = new System.Drawing.Size(291, 17);
            this.lbTagsHint.TabIndex = 47;
            this.lbTagsHint.Text = "downloading the following album";
            this.lbTagsHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtTags
            // 
            this.txtTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTags.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtTags.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtTags.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTags.ButtonImageIndex = -1;
            this.txtTags.ButtonImageKey = "";
            this.txtTags.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtTags.ButtonText = "";
            this.txtTags.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtTags.Location = new System.Drawing.Point(12, 29);
            this.txtTags.Name = "txtTags";
            this.txtTags.ReadOnly = true;
            this.txtTags.Size = new System.Drawing.Size(288, 22);
            this.txtTags.TabIndex = 40;
            this.txtTags.Text = "this is an album im downloading";
            this.txtTags.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTags.TextHint = "";
            // 
            // sbStatus
            // 
            this.sbStatus.Location = new System.Drawing.Point(0, 181);
            this.sbStatus.Name = "sbStatus";
            this.sbStatus.Size = new System.Drawing.Size(312, 22);
            this.sbStatus.SizingGrip = false;
            this.sbStatus.TabIndex = 33;
            this.sbStatus.Text = "waiting for thread to start";
            // 
            // pbTotalStatus
            // 
            this.pbTotalStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTotalStatus.ContainerParent = this;
            this.pbTotalStatus.FastValueUpdate = true;
            this.pbTotalStatus.Location = new System.Drawing.Point(12, 153);
            this.pbTotalStatus.Name = "pbTotalStatus";
            this.pbTotalStatus.ShowInTaskbar = true;
            this.pbTotalStatus.Size = new System.Drawing.Size(288, 22);
            this.pbTotalStatus.State = murrty.controls.ProgressBarState.Normal;
            this.pbTotalStatus.Style = System.Windows.Forms.ProgressBarStyle.Blocks;
            this.pbTotalStatus.TabIndex = 31;
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
            this.pbDownloadStatus.Location = new System.Drawing.Point(12, 125);
            this.pbDownloadStatus.Name = "pbDownloadStatus";
            this.pbDownloadStatus.Size = new System.Drawing.Size(288, 22);
            this.pbDownloadStatus.State = murrty.controls.ProgressBarState.Normal;
            this.pbDownloadStatus.Style = System.Windows.Forms.ProgressBarStyle.Blocks;
            this.pbDownloadStatus.TabIndex = 32;
            this.pbDownloadStatus.Text = "waiting until api is finished being parsed";
            this.pbDownloadStatus.TextColor = System.Drawing.SystemColors.WindowText;
            this.pbDownloadStatus.TextFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lbFileStatus
            // 
            this.lbFileStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFileStatus.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFileStatus.Location = new System.Drawing.Point(12, 54);
            this.lbFileStatus.Name = "lbFileStatus";
            this.lbFileStatus.Size = new System.Drawing.Size(288, 66);
            this.lbFileStatus.TabIndex = 48;
            this.lbFileStatus.Text = "total files parsed: 0\r\n\r\nfiles to download: 0\r\n\r\nexisting files: 0";
            this.lbFileStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // frmImgurDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(312, 203);
            this.Controls.Add(this.lbTagsHint);
            this.Controls.Add(this.txtTags);
            this.Controls.Add(this.sbStatus);
            this.Controls.Add(this.pbTotalStatus);
            this.Controls.Add(this.pbDownloadStatus);
            this.Controls.Add(this.lbFileStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(330, 240);
            this.MinimumSize = new System.Drawing.Size(330, 240);
            this.Name = "frmImgurDownloader";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "downloading imgur album ....";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmImgurDownloader_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbTagsHint;
        private murrty.controls.ExtendedTextBox txtTags;
        private System.Windows.Forms.StatusBar sbStatus;
        private murrty.controls.ExtendedProgressBar pbTotalStatus;
        private murrty.controls.ExtendedProgressBar pbDownloadStatus;
        private System.Windows.Forms.Label lbFileStatus;
    }
}