namespace GameLogic.Entities.Stats.Modifiers;

using GameLogic.Registry;
using GameLogic.Utils;

public class StatModifier : IDeepCopyable<StatModifier>
{
    public ReferenceId StatId { get; }
    public StatModifierType Type { get; }
    public StatModifierSourceType SourceType { get; }
    public float Value { get; }

    public StatModifier(
        ReferenceId statId,
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

    public StatModifier(StatModifier statModifier)
    {
        this.StatId = statModifier.StatId;
        this.Type = statModifier.Type;
        this.SourceType = statModifier.SourceType;
        this.Value = statModifier.Value;
    }

    public StatModifier DeepCopy()
    {
        return new StatModifier(this);
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
