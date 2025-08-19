namespace GameLogic.Entities.Stats;

using System.Diagnostics.CodeAnalysis;
using GameLogic.Config;

public class StatBlock : IConfigurable<StatBlockConfig>
{
    private StatBlockConfig _config { get; set; }
    private List<IStat> _stats { get; } = new();
    private List<StatModifier> _modifiers { get; } = new();

    [SetsRequiredMembers]
    public StatBlock(StatBlockConfig config)
    {
        this.ApplyConfig(config);
    }

    public void ApplyConfig(StatBlockConfig config)
    {
        this._config = config;

        this._stats.Clear();
        foreach (StatConfig statConfig in config.GetAll())
        {
            this._stats.Add((IStat)new Stat(statConfig));
        }
    }

    public StatBlockConfig GetConfig()
    {
        return this._config;
    }

    public List<StatModifier> GetModifiers()
    {
        return this._modifiers;
    }

    public StatModifier GetModifier(string id)
    {
        return this._modifiers.FirstOrDefault(modifier => modifier.Id == id, null);
    }

    public void AddModifier(StatModifier modifier)
    {
        this._modifiers.Add(modifier);
        IStat stat = this.GetStat(modifier.Id);
        stat.UpdateModifiedValue(modifier.Amount);
    }

    public void RemoveModifier(StatModifier modifier)
    {
        this._modifiers.Remove(modifier);
    }

    public IStat GetStat(string id)
    {
        return this._stats.FirstOrDefault(stat => stat.GetConfig().Id == id, null);
    }
}

public class StatModifier
{
    public string Id { get; }
    public int Amount { get; }

    public StatModifier(string id, int amount)
    {
        this.Id = id;
        this.Amount = amount;
    }
}

public static class StatBlockFactory
{
    public static StatBlock CreateFromRecord(StatBlockRecord record)
    {
        StatBlockConfig config = new StatBlockConfig(record);
        return new StatBlock(config);
    }

    public static Stat CreateStatFromRecord(IStatRecord record)
    {
        StatConfig config = new StatConfig(record);
        return new Stat(config);
    }
}
