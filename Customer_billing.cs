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
            List<string> list = new List<string>();

            DbHelper.Inquire($@"select [Private_rooms_ID],[type_Name],[Minimum_consumption],[manner_Name] from [dbo].[Private_rooms] as a
            join [dbo].[Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
            join [dbo].[Billing_type] as c on b.Billing_method = c.manner_ID
            where [Private_rooms_ID] = '{Private_rooms_ID}'",ref list);

            skinLabel3.Text = list[0];
            skinLabel2.Text = list[1];
            skinLabel7.Text = list[2];
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            DbHelper.executeNonQuery($@"update [dbo].[Private_rooms] set
                [Private_room_status] = '1',
                [Start_time] = GETDATE(),
                [remark] = '{skinTextBox4.Text}',
                [deposit] = '{skinTextBox1.Text}'
                where [Private_rooms_ID] = '{Private_rooms_ID}'
            ");

            string i = DbHelper.executeScalar($@"select [Minimum_consumption] from [dbo].[Private_rooms] as a
                join[dbo].[Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
                where[Private_rooms_ID] = '{Private_rooms_ID}'");

            string Fold_rate = DbHelper.executeScalar($@"select [Fold_rate] from [dbo].[Private_rooms] as a
                join[dbo].[Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
                where[Private_rooms_ID] = '{Private_rooms_ID}'");

            DbHelper.executeNonQuery($@"insert into [dbo].[Consumption_list]([Private_room], [project_ID], [unit_price], [Fold_rate], [quantity], [amount], [Crediting_time], [Waiter], [Bookkeeper], [remark])
            values('{Private_rooms_ID}','包间费用','{i}','{Fold_rate}','1','{(float.Parse(i) * float.Parse(Fold_rate))}',GETDATE(),'*','*','*')");

            Close();
        }
    }
}
