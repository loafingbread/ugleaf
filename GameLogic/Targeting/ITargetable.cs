namespace GameLogic.Targeting;

public interface ITargetable
{
    public EFactionRelationship GetRelationTo(ITargeter targeter);
}
