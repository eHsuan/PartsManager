using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public partial class InboundForm : Form
    {
        private readonly ApiClient _apiClient;

        public InboundForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this); // 套用語系
            _apiClient = new ApiClient(ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "http://localhost:5000/");
            
            // 根據全域設定隱藏列印按鈕
            btnPrintLabel.Visible = GlobalSettings.EnableLabelPrinting;
        }

        private async void InboundForm_Load(object sender, EventArgs e)
        {
            btnPrintLabel.Visible = GlobalSettings.EnableLabelPrinting;
            await LoadWarehousesAsync();
        }

        private async System.Threading.Tasks.Task LoadWarehousesAsync()
        {
            try
            {
                var warehouses = await _apiClient.GetWarehousesAsync();
                
                var displayList = warehouses.Select(w => new 
                { 
                    w.WarehouseID, 
                    DisplayName = $"{w.WarehouseCode} - {w.WarehouseName}" 
                }).ToList();

                cmbWarehouse.DataSource = displayList;
                cmbWarehouse.DisplayMember = "DisplayName";
                cmbWarehouse.ValueMember = "WarehouseID";

                string defaultIdStr = ConfigurationManager.AppSettings["DefaultWarehouseId"];
                if (int.TryParse(defaultIdStr, out int defaultId))
                {
                    cmbWarehouse.SelectedValue = defaultId;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_LoadWarehouseError") + ex.Message, 
                    LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                string barcode = txtBarcode.Text.Trim().ToLower();
                if (string.IsNullOrEmpty(barcode)) return;

                await PerformQuery(barcode);
            }
        }

        private async System.Threading.Tasks.Task PerformQuery(string barcode)
        {
            lblStatus.Text = LocalizationService.GetString("Status_InboundSearching");
            lblStatus.ForeColor = Color.Cyan;
            
            btnPrintLabel.Enabled = false;

            try
            {
                var info = await _apiClient.GetInventoryAsync(barcode);
                if (info != null)
                {
                    lblMaterialName.Text = info.Name;
                    lblSpecification.Text = LocalizationService.GetString("Label_PartNoPrefix") + info.PartNo;
                    
                    lblStatus.Text = LocalizationService.GetString("Status_IdentifySuccess");
                    lblStatus.ForeColor = Color.Lime;
                    
                    txtBarcode.Text = barcode;
                    btnPrintLabel.Enabled = true;

                    txtQty.Focus();
                    txtQty.SelectAll();
                }
            }
            catch (Exception)
            {
                Console.Beep();
                lblStatus.Text = LocalizationService.GetString("Status_NotFound");
                lblStatus.ForeColor = Color.Red;
                lblMaterialName.Text = "--";
                lblSpecification.Text = "--";
            }
        }

        private async void btnManualInput_Click(object sender, EventArgs e)
        {
            string promptMsg = LocalizationService.GetString("Dialog_ManualInputLabel");
            string promptTitle = LocalizationService.GetString("Dialog_ManualInputTitle");
            
            string input = Prompt.ShowDialog(promptMsg, promptTitle);
            if (string.IsNullOrWhiteSpace(input)) return;

            try 
            {
                var info = await _apiClient.GetInventoryAsync(input);
                lblMaterialName.Text = info.Name;
                lblSpecification.Text = LocalizationService.GetString("Label_PartNoPrefix") + info.PartNo;
                lblStatus.Text = LocalizationService.GetString("Status_IdentifySuccess");
                lblStatus.ForeColor = Color.Lime;
                txtBarcode.Text = input;
                btnPrintLabel.Enabled = true;
                MessageBox.Show(LocalizationService.GetString("Msg_SearchSuccess"), 
                    LocalizationService.GetString("Common_Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                string notFoundMsg = string.Format(LocalizationService.GetString("Msg_NotFoundCreate"), input);
                DialogResult dr = MessageBox.Show(notFoundMsg, 
                    LocalizationService.GetString("Dialog_NotFoundTitle"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    var form = new MaterialCreationForm();
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        await PerformQuery(input); 
                    }
                }
            }
        }

        private void btnPrintLabel_Click(object sender, EventArgs e)
        {
            string barcode = txtBarcode.Text.Trim();
            string name = lblMaterialName.Text;
            if (string.IsNullOrEmpty(barcode) || name == "--") return;

            MessageBox.Show($"{LocalizationService.GetString("Label_ReprintHeader")}\n----------------\n{barcode}\n{name}\n----------------", 
                LocalizationService.GetString("Common_Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnInbound_Click(object sender, EventArgs e)
        {
            string barcode = txtBarcode.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(barcode))
            {
                MessageBox.Show(LocalizationService.GetString("Label_ScanBarcode"));
                return;
            }

            if (!decimal.TryParse(txtQty.Text, out decimal qty) || qty <= 0)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_InputCorrectQty"));
                return;
            }

            if (cmbWarehouse.SelectedValue == null) return;
            
            int targetWarehouseId = (int)cmbWarehouse.SelectedValue;

            btnInbound.Enabled = false;
            lblStatus.Text = LocalizationService.GetString("Status_InboundProcessing");

            try
            {
                var dto = new InboundDto
                {
                    WarehouseId = targetWarehouseId,
                    Barcode = barcode,
                    Quantity = qty,
                    OperatorId = UserSession.Username
                };

                bool success = await _apiClient.PostInboundAsync(dto);
                if (success)
                {
                    lblStatus.Text = string.Format(LocalizationService.GetString("Status_InboundSuccess"), barcode);
                    lblStatus.ForeColor = Color.Lime;
                    
                    txtBarcode.Clear();
                    txtQty.Text = "1";
                    lblMaterialName.Text = "--";
                    lblSpecification.Text = "--";
                    btnPrintLabel.Enabled = false;
                    txtBarcode.Focus();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = string.Format(LocalizationService.GetString("Status_InboundFailed"), ex.Message);
                lblStatus.ForeColor = Color.Red;
            }
            finally
            {
                btnInbound.Enabled = true;
            }
        }

    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form() 
            {
                Width = 400,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };
            Label textLabel = new Label() { Left = 30, Top = 20, Text = text, AutoSize = true, Font = new Font("Microsoft JhengHei", 10) };
            TextBox textBox = new TextBox() { Left = 30, Top = 50, Width = 320, Font = new Font("Consolas", 12) };
            Button confirmation = new Button() { Text = LocalizationService.GetString("Btn_Confirm"), Left = 250, Width = 100, Top = 90, DialogResult = DialogResult.OK, Font = new Font("Microsoft JhengHei", 10) };
            
            confirmation.Click += (sender, e) => { prompt.Close(); };
            
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
