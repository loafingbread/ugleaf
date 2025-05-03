namespace GameLogic.Usables;

using GameLogic.Entities;

public class Usable : IUsable
{
    public UsableResult Use(Entity user, Entity target)
    {
        // TODO: Implement usable result calculation
        return new UsableResult(user, target, false, true, this, 5);
    }
}