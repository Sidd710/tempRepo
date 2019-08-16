using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.UserPreferences
{
    public class AdvancedModelPreferences
    {
        [Category("Model")]
        [DisplayName("Mirror model before slicing")]
        [Description("Mirror model before slicing")]
        public bool MirrorObjects { get; set; }

        [Category("Model")]
        [DisplayName("First slice min amount of pixels")]
        [Description("First slice min amount of pixels")]
        public int FirstSliceMinAccountOfPixels { get; set; }

        public AdvancedModelPreferences(bool mirrorObjects, int firstSliceMinAccountOfPixels)
        {
            this.MirrorObjects = mirrorObjects;
            this.FirstSliceMinAccountOfPixels = firstSliceMinAccountOfPixels;
        }

    }
}
