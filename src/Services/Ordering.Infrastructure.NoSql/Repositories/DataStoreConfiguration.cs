using Utilities;

namespace Ordering.Infrastructure.NoSql.Repositories
{
    public class DataStoreConfiguration
    {
        public DataStoreConfiguration(string endPointUri, string key)
        {
            Guard.ForNullOrEmpty(endPointUri, "Cosmos URI missing");
            Guard.ForNullOrEmpty(key, "Cosmos Key missing");
            EndPointUri = endPointUri;
            Key = key;
        }

        public string EndPointUri { get; set; }
        public string Key { get; set; }
    }
}