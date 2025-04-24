using GameLogic.Config;

namespace GameLogic.Entities;

public enum ETargetQuantity
{
    None,
    Count,
    All,
}

public record TargeterConfig
{
    public required ETargetQuantity TargetQuantity { get; init; }
    public required List<EFactionRelationship> AllowedTargets { get; init; }
    public int Count { get; init; }
}

public interface ITargeterType
{
    public string Id { get; }
    public string DisplayName { get; }
    public List<EFactionRelationship> AllowedTargets { get; }
    public ETargetQuantity TargetQuantity { get; }
    public int? Count { get; }
    public ITargeter CreateInstance();
}

public static class TargeterTypeRegistry
{
    private static readonly Dictionary<string, ITargeterType> _types = new();

    public static void Register(ITargeterType type) => _types[type.Id] = type;

    public static ITargeterType Get(string id)
    {
        bool found = _types.TryGetValue(id, out var type);
        if (!found)
        {
            throw new KeyNotFoundException($"TargeterTypeRegistry: TargeterType '{id}' not found");
        }
        else if (type == null)
        {
            throw new NullReferenceException(
                $"TargeterTypeRegistry: TargeterType should not be null"
            );
        }

        return type;
    }
}

public enum EFactionRelationship
{
    Self,
    Ally,
    Enemy,
    Neutral,
}

public interface ITargetable
{
    public EFactionRelationship GetRelationTo(object source);
}

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

public class Targeter : ITargeter, IConfigurable<TargeterConfig>
{
    private List<ITargetable> _targets = new();
    public TargeterConfig Config { get; private set; }

    public Targeter(TargeterConfig config)
    {
        this.Config = config;
    }

    public void ApplyConfig(TargeterConfig config)
    {
        this.Config = config;
    }

    // Get max number of eligibile targets
    public int GetMaxTargets(object source, IEnumerable<ITargetable> candidates)
    {
        switch (this.Config.TargetQuantity)
        {
            case ETargetQuantity.Count:
                return this.getMaxTargets(source, candidates, this.Config.Count);
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
            if (targetCount == maxTargets) {
                return targetCount;
            }

            bool IsTargetable = this.IsTargetable(source, candidate);
            if (IsTargetable)
            {
                targetCount++;
            }
        }

        return targetCount;
    }

    public bool IsTargetable(object source, ITargetable candidate)
    {
        return this.Config.AllowedTargets.Contains(candidate.GetRelationTo(source));
    }

    public IEnumerable<ITargetable> GetEligibleTargets(
        object source,
        IEnumerable<ITargetable> candidates
    )
    {
        List<ITargetable> targets = new();
        foreach (ITargetable candidate in candidates)
        {
            if (this.IsTargetable(source, candidate))
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

        if (!this.IsTargetable(source, target))
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

    public bool Untarget(object source, ITargetable target)
    {
        return this._targets.Remove(target);
    }
}
