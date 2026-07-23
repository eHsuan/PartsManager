using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PartsManager.Client
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 初始化語系
            string lang = GlobalSettings.Language;
            PartsManager.Shared.Resources.LocalizationService.SetLanguage(lang);

            // 執行自動更新檢查
            if (CheckForUpdates())
            {
                // 啟動了更新程式，主程式必須退出
                return;
            }

            while (true)
            {
                UserSession.Clear(); // 每次迴圈重置 Session
                LoginForm login = new LoginForm();
                if (login.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainForm());
                }
                else
                {
                    break;
                }
            }
        }

        static bool CheckForUpdates()
        {
            try
            {
                string localVersionStr = "1.0.0";
                var localVersion = Version.Parse(localVersionStr);

                var apiClient = new ApiClient(GlobalSettings.ApiBaseUrl, 10);
                string serverVersionStr = apiClient.GetLatestVersionAsync().GetAwaiter().GetResult();

                if (Version.TryParse(serverVersionStr, out var serverVersion))
                {
                    if (serverVersion > localVersion)
                    {
                        var confirmResult = MessageBox.Show(
                            $"偵測到新版本 ({serverVersionStr})，是否立即進行更新？\n(目前的版本為 {localVersionStr})",
                            "系統自動更新",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);

                        if (confirmResult == DialogResult.Yes)
                        {
                            PerformUpdate(apiClient);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 靜默失敗以避免影響正常啟動
                Console.WriteLine("檢查更新失敗: " + ex.Message);
            }
            return false;
        }

        static void PerformUpdate(ApiClient apiClient)
        {
            try
            {
                string tempDir = Path.Combine(Path.GetTempPath(), "PartsManagerUpdate");
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
                Directory.CreateDirectory(tempDir);

                string zipPath = Path.Combine(tempDir, "Client.zip");

                // 1. 下載更新包
                apiClient.DownloadClientZipAsync(zipPath).GetAwaiter().GetResult();

                // 2. 解壓縮
                string extractPath = Path.Combine(tempDir, "extracted");
                System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);

                // 3. 建立並啟動批次檔以執行檔案覆蓋與重啟
                string batPath = Path.Combine(tempDir, "update.bat");
                int currentPid = System.Diagnostics.Process.GetCurrentProcess().Id;
                string appDir = AppDomain.CurrentDomain.BaseDirectory;

                string batContent = $@"@echo off
:wait_loop
tasklist /FI ""PID eq {currentPid}"" 2>NUL | find /I ""{currentPid}"" >NUL
if ""%ERRORLEVEL%""==""0"" (
    timeout /T 1 /NOBREAK >nul
    goto wait_loop
)

xcopy ""{extractPath}\*.*"" ""{appDir}"" /Y /E /Q

start """" ""{Path.Combine(appDir, "PartsManager.Client.exe")}""
del ""%~f0"" & exit";

                File.WriteAllText(batPath, batContent, Encoding.Default);

                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = batPath,
                    WorkingDirectory = tempDir,
                    CreateNoWindow = true,
                    UseShellExecute = true,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                };
                System.Diagnostics.Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("更新過程中發生錯誤: " + ex.Message, "更新失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

