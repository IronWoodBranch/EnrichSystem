using System.Globalization;

namespace EnrichSystem.Api;

public class RebootStore
{
    public const decimal CopperAnchorYen = 200m;
    public const decimal GuitarFundRate = 500m;
    public const decimal ReturnHomeTarget = 450m;

    public decimal Copper { get; private set; } = 12;
    public decimal Amber { get; private set; } = 8;
    public decimal GuitarFund { get; private set; }
    public ProgressPhase Phase { get; set; } = ProgressPhase.P2;
    public UnlockState Unlocks { get; private set; } = new(false, false, false, false);

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
        new("熬夜券", CurrencyType.Amber, 30),
        new("大餐券", CurrencyType.Amber, 20),
        new("电吉他（本体）", CurrencyType.Amber, 180) { RequiresAwsAndN1 = true },
        new("吉他周边（音箱/效果器）", CurrencyType.Amber, 120) { RequiresWeightAndReading = true },
        new("回国旅游门票", CurrencyType.Amber, ReturnHomeTarget)
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
        ReturnHomeTarget,
        Math.Clamp(Amber / ReturnHomeTarget * 100, 0, 100),
        Phase,
        CurrentMultiplier,
        Unlocks
    );

    public RulebookDto GetRulebook() => new(CopperAnchorYen, 1, GuitarFundRate, ReturnHomeTarget);

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
        if (quest.Kind == QuestKind.Bounty && quest.Completed) return null;

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
            $"+{reward.ToString("0.0", CultureInfo.InvariantCulture)} {quest.Currency}",
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

        var lockedByGuitar = item.RequiresAwsAndN1 && !Unlocks.GuitarUnlocked;
        var lockedByPeripheral = item.RequiresWeightAndReading && !Unlocks.PeripheralUnlocked;
        if (lockedByGuitar || lockedByPeripheral) return BuyResult.Locked;

        if (!CanAfford(item.Currency, item.Price)) return BuyResult.InsufficientFunds;

        AddCurrency(item.Currency, -item.Price);
        Log($"购买：{item.Name}", item.Currency, -item.Price);
        return BuyResult.Success;
    }

    public ChronicleEntry AddChronicle(string text, DateTimeOffset? at)
    {
        var entry = new ChronicleEntry(Guid.NewGuid(), at ?? DateTimeOffset.Now, text, CurrencyType.Copper, 0);
        Chronicle.Add(entry);
        return entry;
    }

    public UnlockState UpdateUnlock(UpdateUnlockRequest request)
    {
        Unlocks = Unlocks with
        {
            N1Passed = request.N1Passed ?? Unlocks.N1Passed,
            AwsSaaPassed = request.AwsSaaPassed ?? Unlocks.AwsSaaPassed,
            WeightLt62_5 = request.WeightLt62_5 ?? Unlocks.WeightLt62_5,
            ReadingDone = request.ReadingDone ?? Unlocks.ReadingDone
        };

        Log("更新解锁树状态", CurrencyType.Amber, 0,
            $"N1={Unlocks.N1Passed},AWS={Unlocks.AwsSaaPassed},Weight={Unlocks.WeightLt62_5},Read={Unlocks.ReadingDone}");

        return Unlocks;
    }

    public ConvertResponse ConvertCopperToFund(ConvertCopperRequest request)
    {
        if (request.CopperAmount <= 0) return new ConvertResponse(0, 0, GetDashboard());
        if (Copper < request.CopperAmount) return new ConvertResponse(0, 0, GetDashboard());

        Copper -= request.CopperAmount;
        var fundAdded = decimal.Round(request.CopperAmount / GuitarFundRate, 2);
        GuitarFund += fundAdded;

        Log("铜币存入吉他基金", CurrencyType.Copper, -request.CopperAmount,
            $"基金 +{fundAdded}");

        return new ConvertResponse(request.CopperAmount, fundAdded, GetDashboard());
    }

    public AmnestyCheckResponse CheckAmnesty(AmnestyCheckRequest request)
    {
        var triggered = request.WorkEndedAt.Hour >= 22 && request.StudyMinutes >= 5;
        if (!triggered)
        {
            return new AmnestyCheckResponse(false, "未触发特赦令：需22:00后且学习至少5分钟。", 0);
        }

        AddCurrency(CurrencyType.Amber, 1);
        Log("触发特赦令", CurrencyType.Amber, 1, "22:00后坚持学习≥5分钟");
        return new AmnestyCheckResponse(true, "特赦令生效：今日打卡成功且不扣分。", 1);
    }

    public void Log(string reason, CurrencyType currency, decimal delta, string? flavor = null)
    {
        Chronicle.Add(new ChronicleEntry(
            Guid.NewGuid(),
            DateTimeOffset.Now,
            string.IsNullOrWhiteSpace(flavor) ? reason : $"{reason}｜{flavor}",
            currency,
            delta));
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
