using Dapper;
using Microsoft.Extensions.Configuration;
using PG.Core.Dto;
using PG.Core.Entities;
using PG.Core.Interface.Repository;
using System.Data;
using static Dapper.SqlMapper;


namespace PG.Infrastructure.Repository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(IConfiguration configuration, IServiceProvider serviceProvider) : base(configuration, serviceProvider)
        {

        }

        /// <summary>
        /// Lấy ra bàn trong nhà hàng cùng trạng thái đặt của nó
        /// </summary>
        /// <returns></returns>
        public List<TableDtoEdit> GetTables()
        {
            string sql = "SELECT t.table_id, t.floor, t.table_name, t.max_size, t.table_status, IF(o.table_id IS NOT NULL, 1, 0) AS table_order_status, o.order_id " +
                        "FROM `table` t " +
                        "LEFT JOIN `order` o ON t.table_id = o.table_id AND (o.paid IS FALSE OR o.paid IS NULL)";

            return _dbConnection.QueryAsync<TableDtoEdit>(sql, commandType: CommandType.Text).GetAwaiter().GetResult().ToList();
        }

        /// <summary>
        /// Lấy ra các món ăn của order
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public List<DishDtoEdit> GetDishes(Guid orderID)
        {
            string sql = $"SELECT od.dish_order_id, od.quantity, od.dish_id, od.order_id, d.dish_name, d.dish_type, d.price, d.dish_status, d.description " +
                            $"FROM order_dish od " +
                            $"INNER JOIN dish d ON od.dish_id = d.dish_id " +
                            $"WHERE od.order_id = '{orderID.ToString()}'";

            return _dbConnection.QueryAsync<DishDtoEdit>(sql, commandType: CommandType.Text).GetAwaiter().GetResult().ToList();
        }

        public List<StatisticDtoEdit> GetStatistic(List<string> invoiceIds)
        {
            var ids = new DynamicParameters();
            ids.Add($"ids", invoiceIds);

            string sql = $"SELECT od.dish_id, d.dish_name, SUM(od.quantity) AS sum_quantity, SUM(od.current_price) AS sum_price " +
                $"FROM order_dish od " +
                $"INNER JOIN `order` o ON od.order_id = o.order_id AND o.invoice_id IN @ids " +
                $"INNER JOIN dish d ON od.dish_id = d.dish_id " +
                $"GROUP BY od.dish_id,   d.dish_name;";

            return _dbConnection.QueryAsync<StatisticDtoEdit>(sql, ids, commandType: CommandType.Text).GetAwaiter().GetResult().ToList();
        }
    }
}
