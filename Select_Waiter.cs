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
    public partial class Select_Waiter : Skin_Mac
    {
        private static string sql = @"select [Waiter_number],[Waiter name],[sex],[Rank name],[Jane_spelling] as Pinyin from [dbo].[Waiter] as a
        join [dbo].[Waiter_type] as b on a.[level] = b.[Grade number]
        where 1=1";

        public Select_Waiter()
        {
            InitializeComponent();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Select_Waiter_Load(object sender, EventArgs e)
        {
            DbHelper.skinDataGridView(skinDataGridView1, sql, "");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DbHelper.skinDataGridView(skinDataGridView1, sql, textBox1.Text);
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
