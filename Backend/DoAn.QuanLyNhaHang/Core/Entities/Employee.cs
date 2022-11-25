using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Entities
{
    /// <summary>
    /// Nhân viên
    /// </summary>
    public class Employee : BaseEntity
    {
        /// <summary>
        /// Id nhân viên
        /// </summary>
        [PrimaryKey]
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [Required]
        [Unique]
        [DisplayName("Mã nhân viên")]
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        [Required]
        [DisplayName("Tên nhân viên")]
        public string EmployeeName { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        [DisplayName("Ngày sinh")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        [DisplayName("Giới tính")]
        public int? Gender { get; set; }

        /// <summary>
        /// Id đơn vị của nhân viên
        /// </summary>
        [Required]
        [DisplayName("Mã phòng ban")]
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// Tên đơn vị
        /// </summary>
        [DisplayName("Đơn vị")]
        public string DepartmentName { get; set; }

        /// <summary>
        /// Số CMND/căn cước
        /// </summary>
        [DisplayName("Số CMND/căn cước")]
        public string IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp CMND/Căn cước
        /// </summary>
        [DisplayName("Ngày cấp CMND/Căn cước")]
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// Nơi cấp CMND/Căn cước
        /// </summary>
        [DisplayName("Nơi cấp CMND/Căn cước")]
        public string IdentityPlace { get; set; }

        /// <summary>
        /// Chức danh nhân viên
        /// </summary>
        [DisplayName("Chức danh")]
        public string EmployeePosition { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        [DisplayName("Địa chỉ")]
        public string Address { get; set; }

        /// <summary>
        /// Số tài khoản ngân hàng
        /// </summary>
        [DisplayName("Số tài khoản ngân hàng")]
        public string BankAccountNumber { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        [DisplayName("Tên ngân hàng")]
        public string BankName { get; set; }

        /// <summary>
        /// Tên chi nhánh ngân hàng
        /// </summary>
        [DisplayName("Tên chi nhánh ngân hàng")]
        public string BankBranchName { get; set; }

        /// <summary>
        /// Địa chỉ ngân hàng
        /// </summary>
        [DisplayName("Địa chỉ ngân hàng")]
        public string BankProvinceName { get; set; }

        /// <summary>
        /// Số điện thoại di động
        /// </summary>
        [DisplayName("Số điện thoại di động")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Số điện thoại cố định
        /// </summary>
        [DisplayName("Số điện thoại cố định")]
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [DisplayName("Email")]
        public string Email { get; set; }
    }
}
