using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace PartsManager.PrinterService
{
    public partial class MainForm : Form
    {
        private float _alpha = 255;
        private bool _fadingOut = true;
        private TcpServer _server;
        private BpacPrinter _printer;

        public MainForm()
        {
            InitializeComponent();
            _printer = new BpacPrinter();
            _server = new TcpServer(Settings.ListenPort); // 從設定檔讀取埠號
            _server.MessageReceived += OnMessageReceived;
            _server.StatusChanged += (s, e) => this.Invoke((Action)UpdateStatus);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Log("Service Starting...");
            try
            {
                _server.Start();
                Log($"TCP Server started on port {Settings.ListenPort}.");
            }
            catch (Exception ex)
            {
                Log("Error starting server: " + ex.Message);
            }
        }

        private void OnMessageReceived(string message)
        {
            this.Invoke((Action)(() => {
                Log("Received: " + message);
                ProcessCommand(message);
            }));
        }

        private void ProcessCommand(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;

            try
            {
                // 1. 修剪可能存在的不可見字元
                json = json.Trim();

                // 2. 容錯處理：如果字串前後缺少大括號，嘗試補齊
                if (!json.StartsWith("{") && json.Contains("\"Type\""))
                {
                    json = "{" + json;
                }
                if (json.StartsWith("{") && !json.EndsWith("}"))
                {
                    json = json + "}";
                }

                var cmd = Newtonsoft.Json.JsonConvert.DeserializeObject<PrintCommand>(json);
                if (cmd != null && (cmd.Type == "PRINT" || cmd.Type == "LABEL"))
                {
                    Log($"Printing label: {cmd.Template}");
                    bool success = _printer.Print(cmd.Template, cmd.Fields);
                    Log(success ? "Print successful." : "Print failed (Check template path/objects).");
                }
                else
                {
                    Log($"Unknown command type: {cmd?.Type ?? "NULL"}");
                }
            }
            catch (Exception ex)
            {
                Log("Command error: " + ex.Message);
                Log("Raw Data causing error: " + json);
            }
        }

        private void UpdateStatus()
        {
            // 邏輯已在 breathingTimer_Tick 中處理，這裡僅作為觸發或擴充
        }

        private void breathingTimer_Tick(object sender, EventArgs e)
        {
            Color baseColor = _server.IsClientConnected ? Color.Lime : Color.Red;
            
            if (_fadingOut)
            {
                _alpha -= 10;
                if (_alpha <= 100) _fadingOut = false;
            }
            else
            {
                _alpha += 10;
                if (_alpha >= 255) _fadingOut = true;
            }

            pnlStatus.BackColor = Color.FromArgb((int)_alpha, baseColor);
        }

        private void Log(string msg)
        {
            if (rtbLogs.InvokeRequired)
            {
                rtbLogs.Invoke((Action)(() => Log(msg)));
                return;
            }
            rtbLogs.AppendText($"[{DateTime.Now:HH:mm:ss}] {msg}\r\n");
            rtbLogs.SelectionStart = rtbLogs.Text.Length;
            rtbLogs.ScrollToCaret();
            
            // 限制日誌長度
            if (rtbLogs.Lines.Length > 500)
            {
                rtbLogs.Text = string.Join("\r\n", rtbLogs.Lines.Skip(100));
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _server?.Stop();
        }
    }

    public class PrintCommand
    {
        public string Type { get; set; }
        public string Template { get; set; }
        public System.Collections.Generic.Dictionary<string, string> Fields { get; set; }
    }
}
