using EnrichSystem.Domain.DailyRoutines;
using EnrichSystem.Usecase.Interfaces.Repositories.DailyRoutineRepoInterface;
using EnrichSystem.Usecase.Interfaces.UseCase.DailyRoutineInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Implementation.DailyRoutineImp
{
    public class DailyRoutineUseCase : IDailyRoutineUsecase
    {
        private readonly IDailyRoutineReopository _dbRepo;
        public DailyRoutineUseCase(IDailyRoutineReopository dbRepo)
        {
            _dbRepo = dbRepo;
        }

        public async Task CreateDailyRoutine(DailyRoutine createObj)
        {
            var res = _dbRepo.CreateNewRoutine(createObj);

        }

        public async Task UpdateDailyRoutine(DailyRoutine updateObj)
        {
            var res = _dbRepo.UpdateRoutine(updateObj);
        }

        public async Task DeleteDailyRoutine(DailyRoutine deleteObj)
        {
            await _dbRepo.DeleteRoutine(deleteObj);
        }

        public Task<DailyRoutine> GetDailyRoutine(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DailyRoutine>> GetAllRoutines()
        {
            var res = await _dbRepo.GetAllRoutines();
            return res.ToList();
        }
    }
}
