namespace GameLogic.Usables.Effects.Variants;

using GameLogic.Targeting;
using GameLogic.Usables.Effects.Results;

public class AttackEffectVariant : IEffectVariant
{
    private readonly EffectTemplateData _data;

    public AttackEffectVariant(EffectTemplateData data) => _data = data;

    public IEffectResult Compute(IUser user, ITargetable target)
    {
        bool didCrit = Random.Shared.NextSingle() < _data.CritChance;
        float damage = didCrit ? _data.Value * 2f : _data.Value;
        return new AttackEffectResult(this, target, damage, didCrit);
    }
}
