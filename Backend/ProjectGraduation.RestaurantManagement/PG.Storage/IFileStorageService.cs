using PG.Storage.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Storage
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Lưu file
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task SaveFileAsync(StorageFileType type, string name, Stream content, string extension = "");

        /// <summary>
        /// Lưu file
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        //Task SaveFileAsync(StorageFileType type, string name, IFormFile content);


        /// <summary>
        /// Xoá file
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        void DeleteFileAsync(StorageFileType type, string name, string extension = "");

        /// <summary>
        /// Cập nhật file
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        Task UpdateFileAsync(StorageFileType type, string name, Stream content, string extension = "");

        /// <summary>
        /// Lấy ra file
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        byte[]? GetFileAsync(StorageFileType type, string name, string extension = "");
    }
}
