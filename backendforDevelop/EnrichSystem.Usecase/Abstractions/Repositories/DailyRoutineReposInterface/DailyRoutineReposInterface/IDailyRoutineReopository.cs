using EnrichSystem.Domain.DailyRoutines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Interfaces.Repositories.DailyRoutineReposInterface.DailyRoutineReposInterface
{
    public interface IDailyRoutineReopository
    {
        /// <summary>
        /// 拿到所有任务定义
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<DailyRoutineDefination>> GetAllRoutines();

        /// <summary>
        /// 根据IdList拿到
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public Task<IEnumerable<DailyRoutineDefination>> GetRoutinesByIds(IEnumerable<int> ids);

        /// <summary>
        /// 增加
        /// </summary>
        /// <returns></returns>
        public Task CreateNewRoutine(DailyRoutineDefination dailyRoutine);

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public Task DeleteRoutine(DailyRoutineDefination dailyRoutine);

        /// <summary>
        /// 更新，todo:以后改改参数
        /// </summary>
        /// <returns></returns>
        public Task UpdateRoutine(DailyRoutineDefination dailyRoutine);

    }
}
