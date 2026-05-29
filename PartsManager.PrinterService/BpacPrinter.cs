using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using bpac;

namespace PartsManager.PrinterService
{
    public class BpacPrinter
    {
        private const string TemplateFolder = "Templates";

        public bool Print(string templateName, Dictionary<string, string> fields)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TemplateFolder, templateName);
            if (!File.Exists(path))
            {
                // 嘗試從主目錄找
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, templateName);
                if (!File.Exists(path)) return false;
            }

            DocumentClass doc = new DocumentClass();
            try
            {
                if (doc.Open(path))
                {
                    foreach (var kvp in fields)
                    {
                        var obj = doc.GetObject(kvp.Key);
                        if (obj != null)
                        {
                            obj.Text = kvp.Value;
                        }
                    }

                    doc.StartPrint("", PrintOptionConstants.bpoDefault);
                    doc.PrintOut(1, PrintOptionConstants.bpoDefault);
                    doc.EndPrint();
                    doc.Close();
                    return true;
                }
            }
            catch (COMException)
            {
                // bpac related error
            }
            finally
            {
                Marshal.ReleaseComObject(doc);
            }
            return false;
        }
    }
}
