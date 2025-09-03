using GameLogic.Targeting;

namespace GameLogic.Usables.Effects;

using GameLogic.Registry;
using GameLogic.Utils;

public class EffectTemplate : ITemplate, IDeepCopyable<EffectTemplate>
{
    public TemplateId TemplateId { get; private set; }

    public EEffectType Type { get; private set; }
    public string Subtype { get; private set; }
    public string Variant { get; private set; }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<string> Tags { get; private set; }

    public int Value { get; private set; }
    public int Duration { get; private set; }

    public EffectTemplate(
        TemplateId templateId,
        EEffectType type,
        string subtype,
        string variant,
        string name,
        string description,
        List<string> tags,
        int value,
        int duration
    )
    {
        this.TemplateId = templateId;
        this.Type = type;
        this.Subtype = subtype;
        this.Variant = variant;
        this.Name = name;
        this.Description = description;
        this.Tags = [.. tags];
        this.Value = value;
        this.Duration = duration;
    }

    public EffectTemplate(EffectTemplate template)
    {
        this.TemplateId = template.TemplateId;
        this.Type = template.Type;
        this.Subtype = template.Subtype;
        this.Variant = template.Variant;
        this.Name = template.Name;
        this.Description = template.Description;
        this.Tags = [.. template.Tags];
        this.Value = template.Value;
        this.Duration = template.Duration;
    }

    public EffectTemplate DeepCopy()
    {
        return new EffectTemplate(this);
    }

    public IEffect Instantiate()
    {
        return EffectFactory.CreateEffectFromTemplate(this);
    }
}

public abstract class Effect : EffectTemplate, IEffect, IInstance
{
    public InstanceId InstanceId { get; private set; }

    public Effect(
        InstanceId instanceId,
        TemplateId templateId,
        EEffectType type,
        string subtype,
        string variant,
        string name,
        string description,
        List<string> tags,
        int value,
        int duration
    )
        : base(templateId, type, subtype, variant, name, description, tags, value, duration)
    {
        this.InstanceId = instanceId;
    }

    public Effect(EffectTemplate template)
        : base(template)
    {
        this.InstanceId = Ids.Instance();
    }

    public Effect(Effect effect)
        : base(
            effect.TemplateId,
            effect.Type,
            effect.Subtype,
            effect.Variant,
            effect.Name,
            effect.Description,
            effect.Tags,
            effect.Value,
            effect.Duration
        )
    {
        this.InstanceId = Ids.Instance();
    }

    public new abstract IEffect DeepCopy();

    public EffectResult Apply(IUser user, ITargetable target)
    {
        return new EffectResult(this, user.GetEntity(), target.GetEntity(), 5, false, true, 0);
    }
}
