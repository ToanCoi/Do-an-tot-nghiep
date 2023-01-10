using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Entities
{
    [Table("invoice")]
    public class Invoice : BaseEntity
    {
        [PrimaryKey]
        public Guid invoice_id { get; set; }

        public int? payment_type { get; set; }

        public DateTime payment_time { get; set; }

        public decimal total { get; set; }

        public Guid employee_id { get; set; }

        public Guid customer_id { get; set; }
    }
}
