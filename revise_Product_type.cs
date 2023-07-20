using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;

namespace KTV_management_system
{
    public partial class revise_Product_type : Skin_Mac
    {
        public int ID;
        public string Type;
        public string Waiter;

        public revise_Product_type()
        {
            InitializeComponent();
        }

        private void revise_Product_type_Load(object sender, EventArgs e)
        {
            skinTextBox1.Text = Type;

            skinRadioButton1.Checked = Waiter == "需要";
            skinRadioButton2.Checked = Waiter != "需要";
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

                DbHelper.executeNonQuery($"update [dbo].[commodityType] set [TypeName] = '{skinTextBox1.Text}',[Waiter] = '{(skinRadioButton1.Checked ? 1 : 0)}' where [CommodityTypeID] = '{ID}'");
                Close();
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
