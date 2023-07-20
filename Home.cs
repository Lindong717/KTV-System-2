using CCWin;
using CCWin.SkinControl;
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
            if (skinListView1.SelectedItems[0].SubItems[1].Text == "占用")
            {
                skinCaptionPanel6.Text = $"{skinListView1.SelectedItems[0].Text}包间\t消费订单";
                return;
            }
            skinCaptionPanel6.Text = null;
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
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
    }
}
