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
            this.cmWishlist = new System.Windows.Forms.ContextMenu();
            this.mOpenPoolInBrowser = new System.Windows.Forms.MenuItem();
            this.mCopyPoolLink = new System.Windows.Forms.MenuItem();
            this.mCopyPoolId = new System.Windows.Forms.MenuItem();
            this.lPoolLink = new murrty.controls.ExtendedLinkLabel();
            this.txtName = new murrty.controls.ExtendedTextBox();
            this.btnDownload = new murrty.controls.ExtendedButton();
            this.btnRemove = new murrty.controls.ExtendedButton();
            this.btnAddUpdate = new murrty.controls.ExtendedButton();
            this.txtURL = new murrty.controls.ExtendedTextBox();
            this.lbWishlistItems = new aphrodite.Controls.AeroListBox();
            this.btnSave = new murrty.controls.ExtendedButton();
            this.btnCancel = new murrty.controls.ExtendedButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkUpdate
            // 
            this.chkUpdate.AutoSize = true;
            this.chkUpdate.Location = new System.Drawing.Point(12, 270);
            this.chkUpdate.Name = "chkUpdate";
            this.chkUpdate.Size = new System.Drawing.Size(95, 17);
            this.chkUpdate.TabIndex = 6;
            this.chkUpdate.Text = "Update mode";
            this.chkUpdate.UseVisualStyleBackColor = true;
            this.chkUpdate.CheckedChanged += new System.EventHandler(this.chkUpdate_CheckedChanged);
            // 
            // cmWishlist
            // 
            this.cmWishlist.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
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
            this.lPoolLink.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lPoolLink.AutoSize = true;
            this.lPoolLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lPoolLink.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lPoolLink.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lPoolLink.Location = new System.Drawing.Point(12, 190);
            this.lPoolLink.Name = "lPoolLink";
            this.lPoolLink.Size = new System.Drawing.Size(196, 13);
            this.lPoolLink.TabIndex = 2;
            this.lPoolLink.TabStop = true;
            this.lPoolLink.Text = "The URL of the pool will display here";
            this.lPoolLink.UseMnemonic = false;
            this.lPoolLink.Visible = false;
            this.lPoolLink.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.lPoolLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lPoolLink_LinkClicked);
            // 
            // txtName
            // 
            this.txtName.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtName.ButtonCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtName.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.ButtonImageIndex = -1;
            this.txtName.ButtonImageKey = "";
            this.txtName.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtName.ButtonText = "";
            this.txtName.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtName.Location = new System.Drawing.Point(12, 211);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(288, 22);
            this.txtName.TabIndex = 3;
            this.txtName.TextHint = "Pool name...";
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(150, 266);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(76, 23);
            this.btnDownload.TabIndex = 7;
            this.btnDownload.Text = "Download pool";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(232, 266);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(68, 23);
            this.btnRemove.TabIndex = 8;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAddUpdate
            // 
            this.btnAddUpdate.Location = new System.Drawing.Point(232, 239);
            this.btnAddUpdate.Name = "btnAddUpdate";
            this.btnAddUpdate.Size = new System.Drawing.Size(68, 23);
            this.btnAddUpdate.TabIndex = 5;
            this.btnAddUpdate.Text = "Add";
            this.btnAddUpdate.UseVisualStyleBackColor = true;
            this.btnAddUpdate.Click += new System.EventHandler(this.btnAddUpdate_Click);
            // 
            // txtURL
            // 
            this.txtURL.ButtonAlignment = murrty.controls.ButtonAlignment.Left;
            this.txtURL.ButtonCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtURL.ButtonFont = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtURL.ButtonImageIndex = -1;
            this.txtURL.ButtonImageKey = "";
            this.txtURL.ButtonSize = new System.Drawing.Size(22, 21);
            this.txtURL.ButtonText = "";
            this.txtURL.ButtonTextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtURL.Location = new System.Drawing.Point(12, 241);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(214, 22);
            this.txtURL.TabIndex = 4;
            this.txtURL.TextHint = "Pool URL...";
            // 
            // lbWishlistItems
            // 
            this.lbWishlistItems.FormattingEnabled = true;
            this.lbWishlistItems.Location = new System.Drawing.Point(12, 10);
            this.lbWishlistItems.Name = "lbWishlistItems";
            this.lbWishlistItems.Size = new System.Drawing.Size(288, 173);
            this.lbWishlistItems.TabIndex = 1;
            this.lbWishlistItems.SelectedIndexChanged += new System.EventHandler(this.lbWish_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(158, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(68, 23);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(232, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Menu;
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 301);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(312, 42);
            this.panel1.TabIndex = 11;
            // 
            // frmPoolWishlist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(312, 343);
            this.Controls.Add(this.lPoolLink);
            this.Controls.Add(this.chkUpdate);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAddUpdate);
            this.Controls.Add(this.txtURL);
            this.Controls.Add(this.lbWishlistItems);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(330, 380);
            this.MinimumSize = new System.Drawing.Size(330, 380);
            this.Name = "frmPoolWishlist";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pool Wishlist";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPoolWishlist_FormClosing);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.AeroListBox lbWishlistItems;
        private murrty.controls.ExtendedTextBox txtURL;
        private murrty.controls.ExtendedButton btnAddUpdate;
        private murrty.controls.ExtendedButton btnRemove;
        private murrty.controls.ExtendedButton btnDownload;
        private murrty.controls.ExtendedTextBox txtName;
        private System.Windows.Forms.CheckBox chkUpdate;
        private murrty.controls.ExtendedLinkLabel lPoolLink;
        private System.Windows.Forms.ContextMenu cmWishlist;
        private System.Windows.Forms.MenuItem mOpenPoolInBrowser;
        private System.Windows.Forms.MenuItem mCopyPoolLink;
        private System.Windows.Forms.MenuItem mCopyPoolId;
        private murrty.controls.ExtendedButton btnSave;
        private murrty.controls.ExtendedButton btnCancel;
        private System.Windows.Forms.Panel panel1;
    }
}