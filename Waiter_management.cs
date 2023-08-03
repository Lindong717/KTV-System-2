using CCWin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTV_management_system
{
    public partial class Waiter_management : Skin_Mac
    {
        private static string sql = @"select [Waiter_number],[Waiter name],[sex] = (case [sex] when '0' then '男' when '1' then '女' end),[identity card],[Contact],[type_Name],[Rank name] from [dbo].[Waiter] as a
        join [dbo].[Type_of_private_room] as b on a.region = b.Private_rooms_type_ID
        join [dbo].[Waiter_type] as c on a.level = c.[Grade number]
        where 1=1";
        private static string tmp = sql;

        public Waiter_management()
        {
            InitializeComponent();
        }

        private void Waiter_management_Load(object sender, EventArgs e)
        {
            flushed();
            skinDataGridView1.RowHeadersVisible = false;
        }

        private void flushed()
        {
            DbHelper.skinDataGridView(skinDataGridView1,tmp,"");
            tmp = sql;
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            tmp += $" and [Waiter_number] like '%{textBox1.Text}%' or [Waiter name] like '%{textBox1.Text}%'";
            flushed();
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            flushed();
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            ExportData();
        }

        public void ExportData()
        {
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ExportExcel ee = new ExportExcel();
                ee.ExportToExcel(skinDataGridView1, saveFileDialog1.FileName);
            }
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            skinContextMenuStrip1.Show(Cursor.Position);
        }

        private void 小型包间ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmp += $" and [region] = '7'";
            flushed();
        }

        private void 豪华包间ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmp += $" and [region] = '1'";
            flushed();
        }

        private void 中型包间ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmp += $" and [region] = '3'";
            flushed();
        }

        private void 大型包间ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmp += $" and [region] = '2'";
            flushed();
        }
    }
}
