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
            this.tbGeneral = new System.Windows.Forms.TabPage();
            this.gbMetadata = new System.Windows.Forms.GroupBox();
            this.chkSaveTagMetadata = new System.Windows.Forms.CheckBox();
            this.chkSaveArtistMetadata = new System.Windows.Forms.CheckBox();
            this.chkSaveMetadata = new System.Windows.Forms.CheckBox();
            this.chkIgnoreFinish = new System.Windows.Forms.CheckBox();
            this.chkSaveBlacklisted = new System.Windows.Forms.CheckBox();
            this.chkSaveInfo = new System.Windows.Forms.CheckBox();
            this.btnBrws = new System.Windows.Forms.Button();
            this.txtSaveTo = new System.Windows.Forms.TextBox();
            this.lbSaveTo = new System.Windows.Forms.Label();
            this.tbTags = new System.Windows.Forms.TabPage();
            this.chkSkipExistingFiles = new System.Windows.Forms.CheckBox();
            this.gbTagDownloadLimit = new System.Windows.Forms.GroupBox();
            this.numLimit = new System.Windows.Forms.NumericUpDown();
            this.gbTagRatings = new System.Windows.Forms.GroupBox();
            this.chkExplicit = new System.Windows.Forms.CheckBox();
            this.chkSeparate = new System.Windows.Forms.CheckBox();
            this.chkQuestionable = new System.Windows.Forms.CheckBox();
            this.chkSafe = new System.Windows.Forms.CheckBox();
            this.gbTagScoreLimit = new System.Windows.Forms.GroupBox();
            this.chkScoreAsTag = new System.Windows.Forms.CheckBox();
            this.numScore = new System.Windows.Forms.NumericUpDown();
            this.chkMinimumScore = new System.Windows.Forms.CheckBox();
            this.gbTagPageLimit = new System.Windows.Forms.GroupBox();
            this.numPageLimit = new System.Windows.Forms.NumericUpDown();
            this.tbPools = new System.Windows.Forms.TabPage();
            this.chkAddWishlistSilent = new System.Windows.Forms.CheckBox();
            this.chkOpen = new System.Windows.Forms.CheckBox();
            this.chkMerge = new System.Windows.Forms.CheckBox();
            this.chkPoolName = new System.Windows.Forms.CheckBox();
            this.tbImages = new System.Windows.Forms.TabPage();
            this.chkUseForm = new System.Windows.Forms.CheckBox();
            this.chkSepArtists = new System.Windows.Forms.CheckBox();
            this.chkSeparateBlacklisted = new System.Windows.Forms.CheckBox();
            this.chkSeparateImages = new System.Windows.Forms.CheckBox();
            this.rbMD5 = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.rbArtist = new System.Windows.Forms.RadioButton();
            this.tbProtocol = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.btnImagesUserscript = new System.Windows.Forms.Button();
            this.btnUserscript = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnTagsProtocol = new System.Windows.Forms.Button();
            this.btnPoolsProtocol = new System.Windows.Forms.Button();
            this.btnImagesProtocol = new System.Windows.Forms.Button();
            this.tbPortable = new System.Windows.Forms.TabPage();
            this.btnExportIni = new System.Windows.Forms.Button();
            this.lbPortable = new System.Windows.Forms.Label();
            this.tbSchemas = new System.Windows.Forms.TabPage();
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
            this.tbGeneral.SuspendLayout();
            this.gbMetadata.SuspendLayout();
            this.tbTags.SuspendLayout();
            this.gbTagDownloadLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLimit)).BeginInit();
            this.gbTagRatings.SuspendLayout();
            this.gbTagScoreLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScore)).BeginInit();
            this.gbTagPageLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPageLimit)).BeginInit();
            this.tbPools.SuspendLayout();
            this.tbImages.SuspendLayout();
            this.tbProtocol.SuspendLayout();
            this.tbPortable.SuspendLayout();
            this.tbSchemas.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(331, 229);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(250, 229);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbMain
            // 
            this.tbMain.Controls.Add(this.tbGeneral);
            this.tbMain.Controls.Add(this.tbTags);
            this.tbMain.Controls.Add(this.tbPools);
            this.tbMain.Controls.Add(this.tbImages);
            this.tbMain.Controls.Add(this.tbProtocol);
            this.tbMain.Controls.Add(this.tbPortable);
            this.tbMain.Controls.Add(this.tbSchemas);
            this.tbMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbMain.Location = new System.Drawing.Point(0, 0);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(410, 223);
            this.tbMain.TabIndex = 10;
            // 
            // tbGeneral
            // 
            this.tbGeneral.Controls.Add(this.gbMetadata);
            this.tbGeneral.Controls.Add(this.chkIgnoreFinish);
            this.tbGeneral.Controls.Add(this.chkSaveBlacklisted);
            this.tbGeneral.Controls.Add(this.chkSaveInfo);
            this.tbGeneral.Controls.Add(this.btnBrws);
            this.tbGeneral.Controls.Add(this.txtSaveTo);
            this.tbGeneral.Controls.Add(this.lbSaveTo);
            this.tbGeneral.Location = new System.Drawing.Point(4, 22);
            this.tbGeneral.Name = "tbGeneral";
            this.tbGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tbGeneral.Size = new System.Drawing.Size(402, 197);
            this.tbGeneral.TabIndex = 0;
            this.tbGeneral.Text = "General";
            this.tbGeneral.UseVisualStyleBackColor = true;
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
            // 
            // chkSaveTagMetadata
            // 
            this.chkSaveTagMetadata.AutoSize = true;
            this.chkSaveTagMetadata.Checked = true;
            this.chkSaveTagMetadata.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveTagMetadata.Location = new System.Drawing.Point(8, 65);
            this.chkSaveTagMetadata.Name = "chkSaveTagMetadata";
            this.chkSaveTagMetadata.Size = new System.Drawing.Size(133, 17);
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
            this.chkSaveArtistMetadata.Size = new System.Drawing.Size(146, 17);
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
            this.chkSaveMetadata.Size = new System.Drawing.Size(156, 17);
            this.chkSaveMetadata.TabIndex = 6;
            this.chkSaveMetadata.Text = "Save metadata with images";
            this.JustTheTips.SetToolTip(this.chkSaveMetadata, "Save metadata to the image file when downloading");
            this.chkSaveMetadata.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreFinish
            // 
            this.chkIgnoreFinish.AutoSize = true;
            this.chkIgnoreFinish.Location = new System.Drawing.Point(23, 145);
            this.chkIgnoreFinish.Name = "chkIgnoreFinish";
            this.chkIgnoreFinish.Size = new System.Drawing.Size(172, 17);
            this.chkIgnoreFinish.TabIndex = 4;
            this.chkIgnoreFinish.Text = "Don\'t notify finished downloads";
            this.JustTheTips.SetToolTip(this.chkIgnoreFinish, "Doesn\'t notify you when downloads are completed\r\nPlugin downloads quit when finis" +
        "hed");
            this.chkIgnoreFinish.UseVisualStyleBackColor = true;
            // 
            // chkSaveBlacklisted
            // 
            this.chkSaveBlacklisted.AutoSize = true;
            this.chkSaveBlacklisted.Checked = true;
            this.chkSaveBlacklisted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveBlacklisted.Location = new System.Drawing.Point(24, 122);
            this.chkSaveBlacklisted.Name = "chkSaveBlacklisted";
            this.chkSaveBlacklisted.Size = new System.Drawing.Size(140, 17);
            this.chkSaveBlacklisted.TabIndex = 3;
            this.chkSaveBlacklisted.Text = "Save blacklisted images";
            this.JustTheTips.SetToolTip(this.chkSaveBlacklisted, "Saves blacklisted images into a separate folder.\r\nImages downloaded with the \'ima" +
        "ges\' protocol are also saved + separated.");
            this.chkSaveBlacklisted.UseVisualStyleBackColor = true;
            // 
            // chkSaveInfo
            // 
            this.chkSaveInfo.AutoSize = true;
            this.chkSaveInfo.Checked = true;
            this.chkSaveInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveInfo.Location = new System.Drawing.Point(23, 99);
            this.chkSaveInfo.Name = "chkSaveInfo";
            this.chkSaveInfo.Size = new System.Drawing.Size(123, 17);
            this.chkSaveInfo.TabIndex = 2;
            this.chkSaveInfo.Text = "Save image info files";
            this.JustTheTips.SetToolTip(this.chkSaveInfo, resources.GetString("chkSaveInfo.ToolTip"));
            this.chkSaveInfo.UseVisualStyleBackColor = true;
            // 
            // btnBrws
            // 
            this.btnBrws.Location = new System.Drawing.Point(362, 36);
            this.btnBrws.Name = "btnBrws";
            this.btnBrws.Size = new System.Drawing.Size(24, 23);
            this.btnBrws.TabIndex = 1;
            this.btnBrws.Text = "...";
            this.btnBrws.UseVisualStyleBackColor = true;
            this.btnBrws.Click += new System.EventHandler(this.btnBrws_Click);
            // 
            // txtSaveTo
            // 
            this.txtSaveTo.Location = new System.Drawing.Point(33, 39);
            this.txtSaveTo.Name = "txtSaveTo";
            this.txtSaveTo.ReadOnly = true;
            this.txtSaveTo.Size = new System.Drawing.Size(323, 20);
            this.txtSaveTo.TabIndex = 0;
            this.JustTheTips.SetToolTip(this.txtSaveTo, "The location where pools, tags, and images will be saved to.");
            // 
            // lbSaveTo
            // 
            this.lbSaveTo.AutoSize = true;
            this.lbSaveTo.Location = new System.Drawing.Point(8, 21);
            this.lbSaveTo.Name = "lbSaveTo";
            this.lbSaveTo.Size = new System.Drawing.Size(47, 13);
            this.lbSaveTo.TabIndex = 17;
            this.lbSaveTo.Text = "Save to:";
            // 
            // tbTags
            // 
            this.tbTags.Controls.Add(this.chkSkipExistingFiles);
            this.tbTags.Controls.Add(this.gbTagDownloadLimit);
            this.tbTags.Controls.Add(this.gbTagRatings);
            this.tbTags.Controls.Add(this.gbTagScoreLimit);
            this.tbTags.Controls.Add(this.gbTagPageLimit);
            this.tbTags.Location = new System.Drawing.Point(4, 22);
            this.tbTags.Name = "tbTags";
            this.tbTags.Padding = new System.Windows.Forms.Padding(3);
            this.tbTags.Size = new System.Drawing.Size(410, 197);
            this.tbTags.TabIndex = 1;
            this.tbTags.Text = "Tags";
            this.tbTags.UseVisualStyleBackColor = true;
            // 
            // chkSkipExistingFiles
            // 
            this.chkSkipExistingFiles.AutoSize = true;
            this.chkSkipExistingFiles.Location = new System.Drawing.Point(74, 156);
            this.chkSkipExistingFiles.Name = "chkSkipExistingFiles";
            this.chkSkipExistingFiles.Size = new System.Drawing.Size(106, 17);
            this.chkSkipExistingFiles.TabIndex = 5;
            this.chkSkipExistingFiles.Text = "Skip existing files";
            this.JustTheTips.SetToolTip(this.chkSkipExistingFiles, "Enable this option to skip adding existing files to the internal download list.");
            this.chkSkipExistingFiles.UseVisualStyleBackColor = true;
            this.chkSkipExistingFiles.Visible = false;
            // 
            // gbTagDownloadLimit
            // 
            this.gbTagDownloadLimit.Controls.Add(this.numLimit);
            this.gbTagDownloadLimit.Location = new System.Drawing.Point(210, 111);
            this.gbTagDownloadLimit.Name = "gbTagDownloadLimit";
            this.gbTagDownloadLimit.Size = new System.Drawing.Size(143, 57);
            this.gbTagDownloadLimit.TabIndex = 4;
            this.gbTagDownloadLimit.TabStop = false;
            this.gbTagDownloadLimit.Text = "Download limit (0 = off)";
            // 
            // numLimit
            // 
            this.numLimit.Location = new System.Drawing.Point(31, 23);
            this.numLimit.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numLimit.Name = "numLimit";
            this.numLimit.Size = new System.Drawing.Size(81, 20);
            this.numLimit.TabIndex = 1;
            this.numLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.JustTheTips.SetToolTip(this.numLimit, "Limits downloads to a certain amount of images");
            // 
            // gbTagRatings
            // 
            this.gbTagRatings.Controls.Add(this.chkExplicit);
            this.gbTagRatings.Controls.Add(this.chkSeparate);
            this.gbTagRatings.Controls.Add(this.chkQuestionable);
            this.gbTagRatings.Controls.Add(this.chkSafe);
            this.gbTagRatings.Location = new System.Drawing.Point(57, 28);
            this.gbTagRatings.Name = "gbTagRatings";
            this.gbTagRatings.Size = new System.Drawing.Size(143, 67);
            this.gbTagRatings.TabIndex = 1;
            this.gbTagRatings.TabStop = false;
            this.gbTagRatings.Text = "Ratings";
            // 
            // chkExplicit
            // 
            this.chkExplicit.AutoSize = true;
            this.chkExplicit.Checked = true;
            this.chkExplicit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExplicit.Location = new System.Drawing.Point(17, 19);
            this.chkExplicit.Name = "chkExplicit";
            this.chkExplicit.Size = new System.Drawing.Size(33, 17);
            this.chkExplicit.TabIndex = 1;
            this.chkExplicit.Text = "E";
            this.JustTheTips.SetToolTip(this.chkExplicit, "Download images rated Explicit");
            this.chkExplicit.UseVisualStyleBackColor = true;
            // 
            // chkSeparate
            // 
            this.chkSeparate.AutoSize = true;
            this.chkSeparate.Checked = true;
            this.chkSeparate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSeparate.Location = new System.Drawing.Point(20, 42);
            this.chkSeparate.Name = "chkSeparate";
            this.chkSeparate.Size = new System.Drawing.Size(103, 17);
            this.chkSeparate.TabIndex = 4;
            this.chkSeparate.Text = "Separate ratings";
            this.JustTheTips.SetToolTip(this.chkSeparate, "Separates ratings into different folders");
            this.chkSeparate.UseVisualStyleBackColor = true;
            // 
            // chkQuestionable
            // 
            this.chkQuestionable.AutoSize = true;
            this.chkQuestionable.Checked = true;
            this.chkQuestionable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkQuestionable.Location = new System.Drawing.Point(55, 18);
            this.chkQuestionable.Name = "chkQuestionable";
            this.chkQuestionable.Size = new System.Drawing.Size(34, 17);
            this.chkQuestionable.TabIndex = 2;
            this.chkQuestionable.Text = "Q";
            this.JustTheTips.SetToolTip(this.chkQuestionable, "Download images rated Questionable");
            this.chkQuestionable.UseVisualStyleBackColor = true;
            // 
            // chkSafe
            // 
            this.chkSafe.AutoSize = true;
            this.chkSafe.Checked = true;
            this.chkSafe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSafe.Location = new System.Drawing.Point(94, 18);
            this.chkSafe.Name = "chkSafe";
            this.chkSafe.Size = new System.Drawing.Size(33, 17);
            this.chkSafe.TabIndex = 3;
            this.chkSafe.Text = "S";
            this.JustTheTips.SetToolTip(this.chkSafe, "Download images rated Safe");
            this.chkSafe.UseVisualStyleBackColor = true;
            // 
            // gbTagScoreLimit
            // 
            this.gbTagScoreLimit.Controls.Add(this.chkScoreAsTag);
            this.gbTagScoreLimit.Controls.Add(this.numScore);
            this.gbTagScoreLimit.Controls.Add(this.chkMinimumScore);
            this.gbTagScoreLimit.Location = new System.Drawing.Point(210, 28);
            this.gbTagScoreLimit.Name = "gbTagScoreLimit";
            this.gbTagScoreLimit.Size = new System.Drawing.Size(143, 82);
            this.gbTagScoreLimit.TabIndex = 2;
            this.gbTagScoreLimit.TabStop = false;
            this.gbTagScoreLimit.Text = "Score limit";
            // 
            // chkScoreAsTag
            // 
            this.chkScoreAsTag.AutoSize = true;
            this.chkScoreAsTag.Enabled = false;
            this.chkScoreAsTag.Location = new System.Drawing.Point(19, 60);
            this.chkScoreAsTag.Name = "chkScoreAsTag";
            this.chkScoreAsTag.Size = new System.Drawing.Size(106, 17);
            this.chkScoreAsTag.TabIndex = 3;
            this.chkScoreAsTag.Text = "Include with tags";
            this.JustTheTips.SetToolTip(this.chkScoreAsTag, "If checked, uses the score minimum as a tag (ex: \"gay score:>25\").\r\nThis widly in" +
        "creases the images that will be downloaded.\r\nOnly used if there are 5 or less ta" +
        "gs being queried");
            this.chkScoreAsTag.UseVisualStyleBackColor = true;
            // 
            // numScore
            // 
            this.numScore.Enabled = false;
            this.numScore.Location = new System.Drawing.Point(40, 19);
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
            this.numScore.TabIndex = 1;
            this.numScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.JustTheTips.SetToolTip(this.numScore, "The minimum score that will be downloaded");
            // 
            // chkMinimumScore
            // 
            this.chkMinimumScore.AutoSize = true;
            this.chkMinimumScore.Location = new System.Drawing.Point(18, 43);
            this.chkMinimumScore.Name = "chkMinimumScore";
            this.chkMinimumScore.Size = new System.Drawing.Size(108, 17);
            this.chkMinimumScore.TabIndex = 2;
            this.chkMinimumScore.Text = "Enable score limit";
            this.JustTheTips.SetToolTip(this.chkMinimumScore, "Only downloads images with a score equal to or greater than provided");
            this.chkMinimumScore.UseVisualStyleBackColor = true;
            this.chkMinimumScore.CheckedChanged += new System.EventHandler(this.chkMinimumScore_CheckedChanged);
            // 
            // gbTagPageLimit
            // 
            this.gbTagPageLimit.Controls.Add(this.numPageLimit);
            this.gbTagPageLimit.Location = new System.Drawing.Point(57, 101);
            this.gbTagPageLimit.Name = "gbTagPageLimit";
            this.gbTagPageLimit.Size = new System.Drawing.Size(143, 49);
            this.gbTagPageLimit.TabIndex = 3;
            this.gbTagPageLimit.TabStop = false;
            this.gbTagPageLimit.Text = "Page limit (0 = off)";
            // 
            // numPageLimit
            // 
            this.numPageLimit.Location = new System.Drawing.Point(40, 19);
            this.numPageLimit.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numPageLimit.Name = "numPageLimit";
            this.numPageLimit.Size = new System.Drawing.Size(63, 20);
            this.numPageLimit.TabIndex = 1;
            this.numPageLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.JustTheTips.SetToolTip(this.numPageLimit, "The amount of pages that will be downloaded (320 images per page)");
            // 
            // tbPools
            // 
            this.tbPools.Controls.Add(this.chkAddWishlistSilent);
            this.tbPools.Controls.Add(this.chkOpen);
            this.tbPools.Controls.Add(this.chkMerge);
            this.tbPools.Controls.Add(this.chkPoolName);
            this.tbPools.Location = new System.Drawing.Point(4, 22);
            this.tbPools.Name = "tbPools";
            this.tbPools.Padding = new System.Windows.Forms.Padding(3);
            this.tbPools.Size = new System.Drawing.Size(410, 197);
            this.tbPools.TabIndex = 2;
            this.tbPools.Text = "Pools";
            this.tbPools.UseVisualStyleBackColor = true;
            // 
            // chkAddWishlistSilent
            // 
            this.chkAddWishlistSilent.AutoSize = true;
            this.chkAddWishlistSilent.Location = new System.Drawing.Point(67, 118);
            this.chkAddWishlistSilent.Name = "chkAddWishlistSilent";
            this.chkAddWishlistSilent.Size = new System.Drawing.Size(155, 17);
            this.chkAddWishlistSilent.TabIndex = 4;
            this.chkAddWishlistSilent.Text = "Add pools to wishlist silently";
            this.JustTheTips.SetToolTip(this.chkAddWishlistSilent, "Add the pool to your wishlist without showing the application.");
            this.chkAddWishlistSilent.UseVisualStyleBackColor = true;
            // 
            // chkOpen
            // 
            this.chkOpen.AutoSize = true;
            this.chkOpen.Location = new System.Drawing.Point(67, 84);
            this.chkOpen.Name = "chkOpen";
            this.chkOpen.Size = new System.Drawing.Size(139, 17);
            this.chkOpen.TabIndex = 3;
            this.chkOpen.Text = "Open after downloading";
            this.JustTheTips.SetToolTip(this.chkOpen, "Opens the pool folder after downloading");
            this.chkOpen.UseVisualStyleBackColor = true;
            // 
            // chkMerge
            // 
            this.chkMerge.AutoSize = true;
            this.chkMerge.Checked = true;
            this.chkMerge.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMerge.Location = new System.Drawing.Point(67, 61);
            this.chkMerge.Name = "chkMerge";
            this.chkMerge.Size = new System.Drawing.Size(277, 17);
            this.chkMerge.TabIndex = 2;
            this.chkMerge.Text = "Merge blacklisted images with non blacklisted images";
            this.JustTheTips.SetToolTip(this.chkMerge, "Merge blacklisted images with the rest of the pool\r\npool.blacklisted.nfo is not s" +
        "aved, instead, blacklisted info is saved in pool.nfo under \"BLACKLISTED PAGE\"");
            this.chkMerge.UseVisualStyleBackColor = true;
            // 
            // chkPoolName
            // 
            this.chkPoolName.AutoSize = true;
            this.chkPoolName.Checked = true;
            this.chkPoolName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPoolName.Location = new System.Drawing.Point(67, 41);
            this.chkPoolName.Name = "chkPoolName";
            this.chkPoolName.Size = new System.Drawing.Size(179, 17);
            this.chkPoolName.TabIndex = 1;
            this.chkPoolName.Text = "Save files as \"poolname_####\"";
            this.JustTheTips.SetToolTip(this.chkPoolName, "Save pool files as the pool name + page number.\r\nEx: The_Internship_05.jpg");
            this.chkPoolName.UseVisualStyleBackColor = true;
            this.chkPoolName.Visible = false;
            // 
            // tbImages
            // 
            this.tbImages.Controls.Add(this.chkUseForm);
            this.tbImages.Controls.Add(this.chkSepArtists);
            this.tbImages.Controls.Add(this.chkSeparateBlacklisted);
            this.tbImages.Controls.Add(this.chkSeparateImages);
            this.tbImages.Controls.Add(this.rbMD5);
            this.tbImages.Controls.Add(this.label9);
            this.tbImages.Controls.Add(this.label8);
            this.tbImages.Controls.Add(this.rbArtist);
            this.tbImages.Location = new System.Drawing.Point(4, 22);
            this.tbImages.Name = "tbImages";
            this.tbImages.Padding = new System.Windows.Forms.Padding(3);
            this.tbImages.Size = new System.Drawing.Size(410, 197);
            this.tbImages.TabIndex = 4;
            this.tbImages.Text = "Images";
            this.tbImages.UseVisualStyleBackColor = true;
            // 
            // chkUseForm
            // 
            this.chkUseForm.AutoSize = true;
            this.chkUseForm.Location = new System.Drawing.Point(94, 111);
            this.chkUseForm.Name = "chkUseForm";
            this.chkUseForm.Size = new System.Drawing.Size(224, 17);
            this.chkUseForm.TabIndex = 4;
            this.chkUseForm.Text = "Use download form for download progress";
            this.JustTheTips.SetToolTip(this.chkUseForm, "Shows a form when downloading images that will report progress");
            this.chkUseForm.UseVisualStyleBackColor = true;
            // 
            // chkSepArtists
            // 
            this.chkSepArtists.AutoSize = true;
            this.chkSepArtists.Location = new System.Drawing.Point(156, 89);
            this.chkSepArtists.Name = "chkSepArtists";
            this.chkSepArtists.Size = new System.Drawing.Size(99, 17);
            this.chkSepArtists.TabIndex = 3;
            this.chkSepArtists.Text = "Separate artists";
            this.chkSepArtists.UseVisualStyleBackColor = true;
            // 
            // chkSeparateBlacklisted
            // 
            this.chkSeparateBlacklisted.AutoSize = true;
            this.chkSeparateBlacklisted.Checked = true;
            this.chkSeparateBlacklisted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSeparateBlacklisted.Location = new System.Drawing.Point(186, 69);
            this.chkSeparateBlacklisted.Name = "chkSeparateBlacklisted";
            this.chkSeparateBlacklisted.Size = new System.Drawing.Size(158, 17);
            this.chkSeparateBlacklisted.TabIndex = 2;
            this.chkSeparateBlacklisted.Text = "Separate blacklisted images";
            this.JustTheTips.SetToolTip(this.chkSeparateBlacklisted, "Separates blacklisted images into separate folder.");
            this.chkSeparateBlacklisted.UseVisualStyleBackColor = true;
            // 
            // chkSeparateImages
            // 
            this.chkSeparateImages.AutoSize = true;
            this.chkSeparateImages.Checked = true;
            this.chkSeparateImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSeparateImages.Location = new System.Drawing.Point(67, 69);
            this.chkSeparateImages.Name = "chkSeparateImages";
            this.chkSeparateImages.Size = new System.Drawing.Size(103, 17);
            this.chkSeparateImages.TabIndex = 1;
            this.chkSeparateImages.Text = "Separate ratings";
            this.JustTheTips.SetToolTip(this.chkSeparateImages, "Separate ratings into separate folders for images");
            this.chkSeparateImages.UseVisualStyleBackColor = true;
            // 
            // rbMD5
            // 
            this.rbMD5.AutoSize = true;
            this.rbMD5.Location = new System.Drawing.Point(222, 174);
            this.rbMD5.Name = "rbMD5";
            this.rbMD5.Size = new System.Drawing.Size(127, 17);
            this.rbMD5.TabIndex = 8;
            this.rbMD5.Text = "Save images as \'md5\'";
            this.JustTheTips.SetToolTip(this.rbMD5, "Saves files as the MD5 hash of the image\r\nex: 7ec39fcc0afe7b237d61b1afdfb9b927.jp" +
        "g");
            this.rbMD5.UseVisualStyleBackColor = true;
            this.rbMD5.Visible = false;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label9.Location = new System.Drawing.Point(59, 32);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(292, 2);
            this.label9.TabIndex = 7;
            this.label9.Text = "bark";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(63, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(285, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "These options only effect the \'images:\' protocol + userscript";
            // 
            // rbArtist
            // 
            this.rbArtist.AutoSize = true;
            this.rbArtist.Checked = true;
            this.rbArtist.Location = new System.Drawing.Point(62, 174);
            this.rbArtist.Name = "rbArtist";
            this.rbArtist.Size = new System.Drawing.Size(155, 17);
            this.rbArtist.TabIndex = 0;
            this.rbArtist.TabStop = true;
            this.rbArtist.Text = "Save images as \'artist_md5\'";
            this.JustTheTips.SetToolTip(this.rbArtist, "Saves images as the artist and the MD5 hash of the file.\r\nex: stigmata_7ec39fcc0a" +
        "fe7b237d61b1afdfb9b927.jpg");
            this.rbArtist.UseVisualStyleBackColor = true;
            this.rbArtist.Visible = false;
            // 
            // tbProtocol
            // 
            this.tbProtocol.Controls.Add(this.label7);
            this.tbProtocol.Controls.Add(this.btnImagesUserscript);
            this.tbProtocol.Controls.Add(this.btnUserscript);
            this.tbProtocol.Controls.Add(this.label5);
            this.tbProtocol.Controls.Add(this.btnTagsProtocol);
            this.tbProtocol.Controls.Add(this.btnPoolsProtocol);
            this.tbProtocol.Controls.Add(this.btnImagesProtocol);
            this.tbProtocol.Location = new System.Drawing.Point(4, 22);
            this.tbProtocol.Name = "tbProtocol";
            this.tbProtocol.Padding = new System.Windows.Forms.Padding(3);
            this.tbProtocol.Size = new System.Drawing.Size(410, 197);
            this.tbProtocol.TabIndex = 3;
            this.tbProtocol.Text = "Protocols";
            this.tbProtocol.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Location = new System.Drawing.Point(59, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(292, 2);
            this.label7.TabIndex = 6;
            this.label7.Text = "bark";
            // 
            // btnImagesUserscript
            // 
            this.btnImagesUserscript.Location = new System.Drawing.Point(246, 149);
            this.btnImagesUserscript.Name = "btnImagesUserscript";
            this.btnImagesUserscript.Size = new System.Drawing.Size(75, 23);
            this.btnImagesUserscript.TabIndex = 5;
            this.btnImagesUserscript.Text = "Userscript";
            this.btnImagesUserscript.UseVisualStyleBackColor = true;
            this.btnImagesUserscript.Click += new System.EventHandler(this.btnImagesUserscript_Click);
            // 
            // btnUserscript
            // 
            this.btnUserscript.Location = new System.Drawing.Point(246, 96);
            this.btnUserscript.Name = "btnUserscript";
            this.btnUserscript.Size = new System.Drawing.Size(75, 23);
            this.btnUserscript.TabIndex = 3;
            this.btnUserscript.Text = "Userscript";
            this.btnUserscript.UseVisualStyleBackColor = true;
            this.btnUserscript.Click += new System.EventHandler(this.btnUserscript_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(374, 52);
            this.label5.TabIndex = 3;
            this.label5.Text = resources.GetString("label5.Text");
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnTagsProtocol
            // 
            this.btnTagsProtocol.Location = new System.Drawing.Point(89, 84);
            this.btnTagsProtocol.Name = "btnTagsProtocol";
            this.btnTagsProtocol.Size = new System.Drawing.Size(151, 23);
            this.btnTagsProtocol.TabIndex = 1;
            this.btnTagsProtocol.Text = "Install \'tags:\' protocol";
            this.btnTagsProtocol.UseVisualStyleBackColor = true;
            this.btnTagsProtocol.Click += new System.EventHandler(this.btnTagsProtocol_Click);
            // 
            // btnPoolsProtocol
            // 
            this.btnPoolsProtocol.Location = new System.Drawing.Point(89, 109);
            this.btnPoolsProtocol.Name = "btnPoolsProtocol";
            this.btnPoolsProtocol.Size = new System.Drawing.Size(151, 23);
            this.btnPoolsProtocol.TabIndex = 2;
            this.btnPoolsProtocol.Text = "Install pool protocols";
            this.btnPoolsProtocol.UseVisualStyleBackColor = true;
            this.btnPoolsProtocol.Click += new System.EventHandler(this.btnPoolsProtocol_Click);
            // 
            // btnImagesProtocol
            // 
            this.btnImagesProtocol.Location = new System.Drawing.Point(89, 149);
            this.btnImagesProtocol.Name = "btnImagesProtocol";
            this.btnImagesProtocol.Size = new System.Drawing.Size(151, 23);
            this.btnImagesProtocol.TabIndex = 4;
            this.btnImagesProtocol.Text = "Install \'image:\' protocol";
            this.btnImagesProtocol.UseVisualStyleBackColor = true;
            this.btnImagesProtocol.Click += new System.EventHandler(this.btnImagesProtocol_Click);
            // 
            // tbPortable
            // 
            this.tbPortable.Controls.Add(this.btnExportIni);
            this.tbPortable.Controls.Add(this.lbPortable);
            this.tbPortable.Location = new System.Drawing.Point(4, 22);
            this.tbPortable.Name = "tbPortable";
            this.tbPortable.Padding = new System.Windows.Forms.Padding(3);
            this.tbPortable.Size = new System.Drawing.Size(410, 197);
            this.tbPortable.TabIndex = 5;
            this.tbPortable.Text = "Portable";
            this.tbPortable.UseVisualStyleBackColor = true;
            // 
            // btnExportIni
            // 
            this.btnExportIni.Location = new System.Drawing.Point(162, 157);
            this.btnExportIni.Name = "btnExportIni";
            this.btnExportIni.Size = new System.Drawing.Size(86, 24);
            this.btnExportIni.TabIndex = 1;
            this.btnExportIni.Text = "Export";
            this.btnExportIni.UseVisualStyleBackColor = true;
            this.btnExportIni.Click += new System.EventHandler(this.btnExportIni_Click);
            // 
            // lbPortable
            // 
            this.lbPortable.Location = new System.Drawing.Point(8, 16);
            this.lbPortable.Name = "lbPortable";
            this.lbPortable.Size = new System.Drawing.Size(394, 133);
            this.lbPortable.TabIndex = 0;
            this.lbPortable.Text = resources.GetString("lbPortable.Text");
            this.lbPortable.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbSchemas
            // 
            this.tbSchemas.Controls.Add(this.btnSchemaUndesiredTags);
            this.tbSchemas.Controls.Add(this.lbImageExt);
            this.tbSchemas.Controls.Add(this.lbPoolExt);
            this.tbSchemas.Controls.Add(this.lbTagExt);
            this.tbSchemas.Controls.Add(this.rtbParams);
            this.tbSchemas.Controls.Add(this.lbSchemaParam);
            this.tbSchemas.Controls.Add(this.lblTagSchema);
            this.tbSchemas.Controls.Add(this.lbPoolSchema);
            this.tbSchemas.Controls.Add(this.txtImageSchema);
            this.tbSchemas.Controls.Add(this.txtTagSchema);
            this.tbSchemas.Controls.Add(this.lbImageSchema);
            this.tbSchemas.Controls.Add(this.txtPoolSchema);
            this.tbSchemas.Location = new System.Drawing.Point(4, 22);
            this.tbSchemas.Name = "tbSchemas";
            this.tbSchemas.Padding = new System.Windows.Forms.Padding(3);
            this.tbSchemas.Size = new System.Drawing.Size(410, 197);
            this.tbSchemas.TabIndex = 6;
            this.tbSchemas.Text = "Schemas";
            this.tbSchemas.UseVisualStyleBackColor = true;
            // 
            // btnSchemaUndesiredTags
            // 
            this.btnSchemaUndesiredTags.Enabled = false;
            this.btnSchemaUndesiredTags.Location = new System.Drawing.Point(272, 84);
            this.btnSchemaUndesiredTags.Name = "btnSchemaUndesiredTags";
            this.btnSchemaUndesiredTags.Size = new System.Drawing.Size(98, 23);
            this.btnSchemaUndesiredTags.TabIndex = 4;
            this.btnSchemaUndesiredTags.Text = "undesired tags...";
            this.btnSchemaUndesiredTags.UseVisualStyleBackColor = true;
            this.btnSchemaUndesiredTags.Visible = false;
            this.btnSchemaUndesiredTags.Click += new System.EventHandler(this.btnSchemaUndesiredTags_Click);
            // 
            // lbImageExt
            // 
            this.lbImageExt.AutoSize = true;
            this.lbImageExt.Location = new System.Drawing.Point(340, 61);
            this.lbImageExt.Name = "lbImageExt";
            this.lbImageExt.Size = new System.Drawing.Size(40, 13);
            this.lbImageExt.TabIndex = 51;
            this.lbImageExt.Text = ".%ext%";
            // 
            // lbPoolExt
            // 
            this.lbPoolExt.AutoSize = true;
            this.lbPoolExt.Location = new System.Drawing.Point(340, 37);
            this.lbPoolExt.Name = "lbPoolExt";
            this.lbPoolExt.Size = new System.Drawing.Size(40, 13);
            this.lbPoolExt.TabIndex = 50;
            this.lbPoolExt.Text = ".%ext%";
            // 
            // lbTagExt
            // 
            this.lbTagExt.AutoSize = true;
            this.lbTagExt.Location = new System.Drawing.Point(340, 13);
            this.lbTagExt.Name = "lbTagExt";
            this.lbTagExt.Size = new System.Drawing.Size(40, 13);
            this.lbTagExt.TabIndex = 49;
            this.lbTagExt.Text = ".%ext%";
            // 
            // rtbParams
            // 
            this.rtbParams.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbParams.Location = new System.Drawing.Point(41, 113);
            this.rtbParams.Name = "rtbParams";
            this.rtbParams.ReadOnly = true;
            this.rtbParams.Size = new System.Drawing.Size(329, 77);
            this.rtbParams.TabIndex = 5;
            this.rtbParams.Text = resources.GetString("rtbParams.Text");
            // 
            // lbSchemaParam
            // 
            this.lbSchemaParam.AutoSize = true;
            this.lbSchemaParam.Location = new System.Drawing.Point(8, 93);
            this.lbSchemaParam.Name = "lbSchemaParam";
            this.lbSchemaParam.Size = new System.Drawing.Size(111, 13);
            this.lbSchemaParam.TabIndex = 47;
            this.lbSchemaParam.Text = "Supported parameters";
            // 
            // lblTagSchema
            // 
            this.lblTagSchema.AutoSize = true;
            this.lblTagSchema.Location = new System.Drawing.Point(36, 13);
            this.lblTagSchema.Name = "lblTagSchema";
            this.lblTagSchema.Size = new System.Drawing.Size(100, 13);
            this.lblTagSchema.TabIndex = 46;
            this.lblTagSchema.Text = "Tags name schema";
            // 
            // lbPoolSchema
            // 
            this.lbPoolSchema.AutoSize = true;
            this.lbPoolSchema.Location = new System.Drawing.Point(34, 37);
            this.lbPoolSchema.Name = "lbPoolSchema";
            this.lbPoolSchema.Size = new System.Drawing.Size(102, 13);
            this.lbPoolSchema.TabIndex = 22;
            this.lbPoolSchema.Text = "Pools name schema";
            // 
            // txtImageSchema
            // 
            this.txtImageSchema.Location = new System.Drawing.Point(142, 58);
            this.txtImageSchema.Name = "txtImageSchema";
            this.txtImageSchema.Size = new System.Drawing.Size(197, 20);
            this.txtImageSchema.TabIndex = 3;
            this.txtImageSchema.Text = "%artist%_%md5%";
            this.JustTheTips.SetToolTip(this.txtImageSchema, resources.GetString("txtImageSchema.ToolTip"));
            // 
            // txtTagSchema
            // 
            this.txtTagSchema.Location = new System.Drawing.Point(142, 10);
            this.txtTagSchema.Name = "txtTagSchema";
            this.txtTagSchema.Size = new System.Drawing.Size(197, 20);
            this.txtTagSchema.TabIndex = 1;
            this.txtTagSchema.Text = "%md5%";
            this.JustTheTips.SetToolTip(this.txtTagSchema, resources.GetString("txtTagSchema.ToolTip"));
            // 
            // lbImageSchema
            // 
            this.lbImageSchema.AutoSize = true;
            this.lbImageSchema.Location = new System.Drawing.Point(31, 61);
            this.lbImageSchema.Name = "lbImageSchema";
            this.lbImageSchema.Size = new System.Drawing.Size(105, 13);
            this.lbImageSchema.TabIndex = 13;
            this.lbImageSchema.Text = "Image name schema";
            // 
            // txtPoolSchema
            // 
            this.txtPoolSchema.Location = new System.Drawing.Point(142, 34);
            this.txtPoolSchema.Name = "txtPoolSchema";
            this.txtPoolSchema.Size = new System.Drawing.Size(197, 20);
            this.txtPoolSchema.TabIndex = 2;
            this.txtPoolSchema.Text = "%poolname%_%page%";
            this.JustTheTips.SetToolTip(this.txtPoolSchema, resources.GetString("txtPoolSchema.ToolTip"));
            // 
            // btnBlacklist
            // 
            this.btnBlacklist.Location = new System.Drawing.Point(4, 229);
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
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(410, 251);
            this.Controls.Add(this.btnBlacklist);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tbMain);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(426, 290);
            this.MinimumSize = new System.Drawing.Size(426, 290);
            this.Name = "frmSettings";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "aphrodite settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.tbMain.ResumeLayout(false);
            this.tbGeneral.ResumeLayout(false);
            this.tbGeneral.PerformLayout();
            this.gbMetadata.ResumeLayout(false);
            this.gbMetadata.PerformLayout();
            this.tbTags.ResumeLayout(false);
            this.tbTags.PerformLayout();
            this.gbTagDownloadLimit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numLimit)).EndInit();
            this.gbTagRatings.ResumeLayout(false);
            this.gbTagRatings.PerformLayout();
            this.gbTagScoreLimit.ResumeLayout(false);
            this.gbTagScoreLimit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScore)).EndInit();
            this.gbTagPageLimit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numPageLimit)).EndInit();
            this.tbPools.ResumeLayout(false);
            this.tbPools.PerformLayout();
            this.tbImages.ResumeLayout(false);
            this.tbImages.PerformLayout();
            this.tbProtocol.ResumeLayout(false);
            this.tbProtocol.PerformLayout();
            this.tbPortable.ResumeLayout(false);
            this.tbSchemas.ResumeLayout(false);
            this.tbSchemas.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tbGeneral;
        private System.Windows.Forms.TabPage tbTags;
        private System.Windows.Forms.TabPage tbPools;
        private System.Windows.Forms.Button btnBrws;
        private System.Windows.Forms.TextBox txtSaveTo;
        private System.Windows.Forms.Label lbSaveTo;
        private System.Windows.Forms.CheckBox chkSaveInfo;
        private System.Windows.Forms.Button btnBlacklist;
        private System.Windows.Forms.CheckBox chkSaveBlacklisted;
        private System.Windows.Forms.CheckBox chkSeparate;
        private System.Windows.Forms.CheckBox chkSafe;
        private System.Windows.Forms.CheckBox chkQuestionable;
        private System.Windows.Forms.CheckBox chkExplicit;
        private System.Windows.Forms.CheckBox chkMinimumScore;
        private System.Windows.Forms.NumericUpDown numScore;
        private System.Windows.Forms.NumericUpDown numLimit;
        private System.Windows.Forms.CheckBox chkMerge;
        private System.Windows.Forms.CheckBox chkPoolName;
        private System.Windows.Forms.CheckBox chkOpen;
        private System.Windows.Forms.TabPage tbProtocol;
        private System.Windows.Forms.Button btnTagsProtocol;
        private System.Windows.Forms.Button btnPoolsProtocol;
        private System.Windows.Forms.Button btnImagesProtocol;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnImagesUserscript;
        private System.Windows.Forms.Button btnUserscript;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tbImages;
        private System.Windows.Forms.RadioButton rbArtist;
        private System.Windows.Forms.ToolTip JustTheTips;
        private System.Windows.Forms.RadioButton rbMD5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkIgnoreFinish;
        private System.Windows.Forms.CheckBox chkSeparateImages;
        private System.Windows.Forms.CheckBox chkSeparateBlacklisted;
        private System.Windows.Forms.CheckBox chkUseForm;
        private System.Windows.Forms.CheckBox chkAddWishlistSilent;
        private System.Windows.Forms.NumericUpDown numPageLimit;
        private System.Windows.Forms.GroupBox gbTagScoreLimit;
        private System.Windows.Forms.GroupBox gbTagPageLimit;
        private System.Windows.Forms.GroupBox gbTagDownloadLimit;
        private System.Windows.Forms.GroupBox gbTagRatings;
        private System.Windows.Forms.CheckBox chkScoreAsTag;
        private System.Windows.Forms.TabPage tbPortable;
        private System.Windows.Forms.Button btnExportIni;
        private System.Windows.Forms.Label lbPortable;
        private System.Windows.Forms.Label lbImageSchema;
        private System.Windows.Forms.TextBox txtImageSchema;
        private System.Windows.Forms.CheckBox chkSepArtists;
        private System.Windows.Forms.Label lblTagSchema;
        private System.Windows.Forms.TextBox txtTagSchema;
        private System.Windows.Forms.Label lbPoolSchema;
        private System.Windows.Forms.TextBox txtPoolSchema;
        private System.Windows.Forms.TabPage tbSchemas;
        private System.Windows.Forms.Label lbSchemaParam;
        private System.Windows.Forms.RichTextBox rtbParams;
        private System.Windows.Forms.Label lbTagExt;
        private System.Windows.Forms.Label lbImageExt;
        private System.Windows.Forms.Label lbPoolExt;
        private System.Windows.Forms.Button btnSchemaUndesiredTags;
        private System.Windows.Forms.CheckBox chkSkipExistingFiles;
        private System.Windows.Forms.GroupBox gbMetadata;
        private System.Windows.Forms.CheckBox chkSaveTagMetadata;
        private System.Windows.Forms.CheckBox chkSaveArtistMetadata;
        private System.Windows.Forms.CheckBox chkSaveMetadata;

    }
}