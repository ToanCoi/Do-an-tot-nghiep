using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PG.Core.Entities;
using PG.Core.Interface.Service;

namespace PG.API.Controllers
{
    public class OrdersController : BaseController<Order>
    {
        #region Constructor
        public OrdersController(IOrderService orderService) : base(orderService)
        {

        }
        #endregion
    }
}
