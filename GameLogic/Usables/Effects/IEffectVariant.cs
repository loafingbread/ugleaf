namespace GameLogic.Usables.Effects;

using GameLogic.Targeting;

public interface IEffectVariant
{
    IEffectResult Compute(IUser user, ITargetable target);
}
