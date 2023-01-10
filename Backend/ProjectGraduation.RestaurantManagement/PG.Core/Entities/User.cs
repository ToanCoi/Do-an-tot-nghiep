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
        [ChildNotUpdate]
        public int role { get; set; }

        [Required]
        [ChildNotUpdate]
        public string user_name { get; set; }

        [Required]
        [Unique]
        [ChildNotUpdate]
        public string? phone { get; set; }

        [ChildNotUpdate]
        public string? id_number { get; set; }

        [ChildNotUpdate]
        public DateTime? birth_date { get; set; }

        [ChildNotUpdate]
        public string? address { get; set; }
    }
}
