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
    public partial class revise_commodity : Skin_Mac
    {
        public string commodityClass;
        public string commodityName;
        public string commodityPinyin;
        public string Repository;
        public string unit_price;
        public string cost;
        public string unit;

        public string exchange;
        public string Integral;

        public revise_commodity()
        {
            InitializeComponent();
        }

        private void revise_commodity_Load(object sender, EventArgs e)
        {
            DbHelper.skinCollections(skinComboBox1, "select [CommodityTypeID],[TypeName] from [dbo].[commodityType]", "CommodityTypeID", "TypeName", "请选择");

            skinComboBox1.Text = commodityClass;
            textBox2.Text = commodityName;
            textBox3.Text = commodityPinyin;
            textBox1.Text = Repository;
            textBox4.Text = unit_price;
            textBox5.Text = cost;
            textBox6.Text = unit;

            skinCheckBox2.Checked = exchange.Trim() == "Y";
            textBox7.Text = Integral;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox3.Text = PingYinHelper.GetFirstSpell(textBox2.Text);
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox7.Enabled = skinCheckBox2.Checked;
        }
    }
}
