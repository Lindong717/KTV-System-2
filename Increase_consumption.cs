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
    public partial class Increase_consumption : Skin_Mac
    {
        private string sql = "select [project_ID],[Name],[Pinyin],[Preset_unit_price],[Repository] from [dbo].[Commodity]";
        private bool mode = true;

        public Increase_consumption()
        {
            InitializeComponent();
        }

        private void Increase_consumption_Load(object sender, EventArgs e)
        {
            DbHelper.skinDataGridView(skinDataGridView1, sql,"");
            DbHelper.Tree(skinTreeView1, "select [CommodityTypeID],[TypeName] from [dbo].[commodityType]");
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
    }
}
