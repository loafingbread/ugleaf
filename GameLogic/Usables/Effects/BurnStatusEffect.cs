namespace GameLogic.Usables.Effects;

using GameLogic.Registry;

public class BurnStatusEffect : StatusEffect
{
    public BurnStatusEffect(
        InstanceId id,
        TemplateId templateId,
        EEffectType type,
        string subtype,
        string variant,
        string name,
        string description,
        List<string> tags,
        int value,
        int duration
    )
        : base(id, templateId, type, subtype, variant, name, description, tags, value, duration) { }

    public BurnStatusEffect(EffectTemplate template)
        : base(template) { }

    public BurnStatusEffect(BurnStatusEffect burnStatusEffect)
        : base(burnStatusEffect) { }

    public override IEffect DeepCopy()
    {
        return new BurnStatusEffect(this);
    }
}
