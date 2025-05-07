using JobBit.DTOs;
using JobBit.Global;
using JobBit_Business;
using JobBit_DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBit.Controllers
{
    [Route("api/Statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {

        [HttpGet("GetStatistics", Name = "GetStatistics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<StatisticDTO> GetStatistics()
        {
            return Ok(Statistics.GetStatistics());
        }
    }
}
