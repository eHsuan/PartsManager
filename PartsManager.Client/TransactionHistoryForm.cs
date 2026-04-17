using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PartsManager.Shared.DTOs;
using System.IO;
using ClosedXML.Excel;
using PartsManager.Client.Services;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public partial class TransactionHistoryForm : Form
    {
        private readonly ApiClient _apiClient;

        public TransactionHistoryForm()
        {
            InitializeComponent();
            _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);
            
            // 設定預設日期範圍：今天
            dtpStart.Value = DateTime.Today;
            dtpEnd.Value = DateTime.Today;

            SetupDataGridView();
            UIStyle.Apply(this);
            I18nHelper.Apply(this);
        }

        private void SetupDataGridView()
        {
            dgvHistory.AutoGenerateColumns = false;
            dgvHistory.Columns.Clear();

            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TransTime", HeaderText = "時間", Name = "TransTime", Width = 150 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "TransType", HeaderText = "類型", Name = "TransType", Width = 80 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PartNo", HeaderText = "料號", Name = "PartNo", Width = 150 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MaterialName", HeaderText = "品名", Name = "MaterialName", Width = 200 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ChangeQty", HeaderText = "變動數量", Name = "ChangeQty", Width = 100 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "AfterQty", HeaderText = "異動後庫存", Name = "AfterQty", Width = 100 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "OperatorID", HeaderText = "作業工號", Name = "OperatorID", Width = 100 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "WarehouseName", HeaderText = "倉庫", Name = "WarehouseName", Width = 100 });
            dgvHistory.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ReasonCode", HeaderText = "備註/原因", Name = "ReasonCode", Width = 150 });

            dgvHistory.ReadOnly = true;
            dgvHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                btnSearch.Enabled = false;
                var history = await _apiClient.GetTransactionHistoryAsync(dtpStart.Value, dtpEnd.Value);
                dgvHistory.DataSource = history;
            }
            catch (Exception ex)
            {
                MessageBox.Show("查詢失敗: " + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSearch.Enabled = true;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvHistory.Rows.Count == 0)
            {
                MessageBox.Show(LocalizationService.GetString("Msg_NoDataToExport") ?? "無資料可供匯出");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Files (*.xlsx)|*.xlsx";
                sfd.FileName = $"TransactionHistory_{DateTime.Now:yyyyMMddHHmm}.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            var worksheet = workbook.Worksheets.Add("History");

                            // 標題列
                            for (int i = 0; i < dgvHistory.Columns.Count; i++)
                            {
                                worksheet.Cell(1, i + 1).Value = dgvHistory.Columns[i].HeaderText;
                                worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                            }

                            // 資料列
                            var data = (List<TransactionDto>)dgvHistory.DataSource;
                            for (int i = 0; i < data.Count; i++)
                            {
                                var item = data[i];
                                worksheet.Cell(i + 2, 1).Value = item.TransTime.ToString("yyyy-MM-dd HH:mm:ss");
                                worksheet.Cell(i + 2, 2).Value = item.TransType;
                                worksheet.Cell(i + 2, 3).Value = item.PartNo;
                                worksheet.Cell(i + 2, 4).Value = item.MaterialName;
                                worksheet.Cell(i + 2, 5).Value = item.ChangeQty;
                                worksheet.Cell(i + 2, 6).Value = item.AfterQty;
                                worksheet.Cell(i + 2, 7).Value = item.OperatorID;
                                worksheet.Cell(i + 2, 8).Value = item.WarehouseName;
                                worksheet.Cell(i + 2, 9).Value = item.ReasonCode;
                            }

                            worksheet.Columns().AdjustToContents();
                            workbook.SaveAs(sfd.FileName);
                        }

                        MessageBox.Show(LocalizationService.GetString("Msg_ExportSuccess") ?? "匯出成功", 
                            LocalizationService.GetString("Common_Info") ?? "資訊", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (IOException)
                    {
                        MessageBox.Show(LocalizationService.GetString("Msg_FileIsLocked") ?? "檔案正被其他程式使用中，請先關閉 Excel。", 
                            LocalizationService.GetString("Common_Error") ?? "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show((LocalizationService.GetString("Msg_ExportError") ?? "匯出失敗：") + ex.Message, 
                            LocalizationService.GetString("Common_Error") ?? "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}

