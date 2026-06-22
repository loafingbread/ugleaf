namespace GameLogic.Usables.Effects.Variants;

using GameLogic.Targeting;
using GameLogic.Usables.Effects.Results;

public class PoisonStatusVariant : IEffectVariant
{
    private readonly EffectTemplateData _data;

    public PoisonStatusVariant(EffectTemplateData data) => _data = data;

    public IEffectResult Compute(IUser user, ITargetable target)
    {
        return new StatusEffectResult(this, target, _data.Value, _data.Duration);
    }
}
