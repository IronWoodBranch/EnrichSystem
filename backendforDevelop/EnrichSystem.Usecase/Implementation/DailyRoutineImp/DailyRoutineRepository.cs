using EnrichSystem.Domain.DailyRoutines;
using EnrichSystem.Domain.Ledgers;
using EnrichSystem.Usecase.Dtos.DailyRoutineDtos.CompleteListDtos;
using EnrichSystem.Usecase.Interfaces.Repositories.DailyRoutineRepoInterface;
using EnrichSystem.Usecase.Interfaces.Repositories.LedgerRepoInterface;
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
        private readonly ILedgerRepository _ledgerRepo;
        public DailyRoutineUseCase(IDailyRoutineReopository dbRepo, ILedgerRepository ledgerRepo)
        {
            _dbRepo = dbRepo;
            _ledgerRepo = ledgerRepo;
        }

        public async Task<DailyRoutine> CreateDailyRoutine(DailyRoutine createObj)
        {
            var res = _dbRepo.CreateNewRoutine(createObj);
            await res;
            return createObj;
        }

        public async Task<DailyRoutine> UpdateDailyRoutine(DailyRoutine updateObj)
        {
            var res = _dbRepo.UpdateRoutine(updateObj);
            return updateObj;
        }

        public async Task<DailyRoutine> DeleteDailyRoutine(DailyRoutine deleteObj)
        {
            await _dbRepo.DeleteRoutine(deleteObj);
            return deleteObj;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetDailyRoutine"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<DailyRoutineCompleteResultDto> CompleteDailyRoutine(DailyRoutineCompleteListDto targetDailyRoutine)
        {
            //validate
            if (targetDailyRoutine == null || targetDailyRoutine.DailyRoutines == null || targetDailyRoutine.DailyRoutines.Count == 0)
            {
                throw new ArgumentException("Invalid input");
            }
            var idList = targetDailyRoutine.DailyRoutines.Select(x => x.Id)
                .Distinct()
                .ToList();


            //todo:这里不用GetAll
            //早期先这么写，免得越写越多，先快速实现功能
            var res = await _dbRepo.GetRoutinesByIds(idList);

            List<Ledger> ledgerList = new List<Ledger>();
            foreach (var id in idList)
            {
                //任务定义
                var routineDefine = res.First(x => x.Id == id);
                //是否完成
                var isTargetCompleted = targetDailyRoutine.DailyRoutines.First(x => x.Id == id).IsCompleted;

                var ledger = new Ledger
                {
                    Amount = isTargetCompleted ? routineDefine.CompleteReward : routineDefine.FailedPunish,
                    Date = DateTime.Now,
                    Description = $"日常任务完成{routineDefine.Key}",
                    CurrencyType = routineDefine.currencyType,
                    QuestCompleteId = id,
                };
                ledgerList.Add(ledger);
            }
            await _ledgerRepo.BulkInsert(ledgerList);
            //todo1:把完成的奖励一览list返回给前端
            //todo2:
            return new DailyRoutineCompleteResultDto
            {
                Items = res.Select(x => new DailyRoutineCompleteResultItem
                {
                    Id = x.Id,
                    IsCompleted = targetDailyRoutine.DailyRoutines.First(r => r.Id == x.Id).IsCompleted,
                    RoutineName = x.Key,
                    Amount = targetDailyRoutine.DailyRoutines.First(r => r.Id == x.Id).IsCompleted ? x.CompleteReward : x.FailedPunish
                }).ToList()
            };
        }
    }
}
