namespace PPDF.TotalAgility.Connector
{
    partial class TAServicesTester
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblSdkUrl = new System.Windows.Forms.Label();
            this.txtSdkUrl = new System.Windows.Forms.TextBox();
            this.lblUserId = new System.Windows.Forms.Label();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogOn = new System.Windows.Forms.Button();
            this.lblProcesses = new System.Windows.Forms.Label();
            this.listBoxProcesses = new System.Windows.Forms.ListBox();
            this.btnGetProcesses = new System.Windows.Forms.Button();
            this.lblVariables = new System.Windows.Forms.Label();
            this.listBoxVariables = new System.Windows.Forms.ListBox();
            this.lblPdfPath = new System.Windows.Forms.Label();
            this.txtPdfPath = new System.Windows.Forms.TextBox();
            this.btnBrowsePdf = new System.Windows.Forms.Button();
            this.btnCreateJobWithDocs = new System.Windows.Forms.Button();
            this.btnGetBase64 = new System.Windows.Forms.Button();
            this.lblJobId = new System.Windows.Forms.Label();
            this.txtJobId = new System.Windows.Forms.TextBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.btnFederatedLogin = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblSdkUrl
            // 
            this.lblSdkUrl.AutoSize = true;
            this.lblSdkUrl.Location = new System.Drawing.Point(12, 15);
            this.lblSdkUrl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSdkUrl.Name = "lblSdkUrl";
            this.lblSdkUrl.Size = new System.Drawing.Size(88, 16);
            this.lblSdkUrl.TabIndex = 0;
            this.lblSdkUrl.Text = "TA SDK URL:";
            // 
            // txtSdkUrl
            // 
            this.txtSdkUrl.Location = new System.Drawing.Point(120, 12);
            this.txtSdkUrl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSdkUrl.Name = "txtSdkUrl";
            this.txtSdkUrl.Size = new System.Drawing.Size(572, 22);
            this.txtSdkUrl.TabIndex = 1;
            this.txtSdkUrl.Text = "http://win-kmqjr1mbelc/TotalAgility/Services/Sdk";
            // 
            // lblUserId
            // 
            this.lblUserId.AutoSize = true;
            this.lblUserId.Location = new System.Drawing.Point(12, 47);
            this.lblUserId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Size = new System.Drawing.Size(55, 16);
            this.lblUserId.TabIndex = 2;
            this.lblUserId.Text = "User ID:";
            // 
            // txtUserId
            // 
            this.txtUserId.Location = new System.Drawing.Point(120, 43);
            this.txtUserId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(212, 22);
            this.txtUserId.TabIndex = 2;
            this.txtUserId.Text = "Administrator";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(353, 47);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(70, 16);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(427, 43);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(172, 22);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // btnLogOn
            // 
            this.btnLogOn.BackColor = System.Drawing.Color.SteelBlue;
            this.btnLogOn.ForeColor = System.Drawing.Color.White;
            this.btnLogOn.Location = new System.Drawing.Point(613, 41);
            this.btnLogOn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLogOn.Name = "btnLogOn";
            this.btnLogOn.Size = new System.Drawing.Size(107, 28);
            this.btnLogOn.TabIndex = 4;
            this.btnLogOn.Text = "1. LogOn";
            this.btnLogOn.UseVisualStyleBackColor = false;
            this.btnLogOn.Click += new System.EventHandler(this.btnLogOn_Click);
            // 
            // lblProcesses
            // 
            this.lblProcesses.AutoSize = true;
            this.lblProcesses.Location = new System.Drawing.Point(12, 84);
            this.lblProcesses.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProcesses.Name = "lblProcesses";
            this.lblProcesses.Size = new System.Drawing.Size(75, 16);
            this.lblProcesses.TabIndex = 5;
            this.lblProcesses.Text = "Processes:";
            // 
            // listBoxProcesses
            // 
            this.listBoxProcesses.ItemHeight = 16;
            this.listBoxProcesses.Location = new System.Drawing.Point(12, 103);
            this.listBoxProcesses.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBoxProcesses.Name = "listBoxProcesses";
            this.listBoxProcesses.Size = new System.Drawing.Size(532, 100);
            this.listBoxProcesses.TabIndex = 5;
            this.listBoxProcesses.SelectedIndexChanged += new System.EventHandler(this.listBoxProcesses_SelectedIndexChanged);
            // 
            // btnGetProcesses
            // 
            this.btnGetProcesses.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGetProcesses.Enabled = false;
            this.btnGetProcesses.ForeColor = System.Drawing.Color.White;
            this.btnGetProcesses.Location = new System.Drawing.Point(560, 103);
            this.btnGetProcesses.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGetProcesses.Name = "btnGetProcesses";
            this.btnGetProcesses.Size = new System.Drawing.Size(160, 28);
            this.btnGetProcesses.TabIndex = 6;
            this.btnGetProcesses.Text = "2. Get Processes";
            this.btnGetProcesses.UseVisualStyleBackColor = false;
            this.btnGetProcesses.Click += new System.EventHandler(this.btnGetProcesses_Click);
            // 
            // lblVariables
            // 
            this.lblVariables.AutoSize = true;
            this.lblVariables.Location = new System.Drawing.Point(12, 215);
            this.lblVariables.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblVariables.Name = "lblVariables";
            this.lblVariables.Size = new System.Drawing.Size(283, 16);
            this.lblVariables.TabIndex = 7;
            this.lblVariables.Text = "Init Variables (select a process above to load):";
            // 
            // listBoxVariables
            // 
            this.listBoxVariables.ItemHeight = 16;
            this.listBoxVariables.Location = new System.Drawing.Point(12, 235);
            this.listBoxVariables.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBoxVariables.Name = "listBoxVariables";
            this.listBoxVariables.Size = new System.Drawing.Size(705, 100);
            this.listBoxVariables.TabIndex = 7;
            // 
            // lblPdfPath
            // 
            this.lblPdfPath.AutoSize = true;
            this.lblPdfPath.Location = new System.Drawing.Point(12, 351);
            this.lblPdfPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPdfPath.Name = "lblPdfPath";
            this.lblPdfPath.Size = new System.Drawing.Size(62, 16);
            this.lblPdfPath.TabIndex = 8;
            this.lblPdfPath.Text = "PDF File:";
            // 
            // txtPdfPath
            // 
            this.txtPdfPath.Location = new System.Drawing.Point(87, 347);
            this.txtPdfPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPdfPath.Name = "txtPdfPath";
            this.txtPdfPath.ReadOnly = true;
            this.txtPdfPath.Size = new System.Drawing.Size(399, 22);
            this.txtPdfPath.TabIndex = 8;
            // 
            // btnBrowsePdf
            // 
            this.btnBrowsePdf.Enabled = false;
            this.btnBrowsePdf.Location = new System.Drawing.Point(493, 346);
            this.btnBrowsePdf.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBrowsePdf.Name = "btnBrowsePdf";
            this.btnBrowsePdf.Size = new System.Drawing.Size(93, 27);
            this.btnBrowsePdf.TabIndex = 9;
            this.btnBrowsePdf.Text = "Browse...";
            this.btnBrowsePdf.Click += new System.EventHandler(this.btnBrowsePdf_Click);
            // 
            // btnCreateJobWithDocs
            // 
            this.btnCreateJobWithDocs.BackColor = System.Drawing.Color.ForestGreen;
            this.btnCreateJobWithDocs.Enabled = false;
            this.btnCreateJobWithDocs.ForeColor = System.Drawing.Color.White;
            this.btnCreateJobWithDocs.Location = new System.Drawing.Point(12, 385);
            this.btnCreateJobWithDocs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCreateJobWithDocs.Name = "btnCreateJobWithDocs";
            this.btnCreateJobWithDocs.Size = new System.Drawing.Size(267, 32);
            this.btnCreateJobWithDocs.TabIndex = 11;
            this.btnCreateJobWithDocs.Text = "3. Create Job with Document";
            this.btnCreateJobWithDocs.UseVisualStyleBackColor = false;
            this.btnCreateJobWithDocs.Click += new System.EventHandler(this.btnCreateJobWithDocs_Click);
            // 
            // btnGetBase64
            // 
            this.btnGetBase64.Location = new System.Drawing.Point(597, 346);
            this.btnGetBase64.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGetBase64.Name = "btnGetBase64";
            this.btnGetBase64.Size = new System.Drawing.Size(120, 27);
            this.btnGetBase64.TabIndex = 10;
            this.btnGetBase64.Text = "Copy Base64";
            this.btnGetBase64.Click += new System.EventHandler(this.btnGetBase64_Click);
            // 
            // lblJobId
            // 
            this.lblJobId.AutoSize = true;
            this.lblJobId.Location = new System.Drawing.Point(293, 391);
            this.lblJobId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblJobId.Name = "lblJobId";
            this.lblJobId.Size = new System.Drawing.Size(49, 16);
            this.lblJobId.TabIndex = 12;
            this.lblJobId.Text = "Job ID:";
            // 
            // txtJobId
            // 
            this.txtJobId.BackColor = System.Drawing.Color.LightGreen;
            this.txtJobId.Location = new System.Drawing.Point(353, 388);
            this.txtJobId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtJobId.Name = "txtJobId";
            this.txtJobId.ReadOnly = true;
            this.txtJobId.Size = new System.Drawing.Size(365, 22);
            this.txtJobId.TabIndex = 12;
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(12, 433);
            this.lblLog.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(33, 16);
            this.lblLog.TabIndex = 13;
            this.lblLog.Text = "Log:";
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.ForeColor = System.Drawing.Color.LimeGreen;
            this.txtLog.Location = new System.Drawing.Point(12, 455);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(739, 233);
            this.txtLog.TabIndex = 14;
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(653, 428);
            this.btnClearLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(80, 22);
            this.btnClearLog.TabIndex = 13;
            this.btnClearLog.Text = "Clear Log";
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // btnFederatedLogin
            // 
            this.btnFederatedLogin.BackColor = System.Drawing.Color.SteelBlue;
            this.btnFederatedLogin.ForeColor = System.Drawing.Color.White;
            this.btnFederatedLogin.Location = new System.Drawing.Point(251, 72);
            this.btnFederatedLogin.Margin = new System.Windows.Forms.Padding(4);
            this.btnFederatedLogin.Name = "btnFederatedLogin";
            this.btnFederatedLogin.Size = new System.Drawing.Size(107, 28);
            this.btnFederatedLogin.TabIndex = 15;
            this.btnFederatedLogin.Text = "Fed LogOn";
            this.btnFederatedLogin.UseVisualStyleBackColor = false;
            this.btnFederatedLogin.Click += new System.EventHandler(this.btnFederatedLogin_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(385, 75);
            this.txtStatus.Margin = new System.Windows.Forms.Padding(4);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(335, 94);
            this.txtStatus.TabIndex = 16;
            this.txtStatus.UseSystemPasswordChar = true;
            // 
            // TAServicesTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 708);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.btnFederatedLogin);
            this.Controls.Add(this.lblSdkUrl);
            this.Controls.Add(this.txtSdkUrl);
            this.Controls.Add(this.lblUserId);
            this.Controls.Add(this.txtUserId);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogOn);
            this.Controls.Add(this.lblProcesses);
            this.Controls.Add(this.listBoxProcesses);
            this.Controls.Add(this.btnGetProcesses);
            this.Controls.Add(this.lblVariables);
            this.Controls.Add(this.listBoxVariables);
            this.Controls.Add(this.lblPdfPath);
            this.Controls.Add(this.txtPdfPath);
            this.Controls.Add(this.btnBrowsePdf);
            this.Controls.Add(this.btnGetBase64);
            this.Controls.Add(this.btnCreateJobWithDocs);
            this.Controls.Add(this.lblJobId);
            this.Controls.Add(this.txtJobId);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.btnClearLog);
            this.Controls.Add(this.txtLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "TAServicesTester";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TotalAgility Service Tester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        // ── Field declarations ────────────────────────────────────────────
        private System.Windows.Forms.Label lblSdkUrl;
        private System.Windows.Forms.TextBox txtSdkUrl;
        private System.Windows.Forms.Label lblUserId;
        private System.Windows.Forms.TextBox txtUserId;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogOn;
        private System.Windows.Forms.Label lblProcesses;
        private System.Windows.Forms.ListBox listBoxProcesses;
        private System.Windows.Forms.Button btnGetProcesses;
        private System.Windows.Forms.Label lblVariables;
        private System.Windows.Forms.ListBox listBoxVariables;
        private System.Windows.Forms.Label lblPdfPath;
        private System.Windows.Forms.TextBox txtPdfPath;
        private System.Windows.Forms.Button btnBrowsePdf;
        private System.Windows.Forms.Button btnGetBase64;
        private System.Windows.Forms.Button btnCreateJobWithDocs;
        private System.Windows.Forms.Label lblJobId;
        private System.Windows.Forms.TextBox txtJobId;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Button btnFederatedLogin;
        private System.Windows.Forms.TextBox txtStatus;
    }
}