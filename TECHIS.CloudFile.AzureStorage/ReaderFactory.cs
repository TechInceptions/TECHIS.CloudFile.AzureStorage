using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHIS.CloudFile;
using TECHIS.Cloud.AzureStorage;
using System.Collections.Concurrent;
using TECHIS.Core;
using Newtonsoft.Json.Bson;

namespace TECHIS.CloudFile.AzureStorage
{
    public class ReaderFactory : ICloudFileReaderFactory
    {
        private readonly IDefaultAppCredentialTokenFactory _DefaultAppCredentialTokenFactory;
        ConcurrentDictionary<string, Reader> _BlobReaders = new ConcurrentDictionary<string, Reader>();
        public ReaderFactory(IDefaultAppCredentialTokenFactory defaultAppCredentialTokenFactory)
        {
            _DefaultAppCredentialTokenFactory = defaultAppCredentialTokenFactory;
        }
        public ICloudFileReader Connect(string containerUri, Encoding encoding = null)
        {
            var blobReader = _BlobReaders.GetOrAdd(containerUri, (key) => new Reader(new BlobReader().Connect(containerUri, encoding)));

            return blobReader;
        }

        public ICloudFileReader Connect(string azureStorageConnectionString, string containerName, Encoding encoding = null)
        {
            var conKey = $"{azureStorageConnectionString}|{containerName}";
            var blobReader = _BlobReaders.GetOrAdd(conKey, (key) => new Reader(new BlobReader().Connect(azureStorageConnectionString, containerName, encoding)));

            return blobReader;

        }

        public ICloudFileReader ConnectWithDefaultCredentials(string containerUri, Encoding encoding = null)
        {
            return _BlobReaders.GetOrAdd(containerUri, (key) => new Reader(new BlobReader().Connect(containerUri, encoding, _DefaultAppCredentialTokenFactory.Create())));
        }

        public ICloudFileReader ConnectWithDefaultCredentials(string storageAccountUri, string containerName, Encoding encoding = null)
        {
            var conKey = $"{storageAccountUri}|{containerName}";
            return _BlobReaders.GetOrAdd(conKey, (key) => new Reader(new BlobReader().Connect(storageAccountUri, containerName, encoding, _DefaultAppCredentialTokenFactory.Create())));
        }
    }
}
