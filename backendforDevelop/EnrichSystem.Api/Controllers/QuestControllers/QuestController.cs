using EnrichSystem.Domain.Quests;
using EnrichSystem.Usecase.Interfaces.Repositories.Quests;
using Microsoft.AspNetCore.Mvc;

namespace EnrichSystem.Api.Controllers.QuestControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestController : ControllerBase
    {
        private readonly IQuestRepository _questRepository;

        public QuestController(IQuestRepository questRepository)
        {
            _questRepository = questRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllQuests()
        {
            var quests = await _questRepository.GetAllQuests();
            return Ok(quests);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetQuestById(int id)
        {
            var quest =await _questRepository.GetQuestById(id);
            if (quest == null)
            {
                return NotFound();
            }
            return Ok(quest);
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuest([FromBody] Quest quest)
        {
            var createdQuest = await _questRepository.CreateQuest(quest);
            return CreatedAtAction(nameof(GetQuestById), new { id = createdQuest.Id }, createdQuest);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateQuest(int id, [FromBody] Quest quest)
        {
            if (id != quest.Id)
            {
                return BadRequest();
            }
            var updatedQuest = await _questRepository.UpdateQuest(quest);
            return Ok(updatedQuest);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteQuest(int id)
        {
            var deletedQuest = await _questRepository.DeleteQuest(id);
            if (deletedQuest == null)
            {
                return NotFound();
            }
            return Ok(deletedQuest);
        }
    }

}