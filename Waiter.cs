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
    public partial class Waiter : Form

    {
        private static string sql = @"select [Grade number], [Rank name] = (case [Rank name] when '1' then '普通服务生' when '2' then '中级服务生' when '3' then '高级服务生'
            when '4' then '究极服务生' when '5' then '终极服务生')  from [dbo].[Waiter_type] where 1=1";
        private static string tmp = sql;
        public Waiter()
        {
            InitializeComponent();
        }

        void select()
        {
            DbHelper.skinDataGridView(skinDataGridView1, "select [Grade number], [Rank name] from [dbo].[Waiter_type]", "");
            DbHelper.skinDataGridView(skinDataGridView2, @"select [Waiter_number], [Waiter name], [Jane_spelling], [sex], [Rank name], [Contact], [identity card], [type_Name],[description] from [dbo].[Waiter] a
            join [dbo].[Waiter_type] b on a.level=b.[Grade number]
            join [dbo].[Type_of_private_room] c on a.[Service Area] = c.Private_rooms_type_ID", "");
    }

        private void skinPanel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void skinPanel2_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            increase i = new increase();
            i.ShowDialog();
            select();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            modify m = new modify();

            m.ID = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value.ToString();
            m.level = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column2"].Value.ToString();

            m.ShowDialog();
            select();

        }

        private void Waiter_Load(object sender, EventArgs e)
        {

            DbHelper.skinCollections(skinComboBox1, "select [Grade number], [Rank name] from [dbo].[Waiter_type]", "Grade number", "Rank name", "请选择");

            select();
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            int i = DbHelper.executeNonQuery($@"select count(*) from [dbo].[Waiter] where [level] = '{(skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value)}'");
            

            if (i <= 0)
            {
                DbHelper.executeNonQuery($@"delete from [dbo].[Waiter_type] where [Grade number]= '{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value}'");
            }
            else
            {
                MessageBox.Show("服务生表中正在使用此等级，不能删除！");
            }
            DbHelper.skinDataGridView(skinDataGridView1, "select [Grade number], [Rank name] from [dbo].[Waiter_type]", "");
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            increase2 increase = new increase2();
            increase.ShowDialog();
            select();
        }

        private void skinDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            modify2 modify = new modify2();

            modify.id = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column3"].Value.ToString();
            modify.name = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column4"].Value.ToString();
            modify.jane = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column5"].Value.ToString();
            modify.sex = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column6"].Value.ToString();
            modify.type = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column8"].Value.ToString();
            modify.Rank = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column9"].Value.ToString();
            modify.phone = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column10"].Value.ToString();
            modify.idcard = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column11"].Value.ToString();
            modify.miaoshu = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column7"].Value.ToString();

            modify.ShowDialog();
            select();
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定删除吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DbHelper.executeNonQuery($@"delete from [dbo].[Waiter] where [Waiter_number]= '{skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column3"].Value}'");
            }
            else if (result == DialogResult.No)
            {
                Close();
            }
            select();
        }

        private void skinComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
            if (Convert.ToInt32(skinComboBox1.SelectedValue) > 0)
            {
                tmp += $" and a.[Grade number] = '{skinComboBox1.SelectedValue}'";
                sx();
                return;
            }
            sx();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            
        }

        public void sx()
        {
            DbHelper.skinDataGridView(skinDataGridView2, tmp, "");

            tmp = sql;
        }

        private void skinDataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
