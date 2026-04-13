using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace PPDF.TotalAgility.Connector
{
    /// <summary>
    /// Configuration control hosted inside Power PDF's
    /// File → Options → Connectors → Send to TotalAgility panel.
    ///
    /// Layout (top to bottom):
    ///   [TotalAgility Credentials]  SDK URL / User ID / Password / Connect button
    ///   [Process Configuration]     Process dropdown (populated after Connect)
    ///   [Initialization Variables]  Dynamic controls rendered per process variable
    ///   [Save button]
    ///
    /// Flow:
    ///   1. User enters credentials and clicks Connect
    ///   2. LogOn → GetProcessesSummary → populate Process dropdown
    ///   3. User selects a process → GetProcessInitialization2 → render variables
    ///      Document variables (type 32760) are hidden — PDF goes in automatically
    ///   4. User fills variable values — mandatory fields highlighted in red if empty
    ///   5. Save validates all fields, serialises variables to JSON, writes registry
    /// </summary>
    public partial class ConfigurationControl : UserControl
    {
        #region Registry Constants
        private const string REG_KEY = @"Software\ScanSoft\Connectors\TotalAgility";
        private const string TA_SDK_URL = "TA_SDK_URL";
        private const string TA_USER_ID = "TA_USER_ID";
        private const string TA_PASSWORD = "TA_PASSWORD";
        private const string TA_SESSION_ID = "TA_SESSION_ID";
        private const string TA_PROCESS_ID = "TA_PROCESS_ID";
        private const string TA_PROCESS_NAME = "TA_PROCESS_NAME";
        private const string TA_INIT_VARIABLES = "TA_INIT_VARIABLES";
        #endregion

        #region State
        private Connector m_conn;
        private TotalAgilityService _taService;
        private string _sessionId;
        private List<TAInputVariable> _allVariables = new List<TAInputVariable>();
        private Dictionary<string, Control> _varControls = new Dictionary<string, Control>();
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

        #region Window Style — required for Power PDF property page embedding
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

        #region Sheet Management — required for Power PDF property page lookup
        protected override void OnHandleDestroyed(EventArgs e)
        {
            ClearSheet();
            base.OnHandleDestroyed(e);
        }

        protected static Dictionary<IntPtr, ConfigurationControl> sheetList =
            new Dictionary<IntPtr, ConfigurationControl>();

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
                rk.SetValue(key, Convert.ToBase64String(enc),
                    RegistryValueKind.String);
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

                // Federated Security — placeholder tooltip
                _toolTip.SetToolTip(checkBoxFederatedSecurity,
                    "Federated Security support is planned for a future release.\n" +
                    "Currently please use Standard Authentication (User ID + Password).");

                // Process and variable sections disabled until Connect succeeds
                SetProcessSectionEnabled(false);
                SetVariableSectionEnabled(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to load connector settings: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Federated Security Checkbox — placeholder only
        private void checkBoxFederatedSecurity_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFederatedSecurity.Checked)
            {
                MessageBox.Show(
                    "Federated Security support is planned for a future release.\n\n" +
                    "Please use Standard Authentication (User ID + Password) for now.",
                    "Federated Security — Coming Soon",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                checkBoxFederatedSecurity.Checked = false;
            }
        }
        #endregion

        #region Step 1 — Connect Button
        /// <summary>
        /// Validates credentials, calls LogOnWithPassword2, then
        /// calls GetProcessesSummary to populate the Process dropdown.
        /// Saves the SessionId to registry for reuse during config.
        /// </summary>
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSdkUrl.Text))
            {
                ShowFieldError(textBoxSdkUrl, "Please enter the TA SDK URL.");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBoxUserId.Text))
            {
                ShowFieldError(textBoxUserId, "Please enter the User ID.");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                ShowFieldError(textBoxPassword, "Please enter the Password.");
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                buttonConnect.Enabled = false;

                ClearFieldError(textBoxSdkUrl);
                ClearFieldError(textBoxUserId);
                ClearFieldError(textBoxPassword);

                _taService = new TotalAgilityService(textBoxSdkUrl.Text.Trim());
                TASession session = _taService.LogOn(
                    textBoxUserId.Text.Trim(),
                    textBoxPassword.Text.Trim());

                _sessionId = session.SessionId;

                // Persist session ID so it survives tab switches within config
                SavePlainSetting(TA_SESSION_ID, _sessionId);

                // Populate process dropdown
                List<TAProcessIdentity> processes = _taService.GetProcesses(_sessionId);

                comboBoxProcess.Items.Clear();
                foreach (var p in processes)
                    comboBoxProcess.Items.Add(p);

                SetProcessSectionEnabled(true);
                SetVariableSectionEnabled(false);

                // Restore previously saved process selection if still in the list
                string savedProcessId = LoadPlainSetting(TA_PROCESS_ID);
                if (!string.IsNullOrEmpty(savedProcessId))
                {
                    foreach (TAProcessIdentity item in comboBoxProcess.Items)
                    {
                        if (item.Id == savedProcessId)
                        {
                            comboBoxProcess.SelectedItem = item;
                            break;
                        }
                    }
                }

                MessageBox.Show(
                    $"Connected successfully!\n" +
                    $"Welcome, {session.DisplayName}.\n" +
                    $"{processes.Count} process(es) loaded.",
                    "Connected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Connection failed:\n\n" + ex.Message,
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetProcessSectionEnabled(false);
                SetVariableSectionEnabled(false);
            }
            finally
            {
                Cursor = Cursors.Default;
                buttonConnect.Enabled = true;
            }
        }
        #endregion

        #region Step 2 — Process Selection
        /// <summary>
        /// Called when user selects a process from the dropdown.
        /// Calls GetProcessInitialization2 and renders variable controls immediately.
        /// Document variables (type 32760) are never rendered.
        /// </summary>
        private void comboBoxProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxProcess.SelectedItem == null) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                TAProcessIdentity selected = (TAProcessIdentity)comboBoxProcess.SelectedItem;

                _allVariables = _taService.GetProcessInitializationVariables(
                    _sessionId, selected);

                RenderVariableControls(_allVariables);
                SetVariableSectionEnabled(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to load process variables:\n\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetVariableSectionEnabled(false);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        #endregion

        #region Step 3 — Render Variable Controls
        /// <summary>
        /// Dynamically builds a label + input control for each non-document variable.
        /// Document variables (VariableType 32760) are skipped entirely.
        /// All fields are marked as mandatory — red asterisk in the label.
        /// Previously saved values are restored from registry.
        /// </summary>
        private void RenderVariableControls(List<TAInputVariable> variables)
        {
            panelVariables.Controls.Clear();
            _varControls.Clear();

            // Restore previously saved values
            var savedValues = new Dictionary<string, string>();
            string savedJson = LoadPlainSetting(TA_INIT_VARIABLES);
            if (!string.IsNullOrEmpty(savedJson))
            {
                try
                {
                    var saved = JsonConvert.DeserializeObject<List<TAInputVariable>>(savedJson);
                    if (saved != null)
                        foreach (var sv in saved)
                            savedValues[sv.Id] = sv.Value ?? "";
                }
                catch { }
            }

            const int xLabel = 8;
            const int xControl = 200;
            const int labelWidth = 188;
            const int controlWidth = 570;
            const int rowHeight = 36;
            const int multiHeight = 70;
            int yPos = 10;

            foreach (var variable in variables)
            {
                // Skip document variables — PDF sent via RuntimeDocumentCollection
                if (variable.VariableType == TotalAgilityService.VARIABLE_TYPE_DOCUMENT)
                    continue;

                // Skip folder variables — not required for job creation
                if (variable.VariableType == TotalAgilityService.VARIABLE_TYPE_FOLDER)
                    continue;

                string displayName = variable.DisplayName ?? variable.Id;
                string savedValue = savedValues.ContainsKey(variable.Id)
                    ? savedValues[variable.Id]
                    : "";

                // Label with red mandatory asterisk
                Label lbl = new Label
                {
                    AutoSize = false,
                    Width = labelWidth,
                    Height = 22,
                    Location = new Point(xLabel, yPos + 5),
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                };

                // Set label text with mandatory marker using RichLabel workaround:
                // We append " *" and colour it red via ForeColor on a separate label
                lbl.Text = displayName + ":";

                Label lblAsterisk = new Label
                {
                    AutoSize = true,
                    Text = " *",
                    ForeColor = Color.Red,
                    Font = new System.Drawing.Font(
                        lbl.Font, System.Drawing.FontStyle.Bold),
                    Location = new Point(
                        xLabel + lbl.PreferredWidth, yPos + 5)
                };

                Control ctrl = null;
                int rowH = rowHeight;

                switch (variable.VariableType)
                {
                    case TotalAgilityService.VARIABLE_TYPE_BOOLEAN:
                        {
                            var chk = new CheckBox
                            {
                                Location = new Point(xControl, yPos + 4),
                                Width = controlWidth,
                                Checked = savedValue?.ToLower() == "true"
                            };
                            // Boolean always has a value (true/false) — no asterisk needed
                            lblAsterisk.Visible = false;
                            ctrl = chk;
                            break;
                        }

                    case TotalAgilityService.VARIABLE_TYPE_DATE:
                        {
                            lbl.Text = displayName + " (yyyy-MM-dd):";
                            var txt = new TextBox
                            {
                                Location = new Point(xControl, yPos + 3),
                                Width = controlWidth,
                                Text = savedValue
                            };
                            _toolTip.SetToolTip(txt, "Format: yyyy-MM-dd  e.g. 2026-04-15");
                            ctrl = txt;
                            break;
                        }

                    case TotalAgilityService.VARIABLE_TYPE_DATETIME:
                        {
                            lbl.Text = displayName + " (yyyy-MM-dd HH:mm:ss):";
                            var txt = new TextBox
                            {
                                Location = new Point(xControl, yPos + 3),
                                Width = controlWidth,
                                Text = savedValue
                            };
                            _toolTip.SetToolTip(txt,
                                "Format: yyyy-MM-dd HH:mm:ss  e.g. 2026-04-15 10:30:00");
                            ctrl = txt;
                            break;
                        }

                    case TotalAgilityService.VARIABLE_TYPE_INTEGER:
                        {
                            lbl.Text = displayName + " (integer):";
                            var txt = new TextBox
                            {
                                Location = new Point(xControl, yPos + 3),
                                Width = controlWidth,
                                Text = savedValue
                            };
                            _toolTip.SetToolTip(txt, "Enter a whole number  e.g. 42");
                            ctrl = txt;
                            break;
                        }

                    case TotalAgilityService.VARIABLE_TYPE_DECIMAL:
                        {
                            lbl.Text = displayName + " (decimal):";
                            var txt = new TextBox
                            {
                                Location = new Point(xControl, yPos + 3),
                                Width = controlWidth,
                                Text = savedValue
                            };
                            _toolTip.SetToolTip(txt, "Enter a decimal number  e.g. 3.14");
                            ctrl = txt;
                            break;
                        }

                    case TotalAgilityService.VARIABLE_TYPE_DATAOBJECT:
                        {
                            lbl.Text = displayName + " (JSON):";
                            lbl.Height = multiHeight;
                            lbl.TextAlign =
                                System.Drawing.ContentAlignment.TopLeft;
                            var txt = new TextBox
                            {
                                Location = new Point(xControl, yPos + 3),
                                Width = controlWidth,
                                Height = multiHeight,
                                Multiline = true,
                                ScrollBars = ScrollBars.Vertical,
                                Text = savedValue
                            };
                            _toolTip.SetToolTip(txt,
                                "Enter valid JSON for this DataObject variable.");
                            rowH = multiHeight + 8;
                            ctrl = txt;
                            break;
                        }

                    default: // String (8) and any other types
                        {
                            var txt = new TextBox
                            {
                                Location = new Point(xControl, yPos + 3),
                                Width = controlWidth,
                                Text = savedValue
                            };
                            ctrl = txt;
                            break;
                        }
                }

                if (ctrl != null)
                {
                    panelVariables.Controls.Add(lbl);
                    panelVariables.Controls.Add(lblAsterisk);
                    panelVariables.Controls.Add(ctrl);
                    _varControls[variable.Id] = ctrl;
                    yPos += rowH;
                }
            }

            panelVariables.AutoScrollMinSize =
                new System.Drawing.Size(0, yPos + 10);
        }
        #endregion

        #region Step 4 — Save Button
        /// <summary>
        /// Validates all mandatory fields, type-checks values, and writes to registry.
        /// Highlights fields with errors in red — does not save if any error exists.
        /// DataObject fields must contain valid JSON — error shown and save blocked if not.
        /// </summary>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Validate credentials section
            if (string.IsNullOrWhiteSpace(textBoxSdkUrl.Text))
            {
                ShowFieldError(textBoxSdkUrl, "SDK URL is required.");
                return;
            }

            if (comboBoxProcess.SelectedItem == null)
            {
                MessageBox.Show(
                    "Please connect and select a process before saving.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate all variable controls
            bool hasErrors = false;
            foreach (var variable in _allVariables)
            {
                if (variable.VariableType == TotalAgilityService.VARIABLE_TYPE_DOCUMENT)
                    continue;

                if (variable.VariableType == TotalAgilityService.VARIABLE_TYPE_FOLDER)
                    continue;

                if (!_varControls.TryGetValue(variable.Id, out Control ctrl))
                    continue;

                // Boolean (CheckBox) is always valid — skip
                if (ctrl is CheckBox) continue;

                TextBox txt = ctrl as TextBox;
                if (txt == null) continue;

                string val = txt.Text.Trim();
                string displayName = variable.DisplayName ?? variable.Id;

                // Mandatory check — all non-boolean fields must be filled
                if (string.IsNullOrEmpty(val))
                {
                    MarkFieldError(txt,
                        $"'{displayName}' is required.");
                    hasErrors = true;
                    continue;
                }

                // Type-specific validation
                switch (variable.VariableType)
                {
                    case TotalAgilityService.VARIABLE_TYPE_INTEGER:
                        if (!int.TryParse(val, out _))
                        {
                            MarkFieldError(txt,
                                $"'{displayName}' must be a whole number  e.g. 42");
                            hasErrors = true;
                        }
                        else ClearFieldError(txt);
                        break;

                    case TotalAgilityService.VARIABLE_TYPE_DECIMAL:
                        if (!decimal.TryParse(val,
                            System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture,
                            out _))
                        {
                            MarkFieldError(txt,
                                $"'{displayName}' must be a decimal number  e.g. 3.14");
                            hasErrors = true;
                        }
                        else ClearFieldError(txt);
                        break;

                    case TotalAgilityService.VARIABLE_TYPE_DATE:
                        if (!DateTime.TryParseExact(val, "yyyy-MM-dd",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None, out _))
                        {
                            MarkFieldError(txt,
                                $"'{displayName}' must be a date in format yyyy-MM-dd  e.g. 2026-04-15");
                            hasErrors = true;
                        }
                        else ClearFieldError(txt);
                        break;

                    case TotalAgilityService.VARIABLE_TYPE_DATETIME:
                        if (!DateTime.TryParseExact(val, "yyyy-MM-dd HH:mm:ss",
                            System.Globalization.CultureInfo.InvariantCulture,
                            System.Globalization.DateTimeStyles.None, out _))
                        {
                            MarkFieldError(txt,
                                $"'{displayName}' must be a date/time in format yyyy-MM-dd HH:mm:ss");
                            hasErrors = true;
                        }
                        else ClearFieldError(txt);
                        break;

                    case TotalAgilityService.VARIABLE_TYPE_DATAOBJECT:
                        // Must be valid JSON — block save if not
                        try
                        {
                            Newtonsoft.Json.Linq.JToken.Parse(val);
                            ClearFieldError(txt);
                        }
                        catch
                        {
                            MarkFieldError(txt,
                                $"'{displayName}' must contain valid JSON.");
                            hasErrors = true;
                        }
                        break;

                    default:
                        ClearFieldError(txt);
                        break;
                }
            }

            if (hasErrors)
            {
                MessageBox.Show(
                    "Please correct the highlighted fields before saving.",
                    "Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // All valid — write to registry
            try
            {
                SavePlainSetting(TA_SDK_URL, textBoxSdkUrl.Text.Trim());
                SaveEncryptedSetting(TA_USER_ID, textBoxUserId.Text.Trim());
                SaveEncryptedSetting(TA_PASSWORD, textBoxPassword.Text.Trim());

                var selectedProcess = (TAProcessIdentity)comboBoxProcess.SelectedItem;
                SavePlainSetting(TA_PROCESS_ID, selectedProcess.Id);
                SavePlainSetting(TA_PROCESS_NAME, selectedProcess.Name);

                // Serialise variable values to JSON array
                var variablesToSave = new List<TAInputVariable>();
                foreach (var variable in _allVariables)
                {
                    if (variable.VariableType == TotalAgilityService.VARIABLE_TYPE_DOCUMENT)
                        continue;

                    // Skip folder variables — not saved or sent to TA
                    if (variable.VariableType == TotalAgilityService.VARIABLE_TYPE_FOLDER)
                        continue;

                    string value = "";
                    if (_varControls.TryGetValue(variable.Id, out Control ctrl))
                    {
                        value = ctrl is CheckBox chk
                            ? chk.Checked.ToString().ToLower()
                            : ((TextBox)ctrl).Text.Trim();
                    }

                    variablesToSave.Add(new TAInputVariable
                    {
                        Id = variable.Id,
                        VariableType = variable.VariableType,
                        DisplayName = variable.DisplayName,
                        Value = value
                    });
                }

                SavePlainSetting(TA_INIT_VARIABLES,
                    JsonConvert.SerializeObject(variablesToSave));

                MessageBox.Show(
                    "Settings saved successfully.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Failed to save settings:\n\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Field Error Highlighting Helpers
        /// <summary>
        /// Highlights a TextBox with a red border and shows a tooltip with the reason.
        /// </summary>
        private void MarkFieldError(TextBox txt, string reason)
        {
            txt.BackColor = Color.FromArgb(255, 220, 220);
            _toolTip.SetToolTip(txt, reason);
        }

        /// <summary>
        /// Highlights a TextBox, shows a MessageBox, and returns focus to the field.
        /// Used for credential fields which are validated one at a time.
        /// </summary>
        private void ShowFieldError(TextBox txt, string message)
        {
            txt.BackColor = Color.FromArgb(255, 220, 220);
            MessageBox.Show(message, "Validation",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txt.Focus();
        }

        /// <summary>
        /// Clears the error highlight from a control.
        /// </summary>
        private void ClearFieldError(Control ctrl)
        {
            ctrl.BackColor = SystemColors.Window;
            _toolTip.SetToolTip(ctrl, "");
        }
        #endregion

        #region UI Section Helpers
        private void SetProcessSectionEnabled(bool enabled)
        {
            labelProcess.Enabled = enabled;
            comboBoxProcess.Enabled = enabled;
        }

        private void SetVariableSectionEnabled(bool enabled)
        {
            panelVariables.Enabled = enabled;
            buttonSave.Enabled = enabled;
        }
        #endregion
    }
}