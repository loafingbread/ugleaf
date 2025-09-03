namespace GameLogic.Usables.Effects;

using GameLogic.Registry;

public class PoisonStatusEffect : StatusEffect
{
    public PoisonStatusEffect(
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

    public PoisonStatusEffect(EffectTemplate template)
        : base(template) { }

    public PoisonStatusEffect(PoisonStatusEffect poisonStatusEffect)
        : base(poisonStatusEffect) { }

    public override IEffect DeepCopy()
    {
        return new PoisonStatusEffect(this);
    }
}
