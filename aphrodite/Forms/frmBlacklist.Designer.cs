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
            this.btnSave = new aphrodite.Controls.ExtendedButton();
            this.btnCancel = new aphrodite.Controls.ExtendedButton();
            this.btnSort = new aphrodite.Controls.ExtendedButton();
            this.rtbBlacklist = new System.Windows.Forms.RichTextBox();
            this.rtbZTB = new System.Windows.Forms.RichTextBox();
            this.btnSortZTB = new aphrodite.Controls.ExtendedButton();
            this.llGraylist = new aphrodite.Controls.LinkLabelHand();
            this.llBlacklist = new aphrodite.Controls.LinkLabelHand();
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
            // btnSort
            // 
            this.btnSort.Location = new System.Drawing.Point(12, 244);
            this.btnSort.Name = "btnSort";
            this.btnSort.Size = new System.Drawing.Size(60, 23);
            this.btnSort.TabIndex = 3;
            this.btnSort.Text = "Sort";
            this.btnSort.UseVisualStyleBackColor = true;
            this.btnSort.Click += new System.EventHandler(this.btnSort_Click);
            // 
            // rtbBlacklist
            // 
            this.rtbBlacklist.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbBlacklist.Location = new System.Drawing.Point(12, 26);
            this.rtbBlacklist.Name = "rtbBlacklist";
            this.rtbBlacklist.Size = new System.Drawing.Size(230, 212);
            this.rtbBlacklist.TabIndex = 1;
            this.rtbBlacklist.Text = "";
            this.rtbBlacklist.TextChanged += new System.EventHandler(this.rtbBlacklist_TextChanged);
            // 
            // rtbZTB
            // 
            this.rtbZTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbZTB.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbZTB.Location = new System.Drawing.Point(260, 26);
            this.rtbZTB.Name = "rtbZTB";
            this.rtbZTB.Size = new System.Drawing.Size(230, 212);
            this.rtbZTB.TabIndex = 2;
            this.rtbZTB.Text = "";
            this.rtbZTB.TextChanged += new System.EventHandler(this.rtbZTB_TextChanged);
            // 
            // btnSortZTB
            // 
            this.btnSortZTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSortZTB.Location = new System.Drawing.Point(260, 244);
            this.btnSortZTB.Name = "btnSortZTB";
            this.btnSortZTB.Size = new System.Drawing.Size(60, 23);
            this.btnSortZTB.TabIndex = 4;
            this.btnSortZTB.Text = "Sort";
            this.btnSortZTB.UseVisualStyleBackColor = true;
            this.btnSortZTB.Click += new System.EventHandler(this.btnSortZTB_Click);
            // 
            // llGraylist
            // 
            this.llGraylist.AutoSize = true;
            this.llGraylist.Location = new System.Drawing.Point(53, 7);
            this.llGraylist.Name = "llGraylist";
            this.llGraylist.Size = new System.Drawing.Size(12, 13);
            this.llGraylist.TabIndex = 9;
            this.llGraylist.TabStop = true;
            this.llGraylist.Text = "?";
            this.llGraylist.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llGraylist_LinkClicked);
            // 
            // llBlacklist
            // 
            this.llBlacklist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.llBlacklist.AutoSize = true;
            this.llBlacklist.Location = new System.Drawing.Point(303, 7);
            this.llBlacklist.Name = "llBlacklist";
            this.llBlacklist.Size = new System.Drawing.Size(12, 13);
            this.llBlacklist.TabIndex = 8;
            this.llBlacklist.TabStop = true;
            this.llBlacklist.Text = "?";
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
            this.ClientSize = new System.Drawing.Size(502, 323);
            this.Controls.Add(this.llGraylist);
            this.Controls.Add(this.llBlacklist);
            this.Controls.Add(this.btnSortZTB);
            this.Controls.Add(this.btnSort);
            this.Controls.Add(this.pnLower);
            this.Controls.Add(this.lbBlacklist);
            this.Controls.Add(this.lbGraylist);
            this.Controls.Add(this.rtbZTB);
            this.Controls.Add(this.rtbBlacklist);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(520, 350);
            this.Name = "frmBlacklist";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blacklists";
            this.Load += new System.EventHandler(this.frmBlacklist_Load);
            this.pnLower.ResumeLayout(false);
            this.pnLower.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnLower;
        private Controls.ExtendedButton btnSave;
        private Controls.ExtendedButton btnCancel;
        private Controls.ExtendedButton btnSort;
        private System.Windows.Forms.RichTextBox rtbBlacklist;
        private System.Windows.Forms.RichTextBox rtbZTB;
        private Controls.ExtendedButton btnSortZTB;
        private System.Windows.Forms.Label lbMutual;
        private Controls.LinkLabelHand llBlacklist;
        private Controls.LinkLabelHand llGraylist;
        private Controls.TransparentLabel lbGraylist;
        private Controls.TransparentLabel lbBlacklist;
    }
}