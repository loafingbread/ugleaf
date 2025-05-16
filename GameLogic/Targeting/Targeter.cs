namespace GameLogic.Targeting;

using GameLogic.Entities;

public class Targeter : ITargeter
{
    private List<ITargetable> _targets = new();
    public TargeterConfig _config { get; private set; }

    public Targeter(TargeterConfig config)
    {
        this._config = config;
    }

    /*********************************
    * IConfigurable<TargeterConfig>
    *********************************/
    public void ApplyConfig(TargeterConfig config)
    {
        this._config = config;
    }

    public TargeterConfig GetConfig() => this._config;

    /*********************************
    * ITargeter
    *********************************/
    public Entity GetEntity() => (Entity)this;

    public bool CanTarget(ITargetable candidate)
    {
        return this._config.AllowedTargets.Contains(candidate.GetRelationTo(this));
    }

    // Get max number of eligibile targets
    public int GetMaxTargets(object source, IEnumerable<ITargetable> candidates)
    {
        switch (this._config.TargetQuantity)
        {
            case ETargetQuantity.Count:
                return this.getMaxTargets(source, candidates, this._config.Count);
            case ETargetQuantity.All:
                return this.getMaxTargets(source, candidates, int.MaxValue);
            default:
                return 0;
        }
    }

    private int getMaxTargets(object source, IEnumerable<ITargetable> candidates, int maxTargets)
    {
        int targetCount = 0;
        foreach (ITargetable candidate in candidates)
        {
            if (targetCount == maxTargets)
            {
                return targetCount;
            }

            bool IsTargetable = this.CanTarget(candidate);
            if (IsTargetable)
            {
                targetCount++;
            }
        }

        return targetCount;
    }

    public IEnumerable<ITargetable> GetEligibleTargets(
        object source,
        IEnumerable<ITargetable> candidates
    )
    {
        List<ITargetable> targets = new();
        foreach (ITargetable candidate in candidates)
        {
            if (this.CanTarget(candidate))
            {
                targets.Add(candidate);
            }
        }

        return targets;
    }

    public void ClearTargets() => this._targets.Clear();

    // Returns whether done targeting or not
    public bool Target(object source, ITargetable target, IEnumerable<ITargetable> candidates)
    {
        int maxTargets = this.GetMaxTargets(source, candidates);
        bool doneTargeting = this._targets.Count >= maxTargets;
        if (doneTargeting)
        {
            return true;
        }

        if (!this.CanTarget(target))
        {
            return false;
        }

        if (this._targets.Contains(target))
        {
            return false;
        }

        this._targets.Add(target);

        doneTargeting = this._targets.Count >= maxTargets;
        return doneTargeting;
    }

    public bool Untarget(ITargetable target)
    {
        return this._targets.Remove(target);
    }
}
