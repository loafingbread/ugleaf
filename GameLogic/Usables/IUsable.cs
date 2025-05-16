namespace GameLogic.Usables;

using GameLogic.Config;
using GameLogic.Entities;
using GameLogic.Usables.Effects;

public interface IUsable : IConfigurable<UsableConfig>
{
    public IEnumerable<UsableResult> Use(Entity user, IEnumerable<Entity> targets);
}

public class UsableResult
{
    public IUsable Usable { get; }
    public Entity User { get; }
    public Entity Target { get; }
    public List<EffectResult> Results { get; } = new();

    public UsableResult(IUsable usable, Entity user, Entity target)
    {
        this.Usable = usable;
        this.User = user;
        this.Target = target;
    }
}
