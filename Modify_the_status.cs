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
    public partial class Modify_the_status : Skin_Mac
    {
        public string Room_number;
        public string state;

        public Modify_the_status()
        {
            InitializeComponent();
        }

        private void Modify_the_status_Load(object sender, EventArgs e)
        {
            skinLabel4.Text = Room_number;
            skinLabel3.Text = state;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                if (flowLayoutPanel1.Controls[i].Capture)
                {
                    DbHelper.executeNonQuery($"update [dbo].[Private_rooms] set [Private_room_status] = '{flowLayoutPanel1.Controls[i].Tag}' where [Private_rooms_ID] = '{Room_number}'");
                    Close();
                }
            }
        }
    }
}
