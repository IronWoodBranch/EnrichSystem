using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(opts =>
{
    opts.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddCors(opts =>
{
    opts.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();
app.UseCors();

var store = new RebootStore();

app.MapGet("/api/dashboard", () => store.GetDashboard());

app.MapGet("/api/chronicle", () => store.Chronicle.OrderByDescending(x => x.At));
app.MapPost("/api/chronicle", (ChronicleEntryRequest request) =>
{
    var entry = store.AddChronicle(request.Text, request.At);
    return Results.Created($"/api/chronicle/{entry.Id}", entry);
});

app.MapGet("/api/quests", () => store.Quests);
app.MapPost("/api/quests", (CreateQuestRequest request) =>
{
    var quest = store.CreateQuest(request);
    return Results.Created($"/api/quests/{quest.Id}", quest);
});
app.MapPost("/api/quests/{id:guid}/complete", (Guid id) =>
{
    var result = store.CompleteQuest(id);
    return result is null ? Results.NotFound() : Results.Ok(result);
});

app.MapGet("/api/shop", () => store.ShopItems);
app.MapPost("/api/shop", (CreateShopItemRequest request) =>
{
    var item = store.CreateShopItem(request);
    return Results.Created($"/api/shop/{item.Id}", item);
});
app.MapPost("/api/shop/{id:guid}/buy", (Guid id) =>
{
    var result = store.Buy(id);
    return result switch
    {
        BuyResult.NotFound => Results.NotFound(),
        BuyResult.InsufficientFunds => Results.BadRequest(new { message = "余额不足" }),
        BuyResult.Locked => Results.BadRequest(new { message = "尚未解锁购买条件" }),
        _ => Results.Ok(store.GetDashboard())
    };
});

app.MapPost("/api/phase", (UpdatePhaseRequest request) =>
{
    store.Phase = request.Phase;
    store.Log($"系统切换至 {request.Phase}，倍率 {store.CurrentMultiplier}", CurrencyType.Amber, 0);
    return Results.Ok(store.GetDashboard());
});

app.Run();

public class RebootStore
{
    public decimal Copper { get; private set; } = 12;
    public decimal Amber { get; private set; } = 8;
    public decimal GuitarFund { get; private set; } = 0;
    public ProgressPhase Phase { get; set; } = ProgressPhase.P2;

    public List<Quest> Quests { get; } =
    [
        new("吃青蛙", "把今天最痛苦的工作先砍掉。", CurrencyType.Copper, 2, QuestKind.Daily),
        new("日语绝境坚持", "22:30 后，仍然坐下学习 5 分钟。", CurrencyType.Amber, 1, QuestKind.Daily),
        new("东京退房 Boss", "处理搬家退房手续与押金交涉。", CurrencyType.Amber, 3, QuestKind.Bounty)
    ];

    public List<ShopItem> ShopItems { get; } =
    [
        new("蒸汽眼罩", CurrencyType.Copper, 2),
        new("懒惰券", CurrencyType.Amber, 60),
        new("回国旅游门票", CurrencyType.Amber, 450)
    ];

    public List<ChronicleEntry> Chronicle { get; } = [];

    public decimal CurrentMultiplier => Phase switch
    {
        ProgressPhase.P2 => 1.0m,
        ProgressPhase.P3 => 1.5m,
        _ => 2.0m
    };

    public DashboardDto GetDashboard() => new(
        Copper,
        Amber,
        GuitarFund,
        450,
        Math.Clamp(Amber / 450m * 100, 0, 100),
        Phase,
        CurrentMultiplier,
        new UnlockState(
            N1Passed: false,
            AwsSaaPassed: false,
            WeightLt62_5: false,
            ReadingDone: false
        )
    );

    public Quest CreateQuest(CreateQuestRequest request)
    {
        var quest = new Quest(request.Title, request.FlavorText, request.Currency, request.Reward, request.Kind);
        Quests.Add(quest);
        return quest;
    }

    public QuestCompleteResponse? CompleteQuest(Guid id)
    {
        var quest = Quests.FirstOrDefault(q => q.Id == id);
        if (quest is null) return null;

        var reward = decimal.Round(quest.Reward * CurrentMultiplier, 1);
        AddCurrency(quest.Currency, reward);
        quest.CompletionCount++;
        if (quest.Kind == QuestKind.Bounty)
        {
            quest.Completed = true;
        }

        Log($"完成任务：{quest.Title}", quest.Currency, reward, quest.FlavorText);

        return new QuestCompleteResponse(
            quest.Id,
            $"✨ Quest Complete: {quest.Title}",
            $"+{reward} {quest.Currency}",
            "播放胜利音效（前端可接入）",
            GetDashboard());
    }

    public ShopItem CreateShopItem(CreateShopItemRequest request)
    {
        var item = new ShopItem(request.Name, request.Currency, request.Price)
        {
            RequiresAwsAndN1 = request.RequiresAwsAndN1,
            RequiresWeightAndReading = request.RequiresWeightAndReading
        };
        ShopItems.Add(item);
        return item;
    }

    public BuyResult Buy(Guid id)
    {
        var item = ShopItems.FirstOrDefault(i => i.Id == id);
        if (item is null) return BuyResult.NotFound;

        if (item.RequiresAwsAndN1 || item.RequiresWeightAndReading)
        {
            return BuyResult.Locked;
        }

        if (!CanAfford(item.Currency, item.Price)) return BuyResult.InsufficientFunds;
        AddCurrency(item.Currency, -item.Price);

        if (item.Name.Contains("吉他基金"))
        {
            GuitarFund += item.Price / 500m;
        }

        Log($"购买：{item.Name}", item.Currency, -item.Price);
        return BuyResult.Success;
    }

    public ChronicleEntry AddChronicle(string text, DateTimeOffset? at)
    {
        var entry = new ChronicleEntry(Guid.NewGuid(), at ?? DateTimeOffset.Now, text, CurrencyType.Copper, 0);
        Chronicle.Add(entry);
        return entry;
    }

    public void Log(string reason, CurrencyType currency, decimal delta, string? flavor = null)
    {
        Chronicle.Add(new ChronicleEntry(Guid.NewGuid(), DateTimeOffset.Now,
            string.IsNullOrWhiteSpace(flavor) ? reason : $"{reason}｜{flavor}", currency, delta));
    }

    private bool CanAfford(CurrencyType currency, decimal amount) => currency switch
    {
        CurrencyType.Copper => Copper >= amount,
        CurrencyType.Amber => Amber >= amount,
        _ => false
    };

    private void AddCurrency(CurrencyType currency, decimal delta)
    {
        if (currency == CurrencyType.Copper) Copper += delta;
        if (currency == CurrencyType.Amber) Amber += delta;
    }
}

public enum CurrencyType { Copper, Amber }
public enum QuestKind { Daily, Bounty }
public enum ProgressPhase { P2, P3, P4 }
public enum BuyResult { Success, NotFound, InsufficientFunds, Locked }

public record DashboardDto(
    decimal Copper,
    decimal Amber,
    decimal GuitarFund,
    decimal ReturnHomeTarget,
    decimal ReturnHomeProgressPercent,
    ProgressPhase Phase,
    decimal Multiplier,
    UnlockState Unlocks);

public record UnlockState(bool N1Passed, bool AwsSaaPassed, bool WeightLt62_5, bool ReadingDone);

public record ChronicleEntry(Guid Id, DateTimeOffset At, string Text, CurrencyType Currency, decimal Delta);

public class Quest(string title, string flavorText, CurrencyType currency, decimal reward, QuestKind kind)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; set; } = title;
    public string FlavorText { get; set; } = flavorText;
    public CurrencyType Currency { get; set; } = currency;
    public decimal Reward { get; set; } = reward;
    public QuestKind Kind { get; set; } = kind;
    public bool Completed { get; set; }
    public int CompletionCount { get; set; }
}

public class ShopItem(string name, CurrencyType currency, decimal price)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public CurrencyType Currency { get; set; } = currency;
    public decimal Price { get; set; } = price;
    public bool RequiresAwsAndN1 { get; set; }
    public bool RequiresWeightAndReading { get; set; }
}

public record CreateQuestRequest(string Title, string FlavorText, CurrencyType Currency, decimal Reward, QuestKind Kind);
public record CreateShopItemRequest(string Name, CurrencyType Currency, decimal Price, bool RequiresAwsAndN1, bool RequiresWeightAndReading);
public record ChronicleEntryRequest(string Text, DateTimeOffset? At);
public record UpdatePhaseRequest(ProgressPhase Phase);
public record QuestCompleteResponse(Guid QuestId, string Headline, string RewardText, string FxHint, DashboardDto Dashboard);
