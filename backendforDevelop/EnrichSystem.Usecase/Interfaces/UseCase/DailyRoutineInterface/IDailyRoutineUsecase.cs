using EnrichSystem.Domain.DailyRoutines;
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
    }
}
