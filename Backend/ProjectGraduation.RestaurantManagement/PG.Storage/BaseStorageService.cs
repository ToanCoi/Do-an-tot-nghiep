using PG.Storage.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace PG.Storage
{
    public class BaseStorageService
    {
        public BaseStorageService() { }

        /// <summary>
        /// Cấu hình custom từ api trỏ đến thư mục lưu file
        /// </summary>
        private const string FILE_CUSTOM_PATH = "FILE_CUSTOM_PATH";

        /// <summary>
        /// Lấy đường dẫn file
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string GetPath(StorageFileType type, string name, string extension)
        {
            string fileName = string.IsNullOrEmpty(name) ? "" : Path.GetFileName(name);
            string path = string.Empty;


            switch(type)
            {
                case StorageFileType.DishImage:
                    path = Path.Combine(this.GetRootPath(), $"DishImage/{name}{extension}");
                    break;
            }

            return path.Replace(@"\", "/");
        }

        protected string GetRootPath()
        {
            var customPath = Environment.GetEnvironmentVariable(FILE_CUSTOM_PATH);
            var basePath = AppContext.BaseDirectory;
            var rootPath = Path.Combine(basePath, customPath, "Stores");

            return rootPath;
        }


        protected void CreateDiretoryIfNotExist(string path)
        {
            var folder = Path.GetDirectoryName(path);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }
    }
}
