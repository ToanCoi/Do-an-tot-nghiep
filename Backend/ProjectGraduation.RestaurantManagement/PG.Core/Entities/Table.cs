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
        public string floor { get; set; }

        /// <summary>
        /// Tên bàn
        /// </summary>
        [Required]
        [DisplayName("Tên bàn")]
        public string table_name { get; set; }

        /// <summary>
        /// Số ghế tối đa
        /// </summary>
        [Required]
        [DisplayName("Số ghế")]
        public string max_size { get; set; }
    }
}
