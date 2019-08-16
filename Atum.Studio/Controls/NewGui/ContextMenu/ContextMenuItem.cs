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

namespace Atum.Studio.Controls.NewGui.ContextMenu
{
    public partial class ContextMenuItem : UserControl
    {

        public new event EventHandler OnClick;
        public string ItemName { get; set; }
        public ContextMenuItem(string itemName)
        {
            ItemName = itemName;
            InitializeComponent();

            this.BackColor = BrandingManager.ContextMenu_BackgroundColor;
            this.lblMenuName.ForeColor = BrandingManager.ContextMenu_Item_ForeColor;
            this.lblMenuName.Text = ItemName;

            if (FontManager.Loaded)
            {
                this.lblMenuName.Font = FontManager.Montserrat14Regular;
            }

        }

        private void OnEnterChange()
        {
            plMenuItem.BackColor = BrandingManager.ContextMenu_Item_HighlightColor;
            this.lblMenuName.ForeColor = BrandingManager.ContextMenu_Item_HighlightForeColor;
        }
        private void OnLeaveChange()
        {
            plMenuItem.BackColor = BrandingManager.ContextMenu_BackgroundColor;
            this.lblMenuName.ForeColor = BrandingManager.ContextMenu_Item_ForeColor;
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
