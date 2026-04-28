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
            foreach(var i in ledger)
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
    }
}
