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
    public partial class Note_information : Skin_Mac
    {
        public string commodityID;
        public string commodityName;
        public string commodityRemark;

        public Note_information()
        {
            InitializeComponent();
        }

        private void Note_information_Load(object sender, EventArgs e)
        {
            skinLabel3.Text = commodityName;
            skinTextBox1.Text = commodityRemark;
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            try
            {
                DbHelper.executeNonQuery($"update [dbo].[Consumption_list] set [remark] = '{skinTextBox1.Text}' where [manifestID] = '{commodityID}'");
                Close();
            }catch(Exception ee)
            {
                MessageBox.Show(ee.Message,"系统提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
