namespace PPDF.TotalAgility.Connector
{
    partial class TAResultForm
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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.pictureBoxStatus = new System.Windows.Forms.PictureBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.panelDetail = new System.Windows.Forms.Panel();
            this.lblJobId = new System.Windows.Forms.Label();
            this.lblJobIdValue = new System.Windows.Forms.TextBox();
            this.lblProcess = new System.Windows.Forms.Label();
            this.lblProcessValue = new System.Windows.Forms.TextBox();
            this.lblDocument = new System.Windows.Forms.Label();
            this.lblDocumentValue = new System.Windows.Forms.TextBox();
            this.lblCreatedAt = new System.Windows.Forms.Label();
            this.lblCreatedAtValue = new System.Windows.Forms.TextBox();
            this.panelError = new System.Windows.Forms.Panel();
            this.lblErrorCaption = new System.Windows.Forms.Label();
            this.txtErrorMessage = new System.Windows.Forms.RichTextBox();
            this.btnClose = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)
                (this.pictureBoxStatus)).BeginInit();
            this.panelHeader.SuspendLayout();
            this.panelDetail.SuspendLayout();
            this.panelError.SuspendLayout();
            this.SuspendLayout();

            // ════════════════════════════════════════════════════════════
            // panelHeader
            // ════════════════════════════════════════════════════════════
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(560, 64);
            this.panelHeader.TabIndex = 0;
            this.panelHeader.Controls.Add(this.pictureBoxStatus);
            this.panelHeader.Controls.Add(this.lblHeader);

            // pictureBoxStatus
            this.pictureBoxStatus.Location = new System.Drawing.Point(14, 12);
            this.pictureBoxStatus.Name = "pictureBoxStatus";
            this.pictureBoxStatus.Size = new System.Drawing.Size(40, 40);
            this.pictureBoxStatus.SizeMode =
                System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxStatus.TabIndex = 0;
            this.pictureBoxStatus.TabStop = false;

            // lblHeader
            this.lblHeader.AutoSize = false;
            this.lblHeader.Font = new System.Drawing.Font(
                "Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(64, 14);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(480, 36);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "";
            this.lblHeader.TextAlign =
                System.Drawing.ContentAlignment.MiddleLeft;

            // ════════════════════════════════════════════════════════════
            // panelDetail — SUCCESS: 4 rows
            // ════════════════════════════════════════════════════════════
            this.panelDetail.Location = new System.Drawing.Point(0, 64);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(560, 185);
            this.panelDetail.TabIndex = 1;
            this.panelDetail.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblJobId,    this.lblJobIdValue,
                this.lblProcess,  this.lblProcessValue,
                this.lblDocument, this.lblDocumentValue,
                this.lblCreatedAt,this.lblCreatedAtValue
            });

            // Row 1 — Job ID
            this.lblJobId.AutoSize = false;
            this.lblJobId.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblJobId.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.lblJobId.Location =
                new System.Drawing.Point(24, 16);
            this.lblJobId.Name = "lblJobId";
            this.lblJobId.Size =
                new System.Drawing.Size(130, 24);
            this.lblJobId.Text = "Job ID:";

            this.lblJobIdValue.BackColor = System.Drawing.SystemColors.Control;
            this.lblJobIdValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblJobIdValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblJobIdValue.ReadOnly = true;
            this.lblJobIdValue.TabStop = false;
            this.lblJobIdValue.Location =
                new System.Drawing.Point(160, 16);
            this.lblJobIdValue.Name = "lblJobIdValue";
            this.lblJobIdValue.Size =
                new System.Drawing.Size(370, 24);
            this.lblJobIdValue.Text = "";

            // Row 2 — Process Name
            this.lblProcess.AutoSize = false;
            this.lblProcess.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblProcess.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.lblProcess.Location =
                new System.Drawing.Point(24, 58);
            this.lblProcess.Name = "lblProcess";
            this.lblProcess.Size =
                new System.Drawing.Size(130, 24);
            this.lblProcess.Text = "Process:";

            this.lblProcessValue.BackColor = System.Drawing.SystemColors.Control;
            this.lblProcessValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblProcessValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblProcessValue.ReadOnly = true;
            this.lblProcessValue.TabStop = false;
            this.lblProcessValue.Location =
                new System.Drawing.Point(160, 58);
            this.lblProcessValue.Name = "lblProcessValue";
            this.lblProcessValue.Size =
                new System.Drawing.Size(370, 24);
            this.lblProcessValue.Text = "";

            // Row 3 — Document Name
            this.lblDocument.AutoSize = false;
            this.lblDocument.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDocument.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.lblDocument.Location =
                new System.Drawing.Point(24, 100);
            this.lblDocument.Name = "lblDocument";
            this.lblDocument.Size =
                new System.Drawing.Size(130, 24);
            this.lblDocument.Text = "Document:";

            this.lblDocumentValue.BackColor = System.Drawing.SystemColors.Control;
            this.lblDocumentValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblDocumentValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblDocumentValue.ReadOnly = true;
            this.lblDocumentValue.TabStop = false;
            this.lblDocumentValue.Location =
                new System.Drawing.Point(160, 100);
            this.lblDocumentValue.Name = "lblDocumentValue";
            this.lblDocumentValue.Size =
                new System.Drawing.Size(370, 24);
            this.lblDocumentValue.Text = "";

            // Row 4 — Created At
            this.lblCreatedAt.AutoSize = false;
            this.lblCreatedAt.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCreatedAt.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.lblCreatedAt.Location =
                new System.Drawing.Point(24, 142);
            this.lblCreatedAt.Name = "lblCreatedAt";
            this.lblCreatedAt.Size =
                new System.Drawing.Size(130, 24);
            this.lblCreatedAt.Text = "Created At:";

            this.lblCreatedAtValue.BackColor = System.Drawing.SystemColors.Control;
            this.lblCreatedAtValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblCreatedAtValue.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCreatedAtValue.ReadOnly = true;
            this.lblCreatedAtValue.TabStop = false;
            this.lblCreatedAtValue.Location =
                new System.Drawing.Point(160, 142);
            this.lblCreatedAtValue.Name = "lblCreatedAtValue";
            this.lblCreatedAtValue.Size =
                new System.Drawing.Size(370, 24);
            this.lblCreatedAtValue.Text = "";

            // ════════════════════════════════════════════════════════════
            // panelError — ERROR: caption + scrollable text
            // ════════════════════════════════════════════════════════════
            this.panelError.Location = new System.Drawing.Point(0, 64);
            this.panelError.Name = "panelError";
            this.panelError.Size = new System.Drawing.Size(560, 215);
            this.panelError.TabIndex = 2;
            this.panelError.Visible = false;
            this.panelError.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblErrorCaption,
                this.txtErrorMessage
            });

            this.lblErrorCaption.AutoSize = false;
            this.lblErrorCaption.Font = new System.Drawing.Font(
                "Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblErrorCaption.ForeColor =
                System.Drawing.Color.FromArgb(180, 30, 30);
            this.lblErrorCaption.Location =
                new System.Drawing.Point(16, 14);
            this.lblErrorCaption.Name = "lblErrorCaption";
            this.lblErrorCaption.Size =
                new System.Drawing.Size(528, 22);
            this.lblErrorCaption.Text =
                "The following error was returned:";

            this.txtErrorMessage.BackColor =
                System.Drawing.Color.FromArgb(255, 245, 245);
            this.txtErrorMessage.BorderStyle =
                System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtErrorMessage.Font = new System.Drawing.Font(
                "Consolas", 9F);
            this.txtErrorMessage.Location =
                new System.Drawing.Point(16, 42);
            this.txtErrorMessage.Name = "txtErrorMessage";
            this.txtErrorMessage.ReadOnly = true;
            this.txtErrorMessage.ScrollBars =
                System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtErrorMessage.Size =
                new System.Drawing.Size(528, 158);
            this.txtErrorMessage.TabIndex = 0;
            this.txtErrorMessage.Text = "";
            this.txtErrorMessage.WordWrap = true;

            // ════════════════════════════════════════════════════════════
            // btnClose
            // ════════════════════════════════════════════════════════════
            this.btnClose.Anchor =
                System.Windows.Forms.AnchorStyles.Bottom |
                System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.Font =
                new System.Drawing.Font("Segoe UI", 9F);
            this.btnClose.Location =
                new System.Drawing.Point(450, 257);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 28);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click +=
                new System.EventHandler(this.btnClose_Click);

            // ════════════════════════════════════════════════════════════
            // TAResultForm
            // ════════════════════════════════════════════════════════════
            this.AutoScaleDimensions =
                new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 295);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelDetail);
            this.Controls.Add(this.panelError);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle =
                System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TAResultForm";
            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Send to TotalAgility";
            this.Load += new System.EventHandler(this.TAResultForm_Load);

            ((System.ComponentModel.ISupportInitialize)
                (this.pictureBoxStatus)).EndInit();
            this.panelHeader.ResumeLayout(false);
            this.panelDetail.ResumeLayout(false);
            this.panelError.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.PictureBox pictureBoxStatus;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel panelDetail;
        private System.Windows.Forms.Label lblJobId;
        private System.Windows.Forms.TextBox lblJobIdValue;
        private System.Windows.Forms.Label lblProcess;
        private System.Windows.Forms.TextBox lblProcessValue;
        private System.Windows.Forms.Label lblDocument;
        private System.Windows.Forms.TextBox lblDocumentValue;
        private System.Windows.Forms.Label lblCreatedAt;
        private System.Windows.Forms.TextBox lblCreatedAtValue;
        private System.Windows.Forms.Panel panelError;
        private System.Windows.Forms.Label lblErrorCaption;
        private System.Windows.Forms.RichTextBox txtErrorMessage;
        private System.Windows.Forms.Button btnClose;
    }
}