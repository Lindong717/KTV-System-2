using CCWin;
using CCWin.SkinControl;
using MetroFramework.Controls;
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
    /// 系统设置
    /// </summary>
    public partial class System_settings : Skin_Mac
    {
        public System_settings()
        {
            InitializeComponent();
        }

        private void System_settings_Load(object sender, EventArgs e)
        {
            OpenForm(new Private_room_settings(), metroTabPage1);
            OpenForm(new Item_merchandise(), metroTabPage2);
            OpenForm(new Waiter(), metroTabPage3);
            OpenForm(new Membership_Settings(), metroTabPage4);
        }

        /// <summary>
        /// 窗体嵌入通用方法
        /// </summary>
        /// <param name="subForm"></param>；
        private void OpenForm(Form subForm,MetroTabPage metroTabPage)
        {
            foreach (Control item in metroTabPage.Controls)
            {
                if (item is Form)
                {
                    ((Form)item).Close();
                }
            }

            subForm.TopLevel = false;// 将子窗体设置为非顶级控件
            subForm.FormBorderStyle = FormBorderStyle.None;//设置无边框
            subForm.Parent = metroTabPage;//设置窗体容器
            subForm.Dock = DockStyle.Fill; //容器大小随着调整窗体大小自动变化
            subForm.Show();
        }
    }
}
