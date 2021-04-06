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
            this.tbMain = new System.Windows.Forms.TabControl();
            this.tbTags = new System.Windows.Forms.TabPage();
            this.gbtPageLimit = new System.Windows.Forms.GroupBox();
            this.numTagsPageLimit = new System.Windows.Forms.NumericUpDown();
            this.gbtRatings = new System.Windows.Forms.GroupBox();
            this.chkTagsSeparateRatings = new System.Windows.Forms.CheckBox();
            this.chkTagsDownloadExplicit = new System.Windows.Forms.CheckBox();
            this.chkTagsDownloadQuestionable = new System.Windows.Forms.CheckBox();
            this.chkTagsDownloadSafe = new System.Windows.Forms.CheckBox();
            this.gbtImageLimit = new System.Windows.Forms.GroupBox();
            this.numTagsImageLimit = new System.Windows.Forms.NumericUpDown();
            this.btnDownloadTags = new System.Windows.Forms.Button();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbtScore = new System.Windows.Forms.GroupBox();
            this.chkTagsUseScoreAsTag = new System.Windows.Forms.CheckBox();
            this.chkTagsUseMinimumScore = new System.Windows.Forms.CheckBox();
            this.numTagsMinimumScore = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPools = new System.Windows.Forms.TabPage();
            this.chkPoolsMergeBlacklisted = new System.Windows.Forms.CheckBox();
            this.chkPoolsOpenAfter = new System.Windows.Forms.CheckBox();
            this.btnDownloadPool = new System.Windows.Forms.Button();
            this.txtPoolId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbImages = new System.Windows.Forms.TabPage();
            this.chkImageSeparateArtists = new System.Windows.Forms.CheckBox();
            this.chkImageUseForm = new System.Windows.Forms.CheckBox();
            this.chkImageSeparateBlacklisted = new System.Windows.Forms.CheckBox();
            this.chkImageSeparateRatings = new System.Windows.Forms.CheckBox();
            this.btnDownloadImage = new System.Windows.Forms.Button();
            this.txtImageUrl = new System.Windows.Forms.TextBox();
            this.lbImageUrlID = new System.Windows.Forms.Label();
            this.tbIni = new System.Windows.Forms.TabPage();
            this.lbIni = new System.Windows.Forms.Label();
            this.toolMenu = new System.Windows.Forms.MainMenu(this.components);
            this.mSettings = new System.Windows.Forms.MenuItem();
            this.mBlacklist = new System.Windows.Forms.MenuItem();
            this.mTools = new System.Windows.Forms.MenuItem();
            this.mWishlist = new System.Windows.Forms.MenuItem();
            this.mRedownloader = new System.Windows.Forms.MenuItem();
            this.mReverseSearch = new System.Windows.Forms.MenuItem();
            this.mSep = new System.Windows.Forms.MenuItem();
            this.mParser = new System.Windows.Forms.MenuItem();
            this.mAbout = new System.Windows.Forms.MenuItem();
            this.mProtocol = new System.Windows.Forms.MenuItem();
            this.TouchingTips = new System.Windows.Forms.ToolTip(this.components);
            this.tbMain.SuspendLayout();
            this.tbTags.SuspendLayout();
            this.gbtPageLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsPageLimit)).BeginInit();
            this.gbtRatings.SuspendLayout();
            this.gbtImageLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsImageLimit)).BeginInit();
            this.gbtScore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsMinimumScore)).BeginInit();
            this.tbPools.SuspendLayout();
            this.tbImages.SuspendLayout();
            this.tbIni.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbMain
            // 
            this.tbMain.Controls.Add(this.tbTags);
            this.tbMain.Controls.Add(this.tbPools);
            this.tbMain.Controls.Add(this.tbImages);
            this.tbMain.Controls.Add(this.tbIni);
            this.tbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMain.Location = new System.Drawing.Point(0, 0);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(272, 196);
            this.tbMain.TabIndex = 7;
            this.tbMain.SelectedIndexChanged += new System.EventHandler(this.tbMain_SelectedIndexChanged);
            // 
            // tbTags
            // 
            this.tbTags.Controls.Add(this.gbtPageLimit);
            this.tbTags.Controls.Add(this.gbtRatings);
            this.tbTags.Controls.Add(this.gbtImageLimit);
            this.tbTags.Controls.Add(this.btnDownloadTags);
            this.tbTags.Controls.Add(this.txtTags);
            this.tbTags.Controls.Add(this.label1);
            this.tbTags.Controls.Add(this.gbtScore);
            this.tbTags.Controls.Add(this.label2);
            this.tbTags.Location = new System.Drawing.Point(4, 22);
            this.tbTags.Name = "tbTags";
            this.tbTags.Padding = new System.Windows.Forms.Padding(3);
            this.tbTags.Size = new System.Drawing.Size(264, 170);
            this.tbTags.TabIndex = 0;
            this.tbTags.Text = "Tag(s)";
            this.tbTags.UseVisualStyleBackColor = true;
            // 
            // gbtPageLimit
            // 
            this.gbtPageLimit.Controls.Add(this.numTagsPageLimit);
            this.gbtPageLimit.Location = new System.Drawing.Point(8, 47);
            this.gbtPageLimit.Name = "gbtPageLimit";
            this.gbtPageLimit.Size = new System.Drawing.Size(128, 46);
            this.gbtPageLimit.TabIndex = 2;
            this.gbtPageLimit.TabStop = false;
            this.gbtPageLimit.Text = "Page limit (0 = off)";
            // 
            // numTagsPageLimit
            // 
            this.numTagsPageLimit.Location = new System.Drawing.Point(33, 19);
            this.numTagsPageLimit.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numTagsPageLimit.Name = "numTagsPageLimit";
            this.numTagsPageLimit.Size = new System.Drawing.Size(63, 20);
            this.numTagsPageLimit.TabIndex = 1;
            this.numTagsPageLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TouchingTips.SetToolTip(this.numTagsPageLimit, "The amount of pages that will be downloaded (320 images per page)");
            // 
            // gbtRatings
            // 
            this.gbtRatings.Controls.Add(this.chkTagsSeparateRatings);
            this.gbtRatings.Controls.Add(this.chkTagsDownloadExplicit);
            this.gbtRatings.Controls.Add(this.chkTagsDownloadQuestionable);
            this.gbtRatings.Controls.Add(this.chkTagsDownloadSafe);
            this.gbtRatings.Location = new System.Drawing.Point(140, 113);
            this.gbtRatings.Name = "gbtRatings";
            this.gbtRatings.Size = new System.Drawing.Size(128, 51);
            this.gbtRatings.TabIndex = 5;
            this.gbtRatings.TabStop = false;
            this.gbtRatings.Text = "Ratings to download";
            // 
            // chkTagsSeparateRatings
            // 
            this.chkTagsSeparateRatings.AutoSize = true;
            this.chkTagsSeparateRatings.Checked = true;
            this.chkTagsSeparateRatings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagsSeparateRatings.Location = new System.Drawing.Point(12, 31);
            this.chkTagsSeparateRatings.Name = "chkTagsSeparateRatings";
            this.chkTagsSeparateRatings.Size = new System.Drawing.Size(102, 17);
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
            this.chkTagsDownloadExplicit.Location = new System.Drawing.Point(11, 14);
            this.chkTagsDownloadExplicit.Name = "chkTagsDownloadExplicit";
            this.chkTagsDownloadExplicit.Size = new System.Drawing.Size(32, 17);
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
            this.chkTagsDownloadQuestionable.Location = new System.Drawing.Point(49, 14);
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
            this.chkTagsDownloadSafe.Location = new System.Drawing.Point(88, 14);
            this.chkTagsDownloadSafe.Name = "chkTagsDownloadSafe";
            this.chkTagsDownloadSafe.Size = new System.Drawing.Size(32, 17);
            this.chkTagsDownloadSafe.TabIndex = 3;
            this.chkTagsDownloadSafe.Text = "S";
            this.TouchingTips.SetToolTip(this.chkTagsDownloadSafe, "Download images rated Safe");
            this.chkTagsDownloadSafe.UseVisualStyleBackColor = true;
            // 
            // gbtImageLimit
            // 
            this.gbtImageLimit.Controls.Add(this.numTagsImageLimit);
            this.gbtImageLimit.Location = new System.Drawing.Point(8, 113);
            this.gbtImageLimit.Name = "gbtImageLimit";
            this.gbtImageLimit.Size = new System.Drawing.Size(130, 51);
            this.gbtImageLimit.TabIndex = 4;
            this.gbtImageLimit.TabStop = false;
            this.gbtImageLimit.Text = "Image limit (0 = off)";
            // 
            // numTagsImageLimit
            // 
            this.numTagsImageLimit.Location = new System.Drawing.Point(25, 21);
            this.numTagsImageLimit.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numTagsImageLimit.Name = "numTagsImageLimit";
            this.numTagsImageLimit.Size = new System.Drawing.Size(81, 20);
            this.numTagsImageLimit.TabIndex = 1;
            this.numTagsImageLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TouchingTips.SetToolTip(this.numTagsImageLimit, "Limits downloads to a certain amount of images");
            // 
            // btnDownloadTags
            // 
            this.btnDownloadTags.Location = new System.Drawing.Point(97, 170);
            this.btnDownloadTags.Name = "btnDownloadTags";
            this.btnDownloadTags.Size = new System.Drawing.Size(81, 23);
            this.btnDownloadTags.TabIndex = 6;
            this.btnDownloadTags.Text = "Download";
            this.btnDownloadTags.UseVisualStyleBackColor = true;
            this.btnDownloadTags.Click += new System.EventHandler(this.btnDownloadTags_Click);
            // 
            // txtTags
            // 
            this.txtTags.Location = new System.Drawing.Point(38, 24);
            this.txtTags.Name = "txtTags";
            this.txtTags.Size = new System.Drawing.Size(190, 20);
            this.txtTags.TabIndex = 1;
            this.txtTags.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TouchingTips.SetToolTip(this.txtTags, "The tags that will be downloaded");
            this.txtTags.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTags_KeyDown);
            this.txtTags.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTags_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Tags (limit 6):";
            // 
            // gbtScore
            // 
            this.gbtScore.Controls.Add(this.chkTagsUseScoreAsTag);
            this.gbtScore.Controls.Add(this.chkTagsUseMinimumScore);
            this.gbtScore.Controls.Add(this.numTagsMinimumScore);
            this.gbtScore.Location = new System.Drawing.Point(141, 47);
            this.gbtScore.Name = "gbtScore";
            this.gbtScore.Size = new System.Drawing.Size(128, 65);
            this.gbtScore.TabIndex = 3;
            this.gbtScore.TabStop = false;
            this.gbtScore.Text = "Score minimum";
            // 
            // chkTagsUseScoreAsTag
            // 
            this.chkTagsUseScoreAsTag.AutoSize = true;
            this.chkTagsUseScoreAsTag.Location = new System.Drawing.Point(70, 18);
            this.chkTagsUseScoreAsTag.Name = "chkTagsUseScoreAsTag";
            this.chkTagsUseScoreAsTag.Size = new System.Drawing.Size(55, 17);
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
            this.chkTagsUseMinimumScore.Location = new System.Drawing.Point(8, 18);
            this.chkTagsUseMinimumScore.Name = "chkTagsUseMinimumScore";
            this.chkTagsUseMinimumScore.Size = new System.Drawing.Size(58, 17);
            this.chkTagsUseMinimumScore.TabIndex = 1;
            this.chkTagsUseMinimumScore.Text = "Enable";
            this.TouchingTips.SetToolTip(this.chkTagsUseMinimumScore, "Only downloads images with a score equal to or greater than provided");
            this.chkTagsUseMinimumScore.UseVisualStyleBackColor = true;
            this.chkTagsUseMinimumScore.CheckedChanged += new System.EventHandler(this.chkTagsUseMinimumScore_CheckedChanged);
            // 
            // numTagsMinimumScore
            // 
            this.numTagsMinimumScore.Enabled = false;
            this.numTagsMinimumScore.Location = new System.Drawing.Point(33, 39);
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
            this.numTagsMinimumScore.Size = new System.Drawing.Size(63, 20);
            this.numTagsMinimumScore.TabIndex = 3;
            this.numTagsMinimumScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TouchingTips.SetToolTip(this.numTagsMinimumScore, "The minimum score required");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(113, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 26;
            this.label2.Text = "awooooo";
            // 
            // tbPools
            // 
            this.tbPools.Controls.Add(this.chkPoolsMergeBlacklisted);
            this.tbPools.Controls.Add(this.chkPoolsOpenAfter);
            this.tbPools.Controls.Add(this.btnDownloadPool);
            this.tbPools.Controls.Add(this.txtPoolId);
            this.tbPools.Controls.Add(this.label3);
            this.tbPools.Location = new System.Drawing.Point(4, 22);
            this.tbPools.Name = "tbPools";
            this.tbPools.Padding = new System.Windows.Forms.Padding(3);
            this.tbPools.Size = new System.Drawing.Size(264, 170);
            this.tbPools.TabIndex = 1;
            this.tbPools.Text = "Pool";
            this.tbPools.UseVisualStyleBackColor = true;
            // 
            // chkPoolsMergeBlacklisted
            // 
            this.chkPoolsMergeBlacklisted.AutoSize = true;
            this.chkPoolsMergeBlacklisted.Location = new System.Drawing.Point(53, 107);
            this.chkPoolsMergeBlacklisted.Name = "chkPoolsMergeBlacklisted";
            this.chkPoolsMergeBlacklisted.Size = new System.Drawing.Size(168, 17);
            this.chkPoolsMergeBlacklisted.TabIndex = 4;
            this.chkPoolsMergeBlacklisted.Text = "Merge blacklisted with the rest";
            this.chkPoolsMergeBlacklisted.UseVisualStyleBackColor = true;
            // 
            // chkPoolsOpenAfter
            // 
            this.chkPoolsOpenAfter.AutoSize = true;
            this.chkPoolsOpenAfter.Location = new System.Drawing.Point(53, 84);
            this.chkPoolsOpenAfter.Name = "chkPoolsOpenAfter";
            this.chkPoolsOpenAfter.Size = new System.Drawing.Size(138, 17);
            this.chkPoolsOpenAfter.TabIndex = 3;
            this.chkPoolsOpenAfter.Text = "Open after downloading";
            this.chkPoolsOpenAfter.UseVisualStyleBackColor = true;
            // 
            // btnDownloadPool
            // 
            this.btnDownloadPool.Location = new System.Drawing.Point(98, 145);
            this.btnDownloadPool.Name = "btnDownloadPool";
            this.btnDownloadPool.Size = new System.Drawing.Size(79, 23);
            this.btnDownloadPool.TabIndex = 5;
            this.btnDownloadPool.Text = "Download";
            this.btnDownloadPool.UseVisualStyleBackColor = true;
            this.btnDownloadPool.Click += new System.EventHandler(this.btnDownloadPool_Click);
            // 
            // txtPoolId
            // 
            this.txtPoolId.Location = new System.Drawing.Point(98, 36);
            this.txtPoolId.Name = "txtPoolId";
            this.txtPoolId.Size = new System.Drawing.Size(79, 20);
            this.txtPoolId.TabIndex = 1;
            this.txtPoolId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPoolId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPoolId_KeyDown);
            this.txtPoolId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPoolId_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(73, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Pool ID:";
            // 
            // tbImages
            // 
            this.tbImages.Controls.Add(this.chkImageSeparateArtists);
            this.tbImages.Controls.Add(this.chkImageUseForm);
            this.tbImages.Controls.Add(this.chkImageSeparateBlacklisted);
            this.tbImages.Controls.Add(this.chkImageSeparateRatings);
            this.tbImages.Controls.Add(this.btnDownloadImage);
            this.tbImages.Controls.Add(this.txtImageUrl);
            this.tbImages.Controls.Add(this.lbImageUrlID);
            this.tbImages.Location = new System.Drawing.Point(4, 22);
            this.tbImages.Name = "tbImages";
            this.tbImages.Padding = new System.Windows.Forms.Padding(3);
            this.tbImages.Size = new System.Drawing.Size(264, 170);
            this.tbImages.TabIndex = 3;
            this.tbImages.Text = "Image";
            this.tbImages.UseVisualStyleBackColor = true;
            // 
            // chkImageSeparateArtists
            // 
            this.chkImageSeparateArtists.AutoSize = true;
            this.chkImageSeparateArtists.Location = new System.Drawing.Point(84, 114);
            this.chkImageSeparateArtists.Name = "chkImageSeparateArtists";
            this.chkImageSeparateArtists.Size = new System.Drawing.Size(98, 17);
            this.chkImageSeparateArtists.TabIndex = 16;
            this.chkImageSeparateArtists.Text = "Separate artists";
            this.chkImageSeparateArtists.UseVisualStyleBackColor = true;
            // 
            // chkImageUseForm
            // 
            this.chkImageUseForm.AutoSize = true;
            this.chkImageUseForm.Location = new System.Drawing.Point(61, 137);
            this.chkImageUseForm.Name = "chkImageUseForm";
            this.chkImageUseForm.Size = new System.Drawing.Size(150, 17);
            this.chkImageUseForm.TabIndex = 5;
            this.chkImageUseForm.Text = "Use form to show progress";
            this.chkImageUseForm.UseVisualStyleBackColor = true;
            // 
            // chkImageSeparateBlacklisted
            // 
            this.chkImageSeparateBlacklisted.AutoSize = true;
            this.chkImageSeparateBlacklisted.Checked = true;
            this.chkImageSeparateBlacklisted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImageSeparateBlacklisted.Location = new System.Drawing.Point(77, 91);
            this.chkImageSeparateBlacklisted.Name = "chkImageSeparateBlacklisted";
            this.chkImageSeparateBlacklisted.Size = new System.Drawing.Size(121, 17);
            this.chkImageSeparateBlacklisted.TabIndex = 4;
            this.chkImageSeparateBlacklisted.Text = "Separate blacklisted";
            this.chkImageSeparateBlacklisted.UseVisualStyleBackColor = true;
            // 
            // chkImageSeparateRatings
            // 
            this.chkImageSeparateRatings.AutoSize = true;
            this.chkImageSeparateRatings.Checked = true;
            this.chkImageSeparateRatings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImageSeparateRatings.Location = new System.Drawing.Point(86, 68);
            this.chkImageSeparateRatings.Name = "chkImageSeparateRatings";
            this.chkImageSeparateRatings.Size = new System.Drawing.Size(102, 17);
            this.chkImageSeparateRatings.TabIndex = 3;
            this.chkImageSeparateRatings.Text = "Separate ratings";
            this.chkImageSeparateRatings.UseVisualStyleBackColor = true;
            // 
            // btnDownloadImage
            // 
            this.btnDownloadImage.Location = new System.Drawing.Point(97, 170);
            this.btnDownloadImage.Name = "btnDownloadImage";
            this.btnDownloadImage.Size = new System.Drawing.Size(81, 23);
            this.btnDownloadImage.TabIndex = 6;
            this.btnDownloadImage.Text = "Download";
            this.btnDownloadImage.UseVisualStyleBackColor = true;
            this.btnDownloadImage.Click += new System.EventHandler(this.btnDownloadImage_Click);
            // 
            // txtImageUrl
            // 
            this.txtImageUrl.Location = new System.Drawing.Point(38, 24);
            this.txtImageUrl.Name = "txtImageUrl";
            this.txtImageUrl.Size = new System.Drawing.Size(190, 20);
            this.txtImageUrl.TabIndex = 1;
            this.txtImageUrl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TouchingTips.SetToolTip(this.txtImageUrl, "The image that will be downloaded");
            this.txtImageUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtImageUrl_KeyDown);
            // 
            // lbImageUrlID
            // 
            this.lbImageUrlID.AutoSize = true;
            this.lbImageUrlID.Location = new System.Drawing.Point(13, 6);
            this.lbImageUrlID.Name = "lbImageUrlID";
            this.lbImageUrlID.Size = new System.Drawing.Size(80, 13);
            this.lbImageUrlID.TabIndex = 15;
            this.lbImageUrlID.Text = "Image URL/ID:";
            // 
            // tbIni
            // 
            this.tbIni.Controls.Add(this.lbIni);
            this.tbIni.Location = new System.Drawing.Point(4, 22);
            this.tbIni.Name = "tbIni";
            this.tbIni.Padding = new System.Windows.Forms.Padding(3);
            this.tbIni.Size = new System.Drawing.Size(264, 170);
            this.tbIni.TabIndex = 2;
            this.tbIni.Text = "// Portable Mode \\\\";
            this.tbIni.UseVisualStyleBackColor = true;
            // 
            // lbIni
            // 
            this.lbIni.AutoSize = true;
            this.lbIni.Location = new System.Drawing.Point(14, 56);
            this.lbIni.Name = "lbIni";
            this.lbIni.Size = new System.Drawing.Size(247, 65);
            this.lbIni.TabIndex = 0;
            this.lbIni.Text = "aphrodite.ini will be used to store and read settings.\r\n\r\nChange aphrodite.ini to" +
    " use system\'s settings.\r\n\r\nor delete it, your call.";
            this.lbIni.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbIni.Visible = false;
            // 
            // toolMenu
            // 
            this.toolMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mSettings,
            this.mBlacklist,
            this.mTools,
            this.mAbout,
            this.mProtocol});
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
            this.mSep,
            this.mParser});
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
            // mSep
            // 
            this.mSep.Index = 3;
            this.mSep.Text = "-";
            // 
            // mParser
            // 
            this.mParser.Index = 4;
            this.mParser.Text = "nfo parser";
            this.mParser.Click += new System.EventHandler(this.mParser_Click);
            // 
            // mAbout
            // 
            this.mAbout.Index = 3;
            this.mAbout.Text = "about";
            this.mAbout.Click += new System.EventHandler(this.mAbout_Click);
            // 
            // mProtocol
            // 
            this.mProtocol.Enabled = false;
            this.mProtocol.Index = 4;
            this.mProtocol.Text = "protocol";
            this.mProtocol.Visible = false;
            this.mProtocol.Click += new System.EventHandler(this.mProtocol_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(272, 196);
            this.Controls.Add(this.tbMain);
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(290, 274);
            this.Menu = this.toolMenu;
            this.MinimumSize = new System.Drawing.Size(290, 274);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "aphrodite";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.tbMain.ResumeLayout(false);
            this.tbTags.ResumeLayout(false);
            this.tbTags.PerformLayout();
            this.gbtPageLimit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numTagsPageLimit)).EndInit();
            this.gbtRatings.ResumeLayout(false);
            this.gbtRatings.PerformLayout();
            this.gbtImageLimit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numTagsImageLimit)).EndInit();
            this.gbtScore.ResumeLayout(false);
            this.gbtScore.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsMinimumScore)).EndInit();
            this.tbPools.ResumeLayout(false);
            this.tbPools.PerformLayout();
            this.tbImages.ResumeLayout(false);
            this.tbImages.PerformLayout();
            this.tbIni.ResumeLayout(false);
            this.tbIni.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tbTags;
        private System.Windows.Forms.TabPage tbPools;
        private System.Windows.Forms.MainMenu toolMenu;
        private System.Windows.Forms.MenuItem mSettings;
        private System.Windows.Forms.MenuItem mAbout;
        private System.Windows.Forms.MenuItem mBlacklist;
        private System.Windows.Forms.Button btnDownloadTags;
        private System.Windows.Forms.CheckBox chkTagsDownloadSafe;
        private System.Windows.Forms.CheckBox chkTagsDownloadQuestionable;
        private System.Windows.Forms.CheckBox chkTagsDownloadExplicit;
        private System.Windows.Forms.NumericUpDown numTagsImageLimit;
        private System.Windows.Forms.CheckBox chkTagsUseMinimumScore;
        private System.Windows.Forms.NumericUpDown numTagsMinimumScore;
        private System.Windows.Forms.TextBox txtTags;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkPoolsMergeBlacklisted;
        private System.Windows.Forms.CheckBox chkPoolsOpenAfter;
        private System.Windows.Forms.Button btnDownloadPool;
        private System.Windows.Forms.TextBox txtPoolId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MenuItem mProtocol;
        private System.Windows.Forms.MenuItem mTools;
        private System.Windows.Forms.MenuItem mWishlist;
        private System.Windows.Forms.MenuItem mRedownloader;
        private System.Windows.Forms.MenuItem mReverseSearch;
        private System.Windows.Forms.MenuItem mSep;
        private System.Windows.Forms.GroupBox gbtScore;
        private System.Windows.Forms.GroupBox gbtRatings;
        private System.Windows.Forms.GroupBox gbtImageLimit;
        private System.Windows.Forms.GroupBox gbtPageLimit;
        private System.Windows.Forms.NumericUpDown numTagsPageLimit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkTagsUseScoreAsTag;
        private System.Windows.Forms.ToolTip TouchingTips;
        private System.Windows.Forms.CheckBox chkTagsSeparateRatings;
        private System.Windows.Forms.TabPage tbIni;
        private System.Windows.Forms.Label lbIni;
        private System.Windows.Forms.TabPage tbImages;
        private System.Windows.Forms.TextBox txtImageUrl;
        private System.Windows.Forms.Label lbImageUrlID;
        private System.Windows.Forms.Button btnDownloadImage;
        private System.Windows.Forms.CheckBox chkImageUseForm;
        private System.Windows.Forms.CheckBox chkImageSeparateBlacklisted;
        private System.Windows.Forms.CheckBox chkImageSeparateRatings;
        private System.Windows.Forms.MenuItem mParser;
        private System.Windows.Forms.CheckBox chkImageSeparateArtists;
    }
}

