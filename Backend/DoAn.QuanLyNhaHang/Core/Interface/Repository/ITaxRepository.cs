using PG.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Interface.Repository
{
    public interface ITaxRepository : IBaseRepository<Tax>
    {
        /// <summary>
        /// Lấy mã kỳ thuế lớn nhất trong db
        /// </summary>
        /// <returns>Mã nhân viên lớn</returns>
        public string GetMaxTaxCode();

        /// <summary>
        /// Lấy danh sách kỳ thuế theo năm và mã hoặc tên
        /// </summary>
        /// <param name="year">Năm cần tìm</param>
        /// <param name="filterValue">Giá trị filter</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="pageNum">Số trang</param>
        /// <returns>Danh sách kỳ thuế theo giá trị truyền vào</returns>
        IEnumerable<Tax> GetTaxsFilter(int year, int? pageSize = null, int? pageNum = null, string filterValue = null);

        /// <summary>
        /// Lấy tổng số bản ghi theo filter và năm
        /// </summary>
        /// <returns>Tổng số bản ghi theo filter và năm</returns>
        string GetTotalRecord(int year, string filterValue);

    }
}
