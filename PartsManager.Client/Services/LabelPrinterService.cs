using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PartsManager.Shared.Resources;

namespace PartsManager.Client.Services
{
    public class LabelPrinterService
    {
        private static PrinterServiceClient _printerClient;
        private static System.Timers.Timer _reconnectTimer;

        /// <summary>
        /// 初始化連線，並啟動背景監測
        /// </summary>
        public static void Initialize()
        {
            if (!GlobalSettings.EnableLabelPrinting) return;

            if (_printerClient == null)
            {
                _printerClient = new PrinterServiceClient(GlobalSettings.PrinterServiceIP, GlobalSettings.PrinterServicePort);
            }

            // 啟動背景定時器，每 5 秒檢查一次連線
            _reconnectTimer = new System.Timers.Timer(5000);
            _reconnectTimer.Elapsed += (s, e) => {
                if (GlobalSettings.EnableLabelPrinting)
                {
                    _printerClient.Connect();
                }
            };
            _reconnectTimer.AutoReset = true;
            _reconnectTimer.Enabled = true;

            // 立即嘗試第一次連線
            System.Threading.Tasks.Task.Run(() => _printerClient.Connect());
        }

        /// <summary>
        /// 外部呼叫進入點
        /// </summary>
        public static void PrintLabel(string barcode, string name = "")
        {
            // 1. 如果設定中關閉了列印，直接返回
            if (!GlobalSettings.EnableLabelPrinting) return;

            try
            {
                if (_printerClient == null)
                {
                    _printerClient = new PrinterServiceClient(GlobalSettings.PrinterServiceIP, GlobalSettings.PrinterServicePort);
                }

                var fields = new Dictionary<string, string>
                {
                    { "PartBarcode", barcode },
                    { "PartName", name }
                };

                // 取得範本檔名 (僅需檔名，路徑由 Service 決定或傳遞相對路徑)
                string templateName = System.IO.Path.GetFileName(GlobalSettings.LabelTemplatePath);

                bool success = _printerClient.SendPrintRequest(templateName, fields);

                if (!success)
                {
                    MessageBox.Show(LocalizationService.GetString("Msg_PrinterServiceOffline") ?? "無法連接至標籤列印服務，請確認服務已啟動。", 
                        LocalizationService.GetString("Common_Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Label print request failed: " + ex.Message, 
                    LocalizationService.GetString("Common_Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
