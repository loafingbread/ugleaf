namespace GameLogic.Usables.Effects;

using GameLogic.Entities;
using GameLogic.Targeting;
using GameLogic.Utils;

public interface IEffect : IDeepCopyable<IEffect>
{
    EffectResult Apply(IUser user, ITargetable target);
}

public class EffectResult
{
    public IEffect Effect { get; }
    public Entity User { get; }
    public Entity Target { get; }
    public int Value { get; }
    public bool DidCrit { get; }
    public bool DidHit { get; }
    public int Resistance { get; }

    public EffectResult(
        IEffect effect,
        Entity user,
        Entity target,
        int value,
        bool didCrit,
        bool didHit,
        int resistance
    )
    {
        this.User = user;
        this.Target = target;
        this.Effect = effect;
        this.Value = value;
        this.DidCrit = didCrit;
        this.DidHit = didHit;
        this.Resistance = resistance;
    }
}
