using EnrichSystem.Domain.Ledgers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Interfaces.Repositories.LedgerRepoInterface
{
    public interface ILedgerRepository
    {
        public Task<Ledger> CreateLedger(Ledger createObj);
        public Task<Ledger> UpdateLedger(Ledger updateObj);
        public Task<Ledger> DeleteLedger(Ledger deleteObj);

        /// <summary>
        /// 拿所有的流水记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IEnumerable<Ledger>> GetAllLedgers(int id);

        /// <summary>
        /// 根据Id拿单个Id记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Ledger> GetLedgerById(int id);
    }
}
