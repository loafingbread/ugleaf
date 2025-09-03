namespace GameLogic.Entities.Characters;

using GameLogic.Config;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Registry;

public class CharacterTemplate
    : IConfigurable<CharacterConfigRecord, CharacterMetadataRecord>,
        ITemplate
{
    public TemplateId Id { get; private set; }
    public CharacterMetadataRecord Metadata { get; private set; }
    public CharacterConfigRecord Config { get; private set; }

    public CharacterTemplate(CharacterTemplateRecord record)
    {
        this.Id = new TemplateId(record.Id);
        this.Metadata = record.Metadata;
        this.Config = record.Config;
    }

    public Character CreateCharacter()
    {
        return CharacterFactory.CreateCharacterFromTemplate(this);
    }
}

public class Character
{
    public InstanceId Id { get; private set; }
    public TemplateId TemplateId { get; private set; }
    public string Name { get; private set; } = "";
    public string Description { get; private set; } = "";
    public List<string> Tags { get; private set; } = new();
    public StatBlock Stats { get; private set; }
    public List<Skill> Skills { get; private set; } = new();

    public Character(
        InstanceId id,
        TemplateId templateId,
        string name,
        string description,
        List<string> tags,
        StatBlock stats,
        List<Skill> skills
    )
    {
        this.Id = id;
        this.TemplateId = templateId;
        this.Name = name;
        this.Description = description;
        this.Tags = [.. tags];
        this.Stats = stats;
        this.Skills = skills;
    }
}
