using System.Windows.Forms;
using PartsManager.Shared.Resources;

namespace PartsManager.Client
{
    public static class I18nHelper
    {
        public static void Apply(Form form)
        {
            // 優先使用 Tag 作為語系 Key，其次才是 Name
            string formKey = form.Tag?.ToString();
            if (string.IsNullOrEmpty(formKey)) formKey = form.Name;

            string translatedTitle = LocalizationService.GetString(formKey);
            if (!string.IsNullOrEmpty(translatedTitle) && translatedTitle != formKey)
            {
                form.Text = translatedTitle;
            }

            foreach (Control ctrl in form.Controls)
            {
                ApplyToControl(ctrl);
            }
        }

        private static void ApplyToControl(Control ctrl)
        {
            // 優先使用 Tag 作為語系 Key
            string key = ctrl.Tag?.ToString();
            
            // 如果沒設定 Tag 但控制項是按鈕或標籤，嘗試用名稱 (選用)
            if (string.IsNullOrEmpty(key) && (ctrl is Button || ctrl is Label))
            {
                // key = ctrl.Name; // 暫時註解，改用 Tag 比較精確
            }

            if (!string.IsNullOrEmpty(key))
            {
                string translation = LocalizationService.GetString(key);
                // 只要翻譯結果不為空就賦值，移除 translation != key 的限制
                // 這樣即使資源檔 fallback 到中文，也會強制重新賦值
                if (!string.IsNullOrEmpty(translation))
                {
                    ctrl.Text = translation;
                }
            }

            // 處理選單 (選單通常沒有 Tag，所以用 Name)
            if (ctrl is MenuStrip ms)
            {
                foreach (ToolStripMenuItem item in ms.Items)
                {
                    ApplyToMenuItem(item);
                }
            }

            // 處理 DataGridView 欄位
            if (ctrl is DataGridView dgv)
            {
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    // 優先使用 Tag，若無則使用 Name
                    string colKey = col.Tag?.ToString();
                    if (string.IsNullOrEmpty(colKey)) colKey = col.Name;

                    string colTranslation = LocalizationService.GetString(colKey);
                    if (!string.IsNullOrEmpty(colTranslation) && colTranslation != colKey)
                    {
                        col.HeaderText = colTranslation;
                    }
                }
            }

            if (ctrl.HasChildren)
            {
                foreach (Control child in ctrl.Controls)
                {
                    ApplyToControl(child);
                }
            }
        }

        private static void ApplyToMenuItem(ToolStripMenuItem item)
        {
            string translation = LocalizationService.GetString(item.Name);
            if (!string.IsNullOrEmpty(translation) && translation != item.Name)
            {
                item.Text = translation;
            }

            if (item.HasDropDownItems)
            {
                foreach (ToolStripItem child in item.DropDownItems)
                {
                    if (child is ToolStripMenuItem subItem)
                        ApplyToMenuItem(subItem);
                }
            }
        }
    }
}
