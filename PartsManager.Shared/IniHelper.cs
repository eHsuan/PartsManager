using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace PartsManager.Shared
{
    public class IniHelper
    {
        private string _filePath;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public IniHelper(string filePath)
        {
            _filePath = Path.GetFullPath(filePath);
        }

        public string Read(string section, string key, string defaultValue = "")
        {
            StringBuilder temp = new StringBuilder(1024);
            int i = GetPrivateProfileString(section, key, defaultValue, temp, 1024, _filePath);
            return temp.ToString();
        }

        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, _filePath);
        }

        public bool KeyExists(string section, string key)
        {
            return Read(section, key).Length > 0;
        }
    }
}
