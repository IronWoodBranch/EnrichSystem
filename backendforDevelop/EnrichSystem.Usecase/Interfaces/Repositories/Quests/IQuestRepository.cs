using EnrichSystem.Domain.Quests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Interfaces.Repositories.Quests
{
    public interface IQuestRepository
    {
        /// <summary>
        /// 新建任务
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        public Task<Quest> CreateQuest(Quest quest);

        /// <summary>
        /// 根据id查询任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Quest?> GetQuestById(int id);

        /// <summary>
        /// 拿到所有任务
        /// todo:后续考虑分页，以及带用户的筛选
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Quest>> GetAllQuests();

        /// <summary>
        /// 改
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        public Task<Quest> UpdateQuest(Quest quest);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Quest?> DeleteQuest(int id);

    }
}
