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
            // ── Credentials ───────────────────────────────────────────
            this.lblSdkUrl = new System.Windows.Forms.Label();
            this.txtSdkUrl = new System.Windows.Forms.TextBox();
            this.lblUserId = new System.Windows.Forms.Label();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogOn = new System.Windows.Forms.Button();

            // ── Processes ─────────────────────────────────────────────
            this.lblProcesses = new System.Windows.Forms.Label();
            this.listBoxProcesses = new System.Windows.Forms.ListBox();
            this.btnGetProcesses = new System.Windows.Forms.Button();

            // ── Variables ─────────────────────────────────────────────
            this.lblVariables = new System.Windows.Forms.Label();
            this.listBoxVariables = new System.Windows.Forms.ListBox();

            // ── PDF + Job ─────────────────────────────────────────────
            this.lblPdfPath = new System.Windows.Forms.Label();
            this.txtPdfPath = new System.Windows.Forms.TextBox();
            this.btnBrowsePdf = new System.Windows.Forms.Button();
            this.btnCreateJobWithDocs = new System.Windows.Forms.Button();
            this.btnGetBase64 = new System.Windows.Forms.Button();

            // ── Job ID result ─────────────────────────────────────────
            this.lblJobId = new System.Windows.Forms.Label();
            this.txtJobId = new System.Windows.Forms.TextBox();

            // ── Log ───────────────────────────────────────────────────
            this.lblLog = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnClearLog = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // ════════════════════════════════════════════════════════════
            // CREDENTIALS
            // ════════════════════════════════════════════════════════════

            // lblSdkUrl
            this.lblSdkUrl.AutoSize = true;
            this.lblSdkUrl.Location = new System.Drawing.Point(9, 12);
            this.lblSdkUrl.Name = "lblSdkUrl";
            this.lblSdkUrl.Text = "TA SDK URL:";

            // txtSdkUrl
            this.txtSdkUrl.Location = new System.Drawing.Point(90, 10);
            this.txtSdkUrl.Name = "txtSdkUrl";
            this.txtSdkUrl.Size = new System.Drawing.Size(430, 20);
            this.txtSdkUrl.TabIndex = 1;
            this.txtSdkUrl.Text = "http://win-kmqjr1mbelc/TotalAgility/Services/Sdk";

            // lblUserId
            this.lblUserId.AutoSize = true;
            this.lblUserId.Location = new System.Drawing.Point(9, 38);
            this.lblUserId.Name = "lblUserId";
            this.lblUserId.Text = "User ID:";

            // txtUserId
            this.txtUserId.Location = new System.Drawing.Point(90, 35);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(160, 20);
            this.txtUserId.TabIndex = 2;
            this.txtUserId.Text = "Administrator";

            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(265, 38);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Text = "Password:";

            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(320, 35);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(130, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;

            // btnLogOn
            this.btnLogOn.BackColor = System.Drawing.Color.SteelBlue;
            this.btnLogOn.ForeColor = System.Drawing.Color.White;
            this.btnLogOn.Location = new System.Drawing.Point(460, 33);
            this.btnLogOn.Name = "btnLogOn";
            this.btnLogOn.Size = new System.Drawing.Size(80, 23);
            this.btnLogOn.TabIndex = 4;
            this.btnLogOn.Text = "1. LogOn";
            this.btnLogOn.UseVisualStyleBackColor = false;
            this.btnLogOn.Click +=
                new System.EventHandler(this.btnLogOn_Click);

            // ════════════════════════════════════════════════════════════
            // PROCESSES
            // ════════════════════════════════════════════════════════════

            // lblProcesses
            this.lblProcesses.AutoSize = true;
            this.lblProcesses.Location = new System.Drawing.Point(9, 68);
            this.lblProcesses.Name = "lblProcesses";
            this.lblProcesses.Text = "Processes:";

            // listBoxProcesses
            this.listBoxProcesses.Location = new System.Drawing.Point(9, 84);
            this.listBoxProcesses.Name = "listBoxProcesses";
            this.listBoxProcesses.Size = new System.Drawing.Size(400, 82);
            this.listBoxProcesses.TabIndex = 5;
            this.listBoxProcesses.SelectedIndexChanged +=
                new System.EventHandler(this.listBoxProcesses_SelectedIndexChanged);

            // btnGetProcesses
            this.btnGetProcesses.BackColor = System.Drawing.Color.SteelBlue;
            this.btnGetProcesses.Enabled = false;
            this.btnGetProcesses.ForeColor = System.Drawing.Color.White;
            this.btnGetProcesses.Location = new System.Drawing.Point(420, 84);
            this.btnGetProcesses.Name = "btnGetProcesses";
            this.btnGetProcesses.Size = new System.Drawing.Size(120, 23);
            this.btnGetProcesses.TabIndex = 6;
            this.btnGetProcesses.Text = "2. Get Processes";
            this.btnGetProcesses.UseVisualStyleBackColor = false;
            this.btnGetProcesses.Click +=
                new System.EventHandler(this.btnGetProcesses_Click);

            // ════════════════════════════════════════════════════════════
            // VARIABLES
            // ════════════════════════════════════════════════════════════

            // lblVariables
            this.lblVariables.AutoSize = true;
            this.lblVariables.Location = new System.Drawing.Point(9, 175);
            this.lblVariables.Name = "lblVariables";
            this.lblVariables.Text =
                "Init Variables (select a process above to load):";

            // listBoxVariables
            this.listBoxVariables.Location = new System.Drawing.Point(9, 191);
            this.listBoxVariables.Name = "listBoxVariables";
            this.listBoxVariables.Size = new System.Drawing.Size(530, 82);
            this.listBoxVariables.TabIndex = 7;

            // ════════════════════════════════════════════════════════════
            // PDF + JOB
            // ════════════════════════════════════════════════════════════

            // lblPdfPath
            this.lblPdfPath.AutoSize = true;
            this.lblPdfPath.Location = new System.Drawing.Point(9, 285);
            this.lblPdfPath.Name = "lblPdfPath";
            this.lblPdfPath.Text = "PDF File:";

            // txtPdfPath
            this.txtPdfPath.Location = new System.Drawing.Point(65, 282);
            this.txtPdfPath.Name = "txtPdfPath";
            this.txtPdfPath.ReadOnly = true;
            this.txtPdfPath.Size = new System.Drawing.Size(300, 20);
            this.txtPdfPath.TabIndex = 8;

            // btnBrowsePdf
            this.btnBrowsePdf.Enabled = false;
            this.btnBrowsePdf.Location = new System.Drawing.Point(370, 281);
            this.btnBrowsePdf.Name = "btnBrowsePdf";
            this.btnBrowsePdf.Size = new System.Drawing.Size(70, 22);
            this.btnBrowsePdf.TabIndex = 9;
            this.btnBrowsePdf.Text = "Browse...";
            this.btnBrowsePdf.Click +=
                new System.EventHandler(this.btnBrowsePdf_Click);

            // btnGetBase64 — copies Base64 of selected PDF to clipboard for Postman testing
            this.btnGetBase64.Location = new System.Drawing.Point(448, 281);
            this.btnGetBase64.Name = "btnGetBase64";
            this.btnGetBase64.Size = new System.Drawing.Size(90, 22);
            this.btnGetBase64.TabIndex = 10;
            this.btnGetBase64.Text = "Copy Base64";
            this.btnGetBase64.Click +=
                new System.EventHandler(this.btnGetBase64_Click);

            // btnCreateJobWithDocs
            this.btnCreateJobWithDocs.BackColor = System.Drawing.Color.ForestGreen;
            this.btnCreateJobWithDocs.Enabled = false;
            this.btnCreateJobWithDocs.ForeColor = System.Drawing.Color.White;
            this.btnCreateJobWithDocs.Location =
                new System.Drawing.Point(9, 313);
            this.btnCreateJobWithDocs.Name = "btnCreateJobWithDocs";
            this.btnCreateJobWithDocs.Size = new System.Drawing.Size(200, 26);
            this.btnCreateJobWithDocs.TabIndex = 11;
            this.btnCreateJobWithDocs.Text =
                "3. Create Job with Document";
            this.btnCreateJobWithDocs.UseVisualStyleBackColor = false;
            this.btnCreateJobWithDocs.Click +=
                new System.EventHandler(this.btnCreateJobWithDocs_Click);

            // ════════════════════════════════════════════════════════════
            // JOB ID RESULT
            // ════════════════════════════════════════════════════════════

            // lblJobId
            this.lblJobId.AutoSize = true;
            this.lblJobId.Location = new System.Drawing.Point(220, 318);
            this.lblJobId.Name = "lblJobId";
            this.lblJobId.Text = "Job ID:";

            // txtJobId
            this.txtJobId.BackColor = System.Drawing.Color.LightGreen;
            this.txtJobId.Location = new System.Drawing.Point(265, 315);
            this.txtJobId.Name = "txtJobId";
            this.txtJobId.ReadOnly = true;
            this.txtJobId.Size = new System.Drawing.Size(275, 20);
            this.txtJobId.TabIndex = 12;

            // ════════════════════════════════════════════════════════════
            // LOG
            // ════════════════════════════════════════════════════════════

            // lblLog
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(9, 352);
            this.lblLog.Name = "lblLog";
            this.lblLog.Text = "Log:";

            // btnClearLog
            this.btnClearLog.Location = new System.Drawing.Point(490, 348);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(60, 18);
            this.btnClearLog.TabIndex = 13;
            this.btnClearLog.Text = "Clear Log";
            this.btnClearLog.Click +=
                new System.EventHandler(this.btnClearLog_Click);

            // txtLog
            this.txtLog.BackColor = System.Drawing.Color.Black;
            this.txtLog.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtLog.ForeColor = System.Drawing.Color.LimeGreen;
            this.txtLog.Location = new System.Drawing.Point(9, 370);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(555, 190);
            this.txtLog.TabIndex = 14;

            // ════════════════════════════════════════════════════════════
            // TAServicesTester FORM
            // ════════════════════════════════════════════════════════════
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 575);
            this.FormBorderStyle =
                System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "TAServicesTester";
            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TotalAgility Service Tester";

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
    }
}