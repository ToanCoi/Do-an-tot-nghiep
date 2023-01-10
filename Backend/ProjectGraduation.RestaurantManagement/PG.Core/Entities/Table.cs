using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Entities
{
    [Table("table")]
    public class Table : BaseEntity
    {
        /// <summary>
        /// ID bàn
        /// </summary>
        [PrimaryKey]
        public Guid table_id { get; set; }

        /// <summary>
        /// Tầng mấy
        /// </summary>
        [Required]
        [DisplayName("Tầng")]
        public int floor { get; set; }

        /// <summary>
        /// Tên bàn
        /// </summary>
        [Required]
        [DisplayName("Tên bàn")]
        public string table_name { get; set; }

        /// <summary>
        /// Số ghế tối đa
        /// </summary>
        [DisplayName("Số ghế")]
        public int? max_size { get; set; }

        /// <summary>
        /// Trạng thái: 1-hỏng, 0-đang sử dụng
        /// </summary>
        [DisplayName("Trạng thái")]
        public int table_status { get; set; }
    }
}
