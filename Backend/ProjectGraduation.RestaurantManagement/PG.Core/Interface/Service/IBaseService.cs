using PG.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PG.Core.Interface.Service
{
    public interface IBaseService<TEntity>
    {
        /// <summary>
        /// Lấy toàn bộ bản ghi
        /// </summary>
        /// <returns>List bản ghi lấy được</returns>
        IEnumerable<TEntity> GetEntities();

        /// <summary>
        /// Lấy dữ liệu paging
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        object GetPagingData(PagingParam param);

        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của đối tượng cần lấy</param>
        /// <returns>Một bản ghi lấy được theo Id</returns>
        TEntity GetEntityById(Guid Id);

        /// <summary>
        /// Lấy bản ghi theo property - dùng cho backend để validate theo FormMode
        /// </summary>
        /// <param name="entity">Object cần kiểm tra</param>
        /// <param name="propName">Tên của trường cần kiểm tra</param>
        /// <returns>Một bản ghi có property với value truyền vào</returns>
        TEntity GetEntityByProperty(TEntity entity, string propName);

        /// <summary>
        /// Hàm lấy bản ghi theo property - cho frontend dùng
        /// </summary>
        /// <param name="propName">Tên trường cần kiểm tra</param>
        /// <param name="value">Giá trị của thuộc tính</param>
        /// <returns>Một bản ghi có property và value truyền vào</returns>
        TEntity GetEntityByProperty(string propName, string propValue);

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm mới</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        ServiceResult InsertEntity(TEntity entity);

        /// <summary>
        /// Sửa thông tin một bản ghi
        /// </summary>
        /// <param name="Id">Id của bản ghi cần sửa</param>
        /// <param name="entity">Đối tượng có những thông tin cần sửa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        ServiceResult UpdateEntity(Guid Id, TEntity entity);

        /// <summary>
        /// Xóa một bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của bản ghi cần xóa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        ServiceResult DeleteEntity(Guid Id);

        /// <summary>
        /// Xóa nhiều bản ghi theo Id
        /// </summary>
        /// <param name="listId">Danh sách id bản ghi cần xóa</param>
        /// <returns>Số bản ghi xóa được</returns>
        ServiceResult MultipleDelete(IEnumerable<Guid> listId);

        /// <summary>
        /// Kiểm tra xem có sự xung đột dữ liệu khi xoá hay không
        /// </summary>
        /// <param name="Id">Id bản ghi</param>
        /// <returns>Có xung đột hay không</returns>
        ServiceResult CheckConflictData(Guid Id);
    }
}
