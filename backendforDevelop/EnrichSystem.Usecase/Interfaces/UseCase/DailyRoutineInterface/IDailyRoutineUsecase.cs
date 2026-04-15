using EnrichSystem.Domain.DailyRoutines;
using EnrichSystem.Usecase.Dtos.DailyRoutineDtos.CompleteListDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Interfaces.UseCase.DailyRoutineInterface
{
    public interface IDailyRoutineUsecase
    {
        public Task<DailyRoutine> CreateDailyRoutine(DailyRoutine createObj);

        public Task<DailyRoutine> UpdateDailyRoutine(DailyRoutine updateObj);
        public Task<DailyRoutine> DeleteDailyRoutine(DailyRoutine deleteObj);
        public Task<DailyRoutine> GetDailyRoutine(int id);
        public Task<List<DailyRoutine>> GetAllRoutines();

        /// <summary>
        /// 完成目标
        /// </summary>
        /// <param name="targeDailyRoutine"></param>
        /// <returns></returns>
        public Task CompleteDailyRoutine(DailyRoutineCompleteListDto targeDailyRoutine);
    }
}
