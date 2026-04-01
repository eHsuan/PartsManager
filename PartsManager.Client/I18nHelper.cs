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

            // 處理表單層級的 ContextMenuStrip (例如右鍵選單)
            if (form.ContextMenuStrip != null)
            {
                ApplyToContextMenu(form.ContextMenuStrip);
            }
        }

        private static void ApplyToControl(Control ctrl)
        {
            // 優先使用 Tag 作為語系 Key
            string key = ctrl.Tag?.ToString();
            
            if (!string.IsNullOrEmpty(key))
            {
                string translation = LocalizationService.GetString(key);
                if (!string.IsNullOrEmpty(translation))
                {
                    ctrl.Text = translation;
                }
            }

            // 處理控制項附屬的 ContextMenuStrip
            if (ctrl.ContextMenuStrip != null)
            {
                ApplyToContextMenu(ctrl.ContextMenuStrip);
            }

            // 處理選單 (MenuStrip)
            if (ctrl is MenuStrip ms)
            {
                foreach (ToolStripItem item in ms.Items)
                {
                    if (item is ToolStripMenuItem tsmi)
                        ApplyToMenuItem(tsmi);
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

        private static void ApplyToContextMenu(ContextMenuStrip cms)
        {
            foreach (ToolStripItem item in cms.Items)
            {
                if (item is ToolStripMenuItem tsmi)
                    ApplyToMenuItem(tsmi);
            }
        }

        private static void ApplyToMenuItem(ToolStripMenuItem item)
        {
            // 優先使用 Tag
            string key = item.Tag?.ToString();
            if (string.IsNullOrEmpty(key)) key = item.Name;

            string translation = LocalizationService.GetString(key);
            if (!string.IsNullOrEmpty(translation) && translation != key)
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
