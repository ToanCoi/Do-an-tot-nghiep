using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Entities
{
    [Table("dish")]
    public class Dish : BaseEntity
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
        public string? dish_name { get; set; }


        /// <summary>
        /// Loại món ăn
        /// </summary>
        [DisplayName("Loại")]
        public int dish_type { get; set; }

        /// <summary>
        /// Đơn giá
        /// </summary>
        [DisplayName("Đơn giá")]
        public decimal price { get; set; }

        /// <summary>
        /// Đơn giá
        /// </summary>
        [DisplayName("Trạng thái")]
        public int dish_status { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [Required]
        [DisplayName("Mô tả")]
        public string? description { get; set; }

        /// <summary>
        /// Ảnh truyền lên
        /// </summary>
        [IgnoreField]
        public IFormFile? img_file { get; set; } = null;

        /// <summary>
        /// Ảnh lấy ra
        /// </summary>
        [IgnoreField]
        public byte[]? dish_img { get; set; }
        /// <summary>
        /// Đơn vị tính
        /// </summary>
        //[DisplayName("Đơn vị tính")]
        //public int unit { get; set; }


    }
}
