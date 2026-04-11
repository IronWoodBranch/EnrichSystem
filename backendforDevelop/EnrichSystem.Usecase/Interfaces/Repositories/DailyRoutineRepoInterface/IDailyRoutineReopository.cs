using EnrichSystem.Domain.DailyRoutines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Interfaces.Repositories.DailyRoutineRepoInterface
{
    public interface IDailyRoutineReopository
    {
        /// <summary>
        /// 拿到所有任务定义
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<DailyRoutine>> GetAllRoutines();

        /// <summary>
        /// 增加
        /// </summary>
        /// <returns></returns>
        public Task CreateNewRoutine(DailyRoutine dailyRoutine);

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public Task DeleteRoutine(DailyRoutine dailyRoutine);

        /// <summary>
        /// 更新，todo:以后改改参数
        /// </summary>
        /// <returns></returns>
        public Task UpdateRoutine(DailyRoutine dailyRoutine);

    }
}
