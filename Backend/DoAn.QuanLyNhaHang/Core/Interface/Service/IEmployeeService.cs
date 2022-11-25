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
    public interface IEmployeeService : IBaseService<Employee>
    {
        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        string GetNewEmployeeCode();

        /// <summary>
        /// Hàm lấy bản ghi theo filter và chia paging
        /// </summary>
        /// <param name="filterValue">Giá trị filter</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="pageNum">Số thứ tự trang</param>
        /// <returns>Danh sách bản ghi theo filter cùng thông tin phân trang</returns>
        object GetEmployeesFilter(int? pageSize = null, int? pageNum = null, string filterValue = null);

        /// <summary>
        /// Lấy nhân viên theo filter ngoại trừ 1 số nhân viên
        /// </summary>
        /// <param name="filterValue">Giá trị filter</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="pageNum">Số trang</param>
        /// <param name="listId">Danh sách Id nhân viên không lấy</param>
        /// <returns>Danh sách nhân viên</returns>
        object GetEmployeesExceptSome(IEnumerable<Guid> listId, int? pageSize = null, int? pageNum = null, string filterValue = null);

    }
}
