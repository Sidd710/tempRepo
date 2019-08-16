using Atum.Studio.Controls.OpenGL;
using System;
using System.Windows.Forms;

namespace Atum.Studio.Controls.QuickTips
{
    class ModelRotationAxisZQuickTip : QuickTips.QuickTipActionInformation
    {
        internal ModelRotationAxisZQuickTip()
        {
            this.QuickTipCaption = "Model Rotation";
            this.QuickTipText = "Selected Rotation-Axis: Z" + Environment.NewLine +
                                "Rotation angle: 0°";

        }

    }


}
