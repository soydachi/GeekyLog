using System.Net.Http.Headers;

namespace GeekyLog.Base
{
    public class HttpBaseConfiguration
    {
        public AuthenticationHeaderValue AuthenticationHeaderValue { get; set; }
        public string Url { get; set; }
    }
}