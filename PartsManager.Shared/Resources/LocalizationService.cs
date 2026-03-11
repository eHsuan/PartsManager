using System.Globalization;
using System.Resources;
using System.Reflection;

namespace PartsManager.Shared.Resources
{
    public static class LocalizationService
    {
        private static ResourceManager _resManager;
        private static CultureInfo _culture;

        static LocalizationService()
        {
            // 注意：這裡的路徑必須是 [Assembly名稱].[資料夾].[檔名]
            // 如果您的專案名稱是 PartsManager.Shared，且在 Resources 資料夾下，則如下：
            _resManager = new ResourceManager("PartsManager.Shared.Resources.Lang", typeof(LocalizationService).Assembly);
            _culture = CultureInfo.CurrentCulture;
        }

        public static void SetLanguage(string langCode)
        {
            try
            {
                _culture = new CultureInfo(langCode);
            }
            catch
            {
                _culture = new CultureInfo("zh-TW");
            }
        }

        public static string GetString(string key)
        {
            try
            {
                return _resManager.GetString(key, _culture) ?? key;
            }
            catch
            {
                return key;
            }
        }
    }
}
