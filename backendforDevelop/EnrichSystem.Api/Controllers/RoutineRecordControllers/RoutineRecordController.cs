using EnrichSystem.Usecase.Abstractions.Services.DailyRoutineRecords;
using EnrichSystem.Usecase.Dtos.DailyRoutineRecordDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnrichSystem.Api.Controllers.RoutineRecordControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutineRecordController : ControllerBase
    {
        private readonly ILogger<RoutineRecordController> _logger;
        private readonly IDailyRoutineRecordService _dailyRoutineRecordService;

        public RoutineRecordController(ILogger<RoutineRecordController> logger, IDailyRoutineRecordService dailyRoutineRecordService)
        {
            _logger = logger;
            _dailyRoutineRecordService = dailyRoutineRecordService;
        }
        [HttpGet("all")]
        public async Task<ActionResult<GetAllDailyRoutineRecordsResultDto>> GetAllDailyRoutineRecords()
        {
            var res = await _dailyRoutineRecordService.GetAllDailyRoutineRecords();
            return Ok(res);
        }

        [HttpGet("GetRecent")]
        public async Task<ActionResult<GetRecentDailyRoutineRecordsResultDto>> GetRecentDailyRoutineRecords(int days = 7)
        {
            var res = await _dailyRoutineRecordService.GetRecentDailyRoutineRecords();
            return Ok(res);
        }
    }
}
