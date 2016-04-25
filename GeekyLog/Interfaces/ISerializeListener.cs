using System.Globalization;
using Newtonsoft.Json;

namespace GeekyLog.Interfaces
{
    public interface ISerializeListener
    {
        T Deserialize<T>(string message);
        string Serialize<T>(T message);
    }

    public class SerializeListener : ISerializeListener
    {
        private readonly JsonSerializerSettings internalSerializationSettings;

        public SerializeListener(JsonSerializerSettings serializationSettings = null)
        {
            internalSerializationSettings = new JsonSerializerSettings()
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                Culture = CultureInfo.InvariantCulture,
                DateFormatString = "0:yyyy-MM-dd HH\\:mm\\:ss\\:ffff"
            };

            if (serializationSettings != null)
                this.internalSerializationSettings = serializationSettings;
        }

        public string Serialize<T>(T message)
        {
            return JsonConvert.SerializeObject(message, internalSerializationSettings);
        }

        public T Deserialize<T>(string message)
        {
            return JsonConvert.DeserializeObject<T>(message, internalSerializationSettings);
        }
    }
}
