using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public partial class UserManagementForm : Form
    {
        private readonly ApiClient _apiClient;

        public UserManagementForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this); // 套用語系
            _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);
        }

        private async void UserManagementForm_Load(object sender, EventArgs e)
        {
            await LoadUsers();
        }

        private async System.Threading.Tasks.Task LoadUsers()
        {
            try
            {
                var users = await _apiClient.GetUsersAsync();
                dgvUsers.DataSource = users;
                
                // 資料載入後重新翻譯一次，以處理動態生成的欄位標題
                I18nHelper.Apply(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            var username = txtNewUsername.Text.Trim();
            var password = txtNewPassword.Text.Trim();
            if (!int.TryParse(txtNewLevel.Text, out int level)) level = 4;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show(LocalizationService.GetString("Msg_InputRequired"));
                return;
            }

            try
            {
                var dto = new CreateUserDto { Username = username, Password = password, UserLevel = level };
                await _apiClient.CreateUserAsync(dto, UserSession.UserLevel);
                txtNewUsername.Clear();
                txtNewPassword.Clear();
                await LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count > 0)
            {
                var user = dgvUsers.SelectedRows[0].DataBoundItem as UserDto;
                if (user == null) return;

                string confirmMsg = string.Format(LocalizationService.GetString("Msg_DeleteUserConfirm"), user.Username);
                if (MessageBox.Show(confirmMsg, LocalizationService.GetString("Menu_Delete"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        await _apiClient.DeleteUserAsync(user.UserID, UserSession.UserLevel);
                        await LoadUsers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
