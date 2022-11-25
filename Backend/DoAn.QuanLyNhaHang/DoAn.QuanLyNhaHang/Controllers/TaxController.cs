using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PG.Core.Entities;
using PG.Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PG.API.Controllers
{
    public class TaxController : BaseController<Tax>
    {
        #region Declare
        ITaxService _taxService;
        #endregion

        #region Constructor
        public TaxController(ITaxService taxService) : base(taxService)
        {
            _taxService = taxService;
        }
        #endregion

        #region Method
        /// <summary>
        /// Tạo mã kỳ thuế mới
        /// </summary>
        /// <returns>Mã kỳ thuế mới</returns>
        [EnableCors("AllowCROSPolicy")]
        [HttpGet("NewTaxCode")]
        public IActionResult GetNewEmployeeCode()
        {
            var newTaxCode = _taxService.GetNewTaxCode();

            if(newTaxCode != null)
            {
                return Ok(newTaxCode);
            }
            else
            {
                return NoContent();
            }
        }


        /// <summary>
        /// Lấy danh sách kỳ thuế theo năm và mã hoặc tên
        /// </summary>
        /// <param name="year">Năm cần tìm</param>
        /// <param name="filterValue">Giá trị filter</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="pageNum">Số trang</param>
        /// <returns>Danh sách kỳ thuế theo giá trị truyền vào</returns>
        [EnableCors("AllowCROSPolicy")]
        [HttpGet("Filter")]
        public IActionResult GetEmployeesFilter([FromQuery]int year, [FromQuery] int? pageSize, [FromQuery] int? pageNum, [FromQuery] string filterValue)
        {
            var taxs = _taxService.GetTaxsFilter(year, pageSize, pageNum, filterValue);

            if (taxs != null)
            {
                return Ok(taxs);
            }
            else
            {
                return NoContent();
            }
        }
        #endregion
    }
}
