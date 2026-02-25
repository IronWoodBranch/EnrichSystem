using EnrichSystem.Domain.Ledgers;
using EnrichSystem.Infrastructure.DbContexts;
using EnrichSystem.Usecase.Interfaces.Repositories.LedgerRepoInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Infrastructure.Repositories.LedgersRepo
{
    public class LegerRepository : ILedgerRepository
    {
        private readonly ApplicationDbContext _context;
        public LegerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Ledger> CreateLedger(Ledger createObj)
        {
            _context.Ledgers.Add(createObj);
            await _context.SaveChangesAsync();
            return createObj;
        }

        public async Task<Ledger> DeleteLedger(Ledger deleteObj)
        {
            _context.Ledgers.Remove(deleteObj);
            await _context.SaveChangesAsync();
            return deleteObj;
        }

        public async Task<IEnumerable<Ledger>> GetAllLedgers(int id)
        {
            //本地系统，先全量查询，后续再加条件
            var result = _context.Ledgers.ToList();
            return result;
        }

        public async Task<Ledger> GetLedgerById(int id)
        {
            var result = _context.Ledgers.FirstOrDefault(x => x.Id == id);
            return result;
        }

        public async Task<Ledger> UpdateLedger(Ledger updateObj)
        {
            var result = _context.Ledgers.FirstOrDefault(x => x.Id == updateObj.Id);
            if (result != null)
            {
                result.Date = updateObj.Date;
                result.Description = updateObj.Description;
                result.Amount = updateObj.Amount;
                result.CurrencyType = updateObj.CurrencyType;
                result.QuestCompleteId = updateObj.QuestCompleteId;
                await _context.SaveChangesAsync();
            }
            return updateObj;
        }
    }
}
