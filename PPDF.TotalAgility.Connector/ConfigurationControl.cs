using Microsoft.Win32;
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPDF.TotalAgility.Connector
{
    /// <summary>
    /// Configuration control hosted inside Power PDF's
    /// File -> Options -> Connectors -> TotalAgility panel.
    ///
    /// Layout: SDK URL, Auth Type, Credentials (context), Connect, Confirmation checkbox, Save.
    /// Process and Variables are configured via the Configure ribbon button.
    /// </summary>
    public partial class ConfigurationControl : UserControl
    {
        #region Registry Constants
        private const string REG_KEY = @"Software\ScanSoft\Connectors\TotalAgility";
        private const string TA_SDK_URL = "TA_SDK_URL";
        private const string TA_USER_ID = "TA_USER_ID";
        private const string TA_PASSWORD = "TA_PASSWORD";
        private const string TA_SESSION_ID = "TA_SESSION_ID";
        private const string TA_SHOW_CONFIRMATION = "TA_SHOW_CONFIRMATION";
        private const string TA_AUTH_TYPE = "TA_AUTH_TYPE";
        private const string TA_WINDOWS_USERNAME = "TA_WINDOWS_USERNAME";
        #endregion

        #region Auth Type Constants
        private const int AUTH_STANDARD = 0;
        private const int AUTH_WINDOWS = 1;
        private const int AUTH_FEDERATED = 2;
        #endregion

        #region State
        private Connector m_conn;
        private TotalAgilityService _taService;
        private string _sessionId;
        private ToolTip _toolTip = new ToolTip();
        #endregion

        #region Construction
        public ConfigurationControl(Connector conn)
        {
            InitializeComponent();
            StoreSheet();
            m_conn = conn;
        }

        public void CheckData() { }
        public void UpdateData() { }
        public void ShowHelp() { }
        #endregion

        #region Window Style
        [Flags] public enum WS_STYLE : int { WS_CHILD = 0x40000000 }
        [Flags] public enum DS_STYLE : int { DS_CONTROL = 0x0400 }
        [Flags] public enum WS_EX_STYLE : int { WS_EX_CONTROLPARENT = 0x00010000 }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams p = base.CreateParams;
                p.Style = (int)WS_STYLE.WS_CHILD | (int)DS_STYLE.DS_CONTROL;
                p.ExStyle = (int)WS_EX_STYLE.WS_EX_CONTROLPARENT;
                return p;
            }
        }
        #endregion

        #region Sheet Management
        protected override void OnHandleDestroyed(EventArgs e)
        {
            ClearSheet();
            base.OnHandleDestroyed(e);
        }

        protected static System.Collections.Generic.Dictionary<IntPtr, ConfigurationControl>
            sheetList = new System.Collections.Generic.Dictionary<IntPtr, ConfigurationControl>();

        protected void StoreSheet()
        {
            if (!sheetList.ContainsKey(Handle))
                sheetList.Add(Handle, this);
        }

        protected void ClearSheet()
        {
            if (sheetList.ContainsKey(Handle))
                sheetList.Remove(Handle);
        }

        public static ConfigurationControl GetSheet(IntPtr hwnd)
        {
            ConfigurationControl c;
            sheetList.TryGetValue(hwnd, out c);
            return c;
        }

        public static ConfigurationControl GetSheet(int hWnd) =>
            GetSheet((IntPtr)hWnd);
        #endregion

        #region Registry Helpers
        private void SaveEncryptedSetting(string key, string value)
        {
            byte[] enc = ProtectedData.Protect(
                Encoding.UTF8.GetBytes(value), null,
                DataProtectionScope.CurrentUser);
            using (RegistryKey rk = Registry.CurrentUser.CreateSubKey(REG_KEY))
                rk.SetValue(key, Convert.ToBase64String(enc), RegistryValueKind.String);
        }

        private string LoadEncryptedSetting(string key)
        {
            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(REG_KEY))
            {
                if (rk == null) return "";
                string b64 = rk.GetValue(key, "") as string;
                if (string.IsNullOrEmpty(b64)) return "";
                try
                {
                    byte[] dec = ProtectedData.Unprotect(
                        Convert.FromBase64String(b64), null,
                        DataProtectionScope.CurrentUser);
                    return Encoding.UTF8.GetString(dec);
                }
                catch { return ""; }
            }
        }

        private void SavePlainSetting(string key, string value)
        {
            using (RegistryKey rk = Registry.CurrentUser.CreateSubKey(REG_KEY))
                rk.SetValue(key, value ?? "", RegistryValueKind.String);
        }

        private string LoadPlainSetting(string key)
        {
            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(REG_KEY))
            {
                if (rk == null) return "";
                return rk.GetValue(key, "") as string ?? "";
            }
        }
        #endregion

        #region Form Load
        private void ConfigurationControl_Load(object sender, EventArgs e)
        {
            try
            {
                textBoxSdkUrl.Text = LoadPlainSetting(TA_SDK_URL);
                textBoxUserId.Text = LoadEncryptedSetting(TA_USER_ID);
                textBoxPassword.Text = LoadEncryptedSetting(TA_PASSWORD);

                string authTypeStr = LoadPlainSetting(TA_AUTH_TYPE);
                int authType = string.IsNullOrEmpty(authTypeStr)
                    ? AUTH_STANDARD : int.Parse(authTypeStr);

                if (authType == AUTH_WINDOWS) radioButtonWindowsAD.Checked = true;
                else if (authType == AUTH_FEDERATED) radioButtonFederated.Checked = true;
                else radioButtonStandard.Checked = true;

                string confFlag = LoadPlainSetting(TA_SHOW_CONFIRMATION);
                checkBoxShowConfirmation.Checked = confFlag != "0";

                ShowAuthFields(authType);
                AutoConnect(authType);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load settings: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Auth Type — Show/Hide Fields
        private void ShowAuthFields(int authType)
        {
            labelUserId.Visible = authType == AUTH_STANDARD;
            textBoxUserId.Visible = authType == AUTH_STANDARD;
            labelPassword.Visible = authType == AUTH_STANDARD;
            textBoxPassword.Visible = authType == AUTH_STANDARD;

            labelWindowsUser.Visible = authType == AUTH_WINDOWS;
            textBoxWindowsUser.Visible = authType == AUTH_WINDOWS;

            if (authType == AUTH_WINDOWS)
                textBoxWindowsUser.Text =
                    Environment.UserDomainName + "\\" + Environment.UserName;
        }

        private void radioButtonStandard_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonStandard.Checked) ShowAuthFields(AUTH_STANDARD);
        }

        private void radioButtonWindowsAD_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonWindowsAD.Checked) ShowAuthFields(AUTH_WINDOWS);
        }

        private void radioButtonFederated_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonFederated.Checked) ShowAuthFields(AUTH_FEDERATED);
        }
        #endregion

        #region Auto Connect
        private void AutoConnect(int authType)
        {
            string sdkUrl = LoadPlainSetting(TA_SDK_URL);
            if (string.IsNullOrEmpty(sdkUrl)) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                _taService = new TotalAgilityService(sdkUrl);

                if (authType == AUTH_STANDARD)
                {
                    string userId = LoadEncryptedSetting(TA_USER_ID);
                    string password = LoadEncryptedSetting(TA_PASSWORD);
                    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(password))
                        return;
                    TASession session = _taService.LogOn(userId, password);
                    _sessionId = session.SessionId;
                    SavePlainSetting(TA_SESSION_ID, _sessionId);
                }
                else if (authType == AUTH_WINDOWS)
                {
                    TASession session = _taService.LogOnWithWindowsAD(Environment.UserName);
                    _sessionId = session.SessionId;
                    SavePlainSetting(TA_SESSION_ID, _sessionId);
                }
                // Federated — do not auto-connect, requires browser interaction
            }
            catch { /* silent — user can click Connect manually */ }
            finally { Cursor = Cursors.Default; }
        }
        #endregion

        #region Connect Button — async to keep UI responsive
        private async void buttonConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSdkUrl.Text))
            {
                ShowFieldError(textBoxSdkUrl, "Please enter the TA SDK URL.");
                return;
            }

            int authType = radioButtonWindowsAD.Checked ? AUTH_WINDOWS
                         : radioButtonFederated.Checked ? AUTH_FEDERATED
                         : AUTH_STANDARD;

            if (authType == AUTH_STANDARD)
            {
                if (string.IsNullOrWhiteSpace(textBoxUserId.Text))
                { ShowFieldError(textBoxUserId, "Please enter the User ID."); return; }
                if (string.IsNullOrWhiteSpace(textBoxPassword.Text))
                { ShowFieldError(textBoxPassword, "Please enter the Password."); return; }
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                buttonConnect.Enabled = false;
                ClearFieldError(textBoxSdkUrl);

                string sdkUrl = textBoxSdkUrl.Text.Trim();
                _taService = new TotalAgilityService(sdkUrl);
                TASession session = null;

                if (authType == AUTH_STANDARD)
                {
                    string userId = textBoxUserId.Text.Trim();
                    string password = textBoxPassword.Text.Trim();
                    ClearFieldError(textBoxUserId);
                    ClearFieldError(textBoxPassword);

                    // Run on background thread — keeps UI responsive
                    session = await Task.Run(() => _taService.LogOn(userId, password));
                }
                else if (authType == AUTH_WINDOWS)
                {
                    string winUser = Environment.UserName;
                    session = await Task.Run(() => _taService.LogOnWithWindowsAD(winUser));
                }
                else if (authType == AUTH_FEDERATED)
                {
                    string taBaseUrl = _taService.GetTaBaseUrl(sdkUrl);
                    IntPtr handle = this.Handle;

                    // Use TaskCompletionSource so STA thread runs WebView2 browser
                    // while UI thread stays responsive via await
                    var tcs = new TaskCompletionSource<TASession>();

                    Thread fedThread = new Thread(() =>
                    {
                        try
                        {
                            TASession result = _taService.LogOnWithFederated(
                                taBaseUrl, new WindowWrapper(handle));
                            tcs.SetResult(result);
                        }
                        catch (Exception ex)
                        {
                            tcs.SetException(ex);
                        }
                    });
                    fedThread.SetApartmentState(ApartmentState.STA);
                    fedThread.Start();

                    // Await without blocking UI thread
                    session = await tcs.Task;
                }

                if (session == null || !session.IsValid)
                    throw new Exception("Authentication failed.");

                _sessionId = session.SessionId;
                SavePlainSetting(TA_SESSION_ID, _sessionId);

                MessageBox.Show(
                    $"Connected successfully!\nWelcome, {session.DisplayName}.",
                    "Connected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection failed:\n\n" + ex.Message,
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                buttonConnect.Enabled = true;
            }
        }
        #endregion

        #region Save Button
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSdkUrl.Text))
            {
                ShowFieldError(textBoxSdkUrl, "SDK URL is required.");
                return;
            }

            int authType = radioButtonWindowsAD.Checked ? AUTH_WINDOWS
                         : radioButtonFederated.Checked ? AUTH_FEDERATED
                         : AUTH_STANDARD;

            try
            {
                SavePlainSetting(TA_SDK_URL, textBoxSdkUrl.Text.Trim());
                SavePlainSetting(TA_AUTH_TYPE, authType.ToString());

                if (authType == AUTH_STANDARD)
                {
                    SaveEncryptedSetting(TA_USER_ID, textBoxUserId.Text.Trim());
                    SaveEncryptedSetting(TA_PASSWORD, textBoxPassword.Text.Trim());
                }
                else if (authType == AUTH_WINDOWS)
                {
                    SavePlainSetting(TA_WINDOWS_USERNAME, textBoxWindowsUser.Text);
                }
                // Federated — SessionId already saved by Connect

                SavePlainSetting(TA_SHOW_CONFIRMATION,
                    checkBoxShowConfirmation.Checked ? "1" : "0");

                MessageBox.Show("Settings saved successfully.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save settings:\n\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Field Error Helpers
        private void ShowFieldError(TextBox txt, string message)
        {
            txt.BackColor = Color.FromArgb(255, 220, 220);
            MessageBox.Show(message, "Validation",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txt.Focus();
        }

        private void ClearFieldError(Control ctrl)
        {
            ctrl.BackColor = SystemColors.Window;
            _toolTip.SetToolTip(ctrl, "");
        }
        #endregion
    }
}