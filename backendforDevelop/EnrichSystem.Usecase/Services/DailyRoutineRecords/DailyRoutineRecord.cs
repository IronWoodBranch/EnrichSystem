using EnrichSystem.Domain.DailyRoutines.DailyRoutineRecord;
using EnrichSystem.Usecase.Abstractions.Services.DailyRoutineRecords;
using EnrichSystem.Usecase.Interfaces.Repositories.DailyRoutineReposInterface.DailyRoutineLedgerRepoInterface;
using System;
using System.Collections.Generic;
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

        public async Task BulkInsertDailyRoutineRecord(List<DailyRoutineRecord> ledgerList)
        {
            await _dailyRoutineRecordRepository.BulkInsertDailyRoutineRecord(ledgerList);
        }

        public async Task CreateDailyRoutineRecord(DailyRoutineRecord ledger)
        {
            await _dailyRoutineRecordRepository.CreateNewLedger(ledger);
        }
    }
}
