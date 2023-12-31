﻿using CCWin;
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
    public partial class Increase_consumption : Skin_Mac
    {
        public string Private_rooms_ID;
        private string sql = "select [project_ID],[Name],[Pinyin],[Preset_unit_price],[Repository] from [dbo].[Commodity]";
        private string Waiter;

        public Increase_consumption()
        {
            InitializeComponent();
        }

        private void Increase_consumption_Load(object sender, EventArgs e)
        {
            Inquire();

            DbHelper.Tree(skinTreeView1, "select [CommodityTypeID],[TypeName] from [dbo].[commodityType]");

            skinLabel7.Text = Private_rooms_ID;
            skinCaptionPanel2.Text = $@"{Private_rooms_ID} 消费清单（包间打折{DbHelper.executeScalar($@"select [Fold_rate] from [dbo].[Private_rooms] as a
                join[dbo].[Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
                where[Private_rooms_ID] = '{Private_rooms_ID}'")}）";

            skinDataGridView2.Focus();
            skinDataGridView1.RowHeadersVisible = false;
            skinDataGridView2.RowHeadersVisible = false;
        }

        private void skinTreeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // 恢复之前被点击的节点字体颜色
            if (skinTreeView1.SelectedNode != null)
            {
                skinTreeView1.SelectedNode.ForeColor = Color.Black;
            }

            // 设置当前被点击的节点字体颜色
            skinTreeView1.SelectedNode = e.Node;
            skinTreeView1.SelectedNode.ForeColor = Color.Red;

            // 更新控件显示
            skinTreeView1.Invalidate();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DbHelper.skinDataGridView(skinDataGridView1, sql, textBox1.Text);
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            metroTabControl1.SelectedTab = metroTabControl1.SelectedTab == metroTabPage1 ? metroTabPage2 : metroTabPage1;
            skinButton3.Text = metroTabControl1.SelectedTab == metroTabPage1 ? "列表" : "清单";
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void Inquire()
        {
            DbHelper.skinDataGridView(skinDataGridView1, sql, "");
            DbHelper.skinDataGridView(skinDataGridView2, $"select * from [dbo].[Consumption_list] where [Private_room] = '{Private_rooms_ID}'","");
        }

        private void skinDataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                Waiter = "*";

                if (numericUpDown1.Value <= 0)
                {
                    MessageBox.Show("数量不能小于或等于0", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if ((int)skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column4"].Value <= 0)
                {
                    MessageBox.Show("该项目已经没有存库", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if ((int)skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column4"].Value - numericUpDown1.Value < 0)
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

                string i = DbHelper.executeScalar($"select [Preset_unit_price] from [dbo].[Commodity] where [project_ID] = '{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value}'");

                string Fold_rate = DbHelper.executeScalar($@"select [Fold_rate] from [dbo].[Private_rooms] as a
                join[dbo].[Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
                where [Private_rooms_ID] = '{Private_rooms_ID}'");

                DbHelper.executeNonQuery($@"insert into [dbo].[Consumption_list]([Private_room], [project_ID],[Pinyin], [unit_price], [Fold_rate], [quantity], [amount], [Crediting_time], [Waiter], [Bookkeeper], [remark])
                values('{Private_rooms_ID}','{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column2"].Value}','{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column5"].Value}','{i}','{Fold_rate}','{numericUpDown1.Value}','{(float.Parse(i) * float.Parse(Fold_rate)) * float.Parse(numericUpDown1.Value.ToString())}',GETDATE(),'{Waiter}','*','*')");

                DbHelper.executeNonQuery($"update [dbo].[Commodity] set [Repository] -= '{numericUpDown1.Value}' where [project_ID] = '{skinDataGridView1.Rows[skinDataGridView1.CurrentCell.RowIndex].Cells["Column1"].Value}'");

                Inquire();
            }catch(Exception ee)
            {
                MessageBox.Show(ee.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void skinTreeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
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

                string i = DbHelper.executeScalar($"select [Preset_unit_price] from [dbo].[Commodity] where [project_ID] = '{skinTreeView1.SelectedNode.Tag}'");

                string Fold_rate = DbHelper.executeScalar($@"select [Fold_rate] from [dbo].[Private_rooms] as a
                join[dbo].[Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
                where [Private_rooms_ID] = '{Private_rooms_ID}'");

                DbHelper.executeNonQuery($@"insert into [dbo].[Consumption_list]([Private_room], [project_ID],[Pinyin], [unit_price], [Fold_rate], [quantity], [amount], [Crediting_time], [Waiter], [Bookkeeper], [remark])
                values('{Private_rooms_ID}','{DbHelper.executeScalar($"select [Name] from [dbo].[Commodity] where [project_ID] = '{skinTreeView1.SelectedNode.Tag}'")}','{DbHelper.executeScalar($"select [Pinyin] from [dbo].[Commodity] where [project_ID] = '{skinTreeView1.SelectedNode.Tag}'")}','{i}','{Fold_rate}','1','{(float.Parse(i) * float.Parse(Fold_rate)) * float.Parse(numericUpDown1.Value.ToString())}',GETDATE(),'{Waiter}','*','*')");

                DbHelper.executeNonQuery($"update [dbo].[Commodity] set [Repository] -= '1'");

                Inquire();
            }catch(Exception ee)
            {
                MessageBox.Show(ee.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Chargebacks()
        {
            DbHelper.skinDataGridView(skinDataGridView2, $"select * from [dbo].[Consumption_list] where [Private_room] = '{Private_rooms_ID}'", textBox2.Text);
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

                if (skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column8"].Value.ToString() == "0")
                {
                    MessageBox.Show("不能对赠单以及退单进行重复操作", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public void remark()
        {
            Note_information note = new Note_information();

            note.commodityID = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column16"].Value.ToString();
            note.commodityName = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column7"].Value.ToString();
            note.commodityRemark = skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column15"].Value.ToString();

            note.ShowDialog();
            Inquire();
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            remark();
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            give();
        }

        private void skinButton5_Click(object sender, EventArgs e)
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

                if (skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column8"].Value.ToString() == "0")
                {
                    MessageBox.Show("不能对赠单以及退单进行重复操作", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DbHelper.executeNonQuery($@"update [dbo].[Consumption_list] set
                [project_ID] += '(退)',
                [unit_price] = '0',
                [amount] = '0'
                where [manifestID] = '{skinDataGridView2.Rows[skinDataGridView2.CurrentCell.RowIndex].Cells["Column16"].Value}'");

                Inquire();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Chargebacks();
        }

        private void 消费退单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            delete();
        }

        private void 消费赠单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            give();
        }

        private void 消费备注ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            remark();
        }
    }
}
