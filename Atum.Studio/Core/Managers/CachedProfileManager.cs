using System;
using System.Timers;

namespace Atum.Studio.Core.Managers
{
    public class CachedProfileManager {

        private static Timer _cleanupTimer;


        public static void Start()
        {
            _cleanupTimer = new Timer();
            _cleanupTimer.Interval = (new TimeSpan(0, 5, 0)).TotalMilliseconds; //5 minutes
            _cleanupTimer.Start();
            _cleanupTimer.Elapsed += _cleanupTimer_Elapsed;
        }

        private static void _cleanupTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _cleanupTimer.Stop();
            UserProfileManager.CleanupCachedFiles();
            _cleanupTimer.Start();
        }

    }
}
