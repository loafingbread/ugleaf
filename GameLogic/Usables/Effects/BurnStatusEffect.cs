namespace GameLogic.Usables.Effects;

using GameLogic.Registry;

public class BurnStatusEffect : StatusEffect
{
    public BurnStatusEffect(
        InstanceId id,
        TemplateIdentifier templateIdentifier,
        EEffectType type,
        string subtype,
        string variant,
        string name,
        string description,
        List<string> tags,
        int value,
        int duration
    )
        : base(id, templateIdentifier, type, subtype, variant, name, description, tags, value, duration) { }

    public BurnStatusEffect(EffectTemplate template)
        : base(template) { }

    public BurnStatusEffect(BurnStatusEffect burnStatusEffect)
        : base(burnStatusEffect) { }

    public override IEffect DeepCopy()
    {
        return new BurnStatusEffect(this);
    }
}
