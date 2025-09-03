namespace GameLogic.Usables;

using GameLogic.Entities;
using GameLogic.Utils;

public interface IUser : IEntity, IDeepCopyable<IUser>
{
    public bool CanUse(IUsable usable);
    public IEnumerable<UsableResult> Use(IUsable usable, Entity user, IEnumerable<Entity> target);
}
