using SQLite.Net.Interop;

namespace GeekyLog.Base
{
    public class SqliteBaseConfiguration
    {
        public ISQLitePlatform SQLitePlatform { get; set; }
        public string Path { get; set; }
    }
}
