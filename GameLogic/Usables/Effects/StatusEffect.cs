namespace GameLogic.Usables.Effects;

using GameLogic.Registry;

public abstract class StatusEffect : Effect
{
    public StatusEffect(
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

    public StatusEffect(EffectTemplate template)
        : base(template) { }

    public StatusEffect(StatusEffect statusEffect)
        : base(statusEffect) { }

    public abstract override IEffect DeepCopy();
}
