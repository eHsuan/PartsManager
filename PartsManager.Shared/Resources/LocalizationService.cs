using System.Globalization;
using System.Resources;
using System.Reflection;
using System.Drawing;
using System.IO;

namespace PartsManager.Shared.Resources
{
    public static class LocalizationService
    {
        private static ResourceManager _resManager;
        private static CultureInfo _culture;

        static LocalizationService()
        {
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

        public static Image GetPdfIcon()
        {
            try
            {
                string base64 = _resManager.GetString("PdfIconBase64");
                if (string.IsNullOrEmpty(base64)) return null;

                byte[] bytes = System.Convert.FromBase64String(base64);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    return Image.FromStream(ms);
                }
            }
            catch { return null; }
        }
    }
}
