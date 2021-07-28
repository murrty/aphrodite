namespace aphrodite {
    partial class frmMain {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabTags = new System.Windows.Forms.TabPage();
            this.lbTagsSeparator = new System.Windows.Forms.Label();
            this.lbTagsLimitsHint = new System.Windows.Forms.Label();
            this.rbTagsOtherSettings = new System.Windows.Forms.RadioButton();
            this.rbTagsRatings = new System.Windows.Forms.RadioButton();
            this.rbTagsLimits = new System.Windows.Forms.RadioButton();
            this.rbTagsMinimumFavorites = new System.Windows.Forms.RadioButton();
            this.rbTagsMinimumScore = new System.Windows.Forms.RadioButton();
            this.btnDownloadTags = new aphrodite.Controls.ExtendedButton();
            this.lbAwoo = new System.Windows.Forms.Label();
            this.txtTags = new aphrodite.Controls.ExtendedTextBox();
            this.panelTagsOtherSettings = new System.Windows.Forms.Panel();
            this.chkTagsOpenAfterDownload = new System.Windows.Forms.CheckBox();
            this.chkTagsDownloadInUploadOrder = new System.Windows.Forms.CheckBox();
            this.chkTagSeparateNonImages = new System.Windows.Forms.CheckBox();
            this.panelTagsRatings = new System.Windows.Forms.Panel();
            this.chkTagsDownloadQuestionable = new System.Windows.Forms.CheckBox();
            this.chkTagsDownloadSafe = new System.Windows.Forms.CheckBox();
            this.chkTagsSeparateRatings = new System.Windows.Forms.CheckBox();
            this.chkTagsDownloadExplicit = new System.Windows.Forms.CheckBox();
            this.panelTagsLimits = new System.Windows.Forms.Panel();
            this.lbTagsImageLimit = new System.Windows.Forms.Label();
            this.lbTagsPageLimit = new System.Windows.Forms.Label();
            this.numTagsImageLimit = new System.Windows.Forms.NumericUpDown();
            this.numTagsPageLimit = new System.Windows.Forms.NumericUpDown();
            this.panelTagsMinimumFavorites = new System.Windows.Forms.Panel();
            this.chkTagsMinimumFavoritesAsTag = new System.Windows.Forms.CheckBox();
            this.numTagsMinimumFavorites = new System.Windows.Forms.NumericUpDown();
            this.panelTagsMinimumScore = new System.Windows.Forms.Panel();
            this.chkTagsMinimumScoreAsTag = new System.Windows.Forms.CheckBox();
            this.numTagsMinimumScore = new System.Windows.Forms.NumericUpDown();
            this.chkTagsUseMinimumScore = new System.Windows.Forms.CheckBox();
            this.tabPools = new System.Windows.Forms.TabPage();
            this.chkPoolDownloadBlacklistedImages = new System.Windows.Forms.CheckBox();
            this.chkPoolMergeBlacklisted = new System.Windows.Forms.CheckBox();
            this.chkPoolMergeGraylisted = new System.Windows.Forms.CheckBox();
            this.chkPoolOpenAfter = new System.Windows.Forms.CheckBox();
            this.txtPoolId = new aphrodite.Controls.ExtendedTextBox();
            this.btnDownloadPool = new aphrodite.Controls.ExtendedButton();
            this.tabImages = new System.Windows.Forms.TabPage();
            this.chkImageSeparateBlacklisted = new System.Windows.Forms.CheckBox();
            this.chkImageOpenAfter = new System.Windows.Forms.CheckBox();
            this.chkImageSeparateNonImages = new System.Windows.Forms.CheckBox();
            this.chkImageSeparateArtists = new System.Windows.Forms.CheckBox();
            this.chkImageUseForm = new System.Windows.Forms.CheckBox();
            this.chkImageSeparateGraylisted = new System.Windows.Forms.CheckBox();
            this.chkImageSeparateRatings = new System.Windows.Forms.CheckBox();
            this.txtImageUrl = new aphrodite.Controls.ExtendedTextBox();
            this.btnDownloadImage = new aphrodite.Controls.ExtendedButton();
            this.tabPortable = new System.Windows.Forms.TabPage();
            this.lbPortable = new System.Windows.Forms.Label();
            this.toolMenu = new System.Windows.Forms.MainMenu(this.components);
            this.mSettings = new System.Windows.Forms.MenuItem();
            this.mBlacklist = new System.Windows.Forms.MenuItem();
            this.mTools = new System.Windows.Forms.MenuItem();
            this.mWishlist = new System.Windows.Forms.MenuItem();
            this.mRedownloader = new System.Windows.Forms.MenuItem();
            this.mReverseSearch = new System.Windows.Forms.MenuItem();
            this.mToolsSeparator = new System.Windows.Forms.MenuItem();
            this.mLog = new System.Windows.Forms.MenuItem();
            this.mAbout = new System.Windows.Forms.MenuItem();
            this.TouchingTips = new System.Windows.Forms.ToolTip(this.components);
            this.tabMain.SuspendLayout();
            this.tabTags.SuspendLayout();
            this.panelTagsOtherSettings.SuspendLayout();
            this.panelTagsRatings.SuspendLayout();
            this.panelTagsLimits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsImageLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsPageLimit)).BeginInit();
            this.panelTagsMinimumFavorites.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsMinimumFavorites)).BeginInit();
            this.panelTagsMinimumScore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsMinimumScore)).BeginInit();
            this.tabPools.SuspendLayout();
            this.tabImages.SuspendLayout();
            this.tabPortable.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabTags);
            this.tabMain.Controls.Add(this.tabPools);
            this.tabMain.Controls.Add(this.tabImages);
            this.tabMain.Controls.Add(this.tabPortable);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(312, 237);
            this.tabMain.TabIndex = 0;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tbMain_SelectedIndexChanged);
            // 
            // tabTags
            // 
            this.tabTags.Controls.Add(this.lbTagsSeparator);
            this.tabTags.Controls.Add(this.lbTagsLimitsHint);
            this.tabTags.Controls.Add(this.rbTagsOtherSettings);
            this.tabTags.Controls.Add(this.rbTagsRatings);
            this.tabTags.Controls.Add(this.rbTagsLimits);
            this.tabTags.Controls.Add(this.rbTagsMinimumFavorites);
            this.tabTags.Controls.Add(this.rbTagsMinimumScore);
            this.tabTags.Controls.Add(this.btnDownloadTags);
            this.tabTags.Controls.Add(this.lbAwoo);
            this.tabTags.Controls.Add(this.txtTags);
            this.tabTags.Controls.Add(this.panelTagsOtherSettings);
            this.tabTags.Controls.Add(this.panelTagsRatings);
            this.tabTags.Controls.Add(this.panelTagsLimits);
            this.tabTags.Controls.Add(this.panelTagsMinimumFavorites);
            this.tabTags.Controls.Add(this.panelTagsMinimumScore);
            this.tabTags.Location = new System.Drawing.Point(4, 22);
            this.tabTags.Name = "tabTags";
            this.tabTags.Padding = new System.Windows.Forms.Padding(3);
            this.tabTags.Size = new System.Drawing.Size(304, 211);
            this.tabTags.TabIndex = 0;
            this.tabTags.Text = "Tag(s)";
            this.tabTags.UseVisualStyleBackColor = true;
            // 
            // lbTagsSeparator
            // 
            this.lbTagsSeparator.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbTagsSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbTagsSeparator.Location = new System.Drawing.Point(124, 39);
            this.lbTagsSeparator.Name = "lbTagsSeparator";
            this.lbTagsSeparator.Size = new System.Drawing.Size(2, 133);
            this.lbTagsSeparator.TabIndex = 7;
            this.lbTagsSeparator.Text = "hey";
            // 
            // lbTagsLimitsHint
            // 
            this.lbTagsLimitsHint.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbTagsLimitsHint.Location = new System.Drawing.Point(126, 0);
            this.lbTagsLimitsHint.Name = "lbTagsLimitsHint";
            this.lbTagsLimitsHint.Size = new System.Drawing.Size(178, 21);
            this.lbTagsLimitsHint.TabIndex = 17;
            this.lbTagsLimitsHint.Text = "0 = off";
            this.lbTagsLimitsHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbTagsLimitsHint.Visible = false;
            // 
            // rbTagsOtherSettings
            // 
            this.rbTagsOtherSettings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbTagsOtherSettings.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbTagsOtherSettings.Location = new System.Drawing.Point(9, 152);
            this.rbTagsOtherSettings.Name = "rbTagsOtherSettings";
            this.rbTagsOtherSettings.Size = new System.Drawing.Size(110, 23);
            this.rbTagsOtherSettings.TabIndex = 6;
            this.rbTagsOtherSettings.TabStop = true;
            this.rbTagsOtherSettings.Text = "Other settings";
            this.rbTagsOtherSettings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbTagsOtherSettings.UseVisualStyleBackColor = true;
            this.rbTagsOtherSettings.CheckedChanged += new System.EventHandler(this.rbTagsOtherSettings_CheckedChanged);
            // 
            // rbTagsRatings
            // 
            this.rbTagsRatings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbTagsRatings.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbTagsRatings.Location = new System.Drawing.Point(9, 123);
            this.rbTagsRatings.Name = "rbTagsRatings";
            this.rbTagsRatings.Size = new System.Drawing.Size(110, 23);
            this.rbTagsRatings.TabIndex = 5;
            this.rbTagsRatings.TabStop = true;
            this.rbTagsRatings.Text = "Ratings";
            this.rbTagsRatings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbTagsRatings.UseVisualStyleBackColor = true;
            this.rbTagsRatings.CheckedChanged += new System.EventHandler(this.rbTagsRatings_CheckedChanged);
            // 
            // rbTagsLimits
            // 
            this.rbTagsLimits.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbTagsLimits.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbTagsLimits.Location = new System.Drawing.Point(9, 94);
            this.rbTagsLimits.Name = "rbTagsLimits";
            this.rbTagsLimits.Size = new System.Drawing.Size(110, 23);
            this.rbTagsLimits.TabIndex = 4;
            this.rbTagsLimits.TabStop = true;
            this.rbTagsLimits.Text = "Page/Image limits";
            this.rbTagsLimits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbTagsLimits.UseVisualStyleBackColor = true;
            this.rbTagsLimits.CheckedChanged += new System.EventHandler(this.rbTagsLimits_CheckedChanged);
            // 
            // rbTagsMinimumFavorites
            // 
            this.rbTagsMinimumFavorites.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbTagsMinimumFavorites.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbTagsMinimumFavorites.Location = new System.Drawing.Point(9, 65);
            this.rbTagsMinimumFavorites.Name = "rbTagsMinimumFavorites";
            this.rbTagsMinimumFavorites.Size = new System.Drawing.Size(110, 23);
            this.rbTagsMinimumFavorites.TabIndex = 3;
            this.rbTagsMinimumFavorites.TabStop = true;
            this.rbTagsMinimumFavorites.Text = "Minimum favorites";
            this.rbTagsMinimumFavorites.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbTagsMinimumFavorites.UseVisualStyleBackColor = true;
            this.rbTagsMinimumFavorites.CheckedChanged += new System.EventHandler(this.rbTagsMinimumFavorites_CheckedChanged);
            // 
            // rbTagsMinimumScore
            // 
            this.rbTagsMinimumScore.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbTagsMinimumScore.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbTagsMinimumScore.Location = new System.Drawing.Point(9, 36);
            this.rbTagsMinimumScore.Name = "rbTagsMinimumScore";
            this.rbTagsMinimumScore.Size = new System.Drawing.Size(110, 23);
            this.rbTagsMinimumScore.TabIndex = 2;
            this.rbTagsMinimumScore.TabStop = true;
            this.rbTagsMinimumScore.Text = "Minimum score";
            this.rbTagsMinimumScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbTagsMinimumScore.UseVisualStyleBackColor = true;
            this.rbTagsMinimumScore.CheckedChanged += new System.EventHandler(this.rbTagsMinimumScore_CheckedChanged);
            // 
            // btnDownloadTags
            // 
            this.btnDownloadTags.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDownloadTags.Location = new System.Drawing.Point(112, 180);
            this.btnDownloadTags.Name = "btnDownloadTags";
            this.btnDownloadTags.Size = new System.Drawing.Size(80, 23);
            this.btnDownloadTags.TabIndex = 14;
            this.btnDownloadTags.Text = "Download";
            this.btnDownloadTags.UseVisualStyleBackColor = true;
            this.btnDownloadTags.Click += new System.EventHandler(this.btnDownloadTags_Click);
            // 
            // lbAwoo
            // 
            this.lbAwoo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbAwoo.AutoSize = true;
            this.lbAwoo.Location = new System.Drawing.Point(124, 170);
            this.lbAwoo.Name = "lbAwoo";
            this.lbAwoo.Size = new System.Drawing.Size(57, 13);
            this.lbAwoo.TabIndex = 13;
            this.lbAwoo.Text = "awooooo";
            // 
            // txtTags
            // 
            this.txtTags.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtTags.ButtonAlignment = aphrodite.Controls.ButtonAlignments.Left;
            this.txtTags.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtTags.ButtonEnabled = true;
            this.txtTags.ButtonFont = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTags.ButtonImageIndex = -1;
            this.txtTags.ButtonImageKey = "";
            this.txtTags.ButtonSize = new System.Drawing.Size(22, 19);
            this.txtTags.ButtonText = "X";
            this.txtTags.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtTags.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTags.Location = new System.Drawing.Point(37, 9);
            this.txtTags.Name = "txtTags";
            this.txtTags.Size = new System.Drawing.Size(230, 20);
            this.txtTags.TabIndex = 1;
            this.txtTags.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTags.TextHint = "Tags to download...";
            this.TouchingTips.SetToolTip(this.txtTags, "The tags that will be downloaded");
            this.txtTags.ButtonClick += new System.EventHandler(this.txtTags_ButtonClick);
            this.txtTags.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTags_KeyDown);
            this.txtTags.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTags_KeyPress);
            // 
            // panelTagsOtherSettings
            // 
            this.panelTagsOtherSettings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelTagsOtherSettings.Controls.Add(this.chkTagsOpenAfterDownload);
            this.panelTagsOtherSettings.Controls.Add(this.chkTagsDownloadInUploadOrder);
            this.panelTagsOtherSettings.Controls.Add(this.chkTagSeparateNonImages);
            this.panelTagsOtherSettings.Location = new System.Drawing.Point(118, 36);
            this.panelTagsOtherSettings.Name = "panelTagsOtherSettings";
            this.panelTagsOtherSettings.Size = new System.Drawing.Size(180, 140);
            this.panelTagsOtherSettings.TabIndex = 12;
            this.panelTagsOtherSettings.Visible = false;
            // 
            // chkTagsOpenAfterDownload
            // 
            this.chkTagsOpenAfterDownload.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkTagsOpenAfterDownload.AutoSize = true;
            this.chkTagsOpenAfterDownload.Location = new System.Drawing.Point(14, 39);
            this.chkTagsOpenAfterDownload.Name = "chkTagsOpenAfterDownload";
            this.chkTagsOpenAfterDownload.Size = new System.Drawing.Size(154, 17);
            this.chkTagsOpenAfterDownload.TabIndex = 9;
            this.chkTagsOpenAfterDownload.Text = "Open after downloading";
            this.TouchingTips.SetToolTip(this.chkTagsOpenAfterDownload, "Opens the folder after download is finished");
            this.chkTagsOpenAfterDownload.UseVisualStyleBackColor = true;
            // 
            // chkTagsDownloadInUploadOrder
            // 
            this.chkTagsDownloadInUploadOrder.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkTagsDownloadInUploadOrder.AutoSize = true;
            this.chkTagsDownloadInUploadOrder.Location = new System.Drawing.Point(14, 85);
            this.chkTagsDownloadInUploadOrder.Name = "chkTagsDownloadInUploadOrder";
            this.chkTagsDownloadInUploadOrder.Size = new System.Drawing.Size(168, 17);
            this.chkTagsDownloadInUploadOrder.TabIndex = 10;
            this.chkTagsDownloadInUploadOrder.Text = "Download newest to oldest";
            this.TouchingTips.SetToolTip(this.chkTagsDownloadInUploadOrder, "Download tags in order of newest to oldest");
            this.chkTagsDownloadInUploadOrder.UseVisualStyleBackColor = true;
            // 
            // chkTagSeparateNonImages
            // 
            this.chkTagSeparateNonImages.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkTagSeparateNonImages.AutoSize = true;
            this.chkTagSeparateNonImages.Checked = true;
            this.chkTagSeparateNonImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagSeparateNonImages.Location = new System.Drawing.Point(14, 62);
            this.chkTagSeparateNonImages.Name = "chkTagSeparateNonImages";
            this.chkTagSeparateNonImages.Size = new System.Drawing.Size(134, 17);
            this.chkTagSeparateNonImages.TabIndex = 6;
            this.chkTagSeparateNonImages.Text = "Separate non-images";
            this.TouchingTips.SetToolTip(this.chkTagSeparateNonImages, "Separates files that are not images (gif, webm, swf...) to a separate folder");
            this.chkTagSeparateNonImages.UseVisualStyleBackColor = true;
            // 
            // panelTagsRatings
            // 
            this.panelTagsRatings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelTagsRatings.Controls.Add(this.chkTagsDownloadQuestionable);
            this.panelTagsRatings.Controls.Add(this.chkTagsDownloadSafe);
            this.panelTagsRatings.Controls.Add(this.chkTagsSeparateRatings);
            this.panelTagsRatings.Controls.Add(this.chkTagsDownloadExplicit);
            this.panelTagsRatings.Location = new System.Drawing.Point(118, 36);
            this.panelTagsRatings.Name = "panelTagsRatings";
            this.panelTagsRatings.Size = new System.Drawing.Size(180, 140);
            this.panelTagsRatings.TabIndex = 11;
            this.panelTagsRatings.Visible = false;
            // 
            // chkTagsDownloadQuestionable
            // 
            this.chkTagsDownloadQuestionable.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkTagsDownloadQuestionable.AutoSize = true;
            this.chkTagsDownloadQuestionable.Checked = true;
            this.chkTagsDownloadQuestionable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagsDownloadQuestionable.Location = new System.Drawing.Point(33, 46);
            this.chkTagsDownloadQuestionable.Name = "chkTagsDownloadQuestionable";
            this.chkTagsDownloadQuestionable.Size = new System.Drawing.Size(144, 17);
            this.chkTagsDownloadQuestionable.TabIndex = 6;
            this.chkTagsDownloadQuestionable.Text = "Save Questionable files";
            this.TouchingTips.SetToolTip(this.chkTagsDownloadQuestionable, "Download images rated Questionable");
            this.chkTagsDownloadQuestionable.UseVisualStyleBackColor = true;
            // 
            // chkTagsDownloadSafe
            // 
            this.chkTagsDownloadSafe.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkTagsDownloadSafe.AutoSize = true;
            this.chkTagsDownloadSafe.Checked = true;
            this.chkTagsDownloadSafe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagsDownloadSafe.Location = new System.Drawing.Point(33, 69);
            this.chkTagsDownloadSafe.Name = "chkTagsDownloadSafe";
            this.chkTagsDownloadSafe.Size = new System.Drawing.Size(97, 17);
            this.chkTagsDownloadSafe.TabIndex = 7;
            this.chkTagsDownloadSafe.Text = "Save Safe files";
            this.TouchingTips.SetToolTip(this.chkTagsDownloadSafe, "Download images rated Safe");
            this.chkTagsDownloadSafe.UseVisualStyleBackColor = true;
            // 
            // chkTagsSeparateRatings
            // 
            this.chkTagsSeparateRatings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkTagsSeparateRatings.AutoSize = true;
            this.chkTagsSeparateRatings.Checked = true;
            this.chkTagsSeparateRatings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagsSeparateRatings.Location = new System.Drawing.Point(33, 104);
            this.chkTagsSeparateRatings.Name = "chkTagsSeparateRatings";
            this.chkTagsSeparateRatings.Size = new System.Drawing.Size(109, 17);
            this.chkTagsSeparateRatings.TabIndex = 8;
            this.chkTagsSeparateRatings.Text = "Separate ratings";
            this.TouchingTips.SetToolTip(this.chkTagsSeparateRatings, "Separates ratings into different folders");
            this.chkTagsSeparateRatings.UseVisualStyleBackColor = true;
            // 
            // chkTagsDownloadExplicit
            // 
            this.chkTagsDownloadExplicit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkTagsDownloadExplicit.AutoSize = true;
            this.chkTagsDownloadExplicit.Checked = true;
            this.chkTagsDownloadExplicit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagsDownloadExplicit.Location = new System.Drawing.Point(33, 23);
            this.chkTagsDownloadExplicit.Name = "chkTagsDownloadExplicit";
            this.chkTagsDownloadExplicit.Size = new System.Drawing.Size(111, 17);
            this.chkTagsDownloadExplicit.TabIndex = 5;
            this.chkTagsDownloadExplicit.Text = "Save Explicit files";
            this.TouchingTips.SetToolTip(this.chkTagsDownloadExplicit, "Download images rated Explicit");
            this.chkTagsDownloadExplicit.UseVisualStyleBackColor = true;
            // 
            // panelTagsLimits
            // 
            this.panelTagsLimits.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelTagsLimits.Controls.Add(this.lbTagsImageLimit);
            this.panelTagsLimits.Controls.Add(this.lbTagsPageLimit);
            this.panelTagsLimits.Controls.Add(this.numTagsImageLimit);
            this.panelTagsLimits.Controls.Add(this.numTagsPageLimit);
            this.panelTagsLimits.Location = new System.Drawing.Point(118, 36);
            this.panelTagsLimits.Name = "panelTagsLimits";
            this.panelTagsLimits.Size = new System.Drawing.Size(180, 140);
            this.panelTagsLimits.TabIndex = 10;
            this.panelTagsLimits.Visible = false;
            // 
            // lbTagsImageLimit
            // 
            this.lbTagsImageLimit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbTagsImageLimit.Location = new System.Drawing.Point(14, 90);
            this.lbTagsImageLimit.Name = "lbTagsImageLimit";
            this.lbTagsImageLimit.Size = new System.Drawing.Size(65, 20);
            this.lbTagsImageLimit.TabIndex = 5;
            this.lbTagsImageLimit.Text = "Image limit";
            this.lbTagsImageLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbTagsPageLimit
            // 
            this.lbTagsPageLimit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbTagsPageLimit.Location = new System.Drawing.Point(14, 50);
            this.lbTagsPageLimit.Name = "lbTagsPageLimit";
            this.lbTagsPageLimit.Size = new System.Drawing.Size(65, 20);
            this.lbTagsPageLimit.TabIndex = 4;
            this.lbTagsPageLimit.Text = "Page limit";
            this.lbTagsPageLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numTagsImageLimit
            // 
            this.numTagsImageLimit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numTagsImageLimit.Location = new System.Drawing.Point(85, 90);
            this.numTagsImageLimit.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numTagsImageLimit.Name = "numTagsImageLimit";
            this.numTagsImageLimit.Size = new System.Drawing.Size(80, 22);
            this.numTagsImageLimit.TabIndex = 1;
            this.numTagsImageLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TouchingTips.SetToolTip(this.numTagsImageLimit, "Limits downloads to a certain amount of images");
            // 
            // numTagsPageLimit
            // 
            this.numTagsPageLimit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numTagsPageLimit.Location = new System.Drawing.Point(85, 50);
            this.numTagsPageLimit.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numTagsPageLimit.Name = "numTagsPageLimit";
            this.numTagsPageLimit.Size = new System.Drawing.Size(80, 22);
            this.numTagsPageLimit.TabIndex = 1;
            this.numTagsPageLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TouchingTips.SetToolTip(this.numTagsPageLimit, "The amount of pages that will be downloaded (320 images per page)");
            // 
            // panelTagsMinimumFavorites
            // 
            this.panelTagsMinimumFavorites.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelTagsMinimumFavorites.Controls.Add(this.chkTagsMinimumFavoritesAsTag);
            this.panelTagsMinimumFavorites.Controls.Add(this.numTagsMinimumFavorites);
            this.panelTagsMinimumFavorites.Location = new System.Drawing.Point(118, 36);
            this.panelTagsMinimumFavorites.Name = "panelTagsMinimumFavorites";
            this.panelTagsMinimumFavorites.Size = new System.Drawing.Size(180, 140);
            this.panelTagsMinimumFavorites.TabIndex = 9;
            this.panelTagsMinimumFavorites.Visible = false;
            // 
            // chkTagsMinimumFavoritesAsTag
            // 
            this.chkTagsMinimumFavoritesAsTag.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkTagsMinimumFavoritesAsTag.AutoSize = true;
            this.chkTagsMinimumFavoritesAsTag.Checked = true;
            this.chkTagsMinimumFavoritesAsTag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagsMinimumFavoritesAsTag.Location = new System.Drawing.Point(66, 95);
            this.chkTagsMinimumFavoritesAsTag.Name = "chkTagsMinimumFavoritesAsTag";
            this.chkTagsMinimumFavoritesAsTag.Size = new System.Drawing.Size(57, 17);
            this.chkTagsMinimumFavoritesAsTag.TabIndex = 4;
            this.chkTagsMinimumFavoritesAsTag.Text = "As tag";
            this.TouchingTips.SetToolTip(this.chkTagsMinimumFavoritesAsTag, resources.GetString("chkTagsMinimumFavoritesAsTag.ToolTip"));
            this.chkTagsMinimumFavoritesAsTag.UseVisualStyleBackColor = true;
            // 
            // numTagsMinimumFavorites
            // 
            this.numTagsMinimumFavorites.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numTagsMinimumFavorites.Location = new System.Drawing.Point(63, 66);
            this.numTagsMinimumFavorites.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numTagsMinimumFavorites.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.numTagsMinimumFavorites.Name = "numTagsMinimumFavorites";
            this.numTagsMinimumFavorites.Size = new System.Drawing.Size(63, 22);
            this.numTagsMinimumFavorites.TabIndex = 5;
            this.numTagsMinimumFavorites.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TouchingTips.SetToolTip(this.numTagsMinimumFavorites, "The minimum favorites count that will be downloaded");
            // 
            // panelTagsMinimumScore
            // 
            this.panelTagsMinimumScore.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelTagsMinimumScore.Controls.Add(this.chkTagsMinimumScoreAsTag);
            this.panelTagsMinimumScore.Controls.Add(this.numTagsMinimumScore);
            this.panelTagsMinimumScore.Controls.Add(this.chkTagsUseMinimumScore);
            this.panelTagsMinimumScore.Location = new System.Drawing.Point(118, 36);
            this.panelTagsMinimumScore.Name = "panelTagsMinimumScore";
            this.panelTagsMinimumScore.Size = new System.Drawing.Size(180, 140);
            this.panelTagsMinimumScore.TabIndex = 8;
            this.panelTagsMinimumScore.Visible = false;
            // 
            // chkTagsMinimumScoreAsTag
            // 
            this.chkTagsMinimumScoreAsTag.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkTagsMinimumScoreAsTag.AutoSize = true;
            this.chkTagsMinimumScoreAsTag.Checked = true;
            this.chkTagsMinimumScoreAsTag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagsMinimumScoreAsTag.Enabled = false;
            this.chkTagsMinimumScoreAsTag.Location = new System.Drawing.Point(66, 95);
            this.chkTagsMinimumScoreAsTag.Name = "chkTagsMinimumScoreAsTag";
            this.chkTagsMinimumScoreAsTag.Size = new System.Drawing.Size(57, 17);
            this.chkTagsMinimumScoreAsTag.TabIndex = 2;
            this.chkTagsMinimumScoreAsTag.Text = "As tag";
            this.TouchingTips.SetToolTip(this.chkTagsMinimumScoreAsTag, resources.GetString("chkTagsMinimumScoreAsTag.ToolTip"));
            this.chkTagsMinimumScoreAsTag.UseVisualStyleBackColor = true;
            // 
            // numTagsMinimumScore
            // 
            this.numTagsMinimumScore.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numTagsMinimumScore.Enabled = false;
            this.numTagsMinimumScore.Location = new System.Drawing.Point(63, 66);
            this.numTagsMinimumScore.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numTagsMinimumScore.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.numTagsMinimumScore.Name = "numTagsMinimumScore";
            this.numTagsMinimumScore.Size = new System.Drawing.Size(63, 22);
            this.numTagsMinimumScore.TabIndex = 3;
            this.numTagsMinimumScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TouchingTips.SetToolTip(this.numTagsMinimumScore, "The minimum score that will be downloaded");
            // 
            // chkTagsUseMinimumScore
            // 
            this.chkTagsUseMinimumScore.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkTagsUseMinimumScore.AutoSize = true;
            this.chkTagsUseMinimumScore.Location = new System.Drawing.Point(28, 33);
            this.chkTagsUseMinimumScore.Name = "chkTagsUseMinimumScore";
            this.chkTagsUseMinimumScore.Size = new System.Drawing.Size(140, 17);
            this.chkTagsUseMinimumScore.TabIndex = 1;
            this.chkTagsUseMinimumScore.Text = "Enable minimum score";
            this.TouchingTips.SetToolTip(this.chkTagsUseMinimumScore, "Only downloads images with a score equal to or greater than provided\r\nThis option" +
        " is required because scores can be in the negatives");
            this.chkTagsUseMinimumScore.UseVisualStyleBackColor = true;
            this.chkTagsUseMinimumScore.CheckedChanged += new System.EventHandler(this.chkTagsUseMinimumScore_CheckedChanged);
            // 
            // tabPools
            // 
            this.tabPools.Controls.Add(this.chkPoolDownloadBlacklistedImages);
            this.tabPools.Controls.Add(this.chkPoolMergeBlacklisted);
            this.tabPools.Controls.Add(this.chkPoolMergeGraylisted);
            this.tabPools.Controls.Add(this.chkPoolOpenAfter);
            this.tabPools.Controls.Add(this.txtPoolId);
            this.tabPools.Controls.Add(this.btnDownloadPool);
            this.tabPools.Location = new System.Drawing.Point(4, 22);
            this.tabPools.Name = "tabPools";
            this.tabPools.Padding = new System.Windows.Forms.Padding(3);
            this.tabPools.Size = new System.Drawing.Size(304, 211);
            this.tabPools.TabIndex = 1;
            this.tabPools.Text = "Pool";
            this.tabPools.UseVisualStyleBackColor = true;
            // 
            // chkPoolDownloadBlacklistedImages
            // 
            this.chkPoolDownloadBlacklistedImages.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkPoolDownloadBlacklistedImages.AutoSize = true;
            this.chkPoolDownloadBlacklistedImages.Checked = true;
            this.chkPoolDownloadBlacklistedImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPoolDownloadBlacklistedImages.Location = new System.Drawing.Point(64, 108);
            this.chkPoolDownloadBlacklistedImages.Name = "chkPoolDownloadBlacklistedImages";
            this.chkPoolDownloadBlacklistedImages.Size = new System.Drawing.Size(176, 17);
            this.chkPoolDownloadBlacklistedImages.TabIndex = 4;
            this.chkPoolDownloadBlacklistedImages.Text = "Download blacklisted images";
            this.chkPoolDownloadBlacklistedImages.UseVisualStyleBackColor = true;
            // 
            // chkPoolMergeBlacklisted
            // 
            this.chkPoolMergeBlacklisted.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkPoolMergeBlacklisted.AutoSize = true;
            this.chkPoolMergeBlacklisted.Location = new System.Drawing.Point(60, 131);
            this.chkPoolMergeBlacklisted.Name = "chkPoolMergeBlacklisted";
            this.chkPoolMergeBlacklisted.Size = new System.Drawing.Size(184, 17);
            this.chkPoolMergeBlacklisted.TabIndex = 5;
            this.chkPoolMergeBlacklisted.Text = "Merge blacklisted with the rest";
            this.chkPoolMergeBlacklisted.UseVisualStyleBackColor = true;
            // 
            // chkPoolMergeGraylisted
            // 
            this.chkPoolMergeGraylisted.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkPoolMergeGraylisted.AutoSize = true;
            this.chkPoolMergeGraylisted.Checked = true;
            this.chkPoolMergeGraylisted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPoolMergeGraylisted.Location = new System.Drawing.Point(63, 85);
            this.chkPoolMergeGraylisted.Name = "chkPoolMergeGraylisted";
            this.chkPoolMergeGraylisted.Size = new System.Drawing.Size(179, 17);
            this.chkPoolMergeGraylisted.TabIndex = 3;
            this.chkPoolMergeGraylisted.Text = "Merge graylisted with the rest";
            this.chkPoolMergeGraylisted.UseVisualStyleBackColor = true;
            // 
            // chkPoolOpenAfter
            // 
            this.chkPoolOpenAfter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkPoolOpenAfter.AutoSize = true;
            this.chkPoolOpenAfter.Location = new System.Drawing.Point(75, 62);
            this.chkPoolOpenAfter.Name = "chkPoolOpenAfter";
            this.chkPoolOpenAfter.Size = new System.Drawing.Size(154, 17);
            this.chkPoolOpenAfter.TabIndex = 2;
            this.chkPoolOpenAfter.Text = "Open after downloading";
            this.TouchingTips.SetToolTip(this.chkPoolOpenAfter, "Opens the folder after download is finished");
            this.chkPoolOpenAfter.UseVisualStyleBackColor = true;
            // 
            // txtPoolId
            // 
            this.txtPoolId.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtPoolId.ButtonAlignment = aphrodite.Controls.ButtonAlignments.Left;
            this.txtPoolId.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtPoolId.ButtonEnabled = true;
            this.txtPoolId.ButtonFont = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPoolId.ButtonImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtPoolId.ButtonImageIndex = -1;
            this.txtPoolId.ButtonImageKey = "";
            this.txtPoolId.ButtonSize = new System.Drawing.Size(22, 19);
            this.txtPoolId.ButtonText = "X";
            this.txtPoolId.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtPoolId.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPoolId.Location = new System.Drawing.Point(97, 19);
            this.txtPoolId.Name = "txtPoolId";
            this.txtPoolId.Size = new System.Drawing.Size(110, 20);
            this.txtPoolId.TabIndex = 1;
            this.txtPoolId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPoolId.TextHint = "Pool ID...";
            this.txtPoolId.ButtonClick += new System.EventHandler(this.txtPoolId_ButtonClick);
            this.txtPoolId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPoolId_KeyDown);
            this.txtPoolId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPoolId_KeyPress);
            // 
            // btnDownloadPool
            // 
            this.btnDownloadPool.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDownloadPool.Location = new System.Drawing.Point(112, 169);
            this.btnDownloadPool.Name = "btnDownloadPool";
            this.btnDownloadPool.Size = new System.Drawing.Size(80, 23);
            this.btnDownloadPool.TabIndex = 6;
            this.btnDownloadPool.Text = "Download";
            this.btnDownloadPool.UseVisualStyleBackColor = true;
            this.btnDownloadPool.Click += new System.EventHandler(this.btnDownloadPool_Click);
            // 
            // tabImages
            // 
            this.tabImages.Controls.Add(this.chkImageSeparateBlacklisted);
            this.tabImages.Controls.Add(this.chkImageOpenAfter);
            this.tabImages.Controls.Add(this.chkImageSeparateNonImages);
            this.tabImages.Controls.Add(this.chkImageSeparateArtists);
            this.tabImages.Controls.Add(this.chkImageUseForm);
            this.tabImages.Controls.Add(this.chkImageSeparateGraylisted);
            this.tabImages.Controls.Add(this.chkImageSeparateRatings);
            this.tabImages.Controls.Add(this.txtImageUrl);
            this.tabImages.Controls.Add(this.btnDownloadImage);
            this.tabImages.Location = new System.Drawing.Point(4, 22);
            this.tabImages.Name = "tabImages";
            this.tabImages.Padding = new System.Windows.Forms.Padding(3);
            this.tabImages.Size = new System.Drawing.Size(304, 211);
            this.tabImages.TabIndex = 3;
            this.tabImages.Text = "Image";
            this.tabImages.UseVisualStyleBackColor = true;
            // 
            // chkImageSeparateBlacklisted
            // 
            this.chkImageSeparateBlacklisted.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkImageSeparateBlacklisted.AutoSize = true;
            this.chkImageSeparateBlacklisted.Checked = true;
            this.chkImageSeparateBlacklisted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImageSeparateBlacklisted.Location = new System.Drawing.Point(153, 62);
            this.chkImageSeparateBlacklisted.Name = "chkImageSeparateBlacklisted";
            this.chkImageSeparateBlacklisted.Size = new System.Drawing.Size(128, 17);
            this.chkImageSeparateBlacklisted.TabIndex = 3;
            this.chkImageSeparateBlacklisted.Text = "Separate blacklisted";
            this.chkImageSeparateBlacklisted.UseVisualStyleBackColor = true;
            // 
            // chkImageOpenAfter
            // 
            this.chkImageOpenAfter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkImageOpenAfter.AutoSize = true;
            this.chkImageOpenAfter.Location = new System.Drawing.Point(75, 131);
            this.chkImageOpenAfter.Name = "chkImageOpenAfter";
            this.chkImageOpenAfter.Size = new System.Drawing.Size(154, 17);
            this.chkImageOpenAfter.TabIndex = 8;
            this.chkImageOpenAfter.Text = "Open after downloading";
            this.TouchingTips.SetToolTip(this.chkImageOpenAfter, "Opens the folder after download is finished");
            this.chkImageOpenAfter.UseVisualStyleBackColor = true;
            // 
            // chkImageSeparateNonImages
            // 
            this.chkImageSeparateNonImages.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkImageSeparateNonImages.AutoSize = true;
            this.chkImageSeparateNonImages.Checked = true;
            this.chkImageSeparateNonImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImageSeparateNonImages.Location = new System.Drawing.Point(143, 85);
            this.chkImageSeparateNonImages.Name = "chkImageSeparateNonImages";
            this.chkImageSeparateNonImages.Size = new System.Drawing.Size(134, 17);
            this.chkImageSeparateNonImages.TabIndex = 5;
            this.chkImageSeparateNonImages.Text = "Separate non-images";
            this.TouchingTips.SetToolTip(this.chkImageSeparateNonImages, "Separates files that are not images (jpg, png, bmg...) to a separate folder");
            this.chkImageSeparateNonImages.UseVisualStyleBackColor = true;
            // 
            // chkImageSeparateArtists
            // 
            this.chkImageSeparateArtists.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkImageSeparateArtists.AutoSize = true;
            this.chkImageSeparateArtists.Location = new System.Drawing.Point(15, 108);
            this.chkImageSeparateArtists.Name = "chkImageSeparateArtists";
            this.chkImageSeparateArtists.Size = new System.Drawing.Size(104, 17);
            this.chkImageSeparateArtists.TabIndex = 6;
            this.chkImageSeparateArtists.Text = "Separate artists";
            this.chkImageSeparateArtists.UseVisualStyleBackColor = true;
            // 
            // chkImageUseForm
            // 
            this.chkImageUseForm.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkImageUseForm.AutoSize = true;
            this.chkImageUseForm.Location = new System.Drawing.Point(125, 108);
            this.chkImageUseForm.Name = "chkImageUseForm";
            this.chkImageUseForm.Size = new System.Drawing.Size(164, 17);
            this.chkImageUseForm.TabIndex = 7;
            this.chkImageUseForm.Text = "Use form to show progress";
            this.chkImageUseForm.UseVisualStyleBackColor = true;
            // 
            // chkImageSeparateGraylisted
            // 
            this.chkImageSeparateGraylisted.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkImageSeparateGraylisted.AutoSize = true;
            this.chkImageSeparateGraylisted.Checked = true;
            this.chkImageSeparateGraylisted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImageSeparateGraylisted.Location = new System.Drawing.Point(24, 62);
            this.chkImageSeparateGraylisted.Name = "chkImageSeparateGraylisted";
            this.chkImageSeparateGraylisted.Size = new System.Drawing.Size(123, 17);
            this.chkImageSeparateGraylisted.TabIndex = 2;
            this.chkImageSeparateGraylisted.Text = "Separate graylisted";
            this.chkImageSeparateGraylisted.UseVisualStyleBackColor = true;
            // 
            // chkImageSeparateRatings
            // 
            this.chkImageSeparateRatings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkImageSeparateRatings.AutoSize = true;
            this.chkImageSeparateRatings.Checked = true;
            this.chkImageSeparateRatings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImageSeparateRatings.Location = new System.Drawing.Point(28, 85);
            this.chkImageSeparateRatings.Name = "chkImageSeparateRatings";
            this.chkImageSeparateRatings.Size = new System.Drawing.Size(109, 17);
            this.chkImageSeparateRatings.TabIndex = 4;
            this.chkImageSeparateRatings.Text = "Separate ratings";
            this.chkImageSeparateRatings.UseVisualStyleBackColor = true;
            // 
            // txtImageUrl
            // 
            this.txtImageUrl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtImageUrl.ButtonAlignment = aphrodite.Controls.ButtonAlignments.Left;
            this.txtImageUrl.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtImageUrl.ButtonEnabled = true;
            this.txtImageUrl.ButtonFont = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtImageUrl.ButtonImageIndex = -1;
            this.txtImageUrl.ButtonImageKey = "";
            this.txtImageUrl.ButtonSize = new System.Drawing.Size(22, 19);
            this.txtImageUrl.ButtonText = "X";
            this.txtImageUrl.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtImageUrl.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtImageUrl.Location = new System.Drawing.Point(57, 19);
            this.txtImageUrl.Name = "txtImageUrl";
            this.txtImageUrl.Size = new System.Drawing.Size(190, 20);
            this.txtImageUrl.TabIndex = 1;
            this.txtImageUrl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtImageUrl.TextHint = "Image ID / URL...";
            this.TouchingTips.SetToolTip(this.txtImageUrl, "The image that will be downloaded");
            this.txtImageUrl.ButtonClick += new System.EventHandler(this.txtImageUrl_ButtonClick);
            this.txtImageUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtImageUrl_KeyDown);
            // 
            // btnDownloadImage
            // 
            this.btnDownloadImage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDownloadImage.Location = new System.Drawing.Point(112, 169);
            this.btnDownloadImage.Name = "btnDownloadImage";
            this.btnDownloadImage.Size = new System.Drawing.Size(80, 23);
            this.btnDownloadImage.TabIndex = 9;
            this.btnDownloadImage.Text = "Download";
            this.btnDownloadImage.UseVisualStyleBackColor = true;
            this.btnDownloadImage.Click += new System.EventHandler(this.btnDownloadImage_Click);
            // 
            // tabPortable
            // 
            this.tabPortable.Controls.Add(this.lbPortable);
            this.tabPortable.Location = new System.Drawing.Point(4, 22);
            this.tabPortable.Name = "tabPortable";
            this.tabPortable.Padding = new System.Windows.Forms.Padding(3);
            this.tabPortable.Size = new System.Drawing.Size(304, 211);
            this.tabPortable.TabIndex = 2;
            this.tabPortable.Text = "// Portable Mode \\\\";
            this.tabPortable.UseVisualStyleBackColor = true;
            // 
            // lbPortable
            // 
            this.lbPortable.AutoSize = true;
            this.lbPortable.Location = new System.Drawing.Point(14, 56);
            this.lbPortable.Name = "lbPortable";
            this.lbPortable.Size = new System.Drawing.Size(278, 65);
            this.lbPortable.TabIndex = 0;
            this.lbPortable.Text = "aphrodite.ini will be used to store and read settings.\r\n\r\nChange aphrodite.ini to" +
    " use system\'s settings.\r\n\r\nor delete it, your call.";
            // 
            // toolMenu
            // 
            this.toolMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mSettings,
            this.mBlacklist,
            this.mTools,
            this.mAbout});
            // 
            // mSettings
            // 
            this.mSettings.Index = 0;
            this.mSettings.Text = "settings";
            this.mSettings.Click += new System.EventHandler(this.mSettings_Click);
            // 
            // mBlacklist
            // 
            this.mBlacklist.Index = 1;
            this.mBlacklist.Text = "blacklist";
            this.mBlacklist.Click += new System.EventHandler(this.mBlacklist_Click);
            // 
            // mTools
            // 
            this.mTools.Index = 2;
            this.mTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mWishlist,
            this.mRedownloader,
            this.mReverseSearch,
            this.mToolsSeparator,
            this.mLog});
            this.mTools.Text = "tools";
            // 
            // mWishlist
            // 
            this.mWishlist.Index = 0;
            this.mWishlist.Text = "pool wishlist";
            this.mWishlist.Click += new System.EventHandler(this.mWishlist_Click);
            // 
            // mRedownloader
            // 
            this.mRedownloader.Index = 1;
            this.mRedownloader.Text = "redownloader";
            this.mRedownloader.Click += new System.EventHandler(this.mRedownloader_Click);
            // 
            // mReverseSearch
            // 
            this.mReverseSearch.Index = 2;
            this.mReverseSearch.Text = "reverse image search";
            this.mReverseSearch.Click += new System.EventHandler(this.mReverseSearch_Click);
            // 
            // mToolsSeparator
            // 
            this.mToolsSeparator.Index = 3;
            this.mToolsSeparator.Text = "-";
            // 
            // mLog
            // 
            this.mLog.Index = 4;
            this.mLog.Text = "aphrodite log";
            this.mLog.Click += new System.EventHandler(this.mLog_Click);
            // 
            // mAbout
            // 
            this.mAbout.Index = 3;
            this.mAbout.Text = "about";
            this.mAbout.Click += new System.EventHandler(this.mAbout_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(312, 237);
            this.Controls.Add(this.tabMain);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(330, 295);
            this.Menu = this.toolMenu;
            this.MinimumSize = new System.Drawing.Size(330, 295);
            this.Name = "frmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "aphrodite";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.tabMain.ResumeLayout(false);
            this.tabTags.ResumeLayout(false);
            this.tabTags.PerformLayout();
            this.panelTagsOtherSettings.ResumeLayout(false);
            this.panelTagsOtherSettings.PerformLayout();
            this.panelTagsRatings.ResumeLayout(false);
            this.panelTagsRatings.PerformLayout();
            this.panelTagsLimits.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numTagsImageLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsPageLimit)).EndInit();
            this.panelTagsMinimumFavorites.ResumeLayout(false);
            this.panelTagsMinimumFavorites.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsMinimumFavorites)).EndInit();
            this.panelTagsMinimumScore.ResumeLayout(false);
            this.panelTagsMinimumScore.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsMinimumScore)).EndInit();
            this.tabPools.ResumeLayout(false);
            this.tabPools.PerformLayout();
            this.tabImages.ResumeLayout(false);
            this.tabImages.PerformLayout();
            this.tabPortable.ResumeLayout(false);
            this.tabPortable.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabTags;
        private System.Windows.Forms.TabPage tabPools;
        private System.Windows.Forms.MainMenu toolMenu;
        private System.Windows.Forms.MenuItem mSettings;
        private System.Windows.Forms.MenuItem mAbout;
        private System.Windows.Forms.MenuItem mBlacklist;
        private Controls.ExtendedButton btnDownloadTags;
        private System.Windows.Forms.NumericUpDown numTagsImageLimit;
        private System.Windows.Forms.CheckBox chkTagsUseMinimumScore;
        private System.Windows.Forms.NumericUpDown numTagsMinimumScore;
        private System.Windows.Forms.CheckBox chkPoolMergeGraylisted;
        private System.Windows.Forms.CheckBox chkPoolOpenAfter;
        private Controls.ExtendedButton btnDownloadPool;
        private Controls.ExtendedTextBox txtPoolId;
        private System.Windows.Forms.MenuItem mTools;
        private System.Windows.Forms.MenuItem mWishlist;
        private System.Windows.Forms.MenuItem mRedownloader;
        private System.Windows.Forms.MenuItem mReverseSearch;
        private System.Windows.Forms.NumericUpDown numTagsPageLimit;
        private System.Windows.Forms.Label lbAwoo;
        private System.Windows.Forms.CheckBox chkTagsMinimumScoreAsTag;
        private System.Windows.Forms.ToolTip TouchingTips;
        private System.Windows.Forms.TabPage tabPortable;
        private System.Windows.Forms.Label lbPortable;
        private System.Windows.Forms.TabPage tabImages;
        private Controls.ExtendedTextBox txtImageUrl;
        private Controls.ExtendedButton btnDownloadImage;
        private System.Windows.Forms.CheckBox chkImageUseForm;
        private System.Windows.Forms.CheckBox chkImageSeparateGraylisted;
        private System.Windows.Forms.CheckBox chkImageSeparateRatings;
        private System.Windows.Forms.CheckBox chkImageSeparateArtists;
        private System.Windows.Forms.CheckBox chkImageSeparateNonImages;
        private System.Windows.Forms.CheckBox chkTagSeparateNonImages;
        private System.Windows.Forms.CheckBox chkTagsOpenAfterDownload;
        private System.Windows.Forms.CheckBox chkImageOpenAfter;
        private System.Windows.Forms.MenuItem mToolsSeparator;
        private System.Windows.Forms.MenuItem mLog;
        private Controls.ExtendedTextBox txtTags;
        private System.Windows.Forms.CheckBox chkPoolMergeBlacklisted;
        private System.Windows.Forms.CheckBox chkPoolDownloadBlacklistedImages;
        private System.Windows.Forms.CheckBox chkImageSeparateBlacklisted;
        private System.Windows.Forms.CheckBox chkTagsDownloadInUploadOrder;
        private System.Windows.Forms.Panel panelTagsMinimumScore;
        private System.Windows.Forms.Panel panelTagsMinimumFavorites;
        private System.Windows.Forms.CheckBox chkTagsMinimumFavoritesAsTag;
        private System.Windows.Forms.NumericUpDown numTagsMinimumFavorites;
        private System.Windows.Forms.Panel panelTagsLimits;
        private System.Windows.Forms.Label lbTagsImageLimit;
        private System.Windows.Forms.Label lbTagsPageLimit;
        private System.Windows.Forms.Panel panelTagsRatings;
        private System.Windows.Forms.CheckBox chkTagsDownloadQuestionable;
        private System.Windows.Forms.CheckBox chkTagsDownloadSafe;
        private System.Windows.Forms.CheckBox chkTagsSeparateRatings;
        private System.Windows.Forms.CheckBox chkTagsDownloadExplicit;
        private System.Windows.Forms.Panel panelTagsOtherSettings;
        private System.Windows.Forms.Label lbTagsLimitsHint;
        private System.Windows.Forms.RadioButton rbTagsOtherSettings;
        private System.Windows.Forms.RadioButton rbTagsRatings;
        private System.Windows.Forms.RadioButton rbTagsLimits;
        private System.Windows.Forms.RadioButton rbTagsMinimumFavorites;
        private System.Windows.Forms.RadioButton rbTagsMinimumScore;
        private System.Windows.Forms.Label lbTagsSeparator;
    }
}

