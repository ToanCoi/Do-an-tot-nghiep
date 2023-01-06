using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PG.Library.Service
{
    public interface ITypeService
    {
        /// <summary>
        /// Lấy tên bảng
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetTableName(Type type);

        /// <summary>
        /// Lấy các cột của bảng
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        List<string> GetTableColumns(Type type);

        /// <summary>
        /// Lấy ra key của bảng
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        PropertyInfo GetKeyField(Type type);

        /// <summary>
        /// Lấy ra key của bảng
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        List<PropertyInfo>? GetKeyFields(Type type);
    }
}
