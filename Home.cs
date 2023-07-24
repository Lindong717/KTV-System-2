using CCWin;
using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTV_management_system
{
    /// <summary>
    /// 首页
    /// </summary>
    public partial class Home : Skin_Mac
    {
        private int lastSelectedIndex = -1;
        private static string sqlCmd = @"select [Private_rooms_ID],[Private_room_status] = (case [Private_room_status] when '0' then '可供' when '1' then '占用' when '2' then '停用' when '3' then '预订' end),[Amount_spent],[Start_time],[Elapsed_time],[remark] from [dbo].[Private_rooms] where 1=1";
        private static string condition;
        private static string state;

        public Home()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            skinCaptionPanel1.Text = DateTime.Now.ToString("yyyy-MM-dd  HH:MM:ss");
            skinCaptionPanel5.Text = DateTime.Now.ToString("yyyy-MM-dd  HH:MM:ss");

            skinLabel7.Text = DbHelper.executeScalar("select count(*) from [dbo].[Private_rooms]");
            skinLabel13.Text = DbHelper.executeScalar("select count(*) from [dbo].[Private_rooms] where [Private_room_status] = '1'");
            skinLabel14.Text = DbHelper.executeScalar("select count(*) from [dbo].[Private_rooms] where [Private_room_status] = '0'");
            skinLabel15.Text = DbHelper.executeScalar("select count(*) from [dbo].[Private_rooms] where [Private_room_status] = '3'");
            skinLabel16.Text = DbHelper.executeScalar("select count(*) from [dbo].[Private_rooms] where [Private_room_status] = '2'");

            foreach (SkinButton skinButton in skinFlowLayoutPanel1.Controls)
            {
                if (skinButton.Capture) // 判断当前 skinButton 是否被鼠标捕获
                {
                    //记录控件标签
                    lastSelectedIndex = Convert.ToInt32(skinButton.Tag);
                }
                else
                {
                    //设置背景颜色
                    skinButton.BaseColor = Color.FromArgb(9, 163, 220);
                }

                if (Convert.ToInt32(skinButton.Tag) == lastSelectedIndex)// 判断标签是否和记录一致
                {
                    //设置背景颜色
                    skinButton.BaseColor = Color.MediumAquamarine;
                }

                //刷新控件样式
                skinButton.Invalidate();
            }

            skinCaptionPanel2.Text = DbHelper.executeScalar($"select [type_Name] from [dbo].[Type_of_private_room] where [Private_rooms_type_ID] = '{lastSelectedIndex}'");

            foreach (SkinButton skinButton1 in skinFlowLayoutPanel1.Controls)
            {
                if (skinButton1.Capture)
                {
                    Inquire();
                }
            }

            UpdateMenuItemsEnabledState();
        }

        public void Inquire()
        {
            //创建临时Sql语句
            string tmp = sqlCmd;

            //拼接查询条件
            condition += $" and [Private_rooms_type] = '{lastSelectedIndex}'";
            condition += state;

            //将条件拼接至临时Sql语句
            tmp += condition;

            //清空原有数据并重新添加
            skinListView1.Items.Clear();
            DbHelper.Private_roomsListView(skinListView1, tmp);

            //清空条件
            condition = null;
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            skinPanel5.Visible = false;
            skinCaptionPanel5.Visible = true;
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            skinPanel5.Visible = true;
            skinCaptionPanel5.Visible = false;
        }

        private void Home_Load(object sender, EventArgs e)
        {
            DbHelper.Add_button(skinFlowLayoutPanel1, @"select [Private_rooms_type],[type_Name] from [dbo].[Private_rooms] as a
            join [Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
            group by [Private_rooms_type],[type_Name]");

            DbHelper.Private_roomsListView(skinListView1, "select [Private_rooms_ID],[Private_room_status] = (case [Private_room_status] when '0' then '可供' when '1' then '占用' when '2' then '停用' when '3' then '预订' end),[Amount_spent],[Start_time],[Elapsed_time],[remark] from [dbo].[Private_rooms] where [Private_rooms_type] = '1'");

            skinListView1.HideSelection = true;
            lastSelectedIndex = 1;

            bool fileExists = CheckFileExists(FilePath());

            if (fileExists)
            {

                using (FileStream stream = new FileStream(FilePath(), FileMode.Open))
                {
                    byte[] bytes = new byte[stream.Length];

                    // 读取文件流中的数据
                    stream.Read(bytes, 0, bytes.Length);

                    // 将字节数组转换为字符串
                    textBox1.Text = Encoding.UTF8.GetString(bytes);

                }
            }
        }

        private void write()
        {
            using (FileStream stream = new FileStream(FilePath(), FileMode.Create))
            {
                string data = textBox1.Text; // 要写入的数据

                // 将字符串转换为字节数组
                byte[] bytes = Encoding.UTF8.GetBytes(data);

                // 写入数据到文件流
                stream.Write(bytes, 0, bytes.Length);

                stream.Close();
            }
        }

        private string FilePath()
        {
            string ExegesisFileName = "114514.txt";
            string currentDirectory = Environment.CurrentDirectory;
            return Path.Combine(currentDirectory, ExegesisFileName);
        }

        static bool CheckFileExists(string folderPath)
        {
            try
            {
                // 判断文件是否存在
                return File.Exists(folderPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
        }

        private void 列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            skinListView1.View = View.Details;
        }

        private void 小图标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View_the_way(Small_icons);
        }

        private void 中图标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View_the_way(medium);
        }

        private void 大图标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            View_the_way(Large_icons);
        }

        private void View_the_way(ImageList imageList)
        {
            skinListView1.LargeImageList = imageList;
            skinListView1.View = View.LargeIcon;
        }

        private void 显示可供ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state = " and [Private_room_status] = '0'";
            Inquire();
        }

        private void 显示占用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state = " and [Private_room_status] = '1'";
            Inquire();
        }

        private void 显示停用ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            state = " and [Private_room_status] = '2'";
            Inquire();
        }

        private void UpdateMenuItemsEnabledState()
        {
            bool enable = skinListView1.SelectedItems.Count > 0;
            foreach (var item in skinContextMenuStrip1.Items)
            {
                if (item is ToolStripMenuItem toolStripMenuItem)
                {
                    toolStripMenuItem.Enabled = enable;
                }
            }

            if (enable)
            {
                List<int> list = new List<int>();

                switch (skinListView1.SelectedItems[0].SubItems[1].Text)
                {
                    case "可供":
                        list.Add(0);
                        list.Add(1);
                        list.Add(2);
                        list.Add(5);
                        list.Add(6);
                        list.Add(9);
                        list.Add(10);
                        list.Add(14);
                        break;
                    case "占用":
                        list.Add(3);
                        list.Add(12);
                        list.Add(14);
                        break;
                    case "停用":
                        list.Add(0);
                        list.Add(1);
                        list.Add(2);
                        list.Add(3);
                        list.Add(5);
                        list.Add(6);
                        list.Add(9);
                        list.Add(10);
                        list.Add(12);
                        list.Add(14);
                        break;
                    case "预订":
                        list.Add(0);
                        list.Add(1);
                        list.Add(2);
                        list.Add(5);
                        list.Add(6);
                        list.Add(9);
                        list.Add(10);
                        list.Add(12);
                        break;
                }

                foreach (int i in list)
                {
                    if (skinContextMenuStrip1.Items[i] is ToolStripMenuItem toolStripMenuItem)
                    {
                        toolStripMenuItem.Enabled = false;
                    }
                }

                list.Clear();
            }
        }

        private void 包间状态ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Modify_the_status modify_The_Status = new Modify_the_status();

            modify_The_Status.Room_number = skinListView1.SelectedItems[0].SubItems[0].Text;
            modify_The_Status.state = skinListView1.SelectedItems[0].SubItems[1].Text;

            modify_The_Status.ShowDialog();

            Inquire();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            System_settings system_Settings = new System_settings();
            system_Settings.ShowDialog();
        }

        private void skinListView1_Click_1(object sender, EventArgs e)
        {
            skinLabel17.Text = null;
            skinLabel18.Text = null;

            skinLabel17.Text = DbHelper.executeScalar($@"select [manner_Name] from [dbo].[Private_rooms] as a
                join [dbo].[Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
                join [dbo].[Billing_type] as c on b.Billing_method = c.manner_ID
                where [Private_rooms_ID] = '{skinListView1.SelectedItems[0].SubItems[0].Text}'");

            skinLabel18.Text = "￥" + DbHelper.executeScalar($@"select [Minimum_consumption] from [dbo].[Private_rooms] as a
                join [dbo].[Type_of_private_room] as b on a.Private_rooms_type = b.Private_rooms_type_ID
                where [Private_rooms_ID] = '{skinListView1.SelectedItems[0].SubItems[0].Text}'");

            if (skinListView1.SelectedItems[0].SubItems[1].Text == "占用")
            {
                skinLabel19.Text = $"{DbHelper.executeScalar($"SELECT MONTH([Start_time]) FROM [dbo].[Private_rooms] where [Private_rooms_ID] = '{skinListView1.SelectedItems[0].SubItems[0].Text}'")}月{DbHelper.executeScalar($"SELECT Day([Start_time]) FROM [dbo].[Private_rooms] where [Private_rooms_ID] = '{skinListView1.SelectedItems[0].SubItems[0].Text}'")}日";
                skinLabel20.Text = DbHelper.executeScalar($"select [Elapsed_time] from [dbo].[Private_rooms] where [Private_rooms_ID] = '{skinListView1.SelectedItems[0].SubItems[0].Text}'");
                skinLabel21.Text = "￥" + DbHelper.executeScalar($"select [deposit] from [dbo].[Private_rooms] where [Private_rooms_ID] = '{skinListView1.SelectedItems[0].SubItems[0].Text}'");
                skinLabel22.Text = "￥" + DbHelper.executeScalar($"select SUM([amount]) from [dbo].[Consumption_list] where [Private_room] = '{skinListView1.SelectedItems[0].SubItems[0].Text}'");

                skinCaptionPanel6.Text = $"{skinListView1.SelectedItems[0].Text}包间\t消费订单";
                return;
            }

            skinCaptionPanel6.Text = null;
            skinLabel19.Text = null;
            skinLabel20.Text = null;
            skinLabel21.Text = null;
            skinLabel22.Text = null;
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            write();

            if (MessageBox.Show("是否退出系统", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            Inquire();
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            state = null;
            Inquire();
        }

        private void 退出系统ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Billing()
        {
            if (string.IsNullOrEmpty(skinListView1.SelectedItems[0].ToString()))
            {
                MessageBox.Show("请选择包间", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (skinListView1.SelectedItems[0].SubItems[1].Text == "停用")
            {
                MessageBox.Show("该包间已被停用", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Customer_billing customer_Billing = new Customer_billing();
            customer_Billing.Private_rooms_ID = skinListView1.SelectedItems[0].SubItems[0].Text;
            customer_Billing.ShowDialog();

            Inquire();
        }

        private void Increase_consumption()
        {
            Increase_consumption increase_Consumption = new Increase_consumption();
            increase_Consumption.Private_rooms_ID = skinListView1.SelectedItems[0].SubItems[0].Text;
            increase_Consumption.ShowDialog();
        }

        private void skinListView1_DoubleClick(object sender, EventArgs e)
        {
            if (skinListView1.SelectedItems[0].SubItems[1].Text != "占用")
            {
                Billing();
                return;
            }

            Increase_consumption();
        }

        private void 顾客开单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Billing();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (skinListView1.SelectedItems[0].SubItems[1].Text == "占用")
            {
                MessageBox.Show("该包间已被占用", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Billing();
        }

        private void 增加消费ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Increase_consumption();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (skinListView1.SelectedItems[0].SubItems[1].Text != "占用")
            {
                MessageBox.Show("不能对非占用包间进行该操作", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Increase_consumption();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < skinListView1.Items.Count; i++)
            {
                if (skinListView1.Items[i].SubItems[1].Text == "占用")
                {
                    string Initial_time = DbHelper.executeScalar($"select [Start_time] from [dbo].[Private_rooms] where [Private_rooms_ID] = '{skinListView1.Items[i].SubItems[0].Text}'");
                    string time = $"{DbHelper.executeScalar($"SELECT DATEDIFF(HH, '{Initial_time}', GETDATE())")}小时，{DbHelper.executeScalar($"SELECT DATEDIFF(MINUTE, '{Initial_time}', GETDATE());")}分钟";
                    
                    DbHelper.executeNonQuery($"update [dbo].[Private_rooms] set [Elapsed_time] = '{time}' where [Private_rooms_ID] = '{skinListView1.Items[i].SubItems[0].Text}'");
                }
            }
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string calculatorPath = @"C:\Windows\System32\calc.exe";

                Process.Start(calculatorPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("无法打开计算器： " + ex.Message);
            }
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            textBox1.Text = null;
            write();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            textBox1.Text += Environment.NewLine + $"----{DateTime.Now}----";
        }
    }
}
