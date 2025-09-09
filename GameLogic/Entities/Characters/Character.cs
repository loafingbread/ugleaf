namespace GameLogic.Entities.Characters;

using GameLogic.Config;
using GameLogic.Entities.Skills;
using GameLogic.Entities.Stats;
using GameLogic.Registry;
using GameLogic.Utils;

public class CharacterTemplate : ITemplate, IDeepCopyable<CharacterTemplate>
{
    public TemplateIdentifier TemplateIdentifier { get; set; }
    public string Name { get; private set; } = "";
    public string Description { get; private set; } = "";
    public List<string> Tags { get; private set; } = new();
    public StatBlock Stats { get; private set; }
    public List<Skill> Skills { get; private set; } = new();

    public CharacterTemplate(
        TemplateIdentifier templateIdentifier,
        string name,
        string description,
        List<string> tags,
        StatBlock stats,
        List<Skill> skills
    )
    {
        this.TemplateIdentifier = templateIdentifier;
        this.Name = name;
        this.Description = description;
        this.Tags = [.. tags];
        this.Stats = stats.DeepCopy();
        this.Skills = skills.DeepCopyList();
    }

    public CharacterTemplate(CharacterTemplate template)
    {
        this.TemplateIdentifier = template.TemplateIdentifier;
        this.Name = template.Name;
        this.Description = template.Description;
        this.Tags = [.. template.Tags];
        this.Stats = template.Stats.DeepCopy();
        this.Skills = template.Skills.DeepCopyList();
    }

    public CharacterTemplate DeepCopy()
    {
        return new CharacterTemplate(this);
    }

    public Character Instantiate()
    {
        return CharacterFactory.CreateCharacterFromTemplate(this);
    }
}

public class Character : CharacterTemplate, IInstance, IDeepCopyable<Character>
{
    public InstanceId InstanceId { get; private set; }

    public Character(
        InstanceId id,
        TemplateIdentifier templateIdentifier,
        string name,
        string description,
        List<string> tags,
        StatBlock stats,
        List<Skill> skills
    )
        : base(templateIdentifier, name, description, tags, stats, skills)
    {
        this.InstanceId = id;
    }

    public Character(Character character)
        : base(
            character.TemplateIdentifier,
            character.Name,
            character.Description,
            character.Tags,
            character.Stats,
            character.Skills
        )
    {
        this.InstanceId = Ids.Instance();
    }

    public new Character DeepCopy()
    {
        return new Character(this);
    }
}
