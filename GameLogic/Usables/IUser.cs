namespace GameLogic.Usables;

using GameLogic.Entities;

public interface IUser
{
    public bool CanUse(IUsable usable);
    public UsableResult Use(IUsable usable, Entity user, Entity target);
}

