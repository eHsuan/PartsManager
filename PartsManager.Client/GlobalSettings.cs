using System;
using System.IO;
using PartsManager.Shared;

namespace PartsManager.Client
{
    public static class GlobalSettings
    {
        private static IniHelper _ini;
        private static string _iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");

        private static bool _enableLabelPrinting;
        private static string _language;
        private static int _autoLogoutMinutes;
        private static string _serverIP;
        private static string _serverPort;
        private static int _defaultWarehouseId;

        static GlobalSettings()
        {
            _ini = new IniHelper(_iniPath);
            LoadSettings();
        }

        public static void LoadSettings()
        {
            _serverIP = _ini.Read("Network", "ServerIP", "localhost");
            _serverPort = _ini.Read("Network", "ServerPort", "5000");
            _language = _ini.Read("System", "Language", "zh-TW");
            
            string timeoutStr = _ini.Read("System", "AutoLogoutMinutes", "10");
            int.TryParse(timeoutStr, out _autoLogoutMinutes);

            string whIdStr = _ini.Read("Inventory", "DefaultWarehouseId", "1");
            int.TryParse(whIdStr, out _defaultWarehouseId);

            string printStr = _ini.Read("Inventory", "EnableLabelPrinting", "true");
            bool.TryParse(printStr, out _enableLabelPrinting);
        }

        public static string ApiBaseUrl => $"http://{_serverIP}:{_serverPort}/";
        public static string ServerIP
        {
            get => _serverIP;
            set
            {
                _serverIP = value;
                _ini.Write("Network", "ServerIP", value);
            }
        }

        public static string ServerPort
        {
            get => _serverPort;
            set
            {
                _serverPort = value;
                _ini.Write("Network", "ServerPort", value);
            }
        }
        public static string Language => _language;
        public static int AutoLogoutMinutes => _autoLogoutMinutes;
        public static int DefaultWarehouseId => _defaultWarehouseId;

        public static bool EnableLabelPrinting
        {
            get => _enableLabelPrinting;
            set
            {
                _enableLabelPrinting = value;
                _ini.Write("Inventory", "EnableLabelPrinting", value.ToString().ToLower());
            }
        }
    }
}
