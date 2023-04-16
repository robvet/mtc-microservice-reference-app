using System;

namespace Basket.API.Infrastructure.Repository
{
    public class AzureTableSettings
    {
        public AzureTableSettings(string storageAccount,
            string storageKey,
            string tableName)
        {
            if (string.IsNullOrEmpty(storageAccount))
                throw new ArgumentNullException("StorageAccount");

            if (string.IsNullOrEmpty(storageKey))
                throw new ArgumentNullException("StorageKey");

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("TableName");

            StorageAccount = storageAccount;
            StorageKey = storageKey;
            TableName = tableName;
        }

        public string StorageAccount { get; }
        public string StorageKey { get; }
        public string TableName { get; }
    }
}