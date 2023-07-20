using CCWin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTV_management_system
{
    public partial class Add_member : Skin_Mac
    {
        public Add_member()
        {
            InitializeComponent();
        }

        public static bool IsNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            //匹配有小数点和没小数点的数字
            const string pattern = "^[0-9]+(.[0-9]{1})?$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(s);
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(skinTextBox1.Text) && string.IsNullOrEmpty(skinTextBox2.Text) && string.IsNullOrEmpty(skinTextBox3.Text))
                {
                    MessageBox.Show("请将内容填写完整", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!IsNumber(skinTextBox2.Text) && !IsNumber(skinTextBox3.Text))
                {
                    MessageBox.Show("初始积分和打折比例只能为数字或小数点", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DbHelper.executeNonQuery($@"insert into [dbo].[Member]([memberID], [TypeName], [Initial_Points], [Fold_rate])
                values(FLOOR(RAND() * POWER(10, 8)), '{skinTextBox1.Text}', '{skinTextBox2.Text}', '{skinTextBox3.Text}')");

                Close();

            }catch(Exception ee)
            {
                MessageBox.Show(ee.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
