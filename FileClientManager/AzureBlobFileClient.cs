using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;

namespace FileClientManager {
    public class AzureBlobFileClient : IFileClient {

        private CloudBlobClient _blobClient;

        public AzureBlobFileClient(string connectionString) {
            var account = CloudStorageAccount.Parse(connectionString);
            _blobClient = account.CreateCloudBlobClient();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task DeleteFile(string storeName, string filePath) {
            var container = _blobClient.GetContainerReference(storeName);
            var blob = container.GetBlockBlobReference(filePath.ToLower());

            await blob.DeleteIfExistsAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<bool> FileExists(string storeName, string filePath) {
            var container = _blobClient.GetContainerReference(storeName);
            var blob = container.GetBlockBlobReference(filePath.ToLower());

            return await blob.ExistsAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<Stream> GetFile(string storeName, string filePath) {
            var container = _blobClient.GetContainerReference(storeName);
            var blob = container.GetBlockBlobReference(filePath.ToLower());

            var mem = new MemoryStream();
            await blob.DownloadToStreamAsync(mem);
            mem.Seek(0, SeekOrigin.Begin);

            return mem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<string> GetFileUrl(string storeName, string filePath) {
            var container = _blobClient.GetContainerReference(storeName);
            var blob = container.GetBlockBlobReference(filePath.ToLower());
            string url = null;

            if (await blob.ExistsAsync()) {
                url = blob.Uri.AbsoluteUri;
            }

            return url;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="filePath"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public async Task SaveFile(string storeName, string filePath, Stream fileStream) {
            var container = _blobClient.GetContainerReference(storeName);
            var blob = container.GetBlockBlobReference(filePath.ToLower());

            await blob.UploadFromStreamAsync(fileStream);
        }
    }
}
