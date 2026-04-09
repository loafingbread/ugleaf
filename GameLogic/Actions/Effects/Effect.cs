namespace GameLogic.Actions.Effects;

using GameLogic.Actions.Usables;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Utils;

public interface IEffect : IDeepCopyable<IEffect>
{
    ReferenceId ReferenceId { get; }
    IEffectVariant Variant { get; }
    IEffectResult Compute(IUser user, ITargetable target);
}

public class Effect : IEffect
{
    public ReferenceId ReferenceId { get; }
    public IEffectVariant Variant { get; }

    public Effect(ReferenceId referenceId, IEffectVariant variant)
    {
        this.ReferenceId = referenceId;
        this.Variant = variant;
    }

    public IEffectResult Compute(IUser user, ITargetable target) =>
        this.Variant.Compute(user, target);

    public IEffect DeepCopy() => new Effect(this.ReferenceId, this.Variant);
}
