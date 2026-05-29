using System;
using System.IO;
using PartsManager.Shared;

namespace PartsManager.PrinterService
{
    public static class Settings
    {
        private static IniHelper _ini;
        private static string _iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");
        
        public static int ListenPort { get; private set; }

        static Settings()
        {
            _ini = new IniHelper(_iniPath);
            Load();
        }

        public static void Load()
        {
            // 讀取 [Network] 節段下的 Port，預設為 9100
            string portStr = _ini.Read("Network", "Port", "9100");
            if (int.TryParse(portStr, out int port))
            {
                ListenPort = port;
            }
            else
            {
                ListenPort = 9100;
            }
        }
    }
}
