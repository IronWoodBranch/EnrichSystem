using EnrichSystem.Domain.QuestCompletes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Interfaces.Repositories.QuestCompletes
{
    public interface IQuestCompleteRepository
    {
        /// <summary>
        /// 增
        /// </summary>
        /// <param name="questComplete"></param>
        /// <returns></returns>
        public Task<QuestComplete> CreateQuestComplete(QuestComplete questComplete);

        /// <summary>
        /// 改
        /// </summary>
        /// <param name="questComplete"></param>
        /// <returns></returns>
        public Task<QuestComplete> UpdateQuestComplete(QuestComplete questComplete);


        /// <summary>
        /// 查所有
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<QuestComplete>> GetAllQuestCompletes();

        /// <summary>
        /// 根据Id查单个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<QuestComplete?> GetQuestCompleteById(int id);

        /// <summary>
        /// 根据Id删除单个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<QuestComplete?> DeleteQuestComplete(int id);
    }
}
