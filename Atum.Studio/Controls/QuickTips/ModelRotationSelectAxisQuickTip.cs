using System;

namespace Atum.Studio.Controls.QuickTips
{
    class ModelRotationSelectAxisQuickTip : QuickTips.QuickTipActionInformation
    {
        internal ModelRotationSelectAxisQuickTip()
        {
            this.QuickTipCaption = "Model Rotation";
            this.QuickTipText = "Select the appropiate rotation axis" + Environment.NewLine + Environment.NewLine +
                                "Press and hold left mouse button" + Environment.NewLine +
                                "to start rotating the model.";

        }

    }


}
