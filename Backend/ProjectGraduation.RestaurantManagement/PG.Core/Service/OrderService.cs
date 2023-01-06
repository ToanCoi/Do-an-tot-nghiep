using PG.Core.Entities;
using PG.Core.Enum;
using PG.Core.Interface.Repository;
using PG.Core.Interface.Service;
using PG.Storage.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Service
{
    public class OrderService : BaseService<Order>, IOrderService
    {
        private IOrderRepository _orderRepository;
        private IOrderDishRepository _orderDishRepository;

        #region Constructor
        public OrderService(IServiceProvider serviceProvider, IOrderRepository orderRepository, IOrderDishRepository orderDishRepository) : base(orderRepository, serviceProvider)
        {
            _orderRepository = orderRepository;
            _orderDishRepository = orderDishRepository;
        }
        #endregion

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm mới</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public override ServiceResult InsertEntity(Order order)
        {
            //Dữ liệu trả về
            _serviceResult.Data = order;

            //Nếu qua validate mà oke thì lưu
            if (_serviceResult.Code == PGCode.ValidData)
            {
                try
                {
                    _orderRepository.InsertEntity(order);

                    order.dishes.ForEach(x =>
                    {
                        var orderDish = new OrderDish()
                                        {
                                            dish_id = x.dish_id,
                                            quantity = x.quantity
                                        };

                        _orderDishRepository.InsertEntity(orderDish);
                    });


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
