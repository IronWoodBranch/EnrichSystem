namespace EnrichSystem.Api;

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

public record UnlockState(bool N1Passed, bool AwsSaaPassed, bool WeightLt62_5, bool ReadingDone)
{
    public bool GuitarUnlocked => N1Passed && AwsSaaPassed;
    public bool PeripheralUnlocked => WeightLt62_5 && ReadingDone;
}

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

public record RulebookDto(decimal CopperAnchorYen, decimal AmberBaseHours, decimal GuitarFundRate, decimal ReturnHomeTarget);

public record CreateQuestRequest(string Title, string FlavorText, CurrencyType Currency, decimal Reward, QuestKind Kind);
public record CreateShopItemRequest(string Name, CurrencyType Currency, decimal Price, bool RequiresAwsAndN1, bool RequiresWeightAndReading);
public record ChronicleEntryRequest(string Text, DateTimeOffset? At);
public record UpdatePhaseRequest(ProgressPhase Phase);
public record UpdateUnlockRequest(bool? N1Passed, bool? AwsSaaPassed, bool? WeightLt62_5, bool? ReadingDone);
public record ConvertCopperRequest(decimal CopperAmount);
public record AmnestyCheckRequest(DateTimeOffset WorkEndedAt, int StudyMinutes);

public record QuestCompleteResponse(Guid QuestId, string Headline, string RewardText, string FxHint, DashboardDto Dashboard);
public record ConvertResponse(decimal SpentCopper, decimal AddedFund, DashboardDto Dashboard);
public record AmnestyCheckResponse(bool Triggered, string Message, decimal AmberReward);
