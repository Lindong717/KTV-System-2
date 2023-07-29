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
        private static string sql = $@"select [Waiter_number],[Waiter name],[Jane_spelling],[sex] = (case [sex] when '1' then '女' when '0' then '男' end),[type_Name],[Rank name],[Contact],[identity card],[description] from [dbo].[Waiter] as a
        join [dbo].[Type_of_private_room] as b on a.region = b.Private_rooms_type_ID
        join [dbo].[Waiter_type] as c on a.level = c.[Grade number]
        where 1=1";
        private static string tmp = sql;

        public Waiter()
        {
            InitializeComponent();
        }

        private void Waiter_Load(object sender, EventArgs e)
        {
            skinDataGridView1.RowHeadersVisible = false;
            skinDataGridView2.RowHeadersVisible = false;

            DbHelper.skinCollections(skinComboBox1, "select [Grade number],[Rank name] from [dbo].[Waiter_type]", "Grade number", "Rank name", "所有等级");
            flushed();
        }

        private void flushed()
        {
            DbHelper.skinDataGridView(skinDataGridView1, "select [Grade number],[Rank name] from [dbo].[Waiter_type]", "");
            DbHelper.skinDataGridView(skinDataGridView2,tmp,"");
            tmp = sql;
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            add_Type();
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_Type();
        }

        private void add_Type()
        {
            Add_waiter_type add = new Add_waiter_type();
            add.ShowDialog();

            flushed();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            revise_Type();
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            revise_Type();
        }

        private void revise_Type()
        {
            Revise_waiter_type revise = new Revise_waiter_type();
            revise.WaiterID = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value.ToString();
            revise.Rank_name = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column2"].Value.ToString();
            revise.ShowDialog();

            flushed();
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            delete_Type();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            delete_Type();
        }

        private void delete_Type()
        {
            if (DbHelper.executeScalar($"select count(*) from [dbo].[Waiter] where [level] = '{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value}'") != "0")
            {
                MessageBox.Show("有服务生正在使用该等级，不可删除", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("该操作不可恢复，是否删除？","系统提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DbHelper.executeNonQuery($"delete from [dbo].[Waiter_type] where [Grade number] = '{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value}'");
            }

            flushed();
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            add_Waiter();
        }

        private void 添加ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            add_Waiter();
        }

        private void add_Waiter()
        {
            Add_waiter add =  new Add_waiter();
            add.ShowDialog();

            flushed();
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            revise_Waiter();
        }

        private void 修改ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            revise_Waiter();
        }

        private void revise_Waiter()
        {
            Revise_Waiter revise = new Revise_Waiter { 
                id = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column3"].Value.ToString(),
                name = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column4"].Value.ToString(),
                Pinyin = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column5"].Value.ToString(),
                identity_card = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column10"].Value.ToString(),
                description = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column11"].Value.ToString(),
                region = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column7"].Value.ToString(),
                grade = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column8"].Value.ToString(),
                Phone = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column9"].Value.ToString(),
                sex = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column6"].Value.ToString()
            };

            revise.ShowDialog();
            flushed();
        }

        private void skinComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((int)skinComboBox1.SelectedValue > 0)
            {
                tmp += $" and [level] = '{skinComboBox1.SelectedValue}'";
            }

            flushed();
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            delete_Waiter();
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            delete_Waiter();
        }

        private void delete_Waiter()
        {
            if (MessageBox.Show("该操作不可恢复，是否删除？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DbHelper.executeNonQuery($"delete from [dbo].[Waiter] where [Waiter_number] = '{skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column3"].Value}'");
            }

            flushed();
        }
    }
}
