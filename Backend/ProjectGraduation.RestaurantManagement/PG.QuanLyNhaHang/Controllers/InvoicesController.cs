using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PG.Core.Dto;
using PG.Core.Entities;
using PG.Core.Enum;
using PG.Core.Interface.Service;

namespace PG.API.Controllers
{
    public class InvoicesController : BaseController<Invoice>
    {
        private readonly IInvoiceService _invoiceService;
        public InvoicesController(IInvoiceService invoiceService) : base(invoiceService)
        {
            _invoiceService = invoiceService;
        }

        /// <summary>
        /// Thêm mới invoice
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        [EnableCors("AllowCROSPolicy")]
        [HttpPost("invoice")]
        public virtual IActionResult InsertInvoice([FromBody] InvoiceDtoEdit invoice)
        {
            var serviceResult = _invoiceService.InsertInvoice(invoice);

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
    }
}
