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
            this.lbID = new System.Windows.Forms.Label();
            this.lbFile = new System.Windows.Forms.Label();
            this.lbPercentage = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.StatusBar();
            this.lbTotal = new System.Windows.Forms.Label();
            this.tmrTitle = new System.Windows.Forms.Timer(this.components);
            this.lbRemoved = new System.Windows.Forms.Label();
            this.lbBytes = new System.Windows.Forms.Label();
            this.pbTotalStatus = new aphrodite.Controls.ExtendedProgressBar();
            this.pbDownloadStatus = new aphrodite.Controls.ExtendedProgressBar();
            this.txtPoolName = new aphrodite.Controls.ExtendedTextBox();
            this.SuspendLayout();
            // 
            // lbID
            // 
            this.lbID.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbID.Location = new System.Drawing.Point(90, 5);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(100, 14);
            this.lbID.TabIndex = 0;
            this.lbID.Text = "Pool ID ???";
            this.lbID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbFile
            // 
            this.lbFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFile.Location = new System.Drawing.Point(10, 176);
            this.lbFile.Name = "lbFile";
            this.lbFile.Size = new System.Drawing.Size(252, 16);
            this.lbFile.TabIndex = 2;
            this.lbFile.Text = "File ? of ?";
            this.lbFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbPercentage
            // 
            this.lbPercentage.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbPercentage.BackColor = System.Drawing.Color.Transparent;
            this.lbPercentage.Location = new System.Drawing.Point(116, 213);
            this.lbPercentage.Name = "lbPercentage";
            this.lbPercentage.Size = new System.Drawing.Size(41, 14);
            this.lbPercentage.TabIndex = 4;
            this.lbPercentage.Text = "0%";
            this.lbPercentage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(0, 261);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(272, 22);
            this.status.SizingGrip = false;
            this.status.TabIndex = 5;
            this.status.Text = "Waiting for initial start";
            // 
            // lbTotal
            // 
            this.lbTotal.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbTotal.Location = new System.Drawing.Point(10, 52);
            this.lbTotal.Name = "lbTotal";
            this.lbTotal.Size = new System.Drawing.Size(252, 123);
            this.lbTotal.TabIndex = 6;
            this.lbTotal.Text = "Waiting for first page to be parsed...";
            this.lbTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrTitle
            // 
            this.tmrTitle.Interval = 1000;
            this.tmrTitle.Tick += new System.EventHandler(this.tmrTitle_Tick);
            // 
            // lbRemoved
            // 
            this.lbRemoved.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbRemoved.AutoSize = true;
            this.lbRemoved.Location = new System.Drawing.Point(66, 236);
            this.lbRemoved.Name = "lbRemoved";
            this.lbRemoved.Size = new System.Drawing.Size(141, 13);
            this.lbRemoved.TabIndex = 12;
            this.lbRemoved.Text = "Removed images detected";
            this.lbRemoved.Visible = false;
            // 
            // lbBytes
            // 
            this.lbBytes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbBytes.Location = new System.Drawing.Point(10, 191);
            this.lbBytes.Name = "lbBytes";
            this.lbBytes.Size = new System.Drawing.Size(252, 18);
            this.lbBytes.TabIndex = 13;
            this.lbBytes.Text = "0kb / 0kb";
            this.lbBytes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbTotalStatus
            // 
            this.pbTotalStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTotalStatus.ContainerControl = this;
            this.pbTotalStatus.Location = new System.Drawing.Point(10, 234);
            this.pbTotalStatus.Name = "pbTotalStatus";
            this.pbTotalStatus.ShowInTaskbar = true;
            this.pbTotalStatus.Size = new System.Drawing.Size(252, 18);
            this.pbTotalStatus.TabIndex = 7;
            // 
            // pbDownloadStatus
            // 
            this.pbDownloadStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbDownloadStatus.ContainerControl = this;
            this.pbDownloadStatus.Location = new System.Drawing.Point(10, 211);
            this.pbDownloadStatus.Name = "pbDownloadStatus";
            this.pbDownloadStatus.Size = new System.Drawing.Size(252, 18);
            this.pbDownloadStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbDownloadStatus.TabIndex = 8;
            // 
            // txtPoolName
            // 
            this.txtPoolName.ButtonAlignment = aphrodite.Controls.ButtonAlignments.Left;
            this.txtPoolName.ButtonCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPoolName.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPoolName.ButtonImageIndex = -1;
            this.txtPoolName.ButtonImageKey = "";
            this.txtPoolName.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtPoolName.ButtonText = "";
            this.txtPoolName.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtPoolName.Location = new System.Drawing.Point(12, 27);
            this.txtPoolName.Name = "txtPoolName";
            this.txtPoolName.ReadOnly = true;
            this.txtPoolName.Size = new System.Drawing.Size(248, 22);
            this.txtPoolName.TabIndex = 14;
            this.txtPoolName.Text = "pool not parsed";
            this.txtPoolName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPoolName.TextHint = "";
            // 
            // frmPoolDownloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(272, 283);
            this.Controls.Add(this.txtPoolName);
            this.Controls.Add(this.lbRemoved);
            this.Controls.Add(this.pbTotalStatus);
            this.Controls.Add(this.status);
            this.Controls.Add(this.lbPercentage);
            this.Controls.Add(this.lbFile);
            this.Controls.Add(this.lbID);
            this.Controls.Add(this.lbTotal);
            this.Controls.Add(this.pbDownloadStatus);
            this.Controls.Add(this.lbBytes);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(290, 320);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(290, 320);
            this.Name = "frmPoolDownloader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Downloading pool ....";
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
        private System.Windows.Forms.Label lbTotal;
        private System.Windows.Forms.Timer tmrTitle;
        private Controls.ExtendedProgressBar pbTotalStatus;
        private Controls.ExtendedProgressBar pbDownloadStatus;
        private System.Windows.Forms.Label lbRemoved;
        private System.Windows.Forms.Label lbBytes;
        private Controls.ExtendedTextBox txtPoolName;
    }
}