using System;
using System.Linq;
using System.Runtime.CompilerServices;
using GeekyLog.Annotations;
using GeekyLog.Base;
using GeekyLog.Interfaces;

namespace GeekyLog
{
    public class LogFactory : LogBuilder
    {
        private BaseEventInfo model;

        public override LogFactory CreateInfo([NotNull] string message, [NotNull] object category,
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
            model.ExceptionName = exception.GetType().Name;
            model.StackTrace = $"{exception.Message}\r\n{exception.StackTrace}";
            return this;
        }

        public override LogFactory SetBackStack([CanBeNull] string backStack)
        {
            model.BackStack = backStack;
            return this;
        }

        public override BaseEventInfo Build()
        {
            return model;
        }

        public BaseEventInfo Build([NotNull] string message, [NotNull] object category, [NotNull] Exception exception,
            [CanBeNull] string backStack = null, [CallerFilePath, CanBeNull] string path = null,
            [CallerMemberName, CanBeNull] string methodName = null)
        {
            return Logger.Factory.CreateInfo(message, category)
                .SetException(exception)
                .SetBackStack(backStack)
                .Build();
        }
    }
}