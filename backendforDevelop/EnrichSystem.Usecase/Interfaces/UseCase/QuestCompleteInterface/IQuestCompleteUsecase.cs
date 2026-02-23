using EnrichSystem.Domain.QuestCompletes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Interfaces.UseCase.QuestCompleteInterface
{
    public interface IQuestCompleteUsecase
    {
        public Task<QuestComplete> CreateQuestComplete(QuestComplete questComplete);
        public Task<QuestComplete> UpdateQuestComplete(QuestComplete questComplete);

        /// <summary>
        /// 完成任务
        /// </summary>
        /// <param name="questComplete"></param>
        /// <returns></returns>
        public Task<QuestComplete> CompleteQuest(QuestComplete questComplete);
    }
}
