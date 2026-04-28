using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace PPDF.TotalAgility.Connector
{
    public partial class TAServicesTester
    {
        private async void btnFederatedLogin_Click(object sender, EventArgs e)
        {
            string sdkUrl = txtSdkUrl.Text.Trim().TrimEnd('/');

            if (string.IsNullOrEmpty(sdkUrl))
            {
                MessageBox.Show("Please enter the TA SDK URL.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                txtStatus.Text = "Getting authentication providers...";
                Application.DoEvents();

                string taBaseUrl = GetTaBaseUrl(sdkUrl);

                // Step 1: Get providers using base callback URL first
                // Use exact callback URL format confirmed from Edge network trace
                string encodedOrigin = Uri.EscapeDataString(taBaseUrl + "/Forms/Custom/logon.html");
                string baseCallbackUrl = $"{taBaseUrl}/FederatedLogin.aspx?Protocol=Workspace&Origin={encodedOrigin}";
                string providerListJson = Fed_CallGetProviders(sdkUrl, baseCallbackUrl, 1, taBaseUrl);
                JObject providerResponse = JObject.Parse(providerListJson);
                JArray providers = providerResponse["d"]?["AuthenticationProviderIdentityCollection"] as JArray;

                if (providers == null || providers.Count == 0)
                {
                    MessageBox.Show("No federated authentication providers found.",
                        "No Providers", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtStatus.Text = "No providers found.";
                    return;
                }

                JObject selectedProvider = null;

                if (providers.Count == 1)
                {
                    selectedProvider = (JObject)providers[0];
                    txtStatus.Text = $"Using provider: {selectedProvider["Name"]}";
                }
                else
                {
                    string[] providerNames = new string[providers.Count];
                    for (int i = 0; i < providers.Count; i++)
                        providerNames[i] = providers[i]["Name"]?.ToString() ?? $"Provider {i + 1}";

                    using (var picker = new ProviderPickerForm(providerNames))
                    {
                        if (picker.ShowDialog() != DialogResult.OK)
                        {
                            txtStatus.Text = "Login cancelled.";
                            return;
                        }
                        selectedProvider = (JObject)providers[picker.SelectedIndex];
                    }
                }

                string providerId = selectedProvider["Id"]?.ToString();
                string providerName = selectedProvider["Name"]?.ToString();

                // Navigate directly to TotalAgility logon page
                // TotalAgility handles the SAML redirect to Azure AD itself
                // Same as what Chrome/Edge does when you open TotalAgility
                string signInUrl = taBaseUrl + "/Forms/Custom/logon.html";

                txtStatus.Text = "Opening login browser...";
                Application.DoEvents();

                string sessionId = await Fed_ShowLoginBrowser(signInUrl, taBaseUrl);

                if (string.IsNullOrEmpty(sessionId))
                {
                    txtStatus.Text = "Login failed or cancelled.";
                    return;
                }

                txtUserId.Text = sessionId;
                txtStatus.Text = "Federated login successful! SessionId extracted.";

                MessageBox.Show(
                    $"Federated login successful!\n\nSessionId:\n{sessionId}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Error: " + ex.Message;
                MessageBox.Show("Federated login error:\n\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string Fed_CallGetProviders(string sdkUrl, string callbackUrl,
            short callbackProtocol, string origin)
        {
            string url = sdkUrl + "/UserService.svc/json/GetFederatedAuthenticationProvidersForSignIn";
            var payload = new { callbackUrl, callbackProtocol, origin };
            return Fed_PostJson(url, JsonConvert.SerializeObject(payload));
        }

        private string Fed_CallGetSignInUrl(string sdkUrl, string callbackUrl,
            string providerId, string providerName,
            short callbackProtocol, string origin)
        {
            string url = sdkUrl + "/UserService.svc/json/GetFederatedAuthenticationSignInUrl";
            var payload = new
            {
                callbackUrl,
                authenticationProviderIdentity = new { Id = providerId, Name = providerName },
                callbackProtocol,
                origin
            };
            return Fed_PostJson(url, JsonConvert.SerializeObject(payload));
        }

        private async Task<string> Fed_ShowLoginBrowser(
            string signInUrl, string taBaseUrl)
        {
            string sessionId = null;

            using (var browserForm = new Form())
            {
                browserForm.Text = "TotalAgility — Federated Login";
                browserForm.Size = new System.Drawing.Size(600, 700);
                browserForm.StartPosition = FormStartPosition.CenterScreen;

                var webView = new WebView2();
                webView.Dock = DockStyle.Fill;
                browserForm.Controls.Add(webView);

                browserForm.Shown += async (showSender, showArgs) =>
                {
                    // Try to reuse existing Edge profile for silent SSO
                    // Falls back to isolated profile if Edge not found or fails
                    try
                    {
                        string edgeFolder = System.IO.Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            "Microsoft", "Edge", "User Data");

                        if (System.IO.Directory.Exists(edgeFolder))
                        {
                            var env = await Microsoft.Web.WebView2.Core.CoreWebView2Environment
                                .CreateAsync(null, edgeFolder);
                            await webView.EnsureCoreWebView2Async(env);
                        }
                        else
                        {
                            await webView.EnsureCoreWebView2Async(null);
                        }
                    }
                    catch
                    {
                        await webView.EnsureCoreWebView2Async(null);
                    }

                    webView.CoreWebView2.Navigate(signInUrl);

                    webView.CoreWebView2.NavigationCompleted += async (s, args) =>
                    {
                        string currentUrl = webView.CoreWebView2.Source;

                        bool isOnMicrosoftLogin =
                            currentUrl.Contains("login.microsoftonline.com") ||
                            currentUrl.Contains("login.microsoft.com");

                        bool isOnTaDomain =
                            currentUrl.Contains(taBaseUrl) ||
                            currentUrl.Contains("FederatedLogin") ||
                            currentUrl.Contains("GeneralWorkqueue") ||
                            currentUrl.Contains("Workspace");

                        if (!isOnMicrosoftLogin && isOnTaDomain)
                        {
                            try
                            {
                                string script = @"
                                    (function() {
                                        try {
                                            var raw = window.sessionStorage.getItem('SESSION_ID');
                                            if (raw) {
                                                var parsed = JSON.parse(raw);
                                                return parsed.value || '';
                                            }
                                            return '';
                                        } catch(e) { return ''; }
                                    })()";

                                string result = await webView.CoreWebView2
                                    .ExecuteScriptAsync(script);

                                result = result?.Trim('"');

                                if (!string.IsNullOrEmpty(result) &&
                                    result != "null" && result != "")
                                {
                                    sessionId = result;
                                    browserForm.Invoke(new Action(() => browserForm.Close()));
                                }
                            }
                            catch { }
                        }
                    };
                };

                browserForm.ShowDialog(this);
            }

            return sessionId;
        }

        private string GetTaBaseUrl(string sdkUrl)
        {
            int idx = sdkUrl.IndexOf("/Services/", StringComparison.OrdinalIgnoreCase);
            if (idx > 0) return sdkUrl.Substring(0, idx);

            Uri uri = new Uri(sdkUrl);
            return uri.Scheme + "://" + uri.Host +
                   (uri.Port != 80 && uri.Port != 443 ? ":" + uri.Port : "");
        }

        private string Fed_PostJson(string url, string json)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Proxy = null;
            request.Timeout = 30000;
            request.ContentType = "application/json";
            request.Method = "POST";

            byte[] body = Encoding.UTF8.GetBytes(json);
            request.ContentLength = body.Length;

            using (Stream stream = request.GetRequestStream())
                stream.Write(body, 0, body.Length);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                return reader.ReadToEnd();
        }
    }

    public class ProviderPickerForm : Form
    {
        public int SelectedIndex { get; private set; } = 0;

        public ProviderPickerForm(string[] providerNames)
        {
            Text = "Select Authentication Provider";
            Size = new System.Drawing.Size(350, 180);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            var lbl = new Label
            {
                Text = "Select a provider to log in with:",
                Location = new System.Drawing.Point(12, 12),
                AutoSize = true
            };

            var combo = new ComboBox
            {
                Location = new System.Drawing.Point(12, 36),
                Width = 310,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            combo.Items.AddRange(providerNames);
            combo.SelectedIndex = 0;

            var btnOk = new Button
            {
                Text = "Continue",
                Location = new System.Drawing.Point(160, 80),
                Width = 80,
                DialogResult = DialogResult.OK
            };
            btnOk.Click += (s, e) => { SelectedIndex = combo.SelectedIndex; Close(); };

            var btnCancel = new Button
            {
                Text = "Cancel",
                Location = new System.Drawing.Point(248, 80),
                Width = 74,
                DialogResult = DialogResult.Cancel
            };

            Controls.AddRange(new Control[] { lbl, combo, btnOk, btnCancel });
            AcceptButton = btnOk;
            CancelButton = btnCancel;
        }
    }
}