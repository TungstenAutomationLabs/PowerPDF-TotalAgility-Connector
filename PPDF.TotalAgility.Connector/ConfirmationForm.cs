using System;
using System.Drawing;
using System.Windows.Forms;

namespace PPDF.TotalAgility.Connector
{
    /// <summary>
    /// Confirmation A dialog shown before sending a PDF to TotalAgility.
    ///
    /// Displays:
    ///   Send "[DocumentName]" to process "[ProcessName]"?
    ///   [ Send ]   [ Change ]   [ Cancel ]
    ///   ☐ Do not show this message again
    ///
    /// Result property indicates which button was clicked.
    /// DoNotShowAgain property reflects the checkbox state.
    /// </summary>
    public partial class ConfirmationForm : Form
    {
        public enum ConfirmationResult { Send, Change, Cancel }

        public ConfirmationResult Result { get; private set; } = ConfirmationResult.Cancel;
        public bool DoNotShowAgain => checkBoxDoNotShow.Checked;

        public ConfirmationForm(string documentName, string processName)
        {
            InitializeComponent();
            lblMessage.Text = $"Send \"{documentName}\" to process \"{processName}\"?";
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Result = ConfirmationResult.Send;
            Close();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            Result = ConfirmationResult.Change;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Result = ConfirmationResult.Cancel;
            Close();
        }

        
    }
}
