using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PG.Core.Entities;
using PG.Core.Enum;
using PG.Core.Interface.Service;
using PG.Core.Service;
using static Dapper.SqlMapper;

namespace PG.API.Controllers
{
    public class DishController : BaseController<Dish>
    {
        private readonly IDishService _dishService;
        public static IWebHostEnvironment _webHostEnvironment;
        #region Constructor
        public DishController(IDishService dishService, IWebHostEnvironment webHostEnvironment) : base(dishService)
        {
            _webHostEnvironment = webHostEnvironment;
            _dishService = dishService;
        }
        #endregion

        #region Method
        /// <summary>
        /// Thêm mới một món ăn
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        [EnableCors("AllowCROSPolicy")]
        [HttpPost]
        public override IActionResult Post([FromForm] Dish dish)
        {
            var serviceResult = new ServiceResult();

            try
            {
                var files = dish.img_file;
                dish.img_file = null;

                if(dish.dish_id == Guid.Empty)
                {
                    dish.dish_id = Guid.NewGuid();
                }

                serviceResult = _dishService.InsertEntity(dish);

                if(serviceResult.Code == PGCode.Success && files != null && files.Length > 0)
                {
                    string path = _webHostEnvironment.WebRootPath + "\\DishPics\\";
                    if(!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = "DishPics_" + dish.dish_id + ".png";

                    if(System.IO.File.Exists(path + fileName))
                    {
                        System.IO.File.Delete(path + fileName);
                    }

                    using(FileStream fileStream = System.IO.File.Create(path + fileName)) 
                    { 
                        files.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                }
            }
            catch(Exception ex) {
                serviceResult.Code = PGCode.Exception;
                serviceResult.Message = ex.Message;
            }

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
        /// Sửa thông tin một bản ghi
        /// </summary>
        /// <param name="Id">Id của bản ghi cần sửa</param>
        /// <param name="entity">Đối tượng với thông tin cần sửa</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        [EnableCors("AllowCROSPolicy")]
        [HttpPut("{Id}")]
        public override IActionResult Put([FromForm] Dish dish)
        {
            var serviceResult = new ServiceResult();

            try
            {
                var files = dish.img_file;
                dish.img_file = null;

                serviceResult = _dishService.UpdateEntity(dish);

                if (serviceResult.Code == PGCode.Success && files != null && files.Length > 0)
                {
                    string path = _webHostEnvironment.WebRootPath + "\\DishPics\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = "DishPics_" + dish.dish_id + ".png";

                    if (System.IO.File.Exists(path + fileName))
                    {
                        System.IO.File.Delete(path + fileName);
                    }

                    using (FileStream fileStream = System.IO.File.Create(path + fileName))
                    {
                        files.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                serviceResult.Code = PGCode.Exception;
                serviceResult.Message = ex.Message;
            }

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


        #endregion
    }
}
