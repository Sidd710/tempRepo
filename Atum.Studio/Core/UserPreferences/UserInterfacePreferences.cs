using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.UserPreferences
{
    public class UserInterfacePreferences
    {
        [Category("User Interface")]
        [DisplayName("Use large toolbar icons")]
        [Description("Use large toolbar icons")]
        public bool UseLargeToolbarIcons { get; set; }

        [Category("User Interface")]
        [DisplayName("Skip welcome screen on next start")]
        [Description("Skip welcome screen on next start")]
        public bool SkipWelcomeScreenOnNextStart { get; set; }

        [Category("User Interface")]
        [DisplayName("Use Support Basement")]
        [Description("Use Support Basement")]
        public bool UseSupportBasement { get; set; }

        [Category("User Interface")]
        [DisplayName("Lift Model On Support")]
        [Description("Lift Model On Support")]
        public bool LiftModelOnSupport { get; set; }

        public UserInterfacePreferences(bool useLargeToolbarIcons, bool skipWelcomeScreenOnNextStart, bool useSupportBasement, bool enableTouchInterfaceMode)
        {
            this.UseLargeToolbarIcons = useLargeToolbarIcons;
            this.SkipWelcomeScreenOnNextStart = skipWelcomeScreenOnNextStart;
            this. = liftModelOnSupport;
            this.UseSupportBasement = useSupportBasement;
        }
    }
}
