using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;
using PartsManager.Client.Services;

namespace PartsManager.Client
{
    public partial class InventoryForm : Form
    {
        private readonly ApiClient _apiClient;
        private List<InventoryCheckItemDto> _scannedItems = new List<InventoryCheckItemDto>();

        public InventoryForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this);
            _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);
            
            this.Load += InventoryForm_Load;
        }

        private async void InventoryForm_Load(object sender, EventArgs e)
        {
            await LoadWarehousesAsync();
            txtBarcode.Focus();
        }

        private async System.Threading.Tasks.Task LoadWarehousesAsync()
        {
            try
            {
                var warehouses = await _apiClient.GetWarehousesAsync();
                cmbWarehouse.DataSource = warehouses;
                cmbWarehouse.DisplayMember = "WarehouseName";
                cmbWarehouse.ValueMember = "WarehouseID";
                cmbWarehouse.SelectedValue = GlobalSettings.DefaultWarehouseId;
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_LoadWarehouseError") + ex.Message);
            }
        }

        private async void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                string barcode = txtBarcode.Text.Trim();
                if (string.IsNullOrEmpty(barcode)) return;

                await ProcessBarcode(barcode);
                txtBarcode.Clear();
                txtBarcode.Focus();
            }
        }

        private async System.Threading.Tasks.Task ProcessBarcode(string barcode)
        {
            try
            {
                var materialInfo = await _apiClient.GetScanResultAsync(barcode);
                if (materialInfo == null)
                {
                    MessageBox.Show(LocalizationService.GetString("Msg_BarcodeNotFound") ?? "Barcode not found.", 
                        LocalizationService.GetString("Common_Error") ?? "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lblMaterialName.Text = materialInfo.Name;
                lblSpec.Text = materialInfo.Specification;

                var existing = _scannedItems.FirstOrDefault(i => i.PartNo == materialInfo.PartNo);
                if (existing != null)
                {
                    string msg = (LocalizationService.GetString("Msg_BarcodeAlreadyScanned") ?? "Item {0} already scanned. Qty: {1}. Edit?")
                                 .Replace("{0}", materialInfo.PartNo).Replace("{1}", existing.ScannedQty.ToString());

                    if (MessageBox.Show(msg, LocalizationService.GetString("Common_Confirm") ?? "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string input = PromptBox.ShowDialog(LocalizationService.GetString("Msg_EnterNewQty") ?? "Enter new quantity:", 
                                                           LocalizationService.GetString("Common_Edit") ?? "Edit", 
                                                           existing.ScannedQty.ToString());

                        if (decimal.TryParse(input, out decimal newQty))
                        {
                            existing.ScannedQty = newQty;
                            existing.ScanTime = DateTime.Now;
                            RefreshGrid();
                        }
                    }
                }
                else
                {
                    string input = PromptBox.ShowDialog(LocalizationService.GetString("Msg_EnterQty") ?? "Enter quantity:", 
                                                       LocalizationService.GetString("Common_Input") ?? "Input", 
                                                       "1");

                    if (decimal.TryParse(input, out decimal qty))
                    {
                        _scannedItems.Add(new InventoryCheckItemDto
                        {
                            PartNo = materialInfo.PartNo,
                            ScannedQty = qty,
                            ScanTime = DateTime.Now
                        });
                        RefreshGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RefreshGrid()
        {
            dgvScanned.Rows.Clear();
            foreach (var item in _scannedItems.OrderByDescending(i => i.ScanTime))
            {
                dgvScanned.Rows.Add(item.PartNo, item.ScannedQty, item.ScanTime.ToString("HH:mm:ss"));
            }
            lblCountInfo.Text = (LocalizationService.GetString("Label_ScannedCount") ?? "Scanned: {0}")
                                .Replace("{0}", _scannedItems.Count.ToString());
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(LocalizationService.GetString("Msg_ConfirmClear") ?? "Clear list?", 
                LocalizationService.GetString("Common_Confirm") ?? "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _scannedItems.Clear();
                RefreshGrid();
                lblMaterialName.Text = "---";
                lblSpec.Text = "---";
            }
        }

        private async void btnCompare_Click(object sender, EventArgs e)
        {
            if (_scannedItems.Count == 0)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_NoItemsScanned") ?? "No items scanned.");
                return;
            }

            try
            {
                var request = new InventoryCheckRequestDto
                {
                    WarehouseId = (int)cmbWarehouse.SelectedValue,
                    Items = _scannedItems
                };

                var report = await _apiClient.CompareInventoryAsync(request);
                var reportForm = new InventoryReportForm(report);
                reportForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    // 簡易輸入視窗取代 Microsoft.VisualBasic.Interaction.InputBox
    public static class PromptBox
    {
        public static string ShowDialog(string text, string caption, string defaultValue = "")
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text, Width = 300, AutoSize = true };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 300, Text = defaultValue, Font = new Font("微軟正黑體", 12) };
            Button confirmation = new Button() { Text = "OK", Left = 250, Width = 100, Top = 90, DialogResult = DialogResult.OK, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 204) };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : defaultValue;
        }
    }
}
