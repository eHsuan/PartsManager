using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PartsManager.Shared.Resources;

namespace PartsManager.Client.Services
{
    public class LabelPrinterService
    {
        public static void PrintLabel(string barcode, string name = "")
        {
            if (!GlobalSettings.EnableLabelPrinting) return;

            // 準備 bPAC 文件物件
            bpac.DocumentClass doc = new bpac.DocumentClass();

            // 範本路徑
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalSettings.LabelTemplatePath);

            if (!File.Exists(templatePath))
            {
                MessageBox.Show($"Label Template not found: {templatePath}", 
                    LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // 開啟標籤底板
                if (doc.Open(templatePath))
                {
                    // 填入資料
                    // 根據 PrintTest 範例，條碼物件名稱為 "PartBarcode"
                    var barcodeObj = doc.GetObject("PartBarcode");
                    if (barcodeObj != null)
                    {
                        barcodeObj.Text = barcode;
                    }

                    // 如果範本中有 PartName 物件，也一併填入
                    var nameObj = doc.GetObject("PartName");
                    if (nameObj != null && !string.IsNullOrEmpty(name))
                    {
                        nameObj.Text = name;
                    }

                    // 設定印表機名稱
                    doc.SetPrinter(GlobalSettings.PrinterName, false);

                    // 執行列印
                    doc.StartPrint("", bpac.PrintOptionConstants.bpoDefault);
                    doc.PrintOut(1, bpac.PrintOptionConstants.bpoDefault);
                    doc.EndPrint();

                    // 關閉底板
                    doc.Close();
                }
                else
                {
                    MessageBox.Show("Failed to open label template.", 
                        LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{LocalizationService.GetString("Msg_PrintError")}: {ex.Message}", 
                    LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 釋放系統資源
                Marshal.ReleaseComObject(doc);
            }
        }
    }
}
