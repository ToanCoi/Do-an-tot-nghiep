using PG.Storage.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PG.Storage
{
    public class FileStorageService : BaseStorageService, IFileStorageService
    {
        public FileStorageService() { }

        /// <summary>
        /// Lưu file vào server
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task SaveFileAsync(StorageFileType type, string name, Stream content, string extension = "")
        {
            var path = this.GetPath(type, name, extension);

            this.CreateDiretoryIfNotExist(path);

            using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                content.Seek(0, SeekOrigin.Begin);
                await content.CopyToAsync(file);
            }
        }

        /// <summary>
        /// Xoá file
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public void DeleteFileAsync(StorageFileType type, string name, string extension = "")
        {
            var path = this.GetPath(type, name, extension);

            if(System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        /// <summary>
        /// Cập nhật file
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task UpdateFileAsync(StorageFileType type, string name, Stream content, string extension = "")
        {
            var path = this.GetPath(type, name, extension);

            this.DeleteFileAsync(type, name);

            await this.SaveFileAsync(type, name, content);
        }

        /// <summary>
        /// Lấy ra file
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public byte[]? GetFileAsync(StorageFileType type, string name, string extension = "")
        {
            var path = this.GetPath(type, name, extension);

            if(System.IO.File.Exists(path))
            {
                return System.IO.File.ReadAllBytes(path);
            }

            return null;
        }
    }
}
