using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PartsManager.Shared.Resources;

namespace PartsManager.Client.Services
{
    public class LabelPrinterService
    {
        /// <summary>
        /// 外部呼叫進入點，具備安全保護機制
        /// </summary>
        public static void PrintLabel(string barcode, string name = "")
        {
            // 1. 如果設定中關閉了列印，直接返回，不接觸 bpac 型別
            if (!GlobalSettings.EnableLabelPrinting) return;

            try
            {
                // 2. 透過另一個私有方法執行實際列印，確保 bpac 型別載入被延後到此處
                // 這樣即使沒 SDK，只要不進入此方法，JIT 就不會嘗試解析 bpac 組件
                ExecutePrintInternal(barcode, name);
            }
            catch (Exception ex)
            {
                // 3. 捕捉 SDK 缺失或通訊錯誤，防止程式直接當機
                string errorMsg = "標籤機列印失敗。請確認是否已安裝 Brother bPAC SDK 與驅動程式。\n\n錯誤詳情: " + ex.Message;
                MessageBox.Show(errorMsg, LocalizationService.GetString("Common_Error"), 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 實際與 SDK 交互的私有方法
        /// </summary>
        private static void ExecutePrintInternal(string barcode, string name)
        {
            // 只有進入此方法時，.NET 才會嘗試載入 bpac 組件
            bpac.DocumentClass doc = new bpac.DocumentClass();

            try
            {
                // 範本路徑
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalSettings.LabelTemplatePath);

                if (!File.Exists(templatePath))
                {
                    MessageBox.Show($"Label Template not found: {templatePath}", 
                        LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 開啟標籤底板
                if (doc.Open(templatePath))
                {
                    // 填入資料 (名稱請對應 bPAC 檔案內的物件名稱)
                    var barcodeObj = doc.GetObject("PartBarcode");
                    if (barcodeObj != null)
                    {
                        barcodeObj.Text = barcode;
                    }

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
            finally
            {
                // 釋放系統資源
                if (doc != null)
                {
                    Marshal.ReleaseComObject(doc);
                }
            }
        }
    }
}
