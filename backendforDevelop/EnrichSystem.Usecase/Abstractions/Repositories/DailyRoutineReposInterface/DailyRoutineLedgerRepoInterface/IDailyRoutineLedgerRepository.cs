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
        public Task CreateNewLedger(DailyRoutineLedger ledger);

        public Task BulkInsertDailyRoutineRecord(List<DailyRoutineLedger> records);
        public Task DeleteLedger(DailyRoutineLedger ledger);

        public Task UpdateLedger(DailyRoutineLedger ledger);

        public Task<IEnumerable<DailyRoutineLedger>> GetAllLedgers();

        public Task<IEnumerable<DailyRoutineLedger>> GetLedgersByIds(IEnumerable<int> ids);
    }
}
