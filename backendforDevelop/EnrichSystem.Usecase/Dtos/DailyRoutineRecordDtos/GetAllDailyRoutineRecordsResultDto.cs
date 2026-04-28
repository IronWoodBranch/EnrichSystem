using EnrichSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Dtos.DailyRoutineRecordDtos
{
    public class GetAllDailyRoutineRecordsResultDto
    {
        public List<DailyRoutineRecordDetailsDto> DailyRoutineRecords { get; set; }
    }

    public class DailyRoutineRecordDetailsDto
    {
        public int Id { get; set; }
        public int DailyRoutineId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        public CurrencyType Currency { get; set; }
    }
}
