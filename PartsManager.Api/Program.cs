using PartsManager.Api.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Diagnostics;
using Microsoft.Win32;

// --- 1. Serilog 設定 (Log to File) ---
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File("logs/parts-api-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.AddIniFile("config.ini", optional: true, reloadOnChange: true);
    builder.Host.UseSerilog();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers();

    builder.Services.AddScoped<PartsManager.Api.Services.IStockService, PartsManager.Api.Services.StockService>();

    // --- 4. 初始化語系 ---
    string lang = builder.Configuration["System:Language"] ?? "zh-TW";
    PartsManager.Shared.Resources.LocalizationService.SetLanguage(lang);

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.MapControllers();

    // --- 2. 自動啟動註冊 (Registry Auto-run) ---
    try
    {
        string? appPath = Process.GetCurrentProcess().MainModule?.FileName;
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
        if (key != null && !string.IsNullOrEmpty(appPath))
        {
            key.SetValue("PartsManagerApi", $"\"{appPath}\"");
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "無法設定自動啟動");
    }

    // --- 3. 系統托盤 (Tray Icon) 控制邏輯 ---
    var cts = new CancellationTokenSource();
    Thread trayThread = new Thread(() =>
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var notifyIcon = new NotifyIcon
        {
            Icon = SystemIcons.Shield,
            Text = PartsManager.Shared.Resources.LocalizationService.GetString("App_Title") + " (Backend)",
            Visible = true
        };

        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add(PartsManager.Shared.Resources.LocalizationService.GetString("Tray_OpenLogs"), null, (s, e) => {
            try { Process.Start(new ProcessStartInfo("explorer.exe", Path.Combine(AppContext.BaseDirectory, "logs")) { UseShellExecute = true }); }
            catch { }
        });
        contextMenu.Items.Add(new ToolStripSeparator());
        contextMenu.Items.Add(PartsManager.Shared.Resources.LocalizationService.GetString("Tray_Exit"), null, (s, e) => {
            if (MessageBox.Show(PartsManager.Shared.Resources.LocalizationService.GetString("Tray_WarnClose"), 
                PartsManager.Shared.Resources.LocalizationService.GetString("Tray_WarnTitle"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                notifyIcon.Visible = false;
                cts.Cancel();
                Application.Exit();
            }
        });

        notifyIcon.ContextMenuStrip = contextMenu;
        Application.Run();
    });

    trayThread.SetApartmentState(ApartmentState.STA);
    trayThread.Start();

    Log.Information("PartsManager API Server 啟動中...");
    app.RunAsync(cts.Token).Wait();
}
catch (Exception ex)
{
    Log.Fatal(ex, "API Server 發生致命錯誤而停止");
}
finally
{
    Log.CloseAndFlush();
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
