namespace GameLogic.Actions.Effects;

using GameLogic.Actions.Usables;
using GameLogic.Targeting;

public interface IEffectVariant
{
    IEffectResult Compute(IUser user, ITargetable target);
}

public interface IAttackEffectVariant : IEffectVariant
{
    float Value { get; }
    float CritChance { get; }
}

public interface IHealEffectVariant : IEffectVariant
{
    float Value { get; }
}

public interface IStatusEffectVariant : IEffectVariant
{
    float Value { get; }
    float Duration { get; }
}
