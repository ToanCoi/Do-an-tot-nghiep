using PG.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Interface.Service
{
    public interface ITaxService : IBaseService<Tax>
    {
        /// <summary>
        /// Tạo mã kỳ thuế mới
        /// </summary>
        /// <returns>Mã kỳ thuế mới</returns>
        public string GetNewTaxCode();

        /// <summary>
        /// Lấy danh sách kỳ thuế theo năm và mã hoặc tên
        /// </summary>
        /// <param name="year">Năm cần tìm</param>
        /// <param name="filterValue">Giá trị filter</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="pageNum">Số trang</param>
        /// <returns>Danh sách kỳ thuế theo giá trị truyền vào</returns>
        object GetTaxsFilter(int year, int? pageSize = null, int? pageNum = null, string filterValue = null);
    }
}
