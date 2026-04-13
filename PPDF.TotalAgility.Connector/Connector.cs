using DMSConnector;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PPDF.TotalAgility.Connector
{
    /// <summary>
    /// Error code constants.
    /// </summary>
    public abstract class ERRORS
    {
        public const int E_CANCELLED = -2147221492;
    }

    /// <summary>
    /// Power PDF DMS Connector for Kofax TotalAgility.
    ///
    /// Sends the currently open PDF to TotalAgility by creating a job
    /// on the configured process via CreateJobWithDocuments.
    ///
    /// Key design point (matching RAI connector exactly):
    ///   DocAddNew() is the main action entry point. Power PDF passes the
    ///   sourceFile path and title directly. The entire API flow runs here:
    ///     1. Load config from registry
    ///     2. Show LoadingForm on STA thread
    ///     3. LogOn → save SessionId → read PDF bytes → CreateJobWithDocuments
    ///     4. Close LoadingForm → show TAResultForm (success or error)
    ///
    ///   MenuAction() handles the ribbon button click but with
    ///   CALLBACK_MENUITEM the document pipeline calls DocAddNew.
    ///
    /// Registration:
    ///   regasm PPDF.TotalAgility.Connector.dll /codebase
    /// </summary>
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [Guid("A3F7C241-88B2-4D1E-9C3A-7E2B5F0D4C8A")]
    [ProgId("TotalAgility")]
    public class Connector : IDMSConnector, IPropertyPageHandler
    {
        // ── COM Registration ──────────────────────────────────────────────
        //
        // Power PDF is a 32-bit application. On 64-bit Windows, 32-bit apps
        // store registry entries under WOW6432Node. We must write to that
        // path explicitly using RegistryView.Registry32 so Power PDF can
        // discover the connector regardless of whether regasm runs as
        // 32-bit or 64-bit.

        [ComRegisterFunction]
        public static void RegisterAsConnector(Type t)
        {
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(
                RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                RegistryKey rkConnectors = OpenOrCreateSubkey(
                    OpenOrCreateSubkey(
                        OpenOrCreateSubkey(hklm, "Software"),
                        "ScanSoft"),
                    "Connectors");
                OpenOrCreateSubkey(rkConnectors, GetProgId(t));
            }
        }

        [ComUnregisterFunction]
        public static void UnregisterAsConnector(Type t)
        {
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(
                RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                hklm.DeleteSubKey(
                    @"Software\ScanSoft\Connectors\" + GetProgId(t), false);
            }
        }

        // ── Registry Constants ────────────────────────────────────────────
        //
        // User settings are stored under HKCU which has no WOW6432Node
        // distinction — HKCU is the same for 32-bit and 64-bit processes.

        private const string REG_KEY = @"Software\ScanSoft\Connectors\TotalAgility";
        private const string TA_SDK_URL = "TA_SDK_URL";
        private const string TA_USER_ID = "TA_USER_ID";
        private const string TA_PASSWORD = "TA_PASSWORD";
        private const string TA_SESSION_ID = "TA_SESSION_ID";
        private const string TA_PROCESS_ID = "TA_PROCESS_ID";
        private const string TA_PROCESS_NAME = "TA_PROCESS_NAME";
        private const string TA_INIT_VARIABLES = "TA_INIT_VARIABLES";

        // ── State ─────────────────────────────────────────────────────────

        protected string _connectorName = "TotalAgility";
        protected string _languageCode;
        protected bool _initialized = false;
        protected IntPtr _parentWindow = IntPtr.Zero;
        protected MenuItemList _menuItems;
        protected Dictionary<string, Document> _documents =
            new Dictionary<string, Document>();

        // ── Constructor ───────────────────────────────────────────────────

        public Connector() { }

        // ── IDMSConnector: Initialization ─────────────────────────────────

        /// <summary>
        /// Called once by Power PDF when the connector is loaded.
        /// </summary>
        void IDMSConnector.Init(object application, string LangCode)
        {
            if (!_initialized)
            {
                _languageCode = LangCode;

                try
                {
                    CultureInfo cultureInfo = Langs.Iso639_3ToCulture(_languageCode);
                    if (!cultureInfo.IsNeutralCulture)
                        Thread.CurrentThread.CurrentCulture = cultureInfo;
                    Thread.CurrentThread.CurrentUICulture = cultureInfo;
                }
                catch { /* non-fatal */ }

                _menuItems = MenuItemList.Create();
                _initialized = true;
            }
        }

        /// <summary>
        /// Called by Power PDF when the connector is unloaded.
        /// Closes all tracked documents and releases resources.
        /// </summary>
        void IDMSConnector.Shutdown()
        {
            if (_initialized)
            {
                foreach (Document doc in _documents.Values)
                    doc.Close();
                _documents.Clear();
                _initialized = false;
            }
        }

        // ── IDMSConnector: Properties ─────────────────────────────────────

        /// <summary>
        /// Returns the connector name shown in Power PDF's connector list.
        /// </summary>
        string IDMSConnector.ConnectorName
        {
            get { return _connectorName; }
        }

        /// <summary>
        /// Receives the parent window handle from Power PDF.
        /// Used to parent dialogs correctly.
        /// </summary>
        int IDMSConnector.ParentWindow
        {
            set { _parentWindow = (IntPtr)value; }
        }

        // ── IDMSConnector: Menu ───────────────────────────────────────────

        void IDMSConnector.MenuGetNumberOfItems(out int num, out string title)
        {
            num = _menuItems.Count;
            title = _connectorName;
        }

        void IDMSConnector.MenuGetMenuItem(int num,
            out int menuItemId, out string text, out string tooltip,
            out bool isPartOfToolbar, out CallbackType cbType,
            out int hIconBig, out int hIconSmall, out bool enabledWithoutDoc)
        {
            if (num < 0 || num >= _menuItems.Count)
                throw new ArgumentOutOfRangeException("num");

            MenuItem item = _menuItems[num];
            menuItemId = item.menuItemId;
            text = item.text;
            tooltip = item.tooltip;
            isPartOfToolbar = item.isPartOfToolbar;
            cbType = item.cbType;
            hIconBig = item.hIconBig.ToInt32();
            hIconSmall = item.hIconSmall.ToInt32();
            enabledWithoutDoc = item.enabledWithoutDoc;
        }

        bool IDMSConnector.MenuGetItemState(int menuItemId, string docId)
        {
            return true;
        }

        void IDMSConnector.MenuAction(int menuItemId, string docId)
        {
            // With CALLBACK_SAVE, Power PDF calls DocAddNew directly when the
            // ribbon button is clicked — MenuAction is not called for our button.
            // This method is implemented as a no-op to satisfy the interface.
        }

        // ── IDMSConnector: DocAddNew — Main Action ────────────────────────

        /// <summary>
        /// Called by Power PDF when the ribbon button is clicked.
        /// sourceFile is the full path to the currently open PDF.
        /// title is the document title shown in Power PDF's tab.
        ///
        /// This is where the entire TotalAgility submission flow runs:
        ///   1. Load config from registry
        ///   2. Show LoadingForm on its own STA thread
        ///   3. LogOn fresh → save SessionId to registry
        ///   4. Read PDF bytes → CreateJobWithDocuments (Base64 inline)
        ///   5. Close LoadingForm
        ///   6. Show TAResultForm on its own STA thread (prevents focus loss)
        ///   7. Restore focus to Power PDF via SetForegroundWindow
        /// </summary>
        string IDMSConnector.DocAddNew(string sourceFile, string title, string[] docProperties)
        {
            // ── Step 1: Load configuration from registry ──────────────
            string sdkUrl = LoadPlainSetting(TA_SDK_URL);
            string userId = LoadEncryptedSetting(TA_USER_ID);
            string password = LoadEncryptedSetting(TA_PASSWORD);
            string processId = LoadPlainSetting(TA_PROCESS_ID);
            string processName = LoadPlainSetting(TA_PROCESS_NAME);
            string initVarsJson = LoadPlainSetting(TA_INIT_VARIABLES);

            if (string.IsNullOrEmpty(sdkUrl) ||
                string.IsNullOrEmpty(userId) ||
                string.IsNullOrEmpty(processId))
            {
                MessageBox.Show(
                    "TotalAgility connector is not fully configured.\n\n" +
                    "Please go to File → Options → Connectors → " +
                    "Send to TotalAgility\nand complete the setup " +
                    "(SDK URL, credentials, and process selection).",
                    "Configuration Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return "";
            }

            // ── Step 2: Resolve document name ─────────────────────────
            string documentName = string.IsNullOrEmpty(title)
                ? Path.GetFileName(sourceFile)
                : title;

            // ── Step 3: Deserialise saved init variables ───────────────
            List<TAInputVariable> inputVariables = new List<TAInputVariable>();
            if (!string.IsNullOrEmpty(initVarsJson))
            {
                try
                {
                    inputVariables =
                        JsonConvert.DeserializeObject<List<TAInputVariable>>(initVarsJson)
                        ?? new List<TAInputVariable>();
                }
                catch { /* non-fatal — proceed with empty list */ }
            }

            // ── Step 4: Show LoadingForm on a separate STA thread ──────
            // Matches RAI connector threading pattern exactly.
            LoadingForm loadingForm = null;
            Thread loadingThread = new Thread(() =>
            {
                loadingForm = new LoadingForm();
                Application.Run(loadingForm);
            });
            loadingThread.SetApartmentState(ApartmentState.STA);
            loadingThread.Start();

            // ── Step 5: Call TotalAgility API ─────────────────────────
            TAJobResult jobResult = null;
            try
            {
                var taService = new TotalAgilityService(sdkUrl);

                TASession session = taService.LogOn(userId, password);
                SavePlainSetting(TA_SESSION_ID, session.SessionId);

                byte[] pdfBytes = File.ReadAllBytes(sourceFile);

                TAProcessIdentity processIdentity = new TAProcessIdentity
                {
                    Id = processId,
                    Name = processName
                };

                jobResult = taService.CreateJobWithDocuments(
                    session.SessionId,
                    processIdentity,
                    pdfBytes,
                    inputVariables);

                jobResult.DocumentName = documentName;
            }
            catch (Exception ex)
            {
                jobResult = new TAJobResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    ProcessName = processName,
                    DocumentName = documentName
                };
            }
            // ── Step 6: Close LoadingForm, Step 7: Show Result ──────
            // The result thread is prepared and started INSIDE the finally block,
            // BEFORE closing the loading form. This means the result form begins
            // initialising while the loading form is still visible — by the time
            // the loading form closes, the result form is ready to appear
            // immediately with no gap and no focus loss.
            TAJobResult resultToShow = jobResult;
            IntPtr ownerHwnd = _parentWindow;
            Thread resultThread = new Thread(() =>
            {
                using (var resultForm = new TAResultForm(resultToShow))
                {
                    if (ownerHwnd != IntPtr.Zero)
                        resultForm.ShowDialog(new WindowWrapper(ownerHwnd));
                    else
                        resultForm.ShowDialog();
                }
            });
            resultThread.SetApartmentState(ApartmentState.STA);

            try
            {
                // Start result thread first — form begins loading immediately
                resultThread.Start();

                // Now close the loading form
                if (loadingForm != null && loadingForm.InvokeRequired)
                    loadingForm.Invoke(new Action(() => loadingForm.Close()));
                else if (loadingForm != null)
                    loadingForm.Close();
            }
            catch { }

            loadingThread.Join();

            // Wait for user to close the result form
            resultThread.Join();

            // ── Step 8: Restore focus to Power PDF ────────────────────
            if (_parentWindow != IntPtr.Zero)
                SetForegroundWindow(_parentWindow);

            return "";
        }

        // ── IDMSConnector: Document Lifecycle ────────────────────────────

        void IDMSConnector.DocOpen(string docId, OpenMode mode)
        {
            if (_documents.TryGetValue(docId, out Document doc))
                doc.Open(mode);
        }

        void IDMSConnector.DocClose(string docId, DMSConnector.CloseReason reason)
        {
            if (_documents.TryGetValue(docId, out Document doc))
            {
                doc.Close();
                _documents.Remove(docId);
            }
        }

        void IDMSConnector.DocModified(string docId)
        {
            // No action needed
        }

        string IDMSConnector.DocGetLocalFile(string docId)
        {
            return _documents.TryGetValue(docId, out Document doc)
                ? doc.LocalFileName : null;
        }

        OpenMode IDMSConnector.DocGetOpenMode(string docId)
        {
            return _documents.TryGetValue(docId, out Document doc)
                ? doc.OpenMode : OpenMode.OPEN_NONE;
        }

        string IDMSConnector.DocGetTitle(string docId)
        {
            return _documents.TryGetValue(docId, out Document doc)
                ? doc.Title : string.Empty;
        }

        void IDMSConnector.DocPrepareSave(string docId, int menuItemId,
            string[] docProperties, out string targetFileName)
        {
            targetFileName = Path.GetTempFileName();
        }

        void IDMSConnector.DocSave(string docId, string targetFileName,
            out string newDocId)
        {
            newDocId = null;
        }

        void IDMSConnector.DocSelectFiles(DMSConnector.SelectType type,
            int MenuIndex, out string[] docIds)
        {
            docIds = new string[0];
        }

        string IDMSConnector.FileIsFromDms(string localFile)
        {
            throw new NotImplementedException();
        }

        // ── IDMSConnector: Property Page ──────────────────────────────────

        IPropertyPageHandler IDMSConnector.PropertyPageHandler
        {
            get { return this; }
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        int IPropertyPageHandler.Create(int parenthWnd)
        {
            ConfigurationControl p = new ConfigurationControl(this);
            SetParent(p.Handle, (IntPtr)parenthWnd);
            return p.Handle.ToInt32();
        }

        void IPropertyPageHandler.ShowHelp(int hWnd)
        {
            ConfigurationControl.GetSheet(hWnd)?.ShowHelp();
        }

        void IPropertyPageHandler.CheckData(int hWnd)
        {
            ConfigurationControl.GetSheet(hWnd)?.CheckData();
        }

        void IPropertyPageHandler.UpdateData(int hWnd)
        {
            ConfigurationControl.GetSheet(hWnd)?.UpdateData();
        }

        // ── Registry Helpers ──────────────────────────────────────────────

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
                string base64 = rk.GetValue(key, "") as string;
                if (string.IsNullOrEmpty(base64)) return "";
                try
                {
                    byte[] encrypted = Convert.FromBase64String(base64);
                    byte[] decrypted = ProtectedData.Unprotect(
                        encrypted, null, DataProtectionScope.CurrentUser);
                    return Encoding.UTF8.GetString(decrypted);
                }
                catch { return ""; }
            }
        }

        // ── COM Registration Helpers ──────────────────────────────────────

        private static RegistryKey OpenOrCreateSubkey(RegistryKey parent,
            string name)
        {
            RegistryKey rk = parent.OpenSubKey(name, true)
                          ?? parent.CreateSubKey(name);
            if (rk == null)
                throw new COMException(
                    "Cannot register connector — check admin permissions.");
            return rk;
        }

        private static string GetProgId(Type t)
        {
            var attrs = t.GetCustomAttributes(typeof(ProgIdAttribute), true);
            if (attrs.Length == 0)
                throw new InvalidOperationException("ProgId attribute missing.");
            return ((ProgIdAttribute)attrs[0]).Value;
        }
    }
}