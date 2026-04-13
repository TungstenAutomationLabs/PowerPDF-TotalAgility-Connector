using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace PPDF.TotalAgility.Connector
{
    /// <summary>
    /// Standalone WinForms test harness for TotalAgilityService.
    /// Walks through the exact runtime flow used by the connector:
    ///
    ///   Step 1 — LogOn
    ///   Step 2 — GetProcessesSummary  (populate process list)
    ///   Step 3 — GetProcessInitialization2  (select process → show variables)
    ///   Step 4 — Browse PDF + CreateJobWithDocuments  (single call, Base64 inline)
    ///
    /// Use this to verify each API call independently before testing
    /// the full connector inside Power PDF.
    /// </summary>
    public partial class TAServicesTester : Form
    {
        private TotalAgilityService _taService;
        private string _sessionId;
        private List<TAProcessIdentity> _processes;
        private List<TAInputVariable> _variables;

        public TAServicesTester()
        {
            InitializeComponent();
        }

        // ── STEP 1: LogOn ─────────────────────────────────────────────────

        private void btnLogOn_Click(object sender, EventArgs e)
        {
            try
            {
                Log("--- Step 1: LogOn ---");
                _taService = new TotalAgilityService(txtSdkUrl.Text.Trim());

                TASession session = _taService.LogOn(
                    txtUserId.Text.Trim(),
                    txtPassword.Text.Trim());

                _sessionId = session.SessionId;

                Log($"✅ LogOn SUCCESS");
                Log($"   SessionId:   {session.SessionId}");
                Log($"   DisplayName: {session.DisplayName}");
                Log($"   IsValid:     {session.IsValid}");

                btnGetProcesses.Enabled = true;
            }
            catch (Exception ex)
            {
                Log($"❌ LogOn FAILED: {ex.Message}");
            }
        }

        // ── STEP 2: GetProcesses ──────────────────────────────────────────

        private void btnGetProcesses_Click(object sender, EventArgs e)
        {
            try
            {
                Log("\n--- Step 2: GetProcessesSummary ---");
                _processes = _taService.GetProcesses(_sessionId);

                listBoxProcesses.Items.Clear();
                foreach (var p in _processes)
                {
                    listBoxProcesses.Items.Add(p);
                    Log($"   Process: {p.Name} | Id: {p.Id}");
                }

                Log($"✅ GetProcesses SUCCESS — {_processes.Count} process(es) found");
            }
            catch (Exception ex)
            {
                Log($"❌ GetProcesses FAILED: {ex.Message}");
            }
        }

        // ── STEP 3: GetProcessInitializationVariables ─────────────────────

        private void listBoxProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxProcesses.SelectedItem == null) return;

            try
            {
                TAProcessIdentity selected =
                    (TAProcessIdentity)listBoxProcesses.SelectedItem;

                Log($"\n--- Step 3: GetProcessInitialization2 for '{selected.Name}' ---");

                _variables = _taService.GetProcessInitializationVariables(
                    _sessionId, selected);

                listBoxVariables.Items.Clear();
                foreach (var v in _variables)
                {
                    string entry = $"[Type:{v.VariableType}] {v.DisplayName ?? v.Id}";
                    listBoxVariables.Items.Add(entry);
                    Log($"   Variable: {v.Id} | Type: {v.VariableType} " +
                        $"| DisplayName: {v.DisplayName} | Value: {v.Value}");
                }

                Log($"✅ GetProcessInitialization SUCCESS — {_variables.Count} variable(s)");

                // Enable PDF browse and job creation once a process is selected
                btnBrowsePdf.Enabled = true;
                btnCreateJobWithDocs.Enabled = false; // re-enabled after PDF selected
            }
            catch (Exception ex)
            {
                Log($"❌ GetProcessInitialization FAILED: {ex.Message}");
            }
        }

        // ── STEP 4a: Browse PDF ───────────────────────────────────────────

        private void btnBrowsePdf_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "PDF files (*.pdf)|*.pdf";
                dlg.Title = "Select a PDF to send to TotalAgility";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtPdfPath.Text = dlg.FileName;
                    btnCreateJobWithDocs.Enabled = true;
                    Log($"\n   PDF selected: {dlg.FileName}");
                    Log($"   File size: {new FileInfo(dlg.FileName).Length / 1024} KB");
                }
            }
        }

        // ── STEP 4b: CreateJobWithDocuments ───────────────────────────────

        private void btnCreateJobWithDocs_Click(object sender, EventArgs e)
        {
            if (listBoxProcesses.SelectedItem == null)
            {
                MessageBox.Show("Please select a process first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPdfPath.Text) ||
                !File.Exists(txtPdfPath.Text))
            {
                MessageBox.Show("Please select a valid PDF file first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Log("\n--- Step 4: CreateJobWithDocuments ---");

                TAProcessIdentity selectedProcess =
                    (TAProcessIdentity)listBoxProcesses.SelectedItem;

                // Read PDF bytes — same as runtime connector flow
                byte[] pdfBytes = File.ReadAllBytes(txtPdfPath.Text);
                Log($"   PDF loaded: {pdfBytes.Length} bytes " +
                    $"({pdfBytes.Length / 1024} KB)");

                // Build input variables list from what GetProcessInitialization2 returned.
                // Document variables (type 32760) are skipped — PDF goes in via
                // RuntimeDocumentCollection automatically.
                // All other variables use their default values from TA for this test.
                var inputVariables = new List<TAInputVariable>();
                foreach (var v in _variables)
                {
                    if (v.VariableType == TotalAgilityService.VARIABLE_TYPE_DOCUMENT)
                    {
                        Log($"   Skipping document variable '{v.Id}' " +
                            $"— PDF sent via RuntimeDocumentCollection");
                        continue;
                    }

                    if (v.VariableType == TotalAgilityService.VARIABLE_TYPE_FOLDER)
                    {
                        Log($"   Skipping folder variable '{v.Id}' — not required");
                        continue;
                    }

                    inputVariables.Add(new TAInputVariable
                    {
                        Id = v.Id,
                        VariableType = v.VariableType,
                        Value = v.Value ?? ""
                    });
                    Log($"   Variable '{v.DisplayName ?? v.Id}' → '{v.Value ?? ""}'");
                }

                Log($"   Calling CreateJobWithDocuments on process '{selectedProcess.Name}'...");

                TAJobResult result = _taService.CreateJobWithDocuments(
                    _sessionId,
                    selectedProcess,
                    pdfBytes,
                    inputVariables);

                txtJobId.Text = result.JobId;

                Log($"✅ CreateJobWithDocuments SUCCESS");
                Log($"   Job ID:      {result.JobId}");
                Log($"   Process:     {result.ProcessName}");
                Log($"   Created At:  {result.CreatedAt:yyyy-MM-dd HH:mm:ss}");
                Log($"\n🎉 Full flow completed successfully!");
            }
            catch (Exception ex)
            {
                Log($"❌ CreateJobWithDocuments FAILED: {ex.Message}");
            }
        }

        // ── Base64 Clipboard Helper ───────────────────────────────────────

        private void btnGetBase64_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPdfPath.Text) ||
                !File.Exists(txtPdfPath.Text))
            {
                MessageBox.Show("Please select a PDF first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] pdfBytes = File.ReadAllBytes(txtPdfPath.Text);
            string base64 = Convert.ToBase64String(pdfBytes);
            Clipboard.SetText(base64);
            Log($"\n   Base64 copied to clipboard ({base64.Length} chars)");
            MessageBox.Show("Base64 string copied to clipboard.",
                "Copied", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ── Clear Log ─────────────────────────────────────────────────────

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }

        // ── Log Helper ────────────────────────────────────────────────────

        private void Log(string message)
        {
            txtLog.AppendText(message + Environment.NewLine);
            txtLog.ScrollToCaret();
        }
    }
}