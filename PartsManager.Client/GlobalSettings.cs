using System;
using System.Configuration;

namespace PartsManager.Client
{
    public static class GlobalSettings
    {
        private static bool _enableLabelPrinting;

        static GlobalSettings()
        {
            string val = ConfigurationManager.AppSettings["EnableLabelPrinting"];
            if (bool.TryParse(val, out bool result))
            {
                _enableLabelPrinting = result;
            }
            else
            {
                _enableLabelPrinting = true; // 預設開啟
            }
        }

        public static bool EnableLabelPrinting
        {
            get => _enableLabelPrinting;
            set
            {
                _enableLabelPrinting = value;
                UpdateAppConfig("EnableLabelPrinting", value.ToString().ToLower());
            }
        }

        private static void UpdateAppConfig(string key, string value)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.AppSettings.Settings[key] == null)
                {
                    config.AppSettings.Settings.Add(key, value);
                }
                else
                {
                    config.AppSettings.Settings[key].Value = value;
                }
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Update Config Error: " + ex.Message);
            }
        }
    }
}
