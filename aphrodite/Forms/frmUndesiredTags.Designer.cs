namespace aphrodite {
    partial class frmUndesiredTags {
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
            this.lbHeader = new System.Windows.Forms.Label();
            this.btnReset = new murrty.controls.ExtendedButton();
            this.btnSave = new murrty.controls.ExtendedButton();
            this.btnCancel = new murrty.controls.ExtendedButton();
            this.txtUndesired = new System.Windows.Forms.TextBox();
            this.btnAdd = new murrty.controls.ExtendedButton();
            this.btnRemove = new murrty.controls.ExtendedButton();
            this.listTags = new aphrodite.Controls.AeroListBox();
            this.pnLower = new System.Windows.Forms.Panel();
            this.pnLower.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbHeader
            // 
            this.lbHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHeader.Location = new System.Drawing.Point(12, 3);
            this.lbHeader.Name = "lbHeader";
            this.lbHeader.Size = new System.Drawing.Size(295, 79);
            this.lbHeader.TabIndex = 0;
            this.lbHeader.Text = "Undesired tags are tags that are filtered out of the artist parsing due to e621 u" +
    "sing artist tags as warning tags.\r\n\r\nThis does not affect the blacklist, only th" +
    "e file name schema.";
            this.lbHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(12, 286);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(106, 23);
            this.btnReset.TabIndex = 22;
            this.btnReset.Text = "Reset to default";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(159, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(71, 23);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(236, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(71, 23);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtUndesired
            // 
            this.txtUndesired.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUndesired.Location = new System.Drawing.Point(84, 244);
            this.txtUndesired.Name = "txtUndesired";
            this.txtUndesired.Size = new System.Drawing.Size(150, 20);
            this.txtUndesired.TabIndex = 24;
            this.txtUndesired.TextChanged += new System.EventHandler(this.txtUndesired_TextChanged);
            this.txtUndesired.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUndesired_KeyPress);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(240, 242);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(36, 23);
            this.btnAdd.TabIndex = 25;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(240, 91);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(70, 40);
            this.btnRemove.TabIndex = 26;
            this.btnRemove.Text = "Remove selected";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // listTags
            // 
            this.listTags.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listTags.FormattingEnabled = true;
            this.listTags.Location = new System.Drawing.Point(84, 87);
            this.listTags.Name = "listTags";
            this.listTags.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listTags.Size = new System.Drawing.Size(150, 147);
            this.listTags.Sorted = true;
            this.listTags.TabIndex = 23;
            // 
            // pnLower
            // 
            this.pnLower.BackColor = System.Drawing.SystemColors.Menu;
            this.pnLower.Controls.Add(this.btnSave);
            this.pnLower.Controls.Add(this.btnCancel);
            this.pnLower.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnLower.Location = new System.Drawing.Point(0, 278);
            this.pnLower.Name = "pnLower";
            this.pnLower.Size = new System.Drawing.Size(319, 42);
            this.pnLower.TabIndex = 27;
            // 
            // frmUndesiredTags
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(319, 320);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtUndesired);
            this.Controls.Add(this.listTags);
            this.Controls.Add(this.lbHeader);
            this.Controls.Add(this.pnLower);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MinimizeBox = false;
            this.Name = "frmUndesiredTags";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Undesired Tags";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUndesiredTags_FormClosing);
            this.pnLower.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbHeader;
        private murrty.controls.ExtendedButton btnReset;
        private murrty.controls.ExtendedButton btnSave;
        private murrty.controls.ExtendedButton btnCancel;
        private Controls.AeroListBox listTags;
        private System.Windows.Forms.TextBox txtUndesired;
        private murrty.controls.ExtendedButton btnAdd;
        private murrty.controls.ExtendedButton btnRemove;
        private System.Windows.Forms.Panel pnLower;
    }
}