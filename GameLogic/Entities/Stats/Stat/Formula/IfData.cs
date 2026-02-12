namespace GameLogic.Entities.Stats.Stat;

using GameLogic.Registry;

public record IfData
{
    public required OperatorData Condition { get; init; }
    public required FormulaData ThenValue { get; init; }
    public required FormulaData ElseValue { get; init; }

    public IfData DeepCopy()
    {
        return new IfData()
        {
            Condition = this.Condition.DeepCopy(),
            ThenValue = this.ThenValue.DeepCopy(),
            ElseValue = this.ElseValue.DeepCopy(),
        };
    }

    public bool IsValid(Func<ReferenceId?, bool> doesReferenceExist)
    {
        if (!this.Condition.IsValid(doesReferenceExist))
        {
            return false;
        }

        if (!this.ThenValue.IsValid(doesReferenceExist))
        {
            return false;
        }

        if (!this.ElseValue.IsValid(doesReferenceExist))
        {
            return false;
        }

        return true;
    }
}
