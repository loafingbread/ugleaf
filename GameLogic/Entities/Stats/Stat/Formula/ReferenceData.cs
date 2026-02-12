namespace GameLogic.Entities.Stats.Stat;

using GameLogic.Registry;

public record ReferenceData
{
    public required ReferenceId ReferenceId { get; init; }
    public required bool ByBaseValue { get; init; }

    public ReferenceData DeepCopy()
    {
        return new ReferenceData()
        {
            ReferenceId = this.ReferenceId,
            ByBaseValue = this.ByBaseValue,
        };
    }

    public bool IsValid(Func<ReferenceId?, bool> doesReferenceExist)
    {
        if (!doesReferenceExist(this.ReferenceId))
        {
            return false;
        }

        return true;
    }
}
