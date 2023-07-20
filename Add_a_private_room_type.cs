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
    /// <summary>
    /// 添加包间类型
    /// </summary>
    public partial class Add_a_private_room_type : Skin_Mac
    {
        private static List<bool> list = new List<bool>();

        public Add_a_private_room_type()
        {
            InitializeComponent();
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
        public static bool IsNumber(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            //匹配有小数点和没小数点的数字
            const string pattern = "^[0-9]+(.[0-9]{1})?$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(s);
        }

        private void Add_a_private_room_type_Load(object sender, EventArgs e)
        {
            DbHelper.skinCollections(skinComboBox1, "select [manner_ID],[manner_Name] from [dbo].[Billing_type]", "manner_ID", "manner_Name", "请选择");
            DbHelper.skinCollections(skinComboBox2, "select [manner_ID],[manner_Name] from [dbo].[Billing_type]", "manner_ID", "manner_Name", "请选择");
            DbHelper.skinCollections(skinComboBox3, "select [manner_ID],[manner_Name] from [dbo].[Billing_type]", "manner_ID", "manner_Name", "请选择");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            skinComboBox1.Enabled = skinCheckBox1.Checked;
            skinComboBox2.Enabled = skinCheckBox2.Checked;
            skinComboBox3.Enabled = skinCheckBox3.Checked;

            skinComboBox1.Text = skinCheckBox1.Checked ? skinComboBox1.Text : "请选择";
            skinComboBox2.Text = skinCheckBox2.Checked ? skinComboBox2.Text : "请选择";
            skinComboBox3.Text = skinCheckBox3.Checked ? skinComboBox3.Text : "请选择";
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!security_guard())
                {
                    if (IsNumber(skinTextBox2.Text) && IsNumber(skinTextBox3.Text) && IsNumber(skinTextBox4.Text) && IsNumber(skinTextBox5.Text))
                    {
                        DbHelper.executeNonQuery($@"insert into Type_of_private_room([type_Name], [Minimum_consumption], [Billing_method], [Ordinary_festivals], [Special_holidays], [Capacity], [Discount_conditions], [Fold_rate])
                        values('{skinTextBox1.Text}', '{skinTextBox2.Text}', '{(skinCheckBox1.Checked == true ? skinComboBox1.SelectedValue : -1)}', '{(skinCheckBox2.Checked == true ? skinComboBox2.SelectedValue : -1)}', '{(skinCheckBox3.Checked == true ? skinComboBox3.SelectedValue : -1)}', '{skinTextBox3.Text}', '{skinTextBox4.Text}', '{skinTextBox5.Text}')");
                        Close();
                    }
                    else
                    {
                        HandleError("一些部分只能包含数字");
                    }
                }
                else
                {
                    HandleError("请将内容填写完整");
                }
            }catch(Exception ee)
            {
                HandleError(ee.Message);
            }
        }

        private void HandleError(string errorMessage)
        {
            MessageBox.Show(errorMessage, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            // 记录错误信息到日志文件
        }

        public bool security_guard()
        {
            list.Add(!string.IsNullOrEmpty(skinTextBox1.Text));
            list.Add(!string.IsNullOrEmpty(skinTextBox2.Text));
            list.Add(!string.IsNullOrEmpty(skinTextBox3.Text));
            list.Add(!string.IsNullOrEmpty(skinTextBox4.Text));
            list.Add(!string.IsNullOrEmpty(skinTextBox5.Text));
            list.Add(judgment());

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

        public bool judgment()
        {
            // 获取父窗口的所有控件
            var allControls = Controls;

            // 使用LINQ查询过滤出所有已启用且选中项大于0的ComboBox控件，并计算其总数
            int totalComboBoxCount = allControls.OfType<SkinComboBox>()
                .Where(c => c.Enabled && c.SelectedIndex > 0)
                .Count();

            // 使用LINQ查询找出选中状态的CheckBox控件的数量
            int skinCheckBox = allControls.OfType<SkinCheckBox>()
                .Count(c => c.Checked);

            if (totalComboBoxCount == skinCheckBox && skinCheckBox > 0)
            {
                return true;
            }

            return false;
        }
    }
}
