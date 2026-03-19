using System.Drawing;
using System.Reflection;
using System.IO;

namespace PartsManager.Client
{
    public static class ClientResources
    {
        private static Image _pdfIcon;

        public static Image PdfIcon
        {
            get
            {
                if (_pdfIcon == null)
                {
                    try
                    {
                        var assembly = Assembly.GetExecutingAssembly();
                        using (Stream stream = assembly.GetManifestResourceStream("PdfIcon.bmp"))
                        {
                            if (stream != null)
                            {
                                _pdfIcon = Image.FromStream(stream);
                            }
                        }
                    }
                    catch { return null; }
                }
                return _pdfIcon;
            }
        }
    }
}
