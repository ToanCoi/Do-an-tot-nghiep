using PG.Core.Dto;
using PG.Core.Entities;
using PG.Core.Enum;
using PG.Core.Interface.Repository;
using PG.Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PG.Core.Service
{
    public class InvoiceService : BaseService<Invoice>, IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public InvoiceService(IInvoiceRepository invoiceRepository, IUserService userService, ICustomerService customerService, IOrderService orderService, IServiceProvider serviceProvider) : base(invoiceRepository, serviceProvider)
        {
            _customerService = customerService;
            _invoiceRepository = invoiceRepository;
            _userService = userService;
            _orderService = orderService;
        }


        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm mới</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public ServiceResult InsertInvoice(InvoiceDtoEdit invoice)
        {
            //Dữ liệu trả về
            _serviceResult.Data = invoice;

            //validate dữ liệu
            this.Validate(invoice);

            //Nếu qua validate mà oke thì lưu
            if (_serviceResult.Code == PGCode.ValidData)
            {
                try
                {
                        // Kiểm tra xem có khách hàng chưa, nếu chưa thì add
                        var user = _userService.GetEntityByProperty("phone", invoice.customer_phone);
                        Guid user_id = user?.user_id ?? Guid.NewGuid();
                        if (user == null)
                        {
                            _customerService.InsertCustomer(new CustomerDtoEdit()
                            {
                                user_id = user_id,
                                user_name = invoice.customer_name,
                                phone = invoice.customer_phone,
                                money_paid = invoice.total
                            });
                        }

                        invoice.invoice_id = Guid.NewGuid();
                        invoice.customer_id = user_id;
                        invoice.payment_time = DateTime.Now;
                        _invoiceRepository.InsertEntity(invoice, typeof(Invoice));


                        //update lại order
                        var order = _orderService.GetEntityById(invoice.order_id);

                        order.paid = true;
                        order.invoice_id = invoice.invoice_id;
                        _orderService.UpdateEntity(order);

                        _serviceResult.Code = PGCode.Success;
                        _serviceResult.Message = Properties.Resources.Msg_SuccessAdd;
                }
                catch (Exception ex)
                {
                    _serviceResult.Code = PGCode.Exception;
                    _serviceResult.Message = Properties.Resources.Msg_ServerError;
                }
            }

            return _serviceResult;
        }

        public List<Invoice> GetByTimes(DateTime startDate, DateTime endDate)
        {
            return _invoiceRepository.GetByTimes(startDate, endDate).ToList();
        }
    }
}
