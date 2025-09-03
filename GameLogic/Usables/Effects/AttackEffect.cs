namespace GameLogic.Usables.Effects;

using GameLogic.Registry;
using GameLogic.Targeting;

public class AttackEffect : Effect
{
    public AttackEffect(
        InstanceId id,
        TemplateId templateId,
        EEffectType type,
        string subtype,
        string variant,
        string name,
        string description,
        List<string> tags,
        int value
    )
        : base(id, templateId, type, subtype, variant, name, description, tags, value, 0) { }

    public AttackEffect(EffectTemplate template)
        : base(template) { }

    public AttackEffect(AttackEffect attackEffect)
        : base(attackEffect) { }

    public override IEffect DeepCopy()
    {
        return new AttackEffect(this);
    }

    public new EffectResult Apply(IUser user, ITargetable target)
    {
        return new EffectResult(
            this,
            user.GetEntity(),
            target.GetEntity(),
            this.Value,
            false,
            true,
            0
        );
    }
}
