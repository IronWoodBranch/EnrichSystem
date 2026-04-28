using EnrichSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Domain.DailyRoutines.DailyRoutineRecord
{
    /// <summary>
    /// 
    /// </summary>
    public class DailyRoutineLedger
    {
        public int Id { get; set; }
        public int DailyRoutineId { get; set; }
        public string Name { get; set; }

        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        /// <summary>
        /// 最终执行结果
        /// </summary>
        public double LedgerAmount { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public double CompleteReward { get; set; }
        public double FailedPunish { get; set; }
        public DailyRoutineLedger() { }

        public DailyRoutineLedger( string name, int dailyRoutineId)
        {
            Name = name;
            DailyRoutineId = dailyRoutineId;
        }

        public void Complete(bool isComplete)
        {
            IsCompleted = isComplete;
            if (isComplete)
            {
                LedgerAmount = CompleteReward;
            }
            else
            {
                FailedPunish = CompleteReward;
            }
            Date = DateTime.Now;
        }
    }
}