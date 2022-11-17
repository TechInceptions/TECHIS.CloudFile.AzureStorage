using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TECHIS.CloudFile;
using TECHIS.CloudFile.AzureStorage;
using TECHIS.Core;
using Xunit;

namespace Test.Cloud.AzureStorage
{

    public class TestBlobReaderWriter
    {
        [Fact]
        public void ReadText()
        {
            string data = (new ReaderFactory(null)).Connect(Connector.GetContainerUri()).ReadText(FixedFileName);

            Assert.False(string.IsNullOrEmpty(data), "Failed to read blob file");
        }

        [Fact]
        public void WriteText()
        {
            
            var containerUri = Connector.GetContainerUri();
            var fileName = "l1/l3";
            byte[] data = System.Text.Encoding.UTF8.GetBytes("Test data");
            (new WriterFactory(null)).Connect(containerUri).WriteToBlob(data, fileName);

            //Assert.False(string.IsNullOrEmpty(data), "Failed to read blob file");
        }
        [Fact]
        public void ReadTextAsync()
        {
            
            var containerUri = Connector.GetContainerUri();
            var fileName = FixedFileName;
            string data = (new ReaderFactory(null)).Connect(containerUri).ReadTextAsync(fileName).Result;

            Assert.False(string.IsNullOrEmpty(data), "Failed to read blob file");
        }
        [Fact]
        public async Task ReadDataAsync()
        {
            var containerUri = Connector.GetContainerUri();
            Encoding e = Encoding.UTF8;
            var br = new ReaderFactory(null).Connect(containerUri, e);
            var fileName = FixedFileName;
            string data = null;

            using (MemoryStream ms = new MemoryStream())
            {
                await br.ReadDataAsync(fileName, ms);
                
                data = new string(e.GetChars(ms.ToArray()));
            }
            Assert.False(string.IsNullOrEmpty(data), $"Failed to read blob file via {nameof(ReadDataAsync)}");
        }
        [Fact]
        public void ReadData()
        {
            var containerUri = Connector.GetContainerUri();
            Encoding e = Encoding.UTF8;
            var br = new ReaderFactory(null).Connect(containerUri, e);
            var fileName = FixedFileName;
            string data = null;

            using (MemoryStream ms = new MemoryStream())
            {
                br.ReadData(fileName, ms);
                data = new string(e.GetChars(ms.ToArray()));
            }
            Assert.False(string.IsNullOrEmpty(data), $"Failed to read blob file via {nameof(ReadData)}");
        }

        [Fact]
        public async Task WriteTextAsync()
        {
            var br = new WriterFactory(null).Connect(Connector.GetContainerUri());
            var fileName = "l1/l3";
            byte[] data = System.Text.Encoding.UTF8.GetBytes($"Test data: {DateTime.UtcNow.ToLongTimeString()}");
            var task = br.WriteToBlobAsync(data, fileName);
            await task;
            Assert.True(task.IsCompleted, "Failed to Write Async blob file");
        }
        [Fact]
        public async Task WriteTextToContainerAsync()
        {
            var br = new WriterFactory(null).Connect(Connector.StorageConnectionString, "test");
            var fileName = "l1/l3";
            byte[] data = System.Text.Encoding.UTF8.GetBytes($"Test data: {DateTime.UtcNow.ToLongTimeString()}");
            var task = br.WriteToBlobAsync(data, fileName);
            await task;
            Assert.True(task.IsCompleted, "Failed to Write Async blob file");
        }

        #region With Factory 

        [Fact]
        public void ReadText2()
        {
            string data = GetConnectedReader(GetDefaultAppCredentialTokenFactory()).ReadText(FixedFileName);

            Assert.False(string.IsNullOrEmpty(data), "Failed to read blob file");
        }

