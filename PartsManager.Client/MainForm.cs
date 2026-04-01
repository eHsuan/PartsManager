using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public partial class MainForm : Form, IMessageFilter
    {
        private readonly ApiClient _apiClient;
        private MaterialStockInfoDto _currentMaterial;
        private Timer _idleTimer;
        private DateTime _lastActivity;
        private int _timeoutMinutes;

        public class WarehouseViewModel
        {
            public int Id { get; set; }
            public string Display { get; set; }
        }

        public MainForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this); // 套用語系
            InitializeNavigation();
            _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);
            
            this.Shown += MainForm_Shown;
            this.Load += MainForm_Load;
            
            cmbWarehouse.SelectedIndexChanged += (s, e) =>
            {
                 ResetInfo();
                 txtBarcode.Focus();
                 txtBarcode.SelectAll();
            };

            InitializeIdleTimer();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await CheckServerStatusAsync();
            var timer = new Timer();
            timer.Interval = 10000;
            timer.Tick += async (s, ev) => await CheckServerStatusAsync();
            timer.Start();
        }

        private async System.Threading.Tasks.Task CheckServerStatusAsync()
        {
            try
            {
                await _apiClient.GetWarehousesAsync();
                pnlServerStatus.BackColor = UIStyle.StatusOkColor;
                lblServerStatus.Text = LocalizationService.GetString("Msg_ServerOnline");
                lblServerStatus.ForeColor = Color.Lime;
            }
            catch
            {
                pnlServerStatus.BackColor = UIStyle.StatusErrorColor;
                lblServerStatus.Text = LocalizationService.GetString("Msg_ServerOffline");
                lblServerStatus.ForeColor = UIStyle.StatusErrorColor;
            }
        }

        private void InitializeIdleTimer()
        {
            _lastActivity = DateTime.Now;
            _timeoutMinutes = GlobalSettings.AutoLogoutMinutes;

            _idleTimer = new Timer();
            _idleTimer.Interval = 5000;
            _idleTimer.Tick += (s, e) =>
            {
                if ((DateTime.Now - _lastActivity).TotalMinutes >= _timeoutMinutes)
                {
                    _idleTimer.Stop();
                    Application.RemoveMessageFilter(this);
                    MessageBox.Show(LocalizationService.GetString("Msg_AutoLogout") ?? "Idle timeout, logging out.", "Auto Logout", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            };
            _idleTimer.Start();
            Application.AddMessageFilter(this);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x0200 || m.Msg == 0x0201 || m.Msg == 0x0202 || m.Msg == 0x0100 || m.Msg == 0x0101)
            {
                _lastActivity = DateTime.Now;
            }
            return false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _idleTimer?.Stop();
            Application.RemoveMessageFilter(this);
            base.OnFormClosing(e);
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            await LoadWarehousesAsync();
        }

        private async System.Threading.Tasks.Task LoadWarehousesAsync()
        {
            try
            {
                var warehouses = await _apiClient.GetWarehousesAsync();
                
                var displayList = warehouses.Select(w => new WarehouseViewModel
                { 
                    Id = w.WarehouseID, 
                    Display = $"{w.WarehouseCode} - {w.WarehouseName}" 
                }).ToList();

                cmbWarehouse.DisplayMember = "Display";
                cmbWarehouse.ValueMember = "Id";
                cmbWarehouse.DataSource = displayList;

                int defaultId = GlobalSettings.DefaultWarehouseId;
                cmbWarehouse.SelectedValue = defaultId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Warehouse Error: " + ex.Message);
            }
        }

        private void InitializeNavigation()
        {
            var navPanel = new Panel();
            navPanel.Dock = DockStyle.Top;
            navPanel.Height = 40;
            navPanel.Padding = new Padding(5);
            navPanel.BackColor = Color.FromArgb(45, 45, 48);

            // --- 建立功能按鈕 ---

            var btnInbound = CreateNavButton(LocalizationService.GetString("Menu_Inbound"), false);
            btnInbound.Visible = UserSession.UserLevel <= 3;
            btnInbound.Click += (s, e) => {
                var form = new InboundForm();
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
            };

            var btnQuery = CreateNavButton(LocalizationService.GetString("Menu_Query"), false);
            btnQuery.Visible = UserSession.UserLevel <= 3;
            btnQuery.Click += (s, e) => {
                var form = new QueryForm();
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
            };

            var btnLowStock = CreateNavButton(LocalizationService.GetString("Menu_LowStock"), false);
            btnLowStock.Visible = UserSession.UserLevel <= 3;
            btnLowStock.Click += (s, e) => {
                var form = new LowStockAlertView(_apiClient);
                form.Show();
            };

            var btnCreateMaterial = CreateNavButton(LocalizationService.GetString("Menu_CreateMaterial"), false);
            btnCreateMaterial.Visible = UserSession.UserLevel <= 2;
            btnCreateMaterial.Click += (s, e) => {
                var form = new MaterialCreationForm();
                form.ShowDialog();
            };

            var btnUserMgmt = CreateNavButton(LocalizationService.GetString("Menu_UserMgmt"), false);
            btnUserMgmt.Visible = UserSession.UserLevel == 1;
            btnUserMgmt.Click += (s, e) => {
                var form = new UserManagementForm();
                form.ShowDialog();
            };

            var btnBatchImport = CreateNavButton(LocalizationService.GetString("Menu_BatchImport"), false);
            btnBatchImport.Visible = UserSession.UserLevel <= 2;
            btnBatchImport.Click += (s, e) => {
                var form = new BatchImportForm();
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
            };

            var btnInventory = CreateNavButton(LocalizationService.GetString("Menu_Inventory"), false);
            btnInventory.Visible = UserSession.UserLevel <= 3;
            btnInventory.Click += (s, e) => {
                var form = new InventoryForm();
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Show();
            };

            // --- 系統設定按鈕 (點擊顯示 ContextMenu) ---
            var btnSettings = CreateNavButton(LocalizationService.GetString("Menu_Settings"), false);
            btnSettings.Dock = DockStyle.Right; // 置右
            btnSettings.Width = 150;
            
            var ctxSettings = new ContextMenuStrip();
            var itemEnablePrinting = new ToolStripMenuItem(LocalizationService.GetString("Setting_EnablePrinting"));
            itemEnablePrinting.CheckOnClick = true;
            itemEnablePrinting.Checked = GlobalSettings.EnableLabelPrinting;
            itemEnablePrinting.CheckedChanged += (s, e) => {
                GlobalSettings.EnableLabelPrinting = itemEnablePrinting.Checked;
            };
            ctxSettings.Items.Add(itemEnablePrinting);

            btnSettings.Click += (s, e) => {
                ctxSettings.Show(btnSettings, new Point(0, btnSettings.Height));
            };

            // --- 加入 Panel (順序：左到右) ---

            if (UserSession.UserLevel <= 3) navPanel.Controls.Add(btnInbound);
            if (UserSession.UserLevel <= 3) navPanel.Controls.Add(btnQuery);
            if (UserSession.UserLevel <= 3) navPanel.Controls.Add(btnLowStock);
            if (UserSession.UserLevel <= 3) navPanel.Controls.Add(btnInventory);
            if (UserSession.UserLevel <= 2) navPanel.Controls.Add(btnCreateMaterial);
            if (UserSession.UserLevel <= 2) navPanel.Controls.Add(btnBatchImport);
            if (UserSession.UserLevel == 1) navPanel.Controls.Add(btnUserMgmt);

            this.Controls.Add(navPanel);
            this.Text = LocalizationService.GetString("App_Title") + $" - {UserSession.Username}";
        }

        private Button CreateNavButton(string text, bool isActive)
        {
            var btn = new Button();
            btn.Text = text;
            btn.Dock = DockStyle.Left;
            btn.Width = 120; // 稍微增加寬度以適應德文
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.ForeColor = Color.White;
            btn.BackColor = isActive ? Color.FromArgb(0, 122, 204) : Color.Transparent;
            return btn;
        }

        private async void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await SearchMaterialAsync();
            }
        }

        private async System.Threading.Tasks.Task SearchMaterialAsync()
        {
            string barcode = txtBarcode.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(barcode)) return;

            try
            {
                lblStatus.Text = LocalizationService.GetString("Status_Searching");
                _currentMaterial = await _apiClient.GetInventoryAsync(barcode);
                DisplayMaterial(_currentMaterial);
                lblStatus.Text = LocalizationService.GetString("Status_Ready");
                lblStatus.ForeColor = Color.Cyan;
                txtQty.Focus();
                txtQty.SelectAll();
            }
            catch (Exception)
            {
                ResetInfo();
                lblStatus.Text = LocalizationService.GetString("Status_NotFound");
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void DisplayMaterial(MaterialStockInfoDto material)
        {
            lblMaterialName.Text = material.Name;
            lblSpecification.Text = material.Specification ?? "--";
            
            int warehouseId = (int)(cmbWarehouse.SelectedValue ?? 0);
            var stock = material.Stocks?.FirstOrDefault(s => s.WarehouseId == warehouseId);
            lblCurrentStock.Text = (stock?.Quantity ?? 0).ToString("N0");
        }

        private void ResetInfo()
        {
            _currentMaterial = null;
            lblMaterialName.Text = "--";
            lblSpecification.Text = "--";
            lblCurrentStock.Text = "0";
        }

        public async void NavigateToOutboundWithBarcode(string barcode)
        {
            this.BringToFront();
            txtBarcode.Text = barcode;
            await SearchMaterialAsync();
        }

        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            if (_currentMaterial == null) return;
            if (!decimal.TryParse(txtQty.Text, out decimal qty) || qty <= 0)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_InputCorrectQty"));
                return;
            }

            try
            {
                var dto = new OutboundDto
                {
                    Barcode = _currentMaterial.Barcode,
                    WarehouseId = (int)cmbWarehouse.SelectedValue,
                    Quantity = qty,
                    OperatorId = UserSession.Username
                };

                var result = await _apiClient.PostOutboundAsync(dto);
                if (result.IsSuccess)
                {
                    MessageBox.Show(LocalizationService.GetString("Status_OutboundSuccess"), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    if (result.IsLowStock)
                    {
                        string template = LocalizationService.GetString("Msg_LowStockAlert");
                        string lowStockMsg = string.Format(template, result.TotalQuantity.ToString("N0"), result.SafeStockQty);
                        LowStockWarningForm.ShowAlert(lowStockMsg);
                    }

                    txtBarcode.Clear();
                    ResetInfo();
                    txtBarcode.Focus();
                }
                else
                {
                    MessageBox.Show(LocalizationService.GetString("Msg_PostFailed") + result.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_PostFailed") + ex.Message);
            }
        }
    }
}
