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
    public partial class Redeem_goods : Skin_Mac
    {
        public string ID;
        public string Private_room_number;

        private static string sql = "select [project_ID],[Name],[Pinyin],[Preset_unit_price],[Repository],[Redeem_points] from [dbo].[Commodity] where [exchange] = 'Y'";
        private string Waiter;

        public Redeem_goods()
        {
            InitializeComponent();
        }

        private void Redeem_goods_Load(object sender, EventArgs e)
        {
            Login_member login = new Login_member();
            login.Dispose();

            skinLabel9.Text = ID;
            skinLabel7.Text = Private_room_number;

            skinCaptionPanel2.Text = $@"{Private_room_number} 消费清单（包间打折{DbHelper.executeScalar($@"select [Fold_rate] from [dbo].[Private_rooms] as a
                join[dbo].[Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
                where[Private_rooms_ID] = '{Private_room_number}'")}）";

            skinDataGridView2.Focus();
            skinDataGridView1.RowHeadersVisible = false;
            skinDataGridView2.RowHeadersVisible = false;

            DbHelper.exchangeTree(skinTreeView1, "select [CommodityTypeID],[TypeName] from [dbo].[commodityType]");

            Inquire();
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            metroTabControl1.SelectedTab = metroTabControl1.SelectedTab == metroTabPage1 ? metroTabPage2 : metroTabPage1;
            skinButton3.Text = metroTabControl1.SelectedTab == metroTabPage1 ? "列表" : "清单";

        }

        private void Inquire()
        {
            skinLabel10.Text = DbHelper.executeScalar($"select [Integral] from [dbo].[Member_Information] where [InformationID] = '{ID}'");
            DbHelper.skinDataGridView(skinDataGridView1, sql, "");
            DbHelper.skinDataGridView(skinDataGridView2, $"select * from [dbo].[Consumption_list] where [Private_room] = '{Private_room_number}'", "");
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void skinDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Waiter = "*";

            if (numericUpDown1.Value <= 0)
            {
                MessageBox.Show("数量不能小于或等于0", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Convert.ToInt32(DbHelper.executeScalar($"select [Integral] from [dbo].[Member_Information] where [InformationID] = '{ID}'")) < Convert.ToInt32(skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column18"].Value) * numericUpDown1.Value)
            {
                MessageBox.Show("积分不足", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Convert.ToInt32(DbHelper.executeScalar($"select [Repository] from [dbo].[Commodity] where [project_ID] = '{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value}'")) < numericUpDown1.Value)
            {
                MessageBox.Show("存库不足", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (DbHelper.executeScalar($"select [category_ID] from [dbo].[Commodity] where [project_ID] = '{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value}'") == "1")
            {
                Select_Waiter select_Waiter = new Select_Waiter();
                select_Waiter.ShowDialog();

                Waiter = select_Waiter.WaiterName;

                if (string.IsNullOrEmpty(Waiter))
                {
                    return;
                }
            }

            string Fold_rate = DbHelper.executeScalar($@"select [Fold_rate] from [dbo].[Private_rooms] as a
                join[dbo].[Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
                where [Private_rooms_ID] = '{Private_room_number}'");

            DbHelper.executeNonQuery($@"insert into [dbo].[Consumption_list]([Private_room], [project_ID], [Pinyin], [unit_price], [Fold_rate], [quantity], [amount], [Crediting_time], [Waiter], [Bookkeeper], [remark])
            values('{Private_room_number}','{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column2"].Value}(兑换)','{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column5"].Value}','0','{Fold_rate}','{numericUpDown1.Value}','0',GETDATE(),'{(string.IsNullOrEmpty(Waiter) ? "*" : Waiter)}','*','*')");

            DbHelper.executeNonQuery($"update [dbo].[Commodity] set [Repository] -= '{numericUpDown1.Value}' where [project_ID] = '{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value}'");

            DbHelper.executeNonQuery($"update [dbo].[Member_Information] set [Integral] -= '{(Convert.ToInt32(skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column18"].Value) * numericUpDown1.Value)}' where [InformationID] = '{ID}'");

            Inquire();
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            give();
        }

        private void 消费赠单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            give();
        }

        public void give()
        {
            try
            {
                if (skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column7"].Value.ToString() == "包间费用")
                {
                    MessageBox.Show("包间费用不可以赠送", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DbHelper.executeNonQuery($"update [dbo].[Consumption_list] set [project_ID] += '(赠)',[unit_price] = '0',[amount] = '0' where [manifestID] = '{skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column16"].Value}'");
                Inquire();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            delete();
        }

        private void 消费退单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            delete();
        }

        private void delete()
        {
            try
            {
                if (skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column7"].Value.ToString() == "包间费用")
                {
                    MessageBox.Show("包间费用不可以退单", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DbHelper.executeNonQuery($"delete from [dbo].[Consumption_list] where [manifestID] = '{skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column16"].Value}'");
                Inquire();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            remark();
        }

        private void 消费备注ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            remark();
        }

        public void remark()
        {
            Note_information note = new Note_information();

            note.commodityID = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column16"].Value.ToString();
            note.commodityName = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column7"].Value.ToString();
            note.commodityRemark = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column15"].Value.ToString();

            note.ShowDialog();
            Inquire();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DbHelper.skinDataGridView(skinDataGridView1, sql, textBox1.Text);
        }

        private void skinTreeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = skinTreeView1.SelectedNode;

            if (node != null && node.Parent == null)
            {
                return;
            }

            Waiter = "*";

            if (Convert.ToInt32(DbHelper.executeScalar($"select [Repository] from [dbo].[Commodity] where [project_ID] = '{skinTreeView1.SelectedNode.Tag}'")) <= 0)
            {
                MessageBox.Show("该项目已经没有存库", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Convert.ToInt32(DbHelper.executeScalar($"select [Redeem_points] from [dbo].[Commodity] where [project_ID] = '{skinTreeView1.SelectedNode.Tag}'")) > Convert.ToInt32(skinLabel10.Text))
            {
                MessageBox.Show("积分不足", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (DbHelper.executeScalar($"select [category_ID] from [dbo].[Commodity] where [project_ID] = '{skinTreeView1.SelectedNode.Tag}'") == "1")
            {
                Select_Waiter select_Waiter = new Select_Waiter();
                select_Waiter.ShowDialog();

                Waiter = select_Waiter.WaiterName;

                if (string.IsNullOrEmpty(Waiter))
                {
                    return;
                }
            }

            string Fold_rate = DbHelper.executeScalar($@"select [Fold_rate] from [dbo].[Private_rooms] as a
                join[dbo].[Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
                where [Private_rooms_ID] = '{Private_room_number}'");

            DbHelper.executeNonQuery($@"insert into [dbo].[Consumption_list]([Private_room], [project_ID], [Pinyin], [unit_price], [Fold_rate], [quantity], [amount], [Crediting_time], [Waiter], [Bookkeeper], [remark])
            values('{Private_room_number}','{DbHelper.executeScalar($"select [Name] from [dbo].[Commodity] where [project_ID] = '{skinTreeView1.SelectedNode.Tag}'")}(兑换)','{DbHelper.executeScalar($"select [Pinyin] from [dbo].[Commodity] where [project_ID] = '{skinTreeView1.SelectedNode.Tag}'")}','0','{Fold_rate}','1','0',GETDATE(),'{(string.IsNullOrEmpty(Waiter) ? "*" : Waiter)}','*','*')");

            Inquire();
        }
    }
}
