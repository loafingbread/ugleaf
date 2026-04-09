namespace GameLogic.Actions.Effects;

using GameLogic.Registry;

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
