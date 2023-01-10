using PG.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Dto
{
    public class TableDtoEdit : Table
    {
        /// <summary>
        /// Trạng thái đặt của bàn: 0-Trống, 1-đang có khách
        /// </summary>
        public int table_order_status { get; set; }

        public Guid? order_id { get; set; }
    }
}
