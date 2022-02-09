
namespace aphrodite {
    partial class frmFurryBooruMain {
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
            this.lbTags = new System.Windows.Forms.Label();
            this.lbInfo = new System.Windows.Forms.Label();
            this.btnDownload = new murrty.controls.ExtendedButton();
            this.txtTags = new murrty.controls.ExtendedTextBox();
            this.SuspendLayout();
            // 
            // lbTags
            // 
            this.lbTags.AutoSize = true;
            this.lbTags.Location = new System.Drawing.Point(34, 14);
            this.lbTags.Name = "lbTags";
            this.lbTags.Size = new System.Drawing.Size(32, 13);
            this.lbTags.TabIndex = 1;
            this.lbTags.Text = "tags:";
            // 
            // lbInfo
            // 
            this.lbInfo.Location = new System.Drawing.Point(12, 108);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(294, 41);
            this.lbInfo.TabIndex = 3;
            this.lbInfo.Text = "Global settings from aphrodite are used for\r\nparsing, sorting, and blacklisting.\r" +
    "\nNo settings are currently available to be overridden.";
            this.lbInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(118, 67);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(86, 23);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // txtTags
            // 
            this.txtTags.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtTags.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtTags.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtTags.ButtonEnabled = true;
            this.txtTags.ButtonFont = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTags.ButtonImageIndex = -1;
            this.txtTags.ButtonImageKey = "";
            this.txtTags.ButtonSize = new System.Drawing.Size(22, 19);
            this.txtTags.ButtonText = "X";
            this.txtTags.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtTags.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTags.Location = new System.Drawing.Point(46, 33);
            this.txtTags.Name = "txtTags";
            this.txtTags.Size = new System.Drawing.Size(230, 20);
            this.txtTags.TabIndex = 4;
            this.txtTags.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTags.TextHint = "Tags to download...";
            this.txtTags.ButtonClick += new System.EventHandler(this.txtTags_ButtonClick);
            this.txtTags.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTags_KeyPress);
            // 
            // frmFurryBooruMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(322, 163);
            this.Controls.Add(this.txtTags);
            this.Controls.Add(this.lbInfo);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.lbTags);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(340, 200);
            this.MinimumSize = new System.Drawing.Size(340, 200);
            this.Name = "frmFurryBooruMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "furry.booru.org";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFurryBooruMain_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lbTags;
        private murrty.controls.ExtendedButton btnDownload;
        private System.Windows.Forms.Label lbInfo;
        private murrty.controls.ExtendedTextBox txtTags;
    }
}