using System;
using System.Runtime.CompilerServices;
using GeekyLog.Annotations;
using GeekyLog.Base;

namespace GeekyLog
{
    public abstract class LogBuilder
    {
        public abstract LogFactory CreateInfo(string message, object category,
            [CallerMemberName, CanBeNull] string methodName = null,
            [CallerFilePath, CanBeNull] string path = null);
        public abstract LogFactory SetException(Exception exception);
        public abstract LogFactory SetBackStack(string backStack);
        public abstract BaseEventInfo Build();
    }
}