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
            this.tbMain = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.chkCheckForUpdates = new System.Windows.Forms.CheckBox();
            this.chkArgumentFormTopMost = new System.Windows.Forms.CheckBox();
            this.chkAutoDownloadWithArguments = new System.Windows.Forms.CheckBox();
            this.chkSkipArgumentCheck = new System.Windows.Forms.CheckBox();
            this.chkIgnoreFinish = new System.Windows.Forms.CheckBox();
            this.chkSaveGraylistedImages = new System.Windows.Forms.CheckBox();
            this.chkOpenAfterDownload = new System.Windows.Forms.CheckBox();
            this.chkSaveInfoFiles = new System.Windows.Forms.CheckBox();
            this.btnBrowseForSaveTo = new aphrodite.Controls.ExtendedButton();
            this.txtSaveTo = new aphrodite.Controls.ExtendedTextBox();
            this.lbSaveTo = new System.Windows.Forms.Label();
            this.tabTags = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbFavCountHint = new System.Windows.Forms.Label();
            this.chkTagsFavoriteCountAsTag = new System.Windows.Forms.CheckBox();
            this.numTagsFavoriteCount = new System.Windows.Forms.NumericUpDown();
            this.lbLimitHint = new System.Windows.Forms.Label();
            this.tcTagLimits = new System.Windows.Forms.TabControl();
            this.tabTagsImageLimit = new System.Windows.Forms.TabPage();
            this.lbTagsImages = new System.Windows.Forms.Label();
            this.numTagsDownloadLimit = new System.Windows.Forms.NumericUpDown();
            this.tabTagsPageLimit = new System.Windows.Forms.TabPage();
            this.lbTagsPages = new System.Windows.Forms.Label();
            this.numTagsPageLimit = new System.Windows.Forms.NumericUpDown();
            this.chkTagsDownloadNewestToOldest = new System.Windows.Forms.CheckBox();
            this.chkTagsDownloadBlacklisted = new System.Windows.Forms.CheckBox();
            this.chkTagsSeparateNonImages = new System.Windows.Forms.CheckBox();
            this.gbTagsRatings = new System.Windows.Forms.GroupBox();
            this.chkTagsExplicit = new System.Windows.Forms.CheckBox();
            this.chkTagsSeparateRatings = new System.Windows.Forms.CheckBox();
            this.chkTagsQuestionable = new System.Windows.Forms.CheckBox();
            this.chkTagsSafe = new System.Windows.Forms.CheckBox();
            this.gbTagsScoreLimit = new System.Windows.Forms.GroupBox();
            this.chkTagsIncludeScoreAsTag = new System.Windows.Forms.CheckBox();
            this.numTagsScoreLimit = new System.Windows.Forms.NumericUpDown();
            this.chkTagsEnableScoreLimit = new System.Windows.Forms.CheckBox();
            this.tabPools = new System.Windows.Forms.TabPage();
            this.chkPoolsMergeBlacklistedImages = new System.Windows.Forms.CheckBox();
            this.chkPoolsDownloadBlacklistedImages = new System.Windows.Forms.CheckBox();
            this.chkPoolsMergeGraylistedImages = new System.Windows.Forms.CheckBox();
            this.chkPoolsAddToWishlistSilently = new System.Windows.Forms.CheckBox();
            this.tabImages = new System.Windows.Forms.TabPage();
            this.lbImagesBlacklistHint = new System.Windows.Forms.Label();
            this.chkImagesSeparateBlacklisted = new System.Windows.Forms.CheckBox();
            this.chkImagesSeparateNonImages = new System.Windows.Forms.CheckBox();
            this.chkImagesUseForm = new System.Windows.Forms.CheckBox();
            this.chkImagesSeparateArtists = new System.Windows.Forms.CheckBox();
            this.chkImagesSeparateGraylisted = new System.Windows.Forms.CheckBox();
            this.chkImagesSeparateRatings = new System.Windows.Forms.CheckBox();
            this.lbImagesSeparator = new System.Windows.Forms.Label();
            this.lbImagesHeader = new System.Windows.Forms.Label();
            this.tabMisc = new System.Windows.Forms.TabPage();
            this.chkUseSessionId = new System.Windows.Forms.CheckBox();
            this.txtSessionId = new aphrodite.Controls.ExtendedTextBox();
            this.lbSessionId = new System.Windows.Forms.Label();
            this.tabProtocol = new System.Windows.Forms.TabPage();
            this.lbProtocolsSeparator = new System.Windows.Forms.Label();
            this.lbProtocols = new System.Windows.Forms.Label();
            this.btnProtocolImagesUserscript = new aphrodite.Controls.ExtendedButton();
            this.btnProtocolUserscript = new aphrodite.Controls.ExtendedButton();
            this.btnProtocolInstallTags = new aphrodite.Controls.ExtendedButton();
            this.btnProtocolInstallPools = new aphrodite.Controls.ExtendedButton();
            this.btnProtocolInstallImages = new aphrodite.Controls.ExtendedButton();
            this.tabSchemas = new System.Windows.Forms.TabPage();
            this.btnSchemaUndesiredTags = new aphrodite.Controls.ExtendedButton();
            this.lbImageExt = new System.Windows.Forms.Label();
            this.lbPoolExt = new System.Windows.Forms.Label();
            this.lbTagExt = new System.Windows.Forms.Label();
            this.rtbParams = new System.Windows.Forms.RichTextBox();
            this.lbSchemaParam = new System.Windows.Forms.Label();
            this.lblTagSchema = new System.Windows.Forms.Label();
            this.lbPoolSchema = new System.Windows.Forms.Label();
            this.txtImageSchema = new aphrodite.Controls.ExtendedTextBox();
            this.txtTagSchema = new aphrodite.Controls.ExtendedTextBox();
            this.lbImageSchema = new System.Windows.Forms.Label();
            this.txtPoolSchema = new aphrodite.Controls.ExtendedTextBox();
            this.tabImportExport = new System.Windows.Forms.TabPage();
            this.chkOverwriteOnImport = new System.Windows.Forms.CheckBox();
            this.btnImportUndesiredTags = new System.Windows.Forms.Button();
            this.btnExportUndesiredTags = new System.Windows.Forms.Button();
            this.btnImportBlacklist = new System.Windows.Forms.Button();
            this.btnExportBlacklist = new System.Windows.Forms.Button();
            this.btnImportGraylist = new System.Windows.Forms.Button();
            this.btnExportGraylist = new System.Windows.Forms.Button();
            this.tabPortable = new System.Windows.Forms.TabPage();
            this.chkEnableIni = new System.Windows.Forms.CheckBox();
            this.lbPortable = new System.Windows.Forms.Label();
            this.JustTheTips = new System.Windows.Forms.ToolTip(this.components);
            this.btnBlacklist = new aphrodite.Controls.ExtendedButton();
            this.btnSave = new aphrodite.Controls.ExtendedButton();
            this.btnCancel = new aphrodite.Controls.ExtendedButton();
            this.pnControls = new System.Windows.Forms.Panel();
            this.chkDontOverwrite = new System.Windows.Forms.CheckBox();
            this.tbMain.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tabTags.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsFavoriteCount)).BeginInit();
            this.tcTagLimits.SuspendLayout();
            this.tabTagsImageLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsDownloadLimit)).BeginInit();
            this.tabTagsPageLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsPageLimit)).BeginInit();
            this.gbTagsRatings.SuspendLayout();
            this.gbTagsScoreLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsScoreLimit)).BeginInit();
            this.tabPools.SuspendLayout();
            this.tabImages.SuspendLayout();
            this.tabMisc.SuspendLayout();
            this.tabProtocol.SuspendLayout();
            this.tabSchemas.SuspendLayout();
            this.tabImportExport.SuspendLayout();
            this.tabPortable.SuspendLayout();
            this.pnControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbMain
            // 
            this.tbMain.Controls.Add(this.tabGeneral);
            this.tbMain.Controls.Add(this.tabTags);
            this.tbMain.Controls.Add(this.tabPools);
            this.tbMain.Controls.Add(this.tabImages);
            this.tbMain.Controls.Add(this.tabMisc);
            this.tbMain.Controls.Add(this.tabProtocol);
            this.tbMain.Controls.Add(this.tabSchemas);
            this.tbMain.Controls.Add(this.tabImportExport);
            this.tbMain.Controls.Add(this.tabPortable);
            this.tbMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbMain.Location = new System.Drawing.Point(0, 0);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(408, 221);
            this.tbMain.TabIndex = 10;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.chkCheckForUpdates);
            this.tabGeneral.Controls.Add(this.chkArgumentFormTopMost);
            this.tabGeneral.Controls.Add(this.chkAutoDownloadWithArguments);
            this.tabGeneral.Controls.Add(this.chkSkipArgumentCheck);
            this.tabGeneral.Controls.Add(this.chkIgnoreFinish);
            this.tabGeneral.Controls.Add(this.chkSaveGraylistedImages);
            this.tabGeneral.Controls.Add(this.chkOpenAfterDownload);
            this.tabGeneral.Controls.Add(this.chkSaveInfoFiles);
            this.tabGeneral.Controls.Add(this.btnBrowseForSaveTo);
            this.tabGeneral.Controls.Add(this.txtSaveTo);
            this.tabGeneral.Controls.Add(this.lbSaveTo);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(400, 195);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // chkCheckForUpdates
            // 
            this.chkCheckForUpdates.AutoSize = true;
            this.chkCheckForUpdates.Location = new System.Drawing.Point(112, 160);
            this.chkCheckForUpdates.Name = "chkCheckForUpdates";
            this.chkCheckForUpdates.Size = new System.Drawing.Size(176, 17);
            this.chkCheckForUpdates.TabIndex = 21;
            this.chkCheckForUpdates.Text = "Check for updates on startup";
            this.JustTheTips.SetToolTip(this.chkCheckForUpdates, "Checks for new version updates everytime the program starts up");
            this.chkCheckForUpdates.UseVisualStyleBackColor = true;
            // 
            // chkArgumentFormTopMost
            // 
            this.chkArgumentFormTopMost.AutoSize = true;
            this.chkArgumentFormTopMost.Checked = true;
            this.chkArgumentFormTopMost.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkArgumentFormTopMost.Location = new System.Drawing.Point(175, 91);
            this.chkArgumentFormTopMost.Name = "chkArgumentFormTopMost";
            this.chkArgumentFormTopMost.Size = new System.Drawing.Size(153, 17);
            this.chkArgumentFormTopMost.TabIndex = 20;
            this.chkArgumentFormTopMost.Text = "Argument form top-most";
            this.JustTheTips.SetToolTip(this.chkArgumentFormTopMost, "The argument form will appear above most windows, when shown");
            this.chkArgumentFormTopMost.UseVisualStyleBackColor = true;
            // 
            // chkAutoDownloadWithArguments
            // 
            this.chkAutoDownloadWithArguments.AutoSize = true;
            this.chkAutoDownloadWithArguments.Checked = true;
            this.chkAutoDownloadWithArguments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoDownloadWithArguments.Location = new System.Drawing.Point(155, 68);
            this.chkAutoDownloadWithArguments.Name = "chkAutoDownloadWithArguments";
            this.chkAutoDownloadWithArguments.Size = new System.Drawing.Size(229, 17);
            this.chkAutoDownloadWithArguments.TabIndex = 19;
            this.chkAutoDownloadWithArguments.Text = "Auto-download when using arguments";
            this.JustTheTips.SetToolTip(this.chkAutoDownloadWithArguments, "When enabled, the program will download files based on the arguments.\r\n\r\nWhen dis" +
        "abled, the arguments will be sent to the main form.");
            this.chkAutoDownloadWithArguments.UseVisualStyleBackColor = true;
            // 
            // chkSkipArgumentCheck
            // 
            this.chkSkipArgumentCheck.AutoSize = true;
            this.chkSkipArgumentCheck.Location = new System.Drawing.Point(17, 68);
            this.chkSkipArgumentCheck.Name = "chkSkipArgumentCheck";
            this.chkSkipArgumentCheck.Size = new System.Drawing.Size(132, 17);
            this.chkSkipArgumentCheck.TabIndex = 18;
            this.chkSkipArgumentCheck.Text = "Skip argument check";
            this.JustTheTips.SetToolTip(this.chkSkipArgumentCheck, "Skips the argument check when downloading through arguments.");
            this.chkSkipArgumentCheck.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreFinish
            // 
            this.chkIgnoreFinish.AutoSize = true;
            this.chkIgnoreFinish.Location = new System.Drawing.Point(177, 114);
            this.chkIgnoreFinish.Name = "chkIgnoreFinish";
            this.chkIgnoreFinish.Size = new System.Drawing.Size(193, 17);
            this.chkIgnoreFinish.TabIndex = 4;
            this.chkIgnoreFinish.Text = "Don\'t notify finished downloads";
            this.JustTheTips.SetToolTip(this.chkIgnoreFinish, "Doesn\'t notify you when downloads are completed, and aborted downloads close when" +
        " requested\r\nPlugin downloads quit when finished");
            this.chkIgnoreFinish.UseVisualStyleBackColor = true;
            // 
            // chkSaveGraylistedImages
            // 
            this.chkSaveGraylistedImages.AutoSize = true;
            this.chkSaveGraylistedImages.Checked = true;
            this.chkSaveGraylistedImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveGraylistedImages.Location = new System.Drawing.Point(31, 114);
            this.chkSaveGraylistedImages.Name = "chkSaveGraylistedImages";
            this.chkSaveGraylistedImages.Size = new System.Drawing.Size(140, 17);
            this.chkSaveGraylistedImages.TabIndex = 3;
            this.chkSaveGraylistedImages.Text = "Save graylisted images";
            this.JustTheTips.SetToolTip(this.chkSaveGraylistedImages, "Saves blacklisted images into a separate folder.\r\nImages downloaded with the \'ima" +
        "ges\' protocol are also saved + separated.");
            this.chkSaveGraylistedImages.UseVisualStyleBackColor = true;
            // 
            // chkOpenAfterDownload
            // 
            this.chkOpenAfterDownload.AutoSize = true;
            this.chkOpenAfterDownload.Location = new System.Drawing.Point(123, 137);
            this.chkOpenAfterDownload.Name = "chkOpenAfterDownload";
            this.chkOpenAfterDownload.Size = new System.Drawing.Size(154, 17);
            this.chkOpenAfterDownload.TabIndex = 3;
            this.chkOpenAfterDownload.Text = "Open after downloading";
            this.JustTheTips.SetToolTip(this.chkOpenAfterDownload, "Opens the download folder after downloading");
            this.chkOpenAfterDownload.UseVisualStyleBackColor = true;
            // 
            // chkSaveInfoFiles
            // 
            this.chkSaveInfoFiles.AutoSize = true;
            this.chkSaveInfoFiles.Checked = true;
            this.chkSaveInfoFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveInfoFiles.Location = new System.Drawing.Point(73, 91);
            this.chkSaveInfoFiles.Name = "chkSaveInfoFiles";
            this.chkSaveInfoFiles.Size = new System.Drawing.Size(96, 17);
            this.chkSaveInfoFiles.TabIndex = 2;
            this.chkSaveInfoFiles.Text = "Save info files";
            this.JustTheTips.SetToolTip(this.chkSaveInfoFiles, resources.GetString("chkSaveInfoFiles.ToolTip"));
            this.chkSaveInfoFiles.UseVisualStyleBackColor = true;
            // 
            // btnBrowseForSaveTo
            // 
            this.btnBrowseForSaveTo.Location = new System.Drawing.Point(353, 31);
            this.btnBrowseForSaveTo.Name = "btnBrowseForSaveTo";
            this.btnBrowseForSaveTo.Size = new System.Drawing.Size(24, 23);
            this.btnBrowseForSaveTo.TabIndex = 1;
            this.btnBrowseForSaveTo.Text = "...";
            this.JustTheTips.SetToolTip(this.btnBrowseForSaveTo, "Browse for a new directory to save downloads to.\r\n\r\nYou\'ll have the option to mov" +
        "e existing downloads.");
            this.btnBrowseForSaveTo.UseVisualStyleBackColor = true;
            this.btnBrowseForSaveTo.Click += new System.EventHandler(this.btnBrws_Click);
            // 
            // txtSaveTo
            // 
            this.txtSaveTo.ButtonAlignment = aphrodite.Controls.ButtonAlignments.Left;
            this.txtSaveTo.ButtonCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSaveTo.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSaveTo.ButtonImageIndex = -1;
            this.txtSaveTo.ButtonImageKey = "";
            this.txtSaveTo.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtSaveTo.ButtonText = "";
            this.txtSaveTo.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtSaveTo.Location = new System.Drawing.Point(24, 32);
            this.txtSaveTo.Name = "txtSaveTo";
            this.txtSaveTo.ReadOnly = true;
            this.txtSaveTo.Size = new System.Drawing.Size(323, 22);
            this.txtSaveTo.TabIndex = 0;
            this.txtSaveTo.TextHint = "";
            this.JustTheTips.SetToolTip(this.txtSaveTo, "The location where pools, tags, and images will be saved to.");
            // 
            // lbSaveTo
            // 
            this.lbSaveTo.AutoSize = true;
            this.lbSaveTo.Location = new System.Drawing.Point(21, 14);
            this.lbSaveTo.Name = "lbSaveTo";
            this.lbSaveTo.Size = new System.Drawing.Size(47, 13);
            this.lbSaveTo.TabIndex = 17;
            this.lbSaveTo.Text = "Save to:";
            // 
            // tabTags
            // 
            this.tabTags.Controls.Add(this.groupBox1);
            this.tabTags.Controls.Add(this.lbLimitHint);
            this.tabTags.Controls.Add(this.tcTagLimits);
            this.tabTags.Controls.Add(this.chkTagsDownloadNewestToOldest);
            this.tabTags.Controls.Add(this.chkTagsDownloadBlacklisted);
            this.tabTags.Controls.Add(this.chkTagsSeparateNonImages);
            this.tabTags.Controls.Add(this.gbTagsRatings);
            this.tabTags.Controls.Add(this.gbTagsScoreLimit);
            this.tabTags.Location = new System.Drawing.Point(4, 22);
            this.tabTags.Name = "tabTags";
            this.tabTags.Padding = new System.Windows.Forms.Padding(3);
            this.tabTags.Size = new System.Drawing.Size(400, 195);
            this.tabTags.TabIndex = 1;
            this.tabTags.Text = "Tags";
            this.tabTags.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbFavCountHint);
            this.groupBox1.Controls.Add(this.chkTagsFavoriteCountAsTag);
            this.groupBox1.Controls.Add(this.numTagsFavoriteCount);
            this.groupBox1.Location = new System.Drawing.Point(267, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(127, 82);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Minimum fav count";
            // 
            // lbFavCountHint
            // 
            this.lbFavCountHint.Location = new System.Drawing.Point(6, 15);
            this.lbFavCountHint.Name = "lbFavCountHint";
            this.lbFavCountHint.Size = new System.Drawing.Size(115, 13);
            this.lbFavCountHint.TabIndex = 4;
            this.lbFavCountHint.Text = "(0 = off)";
            this.lbFavCountHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkTagsFavoriteCountAsTag
            // 
            this.chkTagsFavoriteCountAsTag.AutoSize = true;
            this.chkTagsFavoriteCountAsTag.Location = new System.Drawing.Point(9, 60);
            this.chkTagsFavoriteCountAsTag.Name = "chkTagsFavoriteCountAsTag";
            this.chkTagsFavoriteCountAsTag.Size = new System.Drawing.Size(114, 17);
            this.chkTagsFavoriteCountAsTag.TabIndex = 3;
            this.chkTagsFavoriteCountAsTag.Text = "Include with tags";
            this.JustTheTips.SetToolTip(this.chkTagsFavoriteCountAsTag, resources.GetString("chkTagsFavoriteCountAsTag.ToolTip"));
            this.chkTagsFavoriteCountAsTag.UseVisualStyleBackColor = true;
            // 
            // numTagsFavoriteCount
            // 
            this.numTagsFavoriteCount.Location = new System.Drawing.Point(26, 36);
            this.numTagsFavoriteCount.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numTagsFavoriteCount.Name = "numTagsFavoriteCount";
            this.numTagsFavoriteCount.Size = new System.Drawing.Size(67, 22);
            this.numTagsFavoriteCount.TabIndex = 1;
            this.numTagsFavoriteCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.JustTheTips.SetToolTip(this.numTagsFavoriteCount, "The minimum favorites count that will be downloaded");
            // 
            // lbLimitHint
            // 
            this.lbLimitHint.Location = new System.Drawing.Point(244, 133);
            this.lbLimitHint.Name = "lbLimitHint";
            this.lbLimitHint.Size = new System.Drawing.Size(124, 13);
            this.lbLimitHint.TabIndex = 10;
            this.lbLimitHint.Text = "(0 = off)";
            this.lbLimitHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tcTagLimits
            // 
            this.tcTagLimits.Controls.Add(this.tabTagsImageLimit);
            this.tcTagLimits.Controls.Add(this.tabTagsPageLimit);
            this.tcTagLimits.Location = new System.Drawing.Point(236, 109);
            this.tcTagLimits.Name = "tcTagLimits";
            this.tcTagLimits.SelectedIndex = 0;
            this.tcTagLimits.Size = new System.Drawing.Size(141, 73);
            this.tcTagLimits.TabIndex = 9;
            // 
            // tabTagsImageLimit
            // 
            this.tabTagsImageLimit.Controls.Add(this.lbTagsImages);
            this.tabTagsImageLimit.Controls.Add(this.numTagsDownloadLimit);
            this.tabTagsImageLimit.Location = new System.Drawing.Point(4, 22);
            this.tabTagsImageLimit.Name = "tabTagsImageLimit";
            this.tabTagsImageLimit.Padding = new System.Windows.Forms.Padding(3);
            this.tabTagsImageLimit.Size = new System.Drawing.Size(133, 47);
            this.tabTagsImageLimit.TabIndex = 0;
            this.tabTagsImageLimit.Text = "Image Limit";
            this.tabTagsImageLimit.UseVisualStyleBackColor = true;
            // 
            // lbTagsImages
            // 
            this.lbTagsImages.Location = new System.Drawing.Point(83, 18);
            this.lbTagsImages.Name = "lbTagsImages";
            this.lbTagsImages.Size = new System.Drawing.Size(43, 23);
            this.lbTagsImages.TabIndex = 4;
            this.lbTagsImages.Text = "images";
            this.lbTagsImages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numTagsDownloadLimit
            // 
            this.numTagsDownloadLimit.Location = new System.Drawing.Point(11, 20);
            this.numTagsDownloadLimit.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numTagsDownloadLimit.Name = "numTagsDownloadLimit";
            this.numTagsDownloadLimit.Size = new System.Drawing.Size(72, 22);
            this.numTagsDownloadLimit.TabIndex = 1;
            this.numTagsDownloadLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.JustTheTips.SetToolTip(this.numTagsDownloadLimit, "Limits downloads to a certain amount of images");
            // 
            // tabTagsPageLimit
            // 
            this.tabTagsPageLimit.Controls.Add(this.lbTagsPages);
            this.tabTagsPageLimit.Controls.Add(this.numTagsPageLimit);
            this.tabTagsPageLimit.Location = new System.Drawing.Point(4, 22);
            this.tabTagsPageLimit.Name = "tabTagsPageLimit";
            this.tabTagsPageLimit.Padding = new System.Windows.Forms.Padding(3);
            this.tabTagsPageLimit.Size = new System.Drawing.Size(133, 47);
            this.tabTagsPageLimit.TabIndex = 1;
            this.tabTagsPageLimit.Text = "Page Limit";
            this.tabTagsPageLimit.UseVisualStyleBackColor = true;
            // 
            // lbTagsPages
            // 
            this.lbTagsPages.Location = new System.Drawing.Point(83, 18);
            this.lbTagsPages.Name = "lbTagsPages";
            this.lbTagsPages.Size = new System.Drawing.Size(43, 23);
            this.lbTagsPages.TabIndex = 3;
            this.lbTagsPages.Text = "pages";
            this.lbTagsPages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numTagsPageLimit
            // 
            this.numTagsPageLimit.Location = new System.Drawing.Point(11, 20);
            this.numTagsPageLimit.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numTagsPageLimit.Name = "numTagsPageLimit";
            this.numTagsPageLimit.Size = new System.Drawing.Size(72, 22);
            this.numTagsPageLimit.TabIndex = 1;
            this.numTagsPageLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.JustTheTips.SetToolTip(this.numTagsPageLimit, "The amount of pages that will be downloaded (320 images per page)");
            // 
            // chkTagsDownloadNewestToOldest
            // 
            this.chkTagsDownloadNewestToOldest.AutoSize = true;
            this.chkTagsDownloadNewestToOldest.Location = new System.Drawing.Point(32, 157);
            this.chkTagsDownloadNewestToOldest.Name = "chkTagsDownloadNewestToOldest";
            this.chkTagsDownloadNewestToOldest.Size = new System.Drawing.Size(168, 17);
            this.chkTagsDownloadNewestToOldest.TabIndex = 8;
            this.chkTagsDownloadNewestToOldest.Text = "Download newest to oldest";
            this.JustTheTips.SetToolTip(this.chkTagsDownloadNewestToOldest, "Downloads images newest first to oldest last");
            this.chkTagsDownloadNewestToOldest.UseVisualStyleBackColor = true;
            // 
            // chkTagsDownloadBlacklisted
            // 
            this.chkTagsDownloadBlacklisted.AutoSize = true;
            this.chkTagsDownloadBlacklisted.Location = new System.Drawing.Point(32, 134);
            this.chkTagsDownloadBlacklisted.Name = "chkTagsDownloadBlacklisted";
            this.chkTagsDownloadBlacklisted.Size = new System.Drawing.Size(145, 17);
            this.chkTagsDownloadBlacklisted.TabIndex = 7;
            this.chkTagsDownloadBlacklisted.Text = "Save blacklisted images";
            this.JustTheTips.SetToolTip(this.chkTagsDownloadBlacklisted, "Downloads any images in the blacklisted tags.");
            this.chkTagsDownloadBlacklisted.UseVisualStyleBackColor = true;
            // 
            // chkTagsSeparateNonImages
            // 
            this.chkTagsSeparateNonImages.AutoSize = true;
            this.chkTagsSeparateNonImages.Location = new System.Drawing.Point(32, 111);
            this.chkTagsSeparateNonImages.Name = "chkTagsSeparateNonImages";
            this.chkTagsSeparateNonImages.Size = new System.Drawing.Size(134, 17);
            this.chkTagsSeparateNonImages.TabIndex = 6;
            this.chkTagsSeparateNonImages.Text = "Separate non-images";
            this.JustTheTips.SetToolTip(this.chkTagsSeparateNonImages, "Enable this option to further separate non-image files into their own folder");
            this.chkTagsSeparateNonImages.UseVisualStyleBackColor = true;
            // 
            // gbTagsRatings
            // 
            this.gbTagsRatings.Controls.Add(this.chkTagsExplicit);
            this.gbTagsRatings.Controls.Add(this.chkTagsSeparateRatings);
            this.gbTagsRatings.Controls.Add(this.chkTagsQuestionable);
            this.gbTagsRatings.Controls.Add(this.chkTagsSafe);
            this.gbTagsRatings.Location = new System.Drawing.Point(6, 12);
            this.gbTagsRatings.Name = "gbTagsRatings";
            this.gbTagsRatings.Size = new System.Drawing.Size(122, 67);
            this.gbTagsRatings.TabIndex = 1;
            this.gbTagsRatings.TabStop = false;
            this.gbTagsRatings.Text = "Ratings";
            // 
            // chkTagsExplicit
            // 
            this.chkTagsExplicit.AutoSize = true;
            this.chkTagsExplicit.Checked = true;
            this.chkTagsExplicit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTagsExplicit.Location = new System.Drawing.Point(9, 19);
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
            this.chkTagsSeparateRatings.Location = new System.Drawing.Point(9, 42);
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
            this.chkTagsQuestionable.Location = new System.Drawing.Point(47, 18);
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
            this.chkTagsSafe.Location = new System.Drawing.Point(86, 18);
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
            this.gbTagsScoreLimit.Location = new System.Drawing.Point(134, 12);
            this.gbTagsScoreLimit.Name = "gbTagsScoreLimit";
            this.gbTagsScoreLimit.Size = new System.Drawing.Size(127, 82);
            this.gbTagsScoreLimit.TabIndex = 2;
            this.gbTagsScoreLimit.TabStop = false;
            this.gbTagsScoreLimit.Text = "Minimum score";
            // 
            // chkTagsIncludeScoreAsTag
            // 
            this.chkTagsIncludeScoreAsTag.AutoSize = true;
            this.chkTagsIncludeScoreAsTag.Enabled = false;
            this.chkTagsIncludeScoreAsTag.Location = new System.Drawing.Point(9, 60);
            this.chkTagsIncludeScoreAsTag.Name = "chkTagsIncludeScoreAsTag";
            this.chkTagsIncludeScoreAsTag.Size = new System.Drawing.Size(114, 17);
            this.chkTagsIncludeScoreAsTag.TabIndex = 3;
            this.chkTagsIncludeScoreAsTag.Text = "Include with tags";
            this.JustTheTips.SetToolTip(this.chkTagsIncludeScoreAsTag, resources.GetString("chkTagsIncludeScoreAsTag.ToolTip"));
            this.chkTagsIncludeScoreAsTag.UseVisualStyleBackColor = true;
            // 
            // numTagsScoreLimit
            // 
            this.numTagsScoreLimit.Enabled = false;
            this.numTagsScoreLimit.Location = new System.Drawing.Point(26, 19);
            this.numTagsScoreLimit.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numTagsScoreLimit.Minimum = new decimal(new int[] {
            999999,
            0,
            0,
            -2147483648});
            this.numTagsScoreLimit.Name = "numTagsScoreLimit";
            this.numTagsScoreLimit.Size = new System.Drawing.Size(67, 22);
            this.numTagsScoreLimit.TabIndex = 1;
            this.numTagsScoreLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.JustTheTips.SetToolTip(this.numTagsScoreLimit, "The minimum score that will be downloaded");
            // 
            // chkTagsEnableScoreLimit
            // 
            this.chkTagsEnableScoreLimit.AutoSize = true;
            this.chkTagsEnableScoreLimit.Location = new System.Drawing.Point(9, 43);
            this.chkTagsEnableScoreLimit.Name = "chkTagsEnableScoreLimit";
            this.chkTagsEnableScoreLimit.Size = new System.Drawing.Size(115, 17);
            this.chkTagsEnableScoreLimit.TabIndex = 2;
            this.chkTagsEnableScoreLimit.Text = "Enable score limit";
            this.JustTheTips.SetToolTip(this.chkTagsEnableScoreLimit, "Only downloads images with a score equal to or greater than provided\r\nThis option" +
        " is required because scores can be in the negatives");
            this.chkTagsEnableScoreLimit.UseVisualStyleBackColor = true;
            this.chkTagsEnableScoreLimit.CheckedChanged += new System.EventHandler(this.chkMinimumScore_CheckedChanged);
            // 
            // tabPools
            // 
            this.tabPools.Controls.Add(this.chkPoolsMergeBlacklistedImages);
            this.tabPools.Controls.Add(this.chkPoolsDownloadBlacklistedImages);
            this.tabPools.Controls.Add(this.chkPoolsMergeGraylistedImages);
            this.tabPools.Controls.Add(this.chkPoolsAddToWishlistSilently);
            this.tabPools.Location = new System.Drawing.Point(4, 22);
            this.tabPools.Name = "tabPools";
            this.tabPools.Padding = new System.Windows.Forms.Padding(3);
            this.tabPools.Size = new System.Drawing.Size(400, 195);
            this.tabPools.TabIndex = 2;
            this.tabPools.Text = "Pools";
            this.tabPools.UseVisualStyleBackColor = true;
            // 
            // chkPoolsMergeBlacklistedImages
            // 
            this.chkPoolsMergeBlacklistedImages.AutoSize = true;
            this.chkPoolsMergeBlacklistedImages.Location = new System.Drawing.Point(49, 80);
            this.chkPoolsMergeBlacklistedImages.Name = "chkPoolsMergeBlacklistedImages";
            this.chkPoolsMergeBlacklistedImages.Size = new System.Drawing.Size(302, 17);
            this.chkPoolsMergeBlacklistedImages.TabIndex = 6;
            this.chkPoolsMergeBlacklistedImages.Text = "Merge blacklisted images with non blacklisted images";
            this.JustTheTips.SetToolTip(this.chkPoolsMergeBlacklistedImages, "Merge blacklisted images with the rest of the pool\r\npool.blacklisted.nfo is not s" +
        "aved, instead, blacklisted info is saved in pool.nfo under \"BLACKLISTED PAGE\"");
            this.chkPoolsMergeBlacklistedImages.UseVisualStyleBackColor = true;
            // 
            // chkPoolsDownloadBlacklistedImages
            // 
            this.chkPoolsDownloadBlacklistedImages.AutoSize = true;
            this.chkPoolsDownloadBlacklistedImages.Checked = true;
            this.chkPoolsDownloadBlacklistedImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPoolsDownloadBlacklistedImages.Location = new System.Drawing.Point(128, 57);
            this.chkPoolsDownloadBlacklistedImages.Name = "chkPoolsDownloadBlacklistedImages";
            this.chkPoolsDownloadBlacklistedImages.Size = new System.Drawing.Size(145, 17);
            this.chkPoolsDownloadBlacklistedImages.TabIndex = 5;
            this.chkPoolsDownloadBlacklistedImages.Text = "Save blacklisted images";
            this.JustTheTips.SetToolTip(this.chkPoolsDownloadBlacklistedImages, "Downloads any blacklisted pages in the pool");
            this.chkPoolsDownloadBlacklistedImages.UseVisualStyleBackColor = true;
            // 
            // chkPoolsMergeGraylistedImages
            // 
            this.chkPoolsMergeGraylistedImages.AutoSize = true;
            this.chkPoolsMergeGraylistedImages.Checked = true;
            this.chkPoolsMergeGraylistedImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPoolsMergeGraylistedImages.Location = new System.Drawing.Point(54, 34);
            this.chkPoolsMergeGraylistedImages.Name = "chkPoolsMergeGraylistedImages";
            this.chkPoolsMergeGraylistedImages.Size = new System.Drawing.Size(292, 17);
            this.chkPoolsMergeGraylistedImages.TabIndex = 2;
            this.chkPoolsMergeGraylistedImages.Text = "Merge graylisted images with non graylisted images";
            this.JustTheTips.SetToolTip(this.chkPoolsMergeGraylistedImages, "Merge graylisted images with the rest of the pool\r\npool.graylisted.nfo is not sav" +
        "ed, instead, graylisted info is saved in pool.nfo under \"GRAYLISTED PAGE\"");
            this.chkPoolsMergeGraylistedImages.UseVisualStyleBackColor = true;
            // 
            // chkPoolsAddToWishlistSilently
            // 
            this.chkPoolsAddToWishlistSilently.AutoSize = true;
            this.chkPoolsAddToWishlistSilently.Location = new System.Drawing.Point(114, 139);
            this.chkPoolsAddToWishlistSilently.Name = "chkPoolsAddToWishlistSilently";
            this.chkPoolsAddToWishlistSilently.Size = new System.Drawing.Size(173, 17);
            this.chkPoolsAddToWishlistSilently.TabIndex = 4;
            this.chkPoolsAddToWishlistSilently.Text = "Add pools to wishlist silently";
            this.JustTheTips.SetToolTip(this.chkPoolsAddToWishlistSilently, "Add the pool to your wishlist without showing the application.");
            this.chkPoolsAddToWishlistSilently.UseVisualStyleBackColor = true;
            // 
            // tabImages
            // 
            this.tabImages.Controls.Add(this.lbImagesBlacklistHint);
            this.tabImages.Controls.Add(this.chkImagesSeparateBlacklisted);
            this.tabImages.Controls.Add(this.chkImagesSeparateNonImages);
            this.tabImages.Controls.Add(this.chkImagesUseForm);
            this.tabImages.Controls.Add(this.chkImagesSeparateArtists);
            this.tabImages.Controls.Add(this.chkImagesSeparateGraylisted);
            this.tabImages.Controls.Add(this.chkImagesSeparateRatings);
            this.tabImages.Controls.Add(this.lbImagesSeparator);
            this.tabImages.Controls.Add(this.lbImagesHeader);
            this.tabImages.Location = new System.Drawing.Point(4, 22);
            this.tabImages.Name = "tabImages";
            this.tabImages.Padding = new System.Windows.Forms.Padding(3);
            this.tabImages.Size = new System.Drawing.Size(400, 195);
            this.tabImages.TabIndex = 4;
            this.tabImages.Text = "Images";
            this.tabImages.UseVisualStyleBackColor = true;
            // 
            // lbImagesBlacklistHint
            // 
            this.lbImagesBlacklistHint.Location = new System.Drawing.Point(6, 40);
            this.lbImagesBlacklistHint.Name = "lbImagesBlacklistHint";
            this.lbImagesBlacklistHint.Size = new System.Drawing.Size(388, 19);
            this.lbImagesBlacklistHint.TabIndex = 11;
            this.lbImagesBlacklistHint.Text = "Graylisted && Blacklisted images are downloaded regardless of options";
            this.lbImagesBlacklistHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkImagesSeparateBlacklisted
            // 
            this.chkImagesSeparateBlacklisted.AutoSize = true;
            this.chkImagesSeparateBlacklisted.Checked = true;
            this.chkImagesSeparateBlacklisted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImagesSeparateBlacklisted.Location = new System.Drawing.Point(201, 94);
            this.chkImagesSeparateBlacklisted.Name = "chkImagesSeparateBlacklisted";
            this.chkImagesSeparateBlacklisted.Size = new System.Drawing.Size(167, 17);
            this.chkImagesSeparateBlacklisted.TabIndex = 10;
            this.chkImagesSeparateBlacklisted.Text = "Separate blacklisted images";
            this.JustTheTips.SetToolTip(this.chkImagesSeparateBlacklisted, "Separates blacklisted images into separate folder.");
            this.chkImagesSeparateBlacklisted.UseVisualStyleBackColor = true;
            // 
            // chkImagesSeparateNonImages
            // 
            this.chkImagesSeparateNonImages.AutoSize = true;
            this.chkImagesSeparateNonImages.Checked = true;
            this.chkImagesSeparateNonImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImagesSeparateNonImages.Location = new System.Drawing.Point(243, 117);
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
            this.chkImagesUseForm.Location = new System.Drawing.Point(76, 140);
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
            this.chkImagesSeparateArtists.Location = new System.Drawing.Point(139, 117);
            this.chkImagesSeparateArtists.Name = "chkImagesSeparateArtists";
            this.chkImagesSeparateArtists.Size = new System.Drawing.Size(104, 17);
            this.chkImagesSeparateArtists.TabIndex = 3;
            this.chkImagesSeparateArtists.Text = "Separate artists";
            this.chkImagesSeparateArtists.UseVisualStyleBackColor = true;
            // 
            // chkImagesSeparateGraylisted
            // 
            this.chkImagesSeparateGraylisted.AutoSize = true;
            this.chkImagesSeparateGraylisted.Checked = true;
            this.chkImagesSeparateGraylisted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImagesSeparateGraylisted.Location = new System.Drawing.Point(33, 94);
            this.chkImagesSeparateGraylisted.Name = "chkImagesSeparateGraylisted";
            this.chkImagesSeparateGraylisted.Size = new System.Drawing.Size(162, 17);
            this.chkImagesSeparateGraylisted.TabIndex = 2;
            this.chkImagesSeparateGraylisted.Text = "Separate graylisted images";
            this.JustTheTips.SetToolTip(this.chkImagesSeparateGraylisted, "Separates graylisted images into separate folder.");
            this.chkImagesSeparateGraylisted.UseVisualStyleBackColor = true;
            // 
            // chkImagesSeparateRatings
            // 
            this.chkImagesSeparateRatings.AutoSize = true;
            this.chkImagesSeparateRatings.Checked = true;
            this.chkImagesSeparateRatings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImagesSeparateRatings.Location = new System.Drawing.Point(24, 117);
            this.chkImagesSeparateRatings.Name = "chkImagesSeparateRatings";
            this.chkImagesSeparateRatings.Size = new System.Drawing.Size(109, 17);
            this.chkImagesSeparateRatings.TabIndex = 1;
            this.chkImagesSeparateRatings.Text = "Separate ratings";
            this.JustTheTips.SetToolTip(this.chkImagesSeparateRatings, "Separate ratings into separate folders for images");
            this.chkImagesSeparateRatings.UseVisualStyleBackColor = true;
            // 
            // lbImagesSeparator
            // 
            this.lbImagesSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbImagesSeparator.Location = new System.Drawing.Point(54, 34);
            this.lbImagesSeparator.Name = "lbImagesSeparator";
            this.lbImagesSeparator.Size = new System.Drawing.Size(292, 2);
            this.lbImagesSeparator.TabIndex = 7;
            this.lbImagesSeparator.Text = "bark";
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
            // tabMisc
            // 
            this.tabMisc.Controls.Add(this.chkUseSessionId);
            this.tabMisc.Controls.Add(this.txtSessionId);
            this.tabMisc.Controls.Add(this.lbSessionId);
            this.tabMisc.Location = new System.Drawing.Point(4, 22);
            this.tabMisc.Name = "tabMisc";
            this.tabMisc.Padding = new System.Windows.Forms.Padding(3);
            this.tabMisc.Size = new System.Drawing.Size(400, 195);
            this.tabMisc.TabIndex = 8;
            this.tabMisc.Text = "Misc";
            this.tabMisc.UseVisualStyleBackColor = true;
            // 
            // chkUseSessionId
            // 
            this.chkUseSessionId.AutoSize = true;
            this.chkUseSessionId.Location = new System.Drawing.Point(9, 53);
            this.chkUseSessionId.Name = "chkUseSessionId";
            this.chkUseSessionId.Size = new System.Drawing.Size(98, 17);
            this.chkUseSessionId.TabIndex = 20;
            this.chkUseSessionId.Text = "Use session id";
            this.JustTheTips.SetToolTip(this.chkUseSessionId, "Enables the use of the session id when e621 requires captcha to access");
            this.chkUseSessionId.UseVisualStyleBackColor = true;
            // 
            // txtSessionId
            // 
            this.txtSessionId.ButtonAlignment = aphrodite.Controls.ButtonAlignments.Left;
            this.txtSessionId.ButtonCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSessionId.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSessionId.ButtonImageIndex = -1;
            this.txtSessionId.ButtonImageKey = "";
            this.txtSessionId.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtSessionId.ButtonText = "";
            this.txtSessionId.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtSessionId.Location = new System.Drawing.Point(9, 25);
            this.txtSessionId.Name = "txtSessionId";
            this.txtSessionId.Size = new System.Drawing.Size(383, 22);
            this.txtSessionId.TabIndex = 18;
            this.txtSessionId.TextHint = "";
            this.JustTheTips.SetToolTip(this.txtSessionId, "The \"cf_clearance\" cookie for e621.net to allow site access");
            // 
            // lbSessionId
            // 
            this.lbSessionId.AutoSize = true;
            this.lbSessionId.Location = new System.Drawing.Point(6, 7);
            this.lbSessionId.Name = "lbSessionId";
            this.lbSessionId.Size = new System.Drawing.Size(175, 13);
            this.lbSessionId.TabIndex = 19;
            this.lbSessionId.Text = "Captcha session ID (cf_clearance)";
            // 
            // tabProtocol
            // 
            this.tabProtocol.Controls.Add(this.lbProtocolsSeparator);
            this.tabProtocol.Controls.Add(this.lbProtocols);
            this.tabProtocol.Controls.Add(this.btnProtocolImagesUserscript);
            this.tabProtocol.Controls.Add(this.btnProtocolUserscript);
            this.tabProtocol.Controls.Add(this.btnProtocolInstallTags);
            this.tabProtocol.Controls.Add(this.btnProtocolInstallPools);
            this.tabProtocol.Controls.Add(this.btnProtocolInstallImages);
            this.tabProtocol.Location = new System.Drawing.Point(4, 22);
            this.tabProtocol.Name = "tabProtocol";
            this.tabProtocol.Padding = new System.Windows.Forms.Padding(3);
            this.tabProtocol.Size = new System.Drawing.Size(400, 195);
            this.tabProtocol.TabIndex = 3;
            this.tabProtocol.Text = "Protocols";
            this.tabProtocol.UseVisualStyleBackColor = true;
            // 
            // lbProtocolsSeparator
            // 
            this.lbProtocolsSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbProtocolsSeparator.Location = new System.Drawing.Point(59, 149);
            this.lbProtocolsSeparator.Name = "lbProtocolsSeparator";
            this.lbProtocolsSeparator.Size = new System.Drawing.Size(292, 2);
            this.lbProtocolsSeparator.TabIndex = 6;
            this.lbProtocolsSeparator.Text = "bark";
            // 
            // lbProtocols
            // 
            this.lbProtocols.Location = new System.Drawing.Point(6, 5);
            this.lbProtocols.Name = "lbProtocols";
            this.lbProtocols.Size = new System.Drawing.Size(388, 80);
            this.lbProtocols.TabIndex = 3;
            this.lbProtocols.Text = resources.GetString("lbProtocols.Text");
            this.lbProtocols.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnProtocolImagesUserscript
            // 
            this.btnProtocolImagesUserscript.Location = new System.Drawing.Point(246, 160);
            this.btnProtocolImagesUserscript.Name = "btnProtocolImagesUserscript";
            this.btnProtocolImagesUserscript.Size = new System.Drawing.Size(75, 23);
            this.btnProtocolImagesUserscript.TabIndex = 5;
            this.btnProtocolImagesUserscript.Text = "Userscript";
            this.btnProtocolImagesUserscript.UseVisualStyleBackColor = true;
            this.btnProtocolImagesUserscript.Click += new System.EventHandler(this.btnImagesUserscript_Click);
            // 
            // btnProtocolUserscript
            // 
            this.btnProtocolUserscript.Location = new System.Drawing.Point(246, 107);
            this.btnProtocolUserscript.Name = "btnProtocolUserscript";
            this.btnProtocolUserscript.Size = new System.Drawing.Size(75, 23);
            this.btnProtocolUserscript.TabIndex = 3;
            this.btnProtocolUserscript.Text = "Userscript";
            this.btnProtocolUserscript.UseVisualStyleBackColor = true;
            this.btnProtocolUserscript.Click += new System.EventHandler(this.btnUserscript_Click);
            // 
            // btnProtocolInstallTags
            // 
            this.btnProtocolInstallTags.Location = new System.Drawing.Point(89, 95);
            this.btnProtocolInstallTags.Name = "btnProtocolInstallTags";
            this.btnProtocolInstallTags.Size = new System.Drawing.Size(151, 23);
            this.btnProtocolInstallTags.TabIndex = 1;
            this.btnProtocolInstallTags.Text = "Install tag protocol";
            this.btnProtocolInstallTags.UseVisualStyleBackColor = true;
            this.btnProtocolInstallTags.Click += new System.EventHandler(this.btnTagsProtocol_Click);
            // 
            // btnProtocolInstallPools
            // 
            this.btnProtocolInstallPools.Location = new System.Drawing.Point(89, 120);
            this.btnProtocolInstallPools.Name = "btnProtocolInstallPools";
            this.btnProtocolInstallPools.Size = new System.Drawing.Size(151, 23);
            this.btnProtocolInstallPools.TabIndex = 2;
            this.btnProtocolInstallPools.Text = "Install pool protocols";
            this.btnProtocolInstallPools.UseVisualStyleBackColor = true;
            this.btnProtocolInstallPools.Click += new System.EventHandler(this.btnPoolsProtocol_Click);
            // 
            // btnProtocolInstallImages
            // 
            this.btnProtocolInstallImages.Location = new System.Drawing.Point(89, 160);
            this.btnProtocolInstallImages.Name = "btnProtocolInstallImages";
            this.btnProtocolInstallImages.Size = new System.Drawing.Size(151, 23);
            this.btnProtocolInstallImages.TabIndex = 4;
            this.btnProtocolInstallImages.Text = "Install image protocol";
            this.btnProtocolInstallImages.UseVisualStyleBackColor = true;
            this.btnProtocolInstallImages.Click += new System.EventHandler(this.btnImagesProtocol_Click);
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
            this.tabSchemas.Size = new System.Drawing.Size(400, 195);
            this.tabSchemas.TabIndex = 6;
            this.tabSchemas.Text = "Schemas";
            this.tabSchemas.UseVisualStyleBackColor = true;
            // 
            // btnSchemaUndesiredTags
            // 
            this.btnSchemaUndesiredTags.Location = new System.Drawing.Point(272, 80);
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
            this.lbImageExt.Location = new System.Drawing.Point(333, 57);
            this.lbImageExt.Name = "lbImageExt";
            this.lbImageExt.Size = new System.Drawing.Size(43, 13);
            this.lbImageExt.TabIndex = 51;
            this.lbImageExt.Text = ".%ext%";
            // 
            // lbPoolExt
            // 
            this.lbPoolExt.AutoSize = true;
            this.lbPoolExt.Location = new System.Drawing.Point(333, 33);
            this.lbPoolExt.Name = "lbPoolExt";
            this.lbPoolExt.Size = new System.Drawing.Size(43, 13);
            this.lbPoolExt.TabIndex = 50;
            this.lbPoolExt.Text = ".%ext%";
            // 
            // lbTagExt
            // 
            this.lbTagExt.AutoSize = true;
            this.lbTagExt.Location = new System.Drawing.Point(333, 9);
            this.lbTagExt.Name = "lbTagExt";
            this.lbTagExt.Size = new System.Drawing.Size(43, 13);
            this.lbTagExt.TabIndex = 49;
            this.lbTagExt.Text = ".%ext%";
            // 
            // rtbParams
            // 
            this.rtbParams.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbParams.Location = new System.Drawing.Point(6, 108);
            this.rtbParams.Name = "rtbParams";
            this.rtbParams.ReadOnly = true;
            this.rtbParams.Size = new System.Drawing.Size(388, 84);
            this.rtbParams.TabIndex = 5;
            this.rtbParams.Text = resources.GetString("rtbParams.Text");
            // 
            // lbSchemaParam
            // 
            this.lbSchemaParam.AutoSize = true;
            this.lbSchemaParam.Location = new System.Drawing.Point(8, 89);
            this.lbSchemaParam.Name = "lbSchemaParam";
            this.lbSchemaParam.Size = new System.Drawing.Size(122, 13);
            this.lbSchemaParam.TabIndex = 47;
            this.lbSchemaParam.Text = "Supported parameters";
            // 
            // lblTagSchema
            // 
            this.lblTagSchema.AutoSize = true;
            this.lblTagSchema.Location = new System.Drawing.Point(29, 9);
            this.lblTagSchema.Name = "lblTagSchema";
            this.lblTagSchema.Size = new System.Drawing.Size(102, 13);
            this.lblTagSchema.TabIndex = 46;
            this.lblTagSchema.Text = "Tags name schema";
            // 
            // lbPoolSchema
            // 
            this.lbPoolSchema.AutoSize = true;
            this.lbPoolSchema.Location = new System.Drawing.Point(27, 33);
            this.lbPoolSchema.Name = "lbPoolSchema";
            this.lbPoolSchema.Size = new System.Drawing.Size(107, 13);
            this.lbPoolSchema.TabIndex = 22;
            this.lbPoolSchema.Text = "Pools name schema";
            // 
            // txtImageSchema
            // 
            this.txtImageSchema.ButtonAlignment = aphrodite.Controls.ButtonAlignments.Left;
            this.txtImageSchema.ButtonCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtImageSchema.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtImageSchema.ButtonImageIndex = -1;
            this.txtImageSchema.ButtonImageKey = "";
            this.txtImageSchema.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtImageSchema.ButtonText = "";
            this.txtImageSchema.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtImageSchema.Location = new System.Drawing.Point(135, 54);
            this.txtImageSchema.Name = "txtImageSchema";
            this.txtImageSchema.Size = new System.Drawing.Size(197, 22);
            this.txtImageSchema.TabIndex = 3;
            this.txtImageSchema.TextHint = "%artist%_%md5%";
            this.JustTheTips.SetToolTip(this.txtImageSchema, resources.GetString("txtImageSchema.ToolTip"));
            this.txtImageSchema.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtImageSchema_KeyDown);
            this.txtImageSchema.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtImageSchema_KeyPress);
            // 
            // txtTagSchema
            // 
            this.txtTagSchema.ButtonAlignment = aphrodite.Controls.ButtonAlignments.Left;
            this.txtTagSchema.ButtonCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTagSchema.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTagSchema.ButtonImageIndex = -1;
            this.txtTagSchema.ButtonImageKey = "";
            this.txtTagSchema.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtTagSchema.ButtonText = "";
            this.txtTagSchema.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtTagSchema.Location = new System.Drawing.Point(135, 6);
            this.txtTagSchema.Name = "txtTagSchema";
            this.txtTagSchema.Size = new System.Drawing.Size(197, 22);
            this.txtTagSchema.TabIndex = 1;
            this.txtTagSchema.TextHint = "%md5%";
            this.JustTheTips.SetToolTip(this.txtTagSchema, resources.GetString("txtTagSchema.ToolTip"));
            this.txtTagSchema.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTagSchema_KeyDown);
            this.txtTagSchema.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTagSchema_KeyPress);
            // 
            // lbImageSchema
            // 
            this.lbImageSchema.AutoSize = true;
            this.lbImageSchema.Location = new System.Drawing.Point(24, 57);
            this.lbImageSchema.Name = "lbImageSchema";
            this.lbImageSchema.Size = new System.Drawing.Size(110, 13);
            this.lbImageSchema.TabIndex = 13;
            this.lbImageSchema.Text = "Image name schema";
            // 
            // txtPoolSchema
            // 
            this.txtPoolSchema.ButtonAlignment = aphrodite.Controls.ButtonAlignments.Left;
            this.txtPoolSchema.ButtonCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPoolSchema.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPoolSchema.ButtonImageIndex = -1;
            this.txtPoolSchema.ButtonImageKey = "";
            this.txtPoolSchema.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtPoolSchema.ButtonText = "";
            this.txtPoolSchema.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtPoolSchema.Location = new System.Drawing.Point(135, 30);
            this.txtPoolSchema.Name = "txtPoolSchema";
            this.txtPoolSchema.Size = new System.Drawing.Size(197, 22);
            this.txtPoolSchema.TabIndex = 2;
            this.txtPoolSchema.TextHint = "%poolname%_%page%";
            this.JustTheTips.SetToolTip(this.txtPoolSchema, resources.GetString("txtPoolSchema.ToolTip"));
            this.txtPoolSchema.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPoolSchema_KeyDown);
            this.txtPoolSchema.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPoolSchema_KeyPress);
            // 
            // tabImportExport
            // 
            this.tabImportExport.Controls.Add(this.chkOverwriteOnImport);
            this.tabImportExport.Controls.Add(this.btnImportUndesiredTags);
            this.tabImportExport.Controls.Add(this.btnExportUndesiredTags);
            this.tabImportExport.Controls.Add(this.btnImportBlacklist);
            this.tabImportExport.Controls.Add(this.btnExportBlacklist);
            this.tabImportExport.Controls.Add(this.btnImportGraylist);
            this.tabImportExport.Controls.Add(this.btnExportGraylist);
            this.tabImportExport.Location = new System.Drawing.Point(4, 22);
            this.tabImportExport.Name = "tabImportExport";
            this.tabImportExport.Size = new System.Drawing.Size(400, 195);
            this.tabImportExport.TabIndex = 7;
            this.tabImportExport.Text = "Import/Export";
            this.tabImportExport.UseVisualStyleBackColor = true;
            // 
            // chkOverwriteOnImport
            // 
            this.chkOverwriteOnImport.AutoSize = true;
            this.chkOverwriteOnImport.Location = new System.Drawing.Point(72, 159);
            this.chkOverwriteOnImport.Name = "chkOverwriteOnImport";
            this.chkOverwriteOnImport.Size = new System.Drawing.Size(257, 17);
            this.chkOverwriteOnImport.TabIndex = 6;
            this.chkOverwriteOnImport.Text = "Overwrite saved information when importing";
            this.JustTheTips.SetToolTip(this.chkOverwriteOnImport, "Instead of appending to the present information, it\'ll overwrite it with the new " +
        "data");
            this.chkOverwriteOnImport.UseVisualStyleBackColor = true;
            // 
            // btnImportUndesiredTags
            // 
            this.btnImportUndesiredTags.Location = new System.Drawing.Point(244, 61);
            this.btnImportUndesiredTags.Name = "btnImportUndesiredTags";
            this.btnImportUndesiredTags.Size = new System.Drawing.Size(140, 23);
            this.btnImportUndesiredTags.TabIndex = 5;
            this.btnImportUndesiredTags.Text = "Import Undesired Tags";
            this.JustTheTips.SetToolTip(this.btnImportUndesiredTags, "Imports text from a file into the undesired tags.");
            this.btnImportUndesiredTags.UseVisualStyleBackColor = true;
            this.btnImportUndesiredTags.Click += new System.EventHandler(this.btnImportUndesiredTags_Click);
            // 
            // btnExportUndesiredTags
            // 
            this.btnExportUndesiredTags.Location = new System.Drawing.Point(244, 24);
            this.btnExportUndesiredTags.Name = "btnExportUndesiredTags";
            this.btnExportUndesiredTags.Size = new System.Drawing.Size(140, 23);
            this.btnExportUndesiredTags.TabIndex = 4;
            this.btnExportUndesiredTags.Text = "Export Undesired Tags";
            this.JustTheTips.SetToolTip(this.btnExportUndesiredTags, "Exports the undesired tags to a selected file.");
            this.btnExportUndesiredTags.UseVisualStyleBackColor = true;
            this.btnExportUndesiredTags.Click += new System.EventHandler(this.btnExportUndesiredTags_Click);
            // 
            // btnImportBlacklist
            // 
            this.btnImportBlacklist.Location = new System.Drawing.Point(130, 61);
            this.btnImportBlacklist.Name = "btnImportBlacklist";
            this.btnImportBlacklist.Size = new System.Drawing.Size(100, 23);
            this.btnImportBlacklist.TabIndex = 3;
            this.btnImportBlacklist.Text = "Import Blacklist";
            this.JustTheTips.SetToolTip(this.btnImportBlacklist, "Imports text from a file into the blacklist.");
            this.btnImportBlacklist.UseVisualStyleBackColor = true;
            this.btnImportBlacklist.Click += new System.EventHandler(this.btnImportBlacklist_Click);
            // 
            // btnExportBlacklist
            // 
            this.btnExportBlacklist.Location = new System.Drawing.Point(130, 24);
            this.btnExportBlacklist.Name = "btnExportBlacklist";
            this.btnExportBlacklist.Size = new System.Drawing.Size(100, 23);
            this.btnExportBlacklist.TabIndex = 2;
            this.btnExportBlacklist.Text = "Export Blacklist";
            this.JustTheTips.SetToolTip(this.btnExportBlacklist, "Exports the blacklist to a selected file.");
            this.btnExportBlacklist.UseVisualStyleBackColor = true;
            this.btnExportBlacklist.Click += new System.EventHandler(this.btnExportBlacklist_Click);
            // 
            // btnImportGraylist
            // 
            this.btnImportGraylist.Location = new System.Drawing.Point(16, 61);
            this.btnImportGraylist.Name = "btnImportGraylist";
            this.btnImportGraylist.Size = new System.Drawing.Size(100, 23);
            this.btnImportGraylist.TabIndex = 1;
            this.btnImportGraylist.Text = "Import Graylist";
            this.JustTheTips.SetToolTip(this.btnImportGraylist, "Imports text from a file into the graylist.");
            this.btnImportGraylist.UseVisualStyleBackColor = true;
            this.btnImportGraylist.Click += new System.EventHandler(this.btnImportGraylist_Click);
            // 
            // btnExportGraylist
            // 
            this.btnExportGraylist.Location = new System.Drawing.Point(16, 24);
            this.btnExportGraylist.Name = "btnExportGraylist";
            this.btnExportGraylist.Size = new System.Drawing.Size(100, 23);
            this.btnExportGraylist.TabIndex = 0;
            this.btnExportGraylist.Text = "Export Graylist";
            this.JustTheTips.SetToolTip(this.btnExportGraylist, "Exports the graylist to a selected file.");
            this.btnExportGraylist.UseVisualStyleBackColor = true;
            this.btnExportGraylist.Click += new System.EventHandler(this.btnExportGraylist_Click);
            // 
            // tabPortable
            // 
            this.tabPortable.Controls.Add(this.chkDontOverwrite);
            this.tabPortable.Controls.Add(this.chkEnableIni);
            this.tabPortable.Controls.Add(this.lbPortable);
            this.tabPortable.Location = new System.Drawing.Point(4, 22);
            this.tabPortable.Name = "tabPortable";
            this.tabPortable.Padding = new System.Windows.Forms.Padding(3);
            this.tabPortable.Size = new System.Drawing.Size(400, 195);
            this.tabPortable.TabIndex = 5;
            this.tabPortable.Text = "Portable";
            this.tabPortable.UseVisualStyleBackColor = true;
            // 
            // chkEnableIni
            // 
            this.chkEnableIni.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkEnableIni.Location = new System.Drawing.Point(135, 146);
            this.chkEnableIni.Name = "chkEnableIni";
            this.chkEnableIni.Size = new System.Drawing.Size(131, 23);
            this.chkEnableIni.TabIndex = 1;
            this.chkEnableIni.Text = "Use portable ini file";
            this.chkEnableIni.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkEnableIni.UseVisualStyleBackColor = true;
            this.chkEnableIni.CheckedChanged += new System.EventHandler(this.chkEnableIni_CheckedChanged);
            // 
            // lbPortable
            // 
            this.lbPortable.Location = new System.Drawing.Point(6, 7);
            this.lbPortable.Name = "lbPortable";
            this.lbPortable.Size = new System.Drawing.Size(388, 133);
            this.lbPortable.TabIndex = 0;
            this.lbPortable.Text = resources.GetString("lbPortable.Text");
            this.lbPortable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnBlacklist
            // 
            this.btnBlacklist.Location = new System.Drawing.Point(12, 9);
            this.btnBlacklist.Name = "btnBlacklist";
            this.btnBlacklist.Size = new System.Drawing.Size(102, 23);
            this.btnBlacklist.TabIndex = 7;
            this.btnBlacklist.Text = "Manage blacklist";
            this.JustTheTips.SetToolTip(this.btnBlacklist, "Manage your graylist and blacklist");
            this.btnBlacklist.UseVisualStyleBackColor = true;
            this.btnBlacklist.Click += new System.EventHandler(this.btnBlacklist_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(321, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.JustTheTips.SetToolTip(this.btnSave, "Save changes, does not apply to any in-progress operations.");
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(240, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.JustTheTips.SetToolTip(this.btnCancel, "Do not save any changes");
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnControls
            // 
            this.pnControls.BackColor = System.Drawing.SystemColors.Menu;
            this.pnControls.Controls.Add(this.btnBlacklist);
            this.pnControls.Controls.Add(this.btnSave);
            this.pnControls.Controls.Add(this.btnCancel);
            this.pnControls.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnControls.Location = new System.Drawing.Point(0, 221);
            this.pnControls.Name = "pnControls";
            this.pnControls.Size = new System.Drawing.Size(408, 42);
            this.pnControls.TabIndex = 11;
            // 
            // chkDontOverwrite
            // 
            this.chkDontOverwrite.AutoSize = true;
            this.chkDontOverwrite.Location = new System.Drawing.Point(9, 172);
            this.chkDontOverwrite.Name = "chkDontOverwrite";
            this.chkDontOverwrite.Size = new System.Drawing.Size(192, 17);
            this.chkDontOverwrite.TabIndex = 2;
            this.chkDontOverwrite.Text = "Don\'t overwrite existing settings";
            this.JustTheTips.SetToolTip(this.chkDontOverwrite, "Doesn\'t overwrite existing settings, but will ovewrite them when closing the prog" +
        "ram.");
            this.chkDontOverwrite.UseVisualStyleBackColor = true;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(408, 263);
            this.Controls.Add(this.tbMain);
            this.Controls.Add(this.pnControls);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(426, 300);
            this.MinimumSize = new System.Drawing.Size(426, 300);
            this.Name = "frmSettings";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "aphrodite settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSettings_FormClosing);
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.tbMain.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.tabTags.ResumeLayout(false);
            this.tabTags.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsFavoriteCount)).EndInit();
            this.tcTagLimits.ResumeLayout(false);
            this.tabTagsImageLimit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numTagsDownloadLimit)).EndInit();
            this.tabTagsPageLimit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numTagsPageLimit)).EndInit();
            this.gbTagsRatings.ResumeLayout(false);
            this.gbTagsRatings.PerformLayout();
            this.gbTagsScoreLimit.ResumeLayout(false);
            this.gbTagsScoreLimit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTagsScoreLimit)).EndInit();
            this.tabPools.ResumeLayout(false);
            this.tabPools.PerformLayout();
            this.tabImages.ResumeLayout(false);
            this.tabImages.PerformLayout();
            this.tabMisc.ResumeLayout(false);
            this.tabMisc.PerformLayout();
            this.tabProtocol.ResumeLayout(false);
            this.tabSchemas.ResumeLayout(false);
            this.tabSchemas.PerformLayout();
            this.tabImportExport.ResumeLayout(false);
            this.tabImportExport.PerformLayout();
            this.tabPortable.ResumeLayout(false);
            this.tabPortable.PerformLayout();
            this.pnControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ExtendedButton btnCancel;
        private Controls.ExtendedButton btnSave;
        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabTags;
        private System.Windows.Forms.TabPage tabPools;
        private Controls.ExtendedButton btnBrowseForSaveTo;
        private Controls.ExtendedTextBox txtSaveTo;
        private System.Windows.Forms.Label lbSaveTo;
        private System.Windows.Forms.CheckBox chkSaveInfoFiles;
        private Controls.ExtendedButton btnBlacklist;
        private System.Windows.Forms.CheckBox chkSaveGraylistedImages;
        private System.Windows.Forms.CheckBox chkTagsSeparateRatings;
        private System.Windows.Forms.CheckBox chkTagsSafe;
        private System.Windows.Forms.CheckBox chkTagsQuestionable;
        private System.Windows.Forms.CheckBox chkTagsExplicit;
        private System.Windows.Forms.CheckBox chkTagsEnableScoreLimit;
        private System.Windows.Forms.NumericUpDown numTagsScoreLimit;
        private System.Windows.Forms.NumericUpDown numTagsDownloadLimit;
        private System.Windows.Forms.CheckBox chkPoolsMergeGraylistedImages;
        private System.Windows.Forms.CheckBox chkOpenAfterDownload;
        private System.Windows.Forms.TabPage tabProtocol;
        private Controls.ExtendedButton btnProtocolInstallTags;
        private Controls.ExtendedButton btnProtocolInstallPools;
        private Controls.ExtendedButton btnProtocolInstallImages;
        private System.Windows.Forms.Label lbProtocolsSeparator;
        private Controls.ExtendedButton btnProtocolImagesUserscript;
        private Controls.ExtendedButton btnProtocolUserscript;
        private System.Windows.Forms.Label lbProtocols;
        private System.Windows.Forms.TabPage tabImages;
        private System.Windows.Forms.ToolTip JustTheTips;
        private System.Windows.Forms.Label lbImagesSeparator;
        private System.Windows.Forms.Label lbImagesHeader;
        private System.Windows.Forms.CheckBox chkIgnoreFinish;
        private System.Windows.Forms.CheckBox chkImagesSeparateRatings;
        private System.Windows.Forms.CheckBox chkImagesSeparateGraylisted;
        private System.Windows.Forms.CheckBox chkImagesUseForm;
        private System.Windows.Forms.CheckBox chkPoolsAddToWishlistSilently;
        private System.Windows.Forms.NumericUpDown numTagsPageLimit;
        private System.Windows.Forms.GroupBox gbTagsScoreLimit;
        private System.Windows.Forms.GroupBox gbTagsRatings;
        private System.Windows.Forms.CheckBox chkTagsIncludeScoreAsTag;
        private System.Windows.Forms.TabPage tabPortable;
        private System.Windows.Forms.Label lbPortable;
        private System.Windows.Forms.Label lbImageSchema;
        private Controls.ExtendedTextBox txtImageSchema;
        private System.Windows.Forms.CheckBox chkImagesSeparateArtists;
        private System.Windows.Forms.Label lblTagSchema;
        private Controls.ExtendedTextBox txtTagSchema;
        private System.Windows.Forms.Label lbPoolSchema;
        private Controls.ExtendedTextBox txtPoolSchema;
        private System.Windows.Forms.TabPage tabSchemas;
        private System.Windows.Forms.Label lbSchemaParam;
        private System.Windows.Forms.RichTextBox rtbParams;
        private System.Windows.Forms.Label lbTagExt;
        private System.Windows.Forms.Label lbImageExt;
        private System.Windows.Forms.Label lbPoolExt;
        private Controls.ExtendedButton btnSchemaUndesiredTags;
        private System.Windows.Forms.CheckBox chkTagsSeparateNonImages;
        private System.Windows.Forms.CheckBox chkImagesSeparateNonImages;
        private System.Windows.Forms.CheckBox chkPoolsMergeBlacklistedImages;
        private System.Windows.Forms.CheckBox chkPoolsDownloadBlacklistedImages;
        private System.Windows.Forms.Label lbImagesBlacklistHint;
        private System.Windows.Forms.CheckBox chkImagesSeparateBlacklisted;
        private System.Windows.Forms.CheckBox chkTagsDownloadBlacklisted;
        private System.Windows.Forms.CheckBox chkTagsDownloadNewestToOldest;
        private System.Windows.Forms.TabPage tabImportExport;
        private System.Windows.Forms.Button btnImportUndesiredTags;
        private System.Windows.Forms.Button btnExportUndesiredTags;
        private System.Windows.Forms.Button btnImportBlacklist;
        private System.Windows.Forms.Button btnExportBlacklist;
        private System.Windows.Forms.Button btnImportGraylist;
        private System.Windows.Forms.Button btnExportGraylist;
        private System.Windows.Forms.CheckBox chkOverwriteOnImport;
        private System.Windows.Forms.Panel pnControls;
        private System.Windows.Forms.CheckBox chkSkipArgumentCheck;
        private System.Windows.Forms.CheckBox chkAutoDownloadWithArguments;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbFavCountHint;
        private System.Windows.Forms.CheckBox chkTagsFavoriteCountAsTag;
        private System.Windows.Forms.NumericUpDown numTagsFavoriteCount;
        private System.Windows.Forms.Label lbLimitHint;
        private System.Windows.Forms.TabControl tcTagLimits;
        private System.Windows.Forms.TabPage tabTagsImageLimit;
        private System.Windows.Forms.TabPage tabTagsPageLimit;
        private System.Windows.Forms.Label lbTagsImages;
        private System.Windows.Forms.Label lbTagsPages;
        private System.Windows.Forms.TabPage tabMisc;
        private Controls.ExtendedTextBox txtSessionId;
        private System.Windows.Forms.Label lbSessionId;
        private System.Windows.Forms.CheckBox chkUseSessionId;
        private System.Windows.Forms.CheckBox chkArgumentFormTopMost;
        private System.Windows.Forms.CheckBox chkEnableIni;
        private System.Windows.Forms.CheckBox chkCheckForUpdates;
        private System.Windows.Forms.CheckBox chkDontOverwrite;

    }
}