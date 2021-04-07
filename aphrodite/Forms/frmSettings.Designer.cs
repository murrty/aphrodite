namespace aphrodite {
    partial class frmSettings {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbMain = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.chkIgnoreFinish = new System.Windows.Forms.CheckBox();
            this.chkSaveBlacklistedImages = new System.Windows.Forms.CheckBox();
            this.chkSaveInfoFiles = new System.Windows.Forms.CheckBox();
            this.btnBrowseForSaveTo = new System.Windows.Forms.Button();
            this.txtSaveTo = new System.Windows.Forms.TextBox();
            this.lbSaveTo = new System.Windows.Forms.Label();
            this.gbMetadata = new System.Windows.Forms.GroupBox();
            this.chkSaveTagMetadata = new System.Windows.Forms.CheckBox();
            this.chkSaveArtistMetadata = new System.Windows.Forms.CheckBox();
            this.chkSaveMetadata = new System.Windows.Forms.CheckBox();
            this.tabTags = new System.Windows.Forms.TabPage();
            this.chkTagsSeparateNonImages = new System.Windows.Forms.CheckBox();
            this.gbTagDownloadLimit = new System.Windows.Forms.GroupBox();
            this.numTagsDownloadLimit = new System.Windows.Forms.NumericUpDown();
            this.gbTagsRatings = new System.Windows.Forms.GroupBox();
            this.chkTagsExplicit = new System.Windows.Forms.CheckBox();
            this.chkTagsSeparateRatings = new System.Windows.Forms.CheckBox();
            this.chkTagsQuestionable = new System.Windows.Forms.CheckBox();
            this.chkTagsSafe = new System.Windows.Forms.CheckBox();
            this.gbTagsScoreLimit = new System.Windows.Forms.GroupBox();
            this.chkTagsIncludeScoreAsTag = new System.Windows.Forms.CheckBox();
            this.numTagsScoreLimit = new System.Windows.Forms.NumericUpDown();
            this.chkTagsEnableScoreLimit = new System.Windows.Forms.CheckBox();
            this.gbTagsPageLimit = new System.Windows.Forms.GroupBox();
            this.numTagsPageLimit = new System.Windows.Forms.NumericUpDown();
            this.tabPools = new System.Windows.Forms.TabPage();
            this.chkPoolsMergeBlacklistedImages = new System.Windows.Forms.CheckBox();
            this.chkPoolsAddToWishlistSilently = new System.Windows.Forms.CheckBox();
            this.chkPoolsOpenAfterDownload = new System.Windows.Forms.CheckBox();
            this.tabImages = new System.Windows.Forms.TabPage();
            this.chkImagesSeparateNonImages = new System.Windows.Forms.CheckBox();
            this.chkImagesUseForm = new System.Windows.Forms.CheckBox();
            this.chkImagesSeparateArtists = new System.Windows.Forms.CheckBox();
            this.chkImagesSeparateBlacklisted = new System.Windows.Forms.CheckBox();
            this.chkImagesSeparateRatings = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lbImagesHeader = new System.Windows.Forms.Label();
            this.tabProtocol = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.btnProtocolImagesUserscript = new System.Windows.Forms.Button();
            this.btnProtocolUserscript = new System.Windows.Forms.Button();
            this.lbProtocols = new System.Windows.Forms.Label();
            this.btnProtocolInstallTags = new System.Windows.Forms.Button();
            this.btnProtocolInstallPools = new System.Windows.Forms.Button();
            this.btnProtocolInstallImages = new System.Windows.Forms.Button();
            this.tabPortable = new System.Windows.Forms.TabPage();
            this.btnPortableExportIni = new System.Windows.Forms.Button();
            this.lbPortable = new System.Windows.Forms.Label();
            this.tabSchemas = new System.Windows.Forms.TabPage();
            this.btnSchemaUndesiredTags = new System.Windows.Forms.Button();
            this.lbImageExt = new System.Windows.Forms.Label();
            this.lbPoolExt = new System.Windows.Forms.Label();
            this.lbTagExt = new System.Windows.Forms.Label();
            this.rtbParams = new System.Windows.Forms.RichTextBox();
            this.lbSchemaParam = new System.Windows.Forms.Label();
            this.lblTagSchema = new System.Windows.Forms.Label();
            this.lbPoolSchema = new System.Windows.Forms.Label();
            this.txtImageSchema = new System.Windows.Forms.TextBox();
            this.txtTagSchema = new System.Windows.Forms.TextBox();
            this.lbImageSchema = new System.Windows.Forms.Label();
            this.txtPoolSchema = new System.Windows.Forms.TextBox();
            this.btnBlacklist = new System.Windows.Forms.Button();
            this.JustTheTips = new System.Windows.Forms.ToolTip(this.components);
            this.tbMain.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.gbMetadata.SuspendLayout();
            this.tabTags.SuspendLayout();
            this.gbTagDownloadLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsDownloadLimit)).BeginInit();
            this.gbTagsRatings.SuspendLayout();
            this.gbTagsScoreLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsScoreLimit)).BeginInit();
            this.gbTagsPageLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsPageLimit)).BeginInit();
            this.tabPools.SuspendLayout();
            this.tabImages.SuspendLayout();
            this.tabProtocol.SuspendLayout();
            this.tabPortable.SuspendLayout();
            this.tabSchemas.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(238, 230);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(319, 230);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbMain
            // 
            this.tbMain.Controls.Add(this.tabGeneral);
            this.tbMain.Controls.Add(this.tabTags);
            this.tbMain.Controls.Add(this.tabPools);
            this.tbMain.Controls.Add(this.tabImages);
            this.tbMain.Controls.Add(this.tabProtocol);
            this.tbMain.Controls.Add(this.tabPortable);
            this.tbMain.Controls.Add(this.tabSchemas);
            this.tbMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbMain.Location = new System.Drawing.Point(0, 0);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(408, 223);
            this.tbMain.TabIndex = 10;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.chkIgnoreFinish);
            this.tabGeneral.Controls.Add(this.chkSaveBlacklistedImages);
            this.tabGeneral.Controls.Add(this.chkSaveInfoFiles);
            this.tabGeneral.Controls.Add(this.btnBrowseForSaveTo);
            this.tabGeneral.Controls.Add(this.txtSaveTo);
            this.tabGeneral.Controls.Add(this.lbSaveTo);
            this.tabGeneral.Controls.Add(this.gbMetadata);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(400, 197);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreFinish
            // 
            this.chkIgnoreFinish.AutoSize = true;
            this.chkIgnoreFinish.Location = new System.Drawing.Point(23, 145);
            this.chkIgnoreFinish.Name = "chkIgnoreFinish";
            this.chkIgnoreFinish.Size = new System.Drawing.Size(193, 17);
            this.chkIgnoreFinish.TabIndex = 4;
            this.chkIgnoreFinish.Text = "Don\'t notify finished downloads";
            this.JustTheTips.SetToolTip(this.chkIgnoreFinish, "Doesn\'t notify you when downloads are completed, and aborted downloads close when" +
        " requested\r\nPlugin downloads quit when finished");
            this.chkIgnoreFinish.UseVisualStyleBackColor = true;
            // 
            // chkSaveBlacklistedImages
            // 
            this.chkSaveBlacklistedImages.AutoSize = true;
            this.chkSaveBlacklistedImages.Checked = true;
            this.chkSaveBlacklistedImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveBlacklistedImages.Location = new System.Drawing.Point(24, 122);
            this.chkSaveBlacklistedImages.Name = "chkSaveBlacklistedImages";
            this.chkSaveBlacklistedImages.Size = new System.Drawing.Size(145, 17);
            this.chkSaveBlacklistedImages.TabIndex = 3;
            this.chkSaveBlacklistedImages.Text = "Save blacklisted images";
            this.JustTheTips.SetToolTip(this.chkSaveBlacklistedImages, "Saves blacklisted images into a separate folder.\r\nImages downloaded with the \'ima" +
        "ges\' protocol are also saved + separated.");
            this.chkSaveBlacklistedImages.UseVisualStyleBackColor = true;
            // 
            // chkSaveInfoFiles
            // 
            this.chkSaveInfoFiles.AutoSize = true;
            this.chkSaveInfoFiles.Checked = true;
            this.chkSaveInfoFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveInfoFiles.Location = new System.Drawing.Point(23, 99);
            this.chkSaveInfoFiles.Name = "chkSaveInfoFiles";
            this.chkSaveInfoFiles.Size = new System.Drawing.Size(96, 17);
            this.chkSaveInfoFiles.TabIndex = 2;
            this.chkSaveInfoFiles.Text = "Save info files";
            this.JustTheTips.SetToolTip(this.chkSaveInfoFiles, resources.GetString("chkSaveInfoFiles.ToolTip"));
            this.chkSaveInfoFiles.UseVisualStyleBackColor = true;
            // 
            // btnBrowseForSaveTo
            // 
            this.btnBrowseForSaveTo.Location = new System.Drawing.Point(353, 36);
            this.btnBrowseForSaveTo.Name = "btnBrowseForSaveTo";
            this.btnBrowseForSaveTo.Size = new System.Drawing.Size(24, 23);
            this.btnBrowseForSaveTo.TabIndex = 1;
            this.btnBrowseForSaveTo.Text = "...";
            this.btnBrowseForSaveTo.UseVisualStyleBackColor = true;
            this.btnBrowseForSaveTo.Click += new System.EventHandler(this.btnBrws_Click);
            // 
            // txtSaveTo
            // 
            this.txtSaveTo.Location = new System.Drawing.Point(24, 39);
            this.txtSaveTo.Name = "txtSaveTo";
            this.txtSaveTo.ReadOnly = true;
            this.txtSaveTo.Size = new System.Drawing.Size(323, 22);
            this.txtSaveTo.TabIndex = 0;
            this.JustTheTips.SetToolTip(this.txtSaveTo, "The location where pools, tags, and images will be saved to.");
            // 
            // lbSaveTo
            // 
            this.lbSaveTo.AutoSize = true;
            this.lbSaveTo.Location = new System.Drawing.Point(21, 23);
            this.lbSaveTo.Name = "lbSaveTo";
            this.lbSaveTo.Size = new System.Drawing.Size(47, 13);
            this.lbSaveTo.TabIndex = 17;
            this.lbSaveTo.Text = "Save to:";
            // 
            // gbMetadata
            // 
            this.gbMetadata.Controls.Add(this.chkSaveTagMetadata);
            this.gbMetadata.Controls.Add(this.chkSaveArtistMetadata);
            this.gbMetadata.Controls.Add(this.chkSaveMetadata);
            this.gbMetadata.Location = new System.Drawing.Point(219, 84);
            this.gbMetadata.Name = "gbMetadata";
            this.gbMetadata.Size = new System.Drawing.Size(171, 89);
            this.gbMetadata.TabIndex = 5;
            this.gbMetadata.TabStop = false;
            this.gbMetadata.Text = "Image metadata";
            this.gbMetadata.Visible = false;
            // 
            // chkSaveTagMetadata
            // 
            this.chkSaveTagMetadata.AutoSize = true;
            this.chkSaveTagMetadata.Checked = true;
            this.chkSaveTagMetadata.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveTagMetadata.Location = new System.Drawing.Point(8, 65);
            this.chkSaveTagMetadata.Name = "chkSaveTagMetadata";
            this.chkSaveTagMetadata.Size = new System.Drawing.Size(138, 17);
            this.chkSaveTagMetadata.TabIndex = 8;
            this.chkSaveTagMetadata.Text = "Save tags to metadata";
            this.JustTheTips.SetToolTip(this.chkSaveTagMetadata, "When enabled, the tags of the image will be saved to the file (if it supports met" +
        "adata tagging)");
            this.chkSaveTagMetadata.UseVisualStyleBackColor = true;
            // 
            // chkSaveArtistMetadata
            // 
            this.chkSaveArtistMetadata.AutoSize = true;
            this.chkSaveArtistMetadata.Checked = true;
            this.chkSaveArtistMetadata.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveArtistMetadata.Location = new System.Drawing.Point(8, 42);
            this.chkSaveArtistMetadata.Name = "chkSaveArtistMetadata";
            this.chkSaveArtistMetadata.Size = new System.Drawing.Size(153, 17);
            this.chkSaveArtistMetadata.TabIndex = 7;
            this.chkSaveArtistMetadata.Text = "Save artist(s) to metadata";
            this.JustTheTips.SetToolTip(this.chkSaveArtistMetadata, "When enabled, the artist(s) of the image will be saved to the file (if it support" +
        "s metadata tagging)");
            this.chkSaveArtistMetadata.UseVisualStyleBackColor = true;
            // 
            // chkSaveMetadata
            // 
            this.chkSaveMetadata.AutoSize = true;
            this.chkSaveMetadata.Checked = true;
            this.chkSaveMetadata.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveMetadata.Location = new System.Drawing.Point(8, 19);
            this.chkSaveMetadata.Name = "chkSaveMetadata";
            this.chkSaveMetadata.Size = new System.Drawing.Size(164, 17);
            this.chkSaveMetadata.TabIndex = 6;
            this.chkSaveMetadata.Text = "Save metadata with images";
            this.JustTheTips.SetToolTip(this.chkSaveMetadata, "Save metadata to the image file when downloading");
            this.chkSaveMetadata.UseVisualStyleBackColor = true;
            // 
            // tabTags
            // 
            this.tabTags.Controls.Add(this.chkTagsSeparateNonImages);
            this.tabTags.Controls.Add(this.gbTagDownloadLimit);
            this.tabTags.Controls.Add(this.gbTagsRatings);
            this.tabTags.Controls.Add(this.gbTagsScoreLimit);
            this.tabTags.Controls.Add(this.gbTagsPageLimit);
            this.tabTags.Location = new System.Drawing.Point(4, 22);
            this.tabTags.Name = "tabTags";
            this.tabTags.Padding = new System.Windows.Forms.Padding(3);
            this.tabTags.Size = new System.Drawing.Size(400, 197);
            this.tabTags.TabIndex = 1;
            this.tabTags.Text = "Tags";
            this.tabTags.UseVisualStyleBackColor = true;
            // 
            // chkTagsSeparateNonImages
            // 
            this.chkTagsSeparateNonImages.AutoSize = true;
            this.chkTagsSeparateNonImages.Location = new System.Drawing.Point(58, 148);
            this.chkTagsSeparateNonImages.Name = "chkTagsSeparateNonImages";
            this.chkTagsSeparateNonImages.Size = new System.Drawing.Size(134, 17);
            this.chkTagsSeparateNonImages.TabIndex = 6;
            this.chkTagsSeparateNonImages.Text = "Separate non-images";
            this.JustTheTips.SetToolTip(this.chkTagsSeparateNonImages, "Enable this option to further separate non-image files into their own folder");
            this.chkTagsSeparateNonImages.UseVisualStyleBackColor = true;
            // 
            // gbTagDownloadLimit
            // 
            this.gbTagDownloadLimit.Controls.Add(this.numTagsDownloadLimit);
            this.gbTagDownloadLimit.Location = new System.Drawing.Point(203, 103);
            this.gbTagDownloadLimit.Name = "gbTagDownloadLimit";
            this.gbTagDownloadLimit.Size = new System.Drawing.Size(147, 57);
            this.gbTagDownloadLimit.TabIndex = 4;
            this.gbTagDownloadLimit.TabStop = false;
            this.gbTagDownloadLimit.Text = "Download limit (0 = off)";
            // 
            // numTagsDownloadLimit
            // 
            this.numTagsDownloadLimit.Location = new System.Drawing.Point(31, 23);
            this.numTagsDownloadLimit.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numTagsDownloadLimit.Name = "numTagsDownloadLimit";
            this.numTagsDownloadLimit.Size = new System.Drawing.Size(81, 22);
            this.numTagsDownloadLimit.TabIndex = 1;
            this.numTagsDownloadLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.JustTheTips.SetToolTip(this.numTagsDownloadLimit, "Limits downloads to a certain amount of images");
            // 
            // gbTagsRatings
            // 
            this.gbTagsRatings.Controls.Add(this.chkTagsExplicit);
            this.gbTagsRatings.Controls.Add(this.chkTagsSeparateRatings);
            this.gbTagsRatings.Controls.Add(this.chkTagsQuestionable);
            this.gbTagsRatings.Controls.Add(this.chkTagsSafe);
            this.gbTagsRatings.Location = new System.Drawing.Point(50, 20);
            this.gbTagsRatings.Name = "gbTagsRatings";
            this.gbTagsRatings.Size = new System.Drawing.Size(147, 67);
            this.gbTagsRatings.TabIndex = 1;
            this.gbTagsRatings.TabStop = false;
            this.gbTagsRatings.Text = "Ratings";
            // 
            // chkTagsExplicit
            // 
            this.chkTagsExplicit.AutoSize = true;
            this.chkTagsExplicit.Checked = true;
            this.chkTagsExplicit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagsExplicit.Location = new System.Drawing.Point(17, 19);
            this.chkTagsExplicit.Name = "chkTagsExplicit";
            this.chkTagsExplicit.Size = new System.Drawing.Size(31, 17);
            this.chkTagsExplicit.TabIndex = 1;
            this.chkTagsExplicit.Text = "E";
            this.JustTheTips.SetToolTip(this.chkTagsExplicit, "Download images rated Explicit");
            this.chkTagsExplicit.UseVisualStyleBackColor = true;
            // 
            // chkTagsSeparateRatings
            // 
            this.chkTagsSeparateRatings.AutoSize = true;
            this.chkTagsSeparateRatings.Checked = true;
            this.chkTagsSeparateRatings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagsSeparateRatings.Location = new System.Drawing.Point(20, 42);
            this.chkTagsSeparateRatings.Name = "chkTagsSeparateRatings";
            this.chkTagsSeparateRatings.Size = new System.Drawing.Size(109, 17);
            this.chkTagsSeparateRatings.TabIndex = 4;
            this.chkTagsSeparateRatings.Text = "Separate ratings";
            this.JustTheTips.SetToolTip(this.chkTagsSeparateRatings, "Separates ratings into different folders");
            this.chkTagsSeparateRatings.UseVisualStyleBackColor = true;
            // 
            // chkTagsQuestionable
            // 
            this.chkTagsQuestionable.AutoSize = true;
            this.chkTagsQuestionable.Checked = true;
            this.chkTagsQuestionable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagsQuestionable.Location = new System.Drawing.Point(55, 18);
            this.chkTagsQuestionable.Name = "chkTagsQuestionable";
            this.chkTagsQuestionable.Size = new System.Drawing.Size(33, 17);
            this.chkTagsQuestionable.TabIndex = 2;
            this.chkTagsQuestionable.Text = "Q";
            this.JustTheTips.SetToolTip(this.chkTagsQuestionable, "Download images rated Questionable");
            this.chkTagsQuestionable.UseVisualStyleBackColor = true;
            // 
            // chkTagsSafe
            // 
            this.chkTagsSafe.AutoSize = true;
            this.chkTagsSafe.Checked = true;
            this.chkTagsSafe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagsSafe.Location = new System.Drawing.Point(94, 18);
            this.chkTagsSafe.Name = "chkTagsSafe";
            this.chkTagsSafe.Size = new System.Drawing.Size(31, 17);
            this.chkTagsSafe.TabIndex = 3;
            this.chkTagsSafe.Text = "S";
            this.JustTheTips.SetToolTip(this.chkTagsSafe, "Download images rated Safe");
            this.chkTagsSafe.UseVisualStyleBackColor = true;
            // 
            // gbTagsScoreLimit
            // 
            this.gbTagsScoreLimit.Controls.Add(this.chkTagsIncludeScoreAsTag);
            this.gbTagsScoreLimit.Controls.Add(this.numTagsScoreLimit);
            this.gbTagsScoreLimit.Controls.Add(this.chkTagsEnableScoreLimit);
            this.gbTagsScoreLimit.Location = new System.Drawing.Point(203, 20);
            this.gbTagsScoreLimit.Name = "gbTagsScoreLimit";
            this.gbTagsScoreLimit.Size = new System.Drawing.Size(147, 82);
            this.gbTagsScoreLimit.TabIndex = 2;
            this.gbTagsScoreLimit.TabStop = false;
            this.gbTagsScoreLimit.Text = "Score limit";
            // 
            // chkTagsIncludeScoreAsTag
            // 
            this.chkTagsIncludeScoreAsTag.AutoSize = true;
            this.chkTagsIncludeScoreAsTag.Enabled = false;
            this.chkTagsIncludeScoreAsTag.Location = new System.Drawing.Point(19, 60);
            this.chkTagsIncludeScoreAsTag.Name = "chkTagsIncludeScoreAsTag";
            this.chkTagsIncludeScoreAsTag.Size = new System.Drawing.Size(114, 17);
            this.chkTagsIncludeScoreAsTag.TabIndex = 3;
            this.chkTagsIncludeScoreAsTag.Text = "Include with tags";
            this.JustTheTips.SetToolTip(this.chkTagsIncludeScoreAsTag, "If checked, uses the score minimum as a tag (ex: \"gay score:>25\").\r\nThis widly in" +
        "creases the images that will be downloaded.\r\nOnly used if there are 5 or less ta" +
        "gs being queried");
            this.chkTagsIncludeScoreAsTag.UseVisualStyleBackColor = true;
            // 
            // numTagsScoreLimit
            // 
            this.numTagsScoreLimit.Enabled = false;
            this.numTagsScoreLimit.Location = new System.Drawing.Point(40, 19);
            this.numTagsScoreLimit.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numTagsScoreLimit.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.numTagsScoreLimit.Name = "numTagsScoreLimit";
            this.numTagsScoreLimit.Size = new System.Drawing.Size(63, 22);
            this.numTagsScoreLimit.TabIndex = 1;
            this.numTagsScoreLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.JustTheTips.SetToolTip(this.numTagsScoreLimit, "The minimum score that will be downloaded");
            // 
            // chkTagsEnableScoreLimit
            // 
            this.chkTagsEnableScoreLimit.AutoSize = true;
            this.chkTagsEnableScoreLimit.Location = new System.Drawing.Point(18, 43);
            this.chkTagsEnableScoreLimit.Name = "chkTagsEnableScoreLimit";
            this.chkTagsEnableScoreLimit.Size = new System.Drawing.Size(115, 17);
            this.chkTagsEnableScoreLimit.TabIndex = 2;
            this.chkTagsEnableScoreLimit.Text = "Enable score limit";
            this.JustTheTips.SetToolTip(this.chkTagsEnableScoreLimit, "Only downloads images with a score equal to or greater than provided");
            this.chkTagsEnableScoreLimit.UseVisualStyleBackColor = true;
            this.chkTagsEnableScoreLimit.CheckedChanged += new System.EventHandler(this.chkMinimumScore_CheckedChanged);
            // 
            // gbTagsPageLimit
            // 
            this.gbTagsPageLimit.Controls.Add(this.numTagsPageLimit);
            this.gbTagsPageLimit.Location = new System.Drawing.Point(50, 93);
            this.gbTagsPageLimit.Name = "gbTagsPageLimit";
            this.gbTagsPageLimit.Size = new System.Drawing.Size(147, 49);
            this.gbTagsPageLimit.TabIndex = 3;
            this.gbTagsPageLimit.TabStop = false;
            this.gbTagsPageLimit.Text = "Page limit (0 = off)";
            // 
            // numTagsPageLimit
            // 
            this.numTagsPageLimit.Location = new System.Drawing.Point(40, 19);
            this.numTagsPageLimit.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numTagsPageLimit.Name = "numTagsPageLimit";
            this.numTagsPageLimit.Size = new System.Drawing.Size(63, 22);
            this.numTagsPageLimit.TabIndex = 1;
            this.numTagsPageLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.JustTheTips.SetToolTip(this.numTagsPageLimit, "The amount of pages that will be downloaded (320 images per page)");
            // 
            // tabPools
            // 
            this.tabPools.Controls.Add(this.chkPoolsMergeBlacklistedImages);
            this.tabPools.Controls.Add(this.chkPoolsAddToWishlistSilently);
            this.tabPools.Controls.Add(this.chkPoolsOpenAfterDownload);
            this.tabPools.Location = new System.Drawing.Point(4, 22);
            this.tabPools.Name = "tabPools";
            this.tabPools.Padding = new System.Windows.Forms.Padding(3);
            this.tabPools.Size = new System.Drawing.Size(400, 197);
            this.tabPools.TabIndex = 2;
            this.tabPools.Text = "Pools";
            this.tabPools.UseVisualStyleBackColor = true;
            // 
            // chkPoolsMergeBlacklistedImages
            // 
            this.chkPoolsMergeBlacklistedImages.AutoSize = true;
            this.chkPoolsMergeBlacklistedImages.Checked = true;
            this.chkPoolsMergeBlacklistedImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPoolsMergeBlacklistedImages.Location = new System.Drawing.Point(67, 54);
            this.chkPoolsMergeBlacklistedImages.Name = "chkPoolsMergeBlacklistedImages";
            this.chkPoolsMergeBlacklistedImages.Size = new System.Drawing.Size(302, 17);
            this.chkPoolsMergeBlacklistedImages.TabIndex = 2;
            this.chkPoolsMergeBlacklistedImages.Text = "Merge blacklisted images with non blacklisted images";
            this.JustTheTips.SetToolTip(this.chkPoolsMergeBlacklistedImages, "Merge blacklisted images with the rest of the pool\r\npool.blacklisted.nfo is not s" +
        "aved, instead, blacklisted info is saved in pool.nfo under \"BLACKLISTED PAGE\"");
            this.chkPoolsMergeBlacklistedImages.UseVisualStyleBackColor = true;
            // 
            // chkPoolsAddToWishlistSilently
            // 
            this.chkPoolsAddToWishlistSilently.AutoSize = true;
            this.chkPoolsAddToWishlistSilently.Location = new System.Drawing.Point(114, 126);
            this.chkPoolsAddToWishlistSilently.Name = "chkPoolsAddToWishlistSilently";
            this.chkPoolsAddToWishlistSilently.Size = new System.Drawing.Size(173, 17);
            this.chkPoolsAddToWishlistSilently.TabIndex = 4;
            this.chkPoolsAddToWishlistSilently.Text = "Add pools to wishlist silently";
            this.JustTheTips.SetToolTip(this.chkPoolsAddToWishlistSilently, "Add the pool to your wishlist without showing the application.");
            this.chkPoolsAddToWishlistSilently.UseVisualStyleBackColor = true;
            // 
            // chkPoolsOpenAfterDownload
            // 
            this.chkPoolsOpenAfterDownload.AutoSize = true;
            this.chkPoolsOpenAfterDownload.Location = new System.Drawing.Point(123, 77);
            this.chkPoolsOpenAfterDownload.Name = "chkPoolsOpenAfterDownload";
            this.chkPoolsOpenAfterDownload.Size = new System.Drawing.Size(154, 17);
            this.chkPoolsOpenAfterDownload.TabIndex = 3;
            this.chkPoolsOpenAfterDownload.Text = "Open after downloading";
            this.JustTheTips.SetToolTip(this.chkPoolsOpenAfterDownload, "Opens the pool folder after downloading");
            this.chkPoolsOpenAfterDownload.UseVisualStyleBackColor = true;
            // 
            // tabImages
            // 
            this.tabImages.Controls.Add(this.chkImagesSeparateNonImages);
            this.tabImages.Controls.Add(this.chkImagesUseForm);
            this.tabImages.Controls.Add(this.chkImagesSeparateArtists);
            this.tabImages.Controls.Add(this.chkImagesSeparateBlacklisted);
            this.tabImages.Controls.Add(this.chkImagesSeparateRatings);
            this.tabImages.Controls.Add(this.label9);
            this.tabImages.Controls.Add(this.lbImagesHeader);
            this.tabImages.Location = new System.Drawing.Point(4, 22);
            this.tabImages.Name = "tabImages";
            this.tabImages.Padding = new System.Windows.Forms.Padding(3);
            this.tabImages.Size = new System.Drawing.Size(400, 197);
            this.tabImages.TabIndex = 4;
            this.tabImages.Text = "Images";
            this.tabImages.UseVisualStyleBackColor = true;
            // 
            // chkImagesSeparateNonImages
            // 
            this.chkImagesSeparateNonImages.AutoSize = true;
            this.chkImagesSeparateNonImages.Checked = true;
            this.chkImagesSeparateNonImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImagesSeparateNonImages.Location = new System.Drawing.Point(185, 110);
            this.chkImagesSeparateNonImages.Name = "chkImagesSeparateNonImages";
            this.chkImagesSeparateNonImages.Size = new System.Drawing.Size(134, 17);
            this.chkImagesSeparateNonImages.TabIndex = 9;
            this.chkImagesSeparateNonImages.Text = "Separate non-images";
            this.JustTheTips.SetToolTip(this.chkImagesSeparateNonImages, "Enable this option to further separate non-image files into their own folder");
            this.chkImagesSeparateNonImages.UseVisualStyleBackColor = true;
            // 
            // chkImagesUseForm
            // 
            this.chkImagesUseForm.AutoSize = true;
            this.chkImagesUseForm.Location = new System.Drawing.Point(84, 133);
            this.chkImagesUseForm.Name = "chkImagesUseForm";
            this.chkImagesUseForm.Size = new System.Drawing.Size(249, 17);
            this.chkImagesUseForm.TabIndex = 4;
            this.chkImagesUseForm.Text = "Use download form for download progress";
            this.JustTheTips.SetToolTip(this.chkImagesUseForm, "Shows a form when downloading images that will report progress");
            this.chkImagesUseForm.UseVisualStyleBackColor = true;
            // 
            // chkImagesSeparateArtists
            // 
            this.chkImagesSeparateArtists.AutoSize = true;
            this.chkImagesSeparateArtists.Location = new System.Drawing.Point(81, 110);
            this.chkImagesSeparateArtists.Name = "chkImagesSeparateArtists";
            this.chkImagesSeparateArtists.Size = new System.Drawing.Size(104, 17);
            this.chkImagesSeparateArtists.TabIndex = 3;
            this.chkImagesSeparateArtists.Text = "Separate artists";
            this.chkImagesSeparateArtists.UseVisualStyleBackColor = true;
            // 
            // chkImagesSeparateBlacklisted
            // 
            this.chkImagesSeparateBlacklisted.AutoSize = true;
            this.chkImagesSeparateBlacklisted.Checked = true;
            this.chkImagesSeparateBlacklisted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImagesSeparateBlacklisted.Location = new System.Drawing.Point(176, 87);
            this.chkImagesSeparateBlacklisted.Name = "chkImagesSeparateBlacklisted";
            this.chkImagesSeparateBlacklisted.Size = new System.Drawing.Size(167, 17);
            this.chkImagesSeparateBlacklisted.TabIndex = 2;
            this.chkImagesSeparateBlacklisted.Text = "Separate blacklisted images";
            this.JustTheTips.SetToolTip(this.chkImagesSeparateBlacklisted, "Separates blacklisted images into separate folder.");
            this.chkImagesSeparateBlacklisted.UseVisualStyleBackColor = true;
            // 
            // chkImagesSeparateRatings
            // 
            this.chkImagesSeparateRatings.AutoSize = true;
            this.chkImagesSeparateRatings.Checked = true;
            this.chkImagesSeparateRatings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImagesSeparateRatings.Location = new System.Drawing.Point(57, 87);
            this.chkImagesSeparateRatings.Name = "chkImagesSeparateRatings";
            this.chkImagesSeparateRatings.Size = new System.Drawing.Size(109, 17);
            this.chkImagesSeparateRatings.TabIndex = 1;
            this.chkImagesSeparateRatings.Text = "Separate ratings";
            this.JustTheTips.SetToolTip(this.chkImagesSeparateRatings, "Separate ratings into separate folders for images");
            this.chkImagesSeparateRatings.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label9.Location = new System.Drawing.Point(54, 34);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(292, 2);
            this.label9.TabIndex = 7;
            this.label9.Text = "bark";
            // 
            // lbImagesHeader
            // 
            this.lbImagesHeader.Location = new System.Drawing.Point(6, 10);
            this.lbImagesHeader.Name = "lbImagesHeader";
            this.lbImagesHeader.Size = new System.Drawing.Size(388, 19);
            this.lbImagesHeader.TabIndex = 1;
            this.lbImagesHeader.Text = "These options only effect the \'images:\' protocol + userscript";
            this.lbImagesHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabProtocol
            // 
            this.tabProtocol.Controls.Add(this.label7);
            this.tabProtocol.Controls.Add(this.btnProtocolImagesUserscript);
            this.tabProtocol.Controls.Add(this.btnProtocolUserscript);
            this.tabProtocol.Controls.Add(this.lbProtocols);
            this.tabProtocol.Controls.Add(this.btnProtocolInstallTags);
            this.tabProtocol.Controls.Add(this.btnProtocolInstallPools);
            this.tabProtocol.Controls.Add(this.btnProtocolInstallImages);
            this.tabProtocol.Location = new System.Drawing.Point(4, 22);
            this.tabProtocol.Name = "tabProtocol";
            this.tabProtocol.Padding = new System.Windows.Forms.Padding(3);
            this.tabProtocol.Size = new System.Drawing.Size(400, 197);
            this.tabProtocol.TabIndex = 3;
            this.tabProtocol.Text = "Protocols";
            this.tabProtocol.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Location = new System.Drawing.Point(59, 150);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(292, 2);
            this.label7.TabIndex = 6;
            this.label7.Text = "bark";
            // 
            // btnProtocolImagesUserscript
            // 
            this.btnProtocolImagesUserscript.Location = new System.Drawing.Point(246, 161);
            this.btnProtocolImagesUserscript.Name = "btnProtocolImagesUserscript";
            this.btnProtocolImagesUserscript.Size = new System.Drawing.Size(75, 23);
            this.btnProtocolImagesUserscript.TabIndex = 5;
            this.btnProtocolImagesUserscript.Text = "Userscript";
            this.btnProtocolImagesUserscript.UseVisualStyleBackColor = true;
            this.btnProtocolImagesUserscript.Click += new System.EventHandler(this.btnImagesUserscript_Click);
            // 
            // btnProtocolUserscript
            // 
            this.btnProtocolUserscript.Location = new System.Drawing.Point(246, 108);
            this.btnProtocolUserscript.Name = "btnProtocolUserscript";
            this.btnProtocolUserscript.Size = new System.Drawing.Size(75, 23);
            this.btnProtocolUserscript.TabIndex = 3;
            this.btnProtocolUserscript.Text = "Userscript";
            this.btnProtocolUserscript.UseVisualStyleBackColor = true;
            this.btnProtocolUserscript.Click += new System.EventHandler(this.btnUserscript_Click);
            // 
            // lbProtocols
            // 
            this.lbProtocols.Location = new System.Drawing.Point(6, 7);
            this.lbProtocols.Name = "lbProtocols";
            this.lbProtocols.Size = new System.Drawing.Size(388, 80);
            this.lbProtocols.TabIndex = 3;
            this.lbProtocols.Text = resources.GetString("lbProtocols.Text");
            this.lbProtocols.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnProtocolInstallTags
            // 
            this.btnProtocolInstallTags.Location = new System.Drawing.Point(89, 96);
            this.btnProtocolInstallTags.Name = "btnProtocolInstallTags";
            this.btnProtocolInstallTags.Size = new System.Drawing.Size(151, 23);
            this.btnProtocolInstallTags.TabIndex = 1;
            this.btnProtocolInstallTags.Text = "Install \'tags:\' protocol";
            this.btnProtocolInstallTags.UseVisualStyleBackColor = true;
            this.btnProtocolInstallTags.Click += new System.EventHandler(this.btnTagsProtocol_Click);
            // 
            // btnProtocolInstallPools
            // 
            this.btnProtocolInstallPools.Location = new System.Drawing.Point(89, 121);
            this.btnProtocolInstallPools.Name = "btnProtocolInstallPools";
            this.btnProtocolInstallPools.Size = new System.Drawing.Size(151, 23);
            this.btnProtocolInstallPools.TabIndex = 2;
            this.btnProtocolInstallPools.Text = "Install pool protocols";
            this.btnProtocolInstallPools.UseVisualStyleBackColor = true;
            this.btnProtocolInstallPools.Click += new System.EventHandler(this.btnPoolsProtocol_Click);
            // 
            // btnProtocolInstallImages
            // 
            this.btnProtocolInstallImages.Location = new System.Drawing.Point(89, 161);
            this.btnProtocolInstallImages.Name = "btnProtocolInstallImages";
            this.btnProtocolInstallImages.Size = new System.Drawing.Size(151, 23);
            this.btnProtocolInstallImages.TabIndex = 4;
            this.btnProtocolInstallImages.Text = "Install \'image:\' protocol";
            this.btnProtocolInstallImages.UseVisualStyleBackColor = true;
            this.btnProtocolInstallImages.Click += new System.EventHandler(this.btnImagesProtocol_Click);
            // 
            // tabPortable
            // 
            this.tabPortable.Controls.Add(this.btnPortableExportIni);
            this.tabPortable.Controls.Add(this.lbPortable);
            this.tabPortable.Location = new System.Drawing.Point(4, 22);
            this.tabPortable.Name = "tabPortable";
            this.tabPortable.Padding = new System.Windows.Forms.Padding(3);
            this.tabPortable.Size = new System.Drawing.Size(400, 197);
            this.tabPortable.TabIndex = 5;
            this.tabPortable.Text = "Portable";
            this.tabPortable.UseVisualStyleBackColor = true;
            // 
            // btnPortableExportIni
            // 
            this.btnPortableExportIni.Location = new System.Drawing.Point(162, 159);
            this.btnPortableExportIni.Name = "btnPortableExportIni";
            this.btnPortableExportIni.Size = new System.Drawing.Size(86, 24);
            this.btnPortableExportIni.TabIndex = 1;
            this.btnPortableExportIni.Text = "Export";
            this.btnPortableExportIni.UseVisualStyleBackColor = true;
            this.btnPortableExportIni.Click += new System.EventHandler(this.btnExportIni_Click);
            // 
            // lbPortable
            // 
            this.lbPortable.Location = new System.Drawing.Point(6, 14);
            this.lbPortable.Name = "lbPortable";
            this.lbPortable.Size = new System.Drawing.Size(388, 133);
            this.lbPortable.TabIndex = 0;
            this.lbPortable.Text = resources.GetString("lbPortable.Text");
            this.lbPortable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabSchemas
            // 
            this.tabSchemas.Controls.Add(this.btnSchemaUndesiredTags);
            this.tabSchemas.Controls.Add(this.lbImageExt);
            this.tabSchemas.Controls.Add(this.lbPoolExt);
            this.tabSchemas.Controls.Add(this.lbTagExt);
            this.tabSchemas.Controls.Add(this.rtbParams);
            this.tabSchemas.Controls.Add(this.lbSchemaParam);
            this.tabSchemas.Controls.Add(this.lblTagSchema);
            this.tabSchemas.Controls.Add(this.lbPoolSchema);
            this.tabSchemas.Controls.Add(this.txtImageSchema);
            this.tabSchemas.Controls.Add(this.txtTagSchema);
            this.tabSchemas.Controls.Add(this.lbImageSchema);
            this.tabSchemas.Controls.Add(this.txtPoolSchema);
            this.tabSchemas.Location = new System.Drawing.Point(4, 22);
            this.tabSchemas.Name = "tabSchemas";
            this.tabSchemas.Padding = new System.Windows.Forms.Padding(3);
            this.tabSchemas.Size = new System.Drawing.Size(400, 197);
            this.tabSchemas.TabIndex = 6;
            this.tabSchemas.Text = "Schemas";
            this.tabSchemas.UseVisualStyleBackColor = true;
            // 
            // btnSchemaUndesiredTags
            // 
            this.btnSchemaUndesiredTags.Location = new System.Drawing.Point(272, 81);
            this.btnSchemaUndesiredTags.Name = "btnSchemaUndesiredTags";
            this.btnSchemaUndesiredTags.Size = new System.Drawing.Size(104, 23);
            this.btnSchemaUndesiredTags.TabIndex = 4;
            this.btnSchemaUndesiredTags.Text = "undesired tags...";
            this.btnSchemaUndesiredTags.UseVisualStyleBackColor = true;
            this.btnSchemaUndesiredTags.Click += new System.EventHandler(this.btnSchemaUndesiredTags_Click);
            // 
            // lbImageExt
            // 
            this.lbImageExt.AutoSize = true;
            this.lbImageExt.Location = new System.Drawing.Point(333, 58);
            this.lbImageExt.Name = "lbImageExt";
            this.lbImageExt.Size = new System.Drawing.Size(43, 13);
            this.lbImageExt.TabIndex = 51;
            this.lbImageExt.Text = ".%ext%";
            // 
            // lbPoolExt
            // 
            this.lbPoolExt.AutoSize = true;
            this.lbPoolExt.Location = new System.Drawing.Point(333, 34);
            this.lbPoolExt.Name = "lbPoolExt";
            this.lbPoolExt.Size = new System.Drawing.Size(43, 13);
            this.lbPoolExt.TabIndex = 50;
            this.lbPoolExt.Text = ".%ext%";
            // 
            // lbTagExt
            // 
            this.lbTagExt.AutoSize = true;
            this.lbTagExt.Location = new System.Drawing.Point(333, 10);
            this.lbTagExt.Name = "lbTagExt";
            this.lbTagExt.Size = new System.Drawing.Size(43, 13);
            this.lbTagExt.TabIndex = 49;
            this.lbTagExt.Text = ".%ext%";
            // 
            // rtbParams
            // 
            this.rtbParams.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbParams.Location = new System.Drawing.Point(6, 109);
            this.rtbParams.Name = "rtbParams";
            this.rtbParams.ReadOnly = true;
            this.rtbParams.Size = new System.Drawing.Size(388, 84);
            this.rtbParams.TabIndex = 5;
            this.rtbParams.Text = resources.GetString("rtbParams.Text");
            // 
            // lbSchemaParam
            // 
            this.lbSchemaParam.AutoSize = true;
            this.lbSchemaParam.Location = new System.Drawing.Point(8, 90);
            this.lbSchemaParam.Name = "lbSchemaParam";
            this.lbSchemaParam.Size = new System.Drawing.Size(122, 13);
            this.lbSchemaParam.TabIndex = 47;
            this.lbSchemaParam.Text = "Supported parameters";
            // 
            // lblTagSchema
            // 
            this.lblTagSchema.AutoSize = true;
            this.lblTagSchema.Location = new System.Drawing.Point(29, 10);
            this.lblTagSchema.Name = "lblTagSchema";
            this.lblTagSchema.Size = new System.Drawing.Size(102, 13);
            this.lblTagSchema.TabIndex = 46;
            this.lblTagSchema.Text = "Tags name schema";
            // 
            // lbPoolSchema
            // 
            this.lbPoolSchema.AutoSize = true;
            this.lbPoolSchema.Location = new System.Drawing.Point(27, 34);
            this.lbPoolSchema.Name = "lbPoolSchema";
            this.lbPoolSchema.Size = new System.Drawing.Size(107, 13);
            this.lbPoolSchema.TabIndex = 22;
            this.lbPoolSchema.Text = "Pools name schema";
            // 
            // txtImageSchema
            // 
            this.txtImageSchema.Location = new System.Drawing.Point(135, 55);
            this.txtImageSchema.Name = "txtImageSchema";
            this.txtImageSchema.Size = new System.Drawing.Size(197, 22);
            this.txtImageSchema.TabIndex = 3;
            this.txtImageSchema.Text = "%artist%_%md5%";
            this.JustTheTips.SetToolTip(this.txtImageSchema, resources.GetString("txtImageSchema.ToolTip"));
            this.txtImageSchema.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtImageSchema_KeyPress);
            // 
            // txtTagSchema
            // 
            this.txtTagSchema.Location = new System.Drawing.Point(135, 7);
            this.txtTagSchema.Name = "txtTagSchema";
            this.txtTagSchema.Size = new System.Drawing.Size(197, 22);
            this.txtTagSchema.TabIndex = 1;
            this.txtTagSchema.Text = "%md5%";
            this.JustTheTips.SetToolTip(this.txtTagSchema, resources.GetString("txtTagSchema.ToolTip"));
            this.txtTagSchema.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTagSchema_KeyPress);
            // 
            // lbImageSchema
            // 
            this.lbImageSchema.AutoSize = true;
            this.lbImageSchema.Location = new System.Drawing.Point(24, 58);
            this.lbImageSchema.Name = "lbImageSchema";
            this.lbImageSchema.Size = new System.Drawing.Size(110, 13);
            this.lbImageSchema.TabIndex = 13;
            this.lbImageSchema.Text = "Image name schema";
            // 
            // txtPoolSchema
            // 
            this.txtPoolSchema.Location = new System.Drawing.Point(135, 31);
            this.txtPoolSchema.Name = "txtPoolSchema";
            this.txtPoolSchema.Size = new System.Drawing.Size(197, 22);
            this.txtPoolSchema.TabIndex = 2;
            this.txtPoolSchema.Text = "%poolname%_%page%";
            this.JustTheTips.SetToolTip(this.txtPoolSchema, resources.GetString("txtPoolSchema.ToolTip"));
            this.txtPoolSchema.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPoolSchema_KeyPress);
            // 
            // btnBlacklist
            // 
            this.btnBlacklist.Location = new System.Drawing.Point(16, 230);
            this.btnBlacklist.Name = "btnBlacklist";
            this.btnBlacklist.Size = new System.Drawing.Size(102, 23);
            this.btnBlacklist.TabIndex = 7;
            this.btnBlacklist.Text = "Manage blacklist";
            this.JustTheTips.SetToolTip(this.btnBlacklist, "Manage your blacklist");
            this.btnBlacklist.UseVisualStyleBackColor = true;
            this.btnBlacklist.Click += new System.EventHandler(this.btnBlacklist_Click);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(408, 263);
            this.Controls.Add(this.btnBlacklist);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tbMain);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(426, 300);
            this.MinimumSize = new System.Drawing.Size(426, 300);
            this.Name = "frmSettings";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "aphrodite settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.tbMain.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.gbMetadata.ResumeLayout(false);
            this.gbMetadata.PerformLayout();
            this.tabTags.ResumeLayout(false);
            this.tabTags.PerformLayout();
            this.gbTagDownloadLimit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numTagsDownloadLimit)).EndInit();
            this.gbTagsRatings.ResumeLayout(false);
            this.gbTagsRatings.PerformLayout();
            this.gbTagsScoreLimit.ResumeLayout(false);
            this.gbTagsScoreLimit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsScoreLimit)).EndInit();
            this.gbTagsPageLimit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numTagsPageLimit)).EndInit();
            this.tabPools.ResumeLayout(false);
            this.tabPools.PerformLayout();
            this.tabImages.ResumeLayout(false);
            this.tabImages.PerformLayout();
            this.tabProtocol.ResumeLayout(false);
            this.tabPortable.ResumeLayout(false);
            this.tabSchemas.ResumeLayout(false);
            this.tabSchemas.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabTags;
        private System.Windows.Forms.TabPage tabPools;
        private System.Windows.Forms.Button btnBrowseForSaveTo;
        private System.Windows.Forms.TextBox txtSaveTo;
        private System.Windows.Forms.Label lbSaveTo;
        private System.Windows.Forms.CheckBox chkSaveInfoFiles;
        private System.Windows.Forms.Button btnBlacklist;
        private System.Windows.Forms.CheckBox chkSaveBlacklistedImages;
        private System.Windows.Forms.CheckBox chkTagsSeparateRatings;
        private System.Windows.Forms.CheckBox chkTagsSafe;
        private System.Windows.Forms.CheckBox chkTagsQuestionable;
        private System.Windows.Forms.CheckBox chkTagsExplicit;
        private System.Windows.Forms.CheckBox chkTagsEnableScoreLimit;
        private System.Windows.Forms.NumericUpDown numTagsScoreLimit;
        private System.Windows.Forms.NumericUpDown numTagsDownloadLimit;
        private System.Windows.Forms.CheckBox chkPoolsMergeBlacklistedImages;
        private System.Windows.Forms.CheckBox chkPoolsOpenAfterDownload;
        private System.Windows.Forms.TabPage tabProtocol;
        private System.Windows.Forms.Button btnProtocolInstallTags;
        private System.Windows.Forms.Button btnProtocolInstallPools;
        private System.Windows.Forms.Button btnProtocolInstallImages;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnProtocolImagesUserscript;
        private System.Windows.Forms.Button btnProtocolUserscript;
        private System.Windows.Forms.Label lbProtocols;
        private System.Windows.Forms.TabPage tabImages;
        private System.Windows.Forms.ToolTip JustTheTips;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbImagesHeader;
        private System.Windows.Forms.CheckBox chkIgnoreFinish;
        private System.Windows.Forms.CheckBox chkImagesSeparateRatings;
        private System.Windows.Forms.CheckBox chkImagesSeparateBlacklisted;
        private System.Windows.Forms.CheckBox chkImagesUseForm;
        private System.Windows.Forms.CheckBox chkPoolsAddToWishlistSilently;
        private System.Windows.Forms.NumericUpDown numTagsPageLimit;
        private System.Windows.Forms.GroupBox gbTagsScoreLimit;
        private System.Windows.Forms.GroupBox gbTagsPageLimit;
        private System.Windows.Forms.GroupBox gbTagDownloadLimit;
        private System.Windows.Forms.GroupBox gbTagsRatings;
        private System.Windows.Forms.CheckBox chkTagsIncludeScoreAsTag;
        private System.Windows.Forms.TabPage tabPortable;
        private System.Windows.Forms.Button btnPortableExportIni;
        private System.Windows.Forms.Label lbPortable;
        private System.Windows.Forms.Label lbImageSchema;
        private System.Windows.Forms.TextBox txtImageSchema;
        private System.Windows.Forms.CheckBox chkImagesSeparateArtists;
        private System.Windows.Forms.Label lblTagSchema;
        private System.Windows.Forms.TextBox txtTagSchema;
        private System.Windows.Forms.Label lbPoolSchema;
        private System.Windows.Forms.TextBox txtPoolSchema;
        private System.Windows.Forms.TabPage tabSchemas;
        private System.Windows.Forms.Label lbSchemaParam;
        private System.Windows.Forms.RichTextBox rtbParams;
        private System.Windows.Forms.Label lbTagExt;
        private System.Windows.Forms.Label lbImageExt;
        private System.Windows.Forms.Label lbPoolExt;
        private System.Windows.Forms.Button btnSchemaUndesiredTags;
        private System.Windows.Forms.GroupBox gbMetadata;
        private System.Windows.Forms.CheckBox chkSaveTagMetadata;
        private System.Windows.Forms.CheckBox chkSaveArtistMetadata;
        private System.Windows.Forms.CheckBox chkSaveMetadata;
        private System.Windows.Forms.CheckBox chkTagsSeparateNonImages;
        private System.Windows.Forms.CheckBox chkImagesSeparateNonImages;

    }
}