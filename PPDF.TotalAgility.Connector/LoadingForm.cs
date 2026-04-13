using System;
using System.Drawing;
using System.Windows.Forms;

namespace PPDF.TotalAgility.Connector
{
    /// <summary>
    /// Modal "please wait" form shown while the PDF is being
    /// submitted to TotalAgility via CreateJobWithDocuments.
    ///
    /// Runs on its own STA thread (Application.Run) so it stays
    /// responsive while HTTP calls block the connector thread.
    /// Closed via Invoke() from the connector thread when the
    /// API call completes or fails.
    /// </summary>
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            CenterControls();
            // Use Image_Save as the loading icon — conveys "sending/saving to TotalAgility"
            // Replace with a custom animated GIF when available
            pictureBoxLoading.Image = Resources.Resources.Image_Save;
        }

        /// <summary>
        /// Centres the icon and labels vertically and horizontally
        /// within the form client area regardless of font scaling.
        /// </summary>
        private void CenterControls()
        {
            int spacing = 12;
            int totalHeight = pictureBoxLoading.Height
                            + spacing + lblLoading.Height
                            + spacing + lblInstruction2.Height;

            int maxWidth = Math.Max(pictureBoxLoading.Width,
                           Math.Max(lblLoading.Width, lblInstruction2.Width));

            int startX = (ClientSize.Width - maxWidth) / 2;
            int startY = (ClientSize.Height - totalHeight) / 2;

            pictureBoxLoading.Location = new Point(
                startX + (maxWidth - pictureBoxLoading.Width) / 2, startY);

            lblLoading.Location = new Point(
                startX + (maxWidth - lblLoading.Width) / 2,
                pictureBoxLoading.Bottom + spacing);

            lblInstruction2.Location = new Point(
                startX + (maxWidth - lblInstruction2.Width) / 2,
                lblLoading.Bottom + spacing);
        }
    }
}