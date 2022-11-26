using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PG.Core.Entities;
using PG.Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PG.API.Controllers
{
    public class EmployeesController : BaseController<Employee>
    {
        #region Declare
        IEmployeeService _employeeService;
        #endregion

        #region Constructor
        public EmployeesController(IEmployeeService employeeService) : base(employeeService)
        {
            _employeeService = employeeService;
        }
        #endregion

        #region Method
        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        [EnableCors("AllowCROSPolicy")]
        [HttpGet("NewEmployeeCode")]
        public IActionResult GetNewEmployeeCode()
        {
            var newEmployeeCode = _employeeService.GetNewEmployeeCode();

            if (newEmployeeCode != null)
            {
                return Ok(newEmployeeCode);
            }
            else
            {
                return NoContent();
            }
        }


        /// <summary>
        /// Lấy dữ liệu filter theo mã, tên, số điện thoại
        /// </summary>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="pageNum">Số trang hiện tại</param>
        /// <param name="filterValue">Giá trị filter</param>
        /// <returns>Dữ liệu gồm bản ghi theo filter, tổng số trang</returns>
        [EnableCors("AllowCROSPolicy")]
        [HttpGet("Filter")]
        public IActionResult GetEmployeesFilter([FromQuery]int? pageSize, [FromQuery]int? pageNum, [FromQuery] string filterValue)
        {
            var employees = _employeeService.GetEmployeesFilter(pageSize, pageNum, filterValue);
            
            if(employees != null)
            {
                return Ok(employees);
            }
            else
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Lấy nhân viên theo filter ngoại trừ 1 số nhân viên
        /// </summary>
        /// <param name="filterValue">Giá trị filter</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="pageNum">Số trang</param>
        /// <param name="listId">Danh sách Id nhân viên không lấy</param>
        /// <returns>Danh sách nhân viên</returns>
        [EnableCors("AllowCROSPolicy")]
        [HttpPost("Filter/Except")]
        public IActionResult GetEmployeesExceptSome([FromBody] IEnumerable<Guid> listId, [FromQuery] int? pageSize, [FromQuery] int? pageNum, [FromQuery] string filterValue)
        {
            var employees = _employeeService.GetEmployeesExceptSome(listId, pageSize, pageNum, filterValue);

            if (employees != null)
            {
                return Ok(employees);
            }
            else
            {
                return NoContent();
            }
        }
        #endregion
    }
}
