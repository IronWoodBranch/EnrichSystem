using EnrichSystem.Domain.DailyRoutines.DailyRoutineRecord;
using EnrichSystem.Infrastructure.DbContexts;
using EnrichSystem.Infrastructure.DBModels.DailyRoutineRecord;
using EnrichSystem.Usecase.Interfaces.Repositories.DailyRoutineReposInterface.DailyRoutineLedgerRepoInterface;
using Microsoft.EntityFrameworkCore;
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


        public async Task BulkInsertDailyRoutineRecord(List<DailyRoutineLedger> records)
        {
            var objList = records.Select(ledger => new DailyRoutineLedgerEntity
            {
                Date = ledger.Date,
                Amount = ledger.LedgerAmount,
                DailyRoutineId = ledger.DailyRoutineId,
                DailyRoutineName = ledger.Name,
                IsCompleted = ledger.IsCompleted
            }).ToList();
            _context.DailyRoutineRecords.AddRange(objList);
            await _context.SaveChangesAsync();
        }

        public async Task CreateNewLedger(DailyRoutineLedger record)
        {
            var obj = new DailyRoutineLedgerEntity
            {
                Date = record.Date,
                Amount = record.LedgerAmount,
                DailyRoutineId = record.DailyRoutineId,
                DailyRoutineName = record.Name,
                IsCompleted = record.IsCompleted
            };
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public Task DeleteLedger(DailyRoutineLedger ledger)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DailyRoutineLedger>> GetAllLedgers()
        {
            var dbrecords = await _context.DailyRoutineRecords.ToListAsync();
            List<DailyRoutineLedger> res = new List<DailyRoutineLedger>();
            foreach(var i in dbrecords)
            {
                var singleRecrod = new DailyRoutineLedger
                {
                    Id = i.Id,
                    DailyRoutineId = i.DailyRoutineId,
                    Name = i.DailyRoutineName,
                    Date = i.Date,
                    IsCompleted = i.IsCompleted,
                    LedgerAmount = i.Amount,
                };
                res.Add(singleRecrod);
            }
            return res;
        }

        public async Task<IEnumerable<DailyRoutineLedger>> GetLedgersByDate(DateTime date)
        {
            var dbrecords = await _context.DailyRoutineRecords.Where(r => r.Date.Date >= date.Date).ToListAsync();
            return dbrecords.Select(i => new DailyRoutineLedger
            {
                Id = i.Id,
                DailyRoutineId = i.DailyRoutineId,
                Name = i.DailyRoutineName,
                Date = i.Date,
                IsCompleted = i.IsCompleted,
                LedgerAmount = i.Amount,
                CurrencyType = i.CurrencyType,
            });
        }

        public Task<IEnumerable<DailyRoutineLedger>> GetLedgersByIds(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLedger(DailyRoutineLedger ledger)
        {
            throw new NotImplementedException();
        }
    }
}
