using CCWin;
using CCWin.SkinControl;
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
    public partial class Add_waiter : Skin_Mac
    {
        public Add_waiter()
        {
            InitializeComponent();
        }

        private void Filter_numbers(TextBox textBox)
        {
            Regex regex = new Regex("[^0-9]");
            string input = regex.Replace(textBox.Text, "");

            textBox.Text = input;
            textBox.SelectionStart = textBox.Text.Length;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            Filter_numbers(textBox6);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            Filter_numbers(textBox5);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Filter_numbers(textBox1);
        }

        private void Add_waiter_Load(object sender, EventArgs e)
        {
            skinComboBox1.SelectedIndex = 0;

            DbHelper.skinCollections(skinComboBox2, "select [Private_rooms_type_ID],[type_Name] from [dbo].[Type_of_private_room]", "Private_rooms_type_ID", "type_Name", "请选择");
            DbHelper.skinCollections(skinComboBox3, "select [Grade number],[Rank name] from [dbo].[Waiter_type]", "Grade number", "Rank name", "请选择");
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox6.Text) || (int)skinComboBox2.SelectedValue < 0 || (int)skinComboBox3.SelectedValue < 0)
            {
                MessageBox.Show("请将内容填写完整", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (DbHelper.executeScalar($"select count(*) from [dbo].[Waiter] where [Waiter_number] = '{textBox1.Text}'") != "0")
            {
                MessageBox.Show("该包间已被占用","系统提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            DbHelper.executeNonQuery($@"insert into [dbo].[Waiter]([Waiter_number], [Waiter name], [Jane_spelling], [sex], [region], [level], [Contact], [identity card], [description])
            values('{textBox1.Text}','{textBox2.Text}','{textBox3.Text}','{skinComboBox1.SelectedIndex}','{skinComboBox2.SelectedValue}','{skinComboBox3.SelectedValue}','{textBox6.Text}','{textBox5.Text}','{textBox4.Text}')");
            Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox3.Text = PingYinHelper.GetFirstSpell(textBox2.Text);
        }
    }
}
