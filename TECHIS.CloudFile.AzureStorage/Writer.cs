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
    public class Writer : ICloudFileWriter
    {
        private BlobWriter _BlobWriter;

        public Writer(BlobWriter blobWriter)
        {
            _BlobWriter = blobWriter ?? throw new ArgumentNullException(nameof(blobWriter));
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
