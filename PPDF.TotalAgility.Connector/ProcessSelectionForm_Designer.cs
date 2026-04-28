namespace PPDF.TotalAgility.Connector
{
    partial class ProcessSelectionForm
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
            this.labelSectionProc = new System.Windows.Forms.Label();
            this.labelProcess = new System.Windows.Forms.Label();
            this.comboBoxProcess = new System.Windows.Forms.ComboBox();
            this.labelSectionVars = new System.Windows.Forms.Label();
            this.panelVariables = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancelForm = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // labelSectionProc
            this.labelSectionProc.BackColor = System.Drawing.Color.SteelBlue;
            this.labelSectionProc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionProc.ForeColor = System.Drawing.Color.White;
            this.labelSectionProc.Location = new System.Drawing.Point(8, 8);
            this.labelSectionProc.Name = "labelSectionProc";
            this.labelSectionProc.Size = new System.Drawing.Size(432, 20);
            this.labelSectionProc.Text = "  Process Configuration";
            this.labelSectionProc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelSectionProc.TabIndex = 0;

            // labelProcess
            this.labelProcess.AutoSize = true;
            this.labelProcess.Location = new System.Drawing.Point(10, 38);
            this.labelProcess.Name = "labelProcess";
            this.labelProcess.Text = "Process:";
            this.labelProcess.TabIndex = 1;

            // comboBoxProcess
            this.comboBoxProcess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.comboBoxProcess.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxProcess.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.comboBoxProcess.Location = new System.Drawing.Point(100, 36);
            this.comboBoxProcess.Name = "comboBoxProcess";
            this.comboBoxProcess.Size = new System.Drawing.Size(340, 21);
            this.comboBoxProcess.TabIndex = 2;
            this.comboBoxProcess.SelectedIndexChanged += new System.EventHandler(this.comboBoxProcess_SelectedIndexChanged);
            this.comboBoxProcess.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBoxProcess_KeyDown);
            this.comboBoxProcess.Leave += new System.EventHandler(this.comboBoxProcess_Leave);

            // labelSectionVars
            this.labelSectionVars.BackColor = System.Drawing.Color.SteelBlue;
            this.labelSectionVars.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.labelSectionVars.ForeColor = System.Drawing.Color.White;
            this.labelSectionVars.Location = new System.Drawing.Point(8, 66);
            this.labelSectionVars.Name = "labelSectionVars";
            this.labelSectionVars.Size = new System.Drawing.Size(432, 20);
            this.labelSectionVars.Text = "  Initialization Variables";
            this.labelSectionVars.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelSectionVars.TabIndex = 3;

            // panelVariables
            this.panelVariables.AutoScroll = true;
            this.panelVariables.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelVariables.Location = new System.Drawing.Point(8, 88);
            this.panelVariables.Name = "panelVariables";
            this.panelVariables.Size = new System.Drawing.Size(432, 200);
            this.panelVariables.TabIndex = 4;
            this.panelVariables.Enabled = false;

            // btnSave
            this.btnSave.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSave.Enabled = false;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(248, 300);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 26);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnCancelForm
            this.btnCancelForm.Location = new System.Drawing.Point(350, 300);
            this.btnCancelForm.Name = "btnCancelForm";
            this.btnCancelForm.Size = new System.Drawing.Size(90, 26);
            this.btnCancelForm.TabIndex = 6;
            this.btnCancelForm.Text = "Cancel";
            this.btnCancelForm.UseVisualStyleBackColor = true;
            this.btnCancelForm.Click += new System.EventHandler(this.btnCancelForm_Click);

            // ProcessSelectionForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 338);
            this.Controls.Add(this.labelSectionProc);
            this.Controls.Add(this.labelProcess);
            this.Controls.Add(this.comboBoxProcess);
            this.Controls.Add(this.labelSectionVars);
            this.Controls.Add(this.panelVariables);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancelForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProcessSelectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Send to TotalAgility \u2014 Select Process";
            this.Load += new System.EventHandler(this.ProcessSelectionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label labelSectionProc;
        private System.Windows.Forms.Label labelProcess;
        private System.Windows.Forms.ComboBox comboBoxProcess;
        private System.Windows.Forms.Label labelSectionVars;
        private System.Windows.Forms.Panel panelVariables;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancelForm;
    }
}