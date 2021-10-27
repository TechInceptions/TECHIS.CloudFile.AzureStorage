using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHIS.Cloud.AzureStorage;

namespace TECHIS.CloudFile.AzureStorage
{
    public class Manager : ICloudFileManager
    {

        private readonly BlobManager _BlobManager;

        public Manager(BlobManager blobManager)
        {
            _BlobManager = blobManager ?? throw new ArgumentNullException(nameof(blobManager));
        }

        public void Delete(string fileName)
        {
            _BlobManager.Delete(fileName);
        }

        public async Task DeleteAsync(string fileName)
        {
            await _BlobManager.DeleteAsync(fileName);
        }

        public string[] List(string containerPath)
        {
            return _BlobManager.List(containerPath);
        }

        public Task<string[]> ListAsync(string containerPath)
        {
            return _BlobManager.ListAsync(containerPath);
        }
    }
}
