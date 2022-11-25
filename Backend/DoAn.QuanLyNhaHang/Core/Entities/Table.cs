using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Entities
{
    public class Table : BaseEntity
    {
        /// <summary>
        /// ID bàn
        /// </summary>
        [PrimaryKey]
        public int table_id { get; set; }

        /// <summary>
        /// Tầng mấy
        /// </summary>
        [Required]
        [DisplayName("Tầng")]
        public string floor { get; set; }

        /// <summary>
        /// Số ghế tối đa
        /// </summary>
        [Required]
        [DisplayName("Số ghế")]
        public string max_size { get; set; }
    }
}
