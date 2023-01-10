using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PG.Core.Dto;
using PG.Core.Entities;
using PG.Core.Enum;
using PG.Core.Interface.Service;
using PG.Core.Service;
using static Dapper.SqlMapper;

namespace PG.API.Controllers
{
    public class OrdersController : BaseController<Order>
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService) : base(orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Lấy ra bàn trong nhà hàng cùng trạng thái đặt của nó
        /// </summary>
        /// <returns></returns>
        [EnableCors("AllowCROSPolicy")]
        [HttpGet("tables")]
        public IActionResult GetTables()
        {
            var tablesDto = _orderService.GetTables();

            if (tablesDto != null)
            {
                return Ok(tablesDto);
            }
            else
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Thêm mới order
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        [EnableCors("AllowCROSPolicy")]
        [HttpPost("order")]
        public virtual IActionResult InsertOrder([FromBody] OrderDtoEdit order)
        {
            var serviceResult = _orderService.InsertOrder(order);

            switch (serviceResult.Code)
            {
                case PGCode.Exception:
                    return StatusCode(500, serviceResult);
                case PGCode.InvalidData:
                    return BadRequest(serviceResult);
                default:
                    return StatusCode(201, serviceResult);
            }
        }

        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của bản ghi</param>
        /// <returns>Một đối tượng tìm được theo Id</returns>
        [EnableCors("AllowCROSPolicy")]
        [HttpGet("order/{Id}")]
        public IActionResult GetOrder([FromRoute] Guid Id)
        {
            var entity = _orderService.GetOrderById(Id);

            if (entity != null)
            {
                return Ok(entity);
            }
            else
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Sửa thông tin một bản ghi
        /// </summary>
        /// <param name="Id">Id của bản ghi cần sửa</param>
        /// <param name="entity">Đối tượng với thông tin cần sửa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        [EnableCors("AllowCROSPolicy")]
        [HttpPut("order")]
        public IActionResult UpdateOrder(OrderDtoEdit order)
        {
            var serviceResult = _orderService.UpdateOrder(order);

            switch (serviceResult.Code)
            {
                case PGCode.Exception:
                    return StatusCode(500, serviceResult);
                case PGCode.InvalidData:
                    return BadRequest(serviceResult);
                default:
                    return Ok(serviceResult);
            }
        }

    }
}
