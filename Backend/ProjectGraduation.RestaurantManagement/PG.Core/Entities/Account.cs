using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Entities
{
    [Table("account")]
    public class Account : BaseEntity
    {
        public Guid account_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string salt { get; set; }
        public Guid user_id { get; set; }
    }
}
