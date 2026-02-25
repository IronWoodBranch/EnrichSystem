using EnrichSystem.Domain.Ledgers;
using EnrichSystem.Usecase.Interfaces.Repositories.LedgerRepoInterface;
using EnrichSystem.Usecase.Interfaces.UseCase.LedgerInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Implementation.LedgerImp
{
    public class LedgerUsecase : ILedgerUsecase
    {
        private readonly ILedgerRepository _ledgerRepository;
        public LedgerUsecase(ILedgerRepository ledgerRepository)
        {
            _ledgerRepository = ledgerRepository;
        }
        public async Task<Ledger> CreateLedger(Ledger createObj)
        {
            var result = await _ledgerRepository.CreateLedger(createObj);
            return result;
        }

        public async Task<Ledger> DeleteLedger(Ledger deleteObj)
        {
            var result = await _ledgerRepository.DeleteLedger(deleteObj);
            return result;
        }

        public async Task<IEnumerable<Ledger>> GetAllLedgers(int id)
        {
            var result = await _ledgerRepository.GetAllLedgers(id);
            return result;
        }

        public async Task<Ledger> GetLedgerById(int id)
        {
            var result = await _ledgerRepository.GetLedgerById(id);
            return result;
        }

        public async Task<Ledger> UpdateLedger(Ledger updateObj)
        {
            var result = await _ledgerRepository.UpdateLedger(updateObj);
            return result;
        }
    }
}
