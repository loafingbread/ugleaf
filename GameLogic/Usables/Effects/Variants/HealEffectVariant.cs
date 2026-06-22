namespace GameLogic.Usables.Effects.Variants;

using GameLogic.Targeting;
using GameLogic.Usables.Effects.Results;

public class HealEffectVariant : IEffectVariant
{
    private readonly EffectTemplateData _data;

    public HealEffectVariant(EffectTemplateData data) => _data = data;

    public IEffectResult Compute(IUser user, ITargetable target)
    {
        return new HealEffectResult(this, target, _data.Value);
    }
}
