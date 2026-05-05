namespace GameLogic.Actions.Effects;

using GameLogic.Actions.Usables;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Utils;

public interface IEffect : IDeepCopyable<IEffect>
{
    IEffectResult Compute(IUser user, ITargetable target);
}

public class Effect : IEffect
{
    public ReferenceId ReferenceId { get; }
    public EffectTemplate Template { get; }
    public EffectState State { get; }

    public Effect(ReferenceId referenceId, EffectTemplate template, EffectState state)
    {
        this.ReferenceId = referenceId;
        this.Template = template;
        this.State = state;
    }

    public IEffectResult Compute(IUser user, ITargetable target) =>
        this.State.Variant.Compute(user, target);

    public IEffect DeepCopy() => new Effect(this.ReferenceId, this.Template, this.State);
}
