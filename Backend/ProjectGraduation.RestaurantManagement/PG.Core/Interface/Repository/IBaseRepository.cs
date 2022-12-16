using PG.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Interface.Repository
{
    public interface IBaseRepository<TEntity>
    {
        /// <summary>
        /// Lấy toàn bộ bản ghi
        /// </summary>
        /// <returns>List bản ghi lấy được</returns>
        IEnumerable<TEntity> GetEntities();

        /// <summary>
        /// Lấy bản ghi theo filter
        /// </summary>
        /// <returns>List bản ghi lấy được</returns>
        IEnumerable<TEntity> GetEntitiesFilter(PagingParam pagingParam);

        /// <summary>
        /// Lấy tổng số bản ghi theo filter
        /// </summary>
        /// <returns></returns>
        int GetTotalFilters(PagingParam pagingParam);

        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của đối tượng cần lấy</param>
        /// <returns>Một bản ghi lấy được theo Id</returns>
        TEntity GetEntityById(Guid Id);

        /// <summary>
        /// Lấy bản ghi theo property - dùng cho backend validate theo FormMode
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
        object InsertEntity(TEntity entity);

        /// <summary>
        /// Sửa thông tin một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng có những thông tin cần sửa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        int UpdateEntity(TEntity entity);

        /// <summary>
        /// Xóa một bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của bản ghi cần xóa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        int DeleteEntity(Guid Id);

        /// <summary>
        /// Xóa nhiều bản ghi theo Id
        /// </summary>
        /// <param name="listId">Danh sách id bản ghi cần xóa</param>
        /// <returns>Số bản ghi xóa được</returns>
        int MultipleDelete(IEnumerable<Guid> listId);

        /// <summary>
        /// Kiểm tra xem bản ghi có tồn tại không
        /// </summary>
        /// <param name="Id">Id bản ghi</param>
        /// <returns>Bản ghi có tồn tại hay không</returns>
        int CheckExist(Guid Id);

        /// <summary>
        /// Kiểm tra xem có sự xung đột dữ liệu khi xoá hay không
        /// </summary>
        /// <param name="Id">Id bản ghi</param>
        /// <returns>Có xung đột hay không</returns>
        bool CheckConflictData(Guid Id);
    }
}
