namespace aphrodite {
    partial class frmBlacklist {
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
            this.pnLower = new System.Windows.Forms.Panel();
            this.lbMutual = new System.Windows.Forms.Label();
            this.btnSave = new murrty.controls.ExtendedButton();
            this.btnCancel = new murrty.controls.ExtendedButton();
            this.btnSortGraylist = new murrty.controls.ExtendedButton();
            this.rtbGraylist = new System.Windows.Forms.RichTextBox();
            this.rtbBlacklist = new System.Windows.Forms.RichTextBox();
            this.btnSortBlacklist = new murrty.controls.ExtendedButton();
            this.llGraylist = new murrty.controls.ExtendedLinkLabel();
            this.llBlacklist = new murrty.controls.ExtendedLinkLabel();
            this.lbBlacklist = new aphrodite.Controls.TransparentLabel();
            this.lbGraylist = new aphrodite.Controls.TransparentLabel();
            this.pnLower.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnLower
            // 
            this.pnLower.BackColor = System.Drawing.SystemColors.Menu;
            this.pnLower.Controls.Add(this.lbMutual);
            this.pnLower.Controls.Add(this.btnSave);
            this.pnLower.Controls.Add(this.btnCancel);
            this.pnLower.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnLower.Location = new System.Drawing.Point(0, 281);
            this.pnLower.Name = "pnLower";
            this.pnLower.Size = new System.Drawing.Size(502, 42);
            this.pnLower.TabIndex = 0;
            // 
            // lbMutual
            // 
            this.lbMutual.AutoSize = true;
            this.lbMutual.Location = new System.Drawing.Point(12, 14);
            this.lbMutual.Name = "lbMutual";
            this.lbMutual.Size = new System.Drawing.Size(211, 13);
            this.lbMutual.TabIndex = 21;
            this.lbMutual.Text = "Tags listed here apply to all downloads.";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(415, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(334, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSortGraylist
            // 
            this.btnSortGraylist.Location = new System.Drawing.Point(12, 244);
            this.btnSortGraylist.Name = "btnSortGraylist";
            this.btnSortGraylist.Size = new System.Drawing.Size(60, 23);
            this.btnSortGraylist.TabIndex = 3;
            this.btnSortGraylist.Text = "Sort";
            this.btnSortGraylist.UseVisualStyleBackColor = true;
            this.btnSortGraylist.Click += new System.EventHandler(this.ListSort_Click);
            // 
            // rtbGraylist
            // 
            this.rtbGraylist.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbGraylist.Location = new System.Drawing.Point(12, 26);
            this.rtbGraylist.Name = "rtbGraylist";
            this.rtbGraylist.Size = new System.Drawing.Size(230, 212);
            this.rtbGraylist.TabIndex = 1;
            this.rtbGraylist.Text = "";
            this.rtbGraylist.TextChanged += new System.EventHandler(this.List_TextChanged);
            // 
            // rtbBlacklist
            // 
            this.rtbBlacklist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbBlacklist.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbBlacklist.Location = new System.Drawing.Point(260, 26);
            this.rtbBlacklist.Name = "rtbBlacklist";
            this.rtbBlacklist.Size = new System.Drawing.Size(230, 212);
            this.rtbBlacklist.TabIndex = 2;
            this.rtbBlacklist.Text = "";
            this.rtbBlacklist.TextChanged += new System.EventHandler(this.List_TextChanged);
            // 
            // btnSortBlacklist
            // 
            this.btnSortBlacklist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSortBlacklist.Location = new System.Drawing.Point(260, 244);
            this.btnSortBlacklist.Name = "btnSortBlacklist";
            this.btnSortBlacklist.Size = new System.Drawing.Size(60, 23);
            this.btnSortBlacklist.TabIndex = 4;
            this.btnSortBlacklist.Text = "Sort";
            this.btnSortBlacklist.UseVisualStyleBackColor = true;
            this.btnSortBlacklist.Click += new System.EventHandler(this.ListSort_Click);
            // 
            // llGraylist
            // 
            this.llGraylist.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.llGraylist.AutoSize = true;
            this.llGraylist.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.llGraylist.Location = new System.Drawing.Point(53, 7);
            this.llGraylist.Name = "llGraylist";
            this.llGraylist.Size = new System.Drawing.Size(12, 13);
            this.llGraylist.TabIndex = 9;
            this.llGraylist.TabStop = true;
            this.llGraylist.Text = "?";
            this.llGraylist.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.llGraylist.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llGraylist_LinkClicked);
            // 
            // llBlacklist
            // 
            this.llBlacklist.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.llBlacklist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llBlacklist.AutoSize = true;
            this.llBlacklist.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.llBlacklist.Location = new System.Drawing.Point(303, 7);
            this.llBlacklist.Name = "llBlacklist";
            this.llBlacklist.Size = new System.Drawing.Size(12, 13);
            this.llBlacklist.TabIndex = 8;
            this.llBlacklist.TabStop = true;
            this.llBlacklist.Text = "?";
            this.llBlacklist.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.llBlacklist.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llBlacklist_LinkClicked);
            // 
            // lbBlacklist
            // 
            this.lbBlacklist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbBlacklist.Location = new System.Drawing.Point(260, 7);
            this.lbBlacklist.Name = "lbBlacklist";
            this.lbBlacklist.Size = new System.Drawing.Size(46, 13);
            this.lbBlacklist.TabIndex = 11;
            this.lbBlacklist.TabStop = false;
            this.lbBlacklist.Text = "Blacklist";
            this.lbBlacklist.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // lbGraylist
            // 
            this.lbGraylist.Location = new System.Drawing.Point(13, 7);
            this.lbGraylist.Name = "lbGraylist";
            this.lbGraylist.Size = new System.Drawing.Size(41, 13);
            this.lbGraylist.TabIndex = 10;
            this.lbGraylist.TabStop = false;
            this.lbGraylist.Text = "Graylist";
            this.lbGraylist.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // frmBlacklist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(502, 323);
            this.Controls.Add(this.llGraylist);
            this.Controls.Add(this.llBlacklist);
            this.Controls.Add(this.btnSortBlacklist);
            this.Controls.Add(this.btnSortGraylist);
            this.Controls.Add(this.pnLower);
            this.Controls.Add(this.lbBlacklist);
            this.Controls.Add(this.lbGraylist);
            this.Controls.Add(this.rtbBlacklist);
            this.Controls.Add(this.rtbGraylist);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(520, 350);
            this.Name = "frmBlacklist";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blacklists";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBlacklist_FormClosing);
            this.pnLower.ResumeLayout(false);
            this.pnLower.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnLower;
        private murrty.controls.ExtendedButton btnSave;
        private murrty.controls.ExtendedButton btnCancel;
        private murrty.controls.ExtendedButton btnSortGraylist;
        private System.Windows.Forms.RichTextBox rtbGraylist;
        private System.Windows.Forms.RichTextBox rtbBlacklist;
        private murrty.controls.ExtendedButton btnSortBlacklist;
        private System.Windows.Forms.Label lbMutual;
        private murrty.controls.ExtendedLinkLabel llBlacklist;
        private murrty.controls.ExtendedLinkLabel llGraylist;
        private Controls.TransparentLabel lbGraylist;
        private Controls.TransparentLabel lbBlacklist;
    }
}