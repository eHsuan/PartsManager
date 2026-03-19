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
                
                ws.Cell(1, 1).Value = LocalizationService.GetString("Import_Header_Name") + " *";
                ws.Cell(1, 2).Value = LocalizationService.GetString("Import_Header_Spec");
                ws.Cell(1, 3).Value = LocalizationService.GetString("Import_Header_PartNo") + " *";
                ws.Cell(1, 4).Value = LocalizationService.GetString("Import_Header_Supplier");
                ws.Cell(1, 5).Value = LocalizationService.GetString("Import_Header_Manufacturer");
                ws.Cell(1, 6).Value = LocalizationService.GetString("Import_Header_WarehouseId");
                ws.Cell(1, 7).Value = LocalizationService.GetString("Import_Header_InitialStock");
                ws.Cell(1, 8).Value = LocalizationService.GetString("Import_Header_SafeStock");
                ws.Cell(1, 9).Value = LocalizationService.GetString("Import_Header_LeadTime");
                ws.Cell(1, 10).Value = LocalizationService.GetString("Import_Header_Price");
                ws.Cell(1, 11).Value = LocalizationService.GetString("Import_Header_Attachment1");
                ws.Cell(1, 12).Value = LocalizationService.GetString("Import_Header_Attachment2");

                // 樣式設定
                var headerRange = ws.Range(1, 1, 1, 12);
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
                    var headerRow = ws.Row(1);
                    var columnMap = new Dictionary<string, int>();

                    // 取得所有可能的標題名稱 (包含帶有星號的必填標記)
                    string headName = LocalizationService.GetString("Import_Header_Name");
                    string headSpec = LocalizationService.GetString("Import_Header_Spec");
                    string headPartNo = LocalizationService.GetString("Import_Header_PartNo");
                    string headSupplier = LocalizationService.GetString("Import_Header_Supplier");
                    string headManufacturer = LocalizationService.GetString("Import_Header_Manufacturer");
                    string headWhId = LocalizationService.GetString("Import_Header_WarehouseId");
                    string headStock = LocalizationService.GetString("Import_Header_InitialStock");
                    string headSafeStock = LocalizationService.GetString("Import_Header_SafeStock");
                    string headLeadTime = LocalizationService.GetString("Import_Header_LeadTime");
                    string headPrice = LocalizationService.GetString("Import_Header_Price");
                    string headAtt1 = LocalizationService.GetString("Import_Header_Attachment1");
                    string headAtt2 = LocalizationService.GetString("Import_Header_Attachment2");

                    // 掃描標題列建立映射
                    for (int i = 1; i <= ws.LastColumnUsed().ColumnNumber(); i++)
                    {
                        string cellValue = headerRow.Cell(i).GetString().Trim();
                        if (string.IsNullOrEmpty(cellValue)) continue;

                        if (cellValue.Contains(headName)) columnMap["Name"] = i;
                        else if (cellValue.Contains(headSpec)) columnMap["Spec"] = i;
                        else if (cellValue.Contains(headPartNo)) columnMap["PartNo"] = i;
                        else if (cellValue.Contains(headSupplier)) columnMap["Supplier"] = i;
                        else if (cellValue.Contains(headManufacturer)) columnMap["Manufacturer"] = i;
                        else if (cellValue.Contains(headWhId)) columnMap["WhID"] = i;
                        else if (cellValue.Contains(headStock)) columnMap["Stock"] = i;
                        else if (cellValue.Contains(headSafeStock)) columnMap["SafeStock"] = i;
                        else if (cellValue.Contains(headLeadTime)) columnMap["LeadTime"] = i;
                        else if (cellValue.Contains(headPrice)) columnMap["Price"] = i;
                        else if (cellValue.Contains(headAtt1)) columnMap["Att1"] = i;
                        else if (cellValue.Contains(headAtt2)) columnMap["Att2"] = i;
                    }

                    // 檢查必要標題是否存在
                    if (!columnMap.ContainsKey("PartNo") || !columnMap.ContainsKey("Name"))
                    {
                        AppendLog("[錯誤] Excel 格式不正確，找不到 '料號' 或 '品名' 欄位標題。");
                        return;
                    }

                    var rows = ws.RangeUsed().RowsUsed().Skip(1); // 跳過標題列
                    int totalRows = rows.Count();
                    int currentIndex = 0;

                    foreach (var row in rows)
                    {
                        currentIndex++;
                        
                        // 根據映射讀取資料
                        string name = columnMap.ContainsKey("Name") ? row.Cell(columnMap["Name"]).GetString().Trim() : "";
                        string partNo = columnMap.ContainsKey("PartNo") ? row.Cell(columnMap["PartNo"]).GetString().Trim() : "";
                        
                        if (string.IsNullOrEmpty(partNo) || string.IsNullOrEmpty(name))
                        {
                            if (!string.IsNullOrEmpty(partNo) || !string.IsNullOrEmpty(name))
                                AppendLog(string.Format(LocalizationService.GetString("Log_DataIncomplete"), partNo));
                            continue;
                        }

                        lblStatus.Text = string.Format(LocalizationService.GetString("Msg_Importing"), currentIndex, totalRows);

                        try
                        {
                            var dto = new CreateMaterialDto
                            {
                                Name = name,
                                PartNo = partNo,
                                Specification = columnMap.ContainsKey("Spec") ? row.Cell(columnMap["Spec"]).GetString().Trim() : "",
                                Supplier = columnMap.ContainsKey("Supplier") ? row.Cell(columnMap["Supplier"]).GetString().Trim() : "",
                                Manufacturer = columnMap.ContainsKey("Manufacturer") ? row.Cell(columnMap["Manufacturer"]).GetString().Trim() : ""
                            };

                            // 倉庫ID
                            if (columnMap.ContainsKey("WhID"))
                            {
                                string whIdStr = row.Cell(columnMap["WhID"]).GetString().Trim();
                                if (int.TryParse(whIdStr, out int whid)) dto.WarehouseId = whid;
                                else dto.WarehouseId = 1;
                            }
                            else dto.WarehouseId = 1;

                            // 目前庫存
                            if (columnMap.ContainsKey("Stock") && decimal.TryParse(row.Cell(columnMap["Stock"]).GetString(), out decimal isty)) 
                            {
                                if (isty % 1 != 0)
                                {
                                    AppendLog($"[失敗] 料號 {partNo}: 目前庫存必須為整數");
                                    failCount++;
                                    continue;
                                }
                                dto.InitialStock = isty;
                            }

                            // 安全庫存與交期
                            if (columnMap.ContainsKey("SafeStock") && decimal.TryParse(row.Cell(columnMap["SafeStock"]).GetString(), out decimal ss)) 
                                dto.SafeStockQty = (int)ss;
                            if (columnMap.ContainsKey("LeadTime") && decimal.TryParse(row.Cell(columnMap["LeadTime"]).GetString(), out decimal lt)) 
                                dto.LeadTimeDays = (int)lt;
                            
                            // 金額
                            if (columnMap.ContainsKey("Price") && decimal.TryParse(row.Cell(columnMap["Price"]).GetString(), out decimal price))
                            {
                                if (price <= 0)
                                {
                                    AppendLog($"[失敗] 料號 {partNo}: 金額必須大於 0");
                                    failCount++;
                                    continue;
                                }
                                dto.Price = price;
                            }
                            else
                            {
                                AppendLog($"[失敗] 料號 {partNo}: 金額格式錯誤、未填寫或找不到標題欄位");
                                failCount++;
                                continue;
                            }

                            // 處理附件路徑
                            List<string> filePaths = new List<string>();
                            if (columnMap.ContainsKey("Att1"))
                            {
                                string att1 = row.Cell(columnMap["Att1"]).GetString().Trim();
                                if (!string.IsNullOrEmpty(att1))
                                {
                                    if (!Path.HasExtension(att1)) att1 += ".pdf";
                                    string fullPath = Path.Combine(folderPath, att1);
                                    if (File.Exists(fullPath)) filePaths.Add(fullPath);
                                    else { AppendLog(string.Format(LocalizationService.GetString("Log_FileNotFound"), partNo, att1)); }
                                }
                            }
                            if (columnMap.ContainsKey("Att2"))
                            {
                                string att2 = row.Cell(columnMap["Att2"]).GetString().Trim();
                                if (!string.IsNullOrEmpty(att2))
                                {
                                    if (!Path.HasExtension(att2)) att2 += ".pdf";
                                    string fullPath = Path.Combine(folderPath, att2);
                                    if (File.Exists(fullPath)) filePaths.Add(fullPath);
                                    else { AppendLog(string.Format(LocalizationService.GetString("Log_FileNotFound"), partNo, att2)); }
                                }
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
            catch (IOException ioEx)
            {
                AppendLog(LocalizationService.GetString("Msg_FileIsLocked"));
                MessageBox.Show(LocalizationService.GetString("Msg_FileIsLocked") + "\n\n" + ioEx.Message, 
                    LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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
