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
        private static string sql = @"select [InformationID],[memberName],[memberSex] = (case [memberSex] when '0' then '男' when '1' then '女' end),[Card_type] = (case [Card_type] when '1' then '折扣卡' when '0' then '储值卡' end),[Card_balance],[TypeName],[Integral],[Amount_spent],[birthday],[Phone],[enroll_date],[state] = (case [state] when '1' then '停用' when '0' then '可用' end),[remark],[password] from [dbo].[Member_Information] as a
            join [dbo].[Member] as b on a.[Membership_Level] = b.memberID
            where 1=1";
        private static string tmp = sql;

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
            skinDataGridView1.RowHeadersVisible = false;
        }

        public void flushed()
        {
            DbHelper.skinDataGridView(skinDataGridView1, tmp, "");
            tmp = sql;
        }

        public void revise()
        {
            Revise_Information revise = new Revise_Information
            {
                Membership_Number = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value.ToString(),
                Member_Name = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column2"].Value.ToString(),
                MemberSex = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column3"].Value.ToString(),
                birthday = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column10"].Value.ToString(),
                Phone = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column11"].Value.ToString(),
                state = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column13"].Value.ToString(),
                cardType = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column4"].Value.ToString(),
                Membership_Level = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column6"].Value.ToString(),
                Integral = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column7"].Value.ToString(),
                pwd = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column9"].Value.ToString(),
                remark = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column14"].Value.ToString()
            };

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

            Recharge recharge = new Recharge
            {
                Collections = skinDataGridView1.SelectedRows,
                Membership_Number = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value.ToString(),
                Member_Name = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column2"].Value.ToString(),
                balance = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column5"].Value.ToString(),
                Membership_Level = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column6"].Value.ToString()
            };
            recharge.ShowDialog();

            flushed();
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            Revise_password revise = new Revise_password
            {
                Membership_Number = skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value.ToString()
            };
            revise.ShowDialog();
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                flushed();
                return;
            }

            tmp += $" and [InformationID] like '%{textBox1.Text}%' or [memberName] like '%{textBox1.Text}%'";
            flushed();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            tmp += $" and [Membership_Level] = '12618761'";
            flushed();
        }

        private void 中级会员ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmp += $" and [Membership_Level] = '29659121'";
            flushed();
        }

        private void 高级会员ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tmp += $" and [Membership_Level] = '77142263'";
            flushed();
        }

        private void skinButton8_Click(object sender, EventArgs e)
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
    }
}
