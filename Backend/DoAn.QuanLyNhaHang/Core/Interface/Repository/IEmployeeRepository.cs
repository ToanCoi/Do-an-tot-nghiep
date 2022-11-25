using PG.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Interface.Repository
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        /// <summary>
        /// Lấy mã nhân viên lớn nhất trong db
        /// </summary>
        /// <returns>Mã nhân lớn nhất</returns>
        string GetMaxEmployeeCode();

        /// <summary>
        /// Lấy danh sách nhân viên theo mã hoặc tên
        /// </summary>
        /// <param name="filterValue">Giá trị filter</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="pageNum">Số trang</param>
        /// <returns>Danh sách nhân viên theo giá trị truyền vào</returns>
        IEnumerable<Employee> GetEmployeesFilter(int? pageSize = null, int? pageNum = null, string filterValue = null);

        /// <summary>
        /// Lấy tổng số bản ghi theo filter
        /// </summary>
        /// <returns>Giá trị filter</returns>
        string GetTotalRecord(string filterValue);

        /// <summary>
        /// Lấy bảng để mapping cột với dữ liệu khi export
        /// </summary>
        /// <returns>Thông tin các cột export</returns>
        public IEnumerable<EmployeeExportColumn> GetEmployeeExportColumns();

        /// <summary>
        /// Lấy nhân viên theo filter ngoại trừ 1 số nhân viên
        /// </summary>
        /// <param name="filterValue">Giá trị filter</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="pageNum">Số trang</param>
        /// <param name="listId">Danh sách Id nhân viên không lấy</param>
        /// <returns>Danh sách nhân viên</returns>
        IEnumerable<Employee> GetEmployeesExceptSome(IEnumerable<Guid> listId, int? pageSize = null, int? pageNum = null, string filterValue = null);
    }
}
