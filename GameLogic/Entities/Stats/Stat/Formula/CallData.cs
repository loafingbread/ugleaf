namespace GameLogic.Entities.Stats.Stat;

using GameLogic.Utils;

public record CallData
{
    public required EFunctionType FunctionType { get; init; }
    public required List<FormulaData> Arguments { get; init; }

    public CallData DeepCopy()
    {
        return new CallData()
        {
            FunctionType = this.FunctionType,
            Arguments = this.Arguments.DeepCopyList(),
        };
    }

    public bool IsValid()
    {
        if (!EFunctionType.IsDefined(this.FunctionType))
        {
            return false;
        }

        if (this.Arguments.Count == 0)
        {
            return false;
        }

        return true;
    }
}

public enum EFunctionType
{
    Clamp,
    Round,
}