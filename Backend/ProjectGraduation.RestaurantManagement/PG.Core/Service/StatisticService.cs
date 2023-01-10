using PG.Core.Entities;
using PG.Core.Enum;
using PG.Core.Interface.Repository;
using PG.Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Service
{
    public class StatisticService : IStatisticService
    {
        private readonly IOrderService _orderService;
        private readonly IInvoiceService _ivnoiceService;
        public StatisticService(IOrderService orderService, IInvoiceService invoiceService)
        {
            _orderService = orderService;
            _ivnoiceService = invoiceService;
        }

        /// <summary>
        /// Lấy thống kê theo khoảng thời gian
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ServiceResult GetStatistic(int type)
        {
            DateTime now = DateTime.Now;
            DateTime startDate = new DateTime(now.Year, 1, 1);
            DateTime endDate = new DateTime(now.Year + 1, 1, 1);
            //switch (type)
            //{
            //    case (int)TimeStatistic.Month:
            //        startDate = new DateTime(now.Year, 1, 1);
            //        endDate = new DateTime(now.Year + 1, 1, 1);
            //        break;
            //}

            var serviceResult = new ServiceResult();

            try
            {
                var invoices = _ivnoiceService.GetByTimes(startDate, endDate);

                var orderStat = _orderService.GetStatistic(invoices.Select(x => x.invoice_id.ToString()).ToList())?.OrderBy(x => x.sum_price);

                switch (type)
                {
                    case (int)TimeStatistic.Month:
                        var invGroup = invoices.GroupBy(g => g.payment_time.Month).Select(x => new { Month = x.Key, revenue = x.Sum(a => a.total) });

                        serviceResult.Data = new
                        {
                            revenueStat = invGroup,
                            dishStat = orderStat
                        };
                        break;
                    case (int)TimeStatistic.Quarter:
                        var gr = invoices.GroupBy(g => (g.payment_time.Month - 1) / 3).Select(x => new { Quater = x.Key, revenue = x.Sum(a => a.total) });

                        serviceResult.Data = new
                        {
                            revenueStat = gr,
                            dishStat = orderStat
                        };
                        break;
                }

                serviceResult.Code = PGCode.Success;
                serviceResult.Message = Properties.Resources.Msg_SuccessAdd;
            }
            catch (Exception ex)
            {
                serviceResult.Code = PGCode.Exception;
                serviceResult.Message = Properties.Resources.Msg_ServerError;
            }

            return serviceResult;
        }
    }
}
