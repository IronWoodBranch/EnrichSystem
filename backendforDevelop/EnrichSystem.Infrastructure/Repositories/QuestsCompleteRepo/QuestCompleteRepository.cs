using EnrichSystem.Domain.QuestCompletes;
using EnrichSystem.Infrastructure.DbContexts;
using EnrichSystem.Usecase.Interfaces.Repositories.QuestCompletes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Infrastructure.Repositories.QuestsCompleteRepo
{
    public class QuestCompleteRepository : IQuestCompleteRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public QuestCompleteRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<QuestComplete> CreateQuestComplete(QuestComplete questComplete)
        {
            await _dbContext.QuestCompletes.AddAsync(questComplete);
            _dbContext.SaveChanges();
            // 返回新建的questComplete
            return questComplete;
        }


        public Task<QuestComplete?> DeleteQuestComplete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<QuestComplete>> GetAllQuestCompletes()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据Id查询任务完成的记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QuestComplete?> GetQuestCompleteById(int id)
        {
            var result =await _dbContext.QuestCompletes.Where(q => q.Id == id).FirstOrDefaultAsync();
            return result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="questComplete"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<QuestComplete> UpdateQuestComplete(QuestComplete questComplete)
        {
            var targetObject = await _dbContext.QuestCompletes.Where(q => q.Id == questComplete.Id).FirstOrDefaultAsync();
            if (targetObject == null)
            {
                throw new Exception("QuestComplete not found");
            }
            targetObject.QuestDefinationId = questComplete.QuestDefinationId;
            targetObject.CompletedAt = questComplete.CompletedAt;
            targetObject.Chronicle = questComplete.Chronicle;
            targetObject.Amount = questComplete.Amount;
            targetObject.CurrencyType = questComplete.CurrencyType;
            _dbContext.QuestCompletes.Update(targetObject);
            _dbContext.SaveChanges();
            return targetObject;
        }
    }
}
