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
            this.tbMain = new System.Windows.Forms.TabControl();
            this.tbTags = new System.Windows.Forms.TabPage();
            this.btnDownloadTags = new System.Windows.Forms.Button();
            this.chkSafe = new System.Windows.Forms.CheckBox();
            this.chkQuestionable = new System.Windows.Forms.CheckBox();
            this.chkExplicit = new System.Windows.Forms.CheckBox();
            this.numLimit = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.chkMinimumScore = new System.Windows.Forms.CheckBox();
            this.numScore = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTags = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPools = new System.Windows.Forms.TabPage();
            this.chkMerge = new System.Windows.Forms.CheckBox();
            this.chkOpen = new System.Windows.Forms.CheckBox();
            this.btnDownloadPool = new System.Windows.Forms.Button();
            this.txtID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbSearch = new System.Windows.Forms.TabPage();
            this.btnHLQ = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mSettings = new System.Windows.Forms.MenuItem();
            this.mBlacklist = new System.Windows.Forms.MenuItem();
            this.mAbout = new System.Windows.Forms.MenuItem();
            this.mProtocol = new System.Windows.Forms.MenuItem();
            this.tbMain.SuspendLayout();
            this.tbTags.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numScore)).BeginInit();
            this.tbPools.SuspendLayout();
            this.tbSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbMain
            // 
            this.tbMain.Controls.Add(this.tbTags);
            this.tbMain.Controls.Add(this.tbPools);
            this.tbMain.Controls.Add(this.tbSearch);
            this.tbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbMain.Location = new System.Drawing.Point(0, 0);
            this.tbMain.Name = "tbMain";
            this.tbMain.SelectedIndex = 0;
            this.tbMain.Size = new System.Drawing.Size(282, 203);
            this.tbMain.TabIndex = 12;
            this.tbMain.SelectedIndexChanged += new System.EventHandler(this.tbMain_SelectedIndexChanged);
            // 
            // tbTags
            // 
            this.tbTags.Controls.Add(this.btnDownloadTags);
            this.tbTags.Controls.Add(this.chkSafe);
            this.tbTags.Controls.Add(this.chkQuestionable);
            this.tbTags.Controls.Add(this.chkExplicit);
            this.tbTags.Controls.Add(this.numLimit);
            this.tbTags.Controls.Add(this.label6);
            this.tbTags.Controls.Add(this.chkMinimumScore);
            this.tbTags.Controls.Add(this.numScore);
            this.tbTags.Controls.Add(this.label2);
            this.tbTags.Controls.Add(this.txtTags);
            this.tbTags.Controls.Add(this.label1);
            this.tbTags.Location = new System.Drawing.Point(4, 22);
            this.tbTags.Name = "tbTags";
            this.tbTags.Padding = new System.Windows.Forms.Padding(3);
            this.tbTags.Size = new System.Drawing.Size(274, 177);
            this.tbTags.TabIndex = 0;
            this.tbTags.Text = "Tags";
            this.tbTags.UseVisualStyleBackColor = true;
            // 
            // btnDownloadTags
            // 
            this.btnDownloadTags.Location = new System.Drawing.Point(97, 164);
            this.btnDownloadTags.Name = "btnDownloadTags";
            this.btnDownloadTags.Size = new System.Drawing.Size(81, 23);
            this.btnDownloadTags.TabIndex = 7;
            this.btnDownloadTags.Text = "Download";
            this.btnDownloadTags.UseVisualStyleBackColor = true;
            this.btnDownloadTags.Click += new System.EventHandler(this.btnDownloadTags_Click);
            // 
            // chkSafe
            // 
            this.chkSafe.AutoSize = true;
            this.chkSafe.Checked = true;
            this.chkSafe.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSafe.Location = new System.Drawing.Point(95, 76);
            this.chkSafe.Name = "chkSafe";
            this.chkSafe.Size = new System.Drawing.Size(32, 17);
            this.chkSafe.TabIndex = 3;
            this.chkSafe.Text = "S";
            this.chkSafe.UseVisualStyleBackColor = true;
            // 
            // chkQuestionable
            // 
            this.chkQuestionable.AutoSize = true;
            this.chkQuestionable.Checked = true;
            this.chkQuestionable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkQuestionable.Location = new System.Drawing.Point(56, 76);
            this.chkQuestionable.Name = "chkQuestionable";
            this.chkQuestionable.Size = new System.Drawing.Size(33, 17);
            this.chkQuestionable.TabIndex = 2;
            this.chkQuestionable.Text = "Q";
            this.chkQuestionable.UseVisualStyleBackColor = true;
            // 
            // chkExplicit
            // 
            this.chkExplicit.AutoSize = true;
            this.chkExplicit.Checked = true;
            this.chkExplicit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExplicit.Location = new System.Drawing.Point(18, 76);
            this.chkExplicit.Name = "chkExplicit";
            this.chkExplicit.Size = new System.Drawing.Size(32, 17);
            this.chkExplicit.TabIndex = 1;
            this.chkExplicit.Text = "E";
            this.chkExplicit.UseVisualStyleBackColor = true;
            // 
            // numLimit
            // 
            this.numLimit.Location = new System.Drawing.Point(97, 134);
            this.numLimit.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numLimit.Name = "numLimit";
            this.numLimit.Size = new System.Drawing.Size(81, 20);
            this.numLimit.TabIndex = 6;
            this.numLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(36, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(202, 26);
            this.label6.TabIndex = 13;
            this.label6.Text = "Image download limit (0 = off)\r\n(0 and/or high values isn\'t recommended)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // chkMinimumScore
            // 
            this.chkMinimumScore.AutoSize = true;
            this.chkMinimumScore.Location = new System.Drawing.Point(152, 55);
            this.chkMinimumScore.Name = "chkMinimumScore";
            this.chkMinimumScore.Size = new System.Drawing.Size(96, 17);
            this.chkMinimumScore.TabIndex = 4;
            this.chkMinimumScore.Text = "Score minimum";
            this.chkMinimumScore.UseVisualStyleBackColor = true;
            this.chkMinimumScore.CheckedChanged += new System.EventHandler(this.chkMinimumScore_CheckedChanged);
            // 
            // numScore
            // 
            this.numScore.Enabled = false;
            this.numScore.Location = new System.Drawing.Point(173, 76);
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
            this.numScore.TabIndex = 5;
            this.numScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Ratings";
            // 
            // txtTags
            // 
            this.txtTags.Location = new System.Drawing.Point(38, 27);
            this.txtTags.Name = "txtTags";
            this.txtTags.Size = new System.Drawing.Size(223, 20);
            this.txtTags.TabIndex = 0;
            this.txtTags.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTags.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTags_KeyDown);
            this.txtTags.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTags_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Tags (limit 6):";
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
            this.tbPools.Size = new System.Drawing.Size(274, 177);
            this.tbPools.TabIndex = 1;
            this.tbPools.Text = "Pools";
            this.tbPools.UseVisualStyleBackColor = true;
            // 
            // chkMerge
            // 
            this.chkMerge.AutoSize = true;
            this.chkMerge.Location = new System.Drawing.Point(53, 110);
            this.chkMerge.Name = "chkMerge";
            this.chkMerge.Size = new System.Drawing.Size(168, 17);
            this.chkMerge.TabIndex = 10;
            this.chkMerge.Text = "Merge blacklisted with the rest";
            this.chkMerge.UseVisualStyleBackColor = true;
            // 
            // chkOpen
            // 
            this.chkOpen.AutoSize = true;
            this.chkOpen.Location = new System.Drawing.Point(53, 87);
            this.chkOpen.Name = "chkOpen";
            this.chkOpen.Size = new System.Drawing.Size(138, 17);
            this.chkOpen.TabIndex = 9;
            this.chkOpen.Text = "Open after downloading";
            this.chkOpen.UseVisualStyleBackColor = true;
            // 
            // btnDownloadPool
            // 
            this.btnDownloadPool.Location = new System.Drawing.Point(98, 145);
            this.btnDownloadPool.Name = "btnDownloadPool";
            this.btnDownloadPool.Size = new System.Drawing.Size(79, 23);
            this.btnDownloadPool.TabIndex = 11;
            this.btnDownloadPool.Text = "Download";
            this.btnDownloadPool.UseVisualStyleBackColor = true;
            this.btnDownloadPool.Click += new System.EventHandler(this.btnDownloadPool_Click);
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(98, 47);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(79, 20);
            this.txtID.TabIndex = 8;
            this.txtID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtID.TextChanged += new System.EventHandler(this.txtID_TextChanged);
            this.txtID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtID_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(73, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Pool ID:";
            // 
            // tbSearch
            // 
            this.tbSearch.Controls.Add(this.btnHLQ);
            this.tbSearch.Controls.Add(this.label4);
            this.tbSearch.Location = new System.Drawing.Point(4, 22);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tbSearch.Size = new System.Drawing.Size(274, 177);
            this.tbSearch.TabIndex = 2;
            this.tbSearch.Text = "Reverse image search";
            this.tbSearch.UseVisualStyleBackColor = true;
            // 
            // btnHLQ
            // 
            this.btnHLQ.Location = new System.Drawing.Point(99, 89);
            this.btnHLQ.Name = "btnHLQ";
            this.btnHLQ.Size = new System.Drawing.Size(76, 23);
            this.btnHLQ.TabIndex = 0;
            this.btnHLQ.Text = "go there";
            this.btnHLQ.UseVisualStyleBackColor = true;
            this.btnHLQ.Click += new System.EventHandler(this.btnHLQ_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(216, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "There\'s a site to reverse image search e621.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mSettings,
            this.mBlacklist,
            this.mAbout,
            this.mProtocol});
            // 
            // mSettings
            // 
            this.mSettings.Index = 0;
            this.mSettings.Text = "Settings";
            this.mSettings.Click += new System.EventHandler(this.mSettings_Click);
            // 
            // mBlacklist
            // 
            this.mBlacklist.Index = 1;
            this.mBlacklist.Text = "Blacklist";
            this.mBlacklist.Click += new System.EventHandler(this.mBlacklist_Click);
            // 
            // mAbout
            // 
            this.mAbout.Index = 2;
            this.mAbout.Text = "About";
            this.mAbout.Click += new System.EventHandler(this.mAbout_Click);
            // 
            // mProtocol
            // 
            this.mProtocol.Enabled = false;
            this.mProtocol.Index = 3;
            this.mProtocol.Text = "Protocol";
            this.mProtocol.Visible = false;
            this.mProtocol.Click += new System.EventHandler(this.mProtocol_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(282, 203);
            this.Controls.Add(this.tbMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(290, 274);
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(290, 274);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "aphrodite";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tbMain.ResumeLayout(false);
            this.tbTags.ResumeLayout(false);
            this.tbTags.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numScore)).EndInit();
            this.tbPools.ResumeLayout(false);
            this.tbPools.PerformLayout();
            this.tbSearch.ResumeLayout(false);
            this.tbSearch.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbMain;
        private System.Windows.Forms.TabPage tbTags;
        private System.Windows.Forms.TabPage tbPools;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem mSettings;
        private System.Windows.Forms.MenuItem mAbout;
        private System.Windows.Forms.MenuItem mBlacklist;
        private System.Windows.Forms.Button btnDownloadTags;
        private System.Windows.Forms.CheckBox chkSafe;
        private System.Windows.Forms.CheckBox chkQuestionable;
        private System.Windows.Forms.CheckBox chkExplicit;
        private System.Windows.Forms.NumericUpDown numLimit;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkMinimumScore;
        private System.Windows.Forms.NumericUpDown numScore;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTags;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkMerge;
        private System.Windows.Forms.CheckBox chkOpen;
        private System.Windows.Forms.Button btnDownloadPool;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MenuItem mProtocol;
        private System.Windows.Forms.TabPage tbSearch;
        private System.Windows.Forms.Button btnHLQ;
        private System.Windows.Forms.Label label4;
    }
}

