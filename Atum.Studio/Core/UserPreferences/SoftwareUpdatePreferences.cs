using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.UserPreferences
{
    internal class SoftwareOptionsPreferences
    {
        [Category("Software Options")]
        [DisplayName("Check for software updates")]
        [Description("Check for software updates")]
        public bool CheckForSoftwareUpdates { get; set; }

        internal SoftwareOptionsPreferences(bool checkForSoftwareUpdates)
        {
            this.CheckForSoftwareUpdates = checkForSoftwareUpdates;
        }
    }
}
