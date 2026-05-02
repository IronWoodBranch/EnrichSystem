using EnrichSystem.Domain.DailyRoutines.DailyRoutineRecord;
using EnrichSystem.Usecase.Abstractions.Services.DailyRoutineRecords;
using EnrichSystem.Usecase.Dtos.DailyRoutineRecordDtos;
using EnrichSystem.Usecase.Interfaces.Repositories.DailyRoutineReposInterface.DailyRoutineLedgerRepoInterface;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Services.DailyRoutineRecords
{
    public class DailyRoutineRecordService : IDailyRoutineRecordService
    {
        private readonly IDailyRoutineRecordRepository _dailyRoutineRecordRepository;
        public DailyRoutineRecordService(IDailyRoutineRecordRepository dailyRoutineRecordRepository)
        {
            _dailyRoutineRecordRepository = dailyRoutineRecordRepository;
        }

        public async Task BulkInsertDailyRoutineRecord(List<DailyRoutineLedger> ledgerList)
        {
            await _dailyRoutineRecordRepository.BulkInsertDailyRoutineRecord(ledgerList);
        }

        public async Task CreateDailyRoutineRecord(DailyRoutineLedger ledger)
        {
            await _dailyRoutineRecordRepository.CreateNewLedger(ledger);
        }

        public async Task<GetAllDailyRoutineRecordsResultDto> GetAllDailyRoutineRecords()
        {
            var ledger = await _dailyRoutineRecordRepository.GetAllLedgers();
            GetAllDailyRoutineRecordsResultDto result = new GetAllDailyRoutineRecordsResultDto();
            foreach (var i in ledger)
            {
                var record = new DailyRoutineRecordDetailsDto
                {
                    Id = i.Id,
                    DailyRoutineId = i.DailyRoutineId,
                    Name = i.Name,
                    Date = i.Date,
                    IsCompleted = i.IsCompleted,
                    Amount = i.LedgerAmount,
                    Currency = i.CurrencyType
                };
                result.DailyRoutineRecords.Add(record);
            }
            return result;
        }

        /// <summary>
        /// 根据日期获取最近的日常记录，默认获取最近7天的记录
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public async Task<GetRecentDailyRoutineRecordsResultDto> GetRecentDailyRoutineRecords(int days = 7)
        {
            var recentDate = DateTime.Now.AddDays(-days);
            var ledgers = await _dailyRoutineRecordRepository.GetLedgersByDate(recentDate);

            var recordDetails = ledgers.Select(i => new DailyRoutineRecordDetailsDto
            {
                Id = i.Id,
                DailyRoutineId = i.DailyRoutineId,
                Name = i.Name,
                Date = i.Date,
                IsCompleted = i.IsCompleted,
                Amount = i.LedgerAmount,
                Currency = i.CurrencyType
            }).ToList();

            var result = new GetRecentDailyRoutineRecordsResultDto
            {
                DailyRoutineRecords = recordDetails
            };
            return result;
        }
    }
}
