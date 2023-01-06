using PG.Core.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Entities
{
    [Table("order")]
    public class Order : BaseEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [PrimaryKey]
        public Guid order_id { get; set; }

        /// <summary>
        /// Id Bàn
        /// </summary>
        public Guid? table_id { get; set; }

        /// <summary>
        /// Những món ăn đặt trong order
        /// </summary>
        [IgnoreField]
        public List<DishDtoEdit> dishes { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string? customer_name { get; set; }

        /// <summary>
        /// SĐT
        /// </summary>
        public string? customer_phone { get; set; }

        /// <summary>
        /// Thời gian order
        /// </summary>
        [Required]
        public DateTime order_time { get; set; }

        /// <summary>
        /// Đã thanh toán chưa
        /// </summary>
        public bool? paid { get; set; }
    }
}
