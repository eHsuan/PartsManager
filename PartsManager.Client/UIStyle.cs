using System;
using System.Drawing;
using System.Windows.Forms;

namespace PartsManager.Client
{
    public static class UIStyle
    {
        // 色彩定義
        public static readonly Color BackColor = ColorTranslator.FromHtml("#1E1E1E");
        public static readonly Color ForeColor = Color.White;
        public static readonly Color ControlBackColor = ColorTranslator.FromHtml("#2D2D30");
        public static readonly Color AccentInbound = ColorTranslator.FromHtml("#4CAF50");
        public static readonly Color AccentOutbound = ColorTranslator.FromHtml("#FF9800");
        public static readonly Color ErrorColor = ColorTranslator.FromHtml("#F44336");
        public static readonly Color StatusOkColor = Color.Lime;
        public static readonly Color StatusErrorColor = Color.Red;

        // 字體定義
        public static readonly Font DataFont;
        public static readonly Font LabelFont;

        static UIStyle()
        {
            try
            {
                DataFont = new Font("Consolas", 14, FontStyle.Bold);
            }
            catch
            {
                DataFont = new Font("Courier New", 14, FontStyle.Bold);
            }

            LabelFont = new Font("Microsoft JhengHei", 12, FontStyle.Regular);
        }

        /// <summary>
        /// 套用深色主題至表單
        /// </summary>
        public static void Apply(Form form)
        {
            form.BackColor = BackColor;
            form.ForeColor = ForeColor;

            foreach (Control ctrl in form.Controls)
            {
                ApplyControlTheme(ctrl);
            }
        }

        private static void ApplyControlTheme(Control ctrl)
        {
            if (ctrl is TextBox txt)
            {
                txt.BackColor = ControlBackColor;
                txt.ForeColor = ForeColor;
                txt.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (ctrl is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = ControlBackColor;
                btn.ForeColor = ForeColor;
            }
            else if (ctrl is Label lbl)
            {
                lbl.ForeColor = ForeColor;
            }
            else if (ctrl is DataGridView dgv)
            {
                dgv.DefaultCellStyle.ForeColor = Color.Black;
                dgv.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dgv.RowHeadersDefaultCellStyle.ForeColor = Color.Black;
            }
            
            if (ctrl.HasChildren)
            {
                foreach (Control child in ctrl.Controls)
                {
                    ApplyControlTheme(child);
                }
            }
        }
    }
}
