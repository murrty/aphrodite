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
            this.btnExceptionGithub = new aphrodite.Controls.ExtendedButton();
            this.lbVersion = new System.Windows.Forms.Label();
            this.btnExceptionOk = new aphrodite.Controls.ExtendedButton();
            this.rtbExceptionDetails = new System.Windows.Forms.RichTextBox();
            this.lbExceptionDescription = new System.Windows.Forms.Label();
            this.lbExceptionHeader = new System.Windows.Forms.Label();
            this.lbDate = new System.Windows.Forms.Label();
            this.pnLower = new System.Windows.Forms.Panel();
            this.btnExceptionRetry = new System.Windows.Forms.Button();
            this.pnLower.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExceptionGithub
            // 
            this.btnExceptionGithub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExceptionGithub.Location = new System.Drawing.Point(213, 8);
            this.btnExceptionGithub.Name = "btnExceptionGithub";
            this.btnExceptionGithub.Size = new System.Drawing.Size(85, 24);
            this.btnExceptionGithub.TabIndex = 10;
            this.btnExceptionGithub.Text = "Github >w<";
            this.btnExceptionGithub.UseVisualStyleBackColor = true;
            this.btnExceptionGithub.Click += new System.EventHandler(this.btnExceptionGithub_Click);
            // 
            // lbVersion
            // 
            this.lbVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbVersion.AutoSize = true;
            this.lbVersion.Location = new System.Drawing.Point(12, 13);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(55, 13);
            this.lbVersion.TabIndex = 9;
            this.lbVersion.Text = "lbVersion";
            // 
            // btnExceptionOk
            // 
            this.btnExceptionOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExceptionOk.Location = new System.Drawing.Point(385, 7);
            this.btnExceptionOk.Name = "btnExceptionOk";
            this.btnExceptionOk.Size = new System.Drawing.Size(75, 24);
            this.btnExceptionOk.TabIndex = 11;
            this.btnExceptionOk.Text = "Okie uwu";
            this.btnExceptionOk.UseVisualStyleBackColor = true;
            this.btnExceptionOk.Click += new System.EventHandler(this.btnExceptionOk_Click);
            // 
            // rtbExceptionDetails
            // 
            this.rtbExceptionDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbExceptionDetails.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbExceptionDetails.Location = new System.Drawing.Point(12, 61);
            this.rtbExceptionDetails.Name = "rtbExceptionDetails";
            this.rtbExceptionDetails.ReadOnly = true;
            this.rtbExceptionDetails.Size = new System.Drawing.Size(448, 150);
            this.rtbExceptionDetails.TabIndex = 8;
            this.rtbExceptionDetails.Text = "rtbExceptionDetails";
            // 
            // lbExceptionDescription
            // 
            this.lbExceptionDescription.AutoSize = true;
            this.lbExceptionDescription.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbExceptionDescription.Location = new System.Drawing.Point(13, 34);
            this.lbExceptionDescription.Name = "lbExceptionDescription";
            this.lbExceptionDescription.Size = new System.Drawing.Size(430, 17);
            this.lbExceptionDescription.TabIndex = 7;
            this.lbExceptionDescription.Text = "The pwogwam accidentawy made a fucky wucky wooking at degenewacy";
            // 
            // lbExceptionHeader
            // 
            this.lbExceptionHeader.AutoSize = true;
            this.lbExceptionHeader.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbExceptionHeader.Location = new System.Drawing.Point(12, 3);
            this.lbExceptionHeader.Name = "lbExceptionHeader";
            this.lbExceptionHeader.Size = new System.Drawing.Size(257, 25);
            this.lbExceptionHeader.TabIndex = 6;
            this.lbExceptionHeader.Text = "An exception occowwed qwq";
            // 
            // lbDate
            // 
            this.lbDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDate.Location = new System.Drawing.Point(94, 7);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(113, 24);
            this.lbDate.TabIndex = 12;
            this.lbDate.Text = "99/99/99 99:99:98";
            this.lbDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnLower
            // 
            this.pnLower.BackColor = System.Drawing.SystemColors.Menu;
            this.pnLower.Controls.Add(this.btnExceptionRetry);
            this.pnLower.Controls.Add(this.btnExceptionGithub);
            this.pnLower.Controls.Add(this.lbDate);
            this.pnLower.Controls.Add(this.lbVersion);
            this.pnLower.Controls.Add(this.btnExceptionOk);
            this.pnLower.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnLower.Location = new System.Drawing.Point(0, 219);
            this.pnLower.Name = "pnLower";
            this.pnLower.Size = new System.Drawing.Size(472, 42);
            this.pnLower.TabIndex = 13;
            // 
            // btnExceptionRetry
            // 
            this.btnExceptionRetry.Enabled = false;
            this.btnExceptionRetry.Location = new System.Drawing.Point(304, 8);
            this.btnExceptionRetry.Name = "btnExceptionRetry";
            this.btnExceptionRetry.Size = new System.Drawing.Size(75, 23);
            this.btnExceptionRetry.TabIndex = 13;
            this.btnExceptionRetry.Text = "Try Agaiwn";
            this.btnExceptionRetry.UseVisualStyleBackColor = true;
            this.btnExceptionRetry.Click += new System.EventHandler(this.btnExceptionRetry_Click);
            // 
            // frmException
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 261);
            this.Controls.Add(this.rtbExceptionDetails);
            this.Controls.Add(this.lbExceptionDescription);
            this.Controls.Add(this.lbExceptionHeader);
            this.Controls.Add(this.pnLower);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(490, 290);
            this.Name = "frmException";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exception occowwed unu";
            this.Load += new System.EventHandler(this.frmException_Load);
            this.pnLower.ResumeLayout(false);
            this.pnLower.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.ExtendedButton btnExceptionGithub;
        private System.Windows.Forms.Label lbVersion;
        private Controls.ExtendedButton btnExceptionOk;
        private System.Windows.Forms.RichTextBox rtbExceptionDetails;
        private System.Windows.Forms.Label lbExceptionDescription;
        private System.Windows.Forms.Label lbExceptionHeader;
        private System.Windows.Forms.Label lbDate;
        private System.Windows.Forms.Panel pnLower;
        private System.Windows.Forms.Button btnExceptionRetry;
    }
}