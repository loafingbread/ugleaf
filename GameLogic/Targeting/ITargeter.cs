namespace GameLogic.Targeting;

using GameLogic.Config;
using GameLogic.Entities;

public interface ITargeter : IEntity, IConfigurable<TargeterConfig>
{
    public bool CanTarget(ITargetable candidate);
    public int GetMaxTargets(object source, IEnumerable<ITargetable> candidates);
    public IEnumerable<ITargetable> GetEligibleTargets(
        object source,
        IEnumerable<ITargetable> candidates
    );
    public void ClearTargets();
    public bool Target(object source, ITargetable target, IEnumerable<ITargetable> candidates);
    public bool Untarget(ITargetable target);

}