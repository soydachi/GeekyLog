using System;

namespace GeekyLog.Base
{
    public interface IBaseEventInfo
    {
        Guid Id { get; }
        DateTimeOffset TimeStamp { get; }
        string Message { get; set; }
        string Category { get; set; }
        string ErrorPath { get; set; }
        Exception Exception { get; set; }
        string StackTrace { get; set; }
        string BackStack { get; set; }
    }

    public class BaseEventInfo : IBaseEventInfo
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTimeOffset TimeStamp { get; } = DateTimeOffset.Now;
        public string Message { get; set; }
        public string Category { get; set; }
        public string ErrorPath { get; set; }
        public Exception Exception { get; set; }
        public string StackTrace { get; set; }
        public string BackStack { get; set; }
    }

    public class StorageFileEventInfo : BaseEventInfo
    {
    }
}
