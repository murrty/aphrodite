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
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabTags = new System.Windows.Forms.TabPage();
            this.chkTagsOpenAfterDownload = new System.Windows.Forms.CheckBox();
            this.chkTagSeparateNonImages = new System.Windows.Forms.CheckBox();
            this.gbtPageLimit = new System.Windows.Forms.GroupBox();
            this.numTagsPageLimit = new System.Windows.Forms.NumericUpDown();
            this.gbtRatings = new System.Windows.Forms.GroupBox();
            this.chkTagsSeparateRatings = new System.Windows.Forms.CheckBox();
            this.chkTagsDownloadExplicit = new System.Windows.Forms.CheckBox();
            this.chkTagsDownloadQuestionable = new System.Windows.Forms.CheckBox();
            this.chkTagsDownloadSafe = new System.Windows.Forms.CheckBox();
            this.gbtImageLimit = new System.Windows.Forms.GroupBox();
            this.numTagsImageLimit = new System.Windows.Forms.NumericUpDown();
            this.btnDownloadTags = new aphrodite.Controls.ExtendedButton();
            this.txtTags = new aphrodite.Controls.ExtendedTextBox();
            this.gbtScore = new System.Windows.Forms.GroupBox();
            this.chkTagsUseScoreAsTag = new System.Windows.Forms.CheckBox();
            this.chkTagsUseMinimumScore = new System.Windows.Forms.CheckBox();
            this.numTagsMinimumScore = new System.Windows.Forms.NumericUpDown();
            this.lbAwoo = new System.Windows.Forms.Label();
            this.chkTagsDownloadInUploadOrder = new System.Windows.Forms.CheckBox();
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
            this.gbtPageLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsPageLimit)).BeginInit();
            this.gbtRatings.SuspendLayout();
            this.gbtImageLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsImageLimit)).BeginInit();
            this.gbtScore.SuspendLayout();
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
            this.tabTags.Controls.Add(this.chkTagsOpenAfterDownload);
            this.tabTags.Controls.Add(this.chkTagSeparateNonImages);
            this.tabTags.Controls.Add(this.gbtPageLimit);
            this.tabTags.Controls.Add(this.gbtRatings);
            this.tabTags.Controls.Add(this.gbtImageLimit);
            this.tabTags.Controls.Add(this.btnDownloadTags);
            this.tabTags.Controls.Add(this.txtTags);
            this.tabTags.Controls.Add(this.gbtScore);
            this.tabTags.Controls.Add(this.lbAwoo);
            this.tabTags.Controls.Add(this.chkTagsDownloadInUploadOrder);
            this.tabTags.Location = new System.Drawing.Point(4, 22);
            this.tabTags.Name = "tabTags";
            this.tabTags.Padding = new System.Windows.Forms.Padding(3);
            this.tabTags.Size = new System.Drawing.Size(304, 211);
            this.tabTags.TabIndex = 0;
            this.tabTags.Text = "Tag(s)";
            this.tabTags.UseVisualStyleBackColor = true;
            // 
            // chkTagsOpenAfterDownload
            // 
            this.chkTagsOpenAfterDownload.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkTagsOpenAfterDownload.AutoSize = true;
            this.chkTagsOpenAfterDownload.Location = new System.Drawing.Point(6, 145);
            this.chkTagsOpenAfterDownload.Name = "chkTagsOpenAfterDownload";
            this.chkTagsOpenAfterDownload.Size = new System.Drawing.Size(154, 17);
            this.chkTagsOpenAfterDownload.TabIndex = 9;
            this.chkTagsOpenAfterDownload.Text = "Open after downloading";
            this.TouchingTips.SetToolTip(this.chkTagsOpenAfterDownload, "Opens the folder after download is finished");
            this.chkTagsOpenAfterDownload.UseVisualStyleBackColor = true;
            // 
            // chkTagSeparateNonImages
            // 
            this.chkTagSeparateNonImages.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkTagSeparateNonImages.AutoSize = true;
            this.chkTagSeparateNonImages.Checked = true;
            this.chkTagSeparateNonImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagSeparateNonImages.Location = new System.Drawing.Point(163, 156);
            this.chkTagSeparateNonImages.Name = "chkTagSeparateNonImages";
            this.chkTagSeparateNonImages.Size = new System.Drawing.Size(134, 17);
            this.chkTagSeparateNonImages.TabIndex = 6;
            this.chkTagSeparateNonImages.Text = "Separate non-images";
            this.TouchingTips.SetToolTip(this.chkTagSeparateNonImages, "Separates files that are not images (jpg, png, bmg...) to a separate folder");
            this.chkTagSeparateNonImages.UseVisualStyleBackColor = true;
            // 
            // gbtPageLimit
            // 
            this.gbtPageLimit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gbtPageLimit.Controls.Add(this.numTagsPageLimit);
            this.gbtPageLimit.Location = new System.Drawing.Point(9, 36);
            this.gbtPageLimit.Name = "gbtPageLimit";
            this.gbtPageLimit.Size = new System.Drawing.Size(136, 46);
            this.gbtPageLimit.TabIndex = 2;
            this.gbtPageLimit.TabStop = false;
            this.gbtPageLimit.Text = "Page limit (0 = off)";
            // 
            // numTagsPageLimit
            // 
            this.numTagsPageLimit.Location = new System.Drawing.Point(37, 19);
            this.numTagsPageLimit.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numTagsPageLimit.Name = "numTagsPageLimit";
            this.numTagsPageLimit.Size = new System.Drawing.Size(63, 22);
            this.numTagsPageLimit.TabIndex = 1;
            this.numTagsPageLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TouchingTips.SetToolTip(this.numTagsPageLimit, "The amount of pages that will be downloaded (320 images per page)");
            // 
            // gbtRatings
            // 
            this.gbtRatings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gbtRatings.Controls.Add(this.chkTagsSeparateRatings);
            this.gbtRatings.Controls.Add(this.chkTagsDownloadExplicit);
            this.gbtRatings.Controls.Add(this.chkTagsDownloadQuestionable);
            this.gbtRatings.Controls.Add(this.chkTagsDownloadSafe);
            this.gbtRatings.Location = new System.Drawing.Point(159, 102);
            this.gbtRatings.Name = "gbtRatings";
            this.gbtRatings.Size = new System.Drawing.Size(136, 51);
            this.gbtRatings.TabIndex = 5;
            this.gbtRatings.TabStop = false;
            this.gbtRatings.Text = "Ratings to download";
            // 
            // chkTagsSeparateRatings
            // 
            this.chkTagsSeparateRatings.AutoSize = true;
            this.chkTagsSeparateRatings.Checked = true;
            this.chkTagsSeparateRatings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagsSeparateRatings.Location = new System.Drawing.Point(16, 31);
            this.chkTagsSeparateRatings.Name = "chkTagsSeparateRatings";
            this.chkTagsSeparateRatings.Size = new System.Drawing.Size(109, 17);
            this.chkTagsSeparateRatings.TabIndex = 4;
            this.chkTagsSeparateRatings.Text = "Separate ratings";
            this.TouchingTips.SetToolTip(this.chkTagsSeparateRatings, "Separates ratings into different folders");
            this.chkTagsSeparateRatings.UseVisualStyleBackColor = true;
            // 
            // chkTagsDownloadExplicit
            // 
            this.chkTagsDownloadExplicit.AutoSize = true;
            this.chkTagsDownloadExplicit.Checked = true;
            this.chkTagsDownloadExplicit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagsDownloadExplicit.Location = new System.Drawing.Point(15, 14);
            this.chkTagsDownloadExplicit.Name = "chkTagsDownloadExplicit";
            this.chkTagsDownloadExplicit.Size = new System.Drawing.Size(31, 17);
            this.chkTagsDownloadExplicit.TabIndex = 1;
            this.chkTagsDownloadExplicit.Text = "E";
            this.TouchingTips.SetToolTip(this.chkTagsDownloadExplicit, "Download images rated Explicit");
            this.chkTagsDownloadExplicit.UseVisualStyleBackColor = true;
            // 
            // chkTagsDownloadQuestionable
            // 
            this.chkTagsDownloadQuestionable.AutoSize = true;
            this.chkTagsDownloadQuestionable.Checked = true;
            this.chkTagsDownloadQuestionable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagsDownloadQuestionable.Location = new System.Drawing.Point(53, 14);
            this.chkTagsDownloadQuestionable.Name = "chkTagsDownloadQuestionable";
            this.chkTagsDownloadQuestionable.Size = new System.Drawing.Size(33, 17);
            this.chkTagsDownloadQuestionable.TabIndex = 2;
            this.chkTagsDownloadQuestionable.Text = "Q";
            this.TouchingTips.SetToolTip(this.chkTagsDownloadQuestionable, "Download images rated Questionable");
            this.chkTagsDownloadQuestionable.UseVisualStyleBackColor = true;
            // 
            // chkTagsDownloadSafe
            // 
            this.chkTagsDownloadSafe.AutoSize = true;
            this.chkTagsDownloadSafe.Checked = true;
            this.chkTagsDownloadSafe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagsDownloadSafe.Location = new System.Drawing.Point(92, 14);
            this.chkTagsDownloadSafe.Name = "chkTagsDownloadSafe";
            this.chkTagsDownloadSafe.Size = new System.Drawing.Size(31, 17);
            this.chkTagsDownloadSafe.TabIndex = 3;
            this.chkTagsDownloadSafe.Text = "S";
            this.TouchingTips.SetToolTip(this.chkTagsDownloadSafe, "Download images rated Safe");
            this.chkTagsDownloadSafe.UseVisualStyleBackColor = true;
            // 
            // gbtImageLimit
            // 
            this.gbtImageLimit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gbtImageLimit.Controls.Add(this.numTagsImageLimit);
            this.gbtImageLimit.Location = new System.Drawing.Point(9, 88);
            this.gbtImageLimit.Name = "gbtImageLimit";
            this.gbtImageLimit.Size = new System.Drawing.Size(138, 51);
            this.gbtImageLimit.TabIndex = 4;
            this.gbtImageLimit.TabStop = false;
            this.gbtImageLimit.Text = "Image limit (0 = off)";
            // 
            // numTagsImageLimit
            // 
            this.numTagsImageLimit.Location = new System.Drawing.Point(29, 21);
            this.numTagsImageLimit.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numTagsImageLimit.Name = "numTagsImageLimit";
            this.numTagsImageLimit.Size = new System.Drawing.Size(81, 22);
            this.numTagsImageLimit.TabIndex = 1;
            this.numTagsImageLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TouchingTips.SetToolTip(this.numTagsImageLimit, "Limits downloads to a certain amount of images");
            // 
            // btnDownloadTags
            // 
            this.btnDownloadTags.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDownloadTags.Location = new System.Drawing.Point(112, 182);
            this.btnDownloadTags.Name = "btnDownloadTags";
            this.btnDownloadTags.Size = new System.Drawing.Size(81, 23);
            this.btnDownloadTags.TabIndex = 8;
            this.btnDownloadTags.Text = "Download";
            this.btnDownloadTags.UseVisualStyleBackColor = true;
            this.btnDownloadTags.Click += new System.EventHandler(this.btnDownloadTags_Click);
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
            // gbtScore
            // 
            this.gbtScore.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gbtScore.Controls.Add(this.chkTagsUseScoreAsTag);
            this.gbtScore.Controls.Add(this.chkTagsUseMinimumScore);
            this.gbtScore.Controls.Add(this.numTagsMinimumScore);
            this.gbtScore.Location = new System.Drawing.Point(160, 36);
            this.gbtScore.Name = "gbtScore";
            this.gbtScore.Size = new System.Drawing.Size(136, 65);
            this.gbtScore.TabIndex = 3;
            this.gbtScore.TabStop = false;
            this.gbtScore.Text = "Score minimum";
            // 
            // chkTagsUseScoreAsTag
            // 
            this.chkTagsUseScoreAsTag.AutoSize = true;
            this.chkTagsUseScoreAsTag.Checked = true;
            this.chkTagsUseScoreAsTag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagsUseScoreAsTag.Enabled = false;
            this.chkTagsUseScoreAsTag.Location = new System.Drawing.Point(74, 18);
            this.chkTagsUseScoreAsTag.Name = "chkTagsUseScoreAsTag";
            this.chkTagsUseScoreAsTag.Size = new System.Drawing.Size(57, 17);
            this.chkTagsUseScoreAsTag.TabIndex = 2;
            this.chkTagsUseScoreAsTag.Text = "As tag";
            this.TouchingTips.SetToolTip(this.chkTagsUseScoreAsTag, "If checked, uses the score minimum as a tag (ex: \"gay score:>25\").\r\nThis widly in" +
        "creases the images that will be downloaded.\r\nOnly used if there are 5 or less ta" +
        "gs being queried");
            this.chkTagsUseScoreAsTag.UseVisualStyleBackColor = true;
            // 
            // chkTagsUseMinimumScore
            // 
            this.chkTagsUseMinimumScore.AutoSize = true;
            this.chkTagsUseMinimumScore.Location = new System.Drawing.Point(12, 18);
            this.chkTagsUseMinimumScore.Name = "chkTagsUseMinimumScore";
            this.chkTagsUseMinimumScore.Size = new System.Drawing.Size(60, 17);
            this.chkTagsUseMinimumScore.TabIndex = 1;
            this.chkTagsUseMinimumScore.Text = "Enable";
            this.TouchingTips.SetToolTip(this.chkTagsUseMinimumScore, "Only downloads images with a score equal to or greater than provided");
            this.chkTagsUseMinimumScore.UseVisualStyleBackColor = true;
            this.chkTagsUseMinimumScore.CheckedChanged += new System.EventHandler(this.chkTagsUseMinimumScore_CheckedChanged);
            // 
            // numTagsMinimumScore
            // 
            this.numTagsMinimumScore.Enabled = false;
            this.numTagsMinimumScore.Location = new System.Drawing.Point(37, 39);
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
            this.TouchingTips.SetToolTip(this.numTagsMinimumScore, "The minimum score required");
            // 
            // lbAwoo
            // 
            this.lbAwoo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lbAwoo.AutoSize = true;
            this.lbAwoo.Location = new System.Drawing.Point(124, 172);
            this.lbAwoo.Name = "lbAwoo";
            this.lbAwoo.Size = new System.Drawing.Size(57, 13);
            this.lbAwoo.TabIndex = 7;
            this.lbAwoo.Text = "awooooo";
            // 
            // chkTagsDownloadInUploadOrder
            // 
            this.chkTagsDownloadInUploadOrder.AutoSize = true;
            this.chkTagsDownloadInUploadOrder.Location = new System.Drawing.Point(6, 168);
            this.chkTagsDownloadInUploadOrder.Name = "chkTagsDownloadInUploadOrder";
            this.chkTagsDownloadInUploadOrder.Size = new System.Drawing.Size(123, 17);
            this.chkTagsDownloadInUploadOrder.TabIndex = 10;
            this.chkTagsDownloadInUploadOrder.Text = "Download in order";
            this.TouchingTips.SetToolTip(this.chkTagsDownloadInUploadOrder, "Download tags in order of newest to oldest");
            this.chkTagsDownloadInUploadOrder.UseVisualStyleBackColor = true;
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
            this.chkPoolDownloadBlacklistedImages.AutoSize = true;
            this.chkPoolDownloadBlacklistedImages.Checked = true;
            this.chkPoolDownloadBlacklistedImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPoolDownloadBlacklistedImages.Location = new System.Drawing.Point(64, 105);
            this.chkPoolDownloadBlacklistedImages.Name = "chkPoolDownloadBlacklistedImages";
            this.chkPoolDownloadBlacklistedImages.Size = new System.Drawing.Size(176, 17);
            this.chkPoolDownloadBlacklistedImages.TabIndex = 6;
            this.chkPoolDownloadBlacklistedImages.Text = "Download blacklisted images";
            this.chkPoolDownloadBlacklistedImages.UseVisualStyleBackColor = true;
            // 
            // chkPoolMergeBlacklisted
            // 
            this.chkPoolMergeBlacklisted.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkPoolMergeBlacklisted.AutoSize = true;
            this.chkPoolMergeBlacklisted.Location = new System.Drawing.Point(60, 128);
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
            this.chkPoolMergeGraylisted.Location = new System.Drawing.Point(63, 82);
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
            this.chkPoolOpenAfter.Location = new System.Drawing.Point(75, 59);
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
            this.txtPoolId.Location = new System.Drawing.Point(102, 26);
            this.txtPoolId.Name = "txtPoolId";
            this.txtPoolId.Size = new System.Drawing.Size(100, 20);
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
            this.btnDownloadPool.Location = new System.Drawing.Point(112, 156);
            this.btnDownloadPool.Name = "btnDownloadPool";
            this.btnDownloadPool.Size = new System.Drawing.Size(80, 23);
            this.btnDownloadPool.TabIndex = 4;
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
            this.chkImageSeparateBlacklisted.Location = new System.Drawing.Point(153, 64);
            this.chkImageSeparateBlacklisted.Name = "chkImageSeparateBlacklisted";
            this.chkImageSeparateBlacklisted.Size = new System.Drawing.Size(128, 17);
            this.chkImageSeparateBlacklisted.TabIndex = 11;
            this.chkImageSeparateBlacklisted.Text = "Separate blacklisted";
            this.chkImageSeparateBlacklisted.UseVisualStyleBackColor = true;
            // 
            // chkImageOpenAfter
            // 
            this.chkImageOpenAfter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkImageOpenAfter.AutoSize = true;
            this.chkImageOpenAfter.Location = new System.Drawing.Point(75, 133);
            this.chkImageOpenAfter.Name = "chkImageOpenAfter";
            this.chkImageOpenAfter.Size = new System.Drawing.Size(154, 17);
            this.chkImageOpenAfter.TabIndex = 10;
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
            this.chkImageSeparateNonImages.Location = new System.Drawing.Point(143, 87);
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
            this.chkImageSeparateArtists.Location = new System.Drawing.Point(15, 110);
            this.chkImageSeparateArtists.Name = "chkImageSeparateArtists";
            this.chkImageSeparateArtists.Size = new System.Drawing.Size(104, 17);
            this.chkImageSeparateArtists.TabIndex = 4;
            this.chkImageSeparateArtists.Text = "Separate artists";
            this.chkImageSeparateArtists.UseVisualStyleBackColor = true;
            // 
            // chkImageUseForm
            // 
            this.chkImageUseForm.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkImageUseForm.AutoSize = true;
            this.chkImageUseForm.Location = new System.Drawing.Point(125, 110);
            this.chkImageUseForm.Name = "chkImageUseForm";
            this.chkImageUseForm.Size = new System.Drawing.Size(164, 17);
            this.chkImageUseForm.TabIndex = 6;
            this.chkImageUseForm.Text = "Use form to show progress";
            this.chkImageUseForm.UseVisualStyleBackColor = true;
            // 
            // chkImageSeparateGraylisted
            // 
            this.chkImageSeparateGraylisted.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkImageSeparateGraylisted.AutoSize = true;
            this.chkImageSeparateGraylisted.Checked = true;
            this.chkImageSeparateGraylisted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImageSeparateGraylisted.Location = new System.Drawing.Point(24, 64);
            this.chkImageSeparateGraylisted.Name = "chkImageSeparateGraylisted";
            this.chkImageSeparateGraylisted.Size = new System.Drawing.Size(123, 17);
            this.chkImageSeparateGraylisted.TabIndex = 3;
            this.chkImageSeparateGraylisted.Text = "Separate graylisted";
            this.chkImageSeparateGraylisted.UseVisualStyleBackColor = true;
            // 
            // chkImageSeparateRatings
            // 
            this.chkImageSeparateRatings.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkImageSeparateRatings.AutoSize = true;
            this.chkImageSeparateRatings.Checked = true;
            this.chkImageSeparateRatings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImageSeparateRatings.Location = new System.Drawing.Point(28, 87);
            this.chkImageSeparateRatings.Name = "chkImageSeparateRatings";
            this.chkImageSeparateRatings.Size = new System.Drawing.Size(109, 17);
            this.chkImageSeparateRatings.TabIndex = 2;
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
            this.btnDownloadImage.Location = new System.Drawing.Point(112, 168);
            this.btnDownloadImage.Name = "btnDownloadImage";
            this.btnDownloadImage.Size = new System.Drawing.Size(81, 23);
            this.btnDownloadImage.TabIndex = 7;
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
            this.Menu = this.toolMenu;
            this.MinimumSize = new System.Drawing.Size(290, 274);
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
            this.gbtPageLimit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numTagsPageLimit)).EndInit();
            this.gbtRatings.ResumeLayout(false);
            this.gbtRatings.PerformLayout();
            this.gbtImageLimit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numTagsImageLimit)).EndInit();
            this.gbtScore.ResumeLayout(false);
            this.gbtScore.PerformLayout();
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
        private System.Windows.Forms.CheckBox chkTagsDownloadSafe;
        private System.Windows.Forms.CheckBox chkTagsDownloadQuestionable;
        private System.Windows.Forms.CheckBox chkTagsDownloadExplicit;
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
        private System.Windows.Forms.GroupBox gbtScore;
        private System.Windows.Forms.GroupBox gbtRatings;
        private System.Windows.Forms.GroupBox gbtImageLimit;
        private System.Windows.Forms.GroupBox gbtPageLimit;
        private System.Windows.Forms.NumericUpDown numTagsPageLimit;
        private System.Windows.Forms.Label lbAwoo;
        private System.Windows.Forms.CheckBox chkTagsUseScoreAsTag;
        private System.Windows.Forms.ToolTip TouchingTips;
        private System.Windows.Forms.CheckBox chkTagsSeparateRatings;
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
    }
}

