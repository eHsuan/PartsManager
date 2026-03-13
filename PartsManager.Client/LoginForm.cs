using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public partial class LoginForm : Form
    {
        private readonly ApiClient _apiClient;

        public LoginForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this); // 套用語系
            _apiClient = new ApiClient(ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "http://localhost:5000/");
        }

        private async void LoginForm_Load(object sender, EventArgs e)
        {
            await CheckConnectionAsync();
            
            var timer = new Timer();
            timer.Interval = 5000;
            timer.Tick += async (s, ev) => await CheckConnectionAsync();
            timer.Start();
        }

        private async System.Threading.Tasks.Task CheckConnectionAsync()
        {
            try
            {
                // 簡單呼叫一個輕量級 API 作為健康檢查
                await _apiClient.GetWarehousesAsync();
                pnlStatus.BackColor = UIStyle.StatusOkColor;
                lblStatus.Text = LocalizationService.GetString("Msg_ServerOnline");
                lblStatus.ForeColor = Color.White;
            }
            catch
            {
                pnlStatus.BackColor = UIStyle.StatusErrorColor;
                lblStatus.Text = LocalizationService.GetString("Msg_ServerOffline");
                lblStatus.ForeColor = UIStyle.StatusErrorColor;
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(LocalizationService.GetString("Msg_InputRequired"), 
                                LocalizationService.GetString("Common_Info"), 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnLogin.Enabled = false;
            try
            {
                var user = await _apiClient.LoginAsync(username, password);
                if (user != null)
                {
                    UserSession.UserID = user.UserID;
                    UserSession.Username = user.Username;
                    UserSession.UserLevel = user.UserLevel;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                // 檢查是否為 401 Unauthorized (不論是在 Message 中還是透過 StatusCode)
                bool isAuthError = ex.Message.Contains("401") || ex.Message.Contains("Unauthorized");

                string errorMsg = isAuthError 
                    ? LocalizationService.GetString("Msg_LoginFailed") 
                    : ex.Message;

                MessageBox.Show(errorMsg, 
                                LocalizationService.GetString("Common_Error"), 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
