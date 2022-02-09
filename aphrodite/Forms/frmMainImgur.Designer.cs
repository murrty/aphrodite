namespace aphrodite {
    partial class frmImgurMain {
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
            this.mMenu = new System.Windows.Forms.MainMenu(this.components);
            this.mAPI = new System.Windows.Forms.MenuItem();
            this.mLogin = new System.Windows.Forms.MenuItem();
            this.mClientID = new System.Windows.Forms.MenuItem();
            this.mShowClientID = new System.Windows.Forms.MenuItem();
            this.myShowDecryptedClientID = new System.Windows.Forms.MenuItem();
            this.mCopyDecryptedClientID = new System.Windows.Forms.MenuItem();
            this.mSettings = new System.Windows.Forms.MenuItem();
            this.mLog = new System.Windows.Forms.MenuItem();
            this.mAbout = new System.Windows.Forms.MenuItem();
            this.chkSeparateNonImages = new System.Windows.Forms.CheckBox();
            this.numImageLimit = new System.Windows.Forms.NumericUpDown();
            this.lbImageLimit = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAlbum = new murrty.controls.ExtendedTextBox();
            this.llSchemaHelp = new murrty.controls.ExtendedLinkLabel();
            this.txtFileNameSchema = new murrty.controls.ExtendedTextBox();
            this.btnDownload = new murrty.controls.ExtendedButton();
            ((System.ComponentModel.ISupportInitialize)(this.numImageLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // mMenu
            // 
            this.mMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mAPI,
            this.mSettings,
            this.mLog,
            this.mAbout});
            // 
            // mAPI
            // 
            this.mAPI.Index = 0;
            this.mAPI.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mLogin,
            this.mClientID});
            this.mAPI.Text = "api";
            // 
            // mLogin
            // 
            this.mLogin.Index = 0;
            this.mLogin.Text = "login";
            this.mLogin.Click += new System.EventHandler(this.mLogin_Click);
            // 
            // mClientID
            // 
            this.mClientID.Index = 1;
            this.mClientID.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mShowClientID,
            this.myShowDecryptedClientID,
            this.mCopyDecryptedClientID});
            this.mClientID.Text = "client id";
            // 
            // mShowClientID
            // 
            this.mShowClientID.Index = 0;
            this.mShowClientID.Text = "show client id";
            this.mShowClientID.Click += new System.EventHandler(this.mShowClientID_Click);
            // 
            // myShowDecryptedClientID
            // 
            this.myShowDecryptedClientID.Index = 1;
            this.myShowDecryptedClientID.Text = "show decrypted client id";
            this.myShowDecryptedClientID.Click += new System.EventHandler(this.myShowDecryptedClientID_Click);
            // 
            // mCopyDecryptedClientID
            // 
            this.mCopyDecryptedClientID.Index = 2;
            this.mCopyDecryptedClientID.Text = "copy decrypted client id";
            this.mCopyDecryptedClientID.Click += new System.EventHandler(this.mCopyDecryptedClientID_Click);
            // 
            // mSettings
            // 
            this.mSettings.Index = 1;
            this.mSettings.Text = "settings";
            this.mSettings.Click += new System.EventHandler(this.mSettings_Click);
            // 
            // mLog
            // 
            this.mLog.Index = 2;
            this.mLog.Text = "log";
            this.mLog.Click += new System.EventHandler(this.mLog_Click);
            // 
            // mAbout
            // 
            this.mAbout.Index = 3;
            this.mAbout.Text = "about";
            this.mAbout.Click += new System.EventHandler(this.mAbout_Click);
            // 
            // chkSeparateNonImages
            // 
            this.chkSeparateNonImages.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkSeparateNonImages.AutoSize = true;
            this.chkSeparateNonImages.Enabled = false;
            this.chkSeparateNonImages.Location = new System.Drawing.Point(72, 41);
            this.chkSeparateNonImages.Name = "chkSeparateNonImages";
            this.chkSeparateNonImages.Size = new System.Drawing.Size(134, 17);
            this.chkSeparateNonImages.TabIndex = 0;
            this.chkSeparateNonImages.Text = "Separate non-images";
            this.chkSeparateNonImages.UseVisualStyleBackColor = true;
            // 
            // numImageLimit
            // 
            this.numImageLimit.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.numImageLimit.Enabled = false;
            this.numImageLimit.Location = new System.Drawing.Point(139, 64);
            this.numImageLimit.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numImageLimit.Name = "numImageLimit";
            this.numImageLimit.Size = new System.Drawing.Size(76, 22);
            this.numImageLimit.TabIndex = 1;
            // 
            // lbImageLimit
            // 
            this.lbImageLimit.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lbImageLimit.AutoSize = true;
            this.lbImageLimit.Location = new System.Drawing.Point(70, 68);
            this.lbImageLimit.Name = "lbImageLimit";
            this.lbImageLimit.Size = new System.Drawing.Size(63, 13);
            this.lbImageLimit.TabIndex = 3;
            this.lbImageLimit.Text = "image limit";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "file name schema";
            // 
            // txtAlbum
            // 
            this.txtAlbum.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtAlbum.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtAlbum.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtAlbum.ButtonEnabled = true;
            this.txtAlbum.ButtonFont = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAlbum.ButtonImageIndex = -1;
            this.txtAlbum.ButtonImageKey = "";
            this.txtAlbum.ButtonSize = new System.Drawing.Size(22, 19);
            this.txtAlbum.ButtonText = "X";
            this.txtAlbum.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtAlbum.Enabled = false;
            this.txtAlbum.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAlbum.Location = new System.Drawing.Point(48, 13);
            this.txtAlbum.Name = "txtAlbum";
            this.txtAlbum.Size = new System.Drawing.Size(186, 20);
            this.txtAlbum.TabIndex = 10;
            this.txtAlbum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAlbum.TextHint = "Imgur album...";
            this.txtAlbum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAlbum_KeyDown);
            this.txtAlbum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAlbum_KeyPress);
            // 
            // llSchemaHelp
            // 
            this.llSchemaHelp.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.llSchemaHelp.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.llSchemaHelp.AutoSize = true;
            this.llSchemaHelp.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llSchemaHelp.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.llSchemaHelp.Location = new System.Drawing.Point(258, 113);
            this.llSchemaHelp.Name = "llSchemaHelp";
            this.llSchemaHelp.Size = new System.Drawing.Size(17, 21);
            this.llSchemaHelp.TabIndex = 9;
            this.llSchemaHelp.TabStop = true;
            this.llSchemaHelp.Text = "?";
            this.llSchemaHelp.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.llSchemaHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llSchemaHelp_LinkClicked);
            // 
            // txtFileNameSchema
            // 
            this.txtFileNameSchema.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtFileNameSchema.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtFileNameSchema.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtFileNameSchema.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFileNameSchema.ButtonImageIndex = -1;
            this.txtFileNameSchema.ButtonImageKey = "";
            this.txtFileNameSchema.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtFileNameSchema.ButtonText = "";
            this.txtFileNameSchema.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtFileNameSchema.Enabled = false;
            this.txtFileNameSchema.Location = new System.Drawing.Point(23, 113);
            this.txtFileNameSchema.Name = "txtFileNameSchema";
            this.txtFileNameSchema.Size = new System.Drawing.Size(236, 22);
            this.txtFileNameSchema.TabIndex = 7;
            this.txtFileNameSchema.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtFileNameSchema.TextHint = "%imageindex%_%imageid%";
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnDownload.Enabled = false;
            this.btnDownload.Location = new System.Drawing.Point(100, 145);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(88, 23);
            this.btnDownload.TabIndex = 6;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // frmImgurMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(282, 203);
            this.Controls.Add(this.txtAlbum);
            this.Controls.Add(this.llSchemaHelp);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFileNameSchema);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.lbImageLimit);
            this.Controls.Add(this.numImageLimit);
            this.Controls.Add(this.chkSeparateNonImages);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 240);
            this.Menu = this.mMenu;
            this.MinimumSize = new System.Drawing.Size(300, 240);
            this.Name = "frmImgurMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "imgur downloader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmImgurMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numImageLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MainMenu mMenu;
        private System.Windows.Forms.MenuItem mLogin;
        private System.Windows.Forms.MenuItem mSettings;
        private System.Windows.Forms.CheckBox chkSeparateNonImages;
        private System.Windows.Forms.NumericUpDown numImageLimit;
        private System.Windows.Forms.Label lbImageLimit;
        private murrty.controls.ExtendedButton btnDownload;
        private murrty.controls.ExtendedTextBox txtFileNameSchema;
        private System.Windows.Forms.Label label1;
        private murrty.controls.ExtendedLinkLabel llSchemaHelp;
        private murrty.controls.ExtendedTextBox txtAlbum;
        private System.Windows.Forms.MenuItem mClientID;
        private System.Windows.Forms.MenuItem mCopyDecryptedClientID;
        private System.Windows.Forms.MenuItem mShowClientID;
        private System.Windows.Forms.MenuItem myShowDecryptedClientID;
        private System.Windows.Forms.MenuItem mLog;
        private System.Windows.Forms.MenuItem mAbout;
        private System.Windows.Forms.MenuItem mAPI;
    }
}