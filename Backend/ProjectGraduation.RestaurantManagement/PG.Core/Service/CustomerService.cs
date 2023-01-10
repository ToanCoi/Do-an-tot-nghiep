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

namespace PG.Core.Service
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;
        public CustomerService(ICustomerRepository customerRepository, IUserRepository userRepository, IServiceProvider serviceProvider) : base(customerRepository, serviceProvider)
        {
            _userRepository = userRepository;
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Thêm mới customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ServiceResult InsertCustomer(CustomerDtoEdit customer)
        {
            customer.EntityState = (int)EntityState.Add;


            //Dữ liệu trả về
            _serviceResult.Data = customer;

            //Nếu qua validate mà oke thì lưu
            if (_serviceResult.Code == PGCode.ValidData)
            {
                try
                {
                    customer.role = (int)Role.Customer;
                    if(customer.user_id == null || customer.user_id == Guid.Empty)
                    {
                        customer.user_id = Guid.NewGuid();
                    }
                    _userRepository.InsertEntity(customer, typeof(User));

                    //Tính điểm thưởng
                    if(customer.point == null || customer.point == 0)
                    {
                        customer.point = Convert.ToInt32(customer?.money_paid) / 1000;
                    }

                    _customerRepository.InsertEntity(customer, typeof(Customer));

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
    }
}
