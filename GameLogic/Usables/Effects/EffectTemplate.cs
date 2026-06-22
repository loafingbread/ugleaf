namespace GameLogic.Usables.Effects;

using GameLogic.Registry;

public class EffectTemplate : IToData<EffectTemplateData>
{
    private readonly EffectTemplateData _data;

    public ReferenceId Id { get; }
    public string Type => _data.Type;
    public string Subtype => _data.Subtype;
    public string? VariantName => _data.Variant;
    public IEffectVariant EffectVariant { get; }

    public EffectTemplate(ReferenceId id, EffectTemplateData data, IEffectVariant variant)
    {
        Id = id;
        _data = data;
        EffectVariant = variant;
    }

    public EffectTemplateData ToData() => _data;
}
