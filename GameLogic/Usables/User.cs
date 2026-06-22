using GameLogic.Entities;

namespace GameLogic.Usables;

public class User : IUser
{
    public Entity GetEntity() => (Entity)this;

    public bool CanUse(UsableTemplate usable)
    {
        // TODO: Check prerequisites, resources, cooldowns
        return true;
    }
}
