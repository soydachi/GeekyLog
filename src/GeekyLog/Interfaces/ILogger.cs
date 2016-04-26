using System.Diagnostics.Tracing;
using GeekyLog.Base;
using Newtonsoft.Json;

namespace GeekyLog.Interfaces
{
    public interface ILogger
    {
        void Critical(BaseEventInfo message);
        void Debug(BaseEventInfo message);
        void Error(BaseEventInfo message);
        void Info(BaseEventInfo message);
        void Warn(BaseEventInfo message);
    }

    public class Logger : ILogger
    {
        private static ISerializeListener serializeListener;
        private static EventListener internalEventListener;

        public static void ConfigureListener(EventListener eventListener, EventLevel eventLevel,
            JsonSerializerSettings serializerSettings = null)
        {
            internalEventListener = eventListener;
            internalEventListener.EnableEvents(BaseEventSource.Log, eventLevel);

            serializeListener = new SerializeListener(serializerSettings);
        }

        public static ILogger Log = new Logger();
        public static LogFactory Factory = new LogFactory();
        
        public void Debug(BaseEventInfo message)
        {
            BaseEventSource.Log.Debug(serializeListener.Serialize(message));
        }

        public void Info(BaseEventInfo message)
        {
            BaseEventSource.Log.Info(serializeListener.Serialize(message));
        }

        public void Warn(BaseEventInfo message)
        {
            BaseEventSource.Log.Warn(serializeListener.Serialize(message));
        }

        public void Error(BaseEventInfo message)
        {
            BaseEventSource.Log.Error(serializeListener.Serialize(message));
        }

        public void Critical(BaseEventInfo message)
        {
            BaseEventSource.Log.Critical(serializeListener.Serialize(message));
        }
    }
}