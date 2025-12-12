namespace GameLogic.Usables.Effects;

using GameLogic.Registry;

public class PoisonStatusEffect : StatusEffect
{
    public PoisonStatusEffect(
        ReferenceMetadata referenceMetadata,
        InstanceId id,
        EffectInstanceSpec? instanceState
    )
        : base(referenceMetadata, id, instanceState) { }

    public PoisonStatusEffect(EffectTemplate template)
        : base(template) { }

    public PoisonStatusEffect(PoisonStatusEffect poisonStatusEffect)
        : base(poisonStatusEffect) { }

    public override IEffect DeepCopy()
    {
        return new PoisonStatusEffect(this);
    }
}
