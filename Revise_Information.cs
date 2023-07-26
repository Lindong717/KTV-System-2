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
    public partial class Revise_Information : Skin_Mac
    {
        public string Membership_Number;
        public string Member_Name;
        public string MemberSex;
        public string birthday;
        public string Phone;
        public string state;
        public string cardType;
        public string Membership_Level;
        public string Integral;
        public string pwd;
        public string remark;

        public Revise_Information()
        {
            InitializeComponent();
        }

        private void Revise_Information_Load(object sender, EventArgs e)
        {
            skinCheckBox1.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;

            DbHelper.skinCollections(skinComboBox4, "select [memberID],[TypeName] from [dbo].[Member]", "memberID", "TypeName", "请选择");

            textBox1.Text = Membership_Number;
            textBox2.Text = Member_Name;
            skinComboBox1.Text = MemberSex;
            dateTimePicker1.Value = DateTime.Parse(birthday);
            textBox3.Text = Phone;
            skinComboBox2.Text = state;
            skinComboBox3.Text = cardType;
            skinComboBox4.Text = Membership_Level;
            textBox4.Text = Integral;
            textBox6.Text = pwd;
            textBox7.Text = pwd;
            textBox5.Text = remark;
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        public static bool IsNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            //匹配有小数点和没小数点的数字
            const string pattern = "^[0-9]+(.[0-9]{2})?$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(s);
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (!IsNumber(textBox4.Text) && !IsNumber(textBox3.Text))
            {
                MessageBox.Show("联系电话与当前积分只能存在数字", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) || (long)skinComboBox4.SelectedValue <= 0)
            {
                MessageBox.Show("请将内容填写完整", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DbHelper.executeNonQuery($@"update [dbo].[Member_Information] set
            [memberName] = '{textBox2.Text}',
            [memberSex] = '{skinComboBox1.SelectedIndex}',
            [birthday] = '{dateTimePicker1.Value}',
            [Phone] = '{textBox3.Text}',
            [state] = '{skinComboBox2.SelectedIndex}',
            [Card_type] = '{skinComboBox3.SelectedIndex}',
            [Membership_Level] = '{skinComboBox4.SelectedValue}',
            [Integral] = '{textBox4.Text}',
            [remark] = '{textBox5.Text}'
            where [InformationID] = '{Membership_Number}'");
            Close();
        }
    }
}
