namespace GameLogic.Actions.Effects;

using GameLogic.Registry;

public class HealEffect : Effect
{
    public HealEffect(EffectData data)
        : base(data) { }

    public HealEffect(EffectTemplate template)
        : base(template) { }

    public HealEffect(HealEffect healEffect)
        : base(healEffect) { }

    public override IEffect DeepCopy()
    {
        return new HealEffect(this);
    }
}
