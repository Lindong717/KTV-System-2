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
    public partial class Login_member : Skin_Mac
    {
        public string Private_room_number;

        public Login_member()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Filter_numbers(textBox1);
        }

        private void Filter_numbers(TextBox textBox)
        {
            Regex regex = new Regex("[^0-9]");
            string input = regex.Replace(textBox.Text, "");

            textBox.Text = input;
            textBox.SelectionStart = textBox.Text.Length;
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("请将内容填写完整","系统提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            if (DbHelper.executeScalar($"select count(*) from [dbo].[Member_Information] where [InformationID] = '{textBox1.Text}' and [password] = '{textBox2.Text}'") != "1")
            {
                MessageBox.Show("该会员不存在，可能是编号或密码错误", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Redeem_goods redeem = new Redeem_goods
            {
                ID = textBox1.Text,
                Private_room_number = Private_room_number
            };
            redeem.ShowDialog();

            Hide();
        }
    }
}
