namespace GameLogic.Usables.Effects;

using GameLogic.Registry;

public class BurnStatusEffect : StatusEffect
{
    public BurnStatusEffect(EffectData data)
        : base(data) { }

    public BurnStatusEffect(EffectTemplate template)
        : base(template) { }

    public BurnStatusEffect(BurnStatusEffect burnStatusEffect)
        : base(burnStatusEffect) { }

    public override IEffect DeepCopy()
    {
        return new BurnStatusEffect(this);
    }
}
