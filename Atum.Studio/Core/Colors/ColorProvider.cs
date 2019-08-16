using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Atum.Studio.Core.Colors
{
    internal static class ColorProvider
    {
        private static Color GetHeatColor(int percentage)
        {
            if (percentage > 50)
            {
                return Color.FromArgb(255, 255, ((int)Math.Floor(((double)(255 / 50)) * (100 - percentage))), 0);
            }
            else if (percentage == 50)
            {
                return Color.FromArgb(255, 255, 255, 0);
            }
            else
            {
                return Color.FromArgb(255, (int)Math.Floor(((double)(255 / 50) * percentage)), 255, 0);
            }

        }
    }
}
