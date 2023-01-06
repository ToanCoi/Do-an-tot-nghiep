using Microsoft.AspNetCore.Http;
using PG.Core.Entities;
using PG.Core.Enum;
using PG.Core.Interface.Repository;
using PG.Core.Interface.Service;
using PG.Storage;
using PG.Storage.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Core.Service
{
    public class DishService : BaseService<Dish>, IDishService
    {
        private IDishRepository _dishRepository;
        private IFileStorageService _fileStorageService;

        #region Constructor
        public DishService(IDishRepository dishRepository, IServiceProvider serviceProvider, IFileStorageService fileStorageService) : base(dishRepository, serviceProvider)
        {
            _dishRepository = dishRepository;
            _fileStorageService = fileStorageService;
        }
        #endregion

        #region Method
        /// <summary>
        /// Lấy dữ liệu paging
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public override object GetPagingData(PagingParam param)
        {
            //Lấy dữ liệu về
            var dishes = _dishRepository.GetEntitiesFilter(param);
            dishes.ToList().ForEach(x =>
            {
                x.dish_img = _fileStorageService.GetFileAsync(StorageFileType.DishImage, x.dish_id.ToString());
            });

            //Lấy tổng số bản ghi
            var totalRecord = _dishRepository.GetTotalFilters(param);

            //Dữ liệu trả về
            var data = new
            {
                TotalRecord = totalRecord,
                TotalPage = param.PageSize != 0 ? Math.Ceiling((decimal)((decimal)totalRecord / param.PageSize)) : 1,
                data = dishes
            };

            return data;
        }

        /// <summary>
        /// Thêm mới một bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm mới</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public override ServiceResult InsertEntity(Dish dish)
        {
            var file = dish.img_file;
            dish.img_file = null;

            base.InsertEntity(dish);

            try
            {
                if (_serviceResult.Code == PGCode.Success && file != null && file.Length > 0)
                {
                    _fileStorageService.SaveFileAsync(StorageFileType.DishImage, dish.dish_id.ToString(), file.OpenReadStream());
                }
            }
            catch (Exception ex)
            {
                _serviceResult.Code = PGCode.Exception;
                _serviceResult.Message = Properties.Resources.Msg_ServerError;
            }

            return _serviceResult;
        }

        /// <summary>
        /// Sửa món ăn
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm mới</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public override ServiceResult UpdateEntity(Dish dish)
        {
            var file = dish.img_file;
            dish.img_file = null;

            base.UpdateEntity(dish);

            try
            {
                if (_serviceResult.Code == PGCode.Success && file != null && file.Length > 0)
                {
                    _fileStorageService.UpdateFileAsync(StorageFileType.DishImage, dish.dish_id.ToString(), file.OpenReadStream(), Path.GetExtension(file.FileName));
                }
            }
            catch (Exception ex)
            {
                _serviceResult.Code = PGCode.Exception;
                _serviceResult.Message = Properties.Resources.Msg_ServerError;
            }

            return _serviceResult;
        }

        /// <summary>
        /// Lấy bản ghi theo Id
        /// </summary>
        /// <param name="Id">Id của đối tượng cần lấy</param>
        /// <returns>Một bản ghi lấy được theo Id</returns>
        public override Dish GetEntityById(Guid Id)
        {
            var dish = base.GetEntityById(Id);

            if (dish != null)
            {
                dish.dish_img = _fileStorageService.GetFileAsync(StorageFileType.DishImage, dish.dish_id.ToString());
            }

            return dish;
        }

        /// <summary>
        /// Lấy toàn bộ bản ghi
        /// </summary>
        /// <returns>List bản ghi lấy được</returns>
        public override IEnumerable<Dish> GetEntities()
        {
            var dishes = _dishRepository.GetEntities();

            dishes.ToList().ForEach(dish =>
            {
                if (dish != null)
                {
                    dish.dish_img = _fileStorageService.GetFileAsync(StorageFileType.DishImage, dish.dish_id.ToString());
                }
            });

            return dishes;
        }
        #endregion
    }
}
