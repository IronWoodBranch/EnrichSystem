using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Dtos.DailyRoutineDtos.CompleteListDtos
{
    public class DailyRoutineCompleteResultDto
    {
        public List<DailyRoutineCompleteResultItem> Items { get; set; }
    }

    public class DailyRoutineCompleteResultItem() 
    { 
        public int Id { get; set; }
        public bool IsCompleted { get; set; }
        public string RoutineName { get; set; }
        public double Amount { get; set; }
    }
}
