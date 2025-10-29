namespace GameLogic.Targeting;

using GameLogic.Entities;
using GameLogic.Utils;

public interface ITargeter : IPosition, IDeepCopyable<ITargeter>
{
    public ETargetQuantity QuantityType { get; }
    public int Count { get; }
    public List<EFactionRelationship> AllowedTargets { get; }

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

public class NoTargeter : ITargeter
{
    public ETargetQuantity QuantityType { get; } = ETargetQuantity.None;
    public int Count { get; } = 0;
    public List<EFactionRelationship> AllowedTargets { get; } = new();

    public Position Position { get; } = new Position(0, 0, 0);

    public NoTargeter(Position position)
    {
        this.Position = position;
    }

    public bool CanTarget(ITargetable candidate) => false;

    public int GetMaxTargets(object source, IEnumerable<ITargetable> candidates) => 0;

    public IEnumerable<ITargetable> GetEligibleTargets(
        object source,
        IEnumerable<ITargetable> candidates
    ) => new List<ITargetable>();

    public void ClearTargets() { }

    public bool Target(object source, ITargetable target, IEnumerable<ITargetable> candidates) =>
        false;

    public bool Untarget(ITargetable target) => false;

    public ITargeter DeepCopy() => new NoTargeter(this.Position.DeepCopy());
}
