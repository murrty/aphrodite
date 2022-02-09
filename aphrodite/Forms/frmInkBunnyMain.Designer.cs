namespace aphrodite {
    partial class frmInkBunnyMain {
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
            this.mAccount = new System.Windows.Forms.MenuItem();
            this.mLogin = new System.Windows.Forms.MenuItem();
            this.mSessionIDs = new System.Windows.Forms.MenuItem();
            this.mShowSessionIDs = new System.Windows.Forms.MenuItem();
            this.mShowDecryptedSessionIDs = new System.Windows.Forms.MenuItem();
            this.mCopyGuestSessionID = new System.Windows.Forms.MenuItem();
            this.mCopyDecryptedSessionID = new System.Windows.Forms.MenuItem();
            this.mSettings = new System.Windows.Forms.MenuItem();
            this.mLog = new System.Windows.Forms.MenuItem();
            this.mAbout = new System.Windows.Forms.MenuItem();
            this.chkSkipDebug = new System.Windows.Forms.CheckBox();
            this.lbDownloadingAs = new System.Windows.Forms.Label();
            this.lbSubmissionType = new System.Windows.Forms.Label();
            this.clbSubmissionType = new System.Windows.Forms.CheckedListBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.gbSearchIn = new System.Windows.Forms.GroupBox();
            this.chkSearchInMD5Hash = new System.Windows.Forms.CheckBox();
            this.chkSearchInDescriptionOrStory = new System.Windows.Forms.CheckBox();
            this.chkSearchInTitle = new System.Windows.Forms.CheckBox();
            this.chkSearchInKeywords = new System.Windows.Forms.CheckBox();
            this.gbRatings = new System.Windows.Forms.GroupBox();
            this.chkRatingGeneral = new System.Windows.Forms.CheckBox();
            this.chkRatingMatureNudity = new System.Windows.Forms.CheckBox();
            this.chkRatingMatureViolence = new System.Windows.Forms.CheckBox();
            this.chkRatingAdultSexualThemes = new System.Windows.Forms.CheckBox();
            this.chkRatingAdultStrongViolence = new System.Windows.Forms.CheckBox();
            this.lbSearchTerms = new System.Windows.Forms.Label();
            this.lbFileNameSchema = new System.Windows.Forms.Label();
            this.lbFileNameSchemaMultiPost = new System.Windows.Forms.Label();
            this.chkDownloadGraylistedSubmissions = new System.Windows.Forms.CheckBox();
            this.chkDownloadBlacklistedSubmissions = new System.Windows.Forms.CheckBox();
            this.llbSchemaHelp = new murrty.controls.ExtendedLinkLabel();
            this.txtFileNameSchemaMultiPost = new murrty.controls.ExtendedTextBox();
            this.txtFileNameSchema = new murrty.controls.ExtendedTextBox();
            this.txtSearchUsersFavorites = new murrty.controls.ExtendedTextBox();
            this.txtSearchArtistName = new murrty.controls.ExtendedTextBox();
            this.txtSearchKeywordsMD5 = new murrty.controls.ExtendedTextBox();
            this.numImageLimit = new System.Windows.Forms.NumericUpDown();
            this.numPageLimit = new System.Windows.Forms.NumericUpDown();
            this.lbImageLimit = new System.Windows.Forms.Label();
            this.lbPageLimit = new System.Windows.Forms.Label();
            this.gbSearchIn.SuspendLayout();
            this.gbRatings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numImageLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPageLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // mMenu
            // 
            this.mMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mAccount,
            this.mSettings,
            this.mLog,
            this.mAbout});
            // 
            // mAccount
            // 
            this.mAccount.Index = 0;
            this.mAccount.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mLogin,
            this.mSessionIDs});
            this.mAccount.Text = "account";
            // 
            // mLogin
            // 
            this.mLogin.Index = 0;
            this.mLogin.Text = "login";
            this.mLogin.Click += new System.EventHandler(this.mLogin_Click);
            // 
            // mSessionIDs
            // 
            this.mSessionIDs.Index = 1;
            this.mSessionIDs.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mShowSessionIDs,
            this.mShowDecryptedSessionIDs,
            this.mCopyGuestSessionID,
            this.mCopyDecryptedSessionID});
            this.mSessionIDs.Text = "session id";
            // 
            // mShowSessionIDs
            // 
            this.mShowSessionIDs.Index = 0;
            this.mShowSessionIDs.Text = "show session ids";
            this.mShowSessionIDs.Click += new System.EventHandler(this.mShowSessionIDs_Click);
            // 
            // mShowDecryptedSessionIDs
            // 
            this.mShowDecryptedSessionIDs.Index = 1;
            this.mShowDecryptedSessionIDs.Text = "show decrypted session ids";
            this.mShowDecryptedSessionIDs.Click += new System.EventHandler(this.mShowDecryptedSessionIDs_Click);
            // 
            // mCopyGuestSessionID
            // 
            this.mCopyGuestSessionID.Index = 2;
            this.mCopyGuestSessionID.Text = "copy guest session id to clipboard";
            this.mCopyGuestSessionID.Click += new System.EventHandler(this.mCopyGuestSessionID_Click);
            // 
            // mCopyDecryptedSessionID
            // 
            this.mCopyDecryptedSessionID.Index = 3;
            this.mCopyDecryptedSessionID.Text = "copy decrypted session id to clipboard";
            this.mCopyDecryptedSessionID.Click += new System.EventHandler(this.mCopyDecryptedSessionID_Click);
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
            // chkSkipDebug
            // 
            this.chkSkipDebug.AutoSize = true;
            this.chkSkipDebug.Enabled = false;
            this.chkSkipDebug.Location = new System.Drawing.Point(252, 419);
            this.chkSkipDebug.Name = "chkSkipDebug";
            this.chkSkipDebug.Size = new System.Drawing.Size(85, 17);
            this.chkSkipDebug.TabIndex = 10;
            this.chkSkipDebug.Text = "Skip Debug";
            this.chkSkipDebug.UseVisualStyleBackColor = true;
            this.chkSkipDebug.Visible = false;
            // 
            // lbDownloadingAs
            // 
            this.lbDownloadingAs.AutoSize = true;
            this.lbDownloadingAs.Location = new System.Drawing.Point(210, 6);
            this.lbDownloadingAs.Name = "lbDownloadingAs";
            this.lbDownloadingAs.Size = new System.Drawing.Size(90, 13);
            this.lbDownloadingAs.TabIndex = 8;
            this.lbDownloadingAs.Text = "logged in: none";
            // 
            // lbSubmissionType
            // 
            this.lbSubmissionType.AutoSize = true;
            this.lbSubmissionType.Location = new System.Drawing.Point(17, 113);
            this.lbSubmissionType.Name = "lbSubmissionType";
            this.lbSubmissionType.Size = new System.Drawing.Size(90, 13);
            this.lbSubmissionType.TabIndex = 5;
            this.lbSubmissionType.Text = "submission type";
            // 
            // clbSubmissionType
            // 
            this.clbSubmissionType.Enabled = false;
            this.clbSubmissionType.FormattingEnabled = true;
            this.clbSubmissionType.Items.AddRange(new object[] {
            "picture/pinup",
            "sketch",
            "picture series",
            "comic",
            "portfolio",
            "shockwave/flash - animation",
            "shockwave/flash - interactive",
            "video - feature length",
            "video - animation/3d/cgi",
            "music - single track",
            "music - album",
            "writing - document",
            "character sheet",
            "photography"});
            this.clbSubmissionType.Location = new System.Drawing.Point(17, 131);
            this.clbSubmissionType.Name = "clbSubmissionType";
            this.clbSubmissionType.Size = new System.Drawing.Size(180, 174);
            this.clbSubmissionType.TabIndex = 6;
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(132, 415);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(114, 23);
            this.btnDownload.TabIndex = 9;
            this.btnDownload.Text = "start download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // gbSearchIn
            // 
            this.gbSearchIn.Controls.Add(this.chkSearchInMD5Hash);
            this.gbSearchIn.Controls.Add(this.chkSearchInDescriptionOrStory);
            this.gbSearchIn.Controls.Add(this.chkSearchInTitle);
            this.gbSearchIn.Controls.Add(this.chkSearchInKeywords);
            this.gbSearchIn.Enabled = false;
            this.gbSearchIn.Location = new System.Drawing.Point(232, 31);
            this.gbSearchIn.Name = "gbSearchIn";
            this.gbSearchIn.Size = new System.Drawing.Size(147, 116);
            this.gbSearchIn.TabIndex = 4;
            this.gbSearchIn.TabStop = false;
            this.gbSearchIn.Text = "search in...";
            // 
            // chkSearchInMD5Hash
            // 
            this.chkSearchInMD5Hash.AutoSize = true;
            this.chkSearchInMD5Hash.Location = new System.Drawing.Point(20, 88);
            this.chkSearchInMD5Hash.Name = "chkSearchInMD5Hash";
            this.chkSearchInMD5Hash.Size = new System.Drawing.Size(75, 17);
            this.chkSearchInMD5Hash.TabIndex = 3;
            this.chkSearchInMD5Hash.Text = "md5 hash";
            this.chkSearchInMD5Hash.UseVisualStyleBackColor = true;
            this.chkSearchInMD5Hash.CheckedChanged += new System.EventHandler(this.chkSearchInMD5Hash_CheckedChanged);
            // 
            // chkSearchInDescriptionOrStory
            // 
            this.chkSearchInDescriptionOrStory.AutoSize = true;
            this.chkSearchInDescriptionOrStory.Location = new System.Drawing.Point(20, 65);
            this.chkSearchInDescriptionOrStory.Name = "chkSearchInDescriptionOrStory";
            this.chkSearchInDescriptionOrStory.Size = new System.Drawing.Size(112, 17);
            this.chkSearchInDescriptionOrStory.TabIndex = 2;
            this.chkSearchInDescriptionOrStory.Text = "description/story";
            this.chkSearchInDescriptionOrStory.UseVisualStyleBackColor = true;
            // 
            // chkSearchInTitle
            // 
            this.chkSearchInTitle.AutoSize = true;
            this.chkSearchInTitle.Location = new System.Drawing.Point(20, 42);
            this.chkSearchInTitle.Name = "chkSearchInTitle";
            this.chkSearchInTitle.Size = new System.Drawing.Size(45, 17);
            this.chkSearchInTitle.TabIndex = 1;
            this.chkSearchInTitle.Text = "title";
            this.chkSearchInTitle.UseVisualStyleBackColor = true;
            // 
            // chkSearchInKeywords
            // 
            this.chkSearchInKeywords.AutoSize = true;
            this.chkSearchInKeywords.Checked = true;
            this.chkSearchInKeywords.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSearchInKeywords.Location = new System.Drawing.Point(20, 19);
            this.chkSearchInKeywords.Name = "chkSearchInKeywords";
            this.chkSearchInKeywords.Size = new System.Drawing.Size(74, 17);
            this.chkSearchInKeywords.TabIndex = 0;
            this.chkSearchInKeywords.Text = "keywords";
            this.chkSearchInKeywords.UseVisualStyleBackColor = true;
            // 
            // gbRatings
            // 
            this.gbRatings.Controls.Add(this.chkRatingGeneral);
            this.gbRatings.Controls.Add(this.chkRatingMatureNudity);
            this.gbRatings.Controls.Add(this.chkRatingMatureViolence);
            this.gbRatings.Controls.Add(this.chkRatingAdultSexualThemes);
            this.gbRatings.Controls.Add(this.chkRatingAdultStrongViolence);
            this.gbRatings.Enabled = false;
            this.gbRatings.Location = new System.Drawing.Point(213, 168);
            this.gbRatings.Name = "gbRatings";
            this.gbRatings.Size = new System.Drawing.Size(164, 137);
            this.gbRatings.TabIndex = 7;
            this.gbRatings.TabStop = false;
            this.gbRatings.Text = "submission rating";
            // 
            // chkRatingGeneral
            // 
            this.chkRatingGeneral.AutoSize = true;
            this.chkRatingGeneral.Location = new System.Drawing.Point(17, 19);
            this.chkRatingGeneral.Name = "chkRatingGeneral";
            this.chkRatingGeneral.Size = new System.Drawing.Size(64, 17);
            this.chkRatingGeneral.TabIndex = 0;
            this.chkRatingGeneral.Text = "general";
            this.chkRatingGeneral.UseVisualStyleBackColor = true;
            // 
            // chkRatingMatureNudity
            // 
            this.chkRatingMatureNudity.AutoSize = true;
            this.chkRatingMatureNudity.Location = new System.Drawing.Point(17, 42);
            this.chkRatingMatureNudity.Name = "chkRatingMatureNudity";
            this.chkRatingMatureNudity.Size = new System.Drawing.Size(104, 17);
            this.chkRatingMatureNudity.TabIndex = 1;
            this.chkRatingMatureNudity.Text = "mature - nudity";
            this.chkRatingMatureNudity.UseVisualStyleBackColor = true;
            // 
            // chkRatingMatureViolence
            // 
            this.chkRatingMatureViolence.AutoSize = true;
            this.chkRatingMatureViolence.Location = new System.Drawing.Point(17, 65);
            this.chkRatingMatureViolence.Name = "chkRatingMatureViolence";
            this.chkRatingMatureViolence.Size = new System.Drawing.Size(113, 17);
            this.chkRatingMatureViolence.TabIndex = 2;
            this.chkRatingMatureViolence.Text = "mature - violence";
            this.chkRatingMatureViolence.UseVisualStyleBackColor = true;
            // 
            // chkRatingAdultSexualThemes
            // 
            this.chkRatingAdultSexualThemes.AutoSize = true;
            this.chkRatingAdultSexualThemes.Location = new System.Drawing.Point(17, 88);
            this.chkRatingAdultSexualThemes.Name = "chkRatingAdultSexualThemes";
            this.chkRatingAdultSexualThemes.Size = new System.Drawing.Size(134, 17);
            this.chkRatingAdultSexualThemes.TabIndex = 3;
            this.chkRatingAdultSexualThemes.Text = "adult - sexual themes";
            this.chkRatingAdultSexualThemes.UseVisualStyleBackColor = true;
            // 
            // chkRatingAdultStrongViolence
            // 
            this.chkRatingAdultStrongViolence.AutoSize = true;
            this.chkRatingAdultStrongViolence.Location = new System.Drawing.Point(17, 111);
            this.chkRatingAdultStrongViolence.Name = "chkRatingAdultStrongViolence";
            this.chkRatingAdultStrongViolence.Size = new System.Drawing.Size(141, 17);
            this.chkRatingAdultStrongViolence.TabIndex = 4;
            this.chkRatingAdultStrongViolence.Text = "adult - strong violence";
            this.chkRatingAdultStrongViolence.UseVisualStyleBackColor = true;
            // 
            // lbSearchTerms
            // 
            this.lbSearchTerms.AutoSize = true;
            this.lbSearchTerms.Location = new System.Drawing.Point(55, 6);
            this.lbSearchTerms.Name = "lbSearchTerms";
            this.lbSearchTerms.Size = new System.Drawing.Size(129, 13);
            this.lbSearchTerms.TabIndex = 0;
            this.lbSearchTerms.Text = "Search terms (stackable)";
            // 
            // lbFileNameSchema
            // 
            this.lbFileNameSchema.AutoSize = true;
            this.lbFileNameSchema.Location = new System.Drawing.Point(17, 310);
            this.lbFileNameSchema.Name = "lbFileNameSchema";
            this.lbFileNameSchema.Size = new System.Drawing.Size(95, 13);
            this.lbFileNameSchema.TabIndex = 14;
            this.lbFileNameSchema.Text = "file name schema";
            // 
            // lbFileNameSchemaMultiPost
            // 
            this.lbFileNameSchemaMultiPost.AutoSize = true;
            this.lbFileNameSchemaMultiPost.Location = new System.Drawing.Point(187, 310);
            this.lbFileNameSchemaMultiPost.Name = "lbFileNameSchemaMultiPost";
            this.lbFileNameSchemaMultiPost.Size = new System.Drawing.Size(157, 13);
            this.lbFileNameSchemaMultiPost.TabIndex = 15;
            this.lbFileNameSchemaMultiPost.Text = "file name schema (multi-post)";
            // 
            // chkDownloadGraylistedSubmissions
            // 
            this.chkDownloadGraylistedSubmissions.AutoSize = true;
            this.chkDownloadGraylistedSubmissions.Enabled = false;
            this.chkDownloadGraylistedSubmissions.Location = new System.Drawing.Point(12, 361);
            this.chkDownloadGraylistedSubmissions.Name = "chkDownloadGraylistedSubmissions";
            this.chkDownloadGraylistedSubmissions.Size = new System.Drawing.Size(197, 17);
            this.chkDownloadGraylistedSubmissions.TabIndex = 16;
            this.chkDownloadGraylistedSubmissions.Text = "download graylisted submissions";
            this.chkDownloadGraylistedSubmissions.UseVisualStyleBackColor = true;
            // 
            // chkDownloadBlacklistedSubmissions
            // 
            this.chkDownloadBlacklistedSubmissions.AutoSize = true;
            this.chkDownloadBlacklistedSubmissions.Enabled = false;
            this.chkDownloadBlacklistedSubmissions.Location = new System.Drawing.Point(12, 389);
            this.chkDownloadBlacklistedSubmissions.Name = "chkDownloadBlacklistedSubmissions";
            this.chkDownloadBlacklistedSubmissions.Size = new System.Drawing.Size(202, 17);
            this.chkDownloadBlacklistedSubmissions.TabIndex = 17;
            this.chkDownloadBlacklistedSubmissions.Text = "download blacklisted submissions";
            this.chkDownloadBlacklistedSubmissions.UseVisualStyleBackColor = true;
            // 
            // llbSchemaHelp
            // 
            this.llbSchemaHelp.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.llbSchemaHelp.AutoSize = true;
            this.llbSchemaHelp.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llbSchemaHelp.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.llbSchemaHelp.Location = new System.Drawing.Point(360, 331);
            this.llbSchemaHelp.Name = "llbSchemaHelp";
            this.llbSchemaHelp.Size = new System.Drawing.Size(17, 21);
            this.llbSchemaHelp.TabIndex = 13;
            this.llbSchemaHelp.TabStop = true;
            this.llbSchemaHelp.Text = "?";
            this.llbSchemaHelp.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.llbSchemaHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbSchemaHelp_LinkClicked);
            // 
            // txtFileNameSchemaMultiPost
            // 
            this.txtFileNameSchemaMultiPost.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtFileNameSchemaMultiPost.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtFileNameSchemaMultiPost.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFileNameSchemaMultiPost.ButtonImageIndex = -1;
            this.txtFileNameSchemaMultiPost.ButtonImageKey = "";
            this.txtFileNameSchemaMultiPost.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtFileNameSchemaMultiPost.ButtonText = "";
            this.txtFileNameSchemaMultiPost.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtFileNameSchemaMultiPost.Location = new System.Drawing.Point(190, 331);
            this.txtFileNameSchemaMultiPost.Name = "txtFileNameSchemaMultiPost";
            this.txtFileNameSchemaMultiPost.Size = new System.Drawing.Size(164, 22);
            this.txtFileNameSchemaMultiPost.TabIndex = 12;
            this.txtFileNameSchemaMultiPost.TextHint = "%id%_%multipostindex%";
            // 
            // txtFileNameSchema
            // 
            this.txtFileNameSchema.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtFileNameSchema.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtFileNameSchema.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFileNameSchema.ButtonImageIndex = -1;
            this.txtFileNameSchema.ButtonImageKey = "";
            this.txtFileNameSchema.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtFileNameSchema.ButtonText = "";
            this.txtFileNameSchema.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtFileNameSchema.Location = new System.Drawing.Point(20, 331);
            this.txtFileNameSchema.Name = "txtFileNameSchema";
            this.txtFileNameSchema.Size = new System.Drawing.Size(164, 22);
            this.txtFileNameSchema.TabIndex = 11;
            this.txtFileNameSchema.TextHint = "%id%";
            // 
            // txtSearchUsersFavorites
            // 
            this.txtSearchUsersFavorites.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtSearchUsersFavorites.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtSearchUsersFavorites.ButtonEnabled = true;
            this.txtSearchUsersFavorites.ButtonFont = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchUsersFavorites.ButtonImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtSearchUsersFavorites.ButtonImageIndex = -1;
            this.txtSearchUsersFavorites.ButtonImageKey = "";
            this.txtSearchUsersFavorites.ButtonSize = new System.Drawing.Size(22, 19);
            this.txtSearchUsersFavorites.ButtonText = "X";
            this.txtSearchUsersFavorites.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtSearchUsersFavorites.Enabled = false;
            this.txtSearchUsersFavorites.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchUsersFavorites.Location = new System.Drawing.Point(37, 87);
            this.txtSearchUsersFavorites.Name = "txtSearchUsersFavorites";
            this.txtSearchUsersFavorites.Size = new System.Drawing.Size(166, 20);
            this.txtSearchUsersFavorites.TabIndex = 3;
            this.txtSearchUsersFavorites.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSearchUsersFavorites.TextHint = "users\' favorites (id)";
            this.txtSearchUsersFavorites.ButtonClick += new System.EventHandler(this.txtSearchUsersFavorites_ButtonClick);
            this.txtSearchUsersFavorites.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDownloadOnReturn_KeyPress);
            // 
            // txtSearchArtistName
            // 
            this.txtSearchArtistName.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtSearchArtistName.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtSearchArtistName.ButtonEnabled = true;
            this.txtSearchArtistName.ButtonFont = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchArtistName.ButtonImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtSearchArtistName.ButtonImageIndex = -1;
            this.txtSearchArtistName.ButtonImageKey = "";
            this.txtSearchArtistName.ButtonSize = new System.Drawing.Size(22, 19);
            this.txtSearchArtistName.ButtonText = "X";
            this.txtSearchArtistName.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtSearchArtistName.Enabled = false;
            this.txtSearchArtistName.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchArtistName.Location = new System.Drawing.Point(37, 56);
            this.txtSearchArtistName.Name = "txtSearchArtistName";
            this.txtSearchArtistName.Size = new System.Drawing.Size(166, 20);
            this.txtSearchArtistName.TabIndex = 2;
            this.txtSearchArtistName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSearchArtistName.TextHint = "artists\' gallery";
            this.txtSearchArtistName.ButtonClick += new System.EventHandler(this.txtSearchArtistName_ButtonClick);
            this.txtSearchArtistName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDownloadOnReturn_KeyPress);
            // 
            // txtSearchKeywordsMD5
            // 
            this.txtSearchKeywordsMD5.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtSearchKeywordsMD5.ButtonCursor = System.Windows.Forms.Cursors.Default;
            this.txtSearchKeywordsMD5.ButtonEnabled = true;
            this.txtSearchKeywordsMD5.ButtonFont = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchKeywordsMD5.ButtonImageIndex = -1;
            this.txtSearchKeywordsMD5.ButtonImageKey = "";
            this.txtSearchKeywordsMD5.ButtonSize = new System.Drawing.Size(22, 19);
            this.txtSearchKeywordsMD5.ButtonText = "X";
            this.txtSearchKeywordsMD5.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtSearchKeywordsMD5.Enabled = false;
            this.txtSearchKeywordsMD5.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchKeywordsMD5.Location = new System.Drawing.Point(14, 25);
            this.txtSearchKeywordsMD5.Name = "txtSearchKeywordsMD5";
            this.txtSearchKeywordsMD5.Size = new System.Drawing.Size(212, 20);
            this.txtSearchKeywordsMD5.TabIndex = 1;
            this.txtSearchKeywordsMD5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSearchKeywordsMD5.TextHint = "keywords...";
            this.txtSearchKeywordsMD5.ButtonClick += new System.EventHandler(this.txtSearchKeywordsMD5_ButtonClick);
            this.txtSearchKeywordsMD5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDownloadOnReturn_KeyPress);
            // 
            // numImageLimit
            // 
            this.numImageLimit.Location = new System.Drawing.Point(295, 360);
            this.numImageLimit.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numImageLimit.Name = "numImageLimit";
            this.numImageLimit.Size = new System.Drawing.Size(85, 22);
            this.numImageLimit.TabIndex = 18;
            // 
            // numPageLimit
            // 
            this.numPageLimit.Location = new System.Drawing.Point(296, 388);
            this.numPageLimit.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numPageLimit.Name = "numPageLimit";
            this.numPageLimit.Size = new System.Drawing.Size(84, 22);
            this.numPageLimit.TabIndex = 19;
            // 
            // lbImageLimit
            // 
            this.lbImageLimit.AutoSize = true;
            this.lbImageLimit.Location = new System.Drawing.Point(231, 362);
            this.lbImageLimit.Name = "lbImageLimit";
            this.lbImageLimit.Size = new System.Drawing.Size(63, 13);
            this.lbImageLimit.TabIndex = 20;
            this.lbImageLimit.Text = "image limit";
            // 
            // lbPageLimit
            // 
            this.lbPageLimit.AutoSize = true;
            this.lbPageLimit.Location = new System.Drawing.Point(236, 390);
            this.lbPageLimit.Name = "lbPageLimit";
            this.lbPageLimit.Size = new System.Drawing.Size(58, 13);
            this.lbPageLimit.TabIndex = 21;
            this.lbPageLimit.Text = "page limit";
            // 
            // frmInkBunnyMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(392, 473);
            this.Controls.Add(this.lbPageLimit);
            this.Controls.Add(this.lbImageLimit);
            this.Controls.Add(this.numPageLimit);
            this.Controls.Add(this.numImageLimit);
            this.Controls.Add(this.chkDownloadBlacklistedSubmissions);
            this.Controls.Add(this.chkDownloadGraylistedSubmissions);
            this.Controls.Add(this.lbFileNameSchemaMultiPost);
            this.Controls.Add(this.lbFileNameSchema);
            this.Controls.Add(this.llbSchemaHelp);
            this.Controls.Add(this.txtFileNameSchemaMultiPost);
            this.Controls.Add(this.lbDownloadingAs);
            this.Controls.Add(this.txtFileNameSchema);
            this.Controls.Add(this.lbSearchTerms);
            this.Controls.Add(this.chkSkipDebug);
            this.Controls.Add(this.lbSubmissionType);
            this.Controls.Add(this.clbSubmissionType);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.gbSearchIn);
            this.Controls.Add(this.gbRatings);
            this.Controls.Add(this.txtSearchUsersFavorites);
            this.Controls.Add(this.txtSearchArtistName);
            this.Controls.Add(this.txtSearchKeywordsMD5);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(410, 510);
            this.Menu = this.mMenu;
            this.MinimumSize = new System.Drawing.Size(410, 510);
            this.Name = "frmInkBunnyMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "inkbunny downloader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmInkBunnyMain_FormClosing);
            this.gbSearchIn.ResumeLayout(false);
            this.gbSearchIn.PerformLayout();
            this.gbRatings.ResumeLayout(false);
            this.gbRatings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numImageLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPageLimit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MainMenu mMenu;
        private System.Windows.Forms.MenuItem mLogin;
        private System.Windows.Forms.MenuItem mSettings;
        private System.Windows.Forms.MenuItem mLog;
        private System.Windows.Forms.MenuItem mAbout;
        private System.Windows.Forms.CheckBox chkSkipDebug;
        private System.Windows.Forms.Label lbDownloadingAs;
        private System.Windows.Forms.Label lbSubmissionType;
        private System.Windows.Forms.CheckedListBox clbSubmissionType;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.GroupBox gbSearchIn;
        private System.Windows.Forms.CheckBox chkSearchInMD5Hash;
        private System.Windows.Forms.CheckBox chkSearchInDescriptionOrStory;
        private System.Windows.Forms.CheckBox chkSearchInTitle;
        private System.Windows.Forms.CheckBox chkSearchInKeywords;
        private System.Windows.Forms.GroupBox gbRatings;
        private System.Windows.Forms.CheckBox chkRatingGeneral;
        private System.Windows.Forms.CheckBox chkRatingMatureNudity;
        private System.Windows.Forms.CheckBox chkRatingMatureViolence;
        private System.Windows.Forms.CheckBox chkRatingAdultSexualThemes;
        private System.Windows.Forms.CheckBox chkRatingAdultStrongViolence;
        private murrty.controls.ExtendedTextBox txtSearchUsersFavorites;
        private murrty.controls.ExtendedTextBox txtSearchArtistName;
        private murrty.controls.ExtendedTextBox txtSearchKeywordsMD5;
        private System.Windows.Forms.Label lbSearchTerms;
        private murrty.controls.ExtendedTextBox txtFileNameSchema;
        private murrty.controls.ExtendedTextBox txtFileNameSchemaMultiPost;
        private murrty.controls.ExtendedLinkLabel llbSchemaHelp;
        private System.Windows.Forms.Label lbFileNameSchema;
        private System.Windows.Forms.Label lbFileNameSchemaMultiPost;
        private System.Windows.Forms.MenuItem mSessionIDs;
        private System.Windows.Forms.MenuItem mShowSessionIDs;
        private System.Windows.Forms.MenuItem mShowDecryptedSessionIDs;
        private System.Windows.Forms.MenuItem mCopyDecryptedSessionID;
        private System.Windows.Forms.MenuItem mCopyGuestSessionID;
        private System.Windows.Forms.CheckBox chkDownloadGraylistedSubmissions;
        private System.Windows.Forms.CheckBox chkDownloadBlacklistedSubmissions;
        private System.Windows.Forms.NumericUpDown numImageLimit;
        private System.Windows.Forms.NumericUpDown numPageLimit;
        private System.Windows.Forms.Label lbImageLimit;
        private System.Windows.Forms.Label lbPageLimit;
        private System.Windows.Forms.MenuItem mAccount;
    }
}