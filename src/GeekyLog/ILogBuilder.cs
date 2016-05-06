using System;
using System.Runtime.CompilerServices;
using GeekyLog.Annotations;
using GeekyLog.Base;

namespace GeekyLog
{
    public interface ILogBuilder<out TBaseEventInfo>
    {
        ILogBuilder<TBaseEventInfo> CreateInfo(string message,
            [CallerMemberName, CanBeNull] string methodName = null,
            [CallerFilePath, CanBeNull] string path = null);

        ILogBuilder<TBaseEventInfo> SetException(Exception exception);
        ILogBuilder<TBaseEventInfo> SetBackStack(string backStack);
        TBaseEventInfo Build();
    }
}