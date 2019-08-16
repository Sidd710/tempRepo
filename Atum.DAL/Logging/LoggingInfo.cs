using System;
namespace Atum.DAL.Logging
{

    public class LoggingInfo: IDisposable
    {

        public string LogTime { get; set; }
        public string Property { get; set; }
        public string Value { get; set; }
        public string Phase { get; set; }

        public LoggingInfo(string logTime, string phase, string prop, string value)
        {
            this.LogTime = logTime;
            this.Phase = phase;
            this.Property = prop;
            this.Value = value;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                    this.LogTime = null;
                    this.Property = null;
                    this.Phase = null;
                    this.Value = null;
            }
        }
    }
}
