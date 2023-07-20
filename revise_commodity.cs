using CCWin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTV_management_system
{
    public partial class revise_commodity : Skin_Mac
    {
        private static List<bool> list = new List<bool>();

        public string commodityID;
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

        public static bool IsNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            //匹配有小数点和没小数点的数字
            const string pattern = "^[0-9]+(.[0-9]{1})?$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(s);
        }

        public bool security_guard()
        {
            list.Add((int)skinComboBox1.SelectedValue > 0);

            list.Add(!string.IsNullOrEmpty(textBox1.Text));
            list.Add(!string.IsNullOrEmpty(textBox2.Text));
            list.Add(!string.IsNullOrEmpty(textBox3.Text));
            list.Add(!string.IsNullOrEmpty(textBox4.Text));
            list.Add(!string.IsNullOrEmpty(textBox5.Text));
            list.Add(!string.IsNullOrEmpty(textBox6.Text));

            list.Add(skinCheckBox2.Checked ? Convert.ToInt32(textBox7.Text) > 0 : true);

            foreach (bool i in list)
            {
                if (!i)
                {
                    list.Clear();
                    return true;
                }
            }

            return false;
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            try
            {

                if (security_guard())
                {
                    MessageBox.Show("请将内容填写完毕", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!IsNumber(textBox4.Text) && !IsNumber(textBox5.Text) && !IsNumber(textBox6.Text) && !IsNumber(textBox1.Text))
                {
                    MessageBox.Show("预设单价和单价成本和计价单位只能包含数字", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DbHelper.executeNonQuery($@"update [dbo].[Commodity] set
                    [category_ID] = '{skinComboBox1.SelectedValue}',
                    [Name] = '{textBox2.Text}',
                    [Pinyin] = '{textBox3.Text}',
                    [unit] = '{textBox6.Text}',
                    [Preset_unit_price] = '{textBox4.Text}',
                    [cost] = '{textBox5.Text}',
                    [Repository] = '{textBox1.Text}',
                    [exchange] = '{(skinCheckBox2.Checked ? "Y" : "N")}',
                    [Redeem_points] = '{textBox7.Text}'
                    where [project_ID] = '{commodityID}'
                ");
                Close();
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox7.Enabled = skinCheckBox2.Checked;
        }
    }
}
