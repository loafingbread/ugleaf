namespace GameLogic.Actions.Effects;

using GameLogic.Registry;

/// <summary>
/// Reference type for an effect template.
/// TValue = EffectData (used directly as both JSON template content and resolved data).
/// TPatch = EffectPatch (used for Override kind).
/// </summary>
public record EffectTemplateRef : TemplateRef<EffectData, EffectPatch> { }
