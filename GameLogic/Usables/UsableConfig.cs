using System.Diagnostics.CodeAnalysis;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables.Effects;

namespace GameLogic.Usables;

public interface IUsableRecord
{
    public string Id { get; }
    public TargeterRecord Targeter { get; }
    public List<EffectTemplateData> Effects { get; }
}

public record UsableRecord : IUsableRecord
{
    public required string Id { get; init; }
    public required TargeterRecord Targeter { get; init; }
    public required List<EffectTemplateData> Effects { get; init; } = new();
}

public class UsableConfig
{
    public required string Id { get; init; }
    public required TargeterConfig Targeter { get; init; }
    public required List<EffectTemplate> Effects { get; init; } = new();

    private static readonly EffectTemplateFactory _effectFactory = new();

    [SetsRequiredMembers]
    public UsableConfig(IUsableRecord record)
    {
        this.Id = record.Id;
        this.Targeter = new TargeterConfig(record.Targeter);

        foreach (EffectTemplateData effectData in record.Effects)
        {
            this.Effects.Add(_effectFactory.Create(ReferenceId.New(), effectData));
        }
    }
}
