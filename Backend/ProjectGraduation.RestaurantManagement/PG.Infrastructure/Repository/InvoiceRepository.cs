using Dapper;
using Microsoft.Extensions.Configuration;
using PG.Core.Entities;
using PG.Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Infrastructure.Repository
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(IConfiguration configuration, IServiceProvider serviceProvider) : base(configuration, serviceProvider)
        {

        }

        public IEnumerable<Invoice> GetByTimes(DateTime startDate, DateTime endDate)
        {
            var param = new DynamicParameters();
            param.Add($"startDate", startDate);
            param.Add($"endDate", endDate);

            string sql = $"SELECT * FROM invoice i WHERE i.payment_time BETWEEN @startDate AND @endDate";

            return _dbConnection.QueryAsync<Invoice>(sql, param, commandType: CommandType.Text).GetAwaiter().GetResult();
        }
    }
}
