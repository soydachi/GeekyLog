using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Net.Http;
using System.Text;
using System.Threading;
using GeekyLog.Annotations;
using GeekyLog.Base;
using GeekyLog.Interfaces;
using Newtonsoft.Json;

namespace GeekyLog.Listeners
{
    public sealed class HttpEventListener<TBaseEventInfo> : EventListener where TBaseEventInfo : BaseEventInfo
    {
        private readonly SemaphoreSlim semaphoreSlim;
        private readonly ISerializeListener serializeListener;
        private readonly HttpBaseConfiguration httpConfiguration;
        private readonly HttpClient httpClient;
        private readonly string name;

        public HttpEventListener([NotNull] string name, [NotNull] HttpBaseConfiguration httpConfiguration, [CanBeNull] JsonSerializerSettings serializerSettings = null)
        {
            serializeListener = new SerializeListener(serializerSettings);
            semaphoreSlim = new SemaphoreSlim(1);
            httpClient = new HttpClient();
            this.httpConfiguration = httpConfiguration;
            this.name = name;

            Debug.WriteLine("HttpEventListener for {0} has name {1}", GetHashCode(), name);
        }

        protected override async void OnEventWritten(EventWrittenEventArgs eventData)
        {
            if (httpConfiguration == null) return;

            await semaphoreSlim.WaitAsync();

            try
            {
                var model = serializeListener.Deserialize<TBaseEventInfo>(eventData.Payload[0].ToString());
                model.Level = eventData.Level;
                var json = serializeListener.Serialize(model);

                if (httpConfiguration.AuthenticationHeaderValue != null)
                    httpClient.DefaultRequestHeaders.Authorization = httpConfiguration.AuthenticationHeaderValue;

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                httpClient.PostAsync(new Uri($"{httpConfiguration.Url}"), content);
                
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
}
