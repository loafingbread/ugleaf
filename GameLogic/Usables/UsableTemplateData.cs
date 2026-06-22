namespace GameLogic.Usables;

using GameLogic.Targeting;
using GameLogic.Usables.Effects;

public record UsableTemplateData
{
    public required string Id { get; init; }
    public required TargeterRecord Targeter { get; init; }
    public List<EffectTemplateData> Effects { get; init; } = new();
}

public record UsableTemplatePatch
{
    public string? Id { get; init; }
    public TargeterRecord? Targeter { get; init; }
    public List<EffectTemplateData>? Effects { get; init; }

    public UsableTemplateData ApplyTo(UsableTemplateData baseData) => baseData with
    {
        Id = Id ?? baseData.Id,
        Targeter = Targeter ?? baseData.Targeter,
        Effects = Effects ?? baseData.Effects,
    };
}
