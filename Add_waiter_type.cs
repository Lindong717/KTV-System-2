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
    public partial class Add_waiter_type : Skin_Mac
    {
        public Add_waiter_type()
        {
            InitializeComponent();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) && string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("请填写完整内容","系统提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            if (DbHelper.executeScalar($"select count(*) from [dbo].[Waiter_type] where [Grade number] = '{textBox1.Text}'") == "1")
            {
                MessageBox.Show("该编号已被占用", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DbHelper.executeNonQuery($"insert into [dbo].[Waiter_type]([Grade number], [Rank name]) values('{textBox1.Text}', '{textBox2.Text}')");
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            string input = regex.Replace(textBox1.Text, "");

            textBox1.Text = input;
            textBox1.SelectionStart = textBox1.Text.Length;
        }
    }
}
