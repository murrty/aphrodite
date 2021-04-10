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
            this.chkUpdate = new System.Windows.Forms.CheckBox();
            this.menuWishlist = new System.Windows.Forms.ContextMenu();
            this.mOpenPoolInBrowser = new System.Windows.Forms.MenuItem();
            this.mCopyPoolLink = new System.Windows.Forms.MenuItem();
            this.mCopyPoolId = new System.Windows.Forms.MenuItem();
            this.lPoolLink = new aphrodite.Controls.LinkLabelHand();
            this.txtName = new aphrodite.Controls.ExtendedTextBox();
            this.btnDownload = new aphrodite.Controls.ExtendedButton();
            this.btnRemove = new aphrodite.Controls.ExtendedButton();
            this.btnAddUpdate = new aphrodite.Controls.ExtendedButton();
            this.txtURL = new aphrodite.Controls.ExtendedTextBox();
            this.listWishlistItems = new aphrodite.Controls.AeroListBox();
            this.SuspendLayout();
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
            // menuWishlist
            // 
            this.menuWishlist.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mOpenPoolInBrowser,
            this.mCopyPoolLink,
            this.mCopyPoolId});
            // 
            // mOpenPoolInBrowser
            // 
            this.mOpenPoolInBrowser.Index = 0;
            this.mOpenPoolInBrowser.Text = "Open pool in browser";
            this.mOpenPoolInBrowser.Click += new System.EventHandler(this.mOpenPoolInBrowser_Click);
            // 
            // mCopyPoolLink
            // 
            this.mCopyPoolLink.Index = 1;
            this.mCopyPoolLink.Text = "Copy pool link";
            this.mCopyPoolLink.Click += new System.EventHandler(this.mCopyPoolLink_Click);
            // 
            // mCopyPoolId
            // 
            this.mCopyPoolId.Index = 2;
            this.mCopyPoolId.Text = "Copy pool id";
            this.mCopyPoolId.Click += new System.EventHandler(this.mCopyPoolId_Click);
            // 
            // lPoolLink
            // 
            this.lPoolLink.AutoSize = true;
            this.lPoolLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lPoolLink.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lPoolLink.Location = new System.Drawing.Point(12, 188);
            this.lPoolLink.Name = "lPoolLink";
            this.lPoolLink.Size = new System.Drawing.Size(196, 13);
            this.lPoolLink.TabIndex = 2;
            this.lPoolLink.TabStop = true;
            this.lPoolLink.Text = "The URL of the pool will display here";
            this.lPoolLink.UseMnemonic = false;
            this.lPoolLink.Visible = false;
            this.lPoolLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lPoolLink_LinkClicked);
            // 
            // txtName
            // 
            this.txtName.ButtonAlignment = aphrodite.Controls.ButtonAlignments.Left;
            this.txtName.ButtonCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtName.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.ButtonImageIndex = -1;
            this.txtName.ButtonImageKey = "";
            this.txtName.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtName.ButtonText = "";
            this.txtName.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtName.Location = new System.Drawing.Point(12, 207);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(286, 22);
            this.txtName.TabIndex = 3;
            this.txtName.TextHint = "Pool name...";
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
            // txtURL
            // 
            this.txtURL.ButtonAlignment = aphrodite.Controls.ButtonAlignments.Left;
            this.txtURL.ButtonCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtURL.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtURL.ButtonImageIndex = -1;
            this.txtURL.ButtonImageKey = "";
            this.txtURL.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtURL.ButtonText = "";
            this.txtURL.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtURL.Location = new System.Drawing.Point(12, 233);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(212, 22);
            this.txtURL.TabIndex = 4;
            this.txtURL.TextHint = "Pool URL...";
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

        private Controls.AeroListBox listWishlistItems;
        private Controls.ExtendedTextBox txtURL;
        private Controls.ExtendedButton btnAddUpdate;
        private Controls.ExtendedButton btnRemove;
        private Controls.ExtendedButton btnDownload;
        private Controls.ExtendedTextBox txtName;
        private System.Windows.Forms.CheckBox chkUpdate;
        private Controls.LinkLabelHand lPoolLink;
        private System.Windows.Forms.ContextMenu menuWishlist;
        private System.Windows.Forms.MenuItem mOpenPoolInBrowser;
        private System.Windows.Forms.MenuItem mCopyPoolLink;
        private System.Windows.Forms.MenuItem mCopyPoolId;
    }
}