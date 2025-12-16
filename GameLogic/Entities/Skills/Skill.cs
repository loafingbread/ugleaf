namespace GameLogic.Entities.Skills;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;
using GameLogic.Utils;

public class SkillTemplate : IDeepCopyable<SkillTemplate>
{
    public ReferenceId ReferenceId { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public List<string> Tags { get; set; } = new();

    public ITargeter Targeter { get; set; } = new NoTargeter(new Position(0, 0, 0));
    public List<Usable> Usables { get; set; } = new();

    public SkillTemplate(SkillData data)
    {
        this.ReferenceId = data.ReferenceId;

        this.Name = data.Name;
        this.Description = data.Description;
        this.Tags = [.. data.Tags];

        this.Targeter = TargetingFactory.CreateFromRecord(data.Targeter);
        this.Usables = UsableFactory.CreateUsablesFromData(data.Usables);
    }

    // Copy constructor
    public SkillTemplate(SkillTemplate template)
    {
        this.ReferenceId = Ids.NewReferenceId();

        this.Name = template.Name;
        this.Description = template.Description;
        this.Tags = [.. template.Tags];

        this.Targeter = template.Targeter.DeepCopy();
        this.Usables = template.Usables.DeepCopyList();
    }

    public virtual bool IsInstance() => false;

    public Skill Instantiate()
    {
        return new Skill(this);
    }

    public SkillTemplate DeepCopy()
    {
        return new SkillTemplate(this);
    }
}

public class Skill : SkillTemplate, IDeepCopyable<Skill>
{
    public Skill(SkillData data)
        : base(data) { }

    public Skill(Skill skill)
        : base((skill as SkillTemplate).DeepCopy()) { }

    // Create a new skill from a template
    public Skill(SkillTemplate template)
        : base(template) { }

    public new Skill DeepCopy() => new(this);

    public bool CanTarget() => this.Targeter != null;

    public bool CanUse() => this.Usables.Count > 0;

    public override bool IsInstance() => true;
}
