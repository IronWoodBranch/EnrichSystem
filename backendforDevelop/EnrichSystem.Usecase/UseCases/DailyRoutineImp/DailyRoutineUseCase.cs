using EnrichSystem.Domain.DailyRoutines;
using EnrichSystem.Domain.DailyRoutines.DailyRoutineRecord;
using EnrichSystem.Domain.Enums;
using EnrichSystem.Domain.Ledgers;
using EnrichSystem.Usecase.Abstractions.Services.DailyRoutineRecords;
using EnrichSystem.Usecase.Dtos.DailyRoutineDtos.CompleteListDtos;
using EnrichSystem.Usecase.Interfaces.Repositories.DailyRoutineReposInterface.DailyRoutineReposInterface;
using EnrichSystem.Usecase.Interfaces.Repositories.LedgerRepoInterface;
using EnrichSystem.Usecase.Interfaces.UseCase.DailyRoutineInterface;
using EnrichSystem.Usecase.Services.DailyRoutineRecords;
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
        private readonly IDailyRoutineRecordService _dailyRoutineRecordService;
        public DailyRoutineUseCase(
            IDailyRoutineReopository dbRepo,
            ILedgerRepository ledgerRepo,
            IDailyRoutineRecordService dailyRoutineRecordService)
        {
            _dbRepo = dbRepo;
            _ledgerRepo = ledgerRepo;
            _dailyRoutineRecordService = dailyRoutineRecordService;
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

        public async Task<List<GetDailyRoutineResultDto>> GetAllRoutines()
        {
            var queryRes = await _dbRepo.GetAllRoutines();
            var res = new List<GetDailyRoutineResultDto>();
            foreach (var r in queryRes)
            {
                res.Add(new GetDailyRoutineResultDto
                {
                    Id = r.Id,
                    Name = r.Lable,
                    Key = r.Key,
                    amount = r.CompleteReward
                });
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetDailyRoutine"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// todo:改造成领域事件做完成，这里只做编排
        public async Task<CompleteDailyRoutinesResultDto> CompleteDailyRoutine(DailyRoutineCompleteListDto targetDailyRoutine)
        {
            //validate
            if (targetDailyRoutine == null || targetDailyRoutine.DailyRoutines == null || targetDailyRoutine.DailyRoutines.Count == 0)
            {
                throw new ArgumentException("Invalid input");
            }
            var idList = targetDailyRoutine.DailyRoutines.Select(x => x.Id)
                .Distinct()
                .ToList();


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
            var list = new CompleteDailyRoutinesResultDto
            {
                RoutineDetails = res.Select(x => new CompleteDailyRoutineDetailsDto
                {
                    Id = x.Id,
                    IsCompleted = targetDailyRoutine.DailyRoutines.First(r => r.Id == x.Id).IsCompleted,
                    Name = x.Key,
                    CurrencyType = x.currencyType,
                    Amount = targetDailyRoutine.DailyRoutines.First(r => r.Id == x.Id).IsCompleted ? x.CompleteReward : x.FailedPunish
                }).ToList()
            };


            // 存入每日记录单独的记录表
            var routineLedgerList = new List<DailyRoutineRecord>();
            routineLedgerList = res.Select(x => new DailyRoutineRecord
            {
                Name = x.Key,
                DailyRoutineId = x.Id.ToString(),
                Date = DateTime.Now,
                IsCompleted = targetDailyRoutine.DailyRoutines.First(r => r.Id == x.Id).IsCompleted,
                LedgerAmount = targetDailyRoutine.DailyRoutines.First(r => r.Id == x.Id).IsCompleted ? x.CompleteReward : x.FailedPunish,
                CurrencyType = x.currencyType,
                CompleteReward = x.CompleteReward,
                FailedPunish = x.FailedPunish
            }).ToList();
            await _dailyRoutineRecordService.BulkInsertDailyRoutineRecord(routineLedgerList);


            var platinumList = list.RoutineDetails.Where(x => x.CurrencyType == CurrencyType.Sun);
            var palitnumAmount = platinumList.Sum(x => x.Amount);
            var copperList = list.RoutineDetails.Where(x => x.CurrencyType == CurrencyType.Copper);
            var copperAmount = copperList.Sum(x => x.Amount);

            list.Platinum = palitnumAmount;
            list.Copper = copperAmount;
            return list;
        }
    }
}
