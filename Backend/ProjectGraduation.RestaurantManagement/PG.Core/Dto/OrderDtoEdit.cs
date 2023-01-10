using PG.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Dto
{
    public class OrderDtoEdit : Order
    {
        /// <summary>
        /// Những món ăn đặt trong order
        /// </summary>
        [IgnoreField]
        public List<DishDtoEdit> dishes { get; set; }
    }
}
