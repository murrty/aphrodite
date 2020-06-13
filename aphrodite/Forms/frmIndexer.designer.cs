namespace aphrodite {
    partial class frmIndexer {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIndexer));
            this.rbImage = new System.Windows.Forms.RadioButton();
            this.rbPool = new System.Windows.Forms.RadioButton();
            this.rbTag = new System.Windows.Forms.RadioButton();
            this.lbURL = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.tcTabs = new System.Windows.Forms.TabControl();
            this.tbImage = new System.Windows.Forms.TabPage();
            this.btnDisposeTab = new System.Windows.Forms.Button();
            this.btnDisposeAll = new System.Windows.Forms.Button();
            this.tcImages = new System.Windows.Forms.TabControl();
            this.tbPool = new System.Windows.Forms.TabPage();
            this.txtPoolDesc = new System.Windows.Forms.TextBox();
            this.lbDesc = new System.Windows.Forms.Label();
            this.txtPoolPages = new System.Windows.Forms.TextBox();
            this.lbPoolPages = new System.Windows.Forms.Label();
            this.txtPoolName = new System.Windows.Forms.TextBox();
            this.lbPoolName = new System.Windows.Forms.Label();
            this.lbPoolID = new System.Windows.Forms.Label();
            this.txtPoolId = new System.Windows.Forms.TextBox();
            this.tcPoolPages = new System.Windows.Forms.TabControl();
            this.tbTag = new System.Windows.Forms.TabPage();
            this.tcTags = new System.Windows.Forms.TabControl();
            this.btnRetrieve = new System.Windows.Forms.Button();
            this.pbHint = new System.Windows.Forms.PictureBox();
            this.lbPoss = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tcTabs.SuspendLayout();
            this.tbImage.SuspendLayout();
            this.tbPool.SuspendLayout();
            this.tbTag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHint)).BeginInit();
            this.SuspendLayout();
            // 
            // rbImage
            // 
            this.rbImage.AutoSize = true;
            this.rbImage.Checked = true;
            this.rbImage.Location = new System.Drawing.Point(12, 12);
            this.rbImage.Name = "rbImage";
            this.rbImage.Size = new System.Drawing.Size(52, 17);
            this.rbImage.TabIndex = 0;
            this.rbImage.TabStop = true;
            this.rbImage.Text = "image";
            this.rbImage.UseVisualStyleBackColor = true;
            // 
            // rbPool
            // 
            this.rbPool.AutoSize = true;
            this.rbPool.Location = new System.Drawing.Point(71, 12);
            this.rbPool.Name = "rbPool";
            this.rbPool.Size = new System.Drawing.Size(44, 17);
            this.rbPool.TabIndex = 1;
            this.rbPool.TabStop = true;
            this.rbPool.Text = "pool";
            this.rbPool.UseVisualStyleBackColor = true;
            // 
            // rbTag
            // 
            this.rbTag.AutoSize = true;
            this.rbTag.Location = new System.Drawing.Point(122, 12);
            this.rbTag.Name = "rbTag";
            this.rbTag.Size = new System.Drawing.Size(118, 17);
            this.rbTag.TabIndex = 2;
            this.rbTag.TabStop = true;
            this.rbTag.Text = "tag(s), 120 hard limit";
            this.rbTag.UseVisualStyleBackColor = true;
            // 
            // lbURL
            // 
            this.lbURL.AutoSize = true;
            this.lbURL.Location = new System.Drawing.Point(12, 35);
            this.lbURL.Name = "lbURL";
            this.lbURL.Size = new System.Drawing.Size(143, 13);
            this.lbURL.TabIndex = 3;
            this.lbURL.Text = "id or tags (urls not supported)";
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(26, 51);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(193, 20);
            this.txtInput.TabIndex = 3;
            this.txtInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInput_KeyPress);
            // 
            // tcTabs
            // 
            this.tcTabs.Controls.Add(this.tbImage);
            this.tcTabs.Controls.Add(this.tbPool);
            this.tcTabs.Controls.Add(this.tbTag);
            this.tcTabs.Location = new System.Drawing.Point(12, 78);
            this.tcTabs.Name = "tcTabs";
            this.tcTabs.SelectedIndex = 0;
            this.tcTabs.Size = new System.Drawing.Size(434, 370);
            this.tcTabs.TabIndex = 5;
            // 
            // tbImage
            // 
            this.tbImage.Controls.Add(this.btnDisposeTab);
            this.tbImage.Controls.Add(this.btnDisposeAll);
            this.tbImage.Controls.Add(this.tcImages);
            this.tbImage.Location = new System.Drawing.Point(4, 22);
            this.tbImage.Name = "tbImage";
            this.tbImage.Padding = new System.Windows.Forms.Padding(3);
            this.tbImage.Size = new System.Drawing.Size(426, 344);
            this.tbImage.TabIndex = 0;
            this.tbImage.Text = "image";
            this.tbImage.UseVisualStyleBackColor = true;
            // 
            // btnDisposeTab
            // 
            this.btnDisposeTab.Enabled = false;
            this.btnDisposeTab.Location = new System.Drawing.Point(344, 239);
            this.btnDisposeTab.Name = "btnDisposeTab";
            this.btnDisposeTab.Size = new System.Drawing.Size(75, 23);
            this.btnDisposeTab.TabIndex = 2;
            this.btnDisposeTab.Text = "dispose tab";
            this.btnDisposeTab.UseVisualStyleBackColor = true;
            this.btnDisposeTab.Click += new System.EventHandler(this.btnDisposeTab_Click);
            // 
            // btnDisposeAll
            // 
            this.btnDisposeAll.Enabled = false;
            this.btnDisposeAll.Location = new System.Drawing.Point(329, 268);
            this.btnDisposeAll.Name = "btnDisposeAll";
            this.btnDisposeAll.Size = new System.Drawing.Size(90, 23);
            this.btnDisposeAll.TabIndex = 3;
            this.btnDisposeAll.Text = "dispose all tabs";
            this.btnDisposeAll.UseVisualStyleBackColor = true;
            this.btnDisposeAll.Click += new System.EventHandler(this.btnDisposeAll_Click);
            // 
            // tcImages
            // 
            this.tcImages.Location = new System.Drawing.Point(3, 6);
            this.tcImages.Name = "tcImages";
            this.tcImages.SelectedIndex = 0;
            this.tcImages.Size = new System.Drawing.Size(420, 230);
            this.tcImages.TabIndex = 1;
            // 
            // tbPool
            // 
            this.tbPool.Controls.Add(this.txtPoolDesc);
            this.tbPool.Controls.Add(this.lbDesc);
            this.tbPool.Controls.Add(this.txtPoolPages);
            this.tbPool.Controls.Add(this.lbPoolPages);
            this.tbPool.Controls.Add(this.txtPoolName);
            this.tbPool.Controls.Add(this.lbPoolName);
            this.tbPool.Controls.Add(this.lbPoolID);
            this.tbPool.Controls.Add(this.txtPoolId);
            this.tbPool.Controls.Add(this.tcPoolPages);
            this.tbPool.Location = new System.Drawing.Point(4, 22);
            this.tbPool.Name = "tbPool";
            this.tbPool.Padding = new System.Windows.Forms.Padding(3);
            this.tbPool.Size = new System.Drawing.Size(426, 344);
            this.tbPool.TabIndex = 1;
            this.tbPool.Text = "pool";
            this.tbPool.UseVisualStyleBackColor = true;
            // 
            // txtPoolDesc
            // 
            this.txtPoolDesc.Location = new System.Drawing.Point(87, 84);
            this.txtPoolDesc.Name = "txtPoolDesc";
            this.txtPoolDesc.ReadOnly = true;
            this.txtPoolDesc.Size = new System.Drawing.Size(300, 20);
            this.txtPoolDesc.TabIndex = 4;
            // 
            // lbDesc
            // 
            this.lbDesc.AutoSize = true;
            this.lbDesc.Location = new System.Drawing.Point(39, 87);
            this.lbDesc.Name = "lbDesc";
            this.lbDesc.Size = new System.Drawing.Size(33, 13);
            this.lbDesc.TabIndex = 23;
            this.lbDesc.Text = "desc.";
            // 
            // txtPoolPages
            // 
            this.txtPoolPages.Location = new System.Drawing.Point(87, 58);
            this.txtPoolPages.Name = "txtPoolPages";
            this.txtPoolPages.ReadOnly = true;
            this.txtPoolPages.Size = new System.Drawing.Size(300, 20);
            this.txtPoolPages.TabIndex = 3;
            // 
            // lbPoolPages
            // 
            this.lbPoolPages.AutoSize = true;
            this.lbPoolPages.Location = new System.Drawing.Point(39, 61);
            this.lbPoolPages.Name = "lbPoolPages";
            this.lbPoolPages.Size = new System.Drawing.Size(36, 13);
            this.lbPoolPages.TabIndex = 21;
            this.lbPoolPages.Text = "pages";
            // 
            // txtPoolName
            // 
            this.txtPoolName.Location = new System.Drawing.Point(87, 32);
            this.txtPoolName.Name = "txtPoolName";
            this.txtPoolName.ReadOnly = true;
            this.txtPoolName.Size = new System.Drawing.Size(300, 20);
            this.txtPoolName.TabIndex = 2;
            // 
            // lbPoolName
            // 
            this.lbPoolName.AutoSize = true;
            this.lbPoolName.Location = new System.Drawing.Point(39, 35);
            this.lbPoolName.Name = "lbPoolName";
            this.lbPoolName.Size = new System.Drawing.Size(33, 13);
            this.lbPoolName.TabIndex = 19;
            this.lbPoolName.Text = "name";
            // 
            // lbPoolID
            // 
            this.lbPoolID.AutoSize = true;
            this.lbPoolID.Location = new System.Drawing.Point(39, 9);
            this.lbPoolID.Name = "lbPoolID";
            this.lbPoolID.Size = new System.Drawing.Size(38, 13);
            this.lbPoolID.TabIndex = 18;
            this.lbPoolID.Text = "pool id";
            // 
            // txtPoolId
            // 
            this.txtPoolId.Location = new System.Drawing.Point(87, 6);
            this.txtPoolId.Name = "txtPoolId";
            this.txtPoolId.ReadOnly = true;
            this.txtPoolId.Size = new System.Drawing.Size(300, 20);
            this.txtPoolId.TabIndex = 1;
            // 
            // tcPoolPages
            // 
            this.tcPoolPages.Location = new System.Drawing.Point(3, 110);
            this.tcPoolPages.Name = "tcPoolPages";
            this.tcPoolPages.SelectedIndex = 0;
            this.tcPoolPages.Size = new System.Drawing.Size(420, 231);
            this.tcPoolPages.TabIndex = 5;
            // 
            // tbTag
            // 
            this.tbTag.Controls.Add(this.tcTags);
            this.tbTag.Location = new System.Drawing.Point(4, 22);
            this.tbTag.Name = "tbTag";
            this.tbTag.Padding = new System.Windows.Forms.Padding(3);
            this.tbTag.Size = new System.Drawing.Size(426, 344);
            this.tbTag.TabIndex = 2;
            this.tbTag.Text = "tag(s)";
            this.tbTag.UseVisualStyleBackColor = true;
            // 
            // tcTags
            // 
            this.tcTags.Location = new System.Drawing.Point(3, 6);
            this.tcTags.Name = "tcTags";
            this.tcTags.SelectedIndex = 0;
            this.tcTags.Size = new System.Drawing.Size(420, 230);
            this.tcTags.TabIndex = 1;
            // 
            // btnRetrieve
            // 
            this.btnRetrieve.Location = new System.Drawing.Point(225, 49);
            this.btnRetrieve.Name = "btnRetrieve";
            this.btnRetrieve.Size = new System.Drawing.Size(75, 23);
            this.btnRetrieve.TabIndex = 4;
            this.btnRetrieve.Text = "retrieve info";
            this.btnRetrieve.UseVisualStyleBackColor = true;
            this.btnRetrieve.Click += new System.EventHandler(this.btnRetrieve_Click);
            // 
            // pbHint
            // 
            this.pbHint.BackColor = System.Drawing.Color.Transparent;
            this.pbHint.Image = ((System.Drawing.Image)(resources.GetObject("pbHint.Image")));
            this.pbHint.Location = new System.Drawing.Point(343, 13);
            this.pbHint.Name = "pbHint";
            this.pbHint.Size = new System.Drawing.Size(64, 64);
            this.pbHint.TabIndex = 7;
            this.pbHint.TabStop = false;
            this.pbHint.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbHint_MouseDown);
            this.pbHint.MouseEnter += new System.EventHandler(this.pbHint_MouseEnter);
            this.pbHint.MouseLeave += new System.EventHandler(this.pbHint_MouseLeave);
            // 
            // lbPoss
            // 
            this.lbPoss.AutoSize = true;
            this.lbPoss.Location = new System.Drawing.Point(334, 80);
            this.lbPoss.Name = "lbPoss";
            this.lbPoss.Size = new System.Drawing.Size(85, 13);
            this.lbPoss.TabIndex = 8;
            this.lbPoss.Text = "possm approved";
            this.lbPoss.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(185, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "for use during debugging";
            // 
            // frmIndexer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(458, 458);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbPoss);
            this.Controls.Add(this.pbHint);
            this.Controls.Add(this.btnRetrieve);
            this.Controls.Add(this.tcTabs);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.lbURL);
            this.Controls.Add(this.rbTag);
            this.Controls.Add(this.rbPool);
            this.Controls.Add(this.rbImage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(466, 488);
            this.MinimumSize = new System.Drawing.Size(466, 488);
            this.Name = "frmIndexer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "tag indexer";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmIndexer_MouseDown);
            this.tcTabs.ResumeLayout(false);
            this.tbImage.ResumeLayout(false);
            this.tbPool.ResumeLayout(false);
            this.tbPool.PerformLayout();
            this.tbTag.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbHint)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbImage;
        private System.Windows.Forms.RadioButton rbPool;
        private System.Windows.Forms.RadioButton rbTag;
        private System.Windows.Forms.Label lbURL;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.TabControl tcTabs;
        private System.Windows.Forms.TabPage tbImage;
        private System.Windows.Forms.TabPage tbPool;
        private System.Windows.Forms.TabPage tbTag;
        private System.Windows.Forms.Button btnRetrieve;
        private System.Windows.Forms.TabControl tcPoolPages;
        private System.Windows.Forms.TextBox txtPoolDesc;
        private System.Windows.Forms.Label lbDesc;
        private System.Windows.Forms.TextBox txtPoolPages;
        private System.Windows.Forms.Label lbPoolPages;
        private System.Windows.Forms.TextBox txtPoolName;
        private System.Windows.Forms.Label lbPoolName;
        private System.Windows.Forms.Label lbPoolID;
        private System.Windows.Forms.TextBox txtPoolId;
        private System.Windows.Forms.PictureBox pbHint;
        private System.Windows.Forms.TabControl tcImages;
        private System.Windows.Forms.Button btnDisposeTab;
        private System.Windows.Forms.Button btnDisposeAll;
        private System.Windows.Forms.TabControl tcTags;
        private System.Windows.Forms.Label lbPoss;
        private System.Windows.Forms.Label label1;
    }
}

