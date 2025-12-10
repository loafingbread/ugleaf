namespace GameLogic.Usables.Effects;

using GameLogic.Registry;

public class HealEffect : Effect
{
    public HealEffect(
        ReferenceUnionMetadata referenceMetadata,
        InstanceId id,
        EffectInstanceSpec? instanceState
    )
        : base(referenceMetadata, id, instanceState) { }

    public HealEffect(EffectTemplate template)
        : base(template) { }

    public HealEffect(HealEffect healEffect)
        : base(healEffect) { }

    public override IEffect DeepCopy()
    {
        return new HealEffect(this);
    }
}
