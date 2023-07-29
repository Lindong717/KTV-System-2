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
    public partial class Revise_Waiter : Skin_Mac
    {
        public string id;
        public string name;
        public string Pinyin;
        public string sex;
        public string Phone;
        public string identity_card;
        public string description;
        public string region;
        public string grade;

        public Revise_Waiter()
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

        private void Revise_Waiter_Load(object sender, EventArgs e)
        {
            DbHelper.skinCollections(skinComboBox2, "select [Private_rooms_type_ID],[type_Name] from [dbo].[Type_of_private_room]", "Private_rooms_type_ID", "type_Name", "请选择");
            DbHelper.skinCollections(skinComboBox3, "select [Grade number],[Rank name] from [dbo].[Waiter_type]", "Grade number", "Rank name", "请选择");

            textBox1.Text = id;
            textBox2.Text = name;
            textBox3.Text = Pinyin.Trim();
            textBox6.Text = Phone;
            textBox5.Text = identity_card;
            textBox4.Text = description;

            skinComboBox1.Text = sex;
            skinComboBox2.Text = region;
            skinComboBox3.Text = grade;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            Filter_numbers(textBox6);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            Filter_numbers(textBox5);
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox6.Text) || (int)skinComboBox2.SelectedValue < 0 || (int)skinComboBox3.SelectedValue < 0)
            {
                MessageBox.Show("请将内容填写完整", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DbHelper.executeNonQuery($@"update [dbo].[Waiter] set
            [Waiter name] = '{textBox2.Text}',
            [Jane_spelling] = '{textBox3.Text}',
            [sex] = '{skinComboBox1.SelectedIndex}',
            [region] = '{skinComboBox2.SelectedValue}',
            [level] = '{skinComboBox3.SelectedValue}',
            [Contact] = '{textBox6.Text}',
            [identity card] = '{textBox5.Text}',
            [description] = '{textBox4.Text}'
            where [Waiter_number] = '{id}'");

            Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox3.Text = PingYinHelper.GetFirstSpell(textBox2.Text);
        }
    }
}
