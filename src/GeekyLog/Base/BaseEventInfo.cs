using System;
using System.Diagnostics.Tracing;

namespace GeekyLog.Base
{
    public class BaseEventInfo
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime TimeStamp { get; } = DateTime.Now;
        public EventLevel Level { get; set; }
        public string Message { get; set; }
        public object Category { get; set; }
        public string ErrorPath { get; set; }
        public string ExceptionName { get; set; }
        public string StackTrace { get; set; }
        public string BackStack { get; set; }
    }

    public class StorageFileEventInfo : BaseEventInfo
    {
    }

    public class SqliteEventInfo
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime TimeStamp { get; } = DateTime.Now;
        public EventLevel Level { get; set; }
        public string Message { get; set; }
        public int Category { get; set; }
        public string ErrorPath { get; set; }
        public string ExceptionName { get; set; }
        public string StackTrace { get; set; }
        public string BackStack { get; set; }
    }
}
