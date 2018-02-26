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
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSort = new System.Windows.Forms.Button();
            this.rtbBlacklist = new System.Windows.Forms.RichTextBox();
            this.rtbZTB = new System.Windows.Forms.RichTextBox();
            this.btnSortZTB = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 269);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(502, 47);
            this.panel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(147, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "booze me up and get me high";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(334, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(415, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSort
            // 
            this.btnSort.Location = new System.Drawing.Point(12, 240);
            this.btnSort.Name = "btnSort";
            this.btnSort.Size = new System.Drawing.Size(60, 23);
            this.btnSort.TabIndex = 1;
            this.btnSort.Text = "Sort";
            this.btnSort.UseVisualStyleBackColor = true;
            this.btnSort.Click += new System.EventHandler(this.btnSort_Click);
            // 
            // rtbBlacklist
            // 
            this.rtbBlacklist.Location = new System.Drawing.Point(10, 26);
            this.rtbBlacklist.Name = "rtbBlacklist";
            this.rtbBlacklist.Size = new System.Drawing.Size(231, 208);
            this.rtbBlacklist.TabIndex = 3;
            this.rtbBlacklist.Text = "";
            this.rtbBlacklist.TextChanged += new System.EventHandler(this.rtbBlacklist_TextChanged);
            // 
            // rtbZTB
            // 
            this.rtbZTB.Location = new System.Drawing.Point(260, 26);
            this.rtbZTB.Name = "rtbZTB";
            this.rtbZTB.Size = new System.Drawing.Size(231, 208);
            this.rtbZTB.TabIndex = 4;
            this.rtbZTB.Text = "";
            this.rtbZTB.TextChanged += new System.EventHandler(this.rtbZTB_TextChanged);
            // 
            // btnSortZTB
            // 
            this.btnSortZTB.Location = new System.Drawing.Point(262, 240);
            this.btnSortZTB.Name = "btnSortZTB";
            this.btnSortZTB.Size = new System.Drawing.Size(60, 23);
            this.btnSortZTB.TabIndex = 5;
            this.btnSortZTB.Text = "Sort";
            this.btnSortZTB.UseVisualStyleBackColor = true;
            this.btnSortZTB.Click += new System.EventHandler(this.btnSortZTB_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Blacklist (tags that are optionally saved)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(257, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(247, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Zero-tolerance blacklist (tags that are never saved)";
            // 
            // frmBlacklist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(502, 316);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSortZTB);
            this.Controls.Add(this.rtbZTB);
            this.Controls.Add(this.rtbBlacklist);
            this.Controls.Add(this.btnSort);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(510, 346);
            this.MinimumSize = new System.Drawing.Size(510, 346);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}