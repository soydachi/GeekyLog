using System;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.CompilerServices;
using GeekyLog.Annotations;
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

    public abstract class LogBuilder
    {

        public abstract LogFactory CreateInfo(string message, string category,
            [CallerMemberName, CanBeNull] string methodName = null,
            [CallerFilePath, CanBeNull] string path = null);
        public abstract LogFactory SetException(Exception exception);
        public abstract LogFactory SetBackStack(string backStack);
        public abstract BaseEventInfo Build();
    }

    public class LogFactory : LogBuilder
    {
        private BaseEventInfo model;

        public override LogFactory CreateInfo([NotNull] string message, [NotNull] string category,
            [CallerFilePath, CanBeNull] string path = null, [CallerMemberName, CanBeNull] string methodName = null)
        {
            model = new BaseEventInfo();
            model.Message = message;
            model.Category = category;
            model.ErrorPath = $"{path.Split('\\').Last().Split('.').First()}.{methodName}()";
            return this;
        }

        public override LogFactory SetException([NotNull] Exception exception)
        {
            model.Exception = exception;
            model.StackTrace = exception.StackTrace;
            return this;
        }

        public override LogFactory SetBackStack([NotNull] string backStack)
        {
            model.BackStack = backStack;
            return this;
        }

        public override BaseEventInfo Build()
        {
            return model;
        }

        public BaseEventInfo Build([NotNull] string message, [NotNull] string category, [NotNull] Exception exception,
            [CanBeNull] string backStack = null, [CallerFilePath, CanBeNull] string path = null,
            [CallerMemberName, CanBeNull] string methodName = null)
        {
            var model = new BaseEventInfo();
            model.Message = message;
            model.Category = category;
            model.ErrorPath = $"{path}.{methodName}()";
            model.Exception = exception;
            model.StackTrace = exception.StackTrace;
            model.BackStack = backStack;

            return model;
        }
    }
}