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
    public partial class Batch : Skin_Mac
    {
        private static List<bool> list = new List<bool>();

        public Batch()
        {
            InitializeComponent();
        }

        private void Batch_Load(object sender, EventArgs e)
        {
            DbHelper.skinCollections(skinComboBox1, "select [Private_rooms_type_ID],[type_Name] from [dbo].[Type_of_private_room]", "Private_rooms_type_ID", "type_Name", "请选择");
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!security_guard())
                {
                    for (int i = (int)numericUpDown1.Value; i < (int)numericUpDown2.Value; i++)
                    {
                        StringBuilder splicing = new StringBuilder(i.ToString());

                        if (skinRadioButton1.Checked == true ? true : false)
                        {
                            splicing.Insert(0, skinTextBox1.Text);
                        }
                        else
                        {
                            splicing.AppendLine(skinTextBox1.Text);
                        }

                        DbHelper.executeNonQuery($@"insert into [dbo].[Private_rooms]([Private_rooms_ID], [Private_rooms_type], [Private_room_status],[location])
                        values('{splicing}', '{skinComboBox1.SelectedValue}', '0', '{skinTextBox2.Text}')");
                    }

                    Close();
                }
                else
                {
                    MessageBox.Show("请将内容填写完整并且填写正确", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }catch(Exception ee)
            {
                MessageBox.Show(ee.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool security_guard()
        {
            list.Add(!string.IsNullOrEmpty(skinTextBox1.Text));
            list.Add(!string.IsNullOrEmpty(skinTextBox2.Text));
            list.Add((int)skinComboBox1.SelectedValue > 0);
            list.Add(numericUpDown1.Value < numericUpDown2.Value);

            foreach (bool i in list)
            {
                if (!i)
                {
                    list.Clear();
                    return true;
                }
            }

            return false;
        }
    }
}
