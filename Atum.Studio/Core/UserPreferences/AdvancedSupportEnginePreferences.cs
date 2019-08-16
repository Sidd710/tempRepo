using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.UserPreferences
{
    public class AdvancedSupportEnginePreferences
    {
        [Category("Support Engine")]
        [DisplayName("Support Basement Thickness")]
        [Description("Support Basement Thickness")]
        public float SupportBasementThickness { get; set; }

        [Category("Support Engine")]
        [DisplayName("Support Penetraction Depth")]
        [Description("Support Penetraction Depth")]
        public float SupportPenetrationDepth { get; set; }

        public AdvancedSupportEnginePreferences(float supportBasementThickness, float supportPenetrationDepth)
        {
            this.SupportBasementThickness = supportBasementThickness;
            this.SupportPenetrationDepth = supportPenetrationDepth;
        }

    }
}
