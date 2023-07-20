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
    public partial class Add_commodity : Skin_Mac
    {
        private static List<bool> list = new List<bool>();

        public Add_commodity()
        {
            InitializeComponent();
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

                DbHelper.executeNonQuery($@"insert into [dbo].[Commodity]([Name], [Pinyin], [unit], [Preset_unit_price], [cost], [category_ID], [Repository], [exchange], [Redeem_points])
                values('{textBox2.Text}','{textBox3.Text}','{textBox6.Text}','{textBox4.Text}','{textBox5.Text}','{skinComboBox1.SelectedValue}','{textBox1.Text}','{(skinCheckBox1.Checked ? "Y" : "N")}','{textBox7.Text}')");
                Close();

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            list.Add(skinCheckBox1.Checked ? Convert.ToInt32(textBox7.Text) > 0 : true);

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox7.Enabled = skinCheckBox1.Checked;
        }

        private void Add_commodity_Load(object sender, EventArgs e)
        {
            DbHelper.skinCollections(skinComboBox1, "select [CommodityTypeID],[TypeName] from [dbo].[commodityType]", "CommodityTypeID", "TypeName", "请选择");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox3.Text = PingYinHelper.GetFirstSpell(textBox2.Text);
        }
    }
}
