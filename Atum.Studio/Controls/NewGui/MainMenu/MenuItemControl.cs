using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Controls.NewGui.MainMenu
{
    public partial class MenuItemControl : UserControl
    {
        public new event EventHandler OnClick;
        public string ItemName {get;set;}
        public MenuItemControl(string itemName)
        {
            ItemName = itemName;
            InitializeComponent();

            this.BackColor = BrandingManager.Menu_BackgroundColor;
            this.lblMenuName.ForeColor = BrandingManager.Menu_Item_ForeColor;
            this.lblMenuName.Text = ItemName;

        }

        private void OnEnterChange()
        {
            plMenuItem.BackColor = BrandingManager.Menu_Item_HighlightColor;
        }
        private void OnLeaveChange()
        {
            plMenuItem.BackColor = BrandingManager.Menu_BackgroundColor;
        }

        private void plMenuItem_MouseEnter(object sender, EventArgs e)
        {
            OnEnterChange();
        }

        private void plMenuItem_MouseLeave(object sender, EventArgs e)
        {
            OnLeaveChange();
        }

        private void lblMenuName_Click(object sender, EventArgs e)
        {
            this.OnClick?.Invoke(this, null);
        }
    }
}
