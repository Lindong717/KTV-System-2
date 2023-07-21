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
    public partial class Customer_billing : Skin_Mac
    {
        public string Private_rooms_ID;

        public Customer_billing()
        {
            InitializeComponent();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Customer_billing_Load(object sender, EventArgs e)
        {
            DbHelper.skinCollections(skinComboBox1, "select [Private_rooms_type_ID],[type_Name] from [dbo].[Type_of_private_room]", "Private_rooms_type_ID", "type_Name", "请选择");

            List<string> list = new List<string>();

            DbHelper.Inquire($@"select [Private_rooms_ID],[type_Name],[Minimum_consumption],[manner_Name] from [dbo].[Private_rooms] as a
            join [dbo].[Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
            join [dbo].[Billing_type] as c on b.Billing_method = c.manner_ID
            where [Private_rooms_ID] = '{Private_rooms_ID}'",ref list);

            skinLabel3.Text = list[0];
            skinLabel2.Text = list[1];
            skinLabel7.Text = list[2];
            skinComboBox1.Text = list[3];
        }
    }
}
