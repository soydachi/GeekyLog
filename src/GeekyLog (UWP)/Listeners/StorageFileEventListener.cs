using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Text;
using System.Threading;
using Windows.Storage;
using GeekyLog.Base;
using GeekyLog.Interfaces;
using Newtonsoft.Json;

namespace GeekyLog.Listeners
{
    public sealed class StorageFileEventListener<TBaseEventInfo> : EventListener where TBaseEventInfo : BaseEventInfo
    {
        private readonly ISerializeListener serializeListener;
        private StorageFile storageFile;
        private readonly SemaphoreSlim semaphoreSlim;
        private readonly string name;

        public StorageFileEventListener(string name, JsonSerializerSettings serializerSettings = null)
        {
            serializeListener = new SerializeListener(serializerSettings);
            semaphoreSlim = new SemaphoreSlim(1);
            storageFile = null;
            this.name = name;

            Debug.WriteLine("StorageFileEventListener for {0} has name {1}", GetHashCode(), name);

            AssignLocalFile();
        }

        private async void AssignLocalFile()
        {
            storageFile =
                await
                    ApplicationData.Current.LocalFolder.CreateFileAsync(name.Replace(" ", "_") + ".log",
                        CreationCollisionOption.OpenIfExists);
        }

        private async void WriteToFile(IEnumerable<string> lines)
        {
            await semaphoreSlim.WaitAsync();

            try
            {
                await FileIO.AppendLinesAsync(storageFile, lines);
            }
            catch (Exception)
            {
                Debug.WriteLine("WriteToFile on StorageFileEventListener failed. Listener {0} has name {1}",
                    GetHashCode(), name);
                // TODO
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (storageFile == null) return;

            var lines = new List<string>();

            var model = serializeListener.Deserialize<TBaseEventInfo>(eventData.Payload[0].ToString());
            model.Level = eventData.Level;

            var builder = new StringBuilder();
            builder.AppendLine(
                $"{model.TimeStamp}\tEventLevel: {eventData.Level}\tMessage: {model.Message}");
            builder.AppendLine($"{model.TimeStamp}\tErrorPath: {model.ErrorPath}");

            if (model.ExceptionName != null)
                builder.AppendLine($"{model.TimeStamp}\t{model.ExceptionName}");
            if (!string.IsNullOrEmpty(model.StackTrace))
                builder.AppendLine($"{model.TimeStamp}\t{model.StackTrace}");
            if (!string.IsNullOrEmpty(model.BackStack))
                builder.AppendLine($"{model.TimeStamp}\tBackStack: {model.BackStack}");

            Debug.WriteLine(builder);
            lines.Add(builder.ToString());
            WriteToFile(lines);
        }

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            Debug.WriteLine("OnEventSourceCreated for Listener {0} - {1} got eventSource {2}", GetHashCode(), name, eventSource.Name);
        }
    }
}
