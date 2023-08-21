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
    public partial class Booking_manage : Skin_Mac
    {
        private static string sql = @"select [Customer_number],[Customer_name], [Phone], [state], [type_Name], [Private_room_number], [Arrival_time], [Save_time], [Settling_time], [remark],[Automatic_cancellation] from [dbo].[Appointment_management] as a
        join [dbo].[Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
        where 1=1";
        private static string tmp = sql;

        public Booking_manage()
        {
            InitializeComponent();
        }

        private void Booking_manage_Load(object sender, EventArgs e)
        {
            flushed();
            skinDataGridView1.RowHeadersVisible = false;
        }

        public void flushed()
        {
            DbHelper.skinDataGridView(skinDataGridView1, tmp, "");
            tmp = sql;

            for (int i = 0; i < skinDataGridView1.Rows.Count; i++)
            {
                if (skinDataGridView1.Rows[i].Cells["Column11"].Value.ToString().Trim() == "1")
                {
                    if (DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm")) > DateTime.Parse(skinDataGridView1.Rows[i].Cells["Column7"].Value.ToString()))
                    {
                        DbHelper.executeNonQuery($"update [dbo].[Appointment_management] set [state] = 'N' where [Customer_number] = '{skinDataGridView1.Rows[i].Cells["Column10"].Value}'");
                    }
                }
            }
        }

        public void insert()
        {
            Booking_Recording booking = new Booking_Recording();
            booking.ShowDialog();

            flushed();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            insert();
        }

        public void revise()
        {
            Revise_booking revise = new Revise_booking();

            revise.id = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column10"].Value.ToString();
            revise.name = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value.ToString();
            revise.Phone = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column2"].Value.ToString();
            revise.state = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column3"].Value.ToString();
            revise.remark = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column9"].Value.ToString();
            revise.Private_room_number = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column5"].Value.ToString();
            revise.Automatic_deletion = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column11"].Value.ToString();

            revise.Arrival_time = DateTime.Parse(skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column6"].Value.ToString());
            revise.Retention_time = DateTime.Parse(skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column7"].Value.ToString());

            revise.ShowDialog();

            flushed();
        }

        public void delete()
        {
            if (MessageBox.Show("该操作不可恢复，是否删除？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DbHelper.executeNonQuery($"delete from [dbo].[Appointment_management] where [Customer_number] = '{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column10"].Value}'");
                DbHelper.executeNonQuery($"update [dbo].[Private_rooms] set [Private_room_status] = '0' where [Private_rooms_ID] = '{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column5"].Value}'");
                flushed();
            }
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            delete();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            revise();
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            flushed();
            skinCaptionPanel1.Text = "宾客预订信息";
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            tmp += $" and [Private_room_number] like '%{textBox1.Text}%' or [Customer_name] like '%{textBox1.Text}%'";
            flushed();
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            skinContextMenuStrip1.Show(Cursor.Position);
        }

        private void 今天预订到达的宾客ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmp += $" and DAY([Arrival_time]) = DAY(GETDATE())";
            flushed();
            skinCaptionPanel1.Text = "今天预订到达的宾客";
        }

        private void 明天预订到达的宾客ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmp += $" and DAY([Arrival_time]) = DAY(GETDATE())+1";
            flushed();
            skinCaptionPanel1.Text = "明天预订到达的宾客";
        }

        private void 自定义时间范围ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Custom_time custom = new Custom_time();
            custom.ShowDialog();

            tmp += custom.condition;
            flushed();
            skinCaptionPanel1.Text = "自定义时间范围到达的宾客";
        }

        private void skinButton7_Click(object sender, EventArgs e)
        {
            ExportData();
        }

        public void ExportData()
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ExportExcel ee = new ExportExcel();
                ee.ExportToExcel(skinDataGridView1, saveFileDialog1.FileName);
            }
        }

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportData();
        }

        private void 增加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            insert();
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            revise();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            delete();
        }
    }
}
