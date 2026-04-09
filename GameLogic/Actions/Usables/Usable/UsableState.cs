namespace GameLogic.Actions.Usables.Usable;

using GameLogic.Entities;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Actions.Effects;
using GameLogic.Utils;

public class UsableState
{
    public ReferenceId ReferenceId { get; set; }
    public ReferenceId? DependencyId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Tags { get; set; } = new();
    public ITargeter Targeter { get; set; } = new NoTargeter(new Position(0, 0, 0));
    public List<IEffect> Effects { get; set; } = new();

    public UsableState(
        ReferenceId referenceId,
        ReferenceId? dependencyId,
        string name,
        string description,
        List<string> tags,
        ITargeter targeter,
        List<IEffect> effects
    )
    {
        this.ReferenceId = referenceId;
        this.DependencyId = dependencyId;

        this.Name = name;
        this.Description = description;
        this.Tags = [.. tags];

        this.Targeter = targeter;
        this.Effects = effects;
    }

    public UsableState(UsableTemplate template)
    {
        this.ReferenceId = Ids.NewReferenceId();
        this.DependencyId = template.DependencyId;
        this.Name = template.DefaultName;
        this.Description = template.DefaultDescription;
        this.Tags = [.. template.DefaultTags];
        this.Targeter = new Targeter(template.DefaultTargeter);
        this.Effects = [.. template.DefaultEffects];
    }

    public UsableState DeepCopy()
    {
        return new UsableState(
            Ids.NewReferenceId(),
            this.DependencyId,
            this.Name,
            this.Description,
            [.. this.Tags],
            this.Targeter.DeepCopy(),
            [.. this.Effects.Select(effect => effect.DeepCopy()).ToList()]
        );
    }
}
