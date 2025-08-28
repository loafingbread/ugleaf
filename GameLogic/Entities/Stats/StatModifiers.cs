namespace GameLogic.Entities.Stats;

public class StatModifiers
{
    public List<StatModifier> Modifiers { get; } = new List<StatModifier>();

    public StatModifiers() { }

    /// <summary>
    /// Adds a modifier to the stat modifiers.
    /// </summary>
    /// <param name="modifier">The modifier to add.</param>
    /// <returns>True if the modifier was added, false if the modifier already exists and was updated.</returns>
    public bool AddModifier(StatModifier modifier)
    {
        StatModifier? existingModifier = this.Modifiers.FirstOrDefault(
            (StatModifier? m) => m?.StatId == modifier.StatId && m?.Type == modifier.Type
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

    public int GetModifiedValueFromBase(string statId, int baseValue)
    {
        int finalValue = baseValue;
        finalValue += (int)this.SumModifiers(statId, StatModifierType.Flat);
        finalValue += (int)(
            (float)baseValue * this.SumModifiers(statId, StatModifierType.PercentAdd)
        );
        finalValue *= (int)(1 + this.SumModifiers(statId, StatModifierType.PercentMultiply));

        return finalValue;
    }

    public float SumModifiers(string statId, StatModifierType type)
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

    private bool isModifierApplicable(StatModifier modifier, string statId, StatModifierType type)
    {
        return modifier.StatId == statId && modifier.Type == type;
    }
}

public class StatModifier
{
    public string StatId { get; }
    public StatModifierType Type { get; }
    public StatModifierSourceType SourceType { get; }
    public float Value { get; }

    public StatModifier(
        string statId,
        StatModifierType type,
        StatModifierSourceType sourceType,
        float value
    )
    {
        this.Type = type;
        this.SourceType = sourceType;
        this.StatId = statId;
        this.Value = value;
    }
}

public enum StatModifierType
{
    Flat,
    PercentAdd,
    PercentMultiply,
}

public enum StatModifierSourceType
{
    Trait,
    Passive,
    Item,
    Skill,
}
