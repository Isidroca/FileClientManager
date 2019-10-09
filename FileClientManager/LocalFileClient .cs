using System;
using System.IO;
using System.Threading.Tasks;

namespace FileClientManager {
    public class LocalFileClient : IFileClient {

        private string _fileRoot;

        public LocalFileClient(string fileRoot) {
            _fileRoot = fileRoot;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task DeleteFile(string storeName, string filePath) {

            var path = Path.Combine(_fileRoot, storeName, filePath);

            var task = Task.Run(() => deleteFile(path));
            await task;
        }

        private void deleteFile(string path) {
            if (File.Exists(path)) {
                File.Delete(path);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public Task<bool> FileExists(string storeName, string filePath) {
            var path = Path.Combine(_fileRoot, storeName, filePath);

            return Task.FromResult(File.Exists(path));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<Stream> GetFile(string storeName, string filePath) {
            var path = Path.Combine(_fileRoot, storeName, filePath);
            Stream stream = null;

            if (File.Exists(path)) {
                stream = File.OpenRead(path);
            }

            return await Task.FromResult(stream);
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

            var path = Path.Combine(_fileRoot, storeName, filePath);

            if (File.Exists(path)) {
                File.Delete(path);
            }

            using (var file = new FileStream(path, FileMode.CreateNew)) {
                await fileStream.CopyToAsync(file);
            }
        }
    }
}