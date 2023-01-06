using PG.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Dto
{
    public class DishDtoEdit : Dish
    {
        /// <summary>
        /// Số lượng dùng cho order
        /// </summary>
        public int quantity { get; set; }
    }
}
