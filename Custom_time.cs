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
    public partial class Custom_time : Skin_Mac
    {
        public string condition;

        public Custom_time()
        {
            InitializeComponent();
        }

        private void Custom_time_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            numericUpDown4.Value = date.Hour;
            numericUpDown1.Value = date.Hour;
            numericUpDown3.Value = date.Minute;
            numericUpDown2.Value = date.Minute;
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            string Arrival_time = $"{metroDateTime1.Value:yyyy-MM-dd} {numericUpDown4.Value}:{numericUpDown3.Value}";
            string Save_time = $"{metroDateTime2.Value:yyyy-MM-dd} {numericUpDown1.Value}:{numericUpDown2.Value}";

            condition = $" and [Arrival_time] between '{Save_time}' and '{Arrival_time}'";
            Close();
        }
    }
}
