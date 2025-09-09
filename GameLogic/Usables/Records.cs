using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables.Effects;

namespace GameLogic.Usables;

public record UsableTemplateRecord
{
    public required string Name { get; init; } = "";
    public required string Description { get; init; } = "";
    public required List<string> Tags { get; init; } = new();
    public required TargeterRecord Targeter { get; init; }
    public required List<EffectRecord> Effects { get; init; } = new();
}

public record UsableOverrideRecord : ITemplateOverride<UsableTemplateRecord>
{
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public List<string> Tags { get; init; } = new();
    public TargeterRecord? Targeter { get; init; }
    public List<EffectRecord> Effects { get; init; } = new();
}

public record UsableRecord : UsableTemplateRecord
{
    public string InstanceId { get; init; } = "";
}
