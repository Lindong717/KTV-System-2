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
    public partial class modify : Skin_Mac
    {
        public string ID;
        public string level;
        public modify()
        {
            InitializeComponent();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            DbHelper.executeNonQuery($@"update [dbo].[Waiter_type] set [Rank_name] = {skinTextBox2.Text} where [Grade_number]={skinTextBox1.Text} ");

            Close();
        }

        private void modify_Load(object sender, EventArgs e)
        {
            skinTextBox1.Text = ID;
            skinTextBox2.Text = level;
        }
    }
}
