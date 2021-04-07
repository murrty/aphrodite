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
            this.listWishlistItems = new System.Windows.Forms.ListBox();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.btnAddUpdate = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.chkUpdate = new System.Windows.Forms.CheckBox();
            this.lPoolLink = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // listWishlistItems
            // 
            this.listWishlistItems.FormattingEnabled = true;
            this.listWishlistItems.Location = new System.Drawing.Point(12, 10);
            this.listWishlistItems.Name = "listWishlistItems";
            this.listWishlistItems.Size = new System.Drawing.Size(286, 173);
            this.listWishlistItems.TabIndex = 1;
            this.listWishlistItems.SelectedIndexChanged += new System.EventHandler(this.lbWish_SelectedIndexChanged);
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(12, 233);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(212, 22);
            this.txtURL.TabIndex = 4;
            // 
            // btnAddUpdate
            // 
            this.btnAddUpdate.Location = new System.Drawing.Point(230, 231);
            this.btnAddUpdate.Name = "btnAddUpdate";
            this.btnAddUpdate.Size = new System.Drawing.Size(68, 23);
            this.btnAddUpdate.TabIndex = 5;
            this.btnAddUpdate.Text = "Add";
            this.btnAddUpdate.UseVisualStyleBackColor = true;
            this.btnAddUpdate.Click += new System.EventHandler(this.btnAddUpdate_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(230, 258);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(68, 23);
            this.btnRemove.TabIndex = 8;
            this.btnRemove.Text = "Remove selected";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(148, 258);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(76, 23);
            this.btnDownload.TabIndex = 7;
            this.btnDownload.Text = "Download pool";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(12, 207);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(286, 22);
            this.txtName.TabIndex = 3;
            // 
            // chkUpdate
            // 
            this.chkUpdate.AutoSize = true;
            this.chkUpdate.Location = new System.Drawing.Point(12, 262);
            this.chkUpdate.Name = "chkUpdate";
            this.chkUpdate.Size = new System.Drawing.Size(95, 17);
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
            this.lPoolLink.Location = new System.Drawing.Point(12, 188);
            this.lPoolLink.Name = "lPoolLink";
            this.lPoolLink.Size = new System.Drawing.Size(85, 13);
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
            this.Controls.Add(this.btnAddUpdate);
            this.Controls.Add(this.txtURL);
            this.Controls.Add(this.listWishlistItems);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
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

        private System.Windows.Forms.ListBox listWishlistItems;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Button btnAddUpdate;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.CheckBox chkUpdate;
        private System.Windows.Forms.LinkLabel lPoolLink;
    }
}