using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public partial class ProgressForm : Form
    {
        public ProgressForm(string message)
        {
            InitializeComponent();
            UIStyle.Apply(this);
            this.Text = LocalizationService.GetString("Common_Info");
            lblMessage.Text = message;
        }

        public static async Task ShowLoading(Form owner, string message, Func<Task> action)
        {
            using (var pf = new ProgressForm(message))
            {
                // 當視窗載入完成後執行非同步任務
                pf.Load += async (s, e) =>
                {
                    try
                    {
                        await action();
                    }
                    finally
                    {
                        pf.Close();
                    }
                };

                pf.ShowDialog(owner);
            }
        }
    }
}
