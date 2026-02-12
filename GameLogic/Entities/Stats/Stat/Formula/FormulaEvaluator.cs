namespace GameLogic.Entities.Stats.Stat;

using GameLogic.Registry;

public static class FormulaEvaluator
{
    public static bool IsValid(FormulaData formulaData, Func<ReferenceId?, bool> doesReferenceExist)
    {
        return formulaData.IsValid(doesReferenceExist);
    }

    public static float Evaluate(
        FormulaData formulaData,
        Func<ReferenceId?, bool, float> getValueFunc
    )
    {
        if (formulaData.Constant != null)
        {
            return EvaluateConstant(formulaData.Constant);
        }
        else if (formulaData.Reference != null)
        {
            return EvaluateReference(formulaData.Reference, getValueFunc);
        }
        else if (formulaData.Operator != null)
        {
            EvalValue evalValue = EvaluateOperator(formulaData.Operator, getValueFunc);
            return evalValue.AsNumber();
        }
        else if (formulaData.If != null)
        {
            return EvaluateIf(formulaData.If, getValueFunc);
        }
        else if (formulaData.Call != null)
        {
            return EvaluateCall(formulaData.Call, getValueFunc);
        }

        throw new InvalidOperationException("Invalid formula data");
    }

    private static float EvaluateConstant(ConstantData constantData)
    {
        return constantData.Value;
    }

    private static float EvaluateReference(
        ReferenceData referenceData,
        Func<ReferenceId?, bool, float> getValueFunc
    )
    {
        return getValueFunc(referenceData.ReferenceId, referenceData.ByBaseValue);
    }

    private static EvalValue EvaluateOperator(
        OperatorData operatorData,
        Func<ReferenceId?, bool, float> getValueFunc
    )
    {
        List<float> argumentValues = operatorData
            .Arguments.Select(argument => Evaluate(argument, getValueFunc))
            .ToList();

        if (argumentValues.Count != 2)
        {
            throw new InvalidOperationException(
                $"Operator: {operatorData.OperatorType} requires 2 arguments instead of {argumentValues.Count}"
            );
        }

        Func<float, float, float>? floatOperatorFunction = operatorData.OperatorType switch
        {
            EOperatorType.Add => (a, b) => a + b,
            EOperatorType.Subtract => (a, b) => a - b,
            EOperatorType.Multiply => (a, b) => a * b,
            EOperatorType.Divide => (a, b) => a / b,
            EOperatorType.Modulus => (a, b) => a % b,
            EOperatorType.Power => (a, b) => MathF.Pow(a, b),
            _ => null,
        };
        if (floatOperatorFunction is not null)
        {
            return EvalValue.FromValue(floatOperatorFunction(argumentValues[0], argumentValues[1]));
        }

        Func<float, float, bool>? booleanOperatorFunction = operatorData.OperatorType switch
        {
            EOperatorType.Equals => (a, b) => a == b,
            EOperatorType.NotEquals => (a, b) => a != b,
            EOperatorType.GreaterThan => (a, b) => a > b,
            EOperatorType.GreaterThanOrEqual => (a, b) => a >= b,
            EOperatorType.LessThan => (a, b) => a < b,
            EOperatorType.LessThanOrEqual => (a, b) => a <= b,
            _ => null,
        };
        if (booleanOperatorFunction is not null)
        {
            return EvalValue.FromValue(
                booleanOperatorFunction(argumentValues[0], argumentValues[1])
            );
        }

        throw new InvalidOperationException($"Invalid operator type: {operatorData.OperatorType}");
    }

    private static float EvaluateIf(IfData ifData, Func<ReferenceId?, bool, float> getValueFunc)
    {
        EvalValue conditionValue = FormulaEvaluator.EvaluateOperator(
            ifData.Condition,
            getValueFunc
        );
        if (!conditionValue.IsBoolean())
        {
            throw new InvalidOperationException("Condition must be a boolean");
        }

        if (conditionValue.AsBoolean())
        {
            return FormulaEvaluator.Evaluate(ifData.ThenValue, getValueFunc);
        }
        else
        {
            return FormulaEvaluator.Evaluate(ifData.ElseValue, getValueFunc);
        }
    }

    private static float EvaluateCall(
        CallData callData,
        Func<ReferenceId?, bool, float> getValueFunc
    )
    {
        List<float> argumentValues = callData
            .Arguments.Select(argument => Evaluate(argument, getValueFunc))
            .ToList();
        if (argumentValues.Count != callData.Arguments.Count)
        {
            throw new InvalidOperationException(
                $"Call: {callData.FunctionType} requires {callData.Arguments.Count} arguments instead of {argumentValues.Count}"
            );
        }

        Func<float, float> function = callData.FunctionType switch
        {
            EFunctionType.Clamp => (value) => Math.Clamp(value, 0, 1),
            EFunctionType.Round => (value) => MathF.Round(value),
            _ => throw new InvalidOperationException(
                $"Invalid function type: {callData.FunctionType}"
            ),
        };

        return function(argumentValues[0]);
    }
}
