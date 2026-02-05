namespace GameLogic.Entities.Stats.Stat;

using GameLogic.Registry;

// TODO: Implement stat template vs stat value/instance

public class Stat
{
    public ReferenceId ReferenceId { get; set; }
    public IStatModel Model { get; set; }
    public StatModifiers Modifiers { get; set; } = new();
    public float BaseValue => this.Model.GetValue();
    public float Value => this.Modifiers.GetModifiedValueFromBase(this.ReferenceId, this.BaseValue);

    public Stat(ReferenceId referenceId, IStatModel model)
    {
        this.ReferenceId = referenceId;
        this.Model = model;
    }

    public bool AddModifier(StatModifier modifier)
    {
        return this.Modifiers.AddModifier(modifier);
    }

    public bool RemoveModifier(StatModifier modifier)
    {
        return this.Modifiers.RemoveModifier(modifier);
    }
}
