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
    public class DailyRoutineRecordEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string DailyRoutineId { get; set; }
        public string DailyRoutineName { get; set; }
    }
}
