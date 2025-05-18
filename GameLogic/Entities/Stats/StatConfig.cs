namespace GameLogic.Entities.Stats;

public interface IStatRecord
{
    public string Id { get; }
    public int BaseValue { get; }
    public int CurrentValue { get; }
}

public record StatRecord : IStatRecord
{
    public required string Id { get; init; }
    public required int BaseValue { get; init; }
    public required int CurrentValue { get; init; }
}

public interface IStatBlockRecord
{
    public List<IStatRecord> Stats { get; }
}

public record StatBlockRecord
{
    public required List<IStatRecord> Stats { get; init; } = new();
}
