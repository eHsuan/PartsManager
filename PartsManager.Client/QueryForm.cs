using System;
using System.Collections.Generic;
using System.Linq;
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
            menuOutbound.Text = LocalizationService.GetString("Menu_Outbound");

            string baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"] ?? "http://localhost:5000/";
            _apiClient = new ApiClient(baseUrl);

            // 修正：防止沒有圖片時顯示 X，並設定自動縮放
            Col_Att1.DefaultCellStyle.NullValue = null;
            Col_Att1.ImageLayout = DataGridViewImageCellLayout.Zoom;
            Col_Att2.DefaultCellStyle.NullValue = null;
            Col_Att2.ImageLayout = DataGridViewImageCellLayout.Zoom;

            dgvResults.CellContentClick += dgvResults_CellContentClick;
        }

        private async void dgvResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // 檢查是否點擊附件欄位
            string colName = dgvResults.Columns[e.ColumnIndex].Name;
            if (colName == "Col_Att1" || colName == "Col_Att2")
            {
                var material = dgvResults.Rows[e.RowIndex].DataBoundItem as SparePartSearchResultDto;
                if (material == null) return;

                int index = colName == "Col_Att1" ? 0 : 1;
                if (material.AttachmentFileNames != null && material.AttachmentFileNames.Count > index)
                {
                    string fileName = material.AttachmentFileNames[index];
                    try
                    {
                        var data = await _apiClient.DownloadAttachmentAsync(material.MaterialId, fileName);
                        string tempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), fileName);
                        System.IO.File.WriteAllBytes(tempFile, data);
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempFile) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(LocalizationService.GetString("Msg_CannotOpenFile") + ex.Message, 
                            LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void menuOutbound_Click(object sender, EventArgs e)
        {
            if (dgvResults.SelectedRows.Count > 0)
            {
                var material = dgvResults.SelectedRows[0].DataBoundItem as SparePartSearchResultDto;
                if (material != null)
                {
                    var mainForm = Application.OpenForms.OfType<MainForm>().FirstOrDefault();
                    if (mainForm != null)
                    {
                        mainForm.NavigateToOutboundWithBarcode(material.PartNo);
                        this.Close();
                    }
                }
            }
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

                // 設置附件圖示
                var pdfIcon = LocalizationService.GetPdfIcon();
                for (int i = 0; i < dgvResults.Rows.Count; i++)
                {
                    var material = dgvResults.Rows[i].DataBoundItem as SparePartSearchResultDto;
                    if (material != null && material.AttachmentFileNames != null)
                    {
                        if (material.AttachmentFileNames.Count > 0)
                            dgvResults.Rows[i].Cells["Col_Att1"].Value = pdfIcon;
                        if (material.AttachmentFileNames.Count > 1)
                            dgvResults.Rows[i].Cells["Col_Att2"].Value = pdfIcon;
                    }
                }
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
                if (e.RowIndex >= 0)
                {
                    // 右鍵點擊時自動選中該列
                    dgvResults.ClearSelection();
                    dgvResults.Rows[e.RowIndex].Selected = true;

                    // 手動彈出選單，確保在選取後才顯示
                    ctxMenu.Show(Cursor.Position);
                }
            }
        }

        private void ctxMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // 權限控管：
            // 編輯：Level 1 & 2
            // 刪除：Level 1 Only
            // 領料：Level <= 3 (注意：如果是 0 代表未登入或異常，預設設為不允許)
            int level = UserSession.UserLevel == 0 ? 99 : UserSession.UserLevel;

            menuEdit.Visible = level <= 2;
            menuDelete.Visible = level == 1;
            menuOutbound.Visible = level <= 3;

            // 如果沒有任何選項可見，不要取消事件 (e.Cancel = true 會導致沒反應)
            // 而是可以考慮加入一個「無權限」的提示項，或者至少讓選單知道發生什麼事
            if (!menuEdit.Visible && !menuDelete.Visible && !menuOutbound.Visible)
            {
                // 為了讓使用者知道「右鍵有反應」，我們顯示選單但不提供功能
                // 這裡暫不 Cancel，讓空選單或僅有的項目顯示
            }
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
