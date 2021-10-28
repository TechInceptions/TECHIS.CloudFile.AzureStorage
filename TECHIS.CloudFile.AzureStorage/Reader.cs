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
    public class Reader : ICloudFileReader
    {
        private readonly BlobReader _BlobReader;

        public Reader(BlobReader blobReader)
        {
            _BlobReader = blobReader ?? throw new ArgumentNullException(nameof(blobReader));
        }

        public void ReadData(string fileName, Stream output)
        {
            _BlobReader.ReadData(fileName, output);
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
