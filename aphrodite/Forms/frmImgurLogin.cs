using System;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using murrty.classcontrols;

namespace aphrodite {

    public partial class frmImgurLogin : ExtendedForm {

        /// <summary>
        /// The byte array data retrieved from the API.
        /// </summary>
        private byte[] ApiData;
        /// <summary>
        /// The xml formatted api data.
        /// </summary>
        private string XmlData;

        public frmImgurLogin() {
            InitializeComponent();
            if (Config.ValidPoint(Config.Settings.FormSettings.frmImgurLogin_Location)) {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = Config.Settings.FormSettings.frmImgurLogin_Location;
            }
        }

        private void frmImgurLogin_FormClosing(Object sender, FormClosingEventArgs e) {
            Config.Settings.FormSettings.frmImgurLogin_Location = this.Location;
        }

        private void txtClientId_KeyPress(Object sender, KeyPressEventArgs e) {
            if(e.KeyChar == (char)Keys.Return) {
                btnTestSave_Click(txtClientId, null);
            }
        }

        private void txtClientId_ButtonClick(Object sender, EventArgs e) {
            txtClientId.Clear();
            txtClientId.Focus();
        }

        private void btnTestSave_Click(Object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(txtClientId.Text)) {
                txtClientId.Focus();
                System.Media.SystemSounds.Asterisk.Play();
                return;
            }

            txtClientId.Text = txtClientId.Text.Trim();
            string LoginURL = $"https://api.imgur.com/3/account/test?client_id={txtClientId.Text}";

            XmlData = DownloadString(LoginURL);
            if (XmlData == null) {
                Log.ReportException(new ApiReturnedNullOrEmptyException("Imgur user login data was null."));
            }

            xmlDoc = new();
            xmlDoc.LoadXml(XmlData);
            XmlNodeList xmlSuccess = xmlDoc.DocumentElement.SelectNodes("/root/success");
            XmlNodeList xmlStatus = xmlDoc.DocumentElement.SelectNodes("/root/status");
            if (xmlStatus.Count > 0) {
                if (xmlSuccess[0].InnerText.ToLower() == "true") {
                    switch (xmlStatus[0].InnerText.ToLower()) {
                        case "200": {
                            // The client ID is valid.
                            SaveClientID(txtClientId.Text);
                        } break;

                        default: {
                            // The client ID may be invalid.
                            if (Log.MessageBox($"The success code \"{xmlSuccess[0].InnerText}\" was not expected (200 was expected), while the success state \"true\" was. Would you like to save it anyway?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {
                                SaveClientID(txtClientId.Text);
                            }
                        } break;
                    }
                }
                else {
                    Log.MessageBox($"The success state is {xmlSuccess[0].InnerText} (not \"true\"). Cannot use provided client id.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            else {
                Log.MessageBox("The xml did not contain a status code, and cannot be verified.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btnCancel_Click(Object sender, EventArgs e) {
            this.DialogResult = DialogResult.Cancel;
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
                Data = null;
            }

            return Data;
        }

        private void SaveClientID(string ClientID) {
            ClientID = Cryptography.Encrypt(ClientID);
            Config.Settings.Imgur.ClientID = ClientID;
            Config.Settings.Imgur.Save();
            this.DialogResult = DialogResult.OK;
        }

    }
}
