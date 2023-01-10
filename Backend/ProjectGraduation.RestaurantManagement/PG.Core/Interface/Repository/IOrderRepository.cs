using PG.Core.Dto;
using PG.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Interface.Repository
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        /// <summary>
        /// Lấy ra bàn trong nhà hàng cùng trạng thái đặt của nó
        /// </summary>
        /// <returns></returns>
        List<TableDtoEdit> GetTables();

        /// <summary>
        /// Lấy ra các món ăn của order
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        List<DishDtoEdit> GetDishes(Guid orderID);

        List<StatisticDtoEdit> GetStatistic(List<string> invoiceIds);
    }
}
