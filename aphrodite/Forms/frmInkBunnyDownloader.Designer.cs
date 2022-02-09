namespace aphrodite {
    partial class frmInkBunnyDownloader {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInkBunnyDownloader));
            this.sbStatus = new System.Windows.Forms.StatusBar();
            this.lbKeywords = new System.Windows.Forms.Label();
            this.txtKeywords = new murrty.controls.ExtendedTextBox();
            this.pbTotalStatus = new murrty.controls.ExtendedProgressBar();
            this.pbDownloadStatus = new murrty.controls.ExtendedProgressBar();
            this.lbArtistsGallery = new System.Windows.Forms.Label();
            this.txtArtistsGallery = new murrty.controls.ExtendedTextBox();
            this.lbUsersFavorites = new System.Windows.Forms.Label();
            this.txtUsersFavorites = new murrty.controls.ExtendedTextBox();
            this.lbFileStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // sbStatus
            // 
            this.sbStatus.Location = new System.Drawing.Point(0, 381);
            this.sbStatus.Name = "sbStatus";
            this.sbStatus.Size = new System.Drawing.Size(342, 22);
            this.sbStatus.SizingGrip = false;
            this.sbStatus.TabIndex = 33;
            this.sbStatus.Text = "waiting for thread to start";
            // 
            // lbKeywords
            // 
            this.lbKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbKeywords.Location = new System.Drawing.Point(12, 8);
            this.lbKeywords.Name = "lbKeywords";
            this.lbKeywords.Size = new System.Drawing.Size(318, 18);
            this.lbKeywords.TabIndex = 37;
            this.lbKeywords.Text = "downloading the following keywords";
            this.lbKeywords.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtKeywords
            // 
            this.txtKeywords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKeywords.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtKeywords.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtKeywords.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKeywords.ButtonImageIndex = -1;
            this.txtKeywords.ButtonImageKey = "";
            this.txtKeywords.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtKeywords.ButtonText = "";
            this.txtKeywords.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtKeywords.Location = new System.Drawing.Point(12, 29);
            this.txtKeywords.Name = "txtKeywords";
            this.txtKeywords.ReadOnly = true;
            this.txtKeywords.Size = new System.Drawing.Size(318, 22);
            this.txtKeywords.TabIndex = 36;
            this.txtKeywords.Text = "this is a tag im downloading";
            this.txtKeywords.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtKeywords.TextHint = "";
            // 
            // pbTotalStatus
            // 
            this.pbTotalStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTotalStatus.ContainerParent = this;
            this.pbTotalStatus.FastValueUpdate = true;
            this.pbTotalStatus.Location = new System.Drawing.Point(12, 353);
            this.pbTotalStatus.Name = "pbTotalStatus";
            this.pbTotalStatus.ShowInTaskbar = true;
            this.pbTotalStatus.Size = new System.Drawing.Size(318, 22);
            this.pbTotalStatus.State = murrty.controls.ProgressBarState.Normal;
            this.pbTotalStatus.Style = System.Windows.Forms.ProgressBarStyle.Blocks;
            this.pbTotalStatus.TabIndex = 34;
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
            this.pbDownloadStatus.Location = new System.Drawing.Point(12, 325);
            this.pbDownloadStatus.Name = "pbDownloadStatus";
            this.pbDownloadStatus.Size = new System.Drawing.Size(318, 22);
            this.pbDownloadStatus.State = murrty.controls.ProgressBarState.Normal;
            this.pbDownloadStatus.Style = System.Windows.Forms.ProgressBarStyle.Blocks;
            this.pbDownloadStatus.TabIndex = 35;
            this.pbDownloadStatus.Text = "waiting until api is finished being parsed";
            this.pbDownloadStatus.TextColor = System.Drawing.SystemColors.WindowText;
            this.pbDownloadStatus.TextFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // lbArtistsGallery
            // 
            this.lbArtistsGallery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbArtistsGallery.Location = new System.Drawing.Point(12, 54);
            this.lbArtistsGallery.Name = "lbArtistsGallery";
            this.lbArtistsGallery.Size = new System.Drawing.Size(318, 18);
            this.lbArtistsGallery.TabIndex = 39;
            this.lbArtistsGallery.Text = "downloading the following artists\' gallery";
            this.lbArtistsGallery.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtArtistsGallery
            // 
            this.txtArtistsGallery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtArtistsGallery.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtArtistsGallery.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtArtistsGallery.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArtistsGallery.ButtonImageIndex = -1;
            this.txtArtistsGallery.ButtonImageKey = "";
            this.txtArtistsGallery.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtArtistsGallery.ButtonText = "";
            this.txtArtistsGallery.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtArtistsGallery.Location = new System.Drawing.Point(12, 75);
            this.txtArtistsGallery.Name = "txtArtistsGallery";
            this.txtArtistsGallery.ReadOnly = true;
            this.txtArtistsGallery.Size = new System.Drawing.Size(318, 22);
            this.txtArtistsGallery.TabIndex = 38;
            this.txtArtistsGallery.Text = "this is a tag im downloading";
            this.txtArtistsGallery.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtArtistsGallery.TextHint = "";
            // 
            // lbUsersFavorites
            // 
            this.lbUsersFavorites.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbUsersFavorites.Location = new System.Drawing.Point(12, 100);
            this.lbUsersFavorites.Name = "lbUsersFavorites";
            this.lbUsersFavorites.Size = new System.Drawing.Size(318, 18);
            this.lbUsersFavorites.TabIndex = 41;
            this.lbUsersFavorites.Text = "downloading the following users\' favorites (id)";
            this.lbUsersFavorites.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtUsersFavorites
            // 
            this.txtUsersFavorites.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsersFavorites.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtUsersFavorites.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtUsersFavorites.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsersFavorites.ButtonImageIndex = -1;
            this.txtUsersFavorites.ButtonImageKey = "";
            this.txtUsersFavorites.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtUsersFavorites.ButtonText = "";
            this.txtUsersFavorites.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtUsersFavorites.Location = new System.Drawing.Point(12, 121);
            this.txtUsersFavorites.Name = "txtUsersFavorites";
            this.txtUsersFavorites.ReadOnly = true;
            this.txtUsersFavorites.Size = new System.Drawing.Size(318, 22);
            this.txtUsersFavorites.TabIndex = 40;
            this.txtUsersFavorites.Text = "this is a tag im downloading";
            this.txtUsersFavorites.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtUsersFavorites.TextHint = "";
            // 
            // lbFileStatus
            // 
            this.lbFileStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFileStatus.Location = new System.Drawing.Point(12, 146);
            this.lbFileStatus.Name = "lbFileStatus";
            this.lbFileStatus.Size = new System.Drawing.Size(318, 176);
            this.lbFileStatus.TabIndex = 42;
            this.lbFileStatus.Text = resources.GetString("lbFileStatus.Text");
            this.lbFileStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // frmInkBunnyDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(342, 403);
            this.Controls.Add(this.lbFileStatus);
            this.Controls.Add(this.lbUsersFavorites);
            this.Controls.Add(this.txtUsersFavorites);
            this.Controls.Add(this.lbArtistsGallery);
            this.Controls.Add(this.txtArtistsGallery);
            this.Controls.Add(this.lbKeywords);
            this.Controls.Add(this.txtKeywords);
            this.Controls.Add(this.pbTotalStatus);
            this.Controls.Add(this.pbDownloadStatus);
            this.Controls.Add(this.sbStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(360, 440);
            this.MinimumSize = new System.Drawing.Size(360, 440);
            this.Name = "frmInkBunnyDownloader";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "inkbunny submissions downloading ....";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmInkBunnyDownloader_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusBar sbStatus;
        private System.Windows.Forms.Label lbKeywords;
        private murrty.controls.ExtendedTextBox txtKeywords;
        private murrty.controls.ExtendedProgressBar pbTotalStatus;
        private murrty.controls.ExtendedProgressBar pbDownloadStatus;
        private System.Windows.Forms.Label lbUsersFavorites;
        private murrty.controls.ExtendedTextBox txtUsersFavorites;
        private System.Windows.Forms.Label lbArtistsGallery;
        private murrty.controls.ExtendedTextBox txtArtistsGallery;
        private System.Windows.Forms.Label lbFileStatus;
    }
}