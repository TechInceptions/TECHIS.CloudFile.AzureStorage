using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TECHIS.CloudFile.AzureStorage;

namespace Test.Cloud.AzureStorage
{
    [TestClass]
    public class TestBlobReaderWriter
    {
        [TestMethod]
        public void ReadText()
        {
            var fileName        = "l1/l2";
            string data = (new Reader()).Connect(GetContainerUri()).ReadText(fileName);

            Assert.IsFalse(string.IsNullOrEmpty(data), "Failed to read blob file");
        }

        [TestMethod]
        public void WriteText()
        {
            Writer br = new Writer();
            var containerUri = GetContainerUri();
            var fileName = "l1/l3";
            byte[] data = System.Text.Encoding.UTF8.GetBytes("Test data");
            br.Connect(containerUri).WriteToBlob(data, fileName);

            //Assert.IsFalse(string.IsNullOrEmpty(data), "Failed to read blob file");
        }
        [TestMethod]
        public void ReadTextAsync()
        {
            Reader br = new Reader();
            var containerUri = GetContainerUri();
            var fileName = "l1/l2";
            string data = br.Connect(containerUri).ReadTextAsync(fileName).Result;

            Assert.IsFalse(string.IsNullOrEmpty(data), "Failed to read blob file");
        }
        [TestMethod]
        public void ReadDataAsync()
        {
            Reader br = new Reader();
            var containerUri = GetContainerUri();
            var fileName = "l1/l2";
            string data = null;

            Task task = null;
            using (MemoryStream ms = new MemoryStream() )
            {
                var e = Encoding.UTF8;
                task = br.Connect(containerUri,e).ReadDataAsync(fileName, ms);
                task.Wait();
                data = new string( e.GetChars(ms.ToArray()));
            }
            Assert.IsFalse(string.IsNullOrEmpty(data), "Failed to read blob file");
        }

        [TestMethod]
        public void WriteTextAsync()
        {
            Writer br = new Writer();
            string containerUri = GetContainerUri();
            var fileName = "l1/l3";
            byte[] data = System.Text.Encoding.UTF8.GetBytes($"Test data: {DateTime.UtcNow.ToLongTimeString()}");
            var task = br.Connect(containerUri).WriteToBlobAsync(data, fileName);
            task.Wait();
            Assert.IsTrue(task.IsCompleted, "Failed to Write Async blob file");
        }
        [TestMethod]
        public void WriteTextAsync2()
        {
            Writer br = new Writer();
            var fileName = "l1/l3";
            byte[] data = System.Text.Encoding.UTF8.GetBytes($"Test data: {DateTime.UtcNow.ToLongTimeString()}");
            var task = br.Connect(StorageConnectionString, "test").WriteToBlobAsync(data, fileName);
            task.Wait();
            Assert.IsTrue(task.IsCompleted, "Failed to Write Async blob file");
        }

        private static string GetContainerUri()
        {
            return "https://store4.blob.core.windows.net/test?sv=2015-12-11&si=blobRW&sr=c&sig=MdWR5Pal76ybGbWpqm5pSuNdtTgijU6gODXt17%2Bv3sg%3D";
        }

        private string StorageConnectionString => "SharedAccessSignature=sv=2015-12-11&ss=bfqt&srt=sco&sp=rwdl&st=2017-01-17T13%3A31%3A00Z&se=2017-01-21T13%3A31%3A00Z&sig=ms6DI%2BTf7ywQwXHnuuc5CZPZahaJSHNSExLmpqTedLE%3D;BlobEndpoint=https://store4.blob.core.windows.net/;FileEndpoint=https://store4.file.core.windows.net/;QueueEndpoint=https://store4.queue.core.windows.net/;TableEndpoint=https://store4.table.core.windows.net/";
        
    }
}
