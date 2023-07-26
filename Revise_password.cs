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
    public partial class Revise_password : Skin_Mac
    {
        public string Membership_Number;

        public Revise_password()
        {
            InitializeComponent();
        }

        private void Revise_password_Load(object sender, EventArgs e)
        {
            textBox1.Text = Membership_Number;
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != DbHelper.executeScalar($"select [password] from [dbo].[Member_Information] where [InformationID] = '{Membership_Number}'"))
            {
                MessageBox.Show("旧密码不正确", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox3.Text != textBox4.Text)
            {
                MessageBox.Show("正确密码和新密码不一致", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox3.Text.Length < 6)
            {
                MessageBox.Show("新密码长度不能小于6", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DbHelper.executeNonQuery($"update [dbo].[Member_Information] set [password] = '{textBox3.Text}' where [InformationID] = '{Membership_Number}'");
            Close();
        }
    }
}
