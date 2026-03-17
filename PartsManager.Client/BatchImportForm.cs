using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PartsManager.Shared.Resources;
using PartsManager.Shared.DTOs;

namespace PartsManager.Client
{
    public partial class BatchImportForm : Form
    {
        private readonly ApiClient _apiClient;

        public BatchImportForm()
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this);
            _apiClient = new ApiClient(GlobalSettings.ApiBaseUrl);
        }

        private void btnDownloadTemplate_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Files (*.xlsx)|*.xlsx";
                sfd.FileName = "MaterialImportTemplate.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        GenerateTemplate(sfd.FileName);
                        MessageBox.Show(string.Format(LocalizationService.GetString("Msg_TemplateSaved"), sfd.FileName),
                            LocalizationService.GetString("Common_Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void btnImportFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Excel Files (*.xlsx)|*.xlsx";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    await PerformImport(ofd.FileName);
                }
            }
        }

        private void GenerateTemplate(string filePath)
        {
            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("Materials");
                
                // 標題列 (使用語系資源)
                ws.Cell(1, 1).Value = LocalizationService.GetString("Import_Header_PartNo") + " *";
                ws.Cell(1, 2).Value = LocalizationService.GetString("Import_Header_Name") + " *";
                ws.Cell(1, 3).Value = LocalizationService.GetString("Import_Header_Spec");
                ws.Cell(1, 4).Value = LocalizationService.GetString("Import_Header_InitialStock");
                ws.Cell(1, 5).Value = LocalizationService.GetString("Import_Header_WarehouseId");
                ws.Cell(1, 6).Value = LocalizationService.GetString("Import_Header_SafeStock");
                ws.Cell(1, 7).Value = LocalizationService.GetString("Import_Header_LeadTime");
                ws.Cell(1, 8).Value = LocalizationService.GetString("Import_Header_Supplier");
                ws.Cell(1, 9).Value = LocalizationService.GetString("Import_Header_Manufacturer");
                ws.Cell(1, 10).Value = LocalizationService.GetString("Import_Header_Attachment1");
                ws.Cell(1, 11).Value = LocalizationService.GetString("Import_Header_Attachment2");

                // 樣式設定
                var headerRange = ws.Range(1, 1, 1, 11);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = ClosedXML.Excel.XLColor.LightGray;
                ws.Columns().AdjustToContents();

                workbook.SaveAs(filePath);
            }
        }

        private async System.Threading.Tasks.Task PerformImport(string filePath)
        {
            txtLog.Clear();
            btnImportFile.Enabled = false;
            btnDownloadTemplate.Enabled = false;
            
            string folderPath = Path.GetDirectoryName(filePath);
            int successCount = 0;
            int failCount = 0;

            try
            {
                using (var workbook = new ClosedXML.Excel.XLWorkbook(filePath))
                {
                    var ws = workbook.Worksheet(1);
                    var rows = ws.RangeUsed().RowsUsed().Skip(1); // 跳過標題列
                    int totalRows = rows.Count();
                    int currentIndex = 0;

                    foreach (var row in rows)
                    {
                        currentIndex++;
                        string partNo = row.Cell(1).GetString().Trim();
                        string name = row.Cell(2).GetString().Trim();
                        
                        if (string.IsNullOrEmpty(partNo) || string.IsNullOrEmpty(name))
                        {
                            if (!string.IsNullOrEmpty(partNo) || !string.IsNullOrEmpty(name))
                                AppendLog(string.Format(LocalizationService.GetString("Log_DataIncomplete"), partNo));
                            continue;
                        }

                        lblStatus.Text = string.Format(LocalizationService.GetString("Msg_Importing"), currentIndex, totalRows);

                        try
                        {
                            // 1. 建立 DTO (對應新欄位順序)
                            var dto = new CreateMaterialDto
                            {
                                PartNo = partNo,
                                Name = name,
                                Specification = row.Cell(3).GetString().Trim(),
                                Supplier = row.Cell(8).GetString().Trim(),
                                Manufacturer = row.Cell(9).GetString().Trim()
                            };

                            // 目前庫存 (Cell 4)
                            if (decimal.TryParse(row.Cell(4).GetString(), out decimal isty)) dto.InitialStock = isty;
                            
                            // 倉庫ID (Cell 5)，若為空則預設為 1
                            string whIdStr = row.Cell(5).GetString().Trim();
                            if (string.IsNullOrEmpty(whIdStr)) dto.WarehouseId = 1;
                            else if (int.TryParse(whIdStr, out int whid)) dto.WarehouseId = whid;
                            else dto.WarehouseId = 1;

                            // 安全庫存與交期 (Cell 6, 7)
                            if (decimal.TryParse(row.Cell(6).GetString(), out decimal ss)) dto.SafeStockQty = (int)ss;
                            if (decimal.TryParse(row.Cell(7).GetString(), out decimal lt)) dto.LeadTimeDays = (int)lt;

                            // 2. 處理附件路徑 (Cell 10, 11)
                            List<string> filePaths = new List<string>();
                            string att1 = row.Cell(10).GetString().Trim();
                            string att2 = row.Cell(11).GetString().Trim();

                            if (!string.IsNullOrEmpty(att1))
                            {
                                if (!Path.HasExtension(att1)) att1 += ".pdf";
                                string fullPath = Path.Combine(folderPath, att1);
                                if (File.Exists(fullPath)) filePaths.Add(fullPath);
                                else { AppendLog(string.Format(LocalizationService.GetString("Log_FileNotFound"), partNo, att1)); failCount++; continue; }
                            }
                            if (!string.IsNullOrEmpty(att2))
                            {
                                if (!Path.HasExtension(att2)) att2 += ".pdf";
                                string fullPath = Path.Combine(folderPath, att2);
                                if (File.Exists(fullPath)) filePaths.Add(fullPath);
                                else { AppendLog(string.Format(LocalizationService.GetString("Log_FileNotFound"), partNo, att2)); failCount++; continue; }
                            }

                            // 3. 呼叫 API
                            var material = await _apiClient.CreateMaterialAsync(dto);
                            if (material != null)
                            {
                                if (filePaths.Count > 0)
                                {
                                    await _apiClient.UploadAttachmentsAsync(material.MaterialID, filePaths);
                                }
                                AppendLog(string.Format(LocalizationService.GetString("Log_Success"), partNo));
                                successCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("409") || ex.Message.Contains("Conflict"))
                            {
                                AppendLog(string.Format(LocalizationService.GetString("Log_Duplicate"), partNo));
                            }
                            else
                            {
                                AppendLog(string.Format(LocalizationService.GetString("Log_ApiError"), partNo, ex.Message));
                            }
                            failCount++;
                        }
                    }
                }

                MessageBox.Show(string.Format(LocalizationService.GetString("Msg_ImportComplete"), successCount, failCount),
                    LocalizationService.GetString("Common_Info"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppendLog("CRITICAL ERROR: " + ex.Message);
            }
            finally
            {
                lblStatus.Text = "";
                btnImportFile.Enabled = true;
                btnDownloadTemplate.Enabled = true;
            }
        }

        private void AppendLog(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => AppendLog(message)));
                return;
            }
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }
    }
}
