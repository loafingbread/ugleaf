namespace GameLogic.Usables;

using GameLobic.Usables;
using GameLogic.Config;
using GameLogic.Entities;
using GameLogic.Targeting;

public interface IUsable : IConfigurable<UsableConfig>
{
    public IEnumerable<UsableResult> Use(Entity user, IEnumerable<Entity> targets);
}

public interface IUsableEffect
{
    EffectResult Apply(IUser user, ITargetable target);
}

public class UsableResult
{
    public IUsable Usable { get; }
    public Entity User { get; }
    public Entity Target { get; }
    public List <EffectResult> Results { get; } = new();

    public UsableResult(IUsable usable, Entity user, Entity target)
    {
        this.Usable = usable;
        this.User = user;
        this.Target = target;
    }
}

public class EffectResult
{
    public IUsableEffect Effect { get; }
    public Entity User { get; }
    public Entity Target { get; }
    public int Value { get; }
    public bool DidCrit { get; }
    public bool DidHit { get; }
    public int Resistance { get; }

    public EffectResult(
        IUsableEffect effect,
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
