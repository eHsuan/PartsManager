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
                MessageBox.Show("Search Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    dgvResults.CurrentCell = dgvResults.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dgvResults.Rows[e.RowIndex].Selected = true;
                    ctxMenu.Show(Cursor.Position);
                }
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
                            MessageBox.Show(LocalizationService.GetString("Msg_DeleteSuccess"), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnSearch.PerformClick();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Delete Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}
