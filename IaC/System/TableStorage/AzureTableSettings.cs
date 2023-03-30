using System;

namespace Tools.TableStorage
{
    public class AzureTableSettings
    {
        public AzureTableSettings(string storageAccount,
            string storageKeySecret,
            string tableName)
        {
            if (string.IsNullOrEmpty(storageAccount))
                throw new ArgumentNullException("StorageAccount");

            if (string.IsNullOrEmpty(storageKeySecret))
                throw new ArgumentNullException("storageKeySecret");

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("TableName");

            StorageAccount = storageAccount;
            StorageKey = storageKeySecret;
            TableName = tableName;
        }

        public string StorageAccount { get; }
        public string StorageKey { get; }
        public string TableName { get; }
    }
}