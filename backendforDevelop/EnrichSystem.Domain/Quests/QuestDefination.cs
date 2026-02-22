using EnrichSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnrichSystem.Domain.Quests
{
    public class Quest
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        
        public int rewardId { get; set; }

        public CurrencyType CurrencyType { get; set; }

        public int Amount { get; set; }
        public int Condition { get; set; }

    }
}
