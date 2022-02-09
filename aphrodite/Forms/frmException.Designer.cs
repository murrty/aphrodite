﻿namespace murrty.forms {
    partial class frmException {

        /// <summary>
        /// Contains the forms' controls.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if (disposing) {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Generates the forms' controls and configurations of them.
        /// <para>This should only be ran in the initializer once.</para>
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmException));
            this.btnExceptionOk = new System.Windows.Forms.Button();
            this.lbVersion = new System.Windows.Forms.Label();
            this.btnExceptionGithub = new System.Windows.Forms.Button();
            this.btnExceptionRetry = new System.Windows.Forms.Button();
            this.lbDate = new System.Windows.Forms.Label();
            this.pnLower = new System.Windows.Forms.Panel();
            this.pnDWM = new System.Windows.Forms.Panel();
            this.lbExceptionHeader = new System.Windows.Forms.Label();
            this.rtbExceptionDetails = new aphrodite.Controls.ExtendedRichTextBox();
            this.lbExceptionDescription = new System.Windows.Forms.Label();
            this.pnLower.SuspendLayout();
            this.pnDWM.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExceptionOk
            // 
            this.btnExceptionOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExceptionOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnExceptionOk.Location = new System.Drawing.Point(385, 7);
            this.btnExceptionOk.Name = "btnExceptionOk";
            this.btnExceptionOk.Size = new System.Drawing.Size(75, 23);
            this.btnExceptionOk.TabIndex = 3;
            this.btnExceptionOk.Text = "GenericOk";
            this.btnExceptionOk.UseVisualStyleBackColor = true;
            // 
            // lbVersion
            // 
            this.lbVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbVersion.AutoSize = true;
            this.lbVersion.Location = new System.Drawing.Point(12, 13);
            this.lbVersion.Name = "lbVersion";
            this.lbVersion.Size = new System.Drawing.Size(55, 13);
            this.lbVersion.TabIndex = 4;
            this.lbVersion.Text = "lbVersion";
            // 
            // btnExceptionGithub
            // 
            this.btnExceptionGithub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExceptionGithub.Location = new System.Drawing.Point(223, 7);
            this.btnExceptionGithub.Name = "btnExceptionGithub";
            this.btnExceptionGithub.Size = new System.Drawing.Size(75, 23);
            this.btnExceptionGithub.TabIndex = 5;
            this.btnExceptionGithub.Text = "btnExceptionGithub";
            this.btnExceptionGithub.UseVisualStyleBackColor = true;
            this.btnExceptionGithub.Visible = false;
            this.btnExceptionGithub.Click += new System.EventHandler(this.btnExceptionGithub_Click);
            // 
            // btnExceptionRetry
            // 
            this.btnExceptionRetry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExceptionRetry.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.btnExceptionRetry.Location = new System.Drawing.Point(304, 7);
            this.btnExceptionRetry.Name = "btnExceptionRetry";
            this.btnExceptionRetry.Size = new System.Drawing.Size(75, 23);
            this.btnExceptionRetry.TabIndex = 14;
            this.btnExceptionRetry.Text = "GenericRetry";
            this.btnExceptionRetry.UseVisualStyleBackColor = true;
            this.btnExceptionRetry.Visible = false;
            // 
            // lbDate
            // 
            this.lbDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDate.Location = new System.Drawing.Point(104, 7);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(113, 24);
            this.lbDate.TabIndex = 15;
            this.lbDate.Text = "99/99/99 99:99:98";
            this.lbDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnLower
            // 
            this.pnLower.BackColor = System.Drawing.SystemColors.Menu;
            this.pnLower.Controls.Add(this.btnExceptionOk);
            this.pnLower.Controls.Add(this.lbVersion);
            this.pnLower.Controls.Add(this.lbDate);
            this.pnLower.Controls.Add(this.btnExceptionGithub);
            this.pnLower.Controls.Add(this.btnExceptionRetry);
            this.pnLower.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnLower.Location = new System.Drawing.Point(0, 241);
            this.pnLower.Name = "pnLower";
            this.pnLower.Size = new System.Drawing.Size(472, 42);
            this.pnLower.TabIndex = 16;
            // 
            // pnDWM
            // 
            this.pnDWM.Controls.Add(this.lbExceptionHeader);
            this.pnDWM.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDWM.Location = new System.Drawing.Point(0, 0);
            this.pnDWM.Name = "pnDWM";
            this.pnDWM.Size = new System.Drawing.Size(472, 37);
            this.pnDWM.TabIndex = 0;
            this.pnDWM.Visible = false;
            // 
            // lbExceptionHeader
            // 
            this.lbExceptionHeader.AutoSize = true;
            this.lbExceptionHeader.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold);
            this.lbExceptionHeader.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lbExceptionHeader.Location = new System.Drawing.Point(7, 0);
            this.lbExceptionHeader.Name = "lbExceptionHeader";
            this.lbExceptionHeader.Size = new System.Drawing.Size(174, 25);
            this.lbExceptionHeader.TabIndex = 17;
            this.lbExceptionHeader.Text = "lbExceptionHeader";
            this.lbExceptionHeader.Visible = false;
            // 
            // rtbExceptionDetails
            // 
            this.rtbExceptionDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbExceptionDetails.DetectUrls = false;
            this.rtbExceptionDetails.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbExceptionDetails.Location = new System.Drawing.Point(4, 67);
            this.rtbExceptionDetails.Name = "rtbExceptionDetails";
            this.rtbExceptionDetails.ReadOnly = true;
            this.rtbExceptionDetails.Size = new System.Drawing.Size(464, 166);
            this.rtbExceptionDetails.TabIndex = 19;
            this.rtbExceptionDetails.Text = "rtbExceptionDetails";
            // 
            // lbExceptionDescription
            // 
            this.lbExceptionDescription.AutoSize = true;
            this.lbExceptionDescription.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbExceptionDescription.Location = new System.Drawing.Point(15, 42);
            this.lbExceptionDescription.Name = "lbExceptionDescription";
            this.lbExceptionDescription.Size = new System.Drawing.Size(141, 17);
            this.lbExceptionDescription.TabIndex = 1;
            this.lbExceptionDescription.Text = "lbExceptionDescription";
            // 
            // frmException
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(472, 283);
            this.Controls.Add(this.lbExceptionDescription);
            this.Controls.Add(this.rtbExceptionDetails);
            this.Controls.Add(this.pnLower);
            this.Controls.Add(this.pnDWM);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1280, 720);
            this.MinimumSize = new System.Drawing.Size(490, 320);
            this.Name = "frmException";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmException";
            this.Load += new System.EventHandler(this.frmError_Load);
            this.pnLower.ResumeLayout(false);
            this.pnLower.PerformLayout();
            this.pnDWM.ResumeLayout(false);
            this.pnDWM.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnExceptionOk;
        private System.Windows.Forms.Label lbVersion;
        private System.Windows.Forms.Button btnExceptionGithub;
        private System.Windows.Forms.Button btnExceptionRetry;
        private System.Windows.Forms.Label lbDate;
        private System.Windows.Forms.Panel pnLower;
        private System.Windows.Forms.Panel pnDWM;
        private System.Windows.Forms.Label lbExceptionHeader;
        private aphrodite.Controls.ExtendedRichTextBox rtbExceptionDetails;
        private System.Windows.Forms.Label lbExceptionDescription;
    }
}