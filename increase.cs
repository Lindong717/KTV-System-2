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
    public partial class increase : Skin_Mac

    {
        public increase()
        {
            InitializeComponent();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (skinTextBox1.Text == "" || skinTextBox2.Text == "")
            {
                MessageBox.Show("请填写完整！");
            }
            DbHelper.executeNonQuery($"insert into [dbo].[Waiter_type] ([Grade_number], [Rank_name]) values ({skinTextBox1.Text},'{skinTextBox2.Text}')");
            Close();

        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void increase_Load(object sender, EventArgs e)
        {

        }


    }
}
