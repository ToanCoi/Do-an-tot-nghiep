using PG.Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Entities
{
    /// <summary>
    /// Thông tin về thuế của nhân viên trong một đợt cụ thể
    /// </summary>
    public class EmployeeTax
    {
        /// <summary>
        /// Id
        /// </summary>
        [PrimaryKey]
        public Guid EmployeeTaxId { get; set; }

        /// <summary>
        /// Id viên
        /// </summary>
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Id kỳ thuế
        /// </summary>
        public Guid TaxId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// Lương
        /// </summary>
        [DisplayName("Tiền lương")]
        public double? EmployeeSalary { get; set; }

        /// <summary>
        /// Tiền bảo hiểm
        /// </summary>
        [DisplayName("Tiền BHXH")]
        public double? InsuranceMoney { get; set; }

        /// <summary>
        /// Thu nhập tính thuế
        /// </summary>
        public double? TaxableIncome { get; set; }

        /// <summary>
        /// Tiền thuế
        /// </summary>
        public double? TaxMoney { get; set; }

        /// <summary>
        /// Trạng thái của dữ liệu (thêm/sửa/xoá)
        /// </summary>
        public EntityState? Mode { get; set; }
    }
}
