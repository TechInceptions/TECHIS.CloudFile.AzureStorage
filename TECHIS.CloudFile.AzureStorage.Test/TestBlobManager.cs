using System;
using System.IO;
using System.Threading.Tasks;
using TECHIS.CloudFile.AzureStorage;
using System.Linq;
using TECHIS.CloudFile;
using Xunit;
using TECHIS.Core;
using Moq;
using Microsoft.Extensions.Logging;

namespace Test.Cloud.AzureStorage
{

    public class TestBlobManager
    {
        [Fact]
        public async Task TestDelete()
        {
            string path = "ForDelete";
            var list = await ListFilesAsync(path);

            Assert.True(list != null && list.Length > 0, "Failed to get valid location for testing delete location");

            var initial1st = list[0];
            await GetConnectedManager().DeleteAsync(initial1st);

            var new1st = (await ListFilesAsync(path))[0];

            Assert.False(string.Equals(initial1st, new1st), "Failed to delete item");
        }

        [Fact]
        public void TestListContainerRoot()
        {
            string path = null;
            var list = GetConnectedManager().List(path);

            Assert.True(list != null && list.Length > 0, "failed to list items in container");
        }

        [Fact]
        public async Task TestListContainerChild()
        {
            string path = "PackageRepo";
            string[] list = await GetConnectedManager("cloudfile").ListAsync(path);

            Assert.True(list != null && list.Length > 0 && list.All(p=>p.Contains($"{path}/") ), "failed to list only items in child folder");
        }

        [Fact]
        public void TestListContainerSubChild()
        {
            string path = "Samples/graphics/graphics";
            var list = GetConnectedManager().ListAsync(path).Result;

            Assert.True(list != null && list.Length > 0 && list.All(p => p.Contains($"{path}/")), "failed to list only items in child folder");
        }

        private async Task<string[]> ListFilesAsync(string path)
        {
            return await GetConnectedManager().ListAsync(path);
        }


        [Fact]
        public async Task TestDelete2()
        {
            string path = "ForDelete";
            var list = await ListFilesAsync2(path);

            Assert.True(list != null && list.Length > 0, "Failed to get valid location for testing delete location");

            var initial1st = list[0];
            await GetConnectedManager(GetDefaultAppCredentialTokenFactory()).DeleteAsync(initial1st);

            var new1st = (await ListFilesAsync2(path))[0];

            Assert.False(string.Equals(initial1st, new1st), "Failed to delete item");
        }

        [Fact]
        public void TestListContainerRoot2()
        {
            string path = null;
            var list =  GetConnectedManager(GetDefaultAppCredentialTokenFactory()).List(path);

            Assert.True(list != null && list.Length > 0, "failed to list items in container");
        }

        [Fact]
        public async Task TestListContainerChild2()
        {
            string path = "PackageRepo";
            string[] list = await GetConnectedManager(GetDefaultAppCredentialTokenFactory()).ListAsync(path);

            Assert.True(list != null && list.Length > 0 && list.All(p => p.Contains($"{path}/")), "failed to list only items in child folder");
        }

        [Fact]
        public void TestListContainerSubChild2()
        {
            string path = "Samples/graphics/graphics";
            var list = GetConnectedManager(GetDefaultAppCredentialTokenFactory()).ListAsync(path).Result;

            Assert.True(list != null && list.Length > 0 && list.All(p => p.Contains($"{path}/")), "failed to list only items in child folder");
        }
        [Fact]
        public async Task TestListContainerSubChild3()
        {
            string path = "Samples/graphics/graphics";
            var list = await GetConnectedManager(GetDefaultAppCredentialTokenFactory(), "cloudfile").ListAsync(path);

            Assert.True(list != null && list.Length > 0 && list.All(p => p.Contains($"{path}/")), "failed to list only items in child folder");
        }

        private async Task<string[]> ListFilesAsync2(string path)
        {
            return await GetConnectedManager(GetDefaultAppCredentialTokenFactory()).ListAsync(path);
        }

        private static ILogger<DefaultAppCredentialTokenFactory> _Logger;
        private static IDefaultAppCredentialTokenFactory GetDefaultAppCredentialTokenFactory()
        {
            var logger = _Logger ??= Connector.GetLogger<DefaultAppCredentialTokenFactory>();

            return new DefaultAppCredentialTokenFactory(Connector.GetApplicationSettings(), logger);
        }

        private ICloudFileManager GetConnectedManager(string container) => (new ManagerFactory(null)).Connect(Connector.StorageConnectionString,container);
        private ICloudFileManager GetConnectedManager() => (new ManagerFactory(null)).Connect(Connector.GetContainerUri());

        private ICloudFileManager GetConnectedManager(IDefaultAppCredentialTokenFactory defaultAppCredentialTokenFactory) => 
            (new ManagerFactory(defaultAppCredentialTokenFactory)).ConnectWithDefaultCredentials(Connector.ADControlledContainerUrl);
        private ICloudFileManager GetConnectedManager(IDefaultAppCredentialTokenFactory defaultAppCredentialTokenFactory, string container) =>
            (new ManagerFactory(defaultAppCredentialTokenFactory)).ConnectWithDefaultCredentials(Connector.ADControlledStorage,container);


    }
}
