using EnrichSystem.Domain.DailyRoutines.DailyRoutineRecord;
using EnrichSystem.Infrastructure.DbContexts;
using EnrichSystem.Infrastructure.DBModels.DailyRoutineRecord;
using EnrichSystem.Usecase.Interfaces.Repositories.DailyRoutineReposInterface.DailyRoutineLedgerRepoInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Infrastructure.Repositories.DailyRoutineRepo.DailyRoutineLedgerRepo
{
    public class DailyRoutineLedgerRepository : IDailyRoutineRecordRepository
    {
        private readonly ApplicationDbContext _context;
        public DailyRoutineLedgerRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task BulkInsertDailyRoutineRecord(List<DailyRoutineRecord> records)
        {
            var objList = records.Select(ledger => new DailyRoutineRecordEntity
            {
                Date = ledger.Date,
                Amount = ledger.LedgerAmount,
                DailyRoutineId = ledger.DailyRoutineId,
                DailyRoutineName = ledger.Name
            }).ToList();
            _context.DailyRoutineRecords.AddRange(objList);
            await _context.SaveChangesAsync();
        }

        public async Task CreateNewLedger(DailyRoutineRecord ledger)
        {
            var obj = new DailyRoutineRecordEntity
            {
                Date = ledger.Date,
                Amount = ledger.LedgerAmount,
                DailyRoutineId = ledger.DailyRoutineId,
                DailyRoutineName = ledger.Name
            };
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public Task DeleteLedger(DailyRoutineRecord ledger)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyRoutineRecord>> GetAllLedgers()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DailyRoutineRecord>> GetLedgersByIds(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLedger(DailyRoutineRecord ledger)
        {
            throw new NotImplementedException();
        }
    }
}
