using EnrichSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Infrastructure.DBModels.DailyRoutineRecord
{
    /// <summary>
    /// DailyRoutine要做个专门的入账
    /// </summary>
    public class DailyRoutineLedgerEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public int DailyRoutineId { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public string DailyRoutineName { get; set; }
        /// <summary>
        /// 该任务是否完成
        /// </summary>
        public bool IsCompleted { get; set; }
    }
}
