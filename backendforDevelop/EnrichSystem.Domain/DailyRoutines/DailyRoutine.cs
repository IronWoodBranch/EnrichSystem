using EnrichSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Domain.DailyRoutines
{
    public class DailyRoutine
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 任务名
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// 任务介绍
        /// </summary>

        public string Lable { get; set; } = string.Empty;

        /// <summary>
        /// 输入类型
        /// </summary>
        public string InputType { get; set; } = string.Empty;

        /// <summary>
        /// 排序order
        /// todo:前端按照order分配先后
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 奖励类型
        /// </summary>
        public int currencyType { get; set; }

        /// <summary>
        /// 是否含有特殊规则
        /// </summary>

        public bool HasSpecialRule { get; set; }

        /// <summary>
        /// 完成得奖励
        /// </summary>
        public double CompleteReward { get; set; }

        /// <summary>
        /// 失败的惩罚
        /// </summary>
        public double FailedPunish { get; set; }


    }
}
