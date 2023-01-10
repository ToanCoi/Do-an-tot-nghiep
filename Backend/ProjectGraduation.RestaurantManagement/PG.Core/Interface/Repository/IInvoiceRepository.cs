using PG.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Interface.Repository
{
    public interface IInvoiceRepository : IBaseRepository<Invoice>
    {
        IEnumerable<Invoice> GetByTimes(DateTime startDate, DateTime endDate);
    }
}
