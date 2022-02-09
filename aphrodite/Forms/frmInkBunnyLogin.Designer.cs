namespace aphrodite {
    partial class frmInkBunnyLogin {
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
            this.txtUserLoginPassword = new System.Windows.Forms.TextBox();
            this.lbUserLoginPassword = new System.Windows.Forms.Label();
            this.txtUserLoginUsername = new System.Windows.Forms.TextBox();
            this.lbUserLoginUsername = new System.Windows.Forms.Label();
            this.btnLoginAsUser = new System.Windows.Forms.Button();
            this.rbLoginAsUser = new System.Windows.Forms.RadioButton();
            this.rbLoginAsGuest = new System.Windows.Forms.RadioButton();
            this.plUserLogin = new System.Windows.Forms.Panel();
            this.llLogoutFromUser = new murrty.controls.ExtendedLinkLabel();
            this.lbLoggedInAs = new System.Windows.Forms.Label();
            this.plGuestLogin = new System.Windows.Forms.Panel();
            this.lbGuestSessionID = new System.Windows.Forms.Label();
            this.chkGuestNewSID = new System.Windows.Forms.CheckBox();
            this.btnLoginAsGuest = new System.Windows.Forms.Button();
            this.gbRatings = new System.Windows.Forms.GroupBox();
            this.chkGuestRatingGeneral = new System.Windows.Forms.CheckBox();
            this.chkGuestRatingMatureNudity = new System.Windows.Forms.CheckBox();
            this.chkGuestRatingMatureViolence = new System.Windows.Forms.CheckBox();
            this.chkGuestRatingAdultSexualThemes = new System.Windows.Forms.CheckBox();
            this.chkGuestRatingAdultStrongViolence = new System.Windows.Forms.CheckBox();
            this.rbManuallyApplySID = new System.Windows.Forms.RadioButton();
            this.plManuallySetSID = new System.Windows.Forms.Panel();
            this.lbSidUsername = new System.Windows.Forms.Label();
            this.txtSidUsername = new System.Windows.Forms.TextBox();
            this.lbManuallySetSIDSessionID = new System.Windows.Forms.Label();
            this.txtSessionID = new System.Windows.Forms.TextBox();
            this.btnSetSessionID = new System.Windows.Forms.Button();
            this.chkUseGuestAsPriority = new System.Windows.Forms.CheckBox();
            this.btnSetGuestPriority = new System.Windows.Forms.Button();
            this.pnLower = new System.Windows.Forms.Panel();
            this.btnCancel = new murrty.controls.ExtendedButton();
            this.plUserLogin.SuspendLayout();
            this.plGuestLogin.SuspendLayout();
            this.gbRatings.SuspendLayout();
            this.plManuallySetSID.SuspendLayout();
            this.pnLower.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtUserLoginPassword
            // 
            this.txtUserLoginPassword.Location = new System.Drawing.Point(38, 110);
            this.txtUserLoginPassword.Name = "txtUserLoginPassword";
            this.txtUserLoginPassword.PasswordChar = 'ツ';
            this.txtUserLoginPassword.Size = new System.Drawing.Size(207, 20);
            this.txtUserLoginPassword.TabIndex = 3;
            this.txtUserLoginPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.UserLoginTextBox_KeyPress);
            // 
            // lbUserLoginPassword
            // 
            this.lbUserLoginPassword.AutoSize = true;
            this.lbUserLoginPassword.Location = new System.Drawing.Point(35, 90);
            this.lbUserLoginPassword.Name = "lbUserLoginPassword";
            this.lbUserLoginPassword.Size = new System.Drawing.Size(52, 13);
            this.lbUserLoginPassword.TabIndex = 2;
            this.lbUserLoginPassword.Text = "password";
            // 
            // txtUserLoginUsername
            // 
            this.txtUserLoginUsername.Location = new System.Drawing.Point(38, 59);
            this.txtUserLoginUsername.Name = "txtUserLoginUsername";
            this.txtUserLoginUsername.Size = new System.Drawing.Size(207, 20);
            this.txtUserLoginUsername.TabIndex = 1;
            this.txtUserLoginUsername.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.UserLoginTextBox_KeyPress);
            // 
            // lbUserLoginUsername
            // 
            this.lbUserLoginUsername.AutoSize = true;
            this.lbUserLoginUsername.Location = new System.Drawing.Point(35, 39);
            this.lbUserLoginUsername.Name = "lbUserLoginUsername";
            this.lbUserLoginUsername.Size = new System.Drawing.Size(53, 13);
            this.lbUserLoginUsername.TabIndex = 0;
            this.lbUserLoginUsername.Text = "username";
            // 
            // btnLoginAsUser
            // 
            this.btnLoginAsUser.Location = new System.Drawing.Point(101, 137);
            this.btnLoginAsUser.Name = "btnLoginAsUser";
            this.btnLoginAsUser.Size = new System.Drawing.Size(80, 23);
            this.btnLoginAsUser.TabIndex = 4;
            this.btnLoginAsUser.Text = "login";
            this.btnLoginAsUser.UseVisualStyleBackColor = true;
            this.btnLoginAsUser.Click += new System.EventHandler(this.btnLoginAsUser_Click);
            // 
            // rbLoginAsUser
            // 
            this.rbLoginAsUser.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbLoginAsUser.AutoSize = true;
            this.rbLoginAsUser.Location = new System.Drawing.Point(16, 12);
            this.rbLoginAsUser.Name = "rbLoginAsUser";
            this.rbLoginAsUser.Size = new System.Drawing.Size(76, 23);
            this.rbLoginAsUser.TabIndex = 0;
            this.rbLoginAsUser.TabStop = true;
            this.rbLoginAsUser.Text = "login as user";
            this.rbLoginAsUser.UseVisualStyleBackColor = true;
            this.rbLoginAsUser.CheckedChanged += new System.EventHandler(this.rbLoginAsUser_CheckedChanged);
            // 
            // rbLoginAsGuest
            // 
            this.rbLoginAsGuest.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbLoginAsGuest.AutoSize = true;
            this.rbLoginAsGuest.Location = new System.Drawing.Point(108, 12);
            this.rbLoginAsGuest.Name = "rbLoginAsGuest";
            this.rbLoginAsGuest.Size = new System.Drawing.Size(82, 23);
            this.rbLoginAsGuest.TabIndex = 1;
            this.rbLoginAsGuest.TabStop = true;
            this.rbLoginAsGuest.Text = "login as guest";
            this.rbLoginAsGuest.UseVisualStyleBackColor = true;
            this.rbLoginAsGuest.CheckedChanged += new System.EventHandler(this.rbLoginAsGuest_CheckedChanged);
            // 
            // plUserLogin
            // 
            this.plUserLogin.Controls.Add(this.llLogoutFromUser);
            this.plUserLogin.Controls.Add(this.lbLoggedInAs);
            this.plUserLogin.Controls.Add(this.lbUserLoginUsername);
            this.plUserLogin.Controls.Add(this.txtUserLoginUsername);
            this.plUserLogin.Controls.Add(this.lbUserLoginPassword);
            this.plUserLogin.Controls.Add(this.btnLoginAsUser);
            this.plUserLogin.Controls.Add(this.txtUserLoginPassword);
            this.plUserLogin.Location = new System.Drawing.Point(16, 58);
            this.plUserLogin.Name = "plUserLogin";
            this.plUserLogin.Size = new System.Drawing.Size(281, 179);
            this.plUserLogin.TabIndex = 11;
            this.plUserLogin.Visible = false;
            // 
            // llLogoutFromUser
            // 
            this.llLogoutFromUser.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.llLogoutFromUser.AutoSize = true;
            this.llLogoutFromUser.Enabled = false;
            this.llLogoutFromUser.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.llLogoutFromUser.Location = new System.Drawing.Point(228, 21);
            this.llLogoutFromUser.Name = "llLogoutFromUser";
            this.llLogoutFromUser.Size = new System.Drawing.Size(40, 13);
            this.llLogoutFromUser.TabIndex = 6;
            this.llLogoutFromUser.TabStop = true;
            this.llLogoutFromUser.Text = "Logout";
            this.llLogoutFromUser.Visible = false;
            this.llLogoutFromUser.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            this.llLogoutFromUser.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llLogoutFromUser_LinkClicked);
            // 
            // lbLoggedInAs
            // 
            this.lbLoggedInAs.Location = new System.Drawing.Point(3, 8);
            this.lbLoggedInAs.Name = "lbLoggedInAs";
            this.lbLoggedInAs.Size = new System.Drawing.Size(275, 14);
            this.lbLoggedInAs.TabIndex = 5;
            this.lbLoggedInAs.Text = "logged in as: none";
            this.lbLoggedInAs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // plGuestLogin
            // 
            this.plGuestLogin.Controls.Add(this.lbGuestSessionID);
            this.plGuestLogin.Controls.Add(this.chkGuestNewSID);
            this.plGuestLogin.Controls.Add(this.btnLoginAsGuest);
            this.plGuestLogin.Controls.Add(this.gbRatings);
            this.plGuestLogin.Location = new System.Drawing.Point(16, 43);
            this.plGuestLogin.Name = "plGuestLogin";
            this.plGuestLogin.Size = new System.Drawing.Size(281, 208);
            this.plGuestLogin.TabIndex = 12;
            this.plGuestLogin.Visible = false;
            // 
            // lbGuestSessionID
            // 
            this.lbGuestSessionID.Location = new System.Drawing.Point(3, 161);
            this.lbGuestSessionID.Name = "lbGuestSessionID";
            this.lbGuestSessionID.Size = new System.Drawing.Size(275, 16);
            this.lbGuestSessionID.TabIndex = 3;
            this.lbGuestSessionID.Text = "guest session id: not logged in";
            this.lbGuestSessionID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbGuestSessionID.DoubleClick += new System.EventHandler(this.lbGuestSessionID_DoubleClick);
            // 
            // chkGuestNewSID
            // 
            this.chkGuestNewSID.AutoSize = true;
            this.chkGuestNewSID.Location = new System.Drawing.Point(70, 144);
            this.chkGuestNewSID.Name = "chkGuestNewSID";
            this.chkGuestNewSID.Size = new System.Drawing.Size(141, 17);
            this.chkGuestNewSID.TabIndex = 2;
            this.chkGuestNewSID.Text = "get new guest session id";
            this.chkGuestNewSID.UseVisualStyleBackColor = true;
            this.chkGuestNewSID.CheckedChanged += new System.EventHandler(this.chkGuestNewSID_CheckedChanged);
            // 
            // btnLoginAsGuest
            // 
            this.btnLoginAsGuest.Location = new System.Drawing.Point(75, 179);
            this.btnLoginAsGuest.Name = "btnLoginAsGuest";
            this.btnLoginAsGuest.Size = new System.Drawing.Size(130, 23);
            this.btnLoginAsGuest.TabIndex = 1;
            this.btnLoginAsGuest.Text = "login as guest";
            this.btnLoginAsGuest.UseVisualStyleBackColor = true;
            this.btnLoginAsGuest.Click += new System.EventHandler(this.btnLoginAsGuest_Click);
            // 
            // gbRatings
            // 
            this.gbRatings.Controls.Add(this.chkGuestRatingGeneral);
            this.gbRatings.Controls.Add(this.chkGuestRatingMatureNudity);
            this.gbRatings.Controls.Add(this.chkGuestRatingMatureViolence);
            this.gbRatings.Controls.Add(this.chkGuestRatingAdultSexualThemes);
            this.gbRatings.Controls.Add(this.chkGuestRatingAdultStrongViolence);
            this.gbRatings.Location = new System.Drawing.Point(58, 4);
            this.gbRatings.Name = "gbRatings";
            this.gbRatings.Size = new System.Drawing.Size(164, 137);
            this.gbRatings.TabIndex = 0;
            this.gbRatings.TabStop = false;
            this.gbRatings.Text = "allowed ratings";
            // 
            // chkGuestRatingGeneral
            // 
            this.chkGuestRatingGeneral.AutoSize = true;
            this.chkGuestRatingGeneral.Checked = true;
            this.chkGuestRatingGeneral.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGuestRatingGeneral.Enabled = false;
            this.chkGuestRatingGeneral.Location = new System.Drawing.Point(17, 19);
            this.chkGuestRatingGeneral.Name = "chkGuestRatingGeneral";
            this.chkGuestRatingGeneral.Size = new System.Drawing.Size(60, 17);
            this.chkGuestRatingGeneral.TabIndex = 0;
            this.chkGuestRatingGeneral.Text = "general";
            this.chkGuestRatingGeneral.UseVisualStyleBackColor = true;
            // 
            // chkGuestRatingMatureNudity
            // 
            this.chkGuestRatingMatureNudity.AutoSize = true;
            this.chkGuestRatingMatureNudity.Location = new System.Drawing.Point(17, 42);
            this.chkGuestRatingMatureNudity.Name = "chkGuestRatingMatureNudity";
            this.chkGuestRatingMatureNudity.Size = new System.Drawing.Size(94, 17);
            this.chkGuestRatingMatureNudity.TabIndex = 1;
            this.chkGuestRatingMatureNudity.Text = "mature - nudity";
            this.chkGuestRatingMatureNudity.UseVisualStyleBackColor = true;
            // 
            // chkGuestRatingMatureViolence
            // 
            this.chkGuestRatingMatureViolence.AutoSize = true;
            this.chkGuestRatingMatureViolence.Checked = true;
            this.chkGuestRatingMatureViolence.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGuestRatingMatureViolence.Location = new System.Drawing.Point(17, 65);
            this.chkGuestRatingMatureViolence.Name = "chkGuestRatingMatureViolence";
            this.chkGuestRatingMatureViolence.Size = new System.Drawing.Size(106, 17);
            this.chkGuestRatingMatureViolence.TabIndex = 2;
            this.chkGuestRatingMatureViolence.Text = "mature - violence";
            this.chkGuestRatingMatureViolence.UseVisualStyleBackColor = true;
            // 
            // chkGuestRatingAdultSexualThemes
            // 
            this.chkGuestRatingAdultSexualThemes.AutoSize = true;
            this.chkGuestRatingAdultSexualThemes.Location = new System.Drawing.Point(17, 88);
            this.chkGuestRatingAdultSexualThemes.Name = "chkGuestRatingAdultSexualThemes";
            this.chkGuestRatingAdultSexualThemes.Size = new System.Drawing.Size(124, 17);
            this.chkGuestRatingAdultSexualThemes.TabIndex = 3;
            this.chkGuestRatingAdultSexualThemes.Text = "adult - sexual themes";
            this.chkGuestRatingAdultSexualThemes.UseVisualStyleBackColor = true;
            // 
            // chkGuestRatingAdultStrongViolence
            // 
            this.chkGuestRatingAdultStrongViolence.AutoSize = true;
            this.chkGuestRatingAdultStrongViolence.Location = new System.Drawing.Point(17, 111);
            this.chkGuestRatingAdultStrongViolence.Name = "chkGuestRatingAdultStrongViolence";
            this.chkGuestRatingAdultStrongViolence.Size = new System.Drawing.Size(129, 17);
            this.chkGuestRatingAdultStrongViolence.TabIndex = 4;
            this.chkGuestRatingAdultStrongViolence.Text = "adult - strong violence";
            this.chkGuestRatingAdultStrongViolence.UseVisualStyleBackColor = true;
            // 
            // rbManuallyApplySID
            // 
            this.rbManuallyApplySID.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbManuallyApplySID.AutoSize = true;
            this.rbManuallyApplySID.Location = new System.Drawing.Point(206, 12);
            this.rbManuallyApplySID.Name = "rbManuallyApplySID";
            this.rbManuallyApplySID.Size = new System.Drawing.Size(91, 23);
            this.rbManuallyApplySID.TabIndex = 2;
            this.rbManuallyApplySID.TabStop = true;
            this.rbManuallyApplySID.Text = "manually set sid";
            this.rbManuallyApplySID.UseVisualStyleBackColor = true;
            this.rbManuallyApplySID.CheckedChanged += new System.EventHandler(this.rbManuallyApplySID_CheckedChanged);
            // 
            // plManuallySetSID
            // 
            this.plManuallySetSID.Controls.Add(this.lbSidUsername);
            this.plManuallySetSID.Controls.Add(this.txtSidUsername);
            this.plManuallySetSID.Controls.Add(this.lbManuallySetSIDSessionID);
            this.plManuallySetSID.Controls.Add(this.txtSessionID);
            this.plManuallySetSID.Controls.Add(this.btnSetSessionID);
            this.plManuallySetSID.Location = new System.Drawing.Point(16, 79);
            this.plManuallySetSID.Name = "plManuallySetSID";
            this.plManuallySetSID.Size = new System.Drawing.Size(281, 137);
            this.plManuallySetSID.TabIndex = 14;
            this.plManuallySetSID.Visible = false;
            // 
            // lbSidUsername
            // 
            this.lbSidUsername.AutoSize = true;
            this.lbSidUsername.Location = new System.Drawing.Point(34, 54);
            this.lbSidUsername.Name = "lbSidUsername";
            this.lbSidUsername.Size = new System.Drawing.Size(99, 13);
            this.lbSidUsername.TabIndex = 3;
            this.lbSidUsername.Text = "username (optional)";
            // 
            // txtSidUsername
            // 
            this.txtSidUsername.Location = new System.Drawing.Point(37, 74);
            this.txtSidUsername.Name = "txtSidUsername";
            this.txtSidUsername.Size = new System.Drawing.Size(207, 20);
            this.txtSidUsername.TabIndex = 4;
            this.txtSidUsername.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ManuallySetSessionIDTextBox_KeyPress);
            // 
            // lbManuallySetSIDSessionID
            // 
            this.lbManuallySetSIDSessionID.AutoSize = true;
            this.lbManuallySetSIDSessionID.Location = new System.Drawing.Point(35, 7);
            this.lbManuallySetSIDSessionID.Name = "lbManuallySetSIDSessionID";
            this.lbManuallySetSIDSessionID.Size = new System.Drawing.Size(53, 13);
            this.lbManuallySetSIDSessionID.TabIndex = 0;
            this.lbManuallySetSIDSessionID.Text = "session id";
            // 
            // txtSessionID
            // 
            this.txtSessionID.Location = new System.Drawing.Point(38, 27);
            this.txtSessionID.Name = "txtSessionID";
            this.txtSessionID.Size = new System.Drawing.Size(207, 20);
            this.txtSessionID.TabIndex = 1;
            this.txtSessionID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ManuallySetSessionIDTextBox_KeyPress);
            // 
            // btnSetSessionID
            // 
            this.btnSetSessionID.Location = new System.Drawing.Point(101, 105);
            this.btnSetSessionID.Name = "btnSetSessionID";
            this.btnSetSessionID.Size = new System.Drawing.Size(80, 23);
            this.btnSetSessionID.TabIndex = 2;
            this.btnSetSessionID.Text = "login";
            this.btnSetSessionID.UseVisualStyleBackColor = true;
            this.btnSetSessionID.Click += new System.EventHandler(this.btnSetSessionID_Click);
            // 
            // chkUseGuestAsPriority
            // 
            this.chkUseGuestAsPriority.AutoSize = true;
            this.chkUseGuestAsPriority.Location = new System.Drawing.Point(16, 261);
            this.chkUseGuestAsPriority.Name = "chkUseGuestAsPriority";
            this.chkUseGuestAsPriority.Size = new System.Drawing.Size(134, 17);
            this.chkUseGuestAsPriority.TabIndex = 15;
            this.chkUseGuestAsPriority.Text = "use guest sid as priority";
            this.chkUseGuestAsPriority.UseVisualStyleBackColor = true;
            // 
            // btnSetGuestPriority
            // 
            this.btnSetGuestPriority.Location = new System.Drawing.Point(187, 257);
            this.btnSetGuestPriority.Name = "btnSetGuestPriority";
            this.btnSetGuestPriority.Size = new System.Drawing.Size(110, 23);
            this.btnSetGuestPriority.TabIndex = 16;
            this.btnSetGuestPriority.Text = "set guest priority";
            this.btnSetGuestPriority.UseVisualStyleBackColor = true;
            this.btnSetGuestPriority.Click += new System.EventHandler(this.btnSetGuestPriority_Click);
            // 
            // pnLower
            // 
            this.pnLower.BackColor = System.Drawing.SystemColors.Menu;
            this.pnLower.Controls.Add(this.btnCancel);
            this.pnLower.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnLower.Location = new System.Drawing.Point(0, 291);
            this.pnLower.Name = "pnLower";
            this.pnLower.Size = new System.Drawing.Size(312, 42);
            this.pnLower.TabIndex = 28;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(222, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(71, 23);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmInkBunnyLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(312, 333);
            this.Controls.Add(this.pnLower);
            this.Controls.Add(this.chkUseGuestAsPriority);
            this.Controls.Add(this.btnSetGuestPriority);
            this.Controls.Add(this.rbManuallyApplySID);
            this.Controls.Add(this.rbLoginAsGuest);
            this.Controls.Add(this.rbLoginAsUser);
            this.Controls.Add(this.plManuallySetSID);
            this.Controls.Add(this.plUserLogin);
            this.Controls.Add(this.plGuestLogin);
            this.Icon = global::aphrodite.Properties.Resources.Brad;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(330, 370);
            this.MinimumSize = new System.Drawing.Size(330, 370);
            this.Name = "frmInkBunnyLogin";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "login to inkbunny";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmInkBunnyLogin_FormClosing);
            this.plUserLogin.ResumeLayout(false);
            this.plUserLogin.PerformLayout();
            this.plGuestLogin.ResumeLayout(false);
            this.plGuestLogin.PerformLayout();
            this.gbRatings.ResumeLayout(false);
            this.gbRatings.PerformLayout();
            this.plManuallySetSID.ResumeLayout(false);
            this.plManuallySetSID.PerformLayout();
            this.pnLower.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUserLoginPassword;
        private System.Windows.Forms.Label lbUserLoginPassword;
        private System.Windows.Forms.TextBox txtUserLoginUsername;
        private System.Windows.Forms.Label lbUserLoginUsername;
        private System.Windows.Forms.Button btnLoginAsUser;
        private System.Windows.Forms.RadioButton rbLoginAsUser;
        private System.Windows.Forms.RadioButton rbLoginAsGuest;
        private System.Windows.Forms.Panel plUserLogin;
        private System.Windows.Forms.Panel plGuestLogin;
        private System.Windows.Forms.GroupBox gbRatings;
        private System.Windows.Forms.CheckBox chkGuestRatingGeneral;
        private System.Windows.Forms.CheckBox chkGuestRatingMatureNudity;
        private System.Windows.Forms.CheckBox chkGuestRatingMatureViolence;
        private System.Windows.Forms.CheckBox chkGuestRatingAdultSexualThemes;
        private System.Windows.Forms.CheckBox chkGuestRatingAdultStrongViolence;
        private System.Windows.Forms.Button btnLoginAsGuest;
        private System.Windows.Forms.RadioButton rbManuallyApplySID;
        private System.Windows.Forms.Panel plManuallySetSID;
        private System.Windows.Forms.Label lbManuallySetSIDSessionID;
        private System.Windows.Forms.TextBox txtSessionID;
        private System.Windows.Forms.Button btnSetSessionID;
        private System.Windows.Forms.CheckBox chkUseGuestAsPriority;
        private System.Windows.Forms.Button btnSetGuestPriority;
        private System.Windows.Forms.CheckBox chkGuestNewSID;
        private System.Windows.Forms.Label lbGuestSessionID;
        private System.Windows.Forms.Label lbLoggedInAs;
        private System.Windows.Forms.Label lbSidUsername;
        private System.Windows.Forms.TextBox txtSidUsername;
        private System.Windows.Forms.Panel pnLower;
        private murrty.controls.ExtendedButton btnCancel;
        private murrty.controls.ExtendedLinkLabel llLogoutFromUser;
    }
}