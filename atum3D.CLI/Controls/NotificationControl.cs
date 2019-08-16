
using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace atum3D.Notification
{

    public partial class NotificationControl : UserControl
    {

        public NotificationControl()
        {
            // And then show it
            InitializeComponent();

            this.notificationIcon.Icon = BrandingManager.MainForm_Icon;
            this.notificationIcon.Text = BrandingManager.CLI_Notification_Title;
        }
    }
}
