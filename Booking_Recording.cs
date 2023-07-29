using CCWin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTV_management_system
{
    public partial class Booking_Recording : Skin_Mac
    {
        public string Private_room_number;

        public Booking_Recording()
        {
            InitializeComponent();
        }

        private void Booking_Recording_Load(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            numericUpDown4.Value = date.Hour;
            numericUpDown1.Value = date.Hour;
            numericUpDown3.Value = date.Minute;
            numericUpDown2.Value = date.Minute;

            if (!string.IsNullOrEmpty(Private_room_number))
            {
                skinComboBox2.Visible = true;
                DbHelper.skinCollections(skinComboBox2, $"select [Private_rooms_type],[Private_rooms_ID] from [dbo].[Private_rooms] where [Private_rooms_type] = '{DbHelper.executeScalar($"select [Private_rooms_type] from [dbo].[Private_rooms] where [Private_rooms_ID] = '{Private_room_number}'")}' and [Private_room_status] = '0'", "Private_rooms_type", "Private_rooms_ID", "请选择");
                skinComboBox2.Text = Private_room_number;
                return;
            }

            textBox4.Visible = true;
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        public static bool IsNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            const string pattern = "^[0-9]+(.[0-9]{1})?$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(s);
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Private_room_number))
            {
                if (string.IsNullOrEmpty(textBox1.Text) || (int)skinComboBox2.SelectedValue < 0)
                {
                    MessageBox.Show("请将内容填写完整", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("请将内容填写完整", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                if (!IsNumber(textBox2.Text))
                {
                    MessageBox.Show("联系电话只能包含数字", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (textBox4.Visible)
            {
                if (string.IsNullOrEmpty(textBox4.Text))
                {
                    MessageBox.Show("包间编号填写完整", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (DbHelper.executeScalar($"select count(*) from [dbo].[Private_rooms] where [Private_rooms_ID] = '{textBox4.Text}' and [Private_room_status] = '0'") != "1")
                {
                    MessageBox.Show("该包间状态必须为可供", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Private_room_number = textBox4.Text;
            }

            string Arrival_time = $"{metroDateTime1.Value:yyyy-MM-dd} {numericUpDown4.Value}:{numericUpDown3.Value}";
            string Save_time = $"{metroDateTime2.Value:yyyy-MM-dd} {numericUpDown1.Value}:{numericUpDown2.Value}";

            DbHelper.executeNonQuery($@"insert into [dbo].[Appointment_management]([Customer_name], [Phone], [state], [Private_rooms_type], [Private_room_number], [Arrival_time], [Save_time], [Settling_time], [remark],[Automatic_cancellation])
            values('{textBox1.Text}','{(string.IsNullOrEmpty(textBox2.Text) ? "(无)" : textBox2.Text)}','Y','{DbHelper.executeScalar($"select [Private_rooms_type] from [dbo].[Private_rooms] where [Private_rooms_ID] = '{Private_room_number}'")}','{(textBox4.Visible ? textBox4.Text : skinComboBox2.Text)}','{Arrival_time}','{Save_time}',GETDATE(),'{(string.IsNullOrEmpty(textBox3.Text) ? "*" : textBox3.Text)}','{(skinCheckBox1.Checked ? "1" : "0")}')");

            DbHelper.executeNonQuery($"update [dbo].[Private_rooms] set [Private_room_status] = '3' where [Private_rooms_ID] = '{Private_room_number}'");
            Close();
        }
    }
}
