using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Entities
{
    [Table("user")]
    public class User : BaseEntity
    {
        [PrimaryKey]
        public Guid user_id { get; set; }

        [Required]
        public int role { get; set; }

        public string? phone { get; set; }
        public string? id_number { get; set; }
        public DateTime? birth_date { get; set; }
        public string? address { get; set; }
    }
}
