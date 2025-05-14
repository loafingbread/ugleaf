using GameLogic.Entities;

namespace GameLogic.Targeting;

public interface ITargetable : IEntity
{
    public EFactionRelationship GetRelationTo(ITargeter targeter);
}
