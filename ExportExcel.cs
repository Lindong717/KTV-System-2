using CCWin.SkinControl;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTV_management_system
{
    class ExportExcel
    {
        //导出方法
        public void ExportToExcel(SkinDataGridView dataGridView, string filePath)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // 导出列标题
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dataGridView.Columns[i].HeaderText;
                }

                // 导出数据行
                for (int rowIndex = 0; rowIndex < dataGridView.Rows.Count; rowIndex++)
                {
                    DataGridViewRow row = dataGridView.Rows[rowIndex];
                    for (int columnIndex = 0; columnIndex < row.Cells.Count; columnIndex++)
                    {
                        worksheet.Cells[rowIndex + 2, columnIndex + 1].Value = row.Cells[columnIndex].Value;
                    }
                }

                // 保存Excel文件
                FileInfo file = new FileInfo(filePath);
                package.SaveAs(file);
            }
        }
    }
}