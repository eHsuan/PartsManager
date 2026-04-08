using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;
using PartsManager.Client.Services;

namespace PartsManager.Client
{
    public partial class MachineManagementForm : Form
    {
        private readonly ApiClient _apiClient;
        private List<MachineDto> _machines;
        private int? _selectedMachineId = null;

        public MachineManagementForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this);
            _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);
        }

        private async void MachineManagementForm_Load(object sender, EventArgs e)
        {
            await LoadMachinesAsync();
        }

        private async System.Threading.Tasks.Task LoadMachinesAsync()
        {
            try
            {
                _machines = await _apiClient.GetMachinesAsync();
                dgvMachines.DataSource = null;
                dgvMachines.DataSource = _machines;
                
                // 設定欄位標題 (若 I18nHelper 沒自動處理 Grid)
                if (dgvMachines.Columns["MachineID"] != null) dgvMachines.Columns["MachineID"].Visible = false;
                if (dgvMachines.Columns["MachineCode"] != null) dgvMachines.Columns["MachineCode"].HeaderText = LocalizationService.GetString("Label_MachineCode");
                if (dgvMachines.Columns["MachineName"] != null) dgvMachines.Columns["MachineName"].HeaderText = LocalizationService.GetString("Label_MachineName");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Machines Error: " + ex.Message);
            }
        }

        private void dgvMachines_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMachines.SelectedRows.Count > 0)
            {
                var machine = (MachineDto)dgvMachines.SelectedRows[0].DataBoundItem;
                _selectedMachineId = machine.MachineID;
                txtMachineCode.Text = machine.MachineCode;
                txtMachineName.Text = machine.MachineName;
                btnDelete.Enabled = true;
            }
            else
            {
                ClearInputs();
            }
        }

        private void ClearInputs()
        {
            _selectedMachineId = null;
            txtMachineCode.Clear();
            txtMachineName.Clear();
            btnDelete.Enabled = false;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!_selectedMachineId.HasValue)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_SelectMachineToUpdate"));
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMachineCode.Text))
            {
                MessageBox.Show(LocalizationService.GetString("Msg_MachineCodeRequired"));
                return;
            }

            try
            {
                var dto = new UpdateMachineDto { MachineCode = txtMachineCode.Text.Trim(), MachineName = txtMachineName.Text.Trim() };
                await _apiClient.UpdateMachineAsync(_selectedMachineId.Value, dto);

                MessageBox.Show(LocalizationService.GetString("Msg_MachineSaveSuccess"));
                ClearInputs();
                await LoadMachinesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save Machine Error: " + ex.Message);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_selectedMachineId.HasValue) return;

            string confirmMsg = string.Format(LocalizationService.GetString("Msg_MachineDeleteConfirm"), txtMachineCode.Text);
            if (MessageBox.Show(confirmMsg, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    await _apiClient.DeleteMachineAsync(_selectedMachineId.Value);
                    ClearInputs();
                    await LoadMachinesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Delete Machine Error: " + ex.Message);
                }
            }
        }

        private async void btnAddMachine_Click(object sender, EventArgs e)
        {
            string code = txtMachineCode.Text.Trim();
            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show(LocalizationService.GetString("Msg_MachineCodeRequired"));
                return;
            }

            // 防呆：檢查 MachineCode 是否已存在
            if (_machines != null && _machines.Exists(m => m.MachineCode.Equals(code, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show(string.Format(LocalizationService.GetString("Msg_MachineCodeDuplicate"), code),
                    LocalizationService.GetString("Common_Info"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var dto = new CreateMachineDto { MachineCode = code, MachineName = txtMachineName.Text.Trim() };
                await _apiClient.CreateMachineAsync(dto);

                MessageBox.Show(LocalizationService.GetString("Msg_MachineSaveSuccess"));
                ClearInputs();
                await LoadMachinesAsync();
                dgvMachines.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add Machine Error: " + ex.Message);
            }
        }
    }
}
