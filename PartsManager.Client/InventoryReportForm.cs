using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PartsManager.Shared.DTOs;
using PartsManager.Shared.Resources;
using ClosedXML.Excel;

namespace PartsManager.Client
{
    public partial class InventoryReportForm : Form
    {
        private List<InventoryComparisonResultDto> _data;

        public InventoryReportForm(List<InventoryComparisonResultDto> data)
        {
            InitializeComponent();
            UIStyle.Apply(this);
            I18nHelper.Apply(this);
            _data = data;
            
            dgvReport.DataSource = _data;
        }

        private void dgvReport_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvReport.Columns[e.ColumnIndex].Name == "Status")
            {
                string status = e.Value?.ToString();
                if (status == "Missing") e.CellStyle.ForeColor = Color.Red;
                else if (status == "Excess") e.CellStyle.ForeColor = Color.DeepSkyBlue;
                else if (status == "Different") e.CellStyle.ForeColor = Color.Orange;
                else if (status == "Match") e.CellStyle.ForeColor = Color.LimeGreen;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                sfd.FileName = $"InventoryReport_{DateTime.Now:yyyyMMddHHmm}.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExportToExcel(sfd.FileName);
                        MessageBox.Show(LocalizationService.GetString("Msg_ExportSuccess"));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void ExportToExcel(string filePath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("InventoryReport");
                worksheet.Cell(1, 1).Value = "PartNo";
                worksheet.Cell(1, 2).Value = "Name";
                worksheet.Cell(1, 3).Value = "SystemQty";
                worksheet.Cell(1, 4).Value = "ScannedQty";
                worksheet.Cell(1, 5).Value = "Difference";
                worksheet.Cell(1, 6).Value = "Status";

                for (int i = 0; i < _data.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = _data[i].PartNo;
                    worksheet.Cell(i + 2, 2).Value = _data[i].Name;
                    worksheet.Cell(i + 2, 3).Value = _data[i].SystemQty;
                    worksheet.Cell(i + 2, 4).Value = _data[i].ScannedQty;
                    worksheet.Cell(i + 2, 5).Value = _data[i].Difference;
                    worksheet.Cell(i + 2, 6).Value = _data[i].Status;
                }
                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(filePath);
            }
        }
    }
}
