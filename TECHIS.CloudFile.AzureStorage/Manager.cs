﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHIS.Cloud.AzureStorage;

namespace TECHIS.CloudFile.AzureStorage
{
    public class Manager : ICloudFileManager, ICloudFileManagerFactory
    {

        private BlobManager _BlobManager = new BlobManager();

        public ICloudFileManager Connect(string containerUri, Encoding encoding = null)
        {
            _BlobManager.Connect(containerUri, encoding);
            return this;
        }

        public ICloudFileManager Connect(string azureStorageConnectionString, string containerName, Encoding encoding = null)
        {
            _BlobManager.Connect(azureStorageConnectionString, containerName, encoding);
            return this;
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
