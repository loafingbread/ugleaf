using GameLogic.Entities;

namespace GameLogic.Usables;

public class User : IUser
{
    public bool CanUse(IUsable usable)
    {
        // TODO: Add method to IUsable to check if usable
        return true;
    }

    public UsableResult Use(IUsable usable, Entity user, Entity target)
    {
        return usable.Use(user, target);
    }
}