using EnrichSystem.Domain.DailyRoutines;
using EnrichSystem.Usecase.Dtos.DailyRoutineDtos.CompleteListDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Interfaces.UseCase.DailyRoutineInterface
{
    public interface IDailyRoutineDefUsecase
    {
        public Task<DailyRoutineDefination> CreateDailyRoutine(DailyRoutineDefination createObj);

        public Task<DailyRoutineDefination> UpdateDailyRoutine(DailyRoutineDefination updateObj);
        public Task<DailyRoutineDefination> DeleteDailyRoutine(DailyRoutineDefination deleteObj);
        public Task<DailyRoutineDefination> GetDailyRoutine(int id);
        public Task<List<GetDailyRoutineResultDto>> GetAllRoutines();

        /// <summary>
        /// 完成目标
        /// </summary>
        /// <param name="targeDailyRoutine"></param>
        /// <returns></returns>
        public Task<CompleteDailyRoutinesResultDto> CompleteDailyRoutine(DailyRoutineCompleteListDto targeDailyRoutine);
    }
}
