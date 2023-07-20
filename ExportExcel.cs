using Microsoft.Office.Interop.Excel;
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
        public void ExcelExport(ListView lv, string fileName)
        {
            int rowNum = lv.Items.Count;
            int column = lv.Items[0].SubItems.Count;
            int rowIndex = 1;
            int columnIndex = 0;

            if (rowNum == 0 || string.IsNullOrEmpty(fileName))
            {
                return;
            }

            if (rowNum > 0)
            {
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                if (xlApp == null)
                {
                    MessageBox.Show("无法创建excel");
                    return;
                }
                xlApp.DefaultFilePath = "";
                xlApp.DisplayAlerts = true;
                xlApp.SheetsInNewWorkbook = 1;
                Workbook xlBook = xlApp.Workbooks.Add(true);
                //将ListView的列名导入Excel表的第一行
                foreach (ColumnHeader dc in lv.Columns)
                {
                    columnIndex++;
                    xlApp.Cells[rowIndex, columnIndex] = dc.Text;
                }
                //将ListView中的数据导入到Excel中
                for (rowIndex = 2; rowIndex < lv.Items.Count + 2; rowIndex++)
                {
                    xlApp.Cells[rowIndex, 1] = lv.Items[rowIndex - 2].Text;
                    for (columnIndex = 2; columnIndex <= lv.Columns.Count; columnIndex++)
                    {
                        xlApp.Cells[rowIndex, columnIndex] = lv.Items[rowIndex - 2].SubItems[columnIndex - 1].Text;
                    }
                }

                //例外需要说明的是用strFileName,Excel.XlFileFormat.xlExcel9795保存方式时 当你的Excel版本不是95、97 而是2003、2007 时导出的时候会报一个错误：异常来自 HRESULT:0x800A03EC。 解决办法就是换成strFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal。 
                xlBook.SaveAs(fileName, XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                xlApp = null;
                xlBook = null;
                MessageBox.Show("导出成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}