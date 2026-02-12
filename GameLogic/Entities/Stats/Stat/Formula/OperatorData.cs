namespace GameLogic.Entities.Stats.Stat;

using GameLogic.Registry;
using GameLogic.Utils;

public record OperatorData
{
    public required EOperatorType OperatorType { get; init; }
    public required List<FormulaData> Arguments { get; init; }

    public OperatorData DeepCopy()
    {
        return new OperatorData()
        {
            OperatorType = this.OperatorType,
            Arguments = this.Arguments.DeepCopyList(),
        };
    }

    public bool IsValid(Func<ReferenceId?, bool> doesReferenceExist)
    {
        if (!EOperatorType.IsDefined(this.OperatorType))
        {
            return false;
        }

        if (this.Arguments.Count != 2)
        {
            return false;
        }

        if (this.Arguments.Any(argument => !argument.IsValid(doesReferenceExist)))
        {
            return false;
        }

        return false;
    }
}

public enum EOperatorType
{
    Add,
    Subtract,
    Multiply,
    Divide,
    Modulus,
    Power,
    Equals,
    NotEquals,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    And,
    Or,
}

public readonly record struct EvalValue(float Number, bool Boolean, EEvalKind Kind)
{
    public static EvalValue FromValue(float value)
    {
        return new EvalValue(value, false, EEvalKind.Number);
    }

    public static EvalValue FromValue(bool value)
    {
        return new EvalValue(float.NaN, value, EEvalKind.Boolean);
    }

    public bool IsNumber()
    {
        return this.Kind == EEvalKind.Number;
    }

    public bool IsBoolean()
    {
        return this.Kind == EEvalKind.Boolean;
    }

    public float AsNumber()
    {
        return this.Kind == EEvalKind.Number
            ? this.Number
            : throw new InvalidOperationException("Cannot convert EvalValue to number");
    }

    public bool AsBoolean()
    {
        return this.Kind == EEvalKind.Boolean
            ? this.Boolean
            : throw new InvalidOperationException("Cannot convert EvalValue to boolean");
    }
};

public enum EEvalKind
{
    Number,
    Boolean,
}
