using EnrichSystem.Domain.DailyRoutines;
using EnrichSystem.Usecase.Dtos.DailyRoutineDtos.CompleteListDtos;
using EnrichSystem.Usecase.Interfaces.UseCase.DailyRoutineInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnrichSystem.Api.Controllers.RoutineControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyRoutineController : ControllerBase
    {
        private readonly IDailyRoutineUsecase _dailyRoutineUsecase;
        public DailyRoutineController(IDailyRoutineUsecase dailyRoutineUsecase)
        {
            _dailyRoutineUsecase = dailyRoutineUsecase;
        }

        [HttpPost]
        public async Task<IActionResult> CompleteDailyRoutines([FromBody] DailyRoutineCompleteListDto routines)
        {
            //todo :完成
            await _dailyRoutineUsecase.CompleteDailyRoutine(routines);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoutines()
        {
            var result = await _dailyRoutineUsecase.GetAllRoutines();
            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateDailyRoutine([FromBody] Domain.DailyRoutines.DailyRoutine createObj)
        {
            await _dailyRoutineUsecase.CreateDailyRoutine(createObj);
            return Ok();
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateDailyRoutine([FromBody] Domain.DailyRoutines.DailyRoutine updateObj)
        {
            await _dailyRoutineUsecase.UpdateDailyRoutine(updateObj);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDailyRoutine([FromBody] Domain.DailyRoutines.DailyRoutine deleteObj)
        {
            await _dailyRoutineUsecase.DeleteDailyRoutine(deleteObj);
            return Ok();
        }

    }
}
