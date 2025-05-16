using System.Diagnostics.CodeAnalysis;
using GameLogic.Targeting;
using GameLogic.Usables.Effects;

namespace GameLogic.Usables;

public interface IUsableRecord
{
    public string Id { get; }
    public TargeterRecord Targeter { get; }
    public List<EffectRecord> Effects { get; }
}

public record UsableRecord : IUsableRecord
{
    public required string Id { get; init; }
    public required TargeterRecord Targeter { get; init; }
    public required List<EffectRecord> Effects { get; init; } = new();
}

public interface IUsableConfig
{
    public string Id { get; }
    public TargeterConfig Targeter { get; }
    public List<IEffect> Effects { get; }
}

public class UsableConfig : IUsableConfig
{
    public required string Id { get; init; }
    public required TargeterConfig Targeter { get; init; }
    public required List<IEffect> Effects { get; init; } = new();

    [SetsRequiredMembers]
    public UsableConfig(IUsableRecord record)
    {
        this.Id = record.Id;
        this.Targeter = new TargeterConfig(record.Targeter);

        foreach (EffectRecord effectRecord in record.Effects)
        {
            IEffect effect = EffectFactory.CreateFromRecord(effectRecord);
            this.Effects.Add(effect);
        }
    }
}
