namespace PPDF.TotalAgility.Connector
{
    partial class ConfirmationForm
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.checkBoxDoNotShow = new System.Windows.Forms.CheckBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnChange = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // ── lblMessage ────────────────────────────────────────────
            this.lblMessage.AutoSize = false;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblMessage.Location = new System.Drawing.Point(20, 20);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(400, 50);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ── btnSend ───────────────────────────────────────────────
            this.btnSend.BackColor = System.Drawing.Color.SteelBlue;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(20, 82);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(80, 28);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);

            // ── btnChange ─────────────────────────────────────────────
            this.btnChange.Location = new System.Drawing.Point(112, 82);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(80, 28);
            this.btnChange.TabIndex = 2;
            this.btnChange.Text = "Change";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);

            // ── btnCancel ─────────────────────────────────────────────
            this.btnCancel.Location = new System.Drawing.Point(204, 82);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 28);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // ── checkBoxDoNotShow ─────────────────────────────────────
            this.checkBoxDoNotShow.AutoSize = true;
            this.checkBoxDoNotShow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.checkBoxDoNotShow.ForeColor = System.Drawing.Color.DimGray;
            this.checkBoxDoNotShow.Location = new System.Drawing.Point(22, 122);
            this.checkBoxDoNotShow.Name = "checkBoxDoNotShow";
            this.checkBoxDoNotShow.Size = new System.Drawing.Size(200, 18);
            this.checkBoxDoNotShow.TabIndex = 4;
            this.checkBoxDoNotShow.Text = "Do not show this message again";

            // ── ConfirmationForm ──────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 152);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnChange);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.checkBoxDoNotShow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfirmationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Send to TotalAgility";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.CheckBox checkBoxDoNotShow;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.Button btnCancel;
    }
}