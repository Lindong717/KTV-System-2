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
    public partial class Revise_booking : Skin_Mac
    {
        public string id;
        public string name;
        public string Phone;
        public string state;
        public string remark;
        public string Private_room_number;
        public string Automatic_deletion;

        public DateTime Arrival_time;
        public DateTime Retention_time;

        public Revise_booking()
        {
            InitializeComponent();
        }

        private void Revise_booking_Load(object sender, EventArgs e)
        {
            textBox1.Text = name;
            textBox2.Text = Phone;
            skinRadioButton1.Checked = state.Trim() == "Y";
            skinRadioButton2.Checked = state.Trim() != "Y";
            textBox3.Text = remark;
            textBox4.Text = Private_room_number;
            skinCheckBox1.Checked = Automatic_deletion == "1";

            metroDateTime1.Value = DateTime.Parse(Arrival_time.ToString("yyyy-MM-dd"));
            metroDateTime2.Value = DateTime.Parse(Retention_time.ToString("yyyy-MM-dd"));

            numericUpDown4.Value = decimal.Parse(Arrival_time.ToString("hh"));
            numericUpDown3.Value = decimal.Parse(Arrival_time.ToString("mm"));

            numericUpDown1.Value = decimal.Parse(Retention_time.ToString("hh"));
            numericUpDown2.Value = decimal.Parse(Retention_time.ToString("mm"));
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("请将信息填写完整", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox4.Text != Private_room_number)
            {
                if (DbHelper.executeScalar($"select count(*) from [dbo].[Private_rooms] where [Private_rooms_ID] = '{textBox4.Text}' and [Private_room_status] = '0'") != "1")
                {
                    MessageBox.Show("该包间状态必须为可供", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            string Arrival_time = $"{metroDateTime1.Value:yyyy-MM-dd} {numericUpDown4.Value}:{numericUpDown3.Value}";
            string Save_time = $"{metroDateTime2.Value:yyyy-MM-dd} {numericUpDown1.Value}:{numericUpDown2.Value}";

            DbHelper.executeNonQuery($@"update [dbo].[Appointment_management] set
            [Customer_name] = '{textBox1.Text}',
            [Phone] = '{textBox2.Text}',
            [state] = '{(skinRadioButton1.Checked ? "Y" : "N")}',
            [Private_rooms_type] = '{DbHelper.executeScalar($"select [Private_rooms_type] from [dbo].[Private_rooms] where [Private_rooms_ID] = '{textBox4.Text}'")}',
            [Private_room_number] = '{textBox4.Text}',
            [Arrival_time] = '{Arrival_time}',
            [Save_time] = '{Save_time}',
            [remark]  = '{(string.IsNullOrEmpty(textBox3.Text) ? "*" : textBox3.Text)}',
            [Automatic_cancellation] = '{(skinCheckBox1.Checked ? "1" : "0")}'
            where [Customer_number] = '{id}'");
            Close();
        }
    }
}
