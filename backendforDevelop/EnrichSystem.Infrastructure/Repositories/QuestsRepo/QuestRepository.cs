using EnrichSystem.Domain.Quests;
using EnrichSystem.Infrastructure.DbContexts;
using EnrichSystem.Usecase.Interfaces.Repositories.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Infrastructure.Repositories.QuestsRepo
{
    public class QuestRepository : IQuestRepository
    {
        private readonly ApplicationDbContext _context;
        public QuestRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Quest> CreateQuest(Quest quest)
        {
            _context.Quests.Add(quest);
            _context.SaveChanges();
            return await Task.FromResult(quest);
        }

        public async Task<Quest?> DeleteQuest(int id)
        {
            var deleteTarget = await _context.Quests.FindAsync(id);
            if (deleteTarget == null)
            {
                return null;
            }
            _context.Quests.Remove(deleteTarget);

            await _context.SaveChangesAsync();
            return deleteTarget;
        }


        public async Task<IEnumerable<Quest>> GetAllQuests()
        {
            return _context.Quests.ToList();
        }

        public async Task<Quest?> GetQuestById(int id)
        {
            return await _context.Quests.FindAsync(id);
        }

        public async Task<Quest> UpdateQuest(Quest quest)
        {
            var updateTarget = await _context.Quests.FindAsync(quest.Id);
            if (updateTarget == null)
            {
                throw new Exception("Quest not found");
            }
            updateTarget.Description = quest.Description;
            updateTarget.rewardId = quest.rewardId;
            updateTarget.CurrencyType = quest.CurrencyType;
            _context.Quests.Update(updateTarget);
            return await Task.FromResult(updateTarget);
        }
    }
}
