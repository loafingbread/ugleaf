namespace GameLogic.Actions.Effects;

public class EffectState
{
    public string Name { get; }
    public string Description { get; }
    public string Type { get; }
    public string Subtype { get; }
    public EffectConfigData Config { get; }
    public IEffectVariant Variant { get; }

    public EffectState(
        string name,
        string description,
        string type,
        string subtype,
        EffectConfigData config,
        IEffectVariant variant
    )
    {
        this.Name = name;
        this.Description = description;
        this.Type = type;
        this.Subtype = subtype;
        this.Config = config;
        this.Variant = variant;
    }
}
