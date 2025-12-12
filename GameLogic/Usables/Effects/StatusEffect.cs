namespace GameLogic.Usables.Effects;

using GameLogic.Registry;

public abstract class StatusEffect : Effect
{
    public StatusEffect(
        ReferenceMetadata referenceMetadata,
        InstanceId id,
        EffectInstanceSpec? instanceState
    )
        : base(referenceMetadata, id, instanceState) { }

    public StatusEffect(EffectTemplate template)
        : base(template) { }

    public StatusEffect(StatusEffect statusEffect)
        : base(statusEffect) { }

    public abstract override IEffect DeepCopy();
}
