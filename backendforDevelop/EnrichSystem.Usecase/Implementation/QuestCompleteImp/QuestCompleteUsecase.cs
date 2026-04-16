using EnrichSystem.Domain.QuestCompletes;
using EnrichSystem.Usecase.Interfaces.Repositories.QuestCompletes;
using EnrichSystem.Usecase.Interfaces.UseCase.QuestCompleteInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Implementation.QuestCompleteImp
{
    public class QuestCompleteUsecase : IQuestCompleteUsecase
    {
        private readonly IQuestCompleteRepository _questCompleteRepository;

        public QuestCompleteUsecase(IQuestCompleteRepository questCompleteRepository)
        {
            _questCompleteRepository = questCompleteRepository;
        }

        public Task<QuestComplete> CompleteQuest(QuestComplete questComplete)
        {
            //todo:考虑校验任务定义，暂时不知道怎么校验，今后可能要做
            //step1:创建完成
            throw new NotImplementedException();
        }

        public async Task<QuestComplete> CreateQuestComplete(QuestComplete questComplete)
        {
            var result = await _questCompleteRepository.CreateQuestComplete(questComplete);
            return result;
        }

        public Task<QuestComplete> UpdateQuestComplete(QuestComplete questComplete)
        {
            var result = _questCompleteRepository.UpdateQuestComplete(questComplete);
            return result;
        }
    }
}
