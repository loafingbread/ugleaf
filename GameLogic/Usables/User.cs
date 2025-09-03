using GameLogic.Entities;
using GameLogic.Utils;

namespace GameLogic.Usables;

public class User : IUser
{
    public User() { }

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

    IUser IDeepCopyable<IUser>.DeepCopy() => new User();
}
