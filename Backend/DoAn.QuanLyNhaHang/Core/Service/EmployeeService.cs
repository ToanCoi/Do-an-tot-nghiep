using PG.Core.Entities;
using PG.Core.Enum;
using PG.Core.Interface.Repository;
using PG.Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PG.Core.Service
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        #region Declare
        IEmployeeRepository _employeeRepository;
        #endregion

        #region Constructor
        public EmployeeService(IEmployeeRepository employeeRepository) : base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Method
        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        public string GetNewEmployeeCode()
        {
            var maxEmployeeCode = _employeeRepository.GetMaxEmployeeCode();

            var index = maxEmployeeCode.IndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });

            //Lấy ra các phần số và không phải số
            var alphaPart = maxEmployeeCode.Substring(0, index);
            var numberPart = maxEmployeeCode.Substring(index);
            var numberLength = numberPart.Length;

            //Tăng mã lên 1
            numberPart = (Int32.Parse(numberPart) + 1).ToString();

            //Thêm 0 vào đầu nếu thiếu
            numberPart = numberPart.PadLeft(numberLength, '0');

            var nextCode = alphaPart + numberPart;

            return nextCode;
        }

        /// <summary>
        /// Hàm lấy bản ghi theo filter và chia paging
        /// </summary>
        /// <param name="filterValue">Giá trị filter</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="pageNum">Số thứ tự trang</param>
        /// <returns>Danh sách bản ghi theo filter cùng thông tin phân trang</returns>
        public object GetEmployeesFilter(int? pageSize = null, int? pageNum = null, string filterValue = null)
        {
            //Lấy dữ liệu về
            var employees = _employeeRepository.GetEmployeesFilter(pageSize, pageNum, filterValue);

            //Lấy tổng số bản ghi
            var totalRecord = Int32.Parse(_employeeRepository.GetTotalRecord(filterValue));

            //Dữ liệu trả về
            var data = new
            {
                TotalRecord = totalRecord,
                TotalPage = pageSize != null ? Math.Ceiling((decimal)((decimal)totalRecord / pageSize)) : 1,
                data = employees
            };

            return data;
        }
        /// <summary>
        /// Lấy nhân viên theo filter ngoại trừ 1 số nhân viên
        /// </summary>
        /// <param name="filterValue">Giá trị filter</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="pageNum">Số trang</param>
        /// <param name="listId">Danh sách Id nhân viên không lấy</param>
        /// <returns>Danh sách nhân viên</returns>
        public object GetEmployeesExceptSome(IEnumerable<Guid> listId, int? pageSize = null, int? pageNum = null, string filterValue = null)
        {
            //Lấy dữ liệu về
            var employees = _employeeRepository.GetEmployeesExceptSome(listId, pageSize, pageNum, filterValue);

            //Lấy tổng số bản ghi
            var totalRecord = Int32.Parse(_employeeRepository.GetTotalRecord(filterValue)) - listId.ToList().Count;

            //Dữ liệu trả về
            var data = new
            {
                TotalRecord = totalRecord,
                TotalPage = pageSize != null ? Math.Ceiling((decimal)((decimal)totalRecord / pageSize)) : 1,
                data = employees
            };

            return data;
        }

        /// <summary>
        /// Lấy ra giá trị theo tên trường dữ liệu
        /// </summary>
        /// <param name="employee">Nhân viên</param>
        /// <param name="propName">Tên trường dữ liệu cần lấy giá trị</param>
        /// <returns>Giá trị cần lấy</returns>
        private object GetValueByProperty(Employee employee, string propName)
        {
            var propertyInfo = employee.GetType().GetProperty(propName);
            if (propertyInfo.PropertyType == typeof(DateTime) || propertyInfo.PropertyType == typeof(DateTime?))
            {
                var value = employee.GetType().GetProperty(propName).GetValue(employee, null);
                var date = Convert.ToDateTime(value).ToString("dd/MM/yyyy");

                return value != null ? date : "";
            }

            return employee.GetType().GetProperty(propName).GetValue(employee, null);
        }
        #endregion
    }
}
