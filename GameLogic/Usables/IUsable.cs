namespace GameLogic.Usables;

using GameLobic.Usables;
using GameLogic.Entities;
using GameLogic.Targeting;

public interface IUsable
{
    public IEnumerable<UsableResult> Use(Entity user, IEnumerable<Entity> targets);
}

public interface IUsableEffect
{
    UsableResult Apply(IUser user, ITargetable target);
}


public class UsableResult
{
    public Entity User { get; }
    public Entity Target { get; }
    public bool DidCrit { get; }
    public bool DidHit { get; }
    public IUsable Usable { get; }
    public int Value { get; }

    public UsableResult(
        Entity user, Entity target,
        bool didCrit, bool didHit,
        IUsable usable, int value
    )
    {
        this.User = user;
        this.Target = target;
        this.DidCrit = didCrit;
        this.DidHit = didHit;
        this.Usable = usable;
        this.Value = value;
    }

}