using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GeekyLog.Annotations;
using GeekyLog.Base;
using GeekyLog.Interfaces;

namespace GeekyLog
{
    public class LogFactory<TBaseEventInfo> : ILogBuilder<TBaseEventInfo> where TBaseEventInfo : BaseEventInfo, new()
    {
        private TBaseEventInfo model;

        public ILogBuilder<TBaseEventInfo> CreateInfo([NotNull] string message,
            [CallerFilePath, CanBeNull] string path = null, [CallerMemberName, CanBeNull] string methodName = null)
        {
            model = new TBaseEventInfo();
            model.Message = message;
            model.ErrorPath = $"{path.Split('\\').Last().Split('.').First()}.{methodName}()";
            return this;
        }

        public ILogBuilder<TBaseEventInfo> SetException([NotNull] Exception exception)
        {
            model.ExceptionName = exception.GetType().Name;
            model.StackTrace = $"{exception.Message}\r\n{exception.InnerException?.Message}\r\n{exception.StackTrace}";
            return this;
        }

        public ILogBuilder<TBaseEventInfo> SetBackStack([CanBeNull] string backStack)
        {
            model.BackStack = backStack;
            return this;
        }

        public TBaseEventInfo Build()
        {
            return model;
        }

        public BaseEventInfo Build([NotNull] string message, [NotNull] Exception exception,
            [CanBeNull] string backStack = null, [CallerFilePath, CanBeNull] string path = null,
            [CallerMemberName, CanBeNull] string methodName = null)
        {
            return Logger.Factory.CreateInfo(message)
                .SetException(exception)
                .SetBackStack(backStack)
                .Build();
        }
    }
}