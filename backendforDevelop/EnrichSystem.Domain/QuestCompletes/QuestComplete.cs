using EnrichSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnrichSystem.Domain.QuestCompletes
{
    public class QuestComplete
    {
        public int Id { get; set; }
        public int QuestDefinationId { get; set; }
        public DateTime CompletedAt { get; set; }

        /// <summary>
        /// 任务完成的描述
        /// </summary>
        public string? Chronicle { get; set; }

        public int Amount { get; set; }
        public CurrencyType CurrencyType { get; set; }

        public int Condition { get; set; } = 0;

    }
}
