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
            this.numPageLimit = new System.Windows.Forms.NumericUpDown();
            this.gbtRatings = new System.Windows.Forms.GroupBox();
            this.chkSeparateRatings = new System.Windows.Forms.CheckBox();
            this.chkExplicit = new System.Windows.Forms.CheckBox();
            this.chkQuestionable = new System.Windows.Forms.CheckBox();
            this.chkSafe = new System.Windows.Forms.CheckBox();
            this.gbtImageLimit = new System.Windows.Forms.GroupBox();
            this.numLimit = new System.Windows.Forms.NumericUpDown();
            this.btnDownloadTags = new System.Windows.Forms.Button();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbtScore = new System.Windows.Forms.GroupBox();
            this.chkScoreAsTag = new System.Windows.Forms.CheckBox();
            this.chkMinimumScore = new System.Windows.Forms.CheckBox();
            this.numScore = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPools = new System.Windows.Forms.TabPage();
            this.chkMerge = new System.Windows.Forms.CheckBox();
            this.chkOpen = new System.Windows.Forms.CheckBox();
            this.btnDownloadPool = new System.Windows.Forms.Button();
            this.txtID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbImages = new System.Windows.Forms.TabPage();
            this.chkImageSeparateArtists = new System.Windows.Forms.CheckBox();
            this.chkImageUseForm = new System.Windows.Forms.CheckBox();
            this.chkImageSeparateBlacklisted = new System.Windows.Forms.CheckBox();
            this.chkImageSeparateRatings = new System.Windows.Forms.CheckBox();
            this.btnDownloadImage = new System.Windows.Forms.Button();
            this.txtImageUrl = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.numPageLimit)).BeginInit();
            this.gbtRatings.SuspendLayout();
            this.gbtImageLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLimit)).BeginInit();
            this.gbtScore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScore)).BeginInit();
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
            this.tbMain.Size = new System.Drawing.Size(274, 194);
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
            this.tbTags.Size = new System.Drawing.Size(266, 168);
            this.tbTags.TabIndex = 0;
            this.tbTags.Text = "Tag(s)";
            this.tbTags.UseVisualStyleBackColor = true;
            // 
            // gbtPageLimit
            // 
            this.gbtPageLimit.Controls.Add(this.numPageLimit);
            this.gbtPageLimit.Location = new System.Drawing.Point(8, 47);
            this.gbtPageLimit.Name = "gbtPageLimit";
            this.gbtPageLimit.Size = new System.Drawing.Size(128, 46);
            this.gbtPageLimit.TabIndex = 2;
            this.gbtPageLimit.TabStop = false;
            this.gbtPageLimit.Text = "Page limit (0 = off)";
            // 
            // numPageLimit
            // 
            this.numPageLimit.Location = new System.Drawing.Point(33, 19);
            this.numPageLimit.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numPageLimit.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.numPageLimit.Name = "numPageLimit";
            this.numPageLimit.Size = new System.Drawing.Size(63, 20);
            this.numPageLimit.TabIndex = 1;
            this.numPageLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TouchingTips.SetToolTip(this.numPageLimit, "The amount of pages that will be downloaded (320 images per page)");
            // 
            // gbtRatings
            // 
            this.gbtRatings.Controls.Add(this.chkSeparateRatings);
            this.gbtRatings.Controls.Add(this.chkExplicit);
            this.gbtRatings.Controls.Add(this.chkQuestionable);
            this.gbtRatings.Controls.Add(this.chkSafe);
            this.gbtRatings.Location = new System.Drawing.Point(140, 113);
            this.gbtRatings.Name = "gbtRatings";
            this.gbtRatings.Size = new System.Drawing.Size(128, 51);
            this.gbtRatings.TabIndex = 5;
            this.gbtRatings.TabStop = false;
            this.gbtRatings.Text = "Ratings to download";
            // 
            // chkSeparateRatings
            // 
            this.chkSeparateRatings.AutoSize = true;
            this.chkSeparateRatings.Checked = true;
            this.chkSeparateRatings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSeparateRatings.Location = new System.Drawing.Point(12, 31);
            this.chkSeparateRatings.Name = "chkSeparateRatings";
            this.chkSeparateRatings.Size = new System.Drawing.Size(103, 17);
            this.chkSeparateRatings.TabIndex = 4;
            this.chkSeparateRatings.Text = "Separate ratings";
            this.TouchingTips.SetToolTip(this.chkSeparateRatings, "Separates ratings into different folders");
            this.chkSeparateRatings.UseVisualStyleBackColor = true;
            // 
            // chkExplicit
            // 
            this.chkExplicit.AutoSize = true;
            this.chkExplicit.Checked = true;
            this.chkExplicit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExplicit.Location = new System.Drawing.Point(11, 14);
            this.chkExplicit.Name = "chkExplicit";
            this.chkExplicit.Size = new System.Drawing.Size(33, 17);
            this.chkExplicit.TabIndex = 1;
            this.chkExplicit.Text = "E";
            this.TouchingTips.SetToolTip(this.chkExplicit, "Download images rated Explicit");
            this.chkExplicit.UseVisualStyleBackColor = true;
            // 
            // chkQuestionable
            // 
            this.chkQuestionable.AutoSize = true;
            this.chkQuestionable.Checked = true;
            this.chkQuestionable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkQuestionable.Location = new System.Drawing.Point(49, 14);
            this.chkQuestionable.Name = "chkQuestionable";
            this.chkQuestionable.Size = new System.Drawing.Size(34, 17);
            this.chkQuestionable.TabIndex = 2;
            this.chkQuestionable.Text = "Q";
            this.TouchingTips.SetToolTip(this.chkQuestionable, "Download images rated Questionable");
            this.chkQuestionable.UseVisualStyleBackColor = true;
            // 
            // chkSafe
            // 
            this.chkSafe.AutoSize = true;
            this.chkSafe.Checked = true;
            this.chkSafe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSafe.Location = new System.Drawing.Point(88, 14);
            this.chkSafe.Name = "chkSafe";
            this.chkSafe.Size = new System.Drawing.Size(33, 17);
            this.chkSafe.TabIndex = 3;
            this.chkSafe.Text = "S";
            this.TouchingTips.SetToolTip(this.chkSafe, "Download images rated Safe");
            this.chkSafe.UseVisualStyleBackColor = true;
            // 
            // gbtImageLimit
            // 
            this.gbtImageLimit.Controls.Add(this.numLimit);
            this.gbtImageLimit.Location = new System.Drawing.Point(8, 113);
            this.gbtImageLimit.Name = "gbtImageLimit";
            this.gbtImageLimit.Size = new System.Drawing.Size(130, 51);
            this.gbtImageLimit.TabIndex = 4;
            this.gbtImageLimit.TabStop = false;
            this.gbtImageLimit.Text = "Image limit (0 = off)";
            // 
            // numLimit
            // 
            this.numLimit.Location = new System.Drawing.Point(25, 21);
            this.numLimit.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numLimit.Name = "numLimit";
            this.numLimit.Size = new System.Drawing.Size(81, 20);
            this.numLimit.TabIndex = 1;
            this.numLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TouchingTips.SetToolTip(this.numLimit, "Limits downloads to a certain amount of images");
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
            this.txtTags.Size = new System.Drawing.Size(223, 20);
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
            this.gbtScore.Controls.Add(this.chkScoreAsTag);
            this.gbtScore.Controls.Add(this.chkMinimumScore);
            this.gbtScore.Controls.Add(this.numScore);
            this.gbtScore.Location = new System.Drawing.Point(141, 47);
            this.gbtScore.Name = "gbtScore";
            this.gbtScore.Size = new System.Drawing.Size(128, 65);
            this.gbtScore.TabIndex = 3;
            this.gbtScore.TabStop = false;
            this.gbtScore.Text = "Score minimum";
            // 
            // chkScoreAsTag
            // 
            this.chkScoreAsTag.AutoSize = true;
            this.chkScoreAsTag.Location = new System.Drawing.Point(70, 18);
            this.chkScoreAsTag.Name = "chkScoreAsTag";
            this.chkScoreAsTag.Size = new System.Drawing.Size(56, 17);
            this.chkScoreAsTag.TabIndex = 2;
            this.chkScoreAsTag.Text = "As tag";
            this.TouchingTips.SetToolTip(this.chkScoreAsTag, "If checked, uses the score minimum as a tag (ex: \"gay score:>25\").\r\nThis widly in" +
        "creases the images that will be downloaded.\r\nOnly used if there are 5 or less ta" +
        "gs being queried");
            this.chkScoreAsTag.UseVisualStyleBackColor = true;
            // 
            // chkMinimumScore
            // 
            this.chkMinimumScore.AutoSize = true;
            this.chkMinimumScore.Location = new System.Drawing.Point(8, 18);
            this.chkMinimumScore.Name = "chkMinimumScore";
            this.chkMinimumScore.Size = new System.Drawing.Size(59, 17);
            this.chkMinimumScore.TabIndex = 1;
            this.chkMinimumScore.Text = "Enable";
            this.TouchingTips.SetToolTip(this.chkMinimumScore, "Only downloads images with a score equal to or greater than provided");
            this.chkMinimumScore.UseVisualStyleBackColor = true;
            this.chkMinimumScore.CheckedChanged += new System.EventHandler(this.chkMinimumScore_CheckedChanged);
            // 
            // numScore
            // 
            this.numScore.Enabled = false;
            this.numScore.Location = new System.Drawing.Point(33, 39);
            this.numScore.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numScore.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.numScore.Name = "numScore";
            this.numScore.Size = new System.Drawing.Size(63, 20);
            this.numScore.TabIndex = 3;
            this.numScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TouchingTips.SetToolTip(this.numScore, "The minimum score required");
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
            this.tbPools.Controls.Add(this.chkMerge);
            this.tbPools.Controls.Add(this.chkOpen);
            this.tbPools.Controls.Add(this.btnDownloadPool);
            this.tbPools.Controls.Add(this.txtID);
            this.tbPools.Controls.Add(this.label3);
            this.tbPools.Location = new System.Drawing.Point(4, 22);
            this.tbPools.Name = "tbPools";
            this.tbPools.Padding = new System.Windows.Forms.Padding(3);
            this.tbPools.Size = new System.Drawing.Size(266, 168);
            this.tbPools.TabIndex = 1;
            this.tbPools.Text = "Pool";
            this.tbPools.UseVisualStyleBackColor = true;
            // 
            // chkMerge
            // 
            this.chkMerge.AutoSize = true;
            this.chkMerge.Location = new System.Drawing.Point(53, 107);
            this.chkMerge.Name = "chkMerge";
            this.chkMerge.Size = new System.Drawing.Size(169, 17);
            this.chkMerge.TabIndex = 4;
            this.chkMerge.Text = "Merge blacklisted with the rest";
            this.chkMerge.UseVisualStyleBackColor = true;
            // 
            // chkOpen
            // 
            this.chkOpen.AutoSize = true;
            this.chkOpen.Location = new System.Drawing.Point(53, 84);
            this.chkOpen.Name = "chkOpen";
            this.chkOpen.Size = new System.Drawing.Size(139, 17);
            this.chkOpen.TabIndex = 3;
            this.chkOpen.Text = "Open after downloading";
            this.chkOpen.UseVisualStyleBackColor = true;
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
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(98, 36);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(79, 20);
            this.txtID.TabIndex = 1;
            this.txtID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtID.TextChanged += new System.EventHandler(this.txtID_TextChanged);
            this.txtID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtID_KeyDown);
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
            this.tbImages.Controls.Add(this.label4);
            this.tbImages.Location = new System.Drawing.Point(4, 22);
            this.tbImages.Name = "tbImages";
            this.tbImages.Padding = new System.Windows.Forms.Padding(3);
            this.tbImages.Size = new System.Drawing.Size(266, 168);
            this.tbImages.TabIndex = 3;
            this.tbImages.Text = "Image";
            this.tbImages.UseVisualStyleBackColor = true;
            // 
            // chkImageSeparateArtists
            // 
            this.chkImageSeparateArtists.AutoSize = true;
            this.chkImageSeparateArtists.Location = new System.Drawing.Point(84, 114);
            this.chkImageSeparateArtists.Name = "chkImageSeparateArtists";
            this.chkImageSeparateArtists.Size = new System.Drawing.Size(99, 17);
            this.chkImageSeparateArtists.TabIndex = 16;
            this.chkImageSeparateArtists.Text = "Separate artists";
            this.chkImageSeparateArtists.UseVisualStyleBackColor = true;
            // 
            // chkImageUseForm
            // 
            this.chkImageUseForm.AutoSize = true;
            this.chkImageUseForm.Location = new System.Drawing.Point(61, 137);
            this.chkImageUseForm.Name = "chkImageUseForm";
            this.chkImageUseForm.Size = new System.Drawing.Size(151, 17);
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
            this.chkImageSeparateBlacklisted.Size = new System.Drawing.Size(122, 17);
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
            this.chkImageSeparateRatings.Size = new System.Drawing.Size(103, 17);
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
            this.txtImageUrl.Size = new System.Drawing.Size(223, 20);
            this.txtImageUrl.TabIndex = 1;
            this.txtImageUrl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TouchingTips.SetToolTip(this.txtImageUrl, "The image that will be downloaded");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Image URL/ID:";
            // 
            // tbIni
            // 
            this.tbIni.Controls.Add(this.lbIni);
            this.tbIni.Location = new System.Drawing.Point(4, 22);
            this.tbIni.Name = "tbIni";
            this.tbIni.Padding = new System.Windows.Forms.Padding(3);
            this.tbIni.Size = new System.Drawing.Size(266, 168);
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
            this.ClientSize = new System.Drawing.Size(274, 194);
            this.Controls.Add(this.tbMain);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(290, 274);
            this.Menu = this.toolMenu;
            this.MinimumSize = new System.Drawing.Size(290, 274);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "aphrodite";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            this.tbMain.ResumeLayout(false);
            this.tbTags.ResumeLayout(false);
            this.tbTags.PerformLayout();
            this.gbtPageLimit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numPageLimit)).EndInit();
            this.gbtRatings.ResumeLayout(false);
            this.gbtRatings.PerformLayout();
            this.gbtImageLimit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numLimit)).EndInit();
            this.gbtScore.ResumeLayout(false);
            this.gbtScore.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScore)).EndInit();
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
        private System.Windows.Forms.CheckBox chkSafe;
        private System.Windows.Forms.CheckBox chkQuestionable;
        private System.Windows.Forms.CheckBox chkExplicit;
        private System.Windows.Forms.NumericUpDown numLimit;
        private System.Windows.Forms.CheckBox chkMinimumScore;
        private System.Windows.Forms.NumericUpDown numScore;
        private System.Windows.Forms.TextBox txtTags;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkMerge;
        private System.Windows.Forms.CheckBox chkOpen;
        private System.Windows.Forms.Button btnDownloadPool;
        private System.Windows.Forms.TextBox txtID;
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
        private System.Windows.Forms.NumericUpDown numPageLimit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkScoreAsTag;
        private System.Windows.Forms.ToolTip TouchingTips;
        private System.Windows.Forms.CheckBox chkSeparateRatings;
        private System.Windows.Forms.TabPage tbIni;
        private System.Windows.Forms.Label lbIni;
        private System.Windows.Forms.TabPage tbImages;
        private System.Windows.Forms.TextBox txtImageUrl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDownloadImage;
        private System.Windows.Forms.CheckBox chkImageUseForm;
        private System.Windows.Forms.CheckBox chkImageSeparateBlacklisted;
        private System.Windows.Forms.CheckBox chkImageSeparateRatings;
        private System.Windows.Forms.MenuItem mParser;
        private System.Windows.Forms.CheckBox chkImageSeparateArtists;
    }
}