        [Fact]
        public void WriteText2()
        {

            var fileName = "l1/l3";
            byte[] data = System.Text.Encoding.UTF8.GetBytes("Test data");
            GetConnectedWriter(GetDefaultAppCredentialTokenFactory()).WriteToBlob(data, fileName);

            //Assert.False(string.IsNullOrEmpty(data), "Failed to read blob file");
        }
        [Fact]
        public void ReadTextAsync2()
        {
            var fileName = FixedFileName;
            string data = GetConnectedReader(GetDefaultAppCredentialTokenFactory()).ReadTextAsync(fileName).Result;

            Assert.False(string.IsNullOrEmpty(data), "Failed to read blob file");
        }
        [Fact]
        public async Task ReadDataAsync2()
        {
            var br = GetConnectedReader(GetDefaultAppCredentialTokenFactory(), Encoding.UTF8);
            var fileName = FixedFileName;
            string data = null;

            using (MemoryStream ms = new MemoryStream())
            {
                await br.ReadDataAsync(fileName, ms);

                data = new string(Encoding.UTF8.GetChars(ms.ToArray()));
            }
            Assert.False(string.IsNullOrEmpty(data), $"Failed to read blob file via {nameof(ReadDataAsync)}");
        }
        [Fact]
        public void ReadData2()
        {
            var br = GetConnectedReader(GetDefaultAppCredentialTokenFactory(), "cloudfile", Encoding.UTF8);
            var fileName = FixedFileName;
            string data = null;

            using (MemoryStream ms = new MemoryStream())
            {
                br.ReadData(fileName, ms);
                data = new string(Encoding.UTF8.GetChars(ms.ToArray()));
            }
            Assert.False(string.IsNullOrEmpty(data), $"Failed to read blob file via {nameof(ReadData)}");
        }

        [Fact]
        public async Task WriteTextAsync2()
        {
            var br = GetConnectedWriter(GetDefaultAppCredentialTokenFactory());
            var fileName = "l1/l3";
            byte[] data = System.Text.Encoding.UTF8.GetBytes($"Test data: {DateTime.UtcNow.ToLongTimeString()}");
            var task = br.WriteToBlobAsync(data, fileName);
            await task;
            Assert.True(task.IsCompleted, "Failed to Write Async blob file");
        }
        [Fact]
        public async Task WriteTextToContainerAsync2()
        {
            var br = GetConnectedWriter(GetDefaultAppCredentialTokenFactory(), "test");
            var fileName = "l1/l3";
            byte[] data = System.Text.Encoding.UTF8.GetBytes($"Test data: {DateTime.UtcNow.ToLongTimeString()}");
            var task = br.WriteToBlobAsync(data, fileName);
            await task;
            Assert.True(task.IsCompleted, "Failed to Write Async blob file");
        }
        #endregion

        private string FixedFileName => "FixedDirectory/S2715H.inf";

        private static ILogger<DefaultAppCredentialTokenFactory> _Logger;
        private static IDefaultAppCredentialTokenFactory GetDefaultAppCredentialTokenFactory()
        {
            var logger = _Logger ??= Connector.GetLogger<DefaultAppCredentialTokenFactory>();

            IDefaultAppCredentialTokenFactory defaultAppCredentialTokenFactory = new DefaultAppCredentialTokenFactory(Connector.GetApplicationSettings(), logger);
            return defaultAppCredentialTokenFactory;
        }

        //private ICloudFileReader GetConnectedReader(Encoding encoding) => (new ReaderFactory(null)).Connect(Connector.GetContainerUri(),encoding);
        //private ICloudFileReader GetConnectedReader() => (new ReaderFactory(null)).Connect(Connector.GetContainerUri());
        //private ICloudFileWriter GetConnectedWriter() => (new WriterFactory(null)).Connect(Connector.GetContainerUri());
        //private ICloudFileWriter GetConnectedWriter(Encoding encoding) => (new WriterFactory(null)).Connect(Connector.GetContainerUri(), encoding);

        private ICloudFileReader GetConnectedReader(IDefaultAppCredentialTokenFactory defaultAppCredentialTokenFactory, string container, Encoding encoding) =>
            (new ReaderFactory(defaultAppCredentialTokenFactory)).ConnectWithDefaultCredentials(Connector.ADControlledStorage, container, encoding);
        private ICloudFileReader GetConnectedReader(IDefaultAppCredentialTokenFactory defaultAppCredentialTokenFactory, Encoding encoding) => 
            (new ReaderFactory(defaultAppCredentialTokenFactory)).ConnectWithDefaultCredentials(Connector.ADControlledContainerUrl, encoding);

        private ICloudFileWriter GetConnectedWriter(IDefaultAppCredentialTokenFactory defaultAppCredentialTokenFactory, string container) => 
            (new WriterFactory(defaultAppCredentialTokenFactory)).ConnectWithDefaultCredentials(Connector.ADControlledStorage, container);

        private ICloudFileWriter GetConnectedWriter(IDefaultAppCredentialTokenFactory defaultAppCredentialTokenFactory) => 
            (new WriterFactory(defaultAppCredentialTokenFactory)).ConnectWithDefaultCredentials(Connector.ADControlledContainerUrl);

        private ICloudFileReader GetConnectedReader(IDefaultAppCredentialTokenFactory defaultAppCredentialTokenFactory) => 
            (new ReaderFactory(defaultAppCredentialTokenFactory)).ConnectWithDefaultCredentials(Connector.ADControlledContainerUrl);

    }
}
