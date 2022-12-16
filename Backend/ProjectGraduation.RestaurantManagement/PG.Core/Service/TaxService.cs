using PG.Core.Entities;
using PG.Core.Interface.Repository;
using PG.Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Service
{
    public class TaxService : BaseService<Tax>, ITaxService
    {
        #region Declare
        ITaxRepository _taxRepository;
        IEmployeeRepository _employeeRepository;
        #endregion
        #region Constructor
        public TaxService(ITaxRepository taxRepository, IEmployeeRepository employeeRepository, IServiceProvider serviceProvider) : base(taxRepository, serviceProvider: serviceProvider)
        {
            _taxRepository = taxRepository;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Method

        /// <summary>
        /// Tạo mã kỳ thuế mới
        /// </summary>
        /// <returns>Mã kỳ thuế mới</returns>
        public string GetNewTaxCode()
        {
            var maxTaxCode = _taxRepository.GetMaxTaxCode();

            var index = maxTaxCode.IndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });

            //Lấy ra các phần số và không phải số
            var alphaPart = maxTaxCode.Substring(0, index);
            var numberPart = maxTaxCode.Substring(index);
            var numberLength = numberPart.Length;

            //Tăng mã lên 1
            numberPart = (Int32.Parse(numberPart) + 1).ToString();

            //Thêm 0 vào đầu nếu thiếu
            numberPart = numberPart.PadLeft(numberLength, '0');

            var nextCode = alphaPart + numberPart;

            return nextCode;
        }

        /// <summary>
        /// Lấy danh sách kỳ thuế theo năm và mã hoặc tên
        /// </summary>
        /// <param name="year">Năm cần tìm</param>
        /// <param name="filterValue">Giá trị filter</param>
        /// <param name="pageSize">Số bản ghi/trang</param>
        /// <param name="pageNum">Số trang</param>
        /// <returns>Danh sách kỳ thuế theo giá trị truyền vào</returns>
        public object GetTaxsFilter(int year, int? pageSize = null, int? pageNum = null, string filterValue = null)
        {
            //Lấy dữ liệu về
            var taxs = _taxRepository.GetTaxsFilter(year, pageSize, pageNum, filterValue);
            
            //Lấy tổng số bản ghi
            var totalRecord = Int32.Parse(_taxRepository.GetTotalRecord(year, filterValue));

            //Dữ liệu trả về
            var data = new
            {
                TotalRecord = totalRecord,
                TotalPage = pageSize != null ? Math.Ceiling((decimal)((decimal)totalRecord / pageSize)) : 1,
                data = taxs
            };

            return data;
        }

        protected override bool CustomValidate(Tax tax, List<string> errorMsg)
        {
            return this.ValidateAllEmployeeExist(tax, errorMsg);
        }

        /// <summary>
        /// Hàm kiểm tra tất cả nhân viên đã tồn tại trong csdl
        /// </summary>
        /// <param name="tax">Đối tượng tax</param>
        /// <returns>Dữ liệu hợp lệ hay không</returns>
        private bool ValidateAllEmployeeExist(Tax tax, List<string> errorMsg)
        {
            var allExist = true;

            foreach (var et in tax.TaxDetail)
            {
                //Nếu ID nhân viên chưa có trong csdl
                if (_employeeRepository.CheckExist(et.EmployeeId) != 1)
                {
                    allExist = false;
                    errorMsg.Add(Properties.Resources.Msg_DataNotExist);
                    break;
                }
            }

            return allExist;
        }
        #endregion
    }
}
