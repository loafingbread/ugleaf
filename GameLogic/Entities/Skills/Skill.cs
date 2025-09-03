namespace GameLogic.Entities.Skills;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;
using GameLogic.Utils;

public class SkillTemplate : ITemplate
{
    public TemplateId TemplateId { get; private set; }

    public string Name { get; private set; } = "";
    public string Description { get; private set; } = "";
    public List<string> Tags { get; private set; } = new();

    public ITargeter Targeter { get; private set; }
    public List<Usable> Usables { get; private set; } = new();

    public SkillTemplate(
        TemplateId templateId,
        string name,
        string description,
        List<string> tags,
        ITargeter targeter,
        List<Usable> usables
    )
    {
        this.TemplateId = templateId;
        this.Name = name;
        this.Description = description;
        this.Tags = [.. tags];
        this.Targeter = targeter.DeepCopy();
        this.Usables = usables.DeepCopyList();
    }

    public Skill Instantiate()
    {
        return SkillFactory.CreateSkillFromTemplate(this);
    }
}

public class Skill : SkillTemplate, IInstance, IDeepCopyable<Skill>
{
    public InstanceId InstanceId { get; private set; }

    public Skill(
        GameLogic.Registry.InstanceId instanceId,
        GameLogic.Registry.TemplateId templateId,
        string name,
        string description,
        List<string> tags,
        ITargeter targeter,
        List<Usable> usables
    )
        : base(templateId, name, description, tags, targeter, usables)
    {
        this.InstanceId = instanceId;
    }

    public Skill(Skill skill)
        : base(
            skill.TemplateId,
            skill.Name,
            skill.Description,
            skill.Tags,
            skill.Targeter,
            skill.Usables
        )
    {
        this.InstanceId = Ids.Instance();
    }

    public Skill DeepCopy()
    {
        return new Skill(this);
    }

    public bool CanTarget() => this.Targeter != null;

    public bool CanUse() => this.Usables.Count > 0;
}
