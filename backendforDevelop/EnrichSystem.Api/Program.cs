using EnrichSystem.Infrastructure.DbContexts;
using EnrichSystem.Infrastructure.Repositories.LedgersRepo;
using EnrichSystem.Infrastructure.Repositories.QuestsCompleteRepo;
using EnrichSystem.Infrastructure.Repositories.QuestsRepo;
using EnrichSystem.Usecase.Implementation.LedgerImp;
using EnrichSystem.Usecase.Implementation.QuestCompleteImp;
using EnrichSystem.Usecase.Implementation.QuestImp;
using EnrichSystem.Usecase.Interfaces.Repositories.LedgerRepoInterface;
using EnrichSystem.Usecase.Interfaces.Repositories.QuestCompletes;
using EnrichSystem.Usecase.Interfaces.Repositories.Quests;
using EnrichSystem.Usecase.Interfaces.UseCase.LedgerInterface;
using EnrichSystem.Usecase.Interfaces.UseCase.QuestCompleteInterface;
using EnrichSystem.Usecase.Interfaces.UseCase.QuestsInterface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger/OpenAPI（这两行要配套）
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core
var connectionString =
    builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(connectionString));

builder.Services.AddScoped<IQuestRepository, QuestRepository>();
builder.Services.AddScoped<IQuestUsecase, QuestUsecase>();
builder.Services.AddScoped<IQuestCompleteRepository,QuestCompleteRepository>();
builder.Services.AddScoped<IQuestCompleteUsecase, QuestCompleteUsecase>();
builder.Services.AddScoped<ILedgerRepository, LegerRepository>();
builder.Services.AddScoped<ILedgerUsecase, LedgerUsecase>();

var app = builder.Build();

// Swagger only in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();