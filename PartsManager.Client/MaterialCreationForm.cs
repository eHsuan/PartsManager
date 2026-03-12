using System;
using System.Configuration;
using System.Windows.Forms;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public partial class MaterialCreationForm : Form
    {
        private readonly ApiClient _apiClient;
        private int? _materialId = null;

        public MaterialCreationForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this); // 套用語系
            string baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "http://localhost:5000/";
            _apiClient = new ApiClient(baseUrl);
            this.Load += MaterialCreationForm_Load;
        }

        public MaterialCreationForm(int materialId) : this()
        {
            _materialId = materialId;
            // 編輯模式的標題會被 I18nHelper 覆蓋，若需要特殊處理需在此設定
            this.Text = LocalizationService.GetString("MaterialEditForm");
            btnSave.Text = LocalizationService.GetString("Btn_Save");
        }

        private async void MaterialCreationForm_Load(object sender, EventArgs e)
        {
            if (_materialId.HasValue)
            {
                try
                {
                    var material = await _apiClient.GetMaterialAsync(_materialId.Value);
                    if (material != null)
                    {
                        txtPartNo.Text = material.PartNo;
                        txtName.Text = material.Name;
                        txtSpec.Text = material.Specification;
                        txtStation.Text = material.Station;
                        txtSafeStock.Text = material.SafeStockQty.ToString();
                        txtLeadTime.Text = material.LeadTimeDays.ToString();
                        
                        txtPartNo.Enabled = false; 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(LocalizationService.GetString("Msg_LoadMaterialError") + ex.Message, 
                        LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPartNo.Text))
            {
                MessageBox.Show(LocalizationService.GetString("Msg_PartNoRequired"));
                return;
            }
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(LocalizationService.GetString("Msg_NameRequired"));
                return;
            }
            
            if (!int.TryParse(txtSafeStock.Text, out int safeStock)) safeStock = 0;
            if (!int.TryParse(txtLeadTime.Text, out int leadTime)) leadTime = 0;

            btnSave.Enabled = false;

            try
            {
                if (_materialId.HasValue)
                {
                    var dto = new UpdateMaterialDto
                    {
                        PartNo = txtPartNo.Text.Trim(),
                        Name = txtName.Text.Trim(),
                        Specification = txtSpec.Text.Trim(),
                        Station = txtStation.Text.Trim(),
                        SafeStockQty = safeStock,
                        LeadTimeDays = leadTime
                    };
                    await _apiClient.UpdateMaterialAsync(_materialId.Value, dto);
                    MessageBox.Show(LocalizationService.GetString("Msg_SaveSuccess"), 
                        LocalizationService.GetString("Common_Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var dto = new CreateMaterialDto
                    {
                        PartNo = txtPartNo.Text.Trim(),
                        Name = txtName.Text.Trim(),
                        Specification = txtSpec.Text.Trim(),
                        Station = txtStation.Text.Trim(),
                        SafeStockQty = safeStock,
                        LeadTimeDays = leadTime,
                        SourceType = 1 
                    };
                    var result = await _apiClient.CreateMaterialAsync(dto);
                    string successMsg = string.Format(LocalizationService.GetString("Msg_CreateSuccess"), result.PartNo, result.BarCode);
                    MessageBox.Show(successMsg, LocalizationService.GetString("Common_Success"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    if (GlobalSettings.EnableLabelPrinting)
                    {
                        PrintLabel(result.BarCode, result.Name);
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_SaveError") + ex.Message, 
                    LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PrintLabel(string barcode, string name)
        {
            // 標籤列印模擬
            MessageBox.Show($"{LocalizationService.GetString("Label_ReprintHeader")}\n----------------\n{barcode}\n{name}\n----------------", 
                LocalizationService.GetString("Common_Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
