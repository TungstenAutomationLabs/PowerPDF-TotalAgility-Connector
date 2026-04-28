using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPDF.TotalAgility.Connector
{
    public partial class ProcessSelectionForm : Form
    {
        private const string REG_KEY = @"Software\ScanSoft\Connectors\TotalAgility";
        private const string TA_SDK_URL = "TA_SDK_URL";
        private const string TA_USER_ID = "TA_USER_ID";
        private const string TA_PASSWORD = "TA_PASSWORD";
        private const string TA_SESSION_ID = "TA_SESSION_ID";
        private const string TA_PROCESS_ID = "TA_PROCESS_ID";
        private const string TA_PROCESS_NAME = "TA_PROCESS_NAME";
        private const string TA_INIT_VARIABLES = "TA_INIT_VARIABLES";
        private const string TA_AUTH_TYPE = "TA_AUTH_TYPE";
        private const string TA_WINDOWS_USERNAME = "TA_WINDOWS_USERNAME";

        private const int AUTH_STANDARD = 0;
        private const int AUTH_WINDOWS = 1;
        private const int AUTH_FEDERATED = 2;

        private const string DEBUG_LOG = @"C:\Temp\TAConnectorDebug.txt";

        private TotalAgilityService _taService;
        private string _sessionId;
        private List<TAProcessIdentity> _allProcesses = new List<TAProcessIdentity>();
        private List<TAInputVariable> _allVariables = new List<TAInputVariable>();
        private Dictionary<string, Control> _varControls = new Dictionary<string, Control>();
        private ToolTip _toolTip = new ToolTip();

        public TAProcessIdentity SavedProcess { get; private set; }
        public List<TAInputVariable> SavedVariables { get; private set; }

        public ProcessSelectionForm()
        {
            InitializeComponent();
        }

        private void Log(string msg)
        {
            try
            {
                Directory.CreateDirectory(@"C:\Temp");
                File.AppendAllText(DEBUG_LOG,
                    DateTime.Now + " | ProcessSelectionForm | " + msg + "\r\n");
            }
            catch { }
        }

        // ── Load ──────────────────────────────────────────────────────
        private async void ProcessSelectionForm_Load(object sender, EventArgs e)
        {
            string sdkUrl = LoadPlainSetting(TA_SDK_URL);
            Log("Load — sdkUrl: " + sdkUrl);

            if (string.IsNullOrEmpty(sdkUrl)) return;

            try
            {
                Cursor = Cursors.WaitCursor;
                _taService = new TotalAgilityService(sdkUrl);

                // Step 1: Try saved SessionId first
                string savedSession = LoadPlainSetting(TA_SESSION_ID);
                Log("Saved SessionId: [" + savedSession + "]");

                if (!string.IsNullOrEmpty(savedSession))
                {
                    try
                    {
                        Log("Trying GetProcesses with saved SessionId...");
                        List<TAProcessIdentity> procs = await Task.Run(() =>
                            _taService.GetProcesses(savedSession));
                        _sessionId = savedSession;
                        Log("GetProcesses success — " + procs.Count + " processes");
                        PopulateProcessList(procs);
                        return;
                    }
                    catch (Exception ex)
                    {
                        Log("Saved SessionId failed: " + ex.Message);
                    }
                }

                // Step 2: Re-authenticate based on auth type
                string authTypeStr = LoadPlainSetting(TA_AUTH_TYPE);
                int authType = string.IsNullOrEmpty(authTypeStr)
                    ? AUTH_STANDARD : int.Parse(authTypeStr);
                Log("Auth type: " + authType);

                TASession session = null;

                if (authType == AUTH_STANDARD)
                {
                    string userId = LoadEncryptedSetting(TA_USER_ID);
                    string password = LoadEncryptedSetting(TA_PASSWORD);
                    Log("Standard LogOn — userId: " + userId);
                    if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(password))
                        session = await Task.Run(() => _taService.LogOn(userId, password));
                }
                else if (authType == AUTH_WINDOWS)
                {
                    string winUser = LoadPlainSetting(TA_WINDOWS_USERNAME);
                    if (string.IsNullOrEmpty(winUser)) winUser = Environment.UserName;
                    Log("Windows AD LogOn — user: " + winUser);
                    session = await Task.Run(() => _taService.LogOnWithWindowsAD(winUser));
                }
                else if (authType == AUTH_FEDERATED)
                {
                    Log("Federated — opening browser...");
                    string taBaseUrl = _taService.GetTaBaseUrl(sdkUrl);
                    IntPtr handle = this.Handle;
                    var tcs = new TaskCompletionSource<TASession>();

                    Thread fedThread = new Thread(() =>
                    {
                        try
                        {
                            TASession result = _taService.LogOnWithFederated(
                                taBaseUrl, new WindowWrapper(handle));
                            tcs.SetResult(result);
                        }
                        catch (Exception ex) { tcs.SetException(ex); }
                    });
                    fedThread.SetApartmentState(ApartmentState.STA);
                    fedThread.Start();

                    try
                    {
                        session = await tcs.Task;
                        Log("Federated session: " + (session?.SessionId ?? "NULL"));
                    }
                    catch (Exception ex)
                    {
                        Log("Federated failed: " + ex.Message);
                        MessageBox.Show("Federated login failed:\n\n" + ex.Message,
                            "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (session != null && !string.IsNullOrEmpty(session.SessionId))
                {
                    _sessionId = session.SessionId;
                    SavePlainSetting(TA_SESSION_ID, _sessionId);
                    Log("New SessionId saved. Fetching processes...");
                    List<TAProcessIdentity> procs = await Task.Run(() =>
                        _taService.GetProcesses(_sessionId));
                    Log("GetProcesses success — " + procs.Count + " processes");
                    PopulateProcessList(procs);
                }
                else
                {
                    Log("Session is null or empty after re-auth");
                }
            }
            catch (Exception ex)
            {
                Log("Unhandled error in Load: " + ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void PopulateProcessList(List<TAProcessIdentity> processes)
        {
            _allProcesses = processes ?? new List<TAProcessIdentity>();

            comboBoxProcess.BeginUpdate();
            comboBoxProcess.Items.Clear();
            comboBoxProcess.AutoCompleteCustomSource.Clear();

            foreach (var p in _allProcesses)
            {
                comboBoxProcess.Items.Add(p);
                if (!string.IsNullOrWhiteSpace(p.Name))
                    comboBoxProcess.AutoCompleteCustomSource.Add(p.Name);
            }

            comboBoxProcess.EndUpdate();

            string savedId = LoadPlainSetting(TA_PROCESS_ID);
            if (!string.IsNullOrEmpty(savedId))
            {
                foreach (TAProcessIdentity item in comboBoxProcess.Items)
                {
                    if (item.Id == savedId)
                    {
                        comboBoxProcess.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        // ── Process selection ─────────────────────────────────────────
        private void comboBoxProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            TAProcessIdentity selected = GetSelectedProcessFromCombo();
            if (selected == null || _taService == null) return;

            LoadVariablesForProcess(selected);
        }

        private void comboBoxProcess_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ResolveTypedProcessSelection();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void comboBoxProcess_Leave(object sender, EventArgs e)
        {
            ResolveTypedProcessSelection();
        }

        private void ResolveTypedProcessSelection()
        {
            if (_allProcesses == null || _allProcesses.Count == 0) return;

            string typedName = comboBoxProcess.Text.Trim();
            if (string.IsNullOrEmpty(typedName)) return;

            foreach (TAProcessIdentity process in _allProcesses)
            {
                if (string.Equals(process.Name, typedName, StringComparison.OrdinalIgnoreCase))
                {
                    if (!object.ReferenceEquals(comboBoxProcess.SelectedItem, process))
                        comboBoxProcess.SelectedItem = process;
                    else
                        LoadVariablesForProcess(process);
                    return;
                }
            }
        }

        private TAProcessIdentity GetSelectedProcessFromCombo()
        {
            if (comboBoxProcess.SelectedItem is TAProcessIdentity selectedProcess)
                return selectedProcess;

            string typedName = comboBoxProcess.Text.Trim();
            if (string.IsNullOrEmpty(typedName) || _allProcesses == null)
                return null;

            foreach (TAProcessIdentity process in _allProcesses)
            {
                if (string.Equals(process.Name, typedName, StringComparison.OrdinalIgnoreCase))
                    return process;
            }

            return null;
        }

        private void LoadVariablesForProcess(TAProcessIdentity selected)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                _allVariables = _taService.GetProcessInitializationVariables(
                    _sessionId, selected);

                RenderVariableControls(_allVariables);
                panelVariables.Enabled = true;
                btnSave.Enabled = true;
            }
            catch (Exception ex)
            {
                ShowError("Failed to load process variables:\n\n" + ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // ── Render variables ──────────────────────────────────────────
        private void RenderVariableControls(List<TAInputVariable> variables)
        {
            panelVariables.Controls.Clear();
            _varControls.Clear();

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

            const int xLabel = 8, xControl = 180, labelWidth = 168, controlWidth = 220;
            const int rowHeight = 32;
            int yPos = 8;

            foreach (var variable in variables)
            {
                if (variable.VariableType == TotalAgilityService.VARIABLE_TYPE_DOCUMENT) continue;
                if (variable.VariableType == TotalAgilityService.VARIABLE_TYPE_FOLDER) continue;

                string displayName = variable.DisplayName ?? variable.Id;
                string savedValue = savedValues.ContainsKey(variable.Id)
                    ? savedValues[variable.Id] : "";

                Label lbl = new Label
                {
                    AutoSize = false,
                    Width = labelWidth,
                    Height = 20,
                    Location = new Point(xLabel, yPos + 4),
                    Text = displayName + ":",
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft
                };

                Control ctrl = variable.VariableType == TotalAgilityService.VARIABLE_TYPE_BOOLEAN
                    ? (Control)new CheckBox
                    {
                        Location = new Point(xControl, yPos + 2),
                        Width = controlWidth,
                        Checked = savedValue?.ToLower() == "true"
                    }
                    : new TextBox
                    {
                        Location = new Point(xControl, yPos + 2),
                        Width = controlWidth,
                        Text = savedValue
                    };

                panelVariables.Controls.Add(lbl);
                panelVariables.Controls.Add(ctrl);
                _varControls[variable.Id] = ctrl;
                yPos += rowHeight;
            }

            panelVariables.AutoScrollMinSize = new System.Drawing.Size(0, yPos + 8);
        }

        // ── Save ──────────────────────────────────────────────────────
        private void btnSave_Click(object sender, EventArgs e)
        {
            TAProcessIdentity selectedProcess = GetSelectedProcessFromCombo();
            if (selectedProcess == null)
            {
                ShowError("Please select a valid process before saving.");
                return;
            }
            SavePlainSetting(TA_PROCESS_ID, selectedProcess.Id);
            SavePlainSetting(TA_PROCESS_NAME, selectedProcess.Name);

            var variablesToSave = new List<TAInputVariable>();
            foreach (var variable in _allVariables)
            {
                if (variable.VariableType == TotalAgilityService.VARIABLE_TYPE_DOCUMENT) continue;
                if (variable.VariableType == TotalAgilityService.VARIABLE_TYPE_FOLDER) continue;

                string value = "";
                if (_varControls.TryGetValue(variable.Id, out Control ctrl))
                    value = ctrl is CheckBox chk
                        ? chk.Checked.ToString().ToLower()
                        : ((TextBox)ctrl).Text.Trim();

                variablesToSave.Add(new TAInputVariable
                {
                    Id = variable.Id,
                    VariableType = variable.VariableType,
                    DisplayName = variable.DisplayName,
                    Value = value
                });
            }

            SavePlainSetting(TA_INIT_VARIABLES, JsonConvert.SerializeObject(variablesToSave));
            SavedProcess = selectedProcess;
            SavedVariables = variablesToSave;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        // ── Cancel ────────────────────────────────────────────────────
        private void btnCancelForm_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void ShowError(string msg) =>
            MessageBox.Show(msg, "TotalAgility", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private string LoadPlainSetting(string key)
        {
            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(REG_KEY))
            {
                if (rk == null) return "";
                return rk.GetValue(key, "") as string ?? "";
            }
        }

        private void SavePlainSetting(string key, string value)
        {
            using (RegistryKey rk = Registry.CurrentUser.CreateSubKey(REG_KEY))
                rk.SetValue(key, value ?? "", RegistryValueKind.String);
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
    }
}