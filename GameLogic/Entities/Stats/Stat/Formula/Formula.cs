namespace GameLogic.Entities.Stats.Stat;

using GameLogic.Registry;

public class Formula
{
    public required FormulaData Data { get; init; }
    public Func<ReferenceId?, bool, float>? GetValueFunc { get; init; }

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public Formula(FormulaData data, Func<ReferenceId?, bool, float>? getValueFunc)
    {
        this.Data = data;
        this.GetValueFunc = getValueFunc;
    }

    public float CalculateValue(Func<ReferenceId?, bool, float> getValueFunc)
    {
        return FormulaEvaluator.Evaluate(this.Data, getValueFunc);
    }

    public bool IsValid(Func<ReferenceId?, bool> doesReferenceExist)
    {
        return this.Data.IsValid(doesReferenceExist);
    }
}
