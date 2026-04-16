using EnrichSystem.Domain.Quests;
using EnrichSystem.Usecase.Interfaces.UseCase.QuestsInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Implementation.QuestImp
{
    public class QuestUsecase : IQuestUsecase
    {
        public Task<Quest> CreateQuest(Quest quest)
        {
            throw new NotImplementedException();
        }

        public Task<Quest?> DeleteQuest(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Quest>> GetAllQuests()
        {
            throw new NotImplementedException();
        }

        public Task<Quest?> GetQuestById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Quest> UpdateQuest(Quest quest)
        {
            throw new NotImplementedException();
        }
    }
}
