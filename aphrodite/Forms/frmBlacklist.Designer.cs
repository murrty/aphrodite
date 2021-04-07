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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbIni = new System.Windows.Forms.Label();
            this.lbMutual = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSort = new System.Windows.Forms.Button();
            this.rtbBlacklist = new System.Windows.Forms.RichTextBox();
            this.rtbZTB = new System.Windows.Forms.RichTextBox();
            this.btnSortZTB = new System.Windows.Forms.Button();
            this.llGraylist = new aphrodite.LinkLabelHand();
            this.llBlacklist = new aphrodite.LinkLabelHand();
            this.transparentLabel2 = new aphrodite.TransparentLabel();
            this.transparentLabel1 = new aphrodite.TransparentLabel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbIni);
            this.panel1.Controls.Add(this.lbMutual);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 266);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(492, 47);
            this.panel1.TabIndex = 0;
            // 
            // lbIni
            // 
            this.lbIni.AutoSize = true;
            this.lbIni.Location = new System.Drawing.Point(12, 9);
            this.lbIni.Name = "lbIni";
            this.lbIni.Size = new System.Drawing.Size(105, 13);
            this.lbIni.TabIndex = 8;
            this.lbIni.Text = "Editing ini blacklist";
            this.lbIni.Visible = false;
            // 
            // lbMutual
            // 
            this.lbMutual.AutoSize = true;
            this.lbMutual.Location = new System.Drawing.Point(12, 17);
            this.lbMutual.Name = "lbMutual";
            this.lbMutual.Size = new System.Drawing.Size(261, 13);
            this.lbMutual.TabIndex = 21;
            this.lbMutual.Text = "Blacklisted tags are mutual between pools && tags";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(405, 12);
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
            this.btnCancel.Location = new System.Drawing.Point(324, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSort
            // 
            this.btnSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSort.Location = new System.Drawing.Point(7, 242);
            this.btnSort.Name = "btnSort";
            this.btnSort.Size = new System.Drawing.Size(60, 23);
            this.btnSort.TabIndex = 3;
            this.btnSort.Text = "Sort";
            this.btnSort.UseVisualStyleBackColor = true;
            this.btnSort.Click += new System.EventHandler(this.btnSort_Click);
            // 
            // rtbBlacklist
            // 
            this.rtbBlacklist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbBlacklist.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbBlacklist.Location = new System.Drawing.Point(7, 26);
            this.rtbBlacklist.Name = "rtbBlacklist";
            this.rtbBlacklist.Size = new System.Drawing.Size(231, 212);
            this.rtbBlacklist.TabIndex = 1;
            this.rtbBlacklist.Text = "";
            this.rtbBlacklist.TextChanged += new System.EventHandler(this.rtbBlacklist_TextChanged);
            // 
            // rtbZTB
            // 
            this.rtbZTB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rtbZTB.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbZTB.Location = new System.Drawing.Point(257, 26);
            this.rtbZTB.Name = "rtbZTB";
            this.rtbZTB.Size = new System.Drawing.Size(231, 212);
            this.rtbZTB.TabIndex = 2;
            this.rtbZTB.Text = "";
            this.rtbZTB.TextChanged += new System.EventHandler(this.rtbZTB_TextChanged);
            // 
            // btnSortZTB
            // 
            this.btnSortZTB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSortZTB.Location = new System.Drawing.Point(257, 242);
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
            this.llGraylist.Location = new System.Drawing.Point(45, 10);
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
            this.llBlacklist.Location = new System.Drawing.Point(298, 10);
            this.llBlacklist.Name = "llBlacklist";
            this.llBlacklist.Size = new System.Drawing.Size(12, 13);
            this.llBlacklist.TabIndex = 8;
            this.llBlacklist.TabStop = true;
            this.llBlacklist.Text = "?";
            this.llBlacklist.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llBlacklist_LinkClicked);
            // 
            // transparentLabel2
            // 
            this.transparentLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.transparentLabel2.Location = new System.Drawing.Point(254, 10);
            this.transparentLabel2.Name = "transparentLabel2";
            this.transparentLabel2.Size = new System.Drawing.Size(46, 13);
            this.transparentLabel2.TabIndex = 11;
            this.transparentLabel2.TabStop = false;
            this.transparentLabel2.Text = "Blacklist";
            this.transparentLabel2.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // transparentLabel1
            // 
            this.transparentLabel1.Location = new System.Drawing.Point(4, 10);
            this.transparentLabel1.Name = "transparentLabel1";
            this.transparentLabel1.Size = new System.Drawing.Size(41, 13);
            this.transparentLabel1.TabIndex = 10;
            this.transparentLabel1.TabStop = false;
            this.transparentLabel1.Text = "Graylist";
            this.transparentLabel1.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // frmBlacklist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(492, 313);
            this.Controls.Add(this.llGraylist);
            this.Controls.Add(this.llBlacklist);
            this.Controls.Add(this.btnSortZTB);
            this.Controls.Add(this.btnSort);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.transparentLabel2);
            this.Controls.Add(this.transparentLabel1);
            this.Controls.Add(this.rtbZTB);
            this.Controls.Add(this.rtbBlacklist);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(510, 350);
            this.MinimumSize = new System.Drawing.Size(510, 350);
            this.Name = "frmBlacklist";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blacklists";
            this.Load += new System.EventHandler(this.frmBlacklist_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSort;
        private System.Windows.Forms.RichTextBox rtbBlacklist;
        private System.Windows.Forms.RichTextBox rtbZTB;
        private System.Windows.Forms.Button btnSortZTB;
        private System.Windows.Forms.Label lbMutual;
        private System.Windows.Forms.Label lbIni;
        private LinkLabelHand llBlacklist;
        private LinkLabelHand llGraylist;
        private TransparentLabel transparentLabel1;
        private TransparentLabel transparentLabel2;
    }
}