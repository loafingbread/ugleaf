namespace GameLogic.Usables;

using GameLogic.Entities;

public interface IUser : IEntity
{
    public bool CanUse(UsableTemplate usable);
}
