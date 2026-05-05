namespace GameLogic.Actions.Effects;

using GameLogic.Registry;

public class EffectTemplate
{
    public ReferenceId ReferenceId { get; }

    public string Name { get; }
    public string Description { get; }
    public string Type { get; }
    public string Subtype { get; }
    public EffectConfigData Config { get; }

    public EffectTemplate(
        ReferenceId? referenceId,
        string name,
        string description,
        string type,
        string subtype,
        EffectConfigData config
    )
    {
        this.ReferenceId = referenceId ?? Ids.NewReferenceId();
        this.Name = name;
        this.Description = description;
        this.Type = type;
        this.Subtype = subtype;
        this.Config = config;
    }
}
