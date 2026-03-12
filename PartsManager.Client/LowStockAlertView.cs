using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using PartsManager.Shared.Resources;
using PartsManager.Shared.DTOs;
using System.Threading.Tasks;

namespace PartsManager.Client
{
    public class LowStockAlertView : Form
    {
        private DataGridView dgvLowStock;
        private Button btnRefresh;
        private ApiClient _api;
        private Panel pnlHeader;
        private Label lblTitle;

        public LowStockAlertView(ApiClient api)
        {
            _api = api;
            InitializeComponent();
            this.Load += LowStockAlertView_Load;
            this.dgvLowStock.CellFormatting += DgvLowStock_CellFormatting;
            this.Tag = "UI_LowStockDashboard";
            I18nHelper.Apply(this);
        }

        private void InitializeComponent()
        {
            this.dgvLowStock = new DataGridView();
            this.btnRefresh = new Button();
            this.pnlHeader = new Panel();
            this.lblTitle = new Label();
            
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLowStock)).BeginInit();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.BackColor = Color.Firebrick;
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.btnRefresh);
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 60;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Microsoft JhengHei UI", 16F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Location = new Point(15, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Tag = "UI_LowStockDashboard";
            this.lblTitle.Text = "全廠低水位看板";

            // btnRefresh
            this.btnRefresh.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btnRefresh.BackColor = Color.White;
            this.btnRefresh.FlatStyle = FlatStyle.Flat;
            this.btnRefresh.Location = new Point(680, 15);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(100, 30);
            this.btnRefresh.Tag = "UI_Refresh";
            this.btnRefresh.Text = "重新整理";
            this.btnRefresh.Click += async (s, e) => await LoadData();

            // dgvLowStock
            this.dgvLowStock.AutoGenerateColumns = false;
            this.dgvLowStock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLowStock.BackgroundColor = Color.White;
            this.dgvLowStock.Dock = DockStyle.Fill;
            this.dgvLowStock.ReadOnly = true;
            this.dgvLowStock.AllowUserToAddRows = false;
            this.dgvLowStock.RowTemplate.Height = 35;
            this.dgvLowStock.BorderStyle = BorderStyle.None;
            this.dgvLowStock.Location = new Point(0, 60);

            // Columns
            this.dgvLowStock.Columns.Clear();
            this.dgvLowStock.Columns.Add(new DataGridViewTextBoxColumn { Name = "Col_PartNo", DataPropertyName = "PartNo", HeaderText = "料號" });
            this.dgvLowStock.Columns.Add(new DataGridViewTextBoxColumn { Name = "Col_Name", DataPropertyName = "Name", HeaderText = "品名" });
            this.dgvLowStock.Columns.Add(new DataGridViewTextBoxColumn { Name = "Col_Spec", DataPropertyName = "Specification", HeaderText = "規格" });
            this.dgvLowStock.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "Col_Qty", 
                DataPropertyName = "TotalQuantity", 
                HeaderText = "總庫存",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } 
            });
            this.dgvLowStock.Columns.Add(new DataGridViewTextBoxColumn 
            { 
                Name = "Col_SafeStock", 
                DataPropertyName = "SafeStockQty", 
                HeaderText = "安全水位",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" } 
            });

            // Form
            this.ClientSize = new Size(800, 600);
            this.Controls.Add(this.dgvLowStock);
            this.Controls.Add(this.pnlHeader);
            this.Text = "Low Stock Alert";
            this.StartPosition = FormStartPosition.CenterScreen;

            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLowStock)).EndInit();
            this.ResumeLayout(false);
        }

        private async void LowStockAlertView_Load(object sender, EventArgs e)
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                var list = await _api.GetLowStockAsync();
                dgvLowStock.DataSource = null;
                dgvLowStock.DataSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("載入失敗: " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void DgvLowStock_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // 將所有列背景標示為淺紅色，文字深紅色
            dgvLowStock.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.MistyRose;
            dgvLowStock.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkRed;
            dgvLowStock.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Firebrick;
            dgvLowStock.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.White;
        }
    }
}
