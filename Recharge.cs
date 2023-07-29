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
    public partial class Recharge : Skin_Mac
    {
        public DataGridViewSelectedRowCollection Collections;
        public string Membership_Number;
        public string Member_Name;
        public string balance;
        public string Membership_Level;

        public Recharge()
        {
            InitializeComponent();
        }

        private void Recharge_Load(object sender, EventArgs e)
        {
            skinComboBox1.SelectedIndex = 0;

            if (Collections.Count > 1)
            {
                skinPanel2.Visible = true;
                skinLabel20.Text = Collections.Count.ToString();
                return;
            }

            skinPanel1.Visible = true;
            skinLabel9.Text = Membership_Number;
            skinLabel8.Text = Member_Name;
            skinLabel7.Text = balance;
            skinLabel6.Text = Membership_Level;
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (!IsNumber(textBox1.Text) || !IsNumber(textBox2.Text))
            {
                MessageBox.Show("不可输入字符","系统提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            foreach (DataGridViewRow row in Collections)
            {
                if (row.Cells["Column4"].Value.ToString() == "储值卡")
                {
                    DbHelper.executeNonQuery($"update [dbo].[Member_Information] set [Card_balance] += '{textBox1.Text}' where [InformationID] = '{row.Cells["Column1"].Value}'");
                }
            }

            Close();
        }

        public static bool IsNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            //匹配有小数点和没小数点的数字
            const string pattern = "^[0-9]+(.[0-9]{2})?$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(s);
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = string.IsNullOrEmpty(textBox1.Text) ? "0.00" : textBox1.Text;
        }
    }
}
