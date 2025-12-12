namespace GameLogic.Usables.Effects;

using GameLogic.Registry;
using GameLogic.Targeting;

public class AttackEffect : Effect
{
    public AttackEffect(
        ReferenceMetadata referenceMetadata,
        InstanceId id,
        EffectInstanceSpec? instanceState
    )
        : base(referenceMetadata, id, instanceState) { }

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
