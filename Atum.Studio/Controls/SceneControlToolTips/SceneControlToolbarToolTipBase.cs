using Atum.Studio.Core.Managers;
using Atum.Studio.Core.PInvoke;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.SceneControlToolTips
{
    public class SceneControlToolbarToolTipBase: ToolTip
    {
        internal Font _tooltipFont;
        internal int ToolTipOffsetX = 6;

        internal SceneControlToolbarToolTipBase()
        {
            if (FontManager.Loaded)
            {
                this._tooltipFont = FontManager.Montserrat14Regular;
                this.OwnerDraw = true;
            }
        }

        internal void DisableShadow(IntPtr handle)
        {
            var cs = NATIVE.GetClassLong(handle, NATIVE.GCL_STYLE);
            if ((cs & NATIVE.CS_DROPSHADOW) == NATIVE.CS_DROPSHADOW)
            {
                cs = cs & ~NATIVE.CS_DROPSHADOW;
                NATIVE.SetClassLong(handle, NATIVE.GCL_STYLE, cs);
            }
        }
    }
}
