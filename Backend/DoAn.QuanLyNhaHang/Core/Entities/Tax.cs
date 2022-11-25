using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Entities
{
    /// <summary>
    /// Thuế
    /// </summary>
    public class Tax : BaseEntity
    {
        /// <summary>
        /// Id đợt tính thuế
        /// </summary>
        [PrimaryKey]
        public Guid TaxId { get; set; }

        /// <summary>
        /// Mã kỳ thuế
        /// </summary>
        [Required]
        [Unique]
        [DisplayName("Mã kỳ thuế")]
        public string TaxCode { get; set; }

        /// <summary>
        /// Tên kỳ thuế
        /// </summary>
        [DisplayName("Tên kỳ thuế")]
        public string TaxName { get; set; }

        /// <summary>
        /// Ngày bắt đầu
        /// </summary>
        [DisplayName("Ngày bắt đầu")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        [DisplayName("Ngày kết thúc")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Tổng số nhân viên trong đợt
        /// </summary>
        public int? TotalEmployee { get; set; }

        /// <summary>
        /// Tổng số tiền thuế trong đợt
        /// </summary>
        public double? TotalTaxMoney { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        [DisplayName("Ghi chú")]
        public string Note { get; set; }

        /// <summary>
        /// Chi tiết đợt thuế
        /// </summary>
        public IEnumerable<EmployeeTax> TaxDetail { get; set; }

    }
}
