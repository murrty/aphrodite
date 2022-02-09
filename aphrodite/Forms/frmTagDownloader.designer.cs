namespace aphrodite {
    partial class frmTagDownloader {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTagDownloader));
            this.sbStatus = new System.Windows.Forms.StatusBar();
            this.chkExplicit = new System.Windows.Forms.CheckBox();
            this.chkQuestionable = new System.Windows.Forms.CheckBox();
            this.chkSafe = new System.Windows.Forms.CheckBox();
            this.chkSeparateRatings = new System.Windows.Forms.CheckBox();
            this.chkSeparateNonImages = new System.Windows.Forms.CheckBox();
            this.chkMinimumScore = new System.Windows.Forms.CheckBox();
            this.lbImageLimit = new System.Windows.Forms.Label();
            this.lbPageLimit = new System.Windows.Forms.Label();
            this.lbTagsHint = new System.Windows.Forms.Label();
            this.lbFileStatus = new System.Windows.Forms.Label();
            this.txtPageLimit = new murrty.controls.ExtendedTextBox();
            this.txtImageLimit = new murrty.controls.ExtendedTextBox();
            this.txtMinimumFavCount = new murrty.controls.ExtendedTextBox();
            this.txtMinimumScore = new murrty.controls.ExtendedTextBox();
            this.txtTags = new murrty.controls.ExtendedTextBox();
            this.pbTotalStatus = new murrty.controls.ExtendedProgressBar();
            this.pbDownloadStatus = new murrty.controls.ExtendedProgressBar();
            this.lbMinimumFavoriteCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // sbStatus
            // 
            this.sbStatus.Location = new System.Drawing.Point(0, 351);
            this.sbStatus.Name = "sbStatus";
            this.sbStatus.Size = new System.Drawing.Size(312, 22);
            this.sbStatus.SizingGrip = false;
            this.sbStatus.TabIndex = 13;
            this.sbStatus.Text = "waiting for thread to start";
            // 
            // chkExplicit
            // 
            this.chkExplicit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.chkExplicit.AutoSize = true;
            this.chkExplicit.Checked = true;
            this.chkExplicit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExplicit.Enabled = false;
            this.chkExplicit.Location = new System.Drawing.Point(49, 202);
            this.chkExplicit.Name = "chkExplicit";
            this.chkExplicit.Size = new System.Drawing.Size(61, 17);
            this.chkExplicit.TabIndex = 14;
            this.chkExplicit.Text = "explicit";
            this.chkExplicit.UseVisualStyleBackColor = true;
            // 
            // chkQuestionable
            // 
            this.chkQuestionable.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.chkQuestionable.AutoSize = true;
            this.chkQuestionable.Checked = true;
            this.chkQuestionable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkQuestionable.Enabled = false;
            this.chkQuestionable.Location = new System.Drawing.Point(116, 202);
            this.chkQuestionable.Name = "chkQuestionable";
            this.chkQuestionable.Size = new System.Drawing.Size(93, 17);
            this.chkQuestionable.TabIndex = 15;
            this.chkQuestionable.Text = "questionable";
            this.chkQuestionable.UseVisualStyleBackColor = true;
            // 
            // chkSafe
            // 
            this.chkSafe.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.chkSafe.AutoSize = true;
            this.chkSafe.Checked = true;
            this.chkSafe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSafe.Enabled = false;
            this.chkSafe.Location = new System.Drawing.Point(216, 202);
            this.chkSafe.Name = "chkSafe";
            this.chkSafe.Size = new System.Drawing.Size(46, 17);
            this.chkSafe.TabIndex = 16;
            this.chkSafe.Text = "safe";
            this.chkSafe.UseVisualStyleBackColor = true;
            // 
            // chkSeparateRatings
            // 
            this.chkSeparateRatings.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.chkSeparateRatings.AutoSize = true;
            this.chkSeparateRatings.Checked = true;
            this.chkSeparateRatings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSeparateRatings.Enabled = false;
            this.chkSeparateRatings.Location = new System.Drawing.Point(21, 222);
            this.chkSeparateRatings.Name = "chkSeparateRatings";
            this.chkSeparateRatings.Size = new System.Drawing.Size(119, 17);
            this.chkSeparateRatings.TabIndex = 17;
            this.chkSeparateRatings.Text = "separating ratings";
            this.chkSeparateRatings.UseVisualStyleBackColor = true;
            // 
            // chkSeparateNonImages
            // 
            this.chkSeparateNonImages.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.chkSeparateNonImages.AutoSize = true;
            this.chkSeparateNonImages.Checked = true;
            this.chkSeparateNonImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSeparateNonImages.Enabled = false;
            this.chkSeparateNonImages.Location = new System.Drawing.Point(147, 222);
            this.chkSeparateNonImages.Name = "chkSeparateNonImages";
            this.chkSeparateNonImages.Size = new System.Drawing.Size(144, 17);
            this.chkSeparateNonImages.TabIndex = 18;
            this.chkSeparateNonImages.Text = "separating non-images";
            this.chkSeparateNonImages.UseVisualStyleBackColor = true;
            // 
            // chkMinimumScore
            // 
            this.chkMinimumScore.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.chkMinimumScore.AutoSize = true;
            this.chkMinimumScore.Checked = true;
            this.chkMinimumScore.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMinimumScore.Enabled = false;
            this.chkMinimumScore.Location = new System.Drawing.Point(55, 245);
            this.chkMinimumScore.Name = "chkMinimumScore";
            this.chkMinimumScore.Size = new System.Drawing.Size(102, 17);
            this.chkMinimumScore.TabIndex = 19;
            this.chkMinimumScore.Text = "minimum score";
            this.chkMinimumScore.UseVisualStyleBackColor = true;
            // 
            // lbImageLimit
            // 
            this.lbImageLimit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbImageLimit.AutoSize = true;
            this.lbImageLimit.Location = new System.Drawing.Point(242, 245);
            this.lbImageLimit.Name = "lbImageLimit";
            this.lbImageLimit.Size = new System.Drawing.Size(63, 13);
            this.lbImageLimit.TabIndex = 26;
            this.lbImageLimit.Text = "image limit";
            // 
            // lbPageLimit
            // 
            this.lbPageLimit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbPageLimit.AutoSize = true;
            this.lbPageLimit.Location = new System.Drawing.Point(242, 271);
            this.lbPageLimit.Name = "lbPageLimit";
            this.lbPageLimit.Size = new System.Drawing.Size(58, 13);
            this.lbPageLimit.TabIndex = 27;
            this.lbPageLimit.Text = "page limit";
            // 
            // lbTagsHint
            // 
            this.lbTagsHint.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbTagsHint.Location = new System.Drawing.Point(12, 3);
            this.lbTagsHint.Name = "lbTagsHint";
            this.lbTagsHint.Size = new System.Drawing.Size(288, 20);
            this.lbTagsHint.TabIndex = 28;
            this.lbTagsHint.Text = "downloading the following tags";
            this.lbTagsHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbFileStatus
            // 
            this.lbFileStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFileStatus.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFileStatus.Location = new System.Drawing.Point(12, 54);
            this.lbFileStatus.Name = "lbFileStatus";
            this.lbFileStatus.Size = new System.Drawing.Size(288, 146);
            this.lbFileStatus.TabIndex = 29;
            this.lbFileStatus.Text = resources.GetString("lbFileStatus.Text");
            this.lbFileStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtPageLimit
            // 
            this.txtPageLimit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtPageLimit.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtPageLimit.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtPageLimit.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPageLimit.ButtonImageIndex = -1;
            this.txtPageLimit.ButtonImageKey = "";
            this.txtPageLimit.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtPageLimit.ButtonText = "";
            this.txtPageLimit.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtPageLimit.Location = new System.Drawing.Point(182, 268);
            this.txtPageLimit.Name = "txtPageLimit";
            this.txtPageLimit.ReadOnly = true;
            this.txtPageLimit.Size = new System.Drawing.Size(54, 22);
            this.txtPageLimit.TabIndex = 25;
            this.txtPageLimit.Text = "0";
            this.txtPageLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPageLimit.TextHint = "";
            // 
            // txtImageLimit
            // 
            this.txtImageLimit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtImageLimit.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtImageLimit.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtImageLimit.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtImageLimit.ButtonImageIndex = -1;
            this.txtImageLimit.ButtonImageKey = "";
            this.txtImageLimit.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtImageLimit.ButtonText = "";
            this.txtImageLimit.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtImageLimit.Location = new System.Drawing.Point(182, 243);
            this.txtImageLimit.Name = "txtImageLimit";
            this.txtImageLimit.ReadOnly = true;
            this.txtImageLimit.Size = new System.Drawing.Size(54, 22);
            this.txtImageLimit.TabIndex = 24;
            this.txtImageLimit.Text = "1500";
            this.txtImageLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtImageLimit.TextHint = "";
            // 
            // txtMinimumFavCount
            // 
            this.txtMinimumFavCount.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtMinimumFavCount.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtMinimumFavCount.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtMinimumFavCount.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMinimumFavCount.ButtonImageIndex = -1;
            this.txtMinimumFavCount.ButtonImageKey = "";
            this.txtMinimumFavCount.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtMinimumFavCount.ButtonText = "";
            this.txtMinimumFavCount.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtMinimumFavCount.Location = new System.Drawing.Point(7, 268);
            this.txtMinimumFavCount.Name = "txtMinimumFavCount";
            this.txtMinimumFavCount.ReadOnly = true;
            this.txtMinimumFavCount.Size = new System.Drawing.Size(42, 22);
            this.txtMinimumFavCount.TabIndex = 23;
            this.txtMinimumFavCount.Text = "-";
            this.txtMinimumFavCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMinimumFavCount.TextHint = "";
            // 
            // txtMinimumScore
            // 
            this.txtMinimumScore.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtMinimumScore.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtMinimumScore.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtMinimumScore.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMinimumScore.ButtonImageIndex = -1;
            this.txtMinimumScore.ButtonImageKey = "";
            this.txtMinimumScore.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtMinimumScore.ButtonText = "";
            this.txtMinimumScore.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtMinimumScore.Location = new System.Drawing.Point(7, 243);
            this.txtMinimumScore.Name = "txtMinimumScore";
            this.txtMinimumScore.ReadOnly = true;
            this.txtMinimumScore.Size = new System.Drawing.Size(42, 22);
            this.txtMinimumScore.TabIndex = 22;
            this.txtMinimumScore.Text = "25";
            this.txtMinimumScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMinimumScore.TextHint = "";
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
            this.txtTags.Location = new System.Drawing.Point(12, 27);
            this.txtTags.Name = "txtTags";
            this.txtTags.ReadOnly = true;
            this.txtTags.Size = new System.Drawing.Size(288, 22);
            this.txtTags.TabIndex = 21;
            this.txtTags.Text = "this is a tag im downloading";
            this.txtTags.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTags.TextHint = "";
            // 
            // pbTotalStatus
            // 
            this.pbTotalStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTotalStatus.ContainerParent = this;
            this.pbTotalStatus.FastValueUpdate = true;
            this.pbTotalStatus.Location = new System.Drawing.Point(12, 323);
            this.pbTotalStatus.Name = "pbTotalStatus";
            this.pbTotalStatus.ShowInTaskbar = true;
            this.pbTotalStatus.Size = new System.Drawing.Size(288, 22);
            this.pbTotalStatus.State = murrty.controls.ProgressBarState.Normal;
            this.pbTotalStatus.Style = System.Windows.Forms.ProgressBarStyle.Blocks;
            this.pbTotalStatus.TabIndex = 11;
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
            this.pbDownloadStatus.Location = new System.Drawing.Point(12, 295);
            this.pbDownloadStatus.Name = "pbDownloadStatus";
            this.pbDownloadStatus.Size = new System.Drawing.Size(288, 22);
            this.pbDownloadStatus.State = murrty.controls.ProgressBarState.Normal;
            this.pbDownloadStatus.Style = System.Windows.Forms.ProgressBarStyle.Blocks;
            this.pbDownloadStatus.TabIndex = 12;
            this.pbDownloadStatus.Text = "waiting until api is finished being parsed";
            this.pbDownloadStatus.TextColor = System.Drawing.SystemColors.WindowText;
            this.pbDownloadStatus.TextFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lbMinimumFavoriteCount
            // 
            this.lbMinimumFavoriteCount.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbMinimumFavoriteCount.AutoSize = true;
            this.lbMinimumFavoriteCount.Location = new System.Drawing.Point(55, 271);
            this.lbMinimumFavoriteCount.Name = "lbMinimumFavoriteCount";
            this.lbMinimumFavoriteCount.Size = new System.Drawing.Size(102, 13);
            this.lbMinimumFavoriteCount.TabIndex = 30;
            this.lbMinimumFavoriteCount.Text = "minimum favcount";
            // 
            // frmTagDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 373);
            this.Controls.Add(this.lbMinimumFavoriteCount);
            this.Controls.Add(this.lbTagsHint);
            this.Controls.Add(this.lbPageLimit);
            this.Controls.Add(this.lbImageLimit);
            this.Controls.Add(this.txtPageLimit);
            this.Controls.Add(this.txtImageLimit);
            this.Controls.Add(this.txtMinimumFavCount);
            this.Controls.Add(this.txtMinimumScore);
            this.Controls.Add(this.txtTags);
            this.Controls.Add(this.chkMinimumScore);
            this.Controls.Add(this.chkSeparateNonImages);
            this.Controls.Add(this.chkSeparateRatings);
            this.Controls.Add(this.chkSafe);
            this.Controls.Add(this.chkQuestionable);
            this.Controls.Add(this.chkExplicit);
            this.Controls.Add(this.sbStatus);
            this.Controls.Add(this.pbTotalStatus);
            this.Controls.Add(this.pbDownloadStatus);
            this.Controls.Add(this.lbFileStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(330, 410);
            this.MinimumSize = new System.Drawing.Size(330, 410);
            this.Name = "frmTagDownloader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "downloading tag(s) ....";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTagDownloaderUpdated_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private murrty.controls.ExtendedProgressBar pbTotalStatus;
        private System.Windows.Forms.StatusBar sbStatus;
        private murrty.controls.ExtendedProgressBar pbDownloadStatus;
        private murrty.controls.ExtendedTextBox txtTags;
        private System.Windows.Forms.CheckBox chkMinimumScore;
        private System.Windows.Forms.CheckBox chkSeparateNonImages;
        private System.Windows.Forms.CheckBox chkSeparateRatings;
        private System.Windows.Forms.CheckBox chkSafe;
        private System.Windows.Forms.CheckBox chkQuestionable;
        private System.Windows.Forms.CheckBox chkExplicit;
        private System.Windows.Forms.Label lbPageLimit;
        private System.Windows.Forms.Label lbImageLimit;
        private murrty.controls.ExtendedTextBox txtPageLimit;
        private murrty.controls.ExtendedTextBox txtImageLimit;
        private murrty.controls.ExtendedTextBox txtMinimumFavCount;
        private murrty.controls.ExtendedTextBox txtMinimumScore;
        private System.Windows.Forms.Label lbFileStatus;
        private System.Windows.Forms.Label lbTagsHint;
        private System.Windows.Forms.Label lbMinimumFavoriteCount;
    }
}