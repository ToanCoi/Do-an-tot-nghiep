using Newtonsoft.Json;
using PG.Core.Dto;
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
        /// Lấy ra bàn trong nhà hàng
        /// </summary>
        /// <returns></returns>
        public List<TableDtoEdit> GetTables()
        {
            return _orderRepository.GetTables();
        }

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm mới</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public ServiceResult InsertOrder(OrderDtoEdit order)
        {
            //Dữ liệu trả về
            _serviceResult.Data = order;

            //validate dữ liệu
            this.Validate(order);

            //Nếu qua validate mà oke thì lưu
            if (_serviceResult.Code == PGCode.ValidData)
            {
                try
                {
                    order.order_id = Guid.NewGuid();
                    order.order_time = DateTime.Now;
                    _orderRepository.InsertEntity(order, typeof(Order));

                    order.dishes.ForEach(x =>
                    {
                        var orderDish = new OrderDish()
                        {
                            dish_id = x.dish_id,
                            quantity = x.quantity,
                            order_id = order.order_id
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

        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của đối tượng cần lấy</param>
        /// <returns>Một bản ghi lấy được theo Id</returns>
        public OrderDtoEdit GetOrderById(Guid Id)
        {
            var order =  _orderRepository.GetEntityById(Id);

            var orderDto = JsonConvert.DeserializeObject<OrderDtoEdit>(JsonConvert.SerializeObject(order));

            if(orderDto != null)
            {
                orderDto.dishes = _orderRepository.GetDishes(orderDto.order_id);
            }

            return orderDto;
        }

        /// <summary>
        /// Sửa thông tin một bản ghi
        /// </summary>
        /// <param name="Id">Id của bản ghi cần sửa</param>
        /// <param name="entity">Đối tượng có những thông tin cần sửa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public ServiceResult UpdateOrder(OrderDtoEdit entity)
        {
            entity.EntityState = (int)EntityState.Update;
            //validate dữ liệu
            this.Validate(entity);

            //Dữ liệu trả về
            _serviceResult.Data = entity;

            //Nếu qua validate mà oke thì lưu
            if (_serviceResult.Code == PGCode.ValidData)
            {
                var rowAffect = _orderRepository.UpdateEntity(entity, typeof(Order));

                // Xoá hết order_dish đi để insert lại
                _orderDishRepository.DeleteEntities(nameof(OrderDish.order_id), entity.order_id.ToString());

                entity.dishes.ForEach(x =>
                {
                    var orderDish = new OrderDish()
                    {
                        dish_id = x.dish_id,
                        quantity = x.quantity,
                        order_id = entity.order_id
                    };

                    _orderDishRepository.InsertEntity(orderDish);
                });

                //Trả về code tương ứng
                if (rowAffect > 0)
                {
                    _serviceResult.Code = PGCode.Success;
                    _serviceResult.Message = Properties.Resources.Msg_SuccessUpdate;
                }
                else
                {
                    _serviceResult.Code = PGCode.Exception;
                    _serviceResult.Message = Properties.Resources.Msg_ServerError;
                }
            }

            return _serviceResult;
        }

        public List<StatisticDtoEdit> GetStatistic(List<string> invoiceIds)
        {
            return _orderRepository.GetStatistic(invoiceIds);
        }
    }
}
