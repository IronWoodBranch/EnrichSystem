using System.Text.Json.Serialization;
using EnrichSystem.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(opts =>
{
    opts.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddCors(opts =>
{
    opts.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();
app.UseCors();

var store = new RebootStore();

app.MapGet("/api/dashboard", () => store.GetDashboard());
app.MapGet("/api/rules", () => store.GetRulebook());

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

app.MapPost("/api/unlocks", (UpdateUnlockRequest request) =>
{
    var unlock = store.UpdateUnlock(request);
    return Results.Ok(unlock);
});

app.MapPost("/api/fund/convert", (ConvertCopperRequest request) =>
{
    var result = store.ConvertCopperToFund(request);
    if (result.SpentCopper <= 0) return Results.BadRequest(new { message = "铜币不足或参数无效" });
    return Results.Ok(result);
});

app.MapPost("/api/amnesty/check", (AmnestyCheckRequest request) =>
{
    var result = store.CheckAmnesty(request);
    return Results.Ok(result);
});

app.Run();
