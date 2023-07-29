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
    public partial class Reservation_Information : Skin_Mac
    {
        public string Private_room_number;

        public Reservation_Information()
        {
            InitializeComponent();
        }

        private void Reservation_Information_Load(object sender, EventArgs e)
        {
            skinDataGridView1.RowHeadersVisible = false;
            skinCaptionPanel1.Text = $"已预定{Private_room_number}包间的宾客";
            DbHelper.skinDataGridView(skinDataGridView1, $"select [Customer_name],[Arrival_time],[Phone] from [dbo].[Appointment_management] where [Private_room_number] = '{Private_room_number}'","");
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
