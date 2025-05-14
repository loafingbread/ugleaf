namespace GameLogic.Usables;

using GameLogic.Entities;

public interface IUser : IEntity
{
    public bool CanUse(IUsable usable);
    public IEnumerable<UsableResult> Use(IUsable usable, Entity user, IEnumerable<Entity> target);
}
