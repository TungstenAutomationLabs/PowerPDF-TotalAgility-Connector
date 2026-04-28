namespace PPDF.TotalAgility.Connector
{
    partial class ConfigurationControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labelSectionCredentials = new System.Windows.Forms.Label();
            this.labelSdkUrl = new System.Windows.Forms.Label();
            this.textBoxSdkUrl = new System.Windows.Forms.TextBox();
            this.labelUserId = new System.Windows.Forms.Label();
            this.textBoxUserId = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.checkBoxFederatedSecurity = new System.Windows.Forms.CheckBox();
            this.labelSectionProcess = new System.Windows.Forms.Label();
            this.labelProcess = new System.Windows.Forms.Label();
            this.comboBoxProcess = new System.Windows.Forms.ComboBox();
            this.labelDocVariable = new System.Windows.Forms.Label();
            this.comboBoxDocVariable = new System.Windows.Forms.ComboBox();
            this.labelSectionVariables = new System.Windows.Forms.Label();
            this.panelVariables = new System.Windows.Forms.Panel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.checkBoxShowConfirmation = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelSectionCredentials
            // 
            this.labelSectionCredentials.BackColor = System.Drawing.Color.SteelBlue;
            this.labelSectionCredentials.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionCredentials.ForeColor = System.Drawing.Color.White;
            this.labelSectionCredentials.Location = new System.Drawing.Point(8, 8);
            this.labelSectionCredentials.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSectionCredentials.Name = "labelSectionCredentials";
            this.labelSectionCredentials.Size = new System.Drawing.Size(410, 20);
            this.labelSectionCredentials.TabIndex = 0;
            this.labelSectionCredentials.Text = "  TotalAgility Credentials";
            this.labelSectionCredentials.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelSdkUrl
            // 
            this.labelSdkUrl.AutoSize = true;
            this.labelSdkUrl.Location = new System.Drawing.Point(15, 37);
            this.labelSdkUrl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSdkUrl.Name = "labelSdkUrl";
            this.labelSdkUrl.Size = new System.Drawing.Size(74, 13);
            this.labelSdkUrl.TabIndex = 1;
            this.labelSdkUrl.Text = "TA SDK URL:";
            // 
            // textBoxSdkUrl
            // 
            this.textBoxSdkUrl.Location = new System.Drawing.Point(116, 35);
            this.textBoxSdkUrl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxSdkUrl.Name = "textBoxSdkUrl";
            this.textBoxSdkUrl.Size = new System.Drawing.Size(300, 20);
            this.textBoxSdkUrl.TabIndex = 2;
            // 
            // labelUserId
            // 
            this.labelUserId.AutoSize = true;
            this.labelUserId.Location = new System.Drawing.Point(15, 64);
            this.labelUserId.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelUserId.Name = "labelUserId";
            this.labelUserId.Size = new System.Drawing.Size(46, 13);
            this.labelUserId.TabIndex = 3;
            this.labelUserId.Text = "User ID:";
            // 
            // textBoxUserId
            // 
            this.textBoxUserId.Location = new System.Drawing.Point(116, 62);
            this.textBoxUserId.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxUserId.Name = "textBoxUserId";
            this.textBoxUserId.Size = new System.Drawing.Size(300, 20);
            this.textBoxUserId.TabIndex = 4;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(15, 91);
            this.labelPassword.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(56, 13);
            this.labelPassword.TabIndex = 5;
            this.labelPassword.Text = "Password:";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(116, 89);
            this.textBoxPassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(300, 20);
            this.textBoxPassword.TabIndex = 6;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(341, 116);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 7;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // checkBoxFederatedSecurity
            // 
            this.checkBoxFederatedSecurity.AutoSize = true;
            this.checkBoxFederatedSecurity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Italic);
            this.checkBoxFederatedSecurity.ForeColor = System.Drawing.Color.Gray;
            this.checkBoxFederatedSecurity.Location = new System.Drawing.Point(15, 120);
            this.checkBoxFederatedSecurity.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBoxFederatedSecurity.Name = "checkBoxFederatedSecurity";
            this.checkBoxFederatedSecurity.Size = new System.Drawing.Size(239, 19);
            this.checkBoxFederatedSecurity.TabIndex = 8;
            this.checkBoxFederatedSecurity.Text = "Use Federated Security (Coming Soon)";
            this.checkBoxFederatedSecurity.CheckedChanged += new System.EventHandler(this.checkBoxFederatedSecurity_CheckedChanged);
            // 
            // labelSectionProcess
            // 
            this.labelSectionProcess.BackColor = System.Drawing.Color.SteelBlue;
            this.labelSectionProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionProcess.ForeColor = System.Drawing.Color.White;
            this.labelSectionProcess.Location = new System.Drawing.Point(8, 142);
            this.labelSectionProcess.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSectionProcess.Name = "labelSectionProcess";
            this.labelSectionProcess.Size = new System.Drawing.Size(410, 20);
            this.labelSectionProcess.TabIndex = 9;
            this.labelSectionProcess.Text = "  Process Configuration";
            this.labelSectionProcess.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelProcess
            // 
            this.labelProcess.AutoSize = true;
            this.labelProcess.Location = new System.Drawing.Point(15, 171);
            this.labelProcess.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelProcess.Name = "labelProcess";
            this.labelProcess.Size = new System.Drawing.Size(48, 13);
            this.labelProcess.TabIndex = 10;
            this.labelProcess.Text = "Process:";
            // 
            // comboBoxProcess
            // 
            this.comboBoxProcess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProcess.Location = new System.Drawing.Point(116, 169);
            this.comboBoxProcess.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxProcess.Name = "comboBoxProcess";
            this.comboBoxProcess.Size = new System.Drawing.Size(300, 21);
            this.comboBoxProcess.TabIndex = 11;
            this.comboBoxProcess.SelectedIndexChanged += new System.EventHandler(this.comboBoxProcess_SelectedIndexChanged);
            // 
            // labelDocVariable
            // 
            this.labelDocVariable.AutoSize = true;
            this.labelDocVariable.Location = new System.Drawing.Point(15, 200);
            this.labelDocVariable.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDocVariable.Name = "labelDocVariable";
            this.labelDocVariable.Size = new System.Drawing.Size(100, 13);
            this.labelDocVariable.TabIndex = 12;
            this.labelDocVariable.Text = "Document Variable:";
            this.labelDocVariable.Visible = false;
            // 
            // comboBoxDocVariable
            // 
            this.comboBoxDocVariable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDocVariable.Location = new System.Drawing.Point(116, 197);
            this.comboBoxDocVariable.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBoxDocVariable.Name = "comboBoxDocVariable";
            this.comboBoxDocVariable.Size = new System.Drawing.Size(300, 21);
            this.comboBoxDocVariable.TabIndex = 13;
            this.comboBoxDocVariable.Visible = false;
            // 
            // labelSectionVariables
            // 
            this.labelSectionVariables.BackColor = System.Drawing.Color.SteelBlue;
            this.labelSectionVariables.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionVariables.ForeColor = System.Drawing.Color.White;
            this.labelSectionVariables.Location = new System.Drawing.Point(8, 228);
            this.labelSectionVariables.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSectionVariables.Name = "labelSectionVariables";
            this.labelSectionVariables.Size = new System.Drawing.Size(410, 20);
            this.labelSectionVariables.TabIndex = 14;
            this.labelSectionVariables.Text = "  Initialization Variables";
            this.labelSectionVariables.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelVariables
            // 
            this.panelVariables.AutoScroll = true;
            this.panelVariables.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelVariables.Location = new System.Drawing.Point(8, 250);
            this.panelVariables.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panelVariables.Name = "panelVariables";
            this.panelVariables.Size = new System.Drawing.Size(410, 200);
            this.panelVariables.TabIndex = 15;
            // 
            // buttonSave
            // 
            this.buttonSave.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.buttonSave.ForeColor = System.Drawing.Color.White;
            this.buttonSave.Location = new System.Drawing.Point(268, 474);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(150, 23);
            this.buttonSave.TabIndex = 16;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = false;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // 
            // checkBoxShowConfirmation
            // 
            this.checkBoxShowConfirmation.AutoSize = true;
            this.checkBoxShowConfirmation.Checked = true;
            this.checkBoxShowConfirmation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowConfirmation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.checkBoxShowConfirmation.Location = new System.Drawing.Point(15, 452);
            this.checkBoxShowConfirmation.Name = "checkBoxShowConfirmation";
            this.checkBoxShowConfirmation.Size = new System.Drawing.Size(220, 18);
            this.checkBoxShowConfirmation.TabIndex = 17;
            this.checkBoxShowConfirmation.Text = "Show confirmation before sending";
            // ConfigurationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelSectionCredentials);
            this.Controls.Add(this.labelSdkUrl);
            this.Controls.Add(this.textBoxSdkUrl);
            this.Controls.Add(this.labelUserId);
            this.Controls.Add(this.textBoxUserId);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.checkBoxFederatedSecurity);
            this.Controls.Add(this.labelSectionProcess);
            this.Controls.Add(this.labelProcess);
            this.Controls.Add(this.comboBoxProcess);
            this.Controls.Add(this.labelDocVariable);
            this.Controls.Add(this.comboBoxDocVariable);
            this.Controls.Add(this.labelSectionVariables);
            this.Controls.Add(this.panelVariables);
            this.Controls.Add(this.buttonSave);
            this.Name = "ConfigurationControl";
            this.Size = new System.Drawing.Size(450, 508);
            this.Load += new System.EventHandler(this.ConfigurationControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        // ── Field Declarations ──────────────────────────────────────
        private System.Windows.Forms.Label labelSectionCredentials;
        private System.Windows.Forms.Label labelSdkUrl;
        private System.Windows.Forms.TextBox textBoxSdkUrl;
        private System.Windows.Forms.Label labelUserId;
        private System.Windows.Forms.TextBox textBoxUserId;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.CheckBox checkBoxFederatedSecurity; // ← Placeholder
        private System.Windows.Forms.Label labelSectionProcess;
        private System.Windows.Forms.Label labelProcess;
        private System.Windows.Forms.ComboBox comboBoxProcess;
        private System.Windows.Forms.Label labelDocVariable;
        private System.Windows.Forms.ComboBox comboBoxDocVariable;
        private System.Windows.Forms.Label labelSectionVariables;
        private System.Windows.Forms.Panel panelVariables;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.CheckBox checkBoxShowConfirmation;
    }
}