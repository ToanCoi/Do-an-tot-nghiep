using Dapper;
using Microsoft.Extensions.Configuration;
using PG.Core.Entities;
using PG.Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PG.Infrastructure.Repository;

namespace PG.Infrastructure.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        #region Constructor
        public EmployeeRepository(IConfiguration configuration, IServiceProvider serviceProvider) : base(configuration, serviceProvider)
        {

        }

        #endregion

        #region Method
        /// <summary>
        /// Lấy mã nhân viên lớn nhất trong db
        /// </summary>
        /// <returns>Mã nhân lớn nhất</returns>
        public string GetMaxEmployeeCode()
        {
            //Lấy mã nhân viên lớn nhất
            var maxEmployeeCode = _dbConnection.QueryFirstOrDefault<string>("Proc_GetMaxEmployeeCode", commandType: CommandType.StoredProcedure);

            return maxEmployeeCode.ToString();
        }

        /// <summary>
        /// Lấy danh sách nhân viên theo mã hoặc tên
        /// </summary>
        /// <param name="filterValue">Giá trị filter</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="pageNum">Số trang</param>
        /// <returns>Danh sách nhân viên theo giá trị truyền vào</returns>
        public IEnumerable<Employee> GetEmployeesFilter(int? pageSize = null, int? pageNum = null, string filterValue = null)
        {
            DynamicParameters param = new DynamicParameters();

            param.Add("@FilterValue", filterValue);
            param.Add("@PageSize", pageSize);
            param.Add("@PageNum", pageNum);

            var employees = _dbConnection.Query<Employee>("Proc_GetEmployeesFilter", param, commandType: CommandType.StoredProcedure);

            return employees;
        }

        /// <summary>
        /// Lấy tổng số bản ghi theo filter
        /// </summary>
        /// <returns>Giá trị filter</returns>
        public string GetTotalRecord(string filterValue)
        {
            var param = new DynamicParameters();
            param.Add("@FilterValue", filterValue);

            var totalRecord = _dbConnection.QueryFirstOrDefault<string>("Proc_GetTotalEmployeesFilter", param, commandType: CommandType.StoredProcedure);

            return totalRecord;
        }

        /// <summary>
        /// Lấy bảng để mapping cột với dữ liệu khi export
        /// </summary>
        /// <returns>Thông tin các cột export</returns>
        public IEnumerable<EmployeeExportColumn> GetEmployeeExportColumns()
        {
            return _dbConnection.Query<EmployeeExportColumn>("Proc_GetEmployeeExportColumn", commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Lấy nhân viên theo filter ngoại trừ 1 số nhân viên
        /// </summary>
        /// <param name="filterValue">Giá trị filter</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="pageNum">Số trang</param>
        /// <param name="listId">Danh sách Id nhân viên không lấy</param>
        /// <returns>Danh sách nhân viên</returns>
        public IEnumerable<Employee> GetEmployeesExceptSome(IEnumerable<Guid> listId, int? pageSize = null, int? pageNum = null, string filterValue = null)
        {
            DynamicParameters param = new DynamicParameters();

            param.Add("@ListId", string.Join(",", listId));
            param.Add("@FilterValue", filterValue);
            param.Add("@PageSize", pageSize);
            param.Add("@PageNum", pageNum);

            var employees = _dbConnection.Query<Employee>("Proc_GetEmployeesExceptSome", param, commandType: CommandType.StoredProcedure);

            return employees;
        }


        #endregion
    }
}
