using EnrichSystem.Domain.DailyRoutines.DailyRoutineRecord;
using EnrichSystem.Usecase.Dtos.DailyRoutineRecordDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Abstractions.Services.DailyRoutineRecords
{
    public interface IDailyRoutineRecordService
    {
        public Task CreateDailyRoutineRecord(DailyRoutineLedger ledger);

        public Task BulkInsertDailyRoutineRecord(List<DailyRoutineLedger> ledgerList);

        public Task<GetAllDailyRoutineRecordsResultDto> GetAllDailyRoutineRecords();
        public Task<GetRecentDailyRoutineRecordsResultDto> GetRecentDailyRoutineRecords(int days = 7);
    }
}
