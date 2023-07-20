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
    public partial class Add_Product_type : Skin_Mac
    {
        public Add_Product_type()
        {
            InitializeComponent();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(skinTextBox1.Text))
                {
                    MessageBox.Show("请将内容填写完整", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DbHelper.executeNonQuery($"insert into [dbo].[commodityType]([TypeName], [Waiter]) values('{skinTextBox1.Text}','{(skinRadioButton1.Checked ? 1 : 0)}')");
                Close();

            }catch(Exception ee)
            {
                MessageBox.Show(ee.Message,"系统提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
