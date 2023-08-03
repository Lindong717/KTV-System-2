using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KTV_management_system
{
    class WindowHelper
    {
        public static string GetCurrentWindowText()
        {
            Form currentForm = Form.ActiveForm;

            if (currentForm != null)
            {
                return currentForm.Text;
            }

            return string.Empty;
        }
    }
}
