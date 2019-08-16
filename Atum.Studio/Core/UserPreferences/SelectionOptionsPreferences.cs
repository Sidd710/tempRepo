using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.UserPreferences
{
    internal class SelectionOptionsPreferences
    {
        [Category("Selection Options")]
        [DisplayName("Use Annotations")]
        [Description("Use Annotations")]
        public bool UseAnnotations { get; set; }

        [Category("Selection Options")]
        [DisplayName("Show XYZ-axis")]
        [Description("Show XYZ-axis")]
        public bool ShowXYZAxis { get; set; }

        internal SelectionOptionsPreferences(bool useAnnotations, bool showXYZAxis)
        {
            this.UseAnnotations = useAnnotations;
            this.ShowXYZAxis = showXYZAxis;
        }
    }
}
