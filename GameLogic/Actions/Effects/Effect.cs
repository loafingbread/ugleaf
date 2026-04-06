using GameLogic.Targeting;

namespace GameLogic.Actions.Effects;

using GameLogic.Registry;
using GameLogic.Utils;

public class EffectTemplate : IDeepCopyable<EffectTemplate>
{
    public ReferenceId ReferenceId { get; set; }
    public EEffectType Type { get; set; }
    public string Subtype { get; set; }
    public string Variant { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }

    public int Value { get; set; }
    public int Duration { get; set; }

    public EffectTemplate(EffectData data)
    {
        this.ReferenceId = data.ReferenceId;

        this.Type = Enum.Parse<EEffectType>(data.Type);
        this.Subtype = data.Subtype;
        this.Variant = data.Variant;
        this.Name = data.Name;
        this.Description = data.Description;
        this.Tags = [.. data.Tags];

        this.Value = data.Config.Value;
        this.Duration = data.Config.Duration;
    }

    // Copy constructor
    public EffectTemplate(EffectTemplate template)
    {
        this.ReferenceId = Ids.NewReferenceId();

        this.Type = template.Type;
        this.Subtype = template.Subtype;
        this.Variant = template.Variant;
        this.Name = template.Name;
        this.Description = template.Description;
        this.Tags = [.. template.Tags];

        this.Value = template.Value;
        this.Duration = template.Duration;
    }

    public IEffect Instantiate()
    {
        return EffectFactory.CreateEffect(this);
    }

    public EffectTemplate DeepCopy()
    {
        return new EffectTemplate(this);
    }
}

public abstract class Effect : EffectTemplate, IEffect
{
    public Effect(EffectData data)
        : base(data) { }

    public Effect(Effect effect)
        : base((effect as EffectTemplate).DeepCopy()) { }

    public Effect(EffectTemplate template)
        : base(template) { }

    public new abstract IEffect DeepCopy();

    public EffectResult Apply(IUser user, ITargetable target)
    {
        return new EffectResult(this, user.GetEntity(), target.GetEntity(), 5, false, true, 0);
    }
}

public class EEffect
{
    public ReferenceId ReferenceId { get; set; }

    public IEffectModel Model { get; set; }

    public EEffect(ReferenceId referenceId, IEffectModel model)
    {
        this.ReferenceId = referenceId;
        this.Model = model;
    }
}

public interface IEffectModel
{
    public IAttack? Attack { get; }
    public IHeal? Heal { get; }
    public IBuff? Buff { get; }
    public IDebuff? Debuff { get; }
    public IStatus? Status { get; }
}

public interface IAttack
{
    public float Value { get; set; }
    public float CritChance { get; set; }
}

public interface IHeal
{
    public float Value { get; set; }
}

public interface IBuff
{
    public float Value { get; set; }
}

public interface IDebuff
{
    public float Value { get; set; }
}

public interface IStatus
{
    public float Value { get; set; }
}
