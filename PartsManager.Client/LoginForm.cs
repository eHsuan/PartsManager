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
        private ApiClient _apiClient;

        public LoginForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this); // 套用語系
            _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);
            
            // 初始化設定欄位
            txtServerIP.Text = GlobalSettings.ServerIP;
            txtServerPort.Text = GlobalSettings.ServerPort;
        }

        private async void LoginForm_Load(object sender, EventArgs e)
        {
            await CheckConnectionAsync();
            
            var timer = new Timer();
            timer.Interval = 10000; // 延長至 10 秒檢查一次
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
                lblStatus.ForeColor = Color.DimGray;
            }
            catch
            {
                pnlStatus.BackColor = UIStyle.StatusErrorColor;
                lblStatus.Text = LocalizationService.GetString("Msg_ServerOffline");
                lblStatus.ForeColor = UIStyle.StatusErrorColor;
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            // 切換面板顯示與視窗大小
            bool isExpanded = !pnlSettings.Visible;
            pnlSettings.Visible = isExpanded;
            this.Height = isExpanded ? 520 : 360;
        }

        private async void btnReconnect_Click(object sender, EventArgs e)
        {
            string ip = txtServerIP.Text.Trim();
            string port = txtServerPort.Text.Trim();

            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(port)) return;

            // 1. 儲存到設定檔
            GlobalSettings.ServerIP = ip;
            GlobalSettings.ServerPort = port;

            // 2. 重新建立 ApiClient (測試連線用短逾時 5s)
            var testClient = new ApiClient(GlobalSettings.ApiBaseUrl, 5);

            // 3. 測試連線
            lblStatus.Text = "Testing Connection...";
            try
            {
                await testClient.GetWarehousesAsync();
                
                // 測試成功才更新正式的 _apiClient (預設 30s 逾時)
                _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);

                MessageBox.Show(LocalizationService.GetString("Msg_ConnectSuccess"), 
                    LocalizationService.GetString("Common_Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                // 自動縮回設定面板
                pnlSettings.Visible = false;
                this.Height = 360;
                await CheckConnectionAsync();
            }
            catch
            {
                MessageBox.Show(LocalizationService.GetString("Msg_ConnectFailed"), 
                    LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                await CheckConnectionAsync();
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
