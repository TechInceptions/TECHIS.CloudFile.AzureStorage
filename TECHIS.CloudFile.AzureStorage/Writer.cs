using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHIS.Cloud.AzureStorage;
using TECHIS.CloudFile;
namespace TECHIS.CloudFile.AzureStorage
{
    public class Writer : ICloudFileWriterFactory, ICloudFileWriter
    {
        private BlobWriter _BlobWriter = new BlobWriter();
        public ICloudFileWriter Connect(string containerUri, Encoding encoding = null)
        {
            _BlobWriter.Connect(containerUri, encoding);
            return this;
        }

        public ICloudFileWriter Connect(string azureStorageConnectionString, string containerName, Encoding encoding = null)
        {
            _BlobWriter.Connect(azureStorageConnectionString, containerName, encoding);
            return this;
        }

        public void WriteToBlob(byte[] data, string fileName)
        {
            _BlobWriter.WriteToBlob(data, fileName);
        }

        public void WriteToBlob(Stream ms, string fileName)
        {
            _BlobWriter.WriteToBlob(ms, fileName);
        }

        public Task WriteToBlobAsync(byte[] data, string fileName)
        {
            return _BlobWriter.WriteToBlobAsync(data, fileName);
        }

        public Task WriteToBlobAsync(Stream ms, string fileName)
        {
            return _BlobWriter.WriteToBlobAsync(ms, fileName);
        }
    }
}
