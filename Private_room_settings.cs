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
    public partial class Private_room_settings : Form
    {
        private static string sql = @"select [Private_rooms_ID],[type_Name],[Private_room_status] = (case [Private_room_status] when '0' then '可供' when '1' then '占用' when '2' then '停用' when '3' then '预订' end),[location] from [dbo].[Private_rooms] as a
        join [dbo].[Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
        where 1=1";
        private static string tmp = sql;

        public Private_room_settings()
        {
            InitializeComponent();
        }

        private void skinComboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (Convert.ToInt32(skinComboBox1.SelectedValue) > 0)
            {
                tmp += $" and a.[Private_rooms_type] = '{skinComboBox1.SelectedValue}'";
                Inquire();
                return;
            }

            Inquire();
        }

        private void Inquire()
        {
            DbHelper.skinDataGridView(skinDataGridView2, tmp,"");
            tmp = sql;
        }

        public void revise_method()
        {
            Modify_the_private_room modify_The_Private_Room = new Modify_the_private_room();

            modify_The_Private_Room.Type = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column2"].Value.ToString();
            modify_The_Private_Room.id = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column1"].Value.ToString();
            modify_The_Private_Room.region = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column4"].Value.ToString();

            modify_The_Private_Room.ShowDialog();

            Inquire();
        }

        public void revise_type()
        {
            Modify_the_private_room_type modify = new Modify_the_private_room_type();

            modify.Type = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column5"].Value.ToString();
            modify.Minimum_consumption = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column6"].Value.ToString();
            modify.Capacity = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column8"].Value.ToString();
            modify.Ordinary_festivals = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column9"].Value.ToString();
            modify.Special_holidays = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column10"].Value.ToString();
            modify.Discount_conditions = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column11"].Value.ToString();
            modify.Fold_rate = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column12"].Value.ToString();
            modify.Standard_holidays = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column7"].Value.ToString();

            modify.ShowDialog();
            Private_room_type_refresh();
        }

        public void method_Individually()
        {
            Individually individually = new Individually();
            individually.ShowDialog();

            Inquire();
        }

        public void delete_method()
        {
            if (MessageBox.Show("该操作不可恢复，是否要执行删除？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DbHelper.executeNonQuery($"delete from [dbo].[Private_rooms] where [Private_rooms_ID] = '{skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column1"].Value}'");
                Inquire();
            }
        }

        public void Add_type()
        {
            Add_a_private_room_type add_A_Private_Room_Type = new Add_a_private_room_type();
            add_A_Private_Room_Type.ShowDialog();

            Private_room_type_refresh();
        }

        public void Private_room_type_refresh()
        {
            DbHelper.skinDataGridView(skinDataGridView1, @"select [type_Name],[Minimum_consumption],b.[manner_Name],[Capacity],c.[manner_Name] as 'Ordinary_festivals',d.[manner_Name] as 'Special_holidays',[Discount_conditions],[Fold_rate] from [dbo].[Type_of_private_room] as a
            left join[dbo].[Billing_type] as b on a.Billing_method = b.manner_ID
            left join[dbo].[Billing_type] as c on a.Ordinary_festivals = c.manner_ID
            left join[dbo].[Billing_type] as d on a.Special_holidays = d.manner_ID","");
        }

        public void delete_type()
        {
            if (MessageBox.Show("该操作会将相关数据全部删除并且不可恢复，是否要执行删除？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DbHelper.executeNonQuery($"delete from [dbo].[Private_rooms] where [Private_rooms_type] = '{DbHelper.executeScalar($"select [Private_rooms_type_ID] from [dbo].[Type_of_private_room] where [type_Name] = '{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column5"].Value}'")}'");
                DbHelper.executeNonQuery($"delete from [dbo].[Type_of_private_room] where [type_Name] = '{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column5"].Value}'");

                Private_room_type_refresh();
                Inquire();
            }
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            Batch batch = new Batch();
            batch.ShowDialog();

            Inquire();
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            method_Individually();
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            revise_method();
        }

        private void skinButton7_Click(object sender, EventArgs e)
        {
            delete_method();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            Add_type();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            revise_type();
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            delete_type();
        }

        private void Private_room_settings_Load_1(object sender, EventArgs e)
        {
            Private_room_type_refresh();
            Inquire();
            DbHelper.skinCollections(skinComboBox1, "select [Private_rooms_type_ID],[type_Name] from [dbo].[Type_of_private_room]", "Private_rooms_type_ID", "type_Name", "所有包间");

            skinDataGridView1.RowHeadersVisible = false;
            skinDataGridView2.RowHeadersVisible = false;
        }
    }
}
