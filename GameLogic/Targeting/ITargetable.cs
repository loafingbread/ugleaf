using GameLogic.Entities;
using GameLogic.Utils;

namespace GameLogic.Targeting;


public interface ITargetable : IEntity, IDeepCopyable<ITargetable>
{
    public EFactionRelationship GetRelationTo(ITargeter targeter);
}
