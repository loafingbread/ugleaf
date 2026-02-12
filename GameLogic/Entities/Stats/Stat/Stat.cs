namespace GameLogic.Entities.Stats.Stat;

using GameLogic.Entities.Stats.Modifiers;
using GameLogic.Registry;
using GameLogic.Utils;

// TODO: Implement stat template vs stat value/instance

public class Stat : IDeepCopyable<Stat>
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

    public Stat DeepCopy()
    {
        return new Stat(this.ReferenceId, this.Model);
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
