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
    public partial class Replacement : Skin_Mac
    {
        public string Private_room_number;

        public Replacement()
        {
            InitializeComponent();
        }

        private void Replacement_Load(object sender, EventArgs e)
        {
            skinLabel3.Text = Private_room_number;
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("包间编号不能为空","系统提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            float Price = float.Parse(DbHelper.executeScalar($@"select [Minimum_consumption] from [dbo].[Private_rooms] as a
            join [dbo].[Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
            where [Private_rooms_ID] = '{textBox1.Text}'"));

            if (skinCheckBox1.Checked)
            {
                DbHelper.executeNonQuery($"update [dbo].[Consumption_list] set [unit_price] += '{Price}' where [Private_room] = '{Private_room_number}' and [project_ID] = '包间费用'");
            }
            else
            {
                DbHelper.executeNonQuery($"update [dbo].[Consumption_list] set [unit_price] = '{Price}' where [Private_room] = '{Private_room_number}' and [project_ID] = '包间费用'");
            }


            string Amount_spent = DbHelper.executeScalar($"select [Amount_spent] from [dbo].[Private_rooms] where [Private_rooms_ID] = '{Private_room_number}'");
            string Start_time = DbHelper.executeScalar($"select [Start_time] from [dbo].[Private_rooms] where [Private_rooms_ID] = '{Private_room_number}'");
            string Elapsed_time = DbHelper.executeScalar($"select [Elapsed_time] from [dbo].[Private_rooms] where [Private_rooms_ID] = '{Private_room_number}'");
            string remark = DbHelper.executeScalar($"select [remark] from [dbo].[Private_rooms] where [Private_rooms_ID] = '{Private_room_number}'");
            string deposit = DbHelper.executeScalar($"select [deposit] from [dbo].[Private_rooms] where [Private_rooms_ID] = '{Private_room_number}'");

            DbHelper.executeNonQuery($@"update [dbo].[Private_rooms] set
            [Private_room_status] = '1',
            [Amount_spent] = '{Amount_spent}',
            [Start_time] = '{Start_time}',
            [Elapsed_time] = '{Elapsed_time}',
            [remark] = '{remark}',
            [deposit] = '{deposit}'
            where [Private_rooms_ID] = '{textBox1.Text}'");

            DbHelper.executeNonQuery($"update [dbo].[Consumption_list] set [Private_room] = '{textBox1.Text}' where [Private_room] = '{Private_room_number}'");
            DbHelper.executeNonQuery($"update [dbo].[Consumption_list] set [amount] = ([unit_price] * [Fold_rate]) where [Private_room] = '{textBox1.Text}' and [project_ID] = '包间费用'");

            DbHelper.executeNonQuery($@"update [dbo].[Private_rooms] set
            [Private_room_status] = '0',
            [Amount_spent] = NULL,
            [Start_time] = NULL,
            [Elapsed_time] = NULL,
            [remark] = NULL,
            [deposit] = NULL
            where [Private_rooms_ID] = '{Private_room_number}'");

            Close();
        }
    }
}
