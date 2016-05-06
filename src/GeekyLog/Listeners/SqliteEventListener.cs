using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Threading;
using System.Threading.Tasks;
using GeekyLog.Annotations;
using GeekyLog.Base;
using GeekyLog.Interfaces;
using Newtonsoft.Json;
using SQLite.Net;

namespace GeekyLog.Listeners
{
    public sealed class SqliteEventListener<TBaseEventInfo> : EventListener where TBaseEventInfo : BaseEventInfo
    {
        private readonly SemaphoreSlim semaphoreSlim;
        private readonly ISerializeListener serializeListener;
        private readonly SqliteBaseConfiguration sqliteConfiguration;
        private SQLiteConnection conn;
        private readonly string name;

        public SqliteEventListener([NotNull] string name, [NotNull] SqliteBaseConfiguration sqliteConfiguration, [CanBeNull] JsonSerializerSettings serializerSettings = null)
        {
            serializeListener = new SerializeListener(serializerSettings);
            semaphoreSlim = new SemaphoreSlim(1);
            this.sqliteConfiguration = sqliteConfiguration;
            this.name = name;

            Debug.WriteLine("SqliteEventListener for {0} has name {1}", GetHashCode(), name);

            using (conn = new SQLiteConnection(sqliteConfiguration.SQLitePlatform, sqliteConfiguration.Path, 
                storeDateTimeAsTicks: false))
            {
#if DEBUG
                conn.TraceListener = new DebugTraceListener();
#endif
                conn.CreateTable<TBaseEventInfo>();
            }
        }

        protected override async void OnEventWritten(EventWrittenEventArgs eventData)
        {
            await semaphoreSlim.WaitAsync();

            try
            {
                var model = serializeListener.Deserialize<TBaseEventInfo>(eventData.Payload[0].ToString());
                model.Level = eventData.Level;

                using (var conn = new SQLiteConnection(sqliteConfiguration.SQLitePlatform, sqliteConfiguration.Path,
                        storeDateTimeAsTicks: false))
                {
                    conn.Insert(model);
                }
            }
            catch (Exception ex)
            {
                var test = 1 + 1;
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            Debug.WriteLine("OnEventSourceCreated for Listener {0} - {1} got eventSource {2}", GetHashCode(), name,
                eventSource.Name);
        }
    }

#if DEBUG
    public class DebugTraceListener : ITraceListener
    {
        public void Receive(string message)
        {
            Debug.WriteLine(message);
        }
    }
#endif
}
