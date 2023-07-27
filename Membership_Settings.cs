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
    public partial class Membership_Settings : Form
    {
        public Membership_Settings()
        {
            InitializeComponent();
        }

        public void Member_refresh()
        {
            DbHelper.skinDataGridView(skinDataGridView3, "select [memberID], [TypeName], [Initial_Points], [Fold_rate] from [dbo].[Member]","");
        }

        public void Member_Deletion()
        {
            if (MessageBox.Show("该操作不可恢复，是否要执行删除？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DbHelper.executeNonQuery($"delete from [dbo].[Member] where [TypeName] = '{skinDataGridView3.Rows[skinDataGridView3.CurrentCell.RowIndex].Cells["Column10"].Value}'");
                Member_refresh();
            }
        }

        private void skinButton10_Click(object sender, EventArgs e)
        {
            Modify_Membership modify_Membership = new Modify_Membership();

            modify_Membership.Membership_Level = skinDataGridView3.Rows[skinDataGridView3.CurrentCell.RowIndex].Cells["Column10"].Value.ToString();
            modify_Membership.Membership_Points = skinDataGridView3.Rows[skinDataGridView3.CurrentCell.RowIndex].Cells["Column11"].Value.ToString();
            modify_Membership.Membership_discount = skinDataGridView3.Rows[skinDataGridView3.CurrentCell.RowIndex].Cells["Column12"].Value.ToString();

            modify_Membership.ShowDialog();
            Member_refresh();
        }

        private void skinButton9_Click(object sender, EventArgs e)
        {
            Member_Deletion();
        }

        private void Membership_Settings_Load(object sender, EventArgs e)
        {
            Member_refresh();
            skinDataGridView3.RowHeadersVisible = false;
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            Add_member add_Member = new Add_member();
            add_Member.ShowDialog();

            Member_refresh();
        }
    }
}
