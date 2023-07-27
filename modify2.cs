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
    public partial class modify2 : Skin_Mac
    {
        public string id;
        public string name;
        public string jane;
        public string sex;
        public string type;
        public string Rank;
        public string phone;
        public string idcard;
        public string miaoshu;
        public modify2()
        {
            InitializeComponent();
        }

        private void modify2_Load(object sender, EventArgs e)
        {
            skinTextBox1.Text = id;
            skinComboBox1.Text = sex;
            skinTextBox2.Text = name;
            skinTextBox3.Text = phone;
            skinTextBox4.Text = jane;
            skinTextBox5.Text = idcard;
            skinTextBox6.Text = miaoshu;
            skinComboBox2.Text = type;
            skinComboBox3.Text = Rank;
            DbHelper.skinCollections(skinComboBox3, "select [Grade_number], [Rank_name] from [dbo].[Waiter_type]", "Grade_number", "Rank_name", "请选择");
            DbHelper.skinCollections(skinComboBox2, "select [Private_rooms_type_ID], [type_Name] from [dbo].[Type_of_private_room]", "Private_rooms_type_ID", "type_Name", "请选择");
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (skinComboBox2.SelectedIndex == 0 || skinComboBox3.SelectedIndex == 0)
                {
                    MessageBox.Show("请选择区域和等级");
                    return;
                }

                DbHelper.executeNonQuery($@"update [dbo].[Waiter] set [Waiter name] = '{skinTextBox2.Text}' ,[Jane_spelling]='{skinTextBox4.Text}',
            [sex]='{skinComboBox1.Text}',[level]={skinComboBox3.SelectedIndex},[Contact]='{skinTextBox3.Text}',[identity card]='{skinTextBox5.Text}',[Service Area]={skinComboBox2.SelectedIndex},[description]='{skinTextBox6.Text}'
            where [Waiter_number]={skinTextBox1.Text} ");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
    }
}
