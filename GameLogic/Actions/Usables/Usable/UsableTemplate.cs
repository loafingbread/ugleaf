namespace GameLogic.Actions.Usables.Usable;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Actions.Effects;

public class UsableTemplate
{
    public ReferenceId ReferenceId { get; set; }
    public ReferenceId? DependencyId { get; set; } = null;
    public string DefaultName { get; set; } = "";
    public string DefaultDescription { get; set; } = "";
    public List<string> DefaultTags { get; set; } = new();
    public TargeterData DefaultTargeter { get; set; } = new();
    public List<Effect> DefaultEffects { get; set; } = new();

    public UsableTemplate(
        ReferenceId referenceId,
        ReferenceId? dependencyId,
        string defaultName,
        string defaultDescription,
        List<string> defaultTags,
        TargeterData defaultTargeter,
        List<Effect> defaultEffects
    )
    {
        this.ReferenceId = referenceId;
        this.DependencyId = dependencyId;

        this.DefaultName = defaultName;
        this.DefaultDescription = defaultDescription;
        this.DefaultTags = [.. defaultTags];

        this.DefaultTargeter = defaultTargeter;
        this.DefaultEffects = defaultEffects;
    }

    // Copy constructor
    public UsableTemplate(UsableTemplate template)
    {
        this.ReferenceId = Ids.NewReferenceId();
        this.DefaultName = template.DefaultName;
        this.DefaultDescription = template.DefaultDescription;
        this.DefaultTags = [.. template.DefaultTags];
        this.DefaultTargeter = template.DefaultTargeter.DeepCopy();
        this.DefaultEffects = template
            .DefaultEffects.Select(e => (Effect)e.DeepCopy())
            .ToList();
    }

    public virtual bool IsInstance() => false;

    public Usable Instantiate()
    {
        return new Usable(this, this.State.DeepCopy());
    }

    public UsableTemplate DeepCopy()
    {
        return new UsableTemplate(this);
    }
}
