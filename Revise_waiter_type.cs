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
    public partial class Revise_waiter_type : Skin_Mac
    {
        public string WaiterID;
        public string Rank_name;

        public Revise_waiter_type()
        {
            InitializeComponent();
        }

        private void Revise_waiter_type_Load(object sender, EventArgs e)
        {
            textBox1.Text = WaiterID;
            textBox2.Text = Rank_name;
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("请将内容填写完整", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DbHelper.executeNonQuery($"update [dbo].[Waiter_type] set [Rank name] = '{textBox2.Text}' where [Grade number] = '{WaiterID}'");
            Close();
        }
    }
}
