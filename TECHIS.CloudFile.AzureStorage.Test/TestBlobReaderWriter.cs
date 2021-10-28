using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using TECHIS.CloudFile.AzureStorage;
using Xunit;

namespace Test.Cloud.AzureStorage
{

    public class TestBlobReaderWriter
    {
        [Fact]
        public void ReadText()
        {
            string data = (new ReaderFactory()).Connect(GetContainerUri()).ReadText(FixedFileName);

            Assert.False(string.IsNullOrEmpty(data), "Failed to read blob file");
        }

        [Fact]
        public void WriteText()
        {
            
            var containerUri = GetContainerUri();
            var fileName = "l1/l3";
            byte[] data = System.Text.Encoding.UTF8.GetBytes("Test data");
            (new WriterFactory()).Connect(containerUri).WriteToBlob(data, fileName);

            //Assert.False(string.IsNullOrEmpty(data), "Failed to read blob file");
        }
        [Fact]
        public void ReadTextAsync()
        {
            
            var containerUri = GetContainerUri();
            var fileName = FixedFileName;
            string data = (new ReaderFactory()).Connect(containerUri).ReadTextAsync(fileName).Result;

            Assert.False(string.IsNullOrEmpty(data), "Failed to read blob file");
        }
        [Fact]
        public async Task ReadDataAsync()
        {
            var containerUri = GetContainerUri();
            Encoding e = Encoding.UTF8;
            var br = new ReaderFactory().Connect(containerUri, e);
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
            var containerUri = GetContainerUri();
            Encoding e = Encoding.UTF8;
            var br = new ReaderFactory().Connect(containerUri, e);
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
            string containerUri = GetContainerUri();
            var br = new WriterFactory().Connect(containerUri);
            var fileName = "l1/l3";
            byte[] data = System.Text.Encoding.UTF8.GetBytes($"Test data: {DateTime.UtcNow.ToLongTimeString()}");
            var task = br.WriteToBlobAsync(data, fileName);
            await task;
            Assert.True(task.IsCompleted, "Failed to Write Async blob file");
        }
        [Fact]
        public async Task WriteTextAsync2()
        {
            var br = new WriterFactory().Connect(StorageConnectionString, "test");
            var fileName = "l1/l3";
            byte[] data = System.Text.Encoding.UTF8.GetBytes($"Test data: {DateTime.UtcNow.ToLongTimeString()}");
            var task = br.WriteToBlobAsync(data, fileName);
            await task;
            Assert.True(task.IsCompleted, "Failed to Write Async blob file");
        }

        private string FixedFileName => "FixedDirectory/S2715H.inf";

        private static string GetContainerUri()
        {
            return "https://tests4dev.blob.core.windows.net/cloudfile?sv=2015-12-11&si=cloudfile-RWLD&sr=c&sig=qXnlg3DGNBrT8wWVAeeeqn8asP%2BJXjYdXH1os6mdaCU%3D";
        }

        //private string StorageConnectionString => "SharedAccessSignature=sv=2016-05-31&ss=b&srt=co&sp=rwdl&st=2017-05-05T17%3A36%3A00Z&se=2018-05-06T17%3A36%3A00Z&sig=4qZAoOGRiuwK9THxy%2BTH4tLznQt%2FZIRESPvg%2Bc8qsTA%3D;BlobEndpoint=https://tests4dev.blob.core.windows.net/";
          private string StorageConnectionString => "SharedAccessSignature=sv=2020-04-08&ss=btqf&srt=sco&st=2021-10-26T11%3A48%3A00Z&se=2040-10-29T11%3A48%3A00Z&sp=rwdxftlacup&sig=d02Dv2Xg03j8dZT1oX0gt6FmmF5jk88TXjOIbqNLgzA%3D;BlobEndpoint=https://tests4dev.blob.core.windows.net/;FileEndpoint=https://tests4dev.file.core.windows.net/;QueueEndpoint=https://tests4dev.queue.core.windows.net/;TableEndpoint=https://tests4dev.table.core.windows.net/;";

    }
}
