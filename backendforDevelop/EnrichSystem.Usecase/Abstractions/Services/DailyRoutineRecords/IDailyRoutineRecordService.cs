using EnrichSystem.Domain.DailyRoutines.DailyRoutineRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Abstractions.Services.DailyRoutineRecords
{
    public interface IDailyRoutineRecordService
    {
        public Task CreateDailyRoutineRecord(DailyRoutineRecord ledger);

        public Task BulkInsertDailyRoutineRecord(List<DailyRoutineRecord> ledgerList);
    }
}
