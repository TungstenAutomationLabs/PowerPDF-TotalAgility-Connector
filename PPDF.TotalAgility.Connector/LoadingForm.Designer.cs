namespace PPDF.TotalAgility.Connector
{
    partial class LoadingForm
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
            this.pictureBoxLoading = new System.Windows.Forms.PictureBox();
            this.lblLoading = new System.Windows.Forms.Label();
            this.lblInstruction2 = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)
                (this.pictureBoxLoading)).BeginInit();
            this.SuspendLayout();

            // ── pictureBoxLoading ─────────────────────────────────────
            this.pictureBoxLoading.Location = new System.Drawing.Point(266, 43);
            this.pictureBoxLoading.Name = "pictureBoxLoading";
            this.pictureBoxLoading.Size = new System.Drawing.Size(161, 168);
            this.pictureBoxLoading.SizeMode =
                System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLoading.TabIndex = 0;
            this.pictureBoxLoading.TabStop = false;

            // ── lblLoading ────────────────────────────────────────────
            this.lblLoading.AutoSize = true;
            this.lblLoading.Font = new System.Drawing.Font(
                "Microsoft Sans Serif", 14F,
                System.Drawing.FontStyle.Bold,
                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoading.Location = new System.Drawing.Point(272, 242);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(169, 29);
            this.lblLoading.TabIndex = 1;
            this.lblLoading.Text = "Please wait...";

            // ── lblInstruction2 ───────────────────────────────────────
            this.lblInstruction2.AutoSize = true;
            this.lblInstruction2.Enabled = false;
            this.lblInstruction2.Font = new System.Drawing.Font(
                "Segoe UI", 11F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstruction2.Location = new System.Drawing.Point(161, 282);
            this.lblInstruction2.Name = "lblInstruction2";
            this.lblInstruction2.TabIndex = 2;
            this.lblInstruction2.Text =
                "Submitting document to TotalAgility...";
            this.lblInstruction2.UseCompatibleTextRendering = true;

            // ── LoadingForm ───────────────────────────────────────────
            this.AutoScaleDimensions =
                new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode =
                System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor =
                System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(764, 364);
            this.Controls.Add(this.lblInstruction2);
            this.Controls.Add(this.pictureBoxLoading);
            this.Controls.Add(this.lblLoading);
            this.FormBorderStyle =
                System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadingForm";
            this.StartPosition =
                System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Send to TotalAgility";
            this.Load += new System.EventHandler(this.LoadingForm_Load);

            ((System.ComponentModel.ISupportInitialize)
                (this.pictureBoxLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // Field names must match LoadingForm.cs references exactly
        private System.Windows.Forms.PictureBox pictureBoxLoading;
        private System.Windows.Forms.Label lblLoading;
        private System.Windows.Forms.Label lblInstruction2;
    }
}