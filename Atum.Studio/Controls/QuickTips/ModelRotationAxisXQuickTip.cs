using Atum.Studio.Controls.OpenGL;
using System;
using System.Windows.Forms;

namespace Atum.Studio.Controls.QuickTips
{
    class ModelRotationAxisXQuickTip : QuickTips.QuickTipActionInformation
    {
        internal ModelRotationAxisXQuickTip()
        {
            this.QuickTipCaption = "Model Rotation";
            this.QuickTipText = "Selected Rotation-Axis: X" + Environment.NewLine +
                                "Rotation angle: 0°";

        }

    }


}
