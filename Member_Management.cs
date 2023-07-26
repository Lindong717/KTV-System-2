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
    public partial class Member_Management : Skin_Mac
    {
        public Member_Management()
        {
            InitializeComponent();
        }

        private void skinButton7_Click(object sender, EventArgs e)
        {
            skinContextMenuStrip1.Show(Cursor.Position);
        }

        private void Member_Management_Load(object sender, EventArgs e)
        {
            flushed();
        }

        public void flushed()
        {
            DbHelper.skinDataGridView(skinDataGridView1, @"select [InformationID],[memberName],[memberSex] = (case [memberSex] when '0' then '男' when '1' then '女' end),[Card_type] = (case [Card_type] when '1' then '折扣卡' when '0' then '储值卡' end),[Card_balance],[TypeName],[Integral],[Amount_spent],[birthday],[Phone],[enroll_date],[state] = (case [state] when '1' then '停用' when '0' then '可用' end),[remark],[password] from [dbo].[Member_Information] as a
            join [dbo].[Member] as b on a.[Membership_Level] = b.memberID", "");
        }

        public void revise()
        {
            Revise_Information revise = new Revise_Information();

            revise.Membership_Number = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value.ToString();
            revise.Member_Name = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column2"].Value.ToString();
            revise.MemberSex = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column3"].Value.ToString();
            revise.birthday = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column10"].Value.ToString();
            revise.Phone = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column11"].Value.ToString();
            revise.state = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column13"].Value.ToString();
            revise.cardType = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column4"].Value.ToString();
            revise.Membership_Level = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column6"].Value.ToString();
            revise.Integral = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column7"].Value.ToString();
            revise.pwd = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column9"].Value.ToString();
            revise.remark = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column14"].Value.ToString();

            revise.ShowDialog();
            flushed();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            Increase_membership increase = new Increase_membership();
            increase.ShowDialog();

            flushed();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            revise();
        }

        private void skinDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            revise();
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("该操作不可恢复，是否要继续？","系统提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DbHelper.executeNonQuery($"delete from [dbo].[Member_Information] where [InformationID] = '{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value}'");
                flushed();
            }
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            if (skinDataGridView1.SelectedRows.Count == 1)
            {
                if (skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column4"].Value.ToString() == "折扣卡")
                {
                    MessageBox.Show("只能对储值卡充值", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            Recharge recharge = new Recharge();
            recharge.Collections = skinDataGridView1.SelectedRows;
            recharge.Membership_Number = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value.ToString();
            recharge.Member_Name = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column2"].Value.ToString();
            recharge.balance = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column5"].Value.ToString();
            recharge.Membership_Level = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column6"].Value.ToString();
            recharge.ShowDialog();

            flushed();
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            Revise_password revise = new Revise_password();
            revise.Membership_Number = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value.ToString();
            revise.ShowDialog();
        }
    }
}
