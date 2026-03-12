using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Configuration;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public partial class QueryForm : Form
    {
        private readonly ApiClient _apiClient;

        public QueryForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this); // 套用語系
            
            // 選單項目手動翻譯
            menuEdit.Text = LocalizationService.GetString("Menu_Edit");
            menuDelete.Text = LocalizationService.GetString("Menu_Delete");

            string baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "http://localhost:5000/";
            _apiClient = new ApiClient(baseUrl);
        }

        private async void QueryForm_Load(object sender, EventArgs e)
        {
            await PerformSearch("");
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearchKeyword.Text.Trim();
            await PerformSearch(keyword);
        }

        private async void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearchKeyword.Clear();
            await PerformSearch("");
        }

        private async void txtSearchKeyword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                string keyword = txtSearchKeyword.Text.Trim();
                await PerformSearch(keyword);
            }
        }

        private async System.Threading.Tasks.Task PerformSearch(string keyword)
        {
            try
            {
                dgvResults.Cursor = Cursors.WaitCursor;
                btnSearch.Enabled = false;

                List<SparePartSearchResultDto> results = await _apiClient.SearchInventoryAsync(keyword);
                
                dgvResults.AutoGenerateColumns = false;
                dgvResults.DataSource = results;
            }
            catch (Exception ex)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_SearchError") + ex.Message, 
                    LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dgvResults.Cursor = Cursors.Default;
                btnSearch.Enabled = true;
            }
        }

        private void dgvResults_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    // 右鍵點擊時自動選中該列，但不手動 Show 選單 (交由原生屬性處理)
                    dgvResults.ClearSelection();
                    dgvResults.Rows[e.RowIndex].Selected = true;
                    dgvResults.CurrentCell = dgvResults.Rows[e.RowIndex].Cells[e.ColumnIndex];
                }
            }
        }

        private void ctxMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 權限控管：
            // 編輯：Level 1 & 2
            // 刪除：Level 1 Only
            menuEdit.Visible = UserSession.UserLevel <= 2;
            menuDelete.Visible = UserSession.UserLevel == 1;

            //// 如果沒有任何選項可見，則取消開啟選單
            //if (!menuEdit.Visible && !menuDelete.Visible)
            //{
            //    e.Cancel = true;
            //}
        }

        private void menuEdit_Click(object sender, EventArgs e)
        {
            if (dgvResults.SelectedRows.Count > 0)
            {
                var item = dgvResults.SelectedRows[0].DataBoundItem as SparePartSearchResultDto;
                if (item != null)
                {
                    var form = new MaterialCreationForm(item.MaterialId);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        btnSearch.PerformClick();
                    }
                }
            }
        }

        private async void menuDelete_Click(object sender, EventArgs e)
        {
            if (dgvResults.SelectedRows.Count > 0)
            {
                var item = dgvResults.SelectedRows[0].DataBoundItem as SparePartSearchResultDto;
                if (item != null)
                {
                    string confirmMsg = string.Format(LocalizationService.GetString("Msg_DeleteConfirm"), item.Name, item.PartNo);
                    var result = MessageBox.Show(confirmMsg, 
                        LocalizationService.GetString("Menu_Delete"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            await _apiClient.DeleteMaterialAsync(item.MaterialId);
                            MessageBox.Show(LocalizationService.GetString("Msg_DeleteSuccess"), 
                                LocalizationService.GetString("Common_Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnSearch.PerformClick();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(LocalizationService.GetString("Msg_DeleteError") + ex.Message, 
                                LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}
