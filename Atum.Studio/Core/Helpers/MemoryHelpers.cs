using Atum.DAL.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Helpers
{
    class MemoryHelpers
    {
        internal static void ForceGCCleanup()
        {
            Task.Run(() =>
            {
                LoggingManager.WriteToLog("Memory Helper", "GC.Collect()", "Started");

                GC.Collect();
                GC.Collect();

                LoggingManager.WriteToLog("Memory Helper", "GC.Collect()", "Stopped");
            });
            
        }

    }
}
