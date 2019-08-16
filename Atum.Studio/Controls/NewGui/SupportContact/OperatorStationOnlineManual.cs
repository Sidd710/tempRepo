using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Controls.NewGui.SupportContact
{
    public partial class OperatorStationOnlineManual : UserControl
    {
        public OperatorStationOnlineManual()
        {
            InitializeComponent();

            this.linkLabel2.Links.Add(new LinkLabel.Link() { LinkData = "https://www.atum3d.com/includes/pdf/atum3D_Operator_Station_Manual.pdf" });

            if (FontManager.Loaded)
            {
                this.linkLabel2.Font = FontManager.Montserrat14Regular;
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start((string)e.Link.LinkData, string.Empty);
        }
    }
}
