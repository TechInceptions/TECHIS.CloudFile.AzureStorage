using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHIS.CloudFile;
using TECHIS.Cloud.AzureStorage;
namespace TECHIS.CloudFile.AzureStorage
{
    public class Reader : ICloudFileReader, ICloudFileReaderFactory
    {
        private BlobReader _BlobReader = new BlobReader();

        public ICloudFileReader Connect(string containerUri, Encoding encoding = null)
        {
            _BlobReader.Connect(containerUri, encoding);
            return this;
        }

        public ICloudFileReader Connect(string azureStorageConnectionString, string containerName, Encoding encoding = null)
        {
            _BlobReader.Connect(azureStorageConnectionString, containerName, encoding);
            return this;
        }

        public Task ReadDataAsync(string fileName, Stream output)
        {
            return _BlobReader.ReadDataAsync(fileName, output);
        }

        public string ReadText(string fileName)
        {
            return _BlobReader.ReadText(fileName);
        }

        public Task<string> ReadTextAsync(string fileName)
        {
            return _BlobReader.ReadTextAsync(fileName);
        }
    }
}
