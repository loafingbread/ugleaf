namespace GameLogic.Entities.Skills;

using GameLogic.Config;
using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables;

public class SkillTemplate : IConfigurable<SkillConfigRecord, SkillMetadataRecord>, ITemplate
{
    public TemplateId Id { get; private set; }
    public SkillMetadataRecord Metadata { get; private set; }
    public SkillConfigRecord Config { get; private set; }

    public SkillTemplate(SkillTemplateRecord record)
    {
        this.Id = new TemplateId(record.Id);
        this.Metadata = record.Metadata;
        this.Config = record.Config;
    }

    public Skill CreateSkill()
    {
        return SkillFactory.CreateSkillFromTemplate(this);
    }
}

public class Skill
{
    public InstanceId Id { get; private set; }
    public TemplateId TemplateId { get; private set; }
    public string Name { get; private set; } = "";
    public string Description { get; private set; } = "";
    public List<string> Tags { get; private set; } = new();

    public ITargeter? Targeter { get; private set; } = null;
    public List<IUsable> Usables { get; private set; } = new();

    public Skill(
        GameLogic.Registry.InstanceId id,
        GameLogic.Registry.TemplateId templateId,
        string name,
        string description,
        List<string> tags,
        TargeterConfig? targeter,
        List<UsableConfig> usables
    )
    {
        this.Id = id;
        this.TemplateId = templateId;

        this.Name = name;
        this.Description = description;
        this.Tags = [.. tags];

        if (targeter is not null)
        {
            this.Targeter = new Targeter(targeter);
        }

        foreach (UsableConfig usableConfig in usables)
        {
            this.Usables.Add(new Usable(usableConfig));
        }
    }

    public bool CanTarget() => this.Targeter != null;

    public bool CanUse() => this.Usables.Count > 0;
}
