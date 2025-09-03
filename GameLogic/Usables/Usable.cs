namespace GameLogic.Usables;

using GameLogic.Entities;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables.Effects;
using GameLogic.Utils;

public class UsableTemplate : ITemplate
{
    public TemplateId TemplateId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<string> Tags { get; private set; }
    public ITargeter Targeter { get; private set; }
    public List<IEffect> Effects { get; private set; }

    public UsableTemplate(
        TemplateId templateId,
        string name,
        string description,
        List<string> tags,
        ITargeter targeter,
        List<IEffect> effects
    )
    {
        this.TemplateId = templateId;
        this.Name = name;
        this.Description = description;
        this.Tags = [.. tags];
        this.Targeter = targeter.DeepCopy();
        this.Effects = effects.DeepCopyList();
    }

    public Usable CreateUsable()
    {
        return UsableFactory.CreateUsableFromTemplate(this);
    }
}

public class Usable : IUsable, IInstance, IDeepCopyable<Usable>
{
    public InstanceId InstanceId { get; private set; }
    public TemplateId TemplateId { get; private set; }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<string> Tags { get; private set; }

    public ITargeter Targeter { get; private set; }
    public List<IEffect> Effects { get; private set; }

    public Usable(
        InstanceId? instanceId,
        TemplateId templateId,
        string name,
        string description,
        List<string> tags,
        ITargeter targeter,
        List<IEffect> effects
    )
    {
        if (instanceId == null)
        {
            this.InstanceId = Ids.Instance();
        }
        else
        {
            this.InstanceId = instanceId.Value;
        }

        this.TemplateId = templateId;
        this.Name = name;
        this.Description = description;
        this.Tags = [.. tags];
        this.Targeter = targeter.DeepCopy();
        this.Effects = effects.DeepCopyList();
    }

    public Usable (Usable usable)
    {
        this.InstanceId = Ids.Instance();
        this.TemplateId = usable.TemplateId;
        this.Name = usable.Name;
        this.Description = usable.Description;
        this.Tags = [.. usable.Tags];
        this.Targeter = usable.Targeter.DeepCopy();
        this.Effects = usable.Effects.DeepCopyList();
    }

    public Usable DeepCopy()
    {
        return new Usable(this);
    }

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
