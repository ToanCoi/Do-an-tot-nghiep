using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Entities
{
    public class Dish
    {
        /// <summary>
        /// Id món
        /// </summary>
        [PrimaryKey]
        public Guid dish_id { get; set; }

        /// <summary>
        /// Tên món
        /// </summary>
        [Required]
        [Unique]
        [DisplayName("Tên món")]
        public string dish_name { get; set; }


        /// <summary>
        /// Loại món ăn
        /// </summary>
        [DisplayName("Loại")]
        public int dish_type { get; set; }

        /// <summary>
        /// Đơn giá
        /// </summary>
        [DisplayName("Đơn giá")]
        public string price { get; set; }


        /// <summary>
        /// Đơn giá
        /// </summary>
        [DisplayName("Trạng thái")]
        public string dish_status { get; set; }

        /// <summary>
        /// Đơn vị tính
        /// </summary>
        //[DisplayName("Đơn vị tính")]
        //public int unit { get; set; }


    }
}
