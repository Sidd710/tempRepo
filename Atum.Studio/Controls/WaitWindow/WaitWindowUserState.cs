using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Studio.Controls
{
    internal class WaitWindowUserState
    {
        public double ProgressValue { get; set; }
        public string ProgressText { get; set; }

        public WaitWindowUserState(double progressValue, string progressText)
        {
            this.ProgressValue = progressValue;
            this.ProgressText = progressText;
        }
    }
}
