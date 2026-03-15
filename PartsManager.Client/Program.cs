using System;
using System.Windows.Forms;

namespace PartsManager.Client
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 初始化語系
            string lang = GlobalSettings.Language;
            PartsManager.Shared.Resources.LocalizationService.SetLanguage(lang);

            while (true)
            {
                UserSession.Clear(); // 每次迴圈重置 Session
                LoginForm login = new LoginForm();
                if (login.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainForm());
                }
                else
                {
                    break;
                }
            }
        }
    }
}

