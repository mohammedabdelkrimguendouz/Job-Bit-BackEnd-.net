using System;
using System.Diagnostics;

namespace JobBit_DataAccess
{
    public class clsEventLogData
    {
        public static void WriteEvent(string message, EventLogEntryType eventLogEntryType)
        {
            string messageWithDateTime = DateTime.Now.ToString("d/M/yyyy HH:mm") + " - " + message;
            string sourceName = "JobBit";

            try
            {
                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, messageWithDateTime, eventLogEntryType);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
