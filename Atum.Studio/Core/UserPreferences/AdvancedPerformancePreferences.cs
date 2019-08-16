using System.ComponentModel;

namespace Atum.Studio.Core.UserPreferences
{
    public class AdvancedPerformancePreferences
    {
        [Category("Performance")]
        [DisplayName("Enable MultiThreading")]
        [Description("Enable MultiThreading")]
        public bool UseMultiThreading { get; set; }

        public AdvancedPerformancePreferences(bool useMultiThreading)
        {
            this.UseMultiThreading = useMultiThreading;
        }
    }
}
