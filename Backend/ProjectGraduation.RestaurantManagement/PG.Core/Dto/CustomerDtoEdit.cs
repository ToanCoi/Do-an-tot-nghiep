using PG.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Dto
{
    public class CustomerDtoEdit : Customer
    {
        public decimal money_paid { get; set; }
    }
}
