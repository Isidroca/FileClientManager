using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System.IO;
using System.Threading.Tasks;

namespace FileClientManager {
    public class AzureFileShareClient : IFileClient {

        private CloudFileClient _fileClient;

        public AzureFileShareClient(string connectionString) {

            var account = CloudStorageAccount.Parse(connectionString);
            _fileClient = account.CreateCloudFileClient();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task DeleteFile(string storeName, string filePath) {

            var share = _fileClient.GetShareReference(storeName);
            var folder = share.GetRootDirectoryReference();
            var pathParts = filePath.Split('/');
            var fileName = pathParts[pathParts.Length - 1];

            for (var i = 0; i < pathParts.Length - 2; i++) {
                folder = folder.GetDirectoryReference(pathParts[i]);
                if (!await folder.ExistsAsync()) {
                    return;
                }
            }

            var fileRef = folder.GetFileReference(fileName);

            await fileRef.DeleteIfExistsAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<bool> FileExists(string storeName, string filePath) {

            var share = _fileClient.GetShareReference(storeName);
            var folder = share.GetRootDirectoryReference();
            var pathParts = filePath.Split('/');
            var fileName = pathParts[pathParts.Length - 1];

            for (var i = 0; i < pathParts.Length - 2; i++) {
                folder = folder.GetDirectoryReference(pathParts[i]);
                if (!await folder.ExistsAsync()) {
                    return await Task.FromResult(false);
                }
            }

            var fileRef = folder.GetFileReference(fileName);

            return await fileRef.ExistsAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<Stream> GetFile(string storeName, string filePath) {

            var share = _fileClient.GetShareReference(storeName);
            var folder = share.GetRootDirectoryReference();
            var pathParts = filePath.Split('/');
            var fileName = pathParts[pathParts.Length - 1];

            for (var i = 0; i < pathParts.Length - 2; i++) {
                folder = folder.GetDirectoryReference(pathParts[i]);
                if (!await folder.ExistsAsync()) {
                    return null;
                }
            }

            var fileRef = folder.GetFileReference(fileName);
            if (!await fileRef.ExistsAsync()) {
                return null;
            }

            return await fileRef.OpenReadAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<string> GetFileUrl(string storeName, string filePath) {
            return await Task.FromResult((string)null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="filePath"></param>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public async Task SaveFile(string storeName, string filePath, Stream fileStream) {

            var share = _fileClient.GetShareReference(storeName);
            var folder = share.GetRootDirectoryReference();
            var pathParts = filePath.Split('/');
            var fileName = pathParts[pathParts.Length - 1];

            for (var i = 0; i < pathParts.Length - 2; i++) {
                folder = folder.GetDirectoryReference(pathParts[i]);

                await folder.CreateIfNotExistsAsync();
            }

            var fileRef = folder.GetFileReference(fileName);

            await fileRef.UploadFromStreamAsync(fileStream);
        }
    }
}