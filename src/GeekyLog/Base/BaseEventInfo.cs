using System;
using System.Diagnostics.Tracing;

namespace GeekyLog.Base
{
    public class BaseEventInfo
    {
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public EventLevel Level { get; set; }
        public string Message { get; set; }
        public string ErrorPath { get; set; }
        public string ExceptionName { get; set; }
        public string StackTrace { get; set; }
        public string BackStack { get; set; }
    }
}
