using System;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using murrty.classcontrols;

namespace aphrodite {

    public partial class frmInkBunnyLogin : ExtendedForm {

        /// <summary>
        /// Whether the Guest SessionID exists.
        /// </summary>
        private bool GuestSIDExists = false;
        /// <summary>
        /// The byte array data retrieved from the API.
        /// </summary>
        private byte[] ApiData;
        /// <summary>
        /// The xml formatted api data.
        /// </summary>
        private string XmlData;

        public frmInkBunnyLogin() {
            InitializeComponent();

            if (Config.ValidPoint(Config.Settings.FormSettings.frmInkBunnyLogin_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmInkBunnyLogin_Location;
            }

            lbGuestSessionID.Cursor = NativeMethods.SystemHandCursor;
            Log.Write("User requested to login to InkBunny");

            if (!string.IsNullOrEmpty(Config.Settings.InkBunny.GuestSessionID)) {
                Log.Write("Guest SID exists");
                chkUseGuestAsPriority.Checked = Config.Settings.InkBunny.GuestPriority;
                GuestSIDExists = true;
                chkGuestNewSID.Enabled = true;
                chkGuestRatingGeneral.Checked = Config.Settings.InkBunny.GuestGeneral;
                chkGuestRatingMatureNudity.Checked = Config.Settings.InkBunny.GuestMatureNudity;
                chkGuestRatingMatureViolence.Checked = Config.Settings.InkBunny.GuestMatureViolence;
                chkGuestRatingAdultSexualThemes.Checked = Config.Settings.InkBunny.GuestAdultSexualThemes;
                chkGuestRatingAdultStrongViolence.Checked = Config.Settings.InkBunny.GuestAdultStrongViolence;
                btnLoginAsGuest.Text = "change viewed ratings";
                lbGuestSessionID.Text = "guest session id: " + Cryptography.Decrypt(Config.Settings.InkBunny.GuestSessionID);
            }
            else {
                chkUseGuestAsPriority.Enabled = false;
                btnSetGuestPriority.Enabled = false;
                chkGuestNewSID.Enabled = false;
            }

            if (!string.IsNullOrEmpty(Config.Settings.InkBunny.SessionID)) {
                lbLoggedInAs.Text = $"logged in as: {(string.IsNullOrEmpty(Config.Settings.InkBunny.LoggedInUsername) ? "<sid manually set>" : Config.Settings.InkBunny.LoggedInUsername)}";
                llLogoutFromUser.Enabled = true;
                llLogoutFromUser.Visible = true;
            }
        }

        private void frmInkBunnyLogin_FormClosing(Object sender, FormClosingEventArgs e) {
            Config.Settings.FormSettings.frmInkBunnyLogin_Location = this.Location;
        }

        private void btnCancel_Click(Object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
        }

        private void rbLoginAsUser_CheckedChanged(object sender, EventArgs e) {
            if (rbLoginAsUser.Checked) {
                plGuestLogin.Visible = false;
                plManuallySetSID.Visible = false;
                plUserLogin.Visible = true;

                chkGuestRatingGeneral.Checked = false;
                chkGuestRatingMatureNudity.Checked = false;
                chkGuestRatingMatureViolence.Checked = false;
                chkGuestRatingAdultSexualThemes.Checked = false;
                chkGuestRatingAdultStrongViolence.Checked = false;

                txtSessionID.Clear();
            }
        }
        private void rbLoginAsGuest_CheckedChanged(object sender, EventArgs e) {
            if (rbLoginAsGuest.Checked) {
                plUserLogin.Visible = false;
                plManuallySetSID.Visible = false;
                plGuestLogin.Visible = true;

                chkGuestRatingGeneral.Checked = true;
                chkGuestRatingMatureViolence.Checked = true;

                txtUserLoginUsername.Clear();
                txtUserLoginPassword.Clear();
                chkGuestRatingGeneral.Checked = true;

                txtSessionID.Clear();
            }
        }
        private void rbManuallyApplySID_CheckedChanged(object sender, EventArgs e) {
            if (rbManuallyApplySID.Checked) {
                plGuestLogin.Visible = false;
                plUserLogin.Visible = false;
                plManuallySetSID.Visible = true;

                chkGuestRatingGeneral.Checked = false;
                chkGuestRatingMatureNudity.Checked = false;
                chkGuestRatingMatureViolence.Checked = false;
                chkGuestRatingAdultSexualThemes.Checked = false;
                chkGuestRatingAdultStrongViolence.Checked = false;

                txtUserLoginUsername.Clear();
                txtUserLoginPassword.Clear();
                chkGuestRatingGeneral.Checked = true;
            }
        }

        private void UserLoginTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Return) {
                btnLoginAsUser_Click(sender, null);
            }
        }
        private void ManuallySetSessionIDTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Return) {
                btnSetSessionID_Click(sender, null);
            }
        }

        private void btnLoginAsUser_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtUserLoginUsername.Text)) {
                txtUserLoginUsername.Focus();
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtUserLoginPassword.Text)) {
                txtUserLoginPassword.Focus();
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            Log.Write($"Attempting to log in as {txtUserLoginUsername.Text}...");
            try {
                #region Login with the credentials supplied by the user
                string LoginURL = $"https://inkbunny.net/api_login.php?username={txtUserLoginUsername.Text}&password={txtUserLoginPassword.Text}";
                string SessionID = string.Empty;

                XmlData = DownloadString(LoginURL);
                if (XmlData == null) {
                    throw new ApiReturnedNullOrEmptyException("InkBunny user login data was null.");
                }

                xmlDoc = new();
                xmlDoc.LoadXml(XmlData);
                XmlNodeList xmlSID = xmlDoc.DocumentElement.SelectNodes("/root/sid");
                XmlNodeList xmlErrorCode = xmlDoc.DocumentElement.SelectNodes("/root/error_message");
                if (xmlErrorCode.Count > 0) {
                    Log.MessageBox(xmlErrorCode[0].InnerText);
                    LoginURL = null;
                    return;
                }

                SessionID = xmlSID[0].InnerText;
                #endregion

                #region Save it to the program
                Config.Settings.InkBunny.SessionID = Cryptography.Encrypt(SessionID);
                Config.Settings.InkBunny.LoggedInUsername = txtUserLoginUsername.Text;
                Config.Settings.InkBunny.GuestPriority = chkUseGuestAsPriority.Enabled && chkUseGuestAsPriority.Checked;
                Config.Settings.InkBunny.Save();

                // Clear anything that has a password :)
                XmlData = null;
                txtUserLoginPassword.Clear();
                SessionID = null;

                this.DialogResult = DialogResult.OK;
                #endregion
            }
            catch (ObjectDisposedException) {
                Log.Write("Seems like the inkbunny login form got disposed.");
                return;
            }
            catch (Exception ex) {
                Log.ReportException(ex);
                return;
            }
        }
        private void btnLoginAsGuest_Click(object sender, EventArgs e) {
            Log.Write("Attempting to log in as a Guest...");
            if (!chkGuestRatingGeneral.Checked && !chkGuestRatingMatureNudity.Checked
            && !chkGuestRatingMatureViolence.Checked && !chkGuestRatingAdultSexualThemes.Checked
            && !chkGuestRatingAdultStrongViolence.Checked) {
                Log.Write("No ratings were selected for the guest, using the defaults.");
                chkGuestRatingGeneral.Checked = chkGuestRatingMatureViolence.Checked = true;
            }

            try {
                if (GuestSIDExists && !chkGuestNewSID.Checked) {
                    // Check if the ratings do not match with any of the saved ones.
                    // If any options have changed, then change the allowed ratings.
                    if (chkGuestRatingGeneral.Checked != Config.Settings.InkBunny.GuestGeneral
                    || chkGuestRatingMatureNudity.Checked != Config.Settings.InkBunny.GuestMatureNudity
                    || chkGuestRatingMatureViolence.Checked != Config.Settings.InkBunny.GuestMatureViolence
                    || chkGuestRatingAdultSexualThemes.Checked != Config.Settings.InkBunny.GuestAdultSexualThemes
                    || chkGuestRatingAdultStrongViolence.Checked != Config.Settings.InkBunny.GuestAdultStrongViolence) {
                        SetGuestRating();
                        Config.Settings.InkBunny.Save();
                    }

                    this.DialogResult = DialogResult.Yes;
                }
                else {
                    if (Log.MessageBox("logging in as a guest won't require a password, or encrypting the session id, but, it will limit what kind of submissions you can view.\n\nsome users can opt to hide their submissions from guests, in which case you won't be able to download them.\n\ncontinue logging in as a guest?", MessageBoxButtons.YesNo) == DialogResult.Yes) {

                        #region Get Guest SID
                        string GuestLoginURL = "https://inkbunny.net/api_login.php?username=guest";
                        string GuestSID = string.Empty;

                        XmlData = DownloadString(GuestLoginURL);
                        if (XmlData == null) {
                            throw new ApiReturnedNullOrEmptyException("InkBunny guest login data was null.");
                        }

                        xmlDoc = new();
                        xmlDoc.LoadXml(XmlData);
                        XmlNodeList xmlSID = xmlDoc.DocumentElement.SelectNodes("/root/sid");
                        XmlNodeList xmlErrorMessage = xmlDoc.DocumentElement.SelectNodes("/root/error_message");
                        if (xmlErrorMessage.Count > 0) {
                            Log.MessageBox(xmlErrorMessage[0].InnerText);
                            return;
                        }

                        GuestSID = xmlSID[0].InnerText;
                        Log.Write($"Guest SID retrieved as {GuestSID}");
                        #endregion

                        #region Set the UserRatings for the SID (if applicable)
                        if (chkGuestRatingMatureNudity.Checked || !chkGuestRatingMatureViolence.Checked || chkGuestRatingAdultSexualThemes.Checked || chkGuestRatingAdultStrongViolence.Checked) {
                            SetGuestRating();
                        }
                        else {
                            Config.Settings.InkBunny.GuestGeneral = true;
                            Config.Settings.InkBunny.GuestMatureNudity = false;
                            Config.Settings.InkBunny.GuestMatureViolence = true;
                            Config.Settings.InkBunny.GuestAdultSexualThemes = false;
                            Config.Settings.InkBunny.GuestAdultStrongViolence = false;
                        }
                        #endregion

                        #region Save it to the program
                        Config.Settings.InkBunny.GuestPriority = chkUseGuestAsPriority.Enabled && chkUseGuestAsPriority.Checked;
                        Config.Settings.InkBunny.GuestSessionID = Cryptography.Encrypt(xmlSID[0].InnerText);
                        Config.Settings.InkBunny.Save();
                        GuestSIDExists = true;
                        this.DialogResult = DialogResult.OK;
                        #endregion

                    }
                }
            }
            catch (ObjectDisposedException) {
                Log.Write("Seems like the inkbunny login form got disposed.");
                return;
            }
            catch (Exception ex) {
                Log.ReportException(ex);
                return;
            }
        }
        private void btnSetSessionID_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtSessionID.Text)) {
                txtSessionID.Focus();
                System.Media.SystemSounds.Exclamation.Play();
                return;
            }

            try {
                #region Test the SID
                if (!Program.IsDebug) {
                    Log.Write("Attempting to validate the session id the user provided...");
                    string SidTestUrl = $"https://inkbunny.net/api_search.php?sid={txtSessionID.Text}";
                    XmlData = DownloadString(SidTestUrl);
                    SidTestUrl = null;
                    if (XmlData == null) {
                        throw new ApiReturnedNullOrEmptyException("InkBunny user login data was null.");
                    }
                    xmlDoc = new();
                    xmlDoc.LoadXml(XmlData);
                    XmlData = null;
                    XmlNodeList xmlErrorMessage = xmlDoc.DocumentElement.SelectNodes("/root/error_message");
                    if (xmlErrorMessage.Count > 0) {
                        Log.MessageBox(xmlErrorMessage[0].InnerText);
                        return;
                    }
                    Log.Write("The session id was validated successfully.");
                }
                else {
                    Log.Write("DebugMode active, skipping session id validation");
                }
                #endregion

                #region Save it to the program
                Log.Write("Encrypting the session id.");
                Config.Settings.InkBunny.SessionID = Cryptography.Encrypt(txtSessionID.Text);
                txtSessionID.Clear();
                Config.Settings.InkBunny.LoggedInUsername = txtSidUsername.Text;
                Config.Settings.InkBunny.GuestPriority = chkUseGuestAsPriority.Enabled && chkUseGuestAsPriority.Checked;
                Config.Settings.InkBunny.Save();
                this.DialogResult = DialogResult.OK;
                #endregion
            }
            catch (ObjectDisposedException) {
                Log.Write("Seems like the object got disposed.");
                return;
            }
            catch (Exception ex) {
                Log.ReportException(ex);
                return;
            }
        }

        private void llLogoutFromUser_LinkClicked(Object sender, LinkLabelLinkClickedEventArgs e) {
            Config.Settings.InkBunny.LoggedInUsername = string.Empty;
            Config.Settings.InkBunny.SessionID = string.Empty;
            this.DialogResult = DialogResult.OK;
        }

        private void btnSetGuestPriority_Click(object sender, EventArgs e) {
            Config.Settings.InkBunny.GuestPriority = chkUseGuestAsPriority.Checked;
            Config.Settings.InkBunny.Save();
            this.DialogResult = DialogResult.OK;
        }
        private void lbGuestSessionID_DoubleClick(object sender, EventArgs e) {
            if (GuestSIDExists) {
                Log.Write("Copying Guest SID to Clipboard");
                Clipboard.SetText(Config.Settings.InkBunny.GuestSessionID);
            }
        }
        private void chkGuestNewSID_CheckedChanged(object sender, EventArgs e) {
            btnLoginAsGuest.Text = chkGuestNewSID.Checked ? "login as guest" : "change viewed ratings";
        }

        private void SetGuestRating() {
            #region Set the UserRatings for the SID
            string UserRatingURL =
                $"https://inkbunny.net/api_userrating.php?sid={Cryptography.Decrypt(Config.Settings.InkBunny.GuestSessionID)}" +
                $"&tag[2]={(chkGuestRatingMatureNudity.Checked ? "yes" : "no")}" +
                $"&tag[3]={(chkGuestRatingMatureViolence.Checked ? "yes" : "no")}" +
                $"&tag[4]={(chkGuestRatingAdultSexualThemes.Checked ? "yes" : "no")}" +
                $"&tag[5]={(chkGuestRatingAdultStrongViolence.Checked ? "yes" : "no")}";

            XmlData = DownloadString(UserRatingURL);
            if (XmlData == null) {
                throw new ApiReturnedNullOrEmptyException("InkBunny set guest rating data was null.");
            }

            xmlDoc = new();
            xmlDoc.LoadXml(XmlData);
            XmlNodeList xmlErrorMessage = xmlDoc.DocumentElement.SelectNodes("/root/error_message");
            if (xmlErrorMessage.Count > 0) {
                Log.MessageBox(xmlErrorMessage[0].InnerText);
                return;
            }
            #endregion

            #region Save it to the program
            Config.Settings.InkBunny.GuestPriority = chkUseGuestAsPriority.Enabled && chkUseGuestAsPriority.Checked;
            Config.Settings.InkBunny.GuestGeneral = chkGuestRatingGeneral.Checked;
            Config.Settings.InkBunny.GuestMatureNudity = chkGuestRatingMatureNudity.Checked;
            Config.Settings.InkBunny.GuestMatureViolence = chkGuestRatingMatureViolence.Checked;
            Config.Settings.InkBunny.GuestAdultSexualThemes = chkGuestRatingAdultSexualThemes.Checked;
            Config.Settings.InkBunny.GuestAdultStrongViolence = chkGuestRatingAdultStrongViolence.Checked;
            #endregion
        }

        private string DownloadString(string URL) {
            string Data = null;
            ShouldRetry = true;
            do {
                try {
                    using (DownloadClient = new()) {
                        DownloadClient.Proxy = WebRequest.GetSystemWebProxy();
                        DownloadClient.UserAgent = Program.UserAgent;
                        DownloadClient.Method = HttpMethod.GET;

                        ApiData = DownloadClient.DownloadData(URL);
                        Data = ApiTools.ConvertJsonToXml(Encoding.UTF8.GetString(ApiData));
                        ApiData = null;

                        if (ApiTools.IsXmlDead(Data)) {
                            DownloadError = true;
                            Status = DownloadStatus.ApiReturnedNullOrEmpty;
                        }

                        ShouldRetry = false;
                    }
                }
                catch (Exception ex) {
                    if (ex is WebException webex && ((HttpWebResponse)webex.Response).StatusCode == HttpStatusCode.Forbidden) {
                        Status = DownloadStatus.Forbidden;
                        DownloadError = true;
                        ShouldRetry = false;
                    }
                    else {
                        if (Log.ReportRetriableException(ex, URL) != DialogResult.Retry) {
                            Status = DownloadStatus.Errored;
                            DownloadError = true;
                            ShouldRetry = false;
                        }
                    }
                }
            } while (ShouldRetry);

            if (DownloadError) {
                return null;
            }

            return Data;
        }

    }
}
