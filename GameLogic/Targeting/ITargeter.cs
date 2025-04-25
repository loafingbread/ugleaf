namespace GameLogic.Targeting;

public interface ITargeter
{
    public bool IsTargetable(object source, ITargetable candidate);
    public int GetMaxTargets(object source, IEnumerable<ITargetable> candidates);
    public IEnumerable<ITargetable> GetEligibleTargets(
        object source,
        IEnumerable<ITargetable> candidates
    );
    public void ClearTargets();
    public bool Target(object source, ITargetable target, IEnumerable<ITargetable> candidates);
    public bool Untarget(object source, ITargetable target);
}