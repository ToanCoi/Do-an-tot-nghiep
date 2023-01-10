using PG.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Dto
{
    public class InvoiceDtoEdit : Invoice
    {
        public string customer_name { get; set; }

        public string customer_phone { get; set; }

        public Guid order_id { get; set; }
    }
}
