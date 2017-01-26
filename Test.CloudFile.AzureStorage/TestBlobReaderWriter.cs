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
            string data = (new Reader()).Connect(GetContainerUri()).ReadText(FixedFileName);

            Assert.IsFalse(string.IsNullOrEmpty(data), "Failed to read blob file");
        }

        [TestMethod]
        public void WriteText()
        {
            
            var containerUri = GetContainerUri();
            var fileName = "l1/l3";
            byte[] data = System.Text.Encoding.UTF8.GetBytes("Test data");
            (new Writer()).Connect(containerUri).WriteToBlob(data, fileName);

            //Assert.IsFalse(string.IsNullOrEmpty(data), "Failed to read blob file");
        }
        [TestMethod]
        public void ReadTextAsync()
        {
            
            var containerUri = GetContainerUri();
            var fileName = FixedFileName;
            string data = (new Reader()).Connect(containerUri).ReadTextAsync(fileName).Result;

            Assert.IsFalse(string.IsNullOrEmpty(data), "Failed to read blob file");
        }
        [TestMethod]
        public void ReadDataAsync()
        {
            Reader br = new Reader();
            var containerUri = GetContainerUri();
            var fileName = FixedFileName;
            string data = null;

            Encoding e = Encoding.UTF8;
            Task task = null;
            using (MemoryStream ms = new MemoryStream())
            {
                task = br.Connect(containerUri, e).ReadDataAsync(fileName, ms);
                task.Wait();
                data = new string(e.GetChars(ms.ToArray()));
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

        private string FixedFileName => "FixedDirectory/S2715H.inf";

        private static string GetContainerUri()
        {
            return "https://tests4dev.blob.core.windows.net/cloudfile?sv=2015-12-11&si=cloudfile-RWLD&sr=c&sig=qXnlg3DGNBrT8wWVAeeeqn8asP%2BJXjYdXH1os6mdaCU%3D";
        }

        private string StorageConnectionString => "SharedAccessSignature=sv=2015-12-11&ss=b&srt=co&sp=rwdl&st=2017-01-26T18%3A30%3A00Z&se=2017-01-27T18%3A30%3A00Z&sig=L3aGGQvavraUihwn49sCJRSHY9UJvPiknM6OpGL3GI0%3D;BlobEndpoint=https://tests4dev.blob.core.windows.net/";

    }
}
