using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables.Effects;

namespace GameLogic.Usables;

public record UsableTemplateSpec
{
    public required string Name { get; init; } = "";
    public required string Description { get; init; } = "";
    public required List<string> Tags { get; init; } = new();
    public required TargeterData Targeter { get; init; }
    public required List<ReferenceSpec> Effects { get; init; } = new();
}

public record UsableOverrideSpec : ITemplateOverride<UsableTemplateSpec>
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public List<string>? Tags { get; init; }
    public TargeterData? Targeter { get; init; }
    public List<ReferenceSpec>? Effects { get; init; }
}

public record UsableInstanceSpec : UsableTemplateSpec { }

public record UsableData
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required List<string> Tags { get; init; }

    public required TargeterData Targeter { get; init; }
    public required List<EffectData> Effects { get; init; } = new();
}