using EnrichSystem.Domain.DailyRoutines;
using EnrichSystem.Domain.DailyRoutines.DailyRoutineRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Interfaces.Repositories.DailyRoutineReposInterface.DailyRoutineLedgerRepoInterface
{
    public interface IDailyRoutineRecordRepository
    {
        public Task CreateNewLedger(DailyRoutineRecord ledger);

        public Task BulkInsertDailyRoutineRecord(List<DailyRoutineRecord> records);
        public Task DeleteLedger(DailyRoutineRecord ledger);

        public Task UpdateLedger(DailyRoutineRecord ledger);

        public Task<IEnumerable<DailyRoutineRecord>> GetAllLedgers();

        public Task<IEnumerable<DailyRoutineRecord>> GetLedgersByIds(IEnumerable<int> ids);
    }
}
