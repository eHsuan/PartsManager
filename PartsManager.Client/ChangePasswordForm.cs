using System;
using System.Windows.Forms;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;
using PartsManager.Client.Services;

namespace PartsManager.Client
{
    public partial class ChangePasswordForm : Form
    {
        private readonly ApiClient _apiClient;

        public ChangePasswordForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this);
            _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            string oldPwd = txtOldPassword.Text.Trim();
            string newPwd = txtNewPassword.Text.Trim();
            string confirmPwd = txtConfirmPassword.Text.Trim();

            if (string.IsNullOrEmpty(oldPwd) || string.IsNullOrEmpty(newPwd))
            {
                MessageBox.Show(LocalizationService.GetString("Msg_InputRequired"), 
                    LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPwd != confirmPwd)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_PasswordMismatch"), 
                    LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var dto = new ChangePasswordDto
                {
                    Username = UserSession.Username,
                    OldPassword = oldPwd,
                    NewPassword = newPwd
                };

                await _apiClient.ChangePasswordAsync(dto);

                MessageBox.Show(LocalizationService.GetString("Msg_PasswordChanged"), 
                    LocalizationService.GetString("Common_Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
