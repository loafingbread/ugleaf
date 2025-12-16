namespace GameLogic.Usables;

using GameLogic.Entities;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables.Effects;
using GameLogic.Utils;

public class UsableTemplate : IDeepCopyable<UsableTemplate>
{
    public ReferenceId ReferenceId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Tags { get; set; } = new();
    public ITargeter Targeter { get; set; } = new NoTargeter(new Position(0, 0, 0));
    public List<IEffect> Effects { get; set; } = new();

    public UsableTemplate(UsableData data)
    {
        this.ReferenceId = data.ReferenceId;

        this.Name = data.Name;
        this.Description = data.Description;
        this.Tags = [.. data.Tags];

        this.Targeter = TargetingFactory.CreateFromRecord(data.Targeter);
        this.Effects = EffectFactory.CreateEffectsFromData(data.Effects);
    }

    // Copy constructor
    public UsableTemplate(UsableTemplate template)
    {
        this.ReferenceId = Ids.NewReferenceId();
        this.Name = template.Name;
        this.Description = template.Description;
        this.Tags = [.. template.Tags];
        this.Targeter = template.Targeter.DeepCopy();
        this.Effects = template.Effects.DeepCopyList();
    }

    public virtual bool IsInstance() => false;

    public Usable Instantiate()
    {
        return new Usable(this);
    }

    public UsableTemplate DeepCopy()
    {
        return new UsableTemplate(this);
    }
}

public class Usable : UsableTemplate, IUsable, IDeepCopyable<Usable>
{
    public Usable(UsableData data)
        : base(data) { }

    public Usable(UsableTemplate template)
        : base(template) { }

    public Usable(Usable usable)
        : base((usable as UsableTemplate).DeepCopy()) { }

    public new Usable DeepCopy() => new(this);

    public IEnumerable<UsableResult> Use(Entity user, IEnumerable<Entity> targets)
    {
        // TODO: Implement usable result calculation
        List<UsableResult> results = new();
        foreach (Entity target in targets)
        {
            results.Add(new UsableResult(this, user, target));
        }

        return results;
    }
}