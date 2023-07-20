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
    /// <summary>
    /// 修改包间信息窗口
    /// </summary>
    public partial class Modify_the_private_room : Skin_Mac
    {
        private static List<bool> list = new List<bool>();

        /// <summary>
        /// 包间类型
        /// </summary>
        public string Type;

        /// <summary>
        /// 包间编号
        /// </summary>
        public string id;

        /// <summary>
        /// 包间区域
        /// </summary>
        public string region;

        public Modify_the_private_room()
        {
            InitializeComponent();
        }

        private void Modify_the_private_room_Load(object sender, EventArgs e)
        {
            DbHelper.skinCollections(skinComboBox1, "select [Private_rooms_type_ID],[type_Name] from [dbo].[Type_of_private_room]", "Private_rooms_type_ID", "type_Name", "请选择");
            skinComboBox1.Text = Type;
            skinTextBox1.Text = id;
            skinTextBox2.Text = region;
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
                    if (Convert.ToInt32(DbHelper.executeScalar($"select count(*) from [dbo].[Private_rooms] where [Private_rooms_ID] = '{skinTextBox1.Text}'")) == 0 || skinTextBox1.Text == id)
                    {
                        DbHelper.executeNonQuery($"update [dbo].[Private_rooms] set [Private_rooms_type] = '{skinComboBox1.SelectedValue}',[Private_rooms_ID] = '{skinTextBox1.Text}',[location] = '{skinTextBox2.Text}' where [Private_rooms_ID] = '{id}'");
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("包间编号已被占用", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("请将内容填写完整", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
