namespace aphrodite {
    partial class frmPoolWishlist {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPoolWishlist));
            this.lbWish = new System.Windows.Forms.ListBox();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.chkUpdate = new System.Windows.Forms.CheckBox();
            this.lPoolLink = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lbWish
            // 
            this.lbWish.FormattingEnabled = true;
            this.lbWish.Location = new System.Drawing.Point(12, 12);
            this.lbWish.Name = "lbWish";
            this.lbWish.Size = new System.Drawing.Size(286, 173);
            this.lbWish.TabIndex = 1;
            this.lbWish.SelectedIndexChanged += new System.EventHandler(this.lbWish_SelectedIndexChanged);
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(12, 235);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(212, 20);
            this.txtURL.TabIndex = 4;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(230, 233);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(68, 23);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(200, 260);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(98, 23);
            this.btnRemove.TabIndex = 8;
            this.btnRemove.Text = "Remove selected";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(104, 260);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(94, 23);
            this.btnDownload.TabIndex = 7;
            this.btnDownload.Text = "Download pool";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(12, 209);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(286, 20);
            this.txtName.TabIndex = 3;
            // 
            // chkUpdate
            // 
            this.chkUpdate.AutoSize = true;
            this.chkUpdate.Location = new System.Drawing.Point(12, 264);
            this.chkUpdate.Name = "chkUpdate";
            this.chkUpdate.Size = new System.Drawing.Size(89, 17);
            this.chkUpdate.TabIndex = 6;
            this.chkUpdate.Text = "Update mode";
            this.chkUpdate.UseVisualStyleBackColor = true;
            this.chkUpdate.CheckedChanged += new System.EventHandler(this.chkUpdate_CheckedChanged);
            // 
            // lPoolLink
            // 
            this.lPoolLink.AutoSize = true;
            this.lPoolLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lPoolLink.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lPoolLink.Location = new System.Drawing.Point(12, 190);
            this.lPoolLink.Name = "lPoolLink";
            this.lPoolLink.Size = new System.Drawing.Size(90, 13);
            this.lPoolLink.TabIndex = 2;
            this.lPoolLink.TabStop = true;
            this.lPoolLink.Text = "DEBUG ACCESS";
            this.lPoolLink.UseMnemonic = false;
            this.lPoolLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lPoolLink_LinkClicked);
            // 
            // frmPoolWishlist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(310, 290);
            this.Controls.Add(this.lPoolLink);
            this.Controls.Add(this.chkUpdate);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtURL);
            this.Controls.Add(this.lbWish);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(318, 280);
            this.Name = "frmPoolWishlist";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pool Wishlist";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPoolWishlist_FormClosing);
            this.Load += new System.EventHandler(this.frmPoolWishlist_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbWish;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.CheckBox chkUpdate;
        private System.Windows.Forms.LinkLabel lPoolLink;
    }
}