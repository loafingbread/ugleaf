namespace GameLogic.Actions.Effects;

using GameLogic.Registry;

public abstract class StatusEffect : Effect
{
    public StatusEffect(EffectData data)
        : base(data) { }

    public StatusEffect(EffectTemplate template)
        : base(template) { }

    public StatusEffect(StatusEffect statusEffect)
        : base(statusEffect) { }

    public abstract override IEffect DeepCopy();
}
