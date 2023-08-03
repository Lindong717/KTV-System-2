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
using System.Text.RegularExpressions;

namespace KTV_management_system
{
    public partial class Revise_Recording : Skin_Mac
    {
        public string Private_rooms_ID;

        public Revise_Recording()
        {
            InitializeComponent();
        }

        private void Revise_Recording_Load(object sender, EventArgs e)
        {
            List<string> list = new List<string>();

            DbHelper.Inquire($@"select [Private_rooms_ID],[type_Name],[Minimum_consumption],[deposit],[remark] from [dbo].[Private_rooms] as a
            join [dbo].[Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
            join [dbo].[Billing_type] as c on b.Billing_method = c.manner_ID
            where [Private_rooms_ID] = '{Private_rooms_ID}'", ref list);

            skinLabel3.Text = list[0];
            skinLabel2.Text = list[1];
            skinLabel7.Text = list[2];

            textBox1.Text = DbHelper.executeScalar($"select [deposit] from [dbo].[Private_rooms] where [Private_rooms_ID] = '{Private_rooms_ID}'");
            skinTextBox4.Text = DbHelper.executeScalar($"select [remark] from [dbo].[Private_rooms] where [Private_rooms_ID] = '{Private_rooms_ID}'");
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            DbHelper.executeNonQuery($@"update [dbo].[Private_rooms] set
            [deposit] = '{textBox1.Text}',
            [remark] = '{skinTextBox4.Text}'
            where [Private_rooms_ID] = '{Private_rooms_ID}'");

            Close();
        }

        private void Filter_numbers(TextBox textBox)
        {
            Regex regex = new Regex("[^0-9]");
            string input = regex.Replace(textBox.Text, "");

            textBox.Text = input;
            textBox.SelectionStart = textBox.Text.Length;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Filter_numbers(textBox1);
        }
    }
}
