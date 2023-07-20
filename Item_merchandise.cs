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
    public partial class Item_merchandise : Form
    {
        private static string sql = @"select [project_ID],[Name],[Pinyin],[unit],[Preset_unit_price],[cost],[TypeName],[Repository],[exchange],[Redeem_points] from [dbo].[Commodity] as a
            join[dbo].[commodityType] as b on a.category_ID = b.CommodityTypeID
            where 1=1";
        private static string tmp = sql;

        public Item_merchandise()
        {
            InitializeComponent();
        }

        public void commodity_flushed()
        {
            DbHelper.skinDataGridView(skinDataGridView4, "select [CommodityTypeID], [TypeName], [Waiter] = (case [Waiter] when '1' then '需要' when '0' then '不需要' end) from [dbo].[commodityType]","");
            
            DbHelper.skinDataGridView(skinDataGridView5, tmp,"");

            tmp = sql;
        }

        private void Item_merchandise_Load(object sender, EventArgs e)
        {
            commodity_flushed();

            DbHelper.skinCollections(skinComboBox2, "select [CommodityTypeID],[TypeName] from [dbo].[commodityType]", "CommodityTypeID", "TypeName", "所有项目");
        }

        private void skinComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(skinComboBox2.SelectedValue) > 0)
            {
                tmp += $" and a.[category_ID] = '{skinComboBox2.SelectedValue}'";
                commodity_flushed();
                return;
            }
            commodity_flushed();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DbHelper.skinDataGridView(skinDataGridView5, sql, textBox1.Text);
        }

        //---------------------------------------------------------------------

        private void Add_Type()
        {
            Add_Product_type add_Product_Type = new Add_Product_type();
            add_Product_Type.ShowDialog();

            commodity_flushed();
        }

        private void skinButton12_Click(object sender, EventArgs e)
        {
            Add_Type();
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add_Type();
        }

        //---------------------------------------------------------------------

        private void revise_Type()
        {
            revise_Product_type revise = new revise_Product_type();

            revise.ID = (int)skinDataGridView4.Rows[skinDataGridView4.CurrentCell.RowIndex].Cells["Column13"].Value;
            revise.Type = skinDataGridView4.Rows[skinDataGridView4.CurrentCell.RowIndex].Cells["Column14"].Value.ToString();
            revise.Waiter = skinDataGridView4.Rows[skinDataGridView4.CurrentCell.RowIndex].Cells["Column15"].Value.ToString();

            revise.ShowDialog();

            commodity_flushed();
        }

        private void delete_Type()
        {
            try
            {

                if (MessageBox.Show("该操作不可恢复，是否要删除？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DbHelper.executeNonQuery($"delete from [dbo].[commodityType] where [CommodityTypeID] = '{skinDataGridView4.Rows[skinDataGridView4.CurrentCell.RowIndex].Cells["Column13"].Value}'");
                    commodity_flushed();
                }

            }catch(Exception ee)
            {
                MessageBox.Show(ee.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void skinButton13_Click(object sender, EventArgs e)
        {
            revise_Type();
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            revise_Type();
        }

        private void skinButton14_Click(object sender, EventArgs e)
        {
            delete_Type();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            delete_Type();
        }

        //---------------------------------------------------------------------

        private void insert_commodity()
        {
            Add_commodity add_Commodity = new Add_commodity();
            add_Commodity.ShowDialog();

            commodity_flushed();
        }

        private void revise_rcommodity()
        {
            revise_commodity revise = new revise_commodity();

            revise.commodityID = skinDataGridView5.Rows[skinDataGridView5.CurrentCell.RowIndex].Cells["Column16"].Value.ToString();
            revise.commodityClass = skinDataGridView5.Rows[skinDataGridView5.CurrentCell.RowIndex].Cells["Column21"].Value.ToString();
            revise.commodityName = skinDataGridView5.Rows[skinDataGridView5.CurrentCell.RowIndex].Cells["Column17"].Value.ToString();
            revise.commodityPinyin = skinDataGridView5.Rows[skinDataGridView5.CurrentCell.RowIndex].Cells["Column1"].Value.ToString();
            revise.Repository = skinDataGridView5.Rows[skinDataGridView5.CurrentCell.RowIndex].Cells["Column22"].Value.ToString();
            revise.unit_price = skinDataGridView5.Rows[skinDataGridView5.CurrentCell.RowIndex].Cells["Column19"].Value.ToString();
            revise.cost = skinDataGridView5.Rows[skinDataGridView5.CurrentCell.RowIndex].Cells["Column20"].Value.ToString();
            revise.unit = skinDataGridView5.Rows[skinDataGridView5.CurrentCell.RowIndex].Cells["Column18"].Value.ToString();

            revise.exchange = skinDataGridView5.Rows[skinDataGridView5.CurrentCell.RowIndex].Cells["Column25"].Value.ToString();
            revise.Integral = skinDataGridView5.Rows[skinDataGridView5.CurrentCell.RowIndex].Cells["Column26"].Value.ToString();

            revise.ShowDialog();

            commodity_flushed();


        }

        private void delete_commodity()
        {
            try
            {

                if (MessageBox.Show("该操作不可恢复，是否要删除？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DbHelper.executeNonQuery($"delete from [dbo].[Commodity] where [project_ID] = '{skinDataGridView5.Rows[skinDataGridView5.CurrentCell.RowIndex].Cells["Column16"].Value}'");
                    commodity_flushed();
                }

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void skinButton17_Click(object sender, EventArgs e)
        {
            insert_commodity();
        }

        private void skinButton16_Click(object sender, EventArgs e)
        {
            revise_rcommodity();
        }

        private void skinButton15_Click(object sender, EventArgs e)
        {
            delete_commodity();
        }

        private void 添加ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            insert_commodity();
        }

        private void 修改ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            revise_rcommodity();
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            delete_commodity();
        }
    }
}
