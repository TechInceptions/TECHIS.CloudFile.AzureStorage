using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHIS.Cloud.AzureStorage;
using TECHIS.CloudFile;
using TECHIS.Core;

namespace TECHIS.CloudFile.AzureStorage
{
    public class WriterFactory : ICloudFileWriterFactory
    {
        private readonly IDefaultAppCredentialTokenFactory _DefaultAppCredentialTokenFactory;
        ConcurrentDictionary<string, Writer> _BlobWriters = new ConcurrentDictionary<string, Writer>();

        public WriterFactory(IDefaultAppCredentialTokenFactory defaultAppCredentialTokenFactory)
        {
            _DefaultAppCredentialTokenFactory = defaultAppCredentialTokenFactory;
        }
        public ICloudFileWriter Connect(string containerUri, Encoding encoding = null)
        {

            return _BlobWriters.GetOrAdd(containerUri, (key) => new Writer(new BlobWriter().Connect(containerUri, encoding)));
        }

        public ICloudFileWriter Connect(string azureStorageConnectionString, string containerName, Encoding encoding = null)
        {
            var conKey = $"{azureStorageConnectionString}|{containerName}";

            return _BlobWriters.GetOrAdd(conKey, (key) => new Writer(new BlobWriter().Connect(azureStorageConnectionString, containerName, encoding)));

        }

        public ICloudFileWriter ConnectWithDefaultCredentials(string containerUri, Encoding encoding = null)
        {
            return _BlobWriters.GetOrAdd(containerUri, (key) => new Writer(new BlobWriter().Connect(containerUri, encoding, _DefaultAppCredentialTokenFactory.Create())));
        }

        public ICloudFileWriter ConnectWithDefaultCredentials(string storageAccountUri, string containerName, Encoding encoding = null)
        {
            var conKey = $"{storageAccountUri}|{containerName}";
            return _BlobWriters.GetOrAdd(conKey, (key) => new Writer(new BlobWriter().Connect(storageAccountUri, containerName, encoding, _DefaultAppCredentialTokenFactory.Create())));
        }
    }
}
