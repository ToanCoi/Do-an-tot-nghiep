using PG.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Dto
{
    public class StatisticDtoEdit 
    {
        public Guid dish_id { get; set; }
        public string dish_name { get; set; }
        public int sum_quantity { get; set; }
        public decimal sum_price { get; set; }
    }
}
