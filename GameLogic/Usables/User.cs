using GameLogic.Entities;

namespace GameLogic.Usables;

public class User : IUser
{
    /*********************************
    * IUser
    *********************************/
    public Entity GetEntity() => (Entity)this;

    public bool CanUse(IUsable usable)
    {
        // TODO: Add method to IUsable to check if usable
        return true;
    }

    public IEnumerable<UsableResult> Use(IUsable usable, Entity user, IEnumerable<Entity> targets)
    {
        return usable.Use(user, targets);
    }
}