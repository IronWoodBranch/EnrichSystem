using System;
using System.Collections.Generic;
using System.Text;

namespace EnrichSystem.Domain.Ledgers
{
    public class Ledger
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public double Amount { get; set; }
        public int CurrencyType { get; set; }
        public int QuestCompleteId { get; set; }
    }
}
