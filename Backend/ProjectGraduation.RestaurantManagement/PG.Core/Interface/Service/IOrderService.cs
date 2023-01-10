using PG.Core.Dto;
using PG.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Interface.Service
{
    public interface IOrderService : IBaseService<Order>
    {
        /// <summary>
        /// Lấy ra bàn trong nhà hàng
        /// </summary>
        /// <returns></returns>
        List<TableDtoEdit> GetTables();

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm mới</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        ServiceResult InsertOrder(OrderDtoEdit order);

        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của đối tượng cần lấy</param>
        /// <returns>Một bản ghi lấy được theo Id</returns>
        OrderDtoEdit GetOrderById(Guid Id);

        ServiceResult UpdateOrder(OrderDtoEdit entity);

        List<StatisticDtoEdit> GetStatistic(List<string> invoiceIds);
    }
}
