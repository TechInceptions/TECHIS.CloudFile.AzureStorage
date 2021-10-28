using System;
using System.IO;
using System.Threading.Tasks;
using TECHIS.CloudFile.AzureStorage;
using System.Linq;
using TECHIS.CloudFile;
using Xunit;

namespace Test.Cloud.AzureStorage
{

    public class TestBlobManager
    {
        [Fact]
        public async Task TestDelete()
        {
            string path = "ForDelete";
            var list = ListFilesAsync(path);

            Assert.True(list != null && list.Length > 0, "Failed to get valid location for testing delete location");

            var initial1st = list[0];
            await ConnectedManager.DeleteAsync(initial1st);

            var new1st = ListFilesAsync(path)[0];

            Assert.False(string.Equals(initial1st, new1st), "Failed to delete item");
        }

        [Fact]
        public void TestListContainerRoot()
        {
            string path = null;
            var list = ConnectedManager.ListAsync(path).Result;

            Assert.True(list != null && list.Length > 0, "failed to list items in container");
        }

        [Fact]
        public void TestListContainerChild()
        {
            string path = "PackageRepo";
            string[] list = ConnectedManager.ListAsync(path).Result;

            Assert.True(list != null && list.Length > 0 && list.All(p=>p.Contains($"{path}/") ), "failed to list only items in child folder");
        }

        [Fact]
        public void TestListContainerSubChild()
        {
            string path = "Samples/graphics/graphics";
            var list = ConnectedManager.ListAsync(path).Result;

            Assert.True(list != null && list.Length > 0 && list.All(p => p.Contains($"{path}/")), "failed to list only items in child folder");
        }

        private string[] ListFilesAsync(string path)
        {
            return ConnectedManager.ListAsync(path).Result;
        }

        private ICloudFileManager ConnectedManager=> (new ManagerFactory()).Connect(GetContainerUri());

        
        private static string GetContainerUri()
        {
            return "https://tests4dev.blob.core.windows.net/cloudfile?sv=2015-12-11&si=cloudfile-RWLD&sr=c&sig=qXnlg3DGNBrT8wWVAeeeqn8asP%2BJXjYdXH1os6mdaCU%3D";
        }
    }
}
