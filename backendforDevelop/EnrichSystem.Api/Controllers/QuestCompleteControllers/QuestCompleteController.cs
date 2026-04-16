using EnrichSystem.Domain.QuestCompletes;
using EnrichSystem.Usecase.Interfaces.UseCase.QuestCompleteInterface;
using Microsoft.AspNetCore.Mvc;

namespace EnrichSystem.Api.Controllers.QuestCompleteControllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestCompleteController : ControllerBase
    {
        private readonly IQuestCompleteUsecase _questCompleteUsecase;
        public QuestCompleteController(IQuestCompleteUsecase questCompleteUsecase)
        {
            _questCompleteUsecase = questCompleteUsecase;
        }

        /// <summary>
        /// 创建一条完成的记录
        /// </summary>
        /// <param name="createObj"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateQuestComplete([FromBody] QuestComplete createObj)
        {
            var result = await _questCompleteUsecase.CreateQuestComplete(createObj);
            return Ok(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateQuestComplete([FromBody] QuestComplete updateObj)
        {
            var result = await _questCompleteUsecase.UpdateQuestComplete(updateObj);
            return Ok(result);
        }

        [HttpPost("CompleteQuest")]
        public async Task<IActionResult> CompleteQuest([FromBody] QuestComplete completeObj)
        {
            var result = await _questCompleteUsecase.CompleteQuest(completeObj);
            return Ok(result);
        }
    }
}
