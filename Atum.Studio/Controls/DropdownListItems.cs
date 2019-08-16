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

namespace Atum.Studio.Controls
{
    public partial class DropdownListItems : ToolStripDropDown
    {
        public DropdownListItems()
        {
            InitializeComponent();

            this.AutoSize = false;
            this.Renderer = new ToolStripProfessionalRenderer(new DropdownListItemColorTable());
            this.DropShadowEnabled = false;
            this.Padding = new Padding(0);

            if (FontManager.Loaded)
            {
                this.Font = FontManager.Montserrat14Regular;
            }
        }
    }

    internal class DropdownListItemColorTable : ProfessionalColorTable {

        public override Color MenuItemSelected {
            get
            {
                return Color.White;
            }
        }

        public override Color MenuBorder
        {
            get
            {
                return Color.Gray;
            }
        }

        public override Color MenuItemBorder
        {
            get
            {
                return Color.White;
            }
        }
    }

}
