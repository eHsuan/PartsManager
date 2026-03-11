using System;
using System.Drawing;
using System.Windows.Forms;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public class LowStockWarningForm : Form
    {
        private Label lblMessage;
        private Button btnAcknowledge;

        public LowStockWarningForm(string message)
        {
            InitializeComponent();
            lblMessage.Text = message;
            
            // 語系套用
            string btnText = LocalizationService.GetString("UI_Acknowledge");
            if (!string.IsNullOrEmpty(btnText))
            {
                btnAcknowledge.Text = btnText;
            }
        }

        private void InitializeComponent()
        {
            this.lblMessage = new Label();
            this.btnAcknowledge = new Button();
            this.SuspendLayout();

            // Form
            this.BackColor = Color.Firebrick;
            this.ClientSize = new Size(600, 400);
            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.TopMost = true;

            // lblMessage
            this.lblMessage.Dock = DockStyle.Fill;
            this.lblMessage.Font = new Font("Microsoft JhengHei UI", 24F, FontStyle.Bold);
            this.lblMessage.ForeColor = Color.White;
            this.lblMessage.Location = new Point(0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Padding = new Padding(30);
            this.lblMessage.Size = new Size(600, 300);
            this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;

            // btnAcknowledge
            this.btnAcknowledge.BackColor = Color.White;
            this.btnAcknowledge.Dock = DockStyle.Bottom;
            this.btnAcknowledge.FlatStyle = FlatStyle.Flat;
            this.btnAcknowledge.Font = new Font("Microsoft JhengHei UI", 18F, FontStyle.Bold);
            this.btnAcknowledge.ForeColor = Color.Firebrick;
            this.btnAcknowledge.Location = new Point(0, 300);
            this.btnAcknowledge.Name = "btnAcknowledge";
            this.btnAcknowledge.Size = new Size(600, 100);
            this.btnAcknowledge.Text = "我知道了 (Acknowledge)";
            this.btnAcknowledge.UseVisualStyleBackColor = false;
            this.btnAcknowledge.Click += new EventHandler(this.btnAcknowledge_Click);

            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnAcknowledge);
            this.ResumeLayout(false);
        }

        private void btnAcknowledge_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static void ShowAlert(string message)
        {
            using (var form = new LowStockWarningForm(message))
            {
                form.ShowDialog();
            }
        }
    }
}
