namespace GameLogic.Registry;

using System.Text.Json;
using GameLogic.Entities.Characters;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats.Stat;
using GameLogic.Actions.Usables;
using GameLogic.Actions.Effects;

public record Ref
{
    public required ReferenceMetadata ReferenceMetadata { get; init; }
}

public record InstanceRef<TState> : Ref
{
    public required TState? InstanceState { get; init; }
}

public record TemplateRefBase : Ref
{
    public required ETemplateKind TemplateKind { get; init; }
}

public record ReferenceMetadata
{
    // The kind of reference to create: pure reference, override, inline, instance
    public required EReferenceKind ReferenceKind { get; init; }

    // The type to create a reference for: character, skill, usable, effect, stat
    public required EEntityType EntityType { get; init; }

    // Used to identify the reference in the registry
    public ReferenceId ReferenceId { get; init; } = Ids.NewReferenceId();

    // Used to identify the dependency in the registry. The dependency
    // needs to be the same entity type as reference and will be a template.
    public ReferenceId? DependencyId { get; init; } = null;
};

public record TemplateRef<TValue, TPatch> : TemplateRefBase
{
    // The kind of reference to create: pure reference, override, inline, instance
    public required TValue? TemplateValue { get; init; }
    public required TPatch? TemplatePatch { get; init; }
}

/// <summary>
/// How a reference union spec is to be interpreted.
/// </summary>
public enum ETemplateKind
{
    // A pure reference to a template. No overrides are allowed.
    Ref,

    // A reference to a template with an override.
    Override,

    // An inline definition of a template.
    Inline,
}

public static class TypeMaps
{
    public static (Type ValueType, Type PatchType) GetReferenceTypes(EEntityType templateType)
    {
        if (EEntityType.Character == templateType)
        {
            return (typeof(CharacterTemplateSpec), typeof(CharacterOverrideSpec));
        }
        else if (EEntityType.Skill == templateType)
        {
            return (typeof(SkillTemplateSpec), typeof(SkillOverrideSpec));
        }
        else if (EEntityType.Usable == templateType)
        {
            return (typeof(UsableTemplateSpec), typeof(UsableOverrideSpec));
        }
        else if (EEntityType.Effect == templateType)
        {
            return (typeof(EffectTemplateSpec), typeof(EffectOverrideSpec));
        }
        else if (EEntityType.Stat == templateType)
        {
            return (typeof(StatData), typeof(StatPatch));
        }

        throw new InvalidOperationException($"Invalid template type: {templateType}");
    }
}

public sealed record BuildContext(IRegistry Registry);
