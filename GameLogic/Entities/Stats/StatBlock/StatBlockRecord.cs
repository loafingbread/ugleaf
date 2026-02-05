namespace GameLogic.Entities.Stats.StatBlock;

using GameLogic.Registry;

public interface IStatBlockSpec
{
    public List<ReferenceSpec> Stats { get; init; }
}

public record StatBlockSpec : IStatBlockSpec
{
    public required List<ReferenceSpec> Stats { get; init; } = new();
}
