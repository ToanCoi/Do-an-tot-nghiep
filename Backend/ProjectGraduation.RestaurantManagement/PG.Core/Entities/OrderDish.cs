using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Entities
{
    [Table("order_dish")]
    public class OrderDish : BaseEntity
    {
        /// <summary>
        /// Id order
        /// </summary>
        [PrimaryKey]
        [Required]
        public Guid dish_order_id { get; set; }

        /// <summary>
        /// Id dish
        /// </summary>
        public Guid dish_id { get; set; }

        /// <summary>
        /// Id order
        /// </summary>
        public Guid order_id { get; set; }

        /// <summary>
        /// Số lượng/món
        /// </summary>
        public int quantity { get; set; }

        /// <summary>
        /// Giá lúc khách mua
        /// </summary>
        public decimal current_price { get; set; }

    }
}
