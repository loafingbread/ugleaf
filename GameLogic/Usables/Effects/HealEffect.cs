namespace GameLogic.Usables.Effects;

using GameLogic.Registry;

public class HealEffect : Effect
{
    public HealEffect(
        InstanceId id,
        TemplateIdentifier templateIdentifier,
        EEffectType type,
        string subtype,
        string variant,
        string name,
        string description,
        List<string> tags,
        int value
    )
        : base(id, templateIdentifier, type, subtype, variant, name, description, tags, value, 0) { }

    public HealEffect(EffectTemplate template)
        : base(template) { }

    public HealEffect(HealEffect healEffect)
        : base(healEffect) { }

    public override IEffect DeepCopy()
    {
        return new HealEffect(this);
    }
}
