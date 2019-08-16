using Atum.Studio.Core.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Atum.Studio.Core.Helpers
{
    class ThreadHelper
    {
        internal static bool WaitForAll(ManualResetEvent[] events)
        {
            bool result = false;
            try
            {
                if (events != null)
                {
                    for (int i = 0; i < events.Length; i++)
                    {
                        events[i].WaitOne();
                    }
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }


        internal static bool WaitForAll(IntersectionState[] events)
        {
            bool result = false;
            try
            {
                if (events != null)
                {
                    for (int i = 0; i < events.Length; i++)
                    {
                        events[i].EventWaitHandle.WaitOne();
                    }
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

    }
}
