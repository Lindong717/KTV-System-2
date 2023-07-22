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
        public Waiter()
        {
            InitializeComponent();
        }

        void select()
        {
            DbHelper.skinDataGridView(skinDataGridView1, "select [Grade number], [Rank name] from [dbo].[Waiter_type]", "");
            DbHelper.skinDataGridView(skinDataGridView2, "select [Waiter_number], [Waiter name], [Jane_spelling], [sex], [level], [Contact], [identity card], [description] from [dbo].[Waiter]", "");
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
        }

        private void skinDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
       
    }
}
