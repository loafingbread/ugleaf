using GameLogic.Targeting;
using GameLogic.Usables.Effects;

namespace GameLogic.Usables;

public record UsableTemplateRecord
{
    public required string TemplateId { get; init; }
    public string Name { get; init; } = "";
    public string Description { get; init; } = "";
    public List<string> Tags { get; init; } = new();
    public required TargeterRecord Targeter { get; init; }
    public required List<EffectRecord> Effects { get; init; } = new();
}

public record UsableRecord : UsableTemplateRecord
{
    public string InstanceId { get; init; } = "";
}
