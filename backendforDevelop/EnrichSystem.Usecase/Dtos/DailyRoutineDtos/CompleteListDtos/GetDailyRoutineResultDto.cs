using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Dtos.DailyRoutineDtos.CompleteListDtos
{
    public class GetDailyRoutineResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public double amount { get; set; }
    }
}
