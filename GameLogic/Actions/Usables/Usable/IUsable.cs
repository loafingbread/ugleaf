namespace GameLogic.Actions.Usables.Usable;

using GameLogic.Entities;
using GameLogic.Actions.Effects;
using GameLogic.Utils;

public interface IUsable : IDeepCopyable<IUsable>
{
    public IEnumerable<UsableResult> Use(Entity user, IEnumerable<Entity> targets);
}

public class UsableResult
{
    public IUsable Usable { get; }
    public Entity User { get; }
    public Entity Target { get; }
    public List<IEffectResult> Results { get; } = new();

    public UsableResult(IUsable usable, Entity user, Entity target)
    {
        this.Usable = usable;
        this.User = user;
        this.Target = target;
    }
}
