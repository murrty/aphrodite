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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbGeneral = new System.Windows.Forms.TabPage();
            this.tbTags = new System.Windows.Forms.TabPage();
            this.tbPools = new System.Windows.Forms.TabPage();
            this.btnBrws = new System.Windows.Forms.Button();
            this.txtSaveTo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkSaveInfo = new System.Windows.Forms.CheckBox();
            this.btnBlacklist = new System.Windows.Forms.Button();
            this.chkSaveBlacklisted = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSeparate = new System.Windows.Forms.CheckBox();
            this.chkSafe = new System.Windows.Forms.CheckBox();
            this.chkQuestionable = new System.Windows.Forms.CheckBox();
            this.chkExplicit = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkMinimumScore = new System.Windows.Forms.CheckBox();
            this.numScore = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numLimit = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.chkPoolName = new System.Windows.Forms.CheckBox();
            this.chkMerge = new System.Windows.Forms.CheckBox();
            this.chkOpen = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tbGeneral.SuspendLayout();
            this.tbTags.SuspendLayout();
            this.tbPools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLimit)).BeginInit();
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
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(418, 194);
            this.tabControl1.TabIndex = 2;
            // 
            // tbGeneral
            // 
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
            // btnBrws
            // 
            this.btnBrws.Location = new System.Drawing.Point(378, 23);
            this.btnBrws.Name = "btnBrws";
            this.btnBrws.Size = new System.Drawing.Size(24, 23);
            this.btnBrws.TabIndex = 16;
            this.btnBrws.Text = "...";
            this.btnBrws.UseVisualStyleBackColor = true;
            // 
            // txtSaveTo
            // 
            this.txtSaveTo.Location = new System.Drawing.Point(33, 25);
            this.txtSaveTo.Name = "txtSaveTo";
            this.txtSaveTo.ReadOnly = true;
            this.txtSaveTo.Size = new System.Drawing.Size(339, 20);
            this.txtSaveTo.TabIndex = 15;
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
            // chkSaveInfo
            // 
            this.chkSaveInfo.AutoSize = true;
            this.chkSaveInfo.Location = new System.Drawing.Point(10, 53);
            this.chkSaveInfo.Name = "chkSaveInfo";
            this.chkSaveInfo.Size = new System.Drawing.Size(391, 17);
            this.chkSaveInfo.TabIndex = 18;
            this.chkSaveInfo.Text = "Save image info (tags.nfo, pool.nfo, tags.blacklisted.nfo, pools.blacklisted.nfo)" +
    "";
            this.chkSaveInfo.UseVisualStyleBackColor = true;
            // 
            // btnBlacklist
            // 
            this.btnBlacklist.Location = new System.Drawing.Point(4, 200);
            this.btnBlacklist.Name = "btnBlacklist";
            this.btnBlacklist.Size = new System.Drawing.Size(102, 23);
            this.btnBlacklist.TabIndex = 19;
            this.btnBlacklist.Text = "Manage blacklist";
            this.btnBlacklist.UseVisualStyleBackColor = true;
            this.btnBlacklist.Click += new System.EventHandler(this.btnBlacklist_Click);
            // 
            // chkSaveBlacklisted
            // 
            this.chkSaveBlacklisted.AutoSize = true;
            this.chkSaveBlacklisted.Location = new System.Drawing.Point(11, 76);
            this.chkSaveBlacklisted.Name = "chkSaveBlacklisted";
            this.chkSaveBlacklisted.Size = new System.Drawing.Size(139, 17);
            this.chkSaveBlacklisted.TabIndex = 19;
            this.chkSaveBlacklisted.Text = "Save blacklisted images";
            this.chkSaveBlacklisted.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(237, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Blacklisted tags are mutual between pools && tags";
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
            this.chkSeparate.UseVisualStyleBackColor = true;
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
            // chkMinimumScore
            // 
            this.chkMinimumScore.AutoSize = true;
            this.chkMinimumScore.Location = new System.Drawing.Point(247, 63);
            this.chkMinimumScore.Name = "chkMinimumScore";
            this.chkMinimumScore.Size = new System.Drawing.Size(125, 17);
            this.chkMinimumScore.TabIndex = 35;
            this.chkMinimumScore.Text = "Use a minimum score";
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
            // numLimit
            // 
            this.numLimit.Location = new System.Drawing.Point(165, 136);
            this.numLimit.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numLimit.Name = "numLimit";
            this.numLimit.Size = new System.Drawing.Size(81, 20);
            this.numLimit.TabIndex = 38;
            this.numLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(104, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(202, 26);
            this.label6.TabIndex = 37;
            this.label6.Text = "Image download limit (0 = off)\r\n(0 and/or high values isn\'t recommended)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
            this.chkPoolName.UseVisualStyleBackColor = true;
            // 
            // chkMerge
            // 
            this.chkMerge.AutoSize = true;
            this.chkMerge.Location = new System.Drawing.Point(67, 76);
            this.chkMerge.Name = "chkMerge";
            this.chkMerge.Size = new System.Drawing.Size(276, 17);
            this.chkMerge.TabIndex = 13;
            this.chkMerge.Text = "Merge blacklisted images with non blacklisted images";
            this.chkMerge.UseVisualStyleBackColor = true;
            // 
            // chkOpen
            // 
            this.chkOpen.AutoSize = true;
            this.chkOpen.Location = new System.Drawing.Point(67, 99);
            this.chkOpen.Name = "chkOpen";
            this.chkOpen.Size = new System.Drawing.Size(138, 17);
            this.chkOpen.TabIndex = 19;
            this.chkOpen.Text = "Open after downloading";
            this.chkOpen.UseVisualStyleBackColor = true;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 231);
            this.Controls.Add(this.btnBlacklist);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "aphrodite settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.tabControl1.ResumeLayout(false);
            this.tbGeneral.ResumeLayout(false);
            this.tbGeneral.PerformLayout();
            this.tbTags.ResumeLayout(false);
            this.tbTags.PerformLayout();
            this.tbPools.ResumeLayout(false);
            this.tbPools.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLimit)).EndInit();
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

    }
}