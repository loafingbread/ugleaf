namespace GameLogic.Entities.Stats;

public interface IStatBlockRecord
{
    public List<StatRecord> ValueStats { get; }
}

public record StatBlockRecord : IStatBlockRecord
{
    public required List<StatRecord> ValueStats { get; init; } = new();
}

public class StatBlockConfig
{
    public required List<StatConfig> Stats { get; init; } = new();

    [SetsRequiredMembers]
    public StatBlockConfig(IStatBlockRecord record)
    {
        foreach (StatRecord statRecord in record.Stats)
        {
            StatConfig statConfig = new(statRecord);
            this.Stats.Add(statConfig);
        }
    }
}
