using CCWin;
using CCWin.SkinControl;
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
    public partial class Checkout : Skin_Mac
    {
        public string Private_room_number;
        private static string tmp;

        public Checkout()
        {
            InitializeComponent();
        }

        private void Checkout_Load(object sender, EventArgs e)
        {
            string sum = DbHelper.executeScalar($"select SUM([amount]) from [dbo].[Consumption_list] where [Private_room] = '{Private_room_number}'");
            string Preferential = DbHelper.executeScalar($"select SUM([unit_price] * [quantity] - [amount]) from [dbo].[Consumption_list] where [Private_room] = '{Private_room_number}'");
            string unit_price_sum = DbHelper.executeScalar($"select SUM([unit_price] * [quantity]) from [dbo].[Consumption_list] where [Private_room] = '{Private_room_number}'");
            string deposit = DbHelper.executeScalar($"select [deposit] from [dbo].[Private_rooms] where [Private_rooms_ID] = '{Private_room_number}'");

            DbHelper.skinDataGridView(skinDataGridView1, $@"select [Private_room],[project_ID],[unit_price],[Fold_rate],[amount],CONVERT(decimal(18, 1),[unit_price] * [quantity] - [amount]) as 'WLJSB',[quantity],[amount] as '1145',[Waiter],[Crediting_time],[Bookkeeper] from [dbo].[Consumption_list]
            where [Private_room] = '{Private_room_number}'", "");

            skinLabel3.Text = Private_room_number;
            skinCaptionPanel1.Text = $"{Private_room_number} 消费清单  合计：{sum}";
            textBox2.Text = (float.Parse(sum) - float.Parse(deposit)).ToString("0.00");
            textBox1.Text = textBox2.Text;
            skinLabel16.Text = sum;
            skinLabel20.Text = Preferential;
            skinLabel24.Text = "0.00";
            skinLabel5.Text = unit_price_sum;
            skinLabel18.Text = deposit;
            skinLabel2.Text = $"ZD{DateTime.Now:yyyyMMdd}{Convert.ToInt32(DbHelper.executeScalar("select count(*)+1 from [dbo].[Checkout]")):0000}";

            skinDataGridView1.RowHeadersVisible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    if (float.Parse(textBox2.Text) - float.Parse(textBox1.Text) < 0)
                    {
                        skinLabel24.Text = Math.Abs((double.Parse(textBox2.Text) - double.Parse(textBox1.Text))).ToString("0.00");
                        return;
                    }
                }

                skinLabel24.Text = "0.00";
            }catch(Exception ee)
            {
                MessageBox.Show(ee.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
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
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("宾客支付不能为空","系统提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            if (!IsNumber(textBox1.Text))
            {
                MessageBox.Show("宾客支付只能输入数字", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DbHelper.executeNonQuery($@"insert into [dbo].[Checkout]([Number], [Private_rooms_Name], [Should], [Actual], [deposit])
            values('{skinLabel2.Text}','{Private_room_number}','{skinLabel16.Text}','{textBox2.Text}','{skinLabel18.Text}')");

            DbHelper.executeNonQuery($"delete from [dbo].[Consumption_list] where [Private_room] = '{Private_room_number}'");

            DbHelper.executeNonQuery($@"update [dbo].[Private_rooms] set
            [Private_room_status] = '0',
            [Amount_spent] = NULL,
            [Start_time] = NULL,
            [Elapsed_time] = NULL,
            [remark] = NULL,
            [deposit] = NULL
            where [Private_rooms_ID] = '{Private_room_number}'");

            Close();
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textBox4.Focus())
            {
                if (DbHelper.executeScalar($"select count(*) from [dbo].[Member_Information] where [InformationID] = '{textBox4.Text}' and [state] = '0'") == "1")
                {
                    skinLabel9.Text = DbHelper.executeScalar($"select [memberName] from [dbo].[Member_Information] where [InformationID] = '{textBox4.Text}'");
                    skinLabel7.Text = DbHelper.executeScalar($@"select [TypeName] from [dbo].[Member_Information] as a
                    join [dbo].[Member] as b on a.Membership_Level = b.memberID
                    where [InformationID] = '{textBox4.Text}'");
                    skinLabel11.Text = DbHelper.executeScalar($"select [Integral] from [dbo].[Member_Information] where [InformationID] = '{textBox4.Text}'");
                    skinLabel14.Text = DbHelper.executeScalar($@"select [Fold_rate] from [dbo].[Member_Information] as a
                    join [dbo].[Member] as b on a.Membership_Level = b.memberID
                    where [InformationID] = '{textBox4.Text}'");

                    double Should = 0.00;
                    double Preferential = 0.00;

                    foreach (DataGridViewRow row in skinDataGridView1.Rows)
                    {
                        row.Cells["Column4"].Value = skinLabel14.Text;
                        row.Cells["Column5"].Value = (Convert.ToDouble(row.Cells["Column3"].Value) * Convert.ToDouble(row.Cells["Column4"].Value)).ToString("0.00");
                        row.Cells["Column6"].Value = (Convert.ToDouble(row.Cells["Column3"].Value) - Convert.ToDouble(row.Cells["Column5"].Value)).ToString("0.00");
                        row.Cells["Column8"].Value = (Convert.ToDouble(row.Cells["Column5"].Value) * Convert.ToDouble(row.Cells["Column7"].Value)).ToString("0.00");

                        Should += Convert.ToDouble(row.Cells["Column8"].Value);
                        Preferential += Convert.ToDouble(row.Cells["Column6"].Value);
                    }

                    Should -= Convert.ToDouble(skinLabel18.Text);

                    skinLabel16.Text = Should.ToString();
                    skinLabel20.Text = Preferential.ToString();
                    textBox2.Text = Should.ToString();
                    textBox1.Text = Should.ToString();

                    textBox4.ReadOnly = true;
                    return;
                }

                textBox4.Text = "普通宾客";
                MessageBox.Show("该会员不存在或者已被停用","系统提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
