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
    public partial class increase2 : Form
    {
        public increase2()
        {
            InitializeComponent();
        }

        private void increase2_Load(object sender, EventArgs e)
        {
            skinComboBox1.SelectedIndex = 0;
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
               if (skinTextBox2.Text == "" || skinTextBox3.Text == "" || skinTextBox4.Text == "" || skinTextBox5.Text == "" || skinComboBox2.SelectedIndex == 0 || skinComboBox3.SelectedIndex == 0 || skinComboBox2.SelectedIndex == 0)
                {
                    MessageBox.Show("请填写完整！");
                }
                DbHelper.executeNonQuery($"insert into Waiter ([Waiter name], [Jane_spelling], [sex], [level], [Contact], [identity card], [Service Area],[description]) values ('{skinTextBox2.Text}','{skinTextBox4.Text}','{skinComboBox1.Text}','{skinComboBox3.SelectedIndex}','{skinTextBox3.Text}','{skinTextBox5.Text}','{skinComboBox2.SelectedIndex}','{skinTextBox6.Text}')");
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
