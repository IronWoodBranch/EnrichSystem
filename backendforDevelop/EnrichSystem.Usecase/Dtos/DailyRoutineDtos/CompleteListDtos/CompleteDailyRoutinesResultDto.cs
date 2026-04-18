using EnrichSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Usecase.Dtos.DailyRoutineDtos.CompleteListDtos
{
    public class CompleteDailyRoutinesResultDto
    {
        /// <summary>
        /// 总额：pp
        /// </summary>
        public double Platinum {  get; set; }
        /// <summary>
        /// 总额：cp
        /// </summary>
        public double Copper { get; set; }

        public List<CompleteDailyRoutineDetailsDto> RoutineDetails { get; set; }
    }

    public class CompleteDailyRoutineDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }

        public bool IsCompleted { get; set; }
        public CurrencyType CurrencyType { get; set; }

    }
}
