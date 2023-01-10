using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Entities
{
    [Table("customer")]
    [Exact(exact = true)]
    public class Customer : User
    {
        public int? rank { get; set; }

        public int? point { get; set; }
    }
}
