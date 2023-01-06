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
        /// Id dish
        /// </summary>
        public Guid invoice_id { get; set; }

        /// <summary>
        /// Id dish
        /// </summary>
        public Guid table_reservation_id { get; set; }

        /// <summary>
        /// Số lượng/món
        /// </summary>
        public int quantity { get; set; }

    }
}
