using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.DAL.Managers
{
    public class EventLogManager
    {
        private static EventLog eventLog;

        public static void Start()
        {
            if (!EventLog.Exists("atum3D"))
            {
                eventLog = CreateLog("atum3D");
            }
            else
            {
                eventLog = EventLog.GetEventLogs().First(s => s.LogDisplayName == "atum3D");
            }
        }

        public static void WriteToEventLog(string phase, string value)
        {
            eventLog.Source = "atum3D";
            eventLog.WriteEntry(string.Format("{0}: {1}", phase, value), EventLogEntryType.Information);
            Debug.WriteLine(string.Format("{0}: {1}", phase, value));
            Console.WriteLine(string.Format("{0}: {1}", phase, value));
        }

        static EventLog CreateLog(string strLogName)
        {
            
            try
            {
                EventLog.CreateEventSource(strLogName, strLogName);
                EventLog atum3DEventLog = new EventLog();

                atum3DEventLog.Source = strLogName;
                atum3DEventLog.Log = strLogName;

                atum3DEventLog.Source = strLogName;
                atum3DEventLog.WriteEntry("The " + strLogName + " was successfully initialize component.", EventLogEntryType.Information);
   

                return atum3DEventLog;
            }
            catch
            {
                return null;
            }
        }
    }
}
