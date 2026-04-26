using EnrichSystem.Domain.DailyRoutines;
using EnrichSystem.Domain.Ledgers;
using EnrichSystem.Domain.QuestCompletes;
using EnrichSystem.Domain.Quests;
using EnrichSystem.Infrastructure.DBModels.DailyRoutineRecord;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnrichSystem.Infrastructure.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        //todo:把领域模型改为DBModel
        public DbSet<Quest> Quests { get; set; }
        //todo:把领域模型改为DBModel
        public DbSet<QuestComplete> QuestCompletes{ get; set; }
        //todo:把领域模型改为DBModel
        public DbSet<Ledger> Ledgers {  get; set; }
        //todo:把领域模型改为DBModel
        public DbSet<DailyRoutine> DailyRoutines { get; set; }
        //todo:把领域模型改为DBModel
        public DbSet<DailyRoutineRecordEntity> DailyRoutineRecords { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
