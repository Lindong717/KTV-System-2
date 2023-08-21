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
    public partial class Increase_membership : Skin_Mac
    {
        public Increase_membership()
        {
            InitializeComponent();
        }

        private void Increase_membership_Load(object sender, EventArgs e)
        {
            DbHelper.skinCollections(skinComboBox4, "select [memberID],[TypeName] from [dbo].[Member]", "memberID", "TypeName", "请选择");

            textBox1.Text = DbHelper.executeScalar("select top 1 [InformationID] + 1 from [dbo].[Member_Information] order by [InformationID] desc");
            skinComboBox1.SelectedIndex = 0;
            skinComboBox2.SelectedIndex = 0;
            skinComboBox3.SelectedIndex = 0;
            skinComboBox4.SelectedIndex = 1;
        }

        private void skinCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox6.Enabled = skinCheckBox1.Checked;
            textBox7.Enabled = skinCheckBox1.Checked;

            if (!skinCheckBox1.Checked)
            {
                textBox6.Text = null;
                textBox7.Text = null;
            }
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
                MessageBox.Show("联系电话与当前积分只能存在数字","系统提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            if (skinCheckBox1.Checked)
            {
                if (textBox7.Text != textBox6.Text)
                {
                    MessageBox.Show("确认密码和设置密码不一致", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (textBox6.Text.Length <= 6)
                {
                    MessageBox.Show("密码长度需要有6个字符以上", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) || (long)skinComboBox4.SelectedValue <= 0)
            {
                MessageBox.Show("请将内容填写完整", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DbHelper.executeNonQuery($@"insert into [dbo].[Member_Information]([memberName], [memberSex], [Card_type], [Card_balance], [Membership_Level], [Integral], [Amount_spent], [birthday], [Phone], [enroll_date], [state], [remark], [password])
            values('{textBox2.Text}','{skinComboBox1.SelectedIndex}','{skinComboBox3.SelectedIndex}','0.00','{skinComboBox4.SelectedValue}','{textBox4.Text}','0.00','{dateTimePicker1.Value}','{textBox3.Text}',GETDATE(),'{skinComboBox2.SelectedIndex}','{textBox5.Text}','{(skinCheckBox1.Checked ? textBox7.Text : null)}')");
            Close();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
