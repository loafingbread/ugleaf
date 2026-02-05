namespace GameLogic.Entities.Stats.Capabilities;

using GameLogic.Registry;

public record ImmutableSpec : FormulaSpec { }

public record FormulaSpec
{
    public required EFormulaType Type { get; init; }
    public required List<FormulaSpec> Operands { get; init; }
    public required bool ByBaseValue { get; init; }
    public ReferenceId? ReferenceId { get; init; }
    public float? Value { get; init; }

    bool IsValid()
    {
        if (this.Type == EFormulaType.Constant)
        {
            return this.Value is not null;
        }
        else if (this.Type == EFormulaType.Derived)
        {
            return this.ReferenceId is not null;
        }
        return false;
    }
}

public enum EFormulaType
{
    Constant,
    Derived,
}

public interface IImmutable
{
    StatFormula Formula { get; set; }
    float CalculateValue();
}

public class ImmutableCapability : IImmutable
{
    public StatFormula Formula { get; set; }

    public ImmutableCapability(StatFormula formula)
    {
        this.Formula = formula;
    }

    public float CalculateValue()
    {
        float value = this.Formula.CalculateValue();
        return value;
    }
}

public class StatFormula
{
    public required FormulaSpec Spec { get; init; }
    public Func<ReferenceId?, bool, float>? GetValueFunc { get; init; }

    [System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
    public StatFormula(FormulaSpec spec, Func<ReferenceId?, bool, float>? getValueFunc)
    {
        this.Spec = spec;
        this.GetValueFunc = getValueFunc;
    }

    public float CalculateValue()
    {
        if (this.Spec.Type == EFormulaType.Constant)
        {
            return this.Spec.Value
                ?? throw new InvalidOperationException("Value is required for constant formula");
        }
        else if (this.Spec.Type == EFormulaType.Derived)
        {
            return this.GetValueFunc?.Invoke(this.Spec.ReferenceId, this.Spec.ByBaseValue)
                ?? throw new InvalidOperationException(
                    "Value function is required for derived formula"
                );
        }
        throw new InvalidOperationException("Invalid formula spec");
    }
}
