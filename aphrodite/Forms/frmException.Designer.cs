﻿namespace aphrodite {
    partial class frmException {
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
            this.btnExceptionGithub = new System.Windows.Forms.Button();
            this.lbVersion = new System.Windows.Forms.Label();
            this.btnExceptionOk = new System.Windows.Forms.Button();
            this.rtbExceptionDetails = new System.Windows.Forms.RichTextBox();
            this.lbExceptionDescription = new System.Windows.Forms.Label();
            this.lbExceptionHeader = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnExceptionGithub
            // 
            this.btnExceptionGithub.Location = new System.Drawing.Point(294, 213);
            this.btnExceptionGithub.Name = "btnExceptionGithub";
            this.btnExceptionGithub.Size = new System.Drawing.Size(85, 24);
            this.btnExceptionGithub.TabIndex = 10;
            this.btnExceptionGithub.Text = "btnExceptionGithub";
            this.btnExceptionGithub.UseVisualStyleBackColor = true;
            this.btnExceptionGithub.Click += new System.EventHandler(this.btnExceptionGithub_Click);
            // 
            // lbVersion
            // 
            this.lbVersion.AutoSize = true;
            this.lbVersion.Location = new System.Drawing.Point(14, 219);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(50, 13);
            this.lbVersion.TabIndex = 9;
            this.lbVersion.Text = "lbVersion";
            // 
            // btnExceptionOk
            // 
            this.btnExceptionOk.Location = new System.Drawing.Point(385, 213);
            this.btnExceptionOk.Name = "btnExceptionOk";
            this.btnExceptionOk.Size = new System.Drawing.Size(75, 24);
            this.btnExceptionOk.TabIndex = 11;
            this.btnExceptionOk.Text = "btnExceptionOk";
            this.btnExceptionOk.UseVisualStyleBackColor = true;
            this.btnExceptionOk.Click += new System.EventHandler(this.btnExceptionOk_Click);
            // 
            // rtbExceptionDetails
            // 
            this.rtbExceptionDetails.Location = new System.Drawing.Point(12, 61);
            this.rtbExceptionDetails.Name = "rtbExceptionDetails";
            this.rtbExceptionDetails.ReadOnly = true;
            this.rtbExceptionDetails.Size = new System.Drawing.Size(448, 146);
            this.rtbExceptionDetails.TabIndex = 8;
            this.rtbExceptionDetails.Text = "rtbExceptionDetails";
            // 
            // lbExceptionDescription
            // 
            this.lbExceptionDescription.AutoSize = true;
            this.lbExceptionDescription.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbExceptionDescription.Location = new System.Drawing.Point(13, 34);
            this.lbExceptionDescription.Name = "lbExceptionDescription";
            this.lbExceptionDescription.Size = new System.Drawing.Size(141, 17);
            this.lbExceptionDescription.TabIndex = 7;
            this.lbExceptionDescription.Text = "lbExceptionDescription";
            // 
            // lbExceptionHeader
            // 
            this.lbExceptionHeader.AutoSize = true;
            this.lbExceptionHeader.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbExceptionHeader.Location = new System.Drawing.Point(12, 3);
            this.lbExceptionHeader.Name = "lbExceptionHeader";
            this.lbExceptionHeader.Size = new System.Drawing.Size(171, 25);
            this.lbExceptionHeader.TabIndex = 6;
            this.lbExceptionHeader.Text = "lbExceptionHeader";
            // 
            // frmException
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(472, 249);
            this.Controls.Add(this.btnExceptionGithub);
            this.Controls.Add(this.lbVersion);
            this.Controls.Add(this.btnExceptionOk);
            this.Controls.Add(this.rtbExceptionDetails);
            this.Controls.Add(this.lbExceptionDescription);
            this.Controls.Add(this.lbExceptionHeader);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmException";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmException";
            this.Load += new System.EventHandler(this.frmException_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExceptionGithub;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.Button btnExceptionOk;
        private System.Windows.Forms.RichTextBox rtbExceptionDetails;
        private System.Windows.Forms.Label lbExceptionDescription;
        private System.Windows.Forms.Label lbExceptionHeader;
    }
}