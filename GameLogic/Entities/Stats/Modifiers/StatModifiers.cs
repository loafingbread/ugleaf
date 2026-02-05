namespace GameLogic.Entities.Stats.Modifiers;

using GameLogic.Registry;
using GameLogic.Utils;

public class StatModifiers : IDeepCopyable<StatModifiers>
{
    public List<StatModifier> Modifiers { get; } = new List<StatModifier>();

    public StatModifiers() { }

    public StatModifiers(StatModifiers statModifiers)
    {
        this.Modifiers = statModifiers.Modifiers.DeepCopyList();
    }

    public StatModifiers DeepCopy()
    {
        return new StatModifiers(this);
    }

    /// <summary>
    /// Adds a modifier to the stat modifiers.
    /// </summary>
    /// <param name="modifier">The modifier to add.</param>
    /// <returns>True if the modifier was added, false if the modifier already exists and was updated.</returns>
    public bool AddModifier(StatModifier modifier)
    {
        bool doesStatMatchModifier(StatModifier m, ReferenceId statId) => m.StatId == statId;
        bool doesStatTypeMatchModifier(StatModifier m, StatModifierType type) => m.Type == type;

        StatModifier? existingModifier = this.Modifiers.FirstOrDefault(
            (StatModifier modifier) =>
                doesStatMatchModifier(modifier, modifier.StatId)
                && doesStatTypeMatchModifier(modifier, modifier.Type)
        );

        if (existingModifier == null)
        {
            this.Modifiers.Add(modifier);
            return true;
        }

        // If the modifier already exists, update it by either:
        // 1. Adding the value to the existing modifier and extending the duration
        // 2. Just extending the duration
        // 3. Overwriting the existing modifier
        // 4. Some unique or other way to evolve the modifier

        return false;
    }

    public bool RemoveModifier(StatModifier modifier)
    {
        return this.Modifiers.Remove(modifier);
    }

    public float GetModifiedValueFromBase(ReferenceId statId, float baseValue)
    {
        float finalValue = baseValue;
        finalValue += this.SumModifiers(statId, StatModifierType.Flat);
        finalValue += (baseValue * this.SumModifiers(statId, StatModifierType.PercentAdd));
        finalValue *= (1 + this.SumModifiers(statId, StatModifierType.PercentMultiply));

        return float.Round(finalValue, 2);
    }

    public float SumModifiers(ReferenceId statId, StatModifierType type)
    {
        float value = 0;
        foreach (var modifier in this.Modifiers)
        {
            if (this.isModifierApplicable(modifier, statId, type))
            {
                value += modifier.Value;
            }
        }

        return value;
    }

    private bool isModifierApplicable(
        StatModifier modifier,
        ReferenceId statId,
        StatModifierType type
    )
    {
        return modifier.StatId == statId && modifier.Type == type;
    }
}
