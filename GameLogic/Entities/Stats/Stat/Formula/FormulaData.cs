namespace GameLogic.Entities.Stats.Stat;

using GameLogic.Registry;
using GameLogic.Utils;

public record FormulaData : IDeepCopyable<FormulaData>
{
    public required OperatorData? Operator { get; init; }
    public required ConstantData? Constant { get; init; }
    public required ReferenceData? Reference { get; init; }
    public required IfData? If { get; init; }
    public required CallData? Call { get; init; }

    public FormulaData DeepCopy()
    {
        return new FormulaData()
        {
            Operator = this.Operator?.DeepCopy(),
            Constant = this.Constant?.DeepCopy(),
            Reference = this.Reference?.DeepCopy(),
            If = this.If?.DeepCopy(),
            Call = this.Call?.DeepCopy(),
        };
    }

    /// <summary>
    /// Checks if the formula is valid. Call asap after construction.
    /// </summary>
    /// <returns>True if formula is valid, false otherwise.</returns>
    public bool IsValid(Func<ReferenceId?, bool> doesReferenceExist)
    {
        if (this.Operator != null && this.Operator.IsValid(doesReferenceExist))
        {
            return true;
        }
        else if (this.Constant != null && this.Constant.IsValid())
        {
            return true;
        }
        else if (this.Reference != null && this.Reference.IsValid(doesReferenceExist))
        {
            return true;
        }
        else if (this.If != null && this.If.IsValid(doesReferenceExist))
        {
            return true;
        }
        else if (this.Call != null && this.Call.IsValid())
        {
            return true;
        }

        return false;
    }
}
