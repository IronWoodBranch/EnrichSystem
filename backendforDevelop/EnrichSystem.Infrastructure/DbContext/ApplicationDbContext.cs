using EnrichSystem.Domain.Ledgers;
using EnrichSystem.Domain.QuestCompletes;
using EnrichSystem.Domain.Quests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnrichSystem.Infrastructure.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Quest> Quests { get; set; }
        public DbSet<QuestComplete> QuestCompletes{ get; set; }
        public DbSet<Ledger> Ledgers {  get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
