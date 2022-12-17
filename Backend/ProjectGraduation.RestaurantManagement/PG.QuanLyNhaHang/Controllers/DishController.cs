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
            return base.Post(dish);
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
            return base.Post(dish);
        }


        #endregion
    }
}
