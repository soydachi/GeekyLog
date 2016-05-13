using System;
using System.Diagnostics.Tracing;
using System.Security;
using System.Windows.Input;
using GeekyLog.Base;
using GeekyLog.Interfaces;
using GeekyTool.Base;

namespace SampleUwp.ViewModels
{
    public class MainViewModel : UwpBaseViewModel
    {
        public MainViewModel()
        {
            
            RaiseException = new DelegateCommand<EventLevel>(RaiseExceptionExec);
        }

        private void RaiseExceptionExec(EventLevel level)
        {
            var model = new BaseEventInfo();
            switch (level)
            {
                case EventLevel.Critical:
                    model = (BaseEventInfo) Logger.Factory.CreateInfo($"Sending an {EventLevel.Critical} Log event.")
                        .SetException(new SecurityException())
                        .Build();
                    Logger.Log.Critical(model);
                    break;
                case EventLevel.Error:
                    model = (BaseEventInfo)Logger.Factory.CreateInfo($"Sending an {EventLevel.Error} Log event.")
                        .SetException(new SecurityException())
                        .Build();
                    Logger.Log.Error(model);
                    break;
                case EventLevel.Informational:
                    model = (BaseEventInfo)Logger.Factory.CreateInfo($"Sending an {EventLevel.Informational} Log event.")
                        .SetException(new SecurityException())
                        .Build();
                    Logger.Log.Info(model);
                    break;
                case EventLevel.LogAlways:
                    model = (BaseEventInfo)Logger.Factory.CreateInfo($"Sending an {EventLevel.LogAlways} Log event.")
                        .SetException(new SecurityException())
                        .Build();
                    Logger.Log.Debug(model);
                    break;
                case EventLevel.Verbose:
                    model = (BaseEventInfo) Logger.Factory.CreateInfo($"Sending an {EventLevel.Verbose} Log event.")
                        .SetException(new SecurityException())
                        .Build();
                    Logger.Log.Debug(model);
                    break;
                case EventLevel.Warning:
                    model = (BaseEventInfo)Logger.Factory.CreateInfo($"Sending an {EventLevel.Warning} Log event.")
                        .SetException(new SecurityException())
                        .Build();
                    Logger.Log.Warn(model);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        public ICommand RaiseException { get; private set; }
    }
}
