using Atum.Studio.Controls.OpenGL;
using System;
using System.Windows.Forms;

namespace Atum.Studio.Controls.QuickTips
{
    class ModelRotationAxisYQuickTip : QuickTips.QuickTipActionInformation
    {
        internal ModelRotationAxisYQuickTip ()
        {
            this.QuickTipCaption = "Model Rotation";
            this.QuickTipText = "Selected Rotation-Axis: Y" + Environment.NewLine +
                                "Rotation angle: 0°";

        }

    }


}
