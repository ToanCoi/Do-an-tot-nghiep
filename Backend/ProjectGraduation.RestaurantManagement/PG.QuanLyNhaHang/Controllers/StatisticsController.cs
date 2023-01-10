using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PG.Core.Enum;
using PG.Core.Interface.Service;

namespace PG.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _statisticService;
        public StatisticsController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        /// <summary>
        /// Lấy ra thống kê theo khoảng thời gian
        /// </summary>
        /// <returns></returns>
        [EnableCors("AllowCROSPolicy")]
        [HttpGet("statistic/{type}")]
        public IActionResult GetStatistic(int type)
        {
            
            var tablesDto = _statisticService.GetStatistic(type);

            if (tablesDto != null)
            {
                return Ok(tablesDto);
            }
            else
            {
                return NoContent();
            }
        }
    }
}
