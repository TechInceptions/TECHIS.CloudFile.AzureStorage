using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHIS.Cloud.AzureStorage;

namespace TECHIS.CloudFile.AzureStorage
{
    public class ManagerFactory : ICloudFileManagerFactory
    {
        ConcurrentDictionary<string, Manager> _BlobManagers = new ConcurrentDictionary<string, Manager>();

        public ICloudFileManager Connect(string containerUri, Encoding encoding = null)
        {
            var blobManager = _BlobManagers.GetOrAdd(containerUri, (key) => new Manager( new BlobManager().Connect(containerUri, encoding)));

            return blobManager;
        }

        public ICloudFileManager Connect(string azureStorageConnectionString, string containerName, Encoding encoding = null)
        {
            var conKey = $"{azureStorageConnectionString}|{containerName}";
            var blobManager = _BlobManagers.GetOrAdd(conKey, (key) => new Manager(new BlobManager().Connect(azureStorageConnectionString, containerName, encoding)));

            return blobManager;

        }

    }
}
