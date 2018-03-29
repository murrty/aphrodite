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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbGeneral = new System.Windows.Forms.TabPage();
            this.chkIgnoreFinish = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSaveBlacklisted = new System.Windows.Forms.CheckBox();
            this.chkSaveInfo = new System.Windows.Forms.CheckBox();
            this.btnBrws = new System.Windows.Forms.Button();
            this.txtSaveTo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbTags = new System.Windows.Forms.TabPage();
            this.numLimit = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkMinimumScore = new System.Windows.Forms.CheckBox();
            this.numScore = new System.Windows.Forms.NumericUpDown();
            this.chkSafe = new System.Windows.Forms.CheckBox();
            this.chkQuestionable = new System.Windows.Forms.CheckBox();
            this.chkExplicit = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkSeparate = new System.Windows.Forms.CheckBox();
            this.tbPools = new System.Windows.Forms.TabPage();
            this.chkOpen = new System.Windows.Forms.CheckBox();
            this.chkMerge = new System.Windows.Forms.CheckBox();
            this.chkPoolName = new System.Windows.Forms.CheckBox();
            this.tbImages = new System.Windows.Forms.TabPage();
            this.chkSeparateBlacklist = new System.Windows.Forms.CheckBox();
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
            this.btnBlacklist = new System.Windows.Forms.Button();
            this.JustTheTips = new System.Windows.Forms.ToolTip(this.components);
            this.chkUseForm = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tbGeneral.SuspendLayout();
            this.tbTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numScore)).BeginInit();
            this.tbPools.SuspendLayout();
            this.tbImages.SuspendLayout();
            this.tbProtocol.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(331, 200);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(250, 200);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbGeneral);
            this.tabControl1.Controls.Add(this.tbTags);
            this.tabControl1.Controls.Add(this.tbPools);
            this.tabControl1.Controls.Add(this.tbImages);
            this.tabControl1.Controls.Add(this.tbProtocol);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(418, 194);
            this.tabControl1.TabIndex = 2;
            // 
            // tbGeneral
            // 
            this.tbGeneral.Controls.Add(this.chkIgnoreFinish);
            this.tbGeneral.Controls.Add(this.label1);
            this.tbGeneral.Controls.Add(this.chkSaveBlacklisted);
            this.tbGeneral.Controls.Add(this.chkSaveInfo);
            this.tbGeneral.Controls.Add(this.btnBrws);
            this.tbGeneral.Controls.Add(this.txtSaveTo);
            this.tbGeneral.Controls.Add(this.label3);
            this.tbGeneral.Location = new System.Drawing.Point(4, 22);
            this.tbGeneral.Name = "tbGeneral";
            this.tbGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tbGeneral.Size = new System.Drawing.Size(410, 168);
            this.tbGeneral.TabIndex = 0;
            this.tbGeneral.Text = "General";
            this.tbGeneral.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreFinish
            // 
            this.chkIgnoreFinish.AutoSize = true;
            this.chkIgnoreFinish.Location = new System.Drawing.Point(120, 105);
            this.chkIgnoreFinish.Name = "chkIgnoreFinish";
            this.chkIgnoreFinish.Size = new System.Drawing.Size(171, 17);
            this.chkIgnoreFinish.TabIndex = 21;
            this.chkIgnoreFinish.Text = "Don\'t notify finished downloads";
            this.JustTheTips.SetToolTip(this.chkIgnoreFinish, "Doesn\'t notify you when downloads are completed\r\nPlugin downloads quit when finis" +
        "hed");
            this.chkIgnoreFinish.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(237, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Blacklisted tags are mutual between pools && tags";
            // 
            // chkSaveBlacklisted
            // 
            this.chkSaveBlacklisted.AutoSize = true;
            this.chkSaveBlacklisted.Checked = true;
            this.chkSaveBlacklisted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveBlacklisted.Location = new System.Drawing.Point(121, 82);
            this.chkSaveBlacklisted.Name = "chkSaveBlacklisted";
            this.chkSaveBlacklisted.Size = new System.Drawing.Size(139, 17);
            this.chkSaveBlacklisted.TabIndex = 19;
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
            this.chkSaveInfo.Location = new System.Drawing.Point(120, 59);
            this.chkSaveInfo.Name = "chkSaveInfo";
            this.chkSaveInfo.Size = new System.Drawing.Size(122, 17);
            this.chkSaveInfo.TabIndex = 18;
            this.chkSaveInfo.Text = "Save image info files";
            this.JustTheTips.SetToolTip(this.chkSaveInfo, resources.GetString("chkSaveInfo.ToolTip"));
            this.chkSaveInfo.UseVisualStyleBackColor = true;
            // 
            // btnBrws
            // 
            this.btnBrws.Location = new System.Drawing.Point(362, 22);
            this.btnBrws.Name = "btnBrws";
            this.btnBrws.Size = new System.Drawing.Size(24, 23);
            this.btnBrws.TabIndex = 16;
            this.btnBrws.Text = "...";
            this.btnBrws.UseVisualStyleBackColor = true;
            this.btnBrws.Click += new System.EventHandler(this.btnBrws_Click);
            // 
            // txtSaveTo
            // 
            this.txtSaveTo.Location = new System.Drawing.Point(33, 25);
            this.txtSaveTo.Name = "txtSaveTo";
            this.txtSaveTo.ReadOnly = true;
            this.txtSaveTo.Size = new System.Drawing.Size(323, 20);
            this.txtSaveTo.TabIndex = 15;
            this.JustTheTips.SetToolTip(this.txtSaveTo, "The location where pools, tags, and images will be saved to.");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Save to:";
            // 
            // tbTags
            // 
            this.tbTags.Controls.Add(this.numLimit);
            this.tbTags.Controls.Add(this.label6);
            this.tbTags.Controls.Add(this.label4);
            this.tbTags.Controls.Add(this.chkMinimumScore);
            this.tbTags.Controls.Add(this.numScore);
            this.tbTags.Controls.Add(this.chkSafe);
            this.tbTags.Controls.Add(this.chkQuestionable);
            this.tbTags.Controls.Add(this.chkExplicit);
            this.tbTags.Controls.Add(this.label2);
            this.tbTags.Controls.Add(this.chkSeparate);
            this.tbTags.Location = new System.Drawing.Point(4, 22);
            this.tbTags.Name = "tbTags";
            this.tbTags.Padding = new System.Windows.Forms.Padding(3);
            this.tbTags.Size = new System.Drawing.Size(410, 168);
            this.tbTags.TabIndex = 1;
            this.tbTags.Text = "Tags";
            this.tbTags.UseVisualStyleBackColor = true;
            // 
            // numLimit
            // 
            this.numLimit.Location = new System.Drawing.Point(165, 134);
            this.numLimit.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numLimit.Name = "numLimit";
            this.numLimit.Size = new System.Drawing.Size(81, 20);
            this.numLimit.TabIndex = 38;
            this.numLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.JustTheTips.SetToolTip(this.numLimit, "Limits downloads to a certain amount of images");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(104, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(202, 26);
            this.label6.TabIndex = 37;
            this.label6.Text = "Image download limit (0 = off)\r\n(0 and/or high values isn\'t recommended)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(244, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "Score minimum";
            // 
            // chkMinimumScore
            // 
            this.chkMinimumScore.AutoSize = true;
            this.chkMinimumScore.Location = new System.Drawing.Point(247, 63);
            this.chkMinimumScore.Name = "chkMinimumScore";
            this.chkMinimumScore.Size = new System.Drawing.Size(125, 17);
            this.chkMinimumScore.TabIndex = 35;
            this.chkMinimumScore.Text = "Use a minimum score";
            this.JustTheTips.SetToolTip(this.chkMinimumScore, "Only downloads images with a score equal to or greater than provided");
            this.chkMinimumScore.UseVisualStyleBackColor = true;
            // 
            // numScore
            // 
            this.numScore.Enabled = false;
            this.numScore.Location = new System.Drawing.Point(270, 34);
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
            this.numScore.TabIndex = 34;
            this.numScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.JustTheTips.SetToolTip(this.numScore, "The minimum score that will be downloaded");
            // 
            // chkSafe
            // 
            this.chkSafe.AutoSize = true;
            this.chkSafe.Checked = true;
            this.chkSafe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSafe.Location = new System.Drawing.Point(124, 35);
            this.chkSafe.Name = "chkSafe";
            this.chkSafe.Size = new System.Drawing.Size(32, 17);
            this.chkSafe.TabIndex = 33;
            this.chkSafe.Text = "S";
            this.JustTheTips.SetToolTip(this.chkSafe, "Download images rated Safe");
            this.chkSafe.UseVisualStyleBackColor = true;
            // 
            // chkQuestionable
            // 
            this.chkQuestionable.AutoSize = true;
            this.chkQuestionable.Checked = true;
            this.chkQuestionable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkQuestionable.Location = new System.Drawing.Point(85, 35);
            this.chkQuestionable.Name = "chkQuestionable";
            this.chkQuestionable.Size = new System.Drawing.Size(33, 17);
            this.chkQuestionable.TabIndex = 32;
            this.chkQuestionable.Text = "Q";
            this.JustTheTips.SetToolTip(this.chkQuestionable, "Download images rated Questionable");
            this.chkQuestionable.UseVisualStyleBackColor = true;
            // 
            // chkExplicit
            // 
            this.chkExplicit.AutoSize = true;
            this.chkExplicit.Checked = true;
            this.chkExplicit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExplicit.Location = new System.Drawing.Point(47, 36);
            this.chkExplicit.Name = "chkExplicit";
            this.chkExplicit.Size = new System.Drawing.Size(32, 17);
            this.chkExplicit.TabIndex = 31;
            this.chkExplicit.Text = "E";
            this.JustTheTips.SetToolTip(this.chkExplicit, "Download images rated Explicit");
            this.chkExplicit.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Ratings to download";
            // 
            // chkSeparate
            // 
            this.chkSeparate.AutoSize = true;
            this.chkSeparate.Checked = true;
            this.chkSeparate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSeparate.Location = new System.Drawing.Point(47, 63);
            this.chkSeparate.Name = "chkSeparate";
            this.chkSeparate.Size = new System.Drawing.Size(102, 17);
            this.chkSeparate.TabIndex = 26;
            this.chkSeparate.Text = "Separate ratings";
            this.JustTheTips.SetToolTip(this.chkSeparate, "Separate ratings into different folders");
            this.chkSeparate.UseVisualStyleBackColor = true;
            // 
            // tbPools
            // 
            this.tbPools.Controls.Add(this.chkOpen);
            this.tbPools.Controls.Add(this.chkMerge);
            this.tbPools.Controls.Add(this.chkPoolName);
            this.tbPools.Location = new System.Drawing.Point(4, 22);
            this.tbPools.Name = "tbPools";
            this.tbPools.Padding = new System.Windows.Forms.Padding(3);
            this.tbPools.Size = new System.Drawing.Size(410, 168);
            this.tbPools.TabIndex = 2;
            this.tbPools.Text = "Pools";
            this.tbPools.UseVisualStyleBackColor = true;
            // 
            // chkOpen
            // 
            this.chkOpen.AutoSize = true;
            this.chkOpen.Location = new System.Drawing.Point(67, 99);
            this.chkOpen.Name = "chkOpen";
            this.chkOpen.Size = new System.Drawing.Size(138, 17);
            this.chkOpen.TabIndex = 19;
            this.chkOpen.Text = "Open after downloading";
            this.JustTheTips.SetToolTip(this.chkOpen, "Opens the pool folder after downloading");
            this.chkOpen.UseVisualStyleBackColor = true;
            // 
            // chkMerge
            // 
            this.chkMerge.AutoSize = true;
            this.chkMerge.Checked = true;
            this.chkMerge.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMerge.Location = new System.Drawing.Point(67, 76);
            this.chkMerge.Name = "chkMerge";
            this.chkMerge.Size = new System.Drawing.Size(276, 17);
            this.chkMerge.TabIndex = 13;
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
            this.chkPoolName.Location = new System.Drawing.Point(67, 53);
            this.chkPoolName.Name = "chkPoolName";
            this.chkPoolName.Size = new System.Drawing.Size(178, 17);
            this.chkPoolName.TabIndex = 4;
            this.chkPoolName.Text = "Save files as \"poolname_####\"";
            this.JustTheTips.SetToolTip(this.chkPoolName, "Save pool files as the pool name + page number.\r\nEx: The_Internship_05.jpg");
            this.chkPoolName.UseVisualStyleBackColor = true;
            // 
            // tbImages
            // 
            this.tbImages.Controls.Add(this.chkUseForm);
            this.tbImages.Controls.Add(this.chkSeparateBlacklist);
            this.tbImages.Controls.Add(this.chkSeparateImages);
            this.tbImages.Controls.Add(this.rbMD5);
            this.tbImages.Controls.Add(this.label9);
            this.tbImages.Controls.Add(this.label8);
            this.tbImages.Controls.Add(this.rbArtist);
            this.tbImages.Location = new System.Drawing.Point(4, 22);
            this.tbImages.Name = "tbImages";
            this.tbImages.Padding = new System.Windows.Forms.Padding(3);
            this.tbImages.Size = new System.Drawing.Size(410, 168);
            this.tbImages.TabIndex = 4;
            this.tbImages.Text = "Images";
            this.tbImages.UseVisualStyleBackColor = true;
            // 
            // chkSeparateBlacklist
            // 
            this.chkSeparateBlacklist.AutoSize = true;
            this.chkSeparateBlacklist.Checked = true;
            this.chkSeparateBlacklist.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSeparateBlacklist.Location = new System.Drawing.Point(186, 72);
            this.chkSeparateBlacklist.Name = "chkSeparateBlacklist";
            this.chkSeparateBlacklist.Size = new System.Drawing.Size(157, 17);
            this.chkSeparateBlacklist.TabIndex = 10;
            this.chkSeparateBlacklist.Text = "Separate blacklisted images";
            this.JustTheTips.SetToolTip(this.chkSeparateBlacklist, "Separates blacklisted images into separate folder.");
            this.chkSeparateBlacklist.UseVisualStyleBackColor = true;
            // 
            // chkSeparateImages
            // 
            this.chkSeparateImages.AutoSize = true;
            this.chkSeparateImages.Checked = true;
            this.chkSeparateImages.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSeparateImages.Location = new System.Drawing.Point(67, 72);
            this.chkSeparateImages.Name = "chkSeparateImages";
            this.chkSeparateImages.Size = new System.Drawing.Size(102, 17);
            this.chkSeparateImages.TabIndex = 9;
            this.chkSeparateImages.Text = "Separate ratings";
            this.JustTheTips.SetToolTip(this.chkSeparateImages, "Separate ratings into separate folders for images");
            this.chkSeparateImages.UseVisualStyleBackColor = true;
            // 
            // rbMD5
            // 
            this.rbMD5.AutoSize = true;
            this.rbMD5.Location = new System.Drawing.Point(222, 38);
            this.rbMD5.Name = "rbMD5";
            this.rbMD5.Size = new System.Drawing.Size(126, 17);
            this.rbMD5.TabIndex = 8;
            this.rbMD5.Text = "Save images as \'md5\'";
            this.JustTheTips.SetToolTip(this.rbMD5, "Saves files as the MD5 hash of the image\r\nex: 7ec39fcc0afe7b237d61b1afdfb9b927.jp" +
        "g");
            this.rbMD5.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label9.Location = new System.Drawing.Point(59, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(292, 2);
            this.label9.TabIndex = 7;
            this.label9.Text = "bark";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(63, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(285, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "These options only effect the \'images:\' protocol + userscript";
            // 
            // rbArtist
            // 
            this.rbArtist.AutoSize = true;
            this.rbArtist.Checked = true;
            this.rbArtist.Location = new System.Drawing.Point(62, 38);
            this.rbArtist.Name = "rbArtist";
            this.rbArtist.Size = new System.Drawing.Size(154, 17);
            this.rbArtist.TabIndex = 0;
            this.rbArtist.TabStop = true;
            this.rbArtist.Text = "Save images as \'artist_md5\'";
            this.JustTheTips.SetToolTip(this.rbArtist, "Saves images as the artist and the MD5 hash of the file.\r\nex: stigmata_7ec39fcc0a" +
        "fe7b237d61b1afdfb9b927.jpg");
            this.rbArtist.UseVisualStyleBackColor = true;
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
            this.tbProtocol.Size = new System.Drawing.Size(410, 168);
            this.tbProtocol.TabIndex = 3;
            this.tbProtocol.Text = "Protocols";
            this.tbProtocol.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label7.Location = new System.Drawing.Point(59, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(292, 2);
            this.label7.TabIndex = 6;
            this.label7.Text = "bark";
            // 
            // btnImagesUserscript
            // 
            this.btnImagesUserscript.Location = new System.Drawing.Point(246, 134);
            this.btnImagesUserscript.Name = "btnImagesUserscript";
            this.btnImagesUserscript.Size = new System.Drawing.Size(75, 23);
            this.btnImagesUserscript.TabIndex = 5;
            this.btnImagesUserscript.Text = "Userscript";
            this.btnImagesUserscript.UseVisualStyleBackColor = true;
            this.btnImagesUserscript.Click += new System.EventHandler(this.btnImagesUserscript_Click);
            // 
            // btnUserscript
            // 
            this.btnUserscript.Location = new System.Drawing.Point(246, 81);
            this.btnUserscript.Name = "btnUserscript";
            this.btnUserscript.Size = new System.Drawing.Size(75, 23);
            this.btnUserscript.TabIndex = 4;
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
            this.btnTagsProtocol.Location = new System.Drawing.Point(89, 69);
            this.btnTagsProtocol.Name = "btnTagsProtocol";
            this.btnTagsProtocol.Size = new System.Drawing.Size(151, 23);
            this.btnTagsProtocol.TabIndex = 2;
            this.btnTagsProtocol.Text = "Install \'tags:\' protocol";
            this.btnTagsProtocol.UseVisualStyleBackColor = true;
            this.btnTagsProtocol.Click += new System.EventHandler(this.btnTagsProtocol_Click);
            // 
            // btnPoolsProtocol
            // 
            this.btnPoolsProtocol.Location = new System.Drawing.Point(89, 94);
            this.btnPoolsProtocol.Name = "btnPoolsProtocol";
            this.btnPoolsProtocol.Size = new System.Drawing.Size(151, 23);
            this.btnPoolsProtocol.TabIndex = 1;
            this.btnPoolsProtocol.Text = "Install \'pools:\' protocol";
            this.btnPoolsProtocol.UseVisualStyleBackColor = true;
            this.btnPoolsProtocol.Click += new System.EventHandler(this.btnPoolsProtocol_Click);
            // 
            // btnImagesProtocol
            // 
            this.btnImagesProtocol.Location = new System.Drawing.Point(89, 134);
            this.btnImagesProtocol.Name = "btnImagesProtocol";
            this.btnImagesProtocol.Size = new System.Drawing.Size(151, 23);
            this.btnImagesProtocol.TabIndex = 0;
            this.btnImagesProtocol.Text = "Install \'image:\' protocol";
            this.btnImagesProtocol.UseVisualStyleBackColor = true;
            this.btnImagesProtocol.Click += new System.EventHandler(this.btnImagesProtocol_Click);
            // 
            // btnBlacklist
            // 
            this.btnBlacklist.Location = new System.Drawing.Point(4, 200);
            this.btnBlacklist.Name = "btnBlacklist";
            this.btnBlacklist.Size = new System.Drawing.Size(102, 23);
            this.btnBlacklist.TabIndex = 19;
            this.btnBlacklist.Text = "Manage blacklist";
            this.JustTheTips.SetToolTip(this.btnBlacklist, "Manage your blacklist");
            this.btnBlacklist.UseVisualStyleBackColor = true;
            this.btnBlacklist.Click += new System.EventHandler(this.btnBlacklist_Click);
            // 
            // chkUseForm
            // 
            this.chkUseForm.AutoSize = true;
            this.chkUseForm.Location = new System.Drawing.Point(94, 100);
            this.chkUseForm.Name = "chkUseForm";
            this.chkUseForm.Size = new System.Drawing.Size(223, 17);
            this.chkUseForm.TabIndex = 11;
            this.chkUseForm.Text = "Use download form for download progress";
            this.JustTheTips.SetToolTip(this.chkUseForm, "Shows a form when downloading images that will report progress");
            this.chkUseForm.UseVisualStyleBackColor = true;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(418, 231);
            this.Controls.Add(this.btnBlacklist);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmSettings";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "aphrodite settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.tabControl1.ResumeLayout(false);
            this.tbGeneral.ResumeLayout(false);
            this.tbGeneral.PerformLayout();
            this.tbTags.ResumeLayout(false);
            this.tbTags.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numScore)).EndInit();
            this.tbPools.ResumeLayout(false);
            this.tbPools.PerformLayout();
            this.tbImages.ResumeLayout(false);
            this.tbImages.PerformLayout();
            this.tbProtocol.ResumeLayout(false);
            this.tbProtocol.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbGeneral;
        private System.Windows.Forms.TabPage tbTags;
        private System.Windows.Forms.TabPage tbPools;
        private System.Windows.Forms.Button btnBrws;
        private System.Windows.Forms.TextBox txtSaveTo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkSaveInfo;
        private System.Windows.Forms.Button btnBlacklist;
        private System.Windows.Forms.CheckBox chkSaveBlacklisted;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkSeparate;
        private System.Windows.Forms.CheckBox chkSafe;
        private System.Windows.Forms.CheckBox chkQuestionable;
        private System.Windows.Forms.CheckBox chkExplicit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkMinimumScore;
        private System.Windows.Forms.NumericUpDown numScore;
        private System.Windows.Forms.NumericUpDown numLimit;
        private System.Windows.Forms.Label label6;
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
        private System.Windows.Forms.CheckBox chkSeparateBlacklist;
        private System.Windows.Forms.CheckBox chkUseForm;

    }
}