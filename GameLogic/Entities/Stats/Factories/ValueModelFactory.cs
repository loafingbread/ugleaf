namespace GameLogic.Entities.Stats.Factories;

using GameLogic.Entities.Stats.Stat;

public static class ValueModelFactory
{
    public static IValueModel FromSpecToValueModel(ValueModelData data)
    {
        if (data.Type == EValueModel.Fixed)
        {
            return new FixedValueModel(data.Value ?? 0f);
        }
        else if (data.Type == EValueModel.Mutable)
        {
            return new MutableValueModel(data.Value ?? 0f);
        }
        else if (data.Type != EValueModel.Formula)
        {
            throw new InvalidOperationException($"Invalid value model type: {data.Type}");
        }

        if (data.Formula is null)
        {
            throw new InvalidOperationException("Formula is required for formula value model");
        }

        return new FormulaValueModel(new Formula(data.Formula, null));
    }
}
