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
    public partial class Modify_Membership : Skin_Mac
    {

        /// <summary>
        /// 会员等级
        /// </summary>
        public string Membership_Level;

        /// <summary>
        /// 会员初始积分
        /// </summary>
        public string Membership_Points;

        /// <summary>
        /// 会员折率
        /// </summary>
        public string Membership_discount;

        public Modify_Membership()
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

        private void Modify_Membership_Load(object sender, EventArgs e)
        {
            skinTextBox1.Text = Membership_Level;
            skinTextBox2.Text = Membership_Points;
            skinTextBox3.Text = Membership_discount;
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

                if (Convert.ToInt32(DbHelper.executeScalar($"select count(*) from [dbo].[Member] where [TypeName] = '{skinTextBox1.Text}'")) == 0 || Membership_Level == skinTextBox1.Text)
                {
                    DbHelper.executeNonQuery($@"update [dbo].[Member] set [TypeName] = '{skinTextBox1.Text}',[Initial_Points] = '{skinTextBox2.Text}',[Fold_rate] = '{skinTextBox3.Text}' where [TypeName] = '{Membership_Level}'");

                    Close();

                }
                else
                {
                    MessageBox.Show("该类型名已被占用", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ee)
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
