using PG.Core.Dto;
using PG.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Interface.Service
{
    public interface IInvoiceService : IBaseService<Invoice>
    {
        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm mới</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        ServiceResult InsertInvoice(InvoiceDtoEdit invoice);

        List<Invoice> GetByTimes(DateTime startDate, DateTime endDate);
    }
}
