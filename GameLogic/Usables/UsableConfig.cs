using System.Diagnostics.CodeAnalysis;
using GameLogic.Targeting;
using GameLogic.Usables.Effects;

namespace GameLogic.Usables;

public record UsableRecord
{
    public required string Id { get; init; }
    public required TargeterRecord Targeter { get; init; }
    public required List<EffectRecord> Effects { get; init; } = new();
}

public class UsableConfig
{
    public required string Id { get; init; }
    public required TargeterConfig Targeter { get; init; }
    public required List<IEffect> Effects { get; init; } = new();

    [SetsRequiredMembers]
    public UsableConfig(UsableRecord record)
    {
        this.Id = record.Id;
        this.Targeter = new TargeterConfig(record.Targeter);

        foreach (EffectRecord effectRecord in record.Effects)
        {
            IEffect effect = EffectConfigFactory.CreateFromRecord(effectRecord);
            this.Effects.Add(effect);
        }
    }
}
